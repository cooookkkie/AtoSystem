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
using static AdoNetWindow.SaleManagement.SalesDashboard2.salesDashboard2;

namespace AdoNetWindow.SaleManagement.SalesDashboard2
{
    public partial class SelectProduct : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        List<SelectProductModel> list = new List<SelectProductModel>();
        UsersModel um;
        salesDashboard2 sd2;
        Libs.Tools.Common common = new Libs.Tools.Common();
        public SelectProduct(UsersModel um, salesDashboard2 sd2, List<SelectProductModel> list)
        {
            InitializeComponent();
            this.um = um;
            this.list = list;
            this.sd2 = sd2;
        }

        private void SelectProduct_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int n = dgvSelectProduct.Rows.Add();
                dgvSelectProduct.Rows[n].Cells["select_product"].Value = list[i].product;
                dgvSelectProduct.Rows[n].Cells["select_origin"].Value = list[i].origin;
            }
        }

        #region Button
        private void panel11_MouseClick(object sender, MouseEventArgs e)
        {
            AddProducts();
        }
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            AddProducts();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            list = new List<SelectProductModel>();
            if (dgvSelectProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvSelectProduct.Rows.Count; i++)
                {
                    SelectProductModel model = new SelectProductModel();
                    model.product = dgvSelectProduct.Rows[i].Cells["select_product"].Value.ToString();
                    model.origin = dgvSelectProduct.Rows[i].Cells["select_origin"].Value.ToString();

                    list.Add(model);
                }
            }
            this.Dispose();
        }
        #endregion

        #region Method
        public List<SelectProductModel> GetProducts()
        {
            this.ShowDialog();

            return list;
        }
        private void AddProducts()
        {
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (dgvProduct.Rows[i].Selected)
                {
                    bool isExist = false;
                    for (int j = 0; j < dgvSelectProduct.Rows.Count; j++)
                    {
                        if (dgvSelectProduct.Rows[j].Cells["select_product"].Value == dgvProduct.Rows[i].Cells["product"].Value
                            && dgvSelectProduct.Rows[j].Cells["select_origin"].Value == dgvProduct.Rows[i].Cells["origin"].Value)
                        {
                            isExist = true;
                            break;
                        }
                    }
                    if (!isExist)
                    {
                        int n = dgvSelectProduct.Rows.Add();
                        dgvSelectProduct.Rows[n].Cells["select_product"].Value = dgvProduct.Rows[i].Cells["product"].Value;
                        dgvSelectProduct.Rows[n].Cells["select_origin"].Value = dgvProduct.Rows[i].Cells["origin"].Value;
                    }
                }
            }
        }
        private void GetData()
        {
            dgvProduct.Rows.Clear();
            string[] col = new string[2];
            col[0] = "품명";
            col[1] = "원산지";
            string[] val = new string[2];
            val[0] = txtProduct.Text;
            val[1] = txtOrigin.Text;
            DataTable productDt = seaoverRepository.SelectData("업체별시세관리", "DISTINCT 품명, 원산지", um.seaover_id, col, val, "품명, 원산지");
            if (productDt != null && productDt.Rows.Count > 0)
            { 
                for(int i = 0; i < productDt.Rows.Count; i++) 
                {
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["product"].Value = productDt.Rows[i]["품명"].ToString();
                    dgvProduct.Rows[n].Cells["origin"].Value = productDt.Rows[i]["원산지"].ToString();
                }
            }
            dgvProduct.Focus();
        }
        #endregion

        #region Key event
        private void SelectProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode) 
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = string.Empty;
                        txtOrigin.Text = string.Empty;
                        txtProduct.Focus();
                        break;
                }
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearching.PerformClick();
        }
        #endregion

        private void dgvProduct_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                bool isExist = false;
                for (int j = 0; j < dgvSelectProduct.Rows.Count; j++)
                {
                    if (dgvSelectProduct.Rows[j].Cells["select_product"].Value == dgvProduct.Rows[e.RowIndex].Cells["product"].Value
                        && dgvSelectProduct.Rows[j].Cells["select_origin"].Value == dgvProduct.Rows[e.RowIndex].Cells["origin"].Value)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist)
                {
                    int n = dgvSelectProduct.Rows.Add();
                    dgvSelectProduct.Rows[n].Cells["select_product"].Value = dgvProduct.Rows[e.RowIndex].Cells["product"].Value;
                    dgvSelectProduct.Rows[n].Cells["select_origin"].Value = dgvProduct.Rows[e.RowIndex].Cells["origin"].Value;
                }
            }
        }
    }
}
