using AdoNetWindow.Model;
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

namespace AdoNetWindow.Product
{
    public partial class AddProduct : Form
    {
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        UsersModel um;
        public AddProduct(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }
        public AddProduct(UsersModel uModel, ProductOtherCostModel pModel)
        {
            InitializeComponent();
            um = uModel;

            txtGroup.Text = pModel.group_name;
            txtProduct.Text = pModel.product;
            txtOrigin.Text = pModel.origin;
            txtSIzes.Text = pModel.sizes;
            txtUnit.Text = pModel.unit;
            txtCostUnit.Text = pModel.cost_unit;
            if (pModel.weight_calculate)
                rbWeightCalculate.Checked = true;
            else
                rbWeightCalculate.Checked = true;

            txtCustom.Text = pModel.custom.ToString();
            txtTax.Text = pModel.tax.ToString();
            txtInExpense.Text = pModel.incidental_expense.ToString();
            txtProductionDays.Text = pModel.production_days.ToString();
            txtPurchaseMargin.Text = pModel.purchase_margin.ToString();
            txtBaseArounndMonth.Text = pModel.base_around_month.ToString();

            txtManager.Text = pModel.manager;
        }

        #region Method
        private void AddProductInfo()
        {
            if (string.IsNullOrEmpty(txtProduct.Text))
            {
                MessageBox.Show(this, "품목명을 입력해주세요.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtOrigin.Text))
            {
                MessageBox.Show(this, "원산지를 입력해주세요.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtSIzes.Text))
            {
                MessageBox.Show(this, "규격을 입력해주세요.");
                this.Activate();
                return;
            }

            if (MessageBox.Show(this, "임의 품목을 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProductOtherCostModel model = new ProductOtherCostModel();
                List<StringBuilder> sqlList = new List<StringBuilder>();
                model.group_name = txtGroup.Text;
                model.product = txtProduct.Text;
                model.origin = txtOrigin.Text;
                model.sizes = txtSIzes.Text;
                model.unit = txtUnit.Text;

                double cost_unit;
                if (!double.TryParse(txtCostUnit.Text, out cost_unit))
                    cost_unit = 0;
                model.cost_unit = cost_unit.ToString();
                double custom;
                if (!double.TryParse(txtCustom.Text, out custom))
                    custom = 0;
                model.custom = custom;
                double incidental_expense;
                if (!double.TryParse(txtInExpense.Text, out incidental_expense))
                    incidental_expense = 0;
                model.incidental_expense = incidental_expense;
                double tax;
                if (!double.TryParse(txtTax.Text, out tax))
                    tax = 0;
                model.tax = tax;
                double production_days;
                if (!double.TryParse(txtProductionDays.Text, out production_days))
                    production_days = 0;
                model.production_days = production_days;

                double purchase_margin;
                if (!double.TryParse(txtPurchaseMargin.Text, out purchase_margin))
                    purchase_margin = 0;
                model.purchase_margin = purchase_margin;

                model.manager = txtManager.Text;
                model.updatetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                model.edit_user = um.user_name;

                model.isMonth = false;
                model.show_sttdate = "NULL";
                model.show_enddate = "NULL";
                model.hide_date = "NULL";
                model.base_around_month = 2;
                model.remark = txtRemark.Text;

                StringBuilder sql = productOtherCostRepository.InsertProduct(model);
                sqlList.Add(sql);

                int result = productOtherCostRepository.UpdateTrans(sqlList);
                if (result == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    txtGroup.Text = String.Empty;
                    txtProduct.Text = String.Empty;
                    txtOrigin.Text = String.Empty;
                    txtSIzes.Text = String.Empty;
                    txtUnit.Text = String.Empty;
                    txtManager.Text = String.Empty;
                    txtCostUnit.Text = "0";
                    txtCustom.Text = "0";
                    txtInExpense.Text = "0";
                    txtTax.Text = "0";
                    MessageBox.Show(this, "등록완료");
                    this.Activate();
                }
            }
        }
        #endregion

        #region Key event
        private void AddProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.A:
                        AddProductInfo();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSIzes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtCustom.Text = "0";
                        txtInExpense.Text = "0";
                        txtTax.Text = "0";
                        txtProduct.Focus();
                        break;
                }
            }
        }

        private void txtCustom_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)46 || e.KeyChar == (Char)45 || e.KeyChar == (Char)65294))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Button
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtProduct.Text))
            {
                MessageBox.Show(this, "삭제할 내역이 없습니다.");
                this.Activate();
                return;
            }

            if (MessageBox.Show(this, "임의 품목을 삭제하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProductOtherCostModel model = new ProductOtherCostModel();
                List<StringBuilder> sqlList = new List<StringBuilder>();
                model.product = txtProduct.Text;
                model.origin = txtProduct.Text;
                model.sizes = txtProduct.Text;
                model.unit = txtProduct.Text;
                model.custom = Convert.ToDouble(txtCustom.Text);
                model.incidental_expense = Convert.ToDouble(txtInExpense.Text);
                model.tax = Convert.ToDouble(txtTax.Text);
                StringBuilder sql = productOtherCostRepository.DeleteProduct(model);
                sqlList.Add(sql);

                int result = productOtherCostRepository.UpdateTrans(sqlList);
                if (result == -1)
                {
                    MessageBox.Show(this, "등록 중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    MessageBox.Show(this, "삭제완료");
                    this.Activate();
                    this.Dispose();
                }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            AddProductInfo();
        }
        #endregion
    }
}
