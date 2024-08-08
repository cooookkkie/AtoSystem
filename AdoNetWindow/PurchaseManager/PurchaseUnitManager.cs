using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AdoNetWindow.PurchaseManager
{
    public partial class PurchaseUnitManager : Form
    {
        GraphManager graph;
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        ICommonRepository commonRepository = new CommonRepository();
        PurchaseManager.CostAccounting ca = null;
        Libs.Tools.Common common = new Libs.Tools.Common();
        bool isPasteRow = false;
        UsersModel um;
        public PurchaseUnitManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }

        public PurchaseUnitManager(UsersModel uModel, string enddate, string product, string origin, string sizes, string unit)
        {
            InitializeComponent();
            um = uModel;
            if (string.IsNullOrEmpty(enddate))
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            txtSttdate.Text = Convert.ToDateTime(enddate).AddDays(-7).ToString("yyyy-MM-dd");
            txtEnddate.Text = enddate;
            txtProduct.Text = product;
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtUnit.Text = unit;

            GetData();
        }

        public PurchaseUnitManager(UsersModel uModel, string sttdate, string enddate, string product, string origin, string sizes, string unit)
        {
            InitializeComponent();
            um = uModel;
            if (string.IsNullOrEmpty(enddate))
                enddate = DateTime.Now.ToString("yyyy-MM-dd");
            if (string.IsNullOrEmpty(sttdate))
                sttdate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            txtSttdate.Text = sttdate;
            txtEnddate.Text = enddate;
            txtProduct.Text = product;
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtUnit.Text = unit;

            GetData();
        }

        private void PurchaseUnitManager_Load(object sender, EventArgs e)
        {
            SetDgvHeaderSetting();
            txtSttdate.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        #region Method
        public void SearchProduct(string product, string origin, string sizes, string unit, string company)
        {
            txtProduct.Text = product;
            txtOrigin.Text = origin;
            txtSizes.Text = sizes;
            txtUnit.Text = unit;
            txtCompany.Text = company;
            cbIsExactly.Checked = true;
            GetData();
        }
        private void SetDgvHeaderSetting()
        {
            DataGridView dgv = dgvProduct;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dgv.Columns["id"].Visible = false;

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["purchase_price"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["purchase_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["purchase_price"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["purchase_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["custom"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["tax"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["incidental_expense"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgv.Columns["is_private"].Visible = false;
            dgv.Columns["tray_calculate"].Visible = false;
        }
        private void UpdatePurchasePrice()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "거래처별 매입단가 조회", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (MessageBox.Show(this,"선택한 내역들의 단가를 수정하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Sql Execute
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql;

                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        //Delete
                        sql = purchasePriceRepository.DeletePurchasePrice(dgvProduct.Rows[i].Cells["id"].Value.ToString());
                        sqlList.Add(sql);
                        //Insert
                        PurchasePriceModel ppm = new PurchasePriceModel();
                        ppm.id = Convert.ToInt32(dgvProduct.Rows[i].Cells["id"].Value.ToString());
                        ppm.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                        ppm.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                        ppm.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                        ppm.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                        ppm.cost_unit = dgvProduct.Rows[i].Cells["cost_unit"].Value.ToString();
                        ppm.purchase_price = Convert.ToDouble(dgvProduct.Rows[i].Cells["purchase_price"].Value.ToString());
                        ppm.updatetime = Convert.ToDateTime(dgvProduct.Rows[i].Cells["updatetime"].Value.ToString()).ToString("yyyy-MM-dd");
                        ppm.edit_user = dgvProduct.Rows[i].Cells["edit_user"].Value.ToString();

                        ppm.weight_calculate = Convert.ToBoolean(dgvProduct.Rows[i].Cells["weight_calculate"].Value.ToString());
                        ppm.is_FOB = Convert.ToBoolean(dgvProduct.Rows[i].Cells["is_FOB"].Value.ToString());

                        double fixed_tariff;
                        if (dgvProduct.Rows[i].Cells["fixed_tariff"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["fixed_tariff"].Value.ToString(), out fixed_tariff))
                            fixed_tariff = 0;
                        ppm.fixed_tariff = fixed_tariff;

                        CompanyModel cModel = new CompanyModel();
                        cModel = companyRepository.GetCompanyAsOne2(dgvProduct.Rows[i].Cells["company"].Value.ToString());
                        ppm.company = cModel.id.ToString();

                        sql = purchasePriceRepository.InsertPurchasePrice(ppm);
                        sqlList.Add(sql);
                    }
                }
                //Transaction execute
                if (sqlList.Count > 0)
                {
                    //Execute
                    int result = customsRepository.UpdateCustomTran(sqlList);
                    if (result == -1)
                    {
                        MessageBox.Show(this, "수정 중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        MessageBox.Show(this, "수정완료");
                        this.Activate();
                    }
                }
            }
        }


        private void InsertPurchasePrice()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "거래처별 매입단가 조회", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            if (MessageBox.Show(this,"선택한 내역들의 단가를 현재일 기준으로 신규등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //Sql Execute
                List<StringBuilder> sqlList = new List<StringBuilder>();
                StringBuilder sql;

                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    int id = commonRepository.GetNextId("t_purchase_price", "id");
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isChecked)
                    {
                        //Insert
                        PurchasePriceModel ppm = new PurchasePriceModel();
                        ppm.id = id;
                        ppm.product = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                        ppm.origin = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                        ppm.sizes = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                        ppm.unit = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                        ppm.purchase_price = Convert.ToDouble(dgvProduct.Rows[i].Cells["purchase_price"].Value.ToString());
                        double fixed_tariff;
                        if (dgvProduct.Rows[i].Cells["fixed_tariff"].Value == null || !double.TryParse(dgvProduct.Rows[i].Cells["fixed_tariff"].Value.ToString(), out fixed_tariff))
                            fixed_tariff = 0;
                        ppm.weight_calculate = Convert.ToBoolean(dgvProduct.Rows[i].Cells["weight_calculate"].Value.ToString());
                        ppm.is_FOB = Convert.ToBoolean(dgvProduct.Rows[i].Cells["is_FOB"].Value.ToString());
                        ppm.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                        ppm.edit_user = um.user_name;

                        CompanyModel cModel = new CompanyModel();
                        cModel = companyRepository.GetCompanyAsOne2(dgvProduct.Rows[i].Cells["company"].Value.ToString());
                        ppm.company = cModel.id.ToString();

                        sql = purchasePriceRepository.InsertPurchasePrice(ppm);
                        sqlList.Add(sql);

                        id += 1;
                    }
                }
                //Transaction execute
                if (sqlList.Count > 0)
                {
                    //Execute
                    int result = customsRepository.UpdateCustomTran(sqlList);
                    if (result == -1)
                    {
                        MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                    else
                    {
                        MessageBox.Show(this,"등록 완료");
                        this.Activate();
                    }
                }
            }
        }
        public void GetData()
        {
            DateTime sttDate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttDate))
            {
                MessageBox.Show(this,"시작일 값이 올바르지 않은 날짜 형식입니다.");
                this.Activate();
                return;
            }
            DateTime endDate;
            if (!DateTime.TryParse(txtEnddate.Text, out endDate))
            {
                MessageBox.Show(this,"종료일 값이 올바르지 않은 날짜 형식입니다.");
                this.Activate();
                return;
            }

            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            dgvProduct.Rows.Clear();
            DataTable productDt = purchasePriceRepository.GetPurchasePrice2(txtSttdate.Text, txtEnddate.Text, txtProduct.Text
                                                                            , txtOrigin.Text, txtSizes.Text, txtUnit.Text
                                                                            , txtCompany.Text, cbIsExactly.Checked, txtManager.Text);
            if (productDt.Rows.Count > 0)
            {
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = dgvProduct.Rows[n];

                    row.Cells["id"].Value = productDt.Rows[i]["id"].ToString();
                    row.Cells["product"].Value = productDt.Rows[i]["product"].ToString();
                    row.Cells["origin"].Value = productDt.Rows[i]["origin"].ToString();
                    row.Cells["sizes"].Value = productDt.Rows[i]["sizes"].ToString();
                    row.Cells["unit"].Value = productDt.Rows[i]["unit"].ToString();
                    row.Cells["cost_unit"].Value = productDt.Rows[i]["cost_unit"].ToString();

                    double cost_unit;
                    if (!double.TryParse(productDt.Rows[i]["cost_unit"].ToString(), out cost_unit))
                        cost_unit = 0;
                    if (cost_unit == 0)
                        cost_unit = 1;
                    double unit;
                    if (!double.TryParse(productDt.Rows[i]["unit"].ToString(), out unit))
                        unit = 0;
                    if (unit == 0)
                        unit = 1;

                    row.Cells["unit_per_box"].Value = unit / cost_unit;

                    row.Cells["company"].Value = productDt.Rows[i]["cname"].ToString();

                    row.Cells["custom"].Value = productDt.Rows[i]["custom"].ToString();
                    row.Cells["tax"].Value = productDt.Rows[i]["tax"].ToString();
                    row.Cells["incidental_expense"].Value = productDt.Rows[i]["incidental_expense"].ToString();

                    bool isPrivate = Convert.ToBoolean(productDt.Rows[i]["is_private"].ToString());
                    row.Cells["is_private"].Value = isPrivate;
                    if (!um.department.Contains("무역") && !um.department.Contains("전산") && !um.department.Contains("관리부") && isPrivate)
                        row.Cells["purchase_price"].Value = "-";
                    else
                        row.Cells["purchase_price"].Value = Convert.ToDouble(productDt.Rows[i]["purchase_price"].ToString());
                    
                    row.Cells["updatetime"].Value = Convert.ToDateTime(productDt.Rows[i]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                    row.Cells["edit_user"].Value = productDt.Rows[i]["edit_user"].ToString();

                    bool is_FOB = Convert.ToBoolean(productDt.Rows[i]["is_FOB"].ToString());
                    row.Cells["is_CFR"].Value = !is_FOB;
                    row.Cells["is_FOB"].Value = is_FOB;


                    bool weight_calculate;
                    if (!Boolean.TryParse(productDt.Rows[i]["weight_calculate"].ToString(), out weight_calculate))
                        weight_calculate = false;
                    row.Cells["weight_calculate"].Value = weight_calculate;
                    row.Cells["tray_calculate"].Value = !weight_calculate;

                    double fixed_tariff;
                    if (!double.TryParse(productDt.Rows[i]["fixed_tariff"].ToString(), out fixed_tariff))
                        fixed_tariff = 0;
                    row.Cells["fixed_tariff"].Value = fixed_tariff.ToString("#,##0");


                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        bool IsSameCellValueCheck(int column, int row)
        {
            DataGridViewCell cell1 = dgvProduct[column, row];
            DataGridViewCell cell2 = dgvProduct[column, row - 1];

            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }
        private void CostCalculate()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_visible"))
                    messageBox.Show(this, "권한이 없습니다!");
            }

            if (dgvProduct.Rows.Count > 0)
            {
                int cnt = 0;
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    if (Convert.ToBoolean(row.Cells["chk"].Value))
                    {
                        cnt+= 1;
                    }
                }
                if (cnt == 0)
                {
                    MessageBox.Show(this,"선택된 내역이 없습니다.");
                    this.Activate();
                    return;
                }


                FormCollection fc = Application.OpenForms;
                bool isFormActive = false;
                foreach (Form frm in fc)
                {
                    //iterate through
                    if (frm.Name == "CostAccounting")
                    {
                        ca = (PurchaseManager.CostAccounting)frm;       
                        isFormActive = true;
                        break;
                    }
                }
                //없을 경우
                if (!isFormActive)
                {
                    ca = new PurchaseManager.CostAccounting(um);
                    ca.StartPosition = FormStartPosition.CenterParent;
                    ca.Show();
                }
                //품목추가
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    bool isChecked = Convert.ToBoolean(row.Cells["chk"].Value);
                    if (isChecked)
                    { 
                        ca.AddProduct(row.Cells["updatetime"].Value.ToString()
                            , row.Cells["product"].Value.ToString()
                            , row.Cells["origin"].Value.ToString()
                            , row.Cells["sizes"].Value.ToString()
                            , row.Cells["unit"].Value.ToString()
                            , row.Cells["company"].Value.ToString());
                    }
                }
                ca.Activate();
            }
            else
            { 
                MessageBox.Show(this,"내역을 선택해주세요.");
                this.Activate();
            }
        }
        #endregion

        #region Key Event
        private void PurchaseUnitManager_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
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
        private void txtEnddate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tbb = (TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
                DateTime dt;
                if (DateTime.TryParse(tbb.Text, out dt))
                {
                    tbb.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    GetData();
                    break;
            }
        }
        private void PurchaseUnitManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        if (um.auth_level < 50)
                        {
                            MessageBox.Show(this,"권한이 없습니다.");
                            this.Activate();
                            return;
                        }
                        PurchasePriceInfo ppi = new PurchasePriceInfo(um, this);
                        ppi.ShowDialog();
                        break;
                    case Keys.S:
                        UpdatePurchasePrice();
                        break;
                    case Keys.D:
                        InsertPurchasePrice();
                        break;
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.W:
                        CostCalculate();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = "";
                        txtOrigin.Text = "";
                        txtSizes.Text = "";
                        txtUnit.Text = "";
                        txtCompany.Text = "";
                        txtManager.Text = "";
                        txtProduct.Focus();
                        break;
                    case Keys.G:
                        btnGraph.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                        
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        Copy();
                        break;
                    case Keys.X:
                        Copy();
                        Delete();
                        dgvProduct.EndEdit();
                        break;
                    case Keys.V:
                        Paste();
                        dgvProduct.EndEdit();
                        break;
                }
            }
        }

        private void txtSttdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            if (tb.Name != "dgvProduct")
            {
                switch (keyData)
                {
                    case Keys.Left:
                        if (tb.Name == "txtSttdate" || tb.Name == "txtEnddate")
                             return base.ProcessCmdKey(ref msg, keyData); 
                        else
                            tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Right:
                        if (tb.Name == "txtSttdate" || tb.Name == "txtEnddate")
                            return base.ProcessCmdKey(ref msg, keyData);
                        else
                            tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
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
        #endregion

        #region Datagridview Event

        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewColumn col = dgvProduct.Columns[e.ColumnIndex];
                //체크여부
                if (col.Name == "chk")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["chk"].Value);
                    if (isChecked)
                    {
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.Red;
                        dgvProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                    else
                    {
                        dgvProduct.Rows[e.RowIndex].HeaderCell.Style.BackColor = Color.LightGray;
                        dgvProduct.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                        dgvProduct.Rows[e.RowIndex].Cells["product"].Style.BackColor = Color.FromArgb(221, 235, 247);
                        dgvProduct.Rows[e.RowIndex].Cells["purchase_price"].Style.BackColor = Color.FromArgb(221, 235, 247);
                    }
                }
                //비공개
                if (col.Name == "is_private")
                {
                    bool isChecked = Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["is_private"].Value);
                    if (isChecked)
                    {
                        dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightYellow;
                    }
                    else
                    {
                        dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                    }
                }

                //merge
                e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

                if (e.RowIndex < 1 || e.ColumnIndex < 0)
                {
                    e.AdvancedBorderStyle.Top = dgvProduct.AdvancedCellBorderStyle.Top;

                    return;
                }

                if (IsSameCellValueCheck(2, e.RowIndex) && IsSameCellValueCheck(3, e.RowIndex) && IsSameCellValueCheck(3, e.RowIndex) && IsSameCellValueCheck(4, e.RowIndex))
                {
                    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Top = dgvProduct.AdvancedCellBorderStyle.Top;
                }
            }
        }
        private void dgvProduct_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // If the data source raises an exception when a cell value is 
            // commited, display an error message.
            if (e.Exception != null &&
                e.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show(this,"CustomerID value must be unique.");
                this.Activate();
            }
        }
        private void dgvProduct_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            string name = dgvProduct.CurrentCell.OwningColumn.Name;

            if (name == "purchase_price")
            {
                e.Control.KeyPress += new KeyPressEventHandler(txtCheckNumeric_KeyPress);
            }
            else
            {
                e.Control.KeyPress -= new KeyPressEventHandler(txtCheckNumeric_KeyPress);
            }
        }
        private void txtCheckNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)46 || e.KeyChar == (Char)45 || e.KeyChar == (Char)65294))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "is_CFR")
                {
                    dgvProduct.EndEdit();
                    bool is_CFR;
                    if (dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value == null || !bool.TryParse(dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value.ToString(), out is_CFR))
                        is_CFR = false;

                    dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value = is_CFR;
                    dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value = !is_CFR;
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "is_FOB")
                {
                    dgvProduct.EndEdit();
                    bool is_FOB;
                    if (dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value == null || !bool.TryParse(dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value.ToString(), out is_FOB))
                        is_FOB = false;

                    dgvProduct.Rows[e.RowIndex].Cells["is_CFR"].Value = !is_FOB;
                    dgvProduct.Rows[e.RowIndex].Cells["is_FOB"].Value = is_FOB;
                }
            }
        }
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvProduct.Columns[e.ColumnIndex].Name != "chk")
            {
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                dgvProduct.Rows[e.RowIndex].Cells["chk"].Value = true;

                
                    

                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                try
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        //Selection
                        //dgvProduct.ClearSelection();
                        DataGridViewRow selectRow = this.dgvProduct.Rows[e.RowIndex];
                        selectRow.Selected = !selectRow.Selected;
                    }
                }
                catch {}
            }
        }
        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                dgvProduct.Rows[e.RowIndex].Cells["chk"].Value = !Convert.ToBoolean(dgvProduct.Rows[e.RowIndex].Cells["chk"].Value);
        }
        #endregion

        #region 우클릭 메뉴
        //우클릭 메뉴 Create
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

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("수정");
                    m.Items.Add("선택/선택해제");
                    /*m.Items.Add("일별 그래프");
                    m.Items.Add("월별 그래프");*/
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
            catch {}
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (dgvProduct.SelectedRows.Count == 0)
            {
                return;
            }
            try
            {
                DataGridViewRow dr = dgvProduct.SelectedRows[0];

                if (dr.Index < 0)
                {
                    return;
                }

                int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                /*PendingInfo p;*/

                FormCollection fc = Application.OpenForms;
                bool isFormActive = false;
                switch (e.ClickedItem.Text)
                {
                    case "수정":
                        //권한확인
                        DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
                        if (authorityDt != null && authorityDt.Rows.Count > 0)
                        {
                            if (!common.CheckAuthority(authorityDt, "수입관리", "거래처별 매입단가 조회", "is_delete"))
                            {
                                messageBox.Show(this, "권한이 없습니다!");
                                return;
                            }
                        }
                        int id = Convert.ToInt32(dr.Cells["id"].Value);
                        PurchasePriceInfo ppi = new PurchasePriceInfo(um, this, id.ToString());
                        ppi.Owner = this;
                        ppi.ShowDialog();
                        break;
                    case "선택/선택해제":
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            bool isCheck = !Convert.ToBoolean(dgvProduct.Rows[dgvProduct.SelectedRows[0].Index].Cells["chk"].Value);

                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                row.Cells["chk"].Value = isCheck;
                            }
                            dgvProduct.EndEdit();
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
        #endregion

        #region Button
        private void btnGraph_Click(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this,"선택한 내역이 없습니다.");
                this.Activate();
                return;
            }

            int cnt = 0;
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                bool isCheck = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                if (isCheck)
                    cnt++;
            }
            if (cnt == 0)
            {
                MessageBox.Show(this,"선택한 내역이 없습니다.");
                this.Activate();
                return;
            }


            List<string[]> productList = new List<string[]>();
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                bool isCheck = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                if (isCheck)
                {
                    string[] product = new string[6];
                    product[0] = dgvProduct.Rows[i].Cells["product"].Value.ToString();
                    product[1] = dgvProduct.Rows[i].Cells["origin"].Value.ToString();
                    product[2] = dgvProduct.Rows[i].Cells["sizes"].Value.ToString();
                    product[3] = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                    product[4] = dgvProduct.Rows[i].Cells["unit"].Value.ToString();
                    product[5] = "";

                    productList.Add(product);
                }
            }

            GraphManager gm = new GraphManager(um);
            gm.AddProduct(productList, true);
            gm.Show();
        }

   
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "거래처별 매입단가 조회", "is_delete"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvProduct.Rows.Count == 0)
            {
                MessageBox.Show(this,"삭제할 내역이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                int cnt = 0;
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    bool isCheck = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                    if (isCheck)
                        cnt++;
                }
                if (cnt == 0)
                {
                    MessageBox.Show(this,"삭제할 내역을 선택해주세요.");
                    this.Activate();
                    return;
                }
            }

            //Messagebox
            if (MessageBox.Show(this,"선택한 내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //Sql Execute
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql;
            //Delete
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                bool isCheck = Convert.ToBoolean(dgvProduct.Rows[i].Cells["chk"].Value);
                if (isCheck)
                {
                    sql = purchasePriceRepository.DeletePurchasePrice(dgvProduct.Rows[i].Cells["id"].Value.ToString());
                    sqlList.Add(sql);
                }
            }
            //Execute
            int result = customsRepository.UpdateCustomTran(sqlList);
            if (result == -1)
            {
                MessageBox.Show(this,"삭제 중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                GetData();
            }
        }
        private void btnCostCalculate_Click(object sender, EventArgs e)
        {
            CostCalculate();
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdatePurchasePrice();
        }
        private void btnInsert2_Click(object sender, EventArgs e)
        {
            InsertPurchasePrice();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnCalendarSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtSttdate.Text = sdate;
            }
        }

        private void btnCalendarEnddate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtEnddate.Text = sdate;
            }
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void btnInsert_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "거래처별 매입단가 조회", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            PurchasePriceInfo ppi = new PurchasePriceInfo(um, this);
            ppi.ShowDialog();
        }
        #endregion

        #region 복사, 붙혀넣기
        /// <summary>
        /// 선택한 셀을 복사합니다.
        /// </summary>
        private void Copy()
        {
            DataObject clipboardContent = dgvProduct.GetClipboardContent();

            if (clipboardContent != null)
            {
                Clipboard.SetDataObject((object)clipboardContent);
            }
            isPasteRow = false;
        }

        /// <summary>
        /// 선택한 셀을 삭제합니다.
        /// </summary>
        private void Delete()
        {
            foreach (DataGridViewCell oneCell in dgvProduct.SelectedCells)
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
            if (dgvProduct.CurrentCell != null)
            {
                string clipText = Clipboard.GetText();
                if (string.IsNullOrEmpty(clipText) == false)
                {
                    string[] lines = clipText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    string[] texts = lines[0].Split('\t');

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
                                                    dgvProduct[col, row].Value = txt;
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        cell.Value = texts[0];
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
                            dgvProduct.Rows.Insert(rowIndex, 1);
                            //추가한 행에 붙혀넣기
                            for (int i = 0; i < lines.Length; i++)
                            {
                                foreach (DataGridViewCell cell in dgvProduct.Rows[rowIndex].Cells)
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
                                                    dgvProduct[col, row].Value = txt;
                                                    col++;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        cell.Value = texts[0];
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
                            for (int i = 0; i < lines.Length; i++)
                            {
                                foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                                {
                                    row = cell.RowIndex;
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
                        // one row, Multi columns
                        else if (lines.Length > 1 && texts.Length == 1)
                        {
                            for (int i = 0; i < lines.Length; i++)
                            {
                                foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                                {
                                    row = cell.RowIndex;
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
                        else
                        {
                            string txt = lines[0];
                            foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                            {
                                cell.Value = txt;
                            }
                        }
                    }
                }
            }
        }


        #endregion

        
    }
}

