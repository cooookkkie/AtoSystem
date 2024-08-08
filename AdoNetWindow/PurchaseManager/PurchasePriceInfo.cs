using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using AdoNetWindow.Product;
using Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.PurchaseManager
{
    public partial class PurchasePriceInfo : Form
    {
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        ICommonRepository commonRepository = new CommonRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        ICompanyRepository companyRepository = new CompanyRepository();
        UsersModel um;
        PurchaseManager.PurchaseUnitManager pcum;
        int id;
        public PurchasePriceInfo(UsersModel uModel, PurchaseManager.PurchaseUnitManager pcuManager)
        {
            InitializeComponent();
            id = purchasePriceRepository.GetNextId();
            txtUpdatetime.Text = DateTime.Now.ToString("yyyy-MM-dd");
            pcum = pcuManager;
            um = uModel;
            
        }
        public PurchasePriceInfo(UsersModel uModel, PurchaseManager.PurchaseUnitManager pcuManager, string pid)
        {
            InitializeComponent();
            id = Convert.ToInt32(pid);
            pcum = pcuManager;
            um = uModel;
        }
        private void PurchasePriceInfo_Load(object sender, EventArgs e)
        {
            DataTable productDt = purchasePriceRepository.GetPurchasePrice2("", "", "", "", "", "", "", false, "", id.ToString());
            if (productDt != null && productDt.Rows.Count > 0)
            {
                DataRow dr = productDt.Rows[0];
                DateTime updatetime = Convert.ToDateTime(dr["updatetime"].ToString());
                txtUpdatetime.Text = updatetime.ToString("yyyy-MM-dd");
                txtProduct.Text = dr["product"].ToString();
                txtOrigin.Text = dr["origin"].ToString();
                txtSizes.Text = dr["sizes"].ToString();
                txtUnit.Text = dr["unit"].ToString();
                txtCostUnit.Text = dr["cost_unit"].ToString();
                cbCompany.Text = dr["cname"].ToString();
                double purchase_price = Convert.ToDouble(dr["purchase_price"].ToString());
                txtPurchasePrice.Text = purchase_price.ToString("#,##0.00");
                txtManager.Text = dr["edit_user"].ToString();
                bool is_private = Convert.ToBoolean(dr["is_private"].ToString());
                cbPrivate.Checked = is_private;
                double fixed_tariff = Convert.ToDouble(dr["fixed_tariff"].ToString());
                bool weight_calculate = Convert.ToBoolean(dr["weight_calculate"].ToString());
                if (weight_calculate)
                    rbWeightCalculate.Checked = true;
                else
                    rbTrayCalculate.Checked = true;
                txtFixedTariff.Text = fixed_tariff.ToString("#,##0");
                bool is_FOB = Convert.ToBoolean(dr["is_FOB"].ToString());
                rbFOB.Checked = is_FOB;
            }
            else
                txtManager.Text = um.user_name;
        }

        #region Key Event
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Libs.Tools.Common common = new Libs.Tools.Common();
            Control tb = common.FindFocusedControl(this);
            {
                switch (keyData)
                {
                    case Keys.Up:
                        tb.Parent.SelectNextControl(ActiveControl, false, true, true, true);
                        return true;
                    case Keys.Down:
                        tb.Parent.SelectNextControl(ActiveControl, true, true, true, true);
                        return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
        private void txtUpdatetime_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(65293)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
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
        private void txtIncidentalExpense_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)46 || e.KeyChar == (Char)45 || e.KeyChar == (Char)65294))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void PurchasePriceInfo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        PurchasePriceInsert();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:

                        ProductManager ps = new ProductManager(um);

                        
                        // 부모 Form의 좌표, 크기를 계산
                        int mainformX = this.Location.X;
                        int mainformY = this.Location.Y;
                        int mainfromWidth = this.Size.Width;
                        int mainfromHeight = this.Size.Height;

                        // 자식 Form의 크기를 계산
                        int childformwidth = ps.Size.Width;
                        int childformheight = ps.Size.Height;
                        Point p = new Point(mainformX + (mainfromWidth / 2) - (childformwidth / 2), mainformY + (mainfromHeight / 2) - (childformheight / 2));
                        string[] selectProduct = ps.GetProduct(txtProduct.Text
                                                               , txtOrigin.Text
                                                               , txtSizes.Text
                                                               , txtUnit.Text);

                        if (selectProduct != null)
                        {
                            txtProduct.Text = selectProduct[0];
                            txtOrigin.Text = selectProduct[1];
                            txtSizes.Text = selectProduct[2];
                            txtUnit.Text = selectProduct[3];
                        }


                        break;
                    case Keys.F9:
                        Company.CompanyManager cm = new Company.CompanyManager(um, true);
                        string company = cm.GetCompany();
                        if (company != null)
                        {
                            cbCompany.Text = company;
                        }
                        break;
                }
            }
        }
        #endregion

        #region Method
        private bool CheckProduct()
        {
            bool isFlag = false;
            DataTable dt = productOtherCostRepository.GetProductAsOne(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, "");
            if (dt.Rows.Count != 1)
            {

                ProductSearching ps = new ProductSearching(um, txtProduct.Text
                                                                    , txtOrigin.Text
                                                                    , txtSizes.Text
                                                                    , txtUnit.Text);
                // 부모 Form의 좌표, 크기를 계산
                int mainformX = this.Location.X;
                int mainformY = this.Location.Y;
                int mainfromWidth = this.Size.Width;
                int mainfromHeight = this.Size.Height;

                // 자식 Form의 크기를 계산
                int childformwidth = ps.Size.Width;
                int childformheight = ps.Size.Height;
                Point p = new Point(mainformX + (mainfromWidth / 2) - (childformwidth / 2), mainformY + (mainfromHeight / 2) - (childformheight / 2));
                string[] selectProduct = ps.GetProduct(p);

                if (selectProduct != null)
                {
                    txtProduct.Text = selectProduct[0];
                    txtOrigin.Text = selectProduct[1];
                    txtSizes.Text = selectProduct[2];
                    txtUnit.Text = selectProduct[3];
                    isFlag = true;
                }
            }
            else
            {
                isFlag = true;
            }

            return isFlag;
        }

        private bool Validation()
        {
            bool isFlag = true;
            if (string.IsNullOrEmpty(txtProduct.Text) || string.IsNullOrEmpty(txtOrigin.Text) || string.IsNullOrEmpty(txtSizes.Text))
            {
                isFlag = false;

                MessageBox.Show(this, "품목|원산지|규격을 입력해주세요.");
                this.Activate();
                return isFlag;
            }
            double d;
            if (string.IsNullOrEmpty(txtPurchasePrice.Text))
            {
                txtPurchasePrice.Text = "0";
            }
            if (!double.TryParse(txtPurchasePrice.Text, out d))
            {
                MessageBox.Show(this,"매입단가는 숫자 형식으로만 입력해주세요.");
                this.Activate();
                isFlag = false;
                return isFlag;
            }

            if (string.IsNullOrEmpty(cbCompany.Text))
            {
                MessageBox.Show(this,"거래처를 입력해주세요.");
                this.Activate();
                isFlag = false;
                return isFlag;
            }
            else
            { 
                CompanyModel cModel = new CompanyModel();
                cModel = companyRepository.GetCompanyAsOne2(cbCompany.Text);
                if (cModel == null)
                {
                    MessageBox.Show(this,"거래처 정보를 찾을 수 없습니다.");
                    this.Activate();
                    isFlag = false;
                    return isFlag;
                }
            }

            return isFlag;
        }

        private void PurchasePriceInsert()
        {
            //Validation
            if (!Validation())
            {
                return;
            }
            else if(!CheckProduct())
            {
                return;
            }
            //Messagebox
            if (MessageBox.Show(this,"등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //Sql Execute
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql;
            //Delete
            sql = purchasePriceRepository.DeletePurchasePrice(id.ToString());
            sqlList.Add(sql);
            //Insert
            PurchasePriceModel ppm = new PurchasePriceModel();
            ppm.id = id;
            ppm.product = txtProduct.Text;
            ppm.origin = txtOrigin.Text;
            ppm.sizes = txtSizes.Text;
            ppm.unit = txtUnit.Text;
            ppm.cost_unit = txtCostUnit.Text;
            ppm.purchase_price = Convert.ToDouble(txtPurchasePrice.Text);
            ppm.weight_calculate = rbWeightCalculate.Checked;
            ppm.updatetime = txtUpdatetime.Text;
            ppm.edit_user = txtManager.Text;
            ppm.is_private = cbPrivate.Checked;
            ppm.is_FOB = rbFOB.Checked;

            double fixed_tariff;
            if (!double.TryParse(txtFixedTariff.Text, out fixed_tariff))
                fixed_tariff = 0;
            ppm.fixed_tariff = fixed_tariff;


            CompanyModel cModel = new CompanyModel();
            cModel = companyRepository.GetCompanyAsOne2(cbCompany.Text);
            ppm.company = cModel.id.ToString();

            sql = purchasePriceRepository.InsertPurchasePrice(ppm);
            sqlList.Add(sql);
                    
            //Execute
            int result = customsRepository.UpdateCustomTran(sqlList);
            if (result == -1)
            {
                MessageBox.Show(this,"등록 중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                pcum.GetData();
                MessageBox.Show(this,"등록완료");
                this.Dispose();
                this.Activate();
            }
        }

        private void PurchasePriceDelete()
        {
            //Messagebox
            if (MessageBox.Show(this,"삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            //Sql Execute
            List<StringBuilder> sqlList = new List<StringBuilder>();
            StringBuilder sql;
            //Delete
            sql = purchasePriceRepository.DeletePurchasePrice(id.ToString());
            sqlList.Add(sql);
            //Execute
            int result = customsRepository.UpdateCustomTran(sqlList);
            if (result == -1)
            {
                MessageBox.Show(this,"삭제 중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                pcum.GetData();
                
                MessageBox.Show(this,"삭제완료");
                this.Activate();
                this.Dispose();
            }
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            PurchasePriceInsert();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            PurchasePriceDelete();
        }
        private void btnCalendarUpdatetime_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
            {
                txtUpdatetime.Text = sdate;
            }
        }
        #endregion

        private void cbCompany_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string txt = cbCompany.Text;
                DataTable companyDt = commonRepository.SelectAsOneLike("t_company", "name", "name", cbCompany.Text);
                if (companyDt.Rows.Count > 0)
                {
                    cbCompany.DataSource = companyDt;
                    cbCompany.DisplayMember = "name";
                }
            }
        }
    }
}
