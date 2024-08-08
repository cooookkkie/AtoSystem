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

namespace AdoNetWindow.Domestic
{
    public partial class DomesticManager : Form
    {
        UsersModel um;
        public DomesticManager(UsersModel um, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit)
        {
            InitializeComponent();
            this.um = um;
            dgvProduct.Rows.Clear();
            int n = dgvProduct.Rows.Add();
            dgvProduct.Rows[n].Cells["product"].Value = product;
            dgvProduct.Rows[n].Cells["origin"].Value = origin;
            dgvProduct.Rows[n].Cells["sizes"].Value = sizes;
            dgvProduct.Rows[n].Cells["unit"].Value = unit;
            dgvProduct.Rows[n].Cells["price_unit"].Value = price_unit;
            dgvProduct.Rows[n].Cells["unit_count"].Value = unit_count;
            dgvProduct.Rows[n].Cells["seaover_unit"].Value = seaover_unit;
        }

        private void DomesticManager_Load(object sender, EventArgs e)
        {
            DgvEtcExpenseRefresh();
        }

        private void DgvEtcExpenseRefresh()
        {
            dgvEtcExpense.Rows.Clear();

            int n = dgvEtcExpense.Rows.Add();
            dgvEtcExpense.Rows[n].Cells["division"].Value = "상차비";
            dgvEtcExpense.Rows[n].Cells["is_vat"].Value = true;
            n = dgvEtcExpense.Rows.Add();
            dgvEtcExpense.Rows[n].Cells["division"].Value = "하차비";
            dgvEtcExpense.Rows[n].Cells["is_vat"].Value = true;
            n = dgvEtcExpense.Rows.Add();
            dgvEtcExpense.Rows[n].Cells["division"].Value = "냉장료";
            dgvEtcExpense.Rows[n].Cells["is_vat"].Value = true;
            n = dgvEtcExpense.Rows.Add();
            dgvEtcExpense.Rows[n].Cells["division"].Value = "입고비";
            dgvEtcExpense.Rows[n].Cells["is_vat"].Value = true;
            n = dgvEtcExpense.Rows.Add();
            dgvEtcExpense.Rows[n].Cells["division"].Value = "출고비";
            dgvEtcExpense.Rows[n].Cells["is_vat"].Value = true;
        }

        private void CalculateCostPrice()
        {
            double total_amount1;
            if (!double.TryParse(txtTotalAmount1.Text, out total_amount1))
                total_amount1 = 0;
            double total_amount2;
            if (!double.TryParse(txtTotalAmount2.Text, out total_amount2))
                total_amount2 = 0;
            double pricePerBox = 0;
            if (!double.TryParse(txtPricePerBox.Text, out pricePerBox))
                pricePerBox = 0;

            txtCostPrice.Text = (total_amount1 + total_amount2 + pricePerBox).ToString("#,##0");
        }


