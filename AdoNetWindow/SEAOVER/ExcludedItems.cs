using AdoNetWindow.Model;
using Repositories;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AdoNetWindow.SEAOVER
{
    public partial class ExcludedItems : Form
    {
        IStockRepository stockRepository = new StockRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        Dictionary<int, int> eList = new Dictionary<int, int>();
        UsersModel um;
        public ExcludedItems(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
        }
        private void ExcludedItems_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            DataGridView dgv = dgvProduct;

            //헤더 디자인
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            dgv.Columns["category_code"].Visible = false;
            dgv.Columns["product_code"].Visible = false;
            dgv.Columns["sizes_code"].Visible = false;
            dgv.Columns["origin_code"].Visible = false;
            dgv.Columns["rowIndex"].Visible = false;
            dgv.Columns["unit"].Visible = false;
            dgv.Columns["price_unit"].Visible = false;
            dgv.Columns["unit_count"].Visible = false;
            dgv.Columns["seaover_unit"].Visible = false;


            dgv.Columns["category"].DefaultCellStyle.BackColor = Color.FromArgb(255, 204, 153);
            dgv.Columns["category"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["category"].HeaderCell.Style.ForeColor = Color.Yellow;


            dgv.Columns["product"].DefaultCellStyle.BackColor = Color.FromArgb(221, 235, 247);
            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["weight"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["weight"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sales_price"].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgv.Columns["sales_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
            dgv.Columns["sales_price"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["sales_price"].HeaderCell.Style.ForeColor = Color.Yellow;

            dgv.Columns["seaover_price"].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgv.Columns["seaover_price"].DefaultCellStyle.BackColor = Color.Bisque;
            dgv.Columns["seaover_price"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["seaover_price"].HeaderCell.Style.ForeColor = Color.Yellow;

            dgv.Columns["division"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["division"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["price_change"].HeaderCell.Style.BackColor = Color.Red;
            dgv.Columns["price_change"].HeaderCell.Style.ForeColor = Color.White;

            dgv.AllowUserToAddRows = false;
        }
        #region Method
        private void SearchingProduct()
        {
            string category = txtCategory.Text.Trim();
            string product = txtProduct.Text.Trim();
            string origin = txtOrigin.Text.Trim();
            string weight = txtWeight.Text.Trim();
            string sizes = txtSizes.Text.Trim();
            string division = txtDivision.Text.Trim();

            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 1)
            {
                DataTable dt = Libs.Tools.Common.GetDataGridViewAsDataTable(dgv);

                if (string.IsNullOrEmpty(category)
                        && string.IsNullOrEmpty(product)
                        && string.IsNullOrEmpty(origin)
                        && string.IsNullOrEmpty(weight)
                        && string.IsNullOrEmpty(sizes)
                        && string.IsNullOrEmpty(division))
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        dgv.Rows[i].Visible = true;
                    }
                }
                else
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        dgv.Rows[i].Visible = true;
                    }


                    if (!string.IsNullOrEmpty(category))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["category"].Value.ToString().Contains(category))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(product))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["product"].Value.ToString().Contains(product))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(origin))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["origin"].Value.ToString().Contains(origin))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(weight))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["weight"].Value.ToString().Contains(weight))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(sizes))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["sizes"].Value.ToString().Contains(sizes))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(division))
                    {
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (!dgv.Rows[i].Cells["division"].Value.ToString().Contains(division))
                            {
                                dgv.Rows[i].Visible = false;
                            }
                        }
                    }
                }


            }
        }
        public Dictionary<int, int> Manager(List<FormDataModel> list, Point p)
        {
            DataGridView dgv = dgvProduct;
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                { 
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["category"].Value = list[i].category;
                    dgvProduct.Rows[n].Cells["category_code"].Value = list[i].category_code;
                    dgvProduct.Rows[n].Cells["product_code"].Value = list[i].product_code;
                    dgvProduct.Rows[n].Cells["product"].Value = list[i].product;
                    dgvProduct.Rows[n].Cells["origin_code"].Value = list[i].origin_code;
                    dgvProduct.Rows[n].Cells["origin"].Value = list[i].origin;
                    dgvProduct.Rows[n].Cells["weight"].Value = list[i].weight;
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = list[i].sizes_code;
                    dgvProduct.Rows[n].Cells["sizes"].Value = list[i].sizes;
                    dgvProduct.Rows[n].Cells["division"].Value = list[i].division;
                    dgvProduct.Rows[n].Cells["rowindex"].Value = list[i].page.ToString("#,##0");
                    if (list[i].sales_price == -1)
                        dgvProduct.Rows[n].Cells["sales_price"].Value = "-";
                    else
                        dgvProduct.Rows[n].Cells["sales_price"].Value = "문의";
                    //dgvProduct.Rows[n].Cells["seaover_price"].Value = list[i].purchase_price;
                    dgvProduct.Rows[n].Cells["unit"].Value = list[i].unit;
                    dgvProduct.Rows[n].Cells["price_unit"].Value = list[i].price_unit;
                    dgvProduct.Rows[n].Cells["unit_count"].Value = list[i].unit_count;
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = list[i].seaover_unit;


                    if (!string.IsNullOrEmpty(list[i].updatetime))
                    {
                        string[] tmps = list[i].updatetime.ToString().Split('/');
                        if (tmps.Length > 1)
                        {
                            //ComboBox 유형의 셀 만들고
                            DataGridViewComboBoxCell cCell = new DataGridViewComboBoxCell();
                            cCell.Style.BackColor = Color.Bisque;
                            //DisplayStyle을 ComboBox로 설정
                            cCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;

                            //ComboBox에 데이터 임시로 넣고
                            for (int j = 0; j < tmps.Length; j++)
                            {
                                double d;
                                if(double.TryParse(tmps[j], out d))
                                    cCell.Items.Add(d.ToString("#,##0"));
                                else
                                    cCell.Items.Add(tmps[j]);
                            }
                            cCell.Value = cCell.Items[0];
                            dgvProduct.Rows[n].Cells["seaover_price"] = cCell;
                        }    
                    }
                    else
                    {
                        dgvProduct.Rows[n].Cells["seaover_price"].Value = list[i].purchase_price.ToString("#,##0");
                    }
                }
            }

            this.Location = p;
            this.ShowDialog();
            this.Location = p;
            return eList;
        }
        #endregion

        #region Button
        private void btnStockRefresh_Click(object sender, EventArgs e)
        {
            int result = stockRepository.CallStoredProcedureSTOCK(um.seaover_id, DateTime.Now.ToString("yyyy-MM-dd"));
            DataTable stockDt = stockRepository.GetExistStock();
            if (stockDt.Rows.Count > 0 && dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];
                    string whr = $"품명코드 = '{row.Cells["product_code"].Value.ToString()}'"
                                + $" AND 원산지코드 = '{row.Cells["origin_code"].Value.ToString()}'"
                                + $" AND 규격코드 = '{row.Cells["sizes_code"].Value.ToString()}'"
                                + $" AND 단위 = '{row.Cells["seaover_unit"].Value.ToString()}'";
                    DataRow[] dr = stockDt.Select(whr);
                    if (dr.Length > 0)
                        row.Cells["sales_price"].Value = row.Cells["seaover_price"].Value;
                    else
                    {
                        whr = $"품명코드 = '{row.Cells["product_code"].Value.ToString()}'"
                                + $" AND 원산지코드 = '{row.Cells["origin_code"].Value.ToString()}'"
                                + $" AND 규격코드 = '{row.Cells["sizes_code"].Value.ToString()}'"
                                + $" AND 단위 = '{row.Cells["unit"].Value.ToString()}'";
                        dr = stockDt.Select(whr);
                        if (dr.Length > 0)
                            row.Cells["sales_price"].Value = row.Cells["seaover_price"].Value;  
                    }
                }
            }
            MessageBox.Show(this, "완료");
            this.Activate();
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "제외설정 해제품목을 모든 품목서에 적용하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                AllDataUdpate();
            insertFunc();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            eList = new Dictionary<int, int>();
            this.Dispose();
        }
        #endregion

        #region Method

        private void AllDataUdpate()
        {
            StringBuilder sql = new StringBuilder();
            List<StringBuilder> sqlList = new List<StringBuilder>();

            DataGridView dgv = dgvProduct;
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (dgv.Rows[i].Cells["sales_price"].Value != null && dgv.Rows[i].Cells["sales_price"].Value.ToString() != "-" && dgv.Rows[i].Cells["sales_price"].Value.ToString() != "문의")
                    {
                        DataGridViewRow row = dgv.Rows[i];
                        FormDataModel model = new FormDataModel();
                        model.category_code = row.Cells["category_code"].Value.ToString();
                        model.product_code = row.Cells["product_code"].Value.ToString();
                        model.origin_code = row.Cells["origin_code"].Value.ToString();
                        model.sizes_code = row.Cells["sizes_code"].Value.ToString();
                        model.unit = row.Cells["unit"].Value.ToString();
                        model.price_unit = row.Cells["price_unit"].Value.ToString();
                        model.unit_count = row.Cells["unit_count"].Value.ToString();
                        model.seaover_unit = row.Cells["seaover_unit"].Value.ToString();
                        double price = Convert.ToInt32(row.Cells["sales_price"].Value.ToString().Replace(",", ""));
                        model.sales_price = price;

                        sql = seaoverRepository.AllDataUpdateExlude(model);
                        sqlList.Add(sql);
                    }
                }
                //Execute
                if (sqlList.Count > 0)
                {
                    int results = seaoverRepository.UpdateTrans(sqlList);
                    if (results == -1)
                    {
                        MessageBox.Show(this, "적용중 에러가 발생하였습니다.");
                        this.Activate();
                    }
                }
            }
        }

        private void insertFunc()
        {
            DataGridView dgv = dgvProduct;
            eList = new Dictionary<int, int>();
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    bool is_delete = Convert.ToBoolean(dgv.Rows[i].Cells["is_delete"].Value);
                    if ((dgv.Rows[i].Cells["sales_price"].Value != "-" && dgv.Rows[i].Cells["sales_price"].Value != "문의") || is_delete)
                    {
                        if (is_delete)
                            eList.Add(Convert.ToInt32(dgv.Rows[i].Cells["rowIndex"].Value), -999);
                        else
                            eList.Add(Convert.ToInt32(dgv.Rows[i].Cells["rowIndex"].Value), Convert.ToInt32(dgv.Rows[i].Cells["sales_price"].Value.ToString().Replace(",", "")));
                    }
                }
            }

            this.Dispose();
        }

        #endregion

        #region Key event
        private void txtCategory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchingProduct();
            }
        }

        private void txtProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchingProduct();
            }
        }
        private void ExcludedItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        {
                            btnStockRefresh.PerformClick();
                            break;
                        }
                    case Keys.A:
                        {
                            insertFunc();
                            break;
                        }
                    case Keys.X:
                        {
                            this.Dispose();
                            break;
                        }
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        {
                            eList = new Dictionary<int, int>();
                            this.Dispose();

                            break;
                        }
                }
            }
        }
        #endregion

        #region Datagridview event
        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = dgvProduct;
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 7)
                {
                    dgv.Rows[e.RowIndex].Cells["sales_price"].Value = dgv.Rows[e.RowIndex].Cells["seaover_price"].Value;
                }
            }
            else
            {
                if (e.ColumnIndex == 7)
                {
                    if (dgv.Rows.Count > 0)
                    {
                        if (MessageBox.Show(this, "모든 내역을 변경하시겠습니가?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            for (int i = 0; i < dgv.Rows.Count; i++)
                            {
                                dgv.Rows[i].Cells["sales_price"].Value = dgv.Rows[i].Cells["seaover_price"].Value;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 우클릭 메뉴
        ContextMenuStrip m;
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

                    m = new ContextMenuStrip();
                    m.Items.Add("단가변경");
                    m.Items.Add("품목삭제");
                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
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
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvProduct.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvProduct.SelectedRows[0];
                    if (dr.Index < 0)
                        return;

                    int rowindex = dr.Index;
                    /*PendingInfo p;*/
                    switch (e.ClickedItem.Text)
                    {
                        case "단가변경":
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                                row.Cells["sales_price"].Value = row.Cells["seaover_price"].Value;
                            break;
                        case "품목삭제":
                            foreach (DataGridViewRow row in dgvProduct.SelectedRows)
                            {
                                row.Cells["is_delete"].Value = true;
                                row.Visible = false;
                            }

                            DataGridView dgv = dgvProduct;
                            eList = new Dictionary<int, int>();
                            if (dgv.Rows.Count > 0)
                            {
                                for (int i = 0; i < dgv.Rows.Count; i++)
                                {
                                    bool is_delete = Convert.ToBoolean(dgv.Rows[i].Cells["is_delete"].Value);
                                    if ((dgv.Rows[i].Cells["sales_price"].Value != "-" && dgv.Rows[i].Cells["sales_price"].Value != "문의") || is_delete)
                                    {
                                        if (is_delete)
                                            eList.Add(Convert.ToInt32(dgv.Rows[i].Cells["rowIndex"].Value), -999);
                                        else
                                            eList.Add(Convert.ToInt32(dgv.Rows[i].Cells["rowIndex"].Value), Convert.ToInt32(dgv.Rows[i].Cells["sales_price"].Value.ToString().Replace(",", "")));
                                    }
                                }
                            }

                            break;
                        default:
                            break;
                    }
                }
                catch
                { }
            }
        }

        private void cms_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            /*switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.A:
                    if (dgvProduct.SelectedRows.Count > 0)
                        m.Items[0].PerformClick();
                    break;
                case Keys.D:

                    break;
            }*/
        }
        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0 && dgvProduct.SelectedRows.Count <= 1)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }
        #endregion
        
    }
}
