using AdoNetWindow.Model;
using AdoNetWindow.PurchaseManager;
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

namespace AdoNetWindow.SEAOVER.ProductCostComparison
{
    public partial class ProductCostComparison : Form
    {
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        ICustomsRepository customsRepository = new CustomsRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        UsersModel um;
        double customRate;
        int workDays;
        //최근오퍼 내역
        DataTable copDt;

        public ProductCostComparison(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            dgvProduct.Init(false);
        }

        private void ProductCostComparison_Load(object sender, EventArgs e)
        {
            //현재환율
            customRate = common.GetExchangeRateKEBBank("USD");
            txtExchangeRate.Text = customRate.ToString("#,##0.00");
            //Procedure
            txtSttdate.Text = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            priceComparisonRepository.SetSeaoverId(um.seaover_id);
            SetHeaderStyle();
            //최근 오퍼내역
            copDt = purchasePriceRepository.GetCurrentPurchasePrice("", "", "", "");
        }
        #region Method
        private void CalculateCostPrice(int rowIndex)
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            DataGridViewRow row = dgvProduct.Rows[rowIndex];

            //매입단가
            double purchase_price = 0;
            if (row.Cells["offer_price"].Value != null && double.TryParse(row.Cells["offer_price"].Value.ToString(), out purchase_price))
            {
                if (purchase_price > 0)
                {
                    //관세
                    double custom;
                    if (!double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                        custom = 0;
                    else
                        custom = custom / 100;
                    //과세
                    double tax;
                    if (!double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                        tax = 0;
                    else
                        tax = tax / 100;
                    //부대비용
                    double incidental_expense;
                    if (!double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                        incidental_expense = 0;
                    else
                        incidental_expense = incidental_expense / 100;
                    //단위
                    double unit = Convert.ToDouble(row.Cells["unit"].Value);
                    if (!string.IsNullOrEmpty(row.Cells["cost_unit"].Value.ToString()) && Convert.ToDouble(row.Cells["cost_unit"].Value.ToString()) > 0)
                    {
                        unit = Convert.ToDouble(row.Cells["cost_unit"].Value.ToString());
                    }

                    //환율
                    double exchange_rate;
                    if (!double.TryParse(row.Cells["exchange_rate"].Value.ToString(), out exchange_rate))
                        exchange_rate = 0;
                    row.Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0.00");
                    //원가계산
                    double cost_price;

                    if (rbCostprice.Checked)
                    {
                        cost_price = unit * purchase_price;
                        if (tax > 0)
                        {
                            cost_price += (unit * purchase_price) * tax;
                        }
                        if (custom > 0)
                        {
                            cost_price += (unit * purchase_price) * custom;
                        }
                        if (incidental_expense > 0)
                        {
                            cost_price += (unit * purchase_price) * incidental_expense;
                        }
                        cost_price = cost_price * exchange_rate;
                    }
                    else
                    {
                        //TRQ 계산
                        double trq;
                        if (!double.TryParse(txtTrq.Text, out trq))
                            trq = 0;
                        cost_price = (unit * purchase_price * exchange_rate)
                                    + (unit * purchase_price * incidental_expense * exchange_rate)
                                    + (trq * unit);
                    }

                    //차이가 심할땐 단위수량으로 나눔
                    //평균원가
                    double average_cost_price;
                    if (!double.TryParse(row.Cells["average_cost_price"].Value.ToString(), out average_cost_price))
                        average_cost_price = 0;
                    double sales_price = Convert.ToDouble(row.Cells["sales_price"].Value.ToString());
                    double increase_rate;
                    if (average_cost_price > 0)
                        increase_rate = ((cost_price - average_cost_price) / average_cost_price) * 100;
                    else
                        increase_rate = ((cost_price - sales_price) / sales_price) * 100;
                    if (increase_rate > 150)
                    {
                        cost_price = cost_price / unit;
                    }

                    row.Cells["offer_cost_price"].Value = cost_price.ToString("#,##0");
                    

                    //원가의 평균
                    if (average_cost_price > 0)
                        row.Cells["total_average_cost_price"].Value = ((average_cost_price + cost_price) / 2).ToString("#,##0");
                    else
                        row.Cells["total_average_cost_price"].Value = cost_price.ToString("#,##0");
                }
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        private void CalculateCostPrice()
        {
            if (dgvProduct.Rows.Count > 0)
            {
                this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];


                    //매입단가
                    double purchase_price = 0;
                    if (row.Cells["offer_price"].Value != null && double.TryParse(row.Cells["offer_price"].Value.ToString(), out purchase_price))
                    {
                        if (purchase_price > 0)
                        {
                            //관세
                            double custom;
                            if (!double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                                custom = 0;
                            else
                                custom = custom / 100;
                            //과세
                            double tax;
                            if (!double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                                tax = 0;
                            else
                                tax = tax / 100;
                            //부대비용
                            double incidental_expense;
                            if (!double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                                incidental_expense = 0;
                            else
                                incidental_expense = incidental_expense / 100;
                            //단위
                            double unit = Convert.ToDouble(row.Cells["unit"].Value);
                            if (!string.IsNullOrEmpty(row.Cells["cost_unit"].Value.ToString()) && Convert.ToDouble(row.Cells["cost_unit"].Value.ToString()) > 0)
                            {
                                unit = Convert.ToDouble(row.Cells["cost_unit"].Value.ToString());
                            }

                            //환율
                            double exchange_rate;
                            if (!double.TryParse(row.Cells["exchange_rate"].Value.ToString(), out exchange_rate))
                                exchange_rate = 0;
                            row.Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0.00");
                            //원가계산
                            double cost_price;

                            if (rbCostprice.Checked)
                            {
                                cost_price = unit * purchase_price;
                                if (tax > 0)
                                {
                                    cost_price += (unit * purchase_price) * tax;
                                }
                                if (custom > 0)
                                {
                                    cost_price += (unit * purchase_price) * custom;
                                }
                                if (incidental_expense > 0)
                                {
                                    cost_price += (unit * purchase_price) * incidental_expense;
                                }
                                cost_price = cost_price * exchange_rate;
                            }
                            else
                            {
                                //TRQ 계산
                                double trq;
                                if (!double.TryParse(txtTrq.Text, out trq))
                                    trq = 0;
                                cost_price = (unit * purchase_price * exchange_rate)
                                            + (unit * purchase_price * incidental_expense * exchange_rate)
                                            + (trq * unit);
                            }
                            row.Cells["offer_cost_price"].Value = cost_price.ToString("#,##0");
                            //평균원가
                            double average_cost_price;
                            if (!double.TryParse(row.Cells["average_cost_price"].Value.ToString(), out average_cost_price))
                                average_cost_price = 0;

                            //원가의 평균
                            if (average_cost_price > 0)
                            {
                                double real_stock = Convert.ToDouble(row.Cells["real_stock"].Value.ToString());
                                double offer_qty;
                                if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out offer_qty))
                                {
                                    offer_qty = 0;
                                }

                                double total_average_price = ((average_cost_price * real_stock) + (cost_price * offer_qty)) / (real_stock + offer_qty);
                                row.Cells["total_average_cost_price"].Value = total_average_price.ToString("#,##0");
                            }
                            else
                            {
                                row.Cells["total_average_cost_price"].Value = cost_price.ToString("#,##0");
                            }
                        }
                    }
                }
                this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
            }
        }
        private void CallProductProcedure()
        {
            //업체별시세현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                ////업체별시세현황 스토어프로시져 호출
                if (seaoverRepository.CallStoredProcedure(user_id, sDate, eDate) == 0)
                {
                    MessageBox.Show(this,"호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
                this.Activate();
                return;
            }

        }
        private void CallStockProcedure()
        {
            //품명별재고현황 스토어프로시져 호출
            try
            {
                string sDate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
                string eDate = DateTime.Now.ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (priceComparisonRepository.CallStoredProcedureSTOCK(user_id, eDate) == 0)
                {
                    MessageBox.Show(this,"호출 내용이 없음");
                    this.Activate();
                    return;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(this,e.Message);
                this.Activate();
                return;
            }
            
        }

        //Datagridview Header style 
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvProduct;
            //기본숨김 컬럼
            dgv.Columns["stock"].Visible = false;
            dgv.Columns["reserved_stock"].Visible = false;
            dgv.Columns["reserved_stock_detail"].Visible = false;
            dgv.Columns["shipment_stock"].Visible = false;
            dgv.Columns["cost_unit"].Visible = false;

            dgv.Columns["sales_count"].Visible = false;
            dgv.Columns["sales_amount"].Visible = false;
            dgv.Columns["day_sales_count"].Visible = false;

            dgv.Columns["custom"].Visible = false;
            dgv.Columns["tax"].Visible = false;
            dgv.Columns["incidental_expense"].Visible = false;
            dgv.Columns["exchange_rate"].Visible = false;

            //헤더 디자인
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;

            /*dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;*/
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Regular);
            dgv.DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Regular);

            //파랑
            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgv.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170);
            dgv.Columns["unit"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["unit"].HeaderCell.Style.Font = new Font("나눔고딕", 7, FontStyle.Regular);

            //연두
            dgv.Columns["shipment_stock"].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns["shipment_stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["shipment_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["shipment_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["stock"].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns["stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["reserved_stock"].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns["reserved_stock"].HeaderCell.Style.ForeColor = Color.Black;
            dgv.Columns["reserved_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["reserved_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["reserved_stock_detail"].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns["reserved_stock_detail"].HeaderCell.Style.ForeColor = Color.Black;

            dgv.Columns["real_stock"].HeaderCell.Style.BackColor = Color.Beige;
            dgv.Columns["real_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgv.Columns["real_stock"].DefaultCellStyle.BackColor = Color.FromArgb(226, 239, 218);
            dgv.Columns["real_stock"].DefaultCellStyle.BackColor = Color.FromArgb(253, 255, 234);
            dgv.Columns["real_stock"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["real_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            //찐한 연두
            dgv.Columns["day_sales_count"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["day_sales_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["day_sales_count"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

            dgv.Columns["month_sales_count"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["month_sales_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["month_sales_count"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

            dgv.Columns["sales_count"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["sales_count"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["sales_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["month_around"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["month_around"].DefaultCellStyle.BackColor = Color.FromArgb(242, 255, 202);
            dgv.Columns["month_around"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["month_around"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마

            dgv.Columns["sales_amount"].HeaderCell.Style.BackColor = Color.FromArgb(153, 204, 0);
            dgv.Columns["sales_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["sales_amount"].DefaultCellStyle.Format = "#,###,##0"; // 콤마


            //진파랑
            dgv.Columns["sales_price"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255); ;
            dgv.Columns["sales_price"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["sales_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["sales_price"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgv.Columns["sales_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["sales_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["purchase_price"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255); ;
            dgv.Columns["purchase_price"].HeaderCell.Style.ForeColor = Color.Yellow;
            dgv.Columns["purchase_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["purchase_price"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgv.Columns["purchase_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["purchase_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["average_cost_price"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["average_cost_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["average_cost_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            /*dgv.Columns["average_cost_price"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);*/
            dgv.Columns["average_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["average_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["normal_price"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["normal_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["normal_price"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            /*dgv.Columns["normal_price"].DefaultCellStyle.BackColor = Color.FromArgb(226, 244, 248);*/
            dgv.Columns["normal_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["normal_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            //보라
            dgv.Columns["exchange_rate"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["exchange_rate"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["incidental_expense"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["incidental_expense"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["tax"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["tax"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["custom"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["custom"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["offer_updatetime"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["offer_updatetime"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["offer_price"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["offer_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["offer_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["offer_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["offer_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["offer_cost_price"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["offer_cost_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["offer_cost_price"].DefaultCellStyle.BackColor = Color.FromArgb(217, 225, 242);
            dgv.Columns["offer_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["offer_cost_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["offer_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["offer_company"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["offer_company"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["offer_company"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);

            dgv.Columns["order_qty"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["order_qty"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["order_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgv.Columns["order_qty"].DefaultCellStyle.BackColor = Color.FromArgb(253, 255, 234);
            dgv.Columns["order_qty"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["order_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["total_stock"].HeaderCell.Style.BackColor = Color.BlueViolet;
            dgv.Columns["total_stock"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["total_stock"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgv.Columns["total_stock"].DefaultCellStyle.BackColor = Color.FromArgb(253, 255, 234);
            dgv.Columns["total_stock"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["total_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["total_average_cost_price"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["total_average_cost_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["total_average_cost_price"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgv.Columns["total_average_cost_price"].DefaultCellStyle.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            dgv.Columns["total_average_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["total_average_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["total_month_around"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["total_month_around"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["total_month_around"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgv.Columns["total_month_around"].DefaultCellStyle.Format = "#,###,##0"; // 콤마

            dgv.Columns["exhausted_date"].HeaderCell.Style.BackColor = Color.FromArgb(51, 102, 255);
            dgv.Columns["exhausted_date"].HeaderCell.Style.ForeColor = Color.White;

            //dgv.Columns["manager"].HeaderCell.Style.Font = new Font("나눔고딕", 8, FontStyle.Bold);
            // 보여지는 컬럼 넓이조정
            dgv.Columns["product"].Width = 180;
            dgv.Columns["origin"].Width = 60;
            dgv.Columns["sizes"].Width = 100;
            dgv.Columns["unit"].Width = 45;
            dgv.Columns["sales_price"].Width = 60;

            //dgv.Columns["sales_cost_price"].Width = 60;
            //dgv.Columns["average_purchase_price"].Width = 60;

            /*dgv.Columns["custom"].Width = 50;
            dgv.Columns["tax"].Width = 50;
            dgv.Columns["incidental_expense"].Width = 50;*/

            dgv.Columns["stock"].Width = 50;
            dgv.Columns["reserved_stock"].Width = 50;
            dgv.Columns["reserved_stock_detail"].Width = 200;
            dgv.Columns["shipment_stock"].Width = 50;
            dgv.Columns["real_stock"].Width = 50;

            dgv.Columns["sales_count"].Width = 60;
            dgv.Columns["month_around"].Width = 60;
            dgv.Columns["day_sales_count"].Width = 60;
            dgv.Columns["month_sales_count"].Width = 60;
            dgv.Columns["sales_amount"].Width = 80;
            dgv.Columns["average_cost_price"].Width = 70;
            dgv.Columns["normal_price"].Width = 70;

            dgv.Columns["offer_updatetime"].Width = 70;
            dgv.Columns["offer_price"].Width = 60;
            dgv.Columns["offer_company"].Width = 100;
            dgv.Columns["offer_cost_price"].Width = 60;
            dgv.Columns["order_qty"].Width = 60;
            dgv.Columns["total_stock"].Width = 60;
            dgv.Columns["total_average_cost_price"].Width = 60;
            dgv.Columns["total_month_around"].Width = 60;
            dgv.Columns["exhausted_date"].Width = 70;

            dgv.Columns["custom"].Width = 60;
            dgv.Columns["tax"].Width = 60;
            dgv.Columns["incidental_expense"].Width = 60;
            dgv.Columns["exchange_rate"].Width = 60;
            dgv.Columns["manager"].Width = 60;
            //dgv.Columns["month_around2"].Width = 60;

            // 전체 컬럼의 Sorting 기능 차단 
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void GetData()
        {
            this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);

            //프로시져 호출
            CallProductProcedure();
            CallStockProcedure();

            //유효성검사
            DateTime sDate;
            if (!DateTime.TryParse(txtSttdate.Text, out sDate))
            {
                MessageBox.Show(this,"일반시세 기간 시작일 형식이 날짜 형식이 아닙니다.");
                this.Activate();
                return;
            }
            DateTime eDate;
            if (!DateTime.TryParse(txtEnddate.Text, out eDate))
            {
                MessageBox.Show(this,"일반시세 기간 종료일 형식이 날짜 형식이 아닙니다."); 
                this.Activate();
                return;
            }
            int avgCnt;
            if (!int.TryParse(cbCp.Text, out avgCnt))
            {
                MessageBox.Show(this,"일반시세 업체수 값을 확인해주세요.");
                this.Activate();
                return;
            }
            //초기화
            dgvProduct.Rows.Clear();
            //매출기간
            //매출기간 영업일수
            DateTime stt_date;
            DateTime end_date;
            int salesMonth = 6;
            if (rbeighteenMonths.Checked)
            {
                stt_date = DateTime.Now.AddDays(-1).AddMonths(-18);
                end_date = DateTime.Now.AddDays(-1);
                salesMonth = 18;
            }
            else if (rbtwelveMonths.Checked)
            {
                stt_date = DateTime.Now.AddDays(-1).AddMonths(-12);
                end_date = DateTime.Now.AddDays(-1);
                salesMonth = 12;
            }
            else
            {
                stt_date = DateTime.Now.AddDays(-1).AddMonths(-6);
                end_date = DateTime.Now.AddDays(-1);
                salesMonth = 6;
            }
            common.GetWorkDay(stt_date, end_date, out workDays);

            //Get data
            DataTable productDt = priceComparisonRepository.GetProductCostComparison(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtDivision.Text, sDate, eDate, avgCnt, salesMonth);
            if (productDt.Rows.Count > 0)
            {
                //통관대기중 품목 DATA 
                DataTable uppDt = customsRepository.GetUnpendingProduct(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, false, false);
                //품목 추가정보
                DataTable tmDt = purchasePriceRepository.GetTradeManaer(txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtManager.Text.Trim());
                //오퍼내역
                DataTable pdDt = customsRepository.GetPendingPriceList(DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd"), txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text);

                DataOutput(productDt, uppDt, tmDt, pdDt);
            }
            this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
        }
        //담당자 검색 X
        private void DataOutput(DataTable productDt, DataTable uppDt, DataTable tmDt, DataTable pdDt)
        {
            //매입단가 검색
            int sttPrice = 0;
            int endPrice = 0;
            try
            {
                sttPrice = Convert.ToInt32(txtSttPrice.Text.Replace(",", ""));
                endPrice = Convert.ToInt32(txtEndPrice.Text.Replace(",", ""));
            }
            catch
            { }

            for (int i = 0; i < productDt.Rows.Count; i++)
            {
                bool isOutput = true;
                DataRow[] tmRow = null;

                if (!string.IsNullOrEmpty(txtManager.Text) && tmDt.Rows.Count == 0)
                {
                    isOutput = false;
                }
                else if (tmDt.Rows.Count > 0)
                {
                    string whr = "product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                        + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                        + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                        + " AND (unit = '" + productDt.Rows[i]["단위"].ToString() + "'"
                            + " OR unit = '" + productDt.Rows[i]["계산단위"].ToString() + "')";
                    tmRow = tmDt.Select(whr);
                    if (tmRow.Length > 0)
                    {
                        isOutput = true;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtManager.Text))
                            isOutput = false;
                    }
                        
                }

                //통관대기품목============================================================================
                double shipment_qty = 0;
                double unpending_qty_before = 0;
                double unpending_qty_after = 0;
                int delivery_days = 0;
                DataRow[] uppRow = null;
                if (uppDt.Rows.Count > 0)
                {
                    string whr = "product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                        + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                        + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                        + " AND box_weight = '" + productDt.Rows[i]["단위"].ToString() + "'";
                    uppRow = uppDt.Select(whr);
                    if (uppRow.Length > 0)
                    {
                        for (int j = 0; j < uppRow.Length; j++)
                        {
                            if (!int.TryParse(uppRow[j]["delivery_days"].ToString(), out delivery_days))
                            {
                                delivery_days = 15;
                            }
                            string bl_no = uppRow[j]["bl_no"].ToString();
                            string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                            DateTime dtt;
                            //계약만 한 상태
                            if (string.IsNullOrEmpty(bl_no))
                            {
                                shipment_qty += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                            }
                            //배송중인 상태
                            else if (!string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date))
                            {
                                unpending_qty_before += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                            }
                            //입항은 했지만 미통관인 상태
                            else if (!string.IsNullOrEmpty(bl_no) && !string.IsNullOrEmpty(warehousing_date))
                            {
                                unpending_qty_after += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                            }
                        }
                    }
                }

                //재고수
                double total_stock = Convert.ToDouble(productDt.Rows[i]["재고수"].ToString());
                //예약수
                double reserved_stock = Convert.ToDouble(productDt.Rows[i]["예약수"].ToString());
                
                // 가용재고 = 재고수 - 예약수 + 관리자 예약
                double stock = total_stock - reserved_stock;
                
                //선적 재고포함 체크
                if (cbPendingInStock.Checked)
                    stock += shipment_qty + unpending_qty_before;
                
                //일 평균매출, 회전율
                double sales_count = Convert.ToDouble(productDt.Rows[i]["매출수"].ToString());
                double day_sales = sales_count / workDays;
                //회전율
                double month_around;
                if (stock == 0 && sales_count == 0)
                    month_around = 0;
                else if (stock > 0 && sales_count == 0)
                    month_around = 9999;
                else if (stock == 0 && sales_count > 0)
                    month_around = 0;
                else
                    month_around = (stock / day_sales) / 21;
                //고급검색====================================================================================================
                double sales_price = Convert.ToDouble(productDt.Rows[i]["매출단가"].ToString());              //매출단가
                double purchase_price = Convert.ToDouble(productDt.Rows[i]["최저단가"].ToString());           //매입단가
                double average_purchase_price = Convert.ToDouble(productDt.Rows[i]["일반시세"].ToString());   //일반시세

                if (pnAdvancedSearch.Visible)
                {
                    double temp_stock = stock;
                    if (!cbReservedStock.Checked)
                        temp_stock += reserved_stock;

                    if (isOutput)
                    {
                        if (cbStockExist.Checked && !cbStockZero.Checked)
                        {
                            if (temp_stock <= 0)
                                isOutput = false;
                        }
                        else if (!cbStockExist.Checked && cbStockZero.Checked)
                        {
                            if (temp_stock > 0)
                                isOutput = false;
                        }
                        else if (!cbStockExist.Checked && !cbStockZero.Checked)
                        {
                            isOutput = false;
                        }
                    }

                    //회전율 검색
                    if (cbRoundStock.Checked && isOutput)
                    {
                        if (!(month_around >= (double)nudSttRound.Value && month_around <= (double)nudSttRound.Value))
                            isOutput = false;
                    }

                    //매입단가 검색
                    if (cbPurchasePrice.Checked && isOutput)
                    {
                        double sttPurchasePrice;
                        if (!double.TryParse(txtSttPrice.Text, out sttPurchasePrice))
                            sttPrice = 0;
                        double endPurchasePrice;
                        if (!double.TryParse(txtEndPrice.Text, out endPurchasePrice))
                            endPurchasePrice = 0;

                        if (!(purchase_price >= sttPurchasePrice && purchase_price <= endPurchasePrice))
                            isOutput = false;
                    }

                    //매입단가 비교
                    if (cbSalesPrice.Checked && isOutput)
                    {
                        switch (cbSalesPrice.Text)
                        {
                            case "매출단가 <= 최저단가":
                                if (sales_price > purchase_price)
                                    isOutput = false;

                                break;
                            case "매출단가 <= 일반시세":
                                if (sales_price > average_purchase_price)
                                    isOutput = false;

                                break;
                            case "매출단가 >= 일반시세":
                                if (sales_price < average_purchase_price)
                                    isOutput = false;
                                break;
                        }
                    }
                }

                //출력여부
                if (isOutput)
                { 
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["product"].Value = productDt.Rows[i]["품명"].ToString();
                    dgvProduct.Rows[n].Cells["origin"].Value = productDt.Rows[i]["원산지"].ToString();
                    dgvProduct.Rows[n].Cells["sizes"].Value = productDt.Rows[i]["규격"].ToString();
                    dgvProduct.Rows[n].Cells["unit"].Value = productDt.Rows[i]["단위"].ToString();
                    dgvProduct.Rows[n].Cells["stock"].Value = productDt.Rows[i]["재고수"].ToString();
                    /*dgvProduct.Rows[i].Cells["unit_cost"].Value = 0;*/

                    dgvProduct.Rows[n].Cells["sales_count"].Value = productDt.Rows[i]["매출수"].ToString();
                    dgvProduct.Rows[n].Cells["sales_amount"].Value = Convert.ToDouble(productDt.Rows[i]["매출금액"].ToString()).ToString("#,##0");

                    dgvProduct.Rows[n].Cells["sales_price"].Value = sales_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = purchase_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["normal_price"].Value = Convert.ToDouble(productDt.Rows[i]["일반시세"].ToString()).ToString("#,##0");
                    double average_cout_price = Convert.ToDouble(productDt.Rows[i]["평균원가"].ToString());
                    dgvProduct.Rows[n].Cells["average_cost_price"].Value = average_cout_price.ToString("#,##0");

                    dgvProduct.Rows[n].Cells["reserved_stock"].Value = reserved_stock.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["shipment_stock"].Value = (shipment_qty + unpending_qty_before).ToString("#,##0");
                    dgvProduct.Rows[n].Cells["real_stock"].Value = stock.ToString("#,##0");


                    dgvProduct.Rows[n].Cells["day_sales_count"].Value = day_sales.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["month_sales_count"].Value = (day_sales * 21).ToString("#,##0");
                    dgvProduct.Rows[n].Cells["month_around"].Value = month_around.ToString("#,##0.00");

                    //환율
                    double exchange_rate = customRate;
                    dgvProduct.Rows[n].Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0.00");

                    //품목 추가정보===========================================================================
                    dgvProduct.Rows[n].Cells["custom"].Value = 0;
                    dgvProduct.Rows[n].Cells["tax"].Value = 0;
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = 0;
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = 0;
                    if (tmRow != null && tmRow.Length > 0)
                    {
                        //트레이
                        dgvProduct.Rows[n].Cells["cost_unit"].Value = tmRow[0]["cost_unit"].ToString();
                        //관세
                        double custom;
                        if (!double.TryParse(tmRow[0]["custom"].ToString(), out custom))
                            custom = 0;
                        //과세
                        double tax;
                        if (!double.TryParse(tmRow[0]["tax"].ToString(), out tax))
                            tax = 0;
                        //부대비용
                        double incidental_expense;
                        if (!double.TryParse(tmRow[0]["incidental_expense"].ToString(), out incidental_expense))
                            incidental_expense = 0;

                        dgvProduct.Rows[n].Cells["custom"].Value = custom;
                        dgvProduct.Rows[n].Cells["tax"].Value = tax;
                        dgvProduct.Rows[n].Cells["incidental_expense"].Value = incidental_expense;
                        dgvProduct.Rows[n].Cells["manager"].Value = tmRow[0]["manager"].ToString();
                    }
                    //평균원가 없으면 오퍼내역에서 가져온다==================================================================
                    DataRow[] pdRow = null;
                    if (pdDt.Rows.Count > 0 && Convert.ToDouble(dgvProduct.Rows[n].Cells["average_cost_price"].Value.ToString()) == 0)
                    {
                        string whr = "product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                            + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                            + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                            + " AND (box_weight = '" + productDt.Rows[i]["단위"].ToString() + "'"
                            + " OR box_weight = '" + productDt.Rows[i]["계산단위"].ToString() + "')";
                        pdRow = pdDt.Select(whr);
                        if (pdRow.Length > 0)
                        {
                            double exRate = Convert.ToDouble(pdRow[0]["exchange_rate"].ToString());
                            if (exRate == 0)
                                exRate = exchange_rate;
                            double unit_price = Convert.ToDouble(pdRow[0]["unit_price"].ToString());
                            double product_unit = Convert.ToDouble(pdRow[0]["unit"].ToString());

                            dgvProduct.Rows[n].Cells["average_cost_price"].Value = (unit_price * product_unit * exRate).ToString("#,##0");
                        }
                    }
                    //최근오퍼가 내역===================================================================
                    DataRow[] copRow = null;
                    if (copDt.Rows.Count > 0)
                    {
                        string whr = "product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                            + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                            + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                            + " AND (unit = '" + productDt.Rows[i]["단위"].ToString() + "'"
                            + " OR unit2 = '" + productDt.Rows[i]["단위"].ToString() + "')";
                        copRow = copDt.Select(whr);
                        if (copRow.Length > 0)
                        {
                            double offer_price = Convert.ToDouble(copRow[0]["purchase_price"].ToString());
                            dgvProduct.Rows[n].Cells["offer_price"].Value = copRow[0]["purchase_price"].ToString();
                            dgvProduct.Rows[n].Cells["cost_unit"].Value = copRow[0]["cost_unit"].ToString();

                            //관세
                            double custom;
                            if (!double.TryParse(dgvProduct.Rows[n].Cells["custom"].Value.ToString(), out custom))
                                custom = 0;
                            custom = custom / 100;
                            //과세
                            double tax;
                            if (!double.TryParse(dgvProduct.Rows[n].Cells["tax"].Value.ToString(), out tax))
                                tax = 0;
                            tax = tax / 100;
                            //부대비용
                            double incidental_expense;
                            if (!double.TryParse(dgvProduct.Rows[n].Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                                incidental_expense = 0;
                            incidental_expense = incidental_expense / 100;
                            //동원
                            double dongwon = 0;
                            if (cbDongwon.Checked)
                                dongwon = 0.025;
                            //단위 -> 트레이
                            double unit = Convert.ToDouble(dgvProduct.Rows[n].Cells["unit"].Value);
                            if (!string.IsNullOrEmpty(dgvProduct.Rows[n].Cells["cost_unit"].Value.ToString()) && Convert.ToDouble(dgvProduct.Rows[n].Cells["cost_unit"].Value.ToString()) > 0)
                            {
                                unit = Convert.ToDouble(dgvProduct.Rows[n].Cells["cost_unit"].Value.ToString());
                            }
                            //원가계산
                            double cost_price = unit * offer_price;
                            if (tax > 0)
                            {
                                cost_price += (unit * offer_price) * tax;
                            }
                            if (custom > 0)
                            {
                                cost_price += (unit * offer_price) * custom;
                            }
                            if (incidental_expense > 0)
                            {
                                cost_price += (unit * offer_price) * incidental_expense;
                            }
                            cost_price += (unit * offer_price) * dongwon;
                            cost_price = cost_price * exchange_rate;


                            //차이가 심할땐 단위수량으로 나눔
                            double increase_rate;
                            if(average_cout_price > 0)
                                increase_rate = ((cost_price - average_cout_price) / average_cout_price) * 100;
                            else
                                increase_rate = ((cost_price - sales_price) / sales_price) * 100;
                            if (increase_rate > 150)
                            {
                                cost_price = cost_price / unit;
                            }

                            //원가
                            dgvProduct.Rows[n].Cells["offer_cost_price"].Value = cost_price.ToString("#,##0");
                            dgvProduct.Rows[n].Cells["offer_updatetime"].Value = Convert.ToDateTime(copRow[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                            //거래처
                            dgvProduct.Rows[n].Cells["offer_company"].Value = copRow[0]["company"].ToString();
                            //평균원가
                            double order_qty;
                            if (dgvProduct.Rows[n].Cells["order_qty"].Value == null || !double.TryParse(dgvProduct.Rows[n].Cells["order_qty"].Value.ToString(), out order_qty))
                                order_qty  = 0;

                            double total_average_cost_price = ((Convert.ToDouble(productDt.Rows[n]["평균원가"].ToString()) * stock) + (cost_price * order_qty)) / (stock + order_qty);
                            dgvProduct.Rows[n].Cells["total_average_cost_price"].Value = total_average_cost_price.ToString("#,##0");
                        }
                    }
                }
            }
        }
        //담당자 검색 O
        private void DataOutputManager(DataTable productDt, DataTable uppDt)
        {
            for (int i = 0; i < productDt.Rows.Count; i++)
            {
                DataRow[] copRow = null;
                string whr = "product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                    + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                    + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                    + " AND unit = '" + productDt.Rows[i]["단위"].ToString() + "'";
                copRow = copDt.Select(whr);
                if (copRow.Length > 0 && copRow[0]["edit_user"].ToString().Contains(txtManager.Text.Trim()))
                {
                    int n = dgvProduct.Rows.Add();
                    //품목정보
                    dgvProduct.Rows[n].Cells["product"].Value = productDt.Rows[i]["품명"].ToString();
                    dgvProduct.Rows[n].Cells["origin"].Value = productDt.Rows[i]["원산지"].ToString();
                    dgvProduct.Rows[n].Cells["sizes"].Value = productDt.Rows[i]["규격"].ToString();
                    dgvProduct.Rows[n].Cells["unit"].Value = productDt.Rows[i]["단위"].ToString();
                    //재고 및 시세정보
                    dgvProduct.Rows[n].Cells["stock"].Value = productDt.Rows[i]["재고수"].ToString();
                    dgvProduct.Rows[n].Cells["sales_count"].Value = productDt.Rows[i]["매출수"].ToString();
                    dgvProduct.Rows[n].Cells["sales_amount"].Value = Convert.ToDouble(productDt.Rows[i]["매출금액"].ToString()).ToString("#,##0");
                    dgvProduct.Rows[n].Cells["sales_price"].Value = Convert.ToDouble(productDt.Rows[i]["매출단가"].ToString()).ToString("#,##0");
                    dgvProduct.Rows[n].Cells["normal_price"].Value = Convert.ToDouble(productDt.Rows[i]["일반시세"].ToString()).ToString("#,##0");
                    dgvProduct.Rows[n].Cells["average_cost_price"].Value = Convert.ToDouble(productDt.Rows[i]["평균원가"].ToString()).ToString("#,##0");
                    //통관대기품목
                    double shipment_qty = 0;
                    double unpending_qty_before = 0;
                    double unpending_qty_after = 0;
                    DataRow[] uppRow = null;
                    if (uppDt.Rows.Count > 0)
                    {
                        whr = "product = '" + productDt.Rows[i]["품명"].ToString() + "'"
                            + " AND origin = '" + productDt.Rows[i]["원산지"].ToString() + "'"
                            + " AND sizes = '" + productDt.Rows[i]["규격"].ToString() + "'"
                            + " AND box_weight = '" + productDt.Rows[i]["단위"].ToString() + "'";
                        uppRow = uppDt.Select(whr);
                        if (uppRow.Length > 0)
                        {
                            for (int j = 0; j < uppRow.Length; j++)
                            {
                                string bl_no = uppRow[j]["bl_no"].ToString();
                                string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                                DateTime dtt;
                                //계약만 한 상태
                                if (string.IsNullOrEmpty(bl_no))
                                {
                                    shipment_qty += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                }
                                //배송중인 상태
                                else if (!string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date))
                                {
                                    unpending_qty_before += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                }
                                //입항은 했지만 미통관인 상태
                                else if (!string.IsNullOrEmpty(bl_no) && !string.IsNullOrEmpty(warehousing_date))
                                {
                                    unpending_qty_after += Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                }
                            }
                        }
                    }
                    dgvProduct.Rows[n].Cells["shipment_stock"].Value = (shipment_qty + unpending_qty_before).ToString("#,##0");
                    //재고수
                    double total_stock = Convert.ToDouble(productDt.Rows[i]["재고수"].ToString());
                    //예약수
                    double reserved_stock = Convert.ToDouble(productDt.Rows[i]["예약수"].ToString());
                    //관리자 예약
                    double admin_reserved_stock = 0;
                    string admin_reserved_stock_detail = productDt.Rows[i]["예약상세"].ToString().Trim();
                    if (!string.IsNullOrEmpty(admin_reserved_stock_detail))
                    {
                        string txtDetail = "";
                        string[] detail = admin_reserved_stock_detail.Split(' ');
                        for (int j = 0; j < detail.Length; j++)
                        {
                            string d = detail[j].ToString();
                            if (detail[j].ToString().Contains("관리자"))
                            {
                                if (detail[j].Substring(0, 1) == "/")
                                {
                                    detail[j] = detail[j].Substring(1, detail[j].Length - 1).Trim();
                                }
                                int rStrok = Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                                admin_reserved_stock += Convert.ToInt32(detail[j].Trim().Substring(0, detail[j].Trim().IndexOf("/")));
                            }
                            else
                            {
                                txtDetail += " " + detail[j].ToString();
                            }
                        }
                        admin_reserved_stock_detail = txtDetail;
                    }
                    dgvProduct.Rows[n].Cells["reserved_stock"].Value = (reserved_stock - admin_reserved_stock).ToString("#,##0");
                    dgvProduct.Rows[n].Cells["reserved_stock_detail"].Value = admin_reserved_stock_detail.Trim();
                    // 가용재고 = 재고수 - 예약수 + 관리자 예약
                    double stock = total_stock - reserved_stock + admin_reserved_stock;
                    stock += shipment_qty + unpending_qty_before;
                    dgvProduct.Rows[n].Cells["real_stock"].Value = stock.ToString("#,##0");
                    //일 평균매출, 회전율
                    double month_around = 0;
                    double sales_count = Convert.ToDouble(dgvProduct.Rows[n].Cells["sales_count"].Value.ToString());
                    double day_sales = 0;
                    if (stock > 0 && sales_count > 0)
                    {
                        day_sales = sales_count / workDays;
                        month_around = (stock / day_sales) / 21;
                    }
                    else if (stock > 0 && sales_count == 0)
                    {
                        month_around = 100;
                    }

                    dgvProduct.Rows[n].Cells["day_sales_count"].Value = day_sales.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["month_sales_count"].Value = (day_sales * 21).ToString("#,##0");
                    dgvProduct.Rows[n].Cells["month_around"].Value = month_around.ToString("#,##0.00");
                    //환율
                    double exchange_rate = customRate;
                    dgvProduct.Rows[n].Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0.00");

                    //최근오퍼가 내역(초기화)
                    dgvProduct.Rows[n].Cells["custom"].Value = 0;
                    dgvProduct.Rows[n].Cells["tax"].Value = 0;
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = 0;
                    dgvProduct.Rows[n].Cells["exchange_rate"].Value = 0;
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = 0;
                    //최근오퍼가 내역(출력)
                    dgvProduct.Rows[n].Cells["offer_price"].Value = copRow[0]["purchase_price"].ToString();
                    dgvProduct.Rows[n].Cells["cost_unit"].Value = copRow[0]["cost_unit"].ToString();

                    //관세
                    double custom;
                    if (!double.TryParse(copRow[0]["custom"].ToString(), out custom))
                        custom = 0;
                    //과세
                    double tax;
                    if (!double.TryParse(copRow[0]["tax"].ToString(), out tax))
                        tax = 0;
                    //부대비용
                    double incidental_expense;
                    if (!double.TryParse(copRow[0]["incidental_expense"].ToString(), out incidental_expense))
                        incidental_expense = 0;
                    //동원
                    double dongwon = 0;
                    if (cbDongwon.Checked)
                        dongwon = 0.025;

                    dgvProduct.Rows[n].Cells["custom"].Value = custom;
                    dgvProduct.Rows[n].Cells["tax"].Value = tax;
                    dgvProduct.Rows[n].Cells["incidental_expense"].Value = incidental_expense;

                    custom = custom / 100;
                    tax = tax / 100;
                    incidental_expense = incidental_expense / 100;

                    //트레이 -> 유닛
                    double unit = Convert.ToDouble(dgvProduct.Rows[n].Cells["unit"].Value);
                    if (!string.IsNullOrEmpty(copRow[0]["cost_unit"].ToString()) && Convert.ToDouble(copRow[0]["cost_unit"].ToString()) > 0)
                    {
                        unit = Convert.ToDouble(copRow[0]["cost_unit"].ToString());
                    }
                    //매입단가
                    double purchase_price = Convert.ToDouble(dgvProduct.Rows[n].Cells["offer_price"].Value);
                    double cost_price = unit * purchase_price;
                    if (tax > 0)
                    {
                        cost_price += (unit * purchase_price) * tax;
                    }
                    if (custom > 0)
                    {
                        cost_price += (unit * purchase_price) * custom;
                    }
                    if (incidental_expense > 0)
                    {
                        cost_price += (unit * purchase_price) * incidental_expense;
                    }
                    //동원
                    cost_price = (unit * purchase_price) * dongwon;
                    //원가계산
                    cost_price = cost_price * exchange_rate;
                    /*double sales_price = Convert.ToDouble(dgvProduct.Rows[i].Cells["domestic_sales_price1"].Value);
                    double purchase_price1 = Convert.ToDouble(dgvProduct.Rows[i].Cells["purchase_price1"].Value);
                    double purchase_price2 = Convert.ToDouble(dgvProduct.Rows[i].Cells["purchase_price2"].Value);*/
                    //원가
                    dgvProduct.Rows[n].Cells["offer_cost_price"].Value = cost_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["offer_updatetime"].Value = Convert.ToDateTime(copRow[0]["updatetime"].ToString()).ToString("yyyy-MM-dd");
                    //거래처
                    dgvProduct.Rows[n].Cells["offer_company"].Value = copRow[0]["company"].ToString();
                    //평균원가
                    double sales_cost_price = cost_price;
                    if (Convert.ToDouble(dgvProduct.Rows[n].Cells["average_cost_price"].Value.ToString()) > 0)
                        sales_cost_price = Convert.ToDouble(dgvProduct.Rows[n].Cells["average_cost_price"].Value.ToString());
                    dgvProduct.Rows[n].Cells["total_average_cost_price"].Value = ((cost_price + sales_cost_price) / 2).ToString("#,##0");
                    //담당자
                    dgvProduct.Rows[n].Cells["manager"].Value = copRow[0]["edit_user"].ToString();
                    //소진일자
                    string sttDate, endDate;
                    DateTime exhausted_date;
                    double avg_day_sales = Convert.ToDouble(dgvProduct.Rows[n].Cells["day_sales_count"].Value.ToString());
                    common.GetShortDay3(DateTime.Now, stock, Math.Round(avg_day_sales, 2), uppRow, out sttDate, out endDate);
                    //출력
                    if (!string.IsNullOrEmpty(sttDate) && DateTime.TryParse(sttDate, out exhausted_date))
                    {
                        dgvProduct.Rows[n].Cells["exhausted_date"].Value = exhausted_date.ToString("yyyy-MM-dd");
                    }
                }
            }
        }

        #endregion

        #region Button
        private void btnSttdateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEnddateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

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
        private void txtEnddate_KeyDown(object sender, KeyEventArgs e)
        {
            Control tb = (Control)sender;
            if (e.KeyCode == Keys.Enter)
            {
                //날짜 완성
                Control tbb = (Control)sender;
                tbb.Text = common.strDatetime(tbb.Text);
            }
        }

        private void txtDivision_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetData();
            }
        }

        private void ProductCostComparison_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.W:
                        btnCalculate.PerformClick();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtDivision.Text = String.Empty;
                        txtManager.Text = String.Empty;
                        txtProduct.Focus();
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
                    case Keys.F1:
                        cbStock.Checked = !cbStock.Checked;
                        break;
                    case Keys.F2:
                        cbSales.Checked = !cbSales.Checked;
                        break;
                    case Keys.F3:
                        cbOffer.Checked = !cbOffer.Checked;
                        break;
                }
            }
        }
        private void txtExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double exchange_rate;
                if (double.TryParse(txtExchangeRate.Text, out exchange_rate) && dgvProduct.Rows.Count > 0)
                {
                    this.dgvProduct.CellValueChanged -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        DataGridViewRow row = dgvProduct.Rows[i];
                        dgvProduct.Rows[i].Cells["exchange_rate"].Value = exchange_rate.ToString("#,##0.00");
                    }
                    this.dgvProduct.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProduct_CellValueChanged);
                }
                CalculateCostPrice();
            }
        }
        #endregion

        #region Checkbox, Radio event
        private void rbCostprice_CheckedChanged(object sender, EventArgs e)
        {
            CalculateCostPrice();
        }
        private void cbStock_CheckedChanged(object sender, EventArgs e)
        {
            dgvProduct.Columns["stock"].Visible = cbStock.Checked;
            dgvProduct.Columns["reserved_stock"].Visible = cbStock.Checked;
            //dgvProduct.Columns["reserved_stock_detail"].Visible = cbStock.Checked;
            dgvProduct.Columns["shipment_stock"].Visible = cbStock.Checked;
        }
        private void cbOffer_CheckedChanged(object sender, EventArgs e)
        {
            dgvProduct.Columns["custom"].Visible = cbOffer.Checked;
            dgvProduct.Columns["tax"].Visible = cbOffer.Checked;
            dgvProduct.Columns["exchange_rate"].Visible = cbOffer.Checked;
            dgvProduct.Columns["incidental_expense"].Visible = cbOffer.Checked;
        }

        private void cbSales_CheckStateChanged(object sender, EventArgs e)
        {
            dgvProduct.Columns["day_sales_count"].Visible = cbSales.Checked;
            dgvProduct.Columns["sales_count"].Visible = cbSales.Checked;
            dgvProduct.Columns["sales_amount"].Visible = cbSales.Checked;
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvProduct.Rows[e.RowIndex];

                double qty;
                if ((dgvProduct.Columns[e.ColumnIndex].Name == "order_qty"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "average_cost_price"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "real_stock"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "sales_count"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_cost_price"
                    ))
                {
                    caculate(row, 1);
                }
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "offer_price"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "custom"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "tax"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "incidental_expense"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "exchange_rate")
                {
                    CalculateCostPrice(e.RowIndex);
                }
                //월평균판매
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "month_sales_count")
                {
                    caculate(row, 2);
                }
                //일평균판매
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "day_sales_count")
                {
                    caculate(row, 3);
                }
            }
        }


        private void caculate(DataGridViewRow row, int calculate_type = 1)
        {
            //추가 오더수량
            double qty = 0;
            if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out qty))
            {
                qty = 0;
            }
            //재고수량
            double stock = Convert.ToDouble(row.Cells["real_stock"].Value);
            //수량합계
            row.Cells["total_stock"].Value = stock + qty;

            //일,월 판매량, 회전율
            double month_around = 0;
            double total_month_around = 0;
            double day_sales = 0;
            double month_sales = 0;
            
            //전부계산
            if (calculate_type == 1)
            {
                double sales_count = Convert.ToDouble(row.Cells["sales_count"].Value.ToString());
                if ((stock + qty) > 0 && sales_count > 0)
                {
                    day_sales = sales_count / workDays;
                    month_around = (stock  / day_sales) / 21;
                    total_month_around = ((stock + qty) / day_sales) / 21;
                }
                else if ((stock + qty) > 0 && sales_count == 0)
                {
                    month_around = 100;
                    total_month_around = 100;
                }
            }
            //월 판매량 수정
            else if (calculate_type == 2)
            {
                month_sales = Convert.ToDouble(row.Cells["month_sales_count"].Value.ToString());
                day_sales = month_sales / 21;
                month_around = (stock ) / month_sales;
                total_month_around = (stock + qty) / month_sales;
            }
            //일 판매량 수정
            else if (calculate_type == 3)
            {
                day_sales = Convert.ToDouble(row.Cells["day_sales_count"].Value.ToString());
                total_month_around = stock  / month_sales;
                total_month_around = (stock + qty) / month_sales;
            }

            row.Cells["month_around"].Value = month_around.ToString("#,##0.00");
            row.Cells["total_month_around"].Value = total_month_around.ToString("#,##0.00");

            //원가계산
            double cost_price;
            if (row.Cells["offer_cost_price"].Value == null || !double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out cost_price))
                cost_price = 0;
            //평균원가
            double average_cost_price;
            if (!double.TryParse(row.Cells["average_cost_price"].Value.ToString(), out average_cost_price))
                average_cost_price = 0;

            //원가의 평균
            if (average_cost_price > 0)
            {
                double real_stock = Convert.ToDouble(row.Cells["real_stock"].Value.ToString());
                double total_average_price = ((average_cost_price * real_stock) + (cost_price * qty)) / (real_stock + qty);
                row.Cells["total_average_cost_price"].Value = total_average_price.ToString("#,##0");
            }
            else
            {
                row.Cells["total_average_cost_price"].Value = cost_price.ToString("#,##0");
            }
            //소진일자 재계산
            DateTime exhausted_date;
            common.GetShortDay2(DateTime.Now, stock + qty, Math.Round(day_sales, 2), out exhausted_date);
            row.Cells["exhausted_date"].Value = exhausted_date.ToString("yyyy-MM-dd");
            
        }
        private void dgvProduct_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "normal_price"
                    && Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) > 0)
                {
                    DateTime sttDate = Convert.ToDateTime(txtSttdate.Text);
                    DateTime endDate = Convert.ToDateTime(txtEnddate.Text);

                    string detail = priceComparisonRepository.GetAveragePurchasePriceDetail(dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString()
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString()
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString()
                                                                                            , ""
                                                                                            , ""
                                                                                            , ""
                                                                                            , dgvProduct.Rows[e.RowIndex].Cells["unit"].Value.ToString()
                                                                                            , sttDate
                                                                                            , endDate
                                                                                            , Convert.ToInt16(cbCp.Text));
                    if (!string.IsNullOrEmpty(detail))
                    {
                        dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = detail;
                    }
                }
                //예약상세
                else if (dgvProduct.Columns[e.ColumnIndex].Name == "reserved_stock"
                     && Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) > 0)
                {
                    DateTime sttDate = Convert.ToDateTime(txtSttdate.Text);
                    DateTime endDate = Convert.ToDateTime(txtEnddate.Text);

                    DataTable detailDt = priceComparisonRepository.GetReservedDetail(dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString()
                                                                                    , dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString()
                                                                                    , dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString()
                                                                                    , dgvProduct.Rows[e.RowIndex].Cells["unit"].Value.ToString());
                    if (detailDt.Rows.Count > 0)
                    {
                        string detail = detailDt.Rows[0]["예약상세"].ToString();
                        dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = detail;
                    }
                }
                else if ((dgvProduct.Columns[e.ColumnIndex].Name == "offer_price"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_updatetime"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_company")
                    && dgvProduct.Rows[e.RowIndex].Cells["offer_price"].Value != null
                    && Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells["offer_price"].Value) > 0)
                {
                    string txt = "";
                    DataGridViewRow row = dgvProduct.Rows[e.RowIndex];
                    //선택 오퍼가 정보
                    if (copDt.Rows.Count > 0)
                    {
                        string whr = "product = '" + dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString() + "'"
                            + " AND origin = '" + dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString() + "'"
                            + " AND sizes = '" + dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString() + "'"
                            + " AND unit = '" + dgvProduct.Rows[e.RowIndex].Cells["unit"].Value.ToString() + "'";
                        DataRow[] copRow = copDt.Select(whr);
                        if (copRow.Length > 0)
                        {
                            string offer_list = "";
                            for (int i = 0; i < copRow.Length; i++)
                            {
                                offer_list += "\n"
                                    + Convert.ToDateTime(copRow[i]["updatetime"].ToString()).ToString("yyyy-MM-dd") + " | "
                                    + copRow[i]["company"].ToString() + " | "
                                    + Convert.ToDouble(copRow[i]["purchase_price"].ToString()).ToString("#,##0.00");
                            }
                            if (!string.IsNullOrEmpty(offer_list))
                            {
                                txt = offer_list.Trim();
                            }
                        }
                    }
                    //선택 오퍼가 순위
                    double price = Convert.ToDouble(dgvProduct.Rows[e.RowIndex].Cells["offer_price"].Value);
                    DateTime enddate = DateTime.Now;
                    DateTime sttdate = enddate.AddYears(-1);
                    string rank = "";
                    DataTable rankDt = purchasePriceRepository.GetRankingPurchasePriceASOne(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                        , row.Cells["product"].Value.ToString(), row.Cells["origin"].Value.ToString(), row.Cells["sizes"].Value.ToString(), row.Cells["unit"].Value.ToString()
                        , "", price);
                    if (rankDt.Rows.Count > 0)
                    {
                        //순위
                        for (int i = 0; i < rankDt.Rows.Count; i++)
                        {
                            if (rankDt.Rows[i]["division"].ToString() == "1")
                            {
                                double rate = ((((double)i + 1) / rankDt.Rows.Count) * 100);
                                rank = "상위 " + rate.ToString("#,##0.00") + "%";
                            }
                        }
                        //최대, 최소값
                        double MaxPrice;
                        if (!double.TryParse(rankDt.Rows[0]["purchase_price"].ToString(), out MaxPrice))
                        {
                            MaxPrice = 0;
                        }
                        double MinPrice;
                        if (!double.TryParse(rankDt.Rows[rankDt.Rows.Count - 1]["purchase_price"].ToString(), out MinPrice))
                        {
                            MinPrice = 0;
                        }

                        rank += "\n최대단가 : " + MaxPrice.ToString("#,##0.00") + ", 최소단가 : " + MinPrice.ToString("#,##0.00");
                    }
                    else
                    {
                        rank = "등록된 내역이 없습니다.";
                    }
                    txt += "\n===================\n" + rank;
                    dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = txt;
                }
            }
        }
        private void dgvProduct_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvProduct.Columns[e.ColumnIndex].Name == "offer_price"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_company"
                    || dgvProduct.Columns[e.ColumnIndex].Name == "offer_updatetime")
                {

                    if (copDt.Rows.Count > 0)
                    {
                        string whr = "product = '" + dgvProduct.Rows[e.RowIndex].Cells["product"].Value.ToString() + "'"
                            + " AND origin = '" + dgvProduct.Rows[e.RowIndex].Cells["origin"].Value.ToString() + "'"
                            + " AND sizes = '" + dgvProduct.Rows[e.RowIndex].Cells["sizes"].Value.ToString() + "'"
                            + " AND unit = '" + dgvProduct.Rows[e.RowIndex].Cells["unit"].Value.ToString() + "'";
                        DataRow[] copRow = copDt.Select(whr);
                        if (copRow.Length > 0)
                        {
                            //단가리스트 생성
                            OfferPriceList ofl = new OfferPriceList();
                            Point p = dgvProduct.PointToScreen(dgvProduct.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Location);
                            p = new Point(p.X - ofl.Size.Width - dgvProduct.ColumnHeadersHeight, p.Y - ofl.Size.Height + dgvProduct.Rows[e.RowIndex].Height);

                            ofl.StartPosition = FormStartPosition.Manual;
                            string[] price = ofl.Manager(copRow, p);

                            if (price != null)
                            { 
                                dgvProduct.Rows[e.RowIndex].Cells["offer_updatetime"].Value = price[0].ToString();
                                dgvProduct.Rows[e.RowIndex].Cells["offer_price"].Value = Convert.ToDouble(price[1]).ToString("#,##0.00");
                                dgvProduct.Rows[e.RowIndex].Cells["offer_company"].Value = price[2];
                            }
                        }
                    }                    
                }
            }
        }
        #endregion

        #region 우클릭 Method
        //우클릭 메뉴 Create
        private void dgvProduct_MouseUp(object sender, MouseEventArgs e)
        {
            if (um.auth_level < 50)
            {
                return;
            }
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
                    m.Items.Add("선적내역 확인");
                    m.Items.Add("오퍼내역 확인");
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
            try
            {
                if (dgvProduct.SelectedRows.Count > 0)
                {
                    switch (e.ClickedItem.Text)
                    {
                        case "선적내역 확인":
                            {
                                if (dgvProduct.SelectedRows.Count > 0)
                                {
                                    DataGridViewRow row = dgvProduct.SelectedRows[0];
                                    UnconfirmedPending ufp = new UnconfirmedPending(um.user_id
                                                                                , row.Cells["product"].Value.ToString()
                                                                                , row.Cells["origin"].Value.ToString()
                                                                                , row.Cells["sizes"].Value.ToString()
                                                                                , row.Cells["unit"].Value.ToString());
                                    ufp.Show();
                                }
                                break;
                            }
                        case "오퍼내역 확인":
                            {
                                if (dgvProduct.SelectedRows.Count > 0)
                                { 
                                    DataGridViewRow row = dgvProduct.SelectedRows[0];
                                    PurchaseUnitManager pum = new PurchaseUnitManager(um
                                                                                    , row.Cells["offer_updatetime"].Value.ToString()
                                                                                    , row.Cells["product"].Value.ToString()
                                                                                    , row.Cells["origin"].Value.ToString()
                                                                                    , row.Cells["sizes"].Value.ToString()
                                                                                    , row.Cells["unit"].Value.ToString());
                                    pum.Show();
                                }
                                break;
                            }
                        case "역대단가 확인":
                            for (int i = dgvProduct.Columns.Count - 1; i >= 5; i--)
                            {
                                List<string[]> productList = new List<string[]>();
                                foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                                {
                                    if (cell.ColumnIndex >= 5)
                                    {
                                        string[] product = new string[6];
                                        product[0] = dgvProduct.Rows[cell.RowIndex].Cells["product"].Value.ToString();
                                        product[1] = dgvProduct.Rows[cell.RowIndex].Cells["origin"].Value.ToString();
                                        product[2] = dgvProduct.Rows[cell.RowIndex].Cells["sizes"].Value.ToString();
                                        product[3] = dgvProduct.Rows[cell.RowIndex].Cells["unit"].Value.ToString();

                                        double price;
                                        if (dgvProduct.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value == null || !double.TryParse(dgvProduct.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value.ToString(), out price))
                                        {
                                            price = 0;
                                        }
                                        product[4] = price.ToString("#,##0.00");

                                        productList.Add(product);
                                    }
                                }
                                if (productList.Count > 0)
                                {
                                    PurchaseManager.GraphManager gm = new GraphManager(um, productList);
                                    gm.Show();
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
                this.Activate();
            }
        }
        #endregion

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                dgvProduct.ClearSelection();
                dgvProduct.Rows[e.RowIndex].Selected = true;
            }
        }

        private void cbPendingInStock_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    double stock;
                    if (!double.TryParse(row.Cells["stock"].Value.ToString(), out stock))
                    {
                        stock = 0;
                    }
                    double shipment_stock;
                    if (!double.TryParse(row.Cells["shipment_stock"].Value.ToString(), out shipment_stock))
                    {
                        shipment_stock = 0;
                    }

                    if (cbPendingInStock.Checked)
                        stock += shipment_stock;

                    row.Cells["real_stock"].Value = stock;

                    //일 평균매출, 회전율
                    double month_around = 0;
                    double sales_count = Convert.ToDouble(row.Cells["sales_count"].Value.ToString());
                    double day_sales = 0;
                    if (stock > 0 && sales_count > 0)
                    {
                        day_sales = sales_count / workDays;
                        month_around = (stock / day_sales) / 21;
                    }
                    else if (stock > 0 && sales_count == 0)
                    {
                        month_around = 100;
                    }

                    row.Cells["day_sales_count"].Value = day_sales.ToString("#,##0");
                    row.Cells["month_sales_count"].Value = (day_sales * 21).ToString("#,##0");
                    row.Cells["month_around"].Value = month_around.ToString("#,##0.00");

                    double order_qty;
                    if (row.Cells["order_qty"].Value == null || !double.TryParse(row.Cells["order_qty"].Value.ToString(), out order_qty))
                    {
                        order_qty = 0;
                    }
                    //총회전율
                    double total_month_around = ((stock + order_qty) / day_sales) / 21;
                    row.Cells["total_month_around"].Value = total_month_around.ToString("#,##0.00");

                    //원가계산
                    double cost_price;
                    if (row.Cells["offer_cost_price"].Value == null || !double.TryParse(row.Cells["offer_cost_price"].Value.ToString(), out cost_price))
                        cost_price = 0;
                    //평균원가
                    double average_cost_price;
                    if (!double.TryParse(row.Cells["average_cost_price"].Value.ToString(), out average_cost_price))
                        average_cost_price = 0;

                    //원가의 평균
                    if (average_cost_price > 0)
                    {
                        double total_average_price = ((average_cost_price * stock) + (cost_price * order_qty)) / (stock + order_qty);
                        row.Cells["total_average_cost_price"].Value = total_average_price.ToString("#,##0");
                    }
                    else
                    {
                        row.Cells["total_average_cost_price"].Value = cost_price.ToString("#,##0");
                    }

                    double day_sales_count = sales_count / workDays;
                    DateTime exhausted_date;
                    common.GetShortDay2(DateTime.Now, stock, Math.Round(day_sales_count, 2), out exhausted_date);
                    row.Cells["exhausted_date"].Value = exhausted_date.ToString("yyyy-MM-dd");                    
                }
            }
        }

        private void cbDongwon_CheckedChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {

                    DataGridViewRow row = dgvProduct.Rows[i];
                    //단위(트레이)
                    double unit;
                    if (!double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                        unit = 0;
                    if (row.Cells["cost_unit"].Value != null && row.Cells["cost_unit"].Value.ToString() != "" && Convert.ToDouble(row.Cells["cost_unit"].Value.ToString()) > 0)
                        unit = Convert.ToDouble(row.Cells["cost_unit"].Value.ToString());
                    //오퍼가
                    double offer_price;
                    if (row.Cells["offer_price"].Value == null || !double.TryParse(row.Cells["offer_price"].Value.ToString(), out offer_price))
                        offer_price = 0;
                    //환율
                    double exchange_rate;
                    if (row.Cells["exchange_rate"].Value == null || !double.TryParse(row.Cells["exchange_rate"].Value.ToString(), out exchange_rate))
                        exchange_rate = 0;
                    //관세
                    double custom;
                    if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                        custom = 0;
                    custom = custom / 100;
                    //과세
                    double tax;
                    if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                        tax = 0;
                    tax = tax / 100;
                    //부대비용
                    double incidental_expense;
                    if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense = incidental_expense / 100;
                    //동원
                    double dongwon = 0;
                    if (cbDongwon.Checked)
                        dongwon = 0.025;
                    //원가계산
                    double cost_price = 0;
                    double temp = unit * offer_price * exchange_rate;
                    cost_price = temp;
                    cost_price += temp * tax;
                    cost_price += temp * custom;
                    cost_price += temp * incidental_expense;
                    cost_price += temp * dongwon;

                    row.Cells["offer_cost_price"].Value = cost_price.ToString("#,##0");
                }
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {

                    DataGridViewRow row = dgvProduct.Rows[i];
                    //단위(트레이)
                    double unit;
                    if (!double.TryParse(row.Cells["unit"].Value.ToString(), out unit))
                        unit = 0;
                    if (row.Cells["cost_unit"].Value != null && row.Cells["cost_unit"].Value.ToString() != "" && Convert.ToDouble(row.Cells["cost_unit"].Value.ToString()) > 0)
                        unit = Convert.ToDouble(row.Cells["cost_unit"].Value.ToString());
                    //오퍼가
                    double offer_price;
                    if (row.Cells["offer_price"].Value == null || !double.TryParse(row.Cells["offer_price"].Value.ToString(), out offer_price))
                        offer_price = 0;
                    //환율
                    double exchange_rate;
                    if (row.Cells["exchange_rate"].Value == null || !double.TryParse(row.Cells["exchange_rate"].Value.ToString(), out exchange_rate))
                        exchange_rate = 0;
                    //관세
                    double custom;
                    if (row.Cells["custom"].Value == null || !double.TryParse(row.Cells["custom"].Value.ToString(), out custom))
                        custom = 0;
                    custom = custom / 100;
                    //과세
                    double tax;
                    if (row.Cells["tax"].Value == null || !double.TryParse(row.Cells["tax"].Value.ToString(), out tax))
                        tax = 0;
                    tax = tax / 100;
                    //부대비용
                    double incidental_expense;
                    if (row.Cells["incidental_expense"].Value == null || !double.TryParse(row.Cells["incidental_expense"].Value.ToString(), out incidental_expense))
                        incidental_expense = 0;
                    incidental_expense = incidental_expense / 100;
                    //동원
                    double dongwon = 0;
                    if (cbDongwon.Checked)
                        dongwon = 0.025;
                    //원가계산
                    double cost_price = 0;
                    double temp = unit * offer_price * exchange_rate;
                    cost_price = temp;
                    cost_price += temp * tax;
                    cost_price += temp * custom;
                    cost_price += temp * incidental_expense;
                    cost_price += temp * dongwon;

                    row.Cells["offer_cost_price"].Value = cost_price.ToString("#,##0");
                }
            }
        }

        private void btnAdvancedSearch_Click(object sender, EventArgs e)
        {
            pnAdvancedSearch.Visible = true;
        }

        private void btnAdvancedSearchExit_Click(object sender, EventArgs e)
        {
            pnAdvancedSearch.Visible = false;
        }
    }
}
