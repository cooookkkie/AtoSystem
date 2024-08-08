using AdoNetWindow.Model;
using AdoNetWindow.Product;
using Repositories;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdoNetWindow.Pending
{
    public partial class UnpendingAddManager : Form
    {
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        ICommonRepository commonRepository = new CommonRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        UnconfirmedPending pendingList;
        public UnpendingAddManager(UsersModel uModel, UnconfirmedPending ufpendingList = null)
        {
            InitializeComponent();
            pendingList = ufpendingList;
            um = uModel;
            txtManager.Text = uModel.user_name;
            nudContractYear.Minimum = DateTime.Now.Year - 20;
            nudContractYear.Maximum = DateTime.Now.Year + 20;
            nudContractYear.Value = DateTime.Now.Year;
            SetDgvStyleSetting();
        }

        public UnpendingAddManager(UsersModel uModel, string type, int id, UnconfirmedPending ufpendingList = null)
        {
            InitializeComponent();
            pendingList = ufpendingList;
            um = uModel;
            nudContractYear.Minimum = DateTime.Now.Year - 20;
            nudContractYear.Maximum = DateTime.Now.Year + 20;
            nudContractYear.Value = DateTime.Now.Year;

            //Call Unpending Data
            DataGridView dgv = dgvProduct;
            List<AllCustomsModel> model = customsRepository.GetAllTypePending(type, id.ToString(), true);
            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    int n = dgv.Rows.Add();
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
                    dgv.Rows[n].Cells["tariff_rate"].Value = model[i].tariff_rate * 100;
                    dgv.Rows[n].Cells["vat_rate"].Value = model[i].vat_rate * 100;

                    cbCalendar.Checked = model[i].is_calendar;
                    cbShipmentQty.Checked = model[i].is_shipment_qty;

                    nudContractYear.Value = Convert.ToInt16(model[i].contract_year);
                    txtAtono.Text = model[i].ato_no;
                    txtPidate.Text = model[i].pi_date;
                    txtContractno.Text = model[i].contract_no;
                    txtShipper.Text = model[i].shipper;
                    txtLcopendate.Text = model[i].lc_open_date;
                    txtLcno.Text = model[i].lc_payment_date;
                    txtBlno.Text = model[i].bl_no;
                    txtShipmentdate.Text = model[i].shipment_date;
                    txtEtd.Text = model[i].etd;
                    txtEta.Text = model[i].eta;
                    txtWarehousedate.Text = model[i].warehousing_date;
                    txtPendingdate.Text = model[i].pending_check;
                    cbCcstatus.Text = model[i].cc_status;
                    txtCustomofficer.Text = model[i].broker;
                    txtWarehouse.Text = model[i].warehouse;
                    if (!string.IsNullOrEmpty(model[i].payment_date))
                    { 
                        txtPaymentdate.Text = Convert.ToDateTime(model[i].payment_date).ToString("yyyy-MM-dd");
                    }
                    cbPaydatestatus.Text = model[i].payment_date_status;
                    cbPaybank.Text = model[i].payment_bank;
                    if (model[i].usance_type == "O")
                    {
                        cbUsance.Text = "US";
                    }
                    else if (model[i].usance_type == "X")
                    {
                        cbUsance.Text = "";
                    }
                    else
                    { 
                        cbUsance.Text = model[i].usance_type; 
                    }

                    cbAgency.Text = model[i].agency_type;
                    cbDivision.Text = model[i].division;
                    cbSanitaryCertificate.Text = model[i].sanitary_certificate;
                    txtManager.Text = model[i].manager;
                    txtImportNumber.Text = model[i].import_number;
                }
                Calculate();
            }
        }

        private void UnpendingAddManager_Load(object sender, EventArgs e)
        {
            nudContractYear.Minimum = DateTime.Now.Year - 20;
            nudContractYear.Maximum = DateTime.Now.Year + 20;
            CallProcedure();
            dgvProduct.Init();
            //Style setting
            SetDgvStyleSetting();
        }

        #region Key Event

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            if (tb.Name != "dgvProduct")
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

        private void cbCcstatus_KeyDown(object sender, KeyEventArgs e)
        {
            Control tb = (Control)sender;
            //ato no 체크
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        if (tb.Name == "txtAtono")
                        {
                            if (!string.IsNullOrEmpty(txtAtono.Text) && txtAtono.Text.Length >= 2)
                            {
                                int no = customsRepository.GetNextAtoNo(txtAtono.Text);
                                txtAtono.Text = txtAtono.Text + no.ToString("00");
                            }
                        }
                        break;
                }
            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                    //날짜 완성
                    Control tbb = (Control)sender;
                    tbb.Text = common.strDatetime(tbb.Text);

                    /*if (tbb.Text.Contains("/"))
                    {  
                        if (!string.IsNullOrEmpty(tbb.Text))
                        {
                            DateTime dt;
                            if (DateTime.TryParse(tbb.Text, out dt))
                            {
                                tbb.Text = dt.ToString("yyyy-MM-dd");
                            }
                        }
                    }*/
                }
            }
        }
        private void UnpendingAddManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        {
                            InsertExecute();
                            break;
                        }
                    case Keys.D:
                        {
                            ProductDelete();
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
                if (e.KeyCode == Keys.F2)
                {
                    txtAtono.Focus();
                }
                else if (e.KeyCode == Keys.F4)
                {
                    btnGetProduct.PerformClick();
                }
                else if (e.KeyCode == Keys.F5)
                {
                    Refresh();
                }
                else if (e.KeyCode == Keys.F9)
                {
                    Company.CompanyManager cm = new Company.CompanyManager(um, true);
                    string company = cm.GetCompany();
                    if (company != null)
                    {
                        txtShipper.Text = company;
                        
                    }
                    txtShipper.Focus();
                }
            }
        }
        #endregion

        #region Button Click
        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                bool isSelect = !Convert.ToBoolean(dgvProduct.Rows[0].Cells["cbDelete"].Value);
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    dgvProduct.Rows[i].Cells["cbDelete"].Value = isSelect;
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            ProductDelete();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            //CreatePendingFolder(); 
            InsertExecute();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnPidate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtPidate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnLcopendateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtLcopendate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnShipmentdateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtShipmentdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEtdCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEtd.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEtaCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEta.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnWarehousedateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtWarehousedate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnPendingDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtPendingdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnPaymentDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtPaymentdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnGetProduct_Click(object sender, EventArgs e)
        {
            ProductManager ps = new ProductManager(um, this, true);

            // 부모 Form의 좌표, 크기를 계산
            int mainformX = this.Location.X;
            int mainformY = this.Location.Y;
            int mainfromWidth = this.Size.Width;
            int mainfromHeight = this.Size.Height;

            // 자식 Form의 크기를 계산
            int childformwidth = ps.Size.Width;
            int childformheight = ps.Size.Height;
            Point p = new Point(mainformX + (mainfromWidth / 2) - (childformwidth / 2), mainformY + (mainfromHeight / 2) - (childformheight / 2));

            ps.ShowDialog();
        }
        #endregion

        #region Method
        public void AddProduct(List<string[]> selectProduct, bool isSetChart = false)
        {
            if (selectProduct != null && selectProduct.Count > 0)
            {
                for (int i = 0; i < selectProduct.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["product"].Value = selectProduct[i][0];
                    dgvProduct.Rows[n].Cells["origin"].Value = selectProduct[i][1];
                    dgvProduct.Rows[n].Cells["sizes"].Value = selectProduct[i][2];
                    dgvProduct.Rows[n].Cells["box_weight"].Value = selectProduct[i][3];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = selectProduct[i][4];
                    dgvProduct.Rows[n].Cells["vat_rate"].Value = selectProduct[i][5];
                    dgvProduct.Rows[n].Cells["tariff_rate"].Value = selectProduct[i][6];

                    dgvProduct.Rows[n].Cells["weight_calculate"].Value = Convert.ToBoolean(selectProduct[i][12]);

                    dgvProduct.Rows[n].Cells["unit_price"].Value = 0;
                    dgvProduct.Rows[n].Cells["contract_qty"].Value = 0;
                    dgvProduct.Rows[n].Cells["warehouse_qty"].Value = 0;
                }
            }
        }
        private void Calculate()
        {
            double weight = 0;
            double payment = 0;
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            dgvProduct.EndEdit();
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    double contract_qty;
                    if (dgvProduct.Rows[i].Cells["contract_qty"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["contract_qty"].Value.ToString(), out contract_qty))
                        contract_qty = 0;

                    double box_weight;
                    if (dgvProduct.Rows[i].Cells["box_weight"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["box_weight"].Value.ToString(), out box_weight))
                        box_weight = 0;

                    double unit_price;
                    if (dgvProduct.Rows[i].Cells["unit_price"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["unit_price"].Value.ToString(), out unit_price))
                        unit_price = 0;

                    double cost_unit;
                    if (dgvProduct.Rows[i].Cells["cost_unit"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;

                    bool weight_calculate;
                    if (dgvProduct.Rows[i].Cells["weight_calculate"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                        weight_calculate = true;

                    if(!weight_calculate && cost_unit == 0)
                        weight_calculate = true;

                    if (!weight_calculate)
                    {
                        weight += cost_unit * contract_qty;
                        payment += cost_unit * contract_qty * unit_price;
                    }
                    else
                    {
                        weight += box_weight * contract_qty;
                        payment += box_weight * contract_qty * unit_price;
                    }
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            txtTotalWeight.Text = weight.ToString("#,##0.00");
            txtTotalPayment.Text = payment.ToString("#,##0.00");
        }
        //SEAOVER 프로시져 호출하기
        private void CallProcedure()
        {
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string seaover_id = um.seaover_id;
            //업체별시세현황 스토어프로시져 호출
            if (seaoverRepository.CallStoredProcedure(seaover_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this,"호출 내용이 없음");
                this.Activate();
                return;
            }
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void InsertExecute()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "팬딩관리", "팬딩 등록", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            dgvProduct.EndEdit();
            //유효성검사
            if (!Validation())
                return;
            //Messagebox
            if (MessageBox.Show(this,"등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //Sql Execute
            if (dgvProduct.Rows.Count > 0)
            {
                int id = customsRepository.GetNextId();
                int sub_id = 1;
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql;

                //등록
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    AllCustomsModel model = new AllCustomsModel();
                    model.id = id;
                    model.sub_id = sub_id;
                    model.contract_year = Convert.ToInt16(nudContractYear.Value);
                    model.ato_no = txtAtono.Text;
                    model.pi_date = txtPidate.Text;
                    model.contract_no = txtContractno.Text;
                    model.shipper = txtShipper.Text;
                    model.lc_open_date = txtLcopendate.Text;
                    model.lc_payment_date = txtLcno.Text;    //lc no
                    model.bl_no = txtBlno.Text;
                    model.shipment_date = txtShipmentdate.Text;
                    model.etd = txtEtd.Text;
                    model.eta = txtEta.Text;
                    model.warehousing_date = txtWarehousedate.Text;
                    model.pending_check = txtPendingdate.Text;
                    model.cc_status = cbCcstatus.Text;
                    model.broker = txtCustomofficer.Text;
                    model.warehouse = txtWarehouse.Text;
                    model.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                    model.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                    model.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                    model.box_weight = dgvProduct.Rows[i].Cells["box_weight"].Value.ToString();
                    model.cost_unit = dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString();
                    bool weight_calculate;
                    if (dgvProduct.Rows[i].Cells["weight_calculate"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                        weight_calculate = true;
                    model.weight_calculate = weight_calculate;

                    double unit_price;
                    if (dgvProduct.Rows[i].Cells["unit_price"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["unit_price"].Value.ToString(), out unit_price))
                        unit_price = 0;
                    double contract_qty;
                    if (dgvProduct.Rows[i].Cells["contract_qty"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["contract_qty"].Value.ToString(), out contract_qty))
                        contract_qty = 0;
                    double warehouse_qty;
                    if (dgvProduct.Rows[i].Cells["warehouse_qty"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["warehouse_qty"].Value.ToString(), out warehouse_qty))
                        warehouse_qty = 0;
                    double tariff_rate;
                    if (dgvProduct.Rows[i].Cells["tariff_rate"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["tariff_rate"].Value.ToString(), out tariff_rate))
                        tariff_rate = 0;
                    double vat_rate;
                    if (dgvProduct.Rows[i].Cells["vat_rate"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["vat_rate"].Value.ToString(), out vat_rate))
                        vat_rate = 0;


                    model.unit_price = unit_price;
                    model.quantity_on_paper = contract_qty;
                    model.qty = warehouse_qty;
                    model.tariff_rate = tariff_rate / 100;
                    model.vat_rate = vat_rate / 100;

                    model.payment_date = txtPaymentdate.Text;
                    model.payment_date_status = cbPaydatestatus.Text;
                    model.payment_bank = cbPaybank.Text;
                    model.usance_type = cbUsance.Text;
                    model.agency_type = cbAgency.Text;
                    model.division = cbDivision.Text;
                    model.manager = txtManager.Text;
                    model.sanitary_certificate = cbSanitaryCertificate.Text;
                    model.edit_user = um.user_name;
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                    //원가계산에서 입력되는 값
                    model.remark = "";
                    model.loading_cost_per_box = 0;
                    model.refrigeration_charge = 0;
                    model.total_amount_seaover = "";
                    model.trq_amount = 0;
                    model.clearance_rate = 0;

                    model.is_calendar = cbCalendar.Checked;
                    model.is_shipment_qty = cbShipmentQty.Checked;

                    model.import_number = txtImportNumber.Text;
                    model.is_quarantine = false;

                    sql = customsRepository.InsertSql2(model);
                    sqlList.Add(sql);
                    sub_id += 1;
                }

                int result = customsRepository.UpdateCustomTran(sqlList);
                if (result == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    //서류폴더 생성
                    string ato_no = txtAtono.Text;
                    string[] company = txtShipper.Text.Trim().Split(' ');
                    string folder_path;

                    int d;
                    //3글자 아토번호
                    if (!int.TryParse(ato_no.Substring(2, 1), out d))
                        folder_path = nudContractYear.Value.ToString() + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 3) + "/" + ato_no + " " + ftp.ReplaceName(dgvProduct.Rows[0].Cells["product"].Value.ToString()) + "(" + company[0] + ")";
                    //2글자 아토번호
                    else
                        folder_path = nudContractYear.Value.ToString() + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 2) + "/" + ato_no + " " + ftp.ReplaceName(dgvProduct.Rows[0].Cells["product"].Value.ToString()) + "(" + company[0] + ")";
                    //팬딩 서류폴더 생성
                    MakeTradeDocumentFolder(folder_path, "ATO/아토무역/무역/무역1/ㄴ.수입자료/서류");

                    //박스디자인 서류폴더 생성
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        string product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                        string origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();

                        product = product.Replace(@"\", "");
                        product = product.Replace(@"/", "");
                        product = product.Replace(@":", "");
                        product = product.Replace(@"*", "");
                        product = product.Replace(@"?", "");
                        product = product.Replace(@"<", "");
                        product = product.Replace(@">", "");
                        product = product.Replace(@".", ",");

                        origin = origin.Replace(@"\", "");
                        origin = origin.Replace(@"/", "");
                        origin = origin.Replace(@":", "");
                        origin = origin.Replace(@"*", "");
                        origin = origin.Replace(@"?", "");
                        origin = origin.Replace(@"<", "");
                        origin = origin.Replace(@">", "");
                        origin = origin.Replace(@".", ",");

                        string manager = txtManager.Text;

                        folder_path = manager + "/" + product + "/" + origin;

                        MakeBoxDsignDocumentFolder(folder_path, @"ATO/아토무역/무역/무역1/ㄴ.수입자료/박스디자인");
                    }

                    MessageBox.Show(this, "등록완료");
                    this.Activate();
                    Refresh();
                }
            }
            else
            {
                MessageBox.Show(this, "선택된 품목이 없습니다.");
                this.Activate();
            }
        }

        Libs.ftpCommon ftp = new Libs.ftpCommon();
        private void MakeDocumentFolder(string folder_path, string root_path = "Solution/Document")
        {
            //기본 아토번호 폴더 생성     
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                string product_folder_path = folder_path + "/" + ftp.ReplaceName(dgvProduct.Rows[i].Cells["product"].Value.ToString())
                    + "/" + ftp.ReplaceName(dgvProduct.Rows[i].Cells["origin"].Value.ToString())
                    + "/" + ftp.ReplaceName(dgvProduct.Rows[i].Cells["sizes"].Value.ToString());
                if (!ftp.CheckDirectory(product_folder_path, true, root_path))
                {
                    MessageBox.Show(this, "서류폴더 생성중 에러가 발생하였습니다.");
                    this.Activate();
                }
            }
        }
        private void MakeTradeDocumentFolder(string folder_path, string root_path = "Solution/Document")
        {
            //기본 아토번호 폴더 생성     
            if (!ftp.CheckDirectory(folder_path, true, root_path))
            {
                MessageBox.Show(this, "서류폴더 생성중 에러가 발생하였습니다.");
                this.Activate();
            }
        }

        private void MakeBoxDsignDocumentFolder(string folder_path, string root_path = "Solution/Document")
        {
            //기본 아토번호 폴더 생성     
            if (!ftp.CheckDirectory(folder_path, true, root_path))
            {
                MessageBox.Show(this, "서류폴더 생성중 에러가 발생하였습니다.");
                this.Activate();
            }
        }


        private void SetDgvStyleSetting()
        {
            DataGridView dgv = dgvProduct;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["box_weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["unit_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["contract_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["warehouse_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["tariff_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["vat_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private bool Validation(bool isInsert = true)
        {
            dgvProduct.EndEdit();
            bool isVal = false;
            if (string.IsNullOrEmpty(txtAtono.Text.Trim()))
            {
                MessageBox.Show(this,"Ato No.를 입력해주세요.");
                this.Activate();
                return isVal;
            }
            else if (txtAtono.Text.Trim().Length < 3)
            {
                MessageBox.Show(this,"Ato No는 최소 3글자 이상 사용하셔야합니다.");
                this.Activate();
                return isVal;
            }
            if (!string.IsNullOrEmpty(txtPidate.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtPidate.Text))
                {
                    txtPidate.Focus();
                    MessageBox.Show(this,"Pi date 값이 날짜형식이 아닙니다.");
                    this.Activate();
                    return isVal;
                }
            }
            string contract_year = nudContractYear.Value.ToString().Trim();
            string ato_no = txtAtono.Text.Trim();
            DataTable atoNoDt = customsRepository.GetDuplicateAtono(contract_year, ato_no);
            if (atoNoDt.Rows.Count > 0)
            {
                for (int i = 0; i < atoNoDt.Rows.Count; i++)
                {
                    //완전 같은내역 존재
                    if (atoNoDt.Rows[i]["ato_no"].ToString().Equals(ato_no))
                    {
                        MessageBox.Show(this,"이미 중복된 Ato No가 존재합니다!");
                        this.Activate();
                        return isVal;
                    }
                }
            }
            /*if (!string.IsNullOrEmpty(txtLcopendate.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtLcopendate.Text))
                {
                    txtLcopendate.Focus();
                    MessageBox.Show(this,"L/C open date 값이 날짜형식이 아닙니다.");
                    return isVal;
                }
            }*/
            if (string.IsNullOrEmpty(txtShipmentdate.Text))
            {
                txtShipmentdate.Focus();
                MessageBox.Show(this,"계약선적일 값을 입력해주세요.");
                this.Activate();
                return isVal;
            }
            else if (!string.IsNullOrEmpty(txtShipmentdate.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtShipmentdate.Text))
                {
                    txtShipmentdate.Focus();
                    MessageBox.Show(this,"계약선적일 값이 날짜형식이 아닙니다.");
                    this.Activate();
                    return isVal;
                }
            }
            if (!string.IsNullOrEmpty(txtEtd.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtEtd.Text))
                {
                    txtEtd.Focus();
                    MessageBox.Show(this,"ETD 값이 날짜형식이 아닙니다.");
                    this.Activate();
                    return isVal;
                }
            }
            if (!string.IsNullOrEmpty(txtEta.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtEta.Text))
                {
                    txtEta.Focus();
                    MessageBox.Show(this,"ETA 값이 날짜형식이 아닙니다.");
                    this.Activate();
                    return isVal;
                }
            }
            if (!string.IsNullOrEmpty(txtWarehousedate.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtWarehousedate.Text))
                {
                    txtWarehousedate.Focus();
                    MessageBox.Show(this,"창고입고 예정일 값이 날짜형식이 아닙니다.");
                    this.Activate();
                    return isVal;
                }
            }
            if (!string.IsNullOrEmpty(txtPendingdate.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtPendingdate.Text))
                {
                    txtPendingdate.Focus();
                    MessageBox.Show(this,"통관 예정일 값이 날짜형식이 아닙니다.");
                    this.Activate();
                    return isVal;
                }
            }
            if (!string.IsNullOrEmpty(txtPendingdate.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtPendingdate.Text))
                {
                    txtPendingdate.Focus();
                    MessageBox.Show(this,"통관 예정일 값이 날짜형식이 아닙니다.");
                    this.Activate();
                    return isVal;
                }
            }
            if (!string.IsNullOrEmpty(txtPaymentdate.Text))
            {
                if (!Libs.Tools.Common.isDatetime(txtPaymentdate.Text))
                {
                    txtPaymentdate.Focus();
                    MessageBox.Show(this,"결제일 값이 날짜형식이 아닙니다.");
                    this.Activate();
                    return isVal;
                }
            }
            if (string.IsNullOrEmpty(txtManager.Text))
            {
                txtManager.Focus();
                MessageBox.Show(this, "담당자를 입력해주세요.");
                this.Activate();
                return isVal;
            }
            //품목
            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this,"선택된 품목이 없습니다.");
                this.Activate();
                return isVal;
            }

            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                double cost_unit;
                if (dgvProduct.Rows[i].Cells["cost_unit"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString(), out cost_unit))
                    cost_unit = 0;

                bool weight_calculate;
                if (dgvProduct.Rows[i].Cells["weight_calculate"].Value == null || !bool.TryParse(dgvProduct.Rows[i].Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                    weight_calculate = true;

                if (!weight_calculate && cost_unit == 0)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.Rows[i].Selected = true;
                    MessageBox.Show(this,"트레이 계산법에 트레이값이 입력되지 않았습니다. 중량계산법으로 변경하거나 트레이를 입력해주시기 바랍니다.");
                    this.Activate();
                    return isVal;
                }
            }

            isVal = true;
            return isVal;
        }
        private void Refresh()
        {
            nudContractYear.Value = DateTime.Now.Year;
            txtPidate.Text = "";
            txtContractno.Text = "";
            txtAtono.Text = "";
            txtPidate.Text = "";
            txtShipper.Text = "";
            txtLcopendate.Text = "";
            txtLcno.Text = "";
            txtBlno.Text = "";
            txtShipmentdate.Text = "";
            txtEtd.Text = "";
            txtEta.Text = "";
            txtWarehousedate.Text = "";
            txtPendingdate.Text = "";
            cbCcstatus.Text = "미통관";
            cbAgency.Text = "";
            txtWarehouse.Text = "";
            txtPaymentdate.Text = "";
            txtCustomofficer.Text = "";
            cbPaybank.Text = "";
            cbPaydatestatus.Text = "";
            cbUsance.Text = "";
            cbDivision.Text = "";
            cbAgency.Text = "";
            cbSanitaryCertificate.Text = "";
            dgvProduct.Rows.Clear();


        }

        private void ProductDelete()
        {
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = dgvProduct.Rows.Count - 1; i >= 0; i--)
                {
                    var cbxCell = (DataGridViewCheckBoxCell)dgvProduct.Rows[i].Cells["cbDelete"];
                    //선택했을때만
                    if (cbxCell.Value != null && (bool)cbxCell.Value)
                    {
                        dgvProduct.Rows.Remove(dgvProduct.Rows[i]);
                    }
                }
            }
        }
        public void SetNewProduct(string[] product)
        {
            DataGridView dgv = dgvProduct;
            int n = dgv.Rows.Add();

            dgv.Rows[n].Cells["product"].Value = product[0];
            dgv.Rows[n].Cells["origin"].Value = product[1];
            dgv.Rows[n].Cells["sizes"].Value = product[2];
            dgv.Rows[n].Cells["box_weight"].Value = product[3];
            dgv.Rows[n].Cells["unit_price"].Value = product[4];
            dgv.Rows[n].Cells["contract_qty"].Value = product[5];
            dgv.Rows[n].Cells["warehouse_qty"].Value = product[6];
            dgv.Rows[n].Cells["tariff_rate"].Value = product[7];
            dgv.Rows[n].Cells["vat_rate"].Value = product[8];
        }

        #endregion

        #region Datagridview event
        private void dgvProduct_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // If the data source raises an exception when a cell value is
            // commited, display an error message.
            if (e.Exception != null &&
                e.Context == DataGridViewDataErrorContexts.Commit)
            {
                //MessageBox.Show(this,"CustomerID value must be unique.");
            }
        }
        private void dgvProduct_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            string name = dgvProduct.CurrentCell.OwningColumn.Name;

            if (name == "contract_qty" || name == "warehouse_qty" || name == "unit_price" || name == "box_weight" || name == "tariff_rate" || name == "vat_rate")
            {
                e.Control.KeyPress += new KeyPressEventHandler(txtNumberic_KeyPress);
            }
            else
            {
                e.Control.KeyPress -= new KeyPressEventHandler(txtNumberic_KeyPress);
            }

        }
        private void txtNumberic_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar == 22 || e.KeyChar == 3) && Control.ModifierKeys == Keys.Control)
            {

            }
            //숫자만 입력되도록 필터링
            else if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(46) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))
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
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "weight_calculate")
                {
                    double cost_unit;
                    if (dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value == null || !double.TryParse(dgvProduct.Rows[e.RowIndex].Cells["cost_unit"].Value.ToString(), out cost_unit))
                        cost_unit = 0;

                    bool weight_calculate;
                    if (dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value == null || !bool.TryParse(dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                        weight_calculate = true;

                    if (!weight_calculate && cost_unit == 0)
                    {
                        this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                        messageBox.Show(this, "트레이 값이 0입니다!");
                        dgvProduct.Rows[e.RowIndex].Cells["weight_calculate"].Value = true;
                        this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    }
                }
                Calculate();
            }
        }
        #endregion

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvProduct.Columns[e.ColumnIndex].Name == "weight_calculate")
                dgvProduct.EndEdit();
        }
    }
}
