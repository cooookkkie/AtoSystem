using AdoNetWindow.Model;
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
    public partial class ProductAssortBatchInput : Form
    {
        UsersModel um;
        CostAccountingGroup caig;
        public ProductAssortBatchInput(UsersModel uModel, CostAccountingGroup caiGroup)
        {
            InitializeComponent();
            um = uModel;
            caig = caiGroup;
        }

        private void ProductAssortBatchInput_Load(object sender, EventArgs e)
        {
            SetColumnHeaderStyle();
        }
        #region Method
        private void SetColumnHeaderStyle()
        {
            DataGridView dgv = this.dgvProduct;

            dgv.Columns["sizes2"].Visible = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgv.Columns["assort"].DefaultCellStyle.BackColor = Color.FromArgb(246, 248, 252);
        }

        public void SetProduct(List<DataGridViewRow> product)
        {
            //거래처 중복삭제
            string[] productList = new string[product.Count];
            for (int i = 0; i < product.Count; i++)
            {
                productList[i] = product[i].Cells["product"].Value.ToString()
                    + "^" + product[i].Cells["origin"].Value.ToString()
                    + "^" + product[i].Cells["sizes"].Value.ToString()
                    + "^" + product[i].Cells["sizes2"].Value.ToString()
                    + "^" + product[i].Cells["box_weight"].Value.ToString()
                    + "^" + product[i].Cells["cost_unit"].Value.ToString();
            }
            productList = productList.Distinct().ToArray();

            //순회출력
            for (int i = 0; i < productList.Length; i++)
            {
                string[] splitTxt = productList[i].Split('^');
                if (splitTxt.Length > 0)
                {
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["product"].Value = splitTxt[0];
                    dgvProduct.Rows[n].Cells["origin"].Value = splitTxt[1];
                    dgvProduct.Rows[n].Cells["sizes"].Value = splitTxt[2];
                    dgvProduct.Rows[n].Cells["sizes2"].Value = splitTxt[3];
                    dgvProduct.Rows[n].Cells["box_weight"].Value = splitTxt[4];
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = splitTxt[5];
                }
            }
        }
        #endregion

        #region Key event
        private void ProductAssortBatchInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                { 
                    case Keys.A:
                        btnInput.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        #endregion

        #region Button
        private void btnInput_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> products = new List<DataGridViewRow>();
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                products.Add(dgvProduct.Rows[i]);
            }

            caig.SetBatchInput(products);
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion
    }
}
