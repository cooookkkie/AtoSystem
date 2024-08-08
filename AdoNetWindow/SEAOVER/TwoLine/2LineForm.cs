using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using AdoNetWindow.Model;
using Repositories;
using AdoNetWindow.SEAOVER.TwoLine;
using static AdoNetWindow.SEAOVER.TwoLine.ProductUnit;
using Libs.Tools;
using System.Drawing.Printing;
using System.Threading;
using System.Text.RegularExpressions;
using AdoNetWindow.SEAOVER.OneLine;
using Repositories.SEAOVER;
using Repositories.Config;

namespace AdoNetWindow.SEAOVER._2Line
{
    public partial class _2LineForm : Form
    {
        // Thread Process
        BackgroundWorker thdProcess = null;
        ICommonRepository commonRepository = new CommonRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IFormChangedDataRepository formChangedDataRepository = new FormChangedDataRepository();
        IUsersRepository usersRepository = new UsersRepository();
        IBookmarkRepository bookmarkRepository = new BookmarkRepository();
        IStockRepository stockRepository = new StockRepository();

        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        private PrintDocument printDoc = new PrintDocument();
        private PageSettings pgSettings = new PageSettings();
        private PrinterSettings prtSettings = new PrinterSettings();

        Libs.Tools.Common common = new Libs.Tools.Common();
        List<Dictionary<string, List<SeaoverPriceModel>>> list;
        List<SeaoverCopyModel> clipboardModel = new List<SeaoverCopyModel>();
        double totalHeight = 840;
        int unitHeight;
        UsersModel um;
        int TotalPage;
        bool saveFlag;
        bool processingFlag;
        string savePath = "";

        private System.Windows.Threading.DispatcherTimer timer;
        private int loadingCnt = 0;


        Dictionary<int, Dictionary<int, string>> copyDic;
        Libs.MessageBox messageBox = new Libs.MessageBox();

