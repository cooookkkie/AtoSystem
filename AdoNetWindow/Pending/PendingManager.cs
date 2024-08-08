using AdoNetWindow.Model;
using AdoNetWindow.Product;
using Newtonsoft.Json;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Pending
{
    public partial class PendingManager : Form
    {
        ICustomsRepository customsRepository = new CustomsRepository();
        IShippingRepository shippingRepository = new ShippingRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        CalendarModule.calendar cal;
        Libs.ftpCommon ftp = new Libs.ftpCommon();
        UsersModel um;
        int id;
        string type;
        bool isCalculate;
        public PendingManager(CalendarModule.calendar calendar, UsersModel uModel, string sType, int cid)
        {
            InitializeComponent();
            cal = calendar;
            um = uModel;
            id = cid;
            type = sType;
        }

        private void PendingManager_Load(object sender, EventArgs e)
        {
            //DgvUnPending Columns Header Style 
            SetDgvUnpendigStyleSetting();
            //Call pending Data
            GetCustomsShipping();
            //DatagridView Double Buffered Setting
            Init_DataGridView();
        }
        #region Method
        private void SetFontSizes()
        {
            int sizes = (int)nudFontsize.Value;
            dgvPending.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", sizes, FontStyle.Regular);
            
            foreach (DataGridViewRow row in dgvPending.Rows)
                row.DefaultCellStyle.Font = new Font("나눔고딕", sizes, FontStyle.Regular);
        }
        //DatagridView Double Buffered Setting
        private void Init_DataGridView()
        {
            dgvPending.DoubleBuffered(true);
        }
        //삭제
        private void DeleteExecute()
        {
            if (um.auth_level < 50)
            {

                MessageBox.Show(this, "접근 권한이 없습니다!");
                this.Activate();
                return;
            }
            List<AllCustomsModel> editList = customsRepository.GetUnPending(id.ToString());
            if (editList.Count == 0)
            {
                MessageBox.Show(this,"등록되지 않은 내역입니다.");
                this.Activate();

                return;
            }
            //Messagebox
            if (MessageBox.Show(this,txtAtoNo.Text + "의 모든 내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
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
                    + "(삭제)" + editList[0].ato_no.ToString()
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
                DirectoryInfo di = new DirectoryInfo(folder_path);
                if (!di.Exists)
                {
                    isFlag = true;
                    if (!ftp.RenameDirectory(editList[0].contract_year.ToString(), editList[0].ato_no, folder_path, false))
                    {
                        MessageBox.Show(this, "폴더명 수정중 에러가 발생하였습니다.");
                        this.Activate();
                    }
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
                MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                MessageBox.Show(this,"삭제완료");
                this.Activate();
                this.Dispose();
            }
        }

        private bool Validation()
        {
            bool isFlag = true;
            if (dgvPending.Rows.Count == 0)
            {
                MessageBox.Show(this,"등록할 내역이 없습니다.");
                this.Activate();
                isFlag = false;
            }

            double tmp;
            if (!double.TryParse(txtPaymentExchangeRate.Text, out tmp))
            {
                MessageBox.Show(this,"결제환율 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtCustomsClearanceExchangeRate.Text, out tmp))
            {
                MessageBox.Show(this,"통관환율 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtAdditionalAmount.Text, out tmp))
            {
                MessageBox.Show(this,"박스당 가산금액 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtLogisticsCost.Text, out tmp))
            {
                MessageBox.Show(this,"박스당 국제물류비 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtLcOpenCharge.Text, out tmp))
            {
                MessageBox.Show(this,"신용장 개설수수료 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtLcTelegraphCharge.Text, out tmp))
            {
                MessageBox.Show(this,"신용장 전신료 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtLcConversionFee.Text, out tmp))
            {
                MessageBox.Show(this,"신용장 개설환출료 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtUsanceFee.Text, out tmp))
            {
                MessageBox.Show(this,"유산스 인수수수료 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtImportFee.Text, out tmp))
            {
                MessageBox.Show(this,"수입추심수수료 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtWithdrawalFee.Text, out tmp))
            {
                MessageBox.Show(this,"유산스 인수수수료 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtBuDiscount.Text, out tmp))
            {
                MessageBox.Show(this,"BU 할인료 + 환가료 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtBankOther.Text, out tmp))
            {
                MessageBox.Show(this,"은행 및 기타비용 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            else if (!double.TryParse(txtAgencyFee.Text, out tmp))
            {
                MessageBox.Show(this,"대행수수료 값이 숫자 형식이 아닙니다.");
                this.Activate();
                isFlag = false;
            }
            //내역
            for (int i = 0; i < dgvPending.Rows.Count; i++)
            {
                DataGridViewRow row = dgvPending.Rows[i];
                for (int j = 14; j < dgvPending.ColumnCount; j++)
                {
                    if (dgvPending.Columns[j].Name != "remark" && dgvPending.Columns[j].Name != "weight_calculate")
                    {
                        if (row.Cells[j].Value == null)
                        {
                            row.Cells[j].Value = 0;
                        }
                        else if (!double.TryParse(row.Cells[j].Value.ToString(), out tmp))
                        {
                            MessageBox.Show(this,dgvPending.Columns[j].HeaderText + " 의 값이 숫자 형식이 아닙니다.");
                            dgvPending.CurrentCell = row.Cells[j];
                            this.Activate();
                            return false;
                        }
                    }
                }
            }

            return isFlag;
        }
        //원가계산 등록
        private void InsertPending()
        {
            dgvPending.EndEdit();
            //유효성 검사
            if (!Validation())
                return;
            //Messagebox
            if (MessageBox.Show(this,"등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                this.Activate();
                return;
            }

            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();

            //Matche Input Data(Shipping)
            ShippingModel sModel = new ShippingModel();
            sModel.custom_id = id;
            
            sModel.payment_ex_rate = Convert.ToDouble(txtPaymentExchangeRate.Text.Replace(",", "").Trim());
            sModel.customs_clearance_ex_rate = Convert.ToDouble(txtCustomsClearanceExchangeRate.Text.Replace(",", "").Trim());
            sModel.amount_per_box = Convert.ToDouble(txtAdditionalAmount.Text.Replace(",", "").Trim());
            sModel.cost_per_box = Convert.ToDouble(txtLogisticsCost.Text.Replace(",", "").Trim());
            sModel.lc_opening_charge = Convert.ToDouble(txtLcOpenCharge.Text.Replace(",", "").Trim());
            sModel.lc_telegraph_charge = Convert.ToDouble(txtLcTelegraphCharge.Text.Replace(",", "").Trim());
            sModel.lc_opening_conversion_fee = Convert.ToDouble(txtLcConversionFee.Text.Replace(",", "").Trim());
            sModel.usance_underwriting_fee = Convert.ToDouble(txtUsanceFee.Text.Replace(",", "").Trim());
            sModel.import_collection_fee = Convert.ToDouble(txtImportFee.Text.Replace(",", "").Trim());
            sModel.usance_acceptance_fee = Convert.ToDouble(txtWithdrawalFee.Text.Replace(",", "").Trim());
            sModel.discount_charge = Convert.ToDouble(txtBuDiscount.Text.Replace(",", "").Trim());
            sModel.banking_expenses = Convert.ToDouble(txtBankOther.Text.Replace(",", "").Trim());
            sModel.agency_fee = Convert.ToDouble(txtAgencyFee.Text.Replace(",", "").Trim());
            sModel.amount_matched_seaover = Convert.ToDouble(txtSeaoverTotalAmount.Text.Replace(",", "").Trim());
            sModel.total_amount_seaover = Convert.ToDouble(txtTotalTrqAmountSeaover.Text.Replace(",", "").Trim());
            sModel.shipping_ex_rate = Convert.ToDouble(txtShippingExchangeRate.Text.Replace(",", "").Trim());
            sModel.remark = txtRemark.Text;

            //Shipping data delete
            sql = shippingRepository.DeleteShipping(id);
            sqlList.Add(sql);
            //Shipping data insert
            sql = shippingRepository.InsertShipping(sModel);
            sqlList.Add(sql);

            //Customs data 
            int sub_id = 1;
            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgvPending.Rows[i];
                //Match Customs data
                UncommfirmModel uModel = new UncommfirmModel();
                uModel.id = id;
                uModel.sub_d = sub_id;
                uModel.warehouse = row.Cells["warehouse"].Value.ToString();
                uModel.broker = row.Cells["custom_officer"].Value.ToString();
                uModel.origin = row.Cells["origin"].Value.ToString();
                uModel.product = row.Cells["product"].Value.ToString();
                uModel.sizes = row.Cells["sizes"].Value.ToString();
                uModel.weight_calculate = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                uModel.box_weight = row.Cells["box_weight"].Value.ToString();
                uModel.cost_unit = row.Cells["cost_unit"].Value.ToString();
                uModel.unit_price = Convert.ToDouble(row.Cells["unit_price"].Value.ToString().Replace(",", "").Trim());
                uModel.quantity_on_paper = Convert.ToDouble(row.Cells["contract_qty"].Value.ToString().Replace(",", "").Trim());
                uModel.qty = Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", "").Trim());

                uModel.tariff_rate = Convert.ToDouble(row.Cells["tariff_rate"].Value.ToString().Replace(",", "").Trim()) / 100;
                uModel.vat_rate = Convert.ToDouble(row.Cells["vat_rate"].Value.ToString().Replace(",", "").Trim()) / 100;
                uModel.clearance_rate = Convert.ToDouble(row.Cells["customs_clearance_exchange_rate"].Value.ToString().Replace(",", "").Trim());
                uModel.loading_cost_per_box = Convert.ToDouble(row.Cells["loading_cost"].Value.ToString().Replace(",", "").Trim());
                uModel.refrigeration_charge = Convert.ToDouble(row.Cells["refrigeration_charge"].Value.ToString().Replace(",", "").Trim());
                uModel.remark = row.Cells["remark"].Value.ToString();
                uModel.trq_amount = Convert.ToDouble(row.Cells["trq_tariff"].Value.ToString().Replace(",", "").Trim());
                uModel.shipping_trq_amount = Convert.ToDouble(row.Cells["shipping_trq_tariff"].Value.ToString().Replace(",", "").Trim());
                double box_price_adjust;
                if (row.Cells["box_price_adjust"].Value == null || !double.TryParse(row.Cells["box_price_adjust"].Value.ToString(), out box_price_adjust))
                    box_price_adjust = 0;
                uModel.box_price_adjust = box_price_adjust;

                double shipping_box_price_adjust;
                if (row.Cells["shipping_box_price_adjust"].Value == null || !double.TryParse(row.Cells["shipping_box_price_adjust"].Value.ToString(), out shipping_box_price_adjust))
                    shipping_box_price_adjust = 0;
                uModel.shipping_box_price_adjust = shipping_box_price_adjust;

                sql = customsRepository.UpdateSql(uModel);
                sqlList.Add(sql);

                sub_id += 1;
            }
            int result = customsRepository.UpdateCustomTran(sqlList);
            if (result == -1)
            {
                MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
                this.Activate();
            }
        }

        //통관 및 추가내역 불러오기
        private void GetCustomsShipping()
        {
            this.dgvPending.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
            DataGridView dgv = dgvPending;
            //원가계산 추가입력값 가져오기
            ShippingModel sModel = shippingRepository.GetShipping(id.ToString());
            if (sModel != null)
            {
                txtShippingExchangeRate.Text = sModel.shipping_ex_rate.ToString("#,##0.00");
                txtPaymentExchangeRate.Text = sModel.payment_ex_rate.ToString("#,##0.00");
                txtCustomsClearanceExchangeRate.Text = sModel.customs_clearance_ex_rate.ToString("#,##0.00");
                txtAdditionalAmount.Text = sModel.amount_per_box.ToString("#,##0");
                txtLogisticsCost.Text = sModel.cost_per_box.ToString("#,##0");
                txtLcOpenCharge.Text = sModel.lc_opening_charge.ToString("#,##0");
                txtLcTelegraphCharge.Text = sModel.lc_telegraph_charge.ToString("#,##0");
                txtLcConversionFee.Text = sModel.lc_opening_conversion_fee.ToString("#,##0");
                txtUsanceFee.Text = sModel.usance_underwriting_fee.ToString("#,##0");
                txtImportFee.Text = sModel.import_collection_fee.ToString("#,##0");
                txtWithdrawalFee.Text = sModel.usance_acceptance_fee.ToString("#,##0");
                txtBuDiscount.Text = sModel.discount_charge.ToString("#,##0");
                txtBankOther.Text = sModel.banking_expenses.ToString("#,##0");
                txtAgencyFee.Text = sModel.agency_fee.ToString("#,##0");
                txtRemark.Text = sModel.remark;
                txtSeaoverTotalAmount.Text = sModel.amount_matched_seaover.ToString("#,##0");
                txtTotalTrqAmountSeaover.Text = sModel.total_amount_seaover.ToString("#,##0");
            }
            //통관 품목내역 가져오기
            List<AllCustomsModel> model = customsRepository.GetAllTypePending(type, id.ToString());
            double cWeight = 0;
            double wWeight = 0;

            if (model.Count > 0)
            {
                int n;
                for (int i = 0; i < model.Count; i++)
                {
                    n = dgv.Rows.Add();

                    txtContractYear.Text = model[0].contract_year.ToString();
                    txtAtoNo.Text = model[0].ato_no;
                    txtPidate.Text = model[0].pi_date;
                    txtContractNo.Text = model[0].contract_no;
                    txtShipper.Text = model[0].shipper;
                    txtLcNo.Text = model[0].lc_payment_date;
                    txtUsance.Text = model[0].usance_type;
                    txtAgency.Text = model[0].agency_type;
                    txtDivision.Text = model[0].division;
                    txtManager.Text = model[0].manager;
                    DateTime dt;
                    if (DateTime.TryParse(model[0].updatetime, out dt))
                        txtUpdate.Text = Convert.ToDateTime(model[0].updatetime).ToString("yyyy-MM-dd hh:mm:ss");

                    dgv.Rows[n].Cells["contract_year"].Value = model[i].contract_year;
                    dgv.Rows[n].Cells["lc_open_date"].Value = model[i].lc_open_date;
                    dgv.Rows[n].Cells["bl_no"].Value = model[i].bl_no;
                    dgv.Rows[n].Cells["shipment_date"].Value = model[i].shipment_date;
                    dgv.Rows[n].Cells["etd"].Value = model[i].etd;
                    dgv.Rows[n].Cells["eta"].Value = model[i].eta;
                    dgv.Rows[n].Cells["warehouse_date"].Value = model[i].warehousing_date;
                    dgv.Rows[n].Cells["pending_date"].Value = model[i].pending_check;
                    dgv.Rows[n].Cells["cc_status"].Value = model[i].cc_status;
                    dgv.Rows[n].Cells["custom_officer"].Value = model[i].broker;
                    dgv.Rows[n].Cells["warehouse"].Value = model[i].warehouse;
                    dgv.Rows[n].Cells["origin"].Value = model[i].origin;
                    dgv.Rows[n].Cells["product"].Value = model[i].product;
                    dgv.Rows[n].Cells["sizes"].Value = model[i].sizes;
                    dgv.Rows[n].Cells["weight_calculate"].Value = model[i].weight_calculate;
                    dgv.Rows[n].Cells["weight_calculate"].ToolTipText = "TRUE : 박스단가\nFALSE : 트레이단가";
                    dgv.Rows[n].Cells["box_weight"].Value = model[i].box_weight;
                    dgv.Rows[n].Cells["cost_unit"].Value = model[i].cost_unit;
                    dgv.Rows[n].Cells["unit_price"].Value = model[i].unit_price;
                    dgv.Rows[n].Cells["contract_qty"].Value = model[i].quantity_on_paper;
                    dgv.Rows[n].Cells["warehouse_qty"].Value = model[i].qty;
                    if (model[i].box_weight == "")
                        model[i].box_weight = "0";

                    dgv.Rows[n].Cells["contract_weight"].Value = (model[i].quantity_on_paper * Convert.ToDouble(model[i].box_weight)).ToString("#,##0.00");
                    cWeight += model[i].quantity_on_paper * Convert.ToDouble(model[i].box_weight);
                    dgv.Rows[n].Cells["warehouse_weight"].Value = (model[i].qty * Convert.ToDouble(model[i].box_weight)).ToString("#,##0.00");
                    wWeight += model[i].qty * Convert.ToDouble(model[i].box_weight);

                    dgv.Rows[n].Cells["tariff_rate"].Value = model[i].tariff_rate * 100;
                    dgv.Rows[n].Cells["vat_rate"].Value = model[i].vat_rate * 100;

                    dgv.Rows[n].Cells["shipping_exchange_rate"].Value = sModel.shipping_ex_rate;
                    dgv.Rows[n].Cells["payment_exchange_rate"].Value = sModel.payment_ex_rate;
                    if (model[i].clearance_rate == 0)
                    {
                        dgv.Rows[n].Cells["customs_clearance_exchange_rate"].Value = sModel.customs_clearance_ex_rate;
                        txtCustomsClearanceExchangeRate.Text = sModel.customs_clearance_ex_rate.ToString("#,##0.00");
                    }
                    else
                    {
                        dgv.Rows[n].Cells["customs_clearance_exchange_rate"].Value = model[i].clearance_rate;
                        txtCustomsClearanceExchangeRate.Text = model[i].clearance_rate.ToString("#,##0.00");
                    }
                    dgv.Rows[n].Cells["loading_cost"].Value = model[i].loading_cost_per_box;
                    dgv.Rows[n].Cells["refrigeration_charge"].Value = model[i].refrigeration_charge;
                    dgv.Rows[n].Cells["remark"].Value = model[i].remark;
                    dgv.Rows[n].Cells["trq_tariff"].Value = model[i].trq_amount;
                    dgv.Rows[n].Cells["shipping_trq_tariff"].Value = model[i].shipping_trq_amount;
                    dgv.Rows[n].Cells["box_price_adjust"].Value = model[i].box_price_adjust;
                    dgv.Rows[n].Cells["shipping_box_price_adjust"].Value = model[i].shipping_box_price_adjust;
                }
                //총 합계값 계산===================================================
                n = dgv.Rows.Add();
                dgv.Rows[n].Cells["contract_weight"].Value = cWeight.ToString("#,##0");
                dgv.Rows[n].Cells["warehouse_weight"].Value = wWeight.ToString("#,##0");

                //마지막 합계줄 스타일
                dgv.Rows[n].DefaultCellStyle.BackColor = Color.LightGray;
                dgv.Rows[n].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            }
            this.dgvPending.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
            //원가계산 계산
            CostingCalculate();
        }
        private void TotalCalculate()
        {
            if (dgvPending.Rows.Count > 0)
            {
                dgvPending.EndEdit();
                double total_warehouse_qty = 0;
                double total_contract_qty = 0;
                double bWeight = 0;
                double tPrice = 0;
                double cWeight = 0;
                double wWeight = 0;
                double total_amount_size = 0;
                double trq_total_amount = 0;
                double shipping_total_amount_size = 0;
                double shipping_trq_total_amount = 0;

                for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                {
                    DataGridViewRow row = dgvPending.Rows[i];
                    //계약, 입고수량
                    double contract_qty;
                    if (row.Cells["contract_qty"].Value == null || !double.TryParse(row.Cells["contract_qty"].Value.ToString(), out contract_qty))
                        contract_qty = 0;
                    total_contract_qty += contract_qty;
                    double warehouse_qty;
                    if (row.Cells["warehouse_qty"].Value == null || !double.TryParse(row.Cells["warehouse_qty"].Value.ToString(), out warehouse_qty))
                        warehouse_qty = 0;
                    total_warehouse_qty += warehouse_qty;


                    double box_weight;
                    if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                        box_weight = 0;

                    double unit_price;
                    if (row.Cells["unit_price"].Value == null || !double.TryParse(row.Cells["unit_price"].Value.ToString(), out unit_price))
                        unit_price = 0;


                    //서류선적중량
                    cWeight += box_weight * contract_qty;
                    //실제선적중량
                    wWeight += box_weight * warehouse_qty;
                    //총 중량, 총결제금액
                    bWeight += box_weight;
                    //계약수량 결제금액
                    tPrice += unit_price * contract_qty;


                    double amount_size;
                    if (row.Cells["total_amount_size"].Value == null || !double.TryParse(row.Cells["total_amount_size"].Value.ToString(), out amount_size))
                        amount_size = 0;
                    total_amount_size += amount_size;


                    if (row.Cells["trq_total_amount"].Value == null || !double.TryParse(row.Cells["trq_total_amount"].Value.ToString(), out amount_size))
                        amount_size = 0;
                    trq_total_amount += amount_size;

                    if (row.Cells["shipping_total_amount_size"].Value == null || !double.TryParse(row.Cells["shipping_total_amount_size"].Value.ToString(), out amount_size))
                        amount_size = 0;
                    shipping_total_amount_size += amount_size;

                    if (row.Cells["shipping_trq_total_amount"].Value == null || !double.TryParse(row.Cells["shipping_trq_total_amount"].Value.ToString(), out amount_size))
                        amount_size = 0;
                    shipping_trq_total_amount += amount_size;


                }
                /*txtTotalBoxWeight.Text = bWeight.ToString("#,##0.00");
                txtTotalPrice.Text = tPrice.ToString("#,##0.00");*/

                dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["contract_weight"].Value = cWeight.ToString("#,##0.00");
                dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["warehouse_weight"].Value = wWeight.ToString("#,##0.00");
                dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["contract_qty"].Value = total_contract_qty.ToString("#,##0");
                dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["warehouse_qty"].Value = total_warehouse_qty.ToString("#,##0");

                //dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["warehouse_qty"].Value = total_amount_size.ToString("#,##0");
                dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["trq_total_amount"].Value = trq_total_amount.ToString("#,##0");
                dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["shipping_total_amount_size"].Value = shipping_total_amount_size.ToString("#,##0");
                dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["shipping_trq_total_amount"].Value = shipping_trq_total_amount.ToString("#,##0");

            }
        }
        private void SetDgvUnpendigStyleSetting()
        {
            DataGridView dgv = dgvPending;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            //Disable Sorting for DataGridView
            foreach (DataGridViewColumn item in dgvPending.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //Visible
            dgvPending.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.Columns["contract_year"].Visible = false;
            dgv.Columns["lc_open_date"].Visible = false;
            dgv.Columns["bl_no"].Visible = false;
            dgv.Columns["shipment_date"].Visible = false;
            dgv.Columns["etd"].Visible = false;
            dgv.Columns["eta"].Visible = false;
            dgv.Columns["warehouse_date"].Visible = false;
            dgv.Columns["pending_date"].Visible = false;
            dgv.Columns["cc_status"].Visible = false;
            //숫자값 왼쪽정렬
            foreach (DataGridViewColumn col in dgvPending.Columns)
            {
                if (!(col.Name == "product" || col.Name == "origin" || col.Name == "sizes" || col.Name == "unit" || col.Name == "custom_officer" || col.Name == "warehouse"))
                {
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            //저장값 및 계산값
            dgv.Columns["custom_officer"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["custom_officer"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["warehouse"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["warehouse"].HeaderCell.Style.ForeColor = Color.White;

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

            dgv.Columns["box_weight"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["box_weight"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["cost_unit"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["cost_unit"].HeaderCell.Style.ForeColor = Color.White;

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

            //계산값
            dgv.Columns["contract_weight"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["contract_weight"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["warehouse_weight"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["warehouse_weight"].HeaderCell.Style.ForeColor = Color.Black;

            //추가 입력값
            dgv.Columns["shipping_exchange_rate"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["shipping_exchange_rate"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["payment_exchange_rate"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["payment_exchange_rate"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["customs_clearance_exchange_rate"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["customs_clearance_exchange_rate"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["loading_cost"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["loading_cost"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["refrigeration_charge"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["refrigeration_charge"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["remark"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["remark"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            //계산값2
            dgv.Columns["invoice_usd"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["invoice_usd"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["invoice_krw"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["invoice_krw"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["bank_settlement_amount"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["bank_settlement_amount"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["additional_amount_per_box"].HeaderCell.Style.BackColor = Color.FromArgb(252, 228, 214);
            dgv.Columns["additional_amount_per_box"].HeaderCell.Style.ForeColor = Color.Black;

            //박스원가
            dgv.Columns["box_price"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["box_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["box_price"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["shipping_box_price"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["shipping_box_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["shipping_box_price"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["box_price_copy"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["box_price_copy"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["box_price_copy"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["total_amount_size"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["total_amount_size"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["total_amount_size"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["shipping_total_amount_size"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["shipping_total_amount_size"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["shipping_total_amount_size"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["shipping_box_price_copy"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["shipping_box_price_copy"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["shipping_box_price_copy"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["box_price_adjust"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["box_price_adjust"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["shipping_box_price_adjust"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["shipping_box_price_adjust"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            //TRQ
            dgv.Columns["trq_tariff"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["trq_tariff"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["trq_tariff"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["shipping_trq_tariff"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["shipping_trq_tariff"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["shipping_trq_tariff"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);

            dgv.Columns["trq_after"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["trq_after"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["trq_after"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["shipping_trq_after"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["shipping_trq_after"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["shipping_trq_after"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["trq_box_price"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["trq_box_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["trq_box_price"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["shipping_trq_box_price"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["shipping_trq_box_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["shipping_trq_box_price"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["trq_total_amount"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["trq_total_amount"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["trq_total_amount"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgv.Columns["shipping_trq_total_amount"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["shipping_trq_total_amount"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["shipping_trq_total_amount"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

        }
        #endregion

        #region 원가계산 Method 
        /*private void CostingCalculate()
        {
            isCalculate = false;
            this.dgvPending.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
            this.dgvPending2.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending2_CellValueChanged);
            //내역존재 유무
            if (dgvPending.Rows.Count == 0)
            {
                isCalculate = true;
                this.dgvPending.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
                this.dgvPending2.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending2_CellValueChanged);
                return;
            }
            //빈값 0으로 변경
            else
            {
                for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                {
                    foreach (DataGridViewCell cell in dgvPending.Rows[i].Cells)
                    {
                        string name = dgvPending.Columns[cell.ColumnIndex].Name;
                        if (name == "box_weight" || name == "cost_unit" || name == "unit_price" || name == "contract_qty" || name == "warehouse_qty" || name == "tariff_rate" || name == "vat_rate"
                            || name == "payment_exchange_rate" || name == "customs_clearance_exchange_rate" || name == "loading_cost" || name == "refrigeration_charge" || name == "trq_tariff")
                        {
                            if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                                cell.Value = "0";
                        }
                    }
                }
            }
            //서류선적중량, 실제선적중량
            double total_paper_shipment_weight = 0;
            double total_actual_shipment_weight = 0;
            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgvPending.Rows[i];

                double box_weight;
                if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                    box_weight = 0;
                total_paper_shipment_weight += box_weight * Convert.ToDouble(row.Cells["contract_qty"].Value.ToString().Replace(",", ""));
                total_actual_shipment_weight += box_weight * Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""));
            }

            //TRQ 제외한 실제선적중량
            double total_actual_shipment_weight_not_trq = 0;
            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgvPending.Rows[i];
                double tmp;
                if (!double.TryParse(row.Cells["trq_tariff"].Value.ToString(), out tmp))
                    tmp = 0;

                //계산방법
                bool weight_calculate;
                if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = true;
                //트레이
                double cost_unit;
                if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                    cost_unit = 0;

                //TRQ 값이 0 초과인 품목중량만
                if (tmp == 0)
                {
                    double box_weight;
                    if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                        box_weight = 0;
                    if (!weight_calculate && cost_unit > 0)
                        total_actual_shipment_weight_not_trq += cost_unit * Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""));
                    else
                        total_actual_shipment_weight_not_trq += box_weight * Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""));
                }
            }

            //원가계산
            double total_box_price = 0;
            double invoice_amount_usd = 0;
            double invoice_amount_krw = 0;
            double shipping_invoice_amount_krw = 0;
            //총 원가
            double All_bank_settlement_amount_krw = 0;
            double All_bank_settlement_amount_usd = 0;
            double All_custom_per_box = 0;
            double All_vat = 0;
            double All_logistic_cost = 0;
            double All_total_union_cost_amount = 0;
            double total_agency_fee = 0;
            double total_bank_other = 0;
            double total_bu_discount = 0;
            double total_withdrawal_fee = 0;
            double total_import_fee = 0;
            double total_usance_fee = 0;
            double total_lc_conversion_fee = 0;
            double total_lc_telegraph_charge = 0;
            double total_lc_open_charge = 0;
            double total_logistic_cost = 0;
            double total_contract_weight = 0;
            double total_warehouse_weight = 0;
            //사이즈별 금액합산
            double total_amount_per_size = 0;
            //TRQ
            double total_trq_amount = 0;
            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgvPending.Rows[i];

                double box_weight;
                if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                    box_weight = 0;
                double unit_price;
                if (!double.TryParse(row.Cells["unit_price"].Value.ToString(), out unit_price))
                    unit_price = 0;

                //계산방법
                bool weight_calculate;
                if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = true;
                //트레이
                double cost_unit;
                if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                    cost_unit = 0;

                //2023-06-19  <== 이때보니 이유는 모르겠는데 주석처리가 되어있었음 (AA15 계산이 수정화면과 원가계산 화면이 달랐음 트레이계산) 다시 문제가 생길때 다시 확인바람
                //트레이일 경우 단가(트레이)를 단가(박스중량) 변환계산
                if (!weight_calculate && cost_unit > 0)
                    unit_price = (unit_price * cost_unit) / box_weight;
                double contract_qty = Convert.ToDouble(row.Cells["contract_qty"].Value.ToString().Replace(",", ""));
                double warehouse_qty = Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""));
                row.Cells["contract_weight"].Value = (box_weight * contract_qty).ToString("#,##0.00");
                row.Cells["warehouse_weight"].Value = (box_weight * warehouse_qty).ToString("#,##0.00");
                //선적중량
                double paper_shipment_weight = box_weight * contract_qty;
                total_contract_weight += paper_shipment_weight;
                //실제중량
                double actual_shipment_weight = box_weight * warehouse_qty;
                total_warehouse_weight += actual_shipment_weight;
                //총수입액 USD
                invoice_amount_usd = unit_price * paper_shipment_weight;
                row.Cells["invoice_usd"].Value = invoice_amount_usd.ToString("#,##0.00");
                All_bank_settlement_amount_usd += invoice_amount_usd;
                //환율
                double pay_eRate = Convert.ToDouble(row.Cells["payment_exchange_rate"].Value.ToString().Replace(",", ""));
                double ship_eRate = Convert.ToDouble(row.Cells["shipping_exchange_rate"].Value.ToString().Replace(",", ""));
                double custom_eRate = Convert.ToDouble(row.Cells["customs_clearance_exchange_rate"].Value.ToString().Replace(",", ""));
                //총수입액 KRW
                invoice_amount_krw = invoice_amount_usd * pay_eRate;
                shipping_invoice_amount_krw = invoice_amount_usd * ship_eRate;

                row.Cells["invoice_krw"].Value = invoice_amount_krw.ToString("#,##0");
                All_bank_settlement_amount_krw += invoice_amount_krw;


                // ** 박스당 수입액
                double bank_settlement_amount = invoice_amount_krw / contract_qty;
                double shipping_bank_settlement_amount = shipping_invoice_amount_krw / contract_qty;
                row.Cells["bank_settlement_amount"].Value = bank_settlement_amount.ToString("#,##0");

                // 관세율
                double tariff_rate = Convert.ToDouble(row.Cells["tariff_rate"].Value.ToString().Replace(",", "")) / 100;
                //박스당 가산금액
                if (string.IsNullOrEmpty(txtAdditionalAmount.Text))
                    txtAdditionalAmount.Text = "0";
                double total_add_amount = Convert.ToDouble(txtAdditionalAmount.Text.Replace(",", ""));
                double add_amount = total_add_amount / total_paper_shipment_weight * box_weight;
                row.Cells["additional_amount_per_box"].Value = add_amount.ToString("#,##0");
                //박스당 과세가격
                double settlement_per_box = ((invoice_amount_usd * custom_eRate) / contract_qty) + add_amount;
                //박스당 가산금액
                double total_addiyional_amount = 0;
                if (!double.TryParse(txtAdditionalAmount.Text, out total_addiyional_amount))
                    total_addiyional_amount = 0;
                double add_per_box = total_addiyional_amount / total_paper_shipment_weight * box_weight;


                *//*All_custom_per_box += custom_per_box;*//*  //옛날버전
                                                           //박스당 관세
                double custom_per_box = 0;
                double tmp;
                if (!double.TryParse(row.Cells["trq_tariff"].Value.ToString(), out tmp))
                    tmp = 0;
                //TRP 아닌 품목만 해당
                custom_per_box = tariff_rate * settlement_per_box;
                All_custom_per_box += custom_per_box * contract_qty;
                row.Cells["custom_per_box"].Value = custom_per_box.ToString("#,##0");

                // ** 부가가치세
                double vat_rate = Convert.ToDouble(row.Cells["vat_rate"].Value.ToString().Replace(",", "")) / 100;
                double vat = (settlement_per_box + custom_per_box) * vat_rate;
                row.Cells["vat"].Value = vat.ToString("#,##0");
                All_vat += vat * warehouse_qty;

                // ** 박스당 국제 물류비
                if (string.IsNullOrEmpty(txtLogisticsCost.Text))
                    txtLogisticsCost.Text = "0";
                total_logistic_cost = Convert.ToDouble(txtLogisticsCost.Text.Replace(",", ""));
                double logistic_cost = (total_logistic_cost / total_actual_shipment_weight) * box_weight;
                *//*double logistic_cost = 0;
                // *trp 적용이 안된 품목만 국제물류비 발생함
                double tmp;
                if (!double.TryParse(row.Cells["trq_tariff"].Value.ToString(), out tmp))
                    tmp = 0;
                if (tmp == 0)
                    logistic_cost = (total_logistic_cost / total_actual_shipment_weight_not_trq) * box_weight;
                *//*
                row.Cells["logistics_cost_per_box"].Value = logistic_cost.ToString("#,##0");
                All_logistic_cost += logistic_cost;

                // ** 박스당토탈노조비
                double loading_cost = Convert.ToDouble(row.Cells["loading_cost"].Value.ToString().Replace(",", ""));
                double total_union_cost;
                int contract_year;
                if (row.Cells["contract_year"].Value == null || !int.TryParse(row.Cells["contract_year"].Value.ToString(), out contract_year))
                    contract_year = 2021;

                DateTime dt;
                if (row.Cells["warehouse_date"].Value == null || !DateTime.TryParse(row.Cells["warehouse_date"].Value.ToString(), out dt))
                    dt = DateTime.Now;
                //24년도
                if (dt.Year >= 2024)
                    total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.04 + loading_cost * 0.045 + loading_cost * 0.01 + loading_cost * 0.083);
                //23년도
                else if (dt.Year >= 2023)
                    total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.0399 + loading_cost * 0.045 + loading_cost * 0.01 + loading_cost * 0.083);
                //22년도
                else if (dt.Year >= 2022)
                {
                    //22년 3월 이후
                    if (dt >= Convert.ToDateTime("2022-03-01"))
                        total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.0392 + loading_cost * 0.045 + loading_cost * 0.01 + loading_cost * 0.083);
                    //22년 3월 이전
                    else
                        total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.03 + loading_cost * 0.022 + loading_cost * 0.02 + loading_cost * 0.083);
                }
                //21년도
                else
                    total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.03 + loading_cost * 0.022 + loading_cost * 0.02 + loading_cost * 0.083);


                //박스당토탈노조비
                row.Cells["total_union_cost_per_box"].Value = total_union_cost.ToString("#,##0");
                //전체노조비
                double total_union_cost_amount = total_union_cost * warehouse_qty;
                row.Cells["total_union_cost"].Value = total_union_cost_amount.ToString("#,##0");
                All_total_union_cost_amount += total_union_cost_amount;

                // ** 입출고비 + 냉장고 3달
                double refrigeration_charge = Convert.ToDouble(row.Cells["refrigeration_charge"].Value.ToString().Replace(",", ""));

                // ** 신용장 개설수수료
                if (string.IsNullOrEmpty(txtLcOpenCharge.Text))
                    txtLcOpenCharge.Text = "0";
                total_lc_open_charge = Convert.ToDouble(txtLcOpenCharge.Text.Replace(",", ""));
                double lc_open_charge = total_lc_open_charge / total_paper_shipment_weight * box_weight;
                row.Cells["lc_opening_charge"].Value = lc_open_charge.ToString("#,##0");

                // ** 신용장 전신료*************
                if (string.IsNullOrEmpty(txtLcTelegraphCharge.Text))
                    txtLcTelegraphCharge.Text = "0";
                total_lc_telegraph_charge = Convert.ToDouble(txtLcTelegraphCharge.Text.Replace(",", ""));
                double lc_telegraph_charge = total_lc_telegraph_charge / total_paper_shipment_weight * box_weight;
                row.Cells["lc_telegraph_charge"].Value = lc_telegraph_charge.ToString("#,##0");

                // ** 신용장 전신료
                if (string.IsNullOrEmpty(txtLcConversionFee.Text))
                    txtLcConversionFee.Text = "0";
                total_lc_conversion_fee = Convert.ToDouble(txtLcConversionFee.Text.Replace(",", ""));
                double lc_conversion_fee = total_lc_conversion_fee / total_paper_shipment_weight * box_weight;
                row.Cells["lc_opening_conversion_fee"].Value = lc_conversion_fee.ToString("#,##0");

                // ** 유산스 인수수수료
                if (string.IsNullOrEmpty(txtUsanceFee.Text))
                    txtUsanceFee.Text = "0";
                total_usance_fee = Convert.ToDouble(txtUsanceFee.Text.Replace(",", ""));
                double usance_fee = total_usance_fee / total_paper_shipment_weight * box_weight;
                row.Cells["usance_underwriting_fee"].Value = usance_fee.ToString("#,##0");

                // ** 수입추심수수료
                if (string.IsNullOrEmpty(txtImportFee.Text))
                    txtImportFee.Text = "0";
                total_import_fee = Convert.ToDouble(txtImportFee.Text.Replace(",", ""));
                double import_fee = total_import_fee / total_paper_shipment_weight * box_weight;
                row.Cells["import_collection_fee"].Value = import_fee.ToString("#,##0");

                // ** 유산스 인수수수료
                if (string.IsNullOrEmpty(txtWithdrawalFee.Text))
                    txtWithdrawalFee.Text = "0";
                total_withdrawal_fee = Convert.ToDouble(txtWithdrawalFee.Text.Replace(",", ""));
                double withdrawal_fee = total_withdrawal_fee / total_paper_shipment_weight * box_weight;
                row.Cells["usance_withdrawal_acceptance_fee"].Value = withdrawal_fee.ToString("#,##0");

                // ** BU 할인료 + 환가료
                if (string.IsNullOrEmpty(txtBuDiscount.Text))
                    txtBuDiscount.Text = "0";
                total_bu_discount = Convert.ToDouble(txtBuDiscount.Text.Replace(",", ""));
                double bu_discount = total_bu_discount / total_paper_shipment_weight * box_weight;
                row.Cells["bu_discount"].Value = bu_discount.ToString("#,##0");

                // ** 은행 및 기타비용
                if (string.IsNullOrEmpty(txtBankOther.Text))
                    txtBankOther.Text = "0";
                total_bank_other = Convert.ToDouble(txtBankOther.Text.Replace(",", ""));
                double bank_other = total_bank_other / total_paper_shipment_weight * box_weight;
                row.Cells["banking_expenses"].Value = bank_other.ToString("#,##0");

                // ** 대행수수료
                if (string.IsNullOrEmpty(txtAgencyFee.Text))
                    txtAgencyFee.Text = "0";
                total_agency_fee = Convert.ToDouble(txtAgencyFee.Text.Replace(",", ""));
                double agency_fee = total_agency_fee / total_actual_shipment_weight * box_weight;
                row.Cells["agency_fee"].Value = agency_fee.ToString("#,##0");

                //박스원가
                double box_price = bank_settlement_amount + custom_per_box + vat + logistic_cost + total_union_cost + refrigeration_charge + lc_open_charge + lc_telegraph_charge
                    + lc_conversion_fee + usance_fee + import_fee + withdrawal_fee + bu_discount + bank_other + agency_fee;

                double shipping_box_price = shipping_bank_settlement_amount + custom_per_box + vat + logistic_cost + total_union_cost + refrigeration_charge + lc_open_charge + lc_telegraph_charge
                    + lc_conversion_fee + usance_fee + import_fee + withdrawal_fee + bu_discount + bank_other + agency_fee;

                //박스원가(결제) 조정액
                double price_adjust;
                if (row.Cells["box_price_adjust"].Value == null || !double.TryParse(row.Cells["box_price_adjust"].Value.ToString(), out price_adjust))
                    price_adjust = 0;
                box_price += price_adjust;
                row.Cells["box_price"].Value = box_price.ToString("#,##0.00");
                row.Cells["box_price_copy"].Value = box_price.ToString("#,##0.00");

                //박스원가(선적) 조정액
                double shipping_price_adjust;
                if (row.Cells["shipping_box_price_adjust"].Value == null || !double.TryParse(row.Cells["shipping_box_price_adjust"].Value.ToString(), out shipping_price_adjust))
                    shipping_price_adjust = 0;
                shipping_box_price += shipping_price_adjust;
                row.Cells["shipping_box_price"].Value = box_price.ToString("#,##0.00");
                row.Cells["shipping_box_price_copy"].Value = box_price.ToString("#,##0.00");


                //사이즈별 금액합산
                row.Cells["total_amount_size"].Value = (box_price * contract_qty).ToString("#,##0");
                row.Cells["shipping_total_amount_size"].Value = (shipping_box_price * contract_qty).ToString("#,##0");
                //총 원가
                total_box_price += box_price;
                //사이즈별 금액합산
                total_amount_per_size += box_price * contract_qty;
                //TRQ 박스당 최종원가
                double trq = Convert.ToDouble(row.Cells["trq_tariff"].Value.ToString().Replace(",", ""));
                double trq_box_price = 0;
                double shipping_trq_box_price = 0;
                if (trq > 0)
                {
                    //TRQ 기타
                    double trq_other = lc_open_charge + lc_telegraph_charge + lc_conversion_fee + usance_fee + import_fee + withdrawal_fee + bu_discount + bank_other + agency_fee + total_union_cost + logistic_cost + vat;
                    //TRQ 박스원가 은행결제
                    double box_price_bank_payment = invoice_amount_krw / warehouse_qty;
                    double shipping_box_price_bank_payment = shipping_invoice_amount_krw / warehouse_qty;
                    //KG당 원가
                    double trq_price_per_kg = box_price_bank_payment / box_weight;
                    double shipping_trq_price_per_kg = shipping_box_price_bank_payment / box_weight;
                    //TRQ KG당 원가
                    double trq_price = trq_price_per_kg + trq;
                    double shipping_trq_price = shipping_trq_price_per_kg + trq;
                    //TRQ 박스당 원가
                    double trq_price_per_box = trq_price * box_weight;
                    double sthipping_trq_price_per_box = shipping_trq_price * box_weight;
                    //TRQ 박스 최종원가
                    trq_box_price = trq_price_per_box + trq_other;
                    shipping_trq_box_price = sthipping_trq_price_per_box + trq_other;

                    row.Cells["trq_after"].Value = trq_box_price.ToString("#,##0");
                    row.Cells["shipping_trq_after"].Value = shipping_trq_box_price.ToString("#,##0");
                }
                row.Cells["trq_box_price"].Value = trq_box_price.ToString("#,##0.00");
                row.Cells["shipping_trq_box_price"].Value = shipping_trq_box_price.ToString("#,##0");
                //TRQ 품목당 총금액
                double total_trq = 0;
                double shipping_total_trq = 0;
                if (trq > 0)
                {
                    total_trq = trq_box_price * warehouse_qty;
                    shipping_total_trq = shipping_trq_box_price * warehouse_qty;
                }
                else
                {
                    total_trq = box_price * warehouse_qty;
                    shipping_total_trq = shipping_box_price * warehouse_qty;
                }
                row.Cells["trq_total_amount"].Value = total_trq.ToString("#,##0");
                row.Cells["shipping_trq_total_amount"].Value = shipping_total_trq.ToString("#,##0");
                total_trq_amount += total_trq;
            }
            //총 박스원가 
            txtBoxPrice.Text = total_box_price.ToString("#,##0");
            //총 원가
            txtTotalCost.Text = (All_bank_settlement_amount_krw + All_custom_per_box + All_vat + total_logistic_cost + All_total_union_cost_amount
              + total_agency_fee + total_bank_other + total_bu_discount + total_withdrawal_fee + total_import_fee + total_usance_fee + total_lc_conversion_fee + total_lc_telegraph_charge + total_lc_open_charge).ToString("#,##0");
            //선적중량
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["contract_weight"].Value = total_contract_weight.ToString("#,##0.00");
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["warehouse_weight"].Value = total_warehouse_weight.ToString("#,##0.00");
            //총수입액
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["invoice_krw"].Value = All_bank_settlement_amount_krw.ToString("#,##0");
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["invoice_usd"].Value = All_bank_settlement_amount_usd.ToString("#,##0.00");
            //박스당 관세
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["custom_per_box"].Value = All_custom_per_box.ToString("#,##0");
            //부가가치세
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["vat"].Value = All_vat.ToString("#,##0");
            //전체 노조비
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["total_union_cost"].Value = All_total_union_cost_amount.ToString("#,##0");
            //사이즈별 금액합산
            txtTotalPricePerSize.Text = total_amount_per_size.ToString("#,##0");
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["total_amount_size"].Value = total_amount_per_size.ToString("#,##0");
            //TRQ
            txtTotalTrqAmount.Text = total_trq_amount.ToString("#,##0");
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["trq_total_amount"].Value = total_trq_amount.ToString("#,##0");


            TotalCalculate();
            this.dgvPending.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
            this.dgvPending2.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending2_CellValueChanged);
            isCalculate = true;
        }*/
        private void CostingCalculate()
        {
            isCalculate = false;
            this.dgvPending.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
            //내역존재 유무
            if (dgvPending.Rows.Count == 0)
            {
                isCalculate = true;
                this.dgvPending.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
                return;
            }
            //빈값 0으로 변경
            else
            {
                for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                {
                    foreach (DataGridViewCell cell in dgvPending.Rows[i].Cells)
                    {
                        string name = dgvPending.Columns[cell.ColumnIndex].Name;
                        if (name == "box_weight" || name == "cost_unit" || name == "unit_price" || name == "contract_qty" || name == "warehouse_qty" || name == "tariff_rate" || name == "vat_rate"
                            || name == "payment_exchange_rate" || name == "shipping_exchange_rate" || name == "customs_clearance_exchange_rate" || name == "loading_cost" || name == "refrigeration_charge" 
                            || name == "trq_tariff" || name == "shipping_trq_tariff")
                        {
                            if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                                cell.Value = "0";
                        }
                    }
                }
            }
            //서류선적중량, 실제선적중량
            double total_paper_shipment_weight = 0;
            double total_actual_shipment_weight = 0;
            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgvPending.Rows[i];

                double box_weight;
                if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                    box_weight = 0;
                total_paper_shipment_weight += box_weight * Convert.ToDouble(row.Cells["contract_qty"].Value.ToString().Replace(",", ""));
                total_actual_shipment_weight += box_weight * Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""));
            }

            //TRQ 제외한 실제선적중량
            double total_actual_shipment_weight_not_trq = 0;
            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgvPending.Rows[i];
                double tmp;
                if(!double.TryParse(row.Cells["trq_tariff"].Value.ToString(), out tmp))
                    tmp = 0;

                //계산방법
                bool weight_calculate;
                if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = true;
                //트레이
                double cost_unit;
                if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                    cost_unit = 0;

                //TRQ 값이 0 초과인 품목중량만
                if (tmp == 0)
                {
                    double box_weight;
                    if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                        box_weight = 0;
                    if (!weight_calculate && cost_unit > 0)
                        total_actual_shipment_weight_not_trq += cost_unit * Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""));
                    else
                        total_actual_shipment_weight_not_trq += box_weight * Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""));
                }                
            }

            //원가계산
            double total_box_price = 0;
            double total_shipping_box_price = 0;
            double invoice_amount_usd = 0;
            double invoice_amount_krw = 0;
            double shipping_invoice_amount_krw = 0;
            //총 원가
            double All_bank_settlement_amount_krw = 0;
            double All_shipping_bank_settlement_amount_krw = 0;
            double All_bank_settlement_amount_usd = 0;
            double All_custom_per_box = 0;
            double All_vat = 0;
            double All_logistic_cost = 0;
            double All_total_union_cost_amount = 0;
            double total_agency_fee = 0;
            double total_bank_other = 0;
            double total_bu_discount = 0;
            double total_withdrawal_fee = 0;
            double total_import_fee = 0;
            double total_usance_fee = 0;
            double total_lc_conversion_fee = 0;
            double total_lc_telegraph_charge = 0;
            double total_lc_open_charge = 0;
            double total_logistic_cost = 0;
            double total_contract_weight = 0;
            double total_warehouse_weight = 0;
            //사이즈별 금액합산
            double total_amount_per_size = 0, total_shipping_amount_per_size = 0;
            //TRQ
            double total_trq_amount = 0, total_shipping_trq_amount = 0;
            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
            {
                DataGridViewRow row = dgvPending.Rows[i];

                double box_weight;
                if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                    box_weight = 0;
                double unit_price;
                if (!double.TryParse(row.Cells["unit_price"].Value.ToString(), out unit_price))
                    unit_price = 0;

                //계산방법
                bool weight_calculate;
                if (row.Cells["weight_calculate"].Value == null || !bool.TryParse(row.Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = true;
                //트레이
                double cost_unit;
                if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                    cost_unit = 0;

                //2024-02-05 선적환율, 결제환율 둘다 계산하게 바꿈-====================================================
                //공통계산

                //트레이일 경우 단가(트레이)를 단가(박스중량) 변환계산
                if (!weight_calculate && cost_unit > 0)
                    unit_price = (unit_price * cost_unit) / box_weight;
                double contract_qty = Convert.ToDouble(row.Cells["contract_qty"].Value.ToString().Replace(",", ""));
                double warehouse_qty = Convert.ToDouble(row.Cells["warehouse_qty"].Value.ToString().Replace(",", ""));
                row.Cells["contract_weight"].Value = (box_weight * contract_qty).ToString("#,##0.00");
                row.Cells["warehouse_weight"].Value = (box_weight * warehouse_qty).ToString("#,##0.00");
                //선적중량
                double paper_shipment_weight = box_weight * contract_qty;
                total_contract_weight += paper_shipment_weight;
                //실제중량
                double actual_shipment_weight = box_weight * warehouse_qty;
                total_warehouse_weight += actual_shipment_weight;
                
                

                // ** 박스당 국제 물류비
                if (string.IsNullOrEmpty(txtLogisticsCost.Text))
                    txtLogisticsCost.Text = "0";
                total_logistic_cost = Convert.ToDouble(txtLogisticsCost.Text.Replace(",", ""));
                double logistic_cost = (total_logistic_cost / total_actual_shipment_weight) * box_weight;
                row.Cells["logistics_cost_per_box"].Value = logistic_cost.ToString("#,##0");
                All_logistic_cost += logistic_cost;

                // ** 박스당토탈노조비
                double loading_cost = Convert.ToDouble(row.Cells["loading_cost"].Value.ToString().Replace(",", ""));
                double total_union_cost;
                int contract_year;
                if (row.Cells["contract_year"].Value == null || !int.TryParse(row.Cells["contract_year"].Value.ToString(), out contract_year))
                    contract_year = 2021;
                DateTime dt;
                if (row.Cells["warehouse_date"].Value == null || !DateTime.TryParse(row.Cells["warehouse_date"].Value.ToString(), out dt))
                    dt = DateTime.Now;
                //24년도
                if (dt.Year >= 2024)
                    total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.04 + loading_cost * 0.045 + loading_cost * 0.01 + loading_cost * 0.083);
                //23년도
                else if (dt.Year >= 2023)
                    total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.0399 + loading_cost * 0.045 + loading_cost * 0.01 + loading_cost * 0.083);
                //22년도
                else if(dt.Year >= 2022)
                {
                    //22년 3월 이후
                    if (dt >= Convert.ToDateTime("2022-03-01"))
                        total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.0392 + loading_cost * 0.045 + loading_cost * 0.01 + loading_cost * 0.083);
                    //22년 3월 이전
                    else
                        total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.03 + loading_cost * 0.022 + loading_cost * 0.02 + loading_cost * 0.083);
                }
                //21년도
                else
                    total_union_cost = loading_cost + (loading_cost * 0.022 + loading_cost * 0.03 + loading_cost * 0.022 + loading_cost * 0.02 + loading_cost * 0.083);

                //박스당토탈노조비
                row.Cells["total_union_cost_per_box"].Value = total_union_cost.ToString("#,##0");
                //전체노조비
                double total_union_cost_amount = total_union_cost * warehouse_qty;
                row.Cells["total_union_cost"].Value = total_union_cost_amount.ToString("#,##0");
                All_total_union_cost_amount += total_union_cost_amount;

                // ** 입출고비 + 냉장고 3달
                double refrigeration_charge = Convert.ToDouble(row.Cells["refrigeration_charge"].Value.ToString().Replace(",", ""));

                // ** 신용장 개설수수료
                if (string.IsNullOrEmpty(txtLcOpenCharge.Text))
                    txtLcOpenCharge.Text = "0";
                total_lc_open_charge = Convert.ToDouble(txtLcOpenCharge.Text.Replace(",", ""));
                double lc_open_charge = total_lc_open_charge / total_paper_shipment_weight * box_weight;
                row.Cells["lc_opening_charge"].Value = lc_open_charge.ToString("#,##0");

                // ** 신용장 전신료*************
                if (string.IsNullOrEmpty(txtLcTelegraphCharge.Text))
                    txtLcTelegraphCharge.Text = "0";
                total_lc_telegraph_charge = Convert.ToDouble(txtLcTelegraphCharge.Text.Replace(",", ""));
                double lc_telegraph_charge = total_lc_telegraph_charge / total_paper_shipment_weight * box_weight;
                row.Cells["lc_telegraph_charge"].Value = lc_telegraph_charge.ToString("#,##0");

                // ** 신용장 전신료
                if (string.IsNullOrEmpty(txtLcConversionFee.Text))
                    txtLcConversionFee.Text = "0";
                total_lc_conversion_fee = Convert.ToDouble(txtLcConversionFee.Text.Replace(",", ""));
                double lc_conversion_fee = total_lc_conversion_fee / total_paper_shipment_weight * box_weight;
                row.Cells["lc_opening_conversion_fee"].Value = lc_conversion_fee.ToString("#,##0");

                // ** 유산스 인수수수료
                if (string.IsNullOrEmpty(txtUsanceFee.Text))
                    txtUsanceFee.Text = "0";
                total_usance_fee = Convert.ToDouble(txtUsanceFee.Text.Replace(",", ""));
                double usance_fee = total_usance_fee / total_paper_shipment_weight * box_weight;
                row.Cells["usance_underwriting_fee"].Value = usance_fee.ToString("#,##0");

                // ** 수입추심수수료
                if (string.IsNullOrEmpty(txtImportFee.Text))
                    txtImportFee.Text = "0";
                total_import_fee = Convert.ToDouble(txtImportFee.Text.Replace(",", ""));
                double import_fee = total_import_fee / total_paper_shipment_weight * box_weight;
                row.Cells["import_collection_fee"].Value = import_fee.ToString("#,##0");

                // ** 유산스 인수수수료
                if (string.IsNullOrEmpty(txtWithdrawalFee.Text))
                    txtWithdrawalFee.Text = "0";
                total_withdrawal_fee = Convert.ToDouble(txtWithdrawalFee.Text.Replace(",", ""));
                double withdrawal_fee = total_withdrawal_fee / total_paper_shipment_weight * box_weight;
                row.Cells["usance_withdrawal_acceptance_fee"].Value = withdrawal_fee.ToString("#,##0");

                // ** BU 할인료 + 환가료
                if (string.IsNullOrEmpty(txtBuDiscount.Text))
                    txtBuDiscount.Text = "0";
                total_bu_discount = Convert.ToDouble(txtBuDiscount.Text.Replace(",", ""));
                double bu_discount = total_bu_discount / total_paper_shipment_weight * box_weight;
                row.Cells["bu_discount"].Value = bu_discount.ToString("#,##0");

                // ** 은행 및 기타비용
                if (string.IsNullOrEmpty(txtBankOther.Text))
                    txtBankOther.Text = "0";
                total_bank_other = Convert.ToDouble(txtBankOther.Text.Replace(",", ""));
                double bank_other = total_bank_other / total_paper_shipment_weight * box_weight;
                row.Cells["banking_expenses"].Value = bank_other.ToString("#,##0");

                // ** 대행수수료
                if (string.IsNullOrEmpty(txtAgencyFee.Text))
                    txtAgencyFee.Text = "0";
                total_agency_fee = Convert.ToDouble(txtAgencyFee.Text.Replace(",", ""));
                double agency_fee = total_agency_fee / total_actual_shipment_weight * box_weight;
                row.Cells["agency_fee"].Value = agency_fee.ToString("#,##0");

                //2024-02-05 선적환율, 결제환율 둘다 계산하게 바꿈-====================================================
                //환율별 계산

                //환율
                double pay_eRate = Convert.ToDouble(row.Cells["payment_exchange_rate"].Value.ToString().Replace(",", ""));
                double ship_eRate = Convert.ToDouble(row.Cells["shipping_exchange_rate"].Value.ToString().Replace(",", ""));
                double custom_eRate = Convert.ToDouble(row.Cells["customs_clearance_exchange_rate"].Value.ToString().Replace(",", ""));

                //총수입액 USD
                invoice_amount_usd = unit_price * paper_shipment_weight;
                row.Cells["invoice_usd"].Value = invoice_amount_usd.ToString("#,##0.00");
                All_bank_settlement_amount_usd += invoice_amount_usd;

                //총수입액 KRW
                invoice_amount_krw = invoice_amount_usd * pay_eRate;                     //결제환율 총수입액
                All_bank_settlement_amount_krw += invoice_amount_krw;                    //전체 결제환율 총수입액
                shipping_invoice_amount_krw = invoice_amount_usd * ship_eRate;           //선적환율 총수입액
                All_shipping_bank_settlement_amount_krw += shipping_invoice_amount_krw;  //전체 선적환율 총수입액
                row.Cells["invoice_krw"].Value = shipping_invoice_amount_krw.ToString("#,##0");
                
                // ** 박스당 수입액
                double bank_settlement_amount = invoice_amount_krw / contract_qty;                       //결제환율
                double shipping_bank_settlement_amount = shipping_invoice_amount_krw / contract_qty;     //선적환율
                row.Cells["bank_settlement_amount"].Value = shipping_bank_settlement_amount.ToString("#,##0");

                // 관세율
                double tariff_rate = Convert.ToDouble(row.Cells["tariff_rate"].Value.ToString().Replace(",", "")) / 100;
                //박스당 가산금액
                if (string.IsNullOrEmpty(txtAdditionalAmount.Text))
                    txtAdditionalAmount.Text = "0";
                double total_add_amount = Convert.ToDouble(txtAdditionalAmount.Text.Replace(",", ""));
                double add_amount = total_add_amount / total_paper_shipment_weight * box_weight;
                row.Cells["additional_amount_per_box"].Value = add_amount.ToString("#,##0");
                //박스당 과세가격
                double settlement_per_box = ((invoice_amount_usd * custom_eRate) / contract_qty) + add_amount;
                //박스당 가산금액
                double total_addiyional_amount = 0;
                if (!double.TryParse(txtAdditionalAmount.Text, out total_addiyional_amount))
                    total_addiyional_amount = 0;
                double add_per_box = total_addiyional_amount / total_paper_shipment_weight * box_weight;

                //박스당 관세
                double custom_per_box = 0;
                double tmp;
                if (!double.TryParse(row.Cells["trq_tariff"].Value.ToString(), out tmp))
                    tmp = 0;
                //TRP 아닌 품목만 해당
                custom_per_box = tariff_rate * settlement_per_box;
                All_custom_per_box += custom_per_box * contract_qty;
                row.Cells["custom_per_box"].Value = custom_per_box.ToString("#,##0");

                // ** 부가가치세
                double vat_rate = Convert.ToDouble(row.Cells["vat_rate"].Value.ToString().Replace(",", "")) / 100;
                double vat = (settlement_per_box + custom_per_box) * vat_rate;
                row.Cells["vat"].Value = vat.ToString("#,##0");
                All_vat += vat * warehouse_qty;

                //결제환율 박스원가
                double box_price = bank_settlement_amount + custom_per_box + vat + logistic_cost + total_union_cost + refrigeration_charge + lc_open_charge + lc_telegraph_charge
                    + lc_conversion_fee + usance_fee + import_fee + withdrawal_fee + bu_discount + bank_other + agency_fee;
                //선적환율 박스원가
                double shipping_box_price = shipping_bank_settlement_amount + custom_per_box + vat + logistic_cost + total_union_cost + refrigeration_charge + lc_open_charge + lc_telegraph_charge
                    + lc_conversion_fee + usance_fee + import_fee + withdrawal_fee + bu_discount + bank_other + agency_fee;

                //박스원가(결제) 조정액
                double price_adjust;
                if (row.Cells["box_price_adjust"].Value == null || !double.TryParse(row.Cells["box_price_adjust"].Value.ToString(), out price_adjust))
                    price_adjust = 0;
                box_price += price_adjust;

                row.Cells["box_price"].Value = box_price.ToString("#,##0.000");
                row.Cells["box_price_copy"].Value = box_price.ToString("#,##0.000");

                //박스원가(선적) 조정액
                double shipping_price_adjust;
                if (row.Cells["shipping_box_price_adjust"].Value == null || !double.TryParse(row.Cells["shipping_box_price_adjust"].Value.ToString(), out shipping_price_adjust))
                    shipping_price_adjust = 0;
                shipping_box_price += shipping_price_adjust;
                row.Cells["shipping_box_price"].Value = shipping_box_price.ToString("#,##0.000");
                row.Cells["shipping_box_price_copy"].Value = shipping_box_price.ToString("#,##0.000");

                //사이즈별 금액합산
                row.Cells["total_amount_size"].Value = (box_price * contract_qty).ToString("#,##0");
                row.Cells["shipping_total_amount_size"].Value = (shipping_box_price * contract_qty).ToString("#,##0");

                //총 원가
                total_box_price += box_price;
                total_shipping_box_price += shipping_box_price;
                //사이즈별 금액합산
                total_amount_per_size += box_price * contract_qty;
                total_shipping_amount_per_size += shipping_box_price * contract_qty;
                //TRQ 박스당 최종원가
                double trq = Convert.ToDouble(row.Cells["trq_tariff"].Value.ToString().Replace(",", ""));
                double shipping_trq = Convert.ToDouble(row.Cells["shipping_trq_tariff"].Value.ToString().Replace(",", ""));
                double trq_box_price = 0, shipping_trq_box_price = 0;
                double total_trq = 0, shipping_total_trq = 0;
                if (trq > 0 || shipping_trq > 0)
                {
                    //TRQ 기타
                    double trq_other = lc_open_charge + lc_telegraph_charge + lc_conversion_fee + usance_fee + import_fee + withdrawal_fee + bu_discount + bank_other + agency_fee + total_union_cost + logistic_cost + vat;
                    //TRQ 박스원가 은행결제
                    double box_price_bank_payment = invoice_amount_krw / warehouse_qty;
                    double shipping_box_price_bank_payment = shipping_invoice_amount_krw / warehouse_qty;
                    //KG당 원가
                    double trq_price_per_kg = box_price_bank_payment / box_weight;
                    double shipping_trq_price_per_kg = shipping_box_price_bank_payment / box_weight;
                    //TRQ KG당 원가
                    double trq_price = trq_price_per_kg + trq;
                    double shipping_trq_price = shipping_trq_price_per_kg + shipping_trq;
                    //TRQ 박스당 원가
                    double trq_price_per_box = trq_price * box_weight;
                    double sthipping_trq_price_per_box = shipping_trq_price * box_weight;
                    //TRQ 박스 최종원가
                    trq_box_price = trq_price_per_box + trq_other;
                    shipping_trq_box_price = sthipping_trq_price_per_box + trq_other;

                    row.Cells["trq_after"].Value = trq_box_price.ToString("#,##0");
                    row.Cells["shipping_trq_after"].Value = shipping_trq_box_price.ToString("#,##0");

                    //TRQ 품목당 총금액
                    total_trq = trq_box_price * warehouse_qty;
                    shipping_total_trq = shipping_trq_box_price * warehouse_qty;
                }
                else
                {
                    total_trq = box_price * warehouse_qty;
                    shipping_total_trq = shipping_box_price * warehouse_qty;
                }
                row.Cells["trq_box_price"].Value = trq_box_price.ToString("#,##0.00");
                row.Cells["shipping_trq_box_price"].Value = shipping_trq_box_price.ToString("#,##0");
                row.Cells["trq_total_amount"].Value = total_trq.ToString("#,##0");
                row.Cells["shipping_trq_total_amount"].Value = shipping_total_trq.ToString("#,##0");
                total_trq_amount += total_trq;
                total_shipping_trq_amount += shipping_total_trq;
            }
            //총 박스원가 
            txtBoxPrice.Text = total_box_price.ToString("#,##0");
            txtShippingBoxPrice.Text = total_shipping_box_price.ToString("#,##0");
            txtBoxPriceDifference.Text = (total_shipping_box_price - total_box_price).ToString("#,##0");
            //총 원가
            double total_cost = (All_bank_settlement_amount_krw + All_custom_per_box + All_vat + total_logistic_cost + All_total_union_cost_amount
              + total_agency_fee + total_bank_other + total_bu_discount + total_withdrawal_fee + total_import_fee + total_usance_fee + total_lc_conversion_fee + total_lc_telegraph_charge + total_lc_open_charge);
            txtTotalCost.Text = total_cost.ToString("#,##0");
            double total_shipping_cost = (All_shipping_bank_settlement_amount_krw + All_custom_per_box + All_vat + total_logistic_cost + All_total_union_cost_amount
              + total_agency_fee + total_bank_other + total_bu_discount + total_withdrawal_fee + total_import_fee + total_usance_fee + total_lc_conversion_fee + total_lc_telegraph_charge + total_lc_open_charge);
            txtTotalShippingCost.Text =total_shipping_cost.ToString("#,##0");
            txtTotalCostDifference.Text = (total_shipping_cost - total_cost).ToString("#,##0");

            //선적중량
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["contract_weight"].Value = total_contract_weight.ToString("#,##0.00");
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["warehouse_weight"].Value = total_warehouse_weight.ToString("#,##0.00");
            //총수입액
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["invoice_krw"].Value = All_bank_settlement_amount_krw.ToString("#,##0");
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["invoice_usd"].Value = All_bank_settlement_amount_usd.ToString("#,##0.00");
            //박스당 관세
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["custom_per_box"].Value = All_custom_per_box.ToString("#,##0");
            //부가가치세
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["vat"].Value = All_vat.ToString("#,##0");

            //박스당 관세 + 부가가치세
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["custom_plus_tax"].Value = (All_custom_per_box + All_vat).ToString("#,##0");

            //전체 노조비
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["total_union_cost"].Value = All_total_union_cost_amount.ToString("#,##0");
            //사이즈별 금액합산
            txtTotalPricePerSize.Text = total_amount_per_size.ToString("#,##0");
            txtTotalShippingPricePerSize.Text = total_shipping_amount_per_size.ToString("#,##0");
            txtTotalPricePerSizeDifference.Text = (total_shipping_amount_per_size - total_amount_per_size).ToString("#,##0");
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["total_amount_size"].Value = total_shipping_amount_per_size.ToString("#,##0");
            //TRQ
            txtTotalTrqAmount.Text = total_trq_amount.ToString("#,##0");
            txtTotalShippingTrqAmount.Text = total_shipping_trq_amount.ToString("#,##0");
            txtTotalTrqAmountDifference.Text = (total_shipping_trq_amount - total_trq_amount).ToString("#,##0");
            dgvPending.Rows[dgvPending.Rows.Count - 1].Cells["trq_total_amount"].Value = total_shipping_trq_amount.ToString("#,##0");


            TotalCalculate();
            this.dgvPending.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPending_CellValueChanged);
            isCalculate = true;
        }
        #endregion

        #region 숫자만 입력 Event
        private void txtAdditionalAmount_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            double d;
            if (isCalculate && double.TryParse(tb.Text, out d))
            {
                //총 중량, 결제금액 계산
                CostingCalculate();
            }
        }
        private void dgvPending_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                double d;
                string txt = dgvPending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                if (double.TryParse(txt, out d))
                {
                    //Format 
                    if (dgvPending.Columns[e.ColumnIndex].Name == "box_weight"

                        || dgvPending.Columns[e.ColumnIndex].Name == "contract_qty"
                        || dgvPending.Columns[e.ColumnIndex].Name == "warehouse_qty"
                        || dgvPending.Columns[e.ColumnIndex].Name == "tariff_rate"
                        || dgvPending.Columns[e.ColumnIndex].Name == "vat_rate"
                        || dgvPending.Columns[e.ColumnIndex].Name == "payment_exchange_rate"
                        || dgvPending.Columns[e.ColumnIndex].Name == "customs_clearance_exchange_rate"
                        || dgvPending.Columns[e.ColumnIndex].Name == "loading_cost"
                        || dgvPending.Columns[e.ColumnIndex].Name == "refrigeration_charge"
                        || dgvPending.Columns[e.ColumnIndex].Name == "trq_tariff"
                        )
                    {
                        try
                        {
                            if (txt == "")
                            {
                                txt = "0";
                            }
                            txt = common.GetN2(txt);

                        }
                        catch (Exception ex)
                        {
                            txt = "0";
                        }
                        dgvPending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = txt;
                    }
                    //총 중량, 결제금액 계산
                    CostingCalculate();
                }
            }
        }
        private void txtPaymentExchangeRate_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //Format 
            try
            {
                string num;
                if (tb.Text != "")
                {
                    num = tb.Text;
                }
                else
                {
                    num = "0";
                }
                tb.Text = common.GetN2(num);
            }
            catch (Exception ex)
            {
                tb.Text = "0";
            }
        }
        //Datagridview 숫자만 입력
        private void dgvPending_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                dgvPending.EndEdit();
                if (dgvPending.Columns[e.ColumnIndex].Name == "weight_calculate" && e.RowIndex <= dgvPending.Rows.Count - 1)
                {
                    bool isChecked = Convert.ToBoolean(dgvPending.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    //중량단가 변환
                    if (isChecked)
                    {
                        if (MessageBox.Show(this,"단가를 박스중량 기준으로 변경하시겠습니까? ", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataGridViewRow row = dgvPending.Rows[e.RowIndex];
                            if (row.Cells["unit_price"].Value != null)
                            {
                                double unit_price;
                                if (!double.TryParse(row.Cells["unit_price"].Value.ToString(), out unit_price))
                                {
                                    unit_price = 0;
                                }
                                double box_weight;
                                if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                                {
                                    box_weight = 0;
                                }
                                double cost_unit;
                                if (!double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                                {
                                    cost_unit = 0;
                                }

                                double price = (unit_price * cost_unit) / box_weight;
                                row.Cells["unit_price"].Value = price;
                            }

                        }
                    }
                    //트레이단가 변환
                    else
                    {
                        if (MessageBox.Show(this,"단가를 트레이 기준으로 변경하시겠습니까? ", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            DataGridViewRow row = dgvPending.Rows[e.RowIndex];
                            if (row.Cells["unit_price"].Value != null)
                            {
                                double unit_price;
                                if (!double.TryParse(row.Cells["unit_price"].Value.ToString(), out unit_price))
                                {
                                    unit_price = 0;
                                }
                                double box_weight;
                                if (!double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                                {
                                    box_weight = 0;
                                }
                                double cost_unit;
                                if (!double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                                {
                                    cost_unit = 0;
                                }

                                double price = (unit_price * box_weight) / cost_unit;
                                row.Cells["unit_price"].Value = price;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Button Click
        private void btnCopy1_Click(object sender, EventArgs e)
        {
            string copyTxt = "";
            copyTxt += txtPaymentExchangeRate.Text;
            copyTxt += "\n" + txtCustomsClearanceExchangeRate.Text;
            copyTxt += "\n" + txtAdditionalAmount.Text;
            copyTxt += "\n" + txtLogisticsCost.Text;
            copyTxt += "\n" + txtLcOpenCharge.Text;
            copyTxt += "\n" + txtLcTelegraphCharge.Text;
            copyTxt += "\n" + txtLcConversionFee.Text;
            copyTxt += "\n" + txtUsanceFee.Text;
            copyTxt += "\n" + txtImportFee.Text;
            copyTxt += "\n" + txtWithdrawalFee.Text;
            copyTxt += "\n" + txtBuDiscount.Text;
            copyTxt += "\n" + txtBankOther.Text;
            copyTxt += "\n" + txtAgencyFee.Text;

            Clipboard.SetText(copyTxt);
        }
        private void btnDistribution3_Click(object sender, EventArgs e)
        {
            double total_adjust_amount;
            if (!double.TryParse(txtShippingContractMinusIncome.Text, out total_adjust_amount))
                total_adjust_amount = 0;

            if (total_adjust_amount > 0 && dgvPending.Rows.Count > 1)
            {
                //개당반영 금액
                double total_warehouse_qty = 0;
                for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                {
                    double warehouse_qty;
                    if (dgvPending.Rows[i].Cells["warehouse_qty"].Value == null || !double.TryParse(dgvPending.Rows[i].Cells["warehouse_qty"].Value.ToString(), out warehouse_qty))
                        warehouse_qty = 0;

                    total_warehouse_qty += warehouse_qty;
                }
                //출력
                for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                {
                    dgvPending.Rows[i].Cells["shipping_box_price_adjust"].Value = (total_adjust_amount / total_warehouse_qty);
                }
            }
        }
        private void btnDistribution_Click(object sender, EventArgs e)
        {
            double total_adjust_amount;
            if (!double.TryParse(txtContractMinusIncome.Text, out total_adjust_amount))
                total_adjust_amount = 0;

            if (total_adjust_amount > 0 && dgvPending.Rows.Count > 1)
            {
                //개당반영 금액
                double total_warehouse_qty = 0;
                for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                {
                    double warehouse_qty;
                    if (dgvPending.Rows[i].Cells["warehouse_qty"].Value == null || !double.TryParse(dgvPending.Rows[i].Cells["warehouse_qty"].Value.ToString(), out warehouse_qty))
                        warehouse_qty = 0;

                    total_warehouse_qty += warehouse_qty;
                }
                //출력
                for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                {
                    dgvPending.Rows[i].Cells["box_price_adjust"].Value = (total_adjust_amount / total_warehouse_qty);
                }
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
        private void btnPending_Click(object sender, EventArgs e)
        {
            //Messagebox
            if (MessageBox.Show(this,"통관내역으로 변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            btnInsert.PerformClick();
            int result = customsRepository.UpdateData(id.ToString(), "cc_status", "통관");
            if (result == -1)
            {
                MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                MessageBox.Show(this,"완료");
                this.Activate();
                this.Dispose();
            }
        }
        private void btnComfirm_Click(object sender, EventArgs e)
        {
            //Messagebox
            if (MessageBox.Show(this,"확정처리 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //Update
            btnInsert.PerformClick();
            int result = customsRepository.UpdateData(id.ToString(), "cc_status", "확정");
            if (result == -1)
            {
                MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                MessageBox.Show(this,"완료");
                this.Activate();
            }
        }
        private void btnUnding_Click(object sender, EventArgs e)
        {
            //Messagebox
            if (MessageBox.Show(this,"미통관내역으로 변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //Update
            btnInsert.PerformClick();
            int result = customsRepository.UpdateData(id.ToString(), "cc_status", "미통관");
            if (result == -1)
            {
                MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                MessageBox.Show(this,"완료");
                this.Activate();
                this.Dispose();
            }
        }

        //환율 가져오기
        private void btnGetExchangeRate1_Click(object sender, EventArgs e)
        {
            //하나은행 환율페이지
            System.Diagnostics.Process.Start("https://biz.kebhana.com/index.jsp");
        }
        //원가계산
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            CostingCalculate();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertPending();
        }

        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            UnPendingManager upm = new UnPendingManager(cal, um, "통관", id);
            upm.ShowDialog();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteExecute();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void txtSeaoverTotalAmount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double seaover_total_amount;
                if (!double.TryParse(txtSeaoverTotalAmount.Text, out seaover_total_amount))
                    seaover_total_amount = 0;

                double total_price_per_size;
                if (!double.TryParse(txtTotalPricePerSize.Text, out total_price_per_size))
                    total_price_per_size = 0;

                txtContractMinusIncome.Text = (seaover_total_amount - total_price_per_size).ToString("#,##0");
            }
        }
        private void nudFontsize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetFontSizes();
            }
        }

        private void dgvPending_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        common.GetDgvSelectCellsCapture(dgvPending);
                        break;
                }
            }
        }
        private void txtPaymentExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            //복사, 붙혀넣기 가능
            if ((e.KeyChar == 22 || e.KeyChar == 3) && Control.ModifierKeys == Keys.Control)
            {

            }
            //숫자만 입력되도록 필터링
            else if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)45 || e.KeyChar == (Char)46 || e.KeyChar == (Char)47))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
            else
            { 
                //이전값이 0일경우 삭제 후 입력
                Control con = (Control)sender;
                if (con.Text == "0")
                {
                    con.Text = "";
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            if (tb.Name != "dgvPending" && tb.Name != "dgvPending2")
            {
                switch (keyData)
                {
                    case Keys.Left:
                        return base.ProcessCmdKey(ref msg, keyData);
                    case Keys.Right:
                        return base.ProcessCmdKey(ref msg, keyData);
                    case Keys.Up:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Down:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Datagridview_KeyPress(object sender, KeyPressEventArgs e)
        {
            string name = dgvPending.CurrentCell.OwningColumn.Name;
            if (name != "bl_no" && name != "lc_open_date" && name != "cc_status" && name != "warehouse" && name != "custom_officer" && name != "origin"
                && name != "product" && name != "sizes" && name != "origin" && name != "weight_calculate")
            {
                if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)45 || e.KeyChar == (Char)46 || e.KeyChar == (Char)47))
                    e.Handled = true;
            }
        }
        private void dgvPending_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        Copy();
                        Delete();
                        break;
                    case Keys.C:
                        Copy();
                        break;
                    case Keys.V:
                        Paste();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        if (dgvPending.SelectedCells.Count > 0)
                        {
                            foreach (DataGridViewCell cell in dgvPending.SelectedCells)
                            {
                                cell.Value = string.Empty;
                            }
                        }
                        break;
                }
            }
        }
        private void PendingManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        {
                            CostingCalculate();
                            break;
                        }
                    case Keys.W:
                        {
                            this.Dispose();
                            UnPendingManager upm = new UnPendingManager(cal, um, "통관", id);
                            upm.ShowDialog();
                            break;
                        }
                    case Keys.A:
                        {
                            InsertPending();
                            break;
                        }
                    case Keys.D:
                        {
                            DeleteExecute();
                            break;
                        }
                    case Keys.M:
                        {
                            txtPaymentExchangeRate.Focus();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                /*switch (e.KeyCode)
                {
                    case Keys.Enter:
                        {
                            InsertProduct();
                            break;
                        }
                }*/
            }
            else
            {
            }
        }

        private void txtPaymentExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.KeyCode == Keys.Enter)
            {
                tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                
                if (tb.Name == "txtPaymentExchangeRate")
                {
                    if (MessageBox.Show(this, $"입력한 결제환율로 일괄변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (dgvPending.Rows.Count > 0)
                        {
                            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                                dgvPending.Rows[i].Cells["payment_exchange_rate"].Value = tb.Text;
                        }
                    }
                }
                else if (tb.Name == "txtShippingExchangeRate")
                {
                    if (MessageBox.Show(this, $"입력한 선적환율로 일괄변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (dgvPending.Rows.Count > 0)
                        {
                            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                                dgvPending.Rows[i].Cells["shipping_exchange_rate"].Value = tb.Text;
                        }
                    }
                }
                else if (tb.Name == "txtCustomsClearanceExchangeRate")
                {
                    if (MessageBox.Show(this,$"입력한 통관환율로 일괄변경 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (dgvPending.Rows.Count > 0)
                        {
                            for (int i = 0; i < dgvPending.Rows.Count - 1; i++)
                                dgvPending.Rows[i].Cells["customs_clearance_exchange_rate"].Value = tb.Text;
                        }
                    }
                }
            }
        }
        #endregion

        #region 복사/붙혀넣기
        private void Copy()
        {
            DataObject clipboardContent = dgvPending.GetClipboardContent();

            if (clipboardContent != null)
            {
                Clipboard.SetDataObject((object)clipboardContent);
            }
        }

        /// <summary>
        /// 선택한 셀을 삭제합니다.
        /// </summary>
        private void Delete()
        {
            foreach (DataGridViewCell oneCell in dgvPending.SelectedCells)
            {
                if (oneCell.Selected)
                    oneCell.Value = string.Empty;
            }
        }


        /// <summary>
        /// 붙여넣기 합니다.
        /// </summary>
        private void Paste()
        {
            if (dgvPending.CurrentCell != null)
            {
                string clipText = Clipboard.GetText();
                if (string.IsNullOrEmpty(clipText) == false)
                {
                    string[] lines = clipText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    string[] texts = lines[0].Split('\t');

                    int startRow = dgvPending.CurrentCell.RowIndex;
                    int startCol = dgvPending.CurrentCell.ColumnIndex;

                    int row = startRow;
                    // Multi rows, Multi columns
                    if (lines.Length > 1 && texts.Length > 1)
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            texts = lines[i].Split('\t');

                            int col = startCol;
                            for (int j = 0; j < texts.Length; j++)
                            {
                                if (dgvPending.RowCount <= row || dgvPending.ColumnCount <= col)
                                    break;

                                string txt = texts[j];
                                dgvPending[col, row].Value = txt;
                                col++;
                            }
                            row++;
                        }
                    }
                    // one row, Multi columns
                    if (lines.Length == 1 && texts.Length > 1)
                    {
                        for (int i = 0; i < lines.Length; i++)
                        {
                            foreach (DataGridViewCell cell in dgvPending.SelectedCells)
                            {
                                row = cell.RowIndex;
                                texts = lines[i].Split('\t');

                                int col = startCol;
                                for (int j = 0; j < texts.Length; j++)
                                {
                                    if (dgvPending.RowCount <= row || dgvPending.ColumnCount <= col)
                                        break;

                                    string txt = texts[j];
                                    dgvPending[col, row].Value = txt;
                                    col++;
                                }
                            }
                        }
                        // one row, Multi columns
                        if (lines.Length > 1 && texts.Length == 1)
                        {
                            for (int i = 0; i < lines.Length; i++)
                            {
                                foreach (DataGridViewCell cell in dgvPending.SelectedCells)
                                {
                                    row = cell.RowIndex;
                                    texts = lines[i].Split('\t');

                                    int col = startCol;
                                    for (int j = 0; j < texts.Length; j++)
                                    {
                                        if (dgvPending.RowCount <= row || dgvPending.ColumnCount <= col)
                                            break;

                                        string txt = texts[j];
                                        dgvPending[col, row].Value = txt;
                                        col++;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string txt = lines[0];
                        foreach (DataGridViewCell cell in dgvPending.SelectedCells)
                        {
                            cell.Value = txt;
                        }
                    }
                }
            }
        }




















        #endregion

       
    }
}
