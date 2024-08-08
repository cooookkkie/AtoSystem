using AdoNetWindow.Model;
using Repositories;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace AdoNetWindow.Pending
{
    public partial class PendingList2 : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        ICustomsRepository customsRepository = new CustomsRepository();
        double exchange_rate;
        UsersModel um;
        public PendingList2(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }

        public PendingList2(UsersModel uModel, string product, string origin, string sizes, string unit)
        {
            InitializeComponent();
            um = uModel;
            txtProduct.Text = product + ";";
            txtOrigin.Text = origin + ";";
            txtSizes.Text = sizes + ";";
            txtBoxWeight.Text = unit;
            cbExactly.Checked = true;
            GetData();
        }

        private void PendingList2_Load(object sender, EventArgs e)
        {
            nudSttYear.Value = DateTime.Now.AddYears(-1).Year;
            nudEndYear.Value = DateTime.Now.Year;
            SetHeaderStyleSetting();
            this.ActiveControl = txtProduct;
        }

        #region Method
        private void SetHeaderStyleSetting()
        {
            dgvProduct.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgvProduct.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            
            dgvProduct.Columns["ato_no"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgvProduct.Columns["ato_no"].HeaderCell.Style.ForeColor = Color.White;
            dgvProduct.Columns["ato_no"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);
            
            dgvProduct.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgvProduct.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgvProduct.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgvProduct.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgvProduct.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;
            dgvProduct.Columns["origin"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgvProduct.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgvProduct.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;
            dgvProduct.Columns["sizes"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);

            dgvProduct.Columns["trq_cost_price_per_box"].HeaderCell.Style.BackColor = Color.FromArgb(43, 100, 170);
            dgvProduct.Columns["trq_cost_price_per_box"].HeaderCell.Style.ForeColor = Color.White;

            dgvProduct.Columns["trq_cost_price"].HeaderCell.Style.BackColor = Color.FromArgb(43, 100, 170);
            dgvProduct.Columns["trq_cost_price"].HeaderCell.Style.ForeColor = Color.White;

            dgvProduct.Columns["unit_price"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgvProduct.Columns["unit_price"].HeaderCell.Style.ForeColor = Color.White;
            dgvProduct.Columns["unit_price"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);

            dgvProduct.Columns["unit_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["box_weight"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["contract_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["payment_exchange_rate"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvProduct.Columns["trq_cost_price_per_box"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvProduct.Columns["trq_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            
            
        }
        private void GetData()
        {
            exchange_rate = common.GetExchangeRateKEBBank("USD");
            dgvProduct.Rows.Clear();
            DataTable productDt = customsRepository.GetPendingList2(nudSttYear.Value.ToString(), nudEndYear.Value.ToString()
                , txtAtoNo.Text, txtContractNo.Text, txtShipper.Text, txtBlNo.Text, txtProduct.Text, cbExactly.Checked, txtOrigin.Text, txtSizes.Text, txtBoxWeight.Text, txtManager.Text
                , cbCcStatus.Text);

            for (int i = 0; i < productDt.Rows.Count; i++)
            {
                int n = dgvProduct.Rows.Add();
                DataGridViewRow row = dgvProduct.Rows[n];
                row.Cells["id"].Value = productDt.Rows[i]["id"].ToString();
                row.Cells["sub_id"].Value = productDt.Rows[i]["sub_id"].ToString();
                row.Cells["ato_no"].Value = productDt.Rows[i]["ato_no"].ToString();
                row.Cells["contract_year"].Value = productDt.Rows[i]["contract_year"].ToString();
                row.Cells["pi_date"].Value = productDt.Rows[i]["pi_date"].ToString();
                row.Cells["shipper"].Value = productDt.Rows[i]["shipper"].ToString();
                row.Cells["contracT_no"].Value = productDt.Rows[i]["contracT_no"].ToString();
                row.Cells["bl_no"].Value = productDt.Rows[i]["bl_no"].ToString();
                row.Cells["warehouse"].Value = productDt.Rows[i]["warehouse"].ToString();

                row.Cells["etd"].Value = productDt.Rows[i]["etd"].ToString();
                row.Cells["eta"].Value = productDt.Rows[i]["eta"].ToString();
                row.Cells["warehousing_date"].Value = productDt.Rows[i]["warehousing_date"].ToString();

                row.Cells["cc_status"].Value = productDt.Rows[i]["cc_status"].ToString();
                row.Cells["product"].Value = productDt.Rows[i]["product"].ToString();
                row.Cells["origin"].Value = productDt.Rows[i]["origin"].ToString();
                row.Cells["sizes"].Value = productDt.Rows[i]["sizes"].ToString();

                row.Cells["custom"].Value = productDt.Rows[i]["custom"].ToString();
                row.Cells["tax"].Value = productDt.Rows[i]["tax"].ToString();
                row.Cells["incidental_expense"].Value = productDt.Rows[i]["incidental_expense"].ToString();
                row.Cells["production_days"].Value = productDt.Rows[i]["production_days"].ToString();


                bool isWeight = Convert.ToBoolean(productDt.Rows[i]["weight_calculate"].ToString());
                

                double cost_unit;
                if (!double.TryParse(productDt.Rows[i]["cost_unit"].ToString(), out cost_unit))
                    cost_unit = 0;
                row.Cells["cost_unit"].Value = cost_unit;

                if (!isWeight && cost_unit == 0)
                    isWeight = true;
                row.Cells["weight_calculate"].Value = isWeight;

                double unit_price;
                if (!double.TryParse(productDt.Rows[i]["unit_price"].ToString(), out unit_price))
                    unit_price = 0;
                row.Cells["unit_price"].Value = unit_price.ToString("#,##0.00");

                double box_weight;
                if (!double.TryParse(productDt.Rows[i]["box_weight"].ToString(), out box_weight))
                    box_weight = 1;
                row.Cells["box_weight"].Value = box_weight;

                double custom;
                if (!double.TryParse(productDt.Rows[i]["custom"].ToString(), out custom))
                    custom = 0;
                custom /= 100;
                double tax;
                if (!double.TryParse(productDt.Rows[i]["tax"].ToString(), out tax))
                    tax = 0;
                tax /= 100;
                double incidental_expense;
                if (!double.TryParse(productDt.Rows[i]["incidental_expense"].ToString(), out incidental_expense))
                    incidental_expense = 0;
                incidental_expense /= 100;

                double payment_ex_rate = Convert.ToDouble(productDt.Rows[i]["payment_ex_rate"].ToString());
                if (payment_ex_rate == 0)
                {
                    payment_ex_rate = exchange_rate;
                    row.Cells["payment_exchange_rate"].Style.ForeColor = Color.Red;
                }   
                row.Cells["payment_exchange_rate"].Value = payment_ex_rate.ToString("#,##0.00");

                double shipping_ex_rate = Convert.ToDouble(productDt.Rows[i]["shipping_ex_rate"].ToString());
                if (shipping_ex_rate == 0)
                {
                    shipping_ex_rate = exchange_rate;
                    row.Cells["shipping_exchange_rate"].Style.ForeColor = Color.Red;
                }
                row.Cells["shipping_exchange_rate"].Value = shipping_ex_rate.ToString("#,##0.00");

                double contract_qty = Convert.ToDouble(productDt.Rows[i]["quantity_on_paper"].ToString());
                row.Cells["contract_qty"].Value = contract_qty.ToString("#,##0");

                /*row.Cells["cost_price_per_box"].Value = cost_price.ToString("#,##0");
                row.Cells["cost_price"].Value = (cost_price * contract_qty).ToString("#,##0");*/

                double cost_price;
                if (!double.TryParse(productDt.Rows[i]["before_trq"].ToString(), out cost_price))
                {
                    if (isWeight)
                        cost_price = box_weight * payment_ex_rate * unit_price * (1 + custom + tax + incidental_expense);
                    else
                        cost_price = cost_unit * payment_ex_rate * unit_price * (1 + custom + tax + incidental_expense);
                }

                //2023-05-30 대행일 경우 매출단가 +3.5%
                if (row.Cells["ato_no"].Value.ToString().Contains("AD")
                    || row.Cells["ato_no"].Value.ToString().Contains("DW")
                    || row.Cells["ato_no"].Value.ToString().Contains("OD")
                    || row.Cells["ato_no"].Value.ToString().Contains("JD")
                    || row.Cells["ato_no"].Value.ToString().Contains("HS"))
                    cost_price *= 1.035;

                row.Cells["cost_price_per_box"].Value = cost_price.ToString("#,##0");
                row.Cells["cost_price"].Value = (cost_price * contract_qty).ToString("#,##0");


                double trq_cost_price;
                if (!double.TryParse(productDt.Rows[i]["after_trq"].ToString(), out trq_cost_price))
                {
                    trq_cost_price = 0;
                }

                row.Cells["trq_cost_price_per_box"].Value = trq_cost_price.ToString("#,##0");
                row.Cells["trq_cost_price"].Value = (trq_cost_price * contract_qty).ToString("#,##0");
            }
        }
        private void Calculate(DataGridViewRow row)
        {
            dgvProduct.EndEdit();
            double box_weight;
            if (row.Cells["box_weight"].Value == null || !double.TryParse(row.Cells["box_weight"].Value.ToString(), out box_weight))
                box_weight = 0;
            double cost_unit;
            if (row.Cells["cost_unit"].Value == null || !double.TryParse(row.Cells["cost_unit"].Value.ToString(), out cost_unit))
                cost_unit = 0;
            double contract_qty;
            if (row.Cells["contract_qty"].Value == null || !double.TryParse(row.Cells["contract_qty"].Value.ToString(), out contract_qty))
                contract_qty = 0;
            double unit_price;
            if (row.Cells["unit_price"].Value == null || !double.TryParse(row.Cells["unit_price"].Value.ToString(), out unit_price))
                unit_price = 0;
            double payment_exchange_rate;
            if (row.Cells["payment_exchange_rate"].Value == null || !double.TryParse(row.Cells["payment_exchange_rate"].Value.ToString(), out payment_exchange_rate))
                payment_exchange_rate = 0;
            double shipping_exchange_rate;
            if (row.Cells["shipping_exchange_rate"].Value == null || !double.TryParse(row.Cells["shipping_exchange_rate"].Value.ToString(), out shipping_exchange_rate))
                shipping_exchange_rate = 0;

            double ex_rate;
            if (rbPaymentExchangeRate.Checked)
                ex_rate = payment_exchange_rate;
            else
                ex_rate = shipping_exchange_rate;

            double custom;
            if (!double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                custom = 1;
            custom /= 100;
            double tax;
            if (!double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                tax = 1;
            tax /= 100;
            double incidental_expense;
            if (!double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                incidental_expense = 1;
            incidental_expense /= 100;

            double cost_price = 0;
            bool isChecked = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
            if (isChecked)
                cost_price = box_weight * unit_price * ex_rate * (1 + custom + tax + incidental_expense);
            else
                cost_price = cost_unit * unit_price * ex_rate * (1 + custom + tax + incidental_expense);

            row.Cells["cost_price_per_box"].Value = cost_price.ToString("#,##0");
            row.Cells["cost_price_per_box"].Value = (cost_price * contract_qty).ToString("#,##0");
        }
        #endregion

        #region Button
        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Key event
        private void PendingList2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.N:
                        txtAtoNo.Text = String.Empty;
                        txtContractNo.Text = String.Empty;
                        txtShipper.Text = String.Empty;
                        txtBlNo.Text = String.Empty;
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtBoxWeight.Text = String.Empty;
                        txtManager.Text = String.Empty;
                        cbCcStatus.Text = "전체";
                        txtAtoNo.Focus();
                        break;
                    case Keys.M:
                        txtAtoNo.Focus();
                        break;
                }
            }
            else 
            {
                switch(e.KeyCode) 
                {
                    case Keys.F1:
                        rbShippingExchangeRate.Checked = true;
                        break;
                    case Keys.F2:
                        rbPaymentExchangeRate.Checked = true;
                        break;
                }
            }
        }
        private void txtAtoNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            DataGridView dgv = dgvProduct;
            int i = e.Column.Index;
            if (dgv.Columns[i].Name == "unit" || dgv.Columns[i].Name == "unit_count" || dgv.Columns[i].Name == "seaover_unit"
                    || dgv.Columns[i].Name == "box_weight" || dgv.Columns[i].Name == "cost_unit" || dgv.Columns[i].Name == "unit_price"
                    || dgv.Columns[i].Name == "payment_exchange_rate" || dgv.Columns[i].Name == "contract_qty" || dgv.Columns[i].Name == "cost_price")
            {
                double a;
                if (e.CellValue1 == null)
                    a = 0;
                else
                    a = double.Parse(e.CellValue1.ToString());

                double b;
                if (e.CellValue2 == null)
                    b = 0;
                else
                    b = double.Parse(e.CellValue2.ToString());

                e.SortResult = a.CompareTo(b);
                e.Handled = true;
            }
        }

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                if (e.Button == MouseButtons.Right)
                {
                    dgvProduct.ClearSelection();
                    row.Selected = true;
                }
                else
                {
                    if (dgvProduct.Columns[e.ColumnIndex].Name == "weight_calculate")
                    {
                        bool isChecked = Convert.ToBoolean(row.Cells["weight_calculate"].Value);
                        row.Cells["weight_calculate"].Value = !isChecked;
                        Calculate(dgvProduct.Rows[e.RowIndex]);
                    }
                }
            }
        }
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                dgvProduct.EndEdit();
                if (dgvProduct.Columns[e.ColumnIndex].Name == "box_weight" || dgvProduct.Columns[e.ColumnIndex].Name == "cost_unit"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "contract_qty" || dgvProduct.Columns[e.ColumnIndex].Name == "unit_price"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "payment_exchange_rate")
                {
                    Calculate(dgvProduct.Rows[e.RowIndex]);
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "cost_price_per_vox")
                {
                    double cost_price;
                    if (row.Cells[e.ColumnIndex].Value == null || !double.TryParse(row.Cells[e.ColumnIndex].Value.ToString(), out cost_price))
                        cost_price = 0;
                    double contract_qty;
                    if (row.Cells["contract_qty"].Value == null || !double.TryParse(row.Cells["contract_qty"].Value.ToString(), out contract_qty))
                        contract_qty = 0;
                    row.Cells["cost_price"].Value = (cost_price * contract_qty).ToString("#,##0");
                }
            }
        }

        #endregion

        private void dgvProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();

                    if (um.auth_level > 2)
                    {
                        m.Items.Add("수정(S)");
                    }
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvProduct, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch
            {
            }
        }

        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (dgvProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvProduct.SelectedRows[0];
                    if (dr.Index < 0)
                    {
                        return;
                    }

                    int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                    /*PendingInfo p;*/

                    switch (e.ClickedItem.Text)
                    {
                        case "수정(S)":
                            if (dgvProduct.SelectedRows.Count > 0)
                            {
                                FormCollection fc = Application.OpenForms;
                                bool isFormActive = false;
                                foreach (Form frm in fc)
                                {
                                    //iterate through
                                    if (frm.Name == "UnPendingManager")
                                    {
                                        frm.Activate();
                                        isFormActive = true;
                                    }
                                }

                                if (!isFormActive)
                                {
                                    int id = Convert.ToInt32(dgvProduct.SelectedRows[0].Cells["id"].Value.ToString());
                                    UnPendingManager upm = new UnPendingManager(um, id);
                                    upm.Show();
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
            else
            {
            }
        }

        private void rbShippingExchangeRate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbShippingExchangeRate.Checked)
            {
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    Calculate(row);
                }
            }
            else
            {
                foreach (DataGridViewRow row in dgvProduct.Rows)
                {
                    Calculate(row);
                }
            }
        }
    }
}
