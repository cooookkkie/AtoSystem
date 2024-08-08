using AdoNetWindow.Model;
using Microsoft.Office.Interop.Excel;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER.TwoLine
{
    public partial class PageManager : Form
    {
        List<DgvColumnModel> model;
        List<DgvColumnModel> originModel;
        List<SeaoverCopyModel> clipboardModel = new List<SeaoverCopyModel>();
        Dictionary<int, int> copyList = new Dictionary<int, int>();
        int pages, prePages;
        int totalPages;
        string area, preArea;
        bool isEditmode = false;
        _2Line._2LineForm form;
        Libs.MessageBox messageBox = new Libs.MessageBox();
        public PageManager(_2Line._2LineForm _2form, List<DgvColumnModel> list, int pageNum, int totalPageNum, string areas, bool oneForm)
        {
            InitializeComponent();
            model = list;
            originModel = list;
            pages = pageNum;
            txtCurPage.Text = pages.ToString();
            lbTotal.Text = "/ " + totalPageNum.ToString();
            totalPages = totalPageNum;
            area = areas;
            form = _2form;
            /*rbLeft.Checked = true;
            if (oneForm)
            {
                rbLeft.Visible = false;
                rbRight.Visible = false;
                rbAll.Visible = false;
            }*/
        }

        private void PageManager_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            //Double Buffer
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            /*lbPage.Text = pages.ToString();
            lbArea.Text = area.ToString();*/
            SetData();
            setHeaderStyle();
            setTempHeaderStyle();
            /*this.rbLeft.CheckedChanged += new System.EventHandler(this.rbLeft_CheckedChanged);
            this.rbRight.CheckedChanged += new System.EventHandler(this.rbRight_CheckedChanged);
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);*/
        }

        

        #region Event Handler
        private void dgvProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        clipboardModel = new List<SeaoverCopyModel>();
                        if (dgvProduct.SelectedRows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                if (row.Index < dgvProduct.Rows.Count)
                                {
                                    SeaoverCopyModel model = new SeaoverCopyModel();
                                    model.accent = Convert.ToBoolean(row.Cells["accent"].Value);
                                    model.category1 = row.Cells["category1"].Value.ToString();
                                    model.category2 = row.Cells["category2"].Value.ToString();
                                    model.category3 = row.Cells["category3"].Value.ToString();
                                    model.category_code = row.Cells["category_code"].Value.ToString();
                                    model.category = row.Cells["category"].Value.ToString();
                                    model.product_code = row.Cells["product_code"].Value.ToString();
                                    model.product = row.Cells["product"].Value.ToString();
                                    model.origin_code = row.Cells["origin_code"].Value.ToString();
                                    model.origin = row.Cells["origin"].Value.ToString();
                                    model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                                    model.sizes = row.Cells["sizes"].Value.ToString();
                                    model.weight = row.Cells["weight"].Value.ToString();
                                    model.unit = row.Cells["unit"].Value.ToString();
                                    model.price_unit = row.Cells["price_unit"].Value.ToString();
                                    model.unit_count = row.Cells["unit_count"].Value.ToString();
                                    model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                                    model.purchase_price = row.Cells["purchase_price"].Value.ToString();
                                    model.sales_price = row.Cells["sales_price"].Value.ToString();
                                    model.division = row.Cells["division"].Value.ToString();
                                    model.row = row.Cells["rows"].Value.ToString();
                                    model.row_index = row.Cells["row_index"].Value.ToString();
                                    model.cnt = "";

                                    clipboardModel.Add(model);
                                    dgvProduct.Rows.Remove(row);
                                }
                            }
                            countOver();
                        }
                        break;
                    case Keys.C:
                        CopyToClipboard();
                        break;
                    case Keys.V:

                        PasteClipboardRowsValue();
                        countOver();
                        break;
                }
            }
        }

        private void PageManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnReflection.PerformClick();
                        break;
                    case Keys.Q:
                        btnAutoDistribution.PerformClick();
                        break;
                }
            }
        }

        private void dgvProduct_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                double s;

                if (dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "" || dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "0")
                {
                    MessageBox.Show(this, "행수는 1 이상 입력해주시기 바랍니다.");
                    this.Activate();
                    dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                }
                else if (!double.TryParse(dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out s))
                {
                    MessageBox.Show(this, "행수는 숫자형식으로만 입력해주시기 바랍니다.");
                    this.Activate();
                    dgvProduct.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = 1;
                }
                countOver();
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

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("임시 테이블로 이동");
                    m.Items.Add("왼쪽(L) 으로 옮기기");
                    m.Items.Add("오른쪽(R) 으로 옮기기");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.Show(dgvProduct, e.Location);
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
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            dgvProduct.EndEdit();
            switch (e.ClickedItem.Text)
            {
                case "임시 테이블로 이동":
                    if (dgvProduct.SelectedRows.Count > 0)
                    {
                        //담기
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            int n = dgvTemp.Rows.Add();
                            dgvTemp.Rows[n].Cells["taccent"].Value = row.Cells["accent"].Value;
                            dgvTemp.Rows[n].Cells["tcategory1"].Value = row.Cells["category1"].Value;
                            dgvTemp.Rows[n].Cells["tcategory1"].Value = row.Cells["category1"].Value;
                            dgvTemp.Rows[n].Cells["tcategory2"].Value = row.Cells["category2"].Value;
                            dgvTemp.Rows[n].Cells["tcategory3"].Value = row.Cells["category3"].Value;
                            dgvTemp.Rows[n].Cells["tcategory"].Value = row.Cells["category"].Value;
                            dgvTemp.Rows[n].Cells["tcategory_code"].Value = row.Cells["category_code"].Value;
                            dgvTemp.Rows[n].Cells["tproduct_code"].Value = row.Cells["product_code"].Value;
                            dgvTemp.Rows[n].Cells["tproduct"].Value = row.Cells["product"].Value;
                            dgvTemp.Rows[n].Cells["torigin_code"].Value = row.Cells["origin_code"].Value;
                            dgvTemp.Rows[n].Cells["torigin"].Value = row.Cells["origin"].Value;
                            dgvTemp.Rows[n].Cells["tweight"].Value = row.Cells["weight"].Value;
                            dgvTemp.Rows[n].Cells["tsizes_code"].Value = row.Cells["sizes_code"].Value;
                            dgvTemp.Rows[n].Cells["tsizes"].Value = row.Cells["sizes"].Value;
                            dgvTemp.Rows[n].Cells["tpurchase_price"].Value = row.Cells["purchase_price"].Value;
                            dgvTemp.Rows[n].Cells["tsales_price"].Value = row.Cells["sales_price"].Value;
                            dgvTemp.Rows[n].Cells["tdivision"].Value = row.Cells["division"].Value;
                            dgvTemp.Rows[n].Cells["tunit"].Value = row.Cells["unit"].Value;
                            dgvTemp.Rows[n].Cells["tprice_unit"].Value = row.Cells["price_unit"].Value;
                            dgvTemp.Rows[n].Cells["tunit_count"].Value = row.Cells["unit_count"].Value;
                            dgvTemp.Rows[n].Cells["tseaover_unit"].Value = row.Cells["seaover_unit"].Value;
                            dgvTemp.Rows[n].Cells["trow_index"].Value = row.Cells["row_index"].Value;

                        }
                        //삭제
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            dgvProduct.Rows.Remove(dgvProduct.Rows[row.Index]);
                        }

                        countOver();
                    }
                    break;
                case "왼쪽(L) 으로 옮기기":
                    if (dgvProduct.SelectedRows.Count > 0 && rbAll.Checked)
                    {
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            row.Cells["areas"].Value = "L";
                    }
                    break;
                case "오른쪽(R) 으로 옮기기":
                    if (dgvProduct.SelectedRows.Count > 0 && rbAll.Checked)
                    {
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            row.Cells["areas"].Value = "R";
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region 우클릭 메뉴2
        private void dgvTemp_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvTemp.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    m.Items.Add("<- 품목 테이블로 이동");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked2);
                    //Create 
                    m.Show(dgvTemp, e.Location);
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
        void m_ItemClicked2(object sender, ToolStripItemClickedEventArgs e)
        {
            dgvProduct.EndEdit();
            switch (e.ClickedItem.Text)
            {
                case "<- 품목 테이블로 이동":
                    if (dgvTemp.SelectedRows.Count > 0)
                    {
                        string areas = cbArea.Text;
                        //담기
                        foreach (DataGridViewRow row in dgvTemp.SelectedRows)
                        {
                            int n = dgvProduct.Rows.Add();
                            dgvProduct.Rows[n].Cells["accent"].Value = row.Cells["taccent"].Value;
                            dgvProduct.Rows[n].Cells["category1"].Value = row.Cells["tcategory1"].Value;
                            dgvProduct.Rows[n].Cells["category1"].Value = row.Cells["tcategory1"].Value;
                            dgvProduct.Rows[n].Cells["category2"].Value = row.Cells["tcategory2"].Value;
                            dgvProduct.Rows[n].Cells["category3"].Value = row.Cells["tcategory3"].Value;
                            dgvProduct.Rows[n].Cells["category"].Value = row.Cells["tcategory"].Value;
                            dgvProduct.Rows[n].Cells["category_code"].Value = row.Cells["tcategory_code"].Value;
                            dgvProduct.Rows[n].Cells["product_code"].Value = row.Cells["tproduct_code"].Value;
                            dgvProduct.Rows[n].Cells["product"].Value = row.Cells["tproduct"].Value;
                            dgvProduct.Rows[n].Cells["origin_code"].Value = row.Cells["torigin_code"].Value;
                            dgvProduct.Rows[n].Cells["origin"].Value = row.Cells["torigin"].Value;
                            dgvProduct.Rows[n].Cells["weight"].Value = row.Cells["tweight"].Value;
                            dgvProduct.Rows[n].Cells["sizes_code"].Value = row.Cells["tsizes_code"].Value;
                            dgvProduct.Rows[n].Cells["sizes"].Value = row.Cells["tsizes"].Value;
                            dgvProduct.Rows[n].Cells["purchase_price"].Value = row.Cells["tpurchase_price"].Value;
                            dgvProduct.Rows[n].Cells["sales_price"].Value = row.Cells["tsales_price"].Value;
                            dgvProduct.Rows[n].Cells["division"].Value = row.Cells["tdivision"].Value;
                            dgvProduct.Rows[n].Cells["unit"].Value = row.Cells["tunit"].Value;
                            dgvProduct.Rows[n].Cells["price_unit"].Value = row.Cells["tprice_unit"].Value;
                            dgvProduct.Rows[n].Cells["unit_count"].Value = row.Cells["tunit_count"].Value;
                            dgvProduct.Rows[n].Cells["seaover_unit"].Value = row.Cells["tseaover_unit"].Value;
                            dgvProduct.Rows[n].Cells["areas"].Value = areas;
                            dgvProduct.Rows[n].Cells["row_index"].Value = row.Cells["trow_index"].Value;
                            dgvProduct.Rows[n].Cells["rows"].Value = 1;

                            dgvTemp.Rows.Remove(row);
                        }
                        //삭제
                        foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                        {
                            dgvProduct.Rows.Remove(dgvProduct.Rows[row.Index]);
                        }

                        countOver();
                    }
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region Button, Checkbox, radioButton
        private void btnDown_Click(object sender, EventArgs e)
        {
            Reflection();

            int sPage = Convert.ToInt16(txtCurPage.Text);
            string sArea = cbArea.Text;
            if (string.IsNullOrEmpty(sArea))
                sArea = "L";

            this.cbArea.SelectedIndexChanged -= new System.EventHandler(this.cbArea_SelectedIndexChanged);
            //전페이지
            if (sArea == "L")
            {
                cbArea.Text = "R";
                txtCurPage.Text = (sPage - 1).ToString();
            }
            else if (sPage >= 1)
            {
                cbArea.Text = "L";
            }
            this.cbArea.SelectedIndexChanged += new System.EventHandler(this.cbArea_SelectedIndexChanged);

            
            RadioBtnClick();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            Reflection();
            int sPage = Convert.ToInt16(txtCurPage.Text);
            string sArea = cbArea.Text;
            if (string.IsNullOrEmpty(sArea))
                sArea = "L";

            this.cbArea.SelectedIndexChanged -= new System.EventHandler(this.cbArea_SelectedIndexChanged);
            //다음페이지
            if (sArea == "L")
            {
                cbArea.Text = "R";
            }
            else if (sPage < totalPages)
            {
                txtCurPage.Text = (sPage + 1).ToString();
                cbArea.Text = "L";
                
            }
            this.cbArea.SelectedIndexChanged += new System.EventHandler(this.cbArea_SelectedIndexChanged);VPageBreak:    
            RadioBtnClick();
        }

        private void btnAutoDistribution_Click(object sender, EventArgs e)
        {
            AutoDistribution(cbArea.Text);
            countOver();
        }
        private void btnReflection_Click(object sender, EventArgs e)
        {
            Reflection();
            RadioBtnClick();
        }
        private void Reflection()
        {
            if (!countIsOver())
            {
                if (MessageBox.Show(this, "행수가 초과하거나 부족합니다. 자동맞춤 후 반영하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    btnAutoDistribution.PerformClick();
            }
            setRowsCount();
            countIsOver();
        }
        private void btnSort_Click(object sender, EventArgs e)
        {
            SortSetting();
        }
        #endregion

        #region Method

        private void RadioBtnClick()
        {
            string sArea = cbArea.Text;
            PageMananerOpen(pages, sArea);
            form.formPageChange(pages);
        }
        private bool AutoDistribution(string areas)
        {
            if (dgvProduct.Rows.Count == 0)
            {
                messageBox.Show(this, "수정할 행이 없습니다.");
                return false;
            }

            int standard_rows = 60;
            int rows = 0;
            foreach (DataGridViewRow dgvr in dgvProduct.Rows)
            {
                if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas))
                {
                    if (dgvr.Cells["rows"].Value == null || !int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row))
                        row = 0;
                    rows += row;
                }
            }
            if (rows == 0)
            {
                messageBox.Show(this, "수정할 행이 없습니다.");
                return false;
            }
            //조정할 행수
            int adjust_rows = standard_rows - rows;
            if (adjust_rows == 0)
            {
                messageBox.Show(this, "수정할 행이 없습니다.");
                return false;
            }
            else if (adjust_rows < 0 && dgvProduct.Rows.Count >= 60)
            {
                messageBox.Show(this, "더 이상 줄일 수 없습니다. 품목을 제거하거나 다음페이지로 옮겨주세요.");
                return false;
            }

            //늘려야 할때
            if (adjust_rows > 0)
            {
            retry1:
                int min_row = 9999, max_row = 0;
                foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                {
                    if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas))
                    {
                        if (dgvr.Cells["rows"].Value == null || !int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row))
                            row = 0;
                        if (min_row > row)
                            min_row = row;
                        if (max_row < row)
                            max_row = row;
                    }
                }

                if (adjust_rows != 0 && min_row < max_row)
                {
                    while (min_row < max_row)
                    {
                        foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                        {
                            if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas))
                            {
                                if (dgvr.Cells["rows"].Value == null || !int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row))
                                    row = 0;
                                if (min_row == row)
                                {
                                    dgvr.Cells["rows"].Value = min_row + 1;
                                    adjust_rows--;

                                    if (adjust_rows == 0)
                                        return true;
                                }
                            }
                        }
                        min_row++;
                    }
                    goto retry1;
                }
                else if (adjust_rows != 0)
                {
                    //사이즈
                    int min_sizes_len = 9999;
                    foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                    {
                        if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas)
                            && dgvr.Cells["sizes"].Value != null && int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row) && row == min_row)
                        {
                            int len = dgvr.Cells["sizes"].Value.ToString().Length;

                            if (min_sizes_len > len)
                                min_sizes_len = len;
                        }
                    }

                    foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                    {
                        if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas)
                            && dgvr.Cells["sizes"].Value != null && int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row) && row == min_row)
                        {
                            int len = dgvr.Cells["sizes"].Value.ToString().Length;

                            if (min_sizes_len == len)
                            {
                                dgvr.Cells["rows"].Value = row + 1;
                                adjust_rows--;

                                if (adjust_rows == 0)
                                    return true;
                            }
                        }
                    }
                    //품목
                    if (adjust_rows != 0)
                    {
                        min_sizes_len = 9999;
                        foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                        {
                            if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas)
                                && dgvr.Cells["product"].Value != null && int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row) && row == min_row)
                            {
                                int len = dgvr.Cells["product"].Value.ToString().Length;

                                if (min_sizes_len > len)
                                    min_sizes_len = len;
                            }
                        }

                        foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                        {
                            if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas)
                                && dgvr.Cells["product"].Value != null && int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row) && row == min_row)
                            {
                                int len = dgvr.Cells["product"].Value.ToString().Length;

                                if (min_sizes_len == len)
                                {
                                    dgvr.Cells["rows"].Value = row + 1;
                                    adjust_rows--;

                                    if (adjust_rows == 0)
                                        return true;
                                }
                            }
                        }
                        goto retry1;
                    }
                }
            }
            //줄여야할때
            else
            {
            retry2:
                int min_row = 9999, max_row = 0;
                foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                {
                    if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas))
                    {
                        if (dgvr.Cells["rows"].Value == null || !int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row))
                            row = 0;
                        if (min_row > row)
                            min_row = row;
                        if (max_row < row)
                            max_row = row;
                    }
                }

                if (adjust_rows != 0 && min_row < max_row)
                {
                    while (min_row < max_row)
                    {
                        foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                        {
                            if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas))
                            {
                                if (dgvr.Cells["rows"].Value == null || !int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row))
                                    row = 0;
                                if (max_row == row)
                                {
                                    dgvr.Cells["rows"].Value = row - 1;
                                    adjust_rows++;

                                    if (adjust_rows == 0)
                                        return true;
                                }
                            }
                        }
                        max_row--;
                    }
                    goto retry2;
                }
                else if (adjust_rows != 0 && min_row == max_row && min_row > 1)
                {
                    //사이즈
                    int min_sizes_len = 9999;
                    foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                    {
                        if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas)
                            && dgvr.Cells["sizes"].Value != null && int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row) && row > min_row)
                        {
                            int len = dgvr.Cells["sizes"].Value.ToString().Length;

                            if (min_sizes_len > len)
                                min_sizes_len = len;
                        }
                    }

                    foreach (DataGridViewRow dgvr in dgvProduct.Rows)
                    {
                        if (dgvr.Cells["areas"].Value != null && dgvr.Cells["areas"].Value.ToString().Equals(areas)
                            && dgvr.Cells["sizes"].Value != null && int.TryParse(dgvr.Cells["rows"].Value.ToString(), out int row) && row > min_row)
                        {
                            int len = dgvr.Cells["sizes"].Value.ToString().Length;

                            if (min_sizes_len == len)
                            {
                                dgvProduct.Rows[0].Cells["rows"].Value = row - 1;
                                adjust_rows++;

                                if (adjust_rows == 0)
                                    return true;
                                goto retry2;
                            }
                        }
                    }
                }
                else if (adjust_rows != 0 && min_row == max_row && min_row == 1)
                {
                    messageBox.Show(this, "더 이상 줄일 수 없습니다. 품목을 제거하거나 다음페이지로 옮겨주세요.");
                    return true;
                }
            }
            return true;
        }
        //셀 정렬
        private void SortSetting()
        {
            if (!string.IsNullOrEmpty(cbSort.Text))
            {
                string[] sortStrs = cbSort.Text.Split('+');
                string sortStr = "";
                if (sortStrs.Length > 1)
                {
                    for (int i = 0; i < sortStrs.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(sortStr))
                        {
                            sortStr += ", " + SetsortStr(sortStrs[i]);
                        }
                        else
                        {
                            sortStr = SetsortStr(sortStrs[i]);
                        }
                    }
                }

                //사이즈 정렬 컬럼추가
                System.Data.DataTable tb = Libs.Tools.Common.GetDataGridViewAsDataTable2(dgvProduct);
                tb.Columns.Add("product_sort", typeof(double)).SetOrdinal(1);
                tb.Columns.Add("sizes_sort", typeof(double)).SetOrdinal(1);
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    string tmpStr = "";
                    double res;
                    string str = tb.Rows[i]["product"].ToString();
                    if (str.Contains("F"))
                    {
                        tb.Rows[i]["product_sort"] = 1;
                    }
                    else if (str.Contains("M"))
                    {
                        tb.Rows[i]["product_sort"] = 2;
                    }


                    str = tb.Rows[i]["sizes"].ToString();

                    if (str.Contains("/"))
                    {
                        tmpStr = str.Substring(0, str.IndexOf("/"));
                        if (double.TryParse(tmpStr, out res))
                        {
                            tb.Rows[i]["sizes_sort"] = res;
                        }
                        else
                        {
                            tb.Rows[i]["sizes_sort"] = 99999;
                        }
                    }
                    else if (str.Contains("/") && str.Contains("M"))
                    {
                        tmpStr = str.Substring(0, str.IndexOf("/"));

                        if (double.TryParse(tmpStr, out res))
                        {
                            tb.Rows[i]["sizes_sort"] = Convert.ToDouble("2." + res.ToString());
                        }
                        else
                        {
                            tmpStr = tmpStr.Replace("M", "");
                            if (double.TryParse(tmpStr, out res))
                            {
                                tb.Rows[i]["sizes_sort"] = Convert.ToDouble("2." + res.ToString());
                            }
                        }
                    }
                    else if (str.Contains("/") && str.Contains("F"))
                    {
                        tmpStr = str.Substring(0, str.IndexOf("/"));

                        if (double.TryParse(tmpStr, out res))
                        {
                            tb.Rows[i]["sizes_sort"] = Convert.ToDouble("1." + res.ToString());
                        }
                        else
                        {
                            tmpStr = tmpStr.Replace("F", "");
                            if (double.TryParse(tmpStr, out res))
                            {
                                tb.Rows[i]["sizes_sort"] = Convert.ToDouble("1." + res.ToString());
                            }
                        }
                    }
                    else if (str.Contains("미"))
                    {
                        tmpStr = str.Substring(0, str.IndexOf("미"));
                        if (double.TryParse(tmpStr, out res))
                        {
                            tb.Rows[i]["sizes_sort"] = res;
                        }
                        else
                        {
                            tb.Rows[i]["sizes_sort"] = 99999;
                        }
                    }
                    else if (str.Contains("통"))
                    {
                        tmpStr = str.Substring(0, str.IndexOf("통"));
                        if (double.TryParse(tmpStr, out res))
                        {
                            tb.Rows[i]["sizes_sort"] = res;
                        }
                        else
                        {
                            tb.Rows[i]["sizes_sort"] = 99999;
                        }
                    }
                    else if (str.Contains("손"))
                    {
                        tmpStr = str.Substring(0, str.IndexOf("손"));
                        if (double.TryParse(tmpStr, out res))
                        {
                            tb.Rows[i]["sizes_sort"] = res;
                        }
                        else
                        {
                            tb.Rows[i]["sizes_sort"] = 99999;
                        }
                    }
                    else if (str.Contains("LH"))
                    {
                        tmpStr = str.Substring(0, str.IndexOf("LH"));
                        if (double.TryParse(tmpStr, out res))
                        {
                            tb.Rows[i]["sizes_sort"] = res;
                        }
                        else
                        {
                            tb.Rows[i]["sizes_sort"] = 99999;
                        }
                    }
                    else if (str.Contains("KG"))
                    {
                        tmpStr = str.Substring(0, str.IndexOf("KG"));
                        if (double.TryParse(tmpStr, out res))
                        {
                            tb.Rows[i]["sizes_sort"] = res;
                        }
                        else
                        {
                            tb.Rows[i]["sizes_sort"] = 99999;
                        }
                    }
                    else if (str.Contains("대"))
                    {
                        tb.Rows[i]["sizes_sort"] = 1;
                    }
                    else if (str.Contains("중"))
                    {
                        tb.Rows[i]["sizes_sort"] = 2;
                    }
                    else if (str.Contains("소"))
                    {
                        tb.Rows[i]["sizes_sort"] = 3;
                    }
                    else if (str == "L")
                    {
                        tb.Rows[i]["sizes_sort"] = 1;
                    }
                    else if (str == "2L" || str == "L2")
                    {
                        tb.Rows[i]["sizes_sort"] = 1.1;
                    }
                    else if (str == "3L" || str == "L3")
                    {
                        tb.Rows[i]["sizes_sort"] = 1.2;
                    }
                    else if (str.Contains("L"))
                    {
                        tb.Rows[i]["sizes_sort"] = 1.3;
                    }
                    else if (str == "M")
                    {
                        tb.Rows[i]["sizes_sort"] = 2;
                    }
                    else if (str == "2M" || str == "M2")
                    {
                        tb.Rows[i]["sizes_sort"] = 2.1;
                    }
                    else if (str == "3M" || str == "M3")
                    {
                        tb.Rows[i]["sizes_sort"] = 2.2;
                    }
                    else if (str.Contains("M"))
                    {
                        tb.Rows[i]["sizes_sort"] = 2.3;
                    }
                    else if (str == "S")
                    {
                        tb.Rows[i]["sizes_sort"] = 3;
                    }
                    else if (str == "2S" || str == "S2")
                    {
                        tb.Rows[i]["sizes_sort"] = 3.1;
                    }
                    else if (str == "3S" || str == "S3")
                    {
                        tb.Rows[i]["sizes_sort"] = 3.2;
                    }
                    else if (str.Contains("S"))
                    {
                        tb.Rows[i]["sizes_sort"] = 3.3;
                    }
                    else
                    {
                        tb.Rows[i]["sizes_sort"] = 99999;
                    }

                }
                
                DataView tv = new DataView(tb);

                tv.Sort = sortStr + ", product_sort, product, sizes_sort, sizes";
                tb = tv.ToTable();

                //재출력
                dgvProduct.Rows.Clear();
                for (int i = 0; i < tb.Rows.Count; i++)
                {
                    //Row 추가
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["accent"].Value = Convert.ToBoolean(tb.Rows[i]["accent"]);
                    dgvProduct.Rows[n].Cells["category1"].Value = tb.Rows[i]["category1"].ToString();
                    dgvProduct.Rows[n].Cells["category2"].Value = tb.Rows[i]["category2"].ToString();
                    dgvProduct.Rows[n].Cells["category3"].Value = tb.Rows[i]["category3"].ToString();
                    dgvProduct.Rows[n].Cells["category"].Value = tb.Rows[i]["category"].ToString();
                    dgvProduct.Rows[n].Cells["category_code"].Value = tb.Rows[i]["category_code"].ToString();
                    dgvProduct.Rows[n].Cells["product_code"].Value = tb.Rows[i]["product_code"].ToString();
                    dgvProduct.Rows[n].Cells["product"].Value = tb.Rows[i]["product"].ToString();
                    dgvProduct.Rows[n].Cells["origin_code"].Value = tb.Rows[i]["origin_code"].ToString();
                    dgvProduct.Rows[n].Cells["origin"].Value = tb.Rows[i]["origin"].ToString();
                    dgvProduct.Rows[n].Cells["weight"].Value = tb.Rows[i]["weight"].ToString();
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = tb.Rows[i]["sizes_code"].ToString();
                    dgvProduct.Rows[n].Cells["sizes"].Value = tb.Rows[i]["sizes"].ToString();
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = tb.Rows[i]["purchase_price"].ToString();
                    dgvProduct.Rows[n].Cells["sales_price"].Value = tb.Rows[i]["sales_price"].ToString();
                    dgvProduct.Rows[n].Cells["unit"].Value = tb.Rows[i]["unit"].ToString();
                    dgvProduct.Rows[n].Cells["price_unit"].Value = tb.Rows[i]["price_unit"].ToString();
                    dgvProduct.Rows[n].Cells["unit_count"].Value = tb.Rows[i]["unit_count"].ToString();
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = tb.Rows[i]["seaover_unit"].ToString();
                    dgvProduct.Rows[n].Cells["division"].Value = tb.Rows[i]["division"].ToString();
                    //dgvProduct.Rows[n].Cells["page"].Value = tb.Rows[i]["page"].ToString();
                    //dgvProduct.Rows[n].Cells["cnt"].Value = tb.Rows[i]["cnt"].ToString();
                    dgvProduct.Rows[n].Cells["rows"].Value = tb.Rows[i]["rows"].ToString();
                    dgvProduct.Rows[n].Cells["areas"].Value = tb.Rows[i]["areas"].ToString();
                    dgvProduct.Rows[n].Cells["row_index"].Value = tb.Rows[i]["row_index"].ToString();
                    //영억
                    /*DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgvProduct.Rows[n].Cells["area"];
                    cb.Items.Add("L");
                    cb.Items.Add("R");
                    cb.Items.Add("C");
                    dgvProduct.Rows[n].Cells["area"].Value = tb.Rows[i]["area"].ToString();*/
                }
            }
        }
        private string SetsortStr(string str)
        {
            string results = "";
            if (str == "대분류")
            {
                results = "category1, category2, category3";
            }
            else if (str == "품명")
            {
                results = "product_sort, product";
            }
            else if (str == "원산지")
            {
                results = "origin";
            }
            else if (str == "중량")
            {
                results = "weight";
            }
            else if (str == "사이즈")
            {
                results = "sizes_sort, sizes";
            }

            return results;
        }


        //페이지 왼쪽, 오른쪽 설정창 오픈
        private void PageMananerOpen(int page, string areas)
        {
            area = areas;
            pages = page;

            List<DgvColumnModel> list = new List<DgvColumnModel>();
            list = form.PageMananerList(page, area);
            originModel = list;
            model = list;
            SetData();
        }
        private void setTempHeaderStyle()
        {
            //헤더 디자인
            this.dgvTemp.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            this.dgvTemp.ColumnHeadersDefaultCellStyle.BackColor = Color.RosyBrown;
            this.dgvTemp.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            this.dgvTemp.AllowUserToAddRows = false;
            this.dgvTemp.AllowUserToDeleteRows = false;

            /*this.dgvProduct.Columns["row_index"].Visible = false;
            this.dgvProduct.Columns["row_count"].Visible = false;*/

            this.dgvTemp.ReadOnly = true;
            this.dgvTemp.Columns["taccent"].Visible = false;
            this.dgvTemp.Columns["tcategory_code"].Visible = false;
            this.dgvTemp.Columns["tproduct_code"].Visible = false;
            this.dgvTemp.Columns["torigin_code"].Visible = false;
            this.dgvTemp.Columns["tsizes_code"].Visible = false;
            this.dgvTemp.Columns["tunit"].Visible = false;
            this.dgvTemp.Columns["tprice_unit"].Visible = false;
            this.dgvTemp.Columns["tunit_count"].Visible = false;
            this.dgvTemp.Columns["tseaover_unit"].Visible = false;
            this.dgvTemp.Columns["tcategory1"].Visible = false;
            this.dgvTemp.Columns["tcategory2"].Visible = false;
            this.dgvTemp.Columns["tcategory3"].Visible = false;
            this.dgvTemp.Columns["tpurchase_price"].Visible = false;
            this.dgvTemp.Columns["trow_index"].Visible = false;
        }
        private void setHeaderStyle()
        {
            //헤더 디자인
            this.dgvProduct.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            this.dgvProduct.ColumnHeadersDefaultCellStyle.BackColor = Color.RosyBrown;
            this.dgvProduct.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            
            /*this.dgvProduct.Columns["row_index"].Visible = false;
            this.dgvProduct.Columns["row_count"].Visible = false;*/

            this.dgvProduct.Columns[0].ReadOnly = true;
            this.dgvProduct.Columns[1].ReadOnly = true;
            this.dgvProduct.Columns[2].ReadOnly = true;
            this.dgvProduct.Columns[3].ReadOnly = true;
            this.dgvProduct.Columns[4].ReadOnly = true;
            this.dgvProduct.Columns[5].ReadOnly = true;
            this.dgvProduct.Columns[6].ReadOnly = true;
            this.dgvProduct.Columns[7].ReadOnly = true;
            this.dgvProduct.Columns[8].ReadOnly = true;
            this.dgvProduct.Columns["rows"].ReadOnly = false;

            this.dgvProduct.Columns["accent"].Visible = false;
            this.dgvProduct.Columns["category_code"].Visible = false;
            this.dgvProduct.Columns["product_code"].Visible = false;
            this.dgvProduct.Columns["origin_code"].Visible = false;
            this.dgvProduct.Columns["sizes_code"].Visible = false;
            this.dgvProduct.Columns["unit"].Visible = false;
            this.dgvProduct.Columns["price_unit"].Visible = false;
            this.dgvProduct.Columns["unit_count"].Visible = false;
            this.dgvProduct.Columns["seaover_unit"].Visible = false;
            this.dgvProduct.Columns["category1"].Visible = false;
            this.dgvProduct.Columns["category2"].Visible = false;
            this.dgvProduct.Columns["category3"].Visible = false;
            this.dgvProduct.Columns["purchase_price"].Visible = false;
            this.dgvProduct.Columns["row_index"].Visible = false;
            this.dgvProduct.Columns["row_count"].Visible = false;


            this.dgvProduct.Columns["rows"].HeaderCell.Style.BackColor = Color.Red;
            this.dgvProduct.Columns["rows"].HeaderCell.Style.ForeColor = Color.Yellow;
            this.dgvProduct.Columns["rows"].DefaultCellStyle.Font = new System.Drawing.Font("나눔고딕", 10, FontStyle.Bold);
            this.dgvProduct.Columns["rows"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
        }
        public void SetData()
        {
            //lbInfo.Text = "PAGE : " + pages + " / " + "AREA : " + area;
            dgvProduct.Rows.Clear();
            //저장값 불러오기
            if (model.Count > 0)
            {
                int cnt = 1;
                for (int i = 0; i < model.Count; i++)
                {
                    DgvColumnModel m = model[i];

                    //Row 추가
                    int n = dgvProduct.Rows.Add();


                    dgvProduct.Rows[n].Cells["accent"].Value = m.accent;
                    dgvProduct.Rows[n].Cells["category1"].Value = m.category1;
                    dgvProduct.Rows[n].Cells["category2"].Value = m.category2;
                    dgvProduct.Rows[n].Cells["category3"].Value = m.category3;
                    dgvProduct.Rows[n].Cells["category"].Value = m.category;
                    dgvProduct.Rows[n].Cells["category_code"].Value = m.category_code;
                    dgvProduct.Rows[n].Cells["product"].Value = m.product;
                    dgvProduct.Rows[n].Cells["product_code"].Value = m.product_code;
                    dgvProduct.Rows[n].Cells["origin"].Value = m.origin;
                    dgvProduct.Rows[n].Cells["origin_code"].Value = m.origin_code;
                    dgvProduct.Rows[n].Cells["weight"].Value = m.weight;
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = m.sizes_code;
                    dgvProduct.Rows[n].Cells["sizes"].Value = m.sizes;
                    dgvProduct.Rows[n].Cells["purchase_price"].Value = m.purchase_price.ToString();
                    dgvProduct.Rows[n].Cells["sales_price"].Value = m.sales_price.ToString();
                    dgvProduct.Rows[n].Cells["division"].Value = m.division;
                    dgvProduct.Rows[n].Cells["unit"].Value = m.unit;
                    dgvProduct.Rows[n].Cells["price_unit"].Value = m.price_unit;
                    dgvProduct.Rows[n].Cells["unit_count"].Value = m.unit_count;
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = m.seaover_unit;
                    //dgvProduct.Rows[n].Cells["cnt"].Value = m.cnt;
                    dgvProduct.Rows[n].Cells["rows"].Value = m.rows;
                    dgvProduct.Rows[n].Cells["areas"].Value = m.area;
                    dgvProduct.Rows[n].Cells["row_index"].Value = m.row_index;
                    dgvProduct.Rows[n].Cells["row_count"].Value = m.cnt;

                    preArea = m.area;
                    prePages = m.page;
                }
            }
            
            countOver();
        }
        private bool countIsOver()
        {
            int max_row;
            if (area == "L" || area == "R" || area == "C")
            {
                max_row = 60;
            }
            else
            {
                max_row = 120;
            }
            int cnt = 0;
            int row;
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    row = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                    cnt += row;
                }
            }
            //출력
            if (cnt > max_row)
            {
                return false;
            }
            else if (cnt < max_row)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void countOver()
        {
            int max_row;
            if (area == "L" || area == "R" || area == "C")
            {
                max_row = 60;
            }
            else
            {
                max_row = 120;
            }
            int cnt = 0;
            int row;
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    row = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                    cnt += row;
                }   
            }
            //출력
            if (cnt > max_row)
            {
                lbOverCount.Text = "▲초과한 행수 : " + (cnt - max_row);
                lbOverCount.ForeColor = Color.Red;
            }
            else if (cnt < max_row)
            {
                lbOverCount.Text = "▼부족한 행수 : " + (max_row - cnt);
                lbOverCount.ForeColor = Color.Red;
            }
            else
            {
                lbOverCount.Text = "* 적절함 *";
                lbOverCount.ForeColor = Color.Blue;
            }
        }

        private int maxNum()
        {
            int maxNum = 0;
            int row;
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    row = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                    if (maxNum < row)
                    {
                        maxNum = row;
                    }
                }
            }

            return maxNum;
        }

        private int minNum()
        {
            int minNum = 1;
            int row;
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 0)
            {
                minNum = Convert.ToInt16(dgv.Rows[0].Cells["rows"].Value);
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    row = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                    if (minNum >= row)
                    {
                        minNum = row;
                    }
                }
            }

            return minNum;
        }

        private int DiffCount()
        {
            int max_row;
            if (area == "L" || area == "R" || area == "C")
            {
                max_row = 60;
            }
            else
            {
                max_row = 120;
            }
            int diff;
            int row;
            int cnt = 0;
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 0)
            {
                dgv.EndEdit();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    row = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                    cnt += row;
                }
            }

            diff = cnt - max_row;

            return diff;
        }

        private void AutoCountUpdate()
        {
            int max_row;
            if (area == "L" || area == "R" || area == "C")
            {
                max_row = 60;
            }
            else
            {
                max_row = 120;
            }
            int cnt = 0;
            int row;
            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 0)
            {
                if (dgv.Rows.Count > 60)
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        dgv.Rows[i].Cells["rows"].Value = 1;
                    }
                }
                else 
                {
                //출력
                retry1:
                    int diff = DiffCount();
                    int mNum;
                    if (diff > 0)
                    {
                        mNum = maxNum();
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            row = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                            if (row == mNum)
                            {
                                dgv.Rows[i].Cells["rows"].Value = row - 1;
                                diff -= 1;
                            }
                            if (diff == 0)
                            {
                                break;
                            }
                        }

                        goto retry1;
                    }
                    else if (diff < 0)
                    {
                        mNum = minNum();
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            row = Convert.ToInt16(dgv.Rows[i].Cells["rows"].Value);
                            if (row == mNum)
                            {
                                dgv.Rows[i].Cells["rows"].Value = row + 1;
                                diff += 1;
                            }
                            if (diff == max_row)
                            {
                                break;
                            }
                        }

                        goto retry1;
                    }
                }
            }
            countOver();
        }


        private void setRowsCount()
        {
            DataGridView dgv = dgvProduct;
            //새로 List에 담기
            List<DgvColumnModel> model = new List<DgvColumnModel>();
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv.Rows[i].Cells["category"].Value != null)
                    {
                        DgvColumnModel m = new DgvColumnModel();
                        m.accent = Convert.ToBoolean(dgv.Rows[i].Cells["accent"].Value);
                        m.category1 = dgv.Rows[i].Cells["category1"].Value.ToString();
                        m.category2 = dgv.Rows[i].Cells["category2"].Value.ToString();
                        m.category3 = dgv.Rows[i].Cells["category3"].Value.ToString();
                        m.category_code = dgv.Rows[i].Cells["category_code"].Value.ToString();
                        m.category = dgv.Rows[i].Cells["category"].Value.ToString();
                        m.product_code = dgv.Rows[i].Cells["product_code"].Value.ToString();
                        m.product = dgv.Rows[i].Cells["product"].Value.ToString();
                        m.weight = dgv.Rows[i].Cells["weight"].Value.ToString();
                        m.sizes_code = dgv.Rows[i].Cells["sizes_code"].Value.ToString();
                        m.sizes = dgv.Rows[i].Cells["sizes"].Value.ToString();
                        m.origin_code = dgv.Rows[i].Cells["origin_code"].Value.ToString();
                        m.origin = dgv.Rows[i].Cells["origin"].Value.ToString();
                        m.sales_price = dgv.Rows[i].Cells["sales_price"].Value.ToString();
                        m.purchase_price = dgv.Rows[i].Cells["purchase_price"].Value.ToString();
                        m.unit = dgv.Rows[i].Cells["unit"].Value.ToString();
                        m.price_unit = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                        m.unit_count = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                        m.seaover_unit = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                        m.division = dgv.Rows[i].Cells["division"].Value.ToString();
                        //m.cnt = Convert.ToInt32(dgv.Rows[i].Cells["cnt"].Value.ToString());
                        m.rows= Convert.ToInt32(dgv.Rows[i].Cells["rows"].Value.ToString());
                        m.row_index = Convert.ToInt32(dgv.Rows[i].Cells["row_index"].Value.ToString());
                        m.area = dgv.Rows[i].Cells["areas"].Value.ToString();
                        m.page = prePages;

                        model.Add(m);
                    }
                }
            }
            //반영하기
            form.setRowsCountList(model, originModel, prePages, preArea);
            //새로고침
            //form.SettingForm();
        }
        #endregion

        #region 복사, 붙혀넣기
        //복사 
        //  1.DataGridView에서 선택한 영역 Dataobj 에 담기(GetClipboaredContent)
        //  2.Clipboard에 저장(Clipboard.SetDataObject(Dataobj))
        private void CopyToClipboard()
        {
            //Copy to clipboard

            if (dgvProduct.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell c in dgvProduct.SelectedCells)
                {
                    if (c.Value != null)
                    {
                        c.Value = c.Value.ToString().Replace("\n", @"\n");
                    }
                }
            }

            DataObject dataObj = dgvProduct.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        //붙혀넣기
        //  1.Dictionay에 Clipboard Value를 저장 Dic(int, Dic( int, string ) )
        //  2.Dictionay 반복하면서 (끝행은 무시) 현재 선택된 Cell에 출력
        private void PasteClipboardValue()
        {
            //Show Error if no cell is selected
            if (dgvProduct.SelectedCells.Count == 0)
            {
                MessageBox.Show(this, "Please select a cell", "Paste", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataGridViewCell startCell = GetStartCell(dgvProduct);
            //Get the clipboard value in a dictionary
            Dictionary<int, Dictionary<int, string>> cbValue = ClipBoardValues(Clipboard.GetText());
            int iRowIndex = startCell.RowIndex;
            foreach (int rowKey in cbValue.Keys)
            {
                int iColIndex = startCell.ColumnIndex;

                if (cbValue.Keys.Count == 1)
                {
                    foreach (int cellKey in cbValue[rowKey].Keys)
                    {
                        if (iColIndex <= dgvProduct.Columns.Count - 1 && iRowIndex <= dgvProduct.Rows.Count - 1)
                        {
                            foreach (DataGridViewCell c in dgvProduct.SelectedCells)
                            {
                                c.Value = cbValue[0][0];
                            }
                        }
                        iColIndex++;
                    }
                    iRowIndex++;
                }
                else
                {
                    foreach (int cellKey in cbValue[rowKey].Keys)
                    {
                        //Check if the index is with in the limit
                        if (iColIndex <= dgvProduct.Columns.Count - 1 && iRowIndex <= dgvProduct.Rows.Count - 1)
                        {
                            DataGridViewCell cell = dgvProduct[iColIndex, iRowIndex];
                            //Copy to selected cells if 'chkPasteToSelectedCells' is checked
                            /*if ((chkPasteToSelectedCells.Checked && cell.Selected) ||
                            (!chkPasteToSelectedCells.Checked))*/
                            cell.Value = cbValue[rowKey][cellKey];
                        }
                        iColIndex++;
                    }
                    iRowIndex++;
                }

            }
        }
        //붙혀넣기(ctrl + X , ctrl + V)
        private void PasteClipboardRowsValue()
        {
            int row;
            if (dgvProduct.SelectedRows.Count == 0)
                row = dgvProduct.Rows.Count;
            else
                row = dgvProduct.SelectedRows[0].Index;
            string areas = cbArea.Text;
            if (clipboardModel.Count > 0)
            {
                //Row 추가
                dgvProduct.Rows.Insert(row, clipboardModel.Count);

                for (int i = clipboardModel.Count - 1; i >= 0; i--)
                {
                    dgvProduct.Rows[row].Cells["accent"].Value = Convert.ToBoolean(clipboardModel[i].accent);
                    dgvProduct.Rows[row].Cells["category1"].Value = clipboardModel[i].category1;
                    dgvProduct.Rows[row].Cells["category1"].Value = clipboardModel[i].category1;
                    dgvProduct.Rows[row].Cells["category2"].Value = clipboardModel[i].category2;
                    dgvProduct.Rows[row].Cells["category3"].Value = clipboardModel[i].category3;
                    dgvProduct.Rows[row].Cells["category_code"].Value = clipboardModel[i].category_code;
                    dgvProduct.Rows[row].Cells["category"].Value = clipboardModel[i].category;
                    dgvProduct.Rows[row].Cells["product_code"].Value = clipboardModel[i].product_code;
                    dgvProduct.Rows[row].Cells["product"].Value = clipboardModel[i].product;
                    dgvProduct.Rows[row].Cells["origin_code"].Value = clipboardModel[i].origin_code;
                    dgvProduct.Rows[row].Cells["origin"].Value = clipboardModel[i].origin;
                    dgvProduct.Rows[row].Cells["weight"].Value = clipboardModel[i].weight;
                    dgvProduct.Rows[row].Cells["sizes_code"].Value = clipboardModel[i].sizes_code;
                    dgvProduct.Rows[row].Cells["sizes"].Value = clipboardModel[i].sizes;
                    dgvProduct.Rows[row].Cells["unit"].Value = clipboardModel[i].unit;
                    dgvProduct.Rows[row].Cells["price_unit"].Value = clipboardModel[i].price_unit;
                    dgvProduct.Rows[row].Cells["unit_count"].Value = clipboardModel[i].unit_count;
                    dgvProduct.Rows[row].Cells["seaover_unit"].Value = clipboardModel[i].seaover_unit;
                    dgvProduct.Rows[row].Cells["purchase_price"].Value = clipboardModel[i].purchase_price.ToString();
                    dgvProduct.Rows[row].Cells["sales_price"].Value = clipboardModel[i].sales_price.ToString();
                    dgvProduct.Rows[row].Cells["division"].Value = clipboardModel[i].division;
                    dgvProduct.Rows[row].Cells["rows"].Value = clipboardModel[i].row;
                    dgvProduct.Rows[row].Cells["areas"].Value = areas;
                    dgvProduct.Rows[row].Cells["row_index"].Value = clipboardModel[i].row_index;
                    row += 1;
                }
                clipboardModel = new List<SeaoverCopyModel>();
            }
        }
        private void PasteTempClipboardRowsValue()
        {
            if (clipboardModel.Count > 0)
            {
                int row = 0;
                if (dgvTemp.SelectedRows.Count > 0)
                { 
                    row = dgvTemp.SelectedRows[0].Index;
                }

                //Row 추가
                dgvTemp.Rows.Insert(row, clipboardModel.Count);

                for (int i = clipboardModel.Count - 1; i >= 0; i--)
                {
                    dgvTemp.Rows[row].Cells["taccent"].Value = clipboardModel[i].accent;
                    dgvTemp.Rows[row].Cells["tcategory1"].Value = clipboardModel[i].category1;
                    dgvTemp.Rows[row].Cells["tcategory2"].Value = clipboardModel[i].category2;
                    dgvTemp.Rows[row].Cells["tcategory3"].Value = clipboardModel[i].category3;
                    dgvTemp.Rows[row].Cells["tcategory_code"].Value = clipboardModel[i].category_code;
                    dgvTemp.Rows[row].Cells["tcategory"].Value = clipboardModel[i].category;
                    dgvTemp.Rows[row].Cells["tproduct_code"].Value = clipboardModel[i].product_code;
                    dgvTemp.Rows[row].Cells["tproduct"].Value = clipboardModel[i].product;
                    dgvTemp.Rows[row].Cells["torigin_code"].Value = clipboardModel[i].origin_code;
                    dgvTemp.Rows[row].Cells["torigin"].Value = clipboardModel[i].origin;
                    dgvTemp.Rows[row].Cells["tweight"].Value = clipboardModel[i].weight;
                    dgvTemp.Rows[row].Cells["tsizes_code"].Value = clipboardModel[i].sizes_code;
                    dgvTemp.Rows[row].Cells["tsizes"].Value = clipboardModel[i].sizes;
                    dgvTemp.Rows[row].Cells["tunit"].Value = clipboardModel[i].unit;
                    dgvTemp.Rows[row].Cells["tprice_unit"].Value = clipboardModel[i].price_unit;
                    dgvTemp.Rows[row].Cells["tunit_count"].Value = clipboardModel[i].unit_count;
                    dgvTemp.Rows[row].Cells["tseaover_unit"].Value = clipboardModel[i].seaover_unit;
                    dgvTemp.Rows[row].Cells["tpurchase_price"].Value = clipboardModel[i].purchase_price.ToString();
                    dgvTemp.Rows[row].Cells["tsales_price"].Value = clipboardModel[i].sales_price.ToString();
                    dgvTemp.Rows[row].Cells["tdivision"].Value = clipboardModel[i].division;
                    dgvTemp.Rows[row].Cells["trow_index"].Value = clipboardModel[i].row_index;
                    row += 1;
                }
                clipboardModel = new List<SeaoverCopyModel>();
            }
        }
        //선택한 Cell
        private DataGridViewCell GetStartCell(DataGridView dgView)
        {
            //get the smallest row,column index
            if (dgView.SelectedCells.Count == 0)
                return null;
            int rowIndex = dgView.Rows.Count - 1;
            int colIndex = dgView.Columns.Count - 1;
            foreach (DataGridViewCell dgvCell in dgView.SelectedCells)
            {
                if (dgvCell.RowIndex < rowIndex)
                    rowIndex = dgvCell.RowIndex;
                if (dgvCell.ColumnIndex < colIndex)
                    colIndex = dgvCell.ColumnIndex;
            }
            return dgView[colIndex, rowIndex];
        }
        //Clipboard에 있는 Value를 \t(수평 텝) 기준으로 문자열을 나누어 Dictionary에 담음
        private Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardValue)
        {
            Dictionary<int, Dictionary<int, string>> copyValues = new Dictionary<int, Dictionary<int, string>>();
            String[] lines = clipboardValue.Split('\n');

            for (int i = 0; i <= lines.Length - 1; i++)
            {
                copyValues[i] = new Dictionary<int, string>();
                String[] lineContent = lines[i].Split('\t');
                //if an empty cell value copied, then set the dictionay with an empty string
                //else Set value to dictionary
                if (lineContent.Length == 0)
                    copyValues[i][0] = string.Empty;
                else
                {
                    for (int j = 0; j <= lineContent.Length - 1; j++)
                        copyValues[i][j] = lineContent[j];
                }
            }
            return copyValues;
        }





        #endregion

        #region Key event
        private void txtRows_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (MessageBox.Show(this, "입력하신 행수로 초기화 하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataGridView dgv = dgvProduct;
                    if (dgv.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            row.Cells["rows"].Value = txtRows.Text;
                        }
                    }
                    countOver();
                }
            }
        }

        private void txtRows_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void dgvTemp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.X:
                        clipboardModel = new List<SeaoverCopyModel>();
                        if (dgvTemp.SelectedRows.Count > 0)
                        {
                            foreach (DataGridViewRow row in dgvTemp.SelectedRows)
                            {
                                if (row.Index < dgvTemp.Rows.Count)
                                {
                                    SeaoverCopyModel model = new SeaoverCopyModel();
                                    model.accent = Convert.ToBoolean(row.Cells["taccent"].Value);
                                    model.category1 = row.Cells["tcategory1"].Value.ToString();
                                    model.category2 = row.Cells["tcategory2"].Value.ToString();
                                    model.category3 = row.Cells["tcategory3"].Value.ToString();
                                    model.category_code = row.Cells["tcategory_code"].Value.ToString();
                                    model.category = row.Cells["tcategory"].Value.ToString();
                                    model.product_code = row.Cells["tproduct_code"].Value.ToString();
                                    model.product = row.Cells["tproduct"].Value.ToString();
                                    model.origin_code = row.Cells["torigin_code"].Value.ToString();
                                    model.origin = row.Cells["torigin"].Value.ToString();
                                    model.sizes_code = row.Cells["tsizes_code"].Value.ToString();
                                    model.sizes = row.Cells["tsizes"].Value.ToString();
                                    model.weight = row.Cells["tweight"].Value.ToString();
                                    model.unit = row.Cells["tunit"].Value.ToString();
                                    model.price_unit = row.Cells["tprice_unit"].Value.ToString();
                                    model.unit_count = row.Cells["tunit_count"].Value.ToString();
                                    model.seaover_unit = row.Cells["tseaover_unit"].Value.ToString();
                                    model.purchase_price = row.Cells["tpurchase_price"].Value.ToString();
                                    model.sales_price = row.Cells["tsales_price"].Value.ToString();
                                    model.division = row.Cells["tdivision"].Value.ToString();
                                    model.row = "1";
                                    model.row_index = row.Cells["trow_index"].Value.ToString();
                                    model.cnt = "";

                                    clipboardModel.Add(model);
                                    dgvTemp.Rows.Remove(row);
                                }
                            }
                        }
                        break;
                    case Keys.V:
                        PasteTempClipboardRowsValue();
                        break;
                }
            }
        }

        private void txtCurPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Reflection();
                RadioBtnClick();
            }
        }
        #endregion

        #region Change event
        private void txtCurPage_TextChanged(object sender, EventArgs e)
        {
            pages = Convert.ToInt16(txtCurPage.Text);
        }

        private void cbArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            Reflection();
            RadioBtnClick();
        }
        #endregion

    }
}

