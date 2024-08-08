using AdoNetWindow.Model;
using AdoNetWindow.Product;
using Repositories;
using Repositories.Config;
using Repositories.Pending;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AdoNetWindow.Pending
{
    public partial class UnPendingManager : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        IPaymentRepository paymentRepository = new PaymentRepository();
        ICommonRepository commonRepository = new CommonRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IShippingRepository shippingRepository = new ShippingRepository();
        List<ClipboardModel> clipboardModel = new List<ClipboardModel>();

        List<AllCustomsModel> origin_model;

        CalendarModule.calendar cal = null;
        UnconfirmedPending pendingList = null;
        UsersModel um;
        
        int id;
        string type;
        editMode em;
        enum editMode 
        {
            등록,
            수정
        }
        public UnPendingManager(CalendarModule.calendar calendar, UsersModel uModel, string sType, int customs_id, UnconfirmedPending ufpendingList = null)
        {
            InitializeComponent();
            pendingList = ufpendingList;
            cal = calendar;
            um = uModel;
            id = customs_id;
            type = sType;
        }

        public UnPendingManager(CalendarModule.calendar calendar, UsersModel uModel, string ato_no, UnconfirmedPending ufpendingList = null)
        {
            InitializeComponent();
            pendingList = ufpendingList;
            cal = calendar;
            um = uModel;
            id = customsRepository.GetCustomsId(ato_no);
            //type = "미통관";
        }
        public UnPendingManager(UsersModel uModel, int custom_id)
        {
            InitializeComponent();
            um = uModel;
            id = custom_id;
        }

        private void UnPendingManager_Load(object sender, EventArgs e)
        {
            //업체별시세현황 스토어프로시져 호출
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            txtPaymentEditUser.Text = um.user_name;
            if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this,"호출 내용이 없음");
                this.Activate();
                return;
            }
            //DgvUnPending Columns Header Style 
            SetDgvUnpendigHeaderSetting();
            txtPaymentDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            //Get Trq
            double trq = commonRepository.GetTrq();
            txtTrq.Text = trq.ToString("#,##0");
            //Get Pending Data
            dgvUnpending.Init();
            GetData();
            GetPay();
            //GetAttachment();
            //결제잔액
            double total_amount = GetTotalAmount();
            double total_payment = GetTotalPayment();
            txtTotalBalance.Text = (total_amount - total_payment).ToString("#,##0.00");
            txtPaymentAmount.Text = (total_amount - total_payment).ToString("#,##0.00");
            //총 중량, 금액계산
            TotalCalculate();
        }

        #region Method
        private string GetN2(string A)
        {
            double B = Convert.ToDouble(A);
            string result;
            if (B == (int)B)
                result = B.ToString("#,##0");
            else
                result = B.ToString("N2");

            return result;
        }

        private void GetPay()
        {
            DataGridView dgv = dgvPayment;
            dgv.Rows.Clear();
            DataTable payDt = paymentRepository.GetPayment("", id.ToString(),"pending", "");
            if (payDt.Rows.Count > 0)
            {
                double total_payment_amount;
                if (!double.TryParse(txtTotalPrice.Text, out total_payment_amount))
                    total_payment_amount = 0;

                for (int i = 0; i < payDt.Rows.Count; i++)
                { 
                    int n = dgv.Rows.Add();
                    dgv.Rows[n].Cells["pay_id"].Value = payDt.Rows[i]["id"].ToString();
                    dgv.Rows[n].Cells["pay_currency"].Value = payDt.Rows[i]["payment_currency"].ToString();
                    dgv.Rows[n].Cells["pay_date_status"].Value = payDt.Rows[i]["payment_date_status"].ToString();
                    dgv.Rows[n].Cells["pay_date"].Value = Convert.ToDateTime(payDt.Rows[i]["payment_date"].ToString()).ToString("yyyy-MM-dd");

                    double payment_amount;
                    if (!double.TryParse(payDt.Rows[i]["payment_amount"].ToString(), out payment_amount))
                        payment_amount = 0;
                    total_payment_amount -= payment_amount;
                    dgv.Rows[n].Cells["pay_amount"].Value = payment_amount.ToString("#,##0.00");
                    dgv.Rows[n].Cells["remark"].Value = payDt.Rows[i]["remark"].ToString();
                    dgv.Rows[n].Cells["updatetime"].Value = Convert.ToDateTime(payDt.Rows[i]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                    dgv.Rows[n].Cells["edit_user"].Value = payDt.Rows[i]["edit_user"].ToString();
                }
                dgv.ClearSelection();
                txtTotalPrice.Text = total_payment_amount.ToString("#,##0");
            }
        }
        private void GetData()
        {
            //Call Unpending Data
            DataGridView dgv = dgvUnpending;

            ShippingModel sModel = shippingRepository.GetShipping(id.ToString());
            if (sModel.custom_id.ToString() == id.ToString())
                txtRemark.Text = sModel.remark;


            List<AllCustomsModel> model = customsRepository.GetAllTypePending(type, id.ToString());
            origin_model = model;  // 기존 정보 저장
            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    int n = dgv.Rows.Add();
                    dgv.Rows[n].Cells["contract_year"].Value = model[i].contract_year;
                    dgv.Rows[n].Cells["ato_no"].Value = model[i].ato_no;
                    dgv.Rows[n].Cells["pi_date"].Value = model[i].pi_date;
                    dgv.Rows[n].Cells["contract_no"].Value = model[i].contract_no;
                    dgv.Rows[n].Cells["shipper"].Value = model[i].shipper;
                    dgv.Rows[n].Cells["lc_open_date"].Value = model[i].lc_open_date;
                    dgv.Rows[n].Cells["lc_no"].Value = model[i].lc_payment_date;
                    dgv.Rows[n].Cells["bl_no"].Value = model[i].bl_no;
                    dgv.Rows[n].Cells["shipment_date"].Value = model[i].shipment_date;
                    dgv.Rows[n].Cells["etd"].Value = model[i].etd;
                    dgv.Rows[n].Cells["eta"].Value = model[i].eta;
                    dgv.Rows[n].Cells["warehouse_date"].Value = model[i].warehousing_date;
                    dgv.Rows[n].Cells["pending_date"].Value = model[i].pending_check;

                    dgv.Rows[n].Cells["loading_cost_per_box"].Value = model[i].loading_cost_per_box;
                    dgv.Rows[n].Cells["total_amount_seaover"].Value = model[i].total_amount_seaover;
                    dgv.Rows[n].Cells["refrigeration_charge"].Value = model[i].refrigeration_charge;
                    dgv.Rows[n].Cells["box_price_adjust"].Value = model[i].box_price_adjust;
                    dgv.Rows[n].Cells["shipping_box_price_adjust"].Value = model[i].shipping_box_price_adjust;
                    dgv.Rows[n].Cells["custom_remark"].Value = model[i].remark;
                    dgv.Rows[n].Cells["trq_amount"].Value = model[i].trq_amount;
                    dgv.Rows[n].Cells["shipping_trq_amount"].Value = model[i].shipping_trq_amount;
                    if (model[i].trq_amount > 0)
                        dgv.Rows[n].Cells["is_trq"].Value = true;
                    else
                        dgv.Rows[n].Cells["is_trq"].Value = false;
                    dgv.Rows[n].Cells["clearance_rate"].Value = model[i].clearance_rate;

                    cbCalendar.Checked = model[i].is_calendar;
                    cbShipmentQty.Checked = model[i].is_shipment_qty;

                    cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("미통관");
                    cCell.Items.Add("통관");
                    /*cCell.Items.Add("확정");*/
                    dgv.Rows[n].Cells["cc_status"] = cCell;
                    if (model[i].cc_status == "미통")
                        dgv.Rows[n].Cells["cc_status"].Value = "미통관";
                    else
                    {
                        try
                        {
                            dgv.Rows[n].Cells["cc_status"].Value = model[i].cc_status;
                        }
                        catch
                        {
                            cCell.Items.Add(model[i].cc_status);
                            dgv.Rows[n].Cells["cc_status"].Value = model[i].cc_status;
                        }
                    }
                    dgv.Rows[n].Cells["customs_officer"].Value = model[i].broker;
                    dgv.Rows[n].Cells["warehouse"].Value = model[i].warehouse;
                    dgv.Rows[n].Cells["origin"].Value = model[i].origin;
                    dgv.Rows[n].Cells["product"].Value = model[i].product;
                    dgv.Rows[n].Cells["sizes"].Value = model[i].sizes;
                    dgv.Rows[n].Cells["box_weight"].Value = model[i].box_weight;
                    dgv.Rows[n].Cells["cost_unit"].Value = model[i].cost_unit;
                    dgv.Rows[n].Cells["unit_price"].Value = model[i].unit_price;
                    dgv.Rows[n].Cells["contract_qty"].Value = Convert.ToDouble(model[i].quantity_on_paper).ToString("#,##0.00");
                    dgv.Rows[n].Cells["warehouse_qty"].Value = Convert.ToDouble(model[i].qty).ToString("#,##0.00");
                    dgv.Rows[n].Cells["tariff_rate"].Value = model[i].tariff_rate * 100;
                    dgv.Rows[n].Cells["vat_rate"].Value = model[i].vat_rate * 100;
                    dgv.Rows[n].Cells["is_quarantine"].Value = model[i].is_quarantine;


                    DateTime payment_date;
                    if (!(model[i].payment_date == null || model[i].payment_date == "") && DateTime.TryParse(model[i].payment_date, out payment_date))
                    {
                        dgv.Rows[n].Cells["payment_date"].Value = payment_date.ToString("yyyy-MM-dd");
                        txtPaymentDate.Text = payment_date.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        dgv.Rows[n].Cells["payment_date"].Value = "";
                    }

                    cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("");
                    cCell.Items.Add("미확정");
                    cCell.Items.Add("확정");
                    cCell.Items.Add("확정(LG)");
                    cCell.Items.Add("확정(마감)");
                    cCell.Items.Add("결제완료");
                    dgv.Rows[n].Cells["payment_date_status"] = cCell;

                    try
                    {
                        dgv.Rows[n].Cells["payment_date_status"].Value = model[i].payment_date_status;
                    }
                    catch
                    {
                        cCell.Items.Add(model[i].payment_date_status);
                        dgv.Rows[n].Cells["payment_date_status"].Value = model[i].payment_date_status;
                    }

                    cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("국민");
                    cCell.Items.Add("기업");
                    cCell.Items.Add("부산");
                    cCell.Items.Add("신한");
                    cCell.Items.Add("수협");
                    cCell.Items.Add("하나");
                    /*dgv.Rows[n].Cells["payment_bank"] = cCell;*/
                    string bank = model[i].payment_bank.Replace("에이티오", "").Trim();

                    if (bank != "국민"
                        && bank != "기업"
                        && bank != "부산"
                        && bank != "신한"
                        && bank != "수협"
                        && bank != "하나")

                    {
                        cCell.Items.Add(bank);
                    }
                    dgv.Rows[n].Cells["payment_bank"].Value = bank;

                    cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("");
                    cCell.Items.Add("US");
                    cCell.Items.Add("AT");
                    cCell.Items.Add("T/T");
                    dgv.Rows[n].Cells["usance_type"] = cCell;

                    if (model[i].usance_type == ""
                    || model[i].usance_type == "US"
                    || model[i].usance_type == "AT"
                    || model[i].usance_type == "T/T")
                    {
                        cCell.Value = model[i].usance_type;
                    }
                    else if (model[i].usance_type == "O")
                    {
                        cCell.Value = "US";
                    }
                    else if (model[i].usance_type == "X")
                    {
                        cCell.Value = "";
                    }
                    else
                    {
                        cCell.Items.Add(model[i].usance_type);
                        cCell.Value = model[i].usance_type;
                    }
                    cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("O");
                    cCell.Items.Add("X");
                    /*dgv.Rows[n].Cells["agency_type"] = cCell;*/
                    dgv.Rows[n].Cells["agency_type"].Value = model[i].agency_type;

                    cCell = new DataGridViewComboBoxCell();
                    cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    cCell.Items.Add("아토");
                    cCell.Items.Add("에이티오");
                    cCell.Items.Add("동일");
                    cCell.Items.Add("수리미");
                    cCell.Items.Add("해금");
                    cCell.Items.Add("에스제이");
                    cCell.Items.Add("푸드마을");
                    cCell.Items.Add("에프원에프엔비");
                    cCell.Items.Add("동양섬유");
                    cCell.Items.Add("이안수산");
                    cCell.Items.Add("아토코리아");
                    /*dgv.Rows[n].Cells["division"] = cCell;*/

                    string division = model[i].division;

                    if (division != "아토"
                        && division != "에이티오"
                        && division != "동일"
                        && division != "수리미"
                        && division != "해금"
                        && division != "에스제이"
                        && division != "푸드마을"
                        && division != "에프원에프엔비"
                        && division != "동양섬유"
                        && division != "이안수산"
                        && division != "아토코리아")

                    {
                        cCell.Items.Add(division);
                        dgv.Rows[n].Cells["division"].Value = division;
                    }

                    dgv.Rows[n].Cells["sanitary_certificate"].Value = model[i].sanitary_certificate;
                    dgv.Rows[n].Cells["manager"].Value = model[i].manager;
                    dgv.Rows[n].Cells["division"].Value = model[i].division;
                    dgv.Rows[n].Cells["weight_calculate"].Value = model[i].weight_calculate;
                    dgv.Rows[n].Cells["weight_calculate"].ToolTipText = "TRUE : 박스단가\nFALSE : 트레이단가";
                    dgv.Rows[n].Cells["import_number"].Value = model[i].import_number;
                }
                //첫번째 선택
                DataGridViewRow selectRow = this.dgvUnpending.Rows[0];
                selectRow.Selected = true;
                //null -> ""
                foreach (DataGridViewCell c in selectRow.Cells)
                {
                    if (c.Value == null)
                        c.Value = "";
                }
            }
            dgv.ClearSelection();
        }
        private void SetFontSizes()
        {
            int sizes = (int)nudFontsize.Value;
            dgvUnpending.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", sizes, FontStyle.Regular);
            foreach (DataGridViewRow row in dgvUnpending.Rows)
            {
                row.DefaultCellStyle.Font = new Font("나눔고딕", sizes, FontStyle.Regular);
            }
        }
        private void UpdatePaymentDate()
        {
            {
                //권한확인
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }

                Common.Calendar calendar = new Common.Calendar();
                string paymentDate = calendar.GetDate(true);
                if (paymentDate != null)
                {

                    if (MessageBox.Show(this,paymentDate + "일자로 일괄수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dgvUnpending.Rows)
                        {
                            row.Cells["payment_date"].Value = paymentDate;
                        }
                        //수정
                        int results = customsRepository.UpdatePaymentDate(id.ToString(), paymentDate);
                        if (results == -1)
                        {
                            MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                        else
                        {
                            if (pendingList != null)
                            {
                                pendingList.GetCustomInfo();
                            }
                            else if (cal != null)
                            {
                                cal.displayDays(cal.year, cal.month);
                            }
                            MessageBox.Show(this,"수정완료");
                            this.Activate();
                        }
                    }
                }
            }
        }
        private void SetDgvUnpendigHeaderSetting()
        {
            DataGridView dgv = dgvUnpending;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            //Disable Sorting for DataGridView
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //정렬
            dgv.Columns["unit_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["box_weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["contract_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["warehouse_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["tariff_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["vat_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //Header 폰트크기
            dgv.Columns["contract_year"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            dgv.Columns["lc_open_date"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.Columns["shipment_date"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.Columns["warehouse_date"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.Columns["pending_date"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.Columns["box_weight"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.Columns["contract_qty"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.Columns["warehouse_qty"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.Columns["tariff_rate"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.Columns["vat_rate"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);
            //색상
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["bl_no"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["bl_no"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["weight_calculate"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 255);
            dgv.Columns["weight_calculate"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["box_weight"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 255);
            dgv.Columns["box_weight"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["cost_unit"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 255);
            dgv.Columns["cost_unit"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["unit_price"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["unit_price"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["contract_qty"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["contract_qty"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["warehouse_qty"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["warehouse_qty"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["tariff_rate"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["tariff_rate"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["vat_rate"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["vat_rate"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["payment_date"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["payment_date"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["payment_date_status"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["payment_date_status"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["payment_bank"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["payment_bank"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["division"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["division"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["usance_type"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["usance_type"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["agency_type"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["agency_type"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["manager"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["manager"].HeaderCell.Style.ForeColor = Color.White;

            //결제내역
            dgv = dgvPayment;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            dgv.Columns["pay_id"].Visible = false;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["pay_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        }
        //서류폴더 OPEN
        private void OpenPaperFloder(string contractYear, string atono)
        {
            System.IO.DirectoryInfo di;
            string filePath = @"Z:\아토무역\무역\무역1\ㄴ.수입자료\서류\"; // 선택할 파일 
            string subPath = "";
            string folderName = "";
            bool contain;

            //첫번째 폴더(연도)====================================================================
            if (System.IO.Directory.Exists(filePath))
            {
                di = new System.IO.DirectoryInfo(filePath);
                foreach (var item in di.GetDirectories())
                {
                    contain = item.Name.Contains(contractYear);
                    if (contain)
                    {
                        subPath = item.Name;
                        goto step1;
                    }
                }
                System.Diagnostics.Process.Start(filePath);
                MessageBox.Show(this,"경로를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                MessageBox.Show(this,"경로를 찾을 수 없습니다. NAS 연결을 다시 확인해주세요. \n" + filePath);
                this.Activate();
                return;
            }
        step1:
            //두번째 폴더(담당자별)====================================================================
            if (!string.IsNullOrEmpty(subPath))
            {
                if (atono.Substring(0, 2) != "AD")
                {
                    folderName = atono.Substring(0, 1);
                }
                else
                {
                    folderName = atono.Substring(0, 2);
                }

                di = new System.IO.DirectoryInfo(filePath + subPath);
                foreach (var item in di.GetDirectories())
                {
                    if (folderName.Length == 1)
                    {
                        if (item.Name.Substring(0, 1) == folderName)
                        {
                            subPath += @"\" + item.Name;
                            goto step2;
                        }
                    }
                    else
                    {
                        if (item.Name.Substring(0, 2) == folderName)
                        {
                            subPath += @"\" + item.Name;
                            goto step2;
                        }
                    }
                }
                System.Diagnostics.Process.Start(filePath + subPath);
                MessageBox.Show(this,"경로를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                System.Diagnostics.Process.Start(filePath + subPath);
                MessageBox.Show(this,"경로를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
        step2:
            //세번째 폴더(품목별)====================================================================
            if (System.IO.Directory.Exists(filePath + subPath))
            {
                folderName = atono.Substring(0, 2);

                di = new System.IO.DirectoryInfo(filePath + subPath);
                foreach (var item in di.GetDirectories())
                {
                    if (item.Name.Substring(0, 2) == folderName)
                    {
                        subPath += @"\" + item.Name;
                        goto step3;
                    }
                }
                System.Diagnostics.Process.Start(filePath + subPath);
                return;
            }
            else
            {
                System.Diagnostics.Process.Start(filePath + subPath);
                return;
            }
        step3:
            //네번째 폴더(atono)====================================================================
            if (System.IO.Directory.Exists(filePath + subPath))
            {
                folderName = atono;

                di = new System.IO.DirectoryInfo(filePath + subPath);
                foreach (var item in di.GetDirectories())
                {
                    contain = item.Name.Contains(folderName);
                    if (contain)
                    {
                        subPath += @"\" + item.Name;
                        System.Diagnostics.Process.Start(filePath + subPath);
                        return;
                    }
                }
                System.Diagnostics.Process.Start(filePath + subPath);
                return;
            }
            else
            {
                System.Diagnostics.Process.Start(filePath + subPath);
                MessageBox.Show(this,"경로를 찾을 수 없습니다.");
                this.Activate();
                return;
            }
        }
        private void UpdateExecute()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            dgvUnpending.EndEdit();
            dgvPayment.EndEdit();
            //Data validation
            if (!Validation())
                return;
            //Stored data validation 
            List<AllCustomsModel> editList = customsRepository.GetUnPending(id.ToString());
            if (editList.Count == 0)
            {
                MessageBox.Show(this,"등록되지 않은 내역입니다.");
                this.Activate();
                return;
            }
            //확정상태 통일여부 체크, 확인메세지
            string cc_status = dgvUnpending.Rows[0].Cells["cc_status"].Value.ToString();
            bool isSame = true;
            for (int i = 0; i < dgvUnpending.Rows.Count; i++)
            {
                if (cc_status != dgvUnpending.Rows[i].Cells["cc_status"].Value.ToString())
                {
                    isSame = false;
                    break;
                }
            }
            //Update==============================================================================================
            //Messagebox
            if (!isSame)
            {
                if (MessageBox.Show(this,"통관상태가 통일되지 않았습니다. 계속 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }
            else
            {
                if (MessageBox.Show(this,"수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            //Sql Execute
            if (dgvUnpending.Rows.Count > 0)
            {
                //AtoNO 변경시 폴더명 변경
                if (dgvUnpending.Rows[0].Cells["ato_no"].Value.ToString() != editList[0].ato_no.ToString()
                    || dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString() != editList[0].contract_year.ToString()
                    || dgvUnpending.Rows[0].Cells["shipper"].Value.ToString() != editList[0].shipper.ToString())
                {
                    if (MessageBox.Show(this,"Contract Year, ATO No., Shipper 중 하나가 변경되어 기존 폴더를 새로운 ATO No. 폴더로 옮기겠습니다. 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    bool is_folder_update = false;
                    string errMsg = "";
                    //1.먼저 기존 폴더명 변경
                    string[] company = origin_model[0].shipper.Trim().Split(' ');
                    string ato_no = origin_model[0].ato_no.ToString();
                    string product = origin_model[0].product.ToString();
                    string origin_folder_path;

                    int d;
                    //3글자 아토번호
                    if (!int.TryParse(ato_no.Substring(2, 1), out d))
                    {
                        origin_folder_path = editList[0].contract_year.ToString() + "년/"
                            + ato_no.Substring(0, 1) + "/"
                            + ato_no.Substring(0, 3) + "/";
                    }
                    //2글자 아토번호
                    else
                    {
                        origin_folder_path = editList[0].contract_year.ToString() + "년/"
                            + ato_no.Substring(0, 1) + "/"
                            + ato_no.Substring(0, 2) + "/";
                    }
                    //이전 정보로 폴더찾기
                    origin_folder_path += ato_no + " " + ftp.ReplaceName(product) + "(" + company[0] + ")";
                    DirectoryInfo di = new DirectoryInfo(@"I:\" + origin_folder_path);
                    //없을 경우 아토번호로 유사한 폴더 찾기
                    if (!di.Exists)
                        origin_folder_path = ftp.StartTradePaperFolderPath(origin_model[0].contract_year.ToString(), ato_no, out errMsg).Replace(@"\", "/").Replace(@"I:/", "");
                    //원래 기존폴더 경로가 null이 아닌 경우만
                    if (origin_folder_path != null && !string.IsNullOrEmpty(origin_folder_path))
                    {
                        //수정된 아토번호 폴더구조 생성
                        company = dgvUnpending.Rows[0].Cells["shipper"].Value.ToString().Trim().Split(' ');
                        ato_no = dgvUnpending.Rows[0].Cells["ato_no"].Value.ToString();
                        product = dgvUnpending.Rows[0].Cells["product"].Value.ToString();
                        string update_folder_path;
                        //3글자 아토번호
                        if (!int.TryParse(ato_no.Substring(2, 1), out d))
                        {
                            update_folder_path = dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString() + "년/"
                                + ato_no.Substring(0, 1) + "/"
                                + ato_no.Substring(0, 3) + "/";
                        }
                        //2글자 아토번호
                        else
                        {
                            update_folder_path = dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString() + "년/"
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
                }


                //데이터 수정
                dgvUnpending.ClearSelection();
                List<int> unKnown_idx = new List<int>();
                //데이터 유효성검사
                DataTable seaoverDt = seaoverRepository.GetProductTable2("", false, "", "", "");
                if (seaoverDt.Rows.Count == 0)
                {
                    MessageBox.Show(this,"품목정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }
                //등록
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql;
                int sub_id = 1;
                sql = customsRepository.DeleteSql(id.ToString());
                sqlList.Add(sql);
                //등록
                for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                {
                    foreach (DataGridViewCell cell in dgvUnpending.Rows[i].Cells)
                    {
                        if (cell.Value == null)
                            cell.Value = "";
                    }

                    //등록품목 체크
                    DataRow[] arrRows = null;
                    string whrs = $"품명 = '{dgvUnpending.Rows[i].Cells["product"].Value.ToString()}' " +
                                                 $"AND 원산지 = '{dgvUnpending.Rows[i].Cells["origin"].Value.ToString()}'" +
                                                 $"AND 규격 = '{dgvUnpending.Rows[i].Cells["sizes"].Value.ToString()}'" +
                                                 $"AND SEAOVER단위 = '{dgvUnpending.Rows[i].Cells["box_weight"].Value.ToString()}'";
                    arrRows = seaoverDt.Select(whrs);
                    if (arrRows.Length == 0 && dgvUnpending.Rows[i].Cells["product"].Value.ToString() != "기타비용")
                    {
                        unKnown_idx.Add(i);
                        dgvUnpending.Rows[i].Selected = true;
                    }

                    AllCustomsModel model = new AllCustomsModel();
                    model.id = id;
                    model.sub_id = sub_id;
                    model.contract_year = Convert.ToInt16(dgvUnpending.Rows[i].Cells["contract_year"].Value.ToString());
                    model.ato_no = dgvUnpending.Rows[i].Cells["ato_no"].Value.ToString();
                    model.pi_date = dgvUnpending.Rows[i].Cells["pi_date"].Value.ToString();
                    model.contract_no = dgvUnpending.Rows[i].Cells["contract_no"].Value.ToString();
                    model.shipper = dgvUnpending.Rows[i].Cells["shipper"].Value.ToString();
                    model.lc_open_date = dgvUnpending.Rows[i].Cells["lc_open_date"].Value.ToString();
                    model.lc_payment_date = dgvUnpending.Rows[i].Cells["lc_no"].Value.ToString();
                    model.bl_no = dgvUnpending.Rows[i].Cells["bl_no"].Value.ToString();
                    model.shipment_date = dgvUnpending.Rows[i].Cells["shipment_date"].Value.ToString();
                    model.etd = dgvUnpending.Rows[i].Cells["etd"].Value.ToString();
                    model.eta = dgvUnpending.Rows[i].Cells["eta"].Value.ToString();
                    model.warehousing_date = dgvUnpending.Rows[i].Cells["warehouse_date"].Value.ToString();
                    model.pending_check = dgvUnpending.Rows[i].Cells["pending_date"].Value.ToString();   //통관예정일
                    model.cc_status = dgvUnpending.Rows[i].Cells["cc_status"].Value.ToString();
                    model.broker = dgvUnpending.Rows[i].Cells["customs_officer"].Value.ToString();
                    model.warehouse = dgvUnpending.Rows[i].Cells["warehouse"].Value.ToString();
                    model.origin = dgvUnpending.Rows[i].Cells["origin"].Value.ToString();
                    model.product = dgvUnpending.Rows[i].Cells["product"].Value.ToString();
                    model.sizes = dgvUnpending.Rows[i].Cells["sizes"].Value.ToString();

                    bool isWeight;
                    if (dgvUnpending.Rows[i].Cells["weight_calculate"].Value == null || !bool.TryParse(dgvUnpending.Rows[i].Cells["weight_calculate"].Value.ToString(), out isWeight))
                        isWeight = true;
                    model.weight_calculate = isWeight;
                    model.box_weight = dgvUnpending.Rows[i].Cells["box_weight"].Value.ToString();
                    model.cost_unit = dgvUnpending.Rows[i].Cells["cost_unit"].Value.ToString();
                    model.unit_price = Convert.ToDouble(dgvUnpending.Rows[i].Cells["unit_price"].Value.ToString());
                    model.quantity_on_paper = Convert.ToDouble(dgvUnpending.Rows[i].Cells["contract_qty"].Value.ToString());
                    model.qty = Convert.ToDouble(dgvUnpending.Rows[i].Cells["warehouse_qty"].Value.ToString());
                    model.tariff_rate = Convert.ToDouble(dgvUnpending.Rows[i].Cells["tariff_rate"].Value.ToString()) / 100;
                    model.vat_rate = Convert.ToDouble(dgvUnpending.Rows[i].Cells["vat_rate"].Value.ToString()) / 100;

                    model.payment_date = dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString();
                    model.payment_date_status = dgvUnpending.Rows[i].Cells["payment_date_status"].Value.ToString();
                    model.payment_bank = dgvUnpending.Rows[i].Cells["payment_bank"].Value.ToString();
                    model.usance_type = dgvUnpending.Rows[i].Cells["usance_type"].Value.ToString();
                    model.agency_type = dgvUnpending.Rows[i].Cells["agency_type"].Value.ToString();
                    model.division = dgvUnpending.Rows[i].Cells["division"].Value.ToString();
                    model.sanitary_certificate = dgvUnpending.Rows[i].Cells["sanitary_certificate"].Value.ToString();
                    model.manager = dgvUnpending.Rows[i].Cells["manager"].Value.ToString();
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");


                    //원가계산에서 입력되는 값
                    model.remark = dgvUnpending.Rows[i].Cells["custom_remark"].Value.ToString();
                    double loading_cost_per_box;
                    if (dgvUnpending.Rows[i].Cells["loading_cost_per_box"].Value == null || !double.TryParse(dgvUnpending.Rows[i].Cells["loading_cost_per_box"].Value.ToString(), out loading_cost_per_box))
                        loading_cost_per_box = 0;
                    model.loading_cost_per_box = loading_cost_per_box;
                    double refrigeration_charge;
                    if (dgvUnpending.Rows[i].Cells["refrigeration_charge"].Value == null || !double.TryParse(dgvUnpending.Rows[i].Cells["refrigeration_charge"].Value.ToString(), out refrigeration_charge))
                        refrigeration_charge = 0;
                    model.refrigeration_charge = refrigeration_charge;
                    model.total_amount_seaover = dgvUnpending.Rows[i].Cells["total_amount_seaover"].Value.ToString();
                    double trq_amount;
                    if (dgvUnpending.Rows[i].Cells["trq_amount"].Value == null || !double.TryParse(dgvUnpending.Rows[i].Cells["trq_amount"].Value.ToString(), out trq_amount))
                        trq_amount = 0;
                    model.trq_amount = trq_amount;

                    double shipping_trq_amount;
                    if (dgvUnpending.Rows[i].Cells["shipping_trq_amount"].Value == null || !double.TryParse(dgvUnpending.Rows[i].Cells["shipping_trq_amount"].Value.ToString(), out shipping_trq_amount))
                        shipping_trq_amount = 0;
                    model.shipping_trq_amount = shipping_trq_amount;

                    double clearance_rate;
                    if (dgvUnpending.Rows[i].Cells["clearance_rate"].Value == null || !double.TryParse(dgvUnpending.Rows[i].Cells["clearance_rate"].Value.ToString(), out clearance_rate))
                        clearance_rate = 0;
                    model.clearance_rate = clearance_rate;

                    double box_price_adjust;
                    if (dgvUnpending.Rows[i].Cells["box_price_adjust"].Value == null || !double.TryParse(dgvUnpending.Rows[i].Cells["box_price_adjust"].Value.ToString(), out box_price_adjust))
                        box_price_adjust = 0;
                    model.box_price_adjust = box_price_adjust;

                    double shipping_box_price_adjust;
                    if (dgvUnpending.Rows[i].Cells["shipping_box_price_adjust"].Value == null || !double.TryParse(dgvUnpending.Rows[i].Cells["shipping_box_price_adjust"].Value.ToString(), out shipping_box_price_adjust))
                        shipping_box_price_adjust = 0;
                    model.shipping_box_price_adjust = shipping_box_price_adjust;


                    model.is_calendar = cbCalendar.Checked;
                    model.is_shipment_qty = cbShipmentQty.Checked;
                    model.import_number = dgvUnpending.Rows[i].Cells["import_number"].Value.ToString();

                    bool is_quarantine;
                    if (dgvUnpending.Rows[i].Cells["is_quarantine"].Value == null || !bool.TryParse(dgvUnpending.Rows[i].Cells["is_quarantine"].Value.ToString(), out is_quarantine))
                        is_quarantine = false;
                    model.is_quarantine = is_quarantine;

                    sql = customsRepository.InsertSql2(model);
                    sqlList.Add(sql);
                    sub_id += 1;
                }
                //등록되지 않은 품목
                if (unKnown_idx.Count > 0)
                {
                    if (MessageBox.Show(this,"등록된 품목이 아닙니다. 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;
                }

                //t_shipping (remark 전용)
                ShippingModel sModel = shippingRepository.GetShipping(id.ToString());
                //있을때
                if (sModel.custom_id.ToString() == id.ToString())
                {
                    sModel.remark = txtRemark.Text;
                    sql = shippingRepository.UpdateShipping(sModel);
                }
                //없을때
                else
                {
                    sModel = new ShippingModel();
                    sModel.custom_id = id;
                    sModel.remark = txtRemark.Text;
                    sql = shippingRepository.InsertShipping(sModel);
                }
                sqlList.Add(sql);

                //Execute
                int result = customsRepository.UpdateCustomTran(sqlList);
                if (result == -1)
                {
                    MessageBox.Show(this,"수정 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    MessageBox.Show(this,"수정완료");
                    this.Activate();
                }
            }
            else
            {
                MessageBox.Show(this,"선택된 품목이 없습니다.");
                this.Activate();
            }

            //초기화
            origin_model = customsRepository.GetAllTypePending(type, id.ToString());
        }
        private void DeleteExecute()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            dgvUnpending.EndEdit();
            dgvPayment.EndEdit();
            //Data validation
            /*if (!Validation())
                return;*/
            //Stored data validation 
            List<AllCustomsModel> editList = customsRepository.GetUnPending(id.ToString());
            if (editList.Count == 0)
            {
                MessageBox.Show(this,"등록되지 않은 내역입니다.");
                this.Activate();
                return;
            }
            //확정상태 통일여부 체크, 확인메세지
            string cc_status = dgvUnpending.Rows[0].Cells["cc_status"].Value.ToString();
            bool isSame = true;
            for (int i = 0; i < dgvUnpending.Rows.Count; i++)
            {
                if (cc_status != dgvUnpending.Rows[i].Cells["cc_status"].Value.ToString())
                {
                    isSame = false;
                    break;
                }
            }
            //Update==============================================================================================
            //Messagebox
            if (MessageBox.Show(this,"삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            for (int i = 0; i < dgvUnpending.Rows.Count; i++)
            {
                if (dgvUnpending.Rows[i].Cells["ato_no"].Value.ToString().Contains("삭제") || dgvUnpending.Rows[i].Cells["ato_no"].Value.ToString().Contains("취소"))
                {
                    MessageBox.Show(this,"이미 삭제된 품목입니다.");
                    this.Activate();
                    return;
                }
            }
            for (int i = 0; i < dgvUnpending.Rows.Count; i++)
            {
                string ato_no = dgvUnpending.Rows[i].Cells["ato_no"].Value.ToString() + " (취소)";
                dgvUnpending.Rows[i].Cells["ato_no"].Value = ato_no;
            }
            dgvUnpending.EndEdit();

            //Sql Execute
            if (dgvUnpending.Rows.Count > 0)
            {
                //AtoNO 변경시 폴더명 변경
                if (dgvUnpending.Rows[0].Cells["ato_no"].Value.ToString() != editList[0].ato_no.ToString()
                    || dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString() != editList[0].contract_year.ToString()
                    || dgvUnpending.Rows[0].Cells["shipper"].Value.ToString() != editList[0].shipper.ToString())
                {
                    if (MessageBox.Show(this,"Contract Year, ATO No., Shipper 중 하나가 변경되어 기존 폴더를 새로운 ATO No. 폴더로 옮기겠습니다. 진행하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                        return;

                    bool is_folder_update = false;
                    string errMsg = "";
                    //1.먼저 기존 폴더명 변경
                    string[] company = origin_model[0].shipper.Trim().Split(' ');
                    string ato_no = origin_model[0].ato_no.ToString();
                    string product = origin_model[0].product.ToString();
                    string origin_folder_path;

                    int d;
                    //3글자 아토번호
                    if (!int.TryParse(ato_no.Substring(2, 1), out d))
                    {
                        origin_folder_path = editList[0].contract_year.ToString() + "년/"
                            + ato_no.Substring(0, 1) + "/"
                            + ato_no.Substring(0, 3) + "/";
                    }
                    //2글자 아토번호
                    else
                    {
                        origin_folder_path = editList[0].contract_year.ToString() + "년/"
                            + ato_no.Substring(0, 1) + "/"
                            + ato_no.Substring(0, 2) + "/";
                    }
                    //이전 정보로 폴더찾기
                    origin_folder_path += ato_no + " " + ftp.ReplaceName(product) + "(" + company[0] + ")";
                    DirectoryInfo di = new DirectoryInfo(@"I:\" + origin_folder_path);
                    //없을 경우 아토번호로 유사한 폴더 찾기
                    if (!di.Exists)
                        origin_folder_path = ftp.StartTradePaperFolderPath(origin_model[0].contract_year.ToString(), ato_no, out errMsg).Replace(@"\", "/").Replace(@"I:/", "");
                    //원래 기존폴더 경로가 null이 아닌 경우만
                    if (origin_folder_path != null && !string.IsNullOrEmpty(origin_folder_path))
                    {
                        //수정된 아토번호 폴더구조 생성
                        company = dgvUnpending.Rows[0].Cells["shipper"].Value.ToString().Trim().Split(' ');
                        ato_no = dgvUnpending.Rows[0].Cells["ato_no"].Value.ToString();
                        product = dgvUnpending.Rows[0].Cells["product"].Value.ToString();
                        string update_folder_path;
                        //3글자 아토번호
                        if (!int.TryParse(ato_no.Substring(2, 1), out d))
                        {
                            update_folder_path = dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString() + "년/"
                                + ato_no.Substring(0, 1) + "/"
                                + ato_no.Substring(0, 3) + "/";
                        }
                        //2글자 아토번호
                        else
                        {
                            update_folder_path = dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString() + "년/"
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
                }


                //데이터 수정
                dgvUnpending.ClearSelection();
                List<int> unKnown_idx = new List<int>();
                //데이터 유효성검사
                DataTable seaoverDt = seaoverRepository.GetProductTable2("", false, "", "", "");
                if (seaoverDt.Rows.Count == 0)
                {
                    MessageBox.Show(this,"품목정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }
                //삭제
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql = commonRepository.UpdateData("t_customs"
                    , $"ato_no = '{dgvUnpending.Rows[0].Cells["ato_no"].Value.ToString()}', edit_user = '{um.user_name}', updatetime = '{DateTime.Now.ToString("yyyy-MM-dd")}'"
                    , $"id = {id}");
                sqlList.Add(sql);              

                //Execute
                int result = customsRepository.UpdateCustomTran(sqlList);
                if (result == -1)
                    MessageBox.Show(this, "삭제 중 에러가 발생하였습니다.");
                else
                    MessageBox.Show(this,"삭제완료");
            }
            else
                MessageBox.Show(this,"선택된 품목이 없습니다.");
            this.Activate();
            //초기화
            origin_model = customsRepository.GetAllTypePending(type, id.ToString());
        }
        /*private void DeleteExecute()
        {
            List<AllCustomsModel> editList = customsRepository.GetUnPending(id.ToString());
            if (editList.Count == 0)
            {
                MessageBox.Show(this,"등록되지 않은 내역입니다.");
                return;
            }
            //Messagebox
            if (MessageBox.Show(this,"삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //서류폴더명 변경
            string[] company = editList[0].shipper.ToString().Trim().Split(' ');
            string ato_no = editList[0].ato_no.ToString();
            string folder_path;

            int d;
            //3글자 아토번호
            if (!int.TryParse(ato_no.Substring(2, 1), out d))
            {
                folder_path = editList[0].contract_year.ToString() + "년/"
                    + ato_no.Substring(0, 1) + "/"
                    + ato_no.Substring(0, 3) + "/"
                    + editList[0].ato_no.ToString() + "(삭제)"
                    + " " + ftp.ReplaceName(editList[0].product.ToString().ToString())
                    + "(" + company[0] + ")";
            }
            //2글자 아토번호
            else
            {
                folder_path = editList[0].contract_year.ToString() + "년/"
                    + ato_no.Substring(0, 1) + "/"
                    + ato_no.Substring(0, 2) + "/"
                    + "(삭제)" + editList[0].ato_no.ToString()
                    + " " + ftp.ReplaceName(editList[0].product.ToString().ToString())
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
                    if (!ftp.RenameDirectory(editList[0].contract_year.ToString(), editList[0].ato_no, folder_path, false))
                        MessageBox.Show(this,"폴더명 수정중 에러가 발생하였습니다.");
                }
                else
                {
                    cnt++;
                    folder_path = editList[0].contract_year.ToString() + "년/"
                    + "(삭제)" + editList[0].ato_no.ToString()
                    + " " + ftp.ReplaceName(editList[0].product.ToString().ToString())
                    + "(" + company[0] + ") (" + cnt + ")";
                }
            }

            //ftp.RenameDirectory(origin_model[0].contract_year.ToString(), origin_model[0].ato_no, folder_path);

            //Sql Execute
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql;
            //t_customs Table data delete
            sql = customsRepository.DeleteSql(id.ToString());
            sqlList.Add(sql);
            //t_shipping Table data delete
            sql = shippingRepository.DeleteShipping(id);
            sqlList.Add(sql);
            //t_payment Table data delete
            PaymentModel pModel = new PaymentModel();
            pModel.id = id;
            sql = paymentRepository.DeleteSql(pModel);
            sqlList.Add(sql);

            int result = customsRepository.UpdateCustomTran(sqlList);
            if (result == -1)
                MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
            else
            {
                if (pendingList != null)               
                    pendingList.GetCustomInfo();
                else if (cal != null)
                    cal.displayDays(cal.year, cal.month);

                this.Dispose();
                MessageBox.Show(this,"삭제완료");
            }
        }*/

        private void TxtRefresh()
        {
            id = customsRepository.GetNextId();
            dgvUnpending.Rows.Clear();
        }

        private bool Validation(bool isInsert = true)
        {
            dgvUnpending.EndEdit();
            bool isVal = false;
            
            if (dgvUnpending.Rows.Count == 0)
            {
                MessageBox.Show(this,"선택된 품목이 없습니다.");
                this.Activate();
                return isVal;
            }
            else 
            {
                double d;
                for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvUnpending.Rows[i];

                    if (row.Cells["ato_no"].Value == null || string.IsNullOrEmpty(row.Cells["ato_no"].Value.ToString()))
                    {
                        dgvUnpending.ClearSelection();
                        row.Cells["ato_no"].Selected = true;
                        MessageBox.Show(this,"AtoNo를 입력해주세요.");
                        this.Activate();
                        return isVal;
                    }

                    if (row.Cells["contract_year"].Value == null || string.IsNullOrEmpty(row.Cells["contract_year"].Value.ToString()))
                    {
                        dgvUnpending.ClearSelection();
                        row.Cells["contract_year"].Selected = true;
                        MessageBox.Show(this,"계약연도를 입력해주세요.");
                        this.Activate();
                        return isVal;
                    }

                    if ((row.Cells["shipment_date"].Value == null || string.IsNullOrEmpty(row.Cells["shipment_date"].Value.ToString())) 
                        && (row.Cells["etd"].Value == null || string.IsNullOrEmpty(row.Cells["etd"].Value.ToString()))
                        && (row.Cells["eta"].Value == null || string.IsNullOrEmpty(row.Cells["eta"].Value.ToString()))
                        )
                    {
                        MessageBox.Show(this,"계약선적일, ETD, ETA 중 하나는 필수 입력값입니다.");
                        this.Activate();
                        return isVal;
                    }
                    /*else if ((row.Cells["etd"].Value == null || string.IsNullOrEmpty(row.Cells["etd"].Value.ToString()))
                        && (row.Cells["eta"].Value != null && !string.IsNullOrEmpty(row.Cells["eta"].Value.ToString()))
                        )
                    {
                        MessageBox.Show(this,"ETA입력시 ETD는 필수 입력값입니다.");
                        return isVal;
                    }*/

                    if (row.Cells["shipment_date"].Value != null && !string.IsNullOrEmpty(row.Cells["shipment_date"].Value.ToString()))
                    {
                        DateTime dt;
                        if (!DateTime.TryParse(row.Cells["shipment_date"].Value.ToString(), out dt))
                        {
                            row.Cells["shipment_date"].Selected = true;
                            MessageBox.Show(this,"계약선적일 값을 다시 확인해주시기 바랍니다.");
                            this.Activate();
                            return isVal;
                        }
                    }

                    if (row.Cells["etd"].Value != null && !string.IsNullOrEmpty(row.Cells["etd"].Value.ToString()))
                    {
                        DateTime dt;
                        if (!DateTime.TryParse(row.Cells["etd"].Value.ToString(), out dt))
                        {
                            row.Cells["etd"].Selected = true;
                            MessageBox.Show(this,"ETD 값을 다시 확인해주시기 바랍니다.");
                            this.Activate();
                            return isVal;
                        }
                    }

                    if (row.Cells["eta"].Value != null && !string.IsNullOrEmpty(row.Cells["eta"].Value.ToString()))
                    {
                        DateTime dt;
                        if (!DateTime.TryParse(row.Cells["eta"].Value.ToString(), out dt))
                        {
                            row.Cells["eta"].Selected = true;
                            MessageBox.Show(this,"ETA 값을 다시 확인해주시기 바랍니다.");
                            this.Activate();
                            return isVal;
                        }
                    }

                    if (row.Cells["payment_date"].Value != null && !string.IsNullOrEmpty(row.Cells["payment_date"].Value.ToString()))
                    {
                        DateTime dt;
                        if (!DateTime.TryParse(row.Cells["payment_date"].Value.ToString(), out dt))
                        {
                            row.Cells["payment_date"].Selected = true;
                            MessageBox.Show(this,"결제일 값을 다시 확인해주시기 바랍니다.");
                            this.Activate();
                            return isVal;
                        }
                    }


                    if (!double.TryParse(row.Cells["unit_price"].Value.ToString().Replace(",",""), out d))
                    {
                        MessageBox.Show(this,"가격은 숫자형식으로만 입력해주세요.");
                        this.Activate();
                        return isVal;
                    }
                    if (!double.TryParse(row.Cells["contract_qty"].Value.ToString().Replace(",", ""), out d))
                    {
                        MessageBox.Show(this,"계약수량은 숫자형식으로만 입력해주세요.");
                        this.Activate();
                        return isVal;
                    }
                    if (!double.TryParse(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""), out d))
                    {
                        MessageBox.Show(this,"실제창고 입고수량은 숫자형식으로만 입력해주세요.");
                        this.Activate();
                        return isVal;
                    }
                    if (!double.TryParse(row.Cells["tariff_rate"].Value.ToString().Replace(",", ""), out d))
                    {
                        MessageBox.Show(this,"관세율은 숫자형식으로만 입력해주세요.");
                        this.Activate();
                        return isVal;
                    }
                    else 
                    {
                        if (row.Cells["tariff_rate"].Value.ToString() == "")
                            row.Cells["tariff_rate"].Value = 0;
                    }

                    if (!double.TryParse(row.Cells["vat_rate"].Value.ToString().Replace(",", ""), out d))
                    {
                        MessageBox.Show(this,"부가세율은 숫자형식으로만 입력해주세요.");
                        this.Activate();
                        return isVal;
                    }
                    else
                    {
                        if (row.Cells["vat_rate"].Value.ToString() == "")
                            row.Cells["vat_rate"].Value = 0;
                    }
                    //신용장 NU -> 유산스 
                    if (row.Cells["lc_no"].Value != null && row.Cells["lc_no"].Value.ToString().Contains("NU") 
                        && (row.Cells["usance_type"].Value == null || row.Cells["usance_type"].Value.ToString() != "US"))
                    {
                        MessageBox.Show(this,"유산스 품목은 수입구분을 'US'로 설정해주시기 바랍니다.");
                        this.Activate();
                        return isVal;
                    }

                    double cost_unit;
                    if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;

                    bool weight_calculate;
                    if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                        weight_calculate = true;

                    if (!weight_calculate && cost_unit == 0)
                    {
                        dgvUnpending.ClearSelection();
                        row.Selected = true;
                        MessageBox.Show(this,"트레이 계산법에 트레이값이 입력되지 않았습니다. 중량계산법으로 변경하거나 트레이를 입력해주시기 바랍니다.");
                        this.Activate();
                        return isVal;
                    }
                }


                //AtoNo 중복
                string contract_year = dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString().Trim();
                string ato_no = dgvUnpending.Rows[0].Cells["ato_no"].Value.ToString().Trim();
                DataTable atoNoDt = customsRepository.GetDuplicateAtono(contract_year, ato_no, false, id.ToString());
                if (atoNoDt.Rows.Count > 0)
                {
                    for (int i = 0; i < atoNoDt.Rows.Count; i++)
                    {
                        //완전 같은내역 존재
                        if (atoNoDt.Rows[i]["ato_no"].ToString().Equals(ato_no))
                        {
                            MessageBox.Show(this,"이미 중복된 Ato No가 존재합니다!");
                            this.Activate();
                            return false;
                        }
                    }
                }
            }

            isVal = true;
            return isVal;
        }

        private void TotalCalculate()
        {
            try
            {
                dgvUnpending.EndEdit();
                if (dgvUnpending.Rows.Count > 0)
                {
                    double bWeight = 0;
                    double tPrice = 0;
                    double tQty = 0;
                    double tAmount = 0;
                    foreach (DataGridViewRow row in dgvUnpending.Rows)
                    {
                        double box_weight;
                        if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                        {
                            box_weight = 0;
                            row.Cells["box_weight"].Value = 0;
                        }    
                        double unit_price;
                        if (row.Cells["unit_price"].Value == null || !double.TryParse(row.Cells["unit_price"].Value.ToString(), out unit_price))
                        {
                            unit_price = 0;
                            row.Cells["unit_price"].Value = 0;
                        }
                        double contract_qty;
                        if (row.Cells["contract_qty"].Value == null || !double.TryParse(row.Cells["contract_qty"].Value.ToString(), out contract_qty))
                        {
                            contract_qty = 0;
                            row.Cells["contract_qty"].Value = 0;
                        }
                        double cost_unit;
                        if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                        {
                            row.Cells["cost_unit"].Value = 0;
                            cost_unit = 0;
                        }
                        bool isWeight;
                        if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out isWeight))
                        {
                            isWeight = true;
                            row.Cells["weight_calculate"].Value = true;
                        }

                        double trq_amount;
                        if (row.Cells["trq_amount"].Value == null || !double.TryParse(row.Cells["trq_amount"].Value.ToString(), out trq_amount))
                        {
                            row.Cells["trq_amount"].Value = 0;
                            trq_amount = 0;
                        }
                        //총합
                        if (row.Cells["product"].Value != null && row.Cells["product"].Value.ToString().Equals("기타비용"))
                        {
                            tPrice += unit_price;
                            tAmount += unit_price;
                        }
                        else
                        {
                            tPrice += unit_price;
                            tQty += contract_qty;

                            //트레이 계산
                            if (!isWeight && cost_unit > 0)
                            {
                                bWeight += box_weight * contract_qty;
                                tAmount += cost_unit * unit_price * contract_qty;
                            }
                            //중량 계산
                            else
                            {
                                bWeight += box_weight * contract_qty;
                                tAmount += box_weight * unit_price * contract_qty;
                            }
                        }
                    }
                    txtTotalBoxWeight.Text = bWeight.ToString("#,##0.00");
                    txtTotalPrice.Text = tAmount.ToString("#,##0.00");
                }
            }


            catch
            { }
        }

        #endregion

        #region Key, CellMouseClick Event
        private void txtPaymentAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Back))
            {
            }
            else if (e.KeyChar == Convert.ToChar(Keys.Delete))
            {
            }
            else if (e.KeyChar == Convert.ToChar(46))
            {
            }
            else if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtPaymentAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    txtPaymentAmount.Text = GetN2(txtPaymentAmount.Text);
                    txtPaymentAmount.SelectionStart = txtPaymentAmount.TextLength; //** 캐럿을 맨 뒤로 보낸다...
                    txtPaymentAmount.SelectionLength = 0;
                }
                catch (Exception ex)
                { }
            }
        }
        private void dgvUnpending_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvUnpending.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    if (um.auth_level > 2)
                    {
                        m.Items.Add("삭제");
                        m.Items.Add("품목변경");
                        m.Items.Add("품목추가");
                    }

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvUnpending, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch {}
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                DataGridViewRow dr = dgvUnpending.SelectedRows[0];
                int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                int eRowindedx = 0;
                if (dgvUnpending.SelectedRows.Count > 0)
                {
                    eRowindedx = dgvUnpending.SelectedRows[0].Index;
                }
                else if (dgvUnpending.SelectedCells.Count > 0)
                {
                    eRowindedx = dgvUnpending.SelectedCells[0].RowIndex;
                }
                else
                {
                    MessageBox.Show(this,"행을 먼저 선택해주세요.");
                    this.Activate();
                    return;
                }

                /*PendingInfo p;*/

                switch (e.ClickedItem.Text)
                {
                    case "삭제":
                        dgvUnpending.Rows.Remove(dr);
                        break;
                    case "품목변경":

                        if (dr.Cells["product"].Value == null)
                            dr.Cells["product"].Value = "";

                        if (dr.Cells["origin"].Value == null)
                            dr.Cells["origin"].Value = "";

                        if (dr.Cells["sizes"].Value == null)
                            dr.Cells["sizes"].Value = "";

                        ProductManager pm = new ProductManager(um);
                        string[] selectProduct = pm.GetProduct(dr.Cells["product"].Value.ToString()
                                                                , dr.Cells["origin"].Value.ToString()
                                                                , dr.Cells["sizes"].Value.ToString()
                                                                , dr.Cells["box_weight"].Value.ToString());


                        // 부모 Form의 좌표, 크기를 계산
                        int mainformX = this.Location.X;
                        int mainformY = this.Location.Y;
                        int mainfromWidth = this.Size.Width;
                        int mainfromHeight = this.Size.Height;

                        // 자식 Form의 크기를 계산
                        int childformwidth = pm.Size.Width;
                        int childformheight = pm.Size.Height;
                        Point p = new Point(mainformX + (mainfromWidth / 2) - (childformwidth / 2), mainformY + (mainfromHeight / 2) - (childformheight / 2));

                        if (selectProduct != null)
                        {
                            dr.Cells["product"].Value = selectProduct[0];
                            dr.Cells["origin"].Value = selectProduct[1];
                            dr.Cells["sizes"].Value = selectProduct[2];
                            dr.Cells["box_weight"].Value = selectProduct[3];
                            dr.Cells["cost_unit"].Value = selectProduct[4];
                            bool isWeightCaluclate = Convert.ToBoolean(selectProduct[8]);
                            dr.Cells["weight_calculate"].Value = isWeightCaluclate;
                        }
                        break;
                    case "품목추가":
                        {
                            int n = dgvUnpending.Rows.Add();
                            DataGridViewRow row = dgvUnpending.Rows[eRowindedx];
                            DataGridViewRow copyRow = dgvUnpending.Rows[n];
                            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                            copyRow.Cells["contract_year"].Value = row.Cells["contract_year"].Value;
                            copyRow.Cells["ato_no"].Value = row.Cells["ato_no"].Value;
                            copyRow.Cells["pi_date"].Value = row.Cells["pi_date"].Value;
                            copyRow.Cells["contract_no"].Value = row.Cells["contract_no"].Value;
                            copyRow.Cells["shipper"].Value = row.Cells["shipper"].Value;
                            copyRow.Cells["lc_open_date"].Value = row.Cells["lc_open_date"].Value;
                            copyRow.Cells["lc_no"].Value = row.Cells["lc_no"].Value;
                            copyRow.Cells["bl_no"].Value = row.Cells["bl_no"].Value;
                            copyRow.Cells["shipment_date"].Value = row.Cells["shipment_date"].Value;
                            copyRow.Cells["etd"].Value = row.Cells["etd"].Value;
                            copyRow.Cells["eta"].Value = row.Cells["eta"].Value;
                            copyRow.Cells["warehouse_date"].Value = row.Cells["warehouse_date"].Value;
                            copyRow.Cells["pending_date"].Value = row.Cells["pending_date"].Value;

                            cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("미통관");
                            cCell.Items.Add("통관");
                            /*cCell.Items.Add("확정");*/
                            copyRow.Cells["cc_status"] = cCell;
                            if (row.Cells["cc_status"].Value == "미통")
                            {
                                copyRow.Cells["cc_status"].Value = "미통관";
                            }
                            else
                            {
                                copyRow.Cells["cc_status"].Value = row.Cells["cc_status"].Value;
                            }


                            copyRow.Cells["customs_officer"].Value = row.Cells["customs_officer"].Value;
                            copyRow.Cells["warehouse"].Value = row.Cells["warehouse"].Value;
                            copyRow.Cells["origin"].Value = row.Cells["origin"].Value;
                            copyRow.Cells["product"].Value = row.Cells["product"].Value;
                            copyRow.Cells["sizes"].Value = row.Cells["sizes"].Value;
                            copyRow.Cells["box_weight"].Value = row.Cells["box_weight"].Value;
                            copyRow.Cells["unit_price"].Value = row.Cells["unit_price"].Value;
                            copyRow.Cells["contract_qty"].Value = row.Cells["contract_qty"].Value;
                            copyRow.Cells["warehouse_qty"].Value = row.Cells["warehouse_qty"].Value;
                            copyRow.Cells["tariff_rate"].Value = row.Cells["tariff_rate"].Value;
                            copyRow.Cells["vat_rate"].Value = row.Cells["vat_rate"].Value;
                            copyRow.Cells["is_quarantine"].Value = false;
                            if (!(row.Cells["payment_date"].Value == null || row.Cells["payment_date"].Value.ToString()== ""))
                            {
                                copyRow.Cells["payment_date"].Value = Convert.ToDateTime(row.Cells["payment_date"].Value).ToString("yyyy-MM-dd");
                            }

                            cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("");
                            cCell.Items.Add("미확정");
                            cCell.Items.Add("확정");
                            cCell.Items.Add("확정(LG)");
                            cCell.Items.Add("확정(마감)");
                            cCell.Items.Add("결제완료");
                            copyRow.Cells["payment_date_status"] = cCell;
                            copyRow.Cells["payment_date_status"].Value = row.Cells["payment_date_status"].Value;

                            cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("국민");
                            cCell.Items.Add("기업");
                            cCell.Items.Add("부산");
                            cCell.Items.Add("신한");
                            cCell.Items.Add("수협");
                            cCell.Items.Add("하나");
                            /*copyRow.Cells["payment_bank"] = cCell;*/
                            copyRow.Cells["payment_bank"].Value = row.Cells["payment_bank"].Value;

                            cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("O");
                            cCell.Items.Add("X");
                            /*copyRow.Cells["usance_type"] = cCell;*/
                            copyRow.Cells["usance_type"].Value = row.Cells["usance_type"].Value;

                            cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("O");
                            cCell.Items.Add("X");
                            /*copyRow.Cells["agency_type"] = cCell;*/
                            copyRow.Cells["agency_type"].Value = row.Cells["agency_type"].Value;

                            cCell = new DataGridViewComboBoxCell();
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            cCell.Items.Add("아토");
                            cCell.Items.Add("에이티오");
                            cCell.Items.Add("동일");
                            cCell.Items.Add("수리미");
                            cCell.Items.Add("해금");
                            cCell.Items.Add("에스제이");
                            cCell.Items.Add("푸드마을");
                            cCell.Items.Add("이안수산");
                            /*copyRow.Cells["division"] = cCell;*/
                            copyRow.Cells["division"].Value = row.Cells["division"].Value;
                            copyRow.Cells["manager"].Value = row.Cells["manager"].Value;

                            //복사본 만들기
                            List <DataGridViewRow> originRow = new List<DataGridViewRow>();
                            for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                            {
                                if (eRowindedx == i)
                                {
                                    originRow.Add(copyRow);
                                    originRow.Add(dgvUnpending.Rows[i]);
                                }
                                else
                                {
                                    originRow.Add(dgvUnpending.Rows[i]);
                                }
                            }
                            dgvUnpending.Rows.Clear();
                            //다시 붙혀넣기
                            for (int i = 0; i < originRow.Count - 1; i++)
                            {
                                dgvUnpending.Rows.Add(originRow[i]);
                            }

                            break;
                        }
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
        #endregion

        #region KeyPress Event
        //Datagridview 숫자만 입력
        private void dgvUnpending_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            /*string name = dgvUnpending.CurrentCell.OwningColumn.Name;

            if (name == "contract_year" || name == "box_weight" || name == "unit_price" || name == "contract_qty"
                || name == "warehouse_qty" || name == "tariff_rate" || name == "vat_rate" || name == "pi_date"
                || name == "lc_open_date" || name == "shipment_date" || name == "etd" || name == "eta" || name == "warehouse_date"
                || name == "pending_date" || name == "payment_date")
            {
                e.Control.KeyPress += new KeyPressEventHandler(txtCheckNumeric_KeyPress);
                //총 중량, 금액계산
                TotalCalculate();
            }
            else
            {
                e.Control.KeyPress -= new KeyPressEventHandler(txtCheckNumeric_KeyPress);
            }*/
        }
        private void nudFontsize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetFontSizes();
            }
        }
        private void txtCheckNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(46) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))
                e.Handled = true;*/
        }

        private void UnPendingManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                        {
                            UpdatePaymentDate();
                            break;
                        }
                    case Keys.A:
                        {
                            UpdateExecute();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
            else
            {   
                switch (e.KeyCode)
                {
                    case Keys.F5:
                        {
                            TxtRefresh();
                            break;
                        }

                    case Keys.Enter:
                        if (dgvPayment.SelectedCells.Count > 0)
                        {
                            DataGridViewCell cell = dgvPayment.SelectedCells[0];
                            if (cell.RowIndex >= 0 && cell.ColumnIndex >= 0)
                            {
                                dgvPayment.EndEdit();
                                if (cell.Value != null)
                                {
                                    string sdate = cell.Value.ToString();
                                    if (dgvUnpending.Columns[cell.ColumnIndex].Name == "pi_date"
                                        || dgvUnpending.Columns[cell.ColumnIndex].Name == "lc_open_date"
                                        || dgvUnpending.Columns[cell.ColumnIndex].Name == "shipment_date"
                                        || dgvUnpending.Columns[cell.ColumnIndex].Name == "warehouse_date"
                                        || dgvUnpending.Columns[cell.ColumnIndex].Name == "etd"
                                        || dgvUnpending.Columns[cell.ColumnIndex].Name == "eta"
                                        || dgvUnpending.Columns[cell.ColumnIndex].Name == "pending_date"
                                        || dgvUnpending.Columns[cell.ColumnIndex].Name == "payment_date")
                                    {
                                        if (cell.Value != null)
                                        {

                                            string txt = cell.Value.ToString();
                                            txt = common.strDatetime(txt);
                                            cell.Value = txt;

                                        }
                                    }
                                }
                                TotalCalculate();
                            }
                        }
                        break;
                }
            }

        }

        #endregion

        #region Button Event
        private void btnUpdatePay_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql;
            //유효성검사
            int pay_id;
            if (!int.TryParse(lbPayId.Text, out pay_id))
            {
                MessageBox.Show(this,"수정할 내역을 선택해주세요.");
                this.Activate();
                return;
            }
            if (string.IsNullOrEmpty(cbPaymentDateStatus.Text))
            {
                MessageBox.Show(this,"결제상태를 선택해주세요.");
                this.Activate();
                return;
            }
            DateTime pay_date;
            if (!DateTime.TryParse(txtPaymentDate.Text, out pay_date))
            {
                MessageBox.Show(this,"결제일자를 입력해주세요.");
                this.Activate();
                return;
            }
            double pay_amount;
            if (!double.TryParse(txtPaymentAmount.Text, out pay_amount))
            {
                MessageBox.Show(this,"결제금액을 입력해주세요.");
                this.Activate();
                return;
            }
            else if (pay_amount == 0)
            {
                MessageBox.Show(this,"결제금액을 입력해주세요.");
                this.Activate();
                return;
            }
            //결제잔액
            double total_amount = GetTotalAmount();
            double total_payment = GetTotalPayment();

            //등록
            PaymentModel model = new PaymentModel();
            model.id = pay_id;
            model.division = "pending";
            model.contract_id = id;
            model.payment_date_status = cbPaymentDateStatus.Text;
            model.payment_currency = cbPaymentCurrency.Text;
            model.payment_amount = pay_amount;
            model.payment_date = pay_date.ToString("yyyy-MM-dd");
            model.remark = txtPaymentRemark.Text;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
            model.edit_user = txtPaymentEditUser.Text;

            //Delete
            sql = paymentRepository.DeleteSql(model);
            sqlList.Add(sql);
            //Insert
            sql = paymentRepository.InsertSql(model);
            sqlList.Add(sql);
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                this.Activate();
                return;
            }
            else
            {
                //txtTotalBalance.Text = (total_amount - total_payment - pay_amount).ToString("#,##0.00");
                //txtPaymentAmount.Text = (total_amount - total_payment - pay_amount).ToString("#,##0.00");
                txtPaymentRemark.Text = String.Empty;
                lbPayId.Text = "Null";
                GetPay();
            }
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
        private void btnUpdatePaymentDate_Click(object sender, EventArgs e)
        {
            UpdatePaymentDate();
        }
        private void btnUnconfirm_Click(object sender, EventArgs e)
        {
            {
                //권한확인
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }
                if (dgvUnpending.Rows.Count > 0)
                {
                    //Messagebox
                    if (MessageBox.Show(this,"결제상태를 미확정처리 하시겠습니까??", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                
                        foreach (DataGridViewRow row in dgvUnpending.Rows)
                        {
                            row.Cells["payment_date_status"].Value = "미확정";
                        }
                        //수정
                        int results = customsRepository.UpdatePayment(id.ToString(), "미확정");
                        if (results == -1)
                            MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                        else
                        {
                            if (pendingList != null)
                                pendingList.GetCustomInfo();
                            else if (cal != null)
                                cal.displayDays(cal.year, cal.month);
                            MessageBox.Show(this,"수정완료");
                            this.Activate();
                        }
                    }
                }
            }
        }
        
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            {
                //권한확인
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }

                if (dgvUnpending.Rows.Count > 0)
                {
                    string pay_date = "";
                    for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                    {
                        if (!(dgvUnpending.Rows[i].Cells["payment_date"].Value == null || dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString() == ""))
                        {
                            pay_date = dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(pay_date))
                    {
                        MessageBox.Show(this,"결제일자가 입력되지 않았습니다.");
                        this.Activate();
                        return;
                    }
                    //Messagebox
                    if (MessageBox.Show(this,"결제상태를 확정처리 하시겠습니까??", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dgvUnpending.Rows)
                        {
                            row.Cells["payment_date_status"].Value = "확정";
                        }
                        //수정
                        int results = customsRepository.UpdatePayment(id.ToString(), "확정");
                        if (results == -1)
                        {
                            MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                        else
                        {
                            if (pendingList != null)
                            {
                                pendingList.GetCustomInfo();
                            }
                            else if (cal != null)
                            {
                                cal.displayDays(cal.year, cal.month);
                            }
                            MessageBox.Show(this,"수정완료");
                            this.Activate();
                        }
                    }
                }
            }
        }
        private void btnLGConfirmEnd_Click(object sender, EventArgs e)
        {
            {
                //권한확인
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }
                if (dgvUnpending.Rows.Count > 0)
                {
                    string pay_date = "";
                    for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                    {
                        if (!(dgvUnpending.Rows[i].Cells["payment_date"].Value == null || dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString() == ""))
                        {
                            pay_date = dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(pay_date))
                    {
                        MessageBox.Show(this,"결제일자가 입력되지 않았습니다.");
                        this.Activate();
                        return;
                    }

                    //Messagebox
                    if (MessageBox.Show(this,"결제상태를 확정(LG) 처리 하시겠습니까??", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dgvUnpending.Rows)
                        {
                            row.Cells["payment_date_status"].Value = "확정(LG)";
                        }
                        //수정
                        int results = customsRepository.UpdatePayment(id.ToString(), "확정(LG)");
                        if (results == -1)
                        {
                            MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                        else
                        {
                            if (pendingList != null)
                            {
                                pendingList.GetCustomInfo();
                            }
                            else if (cal != null)
                            {
                                cal.displayDays(cal.year, cal.month);
                            }
                            MessageBox.Show(this,"수정완료");
                            this.Activate();
                        }
                    }
                }
            }
        }
        private void btnConfirmEnd_Click(object sender, EventArgs e)
        {
            {
                //권한확인
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }
                if (dgvUnpending.Rows.Count > 0)
                {
                    string pay_date = "";
                    for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                    {
                        if (!(dgvUnpending.Rows[i].Cells["payment_date"].Value == null || dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString() == ""))
                        {
                            pay_date = dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString();
                        }
                    }

                    if (string.IsNullOrEmpty(pay_date))
                    {
                        MessageBox.Show(this,"결제일자가 입력되지 않았습니다.");
                        this.Activate();
                        return;
                    }

                    //Messagebox
                    if (MessageBox.Show(this,"결제상태를 확정(마감) 처리 하시겠습니까??", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (DataGridViewRow row in dgvUnpending.Rows)
                        {
                            row.Cells["payment_date_status"].Value = "확정(마감)";
                        }
                        //수정
                        int results = customsRepository.UpdatePayment(id.ToString(), "확정(마감)");
                        if (results == -1)
                        {
                            MessageBox.Show(this,"수정중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                        else
                        {
                            if (pendingList != null)
                            {
                                pendingList.GetCustomInfo();
                            }
                            else if (cal != null)
                            {
                                cal.displayDays(cal.year, cal.month);
                            }
                            MessageBox.Show(this,"수정완료");
                            this.Activate();
                        }
                    }
                }
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            {
                //권한확인
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }
                if (dgvUnpending.Rows.Count > 0)
                {
                    string pay_date = "";
                    for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                    {
                        if (!(dgvUnpending.Rows[i].Cells["payment_date"].Value == null || dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString() == ""))
                        {
                            pay_date = dgvUnpending.Rows[i].Cells["payment_date"].Value.ToString();
                        }
                    }
                    //결제금액이 없을 경우
                    if (string.IsNullOrEmpty(pay_date))
                    {
                        if (MessageBox.Show(this,"결제일자가 입력되지 않았습니다. 오늘 날짜로 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            pay_date = DateTime.Now.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            return;
                        }
                    }

                    //Messagebox
                    if (MessageBox.Show(this,"결제상태를 결제완료(" + pay_date + ") 처리 하시겠습니까??", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        StringBuilder sql;

                        /*double total_amount = GetTotalAmount();
                        double total_payment = GetTotalPayment();
                        //등록
                        PaymentModel model = new PaymentModel();
                        model.id = commonRepository.GetNextId("t_payment", "id");
                        model.division = "pending";
                        model.contract_id = id;
                        model.payment_date_status = "결제완료";
                        model.payment_currency = cbPaymentCurrency.Text;
                        model.payment_amount = total_amount - total_payment;
                        model.payment_date = pay_date;
                        model.remark = txtRemark.Text;
                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                        model.edit_user = um.user_name;

                        //Pay Insert
                        sql = paymentRepository.InsertSql(model);
                        sqlList.Add(sql);*/
                        //Customs Insert
                        sql = customsRepository.UpdatePaymentComplete(id.ToString(), pay_date);
                        sqlList.Add(sql);
                        //Update
                        for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                        {
                            dgvUnpending.Rows[i].Cells["payment_date"].Value = pay_date;
                            dgvUnpending.Rows[i].Cells["payment_date_status"].Value = "결제완료";
                        }
                        //Execute
                        if (commonRepository.UpdateTran(sqlList) == -1)
                        {
                            MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                            this.Activate();
                        }
                        else
                        {
                            txtTotalBalance.Text = "0";
                            txtPaymentAmount.Text = "0";
                            txtRemark.Text = String.Empty;
                            lbPayId.Text = "Null";
                            GetPay();

                            if (pendingList != null)
                            {
                                pendingList.GetCustomInfo();
                            }
                            else if (cal != null)
                            {
                                cal.displayDays(cal.year, cal.month);
                            }
                        }
                    }
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateExecute();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteExecute();
        }
        private void btnAllRefresh_Click(object sender, EventArgs e)
        {
            TxtRefresh(); 
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvUnpending.Rows.Clear();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        Libs.ftpCommon ftp = new Libs.ftpCommon();
        Libs.NetDrive netdrive = new Libs.NetDrive();
        private void btnFolder_Click(object sender, EventArgs e)
        {
            List<AllCustomsModel> editList = customsRepository.GetUnPending(id.ToString());
            if (editList.Count == 0)
            {
                MessageBox.Show(this,"등록되지 않은 내역입니다.");
                this.Activate();
                return;
            }
            else
            {
                string contract_year = editList[0].contract_year.ToString();
                string ato_no = editList[0].ato_no.ToString();
                string errMsg;
                if (!ftp.StartTradePaperFolder(contract_year, ato_no, out errMsg))
                {
                    if (MessageBox.Show(this,"서류폴더를 찾을 수 없습니다. 새로 생성하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //서류폴더 생성
                        string[] company = dgvUnpending.Rows[0].Cells["shipper"].Value.ToString().Trim().Split(' ');
                        string folder_path;

                        int d;
                        //3글자 아토번호
                        if (!int.TryParse(ato_no.Substring(2, 1), out d))
                            folder_path = contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 3) + "/" + ato_no + " " + ftp.ReplaceName(dgvUnpending.Rows[0].Cells["product"].Value.ToString()) + "(" + company[0] + ")";
                        //2글자 아토번호
                        else
                            folder_path = contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 2) + "/" + ato_no + " " + ftp.ReplaceName(dgvUnpending.Rows[0].Cells["product"].Value.ToString()) + "(" + company[0] + ")";

                        MakeTradeDocumentFolder(folder_path, "ATO/아토무역/무역/무역1/ㄴ.수입자료/서류");
                    }
                }
            }
        }

        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void btnPending_Click(object sender, EventArgs e)
        {
            {
                //권한확인
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }

                //Messagebox
                if (MessageBox.Show(this,"통관내역으로 변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;

                for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                {
                    dgvUnpending.Rows[i].Cells["cc_status"].Value = "통관";
                }

                int result = customsRepository.UpdateData(id.ToString(), "cc_status", "통관");
                if (result == -1)
                {
                    MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    if (pendingList != null)
                        pendingList.GetCustomInfo();
                    else if (cal != null)
                        cal.displayDays(cal.year, cal.month);
                    MessageBox.Show(this,"완료");
                    this.Activate();
                }
            }
        }

        private void btnUnPending_Click(object sender, EventArgs e)
        {
            {
                //권한확인
                DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                if (authorityDt != null && authorityDt.Rows.Count > 0)
                {
                    if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_update"))
                    {
                        messageBox.Show(this, "권한이 없습니다!");
                        return;
                    }
                }
                //Messagebox
                if (MessageBox.Show(this,"미통관내역으로 변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
                for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                {
                    dgvUnpending.Rows[i].Cells["cc_status"].Value = "미통관";
                }
                int result = customsRepository.UpdateData(id.ToString(), "cc_status", "미통관");
                if (result == -1)
                {
                    MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    if (pendingList != null)
                        pendingList.GetCustomInfo();
                    else if (cal != null)
                        cal.displayDays(cal.year, cal.month);
                    MessageBox.Show(this,"완료");
                    this.Activate();
                }
            }
        }
        #endregion

        #region Mouse Click, Calendar Open Event
        private void dgvUnpending_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Right)
            {
                dgvUnpending.ClearSelection();

                dgvUnpending.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvUnpending.Rows[e.RowIndex].Selected = true;
            }
            else
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    if (dgvUnpending.Columns[e.ColumnIndex].Name == "weight_calculate")
                    {
                        dgvUnpending.EndEdit();
                        bool isWeight = Convert.ToBoolean(dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        double cost_unit;
                        if (dgvUnpending.Rows[e.RowIndex].Cells["cost_unit"].Value == null || !double.TryParse(dgvUnpending.Rows[e.RowIndex].Cells["cost_unit"].Value.ToString(), out cost_unit))
                            cost_unit = 0;
                        if (isWeight && cost_unit == 0)
                        {
                            MessageBox.Show(this,"트레이 값이 0인 품목에서 트레이 계산법으로 변경하였습니다.");
                            this.Activate();
                        }
                        dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !isWeight;
                        TotalCalculate();
                    }
                    else if (dgvUnpending.Columns[e.ColumnIndex].Name == "is_trq")
                    {
                        dgvUnpending.EndEdit();
                        bool is_trq = Convert.ToBoolean(dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !is_trq;
                        if (!is_trq)
                        {
                            double trq_amount;
                            if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq_amount))
                                trq_amount = 0;

                            dgvUnpending.Rows[e.RowIndex].Cells["trq_amount"].Value = trq_amount.ToString("#,##0");
                        }
                        else
                            dgvUnpending.Rows[e.RowIndex].Cells["trq_amount"].Value = "";
                    }
                }
            }
        }
        private void dgvUnpending_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            /*if (dgvUnpending.Rows.Count > 0 && e.RowIndex >= 0 & e.ColumnIndex > 0)
            {
                DataGridViewCell cell = dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (dgvUnpending.Columns[e.ColumnIndex].Name == "pi_date"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "lc_open_date"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "shipment_date"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "etd"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "eta"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "warehouse_date"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "pending_date"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "payment_date")
                {
                    Common.Calendar cd = new Common.Calendar();
                    string sDate = cd.GetDate(true);
                    if (sDate != null)
                    {
                        DateTime tmpDate = Convert.ToDateTime(sDate);
                        cell.Value = tmpDate.ToString("yyyy-MM-dd");
                    }
                }
                else if (dgvUnpending.Columns[e.ColumnIndex].Name == "product"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "origin"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "sizes"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "box_weight"
                    || dgvUnpending.Columns[e.ColumnIndex].Name == "unit_cost")
                {
                    DataGridViewRow dr = dgvUnpending.Rows[e.RowIndex];
                    if (dr.Cells["product"].Value == null)
                        dr.Cells["product"].Value = "";

                    if (dr.Cells["origin"].Value == null)
                        dr.Cells["origin"].Value = "";

                    if (dr.Cells["sizes"].Value == null)
                        dr.Cells["sizes"].Value = "";

                    ProductManager pm = new ProductManager(um);
                    string[] selectProduct = pm.GetProduct(dr.Cells["product"].Value.ToString()
                                                            , dr.Cells["origin"].Value.ToString()
                                                            , dr.Cells["sizes"].Value.ToString()
                                                            , dr.Cells["box_weight"].Value.ToString());
                    

                    // 부모 Form의 좌표, 크기를 계산
                    int mainformX = this.Location.X;
                    int mainformY = this.Location.Y;
                    int mainfromWidth = this.Size.Width;
                    int mainfromHeight = this.Size.Height;

                    // 자식 Form의 크기를 계산
                    int childformwidth = pm.Size.Width;
                    int childformheight = pm.Size.Height;
                    Point p = new Point(mainformX + (mainfromWidth / 2) - (childformwidth / 2), mainformY + (mainfromHeight / 2) - (childformheight / 2));

                    if (selectProduct != null)
                    {
                        dr.Cells["product"].Value = selectProduct[0];
                        dr.Cells["origin"].Value = selectProduct[1];
                        dr.Cells["sizes"].Value = selectProduct[2];
                        dr.Cells["box_weight"].Value = selectProduct[3];
                        dr.Cells["cost_unit"].Value = selectProduct[4];
                    }
                }
                else if (dgvUnpending.Columns[e.ColumnIndex].Name == "shipper")
                {
                    DataGridViewRow dr = dgvUnpending.Rows[e.RowIndex];
                    Company.CompanyManager cm = new Company.CompanyManager(um, true);
                    string company = cm.GetCompany();
                    dr.Cells["shipper"].Value = company;
                }
            }*/
        }
        #endregion

        #region 복사 붙혀넣기
        //복사 
        //  1.DataGridView에서 선택한 영역 Dataobj 에 담기(GetClipboaredContent)
        //  2.Clipboard에 저장(Clipboard.SetDataObject(Dataobj))
        private void CopyToClipboard()
        {
            //Copy to clipboard

            if (dgvUnpending.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell c in dgvUnpending.SelectedCells)
                {
                    if (c.Value != null)
                    {
                        c.Value = c.Value.ToString().Replace("\n", @"\n");
                    }
                }
            }

            DataObject dataObj = dgvUnpending.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        //붙혀넣기
        //  1.Dictionay에 Clipboard Value를 저장 Dic(int, Dic( int, string ) )
        //  2.Dictionay 반복하면서 (끝행은 무시) 현재 선택된 Cell에 출력
        private void PasteClipboardValue()
        {
            //Show Error if no cell is selected
            if (dgvUnpending.SelectedCells.Count == 0)
            {
                MessageBox.Show(this,"Please select a cell", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataGridViewCell startCell = GetStartCell(dgvUnpending);
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
                        if (iColIndex <= dgvUnpending.Columns.Count - 1 && iRowIndex <= dgvUnpending.Rows.Count - 1)
                        {
                            foreach (DataGridViewCell c in dgvUnpending.SelectedCells)
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
                        if (iColIndex <= dgvUnpending.Columns.Count - 1 && iRowIndex <= dgvUnpending.Rows.Count - 1)
                        {
                            DataGridViewCell cell = dgvUnpending[iColIndex, iRowIndex];
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

        #region Cell value changed event

        private void dgvUnpending_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvUnpending.EndEdit();
                DataGridViewCell cell = dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value != null)
                {
                    string sdate = cell.Value.ToString();
                    if (dgvUnpending.Columns[e.ColumnIndex].Name == "pi_date"
                        || dgvUnpending.Columns[e.ColumnIndex].Name == "lc_open_date"
                        || dgvUnpending.Columns[e.ColumnIndex].Name == "shipment_date"
                        || dgvUnpending.Columns[e.ColumnIndex].Name == "warehouse_date"
                        || dgvUnpending.Columns[e.ColumnIndex].Name == "etd"
                        || dgvUnpending.Columns[e.ColumnIndex].Name == "eta"
                        || dgvUnpending.Columns[e.ColumnIndex].Name == "pending_date"
                        || dgvUnpending.Columns[e.ColumnIndex].Name == "payment_date")
                    {
                        if (cell.Value != null)
                        {

                            string txt = cell.Value.ToString();
                            if (!txt.Equals("T/T") && !txt.Equals("t/t"))
                            {
                                txt = common.strDatetime(txt);

                                DateTime dt;
                                if (DateTime.TryParse(txt, out dt))
                                    txt = dt.ToString("yyyy-MM-dd");

                                cell.Value = txt;
                                dgvUnpending.EndEdit();
                            }
                        }
                    }
                }
                TotalCalculate();
            }
        }
        #endregion

        #region Datagridview event
        private void dgvUnpending_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvUnpending.Rows.Count > 0)
            {
                if (dgvUnpending.Columns[e.ColumnIndex].Name == "weight_calculate")
                {
                    bool isChecked;
                    if (dgvUnpending.Rows[0].Cells[e.ColumnIndex].Value == null || !bool.TryParse(dgvUnpending.Rows[0].Cells[e.ColumnIndex].Value.ToString(), out isChecked))
                        isChecked = false;

                    for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                    {
                        dgvUnpending.Rows[i].Cells[e.ColumnIndex].Value = !isChecked;
                    }

                    dgvUnpending.EndEdit();
                }
            }
        }
        private void dgvUnpending_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // If the data source raises an exception when a cell value is
            // commited, display an error message.
            if (e.Exception != null &&
                e.Context == DataGridViewDataErrorContexts.Commit)
            {
                //MessageBox.Show(this,"CustomerID value must be unique.");
            }
        }


        #endregion


        #region 결제내역 관리
        private double GetTotalAmount()
        {
            DataGridView dgv = dgvUnpending;
            //총 결제금액
            double total_amount = 0;
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    //박스중량
                    if (dgv.Rows[i].Cells["cost_unit"].Value == null || dgv.Rows[i].Cells["cost_unit"].Value == "" || Convert.ToDouble(dgv.Rows[i].Cells["cost_unit"].Value.ToString()) <= 0)
                    {
                        total_amount += Convert.ToDouble(dgv.Rows[i].Cells["box_weight"].Value.ToString()) * Convert.ToDouble(dgv.Rows[i].Cells["unit_price"].Value.ToString()) * Convert.ToDouble(dgv.Rows[i].Cells["contract_qty"].Value.ToString());
                    }
                    //트레이
                    else
                    {
                        total_amount += Convert.ToDouble(dgv.Rows[i].Cells["cost_unit"].Value.ToString()) * Convert.ToDouble(dgv.Rows[i].Cells["unit_price"].Value.ToString()) * Convert.ToDouble(dgv.Rows[i].Cells["contract_qty"].Value.ToString());
                    }
                }
            }
            return total_amount;
        }
        private double GetTotalPayment()
        {
            DataGridView dgv = dgvUnpending;
            //총 결제한금액
            dgv = dgvPayment;
            double total_payment = 0;
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    double payment;
                    if (double.TryParse(dgv.Rows[i].Cells["pay_amount"].Value.ToString(), out payment))
                    {
                        total_payment += payment;
                    }
                }
            }

            return total_payment;
        }


        private void btnAddPay_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql;
            //유효성검사'
            if (string.IsNullOrEmpty(cbPaymentDateStatus.Text))
            {
                MessageBox.Show(this,"결제상태를 선택해주세요.");
                this.Activate();
                return;
            }
            DateTime pay_date;
            if (!DateTime.TryParse(txtPaymentDate.Text, out pay_date))
            {
                MessageBox.Show(this,"결제일자를 입력해주세요.");
                this.Activate();
                return;
            }
            double pay_amount;
            if (!double.TryParse(txtPaymentAmount.Text, out pay_amount))
            {
                MessageBox.Show(this,"결제금액을 입력해주세요.");
                this.Activate();
                return;
            }
            else if (pay_amount == 0)
            {
                MessageBox.Show(this,"결제금액을 입력해주세요.");
                this.Activate();
                return;
            }
            //결제잔액
            double total_amount = GetTotalAmount();
            double total_payment = GetTotalPayment();

            if ((total_amount - total_payment) == 0)
            {
                MessageBox.Show(this,"결제금액할 금액이 없습니다.");
                this.Activate();
                return;
            }
            else if((total_amount - total_payment) == pay_amount)
            {
                //결제완료처리
                /*if (MessageBox.Show(this,"결제잔액을 모두 결제하셨습니다. 결제처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    sql = customsRepository.UpdatePaymentComplete(id.ToString(), pay_date.ToString("yyyy-MM-dd"));
                    sqlList.Add(sql);

                    for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                    {
                        dgvUnpending.Rows[i].Cells["payment_date"].Value = pay_date.ToString("yyyy-MM-dd");
                        dgvUnpending.Rows[i].Cells["payment_date_status"].Value = "결제완료";
                    }
                }*/
            }
            //등록
            PaymentModel model = new PaymentModel();
            model.id = commonRepository.GetNextId("t_payment", "id");
            model.division = "pending";
            model.contract_id = id;
            model.payment_date_status = cbPaymentDateStatus.Text;
            model.payment_currency = cbPaymentCurrency.Text;
            model.payment_amount = pay_amount;
            model.payment_date = pay_date.ToString("yyyy-MM-dd");
            model.remark = txtPaymentRemark.Text;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
            model.edit_user = txtPaymentEditUser.Text;

            //Delete
            sql = paymentRepository.DeleteSql(model);
            sqlList.Add(sql);
            //Insert
            sql = paymentRepository.InsertSql(model);
            sqlList.Add(sql);
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                txtTotalBalance.Text = (total_amount - total_payment - pay_amount).ToString("#,##0.00");
                txtPaymentAmount.Text = (total_amount - total_payment - pay_amount).ToString("#,##0.00");
                txtPaymentRemark.Text = String.Empty;
                lbPayId.Text = "Null";
                GetPay();  
            }
        }

        private void btnDeletePay_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 수정", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            int pay_id;
            if (!int.TryParse(lbPayId.Text, out pay_id))
            {
                MessageBox.Show(this,"결제내역을 선택해주세요.");
                this.Activate();
                return;
            }
            if (MessageBox.Show(this,"선택하신 결제내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            PaymentModel model = new PaymentModel();
            model.id = pay_id;
            //Delete
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = paymentRepository.DeleteSql(model);
            sqlList.Add(sql);
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this,"삭제중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                GetPay();
                //결제잔액
                double total_amount = GetTotalAmount();
                double total_payment = GetTotalPayment();
                txtTotalBalance.Text = (total_amount - total_payment).ToString("#,##0.00");
                txtPaymentAmount.Text = (total_amount - total_payment).ToString("#,##0.00");
            }
        }

        private void dgvPayment_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                int pay_id;
                if (!int.TryParse(dgvPayment.Rows[e.RowIndex].Cells["pay_id"].Value.ToString(), out pay_id))
                {
                    MessageBox.Show(this,"결제내역 정보를 찾을 수 없습니다.");
                    this.Activate();
                    return;
                }
                DataTable payDt = paymentRepository.GetPayment(pay_id.ToString(), "", "", "");
                if (payDt.Rows.Count > 0)
                {
                    lbPayId.Text = payDt.Rows[0]["id"].ToString();
                    cbPaymentCurrency.Text = payDt.Rows[0]["payment_currency"].ToString();
                    cbPaymentDateStatus.Text = payDt.Rows[0]["payment_date_status"].ToString();
                    txtPaymentDate.Text = Convert.ToDateTime(payDt.Rows[0]["payment_date"].ToString()).ToString("yyyy-MM-dd");
                    txtPaymentAmount.Text = Convert.ToDouble(payDt.Rows[0]["payment_amount"].ToString()).ToString("#,##.00");
                    txtPaymentRemark.Text = payDt.Rows[0]["remark"].ToString();
                    txtPaymentEditUser.Text = payDt.Rows[0]["edit_user"].ToString();
                }
            }
        }

        private void btnPaymentDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtPaymentDate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void txtPaymentDate_Leave(object sender, EventArgs e)
        {
            DateTime dt;
            if (DateTime.TryParse(txtPaymentDate.Text, out dt))
            {
                txtPaymentDate.Text = dt.ToString("yyyy-MM-dd");
            }
        }
        #endregion

        #region 네트워크 연결

        private void GetAttachment()
        {
            dgvAttachment.Rows.Clear();
            string contract_year = dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString();
            string ato_no = dgvUnpending.Rows[0].Cells["ato_no"].Value.ToString();
            string errMsg;
            string[] files = ftp.GetTradePaperFIles(contract_year, ato_no, out errMsg);
            if (files != null)
            {
                foreach (string file in files)
                {
                    int n = dgvAttachment.Rows.Add();
                    DataGridViewRow row = dgvAttachment.Rows[n];

                    row.Cells["file_name"].Value = Path.GetFileName(file);
                    row.Cells["file_path"].Value = file;
                }

                btnFtpAccess.Text = "연결성공";
                btnFtpAccess.ForeColor = Color.Blue;
            }
            else
            {
                btnFtpAccess.Text = "연결끊김";
                btnFtpAccess.ForeColor = Color.Red;

                if (MessageBox.Show(this,"서류폴더를 찾을 수 없습니다. 새로 생성하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //서류폴더 생성
                    string[] company = dgvUnpending.Rows[0].Cells["shipper"].Value.ToString().Trim().Split(' ');
                    string folder_path;

                    int d;
                    //3글자 아토번호
                    if (!int.TryParse(ato_no.Substring(2, 1), out d))
                        folder_path = contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 3) + "/" + ato_no + " " + ftp.ReplaceName(dgvUnpending.Rows[0].Cells["product"].Value.ToString()) + "(" + company[0] + ")";
                    //2글자 아토번호
                    else
                        folder_path = contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 2) + "/" + ato_no + " " + ftp.ReplaceName(dgvUnpending.Rows[0].Cells["product"].Value.ToString()) + "(" + company[0] + ")";

                    MakeTradeDocumentFolder(folder_path, "ATO/아토무역/무역/무역1/ㄴ.수입자료/서류");

                    btnFtpAccess.Text = "연결성공";
                    btnFtpAccess.ForeColor = Color.Blue;
                }
            }
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

        private void dgvAttachment_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string path = dgvAttachment.Rows[e.RowIndex].Cells["file_path"].Value.ToString();
                //DirectoryInfo.Exists로 폴더 존재유무 확인, 없으면 네트워크 드라이브 연결
                if (File.Exists(path))
                    System.Diagnostics.Process.Start(path);
                else
                {
                    MessageBox.Show(this,"파일을 찾을 수 없습니다. 첨부파일 리스트를 최신화 하겠습니다.");
                    this.Activate();
                    GetAttachment();
                }
            }
        }

        private void dgvAttachment_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string contract_year = dgvUnpending.Rows[0].Cells["contract_year"].Value.ToString();
            string ato_no = dgvUnpending.Rows[0].Cells["ato_no"].Value.ToString();
            string errMsg;
            string product_folder_path = ftp.StartTradePaperFolderPath(contract_year, ato_no, out errMsg);
            if (product_folder_path != null && !string.IsNullOrEmpty(product_folder_path))
            {
                int d;
                //3글자 아토번호
                if (!int.TryParse(ato_no.Substring(2, 1), out d))
                    product_folder_path = "아토무역/무역/무역1/ㄴ.수입자료/서류/" + contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 3) + "/" + Path.GetFileName(product_folder_path);
                //2글자 아토번호
                else
                    product_folder_path = "아토무역/무역/무역1/ㄴ.수입자료/서류/" + contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 2) + "/" + Path.GetFileName(product_folder_path);


                foreach (string file in files)
                {
                    int n = dgvAttachment.Rows.Add();
                    DataGridViewRow row = dgvAttachment.Rows[n];

                    row.Cells["file_name"].Value = Path.GetFileName(file);
                    row.Cells["file_path"].Value = file;

                    //파일업로드
                    if (!ftp.UploadFiles(product_folder_path , file, "ATO"))
                        row.HeaderCell.Style.BackColor = Color.Red;
                    else
                    {
                        row.Cells["file_path"].Value = product_folder_path + "/" + Path.GetFileName(file);
                    }
                }
            }
        }

        private void dgvAttachment_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void dgvUnpending_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.dgvUnpending.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnpending_CellValueChanged);
                if (dgvUnpending.Columns[e.ColumnIndex].Name == "usance_type")
                {
                    string usance_type = "";
                    if (dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        usance_type = dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(); 
                    for (int i = 0; i < dgvUnpending.Rows.Count; i++)
                        dgvUnpending.Rows[i].Cells[e.ColumnIndex].Value = usance_type;
                    dgvUnpending.EndEdit();
                }
                this.dgvUnpending.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUnpending_CellValueChanged);
            }
        }

        private void dgvUnpending_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvUnpending.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dgvUnpending.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txtTrq_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (MessageBox.Show(this,"TRQ 기준금액을 변경하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    double trq_price;
                    if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq_price))
                        trq_price = 0;

                    if (commonRepository.UpdateTrq(trq_price) == -1)
                        MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                    else
                        MessageBox.Show(this,"완료");
                    this.Activate();
                }
            }
        }

        private void btnOtherExpenses_Click(object sender, EventArgs e)
        {
            double expense;
            if (!double.TryParse(txtOtherExpenses.Text, out expense))
            {
                MessageBox.Show(this, "비용 입력값을 다시 확인해주세요!");
                this.Activate();
                return;
            }

            if (dgvUnpending.Rows.Count > 0)
            {
                int n = dgvUnpending.Rows.Add();

                foreach (DataGridViewColumn col in dgvUnpending.Columns)
                    dgvUnpending.Rows[n].Cells[col.Index].Value = dgvUnpending.Rows[n - 1].Cells[col.Index].Value;

                dgvUnpending.Rows[n].Cells["product"].Value = "기타비용";
                dgvUnpending.Rows[n].Cells["origin"].Value = "";
                dgvUnpending.Rows[n].Cells["sizes"].Value = "";
                dgvUnpending.Rows[n].Cells["box_weight"].Value = "1";
                dgvUnpending.Rows[n].Cells["contract_qty"].Value = "1";
                dgvUnpending.Rows[n].Cells["warehouse_qty"].Value = "1";
                dgvUnpending.Rows[n].Cells["cost_unit"].Value = "1";
                dgvUnpending.Rows[n].Cells["unit_price"].Value = expense;

            }
            else
            {
                MessageBox.Show(this, "품목을 먼저 추가해주세요1");
                this.Activate();
                return;
            }
        }

        private void dgvUnpending_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvUnpending.Columns[e.ColumnIndex].Name == "product"
                    && dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null
                    && dgvUnpending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "기타비용")
                {
                    dgvUnpending.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Linen;
                    dgvUnpending.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Linen;

                    dgvUnpending.Rows[e.RowIndex].Cells["product"].Style.ForeColor = Color.Black;
                    dgvUnpending.Rows[e.RowIndex].Cells["unit_price"].Style.ForeColor = Color.Black;
                }
            }
        }

        private void btnFtpAccess_Click(object sender, EventArgs e)
        {
            GetAttachment();
        }

        private void dgvPayment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvPayment.Columns[e.ColumnIndex].Name == "pay_date_status")
                {
                    if (dgvPayment.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "결제완료")
                        dgvPayment.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.LightGray;
                    else
                        dgvPayment.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }
    }
}
