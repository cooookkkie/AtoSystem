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

namespace AdoNetWindow.PurchaseManager
{
    public partial class PurchaseUnitPriceInfo : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        PurchaseManager.CostAccounting ca = null;
        PurchaseManager.PurchaseUnitManager pm = null;
        UsersModel um;
        DataTable productDt = new DataTable();
        DataTable companyDt = new DataTable();
        DataTable priceDt = new DataTable();

        List<SeaoverProductModel> pModel = new List<SeaoverProductModel>();
        List<HandlingProductModel> cModel = new List<HandlingProductModel>();

        int current_colIndex;

        public PurchaseUnitPriceInfo(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }
        private void PurchaseUnitPriceInfo_Load(object sender, EventArgs e)
        {
            //dgvUnitPrice.Init();

            cbCompany.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCompany.AutoCompleteSource = AutoCompleteSource.ListItems;
            companyDt = commonRepository.SelectAsOneLike("t_company", "DISTINCT name", "division", "매입처");
            /*if (companyDt.Rows.Count > 0)
            {
                for (int i = 0; i < companyDt.Rows.Count; i++)
                {
                    cbCompany.Items.Add(companyDt.Rows[i]["name"].ToString());
                }
            }*/
            cbCompany.DataSource = companyDt;
            cbCompany.DisplayMember = "name";
            cbCompany.Text = "";
            priceDt = commonRepository.SelectAsOne("t_purchase_price", "*", "updatetime", "'" + DateTime.Now.ToString("yyyy-MM-dd") + "'");

            string sttdate = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            seaoverRepository.CallStoredProcedure(um.seaover_id, sttdate, enddate);
            txtUpdattime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtManager.Text = um.user_name;
            /*productDt = seaoverRepository.GetOneColumn("품명", "품명", cbProduct.Text);*/
            
            DgvSytleSetting();
            //Seaover 사번이 없는경우 수정
            if (um.seaover_id == null || string.IsNullOrEmpty(um.seaover_id))
            {
                MessageBox.Show(this,"내정보에서 SEAOVER 사번을 입력해주세요.");
                Config.EditMyInfo emi = new Config.EditMyInfo(um);
                um = emi.UpdateSeaoverId();
            }
            CallProcedure();
        }

        #region Method
        private void CallProcedure()
        {
            //업체별시세현황 스토어프로시져 호출
            string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
            string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string user_id = um.seaover_id;
            if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
            {
                MessageBox.Show(this,"호출 내용이 없음");
                return;
            }
        }
        private void companyRefresh()
        {
            if (dgvUnitPrice.Columns.Count > 7)
            {
                for (int i = dgvUnitPrice.Columns.Count - 1; i >= 9; i--)
                    dgvUnitPrice.Columns.Remove(dgvUnitPrice.Columns[i]);
            }
        }
        private void AddCompany()
        {
            if (string.IsNullOrEmpty(cbCompany.Text))
            {
                MessageBox.Show(this,"거래처를 선택해주세요.");
                return;
            }
            //거래처 선택
            CompanyModel model = new CompanyModel();
            model = companyRepository.GetCompanyAsOne2(cbCompany.Text);
            if (model != null)
            {
                for (int i = 9; i < dgvUnitPrice.ColumnCount; i++)
                {
                    if (dgvUnitPrice.Columns[i].HeaderCell.Value.ToString() == model.name)
                    {
                        MessageBox.Show(this,"이미 추가된 거래처입니다.");
                        return;
                    }
                }
                //추가
                int n = dgvUnitPrice.Columns.Add(model.id.ToString(), model.name);
                dgvUnitPrice.Columns[n].Width = 200;
                dgvUnitPrice.Columns[n].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
                dgvUnitPrice.Columns[n].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                //금일 등록된 단가가 있는지 조회
                CheckTodayPrice();
            }
        }
        public void CheckTodayPrice()
        {
            if (dgvUnitPrice.Columns.Count > 7)
            {
                priceDt = purchasePriceRepository.GetPurchasePriceList(txtUpdattime.Text, "", "", "", "");
                //금일 등록된 단가가 있는지 조회
                for (int i = 0; i < dgvUnitPrice.Rows.Count; i++)
                {
                    for (int j = 9; j < dgvUnitPrice.Columns.Count; j++)
                    {
                        if (priceDt != null && priceDt.Rows.Count > 0)
                        {
                            string whr = $" cid = {dgvUnitPrice.Columns[j].Name} AND product = '{dgvUnitPrice.Rows[i].Cells["product"].Value}'" +
                                                                $"AND origin = '{dgvUnitPrice.Rows[i].Cells["origin"].Value}'" +
                                                                $"AND sizes = '{dgvUnitPrice.Rows[i].Cells["sizes"].Value}'" +
                                                                $"AND unit = '{dgvUnitPrice.Rows[i].Cells["weight"].Value}' " +
                                                                $"AND cost_unit = '{dgvUnitPrice.Rows[i].Cells["cost_unit"].Value}' ";
                            DataRow[] tmpRow = priceDt.Select(whr);
                            if (tmpRow.Length > 0)
                            {
                                dgvUnitPrice.Rows[i].Cells[j].Value = tmpRow[0]["purchase_price"].ToString();
                                //단가종류
                                if (tmpRow[0]["is_FOB"].ToString() == "TRUE")
                                    dgvUnitPrice.Rows[i].Cells[j].Style.ForeColor = Color.Blue;
                                else
                                    dgvUnitPrice.Rows[i].Cells[j].Style.ForeColor = Color.Red;
                                //비공개 여부
                                if (tmpRow[0]["is_private"].ToString() == "TRUE")
                                    dgvUnitPrice.Rows[i].Cells[j].Style.BackColor = Color.LightYellow;
                                else
                                    dgvUnitPrice.Rows[i].Cells[j].Style.BackColor = Color.White;
                            }
                        }
                    }
                }
            }
        }

