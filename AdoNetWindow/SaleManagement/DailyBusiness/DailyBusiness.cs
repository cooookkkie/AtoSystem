using AdoNetWindow.Common;
using AdoNetWindow.Model;
using AdoNetWindow.RecoveryPrincipal;
using AdoNetWindow.SEAOVER;
using AdoNetWindow.SEAOVER.GetSales;
using Libs.MultiHeaderGrid;
using Libs.Tools;
using Org.BouncyCastle.Asn1.Crmf;
using Repositories;
using Repositories.Config;
using Repositories.RecoveryPrincipal;
using Repositories.SalesPartner;
using Repositories.SEAOVER;
using Repositories.SEAOVER.Sales;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using Timer = System.Timers.Timer;

namespace AdoNetWindow.SaleManagement.DailyBusiness
{
    public partial class DailyBusiness : Form
    {
        ISeaoverCompanyRepository seaoverCompanyRepository = new SeaoverCompanyRepository();
        IRecoveryPrincipalPriceRepository recoveryPrincipalPriceRepository = new RecoveryPrincipalPriceRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        IDailyBusinessRepository dailyBusinessRepository = new DailyBusinessRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ISalesRepository salesRepository = new SalesRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        ContextMenuStrip m;
        DataTable companyDt = new DataTable();
        Dictionary<string, string> companyDIc = new Dictionary<string, string>();
        Dictionary<string, string> codeDIc = new Dictionary<string, string>();
        Dictionary<string, DataTable> productDIc = new Dictionary<string, DataTable>();
        Dictionary<string, Color> warehouseDIc = new Dictionary<string, Color>();

        Dictionary<string, DataTable> sheetDataDic = new Dictionary<string, DataTable>();
        Dictionary<string, int> sheetOrderDic = new Dictionary<string, int>();

        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        private Timer aTimer;
        private readonly double cycleTime = 1800000; //30분
        //private readonly double cycleTime = 10000; //10초
        //private readonly double cycleTime = 60000; //60초

        LoginCookie cookie;
        DataTable stockDt = null;

        Libs.MessageBox messageBox = new Libs.MessageBox();
        Libs.Tools.JsonCommon jsonCommon;

        public DailyBusiness(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
            this.dgvBusiness.Focus();
        }
        private void DailyBusiness_Load(object sender, EventArgs e)
        {
            //ChangeEditModeToOnPropertyChanged(dgvBusiness);
            //Disable Sorting for DataGridView
            foreach (DataGridViewColumn item in dgvBusiness.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //재고 프로시져 호출
            CallStockProcedure();
            stockDt = priceComparisonRepository.GetCostAccountingProductInfo("", "", "");
            //업체별호출
            CallProductProcedure();
            salesRepository.SetSeaoverId(um.seaover_id);
            //거래처 리스트
            companyDt = salesRepository.GetCompanyList();
            salesRepository.SetSeaoverId(um.seaover_id);

            //LoginCookie
            cookie = new LoginCookie(@"C:\Cookies\TEMP\DAILYBUSINESS\" + DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmm"), null);
            if (cookie.CheckDeleteBackup() && messageBox.Show(this, "한달이상 지난 백업분이 존재합니다. 삭제하겠습니까?.", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                cookie.DeleteBackup();


            jsonCommon = new Libs.Tools.JsonCommon(@"C:\Cookies\TEMP\DAILYBUSINESS\");
            //cookie.DailyTempWrite(this.dgvBusiness);

            this.ActiveControl = dgvBusiness;
            dgvBusiness.DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Regular);
            //거래처 자동완성 Data
            addCompanyItems(CompanyDataCollection, MyCompanyDataCollection);

            //최근 Temp파일데이터 불러오기
            SetHeaderStyleForDgvSale();
            GetJsonData();


            SetTimer();
            current_backuptime = DateTime.Now;
            aTimer.Start();
            /*aTimer.Stop();
            aTimer.Dispose();*/

            dgvBusiness.DoubleBuffered(true);
            dgvBusiness.DoInit();
            dgvBusiness.Push();
            this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
        }
        #region 자동저장
        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(cycleTime);

            // 이벤트 핸들러 연결
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

            // Timer에서 Elapsed 이벤트를 반복해서 발생
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        DateTime current_backuptime = DateTime.Now;
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            TimeSpan ts = DateTime.Now - current_backuptime;
            if (ts.Minutes > 10)
            {
                int document_id = 1;
                InsertJsonData();
                lbUpdatetime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                current_backuptime = DateTime.Now;
                lbUpdatetime.Update();
            }
        }
        #endregion

        #region Method
        private void AddSubTotal()
        {
            int sttIdx = 9999, endIdx = -1;
            if (dgvBusiness.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvBusiness.SelectedRows)
                {
                    if (row.Index <= sttIdx)
                        sttIdx = row.Index;
                    if (row.Index >= endIdx)
                        endIdx = row.Index;
                }
            }
            else if (dgvBusiness.SelectedCells.Count > 0)
            {
                string company = "";
                endIdx = dgvBusiness.SelectedCells[0].RowIndex - 1;
                for (int i = dgvBusiness.SelectedCells[0].RowIndex; i >= 0; i--)
                {
                    DataGridViewRow row = dgvBusiness.Rows[i];
                    if (string.IsNullOrEmpty(company) && row.Cells["company"].Value != null && !string.IsNullOrEmpty(row.Cells["company"].Value.ToString()))
                        company = row.Cells["company"].Value.ToString();
                    else if (!string.IsNullOrEmpty(company) && (row.Cells["company"].Value == null || string.IsNullOrEmpty(row.Cells["company"].Value.ToString())))
                        break;
                    if (sttIdx >= i)
                        sttIdx = i;

                }
            }

            if (sttIdx < 9999 && endIdx >= 0)
            {
                double total_qty = 0, total_weight = 0, total_sales_amount = 0, total_purchase_amount = 0;
                List<string> warehouseList = new List<string>();
                for (int i = sttIdx; i <= endIdx; i++)
                {
                    DataGridViewRow row = dgvBusiness.Rows[i];

                    double qty;
                    if (row.Cells["qty"].Value == null || !double.TryParse(Regex.Replace(row.Cells["qty"].Value.ToString(), @"[^0-9.]", ""), out qty))
                        qty = 0;
                    total_qty += qty;

                    double unit;
                    if (row.Cells["unit"].Value == null || !double.TryParse(Regex.Replace(row.Cells["unit"].Value.ToString(), @"[^0-9.]", ""), out unit))
                        unit = 0;
                    total_weight += (qty * unit);

                    double sale_amount;
                    if (row.Cells["sale_amount"].Value == null || !double.TryParse(row.Cells["sale_amount"].Value.ToString(), out sale_amount))
                        sale_amount = 0;
                    total_sales_amount += sale_amount;

                    double purchase_price;
                    if (row.Cells["purchase_price"].Value == null || !double.TryParse(row.Cells["purchase_price"].Value.ToString(), out purchase_price))
                        purchase_price = 0;
                    total_purchase_amount += purchase_price;

                    if (row.Cells["warehouse"].Value != null && !string.IsNullOrEmpty(row.Cells["warehouse"].Value.ToString()))
                    {
                        if (!warehouseList.Contains(row.Cells["warehouse"].Value.ToString()))
                            warehouseList.Add(row.Cells["warehouse"].Value.ToString());
                    }
                }

                dgvBusiness.Rows.Insert(endIdx + 1, 1);
                dgvBusiness.Rows[endIdx + 1].DefaultCellStyle.Font = new Font("굴림", 8, FontStyle.Bold);
                dgvBusiness.Rows[endIdx + 1].Cells["input_date"].Value = dgvBusiness.Rows[endIdx].Cells["input_date"].Value;
                dgvBusiness.Rows[endIdx + 1].Cells["sizes"].Value = "합계";
                dgvBusiness.Rows[endIdx + 1].Cells["qty"].Value = total_qty.ToString("#,##0");
                dgvBusiness.Rows[endIdx + 1].Cells["unit"].Value = total_weight.ToString("#,##0.0");
                dgvBusiness.Rows[endIdx + 1].Cells["sale_amount"].Value = total_sales_amount.ToString("#,##0");
                dgvBusiness.Rows[endIdx + 1].Cells["purchase_price"].Value = total_purchase_amount.ToString("#,##0");

                dgvBusiness.Rows[endIdx + 1].Cells["warehouse"].Value = "창고(" + warehouseList.Count.ToString("#,##0") + "곳)";

                dgvBusiness.Rows[endIdx + 1].Cells["purchase_company"].Value = "기업은행";
                dgvBusiness.Rows[endIdx + 1].Cells["remark"].Value = "103-096-00601-012";
            }


        }
        private void CallStockProcedure()
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (priceComparisonRepository.CallStoredProcedureSTOCK(user_id, eDate) == 0)
                {
                    messageBox.Show(this,"호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch (Exception e)
            {
                messageBox.Show(this,e.Message);
                this.Activate();
                return;
            }

        }
        private void LocationCurrentCell()
        {
            int max_index = 0;
            for (int i = dgvBusiness.Rows.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j < 6; j++)
                {
                    if (dgvBusiness.Rows[i].Cells[j].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[i].Cells[j].Value.ToString().Trim()))
                    {
                        max_index = i;
                        break;
                    }
                }
                if (max_index > 0)
                    break;
            }

