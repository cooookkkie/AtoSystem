using AdoNetWindow.Model;
using Repositories.SEAOVER.Purchase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.Dashboard
{
    public partial class PurchasePriceDetail : Form
    {
        UsersModel um;
        IPurchaseRepository purchaseRepository = new PurchaseRepository();
        public PurchasePriceDetail(UsersModel uModel, string sdate, string product, string origin, string sizes, string unit, double purchase_margin)
        {
            InitializeComponent();
            purchase_margin = purchase_margin / 100;
            um = uModel;
            string[] dt = sdate.Split('/');
            DateTime sttdate = Convert.ToDateTime("20" + dt[0] + "-" + dt[1] + "-1");
            DateTime enddate = sttdate.AddMonths(1).AddDays(-1);

            txtSttdate.Text = sttdate.ToString("yyyy-MM-dd");
            txtEnddate.Text = enddate.ToString("yyyy-MM-dd");

            DataTable productDt = purchaseRepository.GetPurchase(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"), product, origin, sizes, unit);
            if (productDt.Rows.Count > 0)
            {
                double average_margin = 0;
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    int n = dgvPurchase.Rows.Add();
                    DataGridViewRow row = dgvPurchase.Rows[i];

                    row.Cells["purchase_date"].Value = Convert.ToDateTime(productDt.Rows[i]["매입일자"].ToString()).ToString("yyyy-MM-dd");
                    row.Cells["division"].Value = productDt.Rows[i]["매입구분"].ToString();
                    row.Cells["purchase_company"].Value = productDt.Rows[i]["매입처"].ToString();
                    row.Cells["warehouse"].Value = productDt.Rows[i]["보관처"].ToString();
                    row.Cells["remark"].Value = productDt.Rows[i]["적요"].ToString();

                    row.Cells["qty"].Value = Convert.ToDouble(productDt.Rows[i]["수량"].ToString()).ToString("#,##0");
                    row.Cells["unit_price"].Value = Convert.ToDouble(productDt.Rows[i]["단가"].ToString()).ToString("#,##0");

                    row.Cells["purchase_margin"].Value = (purchase_margin * 100).ToString("#,##0") + "%";
                    row.Cells["purchase_price"].Value = (Convert.ToDouble(productDt.Rows[i]["단가"].ToString()) * (1 - purchase_margin)).ToString("#,##0");

                    average_margin += Convert.ToDouble(row.Cells["purchase_price"].Value.ToString());
                    row.Cells["total_amount"].Value = Convert.ToDouble(productDt.Rows[i]["총매입금액"].ToString()).ToString("#,##0");
                }
                txtAveragePurchasePrice.Text = (average_margin / (productDt.Rows.Count)).ToString("#,##0");
            }
        }

        private void PurchasePriceDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        this.Dispose();
                        break;
                }
            }
        }
    }
}
