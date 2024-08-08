using AdoNetWindow.Model;
using AdoNetWindow.Pending;
using Repositories.Pending;
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
using Repositories.SEAOVER;
using Repositories.SEAOVER.Purchase;

namespace AdoNetWindow.SEAOVER.PriceComparison
{
    public partial class MergeProduct : Form
    {
        IProductGroupRepository productGroupRepository = new ProductGroupRepository();
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um;
        PriceComparison pc;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        DataTable uppDt = null;
        double exchange_rate;
        public MergeProduct(UsersModel um, PriceComparison pc, DataTable uppDt, DataGridView dgvProduct, List<DataRow> subRowList, double exchange_rate)
        {
            InitializeComponent();
            this.um = um;
            this.pc = pc;
            //초기화
            dgvMergeProduct.Rows.Clear();
            dgvMergeProduct.Columns.Clear();

            dgvMergeProduct.ColumnHeadersDefaultCellStyle = dgvProduct.ColumnHeadersDefaultCellStyle;
            for (int i = 0; i < dgvProduct.Columns.Count; i++)
            {
                dgvMergeProduct.Columns.Add("sub_" + dgvProduct.Columns[i].Name, dgvProduct.Columns[i].HeaderText);
                dgvMergeProduct.Columns["sub_" + dgvProduct.Columns[i].Name].HeaderCell.Style = dgvProduct.Columns[i].HeaderCell.Style;
                dgvMergeProduct.Columns["sub_" + dgvProduct.Columns[i].Name].DefaultCellStyle = dgvProduct.Columns[i].DefaultCellStyle;
                dgvMergeProduct.Columns["sub_" + dgvProduct.Columns[i].Name].Width = dgvProduct.Columns[i].Width;
                dgvMergeProduct.Columns["sub_" + dgvProduct.Columns[i].Name].Visible = dgvProduct.Columns[i].Visible;
            }
            SetHeaderStyle();
            //데이터 출력
            if (subRowList.Count > 0)
            {
                for (int i = 0; i < subRowList.Count; i++)
                {
                    int n = dgvMergeProduct.Rows.Add();
                    for (int j = 0; j < subRowList[0].ItemArray.Length; j++)
                    {
                        double d;
                        if(double.TryParse(subRowList[i][j].ToString(), out d))
                            dgvMergeProduct.Rows[n].Cells[j].Value = d;
                        else
                            dgvMergeProduct.Rows[n].Cells[j].Value = subRowList[i][j].ToString();
                    }
                    dgvMergeProduct.Rows[i].Cells["sub_sales_cost_price"].ToolTipText = dgvMergeProduct.Rows[i].Cells["sub_sales_cost_price_tooltip"].Value.ToString();
                }
            }
        }

        #region Method
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvMergeProduct;
            dgv.Columns["sub_shipment_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_unpending_qty_before"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_seaover_unpending"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_seaover_pending"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_reserved_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_real_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_sales_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_average_day_sales_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_excluded_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_average_month_sales_count"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_sales_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_purchase_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_average_purchase_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_normal_price_rate"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_margin"].DefaultCellStyle.Format = "#,###,##0.##"; // 소수점 2자리까지 있으
            dgv.Columns["sub_sales_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_pending_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_average_sales_cost_price1"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_average_sales_cost_price2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_offer_cost_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_offer_price"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_order_qty"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_total_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_total_real_stock"].DefaultCellStyle.Format = "#,###,##0"; // 콤마           
            dgv.Columns["sub_average_purchase_price"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_month_around"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_month_around_in_shipment"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            //dgv.Columns["sub_month_around_out_shipment"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_month_around_1"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_month_around_45"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_month_around_2"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_month_around_3"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_month_around_6"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_month_around_12"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_month_around_18"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_enable_sales_days"].DefaultCellStyle.Format = "#,###,##0"; // 콤마
            dgv.Columns["sub_exhausted_count"].DefaultCellStyle.Format = "#,##0";
            dgv.Columns["sub_month_around2"].DefaultCellStyle.Format = "#,###,##0.00"; // 콤마
            dgv.Columns["sub_enable_sales_days2"].DefaultCellStyle.Format = "#,###,##0"; // 콤마



            // 전체 컬럼의 Sorting 기능 차단 
            foreach (DataGridViewColumn item in dgv.Columns)
            {
                item.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            //틀고정
            dgv.Columns["sub_category"].Frozen = true;
            dgv.Columns["sub_product"].Frozen = true;
            dgv.Columns["sub_origin"].Frozen = true;
            dgv.Columns["sub_sizes"].Frozen = true;
            dgv.Columns["sub_unit"].Frozen = true;
        }
        public void SetLocation(Point p)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = p;
            this.TopMost = true;
        }
        public void SetDgvMergeProductVisible(DataGridView dgvProduct)
        {
            if (dgvMergeProduct.Columns.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Columns.Count; i++)
                    dgvMergeProduct.Columns["sub_" + dgvProduct.Columns[i].Name].Visible = dgvProduct.Columns[i].Visible;
            }
        }
        #endregion

        #region Key down
        private void MergeProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Dispose();
            else if (e.KeyCode == Keys.F1)
                pc.CHangeCheckBox(1);
            else if (e.KeyCode == Keys.F2)
                pc.CHangeCheckBox(2);
            else if (e.KeyCode == Keys.F3)
                pc.CHangeCheckBox(3);
            else if (e.KeyCode == Keys.F4)
                pc.CHangeCheckBox(4);
            else if (e.KeyCode == Keys.F6)
                pc.CHangeCheckBox(5);
        }
        #endregion

