using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using CheckBox = System.Windows.Forms.CheckBox;
using MenuItem = System.Windows.Forms.MenuItem;
using System.Runtime.InteropServices;
using Repositories.SEAOVER;
using Repositories.Config;
using AdoNetWindow.Common;

namespace AdoNetWindow
{
    public partial class UnconfirmedPending : ApplicationRoot
    {
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        IStockRepository stockRepository = new StockRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        IShippingRepository shippingRepository = new ShippingRepository();
        IUsersRepository usersRepository = new UsersRepository();
        string userId;
        UsersModel um = new UsersModel();
        List<CustomsInfoModel> cm = new List<CustomsInfoModel>();
        CalendarModule.calendar cd;
        Libs.Tools.Common common = new Libs.Tools.Common();
        int lastHeaderClickedIndex = 0;
        bool isControlPressed = false;
        bool isShiftPressed = false;
        bool IsContextMenu = false;
        public enum SearchType 
        {
            Contract,
            Shipping,
            AfterStock,
            Customs,
            Comfirm,
            All
        }

        public UnconfirmedPending(string user_id, CalendarModule.calendar calendar)
        {
            InitializeComponent();

            userId = user_id;
            um = usersRepository.GetUserInfo(userId);
            cd = calendar;
        }
        public UnconfirmedPending(string user_id, string product, string origin, string sizes, string unit)
        {
            InitializeComponent();
            userId = user_id;
            um = usersRepository.GetUserInfo(userId);
            cd = null;
            txtProduct.Text = product + ";";
            txtOrigin.Text = origin + ";";
            txtSizes.Text = sizes + ";";
            txtBoxWeight.Text = unit;

            cbCcStatus.Text = "전체";
            rbAll.Checked = true;

            //GetCustomInfo();
        }

        private void UnconfirmedPending_Load(object sender, EventArgs e)
        {
            nudSttYear.Value = DateTime.Now.AddYears(-1).Year;
            nudEndYear.Value = DateTime.Now.Year;
            //디자인 모드에서는 동작하지 않도록 한다.
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                //더블버퍼
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
            }
            //로그인유저 정보
            um = usersRepository.GetUserInfo(userId);

            //권한별 설정
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_excel"))
                {
                    btnExcel.Visible = false;
                }
            }
            //Tooltip
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;
            toolTip.IsBalloon = true;
            string strTooltip = "* 기본 : 띄어쓰기 기준으로 나눈 후 포함된 내역 조회"
                + "\n* 마지막 ';' 추가 : 입력한 문자열 전체가 포함되는 내역 조회";

            toolTip.SetToolTip(this.txtAtoNo, strTooltip);
            toolTip.SetToolTip(this.txtContractNo, strTooltip);
            toolTip.SetToolTip(this.txtShipper, strTooltip);
            toolTip.SetToolTip(this.txtLcNo, strTooltip);
            toolTip.SetToolTip(this.txtBlNo, strTooltip);
            toolTip.SetToolTip(this.txtOrigin, strTooltip);
            toolTip.SetToolTip(this.txtProduct, strTooltip);
            toolTip.SetToolTip(this.txtSizes, strTooltip);
            toolTip.SetToolTip(this.txtBoxWeight, strTooltip);
            toolTip.SetToolTip(this.txtManager, strTooltip);
            toolTip.SetToolTip(this.cbUsance, strTooltip);
            toolTip.SetToolTip(this.cbDivision, strTooltip);
            toolTip.SetToolTip(this.cbAgency, strTooltip);
            toolTip.SetToolTip(this.cbPayment, strTooltip);
            toolTip.SetToolTip(this.cbCcStatus, strTooltip);
            toolTip.SetToolTip(this.txtImportNumber, strTooltip);

            nudEndYear.Text = DateTime.Now.ToString("yyyy");
            this.KeyPreview = true;

