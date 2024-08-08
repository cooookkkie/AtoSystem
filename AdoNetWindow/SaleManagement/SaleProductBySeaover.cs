using AdoNetWindow.Model;
using Repositories.SEAOVER.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement
{
    public partial class SaleProductBySeaover : Form
    {
        ISalesRepository salesRepository = new SalesRepository();
        UsersModel um;
        string company_code;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public SaleProductBySeaover(UsersModel um, string company_code, string company)
        {
            InitializeComponent();
            this.um = um;
            this.company_code = company_code;
            txtCompany.Text = company;
            GetProduct();
        }

        #region Method
        private void GetProduct()
        {
            dgvProduct.Rows.Clear();
            DataTable productDt = salesRepository.GetHandlingItem(company_code);
            if (productDt.Rows.Count > 0)
            {
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = dgvProduct.Rows[n];

                    row.Cells["product"].Value = productDt.Rows[i]["품명"].ToString();
                    row.Cells["origin"].Value = productDt.Rows[i]["원산지"].ToString();
                    row.Cells["sizes"].Value = productDt.Rows[i]["규격"].ToString();
                    row.Cells["unit"].Value = productDt.Rows[i]["단위"].ToString();
                    DateTime sale_date;
                    if(DateTime.TryParse(productDt.Rows[i]["매출일자"].ToString(), out sale_date))
                        row.Cells["current_sale_date"].Value = sale_date.ToString("yyyy-MM-dd");
                    int sale_qty;
                    if (int.TryParse(productDt.Rows[i]["매출수"].ToString(), out sale_qty))
                        row.Cells["sale_qty"].Value = sale_qty.ToString("#,##0");
                    int average_qty;
                    if (int.TryParse(productDt.Rows[i]["평균매출수"].ToString(), out average_qty))
                        row.Cells["average_qty"].Value = average_qty.ToString("#,##0");

                }

                DataTable saleDt = salesRepository.GetHandlingItemDetail(company_code);
                if (saleDt.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        int cycle = 0;
                        int cnt = 0;
                        string whr = $"품명 = '{dgvProduct.Rows[i].Cells["product"].Value.ToString()}' "
                                + $"AND 원산지 = '{dgvProduct.Rows[i].Cells["origin"].Value.ToString()}' "
                                + $"AND 규격 = '{dgvProduct.Rows[i].Cells["sizes"].Value.ToString()}' "
                                + $"AND 단위 = '{dgvProduct.Rows[i].Cells["unit"].Value.ToString()}' ";
                        DataRow[] dr = saleDt.Select(whr);
                        if (dr.Length > 1)
                        {
                            DateTime sale_date;
                            DateTime.TryParse(dr[0]["매출일자"].ToString(), out sale_date);

                            
                            for (int j = 1; j < dr.Length; j++)
                            {
                                DateTime tmp_date;
                                DateTime.TryParse(dr[j]["매출일자"].ToString(), out tmp_date);

                                cycle += (tmp_date - sale_date).Days;
                                cnt++;
                            }
                        }

                        if(cycle > 0)
                            dgvProduct.Rows[i].Cells["order_cycle"].Value = (cycle / cnt).ToString("#,##0");
                    }
                }
            }
        }
        #endregion

        #region Button
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetProduct();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            int a = int.Parse(e.CellValue1.ToString()), b = int.Parse(e.CellValue2.ToString());
            e.SortResult = a.CompareTo(b);
            e.Handled = true;
        }
        #endregion

        #region Key event
        private void SaleProductBySeaover_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
        #endregion
    }
}