        private void GetProduct()
        {
            this.dgvUnitPrice.Rows.Clear();
            DataTable seaoverDt = seaoverRepository.GetProductTable2(txtProduct.Text, false, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            DataTable otherCostDt = productOtherCostRepository.GetProduct(txtProduct.Text, false, txtOrigin.Text, txtSizes.Text, txtUnit.Text);
            //등록된 품목
            for (int i = 0; i < seaoverDt.Rows.Count; i++)
            {

                int n = dgvUnitPrice.Rows.Add();
                dgvUnitPrice.Rows[n].HeaderCell.Value = "S";
                dgvUnitPrice.Rows[n].Cells["product"].Value = seaoverDt.Rows[i]["품명"].ToString();
                dgvUnitPrice.Rows[n].Cells["origin"].Value = seaoverDt.Rows[i]["원산지"].ToString();
                dgvUnitPrice.Rows[n].Cells["sizes"].Value = seaoverDt.Rows[i]["규격"].ToString();

                double cost_unit;
                if (!double.TryParse(seaoverDt.Rows[i]["트레이"].ToString(), out cost_unit))
                    cost_unit = 0;
                if (cost_unit == 0)
                    cost_unit = 1;
                double unit;
                if (!double.TryParse(seaoverDt.Rows[i]["SEAOVER단위"].ToString(), out unit))
                    unit = 0;
                if (unit == 0)
                    unit = 1;

                dgvUnitPrice.Rows[n].Cells["unit2"].Value = unit / cost_unit;
                dgvUnitPrice.Rows[n].Cells["weight"].Value = seaoverDt.Rows[i]["SEAOVER단위"].ToString();
                dgvUnitPrice.Rows[n].Cells["cost_unit"].Value = seaoverDt.Rows[i]["트레이"].ToString();

                //품목 추가등록정보
                DataRow[] arrRows = null;
                arrRows = otherCostDt.Select($"product = '{seaoverDt.Rows[i]["품명"].ToString()}' " +
                                                $"AND origin = '{seaoverDt.Rows[i]["원산지"].ToString()}'" +
                                                $"AND sizes = '{seaoverDt.Rows[i]["규격"].ToString()}'" +
                                                $"AND unit = '{seaoverDt.Rows[i]["SEAOVER단위"].ToString()}'" +
                                                $"AND cost_unit = '{seaoverDt.Rows[i]["트레이"].ToString()}'");
                //출력
                if (arrRows.Length > 0)
                {
                    dgvUnitPrice.Rows[n].Cells["manager"].Value = arrRows[0]["manager"].ToString();
                    bool weight_calculate;
                    if (!bool.TryParse(arrRows[0]["manager"].ToString(), out weight_calculate))
                        weight_calculate = true;
                    dgvUnitPrice.Rows[n].Cells["weight_calculate"].Value = weight_calculate;
                }
                else
                    dgvUnitPrice.Rows[n].Cells["weight_calculate"].Value = true;
            }
            //미등록 품목
            for (int i = 0; i < otherCostDt.Rows.Count; i++)
            {
                DataRow[] arrRows = null;
                string whr = $"품명 = '{otherCostDt.Rows[i]["product"].ToString()}' " +
                            $"AND 원산지 = '{otherCostDt.Rows[i]["origin"].ToString()}'" +
                            $"AND 규격 = '{otherCostDt.Rows[i]["sizes"].ToString()}'" +
                            $"AND SEAOVER단위 = '{otherCostDt.Rows[i]["unit"].ToString()}'";


                arrRows = seaoverDt.Select(whr);
                if (arrRows.Length == 0)
                {
                    int n = dgvUnitPrice.Rows.Add();
                    dgvUnitPrice.Rows[n].Cells["product"].Value = otherCostDt.Rows[i]["product"].ToString();
                    dgvUnitPrice.Rows[n].Cells["origin"].Value = otherCostDt.Rows[i]["origin"].ToString();
                    dgvUnitPrice.Rows[n].Cells["sizes"].Value = otherCostDt.Rows[i]["sizes"].ToString();
                    dgvUnitPrice.Rows[n].Cells["unit"].Value = otherCostDt.Rows[i]["unit"].ToString();
                    dgvUnitPrice.Rows[n].Cells["unit2"].Value = otherCostDt.Rows[i]["unit"].ToString();

                    dgvUnitPrice.Rows[n].Cells["cost_unit"].Value = otherCostDt.Rows[i]["cost_unit"].ToString();
                    double cost_unit;
                    if (!double.TryParse(otherCostDt.Rows[i]["cost_unit"].ToString(), out cost_unit))
                        cost_unit = 0;
                    if (cost_unit == 0)
                        cost_unit = 1;
                    double unit;
                    if (!double.TryParse(otherCostDt.Rows[i]["unit"].ToString(), out unit))
                        unit = 0;
                    if (unit == 0)
                        unit = 1;

                    dgvUnitPrice.Rows[n].Cells["weight"].Value = unit * cost_unit;
                    dgvUnitPrice.Rows[n].Cells["weight_calculate"].Value = true;
                }
            }

            CheckTodayPrice();
            dgvUnitPrice.EndEdit();
        }

        public void InputProduct(List<DataGridViewRow> productList)
        {
            for (int i = 0; i < productList.Count; i++)
            {
                DataGridViewRow row = productList[i];
                int n = dgvUnitPrice.Rows.Add();
                dgvUnitPrice.Rows[n].HeaderCell.Value = "S";
                dgvUnitPrice.Rows[n].Cells["product"].Value = row.Cells["product"].Value.ToString();
                dgvUnitPrice.Rows[n].Cells["origin"].Value = row.Cells["origin"].Value.ToString();
                dgvUnitPrice.Rows[n].Cells["sizes"].Value = row.Cells["sizes"].Value.ToString();
                dgvUnitPrice.Rows[n].Cells["unit"].Value = row.Cells["unit"].Value.ToString();
                dgvUnitPrice.Rows[n].Cells["unit2"].Value = row.Cells["unit"].Value.ToString();
                dgvUnitPrice.Rows[n].Cells["cost_unit"].Value = row.Cells["unit_count"].Value.ToString();
                dgvUnitPrice.Rows[n].Cells["weight"].Value = row.Cells["seaover_unit"].Value.ToString();
                dgvUnitPrice.Rows[n].Cells["manager"].Value = row.Cells["trade_manager"].Value.ToString();
            }
            CheckTodayPrice();
            dgvUnitPrice.EndEdit();
        }

        private void DgvSytleSetting()
        {
            DataGridView dgv = dgvUnitPrice;
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgv.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["unit"].HeaderCell.Style.ForeColor = Color.White;
            
            dgv.Columns["unit2"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["unit2"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["cost_unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["cost_unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["weight"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["weight"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["manager"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["manager"].HeaderCell.Style.ForeColor = Color.White;


            dgv.Columns["unit"].Visible = false;
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void InsertPurchasePrice()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "거래처별 매입단가 일괄등록", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            dgvUnitPrice.EndEdit();
            DateTime datetime;
            if (dgvUnitPrice.Columns.Count < 8 )
            {
                MessageBox.Show(this,"등록할 거래처가 없습니다.");
                return;
            }
            else if (dgvUnitPrice.Rows.Count == 0)
            {
                MessageBox.Show(this,"등록할 품목이 없습니다.");
                return;
            }
            else if (string.IsNullOrEmpty(txtUpdattime.Text))
            {
                MessageBox.Show(this,"등록일자를 선택해주세요.");
                return;
            }
            else if (!DateTime.TryParse(txtUpdattime.Text, out datetime))
            {
                MessageBox.Show(this,"등록일자를 다시 선택해주세요.");
                return;
            }
            
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql = new StringBuilder();
            //New id
            int id = commonRepository.GetNextId("t_purchase_price", "id");
            //등록
            for (int i = 0; i < dgvUnitPrice.Rows.Count; i++)
            {
                for (int j = 9; j < dgvUnitPrice.Columns.Count; j++)
                {
                    if (dgvUnitPrice.Rows[i].Cells[j].Value != null && !string.IsNullOrEmpty(dgvUnitPrice.Rows[i].Cells[j].Value.ToString()))
                    {
                        PurchasePriceModel ppm = new PurchasePriceModel();
                        ppm.id = id;
                        ppm.company = dgvUnitPrice.Columns[j].Name;
                        ppm.product = dgvUnitPrice.Rows[i].Cells["product"].Value.ToString();
                        ppm.origin = dgvUnitPrice.Rows[i].Cells["origin"].Value.ToString();
                        ppm.sizes = dgvUnitPrice.Rows[i].Cells["sizes"].Value.ToString();
                        ppm.unit = dgvUnitPrice.Rows[i].Cells["weight"].Value.ToString();
                        ppm.cost_unit = dgvUnitPrice.Rows[i].Cells["cost_unit"].Value.ToString();
                        ppm.purchase_price = Convert.ToDouble(dgvUnitPrice.Rows[i].Cells[j].Value);
                        ppm.updatetime = txtUpdattime.Text;
                        ppm.edit_user = txtManager.Text;

                        if (dgvUnitPrice.Rows[i].Cells[j].Style.BackColor == Color.LightYellow)
                            ppm.is_private = true;
                        else
                            ppm.is_private = false;

                        ppm.is_FOB = rbFOB.Checked;
                        bool weight_calculate;
                        if (dgvUnitPrice.Rows[i].Cells["weight_calculate"].Value == null || !bool.TryParse(dgvUnitPrice.Rows[i].Cells["weight_calculate"].Value.ToString(), out weight_calculate))
                            weight_calculate = true;
                        ppm.weight_calculate = weight_calculate;

                        sql = purchasePriceRepository.DeletePurchasePrice2(ppm);
                        sqlList.Add(sql);

                        sql = purchasePriceRepository.InsertPurchasePrice(ppm);
                        sqlList.Add(sql);

                        id += 1;
                    }
                }
            }
            //등록
            if (sqlList.Count > 0)
            {
                string price_type;
                if (rbCFR.Checked)
                    price_type = "CFR";
                else
                    price_type = "FOB";

                //Messagebox
                if (MessageBox.Show(this,$"아래 내역을 [{price_type}]으로 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
                else
                {
                    int results = commonRepository.UpdateTran(sqlList);
                    if (results == -1)
                        MessageBox.Show(this,"등록중 에러가 발생하였습니다.");
                    else
                        MessageBox.Show(this,"등록완료");
                }
            }

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


            if (dgvUnitPrice.Rows.Count > 0)
            {
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
                    ca.SetSelectTerm(txtUpdattime.Text, txtUpdattime.Text);
                    ca.Show();
                }
                //품목
                string product = "";
                string origin = "";
                string sizes = "";
                string unit = "";
                foreach (DataGridViewRow row in dgvUnitPrice.Rows)
                {
                    product += "^" + row.Cells["product"].Value.ToString();
                    origin += "^" + row.Cells["origin"].Value.ToString();
                    sizes += "^" + row.Cells["sizes"].Value.ToString();
                    unit += "^" + row.Cells["weight"].Value.ToString();
                }
                //거래처 
                string company = "";
                if (dgvUnitPrice.Columns.Count > 7)
                {
                    for (int i = 9; i < dgvUnitPrice.Columns.Count; i++)
                    {
                        company += "^" + dgvUnitPrice.Columns[i].HeaderText;
                    }
                }
                //품목추가
                ca.AddProduct(txtUpdattime.Text
                            , product
                            , origin
                            , sizes
                            , unit
                            , company);
                //Active
                ca.Activate();
            }
            else
            {
                MessageBox.Show(this,"내역을 조회해주세요.");
            }
        }
        private void PurchaseUnitPriceSearch()
        {
            if (dgvUnitPrice.Rows.Count > 0)
            {
                FormCollection fc = Application.OpenForms;
                bool isFormActive = false;
                foreach (Form frm in fc)
                {
                    //iterate through
                    if (frm.Name == "PurchaseUnitManager")
                    {
                        pm = (PurchaseManager.PurchaseUnitManager)frm;
                        isFormActive = true;
                        break;
                    }
                }
                //없을 경우
                if (!isFormActive)
                {
                    pm = new PurchaseManager.PurchaseUnitManager(um);
                    pm.StartPosition = FormStartPosition.CenterParent;
                    pm.Show();
                }
                //품목추가
                string company = "";
                if (dgvUnitPrice.Columns.Count > 7)
                {
                    for (int i = 9; i < dgvUnitPrice.Columns.Count; i++)
                    {
                        company += " " + dgvUnitPrice.Columns[i].HeaderText;
                    }
                }
                pm.SearchProduct(txtProduct.Text
                            , txtOrigin.Text
                            , txtSizes.Text
                            , txtUnit.Text
                            , company.Trim());
                pm.Activate();
            }
            else
            {
                MessageBox.Show(this,"내역을 조회해주세요.");
            }
        }
        #endregion

        #region Click Event
        private void btnRefreshProduct_Click(object sender, EventArgs e)
        {
            dgvUnitPrice.Rows.Clear();
        }

        private void btnRefreshCompany_Click(object sender, EventArgs e)
        {
            if (dgvUnitPrice.ColumnCount > 7)
            {
                for (int i = dgvUnitPrice.ColumnCount - 1; i >= 9; i--)
                {
                    dgvUnitPrice.Columns.Remove(dgvUnitPrice.Columns[i]);
                }
            }
        }
        private void cbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*if (!string.IsNullOrEmpty(cbProduct.Text))
            {
                DataTable originDt = seaoverRepository.GetOneColumn("원산지", "품명", cbProduct.Text);
                if (originDt.Rows.Count > 0)
                {
                    cbOrigin.DataSource = originDt;
                    cbOrigin.DisplayMember = "원산지";
                }
            }*/
        }
        #endregion

        #region Button
        private void btnProductSearching_Click(object sender, EventArgs e)
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

        private void btnCompanySearching_Click(object sender, EventArgs e)
        {
            Company.CompanyManager cm = new Company.CompanyManager(um, this, true);
            cm.ShowDialog();
        }
        private void btnGetGroupItem_Click(object sender, EventArgs e)
        {
            Config.GroupManager gm = new Config.GroupManager(um, this, true);
            gm.ShowDialog();
        }
        private void btnCostCalculate_Click(object sender, EventArgs e)
        {
            CostCalculate();
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            PurchaseUnitPriceSearch();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvUnitPrice.Rows.Clear();
            companyRefresh();
        }
        private void btnCompanyAdd_Click(object sender, EventArgs e)
        {
            AddCompany();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertPurchasePrice();
        }
        private void btnCalendarUpdatetime_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtUpdattime.Text = sdate;
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            GetProduct();
        }
        #endregion

        #region Key Event
        private void txtUpdattime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tbb = (TextBox)sender;
                if (tbb.Text.Contains("/"))
                {
                    if (!string.IsNullOrEmpty(tbb.Text))
                    {
                        DateTime dt;
                        if (DateTime.TryParse(tbb.Text, out dt))
                        {
                            tbb.Text = dt.ToString("yyyy-MM-dd");
                        }
                    }
                }
            }
        }
        private void txtUpdattime_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(65293) || e.KeyChar == Convert.ToChar(47)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
            else
            {
                //삭제후 재입력
                string tmpStr = tb.Text;
                if (tmpStr.Replace("-", "").Length == 8)
                {
                    tb.Text = "";
                }
            }
        }
        
        
        private void PurchaseUnitPriceInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                Control tb = common.FindFocusedControl(this);
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetProduct();
                        break;
                    case Keys.A:
                        InsertPurchasePrice();
                        break;
                    case Keys.S:
                        AddCompany();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.W:
                        CostCalculate();
                        break;
                    case Keys.E:
                        PurchaseUnitPriceSearch();
                        break;
                    case Keys.N:
                        txtProduct.Text = "";
                        txtOrigin.Text = "";
                        txtSizes.Text = "";
                        if (tb.Name == "txtProduct")
                        {
                            cbCompany.Focus();
                        }
                        else
                        {
                            txtProduct.Focus();
                        }
                        break;
                    case Keys.M:
                        if (tb.Name == "txtProduct")
                        {
                            cbCompany.Focus();
                        }
                        else
                        {
                            txtProduct.Focus();
                        }
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        rbCFR.Checked = true;
                        break;
                    case Keys.F3:
                        rbFOB.Checked = true;
                        break;
                    case Keys.F4:
                        btnProductSearching.PerformClick();
                        break;
                    case Keys.F5:
                        dgvUnitPrice.Rows.Clear();
                        companyRefresh();
                        break;
                    //거래처 선택
                    case Keys.F9:
                        btnCompanySearching.PerformClick();
                        break;
                }
            }
        }
        public void AddCompany(Dictionary<int, string> company, bool isCheckTodayPrice = true)
        {
            if (company != null && company.Count > 0)
            {
                string errMsg = "";
                foreach (int keys in company.Keys)
                {
                    bool isFlag = true;
                    for (int j = 9; j < dgvUnitPrice.ColumnCount; j++)
                    {
                        if (dgvUnitPrice.Columns[j].HeaderCell.Value.ToString() == company[keys])
                        {
                            if(string.IsNullOrEmpty(errMsg))
                                errMsg = "중복거래처 \n";

                            errMsg += "\n" + company[keys];
                            isFlag = false;
                        }
                    }
                    //추가
                    if (isFlag)
                    {
                        int n = dgvUnitPrice.Columns.Add(keys.ToString(), company[keys]);
                        dgvUnitPrice.Columns[n].Width = 200;
                        dgvUnitPrice.Columns[n].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
                        dgvUnitPrice.Columns[n].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        //금일 등록된 단가가 있는지 조회
                        if(isCheckTodayPrice)
                            CheckTodayPrice();
                    }
                    //중복메세지
                    if (!string.IsNullOrEmpty(errMsg))
                        MessageBox.Show(this,errMsg);
                }
            }
        }
        public void AddProduct(List<string[]> selectProduct)
        {
            if (selectProduct != null && selectProduct.Count > 0)
            {
                for (int i = 0; i < selectProduct.Count; i++)
                {
                    int n = dgvUnitPrice.Rows.Add();

                    dgvUnitPrice.Rows[n].Cells["product"].Value = selectProduct[i][0];
                    dgvUnitPrice.Rows[n].Cells["origin"].Value = selectProduct[i][1];
                    dgvUnitPrice.Rows[n].Cells["sizes"].Value = selectProduct[i][2];
                    dgvUnitPrice.Rows[n].Cells["cost_unit"].Value = selectProduct[i][4];

                    double cost_unit;
                    if (!double.TryParse(selectProduct[i][4], out cost_unit))
                        cost_unit = 0;
                    if (cost_unit == 0)
                        cost_unit = 1;
                    double weight;
                    if (!double.TryParse(selectProduct[i][3], out weight))
                        weight = 0;
                    if (weight == 0)
                        weight = 1;

                    dgvUnitPrice.Rows[n].Cells["unit"].Value = weight / cost_unit;
                    dgvUnitPrice.Rows[n].Cells["unit2"].Value = weight / cost_unit;
                    dgvUnitPrice.Rows[n].Cells["weight"].Value = weight;
                    dgvUnitPrice.Rows[n].Cells["manager"].Value = selectProduct[i][8];
                }
            }
        }