            Init_DataGridView();
            //GetCustomInfo();
            this.ActiveControl = txtAtoNo;
        }

        //DatagridView Double Buffered Setting
        private void Init_DataGridView()
        {
            PendingList.DoubleBuffered(true);
        }

        #region Method
        private void SetFontSizes()
        {
            int sizes = (int)nudFontsize.Value;
            PendingList.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", sizes, FontStyle.Regular);
            foreach (DataGridViewRow row in PendingList.Rows)
            {
                row.DefaultCellStyle.Font = new Font("나눔고딕", sizes, FontStyle.Regular);
            }
        }
        public void GetCustomInfo()
        {
            PendingList.DataSource = null;
            //속도개선(끄기)
            this.PendingList.SuspendLayout();

            string sttYear = nudSttYear.Text.Trim();
            string endYear = nudEndYear.Text.Trim();
            string atoNo = txtAtoNo.Text.Trim();
            string contractNo = txtContractNo.Text.Trim();
            string shipper = txtShipper.Text.Trim();
            string lcNo = txtLcNo.Text.Trim();
            string blNo = txtBlNo.Text.Trim();
            string origin = txtOrigin.Text.Trim();
            string product = txtProduct.Text.Trim();
            string sizes = txtSizes.Text.Trim();
            string boxWeight = txtBoxWeight.Text.Trim();
            string manager = txtManager.Text.Trim();
            string usance = cbUsance.Text.Trim();
            string division = cbDivision.Text.Trim();
            if (division == "전체")
                division = string.Empty;
            string agency = cbAgency.Text.Trim();
            if (agency == "전체")
                agency = string.Empty;
            string payment = cbPayment.Text.Trim();
            string cc_status = cbCcStatus.Text.Trim();
            if (cc_status == "전체")
                cc_status = string.Empty;
            string import_number = txtImportNumber.Text.Trim();

            //F1 ~ F6
            SearchType st;
            if (rbContract.Checked)
                st = SearchType.Contract;
            else if (rbShipping.Checked)
                st = SearchType.Shipping;
            else if (rbAfterStock.Checked)
                st = SearchType.AfterStock;
            else if (rbCustoms.Checked)
                st = SearchType.Customs;
            else if (rbComfirm.Checked)
                st = SearchType.Comfirm;
            else 
                st = SearchType.All;

            //정렬
            int sortType = 1;
            switch (cbSortType.Text)
            {
                case "등록순":
                    sortType = 1;
                    break;
                case "수정순":
                    sortType = 2;
                    break;
                case "AtoNo":
                    sortType = 3;
                    break;
                case "PI DATE":
                    sortType = 4;
                    break;
                case "ETD":
                    sortType = 5;
                    break;
                case "ETA":
                    sortType = 6;
                    break;
                case "창고입고일":
                    sortType = 7;
                    break;
            }

            int isDelete = 1;
            if(rbTotalCustom.Checked)
                isDelete = 1;
            else if (rbNotDeleteCustom.Checked)
                isDelete = 2;
            else if (rbDeleteCustom.Checked)
                isDelete = 3;


            //팬딩내용
            //List<UncommfirmModel> model = customsRepository.GetUncom`firm(cYear, atoNo, shipper, origin, product, sizes, manager, ccStatus, payStatus);
            List<CustomsInfoModel> model = customsRepository.GetPending(st.ToString(), sttYear, endYear, atoNo, cbisStart.Checked, contractNo, shipper
                                                                        , lcNo, blNo, import_number, origin, product, sizes, boxWeight, manager, usance
                                                                        , division, agency, payment, cc_status, null, sortType, isDelete);
            //합계계산
            double total_price = 0;
            double total_weight = 0;
            double total_cost_unit = 0;
            double total_contract_qty = 0;
            double total_warehouse_qty = 0;


            double all_weight = 0;
            double all_count = 0;
            double all_price = 0;


            for (int i = 0; i < model.Count; i++)
            {
                if (model[i].sub_id == 9999)
                {
                    all_weight += total_weight;
                    all_price += total_price;
                    all_count += total_contract_qty;

                    model[i].box_weight = total_weight.ToString("#,##0.00");
                    model[i].cost_unit = total_cost_unit.ToString("#,##0.00");
                    model[i].unit_price = Convert.ToDouble(total_price.ToString("#,##0.00"));
                    model[i].quantity_on_paper = Math.Round(total_contract_qty, 0);
                    model[i].qty = Math.Round(total_warehouse_qty, 0);
                    total_weight = 0;
                    total_price = 0;
                    total_cost_unit = 0;
                    total_contract_qty = 0;
                    total_warehouse_qty = 0;
                }
                //기타비용
                else if (model[i].product == "기타비용")
                    total_price += model[i].unit_price;
                else
                {
                    double box_weight;
                    if (model[i].box_weight == null || !double.TryParse(model[i].box_weight, out box_weight))
                        box_weight = 0;

                    double cost_unit;
                    if (model[i].cost_unit == null || !double.TryParse(model[i].cost_unit, out cost_unit))
                        cost_unit = 0;

                    bool weight_calculate = model[i].weight_calculate;
                    //트레이 계산법
                    if (!weight_calculate && cost_unit > 0)
                    {
                        total_weight += box_weight * model[i].quantity_on_paper;
                        total_price += cost_unit * model[i].quantity_on_paper * model[i].unit_price;
                    }
                    //박스중량 계산법
                    else
                    {
                        total_weight += box_weight * model[i].quantity_on_paper;
                        total_price += box_weight * model[i].quantity_on_paper * model[i].unit_price;
                    }

                    total_contract_qty += model[i].quantity_on_paper;
                    total_warehouse_qty += model[i].qty;

                    model[i].tariff_rate = model[i].tariff_rate * 100;
                    model[i].vat_rate = Math.Round(model[i].vat_rate * 100);
                }
            

                DateTime updatetime;
                if (DateTime.TryParse(model[i].updatetime, out updatetime))
                {
                    model[i].updatetime = updatetime.ToString("yyyy-MM-dd hh:mm:ss");
                }
            }
            //출력
            if (model != null)
            {
                PendingList.DataSource = model;
                V_ShowTableHeader();
                //총합
                txtCount.Text = all_count.ToString("#,##0");
                txtWeight.Text = all_weight.ToString("#,##0.00");
                txtPrice.Text = all_price.ToString("#,##0.00");
            }
            //속도개선(켜기)
            this.PendingList.ResumeLayout();
        }
        private void V_ShowTableHeader()
        {
            PendingList.Columns["contract_year"].HeaderText = "계약연도";
            PendingList.Columns["ato_no"].HeaderText = "ATO No.";
            PendingList.Columns["pi_date"].HeaderText = "Pi date";
            PendingList.Columns["contract_no"].HeaderText = "Contract No.";
            PendingList.Columns["shipper"].HeaderText = "거래처";
            PendingList.Columns["shipment_date"].HeaderText = "계약선적일";
            PendingList.Columns["lc_open_date"].HeaderText = "L/C OPEN";
            PendingList.Columns["lc_payment_date"].HeaderText = "L/C No.";
            PendingList.Columns["bl_no"].HeaderText = "B/L No.";
            PendingList.Columns["warehousing_date"].HeaderText = "창고입고일";
            PendingList.Columns["pending_check"].HeaderText = "통관예정일";
            PendingList.Columns["etd"].HeaderText = "ETD";
            PendingList.Columns["eta"].HeaderText = "ETA";
            PendingList.Columns["cc_status"].HeaderText = "상태";
            PendingList.Columns["warehouse"].HeaderText = "창고";
            PendingList.Columns["broker"].HeaderText = "관세사";
            PendingList.Columns["origin"].HeaderText = "원산지";
            PendingList.Columns["product"].HeaderText = "품명";
            PendingList.Columns["sizes"].HeaderText = "사이즈";
            PendingList.Columns["box_weight"].HeaderText = "박스중량(kg)";
            PendingList.Columns["cost_unit"].HeaderText = "트레이";
            PendingList.Columns["unit_price"].HeaderText = "단가($)";
            PendingList.Columns["quantity_on_paper"].HeaderText = "계약수량";
            PendingList.Columns["qty"].HeaderText = "실제창고 입고수량";
            PendingList.Columns["tariff_rate"].HeaderText = "관세율(%)";
            PendingList.Columns["vat_rate"].HeaderText = "부가세율(%)";
            PendingList.Columns["remark"].HeaderText = "비고";
            PendingList.Columns["payment_date"].HeaderText = "결제일";
            PendingList.Columns["payment_date_status"].HeaderText = "결제확정여부";
            PendingList.Columns["payment_bank"].HeaderText = "결제은행";
            PendingList.Columns["manager"].HeaderText = "담당자";
            PendingList.Columns["usance_type"].HeaderText = "수입구분";
            PendingList.Columns["agency_type"].HeaderText = "대행";
            PendingList.Columns["division"].HeaderText = "화주";
            PendingList.Columns["edit_user"].HeaderText = "수정자";
            PendingList.Columns["updatetime"].HeaderText = "수정일자";
            PendingList.Columns["sanitary_certificate"].HeaderText = "HC, CO";
            PendingList.Columns["import_number"].HeaderText = "수입신고번호";

            this.PendingList.Columns["id"].Visible = false;
            this.PendingList.Columns["sub_id"].Visible = false;
            this.PendingList.Columns["loading_cost_per_box"].Visible = false;
            this.PendingList.Columns["refrigeration_charge"].Visible = false;
            this.PendingList.Columns["total_amount_seaover"].Visible = false;
            this.PendingList.Columns["trq_amount"].Visible = false;
            this.PendingList.Columns["weight_calculate"].Visible = false;

            //this.PendingList.Columns["unit_price"].DefaultCellStyle.Format = "#,##0.00";
            this.PendingList.Columns["quantity_on_paper"].DefaultCellStyle.Format = "#,##0.00";
            this.PendingList.Columns["qty"].DefaultCellStyle.Format = "#,##0.00";


            PendingList.Columns["contract_year"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["box_weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["unit_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["quantity_on_paper"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["tariff_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["vat_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["trq_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["total_amount_seaover"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["refrigeration_charge"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            PendingList.Columns["loading_cost_per_box"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            

            //헤더 디자인
            foreach (DataGridViewColumn col in PendingList.Columns)
            {
                col.HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
                if (col.Name == "ato_no" || col.Name == "product" || col.Name == "origin" || col.Name == "sizes")
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                else if (col.Name == "contract_year")
                    col.Width = 40;
                else if (col.Name == "cc_status" || col.Name == "warehouse" || col.Name == "box_weight" || col.Name == "box_weight")
                    col.Width = 50;
                else
                    col.Width = 60;
            }

            this.PendingList.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            this.PendingList.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            this.PendingList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            this.PendingList.Columns["ato_no"].HeaderCell.Style.BackColor = Color.BlueViolet;
            this.PendingList.Columns["ato_no"].HeaderCell.Style.ForeColor = Color.White;
            this.PendingList.Columns["ato_no"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            this.PendingList.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            this.PendingList.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            this.PendingList.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            //this.PendingList.Columns["product"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            this.PendingList.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            this.PendingList.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;
            this.PendingList.Columns["origin"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            //this.PendingList.Columns["origin"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            this.PendingList.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            this.PendingList.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;
            this.PendingList.Columns["sizes"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            //this.PendingList.Columns["sizes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            this.PendingList.Columns["unit_price"].HeaderCell.Style.BackColor = Color.BlueViolet;
            this.PendingList.Columns["unit_price"].HeaderCell.Style.ForeColor = Color.White;
            this.PendingList.Columns["unit_price"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);
            //this.PendingList.Columns["unit_price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            this.PendingList.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
        }
        #endregion

        #region Datagridview event(셀머지, 컬럼해더 선택)
        private void PendingList_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0)
            {
                PendingList.Columns[e.ColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                //PendingList.Columns[e.ColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
        }
        private void PendingList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        private void PendingList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Shift)
            {
                isShiftPressed = true;
            }
            else if (Control.ModifierKeys != Keys.Shift)
            {
                isShiftPressed = false;
            }
            if (Control.ModifierKeys == Keys.Control)
            {
                isControlPressed = true;
            }
            else if (Control.ModifierKeys != Keys.Control)
            {
                isControlPressed = false;
            }

            int col = e.ColumnIndex;

            bool isSelected = true;

            if (PendingList.SelectedCells.Count > 0 && PendingList.Rows.Count > 0)
            {
                for (int i = 0; i < PendingList.Rows.Count; i++)
                {
                    if (PendingList.Rows[i].Cells[col].Selected == true)
                    {
                        isSelected = false;
                    }
                }
            }
            //이미 선택한 컬럼이 난 경우만 
            if (isSelected)
            { 
                if (this.isShiftPressed)
                {
                    int startCol = Math.Min(col, this.lastHeaderClickedIndex);
                    int endCol = Math.Max(col, this.lastHeaderClickedIndex);

                    for (int i = startCol; i <= endCol; i++)
                    {
                        for (int j = 0; j < PendingList.RowCount; j++)
                        {
                            PendingList[i, j].Selected = true;
                        }
                    }
                }
                else if (this.isControlPressed)
                {
                    foreach (DataGridViewRow row in PendingList.Rows)
                    {
                        row.Cells[e.ColumnIndex].Selected = true;
                    }
                }
                else
                {
                    PendingList.ClearSelection();
                    foreach (DataGridViewRow row in PendingList.Rows)
                    {
                        row.Cells[e.ColumnIndex].Selected = true;
                    }
                }
            }
            if (this.isShiftPressed == false)
                this.lastHeaderClickedIndex = e.ColumnIndex;
        }
        private void PendingList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //기타비용
                if (PendingList.Columns[e.ColumnIndex].Name == "product"
                    && PendingList.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "기타비용")
                {
                    PendingList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Linen;
                    PendingList.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Linen;

                    PendingList.Rows[e.RowIndex].Cells["product"].Style.ForeColor = Color.Black;
                    PendingList.Rows[e.RowIndex].Cells["unit_price"].Style.ForeColor = Color.Black;
                }
            }

            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
            {
                return;
            }
            //머지
            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = PendingList.AdvancedCellBorderStyle.Top;
            }
            
        }


        private void PendingList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                return;
                if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
                {
                    e.Value = "";
                    e.FormattingApplied = true;
                }

            }
            else
            {

                string val = PendingList.Rows[e.RowIndex].Cells[1].Value.ToString();
                // SUM
                if (val == "9999")
                {
                    PendingList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    PendingList.Rows[e.RowIndex].Cells[2].Style.ForeColor = Color.LightGray;
                    PendingList.Rows[e.RowIndex].Cells[3].Style.ForeColor = Color.LightGray;
                    PendingList.Rows[e.RowIndex].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                    /*PendingList.Rows[e.RowIndex].Cells["quantity_on_paper"].Style.ForeColor = Color.LightGray;
                    PendingList.Rows[e.RowIndex].Cells["qty"].Style.ForeColor = Color.LightGray;
                    PendingList.Rows[e.RowIndex].Cells["tarrif_rate"].Style.ForeColor = Color.LightGray;
                    PendingList.Rows[e.RowIndex].Cells["vat_rate"].Style.ForeColor = Color.LightGray;*/
                }
            }
        }

        private void PendingList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try { 
                int currentRow = e.RowIndex;
                PendingList.Rows[currentRow].DefaultCellStyle.BackColor = Color.AliceBlue;
                PendingList.Rows[currentRow].HeaderCell.Style.BackColor = Color.Red;
            } 
            catch(Exception e1)
            { }        
        }

        private void PendingList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1) 
            {
                int currentRow = e.RowIndex;
                PendingList.Rows[currentRow].DefaultCellStyle.BackColor = Color.White;
                PendingList.Rows[currentRow].HeaderCell.Style.BackColor = Color.SeaGreen;
            }
        }
        //같으면 줄없애기(머지처럼 보이게)
        bool IsTheSameCellValue(int column, int row)
        {
            DataGridViewCell cell1 = PendingList[0, row];
            DataGridViewCell cell2 = PendingList[0, row - 1];
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }
            return cell1.Value.ToString() == cell2.Value.ToString();
        }
        private void PendingList_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (PendingList.SelectedColumns.Count > 1)
            {
                foreach (DataGridViewColumn col in PendingList.SelectedColumns)
                {
                    col.Width = e.Column.Width;
                }
            }
        }
        private void PendingList_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
        {
            if (PendingList.SelectedRows.Count > 1)
            {
                foreach (DataGridViewRow row in PendingList.SelectedRows)
                {
                    row.Height = e.Row.Height;
                }
            }
        }
        #endregion

        #region Key Event
        private void PendingList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        common.GetDgvSelectCellsCapture(PendingList);
                        break;
                }
            }
        }
        private void nudFontsize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetFontSizes();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            if (tb.Name != "PendingList")
            {
                switch (keyData)
                {
                    case Keys.Up:
                        if (tb.Name == "nudSttYear" || tb.Name == "nudEndYear")
                            return base.ProcessCmdKey(ref msg, keyData);
                        else if (tb.Name == "txtProduct")
                            txtBlNo.Focus();
                        else
                            tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Down:
                        if (tb.Name == "nudSttYear" || tb.Name == "nudEndYear")
                            return base.ProcessCmdKey(ref msg, keyData);
                        else if (tb.Name == "txtBlNo")
                            txtProduct.Focus();
                        else
                            tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void UnconfirmedPending_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                isControlPressed = false;
                isShiftPressed = false;
                switch (e.KeyCode)
                {
                    case Keys.A:
                        {
                            FormCollection fc = Application.OpenForms;
                            bool isFormActive = false;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "UnpendingAddManager")
                                {
                                    frm.Activate();
                                    isFormActive = true;
                                }
                            }

                            if (!isFormActive)
                            {
                                int id = customsRepository.GetNextId();
                                UnpendingAddManager pm = new UnpendingAddManager(um, this);
                                pm.Show();
                            }
                            break;
                        }
                    case Keys.Q:
                        {
                            GetCustomInfo();
                            break;
                        }
                    case Keys.N:
                        {
                            txtAtoNo.Text = "";
                            txtContractNo.Text = "";
                            txtShipper.Text = "";
                            txtLcNo.Text = "";
                            txtBlNo.Text = "";
                            txtImportNumber.Text = "";
                            txtOrigin.Text = "";
                            txtProduct.Text = "";
                            txtSizes.Text = "";
                            txtBoxWeight.Text = "";
                            txtManager.Text = "";
                            cbUsance.Text = "";
                            cbDivision.Text = "";
                            cbAgency.Text = "";
                            cbCcStatus.Text = "";
                            txtAtoNo.Focus();
                            break;
                        }
                    case Keys.M:
                        {
                            txtAtoNo.Focus();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                    case Keys.E:
                        ExcelDownload();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                isControlPressed = true;
                isShiftPressed = false;
                switch (e.KeyCode)
                {

                }
            }
            else if (e.Modifiers == Keys.Shift)
            {
                isControlPressed = false;
                isShiftPressed = true;
            }
            else
            {
                isControlPressed = false;
                isShiftPressed = false;
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        rbContract.Checked = true;
                        GetCustomInfo();
                        break;
                    case Keys.F2:
                        rbShipping.Checked = true;
                        GetCustomInfo();
                        break;
                    case Keys.F3:
                        rbAfterStock.Checked = true;
                        GetCustomInfo();
                        break;
                    case Keys.F4:
                        rbCustoms.Checked = true;
                        GetCustomInfo();
                        break;
                    case Keys.F5:
                        rbComfirm.Checked = true;
                        GetCustomInfo();
                        break;
                    case Keys.F6:
                        rbAll.Checked = true;
                        GetCustomInfo();
                        break;
                }
            }
        }
        private void cms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.A:
                    if (IsContextMenu && PendingList.SelectedRows.Count > 0)
                    {
                        DataGridViewRow dr = PendingList.SelectedRows[0];
                        if (dr.Index < 0)
                        {
                            return;
                        }
                        if (PendingList.SelectedRows.Count > 0)
                        {
                            FormCollection fc = Application.OpenForms;
                            bool isFormActive = false;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "UnpendingAddManager")
                                {
                                    frm.Activate();
                                    isFormActive = true;
                                }
                            }

                            if (!isFormActive)
                            {
                                int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());
                                string cc_status = "";
                                if (rbContract.Checked || rbShipping.Checked || rbAfterStock.Checked)
                                {
                                    cc_status = "미통관";
                                }
                                else if (rbCustoms.Checked)
                                {
                                    cc_status = "통관";
                                }
                                else if (rbComfirm.Checked)
                                {
                                    cc_status = "확정";
                                }
                                UnpendingAddManager upm = new UnpendingAddManager(um, cc_status, id, this);
                                upm.Show();
                            }
                        }
                    }
                    break;
                case Keys.S:
                    if (IsContextMenu && PendingList.SelectedRows.Count > 0)
                    {
                        DataGridViewRow dr = PendingList.SelectedRows[0];
                        if (dr.Index < 0)
                        {
                            return;
                        }
                        if (PendingList.SelectedRows.Count > 0)
                        {
                            FormCollection fc = Application.OpenForms;
                            bool isFormActive = false;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "UnPendingManager")
                                {
                                    frm.Activate();
                                    isFormActive = true;
                                }
                            }

                            if (!isFormActive)
                            {
                                int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());
                                string cc_status = "";
                                if (rbContract.Checked || rbShipping.Checked || rbAfterStock.Checked)
                                {
                                    cc_status = "미통관";
                                }
                                else if (rbCustoms.Checked)
                                {
                                    cc_status = "통관";
                                }
                                else if (rbComfirm.Checked)
                                {
                                    cc_status = "확정";
                                }
                                UnPendingManager upm = new UnPendingManager(cd, um, cc_status, id, this);
                                upm.Show();
                            }
                        }
                    }
                    break;
                case Keys.D:
                    if (IsContextMenu && PendingList.SelectedRows.Count > 0)
                    {
                        DataGridViewRow dr = PendingList.SelectedRows[0];
                        if (dr.Index < 0)
                        {
                            return;
                        }
                        if (PendingList.SelectedRows.Count > 0)
                        {
                            if (MessageBox.Show(this,dr.Cells["ato_no"].Value + " 내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());
                                //Sql Execute
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                StringBuilder sql;
                                //t_customs Table data delete
                                sql = customsRepository.DeleteSql(id.ToString());
                                sqlList.Add(sql);
                                //t_shipping Table data delete
                                sql = shippingRepository.DeleteShipping(id);
                                sqlList.Add(sql);

                                int result = customsRepository.UpdateCustomTran(sqlList);
                                if (result == -1)
                                {
                                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                                    this.Activate(); 
                                }
                                else
                                {
                                    GetCustomInfo();
                                    MessageBox.Show(this, "삭제완료");
                                    this.Activate();
                                }
                            }
                        }
                    }
                    break;
                case Keys.F:
                    if (IsContextMenu && PendingList.SelectedRows.Count > 0)
                    {
                        DataGridViewRow dr = PendingList.SelectedRows[0];
                        if (dr.Index < 0)
                        {
                            return;
                        }
                        if (PendingList.SelectedRows.Count > 0)
                        {
                            int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());

                            string cc_status = "";
                            if (rbContract.Checked || rbShipping.Checked || rbAfterStock.Checked)
                            {
                                cc_status = "미통관";
                            }
                            else if (rbCustoms.Checked)
                            {
                                cc_status = "통관";
                            }
                            else if (rbComfirm.Checked)
                            {
                                cc_status = "확정";
                            }

                            PendingManager pm = new PendingManager(cd, um, cc_status, id);
                            pm.Show();
                        }
                    }
                    break;
            }
        }
        private void txtContractyear_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetCustomInfo();
            }
        }
        #endregion

        #region Button
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetCustomInfo();
        }
        private void btnRefreshProduct_Click(object sender, EventArgs e)
        {
            ConvertProductToSeaover cpts = new ConvertProductToSeaover(um);
            cpts.ShowDialog();
        }
        private void btnArrivalSchedule_Click(object sender, EventArgs e)
        {

            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "ArrivalSchedule")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                ArrivalSchedule arrivalSchedule = new ArrivalSchedule(um);
                arrivalSchedule.Show();
            }
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelDownload();
        }
        //신규등록 버튼
        private void btnInsert_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "UnpendingAddManager")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                int id = customsRepository.GetNextId();
                UnpendingAddManager pm = new UnpendingAddManager(um, this);
                pm.Show();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (um.auth_level < 50)
            {
                MessageBox.Show(this, "접근권한이 없습니다.");
                this.Activate();
                return;
            }
            ConvertToPending ctp = new ConvertToPending(um, this);
            ctp.ShowDialog();
        }

        private void CallStockProcedure(bool isDash = false)
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (stockRepository.CallStoredProcedureSTOCK(user_id, eDate, isDash) == 0)
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

        //닫기
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void rbComfirm_Click(object sender, EventArgs e)
        {
            GetCustomInfo();
        }

        private void btnPlus_Click(object sender, EventArgs e)
        {
            int sizes = (int)nudFontsize.Value;
            if (sizes < 100)
            {
                sizes += 1;
                nudFontsize.Value = sizes;
            }
            SetFontSizes();
        }

        private void btnMinus_Click(object sender, EventArgs e)
        {
            int sizes = (int)nudFontsize.Value;
            if (sizes > 9)
            {
                sizes -= 1;
                nudFontsize.Value = sizes;
            }
            SetFontSizes();
        }
        #endregion

        #region 우클릭 메뉴
        //클릭시 전체줄 Selected
        private void PendingList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (PendingList.SelectedRows.Count <= 1)
                    {
                        //Selection
                        PendingList.ClearSelection();
                        DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;
                        cm = customsRepository.GetPendingInfo(this.PendingList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    }
                    else
                    {
                        bool isIncluded = false;
                        foreach (DataGridViewRow row in PendingList.SelectedRows)
                        {
                            if (row.Index == e.RowIndex)
                            {
                                isIncluded = true;
                                break;
                            }
                        }
                        if (!isIncluded)
                        {
                            //Selection
                            PendingList.ClearSelection();
                            DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                            selectRow.Selected = !selectRow.Selected;
                            cm = customsRepository.GetPendingInfo(this.PendingList.Rows[e.RowIndex].Cells[0].Value.ToString());
                        }
                    }
                }
            }
            catch 
            {
            }
        }
        //우클릭 메뉴 Create
        private void PendingList_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = PendingList.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    //영어 변환
                    ChangeIME(false);

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("복사등록(A)");
                    m.Items.Add("수정(S)");
                    m.Items.Add("삭제(D)");
                    ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                    toolStripSeparator.Name = "toolStripSeparator";
                    toolStripSeparator.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator);

                    m.Items.Add("원가계산(F)");
                    m.Items.Add("원가계산2");


                    //상태별메뉴
                    if (rbContract.Checked)
                    {
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator1";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("입고 처리");
                        m.Items.Add("전체 입고 처리");
                        m.Items.Add("통관 처리");
                        m.Items.Add("전체 통관 처리");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator1";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);

                    }
                    else if (rbShipping.Checked)
                    {
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator1";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("입고 처리");
                        m.Items.Add("전체 입고 처리");
                        m.Items.Add("통관 처리");
                        m.Items.Add("전체 통관 처리");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator1";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                    }
                    else if (rbAfterStock.Checked)
                    {
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator1";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("미입고 처리");
                        m.Items.Add("전체 미입고 처리");
                        m.Items.Add("통관 처리");
                        m.Items.Add("전체 통관 처리");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator1";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                    }
                    else if (rbCustoms.Checked)
                    {

                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator1";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("미통관 처리");
                        m.Items.Add("전체 미통관 처리");
                        m.Items.Add("확정 처리");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator2";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                    }
                    else if (rbComfirm.Checked)
                    {
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator1";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("미통관 처리");
                        m.Items.Add("전체 미통관 처리");
                        m.Items.Add("통관 처리");
                        m.Items.Add("전체 통관 처리");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator2";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                    }
                    //공통
                    m.Items.Add("내역찾기");
                    m.Items.Add("서류폴더");
                    m.Items.Add("서류폴더(박스디자인)");
                    ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
                    toolStripSeparator3.Name = "toolStripSeparator3";
                    toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
                    m.Items.Add(toolStripSeparator3);
                    m.Items.Add("열 숨기기");
                    m.Items.Add("열 펼치기");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.Closed += new System.Windows.Forms.ToolStripDropDownClosedEventHandler(this.cms_Closed);
                    m.Opened += new System.EventHandler(this.cms_Opened);
                    m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(PendingList, e.Location);
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

        #region user32.dll :: keybd_event CapsLock 변경
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);
        [DllImport("user32.dll")]
        public static extern void keybd_event(uint vk, uint scan, uint flags, uint extraInfo);
        #endregion

        private void cms_Opened(object sender, EventArgs e)
        {
            IsContextMenu = true;
        }

        private void cms_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            IsContextMenu = false;
        }

        //우클릭 메뉴 Event Handler
        Libs.ftpCommon ftp = new Libs.ftpCommon();
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (PendingList.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = PendingList.SelectedRows[0];
                    if (dr.Index < 0)
                    {
                        return;
                    }

                    int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                    /*PendingInfo p;*/

                    switch (e.ClickedItem.Text)
                    {
                        case "수정(S)":
                            if (PendingList.SelectedRows.Count > 0)
                            {
                                FormCollection fc = Application.OpenForms;
                                bool isFormActive = false;
                                foreach (Form frm in fc)
                                {
                                    //iterate through
                                    if (frm.Name == "UnPendingManager")
                                    {
                                        frm.Activate();
                                        isFormActive = true;
                                    }
                                }

                                if (!isFormActive)
                                {
                                    int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());
                                    string cc_status = "";
                                    if (rbContract.Checked || rbShipping.Checked || rbAfterStock.Checked)
                                        cc_status = "미통관";
                                    else if (rbCustoms.Checked)
                                        cc_status = "통관";
                                    else if (rbComfirm.Checked)
                                        cc_status = "확정";
                                    UnPendingManager upm = new UnPendingManager(cd, um, cc_status, id, this);
                                    upm.Show();
                                }
                            }
                            break;
                        case "삭제(D)":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_delete"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    if (dr.Cells["ato_no"].Value.ToString().Contains("취소") || dr.Cells["ato_no"].Value.ToString().Contains("삭제"))
                                    {
                                        MessageBox.Show(this, "이미 삭제된 내역입니다.");
                                        this.Activate();
                                        return;
                                    }

                                    if (MessageBox.Show(this, dr.Cells["ato_no"].Value + " 내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    {
                                        int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());

                                        //서류폴더명 변경
                                        List<AllCustomsModel> origin_model = customsRepository.GetUnPending(id.ToString());
                                        if (origin_model == null)
                                        {
                                            MessageBox.Show(this, dr.Cells["ato_no"].Value + " 내역을 찾을 수 없습니다.");
                                            this.Activate();
                                            return;
                                        }
                                        //변경할 폴더명
                                        string[] company = origin_model[0].shipper.ToString().Trim().Split(' ');
                                        string ato_no = origin_model[0].ato_no.ToString();
                                        string folder_path;

                                        int d;
                                        //3글자 아토번호
                                        if (!int.TryParse(ato_no.Substring(2, 1), out d))
                                        {
                                            folder_path = origin_model[0].contract_year.ToString() + "년/"
                                                + ato_no.Substring(0, 1) + "/"
                                                + ato_no.Substring(0, 3) + "/"
                                                + origin_model[0].ato_no.ToString() + "(취소)"
                                                + " " + ftp.ReplaceName(origin_model[0].product.ToString().ToString())
                                                + "(" + company[0] + ")";
                                        }
                                        //2글자 아토번호
                                        else
                                        {
                                            folder_path = origin_model[0].contract_year.ToString() + "년/"
                                                + ato_no.Substring(0, 1) + "/"
                                                + ato_no.Substring(0, 2) + "/"
                                                + origin_model[0].ato_no.ToString() + "(취소)"
                                                + " " + ftp.ReplaceName(origin_model[0].product.ToString().ToString())
                                                + "(" + company[0] + ")";
                                        }

                                        //폴더명 변경
                                        bool isFlag = false;
                                        int cnt = 0;
                                        while (!isFlag)
                                        {
                                            DirectoryInfo di = new DirectoryInfo(@"i:/" + folder_path);
                                            if (!di.Exists)
                                            {
                                                isFlag = true;
                                                if (!ftp.RenameDirectory(origin_model[0].contract_year.ToString(), origin_model[0].ato_no, folder_path, false))
                                                    MessageBox.Show(this, "폴더명 수정중 에러가 발생하였습니다.");
                                                this.Activate();
                                            }
                                            else
                                            {
                                                cnt++;
                                                folder_path = origin_model[0].contract_year.ToString() + "년/"
                                                + origin_model[0].ato_no.ToString() + "(취소)"
                                                + " " + ftp.ReplaceName(origin_model[0].product.ToString().ToString())
                                                + "(" + company[0] + ") (" + cnt + ")";
                                            }
                                        }

                                        //Sql Execute
                                        string atoNo = dr.Cells["ato_no"].Value.ToString() + "(취소)";
                                        int result = customsRepository.UpdateData(dr.Cells["id"].Value.ToString(), "ato_no", atoNo);
                                        if (result == -1)
                                        {
                                            MessageBox.Show(this, "삭제시 에러가 발생하였습니다.");
                                            this.Activate();
                                        }
                                        else
                                            GetCustomInfo();

                                        /*List<StringBuilder> sqlList = new List<StringBuilder>();
                                        StringBuilder sql;
                                        //t_customs Table data delete
                                        sql = customsRepository.DeleteSql(id.ToString());
                                        sqlList.Add(sql);
                                        //t_shipping Table data delete
                                        sql = shippingRepository.DeleteShipping(id);
                                        sqlList.Add(sql);

                                        int result = customsRepository.UpdateCustomTran(sqlList);
                                        if (result == -1)
                                            MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
                                        else
                                        {
                                            GetCustomInfo();
                                            MessageBox.Show(this,"삭제완료");
                                        }*/
                                    }
                                }
                            }
                            break;
                        case "복사등록(A)":
                            if (PendingList.SelectedRows.Count > 0)
                            {
                                FormCollection fc = Application.OpenForms;
                                bool isFormActive = false;
                                foreach (Form frm in fc)
                                {
                                    //iterate through
                                    if (frm.Name == "UnpendingAddManager")
                                    {
                                        frm.Activate();
                                        isFormActive = true;
                                    }
                                }

                                if (!isFormActive)
                                {
                                    int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());
                                    string cc_status = "";
                                    if (rbContract.Checked || rbShipping.Checked || rbAfterStock.Checked)
                                        cc_status = "미통관";
                                    else if (rbCustoms.Checked)
                                        cc_status = "통관";
                                    else if (rbComfirm.Checked)
                                        cc_status = "확정";

                                    UnpendingAddManager upm = new UnpendingAddManager(um, cc_status, id, this);
                                    upm.Show();
                                }
                            }
                            break;
                        case "원가계산(F)":
                            if (PendingList.SelectedRows.Count > 0)
                            {
                                int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());

                                string cc_status = "";
                                if (rbContract.Checked || rbShipping.Checked || rbAfterStock.Checked)
                                    cc_status = "미통관";
                                else if (rbCustoms.Checked)
                                    cc_status = "통관";
                                else if (rbComfirm.Checked)
                                    cc_status = "확정";

                                PendingManager pm = new PendingManager(cd, um, cc_status, id); 
                                pm.Show();
                            }
                            break;

                        case "내역삭제":
                            {
                                if (dr.Cells["ato_no"].Value.ToString().Contains("취소") || dr.Cells["ato_no"].Value.ToString().Contains("삭제"))
                                {
                                    MessageBox.Show(this, "이미 삭제된 내역입니다.");
                                    this.Activate();
                                    return;
                                }

                                if (MessageBox.Show(this, "(" + dr.Cells["ato_no"].Value + ") 내역을 삭제하시겠습니까?", "데이터 삭제", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                    DeleteData(dr);
                            }
                            break;
                        case "입고 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "입고 처리 하시겠습니까?\n * 선택한 품목만 입고내역으로 변경됩니다", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                        return;

                                    try
                                    {
                                        for (int i = 0; i < PendingList.Rows.Count; i++)
                                        {
                                            if (PendingList.Rows[i].Selected && PendingList.Rows[i].Cells["sub_id"].Value.ToString() != "9999")
                                            {
                                                string id = PendingList.Rows[i].Cells["id"].Value.ToString();
                                                string sub_id = PendingList.Rows[i].Cells["sub_id"].Value.ToString();

                                                int result = customsRepository.UpdateData(id + " AND sub_id = " + sub_id, "warehousing_date", DateTime.Now.ToString("yyyy-MM-dd"));
                                                if (result == -1)
                                                {
                                                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                                    this.Activate();
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                        return;
                                    }
                                    GetCustomInfo();
                                    MessageBox.Show(this, "완료");
                                    this.Activate();
                                }
                            }
                            break;
                        case "전체 입고 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "전품목 입고 처리 하시겠습니까?\n * 전체 품목이 입고내역으로 변경됩니다", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    {
                                        return;
                                    }

                                    string id = PendingList.Rows[PendingList.SelectedRows[0].Index].Cells["id"].Value.ToString();

                                    int result = customsRepository.UpdateData(id, "warehousing_date", DateTime.Now.ToString("yyyy-MM-dd"));
                                    if (result == -1)
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                    }
                                    else
                                    {
                                        GetCustomInfo();
                                        MessageBox.Show(this, "완료");
                                        this.Activate();
                                    }
                                }
                            }
                            break;
                        case "미입고 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "미입고 처리 하시겠습니까?\n * 선택한 품목만 미입고내역으로 변경됩니다", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    {
                                        return;
                                    }
                                    try
                                    {
                                        for (int i = 0; i < PendingList.Rows.Count; i++)
                                        {
                                            if (PendingList.Rows[i].Selected && PendingList.Rows[i].Cells["sub_id"].Value.ToString() != "9999")
                                            {

                                                string id = PendingList.Rows[i].Cells["id"].Value.ToString();
                                                string sub_id = PendingList.Rows[i].Cells["sub_id"].Value.ToString();

                                                int result = customsRepository.UpdateData(id + " AND sub_id = " + sub_id, "warehousing_date", "");
                                                if (result == -1)
                                                {
                                                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                                    this.Activate();
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                        return;
                                    }

                                    GetCustomInfo();
                                    MessageBox.Show(this, "완료");
                                    this.Activate();
                                }
                            }
                            break;
                        case "전체 미입고 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "전품목 미입고 처리 하시겠습니까?\n * 전체 품목이 미입고 내역으로 변경됩니다", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                        return;

                                    string id = PendingList.Rows[PendingList.SelectedRows[0].Index].Cells["id"].Value.ToString();

                                    int result = customsRepository.UpdateData(id, "warehousing_date", "");
                                    if (result == -1)
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                    }
                                    else
                                    {
                                        GetCustomInfo();
                                        MessageBox.Show(this, "완료");
                                        this.Activate();
                                    }
                                }
                            }
                            break;
                        case "미통관 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "미통관내역으로 변경 하시겠습니까?\n * 선택한 품목만 미통관내역으로 변경됩니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                        return;

                                    try
                                    {
                                        for (int i = 0; i < PendingList.Rows.Count; i++)
                                        {
                                            if (PendingList.Rows[i].Selected && PendingList.Rows[i].Cells["sub_id"].Value.ToString() != "9999")
                                            {
                                                string id = PendingList.Rows[i].Cells["id"].Value.ToString();
                                                string sub_id = PendingList.Rows[i].Cells["sub_id"].Value.ToString();

                                                int result = customsRepository.UpdateDataAsOne(id, sub_id, "cc_status", "미통관");
                                                if (result == -1)
                                                {
                                                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                                    this.Activate();
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                        return;
                                    }
                                    GetCustomInfo();
                                    MessageBox.Show(this, "완료");
                                    this.Activate();
                                }
                            }
                            break;
                        case "전체 미통관 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "미통관내역으로 변경 하시겠습니까?\n * 전체 품목이 미통관내역으로 변경됩니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                        return;

                                    string id = PendingList.Rows[PendingList.SelectedRows[0].Index].Cells["id"].Value.ToString();
                                    int result = customsRepository.UpdateData(id, "cc_status", "미통관");
                                    if (result == -1)
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                        return;
                                    }
                                    else
                                    {
                                        GetCustomInfo();
                                        MessageBox.Show(this, "완료");
                                        this.Activate();
                                    }
                                }
                            }
                            break;
                        case "통관 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "통관내역으로 변경 하시겠습니까?\n * 선택한 품목만 통관내역으로 변경됩니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    {
                                        return;
                                    }

                                    try
                                    {
                                        for (int i = 0; i < PendingList.Rows.Count; i++)
                                        {
                                            if (PendingList.Rows[i].Selected && PendingList.Rows[i].Cells["sub_id"].Value.ToString() != "9999")
                                            {
                                                string id = PendingList.Rows[i].Cells["id"].Value.ToString();
                                                string sub_id = PendingList.Rows[i].Cells["sub_id"].Value.ToString();
                                                int result = customsRepository.UpdateDataAsOne(id, sub_id, "cc_status", "통관");
                                                if (result == -1)
                                                {
                                                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                                    this.Activate();
                                                    return;
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                        return;
                                    }
                                    GetCustomInfo();
                                    MessageBox.Show(this, "완료");
                                    this.Activate();
                                }
                            }
                            break;
                        case "전체 통관 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "통관내역으로 변경 하시겠습니까?\n * 전체 품목이 통관내역으로 변경됩니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    {
                                        return;
                                    }

                                    string id = PendingList.Rows[PendingList.SelectedRows[0].Index].Cells["id"].Value.ToString();
                                    int result = customsRepository.UpdateData(id, "cc_status", "통관");
                                    if (result == -1)
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                    }
                                    else
                                    {
                                        GetCustomInfo();
                                        MessageBox.Show(this, "완료");
                                        this.Activate();

                                    }
                                }
                            }
                            break;
                        case "확정 처리":
                            {
                                //권한확인
                                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                                if (authorityDt != null && authorityDt.Rows.Count > 0)
                                {
                                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_update"))
                                    {
                                        messageBox.Show(this, "권한이 없습니다!");
                                        return;
                                    }
                                }
                                if (PendingList.SelectedRows.Count > 0)
                                {
                                    //Messagebox
                                    if (MessageBox.Show(this, "확정내역으로 변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                                    {
                                        return;
                                    }

                                    string id = PendingList.Rows[PendingList.SelectedRows[0].Index].Cells["id"].Value.ToString();

                                    int result = customsRepository.UpdateData(id, "cc_status", "확정");
                                    if (result == -1)
                                    {
                                        MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                                        this.Activate();
                                    }
                                    else
                                    {
                                        GetCustomInfo();
                                        MessageBox.Show(this, "완료");
                                        this.Activate();
                                    }
                                }
                            }
                            break;
                        case "내역찾기":
                            try
                            {
                                int year = Convert.ToInt32(Convert.ToDateTime(dr.Cells["payment_date"].Value).ToString("yyyy"));
                                int month = Convert.ToInt32(Convert.ToDateTime(dr.Cells["payment_date"].Value).ToString("MM"));
                                int sday = Convert.ToInt32(Convert.ToDateTime(dr.Cells["payment_date"].Value).ToString("dd"));
                                string ato_no = dr.Cells["ato_no"].Value.ToString();


                                sday = SetFindCustom(year, month, sday);


                                cd.displayDays(year, month, sday, ato_no);
                                cd.displayMemo(year, month);
                                cd.BringToFront();
                            }
                            catch 
                            {
                                MessageBox.Show(this,"결제일자가 입력되지 않았습니다.");
                                this.Activate();
                            }

                            break;
                        case "서류폴더":
                            {
                                List<AllCustomsModel> editList = customsRepository.GetUnPending(dr.Cells["id"].Value.ToString());
                                if (editList.Count == 0)
                                {
                                    MessageBox.Show(this, "등록되지 않은 내역입니다.");
                                    this.Activate();
                                    return;
                                }
                                else
                                {
                                    string contract_year = dr.Cells["contract_year"].Value.ToString();
                                    string ato_no = dr.Cells["ato_no"].Value.ToString();
                                    string errMsg;
                                    if (!ftp.StartTradePaperFolder(contract_year, ato_no, out errMsg))
                                    {
                                        if (MessageBox.Show(this, "서류폴더를 찾을 수 없습니다. 새로 생성하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        {
                                            //서류폴더 생성
                                            string[] company = dr.Cells["shipper"].Value.ToString().Trim().Split(' ');
                                            string folder_path;

                                            int d;
                                            //3글자 아토번호
                                            if (!int.TryParse(ato_no.Substring(2, 1), out d))
                                                folder_path = contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 3) + "/" + ato_no + " " + ftp.ReplaceName(dr.Cells["product"].Value.ToString()) + "(" + company[0] + ")";
                                            //2글자 아토번호
                                            else
                                                folder_path = contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 2) + "/" + ato_no + " " + ftp.ReplaceName(dr.Cells["product"].Value.ToString()) + "(" + company[0] + ")";

                                            MakeTradeDocumentFolder(folder_path, "ATO/아토무역/무역/무역1/ㄴ.수입자료/서류");
                                        }
                                    }
                                }
                            }
                            break;
                        case "서류폴더(박스디자인)":
                            {
                                List<AllCustomsModel> editList = customsRepository.GetUnPending(dr.Cells["id"].Value.ToString());
                                if (editList.Count == 0)
                                {
                                    MessageBox.Show(this, "등록되지 않은 내역입니다.");
                                    this.Activate();
                                    return;
                                }
                                else
                                {
                                    string product = dr.Cells["product"].Value.ToString();
                                    string origin = dr.Cells["origin"].Value.ToString();
                                    string manager = dr.Cells["manager"].Value.ToString();
                                    string errMsg;
                                    if (!ftp.StartTradeBoxDegisnFolder(manager, product, origin, out errMsg))
                                    {
                                        if (!string.IsNullOrEmpty(errMsg))
                                            messageBox.Show(this, errMsg);
                                    }
                                }
                            }
                            break;
                        case "원가계산2":
                            { 
                                int id = Convert.ToInt32(PendingList.SelectedRows[0].Cells["id"].Value.ToString());
                                List<AllCustomsModel> model = customsRepository.GetAllTypePending("", id.ToString());
                                DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                                if (model.Count > 0)
                                {
                                    List<string[]> list = new List<string[]>();
                                    for (int i = 0; i < model.Count; i++)
                                    {
                                        string[] product = new string[14];
                                        product[0] = model[i].product;
                                        product[1] = model[i].origin;
                                        product[2] = model[i].sizes;
                                        product[3] = model[i].box_weight;
                                        product[4] = model[i].cost_unit;

                                        product[5] = model[i].tax.ToString();
                                        product[6] = model[i].custom.ToString();
                                        product[7] = model[i].incidental_expense.ToString();
                                        product[8] = model[i].manager;

                                        product[9] = model[i].unit_price.ToString();
                                        product[10] = model[i].quantity_on_paper.ToString();
                                        product[11] = model[i].shipper;

                                        product[12] = "True";
                                        product[13] = "False";

                                        list.Add(product);
                                    }

                                    PurchaseManager.CostAccounting ca = new PurchaseManager.CostAccounting(um);
                                    ca.Show();
                                    ca.AddProduct2(list);
                                    ca.Activate();
                                    
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,ex.Message);
                    this.Activate();
                }
            }
            else
            {
                switch (e.ClickedItem.Text)
                {
                    case "열 숨기기":
                        foreach (DataGridViewCell cell in PendingList.Rows[PendingList.Rows.Count - 1].Cells)
                        {
                            if (cell.Selected == true)
                            {
                                int colIndex = Convert.ToInt32(cell.ColumnIndex);
                                PendingList.Columns[colIndex].Visible = false;
                            }
                        }

                        break;

                    case "열 펼치기":
                        foreach (DataGridViewColumn col in PendingList.Columns)
                        {
                            if (col.Name != "ID" && col.Name != "sub_id")
                            {
                                col.Visible = true;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void DeleteData(DataGridViewRow row)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            bool is_folder_update = false;
            string errMsg = "";
            //1.먼저 기존 폴더명 변경
            string[] company = row.Cells["shipper"].Value.ToString().Trim().Split(' ');
            string ato_no = row.Cells["ato_no"].Value.ToString().ToString();
            string product = row.Cells["product"].Value.ToString().ToString();
            string origin_folder_path;

            int d;
            //3글자 아토번호
            if (!int.TryParse(ato_no.Substring(2, 1), out d))
            {
                origin_folder_path = row.Cells["contract_year"].Value.ToString().ToString() + "년/"
                    + ato_no.Substring(0, 1) + "/"
                    + ato_no.Substring(0, 3) + "/";
            }
            //2글자 아토번호
            else
            {
                origin_folder_path = row.Cells["contract_year"].Value.ToString().ToString() + "년/"
                    + ato_no.Substring(0, 1) + "/"
                    + ato_no.Substring(0, 2) + "/";
            }
            //이전 정보로 폴더찾기
            origin_folder_path += ato_no + " " + ftp.ReplaceName(product) + "(" + company[0] + ")";
            DirectoryInfo di = new DirectoryInfo(@"I:\" + origin_folder_path);
            //없을 경우 아토번호로 유사한 폴더 찾기
            if (!di.Exists)
                origin_folder_path = ftp.StartTradePaperFolderPath(row.Cells["contract_year"].Value.ToString().ToString(), ato_no, out errMsg).Replace(@"\", "/").Replace(@"I:/", "");
            //원래 기존폴더 경로가 null이 아닌 경우만
            if (origin_folder_path != null && !string.IsNullOrEmpty(origin_folder_path))
            {
                //수정된 아토번호 폴더구조 생성
                company = row.Cells["shipper"].Value.ToString().Trim().Split(' ');
                ato_no = row.Cells["ato_no"].Value.ToString();
                product = row.Cells["product"].Value.ToString();
                string update_folder_path;
                //3글자 아토번호
                if (!int.TryParse(ato_no.Substring(2, 1), out d))
                {
                    update_folder_path = row.Cells["contract_year"].Value.ToString() + "년/"
                        + ato_no.Substring(0, 1) + "/"
                        + ato_no.Substring(0, 3) + "/";
                }
                //2글자 아토번호
                else
                {
                    update_folder_path = row.Cells["contract_year"].Value.ToString() + "년/"
                        + ato_no.Substring(0, 1) + "/"
                        + ato_no.Substring(0, 2) + "/";
                }
                //새로운 아토 번호 폴더구조 만들기
                if (ftp.CheckDirectory(update_folder_path, true, "ATO/아토무역/무역/무역1/ㄴ.수입자료/서류"))
                {
                    //폴더 옮기기
                    try
                    {
                        update_folder_path += ato_no + " " + ftp.ReplaceName(product) + "(" + company[0] + ")";
                        Directory.Move("I:/" + origin_folder_path, "I:/" + update_folder_path);
                        is_folder_update = true;
                    }
                    catch (Exception ex)
                    {
                        errMsg = ex.Message;
                    }
                }
            }

            //폴더 변경 성공여부
            if (!is_folder_update)
            {
                if (MessageBox.Show(this,errMsg + "\n기존 서류폴더를 찾지 못 하였습니다. 데이터 값만 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            string atoNo = row.Cells["contract_year"].Value.ToString() + " (취소)";
            int result = customsRepository.UpdateData(row.Cells["id"].Value.ToString(), "ato_no", atoNo);
            if (result == -1)
            {
                MessageBox.Show(this,"수정시 에러가 발생하였습니다.");
                this.Activate();
            }
            else
                GetCustomInfo();

        }

        private void MakeTradeDocumentFolder(string folder_path, string root_path = "Solution/Document")
        {
            //기본 아토번호 폴더 생성     
            if (!ftp.CheckDirectory(folder_path, true, root_path))
            {
                MessageBox.Show(this,"서류폴더 생성중 에러가 발생하였습니다.");
                this.Activate();
            }
        }
        #endregion

        #region Graphics

        private void PendingList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            using (Brush brush = new SolidBrush(Color.White))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, brush, e.RowBounds.Location.X + 35, e.RowBounds.Location.Y + 4, drawFormat);
            }
        }
        #endregion

        #region 달력공휴일 계산
        private int SetFindCustom(int year, int month, int days)
        {
            int[] no;
            string[] name;
            DateTime tempDt = new DateTime(year, month, days);
            getRedDay(year, month, out no, out name);
            //주말일경우 날짜 수정
            for (int i = 0; i < no.Count(); i++)
            {
                
            retry:
                //주말일 경우 수정
                if (tempDt.DayOfWeek == DayOfWeek.Saturday)
                {
                    tempDt = tempDt.AddDays(2);
                }
                else if (tempDt.DayOfWeek == DayOfWeek.Sunday)
                {
                    tempDt = tempDt.AddDays(1);
                }
                days = tempDt.Day;
                tempDt = new DateTime(year, month, days);

                //법정공휴일일 경우
                if (tempDt.Month == tempDt.Month)
                {
                    foreach (int n in no)
                    {
                        if (n == tempDt.Day)
                        {
                            tempDt = tempDt.AddDays(1);
                            goto retry;
                        }
                    }
                }
                else
                {
                    foreach (int n in no)
                    {
                        if (n == tempDt.Day)
                        {
                            tempDt = tempDt.AddDays(1);
                            goto retry;
                        }
                    }
                }

            }
            return tempDt.Day;
        }

        //음력 공휴일계산=========================================================
        private void getRedDay(int year, int month, out int[] no, out string[] name1)
        {
            List<int> no2 = new List<int>();
            List<string> name2 = new List<string>();
            switch (month)
            {
                case 1:
                    no2.Add(1);
                    name2.Add("신정");
                    break;
                case 3:
                    no2.Add(1);
                    name2.Add("삼일절");
                    break;
                case 5:
                    no2.Add(5);
                    name2.Add("어린이날");
                    break;
                case 6:
                    no2.Add(6);
                    name2.Add("현충일");
                    break;
                case 8:
                    no2.Add(15);
                    name2.Add("광복절");
                    break;
                case 9:
                    break;
                case 10:
                    no2.Add(3);
                    name2.Add("개천절");
                    no2.Add(9);
                    name2.Add("한글날");
                    break;
                case 12:
                    no2.Add(25);
                    name2.Add("크리스마스");
                    break;
                default:
                    no = null;
                    name1 = null;
                    break;
            }
            DateTime dt = new DateTime(year - 1, 12, 30);
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 1, 1);
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 1, 2);//설날
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 4, 8);//석가탄신일
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//석가탄신일
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("석가탄신일");
            }

            dt = new DateTime(year, 8, 14);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            dt = new DateTime(year, 8, 15);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            dt = new DateTime(year, 8, 16);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            no = no2.ToArray();
            name1 = name2.ToArray();
        }

        private DateTime convertKoreanMonth(int n음력년, int n음력월, int n음력일)//음력을 양력 변환
        {
            System.Globalization.KoreanLunisolarCalendar 음력 =
            new System.Globalization.KoreanLunisolarCalendar();

            bool b달 = 음력.IsLeapMonth(n음력년, n음력월);
            int n윤월;

            if (음력.GetMonthsInYear(n음력년) > 12)
            {
                n윤월 = 음력.GetLeapMonth(n음력년);
                if (b달)
                    n음력월++;
                if (n음력월 > n윤월)
                    n음력월++;
            }
            try
            {
                음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
            }
            catch
            {
                return 음력.ToDateTime(n음력년, n음력월, 음력.GetDaysInMonth(n음력년, n음력월), 0, 0, 0, 0);//음력은 마지막 날짜가 매달 다르기 때문에 예외 뜨면 그날 맨 마지막 날로 지정
            }

            return 음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
        }
        //음력 공휴일계산=========================================================
        #endregion

        #region Excel download
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void ExcelDownload()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 조회", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            //비밀번호 확인
            PasswordCheckManager pcm = new PasswordCheckManager(um.excel_password);
            if (!pcm.isPasswordCheck())
                return;


            DataGridView dgv = PendingList;
            dgv.EndEdit();
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;

                if (dgv.Rows.Count > 1)
                {
                    BasicSheetsSetting(excelApp, workBook, workSheet, dgv.Rows.Count);
                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        if (dgv.Rows[i].Cells["sub_id"].Value.ToString() == "9999")
                        {
                            Excel.Range rg1 = workSheet.Range[workSheet.Cells[i + 2, 1], workSheet.Cells[i + 2, 36]];
                            rg1.Interior.Color = Color.LightGray;
                            workSheet.Cells[i + 2, 1].Value = "SUM";
                        }
                        else
                        {
                            workSheet.Cells[i + 2, 1].Value = dgv.Rows[i].Cells["contract_year"].Value;
                            workSheet.Cells[i + 2, 2].Value = dgv.Rows[i].Cells["ato_no"].Value;
                            workSheet.Cells[i + 2, 36].Value = dgv.Rows[i].Cells["updatetime"].Value;
                        }

                        
                        workSheet.Cells[i + 2, 3].Value = dgv.Rows[i].Cells["pi_date"].Value;
                        workSheet.Cells[i + 2, 4].Value = dgv.Rows[i].Cells["contract_no"].Value;
                        workSheet.Cells[i + 2, 5].Value = dgv.Rows[i].Cells["shipper"].Value;
                        workSheet.Cells[i + 2, 6].Value = dgv.Rows[i].Cells["lc_open_date"].Value;
                        workSheet.Cells[i + 2, 7].Value = dgv.Rows[i].Cells["lc_payment_date"].Value;
                        workSheet.Cells[i + 2, 8].Value = dgv.Rows[i].Cells["bl_no"].Value;
                        workSheet.Cells[i + 2, 9].Value = dgv.Rows[i].Cells["shipment_date"].Value;
                        workSheet.Cells[i + 2, 10].Value = dgv.Rows[i].Cells["etd"].Value;
                        workSheet.Cells[i + 2, 11].Value = dgv.Rows[i].Cells["eta"].Value;
                        workSheet.Cells[i + 2, 12].Value = dgv.Rows[i].Cells["warehousing_date"].Value;
                        workSheet.Cells[i + 2, 13].Value = dgv.Rows[i].Cells["cc_status"].Value;
                        workSheet.Cells[i + 2, 14].Value = dgv.Rows[i].Cells["pending_check"].Value;
                        workSheet.Cells[i + 2, 15].Value = dgv.Rows[i].Cells["warehouse"].Value;
                        workSheet.Cells[i + 2, 16].Value = dgv.Rows[i].Cells["origin"].Value;
                        workSheet.Cells[i + 2, 17].Value = dgv.Rows[i].Cells["product"].Value;
                        workSheet.Cells[i + 2, 18].Value = dgv.Rows[i].Cells["sizes"].Value;
                        workSheet.Cells[i + 2, 19].Value = dgv.Rows[i].Cells["box_weight"].Value;
                        workSheet.Cells[i + 2, 20].Value = dgv.Rows[i].Cells["cost_unit"].Value;
                        workSheet.Cells[i + 2, 21].Value = dgv.Rows[i].Cells["unit_price"].Value;
                        workSheet.Cells[i + 2, 22].Value = dgv.Rows[i].Cells["quantity_on_paper"].Value;
                        workSheet.Cells[i + 2, 23].Value = dgv.Rows[i].Cells["qty"].Value;
                        workSheet.Cells[i + 2, 24].Value = dgv.Rows[i].Cells["tariff_rate"].Value;
                        workSheet.Cells[i + 2, 25].Value = dgv.Rows[i].Cells["vat_rate"].Value;
                        workSheet.Cells[i + 2, 26].Value = dgv.Rows[i].Cells["remark"].Value;
                        workSheet.Cells[i + 2, 27].Value = dgv.Rows[i].Cells["payment_date"].Value;
                        workSheet.Cells[i + 2, 28].Value = dgv.Rows[i].Cells["payment_date_status"].Value;
                        workSheet.Cells[i + 2, 29].Value = dgv.Rows[i].Cells["payment_bank"].Value;
                        workSheet.Cells[i + 2, 30].Value = dgv.Rows[i].Cells["usance_type"].Value;
                        workSheet.Cells[i + 2, 31].Value = dgv.Rows[i].Cells["division"].Value;
                        workSheet.Cells[i + 2, 32].Value = dgv.Rows[i].Cells["agency_type"].Value;
                        workSheet.Cells[i + 2, 33].Value = dgv.Rows[i].Cells["sanitary_certificate"].Value;
                        workSheet.Cells[i + 2, 34].Value = dgv.Rows[i].Cells["manager"].Value;
                        workSheet.Cells[i + 2, 35].Value = dgv.Rows[i].Cells["edit_user"].Value;
                        

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
                this.Activate();
            }
            finally
            {
                setAutomatic(excelApp, true);
                //excelApp.ac
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
        }
        //Excel Sheet Basic
        private void BasicSheetsSetting(Excel.Application excel, Excel.Workbook wb, Excel.Worksheet wk, int rows)
        {
            workSheet.Name = "계약정보";

            //Column Width
            wk.Cells[1, 1].Value = "계약연도";
            wk.Cells[1, 2].Value = "ATO NO";
            wk.Cells[1, 3].Value = "PI DATE";
            wk.Cells[1, 4].Value = "CONTRACT NO";
            wk.Cells[1, 5].Value = "SHIPPER";
            wk.Cells[1, 6].Value = "L/C OPEN";
            wk.Cells[1, 7].Value = "L/C NO";
            wk.Cells[1, 8].Value = "B/L NO";
            wk.Cells[1, 9].Value = "계약선적일";
            wk.Cells[1, 10].Value = "ETD";
            wk.Cells[1, 11].Value = "ETA";
            wk.Cells[1, 12].Value = "창고입고예정";
            wk.Cells[1, 13].Value = "상태";
            wk.Cells[1, 14].Value = "통관예정일";
            wk.Cells[1, 15].Value = "창고";
            wk.Cells[1, 16].Value = "원산지";
            wk.Cells[1, 17].Value = "품명";
            wk.Cells[1, 18].Value = "사이즈";
            wk.Cells[1, 19].Value = "박스중량";
            wk.Cells[1, 20].Value = "트레이";
            wk.Cells[1, 21].Value = "단가";
            wk.Cells[1, 22].Value = "계약수량";
            wk.Cells[1, 23].Value = "입고수량";
            wk.Cells[1, 24].Value = "관세율(%)";
            wk.Cells[1, 25].Value = "부가세율(%)";
            wk.Cells[1, 26].Value = "비고";
            wk.Cells[1, 27].Value = "결제일자";
            wk.Cells[1, 28].Value = "결제확정여부";
            wk.Cells[1, 29].Value = "결제은행";
            wk.Cells[1, 30].Value = "유산스";
            wk.Cells[1, 31].Value = "구분";
            wk.Cells[1, 32].Value = "대행";
            wk.Cells[1, 33].Value = "HC, CO";
            wk.Cells[1, 34].Value = "담당자";
            wk.Cells[1, 35].Value = "수정자";
            wk.Cells[1, 36].Value = "수정일자";

            //Font Style
            /*wk.Columns["F"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            wk.Columns["F"].NumberFormatLocal = "#,##0";*/

            //Border Line Style
            Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1 + rows, 36]];
            rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
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






        #endregion

    }
}


