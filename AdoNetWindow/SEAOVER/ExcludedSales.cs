using AdoNetWindow.Model;
using Repositories;
using Repositories.SaleProduct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER
{
    public partial class ExcludedSales : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IProductExcludedSalesRepository productExcludedSalesRepository = new ProductExcludedSalesRepository();
        UsersModel um;
        SEAOVER.PriceComparison.PriceComparison pc = null;
        Libs.Tools.Common common = new Libs.Tools.Common();
        public ExcludedSales(UsersModel uModel, SEAOVER.PriceComparison.PriceComparison pComparison, ProductExcludedSalesModel model, string sttdate, string enddate)
        {
            InitializeComponent();
            um = uModel;

            txtSttdate.Text = sttdate;
            txtEnddate.Text = enddate;

            txtProduct.Text = model.product;
            txtOrigin.Text = model.origin;
            txtSizes.Text = model.sizes;
            txtUnit.Text = model.unit;
            txtPriceUnit.Text = model.price_unit;
            txtUnitCount.Text = model.unit_count;
            txtSeaoverUnit.Text = model.seaover_unit;

            pc = pComparison;
        }
        private void ExcludedSales_Load(object sender, EventArgs e)
        {
            SetHeaderStyleSetting();
        }
        #region Method
        private void SetHeaderStyleSetting()
        {
            DataGridView dgv = dgvProduct;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 94, 170);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        }
        #endregion

        #region Key event
        private void ExcludedSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearching.PerformClick();
                        break;
                    case Keys.N:
                        txtProduct.Text = String.Empty;
                        txtOrigin.Text = String.Empty;
                        txtSizes.Text = String.Empty;
                        txtUnit.Text = String.Empty;
                        txtPriceUnit.Text = String.Empty;
                        txtUnitCount.Text = String.Empty;
                        txtSeaoverUnit.Text = String.Empty;
                        txtProduct.Focus();
                        break;
                    case Keys.M:
                        txtProduct.Focus();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
        }
        private void txtSttdate_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            //숫자만 입력되도록 필터링
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == Convert.ToChar(45) || e.KeyChar == Convert.ToChar(47)))    //숫자와 백스페이스를 제외한 나머지를 바로 처리
            {
                e.Handled = true;
            }
        }

        private void txtSttdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TextBox tbb = (TextBox)sender;
                tbb.Text = common.strDatetime(tbb.Text);
                if (tbb.Name != "txtSttdate" && tbb.Name != "txtEnddate")
                {
                    DateTime dt;
                    if (DateTime.TryParse(tbb.Text, out dt))
                        tbb.Text = dt.ToString("yyyy-MM-dd");
                }
            }
            
        }
        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearching.PerformClick();
        }
        #endregion

        #region Button
        private void btnSearching_Click(object sender, EventArgs e)
        {
            dgvProduct.Rows.Clear();
            DataTable productDt = productExcludedSalesRepository.GetExcludedSales(txtSttdate.Text, txtEnddate.Text, txtProduct.Text, txtOrigin.Text, txtSizes.Text, txtUnit.Text, txtPriceUnit.Text, txtUnitCount.Text, txtSeaoverUnit.Text);
            for (int i = 0; i < productDt.Rows.Count; i++)
            {
                int n = dgvProduct.Rows.Add();
                DataGridViewRow row = dgvProduct.Rows[n];

                row.Cells["id"].Value = productDt.Rows[i]["id"].ToString();
                row.Cells["product"].Value = productDt.Rows[i]["product"].ToString();
                row.Cells["origin"].Value = productDt.Rows[i]["origin"].ToString();
                row.Cells["sizes"].Value = productDt.Rows[i]["sizes"].ToString();
                row.Cells["unit"].Value = productDt.Rows[i]["unit"].ToString();
                row.Cells["price_unit"].Value = productDt.Rows[i]["price_unit"].ToString();
                row.Cells["unit_count"].Value = productDt.Rows[i]["unit_count"].ToString();
                row.Cells["seaover_unit"].Value = productDt.Rows[i]["seaover_unit"].ToString();
                row.Cells["sale_company"].Value = productDt.Rows[i]["sale_company"].ToString();
                row.Cells["sale_date"].Value = Convert.ToDateTime(productDt.Rows[i]["sale_date"].ToString()).ToString("yyyy-MM-dd");
                row.Cells["sale_qty"].Value = Convert.ToDouble(productDt.Rows[i]["sale_qty"].ToString()).ToString("#,##0");
                row.Cells["remark"].Value = productDt.Rows[i]["remark"].ToString();
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnSttDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtSttdate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        private void btnEndDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                txtEnddate.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }

        #endregion

        #region 우클릭 메뉴
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
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        ContextMenuStrip m = new ContextMenuStrip();
                        m.Items.Add("복사등록");
                        m.Items.Add("수정");
                        m.Items.Add("삭제");
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
                        return;
                    
                    //Function
                    StringBuilder sql = new StringBuilder();
                    List<StringBuilder> sqlList = new List<StringBuilder>();
                    ProductExcludedSalesModel model = new ProductExcludedSalesModel();
                    switch (e.ClickedItem.Text)
                    {
                        case "복사등록":
                            { 
                                model.id = commonRepository.GetNextId("t_product_excluded_sales", "id");
                                model.product = dr.Cells["product"].Value.ToString();
                                model.origin = dr.Cells["origin"].Value.ToString();
                                model.sizes = dr.Cells["sizes"].Value.ToString();
                                model.unit = dr.Cells["unit"].Value.ToString();
                                model.price_unit = dr.Cells["price_unit"].Value.ToString();
                                model.unit_count = dr.Cells["unit_count"].Value.ToString();
                                model.seaover_unit = dr.Cells["seaover_unit"].Value.ToString();
                                model.sale_company = dr.Cells["sale_company"].Value.ToString();
                                model.sale_date = dr.Cells["sale_date"].Value.ToString();
                                model.sale_qty = Convert.ToDouble(dr.Cells["sale_qty"].Value.ToString());
                                model.remark = dr.Cells["remark"].Value.ToString();

                                AddExcludedSales aes = new AddExcludedSales(um, null, model);
                                aes.ShowDialog();
                            }
                            break;
                        case "수정":
                            { 
                                model.id = Convert.ToInt32(dr.Cells["id"].Value.ToString());
                                model.product = dr.Cells["product"].Value.ToString();
                                model.origin = dr.Cells["origin"].Value.ToString();
                                model.sizes = dr.Cells["sizes"].Value.ToString();
                                model.unit = dr.Cells["unit"].Value.ToString();
                                model.price_unit = dr.Cells["price_unit"].Value.ToString();
                                model.unit_count = dr.Cells["unit_count"].Value.ToString();
                                model.seaover_unit = dr.Cells["seaover_unit"].Value.ToString();
                                model.sale_company = dr.Cells["sale_company"].Value.ToString();
                                model.sale_date = dr.Cells["sale_date"].Value.ToString();
                                model.sale_qty = Convert.ToDouble(dr.Cells["sale_qty"].Value.ToString());
                                model.remark = dr.Cells["remark"].Value.ToString();

                                AddExcludedSales aes = new AddExcludedSales(um, null, model);
                                aes.ShowDialog();
                            }
                            break;
                        case "삭제":
                            int id = Convert.ToInt32(dr.Cells["id"].Value.ToString());
                            sql = productExcludedSalesRepository.DeleteExcludedSales(id);
                            sqlList.Add(sql);

                            if (commonRepository.UpdateTran(sqlList) == -1)
                            {
                                MessageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                                this.Activate();
                            }
                            else
                                btnSearching.PerformClick();

                            break;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(this,ex.Message);
                    this.Activate();
                }
            }
        }
        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dgvProduct.ClearSelection();
                dgvProduct.Rows[e.RowIndex].Selected = true;
            }
        }
        #endregion

        
    }
}
