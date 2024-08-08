using AdoNetWindow.Model;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class MarginDetails : Form
    {
        UsersModel um;
        DataGridView dgv;
        public MarginDetails(UsersModel um, DataGridView dgv)
        {
            InitializeComponent();
            this.um = um;
            this.dgv = dgv;
        }
        private void MarginDetails_Load(object sender, EventArgs e)
        {
            SetProduct();
            SetHeaderStyle();
        }

        #region Method
        private void SetProduct()
        {
            dgvProduct.Rows.Clear();

            for(int i = 0; i < dgv.Rows.Count; i++) 
            {
                if (dgv.Rows[i].Visible)
                {
                    this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    int n = dgvProduct.Rows.Add();
                    dgvProduct.Rows[n].Cells["product"].Value = dgv.Rows[i].Cells["product"].Value.ToString();
                    dgvProduct.Rows[n].Cells["origin"].Value = dgv.Rows[i].Cells["origin"].Value.ToString();
                    dgvProduct.Rows[n].Cells["sizes"].Value = dgv.Rows[i].Cells["sizes"].Value.ToString();
                    dgvProduct.Rows[n].Cells["unit"].Value = dgv.Rows[i].Cells["unit"].Value.ToString();
                    dgvProduct.Rows[n].Cells["price_unit"].Value = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                    dgvProduct.Rows[n].Cells["unit_count"].Value = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();

                    //SPO재고
                    dgvProduct.Rows[n].Cells["seaover_stock"].Value = (Convert.ToDouble(dgv.Rows[i].Cells["seaover_unpending"].Value.ToString())
                                                                     + Convert.ToDouble(dgv.Rows[i].Cells["seaover_pending"].Value.ToString())
                                                                     - Convert.ToDouble(dgv.Rows[i].Cells["reserved_stock"].Value.ToString())).ToString("#,##0");

                    dgvProduct.Rows[n].Cells["pending_stock"].Value = (Convert.ToDouble(dgv.Rows[i].Cells["shipment_qty"].Value.ToString())
                                                                     + Convert.ToDouble(dgv.Rows[i].Cells["unpending_qty_before"].Value.ToString())).ToString("#,##0");

                    dgvProduct.Rows[n].Cells["offer_qty"].Value = Convert.ToDouble(dgv.Rows[i].Cells["order_qty"].Value.ToString()).ToString("#,##0");

                    //단가
                    dgvProduct.Rows[n].Cells["sales_price"].Value = Convert.ToDouble(dgv.Rows[i].Cells["sales_price"].Value.ToString()).ToString("#,##0");
                    dgvProduct.Rows[n].Cells["normal_price"].Value = Convert.ToDouble(dgv.Rows[i].Cells["average_purchase_price"].Value.ToString()).ToString("#,##0");
                    //원가
                    dgvProduct.Rows[n].Cells["seaover_cost_price"].Value = Convert.ToDouble(dgv.Rows[i].Cells["sales_cost_price"].Value.ToString());
                    dgvProduct.Rows[n].Cells["pending_cost_price"].Value = Convert.ToDouble(dgv.Rows[i].Cells["pending_cost_price"].Value.ToString());
                    dgvProduct.Rows[n].Cells["offer_cost_price"].Value = Convert.ToDouble(dgv.Rows[i].Cells["offer_cost_price"].Value.ToString());
                    dgvProduct.Rows[n].Cells["average_cost_price1"].Value = Convert.ToDouble(dgv.Rows[i].Cells["average_sales_cost_price1"].Value.ToString());
                    dgvProduct.Rows[n].Cells["average_cost_price2"].Value = Convert.ToDouble(dgv.Rows[i].Cells["average_sales_cost_price2"].Value.ToString());
                    this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    CalculateMargin(dgvProduct.Rows[n]);
                }
            }
            CalculateCostPriceMarginAmount();
        }
        private void CalculateMargin(DataGridViewRow row)
        {
            double temp_price;
            if (rbSalesPrice.Checked)
            {
                if (row.Cells["sales_price"].Value == null || !double.TryParse(row.Cells["sales_price"].Value.ToString(), out temp_price))
                    temp_price = 0;
            }
            else
            {
                if (row.Cells["normal_price"].Value == null || !double.TryParse(row.Cells["normal_price"].Value.ToString(), out temp_price))
                    temp_price = 0;
            }

            double stock = 0;
            double temp_cost_price = 0;

            if(rbCostPriceS.Checked)
            {
                if (row.Cells["seaover_stock"].Value != null && double.TryParse(row.Cells["seaover_stock"].Value.ToString(), out double price))
                    stock = price;
                if (row.Cells["seaover_cost_price"].Value != null && double.TryParse(row.Cells["seaover_cost_price"].Value.ToString(), out double cost_price))
                    temp_cost_price = cost_price;
            }
            else if (rbCostPriceP.Checked)
            {
                if (row.Cells["pending_stock"].Value != null && double.TryParse(row.Cells["pending_stock"].Value.ToString(), out double price))
                    stock = price;
                if (row.Cells["pending_cost_price"].Value != null && double.TryParse(row.Cells["pending_cost_price"].Value.ToString(), out double cost_price))
                    temp_cost_price = cost_price;
            }
            else if (rbCostPriceO.Checked)
            {
                if (row.Cells["offer_qty"].Value != null && double.TryParse(row.Cells["offer_qty"].Value.ToString(), out double price))
                    stock = price;
                if (row.Cells["offer_cost_price"].Value != null && double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out double cost_price))
                    temp_cost_price = cost_price;
            }
            else if (rbCostPriceSP.Checked)
            {
                if (row.Cells["seaover_stock"].Value != null && double.TryParse(row.Cells["seaover_stock"].Value.ToString(), out double price1))
                    stock += price1;
                if (row.Cells["pending_stock"].Value != null && double.TryParse(row.Cells["pending_stock"].Value.ToString(), out double price2))
                    stock += price2;
                if (row.Cells["average_cost_price1"].Value != null && double.TryParse(row.Cells["average_cost_price1"].Value.ToString(), out double cost_price))
                    temp_cost_price = cost_price;
            }
            else if (rbCostPriceSPO.Checked)
            {
                if (row.Cells["seaover_stock"].Value != null && double.TryParse(row.Cells["seaover_stock"].Value.ToString(), out double price1))
                    stock += price1;
                if (row.Cells["pending_stock"].Value != null && double.TryParse(row.Cells["pending_stock"].Value.ToString(), out double price2))
                    stock += price2;
                if (row.Cells["offer_qty"].Value != null && double.TryParse(row.Cells["offer_qty"].Value.ToString(), out double price3))
                    stock += price3;
                if (row.Cells["average_cost_price2"].Value != null && double.TryParse(row.Cells["average_cost_price2"].Value.ToString(), out double cost_price))
                    temp_cost_price = cost_price;
            }
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            double total_temp_amount = temp_price * stock;
            row.Cells["total_temp_amount"].Value = total_temp_amount.ToString("#,##0");

            double total_temp_cost_amount = temp_cost_price * stock;
            row.Cells["total_temp_cost_amount"].Value = total_temp_cost_amount.ToString("#,##0");

            row.Cells["total_temp_cost_margin_amount"].Value = (total_temp_amount - total_temp_cost_amount).ToString("#,##0");
            row.Cells["total_temp_cost_margin"].Value = ((total_temp_amount - total_temp_cost_amount) / total_temp_amount * 100).ToString("#,##0.00");
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);

        }
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvProduct;
            if (dgv.Columns.Count > 0)
            {
                //비교단가
                if (rbSalesPrice.Checked)
                {
                    dgv.Columns["sales_price"].Visible = true;
                    dgv.Columns["normal_price"].Visible = false;
                }
                else
                {
                    dgv.Columns["sales_price"].Visible = false;
                    dgv.Columns["normal_price"].Visible = true;
                }

                //비교원가
                dgv.Columns["seaover_stock"].Visible = false;
                dgv.Columns["pending_stock"].Visible = false;
                dgv.Columns["offer_qty"].Visible = false;

                dgv.Columns["seaover_cost_price"].Visible = false;
                dgv.Columns["pending_cost_price"].Visible = false;
                dgv.Columns["offer_cost_price"].Visible = false;
                dgv.Columns["average_cost_price1"].Visible = false;
                dgv.Columns["average_cost_price2"].Visible = false;

                if (rbCostPriceS.Checked)
                {
                    dgv.Columns["seaover_stock"].Visible = true;
                    dgv.Columns["seaover_cost_price"].Visible = true;
                }
                else if (rbCostPriceP.Checked)
                {
                    dgv.Columns["pending_stock"].Visible = true;
                    dgv.Columns["pending_cost_price"].Visible = true;
                }
                else if (rbCostPriceO.Checked)
                {
                    dgv.Columns["offer_qty"].Visible = true;
                    dgv.Columns["offer_cost_price"].Visible = true;
                }
                else if (rbCostPriceSP.Checked)
                {
                    dgv.Columns["seaover_stock"].Visible = true;
                    dgv.Columns["pending_stock"].Visible = true;
                    dgv.Columns["average_cost_price1"].Visible = true;
                }
                else if (rbCostPriceSPO.Checked)
                {
                    dgv.Columns["seaover_stock"].Visible = true;
                    dgv.Columns["pending_stock"].Visible = true;
                    dgv.Columns["offer_qty"].Visible = true;
                    dgv.Columns["average_cost_price2"].Visible = true;
                }
;

                //헤더 디자인
                dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
                dgv.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;

                //기본(회색)
                Color darkBlue = Color.FromArgb(43, 94, 170);    //남색
                dgv.ColumnHeadersDefaultCellStyle.BackColor = darkBlue;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                /*dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
                dgv.Columns["unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["unit_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["seaover_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["cost_unit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["seaover_unit"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);*/
                //재고내역========================================================================================
                Color lightBlue = Color.FromArgb(51, 102, 255);  //진파랑            
                dgv.Columns["seaover_stock"].HeaderCell.Style.BackColor = Color.LightGray;
                dgv.Columns["seaover_stock"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["seaover_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["seaover_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["pending_stock"].HeaderCell.Style.BackColor = Color.LightGray;
                dgv.Columns["pending_stock"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["pending_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["pending_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["offer_qty"].HeaderCell.Style.BackColor = Color.LightGray;
                dgv.Columns["offer_qty"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["offer_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["offer_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
                //매출단가, 일반시세 ======================================================================================
                Color lightGreen = Color.SeaGreen;
                dgv.Columns["sales_price"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["sales_price"].HeaderCell.Style.ForeColor = Color.Yellow;
                dgv.Columns["sales_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["sales_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["normal_price"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["normal_price"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["normal_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["normal_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["total_temp_amount"].HeaderCell.Style.BackColor = lightGreen;
                dgv.Columns["total_temp_amount"].HeaderCell.Style.ForeColor = Color.Yellow;
                dgv.Columns["total_temp_amount"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["total_temp_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["total_temp_amount"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
                //씨오버원가, 선적원가, 평균원가1, 평균원가2======================================================================
                Color violet = Color.FromArgb(125, 135, 255);
                dgv.Columns["seaover_cost_price"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["seaover_cost_price"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["seaover_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["seaover_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["pending_cost_price"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["pending_cost_price"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["pending_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["pending_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["offer_cost_price"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["offer_cost_price"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["offer_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["offer_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_cost_price1"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["average_cost_price1"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["average_cost_price1"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["average_cost_price1"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_cost_price1"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["average_cost_price2"].HeaderCell.Style.BackColor = violet;
                dgv.Columns["average_cost_price2"].HeaderCell.Style.ForeColor = Color.White;
                dgv.Columns["average_cost_price2"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["average_cost_price2"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["average_cost_price2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
                //손익, 마진율================================================================================
                Color lightViolet = Color.FromArgb(205, 209, 255);
                dgv.Columns["total_temp_cost_amount"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["total_temp_cost_amount"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["total_temp_cost_amount"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["total_temp_cost_amount"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["total_temp_cost_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["total_temp_cost_amount"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

                dgv.Columns["total_temp_cost_margin"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["total_temp_cost_margin"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["total_temp_cost_margin"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["total_temp_cost_margin"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["total_temp_cost_margin"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["total_temp_cost_margin"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

                dgv.Columns["total_temp_cost_margin_amount"].HeaderCell.Style.BackColor = lightViolet;
                dgv.Columns["total_temp_cost_margin_amount"].HeaderCell.Style.ForeColor = Color.Black;
                dgv.Columns["total_temp_cost_margin_amount"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
                dgv.Columns["total_temp_cost_margin_amount"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                dgv.Columns["total_temp_cost_margin_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgv.Columns["total_temp_cost_margin_amount"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            }
        }
        private void CalculateCostPriceMarginAmount()
        {
            double total_offer_qty= 0;
            double total_margin_amount0 = 0;
            double total_margin_amount1 = 0;
            double total_margin_amount2 = 0;
            double total_margin_amount3 = 0;
            foreach (DataGridViewRow row in dgvProduct.Rows)
            {
                //재고수
                double seaover_stock;
                if (row.Cells["seaover_stock"].Value == null || !double.TryParse(row.Cells["seaover_stock"].Value.ToString(), out seaover_stock))
                    seaover_stock = 0;
                double pending_stock;
                if (row.Cells["pending_stock"].Value == null || !double.TryParse(row.Cells["pending_stock"].Value.ToString(), out pending_stock))
                    pending_stock = 0;
                double offer_qty;
                if (row.Cells["offer_qty"].Value == null || !double.TryParse(row.Cells["offer_qty"].Value.ToString(), out offer_qty))
                    offer_qty = 0;
                total_offer_qty += offer_qty;
                //S원가
                double seaover_cost_price;
                if (row.Cells["seaover_cost_price"].Value == null || !double.TryParse(row.Cells["seaover_cost_price"].Value.ToString(), out seaover_cost_price))
                    seaover_cost_price = 0;
                //선적원가
                double pending_cost_price;
                if (row.Cells["pending_cost_price"].Value == null || !double.TryParse(row.Cells["pending_cost_price"].Value.ToString(), out pending_cost_price))
                    pending_cost_price = 0;
                //오퍼원가
                double offer_cost_price;
                if (row.Cells["offer_cost_price"].Value == null || !double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out offer_cost_price))
                    offer_cost_price = 0;
                //평균원가1
                double average_cost_price1;
                if (row.Cells["average_cost_price1"].Value == null || !double.TryParse(row.Cells["average_cost_price1"].Value.ToString(), out average_cost_price1))
                    average_cost_price1 = 0;
                //평균원가2
                double average_cost_price2 = ((average_cost_price1 * (seaover_stock + pending_stock)) + (offer_cost_price * offer_qty)) / (seaover_stock + pending_stock + offer_qty);
                row.Cells["average_cost_price2"].Value = average_cost_price2;

                //비교단가
                double temp_price;
                if (rbSalesPrice.Checked)
                {
                    if (!double.TryParse(row.Cells["sales_price"].Value.ToString(), out temp_price))
                        temp_price = 0;
                }
                else
                {
                    if (!double.TryParse(row.Cells["noraml_price"].Value.ToString(), out temp_price))
                        temp_price = 0;
                }


                //O손익
                if (temp_price > 0 && offer_cost_price > 0 && offer_qty > 0)
                    total_margin_amount0 += (temp_price * offer_qty) - (offer_cost_price * offer_qty);
                //손익1 (재고만)
                if (temp_price > 0 && seaover_cost_price > 0)
                    total_margin_amount1 += (temp_price * seaover_stock) - (seaover_cost_price * seaover_stock);
                //손익2 (재고 + 선적)
                if (temp_price > 0 && average_cost_price1 > 0)
                    total_margin_amount2 += (temp_price - average_cost_price1) * (seaover_stock + pending_stock);
                //SPO 손익
                if (temp_price > 0 && average_cost_price2 > 0)
                    total_margin_amount3 += (temp_price - average_cost_price2) * (seaover_stock + pending_stock + offer_qty);
            }
            txtTotalOrderQty.Text = total_offer_qty.ToString("#,##0");
            txtTotalOrderMarginAmount0.Text = total_margin_amount0.ToString("#,##0");
            txtTotalOrderMarginAmount1.Text = total_margin_amount1.ToString("#,##0");
            txtTotalOrderMarginAmount2.Text = total_margin_amount2.ToString("#,##0");
            txtTotalOrderMarginAmount3.Text = total_margin_amount3.ToString("#,##0");
        }
        #endregion

        #region Button, RadioButton
        private void rbSalesPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSalesPrice.Checked)
            {
                SetHeaderStyle();
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    CalculateMargin(row);
                CalculateCostPriceMarginAmount();
            }
            if (rbNormalPrice.Checked)
            {
                SetHeaderStyle();
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    CalculateMargin(row);
                CalculateCostPriceMarginAmount();
            }
            if (rbCostPriceS.Checked)
            {
                SetHeaderStyle();
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    CalculateMargin(row);
                CalculateCostPriceMarginAmount();
            }
            if (rbCostPriceP.Checked)
            {
                SetHeaderStyle();
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    CalculateMargin(row);
                CalculateCostPriceMarginAmount();
            }
            if (rbCostPriceO.Checked)
            {
                SetHeaderStyle();
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    CalculateMargin(row);
                CalculateCostPriceMarginAmount();
            }
            if (rbCostPriceSP.Checked)
            {
                SetHeaderStyle();
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    CalculateMargin(row);
                CalculateCostPriceMarginAmount();
            }
            if (rbCostPriceSPO.Checked)
            {
                SetHeaderStyle();
                foreach (DataGridViewRow row in dgvProduct.Rows)
                    CalculateMargin(row);
                CalculateCostPriceMarginAmount();
            }
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name.Contains("stock") || dgvProduct.Columns[e.ColumnIndex].Name.Contains("qty"))
                {
                    CalculateMargin(dgvProduct.Rows[e.RowIndex]);
                    CalculateCostPriceMarginAmount();
                }
                    
                else if (dgvProduct.Columns[e.ColumnIndex].Name.Contains("cost_price"))
                {
                    CalculateMargin(dgvProduct.Rows[e.RowIndex]);
                    CalculateCostPriceMarginAmount();
                }
            }
        }
        #endregion

    }
}
