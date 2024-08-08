using AdoNetWindow.Model;
using Repositories;
using Repositories.Domestic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace AdoNetWindow.Domestic
{
    public partial class SimpleDomesticManager : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IDomesticRepository domesticRepository = new DomesticRepository();
        UsersModel um;
        DataGridViewRow row;
        public SimpleDomesticManager(UsersModel um, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit, DataGridViewRow row)
        {
            InitializeComponent();
            this.um = um;
            this.row = row;
            //품목정보
            dgvProduct.Rows.Clear();
            int n = dgvProduct.Rows.Add();
            dgvProduct.Rows[n].Cells["product"].Value = product;
            dgvProduct.Rows[n].Cells["origin"].Value = origin;
            dgvProduct.Rows[n].Cells["sizes"].Value = sizes;
            dgvProduct.Rows[n].Cells["unit"].Value = unit;
            dgvProduct.Rows[n].Cells["price_unit"].Value = price_unit;
            dgvProduct.Rows[n].Cells["unit_count"].Value = unit_count;
            dgvProduct.Rows[n].Cells["seaover_unit"].Value = seaover_unit;

            if(row.Cells["domestic_id"].Value != null)
                lbId.Text = row.Cells["domestic_id"].Value.ToString();

            DgvEtcExpenseRefresh();
            GetData();
        }

        private void GetData()
        {
            
            txtWarehouseDate.Text = row.Cells["warehousing_date"].Value.ToString();
            txtRealQty.Text = row.Cells["qty"].Value.ToString();
            txtTotalCostPrice.Text = row.Cells["etd_cost_price"].Value.ToString();

            int domestic_id;
            if (int.TryParse(lbId.Text, out domestic_id))
            {
                DataTable domesticDt = domesticRepository.GetDomestic(domestic_id.ToString());
                if (domesticDt.Rows.Count > 0)
                {
                    double transportation_fee;
                    if (!double.TryParse(domesticDt.Rows[0]["transportation_fee"].ToString(), out transportation_fee))
                        transportation_fee = 0;
                    txtTransportationFee.Text = transportation_fee.ToString("#,##0");

                    double material_price;
                    if (!double.TryParse(domesticDt.Rows[0]["material_price"].ToString(), out material_price))
                        material_price = 0;

                    txtCostPrice.Text = material_price.ToString("#,##0");
                }


                DataTable domesticExpenseDt = domesticRepository.GetDomesticExpense(domestic_id.ToString());
                if (domesticExpenseDt.Rows.Count > 0)
                {
                    dgvEtcExpense.Rows.Clear();
                    for (int i = 0; i < domesticExpenseDt.Rows.Count; i++)
                    {
                        int n = dgvEtcExpense.Rows.Add();
                        dgvEtcExpense.Rows[n].Cells["division"].Value = domesticExpenseDt.Rows[i]["division"].ToString();
                        double expense_price;
                        if (!double.TryParse(domesticExpenseDt.Rows[i]["expense_price"].ToString(), out expense_price))
                            expense_price = 0;
                        dgvEtcExpense.Rows[n].Cells["expense_amount"].Value = expense_price.ToString("#,##0");
                        double expense_price_per_box;
                        if (!double.TryParse(domesticExpenseDt.Rows[i]["expense_price_per_box"].ToString(), out expense_price_per_box))
                            expense_price_per_box = 0;
                        dgvEtcExpense.Rows[n].Cells["expense_amount_per_box"].Value = expense_price_per_box.ToString("#,##0");
                        dgvEtcExpense.Rows[n].Cells["is_vat"].Value = domesticExpenseDt.Rows[i]["is_vat"].ToString();
                    }
                }
            }
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


        #region Key event
        private void txtTransportationFee_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double transportFee;
                if (!double.TryParse(txtTransportationFee.Text, out transportFee))
                    transportFee = 0;
                txtTransportationFee.Text = transportFee.ToString("#,##0");
                double realQty;
                if (!double.TryParse(txtRealQty.Text, out realQty))
                    realQty = 0;

                txtTransportationFeePerBox.Text = (transportFee / realQty).ToString("#,##0");
                CalculateCostPrice();
            }
        }
        #endregion

        #region 창고, 기타비용
        private void dgvEtcExpense_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                double qty;
                if (!double.TryParse(txtRealQty.Text, out qty))
                    qty = 0;

                if (dgvEtcExpense.Columns[e.ColumnIndex].Name == "expense_amount")
                {
                    double expense_amount;
                    if (dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null
                        || !double.TryParse(dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out expense_amount))
                        expense_amount = 0;

                    dgvEtcExpense.Rows[e.RowIndex].Cells["expense_amount_per_box"].Value = (expense_amount / qty).ToString("#,##0");
                    CalculateCostPrice();
                }
                else if (dgvEtcExpense.Columns[e.ColumnIndex].Name == "expense_amount_per_box")
                {
                    double expense_amount;
                    if (dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null
                        || !double.TryParse(dgvEtcExpense.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out expense_amount))
                        expense_amount = 0;

                    dgvEtcExpense.Rows[e.RowIndex].Cells["expense_amount"].Value = (expense_amount * qty).ToString("#,##0");
                    CalculateCostPrice();
                }
            }
        }
        private void dgvEtcExpense_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvEtcExpense.Columns[e.ColumnIndex].Name == "btnDelete")
                    dgvEtcExpense.Rows.Remove(dgvEtcExpense.Rows[e.RowIndex]);
            }
        }
        #endregion

        #region Method
        private void CalculateCostPrice()
        {
            double cost_price;
            if (!double.TryParse(txtCostPrice.Text, out cost_price))
                cost_price = 0;
            txtCostPrice.Text = cost_price.ToString("#,##0");
            double transportationFeePerBox;
            if (!double.TryParse(txtTransportationFeePerBox.Text, out transportationFeePerBox))
                transportationFeePerBox = 0;
            double total_etc_expense = 0;
            for (int i = 0; i < dgvEtcExpense.Rows.Count; i++)
            {
                double expense_amount_per_box;
                if (dgvEtcExpense.Rows[i].Cells["expense_amount_per_box"].Value == null
                    || !double.TryParse(dgvEtcExpense.Rows[i].Cells["expense_amount_per_box"].Value.ToString(), out expense_amount_per_box))
                    expense_amount_per_box = 0;

                total_etc_expense += expense_amount_per_box;
            }

            txtTotalCostPrice.Text = (cost_price + transportationFeePerBox + total_etc_expense).ToString("#,##0");
        }
        #endregion


        #region Button
        private void btnAddDomestic_Click(object sender, EventArgs e)
        {
            int n = dgvEtcExpense.Rows.Add();
            dgvEtcExpense.Rows[n].Cells["is_vat"].Value = true;
            dgvEtcExpense.Focus();
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            
            int id;
            if (!int.TryParse(lbId.Text, out id))
                id = 0;
            if (id == 0)
                id = commonRepository.GetNextId("t_domestic", "id");


            DateTime warehousing_date;
            if (txtWarehouseDate.Text.Trim() == String.Empty || !DateTime.TryParse(txtWarehouseDate.Text, out warehousing_date))
            {
                MessageBox.Show(this,"입고일자를 확인해주세요.");
                this.Activate();
                return;
            }
            if (row == null)
                return;
            DomesticModel model = new DomesticModel();
            model.id = id;
            model.sub_id = 1;
            model.status = "선적전";
            model.warehousing_date = warehousing_date.ToString("yyyy-MM-dd");
            model.product = dgvProduct.Rows[0].Cells["product"].Value.ToString();
            model.origin = dgvProduct.Rows[0].Cells["origin"].Value.ToString();
            model.sizes = dgvProduct.Rows[0].Cells["sizes"].Value.ToString();
            model.unit = dgvProduct.Rows[0].Cells["unit"].Value.ToString();
            model.price_unit = dgvProduct.Rows[0].Cells["price_unit"].Value.ToString();
            model.unit_count = dgvProduct.Rows[0].Cells["unit_count"].Value.ToString();
            model.seaover_unit = dgvProduct.Rows[0].Cells["seaover_unit"].Value.ToString();
            //수량
            double qty;
            if (!double.TryParse(txtRealQty.Text, out qty))
            {
                MessageBox.Show(this,"수량을 입력해주세요.");
                this.Activate();
                return;
            }
            model.qty = qty;
            //재료단가
            double material_price;
            if (!double.TryParse(txtCostPrice.Text, out material_price))
                material_price = 0;
            model.material_price = material_price;
            //제품원가
            double cost_price;
            if (!double.TryParse(txtTotalCostPrice.Text, out cost_price))
                cost_price = 0;
            model.cost_price = cost_price;
            //운반비
            double transportation_fee;
            if (!double.TryParse(txtTransportationFee.Text, out transportation_fee))
                transportation_fee = 0;
            model.transportation_fee = transportation_fee;
            model.is_vat_transportation = cbTransportationFeeVat.Checked;

            model.edit_user = um.user_name;
            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            List<StringBuilder> sqlList = new List<StringBuilder>();

            //T_Domestic
            StringBuilder sql = domesticRepository.DeleteDomestic(model);
            sqlList.Add(sql);

            sql = domesticRepository.InsertDomestic(model);
            sqlList.Add(sql);

            //T_Domestic_expense
            DomesticExpenseModel eModel = new DomesticExpenseModel();
            eModel.domestic_id = model.id;
            sql = domesticRepository.DeleteDomesticExpense(eModel);
            sqlList.Add(sql);

            for (int i = 0; i < dgvEtcExpense.Rows.Count; i++)
            {
                eModel = new DomesticExpenseModel();
                eModel.domestic_id = model.id;
                if (dgvEtcExpense.Rows[i].Cells["expense_amount"].Value == null)
                    eModel.division = "";
                else
                    eModel.division = dgvEtcExpense.Rows[i].Cells["expense_amount"].Value.ToString();

                double expense_amount;
                if (dgvEtcExpense.Rows[i].Cells["expense_amount"].Value == null || !double.TryParse(dgvEtcExpense.Rows[i].Cells["expense_amount"].Value.ToString(), out expense_amount))
                    expense_amount = 0;
                eModel.expense_price = expense_amount;

                double expense_amount_per_box;
                if (dgvEtcExpense.Rows[i].Cells["expense_amount_per_box"].Value == null || !double.TryParse(dgvEtcExpense.Rows[i].Cells["expense_amount_per_box"].Value.ToString(), out expense_amount_per_box))
                    expense_amount_per_box = 0;
                eModel.expense_price_per_box = expense_amount_per_box;

                bool is_vat = Convert.ToBoolean(dgvEtcExpense.Rows[i].Cells["is_vat"].Value);
                eModel.is_vat = is_vat;

                eModel.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                eModel.edit_user = um.user_name;
                sql = domesticRepository.InsertDomesticExpense(eModel);
                sqlList.Add(sql);
            }
            //MSG
            if (MessageBox.Show(this, "국내수입 내역을 저장하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                this.Activate();
                return;
            }
            //Execute
            if (commonRepository.UpdateTran(sqlList) == -1)
            {
                MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                this.Activate();
            }
            else
            {
                row.Cells["domestic_id"].Value = model.id;
                row.Cells["qty"].Value = model.qty.ToString("#,##0");
                row.Cells["etd_cost_price"].Value = model.cost_price.ToString("#,##0");
            }        
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtWarehouseDate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void txtWarehouseDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tb = (TextBox)sender;
                if (tb.Name == "txtWarehouseDate")
                {
                    DateTime dt;
                    if (DateTime.TryParse(tb.Text, out dt))
                        tb.Text = dt.ToString("yyyy-MM-dd");
                }
            }
        }

        private void txtRealQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {               
                double transportFee;
                if (!double.TryParse(txtTransportationFee.Text, out transportFee))
                    transportFee = 0;
                txtTransportationFee.Text = transportFee.ToString("#,##0");
                double realQty;
                if (!double.TryParse(txtRealQty.Text, out realQty))
                    realQty = 0;
                txtRealQty.Text = realQty.ToString("#,##0");

                txtTransportationFeePerBox.Text = (transportFee / realQty).ToString("#,##0");
                CalculateCostPrice();
            }
        }

        private void SimpleDomesticManager_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnRegister.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }

            }
        }
    }
}
