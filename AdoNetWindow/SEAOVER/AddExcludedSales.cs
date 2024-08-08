using AdoNetWindow.Model;
using Repositories;
using Repositories.SaleProduct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER
{
    public partial class AddExcludedSales : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IProductExcludedSalesRepository productExcludedSalesRepository = new ProductExcludedSalesRepository();
        UsersModel um;
        SEAOVER.PriceComparison.PriceComparison pc = null;
        public AddExcludedSales(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;    
        }
        public AddExcludedSales(UsersModel uModel, SEAOVER.PriceComparison.PriceComparison pComparison, ProductExcludedSalesModel model)
        {
            InitializeComponent();
            um = uModel;
            lbId.Text = model.id.ToString();
            txtProduct.Text = model.product;
            txtOrigin.Text = model.origin;
            txtSizes.Text = model.sizes;
            txtUnit.Text = model.unit;
            txtPriceUnit.Text = model.price_unit;
            txtUnitCount.Text = model.unit_count;
            txtSeaoverUnit.Text = model.seaover_unit;

            txtSaleCompany.Text = model.sale_company;
            txtSaleDate.Text = model.sale_date;
            txtSaleQty.Text = model.sale_qty.ToString();
            txtRemark.Text = model.remark;

            txtSaleCompany.Focus();
            pc = pComparison;
            txtSaleDate.Text = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            this.ActiveControl = txtSaleDate;
        }

        #region Key event
        private void AddExcludedSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnAdd.PerformClick();
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
                        break;
                }
            }
        }
        private void txtSaleQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == (Char)45 || e.KeyChar == (Char)46 || e.KeyChar == (Char)47))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
            //이전값이 0일경우 삭제 후 입력
            Control con = (Control)sender;
            if (con.Text == "0")
            {
                con.Text = "";
            }
        }
        #endregion

        #region Button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            //유효성검사
            double saleQty;
            if (string.IsNullOrEmpty(txtProduct.Text))
            {
                MessageBox.Show(this, "품명을 입력해주세요.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtOrigin.Text))
            {
                MessageBox.Show(this, "원산지를 입력해주세요.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtSizes.Text))
            {
                MessageBox.Show(this, "규격을 입력해주세요.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtSeaoverUnit.Text))
            {
                MessageBox.Show(this, "씨오버단위를 입력해주세요.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtSaleDate.Text))
            {
                MessageBox.Show(this, "판매일자를 입력해주세요.");
                this.Activate();
                return;
            }
            else if (string.IsNullOrEmpty(txtSaleQty.Text) || !double.TryParse(txtSaleQty.Text, out saleQty))
            {
                MessageBox.Show(this, "판매수량을 확인해주세요.");
                this.Activate();
                return;
            }
            
            int id;
            if (!int.TryParse(lbId.Text, out id))
                id = commonRepository.GetNextId("t_product_excluded_sales", "id");

            List<StringBuilder> sqlList = new List<StringBuilder>();
            ProductExcludedSalesModel model = new ProductExcludedSalesModel();
            model.id = id;

            model.product = txtProduct.Text;
            model.origin = txtOrigin.Text;
            model.sizes = txtSizes.Text;
            model.unit = txtUnit.Text;
            model.price_unit = txtPriceUnit.Text;
            model.unit_count = txtUnitCount.Text;
            model.seaover_unit = txtSeaoverUnit.Text;
            model.sale_date = txtSaleDate.Text;
            model.sale_company = txtSaleCompany.Text;
            model.sale_qty = saleQty;
            model.remark = txtRemark.Text;

            StringBuilder sql = productExcludedSalesRepository.DeleteExcludedSales(id);
            sqlList.Add(sql);

            sql = productExcludedSalesRepository.InsertExcludedSales(model);
            sqlList.Add(sql);

            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                if (pc != null)
                    pc.SetExcludedSales();
                this.Dispose();
            }
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSaleDate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion


    }
}