        private void rbWorkWeight_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWorkWeight.Checked)
            {
                txtRealQty.BackColor = Color.PeachPuff;
                txtYield.BackColor = Color.White;
            }
            if (rbYield.Checked)
            {
                txtRealQty.BackColor = Color.White;
                txtYield.BackColor = Color.PeachPuff;
            }
        }

        #region 원물 원가계산
        private void txtMaterialPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CalculateMaterialPrice();
            }
        }
        private void CalculateMaterialPrice()
        {
            double materialPrice;
            if (!double.TryParse(txtMaterialPrice.Text, out materialPrice))
                materialPrice = 0;
            txtMaterialPrice.Text = materialPrice.ToString("#,##0");

            double qty;
            if (!double.TryParse(txtQty.Text, out qty))
                qty = 0;
            txtQty.Text = qty.ToString("#,##0");

            double weight;
            if (!double.TryParse(txtWeight.Text, out weight))
                weight = 0;
            txtWeight.Text = weight.ToString("#,##0");

            double workWeight;
            if (!double.TryParse(txtWorkWeight.Text, out workWeight))
                workWeight = 0;
            txtWorkWeight.Text = workWeight.ToString("#,##0");

            double realQty;
            if (!double.TryParse(txtRealQty.Text, out realQty))
                realQty = 0;
            txtRealQty.Text = realQty.ToString("#,##0");

            double yield;
            if (!double.TryParse(txtYield.Text, out yield))
                yield = 0;
            
            double totalWeight = qty * weight;
            txtTotalWeight.Text = totalWeight.ToString("#,##0");

            //수량입력 방식
            if (rbWorkWeight.Checked)
            {
                double goodPointQty = totalWeight / workWeight;
                txtGoodPointQty.Text = goodPointQty.ToString("#,##0");

                yield = goodPointQty / realQty;
                txtYield.Text = yield.ToString("#,##0.00");
            }
            //수율입력 방식
            else
            {
                realQty = (qty * weight) / workWeight * (yield / 100);
                txtRealQty.Text = realQty.ToString("#,##0");

                double goodPointQty = totalWeight / workWeight;
                txtGoodPointQty.Text = goodPointQty.ToString("#,##0");
            }

            //수율Kg
            txtYieldPerKg.Text = (weight * (yield  / 100)).ToString("#,##0.00");
            //단가Kg
            txtPricePerKg.Text = (materialPrice / (weight * (yield / 100))).ToString("#,##0");
            //원물단가
            txtPricePerBox.Text = ((materialPrice / (weight * (yield / 100))) * workWeight).ToString("#,##0");

            CalculateCostPrice();
        }
        #endregion

        #region 작업비 운반비 계산
        private void txtWorkFeePerBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double workFee;
                if (!double.TryParse(txtWorkFeePerBox.Text, out workFee))
                    workFee = 0;
                txtWorkFeePerBox.Text = workFee.ToString("#,##0");

                double workWeight;
                if (!double.TryParse(txtWorkWeight.Text, out workWeight))
                    workWeight = 0;

                txtWorkFeePerKg.Text = (workFee / workWeight).ToString("#,##0");
                CalculateTotalAmount1();
            }
        }

        private void txtWorkFeePerKg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double workFee;
                if (!double.TryParse(txtWorkFeePerKg.Text, out workFee))
                    workFee = 0;
                txtWorkFeePerKg.Text = workFee.ToString("#,##0");

                double workWeight;
                if (!double.TryParse(txtWorkWeight.Text, out workWeight))
                    workWeight = 0;

                txtWorkFeePerBox.Text = (workFee * workWeight).ToString("#,##0");
                CalculateTotalAmount1();
            }
        }
        private void txtTransportationFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { 
                double transportFee;
                if(!double.TryParse(txtTransportationFee.Text, out transportFee))
                    transportFee = 0;
                txtTransportationFee.Text = transportFee.ToString("#,##0");
                double realQty;
                if (!double.TryParse(txtRealQty.Text, out realQty))
                    realQty = 0;

                txtTransportationFeePerBox.Text = (transportFee / realQty).ToString("#,##0");
                CalculateTotalAmount1();
            }
        }
        private void CalculateTotalAmount1()
        {
            double workFee;
            if (!double.TryParse(txtWorkFeePerBox.Text, out workFee))
                workFee = 0;
            double bagFee;
            if (!double.TryParse(txtBagFee.Text, out bagFee))
                bagFee = 0;
            double scotchTapeFee;
            if (!double.TryParse(txtScotchTapeFee.Text, out scotchTapeFee))
                scotchTapeFee = 0;
            double boxFee;
            if (!double.TryParse(txtBoxFee.Text, out boxFee))
                boxFee = 0;
            double transportFee;
            if (!double.TryParse(txtTransportationFeePerBox.Text, out transportFee))
                transportFee = 0;

            txtTotalAmount1.Text = (workFee + bagFee + scotchTapeFee + boxFee + transportFee).ToString("#,##0");
            CalculateCostPrice();
        }
        #endregion

        #region 창고비용, 기타비용
        private void btnAddDomestic_Click(object sender, EventArgs e)
        {
            int n = dgvEtcExpense.Rows.Add();
            dgvEtcExpense.Rows[n].Cells["is_vat"].Value = true;
            dgvEtcExpense.Focus();
        }

        private void dgvEtcExpense_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvEtcExpense.Columns[e.ColumnIndex].Name == "btnDelete")
                {
                    dgvEtcExpense.Rows.Remove(dgvEtcExpense.Rows[e.RowIndex]);
                }
            }
        }

        private void dgvEtcExpense_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                this.dgvEtcExpense.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEtcExpense_CellValueChanged);

                dgvEtcExpense.EndEdit();
                if (dgvEtcExpense.Columns[e.ColumnIndex].Name == "expense_amount_per_box")
                {
                    double expense_amount_per_box;
                    if (dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null
                        || !double.TryParse(dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out expense_amount_per_box))
                        expense_amount_per_box = 0;
                    if (expense_amount_per_box > 0)
                        dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = expense_amount_per_box.ToString("#,##0");

                    double realQty;
                    if (!double.TryParse(txtRealQty.Text, out realQty))
                        realQty = 0;

                    dgvEtcExpense.Rows[e.RowIndex].Cells["expense_amount"].Value = (expense_amount_per_box * realQty).ToString("#,##0");
                    CalculateTotalAmount2();
                }
                else if (dgvEtcExpense.Columns[e.ColumnIndex].Name == "expense_amount")
                {
                    double expense_amount;
                    if (dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null
                        || !double.TryParse(dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out expense_amount))
                        expense_amount = 0;
                    if (expense_amount > 0)
                        dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = expense_amount.ToString("#,##0");

                    double realQty;
                    if (!double.TryParse(txtRealQty.Text, out realQty))
                        realQty = 0;

                    dgvEtcExpense.Rows[e.RowIndex].Cells["expense_amount"].Value = (expense_amount / realQty).ToString("#,##0");
                    CalculateTotalAmount2();
                }
                
                this.dgvEtcExpense.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEtcExpense_CellValueChanged);
            }
        }

        private void CalculateTotalAmount2()
        {
            double total_expense_amount = 0;
            for (int i = 0; i < dgvEtcExpense.Rows.Count; i++)
            {
                double expense_amount;
                if (dgvEtcExpense.Rows[i].Cells["expense_amount_per_box"].Value == null
                    || !double.TryParse(dgvEtcExpense.Rows[i].Cells["expense_amount_per_box"].Value.ToString(), out expense_amount))
                    expense_amount = 0;

                total_expense_amount += expense_amount;
            }

            txtTotalAmount2.Text = total_expense_amount.ToString("#,##0");
            CalculateCostPrice();
        }
        #endregion

        private void btnCalendarWarehousingDate_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtWarehousingDate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void txtWarehousingDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                if (tb.Name == "txtWarehousingDate")
                {
                    DateTime dt;
                    if (DateTime.TryParse(tb.Text, out dt))
                        tb.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }
    }
}