        private void dgvUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(46) || e.KeyChar == Convert.ToChar(45)))
                e.Handled = true;
        }
        private void txtUnit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetProduct();
            }
        }
        private void cbCompany_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                AddCompany();
            }
        }
        #endregion

        #region Datagridview cell event, merge
        private void dgvUnitPrice_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 9)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgvUnitPrice.ClearSelection();
                    dgvUnitPrice.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected = true;
                }
            }
        }
        private void dgvUnitPrice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 9 && e.RowIndex >= 0)
            {
                DataGridViewCell cell = dgvUnitPrice.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Value != null && cell.Value != "")
                {
                    if (cell.Style.BackColor != Color.LightYellow)
                    {
                        cell.Style.BackColor = Color.LightYellow;
                    }
                    else
                    {
                        cell.Style.BackColor = Color.White;
                    }
                }
            }
        }
        bool IsSameCellValueCheck(int column, int row)
        {
            DataGridViewCell cell1 = dgvUnitPrice[column, row];
            DataGridViewCell cell2 = dgvUnitPrice[column, row - 1];

            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }

            return cell1.Value.ToString() == cell2.Value.ToString();
        }
        private void dgvUnitPrice_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            if (e.RowIndex < 1 || e.ColumnIndex < 0)
            {
                e.AdvancedBorderStyle.Top = dgvUnitPrice.AdvancedCellBorderStyle.Top;
                return;
            }

            if (e.ColumnIndex < 1)
            {
                if (IsSameCellValueCheck(e.ColumnIndex, e.RowIndex))
                {
                    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Top = dgvUnitPrice.AdvancedCellBorderStyle.Top;
                }
            }
            else
                e.AdvancedBorderStyle.Top = dgvUnitPrice.AdvancedCellBorderStyle.Top;
        }
        private void dgvUnitPrice_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvUnitPrice.Rows[e.RowIndex];
                double price;
                if (e.ColumnIndex >= 9
                    && row.Cells[e.ColumnIndex].Value != null
                    && double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out price))
                {
                    if (price > 0)
                    {
                        DateTime enddate;
                        if (!DateTime.TryParse(txtUpdattime.Text, out enddate))
                        {
                            return;
                        }
                        DateTime sttdate = enddate.AddYears(-1);
                        string txt = "";
                        DataTable rankDt = purchasePriceRepository.GetRankingPurchasePriceASOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                            , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["weight"].Value.ToString()
                            , "", price);
                        if (rankDt.Rows.Count > 0)
                        {
                            //순위
                            for (int i = 0; i < rankDt.Rows.Count; i++)
                            {
                                if (rankDt.Rows[i]["division"].ToString() == "1")
                                    txt = "순위 : " + (i + 1) + " / " + rankDt.Rows.Count.ToString();
                            }
                            //최대, 최소값
                            double MaxPrice;
                            if (!double.TryParse(rankDt.Rows[rankDt.Rows.Count - 1]["purchase_price"].ToString(), out MaxPrice))
                                MaxPrice = 0;
                            double MinPrice;
                            if (!double.TryParse(rankDt.Rows[0]["purchase_price"].ToString(), out MinPrice))
                                MinPrice = 0;
                            txt += ", 단가 : " + (((price - MinPrice) / (MaxPrice - MinPrice)) * 100).ToString("#,##.0") + "%";
                            txt += "\n최대단가 : " + MaxPrice.ToString("#,##0.00") + ", 최소단가 : " + MinPrice.ToString("#,##0.00");
                        }
                        else
                        {
                            txt = "등록된 내역이 없습니다.";
                        }
                        dgvUnitPrice.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = txt;
                    }
                }
            }
        }


        #endregion

        #region 우클릭 메뉴
        //우클릭 메뉴 Create
        private void dgvUnitPrice_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvUnitPrice.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col >= 5 && row == -1)
                    {
                        if (dgvUnitPrice.Rows.Count == 0)
                            current_colIndex = col;
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("거래처 삭제");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvUnitPrice, e.Location);
                    }
                    else
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("역대단가 확인");
                        m.Items.Add("매입단가 삭제");
                        //Event Method
                        m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                        //Create 
                        m.BackColor = Color.White;
                        m.Show(dgvUnitPrice, e.Location);
                    }
                }
            }
            catch
            {

            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            try
            {
                switch (e.ClickedItem.Text)
                {
                    case "거래처 삭제":
                        for (int i = dgvUnitPrice.Columns.Count - 1; i >= 9; i--)
                        {
                            if (dgvUnitPrice.Rows.Count > 0)
                            {
                                if (dgvUnitPrice.Rows[0].Cells[i].Selected && dgvUnitPrice.Rows[dgvUnitPrice.Rows.Count - 1].Cells[i].Selected)
                                {
                                    dgvUnitPrice.Columns.Remove(dgvUnitPrice.Columns[i]);
                                }
                            }
                            else
                            {
                                dgvUnitPrice.Columns.Remove(dgvUnitPrice.Columns[current_colIndex]);
                                break;
                            }
                        }
                        break;
                    case "역대단가 확인":
                        for (int i = dgvUnitPrice.Columns.Count - 1; i >= 9; i--)
                        {
                            List<string[]> productList = new List<string[]>();
                            foreach (DataGridViewCell cell in dgvUnitPrice.SelectedCells)
                            {
                                if (cell.ColumnIndex >= 5)
                                {
                                    string[] product = new string[6];
                                    product[0] = dgvUnitPrice.Rows[cell.RowIndex].Cells["product"].Value.ToString();
                                    product[1] = dgvUnitPrice.Rows[cell.RowIndex].Cells["origin"].Value.ToString();
                                    product[2] = dgvUnitPrice.Rows[cell.RowIndex].Cells["sizes"].Value.ToString();
                                    product[3] = dgvUnitPrice.Rows[cell.RowIndex].Cells["weight"].Value.ToString();

                                    double price;
                                    if (dgvUnitPrice.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value == null || !double.TryParse(dgvUnitPrice.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value.ToString(), out price))
                                    {
                                        price = 0;
                                    }
                                    product[4] = price.ToString("#,##0.00");

                                    productList.Add(product);
                                }
                            }
                            if (productList.Count > 0)
                            { 
                                PurchaseManager.GraphManager gm = new GraphManager(um, productList);
                                gm.Show();
                            }
                        }
                        break;
                    case "매입단가 삭제":
                        if (dgvUnitPrice.SelectedCells.Count > 0)
                        {
                            DateTime updatetime;
                            if (!DateTime.TryParse(txtUpdattime.Text, out updatetime))
                            {
                                MessageBox.Show(this,"등록일자를 선택해주세요.");
                                this.Activate();
                                return;
                            }
                            if (MessageBox.Show(this,"선택하신 매입내역을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {   
                                DataGridViewCell cell = dgvUnitPrice.SelectedCells[0];
                                DataGridViewRow row = dgvUnitPrice.Rows[cell.RowIndex];
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                PurchasePriceModel model = new PurchasePriceModel();
                                model.company = dgvUnitPrice.Columns[cell.ColumnIndex].Name;
                                model.product = row.Cells["product"].Value.ToString();
                                model.origin = row.Cells["origin"].Value.ToString();
                                model.sizes = row.Cells["sizes"].Value.ToString();
                                model.unit = row.Cells["weight"].Value.ToString();
                                model.updatetime = updatetime.ToString("yyyy-MM-dd");

                                StringBuilder sql = purchasePriceRepository.DeletePurchasePrice2(model);
                                sqlList.Add(sql);

                                if (commonRepository.UpdateTran(sqlList) == -1)
                                {
                                    MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                                    this.Activate();
                                }
                                else
                                {
                                    cell.Value = "";
                                    cell.Style.ForeColor = Color.Black;
                                    cell.ToolTipText = "";
                                }
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
        #endregion

        
    }
}