        #region 우클릭 메뉴
        private void dgvMergeProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvMergeProduct.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("대표품목 취소");
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvMergeProduct, e.Location);
                    //Selection
                    /*PendingList.ClearSelection();
                    DataGridViewRow selectRow = this.PendingList.Rows[e.RowIndex];
                    selectRow.Selected = !selectRow.Selected;*/
                }
            }
            catch (Exception ex)
            {

            }
        }
        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvMergeProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvMergeProduct.SelectedRows[0];
                    if (dr.Index < 0)
                        return;

                    int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                    switch (e.ClickedItem.Text)
                    {
                        case "대표품목 취소":

                            if (messageBox.Show(this, "선택한 품목을 대표품목 묶음에서 취소하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                List<StringBuilder> sqlList = new List<StringBuilder>();
                                for (int i = dgvMergeProduct.RowCount - 1; i >= 0; i--)
                                {
                                    StringBuilder sql = productGroupRepository.DeleteSubProduct(dgvMergeProduct.Rows[i].Cells["main_id"].Value.ToString(), dgvMergeProduct.Rows[i].Cells["sub_id"].Value.ToString());
                                    sqlList.Add(sql);
                                }

                                if (commonRepository.UpdateTran(sqlList) == -1)
                                    messageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                                else
                                {
                                    for (int i = dgvMergeProduct.RowCount - 1; i >= 0; i--)
                                        dgvMergeProduct.Rows.Remove(dgvMergeProduct.Rows[i]);
                                }
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
        private void dgvMergeProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgvMergeProduct.SelectedRows.Count <= 1)
                {
                    dgvMergeProduct.ClearSelection();
                    dgvMergeProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        #endregion

        private void dgvMergeProduct_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //선적원가
                if (dgvMergeProduct.Columns[e.ColumnIndex].Name == "sub_pending_cost_price")
                {
                    DataGridViewRow row = dgvMergeProduct.Rows[e.RowIndex];

                    double custom;
                    if (row.Cells["sub_custom"].Value == null || !double.TryParse(row.Cells["sub_custom"].Value.ToString(), out custom))
                        custom = 0;
                    else
                        custom = custom / 100;
                    double tax;
                    if (row.Cells["sub_tax"].Value == null || !double.TryParse(row.Cells["sub_tax"].Value.ToString(), out tax))
                        tax = 0;
                    else
                        tax = tax / 100;
                    double incidental_expense;
                    if (row.Cells["sub_incidental_expense"].Value == null || !double.TryParse(row.Cells["sub_incidental_expense"].Value.ToString(), out incidental_expense))
                        incidental_expense = 0;
                    else
                        incidental_expense = incidental_expense / 100;

                    string tooltipTxt = "";

                    double shipment_qty = 0;
                    double unpending_qty_before = 0;
                    double unpending_qty_after = 0;
                    double shipment_cost_price = 0;
                    DataRow[] uppRow = null;
                    if (uppDt != null && uppDt.Rows.Count > 0)
                    {
                        string whr = "product = '" + row.Cells["sub_product"].Value.ToString() + "'"
                            + " AND origin = '" + row.Cells["sub_origin"].Value.ToString() + "'"
                            + " AND sizes = '" + row.Cells["sub_sizes"].Value.ToString() + "'"
                            + " AND box_weight = '" + row.Cells["sub_seaover_unit"].Value.ToString() + "'";
                        uppRow = uppDt.Select(whr);
                        if (uppRow.Length > 0)
                        {
                            double total_cost_price = 0;
                            double total_qty = 0;
                            for (int j = 0; j < uppRow.Length; j++)
                            {
                                shipment_qty = 0;
                                unpending_qty_before = 0;
                                unpending_qty_after = 0;


                                string bl_no = uppRow[j]["bl_no"].ToString();
                                string warehousing_date = uppRow[j]["warehousing_date"].ToString();
                                //계약만 한 상태
                                if (string.IsNullOrEmpty(bl_no))
                                    shipment_qty = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                //배송중인 상태
                                else if (!string.IsNullOrEmpty(bl_no) && string.IsNullOrEmpty(warehousing_date))
                                    unpending_qty_before = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                //입항은 했지만 미통관인 상태
                                else if (!string.IsNullOrEmpty(bl_no) && !string.IsNullOrEmpty(warehousing_date))
                                    unpending_qty_after = Convert.ToDouble(uppRow[j]["quantity_on_paper"]);
                                //선적원가
                                if (string.IsNullOrEmpty(warehousing_date))
                                {
                                    double box_weight;
                                    if (uppRow[j]["box_weight"] == null || !double.TryParse(uppRow[j]["box_weight"].ToString(), out box_weight))
                                        box_weight = 1;
                                    double tray;
                                    if (uppRow[j]["cost_unit"] == null || !double.TryParse(uppRow[j]["cost_unit"].ToString(), out tray))
                                        tray = 1;
                                    //단위 <-> 트레이
                                    bool isWeight = Convert.ToBoolean(uppRow[j]["weight_calculate"].ToString());
                                    if (!isWeight)
                                        box_weight = tray;
                                    double unit_count;
                                    if (row.Cells["unit_count"].Value == null || !double.TryParse(row.Cells["unit_count"].Value.ToString(), out unit_count))
                                        unit_count = 1;
                                    //원가계산
                                    shipment_cost_price = (Convert.ToDouble(uppRow[j]["unit_price"].ToString()) * exchange_rate) * (1 + custom + tax + incidental_expense) * (box_weight / unit_count);
                                    //동원 + 2.5% Or 2%
                                    if (uppRow[j]["ato_no"].ToString().Contains("dw") || uppRow[j]["ato_no"].ToString().Contains("DW")
                                        || uppRow[j]["ato_no"].ToString().Contains("hs") || uppRow[j]["ato_no"].ToString().Contains("HS")
                                        || uppRow[j]["ato_no"].ToString().Contains("od") || uppRow[j]["ato_no"].ToString().Contains("OD")
                                        || uppRow[j]["ato_no"].ToString().Contains("ad") || uppRow[j]["ato_no"].ToString().Contains("AD"))
                                        shipment_cost_price = shipment_cost_price * 1.025;
                                    else if (uppRow[j]["ato_no"].ToString().Contains("jd") || uppRow[j]["ato_no"].ToString().Contains("JD"))
                                        shipment_cost_price = shipment_cost_price * 1.02;

                                    //연이자 추가
                                    double interest = 0;
                                    DateTime etd;
                                    if (DateTime.TryParse(uppRow[j]["etd"].ToString(), out etd))
                                    {
                                        TimeSpan ts = DateTime.Now - etd;
                                        int days = ts.Days;
                                        interest = shipment_cost_price * 0.08 / 365 * days;
                                        if (interest < 0)
                                            interest = 0;
                                    }
                                    shipment_cost_price += interest;

                                    //ToolTip
                                    tooltipTxt += "\n" + uppRow[j]["ato_no"].ToString() + "  " + shipment_cost_price.ToString("#,##0") + "(" + exchange_rate.ToString("#,##0.00") + ")  + 이자(" + interest.ToString("#,##0") + ")     수량 : " + (shipment_qty + unpending_qty_before).ToString("#,##0");
                                }
                            }
                            row.Cells["sub_pending_cost_price"].ToolTipText = tooltipTxt.Trim();
                        }
                    }
                }
            }
        }
    }
}