            dgvBusiness.Focus();
            if (max_index + 2 < dgvBusiness.Rows.Count)
            {
                dgvBusiness.ClearSelection();
                if (max_index - 17 >= 0)
                    dgvBusiness.FirstDisplayedScrollingRowIndex = max_index - 17;
                else
                    dgvBusiness.FirstDisplayedScrollingRowIndex = 0;
                dgvBusiness.CurrentCell = dgvBusiness.Rows[max_index + 2].Cells["company"];

            }
            else
            {
                string input_date;
                DateTime input_date_dt;
                if (dgvBusiness.Rows.Count == 0)
                    return;
                if (dgvBusiness.Rows[dgvBusiness.Rows.Count - 1].Cells["input_date"].Value == null || !DateTime.TryParse(dgvBusiness.Rows[dgvBusiness.Rows.Count - 1].Cells["input_date"].Value.ToString(), out input_date_dt))
                    input_date_dt = DateTime.Now;
                input_date = input_date_dt.ToString("MM-dd");

                this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                for (int j = 0; j < 15; j++)
                {
                    int n = dgvBusiness.Rows.Add();
                    dgvBusiness.Rows[n].Cells["input_date"].Value = input_date;
                }
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                dgvBusiness.CurrentCell = dgvBusiness.Rows[max_index + 2].Cells["company"];
                dgvBusiness.FirstDisplayedScrollingRowIndex = dgvBusiness.Rows.Count - 1;
            }
        }
        private void InsertCompanyInfo(bool isMsg = true)
        {
            //유효성검사
            string company_code = lbCompanyCode.Text;
            if (company_code == "NULL" || string.IsNullOrEmpty(company_code))
            {
                if (isMsg)
                    messageBox.Show(this,"거래처를 먼저 선택해주세요.");
                this.Activate();
                return;
            }

            //MSG
            if (isMsg)
            {
                if (messageBox.Show(this,"[" + txtCompanyName.Text + "] 거래처의 추가정보가 저장됩니다", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();

            //저장여부 확인
            DataTable companyDt = companyRepository.GetCompanySaleInfo("", "", false, "", company_code);

            //유통
            string distribution = "";
            if (cbDstributionJapan.Checked)
                distribution += "\n" + cbDstributionJapan.Text;
            if (cbDstributionChinese.Checked)
                distribution += "\n" + cbDstributionChinese.Text;
            if (cbDstributionSchool.Checked)
                distribution += "\n" + cbDstributionSchool.Text;
            if (cbDstributionBuffet.Checked)
                distribution += "\n" + cbDstributionBuffet.Text;
            if (cbDstributionMart.Checked)
                distribution += "\n" + cbDstributionMart.Text;
            if (cbDstributionEtc.Checked)
                distribution += "\n" + txtDstributionEtc.Text;

            distribution = distribution.Trim().Replace("\n", "^");

            //저장되지 않았을때 
            if (companyDt.Rows.Count == 0)
            {
                DataTable seaoverDt = salesRepository.GetSaleCompany("", false, "", "", "", "", "", "", false, false, company_code);
                if (seaoverDt.Rows.Count == 0)
                {
                    messageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }

                CompanyModel model = new CompanyModel();
                model.id = commonRepository.GetNextId("t_company", "id");
                model.division = "매출처";
                model.group_name = "";
                model.name = seaoverDt.Rows[0]["거래처명"].ToString();
                model.registration_number = seaoverDt.Rows[0]["사업자번호"].ToString();
                model.ceo = seaoverDt.Rows[0]["대표자명"].ToString();
                model.tel = seaoverDt.Rows[0]["전화번호"].ToString();
                model.fax = seaoverDt.Rows[0]["팩스번호"].ToString();
                model.phone = seaoverDt.Rows[0]["휴대폰"].ToString();
                model.other_phone = seaoverDt.Rows[0]["기타연락처"].ToString();

                model.distribution = distribution;
                model.handling_item = txtInputInquire.Text;
                model.address = "";
                model.origin = "국내";
                model.ato_manager = seaoverDt.Rows[0]["매출자"].ToString();

                model.email = "";
                model.web = "";
                model.sns1 = "";
                model.sns2 = "";
                model.sns3 = "";
                model.company_manager = "";
                model.company_manager_position = "";
                model.seaover_company_code = seaoverDt.Rows[0]["거래처코드"].ToString();
                model.isManagement1 = false;
                model.isManagement2 = false;
                model.isManagement3 = false;
                model.isManagement4 = false;
                model.isHide = false;
                model.isDelete = false;
                model.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                model.updatetime = "1900-01-01 00:00:00";
                model.remark = "";
                model.remark2 = txtRemark.Text;
                model.remark3 = txtRemark2.Text;
                model.payment_date = txtPaymentDate.Text;

                model.isPotential1 = false;
                model.isPotential2 = false;

                model.isNonHandled = false;
                model.isNotSendFax = false;
                model.isOutBusiness = Convert.ToBoolean(seaoverDt.Rows[0]["폐업유무"].ToString());

                model.isTrading = true;
                model.edit_user = um.user_name;

                //거래처 정보 재등록
                sql = companyRepository.InsertCompany(model);
                sqlList.Add(sql);
            }

            //등록된 내역이 있을떄
            else
            {
                string[] col = new string[6];
                col[0] = "handling_item";
                col[1] = "distribution";
                col[2] = "remark2";
                col[3] = "remark3";
                col[4] = "payment_date";
                col[5] = "edit_user";
                //col[6] = "updatetime";
                string[] val = new string[6];
                val[0] = common.AddSlashes(txtInputInquire.Text);
                val[1] = distribution;
                val[2] = common.AddSlashes(txtRemark.Text);
                val[3] = common.AddSlashes(txtRemark2.Text);
                val[4] = common.AddSlashes(txtPaymentDate.Text);
                val[5] = um.user_name;
                //val[6] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                sql = commonRepository.UpdateData("t_company", col, val, $"seaover_company_code = '{company_code}'");
                sqlList.Add(sql);
            }
            //Execute
            if (sqlList.Count > 0)
            {
                if (commonRepository.UpdateTran(sqlList) == -1)
                    messageBox.Show(this,"수정중 에러가 발생하였습니다.");
                this.Activate();
            }
        }
        public void InputProduct(List<DataGridViewRow> list, bool is_new, bool is_active)
        {
            if (list.Count > 0)
            {
                this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                int idx;
                if (dgvBusiness.Rows.Count == 0)
                {
                    dgvBusiness.Rows.Add(10);
                    idx = 0;
                }
                else if (dgvBusiness.CurrentCell.RowIndex >= dgvBusiness.Rows.Count - 9)
                {

                    int diff_row = 10 - (dgvBusiness.Rows.Count - dgvBusiness.CurrentCell.RowIndex);

                    dgvBusiness.Rows.Add(diff_row);
                    dgvBusiness.FirstDisplayedScrollingRowIndex = dgvBusiness.Rows.Count - 1;
                    idx = 0;
                }
                if (dgvBusiness.SelectedCells.Count == 0)
                {
                    idx = dgvBusiness.Rows.Count - 1;
                    //dgvBusiness.Rows.Add(10);
                }
                else
                    idx = dgvBusiness.SelectedCells[0].RowIndex;

                string input_date = "";
                int pre_idx = idx;
                while (string.IsNullOrEmpty(input_date))
                {
                    if (pre_idx < 0)
                        input_date = DateTime.Now.ToString("MM-dd");
                    else if (dgvBusiness.Rows[pre_idx].Cells["input_date"].Value == null || string.IsNullOrEmpty(dgvBusiness.Rows[pre_idx].Cells["input_date"].Value.ToString()))
                        input_date = DateTime.Now.ToString("MM-dd");
                    else
                        input_date = dgvBusiness.Rows[pre_idx].Cells["input_date"].Value.ToString();
                    pre_idx--;
                }
                string company = "";
                pre_idx = idx;
                while (string.IsNullOrEmpty(company))
                {
                    if (pre_idx < 0)
                        company = " ";
                    else if (dgvBusiness.Rows[pre_idx].Cells["company"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[pre_idx].Cells["company"].Value.ToString()))
                        company = dgvBusiness.Rows[pre_idx].Cells["company"].Value.ToString();

                    pre_idx--;
                }
                company = company.Trim();
                //데이터 출력
                for (int i = 0; i < list.Count; i++)
                {
                    dgvBusiness.Rows[idx + i].Cells["company"].Value = company;
                    dgvBusiness.Rows[idx + i].Cells["input_date"].Value = input_date;
                    dgvBusiness.Rows[idx + i].Cells["product"].Value = list[i].Cells[1].Value;
                    dgvBusiness.Rows[idx + i].Cells["origin"].Value = list[i].Cells[2].Value;
                    dgvBusiness.Rows[idx + i].Cells["sizes"].Value = list[i].Cells[3].Value;
                    dgvBusiness.Rows[idx + i].Cells["unit"].Value = list[i].Cells[8].Value;

                    double sale_price;
                    if (list[i].Cells[4].Value == null || !double.TryParse(list[i].Cells[9].Value.ToString(), out sale_price))
                        sale_price = 0;

                    dgvBusiness.Rows[idx + i].Cells["sale_price"].Value = sale_price.ToString("#,##0");

                    double purchase_price;
                    if (list[i].Cells[5].Value == null || !double.TryParse(list[i].Cells[10].Value.ToString(), out purchase_price))
                        purchase_price = 0;

                    dgvBusiness.Rows[idx + i].Cells["purchase_price"].Value = purchase_price.ToString("#,##0");
                    dgvBusiness.Rows[idx + i].Cells["purchase_company"].Value = list[i].Cells[11].Value;
                    dgvBusiness.Rows[idx + i].Cells["warehouse"].Value = list[i].Cells[12].Value;
                }

                //다음행 셀렉트
                dgvBusiness.ClearSelection();
                if (idx + list.Count > dgvBusiness.Rows.Count - 1)
                {
                    int n = dgvBusiness.Rows.Add();
                    dgvBusiness.Rows[n].Cells["input_date"].Value = input_date;
                }
                else if (dgvBusiness.Rows[idx + list.Count].Cells["product"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx + list.Count].Cells["product"].Value.ToString()))
                {
                    dgvBusiness.Rows.Insert(idx + list.Count, 1);
                    dgvBusiness.Rows[idx + list.Count].Cells["input_date"].Value = input_date;
                }

                dgvBusiness.Rows[idx + list.Count].Cells["product"].Selected = true;
                /*int scroll_idx = idx - 10;
                if (scroll_idx < 0)
                    scroll_idx = 0;
                dgvBusiness.FirstDisplayedScrollingRowIndex = scroll_idx;*/
                dgvBusiness.CurrentCell = dgvBusiness.Rows[idx + list.Count].Cells["qty"];
                if (is_active)
                    this.Activate();
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
            }
        }
        public void InputProduct2(List<DataGridViewRow> list, bool is_new, bool is_active)
        {
            if (list.Count > 0)
            {
                
                this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);

                int idx;
                if (dgvBusiness.SelectedCells.Count == 0)
                    idx = dgvBusiness.Rows.Count - 1;
                else if (dgvBusiness.Rows.Count == 0)
                {
                    dgvBusiness.Rows.Add();
                    idx = 0;
                }
                else
                    idx = dgvBusiness.SelectedCells[0].RowIndex;

                string input_date = "";
                int pre_idx = idx;
                while (string.IsNullOrEmpty(input_date))
                {
                    if (pre_idx < 0)
                        input_date = DateTime.Now.ToString("MM-dd");
                    else if (dgvBusiness.Rows[pre_idx].Cells["input_date"].Value == null || string.IsNullOrEmpty(dgvBusiness.Rows[pre_idx].Cells["input_date"].Value.ToString()))
                        input_date = DateTime.Now.ToString("MM-dd");
                    else
                        input_date = dgvBusiness.Rows[pre_idx].Cells["input_date"].Value.ToString();
                    pre_idx--;
                }
                pre_idx = idx;

                string company = txtCompanyName.Text;
                company = company.Trim();
                bool isInsert = false;

                //데이터 출력
                if (dgvBusiness.Rows[idx].Cells["company"].Value == null)
                    dgvBusiness.Rows[idx].Cells["company"].Value = string.Empty;
                if (dgvBusiness.Rows[idx].Cells["product"].Value == null)
                    dgvBusiness.Rows[idx].Cells["product"].Value = string.Empty;
                if (dgvBusiness.Rows[idx].Cells["origin"].Value == null)
                    dgvBusiness.Rows[idx].Cells["origin"].Value = string.Empty;
                if (dgvBusiness.Rows[idx].Cells["sizes"].Value == null)
                    dgvBusiness.Rows[idx].Cells["sizes"].Value = string.Empty;
                if (dgvBusiness.Rows[idx].Cells["unit"].Value == null)
                    dgvBusiness.Rows[idx].Cells["unit"].Value = string.Empty;

                //같은 품목이면 수량만 다르게
                if (dgvBusiness.Rows[idx].Cells["company"].Value.ToString() == company
                    && dgvBusiness.Rows[idx].Cells["product"].Value.ToString() == list[0].Cells[1].Value.ToString()
                    && dgvBusiness.Rows[idx].Cells["origin"].Value.ToString() == list[0].Cells[2].Value.ToString()
                    && dgvBusiness.Rows[idx].Cells["sizes"].Value.ToString() == list[0].Cells[3].Value.ToString()
                    && dgvBusiness.Rows[idx].Cells["unit"].Value.ToString() == list[0].Cells[4].Value.ToString()
                    && list.Count == 1)
                {
                    double qty;
                    if (list[0].Cells[6].Value == null || !double.TryParse(list[0].Cells[6].Value.ToString(), out qty))
                        qty = 0;
                    dgvBusiness.Rows[idx].Cells["qty"].Value = qty.ToString("#,##0");

                    double sale_price;
                    if (dgvBusiness.Rows[idx].Cells["sale_price"].Value == null || !double.TryParse(dgvBusiness.Rows[idx].Cells["sale_price"].Value.ToString(), out sale_price))
                        sale_price = 0;

                    double unit_count;
                    if (list[0].Cells[15].Value == null || !double.TryParse(list[0].Cells[15].Value.ToString(), out unit_count))
                        unit_count = 1;

                    sale_price *= unit_count;

                    dgvBusiness.Rows[idx].Cells["sale_amount"].Value = (sale_price * qty).ToString("#,##0");
                }
                else
                {
                    if (dgvBusiness.Rows[idx].Cells["product"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx].Cells["product"].Value.ToString())
                        || dgvBusiness.Rows[idx].Cells["sizes"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx].Cells["sizes"].Value.ToString()))
                        idx++;

                    for (int i = 0; i < list.Count; i++)
                    {
                        dgvBusiness.SetUndoRedo(true);
                        if (idx > dgvBusiness.Rows.Count - 1)
                            dgvBusiness.Rows.Add();
                        else
                        {
                            for (int j = 1; j < dgvBusiness.Columns.Count; j++)
                            {
                                if (dgvBusiness.Rows[idx + 1].Cells[j].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx + 1].Cells[j].Value.ToString()))
                                {
                                    dgvBusiness.Rows.Insert(idx, 1);
                                    isInsert = true;
                                    break;
                                }
                            }
                        }
                        if (dgvBusiness.Rows[idx].Cells["product"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx].Cells["product"].Value.ToString())
                            || dgvBusiness.Rows[idx].Cells["sizes"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx].Cells["sizes"].Value.ToString()))
                        {
                            dgvBusiness.Rows.Insert(idx, 1);
                            isInsert = true;
                        }
                        dgvBusiness.Rows[idx + i].Cells["company"].Value = company;
                        dgvBusiness.Rows[idx + i].Cells["input_date"].Value = input_date;
                        
                        dgvBusiness.Rows[idx + i].Cells["origin"].Value = list[i].Cells[1].Value;
                        dgvBusiness.Rows[idx + i].Cells["sizes"].Value = list[i].Cells[3].Value;
                        dgvBusiness.Rows[idx + i].Cells["unit"].Value = list[i].Cells[4].Value;
                        double qty;
                        if (list[i].Cells[6].Value == null || !double.TryParse(list[i].Cells[6].Value.ToString(), out qty))
                            qty = 0;
                        dgvBusiness.Rows[idx + i].Cells["qty"].Value = qty.ToString("#,##0");
                        double pre_sale_price;
                        if (list[i].Cells[7].Value == null || !double.TryParse(list[i].Cells[7].Value.ToString(), out pre_sale_price))
                            pre_sale_price = 0;
                        double sale_price;
                        if (list[i].Cells[8].Value == null || !double.TryParse(list[i].Cells[8].Value.ToString(), out sale_price))
                            sale_price = 0;
                        double unit_count;
                        if (list[i].Cells[15].Value == null || !double.TryParse(list[i].Cells[15].Value.ToString(), out unit_count))
                            unit_count = 1;

                        sale_price *= unit_count;
                        pre_sale_price *= unit_count;


                        if (list[i].Cells["input_qty"].Style.BackColor == Color.Beige)
                        {
                            dgvBusiness.Rows[idx + i].Cells["sale_price"].Value = pre_sale_price.ToString("#,##0");
                            dgvBusiness.Rows[idx + i].Cells["sale_amount"].Value = (pre_sale_price * qty).ToString("#,##0");
                        }
                        else
                        {
                            dgvBusiness.Rows[idx + i].Cells["sale_price"].Value = sale_price.ToString("#,##0");
                            dgvBusiness.Rows[idx + i].Cells["sale_amount"].Value = (sale_price * qty).ToString("#,##0");
                        }
                        //stack 저장
                        dgvBusiness.SetUndoRedo(false);
                        dgvBusiness.Rows[idx + i].Cells["product"].Value = list[i].Cells[2].Value;
                    }
                }
                //다음행 셀렉트
                dgvBusiness.ClearSelection();
                if (idx + list.Count > dgvBusiness.Rows.Count - 1)
                {
                    int n = dgvBusiness.Rows.Add();
                    dgvBusiness.Rows[n].Cells["input_date"].Value = input_date;
                }
                dgvBusiness.Rows[idx + list.Count - 1].Cells["product"].Selected = true;
                dgvBusiness.Rows[idx + list.Count].Cells["product"].Selected = true;
                //시작위치
                if (is_active)
                    this.Activate();
                if (isInsert)
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[idx].Cells["product"];
                else
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[idx + list.Count].Cells["product"];
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            }
        }
        public void InputProduct3(List<DataGridViewRow> list, bool is_new, bool is_active)
        {
            if (list.Count > 0)
            {
                this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);

                int idx;
                if (dgvBusiness.SelectedCells.Count == 0)
                    idx = dgvBusiness.Rows.Count - 1;
                else if (dgvBusiness.Rows.Count == 0)
                {
                    dgvBusiness.Rows.Add();
                    idx = 0;
                }
                else
                    idx = dgvBusiness.SelectedCells[0].RowIndex;

                string input_date = "";
                int pre_idx = idx;
                while (string.IsNullOrEmpty(input_date))
                {
                    if (pre_idx < 0)
                        input_date = DateTime.Now.ToString("MM-dd");
                    else if (dgvBusiness.Rows[pre_idx].Cells["input_date"].Value == null || string.IsNullOrEmpty(dgvBusiness.Rows[pre_idx].Cells["input_date"].Value.ToString()))
                        input_date = DateTime.Now.ToString("MM-dd");
                    else
                        input_date = dgvBusiness.Rows[pre_idx].Cells["input_date"].Value.ToString();
                    pre_idx--;
                }
                string company = "";
                pre_idx = idx;
                while (string.IsNullOrEmpty(company))
                {
                    if (pre_idx < 0)
                        company = " ";
                    else if (dgvBusiness.Rows[pre_idx].Cells["company"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[pre_idx].Cells["company"].Value.ToString()))
                        company = dgvBusiness.Rows[pre_idx].Cells["company"].Value.ToString();

                    pre_idx--;
                }
                company = company.Trim();

                //데이터 출력
                if (dgvBusiness.Rows[idx].Cells["product"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx].Cells["product"].Value.ToString())
                    || dgvBusiness.Rows[idx].Cells["sizes"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx].Cells["sizes"].Value.ToString()))
                    idx++;
                bool isInsert = false;
                for (int i = 0; i < list.Count; i++)
                {
                    //if (idx + i > dgvBusiness.Rows.Count - 1)
                    //dgvBusiness.Rows.Add();

                    if (idx > dgvBusiness.Rows.Count - 1)
                        dgvBusiness.Rows.Add();
                    if (dgvBusiness.Rows[idx].Cells["product"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx].Cells["product"].Value.ToString())
                        || dgvBusiness.Rows[idx].Cells["sizes"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[idx].Cells["sizes"].Value.ToString()))
                    {
                        dgvBusiness.Rows.Insert(idx, 1);
                        isInsert = true;
                    }

                    dgvBusiness.Rows[idx + i].Cells["company"].Value = company;
                    dgvBusiness.Rows[idx + i].Cells["input_date"].Value = input_date;
                    dgvBusiness.Rows[idx + i].Cells["product"].Value = list[i].Cells["product"].Value;
                    dgvBusiness.Rows[idx + i].Cells["origin"].Value = list[i].Cells["origin"].Value;
                    dgvBusiness.Rows[idx + i].Cells["sizes"].Value = list[i].Cells["sizes"].Value;
                    dgvBusiness.Rows[idx + i].Cells["unit"].Value = list[i].Cells["unit"].Value;
                    dgvBusiness.Rows[idx + i].Cells["qty"].Value = list[i].Cells["input_qty"].Value.ToString();
                    double qty;
                    if (list[i].Cells["input_qty"].Value == null || !double.TryParse(Regex.Replace(list[i].Cells["input_qty"].Value.ToString(), @"[^0-9.]", ""), out qty))
                        qty = 0;
                    double sale_price;
                    if (!double.TryParse(list[i].Cells["sale_price"].Value.ToString(), out sale_price))
                        sale_price = 0;
                    dgvBusiness.Rows[idx + i].Cells["sale_price"].Value = sale_price.ToString("#,##0");
                    dgvBusiness.Rows[idx + i].Cells["sale_amount"].Value = (sale_price * qty).ToString("#,##0");
                    dgvBusiness.Rows[idx + i].Cells["purchase_price"].Value = list[i].Cells["purchase_price"].Value;
                }
                //다음행 셀렉트
                dgvBusiness.ClearSelection();
                if (idx + list.Count > dgvBusiness.Rows.Count - 1)
                {
                    int n = dgvBusiness.Rows.Add();
                    dgvBusiness.Rows[n].Cells["input_date"].Value = input_date;
                }
                dgvBusiness.Rows[idx + list.Count - 1].Cells["product"].Selected = true;
                dgvBusiness.Rows[idx + list.Count].Cells["product"].Selected = true;
                //시작위치
                if (is_active)
                    this.Activate();
                if (isInsert)
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[idx].Cells["sale_price"];
                else
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[idx + list.Count].Cells["sale_price"];
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            }
        }
        private void SearchingSaleProduct()
        {
            for (int i = dgvSales.Rows.Count - 1; i >= 0; i--)
            {
                if (dgvSales.Rows[i].Cells["input_qty"].Style.BackColor != Color.Beige)
                    dgvSales.Rows.RemoveAt(i);
            }

            if (dgvSales.Rows.Count > 0)
            {
                string[] product = txtSaleProduct.Text.Trim().Split(' ');
                string[] origin = txtSaleOrigin.Text.Trim().Split(' ');
                string[] sizes = txtSaleSizes.Text.Trim().Split(' ');
                string[] unit = txtSaleUnit.Text.Trim().Split(' ');
                List<string> data;

                int visbleCnt = 0;

                for (int i = 0; i < dgvSales.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvSales.Rows[i];

                    bool isOutput = false;
                    if (product.Length == 1 && string.IsNullOrEmpty(product[0]))
                        isOutput = true;
                    else
                    {
                        for (int j = 0; j < product.Length; j++)
                        {
                            if (common.isContains(row.Cells["product_name"].Value.ToString(), product[j]))
                            {
                                isOutput = true;
                                break;
                            }
                        }
                    }
                    if (isOutput)
                    {
                        if (origin.Length == 1 && string.IsNullOrEmpty(origin[0]))
                            isOutput = true;
                        else
                        {
                            isOutput = false;
                            for (int j = 0; j < origin.Length; j++)
                            {
                                if (common.isContains(row.Cells["origin_name"].Value.ToString(), origin[j]))
                                {
                                    isOutput = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isOutput)
                    {
                        if (sizes.Length == 1 && string.IsNullOrEmpty(sizes[0]))
                            isOutput = true;
                        else
                        {
                            isOutput = false;
                            for (int j = 0; j < sizes.Length; j++)
                            {
                                if (common.isContains(row.Cells["sizes_name"].Value.ToString(), sizes[j]))
                                {
                                    isOutput = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isOutput)
                    {
                        if (unit.Length == 1 && string.IsNullOrEmpty(unit[0]))
                            isOutput = true;
                        else
                        {
                            isOutput = false;
                            for (int j = 0; j < unit.Length; j++)
                            {
                                if (common.isContains(row.Cells["unit_name"].Value.ToString(), unit[j]))
                                {
                                    isOutput = true;
                                    break;
                                }
                            }
                        }
                    }
                    row.Visible = isOutput;
                    if(isOutput)
                        visbleCnt++;
                }
                //검색내역이 없을때
                if (visbleCnt == 0)
                    GetNoneHandledProduct();
            }
        }
        private void CellCounting()
        {
            txtTotalCount.Text = "0";
            txtTotalQty.Text = "0";
            txtTotalWeight.Text = "0";
            txtTotalAmount.Text = "0";
            if (dgvBusiness.SelectedCells.Count > 0)
            {
                double cnt = 0, qty = 0, sale_amount = 0, purchase_amount = 0, weight = 0;
                int idx = dgvBusiness.SelectedCells[0].RowIndex;

                List<int> list = new List<int>();
                foreach (DataGridViewCell cell in dgvBusiness.SelectedCells)
                {
                    //선택한 품목수
                    if (!list.Contains(cell.RowIndex))
                    {
                        cnt++;

                        list.Add(cell.RowIndex);
                    }

                    //총 수량
                    if (dgvBusiness.Columns[cell.ColumnIndex].Name == "qty")
                    {
                        double d;
                        if (cell.Value == null || !double.TryParse(Regex.Replace(cell.Value.ToString(), @"[^0-9.]", ""), out d))
                            d = 0;
                        qty += d;
                    }
                    //총 중량
                    if (dgvBusiness.Columns[cell.ColumnIndex].Name == "unit")
                    {
                        double d1;
                        if (dgvBusiness.Rows[cell.RowIndex].Cells["qty"].Value == null || !double.TryParse(Regex.Replace(dgvBusiness.Rows[cell.RowIndex].Cells["qty"].Value.ToString(), @"[^0-9.]", ""), out d1))
                            d1 = 0;
                        double d2;
                        if (cell.Value == null || !double.TryParse(Regex.Replace(cell.Value.ToString(), @"[^0-9.]", ""), out d2))
                            d2 = 0;
                        weight += d1 * d2;
                    }
                    //총 매출금액
                    else if (dgvBusiness.Columns[cell.ColumnIndex].Name == "sale_price" || dgvBusiness.Columns[cell.ColumnIndex].Name == "sale_amount")
                    {
                        if ((dgvBusiness.Rows[cell.RowIndex].Cells["sale_price"].Selected && dgvBusiness.Rows[cell.RowIndex].Cells["sale_amount"].Selected)
                            || (!dgvBusiness.Rows[cell.RowIndex].Cells["sale_price"].Selected && dgvBusiness.Rows[cell.RowIndex].Cells["sale_amount"].Selected))
                        {
                            //두번 더 해지는거 방지
                            if (dgvBusiness.Columns[cell.ColumnIndex].Name == "sale_amount")
                            {
                                double d;
                                if (dgvBusiness.Rows[cell.RowIndex].Cells["sale_amount"].Value == null || !double.TryParse(dgvBusiness.Rows[cell.RowIndex].Cells["sale_amount"].Value.ToString(), out d))
                                    d = 0;
                                sale_amount += d;
                            }
                        }
                        else if (dgvBusiness.Rows[cell.RowIndex].Cells["sale_price"].Selected && !dgvBusiness.Rows[cell.RowIndex].Cells["sale_amount"].Selected)
                        {
                            double d;
                            if (dgvBusiness.Rows[cell.RowIndex].Cells["sale_price"].Value == null || !double.TryParse(dgvBusiness.Rows[cell.RowIndex].Cells["sale_price"].Value.ToString(), out d))
                                d = 0;
                            sale_amount += d;
                        }
                        else if (!dgvBusiness.Rows[cell.RowIndex].Cells["sale_price"].Selected && dgvBusiness.Rows[cell.RowIndex].Cells["sale_amount"].Selected)
                        {
                            double d;
                            if (dgvBusiness.Rows[cell.RowIndex].Cells["sale_amount"].Value == null || !double.TryParse(dgvBusiness.Rows[cell.RowIndex].Cells["sale_amount"].Value.ToString(), out d))
                                d = 0;
                            sale_amount += d;
                        }
                    }
                    //총 매입금액
                    else if (dgvBusiness.Columns[cell.ColumnIndex].Name == "purchase_price" || dgvBusiness.Columns[cell.ColumnIndex].Name == "purchase_price")
                    {
                        double d;
                        if (cell.Value == null || !double.TryParse(cell.Value.ToString(), out d))
                            d = 0;
                        purchase_amount += d;
                    }

                }

                txtTotalCount.Text = cnt.ToString("#,##0");
                txtTotalQty.Text = qty.ToString("#,##0");
                txtTotalWeight.Text = weight.ToString("N2");
                if (sale_amount > 0)
                    txtTotalAmount.Text = sale_amount.ToString("#,##0");
                else
                    txtTotalAmount.Text = purchase_amount.ToString("#,##0");
            }
        }
        public bool SearchingTxt(string find, string except)
        {
            if (!string.IsNullOrEmpty(find))
            {
                string[] find_txt = find.Split(',');
                string[] except_txt = except.Split(',');

                int rowidx = 0;
                if (dgvBusiness.SelectedCells.Count > 0)
                    rowidx = dgvBusiness.SelectedCells[0].RowIndex;
                if (dgvBusiness.Rows.Count > 0)
                {
                    
                    //현재행부터 끝까지
                    for (int i = rowidx; i < dgvBusiness.Rows.Count; i++)
                    {
                        
                        DataGridViewCell cell = null;
                        if (i == rowidx)
                        {
                            bool is_find = false;
                            for (int j = dgvBusiness.SelectedCells[0].ColumnIndex; j < dgvBusiness.ColumnCount; j++)
                            {
                                if (dgvBusiness.Rows[i].Cells[j].Visible && dgvBusiness.Rows[i].Cells[j] != dgvBusiness.SelectedCells[0])
                                {
                                    cell = dgvBusiness.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        
                                        string val = cell.Value.ToString();
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
                                            dgvBusiness.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                            if (!is_find)
                            {
                                for (int j = 0; j < dgvBusiness.ColumnCount; j++)
                                {
                                    if (dgvBusiness.Rows[i].Cells[j].Visible && dgvBusiness.Rows[i].Cells[j] != dgvBusiness.SelectedCells[0])
                                    {
                                        cell = dgvBusiness.Rows[i].Cells[j];
                                        if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                        {
                                            string val = cell.Value.ToString();
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
                                                dgvBusiness.CurrentCell = cell;
                                                return true;
                                            }
                                        }
                                    }
                                }
                            }
                            
                        }
                        else
                        {
                            for (int j = 0; j < dgvBusiness.ColumnCount; j++)
                            {
                                if (dgvBusiness.Rows[i].Cells[j].Visible && dgvBusiness.Rows[i].Cells[j] != dgvBusiness.SelectedCells[0])
                                {
                                    cell = dgvBusiness.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString();
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
                                            dgvBusiness.CurrentCell = cell;
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
                            for (int j = 0; j < dgvBusiness.SelectedCells[0].ColumnIndex; j++)
                                //for (int j = dgvBusiness.SelectedCells[0].ColumnIndex; j < dgvBusiness.ColumnCount; j++)
                            {
                                if (dgvBusiness.Rows[i].Cells[j].Visible && dgvBusiness.Rows[i].Cells[j] != dgvBusiness.SelectedCells[0])
                                {
                                    cell = dgvBusiness.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString();
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
                                            dgvBusiness.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < dgvBusiness.ColumnCount; j++)
                            {
                                if (dgvBusiness.Rows[i].Cells[j].Visible && dgvBusiness.Rows[i].Cells[j] != dgvBusiness.SelectedCells[0])
                                {
                                    cell = dgvBusiness.Rows[i].Cells[j];
                                    if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                                    {
                                        bool is_find = false;
                                        string val = cell.Value.ToString();
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
                                            dgvBusiness.CurrentCell = cell;
                                            return true;
                                        }
                                    }
                                }
                            }
                        }


                        if (cell != null && cell.Value != null && !string.IsNullOrEmpty(cell.Value.ToString()))
                        {
                            bool is_find = false;
                            string val = cell.Value.ToString();
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
                                dgvBusiness.CurrentCell = cell;
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
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
                    messageBox.Show(this,"호출 내용이 없음");
                this.Activate();
            }
            catch (Exception e)
            {
                messageBox.Show(this,e.Message);
                this.Activate();
            }

        }
        public void GetSalesProduct(string company_code = "")
        {
            //매출내역 가져오기
            if (company_code != null && !string.IsNullOrEmpty(company_code))
            {
                //초기화
                dgvSales.Rows.Clear();
                lbCompanyCode.Text = "";

                DataTable salesDt;
                //매출품목Dic 추가
                if (!productDIc.ContainsKey(company_code))
                {
                    salesDt = salesRepository.GetHandlingProductDashboard(company_code);
                    if (salesDt.Rows.Count > 0)
                        productDIc.Add(company_code, salesDt);
                    else
                        return;
                }
                else
                    salesDt = productDIc[company_code];

                txtCompanyName.Text = salesDt.Rows[0]["매출처"].ToString();

                //거래처 문의품목 가져오기
                DataTable comDt = companyRepository.GetCompanySaleInfo("", "", false, "", company_code);
                if (comDt.Rows.Count > 0)
                {
                    txtInputInquire.Text = comDt.Rows[0]["handling_item"].ToString();
                    txtRemark.Text = comDt.Rows[0]["remark2"].ToString();
                    txtRemark2.Text = comDt.Rows[0]["remark3"].ToString();

                    string[] distribution = comDt.Rows[0]["distribution"].ToString().Trim().Split('^');
                    if (distribution.Length > 0)
                    {
                        for (int i = 0; i < distribution.Length; i++)
                        {
                            switch (distribution[i].Trim())
                            {
                                case "일식":
                                    cbDstributionJapan.Checked = true;
                                    break;
                                case "중식":
                                    cbDstributionChinese.Checked = true;
                                    break;
                                case "급식":
                                    cbDstributionSchool.Checked = true;
                                    break;
                                case "뷔페":
                                    cbDstributionBuffet.Checked = true;
                                    break;
                                case "마트":
                                    cbDstributionMart.Checked = true;
                                    break;
                                default:
                                    cbDstributionEtc.Checked = true;
                                    txtDstributionEtc.Text += " " + distribution[i].ToString();
                                    break;
                            }
                        }

                        cbDstributionEtc.Text = cbDstributionEtc.Text.Trim();
                    }
                }
                else
                {
                    txtInputInquire.Text = String.Empty;
                    txtRemark.Text = String.Empty;

                    cbDstributionJapan.Checked = false;
                    cbDstributionChinese.Checked = false;
                    cbDstributionSchool.Checked = false;
                    cbDstributionBuffet.Checked = false;
                    cbDstributionMart.Checked = false;
                    cbDstributionEtc.Checked = false;
                    txtDstributionEtc.Text = String.Empty;


                }

                //데이터 출력
                if (salesDt.Rows.Count > 0)
                {
                    for (int i = 0; i < salesDt.Rows.Count; i++)
                    {
                        int n = dgvSales.Rows.Add();
                        DataGridViewRow row = dgvSales.Rows[n];
                        DateTime sale_date;
                        if (DateTime.TryParse(salesDt.Rows[i]["매출일자"].ToString(), out sale_date))
                            row.Cells["current_sale_date"].Value = sale_date.ToString("yy-MM-dd");
                        row.Cells["product_name"].Value = salesDt.Rows[i]["품명"].ToString();
                        row.Cells["origin_name"].Value = salesDt.Rows[i]["원산지"].ToString();
                        row.Cells["sizes_name"].Value = salesDt.Rows[i]["규격"].ToString();
                        row.Cells["unit_name"].Value = salesDt.Rows[i]["단위"].ToString();
                        double sale_price;
                        if (!double.TryParse(salesDt.Rows[i]["전매출단가"].ToString(), out sale_price))
                            sale_price = 0;
                        row.Cells["pre_sale_price"].Value = sale_price.ToString("#,##0");

                        if (!double.TryParse(salesDt.Rows[i]["현매출단가"].ToString(), out sale_price))
                            sale_price = 0;
                        row.Cells["current_sale_price"].Value = sale_price.ToString("#,##0");
                        row.Cells["sale_company"].Value = salesDt.Rows[i]["매출처"].ToString();


                        double total_sale_qty;
                        if (!double.TryParse(salesDt.Rows[i]["매출수"].ToString(), out total_sale_qty))
                            total_sale_qty = 0;
                        row.Cells["total_sale_qty"].Value = total_sale_qty.ToString("#,##0");
                        double average_sale_qty;
                        if (!double.TryParse(salesDt.Rows[i]["평균발주량"].ToString(), out average_sale_qty))
                            average_sale_qty = 0;
                        row.Cells["average_sale_qty"].Value = average_sale_qty.ToString("#,##0");

                    }
                    //dgvSales.Focus();
                    dgvSales.CurrentCell = dgvSales.Rows[0].Cells["input_qty"];
                }
            }
        }
        public void SetCompany(string company_code = "", DataGridViewCell cell = null)
        {
            //초기화
            txtInputInquire.Text = String.Empty;
            txtRemark.Text = String.Empty;
            txtRemark2.Text = String.Empty;
            txtPaymentDate.Text = String.Empty;
            cbDstributionJapan.Checked = false;
            cbDstributionChinese.Checked = false;
            cbDstributionSchool.Checked = false;
            cbDstributionBuffet.Checked = false;
            cbDstributionMart.Checked = false;
            cbDstributionEtc.Checked = false;
            txtDstributionEtc.Text = String.Empty;

            //매출내역 가져오기
            if (company_code != null && !string.IsNullOrEmpty(company_code))
            {
                DataTable salesDt;
                //매출품목Dic 추가
                if (!productDIc.ContainsKey(company_code))
                {
                    salesDt = salesRepository.GetHandlingProductDashboard(company_code);
                    if (salesDt.Rows.Count > 0)
                        productDIc.Add(company_code, salesDt);
                    else
                        return;
                }
                else
                    salesDt = productDIc[company_code];

                //txtCompanyName.Text = salesDt.Rows[0]["매출처"].ToString();

                if (cell != null)
                    cell.Value = salesDt.Rows[0]["매출처"].ToString();

                //거래처 문의품목 가져오기
                DataTable comDt = companyRepository.GetCompanySaleInfo("", "", false, "", company_code);
                if (comDt.Rows.Count > 0)
                {
                    txtInputInquire.Text = comDt.Rows[0]["handling_item"].ToString();
                    txtRemark.Text = comDt.Rows[0]["remark2"].ToString();
                    txtRemark2.Text = comDt.Rows[0]["remark3"].ToString();
                    txtPaymentDate.Text = comDt.Rows[0]["payment_date"].ToString();
                    string[] distribution = comDt.Rows[0]["distribution"].ToString().Trim().Split('^');
                    if (distribution.Length > 0)
                    {
                        for (int i = 0; i < distribution.Length; i++)
                        {
                            switch (distribution[i].Trim())
                            {
                                case "일식":
                                    cbDstributionJapan.Checked = true;
                                    break;
                                case "중식":
                                    cbDstributionChinese.Checked = true;
                                    break;
                                case "급식":
                                    cbDstributionSchool.Checked = true;
                                    break;
                                case "뷔페":
                                    cbDstributionBuffet.Checked = true;
                                    break;
                                case "마트":
                                    cbDstributionMart.Checked = true;
                                    break;
                                case "":
                                    break;
                                default:
                                    cbDstributionEtc.Checked = true;
                                    txtDstributionEtc.Text += " " + distribution[i].ToString();
                                    break;
                            }
                        }

                        cbDstributionEtc.Text = cbDstributionEtc.Text.Trim();
                    }
                }

                //데이터 출력
                if (salesDt.Rows.Count > 0)
                {
                    for (int i = 0; i < salesDt.Rows.Count; i++)
                    {
                        int n = dgvSales.Rows.Add();
                        DataGridViewRow row = dgvSales.Rows[n];
                        DateTime sale_date;
                        if (DateTime.TryParse(salesDt.Rows[i]["매출일자"].ToString(), out sale_date))
                            row.Cells["current_sale_date"].Value = sale_date.ToString("yy-MM-dd");
                        row.Cells["product_name"].Value = salesDt.Rows[i]["품명"].ToString();
                        row.Cells["origin_name"].Value = salesDt.Rows[i]["원산지"].ToString();
                        row.Cells["sizes_name"].Value = salesDt.Rows[i]["규격"].ToString();
                        row.Cells["unit_name"].Value = salesDt.Rows[i]["단위"].ToString();


                        double stock = 0;
                        if (stockDt != null && stockDt.Rows.Count > 0)
                        {
                            string whr = $"품명 = '{salesDt.Rows[i]["품명"].ToString()}'"
                                        + $" AND 원산지 = '{salesDt.Rows[i]["원산지"].ToString()}'"
                                        + $" AND 규격 = '{salesDt.Rows[i]["규격"].ToString()}'"
                                        + $" AND 단위 = '{salesDt.Rows[i]["단위"].ToString().Replace("kg", "").Replace("KG", "").Replace("Kg", "")}'";

                            DataRow[] dr = stockDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                if (!double.TryParse(dr[0]["재고수"].ToString(), out stock))
                                    stock = 0;
                            }
                        }
                        row.Cells["stock"].Value = stock.ToString("#,##0");


                        double sale_price;
                        if (!double.TryParse(salesDt.Rows[i]["전매출단가"].ToString(), out sale_price))
                            sale_price = 0;
                        row.Cells["pre_sale_price"].Value = sale_price.ToString("#,##0");

                        if (!double.TryParse(salesDt.Rows[i]["현매출단가"].ToString(), out sale_price))
                            sale_price = 0;
                        row.Cells["current_sale_price"].Value = sale_price.ToString("#,##0");
                        row.Cells["sale_company"].Value = salesDt.Rows[i]["매출처"].ToString();


                        double last_sale_qty;
                        if (!double.TryParse(salesDt.Rows[i]["최근매출수"].ToString(), out last_sale_qty))
                            last_sale_qty = 0;
                        row.Cells["last_sale_qty"].Value = last_sale_qty.ToString("#,##0");

                        DateTime last_sale_date;
                        if (DateTime.TryParse(salesDt.Rows[i]["최근매출일자"].ToString(), out last_sale_date))
                        {
                            TimeSpan ts = DateTime.Now - last_sale_date;
                            row.Cells["last_sale_elapsed_days"].Value = ts.Days.ToString("#,##0");
                        }

                        double average_sale_qty;
                        if (!double.TryParse(salesDt.Rows[i]["평균매출"].ToString(), out average_sale_qty))
                            average_sale_qty = 0;
                        row.Cells["average_sale_qty"].Value = average_sale_qty.ToString("#,##0");

                        //색구분
                        row.Cells["input_qty"].Style.BackColor = Color.Beige;

                        double unit_count;
                        if (!double.TryParse(salesDt.Rows[i]["단위수량"].ToString(), out unit_count))
                            unit_count = 0;
                        if (unit_count == 0)
                            unit_count = 1;
                        row.Cells["unit_count"].Value = unit_count;

                    }
                    //dgvSales.Focus();
                    dgvSales.CurrentCell = dgvSales.Rows[0].Cells["input_qty"];
                }

                GetRecoveryInfo(company_code);
            }
        }
        private void GetRecoveryInfo(string company_code)
        {
            dgvRecoveryInfo.Rows.Clear();
            int n = dgvRecoveryInfo.Rows.Add();
            dgvRecoveryInfo.Rows[n].Cells["div"].Value = "매출금액";
            DataGridViewRow salesRow = dgvRecoveryInfo.Rows[n];
            salesRow.DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Regular);
            n = dgvRecoveryInfo.Rows.Add();
            dgvRecoveryInfo.Rows[n].Cells["div"].Value = "미수금액";
            DataGridViewRow balanceRow = dgvRecoveryInfo.Rows[n];
            balanceRow.DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Regular);
            /*n = dgvRecoveryInfo.Rows.Add();
            dgvRecoveryInfo.Rows[n].Cells["div"].Value = "채권회전수";*/
            n = dgvRecoveryInfo.Rows.Add();
            dgvRecoveryInfo.Rows[n].Cells["div"].Value = "회전일수";
            DataGridViewRow roundDaysRow = dgvRecoveryInfo.Rows[n];
            roundDaysRow.DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Regular);
            n = dgvRecoveryInfo.Rows.Add();
            dgvRecoveryInfo.Rows[n].Cells["div"].Value = "마진율(%)";
            DataGridViewRow marginRow = dgvRecoveryInfo.Rows[n];
            marginRow.DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Regular);
            n = dgvRecoveryInfo.Rows.Add();
            dgvRecoveryInfo.Rows[n].Cells["div"].Value = "회전일수/마진율";
            DataGridViewRow marginDivRow = dgvRecoveryInfo.Rows[n];
            marginDivRow.DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Bold);
            n = dgvRecoveryInfo.Rows.Add();
            dgvRecoveryInfo.Rows[n].Cells["div"].Value = "원금회수기간";
            DataGridViewRow recoveryMonthRow = dgvRecoveryInfo.Rows[n];
            recoveryMonthRow.DefaultCellStyle.Font = new Font("굴림", 9, FontStyle.Regular);


            DataTable companyDt = recoveryPrincipalPriceRepository.GetRecoveryINfo(company_code);
            if (companyDt.Rows.Count > 0)
            {
                //평균
                double all_sales_amount;
                if (!double.TryParse(companyDt.Rows[0]["평균매출금액"].ToString(), out all_sales_amount))
                    all_sales_amount = 0;
                double all_profit;
                if (!double.TryParse(companyDt.Rows[0]["평균손익금액"].ToString(), out all_profit))
                    all_profit = 0;
                double all_balance;
                if (!double.TryParse(companyDt.Rows[0]["평균미수잔액"].ToString(), out all_balance))
                    all_balance = 0;
                double all_margin = all_profit / all_sales_amount * 100;

                /*salesRow.Cells["all_month"].Value = (all_sales_amount).ToString("#,##0");
                balanceRow.Cells["all_month"].Value = (all_balance).ToString("#,##0");*/
                //dgvRecoveryInfo.Rows[2].Cells["all_month"].Value = (all_sales_amount / all_balance).ToString("#,##0.00");
                roundDaysRow.Cells["all_month"].Value = (30 / (all_sales_amount / all_balance)).ToString("#,##0");
                marginRow.Cells["all_month"].Value = all_margin.ToString("#,##0.0") + "%";
                double div_margin = (Math.Round(30 / (all_sales_amount / all_balance), 0) / Math.Round(all_margin, 2));
                marginDivRow.Cells["all_month"].Value = div_margin.ToString("#,##0.00");
                if (div_margin >= 7)
                    marginDivRow.Cells["all_month"].Style.ForeColor = Color.Red;
                else
                    marginDivRow.Cells["all_month"].Style.ForeColor = Color.Black;

                //전월
                double sales_amount;
                if (!double.TryParse(companyDt.Rows[0]["매출금액"].ToString(), out sales_amount))
                    sales_amount = 0;
                double profit;
                if (!double.TryParse(companyDt.Rows[0]["손익금액"].ToString(), out profit))
                    profit = 0;
                double balance;
                if (!double.TryParse(companyDt.Rows[0]["미수잔액"].ToString(), out balance))
                    balance = 0;
                double margin = profit / sales_amount * 100;

                salesRow.Cells["pre_month"].Value = (sales_amount).ToString("#,##0");
                balanceRow.Cells["pre_month"].Value = (balance).ToString("#,##0");
                //dgvRecoveryInfo.Rows[2].Cells["pre_month"].Value = (sales_amount / balance).ToString("#,##0.00");
                roundDaysRow.Cells["pre_month"].Value = (30 / (sales_amount / balance)).ToString("#,##0");
                marginRow.Cells["pre_month"].Value = margin.ToString("#,##0.0") + "%";
                div_margin = (Math.Round(30 / (sales_amount / balance), 0) / Math.Round(margin, 2));
                marginDivRow.Cells["pre_month"].Value = div_margin.ToString("#,##0.00");
                if (div_margin >= 7)
                    marginDivRow.Cells["pre_month"].Style.ForeColor = Color.Red;
                else
                    marginDivRow.Cells["pre_month"].Style.ForeColor = Color.Black;

                //원금회수율
                double pre_margin;
                if (!double.TryParse(companyDt.Rows[0]["전누적손익금액"].ToString(), out pre_margin))
                    pre_margin = 0;
                double recovery_month = (balance - pre_margin) / profit;
                if (double.IsNaN(recovery_month))
                    recovery_month = 0;
                recoveryMonthRow.Cells["pre_month"].Value = recovery_month.ToString("#,##0.0");
                //타이틀
                string pre_month;
                if (companyDt.Rows[0]["전월"].ToString().Length == 6)
                {
                    pre_month = companyDt.Rows[0]["전월"].ToString().Substring(0, 4) + "-" + companyDt.Rows[0]["전월"].ToString().Substring(4, 2);
                    dgvRecoveryInfo.Columns["pre_month"].HeaderText = pre_month;
                }

                //전전월
                if (!double.TryParse(companyDt.Rows[0]["전매출금액"].ToString(), out sales_amount))
                    sales_amount = 0;
                if (!double.TryParse(companyDt.Rows[0]["전손익금액"].ToString(), out profit))
                    profit = 0;
                if (!double.TryParse(companyDt.Rows[0]["전미수잔액"].ToString(), out balance))
                    balance = 0;
                margin = profit / sales_amount * 100;

                salesRow.Cells["pre_month_2_ago"].Value = (sales_amount).ToString("#,##0");
                balanceRow.Cells["pre_month_2_ago"].Value = (balance).ToString("#,##0");
                //dgvRecoveryInfo.Rows[2].Cells["pre_month_2_ago"].Value = (sales_amount / balance).ToString("#,##0.00");
                roundDaysRow.Cells["pre_month_2_ago"].Value = (30 / (sales_amount / balance)).ToString("#,##0");
                marginRow.Cells["pre_month_2_ago"].Value = margin.ToString("#,##0.0") + "%";
                div_margin = (Math.Round(30 / (sales_amount / balance), 0) / Math.Round(margin, 2));
                marginDivRow.Cells["pre_month_2_ago"].Value = div_margin.ToString("#,##0.00");
                if (div_margin >= 7)
                    marginDivRow.Cells["pre_month_2_ago"].Style.ForeColor = Color.Red;
                else
                    marginDivRow.Cells["pre_month_2_ago"].Style.ForeColor = Color.Black;

                //원금회수율
                if (!double.TryParse(companyDt.Rows[0]["전전누적손익금액"].ToString(), out pre_margin))
                    pre_margin = 0;
                recovery_month = (balance - pre_margin) / profit;
                if (double.IsNaN(recovery_month))
                    recovery_month = 0;
                recoveryMonthRow.Cells["pre_month_2_ago"].Value = recovery_month.ToString("#,##0.0");
                //타이틀
                if (companyDt.Rows[0]["전전월"].ToString().Length == 6)
                {
                    pre_month = companyDt.Rows[0]["전전월"].ToString().Substring(0, 4) + "-" + companyDt.Rows[0]["전전월"].ToString().Substring(4, 2);
                    dgvRecoveryInfo.Columns["pre_month_2_ago"].HeaderText = pre_month;
                }



                //전전전월
                if (!double.TryParse(companyDt.Rows[0]["전전매출금액"].ToString(), out sales_amount))
                    sales_amount = 0;
                if (!double.TryParse(companyDt.Rows[0]["전전손익금액"].ToString(), out profit))
                    profit = 0;
                if (!double.TryParse(companyDt.Rows[0]["전전미수잔액"].ToString(), out balance))
                    balance = 0;
                margin = profit / sales_amount * 100;

                salesRow.Cells["pre_month_3_ago"].Value = (sales_amount).ToString("#,##0");
                balanceRow.Cells["pre_month_3_ago"].Value = (balance).ToString("#,##0");
                //dgvRecoveryInfo.Rows[2].Cells["pre_month_2_ago"].Value = (sales_amount / balance).ToString("#,##0.00");
                roundDaysRow.Cells["pre_month_3_ago"].Value = (30 / (sales_amount / balance)).ToString("#,##0");
                marginRow.Cells["pre_month_3_ago"].Value = margin.ToString("#,##0.0") + "%";
                div_margin = (Math.Round(30 / (sales_amount / balance), 0) / Math.Round(margin, 2));
                marginDivRow.Cells["pre_month_3_ago"].Value = div_margin.ToString("#,##0.00");
                if (div_margin >= 7)
                    marginDivRow.Cells["pre_month_3_ago"].Style.ForeColor = Color.Red;
                else
                    marginDivRow.Cells["pre_month_3_ago"].Style.ForeColor = Color.Black;

                //원금회수율
                if (!double.TryParse(companyDt.Rows[0]["전전전누적손익금액"].ToString(), out pre_margin))
                    pre_margin = 0;
                recovery_month = (balance - pre_margin) / profit;
                if (double.IsNaN(recovery_month))
                    recovery_month = 0;
                recoveryMonthRow.Cells["pre_month_3_ago"].Value = recovery_month.ToString("#,##0.0");
                //타이틀
                if (companyDt.Rows[0]["전전전월"].ToString().Length == 6)
                {
                    pre_month = companyDt.Rows[0]["전전전월"].ToString().Substring(0, 4) + "-" + companyDt.Rows[0]["전전전월"].ToString().Substring(4, 2);
                    dgvRecoveryInfo.Columns["pre_month_3_ago"].HeaderText = pre_month;
                }


            }
        }
        private void GetSales(bool is_exactly = false)
        {
            //매출내역 가져오기
            if (dgvBusiness.SelectedCells.Count > 0)
            {
                dgvBusiness.EndEdit();
                DataGridViewRow dr = dgvBusiness.Rows[dgvBusiness.SelectedCells[0].RowIndex];
                if (dr.Cells["company"].Value == null || string.IsNullOrEmpty(dr.Cells["company"].Value.ToString()))
                {
                    SelectNextRow();
                    return;
                }
                
                //Null 제거
                foreach (DataGridViewCell cell in dr.Cells)
                {
                    if (cell.Value == null)
                        cell.Value = string.Empty;
                }
                //유효성검사
                
                
                if (string.IsNullOrEmpty(dr.Cells["company"].Value.ToString()))
                    return;
                /*if (is_exactly && !companyDIc.ContainsKey(dr.Cells["company"].Value.ToString()))
                    return;*/
                if (companyDt.Rows.Count == 0)
                    return;

                //이전 조회된 거래처 정보 저장
                //InsertCompanyInfo(false);
                //초기화
                dgvSales.Rows.Clear();
                lbCompanyCode.Text = "";


                //거래처명 풀네임 검색
                string company = dr.Cells["company"].Value.ToString().Replace("㈜", "").Replace("(주)", "").Replace("주식회사", "").Replace("법인", "");
                if (companyDIc.ContainsKey(dr.Cells["company"].Value.ToString()))
                    company = companyDIc[dr.Cells["company"].Value.ToString()];
                
                
                //거래처 취급품목
                DataTable salesDt;
                DataRow[] companyRow = (companyDt.Select($"매출처 = '{company}'"));
                string company_code;
                if (companyRow.Length > 0)
                {
                    if (!companyDIc.ContainsKey(company))
                        companyDIc.Add(company, companyRow[0]["매출처"].ToString());

                    //코드Dic추가
                    company_code = companyRow[0]["매출처코드"].ToString();
                    if (!codeDIc.ContainsKey(company))
                        codeDIc.Add(company, company_code);
                    //매출품목Dic 추가
                    if (!productDIc.ContainsKey(company_code))
                    {
                        salesDt = salesRepository.GetHandlingProductDashboard(company_code);
                        productDIc.Add(company_code, salesDt);
                    }
                    else
                        salesDt = productDIc[company_code];
                    lbCompanyCode.Text = company_code;
                }
                else
                {
                    SearchCompany sc = new SearchCompany(um, companyDt, company, cbMyCompany.Checked);
                    string company_name;
                    company_code = sc.GetCompanyCode(out company_name);
                    if (company_code == null)
                        return;
                    else
                        dr.Cells["company"].Value = company_name;
                    company = company_name;
                    lbCompanyCode.Text = company_code;
                }
                txtCompanyName.Text = company;


                SetCompany(company_code);
                GetBalace(company_code);
            }
        }
        private void GetNoneHandledProduct()
        {
            for (int i = dgvSales.Rows.Count - 1; i >= 0; i--)
            { 
                if(dgvSales.Rows[i].Cells["current_sale_date"].Value == null || string.IsNullOrEmpty(dgvSales.Rows[i].Cells["current_sale_date"].Value.ToString()))
                    dgvSales.Rows.RemoveAt(i);
            }


                List<DataGridViewRow> list = new List<DataGridViewRow>();
            for (int i = 0; i < dgvSales.Rows.Count; i++)
                list.Add(dgvSales.Rows[i]);

            DataTable exceptDt = salesRepository.GetNoneHandlingProductDashboard(txtSaleProduct.Text, txtSaleOrigin.Text, txtSaleSizes.Text, txtSaleUnit.Text, list);
            for (int i = 0; i < exceptDt.Rows.Count; i++)
            {
                int n = dgvSales.Rows.Add();
                DataGridViewRow row = dgvSales.Rows[n];

                DateTime updatetime;
                if (DateTime.TryParse(exceptDt.Rows[i]["매입일자"].ToString(), out updatetime))
                {
                    row.Cells["current_sale_date"].Value = updatetime.ToString("yy-MM-dd");
                    row.Cells["current_sale_date"].Style.Font = new Font("굴림", 8, FontStyle.Italic);
                }
                    


                row.Cells["product_name"].Value = exceptDt.Rows[i]["품명"].ToString();
                row.Cells["origin_name"].Value = exceptDt.Rows[i]["원산지"].ToString();
                row.Cells["sizes_name"].Value = exceptDt.Rows[i]["규격"].ToString();
                row.Cells["unit_name"].Value = exceptDt.Rows[i]["seaover단위"].ToString();


                double unit_count;
                if (!double.TryParse(exceptDt.Rows[i]["단위수량"].ToString(), out unit_count))
                    unit_count = 0;
                if (unit_count == 0)
                    unit_count = 1;
                row.Cells["unit_count"].Value = unit_count;


                double stock = 0;
                if (stockDt != null && stockDt.Rows.Count > 0)
                {
                    string whr = $"품명 = '{exceptDt.Rows[i]["품명"].ToString()}'"
                                + $" AND 원산지 = '{exceptDt.Rows[i]["원산지"].ToString()}'"
                                + $" AND 규격 = '{exceptDt.Rows[i]["규격"].ToString()}'"
                                + $" AND 단위 = '{exceptDt.Rows[i]["seaover단위"].ToString().Replace("kg", "").Replace("KG", "").Replace("Kg", "")}'";

                    DataRow[] dr = stockDt.Select(whr);
                    if (dr.Length > 0)
                    {
                        if (!double.TryParse(dr[0]["재고수"].ToString(), out stock))
                            stock = 0;
                    }
                }
                row.Cells["stock"].Value = stock.ToString("#,##0");

                double sale_price;
                if (!double.TryParse(exceptDt.Rows[i]["매출단가"].ToString(), out sale_price))
                    sale_price = 0;
                row.Cells["current_sale_price"].Value = sale_price.ToString("#,##0");

                //색구분
                if (exceptDt.Rows[i]["구분"].ToString() == "1")
                    row.Cells["input_qty"].Style.BackColor = Color.AliceBlue;
                else
                    row.Cells["input_qty"].Style.BackColor = Color.Lavender;
            }
        }
        private void GetBalace(string company_code)
        {
            dgvBalance.Rows.Clear();
            txtTotalSalesAmount.Text = "0";
            txtTotalPaymentAmount.Text = "0";
            txtTotalBalanceAmount.Text = "0";

            DateTime sttdate = new DateTime(1900, 1, 1);
            DateTime enddate = DateTime.Now.AddMonths(1);
            switch (cbBalanceTerms.Text)
            {
                case "1개월":
                    sttdate = DateTime.Now.AddMonths(-1);
                    break;
                case "45일":
                    sttdate = DateTime.Now.AddDays(-45);
                    break;
                case "2개월":
                    sttdate = DateTime.Now.AddMonths(-2);
                    break;
                case "3개월":
                    sttdate = DateTime.Now.AddMonths(-3);
                    break;
                case "6개월":
                    sttdate = DateTime.Now.AddMonths(-6);
                    break;
                case "12개월":
                    sttdate = DateTime.Now.AddMonths(-12);
                    break;
            }

            int n;
            DataTable balaceDt = salesRepository.GetsalesLedger(company_code);
            if (balaceDt.Rows.Count > 0)
            {
                double total_sales_amount = 0, total_payment_amount = 0, current_balance_amount = 0;
                for (int i = 0; i < balaceDt.Rows.Count; i++)
                {
                    
                    DateTime sdate;
                    DateTime.TryParse(balaceDt.Rows[i]["거래일자"].ToString(), out sdate);
                    if (sttdate > sdate)
                    {
                        double sales_amount = 0, payment_amount = 0;
                        if (balaceDt.Rows[i]["div"].ToString() == "매출")
                        {
                            if (!double.TryParse(balaceDt.Rows[i]["거래금액"].ToString(), out sales_amount))
                                sales_amount = 0;
                        }
                        else
                        {
                            if (!double.TryParse(balaceDt.Rows[i]["거래금액"].ToString(), out payment_amount))
                                payment_amount = 0;
                        }
                        current_balance_amount += sales_amount - payment_amount;

                        if (sttdate <= sdate && enddate >= sdate)
                        {
                            if (balaceDt.Rows[i]["div"].ToString() == "매출")
                                total_sales_amount += sales_amount;
                            else
                                total_payment_amount += payment_amount;
                        }
                    }
                }
                n = dgvBalance.Rows.Add();
                dgvBalance.Rows[n].Cells["transaction_date"].Value = "이월";
                dgvBalance.Rows[n].Cells["balance_amount"].Value = current_balance_amount.ToString("#,##0");


                total_sales_amount = 0; total_payment_amount = 0; current_balance_amount = 0;
                for (int i = 0; i < balaceDt.Rows.Count; i++)
                {

                    DateTime sdate;
                    DateTime.TryParse(balaceDt.Rows[i]["거래일자"].ToString(), out sdate);

                    double sales_amount = 0, payment_amount = 0;
                    if (balaceDt.Rows[i]["div"].ToString() == "매출")
                    {
                        if (!double.TryParse(balaceDt.Rows[i]["거래금액"].ToString(), out sales_amount))
                            sales_amount = 0;
                    }
                    else
                    {
                        if (!double.TryParse(balaceDt.Rows[i]["거래금액"].ToString(), out payment_amount))
                            payment_amount = 0;
                    }
                    current_balance_amount += sales_amount - payment_amount;

                    if (sttdate <= sdate && enddate >= sdate)
                    {
                        n = dgvBalance.Rows.Add();
                        dgvBalance.Rows[n].Cells["transaction_date"].Value = sdate.ToString("yyyy-MM-dd");


                        if (balaceDt.Rows[i]["div"].ToString() == "매출")
                        {
                            total_sales_amount += sales_amount;
                            dgvBalance.Rows[n].Cells["sales_amount"].Value = sales_amount.ToString("#,##0");
                        }
                        else
                        {
                            total_payment_amount += payment_amount;
                            dgvBalance.Rows[n].Cells["payment_amount"].Value = payment_amount.ToString("#,##0");
                        }

                        dgvBalance.Rows[n].Cells["balance_amount"].Value = current_balance_amount.ToString("#,##0");
                    }
                }

                txtTotalSalesAmount.Text = total_sales_amount.ToString("#,##0");
                txtTotalPaymentAmount.Text = total_payment_amount.ToString("#,##0");
                txtTotalBalanceAmount.Text = current_balance_amount.ToString("#,##0");
                dgvBalance.FirstDisplayedScrollingRowIndex = dgvBalance.Rows.Count - 1;
            }
        }
        private void SetHeaderStyleForDgvSale()
        {
            dgvSales.Columns["input_qty"].DefaultCellStyle.BackColor = Color.Beige;
            dgvSales.Columns["current_sale_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
        }
        

        private Dictionary<string, DataTable> SetSheetTxtData(string sheetData)
        {
            Dictionary<string, DataTable> shtDic = new Dictionary<string, DataTable>();
            
            if (sheetData != null && !string.IsNullOrEmpty(sheetData))
            {

                string[] shtDatas = sheetData.Split(new string[] { "$$$" }, StringSplitOptions.None);
                if (shtDatas.Length > 1)
                { 
                    
                    for(int i = 0; i <  shtDatas.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(shtDatas[i].Trim()))
                        {
                            DataTable datatable = new DataTable();
                            //시트명
                            string[] tempDt = shtDatas[i].Split('\n');
                            string shtName = tempDt[0].Replace("$", "");
                            string shtText = tempDt[1].Replace("$", "");

                            datatable.Columns.Add("shtName", typeof(string));
                            datatable.Columns.Add("row", typeof(Int32));

                            //컬럼정보
                            string[] colName = null;
                            for (int j = 0; j < tempDt.Length; j++)
                            {
                                if (tempDt[j].Trim().Contains("input_date|"))
                                {
                                    colName = tempDt[j].Trim().Split('\n')[0].Split('|');
                                    for (int k = 0; k < colName.Length; k++)
                                    {
                                        datatable.Columns.Add(colName[k], typeof(string));
                                        datatable.Columns.Add(colName[k] + "_style", typeof(string));
                                    }
                                    break;
                                }
                            }

                            //TEMP데이터 출력
                            string[] tempDataTable = shtDatas[i].Split(new string[] { "\n\n" }, StringSplitOptions.None);
                            if (tempDataTable.Length >= 0)
                            {
                                try
                                {
                                    for (int j = 1; j < tempDataTable.Length; j++)
                                    {
                                        
                                        string[] rowData = tempDataTable[j].Split('\n');
                                        for (int k = 0; k < rowData.Length; k++)
                                        {
                                            if (!string.IsNullOrEmpty(rowData[k].Trim()))
                                            {
                                                DataRow datarow = datatable.NewRow();
                                                datarow["shtName"] = shtText;
                                                datarow["row"] = j;
                                                string[] cellData = rowData[k].Split('|');
                                                if (cellData.Length == 15)
                                                {
                                                    for (int l = 0; l < cellData.Length; l++)
                                                    {
                                                        string[] values = cellData[l].Split('^');
                                                        datarow[colName[l]] = values[0];
                                                        datarow[colName[l] + "_style"] = values[1];
                                                    }
                                                }
                                                datatable.Rows.Add(datarow);
                                            }
                                        }
                                        
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //GetData();
                                    messageBox.Show(this, ex.Message);
                                    this.Activate();
                                }
                            }


                            shtDic.Add(shtName, datatable);
                        }

                    }
                }   
            }
            return shtDic;
        }

        

        
        public void NewDocument()
        {
            dgvBusiness.Rows.Clear();
            this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
            dgvBusiness.Rows.Add(50);
            dgvBusiness.CurrentCell = dgvBusiness.Rows[0].Cells["company"];
            for (int i = 0; i < 50; i++)
            {
                dgvBusiness.Rows[i].Cells["input_date"].Value = DateTime.Now.ToString("MM-dd");
            }                
            this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
            this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
        }
        #endregion

        #region 파일불러오기(Main)
        public void GetJsonData(string folder_path = "CURRENT_DOCUMENT")
        {
            DateTime lastModified;
            sheetDataDic = new Dictionary<string, DataTable>();
            sheetDataDic = jsonCommon.GetSheetData(folder_path, out sheetOrderDic, out lastModified);
            lbUpdatetime.Text = lastModified.ToString("yyyy-MM-dd HH:mm:ss");
            if (sheetDataDic == null)
            {
                GetTxtData();
                return;
            }

            SetSheetData();
        }
        public void GetTxtData(string tempFileName = "")
        {
            sheetDataDic = new Dictionary<string, DataTable>();

            //Txt data가져오기
            string tempValue;
            if (string.IsNullOrEmpty(tempFileName))
            {
                cookie = new LoginCookie(@"C:\Cookies\TEMP\DAILYBUSINESS\", null);
                tempValue = cookie.GetTempFile("", "DAILYBUSINESS.txt");
            }
            else
            {
                cookie = new LoginCookie(@"C:\Cookies\TEMP\DAILYBUSINESS\", null);
                tempValue = cookie.GetTempFile("", tempFileName);
            }
            //Txt 데이터 Datatable로 변환
            if (tempValue != null && !string.IsNullOrEmpty(tempValue))
            {
                sheetDataDic = SetSheetTxtData(tempValue);
                if (sheetDataDic.Count > 0)
                {
                    //Dictionary, Button생성
                    foreach (string key in sheetDataDic.Keys)
                    {
                        DataTable shtDataDt = sheetDataDic[key];
                        if (shtDataDt != null && shtDataDt.Rows.Count > 0)
                        {
                            string shtName = key;   //시트이름
                            string shtText = shtDataDt.Rows[0]["shtName"].ToString();
                        //버튼 Name, index
                        retry:
                            int tabIndex = 0;
                            foreach (Control con in pnSheets.Controls)
                            {
                                if (con.Name == "Sheet" + key)
                                    goto retry;
                                tabIndex++;
                            }
                            Button btnSheet = new Button();
                            //btnSheet.MouseClick += btnExit_MouseClick;
                            btnSheet.Name = shtName;
                            btnSheet.Text = shtText;
                            btnSheet.TabIndex = tabIndex;
                            btnSheet.Padding = new Padding(0, 0, 0, 0);
                            btnSheet.Margin = new Padding(0, 0, 0, 0);
                            btnSheet.Width = 100;
                            btnSheet.Height = 21;
                            btnSheet.MouseUp += BtnSheet_MouseUp;
                            btnSheet.MouseMove += btn_ButtonMove;
                            btnSheet.MouseDown += btn_ButtonDown;
                            btnSheet.Click += btn_Click;
                            pnSheets.Controls.Add(btnSheet);
                            pnSheets.Controls.SetChildIndex(btnSheet, tabIndex - 1);

                            //dtStack 초기화
                            int keyId = Convert.ToInt16(shtName.Replace("Sheet", ""));
                            dgvBusiness.StackDictionaryInitialize(keyId);
                        }
                    }

                    //메인 시트 활성화
                    Button btn;
                    foreach (Control con in pnSheets.Controls)
                    {
                        if (con is Button)
                        {
                            if (con.Name.Contains("Sheet1"))
                            {
                                btn = (Button)con;
                                btn.BackColor = Color.White;
                                btn.ForeColor = Color.Blue;

                                if (sheetDataDic.ContainsKey(con.Name))
                                    SetSheetData(sheetDataDic[con.Name]);
                                else
                                    NewDocument();

                                btn.PerformClick();
                            }
                            else
                            {
                                btn = (Button)con;
                                btn.BackColor = Color.LightGray;
                                btn.ForeColor = Color.Black;
                            }
                        }
                    }
                    //SetSheetData(tempValue);
                    LocationCurrentCell();
                }
            }

            this.dgvBusiness.isPush = true;
            this.dgvBusiness.Push();
            //dgvBusiness.Push();
        }
        private void SetSheetData()
        {
            //초기화 
            this.dgvBusiness.isPush = false;
            dgvBusiness.Rows.Clear();

            for (int i = pnSheets.Controls.Count - 1; i >= 0; i--)
            {
                Control con = pnSheets.Controls[i];
                if (con.Name != "btnAddSheet")
                    pnSheets.Controls.Remove(con);
            }

            if (sheetDataDic.Count > 0)
            {
                //Dictionary, Button생성
                foreach (string key in sheetDataDic.Keys)
                {
                    DataTable shtDataDt = sheetDataDic[key];
                    if (shtDataDt != null && shtDataDt.Rows.Count > 0)
                    {
                        string shtName = key;   //시트이름
                        string shtText = shtDataDt.Rows[0]["shtName"].ToString();
                    //버튼 Name, index
                    retry:
                        int tabIndex = 0;
                        foreach (Control con in pnSheets.Controls)
                        {
                            if (con.Name == "Sheet" + key)
                                goto retry;
                            tabIndex++;
                        }
                        Button btnSheet = new Button();
                        //btnSheet.MouseClick += btnExit_MouseClick;
                        btnSheet.Name = shtName;
                        btnSheet.Text = shtText;
                        btnSheet.TabIndex = tabIndex;
                        btnSheet.Padding = new Padding(0, 0, 0, 0);
                        btnSheet.Margin = new Padding(0, 0, 0, 0);
                        btnSheet.Width = 100;
                        btnSheet.Height = 21;
                        btnSheet.MouseUp += BtnSheet_MouseUp;
                        btnSheet.MouseMove += btn_ButtonMove;
                        btnSheet.MouseDown += btn_ButtonDown;
                        btnSheet.Click += btn_Click;
                        pnSheets.Controls.Add(btnSheet);
                        pnSheets.Controls.SetChildIndex(btnSheet, tabIndex - 1);

                        //dtStack 초기화
                        int keyId = Convert.ToInt16(shtName.Replace("Sheet", ""));
                        dgvBusiness.StackDictionaryInitialize(keyId);
                    }
                }
                string mainSheetName = "Sheet1";
                try
                {
                    //시트순서
                    int minCount = 0;
                    if (sheetOrderDic != null)
                    {
                        foreach (string shtName in sheetOrderDic.Keys)
                        {
                            int orderCount = sheetOrderDic[shtName];
                            if (minCount == orderCount - 1)
                            {
                                if (minCount == 0)
                                    mainSheetName = shtName;
                                if (pnSheets.Controls.Count - 1 > orderCount - 1)
                                {
                                    pnSheets.Controls.SetChildIndex(pnSheets.Controls[shtName], orderCount - 1);
                                    minCount++;
                                }
                            }
                        }
                    }

                   
                }
                catch (Exception e)
                { 
                    messageBox.Show(this, e.Message);
                }

                //메인 시트 활성화
                Button btn;
                foreach (Control con in pnSheets.Controls)
                {
                    if (con is Button)
                    {
                        if (con.Name.Contains(mainSheetName))
                        {
                            btn = (Button)con;
                            btn.BackColor = Color.White;
                            btn.ForeColor = Color.Blue;

                            if (sheetDataDic.ContainsKey(con.Name))
                                SetSheetData(sheetDataDic[con.Name]);
                            else
                                NewDocument();

                            btn.PerformClick();
                        }
                        else
                        {
                            btn = (Button)con;
                            btn.BackColor = Color.LightGray;
                            btn.ForeColor = Color.Black;
                        }

                        break;
                    }
                }

                //SetSheetData(tempValue);
                LocationCurrentCell();
            }

            this.dgvBusiness.isPush = true;
            this.dgvBusiness.Push();
        }
        #endregion

        #region 우클릭 메뉴
        private void dgvBusiness_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvBusiness.Rows.Count > 0)
            {
                try
                {
                    string input_date;
                    if (e.RowIndex > 0 && e.RowIndex < dgvBusiness.RowCount - 1 && dgvBusiness.Rows[e.RowIndex - 1].Cells["input_date"].Value != null)
                        input_date = dgvBusiness.Rows[e.RowIndex - 1].Cells["input_date"].Value.ToString();
                    else
                        input_date = DateTime.Now.ToString("MM-dd");
                    dgvBusiness.Rows[e.RowIndex - 1].Cells["input_date"].Value = input_date;
                }
                catch
                { }
            }
        }
        private void dgvBusiness_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                //한글 변환
                ChangeIME(true);
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    //영어 변환
                    ChangeIME(false);
                    hitTestInfo = dgvBusiness.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    m = new ContextMenuStrip();
                    if (row >= 0)
                    {
                        m.Items.Add("입력일자별 정렬");
                        m.Items.Add("창고별 정렬");
                        m.Items.Add("거래처별 정렬");
                        m.Items.Add("매입처별 정렬");
                        m.Items.Add("소계 추가(B)");
                        ToolStripSeparator toolStripSeparator0 = new ToolStripSeparator();
                        toolStripSeparator0.Name = "toolStripSeparator";
                        toolStripSeparator0.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator0);
                        m.Items.Add("행 삽입(A)");
                        m.Items.Add("행 삭제(D)");
                        ToolStripSeparator toolStripSeparator1 = new ToolStripSeparator();
                        toolStripSeparator1.Name = "toolStripSeparator";
                        toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator1);
                        m.Items.Add("단가검색(W)");
                        m.Items.Add("매출검색(E)");
                        ToolStripSeparator toolStripSeparator2 = new ToolStripSeparator();
                        toolStripSeparator2.Name = "toolStripSeparator";
                        toolStripSeparator2.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator2);
                        m.Items.Add("복사");
                        m.Items.Add("잘라내기");
                        m.Items.Add("덮어쓰기");
                        m.Items.Add("여기서부터 새로고침");
                        ToolStripSeparator toolStripSeparator3 = new ToolStripSeparator();
                        toolStripSeparator3.Name = "toolStripSeparator";
                        toolStripSeparator3.Size = new System.Drawing.Size(119, 6);
                        m.Items.Add(toolStripSeparator3);
                        m.Items.Add("글꼴 변경");
                        m.Items.Add("글자색 변경");
                        m.Items.Add("배경색 변경");
                    }
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvBusiness, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }

                //ChangeIME(true);
            }
            catch
            {
            }
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            dgvBusiness.EndEdit();

            if (e.ClickedItem.Text != "글꼴 변경" && e.ClickedItem.Text != "글자색 변경" && e.ClickedItem.Text != "배경색 변경"
                && e.ClickedItem.Text != "복사" && e.ClickedItem.Text != "잘라내기" && e.ClickedItem.Text != "덮어쓰기")
            {
                if (dgvBusiness.SelectedRows.Count <= 0)
                {
                    List<int> rowIndex = new List<int>();
                    foreach (DataGridViewCell cell in dgvBusiness.SelectedCells)
                    {
                        if (!rowIndex.Contains(cell.RowIndex))
                            rowIndex.Add(cell.RowIndex);
                    }
                    dgvBusiness.ClearSelection();
                    for (int i = 0; i < rowIndex.Count; i++)
                    {
                        dgvBusiness.Rows[rowIndex[i]].Selected = true;
                    }
                }
            }


            switch (e.ClickedItem.Text)
            {
                case "입력일자별 정렬":
                    SelectedRangeSorting("input_date");
                    break;
                case "창고별 정렬":
                    SelectedRangeSorting("warehouse");
                    break;
                case "거래처별 정렬":
                    SelectedRangeSorting("company");
                    break;
                case "매입처별 정렬":
                    SelectedRangeSorting("purchase_company");
                    break;
                case "행 삽입(A)":
                    if (dgvBusiness.SelectedRows.Count > 0)
                    {
                        int idx = 99999;
                        foreach (DataGridViewRow row in dgvBusiness.SelectedRows)
                        {
                            if (row.Index < idx)
                                idx = row.Index;
                        }
                        DataGridViewRow dr = dgvBusiness.Rows[idx];
                        if (dr.Index < 0)
                            return;

                        this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                        string input_date = dgvBusiness.Rows[dr.Index].Cells["input_date"].Value.ToString();
                        

                        for (int i = 0; i < dgvBusiness.SelectedRows.Count; i++)
                        {
                            dgvBusiness.Rows.Insert(idx, 1);
                            dgvBusiness.Rows[idx].Cells["input_date"].Value = input_date;
                        }
                        this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                    }
                    break;
                case "행 삭제(D)":
                    if (dgvBusiness.SelectedRows.Count > 0)
                    {
                        DataGridViewRow dr = dgvBusiness.SelectedRows[0];
                        if (dr.Index < 0)
                            return;

                        this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                        for (int i = dgvBusiness.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dgvBusiness.Rows[i].Selected)
                                dgvBusiness.Rows.Remove(dgvBusiness.Rows[i]);
                        }
                        this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                    }
                    break;
                case "단가검색(W)":
                    if (dgvBusiness.SelectedRows.Count > 0)
                    {
                        DataGridViewRow dr = dgvBusiness.SelectedRows[0];
                        if (dr.Index < 0)
                            return;

                        foreach (DataGridViewCell cell in dr.Cells)
                        {
                            if (cell.Value == null)
                                cell.Value = string.Empty;
                        }


                        try
                        {
                            GetSalesPrice gsp = new GetSalesPrice(um, this, dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString(), dr.Cells["sizes"].Value.ToString(), "");
                            gsp.Show();
                        }
                        catch
                        {

                        }
                    }
                    break;
                case "매출검색(E)":
                    {
                        DataGridViewRow dr = dgvBusiness.SelectedRows[0];
                        if (dr.Index < 0)
                            return;

                        foreach (DataGridViewCell cell in dr.Cells)
                        {
                            if (cell.Value == null)
                                cell.Value = string.Empty;
                        }

                        try
                        {
                            GetSales gsp = new GetSales(um, this, dr.Cells["company"].Value.ToString(), dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString(), dr.Cells["sizes"].Value.ToString(), "");
                            gsp.Show();
                        }
                        catch
                        { }
                    }
                    break;
                case "소계 추가(B)":
                    {
                        AddSubTotal();
                    }
                    break;
                case "복사":
                    {
                        dgvBusiness.Copy();
                    }
                    break;
                case "잘라내기":
                    {
                        dgvBusiness.CopyAndDelete();
                    }
                    break;
                case "덮어쓰기":
                    {
                        dgvBusiness.SetIsPasteRow(false);
                        dgvBusiness.Paste();
                    }
                    break;
                case "여기서부터 새로고침":
                    {
                        if (messageBox.Show(this,"선택하신 열에서부터 영업일지를 새로고침합니다. 계속하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            for (int i = dgvBusiness.SelectedRows[0].Index; i < dgvBusiness.Rows.Count; i++)
                            {
                                for (int j = 0; j < dgvBusiness.Columns.Count; j++)
                                {
                                    dgvBusiness.Rows[i].Cells[j].Value = string.Empty;
                                }
                                dgvBusiness.Rows[i].Cells["input_date"].Value = DateTime.Now.ToString("MM-dd");
                            }
                        }
                    }
                    break;
                case "글꼴 변경":
                    btnFont.PerformClick();
                    break;
                case "글자색 변경":
                    btnFontColorSetting.PerformClick();
                    break;
                case "배경색 변경":
                    btnColorSetting.PerformClick();
                    break;

                default:
                    break;
            }
        }
        private void cms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (dgvBusiness.SelectedRows.Count == 0)
                return;
            DataGridViewRow dr = dgvBusiness.SelectedRows[0];
            if (dr.Index < 0)
                return;
            dgvBusiness.EndEdit();
            //int rowindex = Convert.ToInt32(dr.Cells[0].Value);
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.A:
                    m.Items[6].PerformClick();
                    break;
                case Keys.B:
                    m.Items[4].PerformClick();
                    break;
                case Keys.D:
                    m.Items[7].PerformClick();
                    break;
                case Keys.W:
                    m.Items[9].PerformClick();
                    break;
                case Keys.E:
                    m.Items[10].PerformClick();
                    break;
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

        #region Button, Checkbox, ComboBox
        private void cbBalanceTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetBalace(lbCompanyCode.Text);
        }
        private void btnSalesManager_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "거래처 관리", "is_visible"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;

            foreach (Form frm in fc)
            {
                //iterate through
                //if (frm.Name == "RecoveryPrincipalManager")
                if (frm.Name == "SalesManager")
                {
                    frm.WindowState = FormWindowState.Maximized;
                    frm.Activate();
                    isFormActive = true;
                }
            }
            //새로열기
            if (!isFormActive)
            {
                SaleManagement.SalesManager rpm = new SaleManagement.SalesManager(um);
                rpm.Show();
                rpm.GetDatatable();
            }
        }
        private void btnRecoveryDetail_Click(object sender, EventArgs e)
        {
            DataTable companyDt = seaoverCompanyRepository.GetSeaoverCompanyInfo("", lbCompanyCode.Text);
            if (companyDt.Rows.Count > 0)
            {
                RecoveryPrincipalManager rpm = new RecoveryPrincipalManager(um, companyDt.Rows[0]["거래처명"].ToString(), "", DateTime.Now.AddMonths(-1));
                rpm.Show();
            }
            else
            {
                messageBox.Show(this,"거래처를 선택해주세요.");
                this.Activate();
            }
        }
        private void btnColorStting_Click(object sender, EventArgs e)
        {
            Color color = new Color();
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                color = cd.Color;
                this.btnColor.BackColor = color;


                foreach (DataGridViewCell cell in dgvBusiness.SelectedCells)
                {
                    cell.Style.BackColor = color;
                }
            }
        }
        private void btnFontColorSetting_Click(object sender, EventArgs e)
        {
            Color color = new Color();
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                color = cd.Color;
                this.btnFontColor.BackColor = color;

                foreach (DataGridViewCell cell in dgvBusiness.SelectedCells)
                {
                    cell.Style.ForeColor = color;
                }
            }
        }
        private void btnColor_Click(object sender, EventArgs e)
        {
            Color color = btnColor.BackColor;

            foreach (DataGridViewCell cell in dgvBusiness.SelectedCells)
            {
                cell.Style.BackColor = color;
            }
        }
        private void btnFontColor_Click(object sender, EventArgs e)
        {
            Color color = btnFontColor.BackColor;

            foreach (DataGridViewCell cell in dgvBusiness.SelectedCells)
            {
                cell.Style.ForeColor = color;
            }
        }
        private void btnFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                this.btnFont.Text = fd.Font.Name.ToString();

                foreach (DataGridViewCell cell in dgvBusiness.SelectedCells)
                {
                    cell.Style.Font = fd.Font;
                }

            }
        }
        private void btnPurchaseMargin_Click(object sender, EventArgs e)
        {
            KeyHelper kh = new KeyHelper();
            kh.Show();
        }
        private void btnCompanySearching_Click(object sender, EventArgs e)
        {
            CompanyManager cm;
            if (dgvBusiness.Focused && dgvBusiness.CurrentCell.OwningColumn.Name == "company")
                cm = new CompanyManager(um, this, dgvBusiness.CurrentCell);
            else
                cm = new CompanyManager(um, this);

            cm.Show();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelDownload();
        }
        private void btnNewDocument_Click(object sender, EventArgs e)
        {
            InsertData(1);
            NewDocument();
        }
        private void btnDocumentManager_Click(object sender, EventArgs e)
        {
            try
            {
                DocumentManager dm = new DocumentManager(um, this);
                dm.Show();
            }
            catch
            { }
        }
        private void btnInsertCompany_Click(object sender, EventArgs e)
        {
            InsertCompanyInfo();
        }

        int ExitCnt = 0;
        private void btnExit_Click(object sender, EventArgs e)
        {
            //2024-06-11 이상하게 닫기버튼을 누르면 2번 활성화되고 2번째 활성화 될때 시트하나씩 없어짐..
            if(ExitCnt++ < 1)
                FileSave(false);
            this.Dispose();
        }
        private void cbPreSalePriceHide_CheckedChanged(object sender, EventArgs e)
        {
            dgvSales.Columns["pre_sale_price"].Visible = !cbPreSalePriceHide.Checked;
        }
        private void cbStockHide_CheckedChanged(object sender, EventArgs e)
        {
            dgvSales.Columns["stock"].Visible = !cbStockHide.Checked;
        }
        private void cbHide_CheckedChanged(object sender, EventArgs e)
        {

            if (dgvBusiness.Columns.Count > 0)
            {
                int idx = 0;
                for (int i = 0; i < dgvBusiness.Columns.Count; i++)
                {
                    if (dgvBusiness.Columns[i].Name == "qty")
                    { 
                        idx = ++i;
                        break;
                    }
                }

                /*if (cbHide.Checked)
                {
                    dgvBusiness.Columns["sale_price"].Width = 0;
                    dgvBusiness.Columns["sale_amount"].Width = 0;
                }
                else
                {
                    dgvBusiness.Columns["sale_price"].Width = 80;
                    dgvBusiness.Columns["sale_amount"].Width = 80;
                }*/
                if (cbHide.Checked)
                {
                    dgvBusiness.Columns["sale_price"].Visible = false;
                    dgvBusiness.Columns["sale_amount"].Visible = false;
                }
                else
                {
                    dgvBusiness.Columns["sale_price"].Visible = true;
                    dgvBusiness.Columns["sale_amount"].Visible = true;
                }
            }
        }
        private void cbHideOrigin_CheckedChanged(object sender, EventArgs e)
        {
            /*if (cbHideOrigin.Checked)
                dgvBusiness.Columns["origin"].Width = 0;
            else
                dgvBusiness.Columns["origin"].Width = 70;*/
            if (cbHideOrigin.Checked)
                dgvBusiness.Columns["origin"].Visible = false;
            else
                dgvBusiness.Columns["origin"].Visible = true;
        }
        private void btnGetProduct_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "GetSalesPrice")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                try
                {
                    GetSalesPrice gsp = new GetSalesPrice(um, this);
                    gsp.Show();
                    gsp.Focus();
                }
                catch
                { }
            }
        }
        private void btnGetSales_Click(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "GetSales")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            if (!isFormActive)
            {
                try
                {
                    GetSales gsp = new GetSales(um, this);
                    gsp.Show();
                }
                catch
                { }
            }
        }
        private void btnAddRow_Click(object sender, EventArgs e)
        {
            this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);

            try
            {
                if (dgvBusiness.SelectedCells.Count == 0)
                {
                    string input_date = DateTime.Now.ToString("MM-dd");
                    int idx = dgvBusiness.Rows.Count;
                    dgvBusiness.Rows.Insert(idx, 1);
                    dgvBusiness.Rows[idx].Cells["input_date"].Value = input_date;
                }
                else
                {
                    string input_date = DateTime.Now.ToString("MM-dd");
                    if (dgvBusiness.Rows[dgvBusiness.SelectedCells[0].RowIndex].Cells["input_date"].Value != null)
                        input_date = dgvBusiness.Rows[dgvBusiness.SelectedCells[0].RowIndex].Cells["input_date"].Value.ToString();
                    int idx = dgvBusiness.SelectedCells[0].RowIndex + 1;
                    if (idx == dgvBusiness.Rows.Count)
                        input_date = DateTime.Now.ToString("MM-dd");
                    dgvBusiness.Rows.Insert(idx, 1);
                    dgvBusiness.Rows[idx].Cells["input_date"].Value = input_date;
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[idx].Cells[dgvBusiness.SelectedCells[0].ColumnIndex];
                }
            }
            catch
            {
            }
            this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
        }
        private void btnServerBackup_Click(object sender, EventArgs e)
        {
            ServerBackup();
            messageBox.Show(this,"생성완료");
            this.Activate();
        }
        private void btnCapture_Click(object sender, EventArgs e)
        {
            string mode = "min";
            switch (this.WindowState)
            {
                case FormWindowState.Minimized:
                    mode = "min";
                    break;
                case FormWindowState.Maximized:
                    mode = "max";
                    break;
                case FormWindowState.Normal:
                    mode = "normal";
                    break;

            }


            dgvBusiness.EndEdit();
            pnCompanyInfo.Visible = false;
            this.Update();
            this.WindowState = FormWindowState.Maximized;
            DataGridViewCell cell = dgvBusiness.CurrentCell;
            if (dgvBusiness.SelectedRows.Count > 0)
            {
                dgvBusiness.Update();
                common.GetDgvSelectCellsCapture(this, dgvBusiness);
            }
            else if (dgvBusiness.SelectedCells.Count > 0)
                common.GetDgvSelectCellsCapture(this, dgvBusiness);


            switch (mode)
            {
                case "min":
                    this.WindowState = FormWindowState.Minimized;
                    break;
                case "max":
                    this.WindowState = FormWindowState.Maximized;
                    break;
                case "normal":
                    this.WindowState = FormWindowState.Normal;
                    break;

            }

            //this.WindowState = FormWindowState.Normal;
            pnCompanyInfo.Visible = true;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FileSave();
        }

        private void FileSave(bool isMsg = true)
        {
            if (isMsg)
            {
                if (messageBox.Show(this, "현재 영업일지를 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            dgvBusiness.EndEdit();
            dgvBusiness.ClearSelection();
            //InsertData(document_id);
            InsertJsonData();
            InsertCompanyInfo(false);
        }

        #region Control json

        public void InsertJsonData()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "거래처 관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            dgvBusiness.EndEdit();
            try
            {
                //현재 활성화된 시트
                int currentShtCnt = 0;
                if (!int.TryParse(lbCurrentSheetName.Text.Replace("Sheet", ""), out currentShtCnt))
                    currentShtCnt = 0;

                bool isExist = false;
                if (currentShtCnt > 0 && pnSheets.Controls.Count > 1)
                {
                    foreach (Control btn in pnSheets.Controls)
                    {
                        if (btn.Name.Equals(lbCurrentSheetName.Text))
                        {
                            isExist = true;
                            break;
                        }
                    }
                }
                else if (currentShtCnt == 0)
                    lbCurrentSheetName.Text = "Sheet1";

                if (!isExist)
                {
                    //시트 버튼생성
                    Button btnSheet = new Button();
                    //btnSheet.MouseClick += btnExit_MouseClick;
                    btnSheet.Name = lbCurrentSheetName.Text;
                    btnSheet.Text = lbCurrentSheetName.Text;
                    btnSheet.TabIndex = 0;
                    btnSheet.Padding = new Padding(0, 0, 0, 0);
                    btnSheet.Margin = new Padding(0, 0, 0, 0);
                    btnSheet.Width = 100;
                    btnSheet.Height = 21;
                    btnSheet.MouseUp += BtnSheet_MouseUp;
                    btnSheet.MouseMove += btn_ButtonMove;
                    btnSheet.MouseDown += btn_ButtonDown;
                    btnSheet.Click += btn_Click;
                    pnSheets.Controls.Add(btnSheet);
                    pnSheets.Controls.SetChildIndex(btnSheet, 0);

                    /*messageBox.Show(this, "활성화된 시트를 찾지를 못하였습니다.");
                    return;*/
                }
                //시트별 데이터 DICTIONARY 만들기
                //현재시트만 최신화
                foreach (Control con in pnSheets.Controls)
                {
                    int shtIdx;
                    if (int.TryParse(con.Name.Replace("Sheet", ""), out shtIdx) && con.Name == lbCurrentSheetName.Text)
                    {
                        DataTable currentShtData = jsonCommon.ConvertToDatatable(con.Text, this.dgvBusiness);
                        if (sheetDataDic.ContainsKey(con.Name))
                            sheetDataDic[con.Name] = currentShtData;
                        else
                            sheetDataDic.Add(con.Name, currentShtData);
                        break;
                    }
                }

                //시트명 최신화
                List<string> sheetOrderList = new List<string>();
                foreach (Control con in pnSheets.Controls)
                {
                    if (!con.Name.Equals("btnAddSheet"))
                    {
                        DataTable shtDataDt = sheetDataDic[con.Name];
                        if (shtDataDt != null && shtDataDt.Rows.Count > 0)
                        {
                            shtDataDt.Columns["shtName"].ReadOnly = false;
                            foreach(DataRow dr in  shtDataDt.Rows) 
                                dr["shtName"] = con.Text;
                        }
                        sheetOrderList.Add(con.Name);
                    }
                }

                //백업저장
                string savePath = DateTime.Now.ToString("yyyyMMdd") + @"\" + DateTime.Now.ToString("HHmm");
                jsonCommon.InputJson(savePath, sheetDataDic, sheetOrderList);
                //최근 문서저장
                savePath = @"CURRENT_DOCUMENT\";
                jsonCommon.InputJson(savePath, sheetDataDic, sheetOrderList);
            }
            catch (Exception ex)
            {
                messageBox.Show(this, ex.Message);
                this.Activate();
            }
        }   

        #endregion







        IAuthorityRepository authorityRepository = new AuthorityRepository();
        public void InsertData(int document_id = 1)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "거래처 관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            dgvBusiness.EndEdit();
            try
            {
                //현재 활성화된 시트
                int currentShtCnt = 0;
                if (!int.TryParse(lbCurrentSheetName.Text.Replace("Sheet", ""), out currentShtCnt))
                    currentShtCnt = 0;
                if (currentShtCnt == 0)
                {
                    messageBox.Show(this, "활성화된 시트를 찾지를 못하였습니다.");
                    return;
                }

                foreach (Control con in pnSheets.Controls)
                {
                    int shtIdx;
                    if (int.TryParse(con.Name.Replace("Sheet", ""), out shtIdx))
                    {
                        if (con.Name.Equals("Sheet" + currentShtCnt.ToString()))
                        {
                            DataTable currentShtDataDt = jsonCommon.ConvertToDatatable(con.Name, this.dgvBusiness);
                            if (sheetDataDic.ContainsKey(con.Name))
                                sheetDataDic[con.Name] = currentShtDataDt;
                            else
                                sheetDataDic.Add(con.Name, currentShtDataDt);
                            break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                messageBox.Show(this,ex.Message);
                this.Activate();
            }
        }

        public void ServerBackup()
        {
            if (messageBox.Show(this,"서버 백업분을 생성하겠습니다.", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            //이전 데이터 삭제
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql;
            /*StringBuilder sql = dailyBusinessRepository.DeleteData(um.user_id, document_id, DateTime.Now.AddYears(-5).ToString("yyyy-MM-dd"), DateTime.Now.AddYears(3).ToString("yyyy-MM-dd"));
            sqlList.Add(sql);*/
            //데이터 등록
            if (dgvBusiness.Rows.Count > 0)
            {
                if (dgvBusiness.Rows[0].Cells["input_date"].Value == null)
                    dgvBusiness.Rows[0].Cells["input_date"].Value = "";
                string input_date = dgvBusiness.Rows[0].Cells["input_date"].Value.ToString();
                int id = 0;
                int temp_id = 0;

                //Temp데이터
                string[] whr = new string[2];
                whr[0] = "user_id";
                whr[1] = "document_id";
                string[] val = new string[2];
                val[0] = "'" + um.user_id + "'";
                val[1] = "1";
                int temp_document_id = commonRepository.GetNextIdMulti("t_daily_business_temp", "id", whr, val);

                for (int i = 0; i < dgvBusiness.Rows.Count; i++)
                {
                    for (int j = 0; j < dgvBusiness.ColumnCount; j++)
                    {
                        if (dgvBusiness.Rows[i].Cells[j].Value == null)
                            dgvBusiness.Rows[i].Cells[j].Value = "";
                    }

                    if (input_date == dgvBusiness.Rows[i].Cells["input_date"].Value.ToString())
                        id++;
                    else
                    {
                        input_date = dgvBusiness.Rows[i].Cells["input_date"].Value.ToString();
                        id = 1;
                    }

                    DailyBusinessModel model = new DailyBusinessModel();
                    model.user_id = um.user_id;
                    model.document_id = 1;
                    model.sub_id = id;
                    //model.document_name = txtDocumentName.Text;
                    model.input_date = input_date;
                    model.company = dgvBusiness.Rows[i].Cells["company"].Value.ToString();
                    model.product = dgvBusiness.Rows[i].Cells["product"].Value.ToString();
                    model.origin = dgvBusiness.Rows[i].Cells["origin"].Value.ToString();
                    model.sizes = dgvBusiness.Rows[i].Cells["sizes"].Value.ToString();
                    model.unit = dgvBusiness.Rows[i].Cells["unit"].Value.ToString();
                    model.qty = dgvBusiness.Rows[i].Cells["qty"].Value.ToString();
                    double sale_price;
                    if (dgvBusiness.Rows[i].Cells["sale_price"].Value == null || !double.TryParse(dgvBusiness.Rows[i].Cells["sale_price"].Value.ToString(), out sale_price))
                        sale_price = 0;
                    model.sale_price = sale_price;
                    model.warehouse = dgvBusiness.Rows[i].Cells["warehouse"].Value.ToString();
                    model.purchase_company = dgvBusiness.Rows[i].Cells["purchase_company"].Value.ToString();
                    double purchase_price;
                    if (dgvBusiness.Rows[i].Cells["purchase_price"].Value == null || !double.TryParse(dgvBusiness.Rows[i].Cells["purchase_price"].Value.ToString(), out purchase_price))
                        purchase_price = 0;
                    model.purchase_price = purchase_price;
                    model.freight = dgvBusiness.Rows[i].Cells["freight"].Value.ToString();
                    model.remark = dgvBusiness.Rows[i].Cells["remark"].Value.ToString();
                    model.inquire = dgvBusiness.Rows[i].Cells["inquire"].Value.ToString();
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //cell_style
                    string total_cell_style = "";
                    for (int j = 0; j < dgvBusiness.ColumnCount; j++)
                    {
                        DataGridViewCell cell = dgvBusiness.Rows[i].Cells[j];
                        float font_size = 9;
                        string font_name = "굴림";
                        bool font_bold = false;
                        bool font_italic = false;
                        if (cell.Style.Font != null)
                        {
                            font_size = cell.Style.Font.Size;
                            font_name = cell.Style.Font.Name;
                            font_bold = cell.Style.Font.Bold;
                            font_italic = cell.Style.Font.Italic;
                        }

                        Color fore_col = cell.Style.ForeColor;
                        if (fore_col.Name == "0")
                            fore_col = Color.Black;

                        string fore_col_rgb = "#" + fore_col.ToArgb();
                        //string fore_col_rgb = fore_col.R.ToString("X2") + "," + fore_col.G.ToString("X2") + "," + fore_col.B.ToString("X2");
                        Color back_col = cell.Style.BackColor;
                        if (back_col.Name == "0")
                            back_col = Color.White;
                        //string back_col_rgb = back_col.R.ToString("X2") + "," + back_col.G.ToString("X2") + "," + back_col.B.ToString("X2");
                        string back_col_rgb = "#" + back_col.ToArgb();

                        string cell_style = font_size.ToString() + "^" + font_name + "^" + font_bold.ToString() + "^" + font_italic.ToString() + "^" + fore_col_rgb + "^" + back_col_rgb;
                        total_cell_style += "\n" + cell.OwningColumn.Name + "^" + cell_style;
                    }
                    model.cell_style = total_cell_style.Trim();

                    DateTime createtime;
                    if (DateTime.TryParse(lbCreatetime.Text, out createtime))
                        model.createtime = createtime.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        model.createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    //원본데이터
                    /*sql = dailyBusinessRepository.InsertData(model);
                    sqlList.Add(sql);*/
                    //Temp데이터
                    /*string[] whr = new string[2];
                    whr[0] = "user_id";
                    whr[1] = "document_id";
                    string[] val = new string[2];
                    val[0] = "'" + um.user_id + "'";
                    val[1] = document_id.ToString();*/
                    model.id = temp_document_id;
                    temp_id = model.id;
                    //Temp insert
                    sql = dailyBusinessRepository.InsertTempData(model);
                    sqlList.Add(sql);
                }

                //Temp Delete
                sql = dailyBusinessRepository.DeleteTempData(um.user_id, 1, temp_id);
                sqlList.Add(sql);

            }
            //execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                messageBox.Show(this,"수정중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                //lbDocumentId.Text = document_id.ToString();
                lbDocumentDivision.Text = "최근문서";
                lbUpdatetime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion

        #region Key event
        private void dgvSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Z:
                        dgvBusiness.Undo();
                        break;
                    case Keys.Y:
                        dgvBusiness.Redo();
                        break;
                }
            }
        }
        private void txtSaleProduct_Enter(object sender, EventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }
        private void txtSaleProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                SearchingSaleProduct();

        }

        private void DailyBusiness_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.B:
                        AddSubTotal();
                        break;
                    case Keys.Q:
                        SearchingSaleProduct();
                        break;
                    case Keys.S:
                        btnUpdate.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.M:
                        txtSaleProduct.Focus();
                        break;
                    case Keys.N:
                        txtSaleProduct.Text = String.Empty;
                        txtSaleOrigin.Text = String.Empty;
                        txtSaleSizes.Text = String.Empty;
                        txtSaleUnit.Text = String.Empty;
                        txtSaleProduct.Focus();
                        break;
                    case Keys.W:
                        bool isSaleSearch = false;
                        Control tb = common.FindFocusedControl(this);
                        string product = "";
                        string origin = "";
                        string sizes = "";
                        string unit = "";
                        if ((tb.Name == "dgvSales"
                            || tb.Name == "txtSaleProduct"
                            || tb.Name == "txtSaleOrigin"
                            || tb.Name == "txtSaleSizes"
                            || tb.Name == "txtSaleUnit"))
                        {
                            product = txtSaleProduct.Text;
                            txtSaleProduct.Text = String.Empty;
                            origin = txtSaleOrigin.Text;
                            txtSaleOrigin.Text = String.Empty;
                            sizes = txtSaleSizes.Text;
                            txtSaleSizes.Text = String.Empty;
                            unit = txtSaleUnit.Text;
                            txtSaleUnit.Text = String.Empty;
                            SearchingSaleProduct();
                            isSaleSearch=  true;
                        }

                        dgvBusiness.EndEdit();
                        if (dgvBusiness.SelectedCells.Count > 0)
                        {
                            int col_idx = dgvBusiness.SelectedCells[0].ColumnIndex;
                            DataGridViewRow dr = dgvBusiness.Rows[dgvBusiness.SelectedCells[0].RowIndex];
                            foreach (DataGridViewCell cell in dr.Cells)
                            {
                                if (cell.Value == null)
                                    cell.Value = string.Empty;
                            }
                            GetSalesPrice gsp;
                            FormCollection fc = Application.OpenForms;
                            bool isFormActive = false;
                            foreach (Form frm in fc)
                            {
                                //iterate through
                                if (frm.Name == "GetSalesPrice")
                                {
                                    gsp = (GetSalesPrice)frm;
                                    if(isSaleSearch)
                                        gsp.Searching(product, origin, sizes, unit);
                                    else
                                        gsp.Searching("", "", "", "");
                                    isFormActive = true;
                                }
                            }
                            if (!isFormActive)
                            {
                                try
                                {                                                                        
                                    if (isSaleSearch)
                                        gsp = new GetSalesPrice(um, this, product, origin, sizes, unit);
                                    else
                                        gsp = new GetSalesPrice(um, this, "", "", "", "");
                                    gsp.ShowDialog();
                                }
                                catch
                                { }
                            }
                        }
                        dgvSales.Focus();
                        if (dgvSales.Rows.Count > 0)
                            dgvSales.CurrentCell = dgvSales.Rows[0].Cells["input_qty"];
                        break;
                    case Keys.E:
                        dgvBusiness.EndEdit();
                        if (dgvBusiness.SelectedCells.Count > 0)
                        {
                            int col_idx = dgvBusiness.SelectedCells[0].ColumnIndex;
                            DataGridViewRow dr = dgvBusiness.Rows[dgvBusiness.SelectedCells[0].RowIndex];
                            foreach (DataGridViewCell cell in dr.Cells)
                            {
                                if (cell.Value == null)
                                    cell.Value = string.Empty;
                            }
                            if (dgvBusiness.Columns[col_idx].Name == "company")
                            {
                                GetSales gsp;
                                FormCollection fc = Application.OpenForms;
                                bool isFormActive = false;
                                foreach (Form frm in fc)
                                {
                                    //iterate through
                                    if (frm.Name == "GetSales")
                                    {
                                        gsp = (GetSales)frm;
                                        gsp.Searching(dr.Cells["company"].Value.ToString(), dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString(), dr.Cells["sizes"].Value.ToString(), "");
                                        isFormActive = true;
                                    }
                                }

                                if (!isFormActive)
                                {
                                    try
                                    {
                                        gsp = new GetSales(um, this, dr.Cells["company"].Value.ToString(), dr.Cells["product"].Value.ToString(), dr.Cells["origin"].Value.ToString(), dr.Cells["sizes"].Value.ToString(), "");
                                        gsp.Show();
                                    }
                                    catch
                                    { }
                                }
                            }
                        }
                        break;
                    case Keys.Up:
                        {
                            if (dgvBusiness.SelectedCells.Count > 0)
                            {
                                int idx = dgvBusiness.CurrentCell.RowIndex;
                                DateTime current_input_date;
                                if (dgvBusiness.Rows[idx].Cells["input_date"].Value == null || !DateTime.TryParse(dgvBusiness.Rows[idx].Cells["input_date"].Value.ToString(), out current_input_date))
                                    return;
                                idx--;
                                while (idx >= 0)
                                {
                                    DateTime input_date;
                                    if (dgvBusiness.Rows[idx].Cells["input_date"].Value != null && DateTime.TryParse(dgvBusiness.Rows[idx].Cells["input_date"].Value.ToString(), out input_date))
                                    {
                                        if (input_date != current_input_date)
                                        {
                                            dgvBusiness.CurrentCell = dgvBusiness.Rows[idx].Cells["input_date"];
                                            break;
                                        }   
                                    }
                                    else
                                    {
                                        dgvBusiness.CurrentCell = dgvBusiness.Rows[idx].Cells["input_date"];
                                        break;
                                    }
                                    idx--;
                                }
                            }
                        }
                        break;
                    case Keys.Down:
                        {
                            if (dgvBusiness.SelectedCells.Count > 0)
                            {
                                int idx = dgvBusiness.CurrentCell.RowIndex;
                                DateTime current_input_date;
                                if (dgvBusiness.Rows[idx].Cells["input_date"].Value == null || !DateTime.TryParse(dgvBusiness.Rows[idx].Cells["input_date"].Value.ToString(), out current_input_date))
                                    return;
                                idx++;
                                while (idx <= dgvBusiness.Rows.Count - 1)
                                {
                                    DateTime input_date;
                                    if (dgvBusiness.Rows[idx].Cells["input_date"].Value != null && DateTime.TryParse(dgvBusiness.Rows[idx].Cells["input_date"].Value.ToString(), out input_date))
                                    {
                                        if (input_date != current_input_date)
                                        {
                                            dgvBusiness.CurrentCell = dgvBusiness.Rows[idx].Cells["input_date"];
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        dgvBusiness.CurrentCell = dgvBusiness.Rows[idx].Cells["input_date"];
                                        break;
                                    }
                                    idx++;
                                }
                            }
                        }
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.S:
                        btnUpdate.PerformClick();
                        break;
                    case Keys.D:
                        string tempData = "";
                        bool isFirst = true;
                        int pre_col_idx = -1;
                        this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                        for (int i = 0; i < dgvBusiness.ColumnCount; i++)
                        {                            
                            for (int j = 0; j < dgvBusiness.Rows.Count; j++)
                            {
                                if (dgvBusiness.Rows[j].Cells[i].Selected)
                                {
                                    if (pre_col_idx != i)
                                    {
                                        tempData = dgvBusiness.Rows[j].Cells[i].Value.ToString();
                                        pre_col_idx = i;
                                    }
                                    dgvBusiness.Rows[j].Cells[i].Value = tempData;
                                }
                            }
                        }
                        this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                        break;
                    case Keys.Enter:
                        try
                        {
                            btnAddRow.PerformClick();
                        }
                        catch
                        { }
                        break;
                    case Keys.M:
                        Control tb = common.FindFocusedControl(this);
                        if (tb.Name == "dgvBusiness")
                            dgvSales.Focus();
                        else
                            dgvBusiness.Focus();
                        break;
                    case Keys.N:
                        LocationCurrentCell();
                        break;
                    case Keys.F:
                        try
                        {
                            TxtFindManger tfm = new TxtFindManger(um, this);
                            tfm.Show();
                        }
                        catch
                        { }
                        break;
                    case Keys.O:
                        btnSalesManager.PerformClick();
                        break;
                    case Keys.D1:
                        btnSalesManager.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        if (dgvBusiness.SelectedCells.Count > 0)
                            dgvBusiness.CurrentCell = dgvBusiness.SelectedCells[0];
                        dgvBusiness.Focus();
                        break;
                    case Keys.F3:
                        dgvSales.Focus();
                        e.Handled = true;
                        break;
                    case Keys.F4:
                        txtInputInquire.Focus();
                        e.Handled = true;
                        break;
                    case Keys.F5:
                        btnNewDocument.PerformClick();
                        break;
                    case Keys.F6:
                        cbHideOrigin.Checked = !cbHideOrigin.Checked;
                        break;
                    case Keys.F7:
                        cbHide.Checked = !cbHide.Checked;
                        break;
                    case Keys.F8:
                        bool isChecked = !cbHide.Checked;
                        cbHide.Checked = isChecked;
                        cbHideOrigin.Checked = isChecked;
                        break;
                    case Keys.F9:
                        btnCompanySearching.PerformClick();
                        break;
                    case Keys.F10:
                        cbMyCompany.Checked = !cbMyCompany.Checked;
                        break;
                    case Keys.F11:
                        cbStockHide.Checked = !cbStockHide.Checked;
                        break;
                    case Keys.F12:
                        cbPreSalePriceHide.Checked = !cbPreSalePriceHide.Checked;
                        break;
                }
            }
        }

        #endregion

        #region Datagridview event
        private void dgvBusiness_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    if (dgvBusiness.SelectedRows.Count == 0 && dgvBusiness.SelectedCells.Count == 0)
                        dgvBusiness.Rows[e.RowIndex].Selected = true;
                    else if (dgvBusiness.SelectedRows.Count == 1)
                    {
                        dgvBusiness.ClearSelection();
                        dgvBusiness.Rows[e.RowIndex].Selected = true;
                    }
                    else if (dgvBusiness.SelectedCells.Count == 1 && e.ColumnIndex >= 0)
                    {
                        dgvBusiness.ClearSelection();
                        dgvBusiness.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                    }
                }
            }
        }
        private void dgvSales_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            double a, b;
            if (e.CellValue1 != null && e.CellValue2 != null && double.TryParse(e.CellValue1.ToString(), out a) && double.TryParse(e.CellValue2.ToString(), out b))
            {
                e.SortResult = a.CompareTo(b);
                e.Handled = true;
            }
        }
        private void dgvBusiness_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvBusiness.Columns[e.ColumnIndex].Name == "company"
                    && dgvBusiness.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null
                    && !string.IsNullOrEmpty(dgvBusiness.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
                {
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[e.RowIndex].Cells["company"];
                    GetSales();
                }
            }
        }
        private void dgvBusiness_SelectionChanged(object sender, EventArgs e)
        {
            CellCounting();
        }
        private void dgvBusiness_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //MessageBox.Show(this,new Form { TopMost = true },"입력값이 잘못되었습니다. 다시 확인해주세요.");

            e.Cancel = false;
            e.ThrowException = false;
        }
        private void dgvBusiness_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            isTab = false;
        }
        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        private static extern bool GetCursorPos(out Point lpPoint);
        public void dgvBusiness_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvBusiness.EndEdit();
                this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                DataGridViewTextBoxCell txt = dgvBusiness.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewTextBoxCell;
                string titleText = dgvBusiness.Columns[e.ColumnIndex].Name;
                //총액계산
                if (titleText == "qty" || titleText == "sale_price")
                {
                    double qty = 0;
                    //품절일 경우 
                    if (dgvBusiness.Rows[e.RowIndex].Cells["qty"].Value != null && dgvBusiness.Rows[e.RowIndex].Cells["qty"].Value.ToString().Contains("품절"))
                    {
                        dgvBusiness.Rows[e.RowIndex].Cells["qty"].Style.BackColor = Color.Red;
                        dgvBusiness.Rows[e.RowIndex].Cells["qty"].Style.ForeColor = Color.White;
                    }
                    else
                    {
                        dgvBusiness.Rows[e.RowIndex].Cells["qty"].Style.BackColor = Color.White;
                        dgvBusiness.Rows[e.RowIndex].Cells["qty"].Style.ForeColor = Color.Black;
                    }
                    //품절이 아닌경우 숫자만 추출                    
                    if (dgvBusiness.Rows[e.RowIndex].Cells["qty"].Value == null || !double.TryParse(Regex.Replace(dgvBusiness.Rows[e.RowIndex].Cells["qty"].Value.ToString(), @"[^0-9.]", ""), out qty))
                        qty = 0;

                    double sale_price;
                    if (dgvBusiness.Rows[e.RowIndex].Cells["sale_price"].Value == null || !double.TryParse(dgvBusiness.Rows[e.RowIndex].Cells["sale_price"].Value.ToString(), out sale_price))
                        sale_price = 0;
                    else
                        dgvBusiness.Rows[e.RowIndex].Cells["sale_price"].Value = sale_price.ToString("#,##0");
                    dgvBusiness.Rows[e.RowIndex].Cells["sale_amount"].Value = (qty * sale_price).ToString("#,##0");
                }
                //소계
                else if (titleText == "unit")
                {
                    DataGridViewCell cell = dgvBusiness.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (cell.Value != null)
                    {
                        if (cell.Value.ToString() == "합계")
                            cell.Style.Font = new Font("굴림", 9, FontStyle.Bold);
                        else
                            cell.Style.Font = new Font("굴림", 9, FontStyle.Regular);


                        double d;
                        if (double.TryParse(cell.Value.ToString(), out d))
                            cell.Value = cell.Value.ToString() + "Kg";
                    }
                }
                //수량, 매출/매입/총액
                else if (titleText == "sale_price" || titleText == "purchase_price" || titleText == "sale_amount")
                {
                    double sale_price;
                    if (dgvBusiness.Rows[e.RowIndex].Cells["sale_price"].Value == null || !double.TryParse(dgvBusiness.Rows[e.RowIndex].Cells["sale_price"].Value.ToString(), out sale_price))
                        sale_price = 0;
                    else
                        dgvBusiness.Rows[e.RowIndex].Cells["sale_price"].Value = sale_price.ToString("#,##0");

                    double sale_amount;
                    if (dgvBusiness.Rows[e.RowIndex].Cells["sale_amount"].Value == null || !double.TryParse(dgvBusiness.Rows[e.RowIndex].Cells["sale_amount"].Value.ToString(), out sale_amount))
                        sale_amount = 0;
                    else
                        dgvBusiness.Rows[e.RowIndex].Cells["sale_amount"].Value = sale_amount.ToString("#,##0");

                    double purchase_price;
                    if (dgvBusiness.Rows[e.RowIndex].Cells["purchase_price"].Value == null || !double.TryParse(dgvBusiness.Rows[e.RowIndex].Cells["purchase_price"].Value.ToString(), out purchase_price))
                        purchase_price = 0;
                    else
                        dgvBusiness.Rows[e.RowIndex].Cells["purchase_price"].Value = purchase_price.ToString("#,##0");
                }
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            }                
        }
        #endregion

        #region Closing Insert
        private void DailyBusiness_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileSave(false);
            aTimer.Stop();
            //ServerBackup();
        }

        #endregion

        #region 거래처 드롭다운 자동완성
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox textBox = (TextBox)sender;

                // 현재 편집 중인 셀 가져오기
                DataGridViewCell currentCell = dgvBusiness.CurrentCell;

                // 현재 편집 중인 셀의 수정 내역 반영
                currentCell.Value = textBox.Text;

                // 수정 완료 후 TextBox 컨트롤 제거
                dgvBusiness.Controls.Remove(textBox);
            }
        }
        AutoCompleteStringCollection CompanyDataCollection = new AutoCompleteStringCollection();
        AutoCompleteStringCollection MyCompanyDataCollection = new AutoCompleteStringCollection();
        TextBox autoText = null;


        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;
        private const byte VK_BACK = 0x08;
        //private const byte VK_BACK = 0x12;


        private void dgvBusiness_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            /*// 특정 셀을 편집 모드로 변경
            dgvBusiness.BeginEdit(true);

            // 현재 편집 중인 TextBox 컨트롤 가져오기
            if (dgvBusiness.EditingControl is TextBox textBox)
            {
                textBox.Refresh();
                // TextBox 텍스트 조작하여 백스페이스 입력 시뮬레이트Aa
                string txt = "";
                if (textBox.Text.Length > 0)
                    txt = textBox.Text.Substring(0, textBox.Text.Length);
                // 백스페이스 키를 눌렀다는 KeyPress 이벤트 발생
                textBox.Focus();
                // 백스페이스 키를 강제로 누르기
                keybd_event(VK_BACK, 0, 0, 0);
                //keybd_event(VK_BACK, 0, KEYEVENTF_KEYUP, 0);
                //textBox.Text = txt;
                if (pre_keyData != Keys.None)
                {
                    string keyString = pre_keyData.ToString();
                    if (keyString.Length == 1 && char.IsLetter(keyString[0]))
                        keyString = $"{(int)keyString[0]:X4}";

                    // 이전에 입력한 키 값을 강제로 다시 입력
                    SendKeys.Send(keyString);
                    pre_keyData = Keys.None; // 변수 초기화
                }
            }*/


            DataGridViewRow curRow = dgvBusiness.CurrentCell.OwningRow;
            string titleText = dgvBusiness.CurrentCell.OwningColumn.Name;
            if (titleText.Equals("company"))
            {
                if (e.Control is DataGridViewComboBoxEditingControl comboBoxEditingControl)
                {
                    // 자동완성 드롭다운 활성화
                    comboBoxEditingControl.DropDownStyle = ComboBoxStyle.DropDown;
                    comboBoxEditingControl.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    comboBoxEditingControl.AutoCompleteSource = AutoCompleteSource.ListItems;
                }
                
                //자동완성 드롭박스
                autoText = e.Control as TextBox;
                autoText.TextChanged += textBox1_TextChanged;
                AutoComplete(titleText);
            }
            else
            {
                if (autoText != null)
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.None;
                    autoText.AutoCompleteSource = AutoCompleteSource.None;
                    autoText.AutoCompleteCustomSource = null;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (autoText.Text.Length > 2)
            {
                if (autoText.Text.Substring(autoText.Text.Length - 3, 3) == "(주)")
                {
                    autoText.Text = autoText.Text.Substring(0, autoText.Text.Length - 3) + "㈜";
                    autoText.Select(autoText.Text.Length, 0);
                }
            }
            
        }

        private void AutoComplete(string titleText)
        {
            if (autoText == null)
                return;

            if (titleText.Equals("company"))
            {
                if (autoText != null)
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                    autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    if(!cbMyCompany.Checked)
                        autoText.AutoCompleteCustomSource = CompanyDataCollection;
                    else
                        autoText.AutoCompleteCustomSource = MyCompanyDataCollection;
                }
                else
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.None;
                    autoText.AutoCompleteSource = AutoCompleteSource.None;
                    autoText.AutoCompleteCustomSource = null;
                }

            }
            /*else if (titleText.Equals("product"))
            {

                DataGridViewRow row = dgvBusiness.Rows[dgvBusiness.CurrentCell.RowIndex];
                if (row.Cells["company"].Value != null && !string.IsNullOrEmpty(row.Cells["company"].Value.ToString())
                    && (companyDIc.ContainsKey(row.Cells["company"].Value.ToString()) || codeDIc.ContainsKey(row.Cells["company"].Value.ToString())))
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                    autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
                    addProductItems(DataCollection, row);
                    autoText.AutoCompleteCustomSource = DataCollection;
                }
                else
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.None;
                    autoText.AutoCompleteSource = AutoCompleteSource.None;
                    autoText.AutoCompleteCustomSource = null;
                }
            }
            else if (titleText.Equals("sizes"))
            {

                DataGridViewRow row = dgvBusiness.Rows[dgvBusiness.CurrentCell.RowIndex];
                if (row.Cells["company"].Value != null && !string.IsNullOrEmpty(row.Cells["company"].Value.ToString())
                    && (companyDIc.ContainsKey(row.Cells["company"].Value.ToString()) || codeDIc.ContainsKey(row.Cells["company"].Value.ToString())))
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.Suggest;
                    autoText.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataCollection = new AutoCompleteStringCollection();
                    addSizeItems(DataCollection, row);
                    autoText.AutoCompleteCustomSource = DataCollection;
                }
                else
                {
                    autoText.AutoCompleteMode = AutoCompleteMode.None;
                    autoText.AutoCompleteSource = AutoCompleteSource.None;
                    autoText.AutoCompleteCustomSource = null;
                }
            }*/
            else
            {
                autoText.AutoCompleteMode = AutoCompleteMode.None;
                autoText.AutoCompleteSource = AutoCompleteSource.None;
                autoText.AutoCompleteCustomSource = null;
            }
        }


        public void addCompanyItems(AutoCompleteStringCollection col, AutoCompleteStringCollection myCol)
        {
            companyDt.Columns["매출처"].ReadOnly = false;
            companyDIc = new Dictionary<string, string>();
            for (int i = 0; i < companyDt.Rows.Count; i++)
            {
                string company = companyDt.Rows[i]["매출처"].ToString()
                                    .Replace("㈜", "").Replace("(주)", "").Replace("주식회사", "").Replace("법인", "")
                                    .Replace("(선)", "").Replace("선)", "").Replace("선.", "").Replace("선,", "")
                                    .Replace("(유)", "").Replace("(S)", "")
                                    .Replace("[", "(").Replace("]", ")")
                                    .Trim();
                companyDt.Rows[i]["매출처"] = company;
                if (!companyDIc.ContainsKey(company))
                {
                    companyDIc.Add(company, companyDt.Rows[i]["매출처"].ToString());
                    col.Add(company);

                    if(companyDt.Rows[i]["매출자"].ToString() == um.user_name)
                        myCol.Add(company); 
                }
            }
        }

        public void addProductItems(AutoCompleteStringCollection col, DataGridViewRow row)
        {
            if (row.Cells["company"].Value != null
                && !string.IsNullOrEmpty(row.Cells["company"].Value.ToString())
                && (companyDIc.ContainsKey(row.Cells["company"].Value.ToString()) || codeDIc.ContainsKey(row.Cells["company"].Value.ToString())))
            {
                string company = row.Cells["company"].Value.ToString();
                if(companyDIc.ContainsKey(row.Cells["company"].Value.ToString()))
                    company = companyDIc[row.Cells["company"].Value.ToString()];

                if (codeDIc.ContainsKey(company))
                {
                    string company_code = codeDIc[company].ToString();
                    if (productDIc.ContainsKey(company_code))
                    {
                        DataTable productDt = productDIc[company_code];
                        for (int i = 0; i < productDt.Rows.Count; i++)
                        {
                            col.Add(productDt.Rows[i]["품명"].ToString());
                        }
                    }
                }
            }
        }
        public void addSizeItems(AutoCompleteStringCollection col, DataGridViewRow row)
        {
            if (row.Cells["company"].Value != null
                && !string.IsNullOrEmpty(row.Cells["company"].Value.ToString())
                && (companyDIc.ContainsKey(row.Cells["company"].Value.ToString()) || codeDIc.ContainsKey(row.Cells["company"].Value.ToString())))
            {
                string company = row.Cells["company"].Value.ToString();
                if (companyDIc.ContainsKey(row.Cells["company"].Value.ToString()))
                    company = companyDIc[row.Cells["company"].Value.ToString()];

                if (codeDIc.ContainsKey(company))
                {
                    string company_code = codeDIc[company].ToString();
                    if (productDIc.ContainsKey(company_code))
                    {
                        DataTable productDt = productDIc[company_code];
                        for (int i = 0; i < productDt.Rows.Count; i++)
                        {
                            col.Add(productDt.Rows[i]["규격"].ToString());
                        }
                    }
                }
            }
        }


        private void dgvSales_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSales.Columns[e.ColumnIndex].Name == "input_qty")
                {
                    double qty;
                    if (dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || !double.TryParse(dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out qty))
                        qty = 0;
                    if (qty > 0 && dgvBusiness.CurrentCell != null)
                    {
                        //마지막줄이면 15개 추가
                        if (dgvBusiness.CurrentCell.RowIndex == dgvBusiness.RowCount - 1)
                            dgvBusiness.Rows.Add(15);


                        List<DataGridViewRow> list = new List<DataGridViewRow>();
                        list.Add(dgvSales.Rows[e.RowIndex]);
                        InputProduct2(list, true, false);
                    }
                }
                else if (dgvSales.Columns[e.ColumnIndex].Name == "pre_sale_price" || dgvSales.Columns[e.ColumnIndex].Name == "current_sale_price")
                {
                    double sale_price;
                    if (dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && double.TryParse(dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out sale_price))
                        dgvSales.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = sale_price.ToString("#,##0");
                }
            }
        }
        #endregion

        #region Datagridview cell value change EnterKey
        private void dgvBusiness_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvBusiness.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (dgvBusiness.SelectedRows.Count == 0)
                    {
                        dgvBusiness.ClearSelection();
                        dgvBusiness.Rows[e.RowIndex].Selected = true;
                    }
                }
            }
        }

        bool isTab = false;
        int tabStartIdx = 0;
        bool isPaste = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control tb = common.FindFocusedControl(this);
            //Tab시작한 컬럼 기억해놓기
            if (keyData == Keys.Tab)
            {
                if (!isTab)
                    tabStartIdx = dgvBusiness.CurrentCell.ColumnIndex;
                isTab = true;
            }
            else if (keyData == Keys.Up|| keyData == Keys.Down|| keyData == Keys.Left|| keyData == Keys.Right
                || keyData == Keys.PageUp || keyData == Keys.PageDown || keyData == Keys.Home || keyData == Keys.End)
                isTab = false;

            //붙혀넣기시 로우추가 설정
            if (keyData == (Keys.Control | Keys.V))
            {
                dgvBusiness.SetIsPasteRow(true);
                isPaste = true;
            }
            //복사하기
            else if (keyData == (Keys.Control | Keys.C))
            {
                if(dgvBusiness.Focused)
                    dgvBusiness.Copy();
                else if (dgvSales.Focused)
                    dgvSales.Copy();
                return true;
            }
            else
                isPaste = false;

            //취급품목아닌 품목 검색하기
            if ((dgvSales.Focused || txtSaleProduct.Focused || txtSaleOrigin.Focused || txtSaleSizes.Focused || txtSaleUnit.Focused)
                && (keyData == (Keys.Shift | Keys.Enter) || keyData == (Keys.Alt | Keys.Enter)))
            {
                GetNoneHandledProduct();
                return true;
            }
            //위로 이동
            else if (dgvBusiness.Focused && keyData == (Keys.Control | Keys.Up))
            {
                if (dgvBusiness.Rows.Count > 0 && dgvBusiness.CurrentCell != null)
                {
                    int row = dgvBusiness.CurrentCell.RowIndex;
                    int col = dgvBusiness.CurrentCell.ColumnIndex;

                    if (row - 1 >= 0)
                    {
                        string currentValue = string.Empty;
                        if (dgvBusiness.Rows[row].Cells[col].Value != null)
                            currentValue = dgvBusiness.Rows[row].Cells[col].Value.ToString();

                        for (int i = row - 1; i >= 0; i--)
                        {
                            string tempValue = string.Empty;
                            if (dgvBusiness.Rows[i].Cells[col].Value != null)
                                tempValue = dgvBusiness.Rows[i].Cells[col].Value.ToString();

                            if (currentValue != tempValue || i == 0)
                            {
                                if (i - 17 >= 0)
                                    dgvBusiness.FirstDisplayedScrollingRowIndex = i - 17;
                                else
                                    dgvBusiness.FirstDisplayedScrollingRowIndex = 0;
                                dgvBusiness.CurrentCell = dgvBusiness.Rows[i].Cells[col];
                                break;
                            }
                        }
                    }
                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            //아래로 이동
            else if (dgvBusiness.Focused && keyData == (Keys.Control | Keys.Down))
            {
                if (dgvBusiness.Rows.Count > 0 && dgvBusiness.CurrentCell != null)
                {
                    int row = dgvBusiness.CurrentCell.RowIndex;
                    int col = dgvBusiness.CurrentCell.ColumnIndex;

                    if (row + 1 < dgvBusiness.Rows.Count)
                    {
                        string currentValue = string.Empty;
                        if (dgvBusiness.Rows[row].Cells[col].Value != null)
                            currentValue = dgvBusiness.Rows[row].Cells[col].Value.ToString();

                        for (int i = row + 1; i < dgvBusiness.Rows.Count; i++)
                        {
                            string tempValue = string.Empty;
                            if (dgvBusiness.Rows[i].Cells[col].Value != null)
                                tempValue = dgvBusiness.Rows[i].Cells[col].Value.ToString();

                            if (currentValue != tempValue || dgvBusiness.Rows.Count - 1 == i)
                            {
                                dgvBusiness.CurrentCell = dgvBusiness.Rows[i].Cells[col];
                                if (i - 15 >= 0)
                                    dgvBusiness.FirstDisplayedScrollingRowIndex = i - 15;
                                else
                                    dgvBusiness.FirstDisplayedScrollingRowIndex = 0;

                                break;
                            }
                        }
                    }
                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            //쉬프트탭 뒤로
            else if (keyData == (Keys.Shift | Keys.Tab))
            {
                if (dgvBusiness.Focused && dgvBusiness.CurrentCell != null)
                {
                    int row = dgvBusiness.CurrentCell.RowIndex;
                    int col = dgvBusiness.CurrentCell.ColumnIndex - 1;
                    dgvBusiness.ClearSelection();
                    dgvBusiness.SelectionMode = DataGridViewSelectionMode.CellSelect;
                retry:
                    if (col < 0)
                    {
                        row--;
                        col = dgvBusiness.ColumnCount - 1;
                    }
                    if (row < 0)
                    {
                        row = 0;
                        col = 0;
                        dgvBusiness.CurrentCell = dgvBusiness.Rows[row].Cells[col];
                        return true;
                    }

                    if (!dgvBusiness.Rows[row].Cells[col].Visible)
                    {
                        col--;
                        goto retry;
                    }
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[row].Cells[col];

                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            //쉬프트탭 뒤로
            else if (keyData == (Keys.Shift | Keys.Enter))
            {
                dgvBusiness.EndEdit();
                if (dgvBusiness.Focused && dgvBusiness.CurrentCell != null)
                {
                    int row = dgvBusiness.CurrentCell.RowIndex - 1;
                    int col = dgvBusiness.CurrentCell.ColumnIndex;
                    //dgvBusiness.ClearSelection();
                    dgvBusiness.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    if (row < 0)
                        row = 0;
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[row].Cells[col];
                    return true;
                }
                else
                    return base.ProcessCmdKey(ref msg, keyData);
            }
            //캡쳐
            else if (keyData == (Keys.Control | Keys.Shift | Keys.C))
            {
                btnCapture.PerformClick();
                return true;
            }
            //거래처에서 Enter
            else if ((dgvBusiness.Focused || dgvBusiness.EditingControl != null) && keyData == Keys.Enter)
            {
                if (dgvBusiness.CurrentCell != null && dgvBusiness.CurrentCell.OwningColumn.Name == "company")
                {
                    //거래처 데이터 가져오기
                    /*if (dgvBusiness.CurrentCell.Value == null || string.IsNullOrEmpty(dgvBusiness.CurrentCell.Value.ToString()))
                        SelectNextRow();
                    else 
                    {*/
                    GetSales(true);
                    //SelectNextRow();
                    /*}*/
                }
                else
                {
                    //마지막줄일 경우 새줄 추가
                    if (dgvBusiness.CurrentCell != null && dgvBusiness.CurrentCell.RowIndex == dgvBusiness.Rows.Count - 1)
                        SelectNextRow();

                    //tab사용시 처음사용 컬럼, 아니면 같은 다음행 같은 컬럼 선택
                    SelectNextCell();
                }

                return true;
            }
            //거래처에서 Tab
            else if ((dgvBusiness.Focused || dgvBusiness.EditingControl != null) && keyData == Keys.Tab && dgvBusiness.CurrentCell.OwningColumn.Name == "company")
            {
                GetSales(true);
                dgvBusiness.CurrentCell = dgvBusiness.Rows[dgvBusiness.CurrentCell.RowIndex].Cells["origin"];
                return true;

            }
            else if ((txtSaleProduct.Focused || txtSaleOrigin.Focused || txtSaleSizes.Focused || txtSaleUnit.Focused) && keyData == Keys.Left)
            {
                TextBox focusTb = (TextBox)tb;
                if (focusTb.SelectionStart == 0)
                    tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
            }
            else if ((txtSaleProduct.Focused || txtSaleOrigin.Focused || txtSaleSizes.Focused || txtSaleUnit.Focused) && keyData == Keys.Right)
            {
                TextBox focusTb = (TextBox)tb;
                if (focusTb.SelectionStart == focusTb.TextLength)
                    tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
            }
            else if (tb != null && tb.Name != null
                && (tb.Name == "txtSaleProduct" || tb.Name == "txtSaleOrigin" || tb.Name == "txtSaleSizes" || tb.Name == "txtSaleUnit") && dgvSales.Rows.Count > 0)
            {
                switch (keyData)
                {
                    case Keys.Down:
                        dgvSales.Focus();
                        int idx = 0;
                    retry:
                        if (!dgvSales.Rows[idx].Visible)
                        {
                            idx++;
                            goto retry;
                        }

                        dgvSales.CurrentCell = dgvSales.Rows[idx].Cells["input_qty"];
                        return true;
                    default:
                        return base.ProcessCmdKey(ref msg, keyData);
                }
            }
            
            else
                return base.ProcessCmdKey(ref msg, keyData);

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SelectNextRow()
        {
            dgvBusiness.EndEdit();
            try
            {
                this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                var col = 0;
                var row = 0;
                if (dgvBusiness.CurrentCell != null)
                {
                    col = dgvBusiness.CurrentCell.ColumnIndex;
                    row = dgvBusiness.CurrentCell.RowIndex;
                }
                else if (dgvBusiness.Rows.Count == 0)
                {
                    col = 1;
                    row = -1;
                }

                if (row <= dgvBusiness.RowCount - 1)
                {
                    string input_date;
                    if (col == 0)
                    {
                        DateTime pre_input_date;
                        if (dgvBusiness.Rows[row].Cells[col].Value == null || !DateTime.TryParse(dgvBusiness.Rows[row].Cells[col].Value.ToString(), out pre_input_date))
                            pre_input_date = DateTime.Now;
                        input_date = pre_input_date.ToString("MM-dd");
                    }
                    else
                        input_date = DateTime.Now.ToString("MM-dd");



                    if (row == dgvBusiness.Rows.Count - 1 || dgvBusiness.Rows.Count == 0)
                    {
                        for (int i = 0; i < 15; i++)
                        {
                            int n = dgvBusiness.Rows.Add();
                            dgvBusiness.Rows[n].Cells["input_date"].Value = input_date;
                            dgvBusiness.FirstDisplayedScrollingRowIndex = dgvBusiness.RowCount - 1;
                        }
                    }
                    dgvBusiness.CurrentCell = dgvBusiness.Rows[row + 1].Cells[col];
                }
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            }
            catch
            {
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            }
        }

        private void SelectNextCell()
        {
            try
            {
                this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);

                if (dgvBusiness.CurrentCell == null && dgvBusiness.Rows.Count == 0)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        int n = dgvBusiness.Rows.Add();
                        dgvBusiness.Rows[n].Cells["input_date"].Value = DateTime.Now.ToString("MM-dd");
                    }
                    this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                    this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
                    return;
                }

                var col = dgvBusiness.CurrentCell.ColumnIndex;
                var row = dgvBusiness.CurrentCell.RowIndex + 1;

                //앞전 tab사용하지 않았을 경우
                if (isTab)
                    col = tabStartIdx;

                // Optional, select the first cell if the current cell is the last one.
                // You can return; here instead or add new row.
                if (row == dgvBusiness.RowCount) row = 0;

                if (!dgvBusiness[col, row].Visible)
                {
                    col++;
                    if (col > dgvBusiness.ColumnCount - 1)
                        col = 0;
                }
                dgvBusiness.CurrentCell = dgvBusiness[col, row];
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            }
            catch
            {
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            }
        }



        #endregion

        #region 선택된 행 정렬
        private void SelectedRangeSorting(string sort_col)
        {
            if (dgvBusiness.Rows.Count > 0)
            {
                int scroll_index = dgvBusiness.FirstDisplayedScrollingRowIndex;
                //선택한 행 정렬
                DataTable sortDt = ConvertToDatatable();
                if (sortDt.Rows.Count == 0)
                    return;
                //정렬
                DataView dv = new DataView(sortDt);
                dv.Sort = sort_col + ", company, product";
                sortDt = dv.ToTable();
                sortDt.AcceptChanges();
                //정렬한 Dgvr 추출

                DataGridView dgv_copy = new DataGridView();
                dgv_copy.AllowUserToAddRows = false;
                dgv_copy.AllowUserToDeleteRows = false;
                dgv_copy.Rows.Clear();
                try 
                {
                    foreach (DataGridViewColumn dgvc in dgvBusiness.Columns)
                    {
                        dgv_copy.Columns.Add(dgvc.Clone() as DataGridViewColumn);
                    }
                    
                    DataGridViewRow row = new DataGridViewRow();
                    for (int i = 0; i < sortDt.Rows.Count; i++)
                    {
                        int idx = Convert.ToInt32(sortDt.Rows[i]["idx"].ToString());
                        row = (DataGridViewRow)dgvBusiness.Rows[idx].Clone();
                        int intColIndex = 0;
                        foreach (DataGridViewCell cell in dgvBusiness.Rows[idx].Cells)
                        {
                            row.Cells[intColIndex].Value = cell.Value;
                            intColIndex++;
                        }
                        dgv_copy.Rows.Add(row);
                    }
                    dgv_copy.Refresh();

                }
                catch(Exception ex)
                {
                    messageBox.Show(this,ex.Message);
                    this.Activate();
                    return;
                }

                //선택한 셀 index
                int min_row = 99999, max_row = 0;
                foreach (DataGridViewRow row in dgvBusiness.SelectedRows)
                {
                    if (row.Index <= min_row)
                        min_row = row.Index;
                    if (row.Index >= max_row)
                        max_row = row.Index;
                }
                
                //정렬된 데이터 재출력
                this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                for (int i = max_row; i >= min_row; i--)
                    dgvBusiness.Rows.RemoveAt(i);
                for (int i = 0; i < dgv_copy.Rows.Count; i++)
                {
                    dgvBusiness.Rows.Insert(min_row + i, 1);
                    for (int j = 0; j < dgv_copy.ColumnCount; j++)
                    {
                        dgvBusiness.Rows[min_row + i].Cells[j].Value = dgv_copy.Rows[i].Cells[j].Value;
                        dgvBusiness.Rows[min_row + i].Cells[j].Style = dgv_copy.Rows[i].Cells[j].Style;
                    }
                }


                //창고별 색 구분
                if(sort_col == "warehouse")
                    SetWarehouseColor(min_row, max_row);

                dgvBusiness.ClearSelection();
                for (int i = min_row; i <= max_row; i++)
                    dgvBusiness.Rows[i].Selected = true;

                dgvBusiness.FirstDisplayedScrollingRowIndex = scroll_index;
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
            }
        }


        private void SetWarehouseColor(int min_row, int max_row)
        {
            int col_idx = -1;
            Dictionary<string, Color> colDic = new Dictionary<string, Color>();
            for (int i = min_row; i <= max_row; i++)
            {
                DataGridViewRow row = dgvBusiness.Rows[i];
                if (row.Cells["warehouse"].Value != null && !string.IsNullOrEmpty(row.Cells["warehouse"].Value.ToString().Trim()))
                {
                    string warehouse = row.Cells["warehouse"].Value.ToString().Trim();
                    if (colDic.ContainsKey(warehouse))
                    {
                        row.Cells["warehouse"].Style.BackColor = colDic[warehouse];
                    }
                    else
                    {
                        Color col = GetColorList(++col_idx);
                        colDic.Add(warehouse, col);
                        row.Cells["warehouse"].Style.BackColor = colDic[warehouse];
                    }
                }
                else
                    row.Cells["warehouse"].Style.BackColor = Color.White;

            }
        }

        private Color GetColorList(int idx)
        {
            Color col = Color.Black;
            switch (idx)
            {
                case 0: col = Color.Beige; break;
                case 1: col = Color.WhiteSmoke; break;
                case 2: col = Color.LightGray; break;
                case 3: col = Color.AntiqueWhite; break;
                case 4: col = Color.FloralWhite; break;
                case 5: col = Color.GhostWhite; break;
                case 6: col = Color.NavajoWhite; break;
                case 7: col = Color.PapayaWhip; break;
                case 8: col = Color.LightSalmon; break;
                case 9: col = Color.LightSteelBlue; break;
                case 10: col = Color.MintCream; break;
                case 11: col = Color.Linen; break;
                case 12: col = Color.Lime; break;
                case 13: col = Color.LightPink ; break;
                case 14: col = Color.LemonChiffon; break;
                case 15: col = Color.Ivory; break;
                case 16: col = Color.Olive; break;
                case 17: col = Color.Orchid; break;
                case 18: col = Color.Azure; break;
                case 19: col = Color.BurlyWood; break;
                case 20: col = Color.LavenderBlush; break;
                default: col = Color.Black; break;
            }
            return col;
        }


        private DataTable ConvertToDatatable()
        {
            DataTable dt = new DataTable();
            //컬럼설정
            foreach (DataGridViewColumn column in dgvBusiness.Columns)
                dt.Columns.Add(column.Name, typeof(string)); //better to have cell type

            dt.Columns.Add("idx", typeof(int)); //Add datagridview row index
            //데이터 추가
            if (dgvBusiness.SelectedRows.Count > 0)
            { 
                for (int i = 0; i < dgvBusiness.SelectedRows.Count; i++)
                {
                    dt.Rows.Add();
                    for (int j = 0; j < dgvBusiness.Columns.Count; j++)
                    {
                        dt.Rows[i][j] = dgvBusiness.SelectedRows[i].Cells[j].Value;
                        //^^^^^^^^^^^
                    }
                    dt.Rows[i]["idx"] = dgvBusiness.SelectedRows[i].Index;
                }
            }
            return dt;
        }
        #endregion

        #region Excel download
        private void ExcelDownload()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "영업거래처 관리", "영업일보", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            //비밀번호 확인
            PasswordCheckManager pcm = new PasswordCheckManager(um.excel_password);
            if (!pcm.isPasswordCheck())
                return;

            DataGridView dgv = dgvBusiness;
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
                        workSheet.Cells[i + 2, 1].Value = dgv.Rows[i].Cells["input_date"].Value;
                        workSheet.Cells[i + 2, 2].Value = dgv.Rows[i].Cells["company"].Value;
                        workSheet.Cells[i + 2, 3].Value = dgv.Rows[i].Cells["origin"].Value;
                        workSheet.Cells[i + 2, 4].Value = dgv.Rows[i].Cells["product"].Value;
                        workSheet.Cells[i + 2, 5].Value = dgv.Rows[i].Cells["sizes"].Value;
                        workSheet.Cells[i + 2, 6].Value = dgv.Rows[i].Cells["unit"].Value;
                        workSheet.Cells[i + 2, 7].Value = dgv.Rows[i].Cells["qty"].Value;
                        workSheet.Cells[i + 2, 8].Value = dgv.Rows[i].Cells["sale_price"].Value;
                        workSheet.Cells[i + 2, 9].Value = "=RC[-2]*RC[-1]";
                        workSheet.Cells[i + 2, 10].Value = dgv.Rows[i].Cells["warehouse"].Value;
                        workSheet.Cells[i + 2, 11].Value = dgv.Rows[i].Cells["purchase_company"].Value;
                        workSheet.Cells[i + 2, 12].Value = dgv.Rows[i].Cells["purchase_price"].Value;
                        workSheet.Cells[i + 2, 13].Value = dgv.Rows[i].Cells["remark"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                messageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
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
            //string document_name = txtDocumentName.Text.Trim().Replace("/", "").Replace(".", "").Replace(@"\", "").Replace("?", "").Replace("[", "").Replace("]", "").Replace("*", "");
            string document_name = "업무일지";
            if (!string.IsNullOrEmpty(document_name))
                workSheet.Name = document_name;

            //Column Width
            wk.Cells[1, 1].Value = "입력일자";
            wk.Cells[1, 2].Value = "거래처명";
            wk.Cells[1, 3].Value = "원산지";
            wk.Cells[1, 4].Value = "품명";
            wk.Cells[1, 5].Value = "규격";
            wk.Cells[1, 6].Value = "단위";
            wk.Cells[1, 7].Value = "수량";
            wk.Cells[1, 8].Value = "매출단가";
            wk.Cells[1, 9].Value = "총액";
            wk.Cells[1, 10].Value = "창고";
            wk.Cells[1, 11].Value = "매입처";
            wk.Cells[1, 12].Value = "매입단가";
            wk.Cells[1, 13].Value = "비고";

            //Font Style
            wk.Columns["B:B"].ColumnWidth = 25;
            wk.Columns["D:D"].ColumnWidth = 25;
            wk.Columns["E:E"].ColumnWidth = 15;
            wk.Columns["J:K"].ColumnWidth = 25;
            wk.Columns["M:M"].ColumnWidth = 25;

            //전체 영역 텍스트
            Excel.Range rg = wk.Range[wk.Cells[1, 1], wk.Cells[dgvBusiness.Rows.Count + 1, 13]];
            rg.NumberFormat = "@";

            //매출단가, 총액, 매입단가 배경색
            rg = wk.Range[wk.Cells[1, 8], wk.Cells[dgvBusiness.Rows.Count + 1, 8]];
            rg.Interior.Color = Color.FromArgb(198, 224, 180);

            rg = wk.Range[wk.Cells[1, 9], wk.Cells[dgvBusiness.Rows.Count + 1, 9]];
            rg.Interior.Color = Color.FromArgb(198, 224, 180);

            rg = wk.Range[wk.Cells[1, 12], wk.Cells[dgvBusiness.Rows.Count + 1, 12]];
            rg.Interior.Color = Color.FromArgb(221, 235, 247);

            //넓이
            wk.Columns["G"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            wk.Columns["G"].NumberFormatLocal = "#,##0";

            wk.Columns["H"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            wk.Columns["H"].NumberFormatLocal = "#,##0";

            wk.Columns["I"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            wk.Columns["I"].NumberFormatLocal = "#,##0";

            wk.Columns["L"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            wk.Columns["L"].NumberFormatLocal = "#,##0";


            //타이틀
            rg = wk.Range[wk.Cells[1, 1], wk.Cells[1 + rows, 13]];
            rg.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
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

        #region 백업데이터 불러오기
        public void GetServerBackupData(int id, int document_id)
        {
            this.dgvBusiness.isPush = false;
            this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
            dgvBusiness.Rows.Clear();
            DataTable businessDt = dailyBusinessRepository.GetTempData(um.user_id, document_id, id);
            int col_idx = 0;
            if (businessDt.Rows.Count > 0)
            {
                string tmp_date = businessDt.Rows[0]["input_date"].ToString();
                for (int i = 0; i < businessDt.Rows.Count; i++)
                {
                    //txtDocumentName.Text = businessDt.Rows[i]["document_name"].ToString();
                    lbDocumentId.Text = businessDt.Rows[i]["document_id"].ToString();

                    int n = dgvBusiness.Rows.Add();
                    DataGridViewRow row = dgvBusiness.Rows[n];
                    DateTime input_date;
                    if (DateTime.TryParse(businessDt.Rows[i]["input_date"].ToString(), out input_date))
                        row.Cells["input_date"].Value = input_date.ToString("MM-dd");
                    row.Cells["company"].Value = businessDt.Rows[i]["company"].ToString();
                    row.Cells["product"].Value = businessDt.Rows[i]["product"].ToString();
                    row.Cells["origin"].Value = businessDt.Rows[i]["origin"].ToString();
                    row.Cells["sizes"].Value = businessDt.Rows[i]["sizes"].ToString();
                    row.Cells["unit"].Value = businessDt.Rows[i]["unit"].ToString();
                    row.Cells["qty"].Value = businessDt.Rows[i]["qty"].ToString();
                    row.Cells["sale_price"].Value = businessDt.Rows[i]["sale_price"].ToString();
                    /*double sale_price;
                    if (!double.TryParse(businessDt.Rows[i]["sale_price"].ToString(), out sale_price))
                        sale_price = 0;
                    row.Cells["sale_price"].Value = sale_price.ToString("#,##0");*/
                    row.Cells["warehouse"].Value = businessDt.Rows[i]["warehouse"].ToString();
                    row.Cells["purchase_company"].Value = businessDt.Rows[i]["purchase_company"].ToString();
                    /*double purchase_price;
                    if (!double.TryParse(businessDt.Rows[i]["purchase_price"].ToString(), out purchase_price))
                        purchase_price = 0;
                    row.Cells["purchase_price"].Value = purchase_price.ToString("#,##0");*/
                    row.Cells["purchase_price"].Value = businessDt.Rows[i]["purchase_price"].ToString();
                    row.Cells["freight"].Value = businessDt.Rows[i]["freight"].ToString();
                    row.Cells["remark"].Value = businessDt.Rows[i]["remark"].ToString();
                    row.Cells["inquire"].Value = businessDt.Rows[i]["inquire"].ToString();
                    //검색항목
                    bool isOutput = true;

                    DateTime createtime;
                    if (DateTime.TryParse(businessDt.Rows[i]["createtime"].ToString(), out createtime))
                        lbCreatetime.Text = createtime.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        lbCreatetime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    DateTime updatetime;
                    if (DateTime.TryParse(businessDt.Rows[i]["updatetime"].ToString(), out updatetime))
                        lbUpdatetime.Text = updatetime.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        lbUpdatetime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");


                    string[] cell_style = businessDt.Rows[i]["cell_style"].ToString().Trim().Split('\n');
                    if (cell_style.Length > 0)
                    {
                        for (int j = 0; j < cell_style.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(cell_style[j]))
                            {
                                string[] style = cell_style[j].Split('^');
                                if (style.Length > 5)
                                {
                                    int font_size;
                                    if (!int.TryParse(style[1], out font_size))
                                        font_size = 9;

                                    string font_name = style[2];

                                    bool italic;
                                    if (!bool.TryParse(style[4], out italic))
                                        italic = false;

                                    bool bold;
                                    if (!bool.TryParse(style[3], out bold))
                                        bold = false;

                                    if (bold && italic)
                                        row.Cells[style[0]].Style.Font = new Font(font_name, font_size, FontStyle.Bold | FontStyle.Italic);
                                    else if (bold && !italic)
                                        row.Cells[style[0]].Style.Font = new Font(font_name, font_size, FontStyle.Bold);
                                    else if (!bold && italic)
                                        row.Cells[style[0]].Style.Font = new Font(font_name, font_size, FontStyle.Italic);
                                    else
                                        row.Cells[style[0]].Style.Font = new Font(font_name, font_size, FontStyle.Regular);


                                    int fore_col_rgb = Convert.ToInt32(style[5].Replace("#", ""));
                                    Color fore_col = Color.FromArgb(fore_col_rgb);
                                    row.Cells[style[0]].Style.ForeColor = fore_col;

                                    int back_col_rgb = Convert.ToInt32(style[6].Replace("#", ""));
                                    Color back_col = Color.FromArgb(back_col_rgb);
                                    row.Cells[style[0]].Style.BackColor = back_col;

                                }
                            }
                        }
                    }

                }

                //가장마지막에 입력한 위치로 이동
                for (int i = dgvBusiness.Rows.Count - 1; i >= 0; i--)
                {
                    if ((dgvBusiness.Rows[i].Cells["company"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[i].Cells["company"].Value.ToString()))
                        || (dgvBusiness.Rows[i].Cells["product"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[i].Cells["product"].Value.ToString()))
                        || (dgvBusiness.Rows[i].Cells["sizes"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[i].Cells["sizes"].Value.ToString()))
                        || (dgvBusiness.Rows[i].Cells["warehouse"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[i].Cells["warehouse"].Value.ToString()))
                        || (dgvBusiness.Rows[i].Cells["purchase_company"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[i].Cells["purchase_company"].Value.ToString()))
                        || (dgvBusiness.Rows[i].Cells["remark"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[i].Cells["remark"].Value.ToString()))
                        || (dgvBusiness.Rows[i].Cells["inquire"].Value != null && !string.IsNullOrEmpty(dgvBusiness.Rows[i].Cells["inquire"].Value.ToString())))
                    {
                        dgvBusiness.CurrentCell = dgvBusiness.Rows[i].Cells["company"];
                        break;
                    }
                }
                //dgvBusiness.FirstDisplayedScrollingRowIndex = businessDt.Rows.Count - 1;
                dgvBusiness.Focus();
                lbDocumentDivision.Text = "서버 백업";
            }
            else
            {
                lbDocumentId.Text = "1";
                for (int i = 0; i < 50; i++)
                {
                    int n = dgvBusiness.Rows.Add();
                    dgvBusiness.Rows[n].Cells["input_date"].Value = DateTime.Now.ToString("MM-dd");
                }
            }
            this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
            this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);
            this.dgvBusiness.isPush = true;
            this.dgvBusiness.Push();
        }

        #endregion

        #region 선택한 Datagridview 색

        private void dgvBusiness_Enter(object sender, EventArgs e)
        {
            dgvSales.DefaultCellStyle.SelectionBackColor = Color.LightGray;
            dgvBusiness.DefaultCellStyle.SelectionBackColor = Color.SkyBlue;
        }

        private void dgvSales_Enter(object sender, EventArgs e)
        {
            dgvSales.DefaultCellStyle.SelectionBackColor = Color.SkyBlue;
            dgvBusiness.DefaultCellStyle.SelectionBackColor = Color.LightGray;
        }
        #endregion

        #region Sheet event

        private void btnAddSheet_Click(object sender, EventArgs e)
        {
            int shtCnt = pnSheets.Controls.Count;
        retry:
            int tabIndex = 0;
            foreach (Control con in pnSheets.Controls)
            {
                if (con.Name == "Sheet" + shtCnt)
                {
                    shtCnt++;
                    goto retry;
                }
                tabIndex++;
            }
            //시트 버튼생성
            Button btnSheet = new Button();
            //btnSheet.MouseClick += btnExit_MouseClick;
            btnSheet.Name = "Sheet" + shtCnt;
            btnSheet.Text = "Sheet" + shtCnt;
            btnSheet.TabIndex = tabIndex;
            btnSheet.Padding = new Padding(0, 0, 0, 0);
            btnSheet.Margin = new Padding(0, 0, 0, 0);
            btnSheet.Width = 100;
            btnSheet.Height = 21;
            btnSheet.MouseUp += BtnSheet_MouseUp;
            btnSheet.MouseMove += btn_ButtonMove;
            btnSheet.MouseDown += btn_ButtonDown;
            btnSheet.Click += btn_Click;
            pnSheets.Controls.Add(btnSheet);
            pnSheets.Controls.SetChildIndex(btnSheet, tabIndex - 1);

            //dtStack 초기화
            dgvBusiness.StackDictionaryInitialize(shtCnt);

            btnSheet.PerformClick();
        }

        string btnName = null;
        Point btnPoint;
        private void BtnSheet_MouseUp(object sender, MouseEventArgs e)
        {
            //이동
            if (buttonMoveFlage)
            {
                buttonMoveFlage = false;
                pnSheets.Controls[selectButtonIndex].Location = new System.Drawing.Point(
                    (pnSheets.Controls[selectButtonIndex].Location.X + e.X) - gapX,
                    (pnSheets.Controls[selectButtonIndex].Location.Y + e.Y) - gapY);
            }

            try
            {
                Button btn = (Button)sender;
                btnName = btn.Name;
                //한글 변환
                ChangeIME(true);
                if (e.Button == MouseButtons.Right)
                {
                    //영어 변환
                    ChangeIME(false);

                    m = new ContextMenuStrip();
                    m.Items.Add("삭제");
                    m.Items.Add("제목 수정");

                    //Create 
                    m.ItemClicked += new ToolStripItemClickedEventHandler(btn_ItemClicked);
                    m.BackColor = Color.White;
                    btnPoint = e.Location;
                    m.Show(btn, e.Location);
                    
                }
                //ChangeIME(true);
            }
            catch
            {
            }
        }

        void btn_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            dgvBusiness.EndEdit();
            switch (e.ClickedItem.Text)
            {
                case "삭제":
                    {
                        foreach (Control col in pnSheets.Controls)
                        {
                            if (col.Name == btnName)
                            {
                                dgvBusiness.Rows.Clear();
                                pnSheets.Controls.Remove(col);
                                int shtCnt = Convert.ToInt16(col.Name.Replace("Sheet", ""));
                                if (sheetDataDic.ContainsKey(col.Name))
                                {
                                    sheetDataDic.Remove(col.Name);
                                    //dtStack 초기화
                                    dgvBusiness.StackDictionaryInitialize(shtCnt);
                                }
                                foreach (Control col2 in pnSheets.Controls)
                                {
                                    if (col2.Name != "btnAddSheet")
                                    {
                                        col2.BackColor = Color.White;
                                        col2.ForeColor = Color.Blue;
                                        GetSheetDataByDic("", col2.Name, false);

                                    }
                                    else
                                    {
                                        col2.BackColor = Color.LightGray;
                                        col2.ForeColor = Color.Black;
                                    }
                                }

                                break;
                            }
                        }
                    }
                    break;
                case "제목 수정":
                    {
                        foreach (Control col in pnSheets.Controls)
                        {
                            if (col.Name == btnName)
                            {

                                Control con = common.GetControl(Control.MousePosition);
                                if (con != null)
                                {
                                    Point p = con.Location;
                                    /*dgv.PointToScreen(dgv.GetCellDisplayRectangle(0, i, false).Location);*/

                                    SheetNameManager snm = new SheetNameManager(col, p);
                                    try { snm.ShowDialog(); }
                                    catch
                                    { }
                                }
                                break;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void btnExit_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)               
                ((Button)sender).PerformClick();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            //선택한 버튼 초기화
            string preBtnName = lbCurrentSheetName.Text;
            GetSheetDataByDic(preBtnName, btn.Name);
            //dtStack 변경
            int keys = Convert.ToInt16(btn.Name.Replace("Sheet", ""));
            dgvBusiness.SetStackIdx(keys);
        }


        Boolean buttonMoveFlage = false;
        int selectButtonIndex, gapX, gapY;
        private void btn_ButtonDown(object sender, MouseEventArgs e)
        {
            if (!buttonMoveFlage)
            {
                buttonMoveFlage = true;
                string buttonName = ((Button)sender).Name;
                foreach (Control con in pnSheets.Controls) 
                {
                    if (con.Name.Equals(buttonName))
                    {
                        selectButtonIndex = pnSheets.Controls.IndexOf(con);
                        gapX = e.Location.X;
                        gapY = e.Location.Y;
                    }
                }
            }
        }
        private void btn_ButtonMove(object sender, MouseEventArgs e)
        {
            if (buttonMoveFlage)
            {
                //Point p = new Point(e.Location.X, e.Location.Y);
                Control con = common.GetControl(Control.MousePosition);

                
                if (con != null)
                {
                    int childIdx = 0;
                    foreach (Control control in pnSheets.Controls)
                    {
                        if (control.Name.Equals(con.Name) && control.Name != "btnAddSheet")
                        {
                            if (con.Location.X + con.Width > e.X)
                            {
                                childIdx = pnSheets.Controls.GetChildIndex(control);
                                pnSheets.Controls.SetChildIndex(pnSheets.Controls[selectButtonIndex], childIdx);
                            }
                        }
                        childIdx++;
                    } 
                }
                /*pnSheets.Controls[selectButtonIndex].Location = new System.Drawing.Point(
                    (pnSheets.Controls[selectButtonIndex].Location.X + e.X) - gapX,
                    (pnSheets.Controls[selectButtonIndex].Location.Y + e.Y) - gapY);*/
                pnSheets.Update();
            }
        }

        private void btnSeaoverProduct_Click(object sender, EventArgs e)
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
                GetProductList pl = new GetProductList(um);
                pl.Show();
            }
        }

        private void GetSheetDataByDic(string preBtnName, string btnName, bool preDataSave = true)
        {
            //수정사항 반영
            if (preDataSave && !string.IsNullOrEmpty(preBtnName))
            {
                foreach (Control con in pnSheets.Controls)
                {
                    if (con.Name.Equals(preBtnName))
                    {
                        DataTable currentShtData = jsonCommon.ConvertToDatatable(con.Name, this.dgvBusiness);
                        if (sheetDataDic.ContainsKey(con.Name))
                            sheetDataDic[con.Name] = currentShtData;
                        else
                            sheetDataDic.Add(con.Name, currentShtData);
                        break;
                    }
                }
            }

            //선택한 시트 활성화
            Button btn;
            foreach (Control con in pnSheets.Controls)
            {
                if (con is Button)
                {
                    if (con.Name == btnName)
                    {
                        btn = (Button)con;
                        btn.BackColor = Color.White;
                        btn.ForeColor = Color.Blue;

                        int shtCnt = Convert.ToInt32(btn.Name.Replace("Sheet", ""));
                        if (sheetDataDic.ContainsKey(btn.Name))
                            SetSheetData(sheetDataDic[btn.Name]);
                        else
                            NewDocument();
                        lbCurrentSheetName.Text = con.Name;
                    }
                    else
                    {
                        btn = (Button)con;
                        btn.BackColor = Color.LightGray;
                        btn.ForeColor = Color.Black;
                    }
                }
            }
        }


        private void SetSheetData(DataTable shtDataDt)
        {
            dgvBusiness.isPush = false;
            dgvBusiness.Rows.Clear();
            if (shtDataDt != null && shtDataDt.Rows.Count > 0)
            {
                this.dgvBusiness.RowsAdded -= new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);

                try
                {
                    for (int i = 0; i < shtDataDt.Rows.Count; i++)
                    {
                        int n = dgvBusiness.Rows.Add();
                        DataGridViewRow row = dgvBusiness.Rows[n];

                        for (int j = 0; j < shtDataDt.Columns.Count; j++)
                        {
                            string col_name = shtDataDt.Columns[j].ColumnName;
                            //셀 스타일 정보
                            if (col_name.Contains("_style"))
                            {
                                col_name = col_name.Replace("_style", "");
                                string[] styleData = shtDataDt.Rows[i][j].ToString().Split('_');
                                if (styleData.Length > 5)
                                {
                                    double font_sizes = Convert.ToDouble(styleData[0]);
                                    string font_name = styleData[1];
                                    bool font_bold = Convert.ToBoolean(styleData[2]);
                                    bool font_italic = Convert.ToBoolean(styleData[3]);
                                    FontFamily fontFamily;
                                    fontFamily = new FontFamily(font_name);
                                    Font font;
                                    if (font_bold && font_italic)
                                        font = new Font(fontFamily, (float)font_sizes, FontStyle.Bold | FontStyle.Italic);
                                    else if (font_bold && !font_italic)
                                        font = new Font(fontFamily, (float)font_sizes, FontStyle.Bold);
                                    else if (!font_bold && font_italic)
                                        font = new Font(fontFamily, (float)font_sizes, FontStyle.Italic);
                                    else
                                        font = new Font(fontFamily, (float)font_sizes, FontStyle.Regular);

                                    row.Cells[col_name].Style.Font = font;

                                    Color fore_col = Color.Black;
                                    if (int.TryParse(styleData[4].Replace("#", ""), out int fore_col_rgb))
                                        fore_col = Color.FromArgb(fore_col_rgb);
                                    row.Cells[col_name].Style.ForeColor = fore_col;


                                    Color back_col = Color.White;
                                    if (int.TryParse(styleData[5].Replace("#", ""), out int back_col_rgb))
                                        back_col = Color.FromArgb(back_col_rgb);
                                    row.Cells[col_name].Style.BackColor = back_col;

                                }
                            }
                            else if(!col_name.Equals("shtName") && !col_name.Equals("row"))
                                row.Cells[col_name].Value = shtDataDt.Rows[i][j].ToString();

                        }
                    }
                }
                catch (Exception ex)
                {
                    //GetData();
                    messageBox.Show(this, ex.Message);
                    this.Activate();
                }
                this.dgvBusiness.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBusiness_RowsAdded);
                this.dgvBusiness.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBusiness_CellValueChanged);

                LocationCurrentCell();
            }
            dgvBusiness.isPush = true;
        }

        #endregion
    }
}