        public _2LineForm(int height, UsersModel userModel)
        {
            InitializeComponent();
            unitHeight = height;
            um = userModel;
        }
        public _2LineForm(int height, UsersModel userModel, DataTable dt)
        {
            InitializeComponent();
            unitHeight = height;
            um = userModel;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["category"].Value = dt.Rows[i]["category"].ToString();
                    dgvProduct.Rows[n].Cells["category_code"].Value = dt.Rows[i]["category_code"].ToString();
                    dgvProduct.Rows[n].Cells["category1"].Value = dt.Rows[i]["category1"].ToString();
                    dgvProduct.Rows[n].Cells["category2"].Value = dt.Rows[i]["category2"].ToString();
                    dgvProduct.Rows[n].Cells["category3"].Value = dt.Rows[i]["category3"].ToString();
                    dgvProduct.Rows[n].Cells["product"].Value = dt.Rows[i]["product"].ToString();
                    dgvProduct.Rows[n].Cells["product_code"].Value = dt.Rows[i]["product_code"];
                    dgvProduct.Rows[n].Cells["origin"].Value = dt.Rows[i]["origin"];
                    dgvProduct.Rows[n].Cells["origin_code"].Value = dt.Rows[i]["origin_code"];
                    dgvProduct.Rows[n].Cells["sizes"].Value = dt.Rows[i]["sizes"];
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = dt.Rows[i]["sizes_code"];
                    dgvProduct.Rows[n].Cells["price_unit"].Value = dt.Rows[i]["price_unit"];
                    dgvProduct.Rows[n].Cells["unit"].Value = dt.Rows[i]["unit"];
                    dgvProduct.Rows[n].Cells["unit_count"].Value = dt.Rows[i]["unit_count"];
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = dt.Rows[i]["seaover_unit"];
                    dgvProduct.Rows[n].Cells["sales_price"].Value = dt.Rows[i]["sales_price"];
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = dt.Rows[i]["purchase_price"];
                    dgvProduct.Rows[n].Cells["division"].Value = dt.Rows[i]["division"];
                    dgvProduct.Rows[n].Cells["manager1"].Value = dt.Rows[i]["manager1"];
                    dgvProduct.Rows[n].Cells["weight"].Value = dt.Rows[i]["weight"];
                    dgvProduct.Rows[n].Cells["rows"].Value = 1;
                }
            }
        }

        private void _2LineForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            //Double Buffer
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            Init_DataGridView();
            //Header Setting
            //this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.Columns["category_code"].Visible = false;
            this.dgvProduct.Columns["category1"].Visible = false;
            this.dgvProduct.Columns["category2"].Visible = false;
            this.dgvProduct.Columns["category3"].Visible = false;

            this.dgvProduct.Columns["product_code"].Visible = false;
            this.dgvProduct.Columns["origin_code"].Visible = false;
            this.dgvProduct.Columns["sizes_code"].Visible = false;
            this.dgvProduct.Columns["purchase_price"].Visible = false;
            this.dgvProduct.Columns["unit"].Visible = false;
            this.dgvProduct.Columns["price_unit"].Visible = false;
            this.dgvProduct.Columns["unit_count"].Visible = false;
            this.dgvProduct.Columns["seaover_unit"].Visible = false;
            this.dgvProduct.Columns["cnt"].Visible = false;
            this.dgvProduct.Columns["edit_date"].Visible = false;
            this.dgvProduct.Columns["manager1"].Visible = false;
            this.dgvProduct.Columns["insert"].Visible = false;
            this.dgvProduct.Columns["delete2"].Visible = false;
            this.dgvProduct.Columns["accent"].Visible = false;
            //헤더 디자인
            this.dgvProduct.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            /*this.dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.RosyBrown;*/
            this.dgvProduct.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            SetHeaderStyle();

            //품목서 Type
            rbTwoline.Checked = true;
            this.rbOneline.CheckedChanged += new System.EventHandler(this.rbTwoline_CheckedChanged);
            this.rbTwoline.CheckedChanged += new System.EventHandler(this.rbOneline_CheckedChanged);

            //업체별시세현황 스토어프로시져 호출
            retry:
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            try
            {
                if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
                {
                    MessageBox.Show(this, "호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch 
            {
                goto retry;
            }

            // 전체 컬럼의 Sorting 기능 차단
            foreach (DataGridViewColumn item in dgvProduct.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            string managerTxt;
            if (um.grade == null)
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }
            else if (string.IsNullOrEmpty(um.grade))
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }
            else
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " " + um.grade.ToString() + " (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }

            lbRemark.Text = managerTxt + " / " + um.form_remark;

            //품목서 리스트 
            GetTitleList();
            //즐겨찾기 불러오기
            GetBookmark();
            //데이터 불러오기
            //GetProductData("0");
            timer_start();
        }

        private void Init_DataGridView()
        {
            dgvProduct.DoubleBuffered(true);
        }

        //Datagridview Header style 
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvProduct;
            dgv.Columns["category"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["category"].HeaderCell.Style.ForeColor = Color.Yellow;
            //dgv.Columns["category"].DefaultCellStyle.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            dgv.Columns["category"].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 153);

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["weight"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["weight"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["price_unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["price_unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["unit_count"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["unit_count"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["seaover_unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["seaover_unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sales_price"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["sales_price"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["sales_price"].DefaultCellStyle.Font = new Font("맑은 고딕", 10, FontStyle.Bold);
            dgv.Columns["sales_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);

            dgv.Columns["division"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["division"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["page"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204); ;

            dgv.Columns["rows"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204); ;

            dgv.Columns["area"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204); ;

            //dgv.Columns["insert"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204); ;

            //dgv.Columns["delete"].HeaderCell.Style.BackColor = Color.FromArgb(204, 255, 204); ;
        }

        #region 데이터 불러오기 

        private void GetTitleList()
        {
            /*품목서리스트ToolStripMenuItem.DropDownItems.Clear();*/
            lvFormList.Items.Clear();

            List<FormDataTitle> model = seaoverRepository.GetFormDataTitle(2, txtId.Text, txtFormname2.Text, txtManager2.Text);
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    /*ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Name = model[i].id.ToString();
                    item.Text = model[i].form_name;
                    item.Click += new System.EventHandler(GetData);

                    품목서리스트ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {item});*/

                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = model[i].id.ToString();
                    lvi.SubItems.Add(model[i].form_name.ToString());
                    lvi.SubItems.Add(model[i].edit_user.ToString());

                    lvFormList.Items.Add(lvi);
                }
            }
        }
        private void GetProductData(string id)
        {
            //저장값 불러오기
            List<FormDataModel> model = seaoverRepository.GetFormData("2", id);
            if (model.Count > 0)
            {
                lbId.Text = model[0].id.ToString();
                lbUpdatetime.Text = Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd hh:mm:ss");
                lbEditUser.Text = model[0].edit_user.ToString();
                txtFormName.Text = model[0].form_name.ToString();

                for (int i = 0; i < model.Count; i++)
                {
                    FormDataModel m = model[i];
                    //Row 추가
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["category_code"].Value = m.category_code;
                    dgvProduct.Rows[n].Cells["category"].Value = m.category;
                    dgvProduct.Rows[n].Cells["product_code"].Value = m.product_code;
                    dgvProduct.Rows[n].Cells["product"].Value = m.product;
                    dgvProduct.Rows[n].Cells["origin_code"].Value = m.origin_code;
                    dgvProduct.Rows[n].Cells["origin"].Value = m.origin;
                    dgvProduct.Rows[n].Cells["weight"].Value = m.weight;
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = m.sizes_code;
                    dgvProduct.Rows[n].Cells["sizes"].Value = m.sizes;
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = m.purchase_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["sales_price"].Value = m.sales_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["unit"].Value = m.unit;
                    dgvProduct.Rows[n].Cells["price_unit"].Value = m.price_unit;
                    dgvProduct.Rows[n].Cells["unit_count"].Value = m.unit_count;
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = m.seaover_unit;
                    dgvProduct.Rows[n].Cells["division"].Value = m.division;
                    dgvProduct.Rows[n].Cells["page"].Value = m.page;
                    dgvProduct.Rows[n].Cells["cnt"].Value = m.cnt;
                    dgvProduct.Rows[n].Cells["rows"].Value = m.row_index;
                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[n].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");
                    dgvProduct.Rows[n].Cells["area"].Value = m.area;
                }
                //SetTitleForm();
                SettingForm();
            }
        }


        #endregion

        #region 양식만들기
        public void SettingForm()
        {
            lCategory.Controls.Clear();
            rCategory.Controls.Clear();
            lProduct.Controls.Clear();
            rProduct.Controls.Clear();
            lOrigin.Controls.Clear();
            rOrigin.Controls.Clear();
            lWeight.Controls.Clear();
            rWeight.Controls.Clear();
            lSize.Controls.Clear();
            rSize.Controls.Clear();
            lPrice.Controls.Clear();
            rPrice.Controls.Clear();

            DataGridView dgv = dgvProduct;
            int errCnt = 0;
            if (dgv.Rows.Count > 1)
            {
                foreach (DataGridViewRow r in dgvProduct.Rows)
                {
                    foreach (DataGridViewCell c in r.Cells)
                    {
                        if (c.Value != null)
                            c.Value = c.Value.ToString().Replace(@"\n", "\n");
                    }
                }

                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    dgv.Rows[i].Cells["cnt"].Value = "";
                    dgv.Rows[i].Cells["area"].Value = "";
                    if (string.IsNullOrEmpty(dgv.Rows[i].Cells["rows"].Value.ToString()))
                        dgv.Rows[i].Cells["rows"].Value = 1;
                }
            }

            if (rbOneline.Checked)
            {
                for (int i = 1; i < 60; i++)
                    SetPagecnt(i, "C");
            }
            else
            {
                for (int i = 1; i < 30; i++)
                {
                    SetPagecnt(i, "L");
                    SetPagecnt(i, "R");
                }
            }

            lbTotal.Text = "/ " + TotalPage.ToString();
            txtTotalPage.Text = TotalPage.ToString();

            PageCountSetting();
            if (string.IsNullOrEmpty(txtCurPage.Text))
                txtCurPage.Text = "1";
            //한출품목서
            if (rbOneline.Checked)
                SetOneFormProduct((Convert.ToInt16(txtCurPage.Text)));
            else
                SetFormProduct((Convert.ToInt16(txtCurPage.Text)));
        }

        private void SetFormProduct(int page)
        {
            //기본설정
            rightTitlePanel.Visible = true;
            RightProductPanel.Visible = true;

            leftTitlePanel.Width = 348;
            LeftProductPanel.Width = 348;


            pnCategory.Width = 33;
            pnCategory.Location = new Point(0, 0);
            lbCategory.Location = new Point(0, 0);
            lbCategory.Width = pnCategory.Width;
            lbCategory.Height = pnCategory.Height;

            pnProduct.Width = 82;
            pnProduct.Location = new Point(32, 0);
            lbProduct.Location = new Point(0, 0);
            lbProduct.Width = pnProduct.Width;
            lbProduct.Height = pnProduct.Height;

            pnOrigin.Width = 35;
            pnOrigin.Location = new Point(113, 0);
            lbOrigin.Location = new Point(0, 0);
            lbOrigin.Width = pnOrigin.Width;
            lbOrigin.Height = pnOrigin.Height;

            pnWeight.Width = 71;
            pnWeight.Location = new Point(148, 0);
            lbWeight.Location = new Point(0, 0);
            lbWeight.Width = pnWeight.Width;
            lbWeight.Height = pnWeight.Height;

            pnSizes.Width = 64;
            pnSizes.Location = new Point(218, 0);
            lbSizes.Location = new Point(0, 0);
            lbSizes.Width = pnSizes.Width;
            lbSizes.Height = pnSizes.Height;

            pnPrice.Width = 69;
            pnPrice.Location = new Point(281, 0);
            lbPrice.Location = new Point(0, 0);
            lbPrice.Width = pnPrice.Width;
            lbPrice.Height = pnPrice.Height;

            lCategory.Width = 30;
            lProduct.Width = 81;
            lOrigin.Width = 36;
            lWeight.Width = 69;
            lSize.Width = 63;
            lPrice.Width = 67;

            DataGridView dgv = dgvProduct;
            ProductUnit ui;
            SeaoverPriceModel model;
            int cHeight;
            int pHeight;
            int oHeight;
            int wHeight;
            int zHeight;
            int rHeight;
            int accent2;

            int row = 60;
            double s;
            if (dgv.Rows.Count > 1)
            {
                //Page Left Product Unit Setting==============================================================================
                //Page Product Unit Height
                cHeight = unitHeight;
                pHeight = unitHeight;
                oHeight = unitHeight;
                wHeight = unitHeight;
                bool accent;
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    for (int j = 0; j < dgv.ColumnCount; j++)
                    {
                        if (dgv.Rows[i].Cells[j].Value == null)
                            dgv.Rows[i].Cells[j].Value = string.Empty;
                    }

                    if (dgv.Rows[i].Cells["accent2"].Value == null || !int.TryParse(dgv.Rows[i].Cells["accent2"].Value.ToString(), out accent2))
                        accent2 = 0;

                    if (!string.IsNullOrEmpty(dgv.Rows[i].Cells["product"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(dgv.Rows[i].Cells["cnt"].Value.ToString()))
                        {
                            break;
                        }
                        if ((int)dgv.Rows[i].Cells["cnt"].Value == 1)
                        {
                            //row = GetPageCount(page, dgv.Rows[i].Cells["area"].Value.ToString());
                            //unitHeight = (int)((double)843 / (double)row);
                            unitHeight = 14 * Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                            cHeight = unitHeight;
                            pHeight = unitHeight;
                            oHeight = unitHeight;
                            wHeight = unitHeight;
                            zHeight = unitHeight;
                            rHeight = unitHeight;
                        }


                        zHeight = unitHeight;
                        rHeight = unitHeight;
                        if (!string.IsNullOrEmpty(dgv.Rows[i].Cells["page"].Value.ToString()))
                        {
                            if (Convert.ToInt32(dgv.Rows[i].Cells["page"].Value) == page && dgv.Rows[i].Cells["area"].Value.ToString() == "L")
                            {
                                model = new SeaoverPriceModel();
                                if (dgv.Rows[i].Cells["accent"].Value == null || !bool.TryParse(dgv.Rows[i].Cells["accent"].Value.ToString(), out accent))
                                    accent = false;
                                model.accent = accent;

                                

                                model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                if (dgv.Rows[i].Cells["edit_date"].Value == null)
                                { dgv.Rows[i].Cells["edit_date"].Value = ""; }
                                model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                if (dgv.Rows[i].Cells["manager1"].Value == null)
                                { dgv.Rows[i].Cells["manager1"].Value = ""; }
                                model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();
                                //model.sales_price = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString());
                                if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                    model.sales_price = s;
                                else
                                {
                                    if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                        model.sales_price = -1;
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                        model.sales_price = -2;
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                        model.sales_price = -3;
                                }
                                model.note = dgv.Rows[i].Cells["weight"].Value.ToString();

                                //행수가 2 이상일 경우
                                if (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) > 1)
                                {
                                    cHeight = cHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    pHeight = pHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    oHeight = oHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    wHeight = wHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    zHeight = zHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    rHeight = rHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                }

                                /*cHeight = cHeight;
                                pHeight = pHeight;
                                oHeight = oHeight;
                                wHeight = wHeight;
                                zHeight = zHeight;
                                rHeight = rHeight;*/


                                //사이즈
                                ui = new ProductUnit(this, UnitType.sizes, model, zHeight, model.accent, accent2);
                                lSize.Controls.Add(ui);
                                //단가
                                ui = new ProductUnit(this, UnitType.price, model, rHeight, model.accent, accent2);
                                lPrice.Controls.Add(ui);

                                //머지항목
                                if ((dgv.Rows.Count - 2) > i)
                                {
                                    if (dgv.Rows[i].Cells["page"].Value.ToString() == dgv.Rows[i + 1].Cells["page"].Value.ToString()
                                        && dgv.Rows[i].Cells["area"].Value.ToString() == dgv.Rows[i + 1].Cells["area"].Value.ToString())
                                    {
                                        //대분류
                                        if (dgv.Rows[i].Cells["category"].Value.ToString() != dgv.Rows[i + 1].Cells["category"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Category, model, cHeight, model.accent, accent2);
                                            lCategory.Controls.Add(ui);
                                            cHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Product, model, pHeight, model.accent, accent2);
                                            lProduct.Controls.Add(ui);
                                            pHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                            lOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                            lWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;

                                            goto nextIndex1;
                                        }
                                        else
                                        {
                                            cHeight += (int)unitHeight;
                                        }

                                        //품목
                                        if (dgv.Rows[i].Cells["product"].Value.ToString() != dgv.Rows[i + 1].Cells["product"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Product, model, pHeight, model.accent, accent2);
                                            lProduct.Controls.Add(ui);
                                            pHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                            lOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                            lWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;

                                            goto nextIndex1;
                                        }
                                        else
                                        {
                                            pHeight += (int)unitHeight;
                                        }

                                        //원산지
                                        if (dgv.Rows[i].Cells["origin"].Value.ToString() != dgv.Rows[i + 1].Cells["origin"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                            lOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;
                                        }
                                        else
                                        {
                                            oHeight += (int)unitHeight;
                                        }

                                        //중량
                                        if (dgv.Rows[i].Cells["weight"].Value.ToString() != dgv.Rows[i + 1].Cells["weight"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                            lWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;
                                        }
                                        else
                                        {
                                            wHeight += (int)unitHeight;
                                        }

                                    nextIndex1:        //①
                                        Console.WriteLine(page + " / " + area);
                                    }
                                    else
                                    {
                                        model = new SeaoverPriceModel();
                                        if (dgv.Rows[i].Cells["accent"].Value == null || !bool.TryParse(dgv.Rows[i].Cells["accent"].Value.ToString(), out accent))
                                            accent = false;
                                        else
                                            accent = Convert.ToBoolean(dgv.Rows[i].Cells["accent"].Value);

                                        model.accent = accent;
                                        model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                        model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                        model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                        model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                        model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                        model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                        model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                        model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                        model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                        model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                        model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                        model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                        model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();

                                        if (dgv.Rows[i].Cells["edit_date"].Value == null)
                                        { dgv.Rows[i].Cells["edit_date"].Value = ""; }
                                        model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                        if (dgv.Rows[i].Cells["manager1"].Value == null)
                                        { dgv.Rows[i].Cells["manager1"].Value = ""; }
                                        model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();

                                        /*model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                        model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();*/
                                        //model.sales_price = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString());

                                        if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                        {
                                            model.sales_price = s;
                                        }
                                        else
                                        {
                                            if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                                model.sales_price = -1;
                                            else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                                model.sales_price = -2;
                                            else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                                model.sales_price = -3;
                                        }
                                        model.note = dgv.Rows[i].Cells["weight"].Value.ToString();
                                        //대분류
                                        ui = new ProductUnit(this, UnitType.Category, model, cHeight, model.accent, accent2);
                                        lCategory.Controls.Add(ui);
                                        cHeight = (int)unitHeight;
                                        //폼목
                                        ui = new ProductUnit(this, UnitType.Product, model, pHeight, model.accent, accent2);
                                        lProduct.Controls.Add(ui);
                                        pHeight = (int)unitHeight;
                                        //원산지
                                        ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                        lOrigin.Controls.Add(ui);
                                        oHeight = (int)unitHeight;
                                        //중량6
                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                        lWeight.Controls.Add(ui);
                                        wHeight = (int)unitHeight;
                                    }
                                }
                                else
                                {
                                    model = new SeaoverPriceModel();
                                    if (dgv.Rows[i].Cells["accent"].Value == null || !bool.TryParse(dgv.Rows[i].Cells["accent"].Value.ToString(), out accent))
                                        accent = false;
                                    else
                                        accent = Convert.ToBoolean(dgv.Rows[i].Cells["accent"].Value);

                                    model.accent = accent;
                                    model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                    model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                    model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                    model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                    model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                    model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                    model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                    model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                    model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                    model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                    model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                    model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                                    if (dgv.Rows[i].Cells["edit_date"].Value == null)
                                    { dgv.Rows[i].Cells["edit_date"].Value = ""; }
                                    model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                    if (dgv.Rows[i].Cells["manager1"].Value == null)
                                    { dgv.Rows[i].Cells["manager1"].Value = ""; }
                                    model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();
                                    /*model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                    model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();*/
                                    //model.sales_price = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString());
                                    if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                    {
                                        model.sales_price = s;
                                    }
                                    else
                                    {
                                        if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                            model.sales_price = -1;
                                        else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                            model.sales_price = -2;
                                        else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                            model.sales_price = -3;
                                    }
                                    model.note = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    //대분류
                                    ui = new ProductUnit(this, UnitType.Category, model, cHeight, model.accent, accent2);
                                    lCategory.Controls.Add(ui);
                                    cHeight = (int)unitHeight;
                                    //폼목
                                    ui = new ProductUnit(this, UnitType.Product, model, pHeight, model.accent, accent2);
                                    lProduct.Controls.Add(ui);
                                    pHeight = (int)unitHeight;
                                    //원산지
                                    ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                    lOrigin.Controls.Add(ui);
                                    oHeight = (int)unitHeight;
                                    //중량6
                                    ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                    lWeight.Controls.Add(ui);
                                    wHeight = (int)unitHeight;

                                }
                            }
                        }
                    }
                }

                //Page Right Product Unit Setting==============================================================================
                //Page Product Unit Height
                cHeight = unitHeight;
                pHeight = unitHeight;
                oHeight = unitHeight;
                wHeight = unitHeight;

                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(dgv.Rows[i].Cells["product"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(dgv.Rows[i].Cells["cnt"].Value.ToString()))
                        {
                            break;
                        }

                        if (dgv.Rows[i].Cells["accent2"].Value == null || !int.TryParse(dgv.Rows[i].Cells["accent2"].Value.ToString(), out accent2))
                            accent2 = 0;

                        if ((int)dgv.Rows[i].Cells["cnt"].Value == 1)
                        {
                            //row = GetPageCount(page, dgv.Rows[i].Cells["area"].Value.ToString());
                            //unitHeight = (int)((double)843 / (double)row);
                            unitHeight = 14 * Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                            cHeight = unitHeight;
                            pHeight = unitHeight;
                            oHeight = unitHeight;
                            wHeight = unitHeight;
                            zHeight = unitHeight;
                            rHeight = unitHeight;
                        }

                        zHeight = unitHeight;
                        rHeight = unitHeight;
                        if (!string.IsNullOrEmpty(dgv.Rows[i].Cells["page"].Value.ToString()))
                        {
                            if ((int)dgv.Rows[i].Cells["page"].Value == page && dgv.Rows[i].Cells["area"].Value.ToString() == "R")
                            {
                                model = new SeaoverPriceModel();
                                if (dgv.Rows[i].Cells["accent"].Value == null || !bool.TryParse(dgv.Rows[i].Cells["accent"].Value.ToString(), out accent))
                                    accent = false;
                                else
                                    accent = Convert.ToBoolean(dgv.Rows[i].Cells["accent"].Value);

                                model.accent = accent;
                                model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                                if (dgv.Rows[i].Cells["edit_date"].Value == null)
                                { dgv.Rows[i].Cells["edit_date"].Value = ""; }
                                model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                if (dgv.Rows[i].Cells["manager1"].Value == null)
                                { dgv.Rows[i].Cells["manager1"].Value = ""; }
                                model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();
                                if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                {
                                    model.sales_price = s;
                                }
                                else
                                {
                                    if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                        model.sales_price = -1;
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                        model.sales_price = -2;
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                        model.sales_price = -3;
                                }

                                model.note = dgv.Rows[i].Cells["weight"].Value.ToString();

                                //행수가 2 이상일 경우
                                if (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) > 1)
                                {
                                    cHeight = cHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    pHeight = pHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    oHeight = oHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    wHeight = wHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    zHeight = zHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    rHeight = rHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                }
                                /*cHeight = cHeight;
                                pHeight = pHeight;
                                oHeight = oHeight;
                                wHeight = wHeight;
                                zHeight = zHeight;
                                rHeight = rHeight;*/


                                //사이즈
                                ui = new ProductUnit(this, UnitType.sizes, model, zHeight, model.accent, accent2);
                                rSize.Controls.Add(ui);
                                //단가
                                ui = new ProductUnit(this, UnitType.price, model, rHeight, model.accent, accent2);
                                rPrice.Controls.Add(ui);

                                //머지항목
                                if ((dgv.Rows.Count - 2) > i)
                                {
                                    if (dgv.Rows[i].Cells["page"].Value.ToString() == dgv.Rows[i + 1].Cells["page"].Value.ToString()
                                        && dgv.Rows[i].Cells["area"].Value.ToString() == dgv.Rows[i + 1].Cells["area"].Value.ToString())
                                    {
                                        //대분류
                                        if (dgv.Rows[i].Cells["category"].Value.ToString() != dgv.Rows[i + 1].Cells["category"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Category, model, cHeight, model.accent, accent2);
                                            rCategory.Controls.Add(ui);
                                            cHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Product, model, pHeight, model.accent, accent2);
                                            rProduct.Controls.Add(ui);
                                            pHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                            rOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                            rWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;

                                            goto nextIndex2;
                                        }
                                        else
                                        {
                                            cHeight += (int)unitHeight;
                                        }

                                        //품목
                                        if (dgv.Rows[i].Cells["product"].Value.ToString() != dgv.Rows[i + 1].Cells["product"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Product, model, pHeight, model.accent, accent2);
                                            rProduct.Controls.Add(ui);
                                            pHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                            rOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                            rWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;

                                            goto nextIndex2;
                                        }
                                        else
                                        {
                                            pHeight += (int)unitHeight;
                                        }

                                        //원산지
                                        if (dgv.Rows[i].Cells["origin"].Value.ToString() != dgv.Rows[i + 1].Cells["origin"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                            rOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;
                                        }
                                        else
                                        {
                                            oHeight += (int)unitHeight;
                                        }

                                        //중량
                                        if (dgv.Rows[i].Cells["weight"].Value.ToString() != dgv.Rows[i + 1].Cells["weight"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                            rWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;
                                        }
                                        else
                                        {
                                            wHeight += (int)unitHeight;
                                        }

                                    nextIndex2:        //①
                                        Console.WriteLine(page + " / " + area);
                                    }
                                    else
                                    {
                                        model = new SeaoverPriceModel();
                                        if (dgv.Rows[i].Cells["accent"].Value == null || !bool.TryParse(dgv.Rows[i].Cells["accent"].Value.ToString(), out accent))
                                            accent = false;
                                        else
                                            accent = Convert.ToBoolean(dgv.Rows[i].Cells["accent"].Value);

                                        model.accent = accent;
                                        model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                        model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                        model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                        model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                        model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                        model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                        model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                        model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                        model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                        model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                        model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                        model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                        model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                                        if (dgv.Rows[i].Cells["edit_date"].Value == null)
                                        { dgv.Rows[i].Cells["edit_date"].Value = ""; }
                                        model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                        if (dgv.Rows[i].Cells["manager1"].Value == null)
                                        { dgv.Rows[i].Cells["manager1"].Value = ""; }
                                        model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();
                                        /*model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                        model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();*/
                                        if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                        {
                                            model.sales_price = s;
                                        }
                                        else
                                        {
                                            if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                                model.sales_price = -1;
                                            else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                                model.sales_price = -2;
                                            else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                                model.sales_price = -3;
                                        }

                                        model.note = dgv.Rows[i].Cells["weight"].Value.ToString();
                                        //대분류
                                        ui = new ProductUnit(this, UnitType.Category, model, cHeight, model.accent, accent2);
                                        rCategory.Controls.Add(ui);
                                        cHeight = (int)unitHeight;
                                        //폼목
                                        ui = new ProductUnit(this, UnitType.Product, model, pHeight, model.accent, accent2);
                                        rProduct.Controls.Add(ui);
                                        pHeight = (int)unitHeight;
                                        //원산지
                                        ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                        rOrigin.Controls.Add(ui);
                                        oHeight = (int)unitHeight;
                                        //중량6
                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                        rWeight.Controls.Add(ui);
                                        wHeight = (int)unitHeight;
                                    }
                                }
                                else
                                {
                                    model = new SeaoverPriceModel();
                                    if (dgv.Rows[i].Cells["accent"].Value == null || !bool.TryParse(dgv.Rows[i].Cells["accent"].Value.ToString(), out accent))
                                        accent = false;
                                    else
                                        accent = Convert.ToBoolean(dgv.Rows[i].Cells["accent"].Value);

                                    model.accent = accent;
                                    model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                    model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                    model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                    model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                    model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                    model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                    model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                    model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                    model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                    model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                    model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                    model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                                    if (dgv.Rows[i].Cells["edit_date"].Value == null)
                                    { dgv.Rows[i].Cells["edit_date"].Value = ""; }
                                    model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                    if (dgv.Rows[i].Cells["manager1"].Value == null)
                                    { dgv.Rows[i].Cells["manager1"].Value = ""; }
                                    model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();
                                    /*model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                    model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();*/
                                    //model.sales_price = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString());
                                    if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                        model.sales_price = s;
                                    else
                                    {
                                        if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                            model.sales_price = -1;
                                        else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                            model.sales_price = -2;
                                        else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                            model.sales_price = -3;
                                    }
                                    model.note = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    //대분류
                                    ui = new ProductUnit(this, UnitType.Category, model, cHeight, model.accent, accent2);
                                    rCategory.Controls.Add(ui);
                                    cHeight = (int)unitHeight;
                                    //폼목
                                    ui = new ProductUnit(this, UnitType.Product, model, pHeight, model.accent, accent2);
                                    rProduct.Controls.Add(ui);
                                    pHeight = (int)unitHeight;
                                    //원산지
                                    ui = new ProductUnit(this, UnitType.Origin, model, oHeight, model.accent, accent2);
                                    rOrigin.Controls.Add(ui);
                                    oHeight = (int)unitHeight;
                                    //중량6
                                    ui = new ProductUnit(this, UnitType.Weight, model, wHeight, model.accent, accent2);
                                    rWeight.Controls.Add(ui);
                                    wHeight = (int)unitHeight;

                                }
                            }
                        }
                    }
                }
            }
        }

        //1줄 품목서
        private void SetOneFormProduct(int page)
        {
            //기본설정
            rightTitlePanel.Visible = false;
            RightProductPanel.Visible = false;

            leftTitlePanel.Width = 698;
            LeftProductPanel.Width = 698;


            pnCategory.Width = 63;
            pnCategory.Location = new Point(0, 0);
            lbCategory.Location = new Point(0, 0);
            lbCategory.Width = pnCategory.Width;
            lbCategory.Height = pnCategory.Height;

            pnProduct.Width = 163;
            pnProduct.Location = new Point(62, 0);
            lbProduct.Location = new Point(0, 0);
            lbProduct.Width = pnProduct.Width;
            lbProduct.Height = pnProduct.Height;

            pnOrigin.Width = 70;
            pnOrigin.Location = new Point(224, 0);
            lbOrigin.Location = new Point(0, 0);
            lbOrigin.Width = pnOrigin.Width;
            lbOrigin.Height = pnOrigin.Height;

            pnWeight.Width = 139;
            pnWeight.Location = new Point(294, 0);
            lbWeight.Location = new Point(0, 0);
            lbWeight.Width = pnWeight.Width;
            lbWeight.Height = pnWeight.Height;

            pnSizes.Width = 130;
            pnSizes.Location = new Point(432, 0);
            lbSizes.Location = new Point(0, 0);
            lbSizes.Width = pnSizes.Width;
            lbSizes.Height = pnSizes.Height;

            pnPrice.Width = 140;
            pnPrice.Location = new Point(558, 0);
            lbPrice.Location = new Point(0, 0);
            lbPrice.Width = pnPrice.Width;
            lbPrice.Height = pnPrice.Height;

            lCategory.Width = 60;
            lProduct.Width = 162;
            lOrigin.Width = 70;
            lWeight.Width = 138;
            lSize.Width = 126;
            lPrice.Width = 140;

            DataGridView dgv = dgvProduct;
            ProductUnit ui;
            SeaoverPriceModel model;
            int cHeight;
            int pHeight;
            int oHeight;
            int wHeight;
            int zHeight;
            int rHeight;


            int row = 60;
            double s;
            if (dgv.Rows.Count > 1)
            {
                //Page Left Product Unit Setting==============================================================================
                //Page Product Unit Height
                cHeight = unitHeight;
                pHeight = unitHeight;
                oHeight = unitHeight;
                wHeight = unitHeight;

                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    if (!string.IsNullOrEmpty(dgv.Rows[i].Cells["product"].Value.ToString()))
                    {
                        if (string.IsNullOrEmpty(dgv.Rows[i].Cells["cnt"].Value.ToString()))
                        {
                            break;
                        }
                        if ((int)dgv.Rows[i].Cells["cnt"].Value == 1)
                        {
                            //row = GetPageCount(page, dgv.Rows[i].Cells["area"].Value.ToString());
                            //unitHeight = (int)((double)843 / (double)row);
                            unitHeight = 14 * Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                            cHeight = unitHeight;
                            pHeight = unitHeight;
                            oHeight = unitHeight;
                            wHeight = unitHeight;
                            zHeight = unitHeight;
                            rHeight = unitHeight;
                        }


                        zHeight = unitHeight;
                        rHeight = unitHeight;


                        if (!string.IsNullOrEmpty(dgv.Rows[i].Cells["page"].Value.ToString()))
                        {
                            if (Convert.ToInt32(dgv.Rows[i].Cells["page"].Value) == page)
                            {
                                model = new SeaoverPriceModel();
                                model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                                model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();
                                model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                //model.sales_price = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString());
                                if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                {
                                    model.sales_price = s;
                                }
                                else
                                {
                                    if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                        model.sales_price = -1;
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                        model.sales_price = -2;
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                        model.sales_price = -3;
                                }
                                model.note = dgv.Rows[i].Cells["weight"].Value.ToString();

                                //행수가 2 이상일 경우
                                if (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) > 1)
                                {
                                    cHeight = cHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    pHeight = pHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    oHeight = oHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    wHeight = wHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    zHeight = zHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                    rHeight = rHeight + unitHeight * (Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value) - 1);
                                }

                                /*cHeight = cHeight;
                                pHeight = pHeight;
                                oHeight = oHeight;
                                wHeight = wHeight;
                                zHeight = zHeight;
                                rHeight = rHeight;*/


                                //사이즈
                                ui = new ProductUnit(this, UnitType.sizes, model, zHeight, 1);
                                lSize.Controls.Add(ui);
                                //단가
                                ui = new ProductUnit(this, UnitType.price, model, rHeight, 1);
                                lPrice.Controls.Add(ui);

                                //머지항목
                                if ((dgv.Rows.Count - 2) > i)
                                {
                                    if (dgv.Rows[i].Cells["page"].Value.ToString() == dgv.Rows[i + 1].Cells["page"].Value.ToString()
                                        && dgv.Rows[i].Cells["area"].Value.ToString() == dgv.Rows[i + 1].Cells["area"].Value.ToString())
                                    {
                                        //대분류
                                        if (dgv.Rows[i].Cells["category"].Value.ToString() != dgv.Rows[i + 1].Cells["category"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Category, model, cHeight, 1);
                                            lCategory.Controls.Add(ui);
                                            cHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Product, model, pHeight, 1);
                                            lProduct.Controls.Add(ui);
                                            pHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, 1);
                                            lOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                            lWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;

                                            goto nextIndex1;
                                        }
                                        else
                                        {
                                            cHeight += (int)unitHeight;
                                        }

                                        //품목
                                        if (dgv.Rows[i].Cells["product"].Value.ToString() != dgv.Rows[i + 1].Cells["product"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Product, model, pHeight, 1);
                                            lProduct.Controls.Add(ui);
                                            pHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, 1);
                                            lOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;

                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                            lWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;

                                            goto nextIndex1;
                                        }
                                        else
                                        {
                                            pHeight += (int)unitHeight;
                                        }

                                        //원산지
                                        if (dgv.Rows[i].Cells["origin"].Value.ToString() != dgv.Rows[i + 1].Cells["origin"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Origin, model, oHeight, 1);
                                            lOrigin.Controls.Add(ui);
                                            oHeight = (int)unitHeight;
                                        }
                                        else
                                        {
                                            oHeight += (int)unitHeight;
                                        }

                                        //중량
                                        if (dgv.Rows[i].Cells["weight"].Value.ToString() != dgv.Rows[i + 1].Cells["weight"].Value.ToString())
                                        {
                                            ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                            lWeight.Controls.Add(ui);
                                            wHeight = (int)unitHeight;
                                        }
                                        else
                                        {
                                            wHeight += (int)unitHeight;
                                        }

                                    nextIndex1:        //①
                                        Console.WriteLine(page + " / " + area);
                                    }
                                    else
                                    {
                                        model = new SeaoverPriceModel();
                                        model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                        model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                        model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                        model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                        model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                        model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                        model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                        model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                        model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                        model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                        model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                        model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                        model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                                        model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                        model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();
                                        //model.sales_price = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString());

                                        if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                        {
                                            model.sales_price = s;
                                        }
                                        else
                                        {
                                            if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                                model.sales_price = -1;
                                            else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                                model.sales_price = -2;
                                            else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                                model.sales_price = -3;
                                        }
                                        model.note = dgv.Rows[i].Cells["weight"].Value.ToString();
                                        //대분류
                                        ui = new ProductUnit(this, UnitType.Category, model, cHeight, 1);
                                        lCategory.Controls.Add(ui);
                                        cHeight = (int)unitHeight;
                                        //폼목
                                        ui = new ProductUnit(this, UnitType.Product, model, pHeight, 1);
                                        lProduct.Controls.Add(ui);
                                        pHeight = (int)unitHeight;
                                        //원산지
                                        ui = new ProductUnit(this, UnitType.Origin, model, oHeight, 1);
                                        lOrigin.Controls.Add(ui);
                                        oHeight = (int)unitHeight;
                                        //중량6
                                        ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                        lWeight.Controls.Add(ui);
                                        wHeight = (int)unitHeight;
                                    }
                                }
                                else
                                {
                                    model = new SeaoverPriceModel();
                                    model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                                    model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                                    model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                                    model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                                    model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                                    model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                                    model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                                    model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                                    model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                                    model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                                    model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                                    model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                                    model.edit_date = dgv.Rows[i].Cells["edit_date"].Value.ToString();
                                    model.manager1 = dgv.Rows[i].Cells["manager1"].Value.ToString();
                                    //model.sales_price = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString());
                                    if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                                    {
                                        model.sales_price = s;
                                    }
                                    else
                                    {
                                        if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                            model.sales_price = -1;
                                        else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                            model.sales_price = -2;
                                        else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                            model.sales_price = -3;
                                    }
                                    model.note = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    //대분류
                                    ui = new ProductUnit(this, UnitType.Category, model, cHeight, 1);
                                    lCategory.Controls.Add(ui);
                                    cHeight = (int)unitHeight;
                                    //폼목
                                    ui = new ProductUnit(this, UnitType.Product, model, pHeight, 1);
                                    lProduct.Controls.Add(ui);
                                    pHeight = (int)unitHeight;
                                    //원산지
                                    ui = new ProductUnit(this, UnitType.Origin, model, oHeight, 1);
                                    lOrigin.Controls.Add(ui);
                                    oHeight = (int)unitHeight;
                                    //중량6
                                    ui = new ProductUnit(this, UnitType.Weight, model, wHeight, 1);
                                    lWeight.Controls.Add(ui);
                                    wHeight = (int)unitHeight;

                                }
                            }
                        }
                    }
                }
            }
        }


        public void UpdateCommit(SeaoverPriceModel model, ProductUnit.UnitType ut, string updateTxt)
        {
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1 && model != null)
            {
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    string cate1 = dgv.Rows[i].Cells["category_code"].Value.ToString();
                    string cate2 = model.category_code.ToString();
                    if (!string.IsNullOrEmpty(cate1))
                    {
                        if (cate1.Substring(1) == "C" || cate1.Substring(1) == "F")
                        {
                            if(cate1.Length >= 1)
                                cate1 = cate1.Substring(1);
                            if (cate2.Length >= 1)
                                cate2 = cate2.Substring(1);
                        }
                        else
                        {
                            if (cate1.Length >= 2)
                                cate1 = cate1.Substring(0, 2);
                            if (cate2.Length >= 2)
                                cate2 = cate2.Substring(0, 2);
                        }
                    }
                    else
                    {
                        cate1 = cate1;
                        cate2 = cate2;
                    }


                    switch (ut)
                    {
                        case UnitType.Category:

                            if (cate1 == cate2)
                            {
                                model.category = updateTxt;
                                dgv.Rows[i].Cells["category"].Value = updateTxt;
                            }
                            break;

                        case UnitType.Product:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString())
                            {
                                /*model.product = updateTxt;*/
                                dgv.Rows[i].Cells["product"].Value = updateTxt;
                            }
                            break;
                        case UnitType.Origin:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                            && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString())
                            {
                                /*model.origin = updateTxt;*/
                                dgv.Rows[i].Cells["origin"].Value = updateTxt;
                            }
                            break;
                        case UnitType.sizes:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                            && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString()
                            && dgv.Rows[i].Cells["sizes_code"].Value.ToString() == model.sizes_code.ToString())
                            {
                                /*model.sizes = updateTxt;*/
                                dgv.Rows[i].Cells["sizes"].Value = updateTxt;
                            }
                            break;
                        case UnitType.Weight:
                            if (cate1 == cate2
                            && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                            && dgv.Rows[i].Cells["weight"].Value.ToString() == model.weight.ToString())
                            {
                                /*model.note = updateTxt;*/
                                dgv.Rows[i].Cells["weight"].Value = updateTxt;
                            }
                            break;
                        case UnitType.price:

                            if (cate1 == cate2
                                && dgv.Rows[i].Cells["product_code"].Value.ToString() == model.product_code.ToString()
                                && dgv.Rows[i].Cells["origin_code"].Value.ToString() == model.origin_code.ToString()
                                && dgv.Rows[i].Cells["sizes_code"].Value.ToString() == model.sizes_code.ToString()
                                && dgv.Rows[i].Cells["weight"].Value.ToString() == model.weight.ToString())
                            {

                                if ((model.sales_price >= 0 && dgv.Rows[i].Cells["sales_price"].Value.ToString().Replace(",", "") == model.sales_price.ToString().Replace(",", "")))
                                    dgv.Rows[i].Cells["sales_price"].Value = updateTxt;
                                else if (model.sales_price < 0)
                                { 
                                    if((model.sales_price == -1 && dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-") 
                                        || (model.sales_price == -2 && dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                        || (model.sales_price == -3 && dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정")))
                                        dgv.Rows[i].Cells["sales_price"].Value = updateTxt;
                                }


                                /*updateTxt = updateTxt.Replace("/팩", "");
                                updateTxt = updateTxt.Replace("/p", "");
                                updateTxt = updateTxt.Replace("/k", "");
                                model.sales_price = Convert.ToDouble(updateTxt);*/
                                
                            }
                            break;
                    }
                }
            }
            SettingForm();
        }
        #endregion

        #region 단가정보 가져오기
        private void SetCelssNullData()
        {
            if (dgvProduct.Rows.Count > 1)
            {
                for (int i = 0; i < dgvProduct.Rows.Count - 1; i++)
                {
                    if (string.IsNullOrEmpty((string)dgvProduct.Rows[i].Cells["category"].Value.ToString()))
                    {
                        dgvProduct.Rows[i].Cells["category"].Value = "";
                    }
                    if (string.IsNullOrEmpty((string)dgvProduct.Rows[i].Cells["product"].Value.ToString()))
                    {
                        dgvProduct.Rows[i].Cells["product"].Value = "";
                    }
                    if (string.IsNullOrEmpty((string)dgvProduct.Rows[i].Cells["origin"].Value.ToString()))
                    {
                        dgvProduct.Rows[i].Cells["origin"].Value = "";
                    }
                    if (string.IsNullOrEmpty((string)dgvProduct.Rows[i].Cells["weight"].Value.ToString()))
                    {
                        dgvProduct.Rows[i].Cells["weight"].Value = "";
                    }
                    if (string.IsNullOrEmpty((string)dgvProduct.Rows[i].Cells["sizes"].Value.ToString()))
                    {
                        dgvProduct.Rows[i].Cells["sizes"].Value = "";
                    }
                    /*if (string.IsNullOrEmpty((string)dgvProduct.Rows[i].Cells["price"].Value.ToString()))
                    {
                        dgvProduct.Rows[i].Cells["price"].Value = 0;
                    }*/

                    if (string.IsNullOrEmpty((string)dgvProduct.Rows[i].Cells["division"].Value.ToString()))
                    {
                        dgvProduct.Rows[i].Cells["division"].Value = "";
                    }
                }
            }
        }

        //단가내용 Dic에 담아 나누는 Method 
        private List<Dictionary<string, List<SeaoverPriceModel>>> createPageList()
        {
            SetCelssNullData();
            List<Dictionary<string, List<SeaoverPriceModel>>> pageList = new List<Dictionary<string, List<SeaoverPriceModel>>>();     //페이지별 단가내용(좌/우)
            Dictionary<string, List<SeaoverPriceModel>> productDic = new Dictionary<string, List<SeaoverPriceModel>>();               //대분류별 단가리스트
            List<SeaoverPriceModel> list = new List<SeaoverPriceModel>();                                                             //단가리스트
            SeaoverPriceModel model = new SeaoverPriceModel();                                                                        //단가

            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count <= 1)
            {
                return pageList;
            }
            //순회
            int rows = 0;
            int idx = 0;
            string tempStr = dgv.Rows[0].Cells["category"].Value.ToString(); ;
            for (int i = 0; i < dgv.Rows.Count - 1; i++)
            {
                rows += 1;    //실제출력 rowindx
                idx = i;
                bool isHave;

                //Dic Add Page (한쪽에 59개씩 출력)
                if (((double)rows % (double)59) == 0)
                {
                    model = new SeaoverPriceModel();
                    model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                    model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                    model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                    model.unit = dgv.Rows[i].Cells["weight"].Value.ToString();
                    if (isHave = dgv.Rows[i].Cells["weight"].Value.ToString().Contains("팩"))
                    {
                        model.price_unit = "팩";
                    }
                    model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                    //model.sales_price = dgv.Rows[i].Cells["price"].Value.ToString();
                    //리스트, Dic 추가
                    list.Add(model);
                    //productDic.Add(dgv.Rows[i].Cells["category"].Value.ToString(), list);
                    productDic = AddProductDic(productDic, dgv.Rows[i].Cells["category"].Value.ToString(), list);
                    //비교 품목최신화
                    if (i + 1 < dgv.Rows.Count - 1)
                    {
                        tempStr = dgv.Rows[i + 1].Cells["category"].Value.ToString();
                    }
                    else
                    {
                        tempStr = "";
                    }
                    //PageList 추가 
                    pageList.Add(productDic);
                    //초기화
                    productDic = new Dictionary<string, List<SeaoverPriceModel>>();
                    list = new List<SeaoverPriceModel>();
                }
                //Dic Add Page (한쪽에 58개씩 출력)
                else
                {
                    //대분류가 같을 경우 list추가
                    if (tempStr == dgv.Rows[i].Cells["category"].Value.ToString())
                    {
                        model = new SeaoverPriceModel();
                        model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                        model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                        model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                        model.unit = dgv.Rows[i].Cells["weight"].Value.ToString();
                        if (isHave = dgv.Rows[i].Cells["weight"].Value.ToString().Contains("팩"))
                        {
                            model.price_unit = "팩";
                        }
                        model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                        //model.sales_price = dgv.Rows[i].Cells["price"].Value.ToString();

                        list.Add(model);
                    }
                    //대분류가 같지 않을 경우 Dic에 list 추가
                    else if (tempStr != dgv.Rows[i].Cells["category"].Value.ToString())
                    {
                        productDic = AddProductDic(productDic, tempStr, list);
                        //productDic.Add(tempStr, list);            //Dic 추가

                        list = new List<SeaoverPriceModel>();                                     //List 초기화 후 현재행 부터 다시 list
                        model = new SeaoverPriceModel();
                        model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                        model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                        model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                        model.unit = dgv.Rows[i].Cells["weight"].Value.ToString();
                        if (isHave = dgv.Rows[i].Cells["weight"].Value.ToString().Contains("팩"))
                        {
                            model.price_unit = "팩";
                        }
                        model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                        //model.sales_price = dgv.Rows[i].Cells["price"].Value.ToString();

                        list.Add(model);

                        //비교 대분류, 실제출력INDEX 최신화
                        tempStr = dgv.Rows[i].Cells["category"].Value.ToString();

                        //Dic Add Page (한쪽에 59개씩 출력)
                        if (((double)rows % (double)59) == 0)
                        {
                            model = new SeaoverPriceModel();
                            model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                            model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                            model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                            model.unit = dgv.Rows[i].Cells["weight"].Value.ToString();
                            if (isHave = dgv.Rows[i].Cells["weight"].Value.ToString().Contains("팩"))
                            {
                                model.price_unit = "팩";
                            }
                            model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                            //model.sales_price = dgv.Rows[i].Cells["price"].Value.ToString();
                            //리스트, Dic 추가
                            list.Add(model);
                            //productDic.Add(dgv.Rows[i].Cells["category"].Value.ToString(), list);
                            productDic = AddProductDic(productDic, dgv.Rows[i].Cells["category"].Value.ToString(), list);
                            //비교 품목최신화
                            if (i + 1 < dgv.Rows.Count)
                            {
                                tempStr = dgv.Rows[i + 1].Cells["category"].Value.ToString();
                            }
                            else
                            {
                                tempStr = "";
                            }
                            //PageList 추가 
                            pageList.Add(productDic);
                            //초기화
                            productDic = new Dictionary<string, List<SeaoverPriceModel>>();
                            list = new List<SeaoverPriceModel>();
                        }
                    }
                }
            }
            //마지막 list Dic,PageList 추가
            //productDic.Add(tempStr, list);
            productDic = AddProductDic(productDic, tempStr, list);
            pageList.Add(productDic);
            return pageList;
        }

        //Dictionary 추가
        private Dictionary<string, List<SeaoverPriceModel>> AddProductDic(Dictionary<string, List<SeaoverPriceModel>> dic, string str, List<SeaoverPriceModel> pList)
        {
            bool keyExists = dic.ContainsKey(str);
            if (keyExists)
            {
                for (int i = 0; i < pList.Count; i++)
                {
                    dic[str].Add(pList[i]);
                }
            }
            else
            {
                dic.Add(str, pList);
            }

            return dic;
        }

        #endregion

        #region SEAOVER 데이터 불러오기, 엑셀내려받기

        private void excelOneFormCreate()
        {
            processingFlag = true;
            DataGridView dgv = dgvProduct;
            // ToDo
            this.dgvProduct.CellPainting -= new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvProduct_CellPainting);
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;

                if (dgv.Rows.Count > 1)
                {
                    //시트복사
                    //double tPage = Convert.ToDouble(lbTotal.Text.Replace("/", "").Trim());
                    double tPage = TotalPage;
                    if (tPage > 0)
                    {
                        //index
                        int rowIndex = 7;
                        int colIndex = 2;
                        int cPage = 1;
                        setOneFormSheetBasicSetting(excelApp, workBook, workSheet, cPage, (int)tPage);
                        Excel.Worksheet xlSht = workBook.Sheets[1];
                        for (int i = 2; i <= tPage; i++)
                        {
                            xlSht.Copy(Type.Missing, workBook.Sheets[workBook.Sheets.Count]);
                            string formName = txtFormName.Text;
                            formName = formName.Replace("/", "").Replace(".", "").Replace(@"\", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("*", "");
                            if (formName.Length > 24)
                                formName = formName.Substring(0, 24);
                            workBook.Sheets[workBook.Sheets.Count].Name = formName + "(" + i + "-" + tPage + ")";
                            workBook.Sheets[workBook.Sheets.Count].Cells[2, 2].Value = formName + "(" + i + " / " + tPage + ")";
                        }
                        //단가출력
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            //Add Sheet
                            if (cPage != (int)dgv.Rows[i].Cells["page"].Value)
                            {
                                cellsMerge(workSheet);
                                cPage = (int)dgv.Rows[i].Cells["page"].Value;
                                /*workBook.Worksheets.Add(After: workSheet);
                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;
                                setOneFormSheetBasicSetting(excelApp, workBook, workSheet, cPage, (int)tPage);*/

                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;

                                rowIndex = 7;
                            }
                            else
                            {
                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;
                            }

                            //Output
                            for (int j = 0; j < Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value); j++)
                            {
                                if (rowIndex + j <= 66)
                                {
                                    workSheet.Cells[rowIndex + j, colIndex].Value = dgv.Rows[i].Cells["category"].Value.ToString();

                                    bool isAccent = Convert.ToBoolean(dgv.Rows[i].Cells["accent"].Value.ToString());
                                    if (isAccent)
                                    {
                                        workSheet.Cells[rowIndex + j, colIndex + 1].Value = "★" + dgv.Rows[i].Cells["product"].Value.ToString();
                                    }
                                    else
                                    {
                                        workSheet.Cells[rowIndex + j, colIndex + 1].Value = dgv.Rows[i].Cells["product"].Value.ToString();
                                    }

                                    string str = dgv.Rows[i].Cells["origin"].Value.ToString();
                                    string originStr = "";
                                    if (str.Length > 3)
                                    {
                                        int indexStart = 0;
                                        int indexEnd = 0;
                                        int iSplit = 1;
                                        int totalLength = str.Length;
                                        int forCount = totalLength / iSplit;


                                        for (int k = 0; k < forCount + 1; k++)
                                        {
                                            if (totalLength < indexStart + iSplit)
                                            {
                                                indexEnd = totalLength - indexStart;
                                            }
                                            else
                                            {
                                                indexEnd = str.Substring(indexStart, iSplit).LastIndexOf("") + 1;
                                            }

                                            originStr += str.Substring(indexStart, indexEnd) + "\r\n";

                                            indexStart += indexEnd;
                                        }
                                        if (originStr.Length > 4)
                                        {
                                            originStr = originStr.Substring(0, originStr.Length - 4);
                                        }

                                    }
                                    else
                                    {
                                        originStr = str;
                                    }

                                    workSheet.Cells[rowIndex + j, colIndex + 2].Value = originStr;
                                    workSheet.Cells[rowIndex + j, colIndex + 3].Value = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    workSheet.Cells[rowIndex + j, colIndex + 4].Value = " " + dgv.Rows[i].Cells["sizes"].Value.ToString() + " ";


                                    if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "0" || dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-" || dgv.Rows[i].Cells["sales_price"].Value.ToString() == "")
                                        workSheet.Cells[rowIndex + j, colIndex + 5].Value = "-";
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                        workSheet.Cells[rowIndex + j, colIndex + 5].Value = "문의";
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                        workSheet.Cells[rowIndex + j, colIndex + 5].Value = "★통관예정★";
                                    else
                                    {
                                        if (dgv.Rows[i].Cells["price_unit"].Value.ToString().Contains("팩"))
                                        {
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = dgv.Rows[i].Cells["sales_price"].Value.ToString() + "/p";
                                        }
                                        else if (dgv.Rows[i].Cells["price_unit"].Value.ToString() == "kg")
                                        {
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = dgv.Rows[i].Cells["sales_price"].Value.ToString() + "/k";
                                        }
                                        else
                                        {
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = dgv.Rows[i].Cells["sales_price"].Value.ToString();
                                        }
                                    }
                                    //강조
                                    if (isAccent)
                                    {
                                        double sales_price;
                                        if (double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out sales_price))
                                        {
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = "★" + sales_price.ToString("#,##0");
                                        }
                                        else
                                        {
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = "★" + dgv.Rows[i].Cells["sales_price"].Value.ToString();
                                        }
                                    }
                                }
                            }

                            rowIndex += Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);

                            //마지막 머지
                            if (i == dgv.Rows.Count - 2)
                            {
                                cellsMerge(workSheet);
                            }
                        }
                    }
                }
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
                this.Activate();
                setAutomatic(excelApp, true);
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
            finally
            {
                if (saveFlag)
                {
                    try
                    {
                        workBook.SaveAs(savePath);
                        ReleaseObject(workSheet);
                        ReleaseObject(workBook);
                        ReleaseObject(excelApp);
                        excelApp.Quit();
                    }
                    catch
                    { }
                }
                else
                {
                    //workBook.Worksheets.PrintPreview(true);
                    ReleaseObject(workSheet);
                    ReleaseObject(workBook);
                    ReleaseObject(excelApp);
                }
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
            processingFlag = false;
        }

        private void excelCreate()
        {
            processingFlag = true;
            DataGridView dgv = dgvProduct;
            // ToDo
            this.dgvProduct.CellPainting -= new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvProduct_CellPainting);
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;
                if (dgv.Rows.Count > 1)
                {
                    //시트복사
                    //double tPage = Convert.ToDouble(lbTotal.Text.Replace("/", "").Trim());
                    double tPage = TotalPage;
                    if (tPage > 0)
                    {
                        //Index
                        int rowIndex = 7;
                        int colIndex = 2;
                        int cPage = 1;
                        setSheetBasicSetting(excelApp, workBook, workSheet, cPage, (int)tPage);
                        Excel.Worksheet xlSht = workBook.Sheets[1];
                        for (int i = 2; i <= tPage; i++)
                        {
                            xlSht.Copy(Type.Missing, workBook.Sheets[workBook.Sheets.Count]);
                            string formName = txtFormName.Text;
                            formName = formName.Replace("/", "").Replace(".", "").Replace(@"\", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("*", "");
                            if (formName.Length > 24)
                                formName = formName.Substring(0, 24);
                            workBook.Sheets[workBook.Sheets.Count].Name = formName + "(" + i + "-" + tPage + ")";
                            workBook.Sheets[workBook.Sheets.Count].Cells[2, 2].Value = formName + "(" + i + " / " + tPage + ")";
                        }

                        //Output location
                        Excel.Range rng = workSheet.get_Range("B7", "CG3000");
                        object[,] only_data = (object[,])rng.get_Value();


                        //단가출력
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            int accent2;
                            if (dgv.Rows[i].Cells["accent2"].Value == null || !int.TryParse(dgv.Rows[i].Cells["accent2"].Value.ToString(), out accent2))
                                accent2 = 0;

                            //Add Sheet
                            if (cPage != (int)dgv.Rows[i].Cells["page"].Value)
                            {
                                cellsMerge(workSheet);
                                cPage = (int)dgv.Rows[i].Cells["page"].Value;
                                /*workBook.Worksheets.Add(After: workSheet);
                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;
                                setSheetBasicSetting(excelApp, workBook, workSheet, cPage, (int)tPage);*/

                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;

                                rowIndex = 7;
                            }
                            else
                            {
                                workSheet = workBook.Worksheets.get_Item((int)dgv.Rows[i].Cells["page"].Value) as Excel.Worksheet;
                            }




                            // row, column Index
                            if (dgv.Rows[i].Cells["area"].Value == "L")
                            {
                                colIndex = 2;
                            }
                            else if (dgv.Rows[i].Cells["area"].Value == "R")
                            {
                                colIndex = 9;
                                if (dgv.Rows[i - 1].Cells["area"].Value != dgv.Rows[i].Cells["area"].Value)
                                {
                                    rowIndex = 7;
                                }
                            }

                            //Output
                            for (int j = 0; j < Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value); j++)
                            {
                                if (rowIndex + j <= 66)
                                {
                                    //Accent
                                    bool isAccent = Convert.ToBoolean(dgv.Rows[i].Cells["accent"].Value);
                                    //category
                                    string category = dgv.Rows[i].Cells["category"].Value.ToString();
                                    category = Regex.Replace(category, "(?<!\r>\n)", "");
                                    workSheet.Cells[rowIndex + j, colIndex].Value = category;
                                    //product
                                    string product;
                                    if (isAccent)
                                        product = "★" + dgv.Rows[i].Cells["product"].Value.ToString();
                                    else
                                        product = dgv.Rows[i].Cells["product"].Value.ToString();
                                    product = Regex.Replace(product, "\u00A0", "");
                                    product = Regex.Replace(product, "(?<!\r>\n)", "");
                                    product = product.Replace("\r\n​", "\n");
                                    workSheet.Cells[rowIndex + j, colIndex + 1].Value = product;
                                    //Origin
                                    string str = dgv.Rows[i].Cells["origin"].Value.ToString();
                                    string originStr = "";
                                    if (str.Length > 3)
                                    {
                                        int indexStart = 0;
                                        int indexEnd = 0;
                                        int iSplit = 2;
                                        int totalLength = str.Length;
                                        int forCount = totalLength / iSplit;


                                        for (int k = 0; k < forCount + 1; k++)
                                        {
                                            if (totalLength < indexStart + iSplit)
                                            {
                                                indexEnd = totalLength - indexStart;
                                            }
                                            else
                                            {
                                                indexEnd = str.Substring(indexStart, iSplit).LastIndexOf("") + 1;
                                            }

                                            originStr += str.Substring(indexStart, indexEnd) + "\r\n";

                                            indexStart += indexEnd;
                                        }
                                        if (originStr.Length > 4)
                                        {
                                            originStr = originStr.Substring(0, originStr.Length - 4);
                                        }

                                    }
                                    else
                                    {
                                        originStr = str;
                                    }
                                    originStr = Regex.Replace(originStr, "\u00A0", "");
                                    originStr = Regex.Replace(originStr, "(?<!\r>\n)", "");
                                    
                                    workSheet.Cells[rowIndex + j, colIndex + 2].Value = originStr;
                                    //Weight
                                    string weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                                    weight = Regex.Replace(weight, "\u00A0", "");
                                    weight = Regex.Replace(weight, "(?<!\r>\n)", "");
                                    workSheet.Cells[rowIndex + j, colIndex + 3].Value = weight;
                                    //Sizes
                                    string sizes = " " + dgv.Rows[i].Cells["sizes"].Value.ToString() +  "";
                                    sizes = Regex.Replace(sizes, "\u00A0", "");
                                    sizes = Regex.Replace(sizes, "(?<!\r>\n)", "");
                                    workSheet.Cells[rowIndex + j, colIndex + 4].Value = sizes;

                                    //Sales Price
                                    if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "0" || dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-" || dgv.Rows[i].Cells["sales_price"].Value.ToString() == "")
                                        workSheet.Cells[rowIndex + j, colIndex + 5].Value = "-";
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                        workSheet.Cells[rowIndex + j, colIndex + 5].Value = "문의";
                                    else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                        workSheet.Cells[rowIndex + j, colIndex + 5].Value = "★통관예정★";
                                    else
                                    {
                                        string sales_price = dgv.Rows[i].Cells["sales_price"].Value.ToString();
                                        sales_price = Regex.Replace(sales_price, "\u00A0", "");
                                        sales_price = Regex.Replace(sales_price, "(?<!\r>\n)", "");

                                        if (dgv.Rows[i].Cells["price_unit"].Value.ToString().Contains("팩"))
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = sales_price + "/p";
                                        else if (dgv.Rows[i].Cells["price_unit"].Value.ToString() == "kg")
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = sales_price + "/k";
                                        else
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = sales_price;
                                    }

                                    //강조
                                    if (isAccent)
                                    {
                                        string sales_price = dgv.Rows[i].Cells["sales_price"].Value.ToString();
                                        sales_price = Regex.Replace(sales_price, "\u00A0", "");
                                        sales_price = Regex.Replace(sales_price, "(?<!\r>\n)", "");

                                        if (double.TryParse(sales_price, out double sales_price_dbl))
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = "★" + sales_price_dbl.ToString("#,##0");
                                        else
                                            workSheet.Cells[rowIndex + j, colIndex + 5].Value = "★" + sales_price;
                                    }
                                    //음표제거

                                    /*string tmpStr = workSheet.Cells[rowIndex + j, colIndex].Value;
                                    tmpStr = tmpStr.Replace((char)13, (char)0);*/

                                    if (accent2 < 0)
                                    {
                                        Excel.Range rng2 = workSheet.Range[workSheet.Cells[rowIndex + j, colIndex + 5], workSheet.Cells[rowIndex + j, colIndex + 5]];
                                        rng2.Font.Color = Color.LightGray;
                                    }
                                }
                            }

                            rowIndex += Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);

                            //마지막 머지
                            if (i == dgv.Rows.Count - 2)
                            {
                                cellsMerge(workSheet);
                            }
                        }
                    }
                }
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
                this.Activate();
                setAutomatic(excelApp, true);
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
            finally
            {
                if (saveFlag)
                {
                    try
                    {
                        workBook.SaveAs(savePath);
                        ReleaseObject(workSheet);
                        ReleaseObject(workBook);
                        ReleaseObject(excelApp);
                        excelApp.Quit();
                    }
                    catch
                    { }
                }
                else
                {
                    //workBook.Worksheets.PrintPreview(true);
                    ReleaseObject(workSheet);
                    ReleaseObject(workBook);
                    ReleaseObject(excelApp);
                }
                
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
            processingFlag = false;
        }
        private void cellsMerge(Excel.Worksheet wk)
        {
            //Merge
            int[] endRow = { 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7 };

            Excel.Range rg1 = wk.Range[wk.Cells[7, 8], wk.Cells[66, 8]];
            rg1.Merge();
            int col;
            string str1, str2;
            for (int j = 8; j < 68; j++)
            {
                //Left
                col = 2;
                str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                str2 = string.Concat(wk.Cells[j, col].Value, " ");
                if (str1 != str2)
                //if (wk.Cells[endRow[col - 2], col].Value != wk.Cells[j, col].Value)
                {
                    int tmp_row = j - endRow[col - 2];
                    string val = wk.Cells[endRow[col - 2], col].Value;
                    if (wk.Cells[endRow[col - 2], col].Value == null)
                        val = "";
                    if(val == null)
                        val = "";
                    val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                    int valLen = val.Length;
                    //5행이하, 6글자 이상
                    if (valLen > 5 & tmp_row <= 5)
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((k + 1) % 2 == 0)
                            {
                                if ((stt_idx + 2) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 2);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                }
                                stt_idx += 2;
                            }
                        }
                        wk.Cells[endRow[col - 2], col].Value = tempStr;
                    }
                    //5행이하, 6글자 이상
                    else if (valLen > 1 & tmp_row <= 2)
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((k + 1) % 2 == 0)
                            {
                                if ((stt_idx + 2) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 2);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                }
                                stt_idx += 2;
                            }
                        }
                        wk.Cells[endRow[col - 2], col].Value = tempStr;
                    }
                    //6행이상, 6글자 이상
                    else
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((stt_idx + 1) == valLen)
                            {
                                tempStr += val.Substring(stt_idx, 1);
                            }
                            else
                            {
                                tempStr += val.Substring(stt_idx, 1) + "\r\n";
                            }
                            stt_idx += 1;
                        }
                        wk.Cells[endRow[col - 2], col].Value = tempStr;
                    }


                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                    rg1.Merge();
                    rg1.WrapText = true;
                    rg1.Interior.Color = Color.Beige;
                    endRow[col - 2] = j;
                }
                else
                {
                    wk.Cells[j, col].Value = "";
                }

                col = 3;
                str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                str2 = string.Concat(wk.Cells[j, col].Value, " ");
                if (str1 != str2)
                //if (wk.Cells[endRow[col - 2], col].Value != wk.Cells[j, col].Value)
                {
                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                    rg1.Merge();
                    rg1.WrapText = true;
                    endRow[col - 2] = j;
                }
                else
                {
                    wk.Cells[j, col].Value = "";
                }

                col = 4;
                str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                str2 = string.Concat(wk.Cells[j, col].Value, " ");
                if (str1 != str2)
                {
                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                    rg1.Merge();
                    rg1.WrapText = true;
                    endRow[col - 2] = j;
                }
                else
                {
                    wk.Cells[j, col].Value = "";
                }

                col = 5;
                str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                str2 = string.Concat(wk.Cells[j, col].Value, " ");
                if (str1 != str2)
                {
                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                    rg1.Merge();
                    rg1.WrapText = true;
                    endRow[col - 2] = j;
                }
                else
                {
                    wk.Cells[j, col].Value = "";
                }

                col = 6;
                str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                str2 = string.Concat(wk.Cells[j, col].Value, " ");
                if (str1 != str2)
                {
                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                    rg1.Merge();
                    rg1.WrapText = true;
                    endRow[col - 2] = j;
                }
                else
                {
                    wk.Cells[j, col].Value = "";
                }

                col = 7;
                str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                str2 = string.Concat(wk.Cells[j, col].Value, " ");
                if (str1 != str2)
                {
                    rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                    rg1.Merge();
                    rg1.WrapText = true;
                    endRow[col - 2] = j;
                }
                else
                {
                    wk.Cells[j, col].Value = "";
                }

                if (!rbOneline.Checked)
                {
                    //Right
                    col = 9;
                    str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                    str2 = string.Concat(wk.Cells[j, col].Value, " ");
                    if (str2.Contains("기타"))
                        str2 = string.Concat(wk.Cells[j, col].Value, " ");
                    if (str1 != str2)
                    {
                        int tmp_row = j - endRow[col - 2];
                        string val = wk.Cells[endRow[col - 2], col].Value;
                        if (val == null)
                            val = "";
                        val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        int valLen = val.Length;
                        //5행이하, 6글자 이상
                        if (valLen > 5 & tmp_row <= 5)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            wk.Cells[endRow[col - 2], col].Value = tempStr;
                        }
                        //5행이하, 6글자 이상
                        else if (valLen > 1 & tmp_row <= 2)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            wk.Cells[endRow[col - 2], col].Value = tempStr;
                        }
                        //6행이상, 6글자 이상
                        else
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((stt_idx + 1) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 1);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                }
                                
                                stt_idx += 1;
                            }
                            wk.Cells[endRow[col - 2], col].Value = tempStr;
                        }

                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                        rg1.Merge();
                        rg1.WrapText = true;
                        rg1.Interior.Color = Color.Beige;
                        endRow[col - 2] = j;
                    }
                    else
                    {
                        wk.Cells[j, col].Value = "";
                    }
                    col = 10;
                    str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                    str2 = string.Concat(wk.Cells[j, col].Value, " ");
                    
                    if (str1 != str2)
                    {
                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                        rg1.Merge();
                        rg1.WrapText = true;
                        endRow[col - 2] = j;
                    }
                    else
                    {
                        wk.Cells[j, col].Value = "";
                    }
                    col = 11;
                    str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                    str2 = string.Concat(wk.Cells[j, col].Value, " ");
                    if (str1 != str2)
                    {
                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                        rg1.Merge();
                        rg1.WrapText = true;
                        endRow[col - 2] = j;
                    }
                    else
                    {
                        wk.Cells[j, col].Value = "";
                    }
                    col = 12;
                    str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                    str2 = string.Concat(wk.Cells[j, col].Value, " ");
                    if (str1 != str2)
                    {
                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                        rg1.Merge();
                        rg1.WrapText = true;
                        endRow[col - 2] = j;
                    }
                    else
                    {
                        wk.Cells[j, col].Value = "";
                    }
                    col = 13;
                    str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                    str2 = string.Concat(wk.Cells[j, col].Value, " ");
                    if (str1 != str2)
                    {
                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                        rg1.Merge();
                        rg1.WrapText = true;
                        endRow[col - 2] = j;
                    }
                    else
                    {
                        wk.Cells[j, col].Value = "";
                    }
                    col = 14;
                    str1 = string.Concat(wk.Cells[endRow[col - 2], col].Value, " ");
                    str2 = string.Concat(wk.Cells[j, col].Value, " ");
                    if (str1 != str2)
                    {
                        rg1 = wk.Range[wk.Cells[endRow[col - 2], col], wk.Cells[j - 1, col]];
                        rg1.Merge();
                        rg1.WrapText = true;
                        endRow[col - 2] = j;
                    }
                    else
                    {
                        wk.Cells[j, col].Value = "";
                    }
                }
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

        //Excel Sheet Basic
        private void setOneFormSheetBasicSetting(Excel.Application excel, Excel.Workbook wb, Excel.Worksheet wk, int cPage, int tPage)
        {
            string formName = txtFormName.Text;
            formName = formName.Replace("/", "").Replace(".", "").Replace(@"\", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("*", "");
            if (formName.Length > 24)
                formName = formName.Substring(0, 24);
            workSheet.Name = formName + "(" + cPage + "-" + tPage + ")";
            Excel.Range rg1;

            //Column Width
            if (isExcelPriceVisible)
            {
                wk.Columns["A"].ColumnWidth = 2;
                wk.Columns["B"].ColumnWidth = 20;      //대분류

                wk.Columns["C"].ColumnWidth = 60;         //품목

                wk.Columns["D"].ColumnWidth = 30;      //원산지

                wk.Columns["E"].ColumnWidth = 50;      //중량
                wk.Columns["F"].ColumnWidth = 55;      //사이즈
                wk.Columns["G"].ColumnWidth = 55;      //단가
            }
            else
            {
                wk.Columns["A"].ColumnWidth = 2;
                wk.Columns["B"].ColumnWidth = 31;      //대분류

                wk.Columns["C"].ColumnWidth = 71;         //품목

                wk.Columns["D"].ColumnWidth = 41;      //원산지

                wk.Columns["E"].ColumnWidth = 61;      //중량
                wk.Columns["F"].ColumnWidth = 66;      //사이즈
               
                rg1 = wk.Range[wk.Cells[1, 7], wk.Cells[67, 7]];
                rg1.EntireColumn.Hidden = true;
            }

            rg1 = wk.Range[wk.Cells[1, 2], wk.Cells[67, 14]];
            rg1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rg1.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rg1.RowHeight = 37;
            //Font Style
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 30;

            wk.Columns["D"].Font.Name = "맑은 고딕";
            wk.Columns["E"].Font.Name = "맑은 고딕";
            wk.Columns["F"].Font.Name = "맑은 고딕";
            wk.Columns["G"].Font.Name = "맑은 고딕";
            wk.Columns["G"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            wk.Columns["G"].NumberFormatLocal = "#,##0";

            //고정값 및 타이틀
            rg1 = wk.Range[wk.Cells[1, 2], wk.Cells[1, 7]];
            rg1.Merge();

            DateTime updatetime = DateTime.Now;
            /*if (!DateTime.TryParse(lbUpdatetime.Text, out updatetime))
                updatetime = DateTime.Now;*/

            wk.Cells[1, 2].value = "'(광고) ㈜아토무역 TEL : 051-256-3100 , FAX : 051-256-4100  부산광역시 서구 원양로 310     발신일자 : " + updatetime.ToString("yyyy-MM-dd") + "   수신인: 구매담당자님 귀하";
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 22;

            rg1 = wk.Range[wk.Cells[2, 2], wk.Cells[3, 7]];
            rg1.Merge();
            wk.Cells[2, 2].value = txtFormName.Text + " (" + cPage + "/" + tPage + ")";
            rg1.Font.Name = "HY견고딕";
            rg1.Font.Size = 40;
            rg1.Font.Bold = true;

            rg1 = wk.Range[wk.Cells[4, 2], wk.Cells[5, 7]];
            rg1.Merge();

            wk.Cells[4, 2].value = lbRemark.Text;
            wk.Cells[4, 2].Font.Name = "맑은 고딕";
            wk.Cells[4, 2].Font.Size = 28;
            wk.Cells[4, 2].Font.Bold = true;

            rg1 = wk.Range[wk.Cells[67, 2], wk.Cells[67, 7]];
            rg1.Merge();
            wk.Cells[67, 2].value = "'광고수신을 원하지 않는경우 무료전화 080-855-8825 로 연락주시기 바랍니다'";
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 22;

            //Title
            rg1 = wk.Range[wk.Cells[6, 2], wk.Cells[6, 7]];
            rg1.Font.Size = 20;
            rg1.Font.Bold = true;

            wk.Cells[6, 2].value = "구분";
            wk.Cells[6, 3].value = "품목";
            wk.Cells[6, 4].value = "산지";
            wk.Cells[6, 5].value = "중량";
            wk.Cells[6, 6].value = "사이즈";
            wk.Cells[6, 7].value = "단가";
            rg1 = wk.Range[wk.Cells[6, 7], wk.Cells[6, 7]];
            rg1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            rg1.NumberFormatLocal = "@";

            //Border Line Style
            rg1 = wk.Range[wk.Cells[4, 2], wk.Cells[67, 7]];
            rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

            //엑셀 Zoom
            SetZoom(40, wb, excel);
            //인쇄영역 설정
            wk.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
            wk.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
            wk.PageSetup.PrintArea = "B1:G67";
            wk.PageSetup.FitToPagesTall = false;
            /*wk.PageSetup.CenterFooter = "Page &P page, &N page";*/
            wk.PageSetup.FooterMargin = 0;
            wk.PageSetup.PrintGridlines = false;
            wk.PageSetup.TopMargin = 0;
            wk.PageSetup.BottomMargin = 0;
            wk.PageSetup.LeftMargin = 0;
            wk.PageSetup.RightMargin = 0;
            wk.PageSetup.CenterHorizontally = true;
            wk.PageSetup.CenterVertically = true;
            wk.PageSetup.FitToPagesWide = 1;
            wk.Application.ActiveWindow.View = Excel.XlWindowView.xlPageBreakPreview;


            /*wk.VPageBreaks[1].DragOff(Excel.XlDirection.xlToRight, 1);
            wk.HPageBreaks[1].DragOff(Excel.XlDirection.xlDown, 1);*/
            try
            {
                wk.VPageBreaks[1].DragOff(Excel.XlDirection.xlToRight, 1);
            }
            catch (Exception ex)
            { }
            try
            {
                wk.HPageBreaks[1].DragOff(Excel.XlDirection.xlDown, 1);
            }
            catch (Exception ex)
            { }
            wk.Application.ActiveWindow.View = Excel.XlWindowView.xlNormalView;
        }

        //Excel Sheet Basic
        private void setSheetBasicSetting(Excel.Application excel, Excel.Workbook wb, Excel.Worksheet wk, int cPage, int tPage)
        {
            string formName = txtFormName.Text;
            formName = formName.Replace("/", "").Replace(".", "").Replace(@"\", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("*", "");
            if (formName.Length > 24)
                formName = formName.Substring(0, 24);
            workSheet.Name = formName + "(" + cPage + "-" + tPage + ")";
            Excel.Range rg1;

            //Column Width
            wk.Columns["A"].ColumnWidth = 2;
            wk.Columns["H"].ColumnWidth = 1.25;       //중간선


            //단가 노출
            if (isExcelPriceVisible)
            {
                wk.Columns["B"].ColumnWidth = 15;      //대분류
                wk.Columns["I"].ColumnWidth = 15;      //대분류

                wk.Columns["C"].ColumnWidth = 30;         //품목
                wk.Columns["J"].ColumnWidth = 30;         //품목

                wk.Columns["D"].ColumnWidth = 15;      //원산지
                wk.Columns["K"].ColumnWidth = 15;      //원산지

                wk.Columns["E"].ColumnWidth = 25;      //중량
                wk.Columns["F"].ColumnWidth = 30;      //사이즈
                wk.Columns["G"].ColumnWidth = 30;      //단가

                wk.Columns["L"].ColumnWidth = 25;      //중량
                wk.Columns["M"].ColumnWidth = 30;      //사이즈
                wk.Columns["N"].ColumnWidth = 30;      //단가
            }
            else
            {
                wk.Columns["B"].ColumnWidth = 15;      //대분류
                wk.Columns["I"].ColumnWidth = 15;      //대분류

                wk.Columns["C"].ColumnWidth = 40;         //품목
                wk.Columns["J"].ColumnWidth = 40;         //품목

                wk.Columns["D"].ColumnWidth = 23;      //원산지
                wk.Columns["K"].ColumnWidth = 23;      //원산지

                wk.Columns["E"].ColumnWidth = 26;      //중량
                wk.Columns["F"].ColumnWidth = 36;      //사이즈

                rg1 = wk.Range[wk.Cells[1, 7], wk.Cells[999, 7]];
                rg1.EntireColumn.Hidden = true;

                wk.Columns["L"].ColumnWidth = 26;      //중량
                wk.Columns["M"].ColumnWidth = 36;      //사이즈

                rg1 = wk.Range[wk.Cells[1, 14], wk.Cells[999, 14]];
                rg1.EntireColumn.Hidden = true;
            }

            rg1 = wk.Range[wk.Cells[1, 2], wk.Cells[67, 14]];
            rg1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rg1.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rg1.RowHeight = 37;

            //Font Style
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 22;
            wk.Columns["G"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            wk.Columns["G"].NumberFormatLocal = "#,##0";
            wk.Columns["N"].Font.Name = "맑은 고딕";
            wk.Columns["N"].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            wk.Columns["N"].NumberFormatLocal = "#,##0";

            //고정값 및 타이틀
            rg1 = wk.Range[wk.Cells[1, 2], wk.Cells[1, 14]];
            rg1.Merge();
            DateTime updatetime = DateTime.Now;
            /*if (!DateTime.TryParse(lbUpdatetime.Text, out updatetime))
                updatetime = DateTime.Now;*/

            wk.Cells[1, 2].value = "'(광고) ㈜아토무역 TEL : 051-256-3100 , FAX : 051-256-4100  부산광역시 서구 원양로 310     발신일자 : " + updatetime.ToString("yyyy-MM-dd") + "   수신인: 구매담당자님 귀하";
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 22;

            rg1 = wk.Range[wk.Cells[2, 2], wk.Cells[3, 14]];
            rg1.Merge();
            wk.Cells[2, 2].value = txtFormName.Text + " (" + cPage + "/" + tPage + ")";
            rg1.Font.Name = "HY견고딕";
            rg1.Font.Size = 40;
            rg1.Font.Bold = true;

            rg1 = wk.Range[wk.Cells[4, 2], wk.Cells[5, 14]];
            rg1.Merge();

            /*string managerTxt;
            if (um.grade == null)
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }
            else if (string.IsNullOrEmpty(um.grade))
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }
            else
            {
                managerTxt = "담당자 : " + um.user_name.ToString() + " " + um.grade.ToString() + " (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
            }
            wk.Cells[4, 2].value = managerTxt + " / *단가 조정 필요하신 부분은 문의 부탁드리겠습니다*";*/
            wk.Cells[4, 2].value = lbRemark.Text;
            wk.Cells[4, 2].Font.Name = "맑은 고딕";
            wk.Cells[4, 2].Font.Size = 28;
            wk.Cells[4, 2].Font.Bold = true;

            rg1 = wk.Range[wk.Cells[67, 2], wk.Cells[67, 14]];
            rg1.Merge();
            wk.Cells[67, 2].value = "'광고수신을 원하지 않는경우 무료전화 080-855-8825 로 연락주시기 바랍니다'";
            rg1.Font.Name = "맑은 고딕";
            rg1.Font.Size = 22;

            //Title
            rg1 = wk.Range[wk.Cells[6, 2], wk.Cells[6, 14]];
            rg1.Font.Size = 22;
            rg1.Font.Bold = true;
            wk.Cells[6, 2].value = "구분";
            wk.Cells[6, 9].value = "구분";
            wk.Cells[6, 3].value = "품목";
            wk.Cells[6, 10].value = "품목";
            wk.Cells[6, 4].value = "산지";
            wk.Cells[6, 11].value = "산지";
            wk.Cells[6, 5].value = "중량";
            wk.Cells[6, 12].value = "중량";
            wk.Cells[6, 6].value = "사이즈";
            wk.Cells[6, 13].value = "사이즈";
            wk.Cells[6, 7].value = "단가";
            rg1 = wk.Range[wk.Cells[6, 7], wk.Cells[6, 7]];
            rg1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            rg1.NumberFormatLocal = "@";
            wk.Cells[6, 14].value = "단가";
            rg1 = wk.Range[wk.Cells[6, 14], wk.Cells[6, 14]];
            rg1.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            rg1.NumberFormatLocal = "@";

            //Border Line Style
            rg1 = wk.Range[wk.Cells[4, 2], wk.Cells[67, 14]];
            rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

            //엑셀 Zoom
            SetZoom(40, wb, excel);
            //인쇄영역 설정
            wk.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
            wk.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
            wk.PageSetup.PrintArea = "B1:N67";
            wk.PageSetup.FitToPagesTall = false;
            /*wk.PageSetup.CenterFooter = "Page &P page, &N page";*/
            wk.PageSetup.FooterMargin = 0;
            wk.PageSetup.PrintGridlines = false;
            wk.PageSetup.TopMargin = 0;
            wk.PageSetup.BottomMargin = 0;
            wk.PageSetup.LeftMargin = 0;
            wk.PageSetup.RightMargin = 0;
            wk.PageSetup.CenterHorizontally = true;
            wk.PageSetup.CenterVertically = true;
            wk.PageSetup.FitToPagesWide = 1;
            wk.Application.ActiveWindow.View = Excel.XlWindowView.xlPageBreakPreview;


            /*wk.VPageBreaks[1].DragOff(Excel.XlDirection.xlToRight, 1);
            wk.HPageBreaks[1].DragOff(Excel.XlDirection.xlDown, 1);*/
            try
            {
                wk.VPageBreaks[1].DragOff(Excel.XlDirection.xlToRight, 1);
            }
            catch (Exception ex)
            { }
            try
            {
                wk.HPageBreaks[1].DragOff(Excel.XlDirection.xlDown, 1);
            }
            catch (Exception ex)
            { }
            wk.Application.ActiveWindow.View = Excel.XlWindowView.xlNormalView;
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

        // Zoom Event Handler
        public static void SetZoom(int zoomLevel,
                             Excel.Workbook wb,
                             Excel.Application excelInstance)
        {
            foreach (Excel.Worksheet ws in wb.Worksheets)
            {
                ws.Activate();
                excelInstance.ActiveWindow.Zoom = zoomLevel;
            }
        }

        public static Excel.Workbook Open(Excel.Application excelInstance,
                                   string fileName, bool readOnly = false,
                                   bool editable = true, bool updateLinks = true)
        {
            Excel.Workbook book = excelInstance.Workbooks.Open(
                fileName, updateLinks, readOnly);
            return book;
        }
        #endregion

        #region 품목서 저장/수정/삭제
        private void cbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSort.PerformClick();
            SettingForm();
        }

        private void btnSettingPassword_Click(object sender, EventArgs e)
        {
            int id;
            if (!int.TryParse(lbId.Text, out id))
            {
                MessageBox.Show(this,"품목서를 불러와주세요.");
                this.Activate();
                return;
            }
            List<FormDataModel> model = seaoverRepository.GetFormData("", id.ToString());
            if (model.Count == 0)
            {
                MessageBox.Show(this,"품목서를 정보를 찾을수 없습니다.");
                this.Activate();
                return;
            }
            FormPasswordManager fpm = new FormPasswordManager(model[0].password);
            //이미 잠금되있음
            if (model[0].is_rock)
            {
                if (fpm.CheckPassword())
                {
                    if (seaoverRepository.UpdateFormDataRock(id.ToString(), false, "") == -1)
                    {
                        MessageBox.Show(this,"설정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        btnSettingPassword.Text = "품목서 잠금";
                        btnSettingPassword.ForeColor = Color.Blue;
                    }
                }
                else
                {
                    MessageBox.Show(this,"비밀번호가 틀렷습니다.");
                    this.Activate();
                }
            }
            //잠금되있지 않음
            else
            {
                string password = fpm.SettingPassword();
                if (!string.IsNullOrEmpty(password))
                {
                    if (seaoverRepository.UpdateFormDataRock(id.ToString(), true, password) == -1)
                    {
                        MessageBox.Show(this,"설정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        btnSettingPassword.Text = "품목서 잠금해제";
                        btnSettingPassword.ForeColor = Color.Red;
                    }
                }
            }
        }
        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 1)
            {
                txtCategory.Text = "";
                txtProduct.Text = "";
                txtOrigin.Text = "";
                txtWeight.Text = "";
                txtSizes.Text = "";
                SelectList();
                SetBasicSetting();
                if(DataInspection())
                    SettingForm();
            }
        }
        //단가정보 삭제하기
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            UsersModel cum = new UsersModel();
            cum = usersRepository.GetUserInfo2(lbCreateUser.Text);
            if (cum != null)
            {
                if (um.user_id == cum.user_id || um.user_name == "관리자")
                {
                    int id = Convert.ToInt16(lbId.Text);
                    if (!Int32.TryParse(lbId.Text, out id))
                    {
                        MessageBox.Show(this, "품목서 정보를 찾을 수 없습니다.");
                        this.Activate();
                        return;
                    }
                    //품목서 유효성검사
                    List<FormDataModel> model = seaoverRepository.GetFormData("", id.ToString());
                    if (model.Count == 0)
                    {
                        MessageBox.Show(this, "품목서 정보를 찾을 수 없습니다.");
                        this.Activate();
                        return;
                    }

                    try
                    {
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        StringBuilder sql = seaoverRepository.DeleteFormData(id.ToString());
                        sqlList.Add(sql);

                        sql = bookmarkRepository.SupperDeleteSql(id);
                        sqlList.Add(sql);

                        if (MessageBox.Show(this, "취급품목서 내용을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int results = seaoverRepository.UpdateTrans(sqlList);
                            if (results == -1)
                            {
                                MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            else
                            {
                                PageCountSetting();
                                GetTitleList();
                                GetBookmark();
                                Refresh();
                                
                                GetFormDataTemp(Convert.ToInt32(id));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "수정할 내역을 불러와주세요.");
                        this.Activate();
                    }
                }
                else
                {
                    MessageBox.Show(this, "작성자만 품목서를 삭제할 수 있습니다.");
                    this.Activate();
                }
            }
            else
            {
                MessageBox.Show(this, "유저정보를 찾을 수 없습니다.");
                this.Activate();
            }
        }
        //문자발소용 텍스트 만들기
        private void btnMakeText_Click(object sender, EventArgs e)
        {
            PriceUpdate();
            dgvProduct.SelectAll();
            dgvProduct.EndEdit();
            Clipboard.SetText(GetHandlingProductTxt());
        }
        //단가정보 저장하기
        private void bntUpdate_Click(object sender, EventArgs e)
        {
            UpdateForm();
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        private void UpdateForm()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            UsersModel cum = new UsersModel();
            cum = usersRepository.GetUserInfo2(lbCreateUser.Text);
            if (cum != null)
            {
                int id = Convert.ToInt16(lbId.Text);
                if (!Int32.TryParse(lbId.Text, out id))
                {
                    MessageBox.Show(this, "품목서 정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }
                //품목서 유효성검사
                List<FormDataModel> model = seaoverRepository.GetFormData("", id.ToString());
                if (model.Count == 0)
                {
                    MessageBox.Show(this, "품목서 정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }

                //잠금이 되있을 경우
                if (model[0].is_rock)
                {
                    FormPasswordManager fpm = new FormPasswordManager(model[0].password);
                    if (!fpm.CheckPassword())
                    {
                        MessageBox.Show(this, "비밀번호가 틀렸습니다.");
                        this.Activate();
                        return;
                    }
                }
                //수정
                try
                {
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    StringBuilder sql = new StringBuilder();

                    //tempid++
                    sql = seaoverRepository.UpdateFormDataTemp(0, id);
                    sqlList.Add(sql);
                    //Delete tempid > 5
                    sql = seaoverRepository.DeleteFormDataTemp(id.ToString());
                    sqlList.Add(sql);

                    if (commonRepository.UpdateTran(sqlList) == -1)
                        MessageBox.Show("수정중 에러가 발생하였습니다.");
                    else
                    {
                        sqlList = new List<StringBuilder>();
                        InsertProductPrice(id, sqlList, "수정");
                        GetTitleList();
                        GetBookmark();
                        GetFormDataTemp(Convert.ToInt32(id));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "수정할 내역을 불러와주세요.");
                    this.Activate();
                }

            }
            else
            {
                MessageBox.Show(this, "유저정보를 찾을 수 없습니다.");
                this.Activate();
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertForm();
        }
        private void btnRowRefresh_Click(object sender, EventArgs e)
        {
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    row.Cells["rows"].Value = 1;
                }
            }
            SettingForm();
        }
        private void InsertForm()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            int id = seaoverRepository.getNextId();
            List<StringBuilder> sqlList = new List<StringBuilder>();
            InsertProductPrice(id, sqlList, "신규등록");
            GetTitleList();
            GetBookmark();
        }
        private void InsertProductPrice(int id, List<StringBuilder> sqlList, string mode)
        {
            if (string.IsNullOrEmpty(txtFormName.Text))
            {
                MessageBox.Show(this, "품목서 제목을 입력해주세요.");
                this.Activate();
                return;
            }
            StringBuilder sql;
            if (mode != "갱신")
            {
                if (MessageBox.Show(this, "취급품목서 내용을 " + mode + "하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
            }

            lbId.Text = id.ToString();
            //lbEditUser.Text = um.user_name.ToString();
            lbEditUser.Text = lbEditUser.Text;
            lbUpdatetime.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //lbUpdatedate.Text = "작성일자 : " + DateTime.Now.ToString("yyyy-MM-dd");
            //lbRemark.Text = "/ *단가 조정 필요시 문의바람* (작성일자:" + DateTime.Now.ToString("yyyy-MM-dd") + ")";
            lbEditDate.Text = "작성일자 : " + DateTime.Now.ToString("yyyy-MM-dd");

            bool isRock = false;
            string password = "";
            if (mode == "수정")
            {
                List<FormDataModel> formData = seaoverRepository.GetFormData("", id.ToString());
                if (formData == null || formData.Count == 0)
                {
                    MessageBox.Show(this,"등록된 정보를 찾을수 없습니다.");
                    this.Activate();
                    return;
                }
                else
                {
                    isRock = formData[0].is_rock;
                    password = formData[0].password;

                    //기존 데이터 삭제
                    sql = seaoverRepository.DeleteFormData(id.ToString());
                    sqlList.Add(sql);
                }
            }

            double s;
            if (dgvProduct.Rows.Count > 0)
            {
                PageCountSetting();
                int cnt = 1;

                for (int i = 0; i < dgvProduct.Rows.Count - 1; i++)
                {
                    foreach (DataGridViewCell c in dgvProduct.Rows[i].Cells)
                    {
                        if (c.Value == null)
                            c.Value = "";
                    }

                    FormDataModel model = new FormDataModel();
                    model.id = id;
                    model.sid = cnt;
                    model.form_type = 2;
                    model.form_name = txtFormName.Text;
                    if (string.IsNullOrEmpty(dgvProduct.Rows[i].Cells["accent"].Value.ToString()))
                        model.accent = false;
                    else
                        model.accent = Convert.ToBoolean(dgvProduct.Rows[i].Cells["accent"].Value);
                    model.category = dgvProduct.Rows[i].Cells["category"].Value.ToString();
                    model.category_code = dgvProduct.Rows[i].Cells["category_code"].Value.ToString();
                    model.product_code = dgvProduct.Rows[i].Cells["product_code"].Value.ToString();
                    model.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                    model.origin_code = dgvProduct.Rows[i].Cells["origin_code"].Value.ToString();
                    model.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                    model.weight = dgvProduct.Rows[i].Cells["weight"].Value.ToString();
                    model.sizes_code = dgvProduct.Rows[i].Cells["sizes_code"].Value.ToString();
                    model.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                    model.purchase_price = Convert.ToDouble(dgvProduct.Rows[i].Cells["purchase_price"].Value);
                    //model.sales_price = Convert.ToDouble(dgvProduct.Rows[i].Cells["sales_price"].Value);
                    if (double.TryParse(dgvProduct.Rows[i].Cells["sales_price"].Value.ToString(), out s))
                        model.sales_price = Convert.ToDouble(dgvProduct.Rows[i].Cells["sales_price"].Value);
                    else
                    {
                        if (dgvProduct.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                            model.sales_price = -1;
                        else
                            model.sales_price = -2;
                    }
                    model.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                    model.price_unit = dgvProduct.Rows[i].Cells["price_unit"].Value.ToString();
                    model.unit_count = dgvProduct.Rows[i].Cells["unit_count"].Value.ToString();
                    model.seaover_unit = dgvProduct.Rows[i].Cells["seaover_unit"].Value.ToString();

                    model.division = dgvProduct.Rows[i].Cells["division"].Value.ToString();
                    model.page = Convert.ToInt16(dgvProduct.Rows[i].Cells["page"].Value);
                    model.cnt = Convert.ToInt16(dgvProduct.Rows[i].Cells["cnt"].Value);
                    model.row_index = Convert.ToInt16(dgvProduct.Rows[i].Cells["rows"].Value);
                    model.area = dgvProduct.Rows[i].Cells["area"].Value.ToString();

                    if (mode == "수정")
                        model.create_user = lbCreateUser.Text;
                    else if (mode == "신규등록")
                        model.create_user = um.user_name;
                    else
                        model.create_user = lbCreateUser.Text;

                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                    //메모변경 
                    string managerTxt;
                    if (um.grade == null)
                        managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
                    else if (string.IsNullOrEmpty(um.grade))
                        managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
                    else
                        managerTxt = "담당자 : " + um.user_name.ToString() + " " + um.grade.ToString() + " (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";

                    managerTxt += " / " + um.form_remark;
                    if (lbRemark.Text != managerTxt)
                        model.form_remark = lbRemark.Text;

                    //품목서 잠금
                    model.is_rock = isRock;
                    model.password = password;

                    

                    //Form data
                    sql = seaoverRepository.InsertFormData(model);
                    sqlList.Add(sql);
                    //Temp data
                    sql = seaoverRepository.InsertFormDataTemp(model);
                    sqlList.Add(sql);

                    cnt += 1;
                }

                int results = seaoverRepository.UpdateTrans(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    MessageBox.Show(this, "완료");
                    this.Activate();
                }
            }
        }
        #endregion

        #region 페이지별 품목수 설정
        private void pageAutoSetting()
        {
            DataGridView dgv = dgvProduct;
            //int total_page = TotalPage;
            int total_page;
            if (!int.TryParse(txtTotalPage.Text, out total_page))
            {
                MessageBox.Show(this, "전체페이지를 입력해주세요.");
                this.Activate();
                return;
            }
            if (dgv.Rows.Count > 1)
            {
                TotalPage = total_page;
                //현재까지 채워진 카운터
                int CurCount = 0;
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {

                    int rows;
                    if (dgv.Rows[i].Cells["rows"].Value == null || !int.TryParse(dgv.Rows[i].Cells["rows"].Value.ToString(), out rows))
                        rows = 1;

                    CurCount += rows;
                }

                bool isOneLine = true;
                int totalCount = 0;
                //총 필요한 카운터 수
                if (rbOneline.Checked)
                    totalCount = TotalPage * 60;
                else if (rbTwoline.Checked)
                    totalCount = TotalPage * 120;

                int diffCnt = totalCount - CurCount;//필요한 카운트
                //첫번째 수정(1줄 짜리 품목부터 행수 늘리기)
                if (diffCnt > 0)
                {
                    int categoryCnt = 0;
                    int productCnt = 0;
                    int originCnt = 0;
                    int wegithCnt = 0;
                    int sizeCnt = 0;
                    int minRow = 100;

                    //최소 행수 찾기
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        int currentRow;
                        if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                            currentRow = 0;
                        if (currentRow < minRow)
                            minRow = currentRow;
                    }
                    //다음내역과 달라 혼자만 출력되는 내역들 부터 Row + 1
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        //끝행이 아닐때
                        if (i < dgv.Rows.Count - 2)
                        {
                            //대분류
                            if (dgv.Rows[i].Cells["category"].Value.ToString() == dgv.Rows[i + 1].Cells["category"].Value.ToString())
                            {
                                categoryCnt += 1;
                            }
                            else if (dgv.Rows[i].Cells["category"].Value.ToString() != dgv.Rows[i + 1].Cells["category"].Value.ToString())
                            {
                                if (categoryCnt == 0 && dgv.Rows[i].Cells["rows"].Value.ToString() == minRow.ToString())
                                {
                                    dgv.Rows[i].Cells["rows"].Value = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value) + 1;
                                    diffCnt -= 1;
                                    if (diffCnt == 0)
                                        break;
                                    else
                                        continue;
                                }
                                categoryCnt = 0;
                            }
                            //품목
                            if (dgv.Rows[i].Cells["product"].Value.ToString() == dgv.Rows[i + 1].Cells["product"].Value.ToString())
                            {
                                productCnt += 1;
                            }
                            else if (dgv.Rows[i].Cells["product"].Value.ToString() != dgv.Rows[i + 1].Cells["product"].Value.ToString())
                            {
                                if (productCnt == 0 && dgv.Rows[i].Cells["rows"].Value.ToString() == minRow.ToString())
                                {
                                    dgv.Rows[i].Cells["rows"].Value = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value) + 1;
                                    diffCnt -= 1;
                                    if (diffCnt == 0)
                                        break;
                                    else
                                        continue;
                                }
                                productCnt = 0;
                            }
                            //원산지
                            if (dgv.Rows[i].Cells["origin"].Value.ToString() == dgv.Rows[i + 1].Cells["origin"].Value.ToString())
                            {
                                originCnt += 1;
                            }
                            else if (dgv.Rows[i].Cells["weight"].Value.ToString() != dgv.Rows[i + 1].Cells["weight"].Value.ToString())
                            {
                                if (originCnt == 0 && dgv.Rows[i].Cells["rows"].Value.ToString() == minRow.ToString())
                                {
                                    dgv.Rows[i].Cells["rows"].Value = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value) + 1;
                                    diffCnt -= 1;
                                    if (diffCnt == 0)
                                        break;
                                    else
                                        continue;
                                }
                                originCnt = 0;
                            }
                            //중량
                            if (dgv.Rows[i].Cells["weight"].Value.ToString() == dgv.Rows[i + 1].Cells["weight"].Value.ToString())
                            {
                                wegithCnt += 1;
                            }
                            else if (dgv.Rows[i].Cells["weight"].Value.ToString() != dgv.Rows[i + 1].Cells["weight"].Value.ToString())
                            {
                                if (wegithCnt == 0 && dgv.Rows[i].Cells["rows"].Value.ToString() == minRow.ToString())
                                {
                                    int currentRow;
                                    if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                                        currentRow = 0;

                                    dgv.Rows[i].Cells["rows"].Value = currentRow + 1;
                                    diffCnt -= 1;
                                    if (diffCnt == 0)
                                        break;
                                    else
                                        continue;
                                }
                                wegithCnt = 0;
                            }
                            //사이즈
                            if (dgv.Rows[i].Cells["sizes"].Value.ToString() == dgv.Rows[i + 1].Cells["sizes"].Value.ToString())
                            {
                                sizeCnt += 1;
                            }
                            else if (dgv.Rows[i].Cells["sizes"].Value.ToString() != dgv.Rows[i + 1].Cells["sizes"].Value.ToString())
                            {
                                if (sizeCnt == 0 && dgv.Rows[i].Cells["rows"].Value.ToString() == minRow.ToString()
                                    && dgv.Rows[i].Cells["sizes"].Value.ToString().Length > 10)
                                {
                                    int currentRow;
                                    if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                                        currentRow = 0;

                                    dgv.Rows[i].Cells["rows"].Value = currentRow + 1;
                                    diffCnt -= 1;
                                    if (diffCnt == 0)
                                        break;
                                    else
                                        continue;
                                }
                                sizeCnt = 0;
                            }
                        }
                        //끝행일때
                        else if (i == dgv.Rows.Count - 2)
                        {
                            int currentRow;
                            if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                                currentRow = 0;

                            if (dgv.Rows[i - 1].Cells["category"].Value.ToString() != dgv.Rows[i].Cells["category"].Value.ToString())
                            {
                                dgv.Rows[i].Cells["rows"].Value = currentRow + 1;
                                diffCnt -= 1;
                            }
                            else if (dgv.Rows[i - 1].Cells["product"].Value.ToString() != dgv.Rows[i].Cells["product"].Value.ToString())
                            {
                                dgv.Rows[i].Cells["rows"].Value = currentRow + 1;
                                diffCnt -= 1;
                            }
                            else if (dgv.Rows[i - 1].Cells["origin"].Value.ToString() != dgv.Rows[i].Cells["origin"].Value.ToString())
                            {
                                dgv.Rows[i].Cells["rows"].Value = currentRow + 1;
                                diffCnt -= 1;
                            }
                            else if (dgv.Rows[i - 1].Cells["weight"].Value.ToString() != dgv.Rows[i].Cells["weight"].Value.ToString())
                            {
                                dgv.Rows[i].Cells["rows"].Value = currentRow + 1;
                                diffCnt -= 1;
                            }

                            if (diffCnt == 0)
                                break;
                        }
                    }
                }
            retry1:
                //글자수 많은 내역부터 Row + 1 
                if (diffCnt > 0)
                {
                    int minRow = 100;

                    //최소 행수 찾기
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        int currentRow;
                        if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                            currentRow = 0;

                        if (currentRow < minRow)
                            minRow = currentRow;
                    }
                    //글자수 MAX
                    int maxLen = 0;
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        if (dgv.Rows[i].Cells["rows"].Value.ToString() == minRow.ToString()
                        && (dgv.Rows[i].Cells["weight"].Value.ToString().Length + dgv.Rows[i].Cells["sizes"].Value.ToString().Length) > maxLen)
                        {
                            maxLen = (dgv.Rows[i].Cells["weight"].Value.ToString().Length + dgv.Rows[i].Cells["sizes"].Value.ToString().Length);
                        }
                    }
                    //차감
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        if (dgv.Rows[i].Cells["rows"].Value.ToString() == minRow.ToString()
                        && (dgv.Rows[i].Cells["weight"].Value.ToString().Length + dgv.Rows[i].Cells["sizes"].Value.ToString().Length) == maxLen)
                        {
                            dgv.Rows[i].Cells["rows"].Value = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value) + 1;
                            diffCnt -= 1;
                            if (diffCnt == 0)
                            {
                                break;
                            }
                        }
                    }
                }
                if (diffCnt > 0)
                    goto retry1;

                //PAGE
                if (TotalPage == 0)
                    TotalPage = 1;

                //오버된 행수 재조정
                int tCnt = 0;
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    int currentRow;
                    if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                        currentRow = 0;
                    tCnt += currentRow;
                }
                if (tCnt > totalCount)
                {
                    diffCnt = tCnt - totalCount;

                    //페이지별 추가행수 할당
                    int pCnt = (int)(diffCnt / TotalPage);
                    int tempCnt = (int)(diffCnt / TotalPage);
                    int rCnt = (int)(diffCnt % TotalPage);

                    //부족한 행수 채우기
                    for (int j = 1; j <= TotalPage; j++)
                    {
                        if (j == TotalPage)
                            pCnt = tempCnt + rCnt;
                        else
                            pCnt = tempCnt;
                        //최신화
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            dgv.Rows[i].Cells["page"].Value = "";
                        }
                        dgv.EndEdit();
                        for (int i = 1; i <= j; i++)
                        {
                            if (rbTwoline.Checked)
                            {
                                SetPagecnt(i, "L");
                                SetPagecnt(i, "R");
                            }
                            else
                            {
                                SetPagecnt(i, "C");
                            }
                        }
                        if (total_page != 0)
                        {
                            TotalPage = total_page;
                        }

                        dgv.EndEdit();


                        //행수 추가
                        if (pCnt > 0)
                        {
                            diffCnt -= pCnt;  //남은 행수에 차감
                        retry5:
                            //최소행수
                            int currentRow;
                            if (!int.TryParse(dgvProduct.Rows[0].Cells["rows"].Value.ToString(), out currentRow))
                                currentRow = 0;
                            int maxRow = currentRow;
                            for (int i = dgv.Rows.Count - 2; i >= 1; i--)
                            {
                                if (dgv.Rows[i].Cells["page"].Value.ToString() == j.ToString())
                                {
                                    if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                                        currentRow = 0;

                                    if (maxRow < currentRow)
                                        maxRow = currentRow;
                                }
                            }
                            //최소행수에 행수추가
                            if (dgv.Rows.Count >= 60)
                            {
                                for (int i = dgv.Rows.Count - 2; i >= 1; i--)
                                {
                                    if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                                        currentRow = 0;

                                    if (dgv.Rows[i].Cells["page"].Value.ToString() == j.ToString()
                                        && currentRow == maxRow)
                                    {
                                        dgv.Rows[i].Cells["rows"].Value = currentRow - 1;
                                        pCnt -= 1;
                                        if (pCnt == 0)
                                            break;
                                    }
                                }
                                //남아있다면 다시 돌아가서 차감
                                if (pCnt > 0)
                                    goto retry5;
                            }
                            //차감이 완료되면 반복문 탈출
                            if (diffCnt == 0)
                                break;
                        }
                    }
                }

                //넘치는 품목 재수정
                int tmpCnt = 0;
                int errCnt = 0;
            retry:
                int pageCnt = 0;
                int overRow = 0;
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    int currentRow;
                    if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                        currentRow = 0;

                    pageCnt += currentRow;
                    if (pageCnt == 60)
                        pageCnt = 0;
                    else if (pageCnt > 60)
                    {
                        overRow += 1;
                        if (i - 1 - tmpCnt > 0)
                        {
                            if (!int.TryParse(dgvProduct.Rows[i - 1 - tmpCnt].Cells["rows"].Value.ToString(), out currentRow))
                                currentRow = 0;

                            dgv.Rows[i - 1 - tmpCnt].Cells["rows"].Value = currentRow + 1;
                        }
                        else
                        {
                            if (!int.TryParse(dgvProduct.Rows[i].Cells["rows"].Value.ToString(), out currentRow))
                                currentRow = 0;

                            dgv.Rows[i].Cells["rows"].Value = currentRow + 1;
                        }
                        pageCnt = 0;
                        //위 수정한 내역보다 뒤에 있는 행에서 수정
                        for (int j = i; j < dgv.Rows.Count - 1; j++)
                        {
                            if (!int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                currentRow = 0;

                            if (currentRow > 1)
                            {
                                dgv.Rows[j].Cells["rows"].Value = currentRow - 1;
                                break;
                            }
                        }
                    }
                }
                if (overRow > 0)
                {
                    tmpCnt += 1;
                    errCnt += 1;
                    if (errCnt <= 10)
                        goto retry;
                }

                //한줄짜리 전 페이지로 올리기


                



                //최신화
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    dgv.Rows[i].Cells["page"].Value = "";
                }
                dgv.EndEdit();
                for (int i = 1; i <= total_page; i++)
                {
                    if (rbTwoline.Checked)
                    {
                        SetPagecnt(i, "L");
                        SetPagecnt(i, "R");
                    }
                    else
                    {
                        SetPagecnt(i, "C");
                    }
                }
                TotalPage = total_page;
                dgv.EndEdit();
                int cPage = 1;


                
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    //2페이지 이상 LEFT 첫줄 확인
                    if (cPage <= total_page)
                    {
                        //1줄품목서=======================================================================
                        if (rbTwoline.Checked)
                        {
                            //페이지와 영역이 처음으로 확인될 때
                            if (dgv.Rows[i].Cells["page"].Value.ToString() == total_page.ToString()
                                && dgv.Rows[i].Cells["area"].Value.ToString() == "R"
                                && i > 0)
                            {
                                //앞뒤로 category, product가 일치하지 않을때
                                if ((dgv.Rows[i + 1].Cells["category"].Value == null && dgv.Rows[i + 1].Cells["product"].Value == null)
                                    || (dgv.Rows[i].Cells["category"].Value.ToString() != dgv.Rows[i + 1].Cells["category"].Value.ToString())
                                    || (dgv.Rows[i].Cells["product"].Value.ToString() != dgv.Rows[i + 1].Cells["product"].Value.ToString()))
                                {

                                    //최대행수 찾기
                                    int maxRow = 0;
                                    for (int j = dgv.Rows.Count - 2; j >= 0; j--)
                                    {
                                        int currentRow;
                                        if (!int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                            currentRow = 0;


                                        if ((total_page - 1) > 0
                                            && dgv.Rows[j].Cells["page"].Value.ToString() == total_page.ToString()
                                            && dgv.Rows[j].Cells["area"].Value.ToString() == "L"
                                            && j > 0)
                                        {
                                            if (maxRow < currentRow)
                                                maxRow = currentRow;

                                        }
                                    }
                                    //최대행수가 3이상 일때만 적용
                                    if (maxRow > 2)
                                    {
                                        dgv.Rows[i + 1].Cells["rows"].Value = Convert.ToInt16(dgv.Rows[i + 1].Cells["rows"].Value) + 1;

                                        for (int j = dgv.Rows.Count - 2; j >= 0; j--)
                                        {
                                            if (dgv.Rows[j].Cells["page"].Value.ToString() == total_page.ToString()
                                                && dgv.Rows[j].Cells["area"].Value.ToString() == "L"
                                                && dgv.Rows[j].Cells["rows"].Value.ToString() == maxRow.ToString()
                                                && j > 0)
                                            {

                                                int currentRow;
                                                if (!int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                                    currentRow = 0;

                                                dgv.Rows[j].Cells["rows"].Value = currentRow - 1;
                                                break;
                                            }
                                        }
                                    }
                                }

                                cPage += 1;

                                //페이지와 영역이 처음으로 확인될 때
                                if (dgv.Rows[i].Cells["page"].Value.ToString() == total_page.ToString()
                                    && dgv.Rows[i].Cells["area"].Value.ToString() == "L"
                                    && i > 0)
                                {
                                    //앞뒤로 category, product가 일치하지 않을때
                                    if ((dgv.Rows[i + 1].Cells["category"].Value == null && dgv.Rows[i + 1].Cells["product"].Value == null)
                                        || (dgv.Rows[i].Cells["category"].Value.ToString() != dgv.Rows[i + 1].Cells["category"].Value.ToString())
                                        || (dgv.Rows[i].Cells["product"].Value.ToString() != dgv.Rows[i + 1].Cells["product"].Value.ToString()))
                                    {

                                        //최대행수 찾기
                                        int maxRow = 0;
                                        for (int j = dgv.Rows.Count - 2; j >= 0; j--)
                                        {
                                            if ((total_page - 1) > 0
                                                && dgv.Rows[j].Cells["page"].Value.ToString() == (total_page - 1).ToString()
                                                && dgv.Rows[j].Cells["area"].Value.ToString() == "R"
                                                && j > 0)
                                            {
                                                int currentRow;
                                                if (!int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                                    currentRow = 0;

                                                if (maxRow < currentRow)
                                                    maxRow = currentRow;
                                            }
                                        }
                                        //최대행수가 3이상 일때만 적용
                                        if (maxRow > 2)
                                        {
                                            dgv.Rows[i + 1].Cells["rows"].Value = Convert.ToInt16(dgv.Rows[i + 1].Cells["rows"].Value) + 1;

                                            for (int j = dgv.Rows.Count - 2; j >= 0; j--)
                                            {
                                                if (dgv.Rows[j].Cells["page"].Value.ToString() == (total_page - 1).ToString()
                                                    && dgv.Rows[j].Cells["area"].Value.ToString() == "R"
                                                    && dgv.Rows[j].Cells["rows"].Value.ToString() == maxRow.ToString()
                                                    && j > 0)
                                                {
                                                    int currentRow;
                                                    if (!int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                                        currentRow = 0;

                                                    dgv.Rows[j].Cells["rows"].Value = currentRow - 1;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        //2줄품목서=======================================================================
                        else
                        {
                            //앞뒤로 category, product가 일치하지 않을때
                            if ((dgv.Rows[i + 1].Cells["category"].Value == null && dgv.Rows[i + 1].Cells["product"].Value == null)
                                        || (dgv.Rows[i].Cells["category"].Value.ToString() != dgv.Rows[i + 1].Cells["category"].Value.ToString())
                                        || (dgv.Rows[i].Cells["product"].Value.ToString() != dgv.Rows[i + 1].Cells["product"].Value.ToString()))
                            {

                                //최대행수 찾기
                                int maxRow = 0;
                                for (int j = dgv.Rows.Count - 2; j >= 0; j--)
                                {
                                    if ((total_page - 1) > 0
                                        && dgv.Rows[j].Cells["page"].Value.ToString() == total_page.ToString()
                                        && j > 0)
                                    {
                                        int currentRow;
                                        if (!int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                            currentRow = 0;

                                        if (maxRow < currentRow)
                                            maxRow = currentRow;
                                    }
                                }
                                //최대행수가 3이상 일때만 적용
                                if (maxRow > 2)
                                {
                                    dgv.Rows[i + 1].Cells["rows"].Value = Convert.ToInt16(dgv.Rows[i + 1].Cells["rows"].Value) + 1;

                                    for (int j = dgv.Rows.Count - 2; j >= 0; j--)
                                    {
                                        if (dgv.Rows[j].Cells["page"].Value.ToString() == total_page.ToString()
                                            && dgv.Rows[j].Cells["rows"].Value.ToString() == maxRow.ToString()
                                            && j > 0)
                                        {
                                            int currentRow;
                                            if (!int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                                currentRow = 0;

                                            dgv.Rows[j].Cells["rows"].Value = currentRow - 1;
                                            break;
                                        }
                                    }
                                }
                            }

                            cPage += 1;
                        }
                    }
                }



                //영역별로 돌면서 행수 최종수정
                int pageArea = TotalPage * 2;
                int tempPageArea = 1;
                while (tempPageArea <= pageArea)
                {
                    int currentPage;
                    string currentArea;
                    //LEFT
                    if (tempPageArea % 2 == 1)
                    {
                        currentPage = tempPageArea / 2 + 1;
                        currentArea = "L";
                    }
                    //RIGHT
                    else
                    {
                        currentPage = tempPageArea / 2;
                        currentArea = "R";
                    }

                    //페이지, 영역별 총행수
                    int pageAreaTotalRows = 0;
                    int minRow = 9999, maxRow = 0;
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        DataGridViewRow row = dgv.Rows[i];
                        int currentRow;
                        if (row.Cells["rows"].Value == null || !int.TryParse(row.Cells["rows"].Value.ToString(), out currentRow))
                            currentRow = 0;

                        if (row.Cells["page"].Value.ToString() == currentPage.ToString() && row.Cells["area"].Value.ToString() == currentArea.ToString())
                        {
                            pageAreaTotalRows += currentRow;
                            if (i < minRow)
                                minRow = i;
                            if (i > maxRow)
                                maxRow = i;
                        }
                    }

                    //영역별 행수조절
                    int maxTotalRows;
                    if (rbOneline.Checked)
                        maxTotalRows = 120;
                    else
                        maxTotalRows = 60;

                    if (pageAreaTotalRows != maxTotalRows)
                    {
                        diffCnt = maxTotalRows - pageAreaTotalRows;

                    reAdjust:
                        //영역안에서만 가장큰 행수 찾기
                        int maxRows = 0, minRows = 9999;
                        for (int j = minRow; j <= maxRow; j++)
                        {
                            int currentRow;
                            if (dgvProduct.Rows[j].Cells["rows"].Value == null || !int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                currentRow = 0;

                            if (maxRows < currentRow)
                                maxRows = currentRow;
                            if (minRows > currentRow)
                                minRows = currentRow;
                        }

                        //큰수부터 적용
                        for (int j = minRow; j <= maxRow; j++)
                        {
                            if (diffCnt == 0)
                                break;

                            int currentRow;
                            if (dgvProduct.Rows[j].Cells["rows"].Value == null || !int.TryParse(dgvProduct.Rows[j].Cells["rows"].Value.ToString(), out currentRow))
                                currentRow = 0;

                            //남는다
                            if (diffCnt > 0 && currentRow == minRows)
                            {
                                dgvProduct.Rows[j].Cells["rows"].Value = currentRow + 1;
                                diffCnt--;
                                goto reAdjust;
                            }
                            //넘친다
                            else if (diffCnt < 0 && currentRow == maxRows)
                            {
                                dgvProduct.Rows[j].Cells["rows"].Value = currentRow - 1;
                                diffCnt++;
                                goto reAdjust;
                            }
                        }

                        
                    }
                    tempPageArea++;
                }

            }
        }


        //품목리스트 Page, Cnt, Area 초기화
        private void PageCountSetting()
        {
            int row = 0;
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1)
            {
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    dgv.Rows[i].Cells["page"].Value = "";
                    dgv.Rows[i].Cells["area"].Value = "";
                    if (string.IsNullOrEmpty(dgv.Rows[i].Cells["rows"].Value.ToString()))
                    {
                        dgv.Rows[i].Cells["rows"].Value = 1;
                    }
                }

                dgv.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;

                if (rbOneline.Checked)
                {
                    for (int i = 1; i < 60; i++)
                    {
                        SetPagecnt(i, "C");
                    }
                }
                else
                {
                    for (int i = 1; i < 30; i++)
                    {
                        SetPagecnt(i, "L");
                        SetPagecnt(i, "R");
                    }
                }

                lbTotal.Text = "/ " + TotalPage.ToString();
                txtTotalPage.Text = TotalPage.ToString();
            }
        }
        //품목리스트 Page, Cnt, Area 분배하기
        private void SetPagecnt(int page, string area)
        {
            DataGridView dgv = dgvProduct;

            //Page Count Setting
            //int row = GetPageCount(page, area);
            int row = 60;

            //Datagridview Page cells Value Setting
            if (dgv.Rows.Count > 1)
            {
                int cnt = 0;
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    if (row > 0)
                    {
                        if (dgv.Rows[i].Cells["page"].Value == null || dgv.Rows[i].Cells["page"].Value == "")
                        {
                            row -= Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value);
                            cnt += Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value);
                            TotalPage = page;

                            dgv.Rows[i].Cells["cnt"].Value = cnt;
                            dgv.Rows[i].Cells["page"].Value = page;
                            dgv.Rows[i].Cells["area"].Value = area;
                        }
                    }
                    else if (row == 0)
                    {
                        break;
                    }
                }
            }
        }
        #endregion

        #region 품목리스트 불러오기
        public void SetProduct(List<SeaoverPriceModel> model)
        {
            if (model.Count > 0)
            {
                for (int i = model.Count - 1; i >= 0; i--)
                {
                    SeaoverPriceModel m = model[i];

                    //Row 추가
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["accent"].Value = false;
                    dgvProduct.Rows[n].Cells["category"].Value = m.category;
                    dgvProduct.Rows[n].Cells["category_code"].Value = m.category_code;
                    if (m.category_code != null && !string.IsNullOrEmpty(m.category_code))
                    {
                        dgvProduct.Rows[n].Cells["category1"].Value = m.category_code.Substring(0, 1);

                        int d;
                        if (!int.TryParse(m.category_code.Substring(1, 1), out d))
                        {
                            dgvProduct.Rows[n].Cells["category2"].Value = m.category_code.ToString().Substring(1, 1);
                        }

                        if (m.category_code.Length <= 5)
                            dgvProduct.Rows[n].Cells["category3"].Value = m.category_code.Substring(m.category_code.Length - 3, 3);
                        else if (dgvProduct.Rows[n].Cells["category2"].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[n].Cells["category2"].Value.ToString()))
                            dgvProduct.Rows[n].Cells["category3"].Value = m.category_code.Substring(2, 3);
                        else
                            dgvProduct.Rows[n].Cells["category3"].Value = m.category_code.Substring(1, 3);
                    }
                    else
                    {
                        dgvProduct.Rows[n].Cells["category1"].Value = m.category_code;
                        dgvProduct.Rows[n].Cells["category2"].Value = m.category_code;
                        dgvProduct.Rows[n].Cells["category3"].Value = m.category_code;
                    }
                    dgvProduct.Rows[n].Cells["product_code"].Value = m.product_code;
                    dgvProduct.Rows[n].Cells["product"].Value = m.product.Replace("(", "\n(");
                    dgvProduct.Rows[n].Cells["origin_code"].Value = m.origin_code;
                    dgvProduct.Rows[n].Cells["origin"].Value = m.origin;

                    dgvProduct.Rows[n].Cells["sizes_code"].Value = m.sizes_code;
                    dgvProduct.Rows[n].Cells["sizes"].Value = m.sizes;

                    dgvProduct.Rows[n].Cells["unit"].Value = m.unit;
                    dgvProduct.Rows[n].Cells["price_unit"].Value = m.price_unit;
                    dgvProduct.Rows[n].Cells["unit_count"].Value = m.unit_count;
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = m.seaover_unit;
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = m.purchase_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["sales_price"].Value = m.sales_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["division"].Value = m.division;
                    dgvProduct.Rows[n].Cells["page"].Value = "";
                    dgvProduct.Rows[n].Cells["area"].Value = "";
                    dgvProduct.Rows[n].Cells["rows"].Value = 1;
                    dgvProduct.Rows[n].Cells["edit_date"].Value = m.edit_date;
                    dgvProduct.Rows[n].Cells["manager1"].Value = m.manager1;

                    //중량
                    if (m.price_unit.Contains("팩"))
                    {
                        if (Convert.ToDouble(m.unit) < 1)
                            dgvProduct.Rows[n].Cells["weight"].Value = m.seaover_unit + "kg \n (" + (Convert.ToDouble(m.unit) * 1000).ToString() + "g x " + m.unit_count + "p)";
                        else
                            dgvProduct.Rows[n].Cells["weight"].Value = m.seaover_unit + "kg \n (" + m.unit.ToString() + "k x " + m.unit_count + "p)";
                    }
                    //kg
                    else if (m.price_unit == "kg" || m.price_unit == "Kg" || m.price_unit == "KG")
                    {
                        if (Convert.ToDouble(m.unit) < 1)
                            dgvProduct.Rows[n].Cells["weight"].Value = m.seaover_unit + "kg \n (" + (Convert.ToDouble(m.unit) * 1000).ToString() + "g x " + m.unit_count + "p)";
                        else
                            dgvProduct.Rows[n].Cells["weight"].Value = m.seaover_unit + "kg \n (" + m.unit.ToString() + "k x " + m.unit_count + "p)";
                    }
                    //묶음
                    else if (m.price_unit == "묶" || m.price_unit == "묶음")
                        dgvProduct.Rows[n].Cells["weight"].Value = m.unit + "kg";
                    //나머지
                    else
                        dgvProduct.Rows[n].Cells["weight"].Value = m.seaover_unit + "kg";

                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[n].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");
                }
            }
        }

        #endregion

        #region 버튼, 셀버튼
        private void txtAutoSetting_Click(object sender, EventArgs e)
        {
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1)
            {
                int input_page;
                if (!int.TryParse(txtTotalPage.Text, out input_page))
                    input_page = 0;
                double min_total_page = Math.Ceiling((double)dgvProduct.Rows.Count / 120);
                if (input_page < min_total_page)
                {
                    MessageBox.Show(this, "최소 " + min_total_page.ToString() + "페이지 이상은 입력해주셔야 합니다!");
                    this.Activate();
                    return;
                }
                else
                {
                    int row_cnt = 0;
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        double rows;
                        if(dgv.Rows[i].Cells["rows"].Value == null || !double.TryParse(dgv.Rows[i].Cells["rows"].Value.ToString(), out rows))
                            row_cnt ++;
                        else
                            row_cnt += Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value.ToString());
                    }
                    min_total_page = Math.Ceiling((double)row_cnt / 120);
                    if (input_page < min_total_page)
                        btnRowRefresh.PerformClick();
                }

                pageAutoSetting();
                SettingForm();
            }
        }
        //백업하기
        private void cbArchive_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            try
            {
                int form_id = Convert.ToInt32(lbId.Text);
                int temp_id = (cbArchive.SelectedItem as dynamic).Value;
                GetTempData(form_id, temp_id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "에러가 발생하였습니다. 품목서를 다시 선택해주십시오.");
                this.Activate();
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private void btnLeftManger_Click(object sender, EventArgs e)
        {
            int p = Convert.ToInt16(txtCurPage.Text);
            if (rbOneline.Checked)
            {
                PageMananerOpen(p, "C");
            }
            else
            {
                PageMananerOpen(p, "L");
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            SortSetting();
        }
        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (lbId.Text == null || lbId.Text == "")
            {
                MessageBox.Show(this, "품목서를 선택해주세요.");
                this.Activate();
                return;
            }
            ExcelDownload();
        }
        private void btnExculed_Click(object sender, EventArgs e)
        {
            ExculedProduct();
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (lbId.Text == null || lbId.Text == "")
            {
                MessageBox.Show(this, "품목서를 선택해주세요.");
                this.Activate();
                return;
            }
            PreviewForms();
        }
        private void btnGetSeaover_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "GetProductList")
                {
                    isFormActive = true;
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                }
            }

            if (!isFormActive)
            {
                GetProductList pl = new GetProductList(um, this);
                pl.Show();
            }
        }
        private void SetBasicSetting()
        {
            if (MessageBox.Show(this, "명칭을 기본값으로 변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            DataGridView dgv = dgvProduct;
            DataTable dt = new DataTable();
            dt = formChangedDataRepository.GetChangeCurrentData();
            Dictionary<string, int> tempStr;
            string whrStr = "";
            if (dgv.Rows.Count > 1)
            {
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    DataGridViewRow r = dgv.Rows[i];
                    DataTable tempDt = dt;
                    DataRow[] arrRows = null;

                    if (r.Cells["category_code"].Value != null && !string.IsNullOrEmpty(r.Cells["category_code"].Value.ToString()))
                    { 
                        arrRows = tempDt.Select($"column_name='category' AND column_code = '{r.Cells["category_code"].Value.ToString()}'");
                        if (arrRows.Length > 0)
                            r.Cells["category"].Value = arrRows[0][4].ToString();
                    }
                    if (r.Cells["product_code"].Value != null)
                    {
                        tempDt = dt;
                        arrRows = null;
                        arrRows = tempDt.Select($"column_name='product' AND column_code = '{r.Cells["product_code"].Value.ToString()}'");
                        if (arrRows.Length > 0)
                            r.Cells["product"].Value = arrRows[0][4].ToString();
                    }
                    
                    if (r.Cells["sizes_code"].Value != null && r.Cells["sizes"].Value != null && r.Cells["product_code"].Value != null && r.Cells["origin_code"].Value != null)
                    {
                        whrStr = $"column_name='sizes' AND column_code = '{r.Cells["product_code"].Value.ToString()}^{r.Cells["origin_code"].Value.ToString()}^{r.Cells["sizes_code"].Value.ToString()}'";
                        tempDt = dt;
                        arrRows = null;
                        arrRows = tempDt.Select(whrStr);
                        if (arrRows.Length > 0)
                            r.Cells["sizes"].Value = arrRows[0][4].ToString();
                    }

                    if (r.Cells["sizes_code"].Value != null && r.Cells["weight"].Value != null && r.Cells["product_code"].Value != null && r.Cells["origin_code"].Value != null)
                    {
                        whrStr = $"column_name='weight' AND column_code = '"
                            + $"{r.Cells["product_code"].Value.ToString()}^{r.Cells["origin_code"].Value.ToString()}^{r.Cells["sizes_code"].Value.ToString()}"
                            + $"^{r.Cells["unit"].Value.ToString()}^{r.Cells["price_unit"].Value.ToString()}^{r.Cells["unit_count"].Value.ToString()}'";
                        tempDt = dt;
                        arrRows = null;
                        arrRows = tempDt.Select(whrStr);
                        if (arrRows != null && arrRows.Length > 0)
                            r.Cells["weight"].Value = arrRows[0][4].ToString();
                    }
                }
            }
        }
        private void ExcelDownload()
        {
            string formName = txtFormName.Text;
            formName = formName.Replace("/", "");
            int id = Convert.ToInt16(lbId.Text);
            PageCountSetting();
            PriceUpdate();
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = @"C:";
            saveFile.Title = "Excel 저장 위치 지정";
            saveFile.DefaultExt = "xlsx";
            saveFile.FileName = formName;
            saveFile.Filter = "Xls files(*.xls*)|*.xls*";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                savePath = saveFile.FileName.ToString();
                //MessageBox.Show(this,"Excel 파일을 생성하기 약간의 시간이 걸립니다. 우측하단의 상태 표시가 사라지면 자동으로 Excel파일이 열립니다.");
                saveFlag = true;
                bgwExcel.RunWorkerAsync();
            }
            pnGlass.Visible = false;
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.RowIndex >= 0 & e.ColumnIndex >= 0)
            {
                int row = dgvProduct.Rows[e.RowIndex].Index;
                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    int rows;
                    if (e.ColumnIndex == 11)
                    {
                        rows = Convert.ToInt16(dgvProduct.Rows[row].Cells["rows"].Value);
                        rows += 1;
                        dgvProduct.Rows[row].Cells["rows"].Value = rows;
                    }
                    else if (e.ColumnIndex == 12)
                    {
                        rows = Convert.ToInt16(dgvProduct.Rows[row].Cells["rows"].Value);
                        if (rows > 1)
                        {
                            rows -= 1;
                            dgvProduct.Rows[row].Cells["rows"].Value = rows;
                        }
                    }

                        /*//줄추가
                        if (e.ColumnIndex == 11)
                        {
                            dgvProduct.Rows.Insert(row, 1);
                            dgvProduct.Rows[row].Cells["category1"].Value = dgvProduct.Rows[row + 1].Cells["category1"].Value;
                            dgvProduct.Rows[row].Cells["category2"].Value = dgvProduct.Rows[row + 1].Cells["category2"].Value;
                            dgvProduct.Rows[row].Cells["category3"].Value = dgvProduct.Rows[row + 1].Cells["category3"].Value;
                            dgvProduct.Rows[row].Cells["category_code"].Value = dgvProduct.Rows[row + 1].Cells["category_code"].Value;
                            dgvProduct.Rows[row].Cells["category"].Value = dgvProduct.Rows[row + 1].Cells["category"].Value;
                            dgvProduct.Rows[row].Cells["product_code"].Value = dgvProduct.Rows[row + 1].Cells["product_code"].Value;
                            dgvProduct.Rows[row].Cells["product"].Value = dgvProduct.Rows[row + 1].Cells["product"].Value;
                            dgvProduct.Rows[row].Cells["origin_code"].Value = dgvProduct.Rows[row + 1].Cells["origin_code"].Value;
                            dgvProduct.Rows[row].Cells["origin"].Value = dgvProduct.Rows[row + 1].Cells["origin"].Value;
                            dgvProduct.Rows[row].Cells["weight"].Value = dgvProduct.Rows[row + 1].Cells["weight"].Value;
                            dgvProduct.Rows[row].Cells["sizes_code"].Value = dgvProduct.Rows[row + 1].Cells["sizes_code"].Value;
                            dgvProduct.Rows[row].Cells["sizes"].Value = dgvProduct.Rows[row + 1].Cells["sizes"].Value;
                            dgvProduct.Rows[row].Cells["purchase_price"].Value = dgvProduct.Rows[row + 1].Cells["purchase_price"].Value;
                            dgvProduct.Rows[row].Cells["sales_price"].Value = dgvProduct.Rows[row + 1].Cells["sales_price"].Value;
                            dgvProduct.Rows[row].Cells["unit"].Value = dgvProduct.Rows[row + 1].Cells["unit"].Value;
                            dgvProduct.Rows[row].Cells["unit_count"].Value = dgvProduct.Rows[row + 1].Cells["unit_count"].Value;
                            dgvProduct.Rows[row].Cells["price_unit"].Value = dgvProduct.Rows[row + 1].Cells["price_unit"].Value;
                            dgvProduct.Rows[row].Cells["seaover_unit"].Value = dgvProduct.Rows[row + 1].Cells["seaover_unit"].Value;
                            dgvProduct.Rows[row].Cells["division"].Value = dgvProduct.Rows[row + 1].Cells["division"].Value;
                            dgvProduct.Rows[row].Cells["rows"].Value = dgvProduct.Rows[row + 1].Cells["rows"].Value;
                            //영억
                            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[row].Cells["area"];
                            cb.Items.Add("L");
                            cb.Items.Add("R");
                        }
                        //줄삭제
                        else if (e.ColumnIndex == 12)
                        {
                            if (e.RowIndex < dgvProduct.Rows.Count - 1)
                            {
                                dgvProduct.Rows.Remove(dgvProduct.Rows[row]);
                            }
                        }*/
                    }
            }
        }
        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (TotalPage > 0)
            {
                int curPage = Convert.ToInt16(txtCurPage.Text);
                if (curPage > 1)
                {
                    txtCurPage.Text = (curPage - 1).ToString();
                    SettingForm();
                }
                else
                {
                    txtCurPage.Text = TotalPage.ToString();
                    SettingForm();
                }
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (TotalPage > 0)
            {
                int curPage = Convert.ToInt16(txtCurPage.Text);
                if (curPage < TotalPage)
                {
                    txtCurPage.Text = (curPage + 1).ToString();
                    SettingForm();
                }
                else
                {
                    txtCurPage.Text = "1";
                    SettingForm();
                }
            }
        }
        private void btnFormRefresh_Click(object sender, EventArgs e)
        {
            //SettingForm();
            Refresh();
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 1)
            {
                txtCategory.Text = "";
                txtProduct.Text = "";
                txtOrigin.Text = "";
                txtWeight.Text = "";
                txtSizes.Text = "";
                //SelectList();
                //SetBasicSetting();
                int c = dgvProduct.Rows.Count;
                if (DataInspection())
                {
                    if (MessageBox.Show(this, "단가최신화 전에 \n" +
                                              "  * 재고있는 품목 -> 매출단가 최신화 \n" +
                                              "  * 품절된 품목 -> 매출단가 '문의' \n" +
                                              "재입고/품절 품목 최신화하시겠습니까?\n" +
                                              "  * 재고수 0 : '-' , 재고-예약 0 : '문의'", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        CheckInStockProduct();   //재입고된 품목확인
                        CheckOutStockProduct();  //품절된 품목확인
                    }
                    PriceUpdate();
                    SettingForm();
                }
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

                    if (this.dgvProduct.Rows.Count > 0)
                    { 
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("선택삭제(D)");
                        m.Items.Add("잘라내기(X)");
                        m.Items.Add("붙혀넣기(V)");
                        ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                        toolStripSeparator.Name = "toolStripSeparator";
                        toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator);
                        m.Items.Add("강조/취소하기");
                        m.Items.Add("품절품목 일괄처리");
                        m.Items.Add("문자용 텍스트 복사");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator1";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("씨오버 명칭으로 변경");
                        m.Items.Add("명칭변경 설정 초기화");

                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator1";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                        m.Items.Add("단가조정");

                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.Show(dgvProduct, e.Location);
                        //Selection
                        /*PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;*/
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "선택삭제(D)":
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow dr in dgvProduct.SelectedRows)
                        {
                            if (dr.Visible && dr.Index < dgvProduct.Rows.Count - 1)
                            {
                                dgvProduct.Rows.Remove(dr);
                            }
                        }
                    }
                    break;
                case "잘라내기(X)":
                    clipboardModel = new List<SeaoverCopyModel>();
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            if (row.Index < dgvProduct.Rows.Count - 1)
                            {
                                SeaoverCopyModel model = new SeaoverCopyModel();
                                model.category_code = row.Cells["category_code"].Value.ToString();
                                model.category = row.Cells["category"].Value.ToString();
                                model.product_code = row.Cells["product_code"].Value.ToString();
                                model.product = row.Cells["product"].Value.ToString();
                                model.origin_code = row.Cells["origin_code"].Value.ToString();
                                model.origin = row.Cells["origin"].Value.ToString();
                                model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                                model.sizes = row.Cells["sizes"].Value.ToString();
                                model.weight = row.Cells["weight"].Value.ToString();
                                model.unit = row.Cells["unit"].Value.ToString();
                                model.price_unit = row.Cells["price_unit"].Value.ToString();
                                model.unit_count = row.Cells["unit_count"].Value.ToString();
                                model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                                model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                                model.sales_price = row.Cells["sales_price"].Value.ToString();
                                model.division = row.Cells["division"].Value.ToString();
                                model.page = row.Cells["page"].Value.ToString();
                                model.area = row.Cells["area"].Value.ToString();
                                model.row = row.Cells["rows"].Value.ToString();
                                model.cnt = "";
                                model.edit_date = row.Cells["edit_date"].Value.ToString();
                                model.manager1 = row.Cells["manager1"].Value.ToString();
                                clipboardModel.Add(model);
                                dgvProduct.Rows.Remove(row);
                            }
                        }
                    }
                    break;
                case "붙혀넣기(V)":
                    if (dgvProduct.SelectedRows.Count > 0 && clipboardModel.Count > 0)
                        PasteClipboardRowsValue();
                    else
                        Paste();
                    break;
                case "강조/취소하기":
                    if (dgvProduct.SelectedCells.Count > 0)
                    {
                        bool isAccent = !Convert.ToBoolean(dgvProduct.SelectedRows[0].Cells["accent"].Value);
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            if (row.Index < dgvProduct.Rows.Count - 1)
                            {
                                row.Cells["accent"].Value = isAccent;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "행을 선택해주세요.");
                        this.Activate();
                    }
                    break;
                case "품절품목 일괄처리":
                    CheckOutStockProduct(true);
                    break;
                case "문자용 텍스트 복사":
                    if(this.dgvProduct.SelectedRows.Count > 0)
                        Clipboard.SetText(GetHandlingProductTxt());
                    break;
                case "씨오버 명칭으로 변경":
                    SetChangeTextRefresh(false);
                    break;
                case "명칭변경 설정 초기화":
                    SetChangeTextRefresh(true);
                    break;
                case "단가조정":
                    {
                        try
                        {
                            AdjustUnitPriceManager aupm = new AdjustUnitPriceManager(um, this);
                            aupm.Owner = this;
                            aupm.ShowDialog();
                        }
                        catch
                        { }
                    }
                    break;
                default:
                    break;
            }
        }


        private void SetChangeTextRefresh(bool isInit)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                string msg_txt = "";
                if (isInit)
                    msg_txt = "\n * 관련 품목의 설정된 변경명칭 내역을 삭제합니다.";
                else
                    msg_txt = "\n * 변경명칭 내역은 삭제되지 않고 현재 품목서에서만 변경됩니다.";

                if (MessageBox.Show(this, "변경한 명칭내역을 초기화 하시겠습니까?" + msg_txt, "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                //씨오버 원본 데이터
                DataTable productDt = seaoverRepository.GetProductByCode("", "", "", "");
                DataRow[] dr;
                //삭제 and 원본 데이터 
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = new StringBuilder();
                foreach (DataGridViewRow r in dgvProduct.SelectedRows)
                {
                    //기존 명칭 변경내용 삭제
                    if (r.Cells["category_code"].Value != null)
                    {
                        //삭제
                        string whr = $"column_name='category' AND column_code = '{r.Cells["category_code"].Value.ToString()}'";
                        sql = formChangedDataRepository.DeleteChangeData(whr);
                        sqlList.Add(sql);

                        r.Cells["category"].Value = GetCategory(r.Cells["category_code"].Value.ToString());
                    }
                    if (r.Cells["product_code"].Value != null)
                    {
                        string whr = $"column_name='product' AND column_code = '{r.Cells["product_code"].Value.ToString()}'";
                        sql = formChangedDataRepository.DeleteChangeData(whr);
                        sqlList.Add(sql);

                        whr = $"품목코드='{r.Cells["product_code"].Value.ToString()}'";
                        dr = productDt.Select(whr);
                        if (dr.Length > 0)
                            r.Cells["product"].Value = dr[0]["품명"].ToString();
                    }

                    if (r.Cells["sizes_code"].Value != null && r.Cells["sizes"].Value != null && r.Cells["product_code"].Value != null && r.Cells["origin_code"].Value != null)
                    {
                        string whr = $"column_name='sizes' AND column_code = '{r.Cells["product_code"].Value.ToString()}^{r.Cells["origin_code"].Value.ToString()}^{r.Cells["sizes_code"].Value.ToString()}'";
                        sql = formChangedDataRepository.DeleteChangeData(whr);
                        sqlList.Add(sql);

                        whr = $"품목코드='{r.Cells["product_code"].Value.ToString()}' AND 규격코드='{r.Cells["sizes_code"].Value.ToString()}' ";
                        dr = productDt.Select(whr);
                        if (dr.Length > 0)
                            r.Cells["sizes"].Value = dr[0]["규격"].ToString();
                    }

                    if (r.Cells["sizes_code"].Value != null && r.Cells["weight"].Value != null && r.Cells["product_code"].Value != null && r.Cells["origin_code"].Value != null)
                    {
                        string whr = $"column_name='weight' AND column_code = '"
                            + $"{r.Cells["product_code"].Value.ToString()}^{r.Cells["origin_code"].Value.ToString()}^{r.Cells["sizes_code"].Value.ToString()}"
                            + $"^{r.Cells["unit"].Value.ToString()}^{r.Cells["price_unit"].Value.ToString()}^{r.Cells["unit_count"].Value.ToString()}'";
                        sql = formChangedDataRepository.DeleteChangeData(whr);
                        sqlList.Add(sql);

                        //중량
                        if (r.Cells["price_unit"].Value == null)
                            r.Cells["price_unit"].Value = "";
                        string price_unit = r.Cells["price_unit"].Value.ToString();
                        double unit;
                        if (r.Cells["unit"].Value == null || !double.TryParse(r.Cells["unit"].Value.ToString(), out unit))
                            unit = 0;

                        if (price_unit.Contains("팩"))
                        {
                            if (unit < 1)
                                r.Cells["weight"].Value = r.Cells["seaover_unit"].Value.ToString() + "kg \n (" + (Convert.ToDouble(unit) * 1000).ToString() + "g x " + r.Cells["unit_count"].Value.ToString() + "p)";
                            else
                                r.Cells["weight"].Value = r.Cells["seaover_unit"].Value.ToString() + "kg \n (" + unit.ToString() + "k x " + r.Cells["unit_count"].Value.ToString() + "p)";
                        }
                        //kg
                        else if (price_unit == "kg" || price_unit == "Kg" || price_unit == "KG")
                        {
                            if (unit < 1)
                                r.Cells["weight"].Value = r.Cells["seaover_unit"].Value.ToString() + "kg \n (" + (unit * 1000).ToString() + "g x " + r.Cells["unit_count"].Value.ToString() + "p)";
                            else
                                r.Cells["weight"].Value = r.Cells["seaover_unit"].Value.ToString() + "kg \n (" + unit.ToString() + "k x " + r.Cells["unit_count"].Value.ToString() + "p)";
                        }
                        //묶음
                        else if (price_unit == "묶" || price_unit == "묶음")
                            r.Cells["weight"].Value = unit + "kg";
                        //나머지
                        else
                            r.Cells["weight"].Value = r.Cells["seaover_unit"].Value.ToString() + "kg";
                    }

                    //씨오버 명칭으로 다시 변경
                    dr = productDt.Select($"품목코드='{r.Cells["product_code"].Value.ToString()}' " +
                                        $" AND 원산지코드='{r.Cells["origin_code"].Value.ToString()}'" +
                                        $" AND 규격코드='{r.Cells["sizes_code"].Value.ToString()}'" +
                                        $" AND SEAOVER단위='{r.Cells["seaover_unit"].Value.ToString()}'");
                    if (dr.Length > 0)
                    {
                        r.Cells["product"].Value = dr[0]["품명"].ToString();
                        r.Cells["origin"].Value = dr[0]["원산지"].ToString();
                        r.Cells["sizes"].Value = dr[0]["규격"].ToString();
                        //r.Cells["weight"].Value = dr[0]["품명"].ToString();
                    }


                }
                //Execute
                if (isInit && sqlList.Count > 0)
                {
                    if (commonRepository.UpdateTran(sqlList) == -1)
                    {
                        MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                }
            }
        }

        private string GetCategory(string category_code)
        {
            if (category_code.Length < 2)
                return "";
            else if (category_code.Substring(0, 2) == "Aa")
                return "라운드새우류";
            else if (category_code.Substring(0, 2) == "Ab")
                return "새우살류";
            else if (category_code.Substring(0, 2) == "Ba")
                return "쭈꾸미류";
            else if (category_code.Substring(0, 2) == "Bb")
                return "낙지류";
            else if (category_code.Substring(0, 2) == "Bc")
                return "갑오징어류";
            else if (category_code.Substring(0, 2) == "Bd")
                return "오징어류";
            else if (category_code.Substring(0, 2) == "Be")
                return "문어류";
            else if (category_code.Substring(0, 1) == "C")
                return "갑각류";
            else if (category_code.Substring(0, 2) == "Da")
                return "어패류";
            else if (category_code.Substring(0, 2) == "Db")
                return "살류";
            else if (category_code.Substring(0, 2) == "Dc")
                return "해물류";
            else if (category_code.Substring(0, 2) == "Ea")
                return "초밥/일식류";
            else if (category_code.Substring(0, 2) == "Eb")
                return "기타가공품(튀김, 가금류)";
            else if (category_code.Substring(0, 2) == "Ec")
                return "기타가공품(야채, 과일)";
            else if (category_code.Substring(0, 2) == "F")
                return "선어류";
            else
                return "";
        }

        #endregion

        #region 복사/붙혀넣기 Method
        private void txtCurPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCategory.Text = "";
                txtProduct.Text = "";
                txtOrigin.Text = "";
                txtWeight.Text = "";
                txtSizes.Text = "";
                SelectList();
                //SetBasicSetting();
                /*if (DataInspection())
                {
                    PriceUpdate();
                    SettingForm();
                }*/
                SettingForm();
            }
        }
        private void dgvProduct_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Modifiers == Keys.Control)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.C:
                            
                            clipboardModel = new List<SeaoverCopyModel>();
                            if (dgvProduct.SelectedRows.Count > 0)
                            {

                                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                                {
                                    foreach (DataGridViewCell cell in row.Cells)
                                    {
                                        if (cell.Value == null)
                                            cell.Value = string.Empty;
                                    }


                                    if (row.Index < dgvProduct.Rows.Count - 1)
                                    {
                                        SeaoverCopyModel model = new SeaoverCopyModel();
                                        model.accent = Convert.ToBoolean(row.Cells["accent"].Value);
                                        model.category_code = row.Cells["category_code"].Value.ToString();
                                        model.category1 = row.Cells["category1"].Value.ToString();
                                        model.category2 = row.Cells["category2"].Value.ToString();
                                        model.category3 = row.Cells["category3"].Value.ToString();
                                        model.category = row.Cells["category"].Value.ToString();
                                        model.product_code = row.Cells["product_code"].Value.ToString();
                                        model.product = row.Cells["product"].Value.ToString();
                                        model.origin_code = row.Cells["origin_code"].Value.ToString();
                                        model.origin = row.Cells["origin"].Value.ToString();
                                        model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                                        model.sizes = row.Cells["sizes"].Value.ToString();
                                        model.weight = row.Cells["weight"].Value.ToString();
                                        model.unit = row.Cells["unit"].Value.ToString();
                                        model.price_unit = row.Cells["price_unit"].Value.ToString();
                                        model.unit_count = row.Cells["unit_count"].Value.ToString();
                                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                                        model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                                        model.sales_price = row.Cells["sales_price"].Value.ToString();
                                        model.division = row.Cells["division"].Value.ToString();
                                        model.page = row.Cells["page"].Value.ToString();
                                        model.area = row.Cells["area"].Value.ToString();
                                        model.row = row.Cells["rows"].Value.ToString();
                                        model.cnt = "";

                                        clipboardModel.Add(model);
                                        //dgvProduct.Rows.Remove(row);
                                    }
                                }
                            }
                            else
                                CopyToClipboard();
                            break;
                        case Keys.V:
                            if (clipboardModel.Count > 0)
                            {
                                if (dgvProduct.SelectedRows.Count == 0)
                                    dgvProduct.Rows[dgvProduct.Rows.Count - 1].Selected = true;
                                PasteClipboardRowsValue();
                            }
                            else
                                Paste();
                            break;
                        case Keys.X:
                            clipboardModel = new List<SeaoverCopyModel>();
                            if (dgvProduct.SelectedRows.Count > 0)
                            {
                                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                                {
                                    if (row.Index < dgvProduct.Rows.Count - 1)
                                    {
                                        foreach (DataGridViewCell cell in row.Cells)
                                        {
                                            if (cell.Value == null)
                                                cell.Value = string.Empty;
                                        }

                                        SeaoverCopyModel model = new SeaoverCopyModel();
                                        bool accent;
                                        if (!bool.TryParse(row.Cells["accent"].Value.ToString(), out accent))
                                            accent = false;
                                        model.accent = accent;
                                        model.category_code = row.Cells["category_code"].Value.ToString();
                                        model.category1 = row.Cells["category1"].Value.ToString();
                                        model.category2 = row.Cells["category2"].Value.ToString();
                                        model.category3 = row.Cells["category3"].Value.ToString();
                                        model.category = row.Cells["category"].Value.ToString();
                                        model.product_code = row.Cells["product_code"].Value.ToString();
                                        model.product = row.Cells["product"].Value.ToString();
                                        model.origin_code = row.Cells["origin_code"].Value.ToString();
                                        model.origin = row.Cells["origin"].Value.ToString();
                                        model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                                        model.sizes = row.Cells["sizes"].Value.ToString();
                                        model.weight = row.Cells["weight"].Value.ToString();
                                        model.unit = row.Cells["unit"].Value.ToString();
                                        model.price_unit = row.Cells["price_unit"].Value.ToString();
                                        model.unit_count = row.Cells["unit_count"].Value.ToString();
                                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                                        model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                                        model.sales_price = row.Cells["sales_price"].Value.ToString();
                                        model.division = row.Cells["division"].Value.ToString();
                                        model.page = row.Cells["page"].Value.ToString();
                                        model.area = row.Cells["area"].Value.ToString();
                                        model.row = row.Cells["rows"].Value.ToString();
                                        model.cnt = "";

                                        clipboardModel.Add(model);
                                        dgvProduct.Rows.Remove(row);
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    if (e.KeyCode == Keys.Escape)
                    {
                        for (int ix = dgvProduct.Controls.Count - 1; ix >= 0; ix--)
                        {
                            if (dgvProduct.Controls[ix] is DateTimePicker) dgvProduct.Controls[ix].Dispose();
                        }
                    }
                    else if (e.KeyCode == Keys.Delete)
                    {
                        if (dgvProduct.SelectedCells.Count > 0)
                        {
                            foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                            {
                                cell.Value = "";
                            }
                        }
                        else if (dgvProduct.SelectedRows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                if (rows.Visible)
                                    dgvProduct.Rows.Remove(row);
                            }
                            e.Handled = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        //Error Cancel
        private void dgvProduct_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        //복사 
        //  1.DataGridView에서 선택한 영역 Dataobj 에 담기(GetClipboaredContent)
        //  2.Clipboard에 저장(Clipboard.SetDataObject(Dataobj))
        private void CopyToClipboard()
        {
            //Copy to clipboard
            /*if (dgvProduct.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell c in dgvProduct.SelectedCells)
                {
                    if (c.Value != null)
                    {
                        c.Value = c.Value.ToString().Replace("\n", @"\n");
                    }
                }
            }*/

            DataObject dataObj = dgvProduct.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        //붙혀넣기(ctrl + X , ctrl + V)
        private void PasteClipboardRowsValue()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            int row = 0;
            if (dgvProduct.SelectedRows.Count > 0) 
                row = dgvProduct.SelectedRows[0].Index;
            else if (dgvProduct.SelectedCells.Count > 0)
                row = dgvProduct.SelectedCells[0].RowIndex;
            if (clipboardModel.Count > 0)
            {
                if (dgvProduct.Rows.Count > 0)
                {
                    //Row 추가
                    dgvProduct.Rows.Insert(row, clipboardModel.Count);

                    for (int i = clipboardModel.Count - 1; i >= 0; i--)
                    {
                        dgvProduct.Rows[row].Cells["accent"].Value = clipboardModel[i].accent;
                        dgvProduct.Rows[row].Cells["category1"].Value = clipboardModel[i].category1;
                        dgvProduct.Rows[row].Cells["category2"].Value = clipboardModel[i].category2;
                        dgvProduct.Rows[row].Cells["category3"].Value = clipboardModel[i].category3;
                        dgvProduct.Rows[row].Cells["category_code"].Value = clipboardModel[i].category_code;
                        dgvProduct.Rows[row].Cells["category"].Value = clipboardModel[i].category;
                        dgvProduct.Rows[row].Cells["product_code"].Value = clipboardModel[i].product_code;
                        dgvProduct.Rows[row].Cells["product"].Value = clipboardModel[i].product;
                        dgvProduct.Rows[row].Cells["origin_code"].Value = clipboardModel[i].origin_code;
                        dgvProduct.Rows[row].Cells["origin"].Value = clipboardModel[i].origin;
                        dgvProduct.Rows[row].Cells["weight"].Value = clipboardModel[i].weight;
                        dgvProduct.Rows[row].Cells["sizes_code"].Value = clipboardModel[i].sizes_code;
                        dgvProduct.Rows[row].Cells["sizes"].Value = clipboardModel[i].sizes;
                        dgvProduct.Rows[row].Cells["unit"].Value = clipboardModel[i].unit;
                        dgvProduct.Rows[row].Cells["price_unit"].Value = clipboardModel[i].price_unit;
                        dgvProduct.Rows[row].Cells["unit_count"].Value = clipboardModel[i].unit_count;
                        dgvProduct.Rows[row].Cells["seaover_unit"].Value = clipboardModel[i].seaover_unit;
                        dgvProduct.Rows[row].Cells["purchase_price"].Value = clipboardModel[i].purchase_price.ToString();
                        dgvProduct.Rows[row].Cells["sales_price"].Value = clipboardModel[i].sales_price.ToString();
                        dgvProduct.Rows[row].Cells["division"].Value = clipboardModel[i].division;
                        //영억
                        DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[row].Cells["area"];
                        cb.Items.Add("L");
                        cb.Items.Add("R");
                        cb.Items.Add("C");

                        dgvProduct.Rows[row].Cells["page"].Value = clipboardModel[i].page;
                        dgvProduct.Rows[row].Cells["area"].Value = clipboardModel[i].area;
                        dgvProduct.Rows[row].Cells["rows"].Value = clipboardModel[i].row;
                        dgvProduct.Rows[row].Cells["cnt"].Value = clipboardModel[i].cnt;

                        dgvProduct.Rows[row].Cells["edit_date"].Value = clipboardModel[i].edit_date;
                        dgvProduct.Rows[row].Cells["manager1"].Value = clipboardModel[i].manager1;
                        row += 1;

                    }
                }
                else
                {
                    for (int i = clipboardModel.Count - 1; i >= 0; i--)
                    {
                        row = dgvProduct.Rows.Add();
                        dgvProduct.Rows[row].Cells["accent"].Value = clipboardModel[i].accent;
                        dgvProduct.Rows[row].Cells["category1"].Value = clipboardModel[i].category1;
                        dgvProduct.Rows[row].Cells["category2"].Value = clipboardModel[i].category2;
                        dgvProduct.Rows[row].Cells["category3"].Value = clipboardModel[i].category3;
                        dgvProduct.Rows[row].Cells["category_code"].Value = clipboardModel[i].category_code;
                        dgvProduct.Rows[row].Cells["category"].Value = clipboardModel[i].category;
                        dgvProduct.Rows[row].Cells["product_code"].Value = clipboardModel[i].product_code;
                        dgvProduct.Rows[row].Cells["product"].Value = clipboardModel[i].product;
                        dgvProduct.Rows[row].Cells["origin_code"].Value = clipboardModel[i].origin_code;
                        dgvProduct.Rows[row].Cells["origin"].Value = clipboardModel[i].origin;
                        dgvProduct.Rows[row].Cells["weight"].Value = clipboardModel[i].weight;
                        dgvProduct.Rows[row].Cells["sizes_code"].Value = clipboardModel[i].sizes_code;
                        dgvProduct.Rows[row].Cells["sizes"].Value = clipboardModel[i].sizes;
                        dgvProduct.Rows[row].Cells["unit"].Value = clipboardModel[i].unit;
                        dgvProduct.Rows[row].Cells["price_unit"].Value = clipboardModel[i].price_unit;
                        dgvProduct.Rows[row].Cells["unit_count"].Value = clipboardModel[i].unit_count;
                        dgvProduct.Rows[row].Cells["seaover_unit"].Value = clipboardModel[i].seaover_unit;
                        dgvProduct.Rows[row].Cells["purchase_price"].Value = clipboardModel[i].purchase_price.ToString();
                        dgvProduct.Rows[row].Cells["sales_price"].Value = clipboardModel[i].sales_price.ToString();
                        dgvProduct.Rows[row].Cells["division"].Value = clipboardModel[i].division;
                        //영억
                        DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[row].Cells["area"];
                        cb.Items.Add("L");
                        cb.Items.Add("R");
                        cb.Items.Add("C");

                        dgvProduct.Rows[row].Cells["page"].Value = clipboardModel[i].page;
                        dgvProduct.Rows[row].Cells["area"].Value = clipboardModel[i].area;
                        dgvProduct.Rows[row].Cells["rows"].Value = clipboardModel[i].row;
                        dgvProduct.Rows[row].Cells["cnt"].Value = clipboardModel[i].cnt;

                        dgvProduct.Rows[row].Cells["edit_date"].Value = clipboardModel[i].edit_date;
                        dgvProduct.Rows[row].Cells["manager1"].Value = clipboardModel[i].manager1;
                        row += 1;

                    }
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }

        //붙혀넣기
        //  1.Dictionay에 Clipboard Value를 저장 Dic(int, Dic( int, string ) )
        //  2.Dictionay 반복하면서 (끝행은 무시) 현재 선택된 Cell에 출력
        private void PasteClipboardValue()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            //Show Error if no cell is selected
            /*if (dgvProduct.SelectedCells.Count == 0)
            {
                MessageBox.Show(this,"Please select a cell", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }*/
            DataGridViewCell startCell = GetStartCell(dgvProduct);
            if (startCell == null)
            {
                int n = dgvProduct.Rows.Add();
                startCell = dgvProduct.Rows[n].Cells[0];
            }
            //Get the clipboard value in a dictionary
            Dictionary<int, Dictionary<int, string>> cbValue = ClipBoardValues(Clipboard.GetText());
            
            int iRowIndex = startCell.RowIndex;
            foreach (int rowKey in cbValue.Keys)
            {
                int iColIndex = startCell.ColumnIndex;

                if (cbValue.Keys.Count == 1)
                {
                    foreach (int cellKey in cbValue[rowKey].Keys)
                    {
                        if (iColIndex <= dgvProduct.Columns.Count - 1 && iRowIndex <= dgvProduct.Rows.Count - 1)
                        {
                            foreach (DataGridViewCell c in dgvProduct.SelectedCells)
                            {
                                c.Value = cbValue[0][0];
                            }
                        }
                        iColIndex++;
                    }
                    iRowIndex++;
                }
                else
                {
                    foreach (int cellKey in cbValue[rowKey].Keys)
                    {
                        //Check if the index is with in the limit
                        if (iColIndex <= dgvProduct.Columns.Count - 1 && iRowIndex <= dgvProduct.Rows.Count - 1)
                        {
                            DataGridViewCell cell = dgvProduct[iColIndex, iRowIndex];
                            //Copy to selected cells if 'chkPasteToSelectedCells' is checked
                            /*if ((chkPasteToSelectedCells.Checked && cell.Selected) ||
                            (!chkPasteToSelectedCells.Checked))*/
                            cell.Value = cbValue[rowKey][cellKey];
                        }
                        iColIndex++;
                    }
                    iRowIndex++;
                }

            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        //선택한 Cell
        private DataGridViewCell GetStartCell(DataGridView dgView)
        {
            //get the smallest row,column index
            if (dgView.SelectedCells.Count == 0)
                return null;
            int rowIndex = dgView.Rows.Count - 1;
            int colIndex = dgView.Columns.Count - 1;
            foreach (DataGridViewCell dgvCell in dgView.SelectedCells)
            {
                if (dgvCell.RowIndex < rowIndex)
                    rowIndex = dgvCell.RowIndex;
                if (dgvCell.ColumnIndex < colIndex)
                    colIndex = dgvCell.ColumnIndex;
            }
            return dgView[colIndex, rowIndex];
        }
        //Clipboard에 있는 Value를 \t(수평 텝) 기준으로 문자열을 나누어 Dictionary에 담음
        private Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardValue)
        {
            Dictionary<int, Dictionary<int, string>> copyValues = new Dictionary<int, Dictionary<int, string>>();
            String[] lines = clipboardValue.Split('\n');

            for (int i = 0; i <= lines.Length - 1; i++)
            {
                copyValues[i] = new Dictionary<int, string>();
                String[] lineContent = lines[i].Split('\t');
                //if an empty cell value copied, then set the dictionay with an empty string
                //else Set value to dictionary
                if (lineContent.Length == 0)
                    copyValues[i][0] = string.Empty;
                else
                {
                    for (int j = 0; j <= lineContent.Length - 1; j++)
                        copyValues[i][j] = lineContent[j];
                }
            }
            return copyValues;
        }
        #endregion

        #region 셀머지
        //셀머지
        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = dgvProduct.AdvancedCellBorderStyle.Top;
            }

            if (dgvProduct.Rows.Count > 0 && dgvProduct.Rows[e.RowIndex].Cells["cnt"].Value != null)
            {
                if (!string.IsNullOrEmpty(dgvProduct.Rows[e.RowIndex].Cells["cnt"].Value.ToString()))
                {
                    if (Convert.ToInt16(dgvProduct.Rows[e.RowIndex].Cells["cnt"].Value.ToString()) > 60)
                    {
                        DataGridViewCellStyle rowStyle; // = Grid.RowHeadersDefaultCellStyle;
                        rowStyle = dgvProduct.Rows[e.RowIndex].HeaderCell.Style;
                        rowStyle.BackColor = Color.Red;
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Style = rowStyle;
                    }
                    else
                    {
                        if (dgvProduct.Rows[e.RowIndex].HeaderCell.Style.BackColor == Color.Red)
                        {

                            if (Convert.ToInt16(dgvProduct.Rows[e.RowIndex].Cells["cnt"].Value.ToString()) <= 60)
                            {
                                DataGridViewCellStyle rowStyle; // = Grid.RowHeadersDefaultCellStyle;
                                rowStyle = dgvProduct.Rows[e.RowIndex].HeaderCell.Style;
                                rowStyle.BackColor = Color.SeaGreen;
                                rowStyle.ForeColor = Color.White;
                                dgvProduct.Rows[e.RowIndex].HeaderCell.Style = rowStyle;
                            }
                        }
                    }
                }
            }
            //강조
            if (dgvProduct.Rows.Count > 0  )
            {
                if (dgvProduct.Rows[e.RowIndex].Cells["accent"].Value == null)
                {
                    dgvProduct.Rows[e.RowIndex].HeaderCell.Value = "";
                }
                else if (string.IsNullOrEmpty(dgvProduct.Rows[e.RowIndex].Cells["accent"].Value.ToString()))
                {
                    dgvProduct.Rows[e.RowIndex].HeaderCell.Value = "";
                }
                else
                { 
                    if (Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["accent"].Value))
                    {
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Value = "★";
                    }
                    else
                    {
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Value = "";
                    }
                }
            }

        }
        //같으면 줄없애기(머지처럼 보이게)
        bool IsTheSameCellValue(int column, int row)
        {
            DataGridViewCell cell1 = null;
            DataGridViewCell cell2 = null;
            if (row > 0)
            {
                cell1 = dgvProduct[0, row];
                cell2 = dgvProduct[0, row - 1];
                if (cell1.Value == null || cell2.Value == null)
                {
                    return false;
                }
                return cell1.Value.ToString() == cell2.Value.ToString();
            }
            else 
            {
                return false;
            }   
        }
        #endregion

        #region 기타 Method
        public bool SearchingTxt(string find, string except)
        {
            if (!string.IsNullOrEmpty(find))
            {
                string[] find_txt = find.Split(',');
                string[] except_txt = except.Split(',');

                int rowidx = 0;
                if (dgvProduct.SelectedCells.Count > 0)
                    rowidx = dgvProduct.SelectedCells[0].RowIndex;
                if (dgvProduct.Rows.Count > 0)
                {

                    //현재행부터 끝까지
                    for (int i = rowidx; i < dgvProduct.Rows.Count; i++)
                    {

                        DataGridViewCell cell = null;
                        if (i == rowidx)
                        {
                            bool is_find = false;
                            for (int j = dgvProduct.SelectedCells[0].ColumnIndex; j < dgvProduct.ColumnCount; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                {
                                    cell = dgvProduct.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {

                                        string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvProduct.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                            if (!is_find)
                            {
                                for (int j = 0; j < dgvProduct.ColumnCount; j++)
                                {
                                    if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                    {
                                        cell = dgvProduct.Rows[i].Cells[j];
                                        if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                        {
                                            string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                            //찾을 단어
                                            if (!string.IsNullOrEmpty(find))
                                            {
                                                for (int k = 0; k < find_txt.Length; k++)
                                                {
                                                    if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                    {
                                                        is_find = true;
                                                        break;
                                                    }
                                                }
                                            }
                                            //제외 단어
                                            if (is_find && !string.IsNullOrEmpty(except))
                                            {
                                                for (int k = 0; k < except_txt.Length; k++)
                                                {
                                                    if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                    {
                                                        is_find = false;
                                                        break;
                                                    }
                                                }
                                            }
                                            //찾기 성공!
                                            if (is_find)
                                            {
                                                dgvProduct.CurrentCell = cell;
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {
                            for (int j = 0; j < dgvProduct.ColumnCount; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                {
                                    cell = dgvProduct.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvProduct.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //첨부터 현재행까지
                    for (int i = 0; i <= rowidx; i++)
                    {
                        DataGridViewCell cell = null;
                        if (i == rowidx)
                        {
                            for (int j = 0; j < dgvProduct.SelectedCells[0].ColumnIndex; j++)
                            //for (int j = dgvBusiness.SelectedCells[0].ColumnIndex; j < dgvBusiness.ColumnCount; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                {
                                    cell = dgvProduct.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvProduct.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < dgvProduct.ColumnCount; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Visible && dgvProduct.Rows[i].Cells[j] != dgvProduct.SelectedCells[0])
                                {
                                    cell = dgvProduct.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                                        //찾을 단어
                                        if (!string.IsNullOrEmpty(find))
                                        {
                                            for (int k = 0; k < find_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(find_txt[k].Trim()) && common.isContains(val, find_txt[k].Trim()))
                                                {
                                                    is_find = true;
                                                    break;
                                                }
                                            }
                                        }
                                        //제외 단어
                                        if (is_find && !string.IsNullOrEmpty(except))
                                        {
                                            for (int k = 0; k < except_txt.Length; k++)
                                            {
                                                if (!string.IsNullOrEmpty(except_txt[k].Trim()) && common.isContains(val, except_txt[k].Trim()))
                                                {
                                                    is_find = false;
                                                    break;
                                                }
                                            }
                                        }
                                        //찾기 성공!
                                        if (is_find)
                                        {
                                            dgvProduct.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }


                        if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                        {
                            bool is_find = false;
                            string val = cell.Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                            //찾을 단어
                            if (!string.IsNullOrEmpty(find))
                            {
                                for (int k = 0; k < find_txt.Length; k++)
                                {
                                    if (!string.IsNullOrEmpty(find_txt[k].Trim()) && val.Contains(find_txt[k].Trim()))
                                    {
                                        is_find = true;
                                        break;
                                    }
                                }
                            }
                            //제외 단어
                            if (is_find && !string.IsNullOrEmpty(except))
                            {
                                for (int k = 0; k < except_txt.Length; k++)
                                {
                                    if (!string.IsNullOrEmpty(except_txt[k].Trim()) && val.Contains(except_txt[k].Trim()))
                                    {
                                        is_find = false;
                                        break;
                                    }
                                }
                            }
                            //찾기 성공!
                            if (is_find)
                            {
                                dgvProduct.CurrentCell = cell;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        public void SetAdjustUnitPrice(string type, double rate)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                {
                    double sales_price;
                    if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                        sales_price = 0;

                    if (sales_price > 0)
                    {
                        if (type == "인상")
                            sales_price += sales_price * rate / 100;
                        else
                            sales_price -= sales_price * rate / 100;

                        row.Cells["sales_price"].Value = sales_price.ToString("#,##0");
                    }
                }
            }
        }

        public void formPageChange(int page)
        {   
            SettingForm();                
        }

        bool isExcelPriceVisible = true;
        private void PreviewForms()
        {
            //int id = Convert.ToInt16(lbId.Text);
            PageCountSetting();
            PriceUpdate();


            DialogResult dr = MessageBox.Show(this, "취급품목서의 단가를 노출하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
                isExcelPriceVisible = true;
            else if (dr == DialogResult.No)
                isExcelPriceVisible = false;
            else
                return;

            //MessageBox.Show(this,"Excel 파일을 생성하기 약간의 시간이 걸립니다. 우측하단의 상태 표시가 사라지면 자동으로 Excel파일이 열립니다.");
            saveFlag = false;
            bgwExcel.RunWorkerAsync();
        }
        //문자발송용 텍스트 만들기
        private string GetHandlingProductTxt()
        {
            string HandlingProductTxt = "";
            if (dgvProduct.SelectedRows.Count > 0)
            {
                string headerTxt = "(광고)(주)아토무역\n";
                string titleTxt = DateTime.Now.ToString("yyyy.MM") + " " + txtFormName.Text + "\n";
                string managerTxt = um.tel + " " + um.user_name + " " + um.grade + "\n";
                string footerTxt = "\n\n수신을 원하지 않을경우\n문자나 전화 주시면 발송을 안하도록 하겠습니다.\n-------------------\n무료수신거부\n080-855-8825";

                int idx = dgvProduct.SelectedRows.Count - 1;
                if (dgvProduct.SelectedRows[idx].Index == dgvProduct.Rows.Count - 1)
                    idx -= 1;
                string categoryTxt = dgvProduct.SelectedRows[idx].Cells["category"].Value.ToString();
                string productTxt = "♧ " + dgvProduct.SelectedRows[idx].Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "") 
                    + " " + dgvProduct.SelectedRows[idx].Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "")
                    + "(" + dgvProduct.SelectedRows[idx].Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "") + ")" + " ♧";
                string tempTxt = "--------------------------\n【" + categoryTxt + "】\n" + productTxt;

                for (int i = dgvProduct.SelectedRows.Count - 1; i >= 0; i--)
                {
                    DataGridViewRow row = dgvProduct.SelectedRows[i];
                    if (row.Cells["sales_price"].Value != null)
                    { 
                        string sales_price = "";
                        if (row.Cells["price_unit"].Value != null && row.Cells["price_unit"].Value.ToString().Contains("팩"))
                            sales_price = row.Cells["sales_price"].Value.ToString() + "/p";
                        else if (row.Cells["price_unit"].Value != null && row.Cells["price_unit"].Value.ToString() == "kg")
                            sales_price = row.Cells["sales_price"].Value.ToString() + "/k";
                        else
                            sales_price = row.Cells["sales_price"].Value.ToString();


                        if (row.Cells["category"].Value != null && row.Cells["product"].Value != null)
                        {
                            if (categoryTxt != row.Cells["category"].Value.ToString())
                            {
                                HandlingProductTxt += tempTxt;
                                categoryTxt = dgvProduct.SelectedRows[i].Cells["category"].Value.ToString();
                                productTxt = "♧ " + dgvProduct.SelectedRows[i].Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                    + " " + dgvProduct.SelectedRows[i].Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                    + "(" + dgvProduct.SelectedRows[i].Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "") + ")" + " ♧";
                                tempTxt = "\n\n--------------------------\n【" + categoryTxt + "】\n" + productTxt + "\n" + row.Cells["sizes"].Value.ToString().Replace("\n", "").Replace("\r", "") + " @ " + sales_price;
                                if (row.Cells["accent"].Value.ToString() == "True")
                                    tempTxt += "★";
                            }
                            else if (productTxt != "♧ " + dgvProduct.SelectedRows[i].Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                + " " + dgvProduct.SelectedRows[i].Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                + "(" + dgvProduct.SelectedRows[i].Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "") + ")" + " ♧")
                            {
                                productTxt = "♧ " + dgvProduct.SelectedRows[i].Cells["product"].Value.ToString().Replace("\n", "").Replace("\r", "") 
                                    + " " + dgvProduct.SelectedRows[i].Cells["weight"].Value.ToString().Replace("\n", "").Replace("\r", "")
                                    + "(" + dgvProduct.SelectedRows[i].Cells["origin"].Value.ToString().Replace("\n", "").Replace("\r", "") + ")" + " ♧";
                                tempTxt += "\n\n" + productTxt + "\n" + row.Cells["sizes"].Value.ToString().Replace("\n", "").Replace("\r", "") + " @ " + sales_price;
                                if (row.Cells["accent"].Value.ToString() == "True")
                                    tempTxt += "★";
                            }
                            else
                            {
                                tempTxt += "\n" + row.Cells["sizes"].Value.ToString().Replace("\n", "").Replace("\r", "") + " @ " + sales_price;

                                if (row.Cells["accent"].Value.ToString() == "True")
                                    tempTxt += "★";
                            }
                        }
                    }
                }
                HandlingProductTxt += tempTxt;
                HandlingProductTxt = headerTxt + titleTxt + managerTxt + HandlingProductTxt + footerTxt;
            }

            return HandlingProductTxt;
        }

        public void SetClipboardModel(List<SeaoverCopyModel> model)
        {
            clipboardModel = model;
        }
        public void copyUnit(string category)
        {
            clipboardModel = new List<SeaoverCopyModel>();

            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                if (row.Index < dgvProduct.Rows.Count - 1)
                {
                    if (row.Cells["category"].Value.ToString() == category)
                    { 
                        SeaoverCopyModel model = new SeaoverCopyModel();
                        model.category_code = row.Cells["category_code"].Value.ToString();
                        model.category = row.Cells["category"].Value.ToString();
                        model.product_code = row.Cells["product_code"].Value.ToString();
                        model.product = row.Cells["product"].Value.ToString();
                        model.origin_code = row.Cells["origin_code"].Value.ToString();
                        model.origin = row.Cells["origin"].Value.ToString();
                        model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                        model.sizes = row.Cells["sizes"].Value.ToString();
                        model.weight = row.Cells["weight"].Value.ToString();
                        model.unit = row.Cells["unit"].Value.ToString();
                        model.price_unit = row.Cells["price_unit"].Value.ToString();
                        model.unit_count = row.Cells["unit_count"].Value.ToString();
                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                        model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                        model.sales_price = row.Cells["sales_price"].Value.ToString();
                        model.division = row.Cells["division"].Value.ToString();
                        model.page = row.Cells["page"].Value.ToString();
                        model.area = row.Cells["area"].Value.ToString();
                        model.row = row.Cells["rows"].Value.ToString();
                        model.cnt = "";
                        model.edit_date = row.Cells["edit_date"].Value.ToString();
                        model.manager1 = row.Cells["manager1"].Value.ToString();
                        clipboardModel.Add(model);
                        row.HeaderCell.Value = "삭제";
                        //dgvProduct.Rows.Remove(row);
                    }
                }
            }

            //삭제
            retry:
            int del = 0;
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                if (row.HeaderCell.Value == "삭제")
                {
                    dgvProduct.Rows.Remove(row);
                    del += 1;
                }
            }
            if (del > 0)
            {
                goto retry;
            }
            SettingForm();
        }
        public void pasteUnit(string category)
        {
            int row = 0;
            for (int i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                if (dgvProduct.Rows[i].Cells["category"].Value.ToString() == category)
                {
                    row = i;
                    break; 
                }
            }

            //붙혀넣기
            if (clipboardModel.Count > 0)
            {
                //Row 추가
                dgvProduct.Rows.Insert(row, clipboardModel.Count);

                for (int i = 0; i < clipboardModel.Count; i++)
                {
                    dgvProduct.Rows[row].Cells["category_code"].Value = clipboardModel[i].category_code;
                    dgvProduct.Rows[row].Cells["category"].Value = clipboardModel[i].category;
                    dgvProduct.Rows[row].Cells["product_code"].Value = clipboardModel[i].product_code;
                    dgvProduct.Rows[row].Cells["product"].Value = clipboardModel[i].product;
                    dgvProduct.Rows[row].Cells["origin_code"].Value = clipboardModel[i].origin_code;
                    dgvProduct.Rows[row].Cells["origin"].Value = clipboardModel[i].origin;
                    dgvProduct.Rows[row].Cells["weight"].Value = clipboardModel[i].weight;
                    dgvProduct.Rows[row].Cells["sizes_code"].Value = clipboardModel[i].sizes_code;
                    dgvProduct.Rows[row].Cells["sizes"].Value = clipboardModel[i].sizes;
                    dgvProduct.Rows[row].Cells["unit"].Value = clipboardModel[i].unit;
                    dgvProduct.Rows[row].Cells["price_unit"].Value = clipboardModel[i].price_unit;
                    dgvProduct.Rows[row].Cells["unit_count"].Value = clipboardModel[i].unit_count;
                    dgvProduct.Rows[row].Cells["seaover_unit"].Value = clipboardModel[i].seaover_unit;
                    dgvProduct.Rows[row].Cells["purchase_price"].Value = clipboardModel[i].purchase_price.ToString();
                    dgvProduct.Rows[row].Cells["sales_price"].Value = clipboardModel[i].sales_price.ToString();
                    dgvProduct.Rows[row].Cells["division"].Value = clipboardModel[i].division;
                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[row].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");

                    dgvProduct.Rows[row].Cells["page"].Value = clipboardModel[i].page;
                    dgvProduct.Rows[row].Cells["area"].Value = clipboardModel[i].area;
                    dgvProduct.Rows[row].Cells["rows"].Value = clipboardModel[i].row;
                    dgvProduct.Rows[row].Cells["cnt"].Value = clipboardModel[i].cnt;

                    dgvProduct.Rows[row].Cells["edit_date"].Value = clipboardModel[i].edit_date;
                    dgvProduct.Rows[row].Cells["manager1"].Value = clipboardModel[i].manager1;
                    row += 1;

                }
            }
            SettingForm();
        }

        //이전, 다음페이지 보내기
        public void GotoThePage(Dictionary<int, int> dic, int CurPage, string area)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            DataGridView dgv = dgvProduct;
            List<SeaoverCopyModel> minus_list = new List<SeaoverCopyModel>();
            List<SeaoverCopyModel> plus_list = new List<SeaoverCopyModel>();
            foreach (int k in dic.Keys)
            {
                DataGridViewRow row = dgv.Rows[k];
                SeaoverCopyModel model = new SeaoverCopyModel();
                model.category1 = row.Cells["category1"].Value.ToString();
                model.category2 = row.Cells["category2"].Value.ToString();
                model.category3 = row.Cells["category3"].Value.ToString();
                model.category_code = row.Cells["category_code"].Value.ToString();
                model.category = row.Cells["category"].Value.ToString();
                model.product_code = row.Cells["product_code"].Value.ToString();
                model.product = row.Cells["product"].Value.ToString();
                model.origin_code = row.Cells["origin_code"].Value.ToString();
                model.origin = row.Cells["origin"].Value.ToString();
                model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                model.sizes = row.Cells["sizes"].Value.ToString();
                model.weight = row.Cells["weight"].Value.ToString();
                model.unit = row.Cells["unit"].Value.ToString();
                model.price_unit = row.Cells["price_unit"].Value.ToString();
                model.unit_count = row.Cells["unit_count"].Value.ToString();
                model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                model.sales_price = row.Cells["sales_price"].Value.ToString();
                model.division = row.Cells["division"].Value.ToString();
                model.page = row.Cells["page"].Value.ToString();
                //model.area = row.Cells["area"].Value.ToString();
                model.row = row.Cells["rows"].Value.ToString();
                model.cnt = "";
                model.edit_date = row.Cells["edit_date"].Value.ToString();
                model.manager1 = row.Cells["manager1"].Value.ToString();
                //다음페이지
                if (dic[k] > 0)
                {
                    plus_list.Add(model);
                }
                //이전페이지
                else if (dic[k] < 0)
                {
                    minus_list.Add(model);
                }
                
            }
            //실제이동
            int go_Index;
            bool isTop;
            if (minus_list.Count > 0)
            {
                isTop = false;
                go_Index = GetPageTopBottom(CurPage - 1, area, isTop);

                dgvProduct.Rows.Insert(go_Index, minus_list.Count);

                for (int i = minus_list.Count - 1; i >= 0; i--)
                {
                    dgvProduct.Rows[go_Index].Cells["category1"].Value = minus_list[i].category1;
                    dgvProduct.Rows[go_Index].Cells["category2"].Value = minus_list[i].category2;
                    dgvProduct.Rows[go_Index].Cells["category3"].Value = minus_list[i].category3;
                    dgvProduct.Rows[go_Index].Cells["category_code"].Value = minus_list[i].category_code;
                    dgvProduct.Rows[go_Index].Cells["category"].Value = minus_list[i].category;
                    dgvProduct.Rows[go_Index].Cells["product_code"].Value = minus_list[i].product_code;
                    dgvProduct.Rows[go_Index].Cells["product"].Value = minus_list[i].product;
                    dgvProduct.Rows[go_Index].Cells["origin_code"].Value = minus_list[i].origin_code;
                    dgvProduct.Rows[go_Index].Cells["origin"].Value = minus_list[i].origin;
                    dgvProduct.Rows[go_Index].Cells["weight"].Value = minus_list[i].weight;
                    dgvProduct.Rows[go_Index].Cells["sizes_code"].Value = minus_list[i].sizes_code;
                    dgvProduct.Rows[go_Index].Cells["sizes"].Value = minus_list[i].sizes;
                    dgvProduct.Rows[go_Index].Cells["unit"].Value = minus_list[i].unit;
                    dgvProduct.Rows[go_Index].Cells["price_unit"].Value = minus_list[i].price_unit;
                    dgvProduct.Rows[go_Index].Cells["unit_count"].Value = minus_list[i].unit_count;
                    dgvProduct.Rows[go_Index].Cells["seaover_unit"].Value = minus_list[i].seaover_unit;
                    dgvProduct.Rows[go_Index].Cells["purchase_price"].Value = minus_list[i].purchase_price.ToString();
                    dgvProduct.Rows[go_Index].Cells["sales_price"].Value = minus_list[i].sales_price.ToString();
                    dgvProduct.Rows[go_Index].Cells["division"].Value = minus_list[i].division;
                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[go_Index].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");

                    dgvProduct.Rows[go_Index].Cells["page"].Value = CurPage - 1;
                    dgvProduct.Rows[go_Index].Cells["area"].Value = "R";
                    dgvProduct.Rows[go_Index].Cells["rows"].Value = minus_list[i].row;
                    dgvProduct.Rows[go_Index].Cells["cnt"].Value = minus_list[i].cnt;

                    dgvProduct.Rows[go_Index].Cells["edit_date"].Value = minus_list[i].edit_date;
                    dgvProduct.Rows[go_Index].Cells["manager1"].Value = minus_list[i].manager1;
                    go_Index += 1;
                }
            }

            //실제이동
            if (plus_list.Count > 0)
            {
                CurPage += 1;
                go_Index = GetPageTopBottom(CurPage, area, true);
                dgvProduct.Rows.Insert(go_Index, plus_list.Count);

                for (int i = plus_list.Count - 1; i >= 0; i--)
                {
                    dgvProduct.Rows[go_Index].Cells["category1"].Value = plus_list[i].category1;
                    dgvProduct.Rows[go_Index].Cells["category2"].Value = plus_list[i].category2;
                    dgvProduct.Rows[go_Index].Cells["category3"].Value = plus_list[i].category3;
                    dgvProduct.Rows[go_Index].Cells["category_code"].Value = plus_list[i].category_code;
                    dgvProduct.Rows[go_Index].Cells["category"].Value = plus_list[i].category;
                    dgvProduct.Rows[go_Index].Cells["product_code"].Value = plus_list[i].product_code;
                    dgvProduct.Rows[go_Index].Cells["product"].Value = plus_list[i].product;
                    dgvProduct.Rows[go_Index].Cells["origin_code"].Value = plus_list[i].origin_code;
                    dgvProduct.Rows[go_Index].Cells["origin"].Value = plus_list[i].origin;
                    dgvProduct.Rows[go_Index].Cells["weight"].Value = plus_list[i].weight;
                    dgvProduct.Rows[go_Index].Cells["sizes_code"].Value = plus_list[i].sizes_code;
                    dgvProduct.Rows[go_Index].Cells["sizes"].Value = plus_list[i].sizes;
                    dgvProduct.Rows[go_Index].Cells["unit"].Value = plus_list[i].unit;
                    dgvProduct.Rows[go_Index].Cells["price_unit"].Value = plus_list[i].price_unit;
                    dgvProduct.Rows[go_Index].Cells["unit_count"].Value = plus_list[i].unit_count;
                    dgvProduct.Rows[go_Index].Cells["seaover_unit"].Value = plus_list[i].seaover_unit;
                    dgvProduct.Rows[go_Index].Cells["purchase_price"].Value = plus_list[i].purchase_price.ToString();
                    dgvProduct.Rows[go_Index].Cells["sales_price"].Value = plus_list[i].sales_price.ToString();
                    dgvProduct.Rows[go_Index].Cells["division"].Value = plus_list[i].division;
                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[go_Index].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");

                    dgvProduct.Rows[go_Index].Cells["page"].Value = CurPage + 1;
                    dgvProduct.Rows[go_Index].Cells["area"].Value = "L";
                    dgvProduct.Rows[go_Index].Cells["rows"].Value = plus_list[i].row;
                    dgvProduct.Rows[go_Index].Cells["cnt"].Value = plus_list[i].cnt;

                    dgvProduct.Rows[go_Index].Cells["edit_date"].Value = plus_list[i].edit_date;
                    dgvProduct.Rows[go_Index].Cells["manager1"].Value = plus_list[i].manager1;
                    go_Index += 1;
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }

        //해당 페이지의 끝과 처음을 찾기
        private int GetPageTopBottom(int page, string areas, bool isTop)
        {
            int rIndex = 0;

            if (page == 0)
            {
                return rIndex;
            }
            else
            { 

                DataGridView dgv = dgvProduct;
                if (isTop)
                {
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        if (dgv.Rows[i].Cells["page"].Value != null)
                        {
                            if (dgv.Rows[i].Cells["page"].Value.ToString() == page.ToString() && dgv.Rows[i].Cells["area"].Value.ToString() == areas)
                            {
                                rIndex = i;
                                break;
                            }
                        }
                    }
                }
                else 
                {
                    for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dgv.Rows[i].Cells["page"].Value != null)
                        { 
                            if (dgv.Rows[i].Cells["page"].Value.ToString() == page.ToString() && dgv.Rows[i].Cells["area"].Value.ToString() == areas)
                            {
                                rIndex = i + 1;
                                break;
                            }
                        }
                    }
                }

                return rIndex;
            }
        }


        //페이지 왼쪽, 오른쪽 설정창 오픈
        private void PageMananerOpen(int page, string area)
        {
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1)
            {
                List<DgvColumnModel> list = new List<DgvColumnModel>();
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    if (dgv.Rows[i].Cells["page"].Value != null && dgv.Rows[i].Cells["page"].Value != ""
                        && dgv.Rows[i].Cells["area"].Value != null && dgv.Rows[i].Cells["area"].Value != "")
                    {
                        if (dgv.Rows[i].Cells["page"].Value.ToString() == page.ToString()
                            && dgv.Rows[i].Cells["area"].Value.ToString() == area)
                        {
                            DgvColumnModel model = new DgvColumnModel();
                            DataGridViewRow row = dgv.Rows[i];
                            foreach (DataGridViewCell c in row.Cells)
                            { 
                                if(c.Value == null)
                                {
                                    c.Value = "";
                                }
                            }
                            model.accent = Convert.ToBoolean(row.Cells["accent"].Value);
                            model.category1 = row.Cells["category1"].Value.ToString();
                            model.category2 = row.Cells["category2"].Value.ToString();
                            model.category3 = row.Cells["category3"].Value.ToString();
                            model.category_code = row.Cells["category_code"].Value.ToString();
                            model.category = row.Cells["category"].Value.ToString();
                            model.product_code = row.Cells["product_code"].Value.ToString();
                            model.product = row.Cells["product"].Value.ToString();
                            model.origin_code = row.Cells["origin_code"].Value.ToString();
                            model.origin = row.Cells["origin"].Value.ToString();
                            model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                            model.sizes = row.Cells["sizes"].Value.ToString();
                            model.weight = row.Cells["weight"].Value.ToString();
                            model.unit = row.Cells["unit"].Value.ToString();
                            model.price_unit = row.Cells["price_unit"].Value.ToString();
                            model.unit_count = row.Cells["unit_count"].Value.ToString();
                            model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                            model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                            model.sales_price = row.Cells["sales_price"].Value.ToString();
                            model.division = row.Cells["division"].Value.ToString();
                            model.page = page;
                            model.cnt = Convert.ToInt16(row.Cells["cnt"].Value);
                            model.rows = Convert.ToInt16(row.Cells["rows"].Value);
                            model.row_index = Convert.ToInt16(i);   //현재리스트의 rowindex
                            model.area = row.Cells["area"].Value.ToString();
                           
                            list.Add(model);
                        }
                    }
                }
                //매니저 오픈
                if (list.Count > 0)
                {
                    PageManager pm = new PageManager(this, list, page, TotalPage, area, rbOneline.Checked);
                    pm.StartPosition = FormStartPosition.Manual;
                    pm.ShowDialog();
                }
            }
        }
        public List<DgvColumnModel> PageMananerList(int page, string area)
        {
            DataGridView dgv = dgvProduct;
            List<DgvColumnModel> list = new List<DgvColumnModel>();
            if (dgv.Rows.Count > 1)
            {
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    if (dgv.Rows[i].Cells["page"].Value != null && dgv.Rows[i].Cells["page"].Value != ""
                        && dgv.Rows[i].Cells["area"].Value != null && dgv.Rows[i].Cells["area"].Value != "")
                    {

                        if (area == "L" || area == "R")
                        {
                            if (dgv.Rows[i].Cells["page"].Value.ToString() == page.ToString()
                                && dgv.Rows[i].Cells["area"].Value.ToString() == area)
                            {
                                DgvColumnModel model = new DgvColumnModel();
                                DataGridViewRow row = dgv.Rows[i];

                                if(row.Cells["category1"].Value == null)
                                {
                                    row.Cells["category1"].Value = "";
                                }

                                model.category1 = row.Cells["category1"].Value.ToString();
                                if (row.Cells["category2"].Value == null)
                                {
                                    model.category2 = "";
                                }
                                else
                                {
                                    model.category2 = row.Cells["category2"].Value.ToString();
                                }
                                if (row.Cells["category3"].Value == null)
                                {
                                    model.category3 = "";
                                }
                                else
                                {
                                    model.category3 = row.Cells["category3"].Value.ToString();
                                }
                                model.category_code = row.Cells["category_code"].Value.ToString();
                                model.category = row.Cells["category"].Value.ToString();
                                model.product_code = row.Cells["product_code"].Value.ToString();
                                model.product = row.Cells["product"].Value.ToString();
                                model.origin_code = row.Cells["origin_code"].Value.ToString();
                                model.origin = row.Cells["origin"].Value.ToString();
                                model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                                model.sizes = row.Cells["sizes"].Value.ToString();
                                model.weight = row.Cells["weight"].Value.ToString();
                                model.unit = row.Cells["unit"].Value.ToString();
                                model.price_unit = row.Cells["price_unit"].Value.ToString();
                                model.unit_count = row.Cells["unit_count"].Value.ToString();
                                model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                                model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                                model.sales_price = row.Cells["sales_price"].Value.ToString();
                                model.division = row.Cells["division"].Value.ToString();
                                model.page = page;
                                model.cnt = Convert.ToInt16(row.Cells["cnt"].Value);
                                model.rows = Convert.ToInt16(row.Cells["rows"].Value);
                                model.row_index = Convert.ToInt16(i);   //현재리스트의 rowindex
                                model.area = row.Cells["area"].Value.ToString();

                                list.Add(model);
                            }
                        }
                        else
                        {
                            if (dgv.Rows[i].Cells["page"].Value.ToString() == page.ToString())
                            {
                                DgvColumnModel model = new DgvColumnModel();
                                DataGridViewRow row = dgv.Rows[i];

                                model.category1 = row.Cells["category1"].Value.ToString();
                                if (row.Cells["category2"].Value == null)
                                {
                                    model.category2 = "";
                                }
                                else
                                {
                                    model.category2 = row.Cells["category2"].Value.ToString();
                                }
                                if (row.Cells["category3"].Value == null)
                                {
                                    model.category3 = "";
                                }
                                else
                                {
                                    model.category3 = row.Cells["category3"].Value.ToString();
                                }
                                model.category_code = row.Cells["category_code"].Value.ToString();
                                model.category = row.Cells["category"].Value.ToString();
                                model.product_code = row.Cells["product_code"].Value.ToString();
                                model.product = row.Cells["product"].Value.ToString();
                                model.origin_code = row.Cells["origin_code"].Value.ToString();
                                model.origin = row.Cells["origin"].Value.ToString();
                                model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                                model.sizes = row.Cells["sizes"].Value.ToString();
                                model.weight = row.Cells["weight"].Value.ToString();
                                model.unit = row.Cells["unit"].Value.ToString();
                                model.price_unit = row.Cells["price_unit"].Value.ToString();
                                model.unit_count = row.Cells["unit_count"].Value.ToString();
                                model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                                model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                                model.sales_price = row.Cells["sales_price"].Value.ToString();
                                model.division = row.Cells["division"].Value.ToString();
                                model.page = page;
                                model.cnt = Convert.ToInt16(row.Cells["cnt"].Value);
                                model.rows = Convert.ToInt16(row.Cells["rows"].Value);
                                model.row_index = Convert.ToInt16(i);   //현재리스트의 rowindex
                                model.area = row.Cells["area"].Value.ToString();

                                list.Add(model);
                            }
                        }
                    }
                }                
            }
            return list;
        }
        //페이지 설정창 반영하기
        public void setRowsCountList(List<DgvColumnModel> list, List<DgvColumnModel> deleteList, int curPage, string area)
        {
            txtCurPage.Text = curPage.ToString();
            if (list.Count > 0)
            {
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                DataGridView dgv = dgvProduct;
                int sPage = list[0].page;
                int insert_row = list[0].row_index;
                //삭제표시
                for (int i = 0; i < deleteList.Count; i++)
                {   
                    dgv.Rows[deleteList[i].row_index].HeaderCell.Value = "삭제";
                }

                //삭제표시
                int iRow = dgv.Rows.Count - 1;
            retry:
                int del = 0;
                for (int i = 0; i < dgv.Rows.Count - 1; i++)
                {
                    if (dgv.Rows[i].HeaderCell.Value == "삭제")
                    {
                        if (iRow > i)
                        {
                            iRow = i;
                        }
                        dgv.Rows.Remove(dgv.Rows[i]);
                        del += 1;
                    }
                }
                if (del > 0)
                {
                    goto retry;
                }
                //Row 추가
                dgv.Rows.Insert(iRow, list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    DgvColumnModel m = list[i];
                    dgvProduct.Rows[iRow].Cells["accent"].Value = m.accent;
                    dgvProduct.Rows[iRow].Cells["category1"].Value = m.category1;
                    dgvProduct.Rows[iRow].Cells["category2"].Value = m.category2;
                    dgvProduct.Rows[iRow].Cells["category3"].Value = m.category3;
                    dgvProduct.Rows[iRow].Cells["category_code"].Value = m.category_code;
                    dgvProduct.Rows[iRow].Cells["category"].Value = m.category;
                    dgvProduct.Rows[iRow].Cells["product_code"].Value = m.product_code;
                    dgvProduct.Rows[iRow].Cells["product"].Value = m.product;
                    dgvProduct.Rows[iRow].Cells["origin_code"].Value = m.origin_code;
                    dgvProduct.Rows[iRow].Cells["origin"].Value = m.origin;
                    dgvProduct.Rows[iRow].Cells["weight"].Value = m.weight;
                    dgvProduct.Rows[iRow].Cells["sizes_code"].Value = m.sizes_code;
                    dgvProduct.Rows[iRow].Cells["sizes"].Value = m.sizes;
                    dgvProduct.Rows[iRow].Cells["unit"].Value = m.unit;
                    dgvProduct.Rows[iRow].Cells["price_unit"].Value = m.price_unit;
                    dgvProduct.Rows[iRow].Cells["unit_count"].Value = m.unit_count;
                    dgvProduct.Rows[iRow].Cells["seaover_unit"].Value = m.seaover_unit;
                    dgvProduct.Rows[iRow].Cells["purchase_price"].Value = m.purchase_price.ToString();
                    dgvProduct.Rows[iRow].Cells["sales_price"].Value = m.sales_price.ToString();
                    dgvProduct.Rows[iRow].Cells["division"].Value = m.division;
                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[iRow].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");

                    dgvProduct.Rows[iRow].Cells["page"].Value = m.page;
                    dgvProduct.Rows[iRow].Cells["area"].Value = m.area;
                    dgvProduct.Rows[iRow].Cells["rows"].Value = m.rows;
                    dgvProduct.Rows[iRow].Cells["cnt"].Value = m.cnt;
                    dgvProduct.Rows[iRow].Cells["edit_date"].Value = "";
                    dgvProduct.Rows[iRow].Cells["manager1"].Value = "";
                    iRow += 1;
                }
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }
        //강제 제외품목 설정
        private void CheckInStockProduct()
        {
            DataGridView dgv = dgvProduct;

            int result = stockRepository.CallStoredProcedureSTOCK(um.seaover_id, DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable stockDt = stockRepository.GetExistStock();
            if (stockDt.Rows.Count > 0 && dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    if (row.Cells["sales_price"].Value != null
                        && (row.Cells["sales_price"].Value.ToString() == "-"
                        || row.Cells["sales_price"].Value.ToString() == "문의"
                        || row.Cells["sales_price"].Value.ToString().Contains("통관예정")))
                    {
                        string whr = $"품명코드 = '{row.Cells["product_code"].Value.ToString()}'"
                                + $" AND 원산지코드 = '{row.Cells["origin_code"].Value.ToString()}'"
                                + $" AND 규격코드 = '{row.Cells["sizes_code"].Value.ToString()}'"
                                + $" AND 단위 = '{row.Cells["seaover_unit"].Value.ToString()}'";
                        DataRow[] dr = stockDt.Select(whr);
                        if (dr.Length > 0)
                            row.Cells["sales_price"].Value = "0";
                        else
                        {
                            whr = $"품명코드 = '{row.Cells["product_code"].Value.ToString()}'"
                                    + $" AND 원산지코드 = '{row.Cells["origin_code"].Value.ToString()}'"
                                    + $" AND 규격코드 = '{row.Cells["sizes_code"].Value.ToString()}'"
                                    + $" AND 단위 = '{row.Cells["unit"].Value.ToString()}'";
                            dr = stockDt.Select(whr);
                            if (dr.Length > 0)
                                row.Cells["sales_price"].Value = "0";
                        }
                    }
                }
            }
        }

        private void CheckOutStockProduct(bool only_select_row = false)
        {
            int cnt = 0;
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            priceComparisonRepository.CallStoredProcedureSTOCK(um.seaover_id, DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable soldoutDt = priceComparisonRepository.GetSoldOutProduct();
            if (soldoutDt.Rows.Count > 0)
            {
                if (only_select_row)
                {
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            double sales_price;
                            if (row.Cells["sales_price"].Value != null && double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                            {
                                string whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                    + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                    + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                    + " AND 단위 = '" + row.Cells["seaover_unit"].Value + "'";
                                DataRow[] dtRow = soldoutDt.Select(whr);
                                if (dtRow.Length == 0)
                                {
                                    whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                    + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                    + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                    + " AND 단위 = '" + row.Cells["unit"].Value + "'";
                                    dtRow = soldoutDt.Select(whr);
                                }
                                //품절품목
                                if (dtRow.Length > 0)
                                {
                                    //재고수
                                    double total_stock = Convert.ToDouble(dtRow[0]["재고수"].ToString());
                                    //예약수
                                    double reserved_stock = Convert.ToDouble(dtRow[0]["예약수"].ToString());
                                    //관리자 예약
                                    double admin_reserved_stock = 0;
                                    string admin_reserved_stock_detail = dtRow[0]["예약상세"].ToString().Trim();
                                    if (!string.IsNullOrEmpty(admin_reserved_stock_detail))
                                    {
                                        string txtDetail = "";
                                        string[] detail = admin_reserved_stock_detail.Split(' ');
                                        for (int j = 0; j < detail.Length; j++)
                                        {
                                            string d = detail[j].ToString();
                                            if (detail[j].ToString().Contains("관리자"))
                                            {
                                                if (detail[j].Substring(0, 1) == "/")
                                                {
                                                    detail[j] = detail[j].Substring(1, detail[j].Length - 1);
                                                }
                                                detail[j] = detail[j].Replace(",", "");
                                                int rStrok = Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                                admin_reserved_stock += Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                            }
                                            else
                                            {
                                                txtDetail += " " + detail[j].ToString();
                                            }
                                        }
                                        admin_reserved_stock_detail = txtDetail;
                                    }
                                    // 가용재고 = 재고수 - 예약수 + 관리자 예약
                                    double stock = total_stock - reserved_stock + admin_reserved_stock;


                                    if (total_stock == 0)
                                    {
                                        row.Cells["sales_price"].Value = "-";
                                        cnt += 1;
                                    }
                                    else if (stock == 0)
                                    {
                                        row.Cells["sales_price"].Value = "문의";
                                        cnt += 1;
                                    }
                                }
                                else
                                {
                                    row.Cells["sales_price"].Value = "-";
                                    cnt += 1;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        double sales_price;
                        if (row.Cells["sales_price"].Value != null && double.TryParse(row.Cells["sales_price"].Value.ToString(), out sales_price))
                        {
                            string whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                + " AND 단위 = '" + row.Cells["seaover_unit"].Value + "'";
                            DataRow[] dtRow = soldoutDt.Select(whr);
                            if (dtRow.Length == 0)
                            {
                                whr = "품명코드 = '" + row.Cells["product_code"].Value + "'"
                                + " AND 원산지코드 = '" + row.Cells["origin_code"].Value + "'"
                                + " AND 규격코드 = '" + row.Cells["sizes_code"].Value + "'"
                                + " AND 단위 = '" + row.Cells["unit"].Value + "'";
                                dtRow = soldoutDt.Select(whr);
                            }
                            //품절품목
                            if (dtRow.Length > 0)
                            {
                                //재고수
                                double total_stock = Convert.ToDouble(dtRow[0]["재고수"].ToString());
                                //예약수
                                double reserved_stock = Convert.ToDouble(dtRow[0]["예약수"].ToString());
                                //관리자 예약
                                double admin_reserved_stock = 0;
                                string admin_reserved_stock_detail = dtRow[0]["예약상세"].ToString().Trim();
                                if (!string.IsNullOrEmpty(admin_reserved_stock_detail))
                                {
                                    string txtDetail = "";
                                    string[] detail = admin_reserved_stock_detail.Split(' ');
                                    for (int j = 0; j < detail.Length; j++)
                                    {
                                        string d = detail[j].ToString();
                                        if (detail[j].ToString().Contains("관리자"))
                                        {
                                            if (detail[j].Substring(0, 1) == "/")
                                            {
                                                detail[j] = detail[j].Substring(1, detail[j].Length - 1);
                                            }
                                            detail[j] = detail[j].Replace(",", "");
                                            int rStrok = Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                            admin_reserved_stock += Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                        }
                                        else
                                        {
                                            txtDetail += " " + detail[j].ToString();
                                        }
                                    }
                                    admin_reserved_stock_detail = txtDetail;
                                }
                                // 가용재고 = 재고수 - 예약수 + 관리자 예약
                                double stock = total_stock - reserved_stock + admin_reserved_stock;


                                if (total_stock == 0)
                                {
                                    row.Cells["sales_price"].Value = "-";
                                    cnt += 1;
                                }
                                else if (stock == 0)
                                {
                                    row.Cells["sales_price"].Value = "문의";
                                    cnt += 1;
                                }
                            }
                            else
                            {
                                row.Cells["sales_price"].Value = "-";
                                cnt += 1;
                            }
                        }
                    }
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            //MessageBox.Show(this, cnt + "개 완료");
            this.Activate();
        }

        private void ExculedProduct()
        {
            DataGridView dgv = dgvProduct;
            //업체별시세현황 스토어프로시져 호출
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this, "호출 내용이 없음");
                this.Activate();
                return;
            }

            DataTable dt = new DataTable("SEAOVER");
            dt = seaoverRepository.GetProductTapble4();

            if (dt.Rows.Count > 0)
            {
                //0원 품목List
                List<FormDataModel> list = new List<FormDataModel>();
                if (dgv.Rows.Count > 1)
                {
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                    {
                        if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-" 
                            || dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의" 
                            || dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                        {
                            FormDataModel model = new FormDataModel();
                            model.category = dgv.Rows[i].Cells["category"].Value.ToString();
                            model.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                            model.product = dgv.Rows[i].Cells["product"].Value.ToString();
                            model.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                            model.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                            model.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                            model.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                            model.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                            model.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                            model.division = dgv.Rows[i].Cells["division"].Value.ToString();
                            model.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                            model.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                            model.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                            model.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                            model.page = i;     //rowIndex

                            if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "-")
                                model.sales_price = -1;
                            else if (dgv.Rows[i].Cells["sales_price"].Value.ToString() == "문의")
                                model.sales_price = -2;
                            else if (dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                                model.sales_price = -3;


                            string whrStr  = $"품목코드='{dgv.Rows[i].Cells["product_code"].Value}'"
                                        + $" AND 원산지코드='{dgv.Rows[i].Cells["origin_code"].Value}'"
                                        + $" AND 규격코드='{dgv.Rows[i].Cells["sizes_code"].Value}'";


                            /*if (string.IsNullOrEmpty(dgv.Rows[i].Cells["division"].Value.ToString()))
                                whrStr += $" AND 구분 IS NULL";
                            else
                                whrStr += $" AND 구분='{dgv.Rows[i].Cells["division"].Value}'";*/

                            if (string.IsNullOrEmpty(dgv.Rows[i].Cells["unit"].Value.ToString()))
                                whrStr += $" AND 단위 IS NULL";
                            else
                                whrStr += $" AND 단위='{dgv.Rows[i].Cells["unit"].Value}'";

                            if (string.IsNullOrEmpty(dgv.Rows[i].Cells["price_unit"].Value.ToString()))
                                whrStr += $" AND 가격단위 IS NULL";
                            else
                                whrStr += $" AND 가격단위='{dgv.Rows[i].Cells["price_unit"].Value}'";

                            if (string.IsNullOrEmpty(dgv.Rows[i].Cells["unit_count"].Value.ToString()))
                                whrStr += $" AND 단위수량 IS NULL";
                            else
                                whrStr += $" AND 단위수량='{dgv.Rows[i].Cells["unit_count"].Value}'";

                            if (string.IsNullOrEmpty(dgv.Rows[i].Cells["seaover_unit"].Value.ToString()))
                                whrStr += $" AND SEAOVER단위 IS NULL";
                            else
                                whrStr += $" AND SEAOVER단위='{dgv.Rows[i].Cells["seaover_unit"].Value}'";

                            DataRow[] dr = dt.Select(whrStr);
                            if (dr.Length > 0)
                            {
                                DataTable tmpDt = dr.CopyToDataTable();

                                if (tmpDt.Rows.Count == 1)
                                {
                                    model.purchase_price = Convert.ToDouble(tmpDt.Rows[0]["매출단가"]);
                                }
                                else if (tmpDt.Rows.Count > 1)
                                {
                                    string tmp = "";
                                    for (int j = 0; j < tmpDt.Rows.Count; j++)
                                    {
                                        if (string.IsNullOrEmpty(tmp))                                       
                                            tmp = Convert.ToDouble(tmpDt.Rows[j]["매출단가"]).ToString();
                                        else
                                            tmp = tmp + "/" + Convert.ToDouble(tmpDt.Rows[j]["매출단가"]).ToString();
                                         
                                        model.updatetime = tmp;

                                    }
                                }
                            }

                            list.Add(model);
                        }
                    }
                    //반영
                    if (list.Count > 0)
                    {
                        // fmain -> 부모 Form
                        // 부모 Form의 좌표, 크기를 계산
                        int mainformx = this.Location.X;
                        int mainformy = this.Location.Y;
                        int mainformwidth = this.Size.Width;
                        int mainformheight = this.Size.Height;
                        // 자식 Form의 위치 수정
                        // fchild -> 자식 Form
                        // 자식 Form의 크기를 계산
                        ExcludedItems ei = new ExcludedItems(um);
                        int childformwidth = ei.Size.Width;
                        int childformheight = ei.Size.Height;
                        // 자식 Form의 위치 수정
                        Point p = new Point(mainformx + (mainformwidth / 2) - (childformwidth / 2), mainformy + (mainformheight / 2) - (childformheight / 2));
                        ei.StartPosition = FormStartPosition.Manual;
                        Dictionary<int, int> excludeList = ei.Manager(list, p);
                        //List<int> excludeList = ei.Manager(list);
                        foreach (int row in excludeList.Keys)
                        {
                            dgv.Rows[row].Cells["sales_price"].Value = excludeList[row].ToString("#,##0");
                        }
                        //삭제품목
                        for (int i = dgvProduct.Rows.Count - 2; i >= 0; i--)
                        {
                            double sales_price;
                            if (dgv.Rows[i].Cells["sales_price"].Value == null || !double.TryParse(dgv.Rows[i].Cells["sales_price"].Value.ToString(), out sales_price))
                                sales_price = 0;
                            if (sales_price == -999)
                                dgv.Rows.Remove(dgv.Rows[i]);
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "설정된 제외품목이 없습니다.");
                        this.Activate();
                    }
                }
            }
        }
        //품목서 리스트 클릭후 데이터 불러오기
        private void GetFormData(int idx, string id)
        {
            //이벤트 끄기
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            //출력
            cbArchive.Text = "";
            dgvProduct.Rows.Clear();
            //저장값 불러오기
            List<FormDataModel> model = seaoverRepository.GetFormData("", id);
            if (model.Count > 0)
            {
                lbId.Text = model[0].id.ToString();
                lbUpdatetime.Text = Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd hh:mm:ss");
                //lbUpdatedate.Text = "작성일자 : " + Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd");
                //lbRemark.Text = "/ *단가 조정 필요시 문의바람* (작성일자:" + Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd") + ")";
                //lbEditDate.Text = "작성일자 : " + Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd");
                lbEditDate.Text = "작성일자 : " + DateTime.Now.ToString("yyyy-MM-dd");
                lbEditUser.Text = model[0].edit_user.ToString();
                txtFormName.Text = model[0].form_name.ToString();
                lbCreateUser.Text = model[0].create_user.ToString();

                if (model[0].is_rock)
                {
                    btnSettingPassword.Text = "품목서 잠금해제";
                    btnSettingPassword.ForeColor = Color.Red;
                }
                else
                {
                    btnSettingPassword.Text = "품목서 잠금";
                    btnSettingPassword.ForeColor = Color.Blue;
                }


                for (int i = 0; i < model.Count; i++)
                {
                    FormDataModel m = model[i];
                    //Row 추가
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["accent"].Value = m.accent;
                    dgvProduct.Rows[n].Cells["category"].Value = m.category.Trim();
                    dgvProduct.Rows[n].Cells["category_code"].Value = m.category_code;
                    if (m.category_code != null && !string.IsNullOrEmpty(m.category_code))
                    {
                        dgvProduct.Rows[n].Cells["category1"].Value = m.category_code.Substring(0, 1);

                        int d;
                        if (!int.TryParse(m.category_code.Substring(1, 1), out d))
                        {
                            dgvProduct.Rows[n].Cells["category2"].Value = m.category_code.ToString().Substring(1, 1);
                        }

                        if (m.category_code.Length <= 5)
                            dgvProduct.Rows[n].Cells["category3"].Value = m.category_code.Substring(m.category_code.Length - 3, 3);
                        else if (dgvProduct.Rows[n].Cells["category2"].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[n].Cells["category2"].Value.ToString()))
                            dgvProduct.Rows[n].Cells["category3"].Value = m.category_code.Substring(2, 3);
                        else
                            dgvProduct.Rows[n].Cells["category3"].Value = m.category_code.Substring(1, 3);
                    }
                    else
                    {
                        dgvProduct.Rows[n].Cells["category1"].Value = m.category_code;
                        dgvProduct.Rows[n].Cells["category2"].Value = m.category_code;
                        dgvProduct.Rows[n].Cells["category3"].Value = m.category_code;
                    }
                    dgvProduct.Rows[n].Cells["product_code"].Value = m.product_code;
                    dgvProduct.Rows[n].Cells["product"].Value = m.product;
                    dgvProduct.Rows[n].Cells["origin_code"].Value = m.origin_code;
                    dgvProduct.Rows[n].Cells["origin"].Value = m.origin;
                    dgvProduct.Rows[n].Cells["weight"].Value = m.weight;
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = m.sizes_code;
                    dgvProduct.Rows[n].Cells["sizes"].Value = m.sizes;
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = m.purchase_price.ToString("#,##0");
                    //dgvProduct.Rows[n].Cells["sales_price"].Value = m.sales_price.ToString("#,##0");
                    if (m.sales_price == -1)
                        dgvProduct.Rows[n].Cells["sales_price"].Value = "-";
                    else if (m.sales_price == -2)
                        dgvProduct.Rows[n].Cells["sales_price"].Value = "문의";
                    else if (m.sales_price == -3)
                        dgvProduct.Rows[n].Cells["sales_price"].Value = "★통관예정★";
                    else
                    {
                        dgvProduct.Rows[n].Cells["sales_price"].Value = m.sales_price.ToString("#,##0");
                    }
                    dgvProduct.Rows[n].Cells["unit"].Value = m.unit;
                    dgvProduct.Rows[n].Cells["price_unit"].Value = m.price_unit;
                    dgvProduct.Rows[n].Cells["unit_count"].Value = m.unit_count;
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = m.seaover_unit;
                    dgvProduct.Rows[n].Cells["division"].Value = m.division;
                    dgvProduct.Rows[n].Cells["page"].Value = m.page;
                    dgvProduct.Rows[n].Cells["cnt"].Value = m.cnt;
                    dgvProduct.Rows[n].Cells["rows"].Value = m.row_index;
                    dgvProduct.Rows[n].Cells["edit_date"].Value = "";
                    dgvProduct.Rows[n].Cells["manager1"].Value = "";

                    if (m.form_remark != null && !string.IsNullOrEmpty(m.form_remark))
                    {
                        lbRemark.Text = m.form_remark;
                    }
                    else
                    {
                        string managerTxt;
                        if (um.grade == null)
                        {
                            managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
                        }
                        else if (string.IsNullOrEmpty(um.grade))
                        {
                            managerTxt = "담당자 : " + um.user_name.ToString() + " 담당자 (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
                        }
                        else
                        {
                            managerTxt = "담당자 : " + um.user_name.ToString() + " " + um.grade.ToString() + " (" + Libs.Tools.Common.autoHypehen(um.tel.ToString()) + ")";
                        }

                        lbRemark.Text = managerTxt + " / " + um.form_remark;
                    }
                    

                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[n].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");
                    dgvProduct.Rows[n].Cells["area"].Value = m.area;
                }
                //Font 
                Font f = AutoFontSize(lbRemark, lbRemark.Text);
                lbRemark.Font = f;
                //DataInspection();
                //SetTitleForm();
                txtCurPage.Text = "1";
                SettingForm();
            }
            //이벤트 끄기
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        //품목서 리스트 클릭후 Archive 복원
        private void GetTempData(int form_id, int temp_id)
        {
            dgvProduct.Rows.Clear();
            //저장값 불러오기
            List<FormDataModel> model = seaoverRepository.GetFormDataTemp("2", form_id.ToString(), temp_id.ToString());
            if (model.Count > 0)
            {
                lbId.Text = model[0].id.ToString();
                lbUpdatetime.Text = Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd hh:mm:ss");
                //lbUpdatedate.Text = "작성일자 : " + Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd");
                //lbRemark.Text = "/ *단가 조정 필요시 문의바람* (작성일자 : " + Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd") + ")";
                //lbEditDate.Text = "작성일자 : " + Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd");
                lbEditDate.Text = "작성일자 : " + DateTime.Now.ToString("yyyy-MM-dd");
                lbEditUser.Text = model[0].edit_user.ToString();
                txtFormName.Text = model[0].form_name.ToString();
                lbCreateUser.Text = model[0].create_user.ToString();

                for (int i = 0; i < model.Count; i++)
                {
                    FormDataModel m = model[i];
                    //Row 추가
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["accent"].Value = m.accent;
                    if (m.category_code.Length > 0)
                    {
                        dgvProduct.Rows[n].Cells["category1"].Value = m.category_code.Substring(0, 1);
                        int d;
                        if (!int.TryParse(m.category_code.Substring(1, 1), out d))
                        {
                            dgvProduct.Rows[n].Cells["category2"].Value = m.category_code.Substring(1, 1);
                        }
                        dgvProduct.Rows[n].Cells["category3"].Value = m.category_code.Substring(m.category_code.Length - 3, 3);
                    }

                    dgvProduct.Rows[n].Cells["category"].Value = m.category;
                    dgvProduct.Rows[n].Cells["category_code"].Value = m.category_code;
                    dgvProduct.Rows[n].Cells["product_code"].Value = m.product_code;
                    dgvProduct.Rows[n].Cells["product"].Value = m.product;
                    dgvProduct.Rows[n].Cells["origin_code"].Value = m.origin_code;
                    dgvProduct.Rows[n].Cells["origin"].Value = m.origin;
                    dgvProduct.Rows[n].Cells["weight"].Value = m.weight;
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = m.sizes_code;
                    dgvProduct.Rows[n].Cells["sizes"].Value = m.sizes;
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = m.purchase_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["edit_date"].Value = "";
                    dgvProduct.Rows[n].Cells["manager1"].Value = "";
                    //dgvProduct.Rows[n].Cells["sales_price"].Value = m.sales_price.ToString("#,##0");
                    if (m.sales_price == -1)
                        dgvProduct.Rows[n].Cells["sales_price"].Value = "-";
                    else if (m.sales_price == -2)
                        dgvProduct.Rows[n].Cells["sales_price"].Value = "문의";
                    else if (m.sales_price == -3)
                        dgvProduct.Rows[n].Cells["sales_price"].Value = "★통관예정★";
                    else
                        dgvProduct.Rows[n].Cells["sales_price"].Value = m.sales_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["unit"].Value = m.unit;
                    dgvProduct.Rows[n].Cells["price_unit"].Value = m.price_unit;
                    dgvProduct.Rows[n].Cells["unit_count"].Value = m.unit_count;
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = m.seaover_unit;
                    dgvProduct.Rows[n].Cells["division"].Value = m.division;
                    dgvProduct.Rows[n].Cells["page"].Value = m.page;
                    dgvProduct.Rows[n].Cells["cnt"].Value = m.cnt;
                    dgvProduct.Rows[n].Cells["rows"].Value = m.row_index;
                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[n].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");
                    dgvProduct.Rows[n].Cells["area"].Value = m.area;
                }
                //DataInspection();
                //SetTitleForm();
                SettingForm();
            }

        }
        //선택한 품목서의 Backup 되돌리기
        private void GetFormDataTemp(int id)
        {
            cbArchive.Items.Clear();
            DataTable dt = new DataTable();
            dt = seaoverRepository.GetFormDataTemp(id);

            cbArchive.DisplayMember = "Display";
            cbArchive.ValueMember = "Value";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cbArchive.Items.Add(new { Display = Convert.ToDateTime(dt.Rows[i]["updatetime"]).ToString("yyyy-MM-dd hh:mm"), Value = dt.Rows[i]["temp_id"] });
            }

        }
        //데이터 검수
        private bool DataInspection()
        {
            Dictionary<string, int> dataDic = new Dictionary<string, int>();
            DataGridView dgv = dgvProduct;
            bool isDelete;
            isDelete = true;

            if (dgv.Rows.Count > 1)
            {
                List<int> removeRowIndexs = new List<int>();
                //중복삭제
                foreach (DataGridViewRow r in dgv.Rows)
                {
                    if (r.Cells["category_code"].Value != null && r.Cells["product_code"].Value != null
                            && r.Cells["origin_code"].Value != null && r.Cells["sizes_code"].Value != null && r.Cells["unit"].Value != null)
                    {
                        string str = r.Cells["category_code"].Value.ToString() + "/" + r.Cells["product_code"].Value.ToString()
                              + "/" + r.Cells["origin_code"].Value.ToString() + "/" + r.Cells["sizes_code"].Value.ToString()
                              + "/" + r.Cells["seaover_unit"].Value.ToString();

                        bool keyExists = dataDic.ContainsKey(str);
                        if (!keyExists)
                        {
                            dataDic.Add(str, r.Index);
                            r.HeaderCell.Value = "";
                            r.Cells["product"].Style.ForeColor = Color.Black;
                        }
                        else
                        {
                            removeRowIndexs.Add(r.Index);
                            r.Cells["product"].Style.ForeColor = Color.Red;
                            dgv.Rows[dataDic[str]].Cells["product"].Style.ForeColor = Color.Red;
                        }
                    }
                }


            retry:
                int delCnt = 0;
                foreach (DataGridViewRow r in dgv.Rows)
                {
                    if (r.Cells["product"].Style.ForeColor == Color.Red)
                    {
                        MessageBox.Show(this, "중복된 품목이 있습니다!");
                        this.Activate();

                        dgvProduct.Rows[r.Index].Selected = true;
                        dgvProduct.FirstDisplayedScrollingRowIndex = r.Index;
                        delCnt++;
                        return false;
                    }
                        
                }
                //중복삭제
                /*if (delCnt > 0)
                { 
                    foreach (DataGridViewRow r in dgv.Rows)
                    {
                        if (r.HeaderCell.Value == "중복")
                            dgv.Rows.Remove(r);
                    }
                    //중복삭제
                    foreach (DataGridViewRow r in dgv.Rows)
                    {
                        if (r.HeaderCell.Value == "중복")
                            dgv.Rows.Remove(r);
                    }
                    goto retry;
                }*/

            }
            return true;
        }

        //단가 갱신
        private void PriceUpdate(bool isMsg = true)
        {
            //업체별시세현황 스토어프로시져 호출
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
            {
                messageBox.Show(this, "호출 내용이 없음");
                this.Activate();
                return;
            }
            //MSGBOX
            if (isMsg && messageBox.Show(this, "단가를 최신단가로 갱신하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

                
            {
                DataGridView dgv = dgvProduct;
                //단가갱신
                DataTable dt = new DataTable("SEAOVER");
                dt = seaoverRepository.GetProductTapble2();
                if (dt.Rows.Count > 0)
                {
                    if (dgv.Rows.Count > 1)
                    {
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            if (!dgv.Rows[i].Visible)
                                dgv.Rows[i].Visible = true;

                            //공백없애기
                            dgv.Rows[i].Cells["category"].Value = dgv.Rows[i].Cells["category"].Value.ToString().Trim();
                            dgv.Rows[i].Cells["origin"].Value = dgv.Rows[i].Cells["origin"].Value.ToString().Trim();
                            dgv.Rows[i].Cells["product"].Value = dgv.Rows[i].Cells["product"].Value.ToString().Trim();
                            dgv.Rows[i].Cells["sizes"].Value = dgv.Rows[i].Cells["sizes"].Value.ToString().Trim();
                            dgv.Rows[i].Cells["weight"].Value = dgv.Rows[i].Cells["weight"].Value.ToString().Trim();
                        }
                        dgv.Update();
                        int errCnt = 0;
                        int errRow = 0;
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            if (dgv.Rows[i].Cells["sales_price"].Value.ToString() != "-" && dgv.Rows[i].Cells["sales_price"].Value.ToString() != "문의" && !dgv.Rows[i].Cells["sales_price"].Value.ToString().Contains("통관예정"))
                            {
                                foreach (DataGridViewCell c in dgv.Rows[i].Cells)
                                {
                                    if (c.Value == null && c.ColumnIndex < 11)
                                        c.Value = "";
                                }

                                string whrStr = "";

                                whrStr = $"품목코드='{dgv.Rows[i].Cells["product_code"].Value}'"
                                    + $" AND 원산지코드='{dgv.Rows[i].Cells["origin_code"].Value}'"
                                    + $" AND 규격코드='{dgv.Rows[i].Cells["sizes_code"].Value}'";


                                if (string.IsNullOrEmpty(dgv.Rows[i].Cells["unit"].Value.ToString()))
                                    whrStr += $" AND 단위 IS NULL";
                                else
                                    whrStr += $" AND 단위='{dgv.Rows[i].Cells["unit"].Value}'";

                                if (string.IsNullOrEmpty(dgv.Rows[i].Cells["price_unit"].Value.ToString()))
                                    whrStr += $" AND 가격단위 IS NULL";
                                else
                                    whrStr += $" AND 가격단위='{dgv.Rows[i].Cells["price_unit"].Value}'";

                                if (string.IsNullOrEmpty(dgv.Rows[i].Cells["unit_count"].Value.ToString()))
                                    whrStr += $" AND 단위수량 IS NULL";
                                else
                                {
                                    if (dgv.Rows[i].Cells["unit_count"].Value.ToString() == "0")
                                        whrStr += $" AND (단위수량='0' OR 단위수량 IS NULL)";
                                    else
                                        whrStr += $" AND 단위수량='{dgv.Rows[i].Cells["unit_count"].Value}'";
                                }

                                if (string.IsNullOrEmpty(dgv.Rows[i].Cells["seaover_unit"].Value.ToString()))
                                    whrStr += $" AND SEAOVER단위 IS NULL";
                                else
                                    whrStr += $" AND SEAOVER단위='{dgv.Rows[i].Cells["seaover_unit"].Value}'";

                                DataRow[] dr = dt.Select(whrStr);
                                if (dr.Length > 0)
                                {
                                    DataTable tmpDt = dr.CopyToDataTable();
                                    tmpDt.Columns.Add("disc", typeof(bool));
                                    //중복체크
                                    Dictionary<string, int> discDic = new Dictionary<string, int>();
                                    for (int j = tmpDt.Rows.Count - 1; j >= 0; j--)
                                    {
                                        tmpDt.Rows[j]["disc"] = false;
                                        string keys = tmpDt.Rows[j]["품목코드"] + "/" + tmpDt.Rows[j]["원산지코드"] + "/" + tmpDt.Rows[j]["규격코드"] + "/" + tmpDt.Rows[j]["단위"]
                                             + "/" + tmpDt.Rows[j]["가격단위"] + "/" + tmpDt.Rows[j]["단위수량"] + "/" + tmpDt.Rows[j]["SEAOVER단위"];

                                        if (discDic.ContainsKey(keys))
                                            tmpDt.Rows[j]["disc"] = true;
                                        else
                                            discDic.Add(keys, j);
                                    }
                                    //중복삭제
                                    DataTable newDt = new DataTable();
                                    newDt.Columns.Add("원산지코드", typeof(string));
                                    newDt.Columns.Add("품목코드", typeof(string));
                                    newDt.Columns.Add("규격코드", typeof(string));
                                    newDt.Columns.Add("대분류", typeof(string));
                                    newDt.Columns.Add("단위", typeof(string));
                                    newDt.Columns.Add("가격단위", typeof(string));
                                    newDt.Columns.Add("단위수량", typeof(string));
                                    newDt.Columns.Add("SEAOVER단위", typeof(string));
                                    newDt.Columns.Add("매출단가", typeof(int));
                                    newDt.Columns.Add("단가수정일", typeof(string));
                                    newDt.Columns.Add("구분", typeof(string));
                                    newDt.Columns.Add("담당자1", typeof(string));


                                    try
                                    {
                                        for (int j = tmpDt.Rows.Count - 1; j >= 0; j--)
                                        {
                                            if (!Convert.ToBoolean(tmpDt.Rows[j]["disc"]))
                                            {
                                                DataRow r = tmpDt.Rows[j];
                                                bool isExist = false;
                                                foreach (DataRow newDr in newDt.Rows)
                                                {
                                                    if (Convert.ToDouble(newDr["매출단가"].ToString()) == Convert.ToDouble(r["매출단가"].ToString()))
                                                    {
                                                        isExist = true;
                                                        break;
                                                    }
                                                }
                                                if (!isExist)
                                                {
                                                    newDt.Rows.Add(r["원산지코드"], r["품목코드"], r["규격코드"], r["대분류"], r["단위"]
                                                            , r["가격단위"], r["단위수량"], r["SEAOVER단위"], Convert.ToInt32(r["매출단가"]), r["단가수정일"], r["구분"], r["담당자1"]);
                                                }
                                            }
                                        }
                                        //출력
                                        if (newDt.Rows.Count > 0)
                                        {
                                            if (dgv.Rows[i].Cells["sales_price"].Value != "-")
                                            {
                                                if (newDt.Rows.Count == 1)
                                                {
                                                    dgv.Rows[i].Cells["sales_price"].Value = Convert.ToInt32(newDt.Rows[0]["매출단가"]).ToString("#,##0");
                                                    if (newDt.Rows[0]["단가수정일"] == null || newDt.Rows[0]["단가수정일"].ToString() == "")
                                                        dgv.Rows[i].Cells["edit_date"].Value = "";
                                                    else
                                                        dgv.Rows[i].Cells["edit_date"].Value = Convert.ToDateTime(newDt.Rows[0]["단가수정일"]).ToString("yyyy-MM-dd");

                                                    dgv.Rows[i].Cells["manager1"].Value = newDt.Rows[0]["담당자1"];
                                                }
                                                else
                                                {

                                                    //해당 Row 선택
                                                    dgv.CurrentRow.Selected = false;
                                                    dgv.Rows[i].Selected = true;
                                                    dgv.FirstDisplayedScrollingRowIndex = i;
                                                    //단가리스트 생성
                                                    PriceSelectManager pm = new PriceSelectManager();
                                                    Point p = dgv.PointToScreen(dgv.GetCellDisplayRectangle(0, i, false).Location);
                                                    p = new Point(p.X - pm.Size.Width - dgv.ColumnHeadersHeight, p.Y - pm.Size.Height + dgv.Rows[i].Height);

                                                    pm.StartPosition = FormStartPosition.Manual;
                                                    string[] price = pm.Manager(newDt, p);

                                                    dgv.Rows[i].Cells["sales_price"].Value = Convert.ToInt32(price[0]).ToString("#,##0");

                                                    DateTime edit_date;
                                                    if (DateTime.TryParse(price[1], out edit_date))
                                                        dgv.Rows[i].Cells["edit_date"].Value = Convert.ToDateTime(price[1]).ToString("yyyy-MM-dd");
                                                    else
                                                        dgv.Rows[i].Cells["edit_date"].Value = "";
                                                    dgv.Rows[i].Cells["manager1"].Value = price[2];
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    { 
                                        messageBox.Show(this, ex.Message);
                                    }
                                }
                                else
                                {
                                    dgv.Rows[i].Visible = true;
                                    dgv.CurrentRow.Selected = false;
                                    dgv.Rows[i].Selected = true;
                                    dgv.FirstDisplayedScrollingRowIndex = i;
                                    dgv.Rows[i].Cells["sales_price"].Value = "0";
                                    errCnt++;
                                    errRow = i;
                                }
                            }
                            else
                            {
                                dgv.Rows[i].HeaderCell.Value = "";
                            }
                        }
                        //에러메세지
                        if (errCnt > 0)
                        {
                            MessageBox.Show(this, "씨오버에서 찾지 못한 내역이 " + errCnt + "개 확인되었습니다!");
                            this.Activate();
                            dgvProduct.FirstDisplayedScrollingRowIndex = errRow;
                        }
                    }
                }
            }

            //재등록
            /*int id = Convert.ToInt32(lbId.Text);
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = seaoverRepository.DeleteFormData(id.ToString());
            sqlList.Add(sql);
            InsertProductPrice(id, sqlList, "갱신");*/

            //최신화
            /*GetTitleList();
            SettingForm();*/
        }

        private void PriceUpdateSql()
        {
            int id;
            if (int.TryParse(lbId.Text, out id))
            {
                if (dgvProduct.Rows.Count > 1)
                { 
                    try
                    {
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        StringBuilder sql = seaoverRepository.DeleteFormData(id.ToString());
                        sqlList.Add(sql);

                        InsertProductPrice(id, sqlList, "갱신");
                        GetTitleList();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                //MessageBox.Show(this,"갱신할 내역을 불러와주세요.");
            }
        }


        //품목서 제목
        private void txtFormName_TextChanged(object sender, EventArgs e)
        {
            lbFormName.Text = txtFormName.Text;
        }
        //초기화
        private void Refresh()
        {
            dgvProduct.Rows.Clear();
            lCategory.Controls.Clear();
            rCategory.Controls.Clear();
            lProduct.Controls.Clear();
            rProduct.Controls.Clear();
            lOrigin.Controls.Clear();
            rOrigin.Controls.Clear();
            lWeight.Controls.Clear();
            rWeight.Controls.Clear();
            lSize.Controls.Clear();
            rSize.Controls.Clear();
            lPrice.Controls.Clear();
            rPrice.Controls.Clear();

            txtFormName.Text = "";
            lbId.Text = "NULL";
            lbEditUser.Text = "NULL";
            lbUpdatetime.Text = "NULL";
        }
        //셀 정렬
        private void SortSetting()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            if (!string.IsNullOrEmpty(cbSort.Text))
            {
                string[] sortStrs = cbSort.Text.Split('+');
                string sortStr = "";
                if (sortStrs.Length > 0)
                {
                    for (int i = 0; i < sortStrs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(sortStr))
                            sortStr += ", " + SetsortStr(sortStrs[i]);
                        else
                            sortStr = SetsortStr(sortStrs[i]);
                    }
                }

                //사이즈 정렬 컬럼추가
                DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable(dgvProduct);
                tb.Columns.Add("category_hold", typeof(double)).SetOrdinal(1);
                tb.Columns.Add("product_sort", typeof(double)).SetOrdinal(1);
                tb.Columns.Add("sizes_sort", typeof(double)).SetOrdinal(1);
                tb.Columns.Add("sizes_sort2", typeof(double)).SetOrdinal(1);


                string strCategory = null;
                int category_id = 0;
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    if (strCategory != tb.Rows[i]["category"].ToString())
                    {
                        category_id++;
                        strCategory = tb.Rows[i]["category"].ToString();
                    }
                    tb.Rows[i]["category_hold"] = category_id;

                    string tmpStr = "";
                    double res;
                    string str = tb.Rows[i]["product"].ToString();
                    if (str.Contains("F"))
                    {
                        tb.Rows[i]["product_sort"] = 1;
                    }
                    else if (str.Contains("M"))
                    {
                        tb.Rows[i]["product_sort"] = 2;
                    }


                    str = tb.Rows[i]["sizes"].ToString();
                    if (str.Length > 0)
                    {
                        double d = 0;
                        for (int j = 0; j < str.Length; j++)
                        {
                            string tempStr = str.Substring(0, j + 1);
                            if (!double.TryParse(tempStr, out d))
                            {
                                if (tempStr.Length == 1)
                                    tb.Rows[i]["sizes_sort"] = 99999;
                                else
                                {
                                    if (double.TryParse(tempStr.Substring(0, tempStr.Length - 1), out d))
                                        tb.Rows[i]["sizes_sort"] = d;
                                }
                                break;
                            }
                        }
                        //전부 숫자일 경우
                        if (d == 0)
                            d = 99999;
                        tb.Rows[i]["sizes_sort"] = d;

                    }
                    else
                        tb.Rows[i]["sizes_sort"] = 99999;

                    //S,M,L 특수정렬
                    if (str.Contains("S"))
                    {
                        if (str.Contains("4"))
                            tb.Rows[i]["sizes_sort2"] = 111;
                        else if (str.Contains("3"))
                            tb.Rows[i]["sizes_sort2"] = 112;
                        else if (str.Contains("2"))
                            tb.Rows[i]["sizes_sort2"] = 113;
                        else if (str.Contains("1"))
                            tb.Rows[i]["sizes_sort2"] = 114;
                        else
                            tb.Rows[i]["sizes_sort2"] = 115;

                        tb.Rows[i]["sizes_sort"] = 99999;
                    }
                    else if (str.Contains("M") && !str.Contains("CM"))
                    {
                        if (str.Contains("4"))
                            tb.Rows[i]["sizes_sort2"] = 121;
                        else if (str.Contains("3"))
                            tb.Rows[i]["sizes_sort2"] = 122;
                        else if (str.Contains("2"))
                            tb.Rows[i]["sizes_sort2"] = 123;
                        else if (str.Contains("1"))
                            tb.Rows[i]["sizes_sort2"] = 124;
                        else
                            tb.Rows[i]["sizes_sort2"] = 125;

                        tb.Rows[i]["sizes_sort"] = 99999;
                    }
                    else if (str.Contains("L") && !str.Contains("ML"))
                    {
                        if (str.Contains("4"))
                            tb.Rows[i]["sizes_sort2"] = 131;
                        else if (str.Contains("3"))
                            tb.Rows[i]["sizes_sort2"] = 132;
                        else if (str.Contains("2"))
                            tb.Rows[i]["sizes_sort2"] = 133;
                        else if (str.Contains("1"))
                            tb.Rows[i]["sizes_sort2"] = 134;
                        else
                            tb.Rows[i]["sizes_sort2"] = 135;

                        tb.Rows[i]["sizes_sort"] = 99999;
                    }
                    else if (str.Contains("대"))
                        tb.Rows[i]["sizes_sort2"] = 1;
                    else if (str.Contains("중"))
                        tb.Rows[i]["sizes_sort2"] = 2;
                    else if (str.Contains("소"))
                        tb.Rows[i]["sizes_sort2"] = 3;
                }

                DataView tv = new DataView(tb);

                tv.Sort = sortStr + ", product_sort, product, sizes_sort, sizes";
                tb = tv.ToTable();

                //재출력
                dgvProduct.Rows.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    //Row 추가
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["category1"].Value = tb.Rows[i]["category1"].ToString();
                    dgvProduct.Rows[n].Cells["category2"].Value = tb.Rows[i]["category2"].ToString();
                    dgvProduct.Rows[n].Cells["category3"].Value = tb.Rows[i]["category3"].ToString();
                    dgvProduct.Rows[n].Cells["category"].Value = tb.Rows[i]["category"].ToString();
                    dgvProduct.Rows[n].Cells["category_code"].Value = tb.Rows[i]["category_code"].ToString();
                    dgvProduct.Rows[n].Cells["product_code"].Value = tb.Rows[i]["product_code"].ToString();
                    dgvProduct.Rows[n].Cells["product"].Value = tb.Rows[i]["product"].ToString();
                    dgvProduct.Rows[n].Cells["origin_code"].Value = tb.Rows[i]["origin_code"].ToString();
                    dgvProduct.Rows[n].Cells["origin"].Value = tb.Rows[i]["origin"].ToString();
                    dgvProduct.Rows[n].Cells["weight"].Value = tb.Rows[i]["weight"].ToString();
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = tb.Rows[i]["sizes_code"].ToString();
                    dgvProduct.Rows[n].Cells["sizes"].Value = tb.Rows[i]["sizes"].ToString();
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = tb.Rows[i]["purchase_price"].ToString();
                    dgvProduct.Rows[n].Cells["sales_price"].Value = tb.Rows[i]["sales_price"].ToString();
                    dgvProduct.Rows[n].Cells["unit"].Value = tb.Rows[i]["unit"].ToString();
                    dgvProduct.Rows[n].Cells["price_unit"].Value = tb.Rows[i]["price_unit"].ToString();
                    dgvProduct.Rows[n].Cells["unit_count"].Value = tb.Rows[i]["unit_count"].ToString();
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = tb.Rows[i]["seaover_unit"].ToString();
                    dgvProduct.Rows[n].Cells["division"].Value = tb.Rows[i]["division"].ToString();
                    dgvProduct.Rows[n].Cells["page"].Value = tb.Rows[i]["page"].ToString();
                    dgvProduct.Rows[n].Cells["cnt"].Value = tb.Rows[i]["cnt"].ToString();
                    dgvProduct.Rows[n].Cells["rows"].Value = tb.Rows[i]["rows"].ToString();
                    dgvProduct.Rows[n].Cells["accent"].Value = tb.Rows[i]["accent"].ToString();
                    dgvProduct.Rows[n].Cells["edit_date"].Value = tb.Rows[i]["edit_date"].ToString();
                    dgvProduct.Rows[n].Cells["manager1"].Value = tb.Rows[i]["manager1"].ToString();

                    //영억
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[n].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");
                    dgvProduct.Rows[n].Cells["area"].Value = tb.Rows[i]["area"].ToString();
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private string SetsortStr(string str)
        {
            string results = "";
            if (str == "대분류")
            {
                results = "category";
            }
            else if (str == "코드")
            {
                results = "category1, category2, category3";
            }
            else if (str == "품명")
            {
                results = "product_sort, product";
            }
            else if (str == "원산지")
            {
                results = "origin";
            }
            else if (str == "중량")
            {
                results = "weight";
            }
            else if (str == "사이즈")
            {
                results = "sizes_sort, sizes_sort2, sizes";
            }
            else if (str == "대분류(고정)")
            {
                results = "category_hold";
            }

            return results;
        }

        private void SelectList()
        {
            //SortSetting();
            string category = txtCategory.Text.Trim();
            string product = txtProduct.Text.Trim();
            string origin = txtOrigin.Text.Trim();
            string weight = txtWeight.Text.Trim();
            string sizes = txtSizes.Text.Trim();

            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1)
            {
                //DataTable dt = Libs.Tools.Common.GetDataGridViewAsDataTable(dgv);
                if (string.IsNullOrEmpty(category)
                        && string.IsNullOrEmpty(product)
                        && string.IsNullOrEmpty(origin)
                        && string.IsNullOrEmpty(weight)
                        && string.IsNullOrEmpty(sizes))
                {
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        dgv.Rows[i].Visible = true;
                }
                else
                {
                    for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        dgv.Rows[i].Visible = true;


                    if (!string.IsNullOrEmpty(category))
                    {
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            if (!dgv.Rows[i].Cells["category"].Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Contains(category))
                                dgv.Rows[i].Visible = false;
                        }
                    }

                    if (!string.IsNullOrEmpty(product))
                    {
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            if (!dgv.Rows[i].Cells["product"].Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Contains(product))
                                dgv.Rows[i].Visible = false;
                        }
                    }

                    if (!string.IsNullOrEmpty(origin))
                    {
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            if (!dgv.Rows[i].Cells["origin"].Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Contains(origin))
                                dgv.Rows[i].Visible = false;
                        }
                    }

                    if (!string.IsNullOrEmpty(weight))
                    {
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            if (!dgv.Rows[i].Cells["weight"].Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Contains(weight))
                                dgv.Rows[i].Visible = false;
                        }
                    }

                    if (!string.IsNullOrEmpty(sizes))
                    {
                        for (int i = 0; i < dgv.Rows.Count - 1; i++)
                        {
                            if (!dgv.Rows[i].Cells["sizes"].Value.ToString().Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Contains(sizes))
                                dgv.Rows[i].Visible = false;
                        }
                    }

                }    
            }
        }
        #endregion
        
        #region 인쇄/팩스
        // 프린트 핸들러
        private void PreviewPage()
        {
            printDoc.PrinterSettings = prtSettings;
            printDoc.DefaultPageSettings = pgSettings;
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            dlg.Document = printDoc;
            dlg.ShowDialog();
        }
        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            String textToPrint = ".Net Printing is Easy";
            Font printFont = new Font("Courier New", 40);
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;
            float width = e.PageSettings.PaperSize.Width;
            float height = e.PageSettings.PaperSize.Height;

            // 이미지는 각자 경로로 바꿔주십시오~
            Bitmap bmp = new Bitmap(pnForm.Width, pnForm.Height);
            pnForm.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, pnForm.Width, pnForm.Height));

            Image img = bmp;
            e.Graphics.DrawImage(img, 0, 0, (int)width, (int)height);
            e.Graphics.DrawString(textToPrint, printFont, Brushes.Red, 50, 100);
        }
        private void printPreviewBtn_Click(object sender, EventArgs e)
        {
            printDoc.PrinterSettings = prtSettings;
            printDoc.DefaultPageSettings = pgSettings;
            PrintPreviewDialog dlg = new PrintPreviewDialog();
            printDoc.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
            dlg.Document = printDoc;
            dlg.ShowDialog();
        }

        #endregion

        #region BackGroundWorker
        private void bgwExcel_DoWork(object sender, DoWorkEventArgs e)
        {
            if (rbOneline.Checked)
                excelOneFormCreate();
            else
                excelCreate();
        }
        #endregion

        #region 로딩 효과
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

        #region MouseClick, KeyDown, Changed Event
        private void txtTotalPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnAutoSetting.PerformClick();
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
        private void txtCurPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }
        private void rbTwoline_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTwoline.Checked)
            {
                SettingForm();
            }
        }


        private void rbOneline_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOneline.Checked)
            {
                SettingForm();
            }
        }

        private void txtCurPage_TextChanged(object sender, EventArgs e)
        {
            lbCurPage.Text = txtCurPage.Text;
        }
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 5)
            {
                DataGridView dgv = dgvProduct;
                DataGridViewRow row = dgv.Rows[e.RowIndex];
                DataGridViewCell cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                int sPrice = 0;
                if (cell.Value.ToString() == "-")
                    sPrice = -1;
                else if (cell.Value.ToString() == "문의")
                    sPrice = -2;
                else if (cell.Value.ToString().Contains("통관예정"))
                    sPrice = -3;

                if (sPrice < 0)
                {
                    if (MessageBox.Show(this, "모든 취급품목서에 반영하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        FormDataModel model = new FormDataModel();
                        model.category_code = row.Cells["category_code"].Value.ToString();
                        model.product_code = row.Cells["product_code"].Value.ToString();
                        model.origin_code = row.Cells["origin_code"].Value.ToString();
                        model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                        model.unit = row.Cells["unit"].Value.ToString();
                        model.price_unit = row.Cells["price_unit"].Value.ToString();
                        model.unit_count = row.Cells["unit_count"].Value.ToString();
                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                        model.sales_price = sPrice;

                        int results = seaoverRepository.UpdateExculde(model);
                        if (results == -1)
                        {
                            MessageBox.Show(this, "모든 품목서에 반영중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                    }
                }

            }
        }
        private void btnSort_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                label3.Focus();
            }
        }

        private void txtManager1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Tab)
            {
                label2.Focus();
            }
        }
        private void txtFormname1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetBookmark();
            }
        }

        private void txtManager1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetBookmark();
            }
        }
        private void txtFormname2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetTitleList();
            }
        }

        private void txtManager2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetTitleList();
            }
        }
        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectList();
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectList();
            }
        }

        private void txtOrigin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectList();
            }
        }

        private void txtWeight_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectList();
            }
        }

        private void txtSizes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectList();
            }
        }
        

        //즐겨찾기 클릭
        private void lvBookmark_MouseClick(object sender, MouseEventArgs e)
        {
            //품목서 클릭
            if (e.Button.Equals(MouseButtons.Left))
            {
                if (lvBookmark.SelectedItems.Count != 0)
                {
                    int idx = lvBookmark.SelectedItems[0].Index;
                    int form_type = Convert.ToInt32(lvBookmark.Items[idx].SubItems[4].Text);
                    int id = Convert.ToInt32(lvBookmark.Items[idx].SubItems[0].Text);
                    //초밥품목서
                    if (form_type == 3)
                    {
                        if (MessageBox.Show(this,"초밥품목서입니다. 초밥품목서(한줄) 폼으로 열겠습니까?\n  *예 : 초밥품목서(한줄)  * 아니오 : 일반품목서(두줄)", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            OneLineForm olf = new OneLineForm(um);
                            olf.InputFormData(id);
                            olf.Show();
                        }
                        else
                        {
                            lbId.Text = id.ToString();
                            txtFormName.Text = lvBookmark.Items[idx].SubItems[1].Text;
                            GetFormData(idx, id.ToString());
                            GetFormDataTemp(Convert.ToInt32(id));
                        }
                    }
                    //일반품목서
                    else
                    {
                        lbId.Text = id.ToString();
                        txtFormName.Text = lvBookmark.Items[idx].SubItems[1].Text;
                        GetFormData(idx, id.ToString());
                        GetFormDataTemp(Convert.ToInt32(id));
                    }
                }
            }
            //품목서 우클릭클릭
            else if (e.Button.Equals(MouseButtons.Right))
            {
                //선택된 아이템의 Text를 저장해 놓습니다. 중요한 부분.
                string selectedNickname = lvBookmark.GetItemAt(e.X, e.Y).Text;

                
                ContextMenu m = new ContextMenu();

                //item1
                MenuItem m1 = new MenuItem();
                m1.Text = "즐겨찾기 삭제";
                m1.Click += (senders, es) =>
                {
                    DeleteBookmark();
                };
                m.MenuItems.Add(m1);
                //item2
                int idx = lvBookmark.SelectedItems[0].Index;
                int id = Convert.ToInt32(lvBookmark.Items[idx].SubItems[0].Text);
                MenuItem m2 = new MenuItem();
                m2.Text = "현재 품목서에서 공통 품목만 강조";
                m2.Click += (senders, es) =>
                {
                    CommonProductAccent(id.ToString());

                };
                m.MenuItems.Add(m2);

                //현재 마우스가 위치한 장소에 메뉴를 띄워줍니다
                m.Show(lvBookmark, new Point(e.X, e.Y));
            }
        }


        private void CommonProductAccent(string id)
        {
            if (lvBookmark.SelectedItems.Count != 0 && dgvProduct.Rows.Count > 0)
            {
                List<FormDataModel> formDataModelList = seaoverRepository.GetFormData("", id);
                if (formDataModelList != null && formDataModelList.Count > 0)
                {
                    foreach (DataGridViewRow row in dgvProduct.Rows)
                    {
                        foreach (FormDataModel model in formDataModelList)
                        {
                            if (row.Cells["product_code"].Value != null
                                && row.Cells["origin_code"].Value != null
                                && row.Cells["sizes_code"].Value != null
                                && row.Cells["seaover_unit"].Value != null)
                            {
                                //같은품목
                                if (!(row.Cells["product_code"].Value.ToString().Equals(model.product_code)
                                && row.Cells["origin_code"].Value.ToString().Equals(model.origin_code)
                                && row.Cells["sizes_code"].Value.ToString().Equals(model.sizes_code)
                                && row.Cells["seaover_unit"].Value.ToString().Equals(model.seaover_unit)))
                                    row.Cells["accent2"].Value = -1;
                                else
                                {
                                    row.Cells["accent2"].Value = 0;
                                    break;
                                }
                            }
                        }
                    }
                }

                SettingForm();
            }
        }


        private void DeleteBookmark()
        {
            if (lvBookmark.SelectedItems.Count != 0)
            {
                BookmarkModel model = new BookmarkModel();
                model.user_id = um.user_id;
                model.form_id = Convert.ToInt32(lvBookmark.SelectedItems[0].SubItems[0].Text);
                model.form_type = Convert.ToInt32(lvBookmark.SelectedItems[0].SubItems[4].Text); ;
                model.is_notification = Convert.ToBoolean(lvBookmark.SelectedItems[0].SubItems[3].Text);


                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = bookmarkRepository.DeleteSql(model);
                sqlList.Add(sql);

                int results = bookmarkRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    GetBookmark();
                }
            }
            else
            {
                MessageBox.Show(this, "품목서를 선택해주세요.");
                this.Activate();
            }
        }

        //품목서 ListView 클릭
        private void lvFormList_MouseClick(object sender, MouseEventArgs e)
        {
            //품목서 클릭
            if (e.Button.Equals(MouseButtons.Left))
            {
                if (lvFormList.SelectedItems.Count != 0)
                {
                    int idx = lvFormList.SelectedItems[0].Index;
                    string id = lvFormList.Items[idx].SubItems[0].Text;
                    GetFormData(idx, id);
                    GetFormDataTemp(Convert.ToInt32(id));
                }
            }
            //품목서 우클릭클릭
            else if (e.Button.Equals(MouseButtons.Right))
            {
                //선택된 아이템의 Text를 저장해 놓습니다. 중요한 부분.
                string selectedNickname = lvFormList.GetItemAt(e.X, e.Y).Text;

                //오른쪽 메뉴를 만듭니다
                ContextMenu m = new ContextMenu();

                //메뉴에 들어갈 아이템을 만듭니다
                MenuItem m1 = new MenuItem();
                m1.Text = "공통 즐겨찾기 추가";
                m1.Click += (senders, es) =>
                {
                    SetBookmark(true);
                };
                m.MenuItems.Add(m1);
                MenuItem m2 = new MenuItem();
                m2.Text = "나만의 즐겨찾기 추가";
                //각 메뉴를 선택했을 때의 이벤트 핸들러를 작성합니다. 람다식을 이용해 작성하는것이 해법입니다.
                //검색 넣어줌
                m2.Click += (senders, es) =>
                {  
                    SetBookmark();
                };
                //메뉴에 메뉴 아이템을 등록해줍니다
                
                m.MenuItems.Add(m2);

                //item2
                int idx = lvFormList.SelectedItems[0].Index;
                string id = lvFormList.Items[idx].SubItems[0].Text;
                MenuItem m3 = new MenuItem();
                m3.Text = "현재 품목서에서 공통 품목만 강조";
                m3.Click += (senders, es) =>
                {
                    CommonProductAccent(id);
                };
                m.MenuItems.Add(m3);

                //현재 마우스가 위치한 장소에 메뉴를 띄워줍니다
                m.Show(lvFormList, new Point(e.X, e.Y));
            }
        }

        private void SetBookmark(bool is_notification = false)
        {
            if (lvFormList.SelectedItems.Count != 0)
            {
                BookmarkModel model = new BookmarkModel();
                model.user_id = um.user_id;
                model.form_id = Convert.ToInt32(lvFormList.SelectedItems[0].SubItems[0].Text);
                model.form_type = 2;
                model.is_notification = is_notification;

                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = bookmarkRepository.DeleteSql(model);
                sqlList.Add(sql);
                sql = bookmarkRepository.InsertSql(model);
                sqlList.Add(sql);

                int results = bookmarkRepository.UpdateTran(sqlList);
                if (results == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    GetBookmark();
                }
            }
            else
            {
                MessageBox.Show(this, "품목서를 선택해주세요.");
                this.Activate();
            }
        }
        private void GetBookmark()
        {
            lvBookmark.Items.Clear();

            List<BookmarkModel> model = new List<BookmarkModel>();
            model = bookmarkRepository.GetBookmark(um.user_id, 2, txtFormname1.Text, txtManager1.Text);

            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = model[i].form_id.ToString();
                    lvi.SubItems.Add(model[i].form_name.ToString());
                    lvi.SubItems.Add(model[i].edit_user.ToString());
                    lvi.SubItems.Add(model[i].is_notification.ToString());
                    lvi.SubItems.Add(model[i].form_type.ToString());
                    if (model[i].is_notification)
                    {
                        lvi.Font = new Font("굴림", 9, FontStyle.Bold);
                        lvi.ForeColor = Color.Blue;
                    }
                        

                    lvBookmark.Items.Add(lvi);
                }
            }
        }

        private void _2LineForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        {
                            UpdateForm();
                            break;
                        }
                    case Keys.A:
                        {
                            InsertForm();
                            break;
                        }
                    case Keys.N:
                        {
                            txtCategory.Text = "";
                            txtProduct.Text = "";
                            txtOrigin.Text = "";
                            txtWeight.Text = "";
                            txtSizes.Text = "";
                            SelectList();
                            txtProduct.Focus();
                            break;
                        }
                    case Keys.M:
                        {
                            txtProduct.Focus();
                            break;
                        }
                    case Keys.W:
                        {
                            btnDefault.PerformClick();
                            break;
                        }
                    case Keys.Q:
                        {
                            btnReturn.PerformClick();
                            break;
                        }
                    case Keys.E:
                        {
                            ExculedProduct();
                            break;
                        }
                    case Keys.R:
                        {
                            PriceUpdate();
                            dgvProduct.SelectAll();
                            dgvProduct.EndEdit();
                            Clipboard.SetText(GetHandlingProductTxt());
                            break;
                        }
                    case Keys.C:
                        btnPreview.PerformClick();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.E:
                        btnPreview.PerformClick();
                        break;
                    case Keys.P:
                        btnPrint.PerformClick();
                        break;
                    case Keys.F:
                        TextFinder tf = new TextFinder(this);
                        tf.Owner = this;
                        tf.Show();
                        break;
                }
            }
            /*else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Copy();
                        break;
                    case Keys.V:
                        Paste();
                        break;
                    case Keys.X:
                        CopyAndDelete();
                        break;
                }
            }*/
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        {
                            rbOneline.Checked = true;
                            break;
                        }
                    case Keys.F2:
                        {
                            rbTwoline.Checked = true;
                            break;
                        }
                    case Keys.F3:
                        {
                            int p = Convert.ToInt16(txtCurPage.Text);
                            if (rbOneline.Checked)
                            {
                                PageMananerOpen(p, "C");
                            }
                            else
                            {
                                PageMananerOpen(p, "L");
                            }
                            break;
                        }
                    case Keys.F4:
                        {
                            FormCollection fc = Application.OpenForms;
                            bool isFormActive = false;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "GetProductList")
                                {
                                    isFormActive = true;
                                    frm.WindowState = FormWindowState.Maximized;
                                    frm.Activate();
                                }
                            }

                            if (!isFormActive)
                            {
                                GetProductList pl = new GetProductList(um, this);
                                pl.StartPosition = FormStartPosition.Manual;
                                pl.Show();
                            }
                            break;
                        }
                    case Keys.F5:
                        Refresh();
                        break;
                    case Keys.F6:
                        btnAutoSetting.PerformClick();
                        break;
                    case Keys.F7:
                        btnRowRefresh.PerformClick();
                        break;
                }
            }

        }
        #endregion

        #region 기타 Event
        private void _2LineForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (processingFlag)
            {
                e.Cancel = true;
            }
        }
        #endregion

        #region Print 
        int count = 0;
        int pageNo = 1;
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "취급품목서", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            //유효성 검사
            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this, "출력할 내역이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                for (int i = 0; i < dgvProduct.Rows.Count - 1; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    if (row.Cells["page"].Value == null && row.Cells["area"].Value == null)
                    {
                        MessageBox.Show(this, "양식새로 고침후 다시 해주시기 바랍니다.");
                        this.Activate();
                        return;
                    }
                }
            }
            try
            {
                //재설정
                btnReturn.PerformClick();
                //프린터 설정
                count = 0;
                pageNo = 1;
                this.printDocument1 = new PrintDocument();
                if (rbTwoline.Checked)
                    this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(prtDoc_TwoLine_PrintPage);
                else if (rbOneline.Checked)
                    this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(prtDoc_OneLine_PrintPage);
                int pages = common.GetPageCount(printDocument1);
                //미리보고 폼
                Common.PrintManager.PrintManager pm = new Common.PrintManager.PrintManager(this, printDocument1, pages);
                pm.Show();
            }
            catch
            { }
        }
        //초기화
        public void InitVariable()
        {
            count = 0;
            pageNo = 1;
        }

        //두줄품목서 양식만들기
        private void prtDoc_TwoLine_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridView dgv = this.dgvProduct;

            int dialogHeight = printDocument1.DefaultPageSettings.PaperSize.Height - 60;    //페이지 전체넓이 printPreivew.Width  (가로모드는 반대로)
            int dialogWidth = printDocument1.DefaultPageSettings.PaperSize.Width - 61;           //페이지 전체넓이 printPreivew.Height  (가로모드는 반대로)

            StringFormat sf = new StringFormat();  //컬럼안에 있는 값들 가운데로 정렬하기 위해서.
            sf.Alignment = StringAlignment.Center;
            //Header
            string top_str = $"(광고) ㈜아토무역 TEL : 051-256-3100 , FAX : 051-256-4100  부산광역시 서구 원양로 310     발신일자 : {DateTime.Now.ToString("yyyy-MM-dd")}   수신인: 구매담당자님 귀하";
            e.Graphics.DrawString(top_str, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 20, 20);
            //Title
            Label lb = new Label();
            lb.Font = new Font("Arial", 16, FontStyle.Bold);
            Graphics g = lb.CreateGraphics();
            int txt_width = (int)g.MeasureString(txtFormName.Text, new Font("Arial", 20, FontStyle.Bold)).Width;
            e.Graphics.DrawString(txtFormName.Text + "   (" + pageNo + " / " + TotalPage + ")" , new Font("Arial", 20, FontStyle.Bold), Brushes.Black, (dialogWidth / 2) - (txt_width / 2), 45);
            //Footer
            string foot_str = "'광고수신을 원하지 않는 경우 무료전화 080-855-8825 로 연락주시기 바랍니다.'";
            e.Graphics.DrawString(foot_str, new Font("Arial", 8), Brushes.Black, 20, dialogHeight + 5);

            //전체테두리
            RectangleF dr = new RectangleF(20, 40, dialogWidth, dialogHeight);
            e.Graphics.DrawRectangle(Pens.Black, 20, 40, dialogWidth, dialogHeight - 40);
            e.Graphics.DrawString("", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, dr, sf);
            //담당자 + 품목서 비고
            dr = new RectangleF(20, 90, dialogWidth, 80);
            e.Graphics.DrawRectangle(Pens.Black, 20, 80, dialogWidth, 40);
            string manager = lbRemark.Text;
            Font f = AutoFontSize(lbRemark, manager);        //글자길이에 맞는 폰트
            e.Graphics.DrawString(manager, f, Brushes.Black, dr, sf);


            //품목정보 타이틀===========================================================================
            //Left
            Font font = new Font("Arial", 8, FontStyle.Regular);
            int col_width = 30;
            int str_width = 20;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("구분", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 90;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("품명", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 33;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("산지", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 70;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("중량", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 80;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("사이즈", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 80;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("단가", font, Brushes.Black, dr, sf);
            //Right
            str_width += col_width;
            col_width = 30;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("구분", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 90;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("품명", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 33;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("산지", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 70;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("중량", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 80;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("사이즈", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 80;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("단가", font, Brushes.Black, dr, sf);
            //=======================================================================================
            //column 순회
            sf.LineAlignment = StringAlignment.Center;
            Font bold_font = new Font("Arial", 7, FontStyle.Bold);
            Font regular_font = new Font("Arial", 7, FontStyle.Regular);
            int i, j;            
            int row_cnt;
            float height = ((float)dialogHeight - 140) / 60;              //셀 하나의 높이 
            //Left area=================================================================
            //  1.Category
            col_width = 30;                 //셀 하나의 넓이
            str_width = 20;                 //셀 시작지점
            float str_height = 140;         //셀 시작높이
            float total_cell_height = 0;    //셀 총 높이
            float cell_height;              //하나의 내역셀 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1 ; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);   //행수
                cell_height = height * row_cnt;                       //해당셀의 높이
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        string val = row.Cells["category"].Value.ToString();
                        if(val == null)
                            val = "";
                        val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        int valLen = val.Length;
                        string category;
                        //5행이하, 6글자 이상
                        if (valLen > 5 & total_cell_height <= height * 5)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //5행이하, 6글자 이상
                        else if (valLen > 1 & total_cell_height <= height * 2)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //6행이상, 6글자 이상
                        else
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((stt_idx + 1) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 1);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                }

                                stt_idx += 1;
                            }
                            category = tempStr;
                        }
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {
                        total_cell_height += cell_height;
                        if (row.Cells["category"].Value.ToString() != next_row.Cells["category"].Value.ToString())
                        {
                            string val = row.Cells["category"].Value.ToString();
                            if(val == null)
                                val = "";
                            val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                            int valLen = val.Length;
                            string category;
                            //5행이하, 6글자 이상
                            if (valLen > 5 & total_cell_height <= height * 5)
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((k + 1) % 2 == 0)
                                    {
                                        if ((stt_idx + 2) == valLen)
                                        {
                                            tempStr += val.Substring(stt_idx, 2);
                                        }
                                        else
                                        {
                                            tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                        }
                                        stt_idx += 2;
                                    }
                                }
                                category = tempStr;
                            }
                            //5행이하, 6글자 이상
                            else if (valLen > 1 & total_cell_height <= height * 2)
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((k + 1) % 2 == 0)
                                    {
                                        if ((stt_idx + 2) == valLen)
                                        {
                                            tempStr += val.Substring(stt_idx, 2);
                                        }
                                        else
                                        {
                                            tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                        }
                                        stt_idx += 2;
                                    }
                                }
                                category = tempStr;
                            }
                            //6행이상, 6글자 이상
                            else
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((stt_idx + 1) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 1);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                    }

                                    stt_idx += 1;
                                }
                                category = tempStr;
                            }

                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);
                            //e.Graphics.DrawString(row.Cells["category"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;

                    string val = row.Cells["category"].Value.ToString();
                    if(val == null)
                        val = "";
                    val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                    int valLen = val.Length;
                    string category;
                    //5행이하, 6글자 이상
                    if (valLen > 5 & total_cell_height <= height * 5)
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((k + 1) % 2 == 0)
                            {
                                if ((stt_idx + 2) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 2);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                }
                                stt_idx += 2;
                            }
                        }
                        category = tempStr;
                    }
                    //5행이하, 6글자 이상
                    else if (valLen > 1 & total_cell_height <= height * 2)
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((k + 1) % 2 == 0)
                            {
                                if ((stt_idx + 2) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 2);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                }
                                stt_idx += 2;
                            }
                        }
                        category = tempStr;
                    }
                    //6행이상, 6글자 이상
                    else
                    {
                        int stt_idx = 0;
                        string tempStr = "";
                        for (int k = 0; k < valLen; k++)
                        {
                            if ((stt_idx + 1) == valLen)
                            {
                                tempStr += val.Substring(stt_idx, 1);
                            }
                            else
                            {
                                tempStr += val.Substring(stt_idx, 1) + "\r\n";
                            }

                            stt_idx += 1;
                        }
                        category = tempStr;
                    }
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //  2.Product
            str_width += col_width;         //셀 시작지점
            col_width = 90;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["product"].Value.ToString() != next_row.Cells["product"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //  3.Origin
            str_width += col_width;         //셀 시작지점
            col_width = 33;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;

                string origin = row.Cells["origin"].Value.ToString().Trim();
                if (origin.Length == 5)
                {
                    origin = origin.Substring(0, 4);
                }

                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        if (origin.Length == 3)
                            regular_font = new Font("Arial", 6, FontStyle.Regular);

                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                        regular_font = new Font("Arial", 7, FontStyle.Regular);
                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {
                        total_cell_height += cell_height;
                        if (row.Cells["origin"].Value.ToString() != next_row.Cells["origin"].Value.ToString())
                        {
                            if (origin.Length == 3)
                                regular_font = new Font("Arial", 6, FontStyle.Regular);

                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                            regular_font = new Font("Arial", 7, FontStyle.Regular);
                        }
                    }
                }
                else
                {
                    if (origin.Length >= 3)
                        regular_font = new Font("Arial", 6, FontStyle.Regular);

                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);

                    regular_font = new Font("Arial", 7, FontStyle.Regular);
                }
            }
            //  4.Weight
            str_width += col_width;         //셀 시작지점
            col_width = 70;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["weight"].Value.ToString() != next_row.Cells["weight"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  5.Sizes
            str_width += col_width;         //셀 시작지점
            col_width = 80;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sizes"].Value.ToString() != next_row.Cells["sizes"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  6.Sales_price
            str_width += col_width;         //셀 시작지점
            col_width = 80;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;

                double price;
                string txt_price = "";
                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out price))
                {
                    txt_price = row.Cells["sales_price"].Value.ToString();
                }
                else
                {
                    if (row.Cells["price_unit"].Value.ToString() == "팩")
                        txt_price = price.ToString("#,##0") + " /p";
                    else if (row.Cells["sales_price"].Value.ToString() == "kg" || row.Cells["sales_price"].Value.ToString() == "Kg" || row.Cells["sales_price"].Value.ToString() == "KG")
                        txt_price = price.ToString("#,##0") + " /k";
                    else
                        txt_price = price.ToString("#,##0");
                }
                //출력=====================================================================================================================
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == pageNo.ToString() && next_row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sales_price"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "L")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sales_price"].Value.ToString() != next_row.Cells["sales_price"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //Right area=================================================================
            //  1.Category
            col_width = 30;                 //셀 하나의 넓이
            str_width = 403;                //셀 시작지점
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        string val = row.Cells["category"].Value.ToString();
                        if(val == null)
                            val = "";
                        val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        int valLen = val.Length;
                        string category;
                        //5행이하, 6글자 이상
                        if (valLen > 5 & total_cell_height <= height * 5)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //5행이하, 6글자 이상
                        else if (valLen > 1 & total_cell_height <= height * 2)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //6행이상, 6글자 이상
                        else
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((stt_idx + 1) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 1);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                }

                                stt_idx += 1;
                            }
                            category = tempStr;
                        }

                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["category"].Value.ToString() != next_row.Cells["category"].Value.ToString())
                        {
                            string val = row.Cells["category"].Value.ToString();
                            if(val == null)
                                val = "";
                            val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                            int valLen = val.Length;
                            string category;
                            //5행이하, 6글자 이상
                            if (valLen > 5 & total_cell_height <= height * 5)
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((k + 1) % 2 == 0)
                                    {
                                        if ((stt_idx + 2) == valLen)
                                        {
                                            tempStr += val.Substring(stt_idx, 2);
                                        }
                                        else
                                        {
                                            tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                        }
                                        stt_idx += 2;
                                    }
                                }
                                category = tempStr;
                            }
                            //5행이하, 6글자 이상
                            else if (valLen > 1 & total_cell_height <= height * 2)
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((k + 1) % 2 == 0)
                                    {
                                        if ((stt_idx + 2) == valLen)
                                        {
                                            tempStr += val.Substring(stt_idx, 2);
                                        }
                                        else
                                        {
                                            tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                        }
                                        stt_idx += 2;
                                    }
                                }
                                category = tempStr;
                            }
                            //6행이상, 6글자 이상
                            else
                            {
                                int stt_idx = 0;
                                string tempStr = "";
                                for (int k = 0; k < valLen; k++)
                                {
                                    if ((stt_idx + 1) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 1);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                    }

                                    stt_idx += 1;
                                }
                                category = tempStr;
                            }
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        string val = row.Cells["category"].Value.ToString();
                        if(val == null)
                            val = "";
                        val = val.Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        int valLen = val.Length;
                        string category;
                        //5행이하, 6글자 이상
                        if (valLen > 5 & total_cell_height <= height * 5)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //5행이하, 6글자 이상
                        else if (valLen > 1 & total_cell_height <= height * 2)
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((k + 1) % 2 == 0)
                                {
                                    if ((stt_idx + 2) == valLen)
                                    {
                                        tempStr += val.Substring(stt_idx, 2);
                                    }
                                    else
                                    {
                                        tempStr += val.Substring(stt_idx, 2) + "\r\n";
                                    }
                                    stt_idx += 2;
                                }
                            }
                            category = tempStr;
                        }
                        //6행이상, 6글자 이상
                        else
                        {
                            int stt_idx = 0;
                            string tempStr = "";
                            for (int k = 0; k < valLen; k++)
                            {
                                if ((stt_idx + 1) == valLen)
                                {
                                    tempStr += val.Substring(stt_idx, 1);
                                }
                                else
                                {
                                    tempStr += val.Substring(stt_idx, 1) + "\r\n";
                                }

                                stt_idx += 1;
                            }
                            category = tempStr;
                        }
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(category, bold_font, Brushes.Black, drawRect, sf);
                    }
                }
            }
            //  2.Product
            str_width += col_width;         //셀 시작지점
            col_width = 90;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["product"].Value.ToString() != next_row.Cells["product"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                    }
                }
            }
            //  3.Origin
            str_width += col_width;         //셀 시작지점
            col_width = 33;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;

                string origin = row.Cells["origin"].Value.ToString().Trim();
                if (origin.Length == 5)
                {
                    origin = origin.Substring(0, 4);
                }

                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        if (origin.Length == 3)
                            regular_font = new Font("Arial", 6, FontStyle.Regular);
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);

                        regular_font = new Font("Arial", 7, FontStyle.Regular);
                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["origin"].Value.ToString() != next_row.Cells["origin"].Value.ToString())
                        {
                            if (origin.Length == 3)
                                regular_font = new Font("Arial", 6, FontStyle.Regular);
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;

                            regular_font = new Font("Arial", 7, FontStyle.Regular);
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        if (origin.Length == 3)
                            regular_font = new Font("Arial", 6, FontStyle.Regular);
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);

                        regular_font = new Font("Arial", 7, FontStyle.Regular);
                    }
                }
            }
            //  4.Weight
            str_width += col_width;         //셀 시작지점
            col_width = 70;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["weight"].Value.ToString() != next_row.Cells["weight"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                    }
                }
            }
            //  5.Sizes
            str_width += col_width;         //셀 시작지점
            col_width = 80;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sizes"].Value.ToString() != next_row.Cells["sizes"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                    }
                }
            }
            //  6.Sales Price
            str_width += col_width;         //셀 시작지점
            col_width = 80;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;

                double price;
                string txt_price = "";
                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out price))
                {
                    txt_price = row.Cells["sales_price"].Value.ToString();
                }
                else
                {
                    if (row.Cells["price_unit"].Value.ToString() == "팩")
                        txt_price = price.ToString("#,##0") + " /p";
                    else if (row.Cells["sales_price"].Value.ToString() == "kg" || row.Cells["sales_price"].Value.ToString() == "Kg" || row.Cells["sales_price"].Value.ToString() == "KG")
                        txt_price = price.ToString("#,##0") + " /k";
                    else
                        txt_price = price.ToString("#,##0");
                }

                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sales_price"].Value.ToString() != next_row.Cells["sales_price"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "R")
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(txt_price, bold_font, Brushes.Black, drawRect, sf);
                    }
                }
            }



            //==========================================================================
            //Page++
            if (pageNo < TotalPage)
            { 
                e.HasMorePages = true;
                pageNo++;
                return;
            }
        }
        //한줄품목서 양식만들기
        private void prtDoc_OneLine_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridView dgv = this.dgvProduct;

            int dialogHeight = printDocument1.DefaultPageSettings.PaperSize.Height - 60;    //페이지 전체넓이 printPreivew.Width  (가로모드는 반대로)
            int dialogWidth = printDocument1.DefaultPageSettings.PaperSize.Width - 61;           //페이지 전체넓이 printPreivew.Height  (가로모드는 반대로)

            StringFormat sf = new StringFormat();  //컬럼안에 있는 값들 가운데로 정렬하기 위해서.
            sf.Alignment = StringAlignment.Center;
            //Header
            string top_str = $"(광고) ㈜아토무역 TEL : 051-256-3100 , FAX : 051-256-4100  부산광역시 서구 원양로 310     발신일자 : {DateTime.Now.ToString("yyyy-MM-dd")}   수신인: 구매담당자님 귀하";
            e.Graphics.DrawString(top_str, new Font("Arial", 8, FontStyle.Regular), Brushes.Black, 20, 20);
            //Title
            Label lb = new Label();
            lb.Font = new Font("Arial", 16, FontStyle.Bold);
            Graphics g = lb.CreateGraphics();
            int txt_width = (int)g.MeasureString(txtFormName.Text, new Font("Arial", 20, FontStyle.Bold)).Width;
            e.Graphics.DrawString(txtFormName.Text + "   (" + pageNo + " / " + TotalPage + ")", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, (dialogWidth / 2) - (txt_width / 2), 45);
            //Footer
            string foot_str = "'광고수신을 원하지 않는 경우 무료전화 080-855-8825 로 연락주시기 바랍니다.'";
            e.Graphics.DrawString(foot_str, new Font("Arial", 8), Brushes.Black, 20, dialogHeight + 5);

            //전체테두리
            RectangleF dr = new RectangleF(20, 40, dialogWidth, dialogHeight);
            e.Graphics.DrawRectangle(Pens.Black, 20, 40, dialogWidth, dialogHeight - 40);
            e.Graphics.DrawString("", new Font("Arial", 8, FontStyle.Bold), Brushes.Black, dr, sf);
            //담당자 + 품목서 비고
            dr = new RectangleF(20, 90, dialogWidth, 80);
            e.Graphics.DrawRectangle(Pens.Black, 20, 80, dialogWidth, 40);
            string manager = lbRemark.Text;
            Font f = AutoFontSize(lbRemark, manager);        //글자길이에 맞는 폰트
            e.Graphics.DrawString(manager, f, Brushes.Black, dr, sf);


            //품목정보 타이틀===========================================================================
            Font font = new Font("Arial", 8, FontStyle.Regular);
            int col_width = 60;
            int str_width = 20;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("구분", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 180;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("품명", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 66;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("산지", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 140;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("중량", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 160;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("사이즈", font, Brushes.Black, dr, sf);
            str_width += col_width;
            col_width = 160;
            dr = new RectangleF(str_width, 123, col_width, 20);
            e.Graphics.DrawRectangle(Pens.Black, str_width, 120, col_width, 20);
            e.Graphics.DrawString("단가", font, Brushes.Black, dr, sf);
            //=======================================================================================
            //column 순회
            sf.LineAlignment = StringAlignment.Center;
            Font bold_font = new Font("Arial", 8, FontStyle.Bold);
            Font regular_font = new Font("Arial", 7, FontStyle.Regular);
            int i, j;
            int row_cnt;
            float height = ((float)dialogHeight - 140) / 60;              //셀 하나의 높이 
            //  1.Category
            col_width = 60;                 //셀 하나의 넓이
            str_width = 20;                 //셀 시작지점
            float str_height = 140;         //셀 시작높이
            float total_cell_height = 0;    //셀 총 높이
            float cell_height;              //하나의 내역셀 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);   //행수
                cell_height = height * row_cnt;                       //해당셀의 높이
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["category"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {
                        total_cell_height += cell_height;
                        if (row.Cells["category"].Value.ToString() != next_row.Cells["category"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["category"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.FillRectangle(Brushes.LightYellow, drawRect);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["category"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //  2.Product
            str_width += col_width;         //셀 시작지점
            col_width = 180;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["product"].Value.ToString() != next_row.Cells["product"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["product"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //  3.Origin
            str_width += col_width;         //셀 시작지점
            col_width = 66;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                string origin = row.Cells["origin"].Value.ToString().Trim();
                if (origin.Length == 3)
                {
                    origin = origin.Substring(0, 2);
                }
                else if (origin.Length == 5)
                {
                    origin = origin.Substring(0, 4);
                }

                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["origin"].Value.ToString() != next_row.Cells["origin"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(origin, regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  4.Weight
            str_width += col_width;         //셀 시작지점
            col_width = 140;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["weight"].Value.ToString() != next_row.Cells["weight"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["weight"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  5.Sizes
            str_width += col_width;         //셀 시작지점
            col_width = 160;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sizes"].Value.ToString() != next_row.Cells["sizes"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["sizes"].Value.ToString(), regular_font, Brushes.Black, drawRect, sf);
                }
            }
            //  6.Sales_price
            str_width += col_width;         //셀 시작지점
            col_width = 160;                 //셀 하나의 넓이
            str_height = 140;               //셀 시작높이
            total_cell_height = 0;          //셀 총 높이
            for (i = 0; i < dgvProduct.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgv.Rows[i];
                DataGridViewRow next_row = dgv.Rows[i + 1];
                row_cnt = Convert.ToInt16(row.Cells["rows"].Value);
                cell_height = height * row_cnt;
                if (i + 1 < dgvProduct.Rows.Count - 1)
                {
                    if (next_row.Cells["page"].Value.ToString() == (pageNo + 1).ToString())
                    {
                        total_cell_height += cell_height;
                        RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                        e.Graphics.DrawString(row.Cells["sales_price"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);

                        break;
                    }
                    else if (row.Cells["page"].Value.ToString() == pageNo.ToString() && row.Cells["area"].Value.ToString() == "C")
                    {

                        total_cell_height += cell_height;
                        if (row.Cells["sales_price"].Value.ToString() != next_row.Cells["sales_price"].Value.ToString())
                        {
                            RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                            e.Graphics.DrawString(row.Cells["sales_price"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                            str_height += total_cell_height;
                            total_cell_height = 0;
                        }
                    }
                }
                else
                {
                    total_cell_height += cell_height;
                    RectangleF drawRect = new RectangleF(str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawRectangle(Pens.Black, str_width, str_height, col_width, total_cell_height);
                    e.Graphics.DrawString(row.Cells["sales_price"].Value.ToString(), bold_font, Brushes.Black, drawRect, sf);
                }
            }
            //==========================================================================
            //Page++
            if (pageNo < TotalPage)
            {
                e.HasMorePages = true;
                pageNo++;
                return;
            }
        }
        #endregion Method


        #region Remark, Memo Update
        private void lbRemark_DoubleClick(object sender, EventArgs e)
        {
            ManagerSelect ms = new ManagerSelect(um);
            string remark = ms.GetRemark();
            if (remark != null && !string.IsNullOrEmpty(remark))
            {
                Font font = AutoFontSize(lbRemark, remark);
                lbRemark.Text = remark;
                lbRemark.Font = font;
            }
        }
        //자동 폰트 사이즈 정렬
        public Font AutoFontSize(Label label, String text)
        {
            Font ft;
            Graphics gp;
            SizeF sz;
            Single Faktor, FaktorX, FaktorY;

            gp = label.CreateGraphics();
            sz = gp.MeasureString(text, label.Font);

            gp.Dispose();

            FaktorX = (label.Width) / sz.Width;
            FaktorY = (label.Height) / sz.Height;

            if (FaktorX > FaktorY)
            {
                Faktor = FaktorY;
            }
            else
            {
                Faktor = FaktorX;
            }

            ft = label.Font;
            float f = ft.SizeInPoints * (Faktor) - 1;

            if (f < 8)
            {
                f = 8;
            }

            return new Font(ft.Name, f);
        }
        #endregion

        #region ToolTip
        private void lbRemark_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.IsBalloon = true;
            tt.SetToolTip(this.lbRemark, "더블클릭 하시면 내용을 수정할 수 있습니다.");
        }
        private void lbFormName_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.IsBalloon = true;
            tt.SetToolTip(this.lbFormName, "우측상단 '품목서 제목' 입력상자에서 제목을 수정할 수 있습니다.");
        }


        #endregion


        #region Datagridview 복사 붙혀넣기

        /// <summary>
        /// 사용자가 선택한 셀을 잘라냅니다.
        /// </summary>
        public void CopyAndDelete()
        {
            Copy();
            if (dgvProduct.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null)
                            cell.Value = cell.Value.ToString().Replace("\n", @"\n");
                    }
                    dgvProduct.Rows.Remove(row);
                }
            }
            else
            {
                Delete();
            }
            isPasteRow = true;
        }

        /// <summary>
        /// 선택한 셀을 복사합니다.
        /// </summary>
        public void Copy()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            if (dgvProduct.SelectedCells.Count > 0)
            {
                string clipboardTxt = "";
                if (dgvProduct.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        if (dgvProduct.Rows[i].Selected)
                        {
                            clipboardTxt += "\n";
                            string rowTxt = "";
                            for (int j = 0; j < dgvProduct.Columns.Count; j++)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Value == null)
                                    dgvProduct.Rows[i].Cells[j].Value = string.Empty;
                                rowTxt += "	" + dgvProduct.Rows[i].Cells[j].Value.ToString().Replace("\n", " ");
                            }
                            clipboardTxt += rowTxt.Trim();
                        }
                    }
                }
                else if (dgvProduct.SelectedCells.Count > 0)
                {

                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        string rowTxt = "";
                        for (int j = 0; j < dgvProduct.Columns.Count; j++)
                        {
                            if (dgvProduct.Rows[i].Cells[j].Selected)
                            {
                                if (dgvProduct.Rows[i].Cells[j].Value == null)
                                    dgvProduct.Rows[i].Cells[j].Value = string.Empty;
                                rowTxt += "	" + dgvProduct.Rows[i].Cells[j].Value.ToString().Replace("\n", " ");
                            }
                        }

                        if (!string.IsNullOrEmpty(rowTxt.Trim()))
                            clipboardTxt += rowTxt.Trim();
                    }
                }

                if (!string.IsNullOrEmpty(clipboardTxt))
                    Clipboard.SetDataObject((object)clipboardTxt);
                isPasteRow = false;
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }

        /// <summary>
        /// 선택한 셀을 삭제합니다.
        /// </summary>
        private void Delete()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            try
            {
                if (dgvProduct.SelectedRows.Count > 0)
                {
                    int delete_cnt = dgvProduct.SelectedRows.Count;
                    for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
                    {
                        if (dgvProduct.Rows[i].Selected)
                        {
                            dgvProduct.Rows.Remove(dgvProduct.Rows[i]);
                            delete_cnt--;
                            if (delete_cnt == 0)
                                break;
                        }
                    }
                }
                else
                {
                    foreach (DataGridViewCell oneCell in dgvProduct.SelectedCells)
                    {
                        if (oneCell.Selected)
                            oneCell.Value = string.Empty;
                    }
                }
            }
            catch { }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }


        /// <summary>
        /// 붙여넣기 합니다.
        /// </summary>
        /// 
        bool isPasteRow = false;

        public void SetIsPasteRow(bool is_add)
        {
            isPasteRow = is_add;
        }

        public void Paste()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            if (dgvProduct.CurrentCell != null)
            {
                string clipText = Clipboard.GetText();
                if (string.IsNullOrEmpty(clipText) == false)
                {
                    string[] lines = clipText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    string[] texts = lines[0].Split('\t');
                    bool isLastRow = false;

                    int startRow = dgvProduct.CurrentCell.RowIndex;
                    int startCol = dgvProduct.CurrentCell.ColumnIndex;

                    int row = startRow;



                    //행단위 붙혀넣기
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        //행 추가하지 않고 붙혀넣기 상태
                        if (!isPasteRow)
                        {
                            for (int i = 0; i < lines.Length; i++)
                            {
                                int selectLines = 0;
                                foreach (DataGridViewRow rows in dgvProduct.SelectedRows)
                                {
                                    selectLines++;
                                }

                                //복사한 줄이랑 붙혀넣을때 잡은 줄수랑 같을때
                                if (lines.Length == selectLines)
                                {

                                    DataGridViewRow selectRow = dgvProduct.Rows[startRow];
                                    row = startRow;
                                    texts = lines[i].Split('\t');

                                    int col = startCol;
                                    if (texts.Length > 1)
                                    {
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                            { break; }
                                            else
                                            {
                                                if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                {

                                                }
                                                else
                                                {
                                                    string txt = texts[j];
                                                    dgvProduct[col, row].Value = txt.Replace(@"\n", "\n");
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataGridViewCell cell in selectRow.Cells)
                                            cell.Value = texts[0].Replace(@"\n", "\n");
                                    }
                                    startRow++;
                                }
                                //복사한 줄은 많은데 붙잡은 줄은 하나일 때
                                else if (lines.Length > selectLines && selectLines == 1)
                                {
                                    row = startRow;
                                    int col = startCol;

                                    texts = lines[i].Split('\t');
                                    if (texts.Length > 1)
                                    {
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                            { break; }
                                            else
                                            {
                                                if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                {

                                                }
                                                else
                                                {
                                                    string txt = texts[j];
                                                    dgvProduct[col, row].Value = txt.Replace(@"\n", "\n");
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    startRow++;
                                }
                                //나머지
                                else
                                {
                                    foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                                    {
                                        row = cell.RowIndex;
                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        if (texts.Length > 1)
                                        {
                                            for (int j = 0; j < texts.Length; j++)
                                            {
                                                if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                                { break; }
                                                else
                                                {
                                                    if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        string txt = texts[j];
                                                        dgvProduct[col, row].Value = txt.Replace(@"\n", "\n");
                                                        col++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            cell.Value = texts[0].Replace(@"\n", "\n");
                                        }
                                    }
                                }
                            }
                        }
                        //행 추가하고 붙혀넣기 상태
                        else
                        {
                            //행추가
                            int rowIndex = 0;
                            if (dgvProduct.CurrentCell != null)
                            {
                                rowIndex = dgvProduct.CurrentCell.RowIndex;
                            }
                            dgvProduct.Rows.Insert(rowIndex, lines.Length);
                            //추가한 행에 붙혀넣기
                            for (int i = 0; i < lines.Length; i++)
                            {
                                int selectLines = 0;
                                foreach (DataGridViewRow rows in dgvProduct.SelectedRows)
                                {
                                    selectLines++;
                                }

                                //복사한 줄이랑 붙혀넣을때 잡은 줄수랑 같을때
                                if (lines.Length == selectLines)
                                {

                                    DataGridViewRow selectRow = dgvProduct.Rows[startRow];
                                    row = startRow;
                                    texts = lines[i].Split('\t');

                                    int col = startCol;
                                    if (texts.Length > 1)
                                    {
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                            { break; }
                                            else
                                            {
                                                if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                {

                                                }
                                                else
                                                {
                                                    string txt = texts[j];
                                                    dgvProduct[col, row].Value = txt.Replace(@"\n", "\n");
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataGridViewCell cell in selectRow.Cells)
                                            cell.Value = texts[0].Replace(@"\n", "\n");
                                    }
                                    startRow++;
                                }
                                //복사한 줄은 많은데 붙잡은 줄은 하나일 때
                                else if (lines.Length > selectLines && selectLines == 1)
                                {
                                    row = startRow;
                                    int col = startCol;

                                    texts = lines[i].Split('\t');
                                    if (texts.Length > 1)
                                    {
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                            { break; }
                                            else
                                            {
                                                if (j == 0 && string.IsNullOrEmpty(texts[j]))
                                                {

                                                }
                                                else
                                                {
                                                    string txt = texts[j];
                                                    dgvProduct[col, row].Value = txt.Replace(@"\n", "\n");
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    startRow++;
                                }
                                //나머지
                                else
                                {
                                    foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                                    {
                                        row = cell.RowIndex;
                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        if (texts.Length > 1)
                                        {
                                            for (int j = 0; j < texts.Length; j++)
                                            {
                                                if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                                { break; }
                                                else
                                                {
                                                    if (j == 0 && string.IsNullOrEmpty(texts[0]))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        string txt = texts[j];
                                                        dgvProduct[col, row].Value = txt.Replace(@"\n", "\n");
                                                        col++;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            cell.Value = texts[0].Replace(@"\n", "\n");
                                        }
                                    }
                                }
                            }
                        }
                        //행 추가 후 붙혀넣기 상태 취소
                        isPasteRow = false;
                    }
                    //셀단위 붙혀넣기
                    else
                    {
                        // Multi rows, Multi columns
                        if (lines.Length > 1 && texts.Length > 1)
                        {
                            for (int i = 0; i < lines.Length; i++)
                            {
                                texts = lines[i].Split('\t');

                                int col = startCol;
                                for (int j = 0; j < texts.Length; j++)
                                {
                                    if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                        break;

                                    string txt = texts[j];
                                    dgvProduct[col, row].Value = txt;
                                    col++;
                                }
                                row++;
                            }
                        }
                        // one row, Multi columns
                        else if (lines.Length == 1 && texts.Length > 1)
                        {
                            //한점잡고 붙혀넣기
                            if (dgvProduct.SelectedCells.Count == 1)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                                    {

                                        if (cell.RowIndex == dgvProduct.Rows.Count - 1 && dgvProduct.AllowUserToAddRows)
                                        {
                                            dgvProduct.Rows.Add();

                                            DataGridViewCell tmpCell = dgvProduct.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex];
                                            row = tmpCell.RowIndex;
                                        }
                                        else
                                        {
                                            row = cell.RowIndex;
                                        }
                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                                break;

                                            string txt = texts[j];
                                            dgvProduct[col, row].Value = txt;
                                            col++;
                                        }
                                    }
                                }
                            }
                            //복사, 붙혀넣기 규격이 같을때
                            else if (dgvProduct.SelectedCells.Count == texts.Length)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                                    {

                                        if (cell.RowIndex == dgvProduct.Rows.Count - 1 && dgvProduct.AllowUserToAddRows)
                                        {
                                            dgvProduct.Rows.Add();

                                            DataGridViewCell tmpCell = dgvProduct.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex];
                                            row = tmpCell.RowIndex;
                                        }
                                        else
                                        {
                                            row = cell.RowIndex;
                                        }
                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        foreach (DataGridViewCell c in dgvProduct.SelectedCells)
                                        {
                                            if (col > c.ColumnIndex)
                                                col = c.ColumnIndex;
                                        }
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                                break;

                                            string txt = texts[j];
                                            dgvProduct[col, row].Value = txt;
                                            col++;
                                        }
                                    }
                                }
                            }
                        }
                        // one row, Multi columns
                        else if (lines.Length > 1 && texts.Length == 1)
                        {
                            row = dgvProduct.SelectedCells[0].RowIndex;
                            foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                            {
                                if (row > cell.RowIndex)
                                    row = cell.RowIndex;
                            }
                            //한점잡고 붙혀넣기
                            if (dgvProduct.SelectedCells.Count == 1)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                                    {

                                        texts = lines[i].Split('\t');

                                        int col = startCol;
                                        for (int j = 0; j < texts.Length; j++)
                                        {
                                            if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                                break;

                                            string txt = texts[j];
                                            dgvProduct[col, row].Value = txt;
                                            row++;
                                        }
                                    }
                                }
                            }
                            //같은 규격으로 붙혀넣기
                            else if (dgvProduct.SelectedCells.Count == lines.Length)
                            {
                                for (int i = 0; i < lines.Length; i++)
                                {
                                    texts = lines[i].Split('\t');

                                    int col = startCol;
                                    for (int j = 0; j < texts.Length; j++)
                                    {
                                        if (dgvProduct.RowCount <= row || dgvProduct.ColumnCount <= col)
                                            break;

                                        string txt = texts[j];
                                        dgvProduct[col, row].Value = txt;
                                        row++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string txt = lines[0];
                            foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                            {
                                if (cell.RowIndex == dgvProduct.Rows.Count - 1 && dgvProduct.AllowUserToAddRows)
                                {
                                    dgvProduct.Rows.Add();
                                    dgvProduct.Rows[cell.RowIndex - 1].Cells[cell.ColumnIndex].Value = txt;
                                }
                                else
                                {
                                    cell.Value = txt;
                                }

                            }
                        }
                    }
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }



        #endregion

        #region LIstbox Drag & Drop

        #endregion

        private void lvBookmark_DragDrop(object sender, DragEventArgs e)
        {
            Point cp = lvBookmark.PointToClient(new Point(e.X, e.Y));
            ListViewItem dragToItem = lvBookmark.GetItemAt(cp.X, cp.Y);
            if (dragToItem != null)
            {
                ListViewItem draggetItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

                int indexToInsert = dragToItem.Index;
                if (draggetItem.Index < indexToInsert)
                    indexToInsert--;

                lvBookmark.Items.Remove(draggetItem);
                lvBookmark.Items.Insert(indexToInsert, draggetItem);

                //순서저장
                StringBuilder sql = new StringBuilder();
                List<StringBuilder> sqlList = new List<StringBuilder>();
                int order_count = 0;
                lvBookmark.EndUpdate();
                for (int i = 0; i < lvBookmark.Items.Count; i++)
                {
                    bool isNotification;
                    if (!bool.TryParse(lvBookmark.Items[i].SubItems[3].Text, out isNotification))
                        isNotification = false;

                    if (isNotification)
                    {
                        int form_id = Convert.ToInt32(lvBookmark.Items[i].SubItems[0].Text);
                        sql = bookmarkRepository.UpdateOrderSql(form_id, order_count++);
                        sqlList.Add(sql);
                    }
                }

                if (commonRepository.UpdateTran(sqlList) == -1)
                    MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
            }
        }

        private void lvBookmark_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void lvBookmark_ItemDrag(object sender, ItemDragEventArgs e)
        {
            lvBookmark.DoDragDrop(e.Item, DragDropEffects.Move);
        }
    }
}
