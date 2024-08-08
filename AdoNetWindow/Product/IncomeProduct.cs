using AdoNetWindow.Common.PrintManager;
using AdoNetWindow.Model;
using Repositories;
using Repositories.Config;
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
using Excel = Microsoft.Office.Interop.Excel;


namespace AdoNetWindow.Product
{
    public partial class IncomeProduct : Form
    {
        ICommonRepository commonRepository = new CommonRepository();
        IincomeProductRepository incomeProductRepository = new IncomeProductRepository();
        IProductOtherCostRepository productOtherCostRepository = new ProductOtherCostRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        string[] strHeaders = new string[0];
        UsersModel um;
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;
        Libs.MessageBox messageBox = new Libs.MessageBox();

        public IncomeProduct(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
            txtManager.Text = um.user_name;
        }

        private void IncomeProduct_Load(object sender, EventArgs e)
        {
            txtUserName.Text = um.user_name;
            GetUserProduct();
            btnSearch.PerformClick();

            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "수입예정관리", "is_print"))
                {
                    btnPrinting.Visible = false;
                }
            }
        }

        #region Button
        private void btnPrinting_Click(object sender, EventArgs e)
        {
            Printing();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            dgvProduct.Rows.Clear();
            dgvProduct.Columns.Clear();
            strHeaders = new string[0];

            DataTable incomeProductDt = incomeProductRepository.GetIncomeProduct(txtDivision.Text, txtProduct.Text, txtOrigin.Text, txtManager.Text);
            if (incomeProductDt.Rows.Count > 0)
            {
                List<string> divisionList = new List<string>();
                for (int i = 0; i < incomeProductDt.Rows.Count; i++)
                {
                    if (!divisionList.Contains(incomeProductDt.Rows[i]["division"].ToString()))
                        divisionList.Add(incomeProductDt.Rows[i]["division"].ToString());
                }
                int max_row = 0;
                //구분 헤더
                strHeaders = new string[divisionList.Count];
                for (int i = 0; i < strHeaders.Length; i++)
                {
                    strHeaders[i] = divisionList[i];
                    //컬럼추가
                    string col_name = um.user_name + "_" + strHeaders[i] + "_product";
                    dgvProduct.Columns.Add(col_name, "품명");
                    dgvProduct.Columns[col_name].Width = 100;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                    int col = dgvProduct.Columns[col_name].Index;

                    col_name = um.user_name + "_" + strHeaders[i] + "_origin";
                    dgvProduct.Columns.Add(col_name, "원산지");
                    dgvProduct.Columns[col_name].Width = 70;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    col_name = um.user_name + "_" + strHeaders[i] + "_process";
                    dgvProduct.Columns.Add(col_name, "진행");
                    dgvProduct.Columns[col_name].Width = 70;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    col_name = um.user_name + "_" + strHeaders[i] + "_reason";
                    dgvProduct.Columns.Add(col_name, "이유");
                    dgvProduct.Columns[col_name].Width = 100;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    col_name = um.user_name + "_" + strHeaders[i] + "_remark";
                    dgvProduct.Columns.Add(col_name, "비고");
                    dgvProduct.Columns[col_name].Width = 100;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    col_name = um.user_name + "_" + strHeaders[i] + "_manager";
                    dgvProduct.Columns.Add(col_name, "담당");
                    dgvProduct.Columns[col_name].Width = 50;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                    //Row추가
                    DataRow[] dr = incomeProductDt.Select($"division = '{strHeaders[i]}'");
                    if (dr.Length > 0)
                    {
                        for (int j = 0; j < dr.Length; j++)
                        {
                            int n = GetNextRowindex(col);
                            if (dgvProduct.Rows.Count - 2 < n)
                                n = dgvProduct.Rows.Add();

                            dgvProduct.Rows[n].Cells[col].Value = dr[j]["product"].ToString();
                            dgvProduct.Rows[n].Cells[col + 1].Value = dr[j]["origin"].ToString();
                            dgvProduct.Rows[n].Cells[col + 2].Value = dr[j]["process"].ToString();
                            dgvProduct.Rows[n].Cells[col + 3].Value = dr[j]["reason"].ToString();
                            dgvProduct.Rows[n].Cells[col + 4].Value = dr[j]["remark"].ToString();
                            dgvProduct.Rows[n].Cells[col + 5].Value = dr[j]["manager"].ToString();
                        }
                    }
                }
            }
            else 
            {
                if (messageBox.Show(this, "검색된 내역이 없습니다. 담당 품목을 불러오시겠습니까?", "YesOrNo" , MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    strHeaders = new string[1];
                    strHeaders[0] = "담당품목";
                    //컬럼추가
                    string col_name = um.user_name + "_" + strHeaders[0] + "_product";
                    dgvProduct.Columns.Add(col_name, "품명");
                    dgvProduct.Columns[col_name].Width = 100;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
                    int col = dgvProduct.Columns[col_name].Index;

                    col_name = um.user_name + "_" + strHeaders[0] + "_origin";
                    dgvProduct.Columns.Add(col_name, "원산지");
                    dgvProduct.Columns[col_name].Width = 70;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    col_name = um.user_name + "_" + strHeaders[0] + "_process";
                    dgvProduct.Columns.Add(col_name, "진행");
                    dgvProduct.Columns[col_name].Width = 70;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    col_name = um.user_name + "_" + strHeaders[0] + "_reason";
                    dgvProduct.Columns.Add(col_name, "이유");
                    dgvProduct.Columns[col_name].Width = 100;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    col_name = um.user_name + "_" + strHeaders[0] + "_remark";
                    dgvProduct.Columns.Add(col_name, "비고");
                    dgvProduct.Columns[col_name].Width = 100;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    col_name = um.user_name + "_" + strHeaders[0] + "_manager";
                    dgvProduct.Columns.Add(col_name, "담당");
                    dgvProduct.Columns[col_name].Width = 50;
                    dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

                    DataTable productDt = productOtherCostRepository.GetProductByUser("", "", um.user_name);
                    if (productDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < productDt.Rows.Count; i++)
                        {
                            int n = dgvProduct.Rows.Add();
                            dgvProduct.Rows[n].Cells[um.user_name + "_" + strHeaders[0] + "_product"].Value = productDt.Rows[i]["product"].ToString();
                            dgvProduct.Rows[n].Cells[um.user_name + "_" + strHeaders[0] + "_origin"].Value = productDt.Rows[i]["origin"].ToString();
                            dgvProduct.Rows[n].Cells[um.user_name + "_" + strHeaders[0] + "_manager"].Value = productDt.Rows[i]["manager"].ToString();
                        }
                    }

                }
            }
        }


        private int GetNextRowindex(int col)
        {
            int row = 0;
            dgvProduct.EndEdit();
            bool clearCell = true;
            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (dgvProduct.Rows[i].Cells[col].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[i].Cells[col].Value.ToString()))
                    clearCell = false;
            }
            if (clearCell)
                return 0;


            for (int i = 0; i < dgvProduct.Rows.Count; i++)
            {
                if (dgvProduct.Rows[i].Cells[col].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[i].Cells[col].Value.ToString()))
                    row = i;
            }
            return row + 1;
        }
        private void btnRegister_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "수입예정관리", "is_update"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvProduct.Rows.Count > 0)
            {
                List<StringBuilder> sqlList = new List<StringBuilder>();

                for (int i = 0; i < dgvProduct.Columns.Count; i = i + 6)
                {
                    string col_name = dgvProduct.Columns[i].Name;
                    string manager = col_name.Split('_')[0].Trim();
                    string division = col_name.Split('_')[1].Trim();
                    //기존내역 삭제
                    IncomeProductModel deleteModel = new IncomeProductModel();
                    deleteModel.division = division;
                    deleteModel.manager = manager;
                    StringBuilder sql = incomeProductRepository.DeleteData(deleteModel);
                    sqlList.Add(sql);

                    //등록
                    for (int j = 0; j < dgvProduct.Rows.Count; j++)
                    {
                        //Null제거
                        for (int k = 0; k < 6; k++)
                        {
                            if (dgvProduct.Rows[j].Cells[i + k].Value == null)
                                dgvProduct.Rows[j].Cells[i + k].Value = "";
                        }
                        if (dgvProduct.Rows[j].Cells[i].Value != null && !string.IsNullOrEmpty(dgvProduct.Rows[j].Cells[i].Value.ToString()))
                        {
                            IncomeProductModel model = new IncomeProductModel();
                            model.division = division;
                            model.product = dgvProduct.Rows[j].Cells[i].Value.ToString();
                            model.origin = dgvProduct.Rows[j].Cells[i + 1].Value.ToString();
                            model.process = dgvProduct.Rows[j].Cells[i + 2].Value.ToString();
                            model.reason = dgvProduct.Rows[j].Cells[i + 3].Value.ToString();
                            model.remark = dgvProduct.Rows[j].Cells[i + 4].Value.ToString();
                            model.manager = dgvProduct.Rows[j].Cells[i + 5].Value.ToString();
                            model.edit_user = um.user_name;
                            model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            sql = incomeProductRepository.InsertData(model);
                            sqlList.Add(sql);
                        }
                    }
                }

                if (sqlList.Count > 0)
                {
                    if (commonRepository.UpdateTran(sqlList) == -1)
                        messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                    else
                        messageBox.Show(this, "등록완료");
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        private void btnAddDivision_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "수입예정관리", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (string.IsNullOrEmpty(txtInputDivision.Text.Trim()))
            {
                messageBox.Show(this, "구분을 입력해주세요!");
                return;
            }
            //유효성검사
            for (int i = 0; i < dgvProduct.ColumnCount; i++)
            {
                if (dgvProduct.Columns[i].Name == um.user_name + "_" + txtInputDivision.Text + "_product")
                {
                    messageBox.Show(this, "이미 동일한 구분이 존재합니다");
                    return;
                }
            }
            //구분 헤더
            string[] tempHeaders = new string[strHeaders.Length + 1];
            for (int i = 0; i < strHeaders.Length; i++)
            {
                tempHeaders[i] = strHeaders[i];
            }
            tempHeaders[strHeaders.Length] = txtInputDivision.Text;
            strHeaders = tempHeaders;
            //컬럼추가
            string col_name = um.user_name + "_" + txtInputDivision.Text + "_product";
            dgvProduct.Columns.Add(col_name, "품명");
            dgvProduct.Columns[col_name].Width = 100;
            dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

            col_name = um.user_name + "_" + txtInputDivision.Text + "_origin";
            dgvProduct.Columns.Add(col_name, "원산지");
            dgvProduct.Columns[col_name].Width = 70;
            dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

            col_name = um.user_name + "_" + txtInputDivision.Text + "_process";
            dgvProduct.Columns.Add(col_name, "진행");
            dgvProduct.Columns[col_name].Width = 70;
            dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

            col_name = um.user_name + "_" + txtInputDivision.Text + "_reason";
            dgvProduct.Columns.Add(col_name, "이유");
            dgvProduct.Columns[col_name].Width = 100;
            dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

            col_name = um.user_name + "_" + txtInputDivision.Text + "_remark";
            dgvProduct.Columns.Add(col_name, "비고");
            dgvProduct.Columns[col_name].Width = 100;
            dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;

            col_name = um.user_name + "_" + txtInputDivision.Text + "_manager";
            dgvProduct.Columns.Add(col_name, "담당");
            dgvProduct.Columns[col_name].Width = 50;
            dgvProduct.Columns[col_name].SortMode = DataGridViewColumnSortMode.NotSortable;
            dgvProduct.Columns[col_name].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
        }
        #endregion

        #region Datagridview 멀티헤더

        private void dgvProduct_Paint(object sender, PaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            if (strHeaders.Length > 0)
            {
                int col_inx = -1;
                for (int i = 0; i < strHeaders.Length; i++)
                {
                    Color col = Color.FromArgb(226, 239, 218);
                    col_inx += 1;
                    Rectangle r1 = gv.GetCellDisplayRectangle(col_inx, -1, false);
                    col_inx += 1;
                    int width1 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width2 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width3 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width4 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    col_inx += 1;
                    int width5 = gv.GetCellDisplayRectangle(col_inx, -1, false).Width;
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + width1 + width2 + width3 + width4 + width5 - 2;
                    r1.Height = (r1.Height / 2) - 2;
                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                    Color colBlue = Color.FromArgb(221, 235, 247);
                    e.Graphics.FillRectangle(new SolidBrush(col), r1);
                    e.Graphics.DrawString(strHeaders[i], gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                }
            }
        }
        private void dgvProduct_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r = e.CellBounds;
                r.Y += e.CellBounds.Height / 2;
                r.Height = e.CellBounds.Height / 2;
                e.PaintBackground(r, true);
                e.PaintContent(r);
                e.Handled = true;
            }
        }

        private void dgvProduct_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void dgvProduct_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }


        #endregion

        #region Key event
        private void txtManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSearch.PerformClick();
        }
        private void IncomeProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                { 
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                    case Keys.S:
                        btnAddDivision.PerformClick();
                        break;
                    case Keys.A:
                        btnRegister.PerformClick();
                        break;
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        Printing();
                        break;
                }
            }
        }
        #endregion

        #region 우클릭 메뉴
        ContextMenuStrip m = new ContextMenuStrip();
        int col = -1;
        private void dgvUserProduct_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvUserProduct.HitTest(e.X, e.Y);

                    col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;
                    m = new ContextMenuStrip();
                        
                    m.Items.Add("추가");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    //Create 
                    m.Show(dgvUserProduct, e.Location);
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
        private void dgvUsers_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvProduct.HitTest(e.X, e.Y);

                    col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    m = new ContextMenuStrip();
                    if (row == -1)
                    {
                        col = (int)Math.Truncate((double)col / 6) * 6;
                        dgvProduct.ClearSelection();
                        for (int i = col; i < col + 6; i++)
                        {
                            for (int j = 0; j < dgvProduct.Rows.Count; j++)
                            {
                                dgvProduct.Rows[j].Cells[i].Selected = true;
                            }
                        }
                        m.Items.Add("정렬하기(품명+원산지");
                        m.Items.Add("정렬하기(원산지+품명");
                        m.Items.Add("삭제");

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
            }
            catch (Exception ex)
            {

            }
        }

        //우클릭 메뉴 Event Handler
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "삭제":
                    if (dgvProduct.ColumnCount > 0 && col >= 0)
                    {

                        string col_name = dgvProduct.CurrentCell.OwningColumn.Name;
                        string manager = col_name.Split('_')[0].Trim();
                        string division = col_name.Split('_')[1].Trim();
                        //기존내역 삭제
                        IncomeProductModel deleteModel = new IncomeProductModel();
                        deleteModel.division = division;
                        deleteModel.manager = manager;
                        StringBuilder sql = incomeProductRepository.DeleteData(deleteModel);
                        List<StringBuilder> sqlList = new List<StringBuilder>();
                        sqlList.Add(sql);
                        if (commonRepository.UpdateTran(sqlList) == -1)
                        {
                            messageBox.Show(this, "삭제중 에러가 발생하였습니다.");
                            return;
                        }

        
                        string divisin = dgvProduct.Columns[col].Name.Split('_')[1];

                        for (int i = dgvProduct.ColumnCount - 1; i >= 0; i--)
                        {
                            string col_divisin = dgvProduct.Columns[i].Name.Split('_')[1];
                            if (col_divisin == divisin)
                                dgvProduct.Columns.Remove(dgvProduct.Columns[i]);

                        }

                        string[] tempHeaders = new string[strHeaders.Length - 1];
                        int idx = 0;
                        for (int i = 0; i < strHeaders.Length; i++)
                        {
                            if (strHeaders[i] != divisin)
                            { 
                                tempHeaders[idx] = strHeaders[i];
                                idx++;
                            }
                        }
                        strHeaders = tempHeaders;   
                        
                    }
                    break;

                case "정렬하기(품명+원산지)":
                    if (dgvProduct.ColumnCount > 0 && col >= 0)
                    {
                        string col_name = dgvProduct.CurrentCell.OwningColumn.Name;
                        string manager = col_name.Split('_')[0].Trim();
                        string division = col_name.Split('_')[1].Trim();
                        int col_index = dgvProduct.Columns[manager + "_" + division + "_product"].Index;

                        DataTable sortDt = common.ConvertDgvToDataTable(dgvProduct, true);
                        DataView sortView = new DataView(sortDt);
                        sortView.Sort = manager + "_" + division + "_product" + ", " + manager + "_" + division + "_origin";
                        sortDt = sortView.ToTable();

                        if (sortDt.Rows.Count > 0)
                        {
                            int min_col = 999, max_col = 0;
                            for (int i = 0; i < dgvProduct.ColumnCount; i++)
                            {
                                if (dgvProduct.Rows[0].Cells[i].Selected && i <= min_col)
                                    min_col = i;
                                if (dgvProduct.Rows[0].Cells[i].Selected && i >= max_col)
                                    max_col = i;
                            }

                            for (int i = 0; i < sortDt.Rows.Count; i++)
                            {
                                for (int j = 0; j < sortDt.Columns.Count; j++)
                                {
                                    dgvProduct.Rows[i].Cells[min_col + j].Value = sortDt.Rows[i][j].ToString();
                                }
                            }
                        }

                    }
                    break;
                case "정렬하기(원산지+품명)":
                    if (dgvProduct.ColumnCount > 0 && col >= 0)
                    {
                        string col_name = dgvProduct.CurrentCell.OwningColumn.Name;
                        string manager = col_name.Split('_')[0].Trim();
                        string division = col_name.Split('_')[1].Trim();
                        int col_index = dgvProduct.Columns[manager + "_" + division + "_product"].Index;

                        DataTable sortDt = common.ConvertDgvToDataTable(dgvProduct, true);
                        DataView sortView = new DataView(sortDt);
                        sortView.Sort = manager + "_" + division + "_origin" + ", " + manager + "_" + division + "_product";
                        sortDt = sortView.ToTable();

                        if (sortDt.Rows.Count > 0)
                        {
                            int min_col = 999, max_col = 0;
                            for (int i = 0; i < dgvProduct.ColumnCount; i++)
                            {
                                if (dgvProduct.Rows[0].Cells[i].Selected && i <= min_col)
                                    min_col = i;
                                if (dgvProduct.Rows[0].Cells[i].Selected && i >= max_col)
                                    max_col = i;
                            }

                            for (int i = 0; i < sortDt.Rows.Count; i++)
                            {
                                for (int j = 0; j < sortDt.Columns.Count; j++)
                                {
                                    dgvProduct.Rows[i].Cells[min_col + j].Value = sortDt.Rows[i][j].ToString();
                                }
                            }
                        }

                    }
                    break;
                case "추가":
                    if (dgvUserProduct.ColumnCount > 0 && dgvUserProduct.SelectedRows.Count >= 0)
                    {

                    }
                    break;
                default:
                    break;
            }
        }


        #endregion

        #region 인쇄, 미리보기

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;
        private void Printing()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "기준정보", "수입예정관리", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            CaptureScreen();
            printDocument1.DefaultPageSettings.Landscape = true;
            //printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A3", 297 * 4, 420 * 5);
            printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("A4", this.Height, this.Width);
            PrintManager pm = new PrintManager(printDocument1);
            pm.ShowDialog();
        }

        private void CaptureScreen()
        {
            Graphics mygraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }




        #endregion

        #region Datagridview event
        private void dgvProduct_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (!dgvProduct.Columns[e.ColumnIndex].Name.Contains("manager"))
                {
                    int col = (int)Math.Truncate((double)e.ColumnIndex / 6) * 6;
                    if (dgvProduct.Rows[e.RowIndex].Cells[col + 5].Value == null || string.IsNullOrEmpty(dgvProduct.Rows[e.RowIndex].Cells[col + 5].Value.ToString()))
                        dgvProduct.Rows[e.RowIndex].Cells[col + 5].Value = um.user_name;
                }
            }
        }
        #endregion

        #region 담당자별 품목검색
        private void GetUserProduct()
        {
            dgvUserProduct.Rows.Clear();
            DataTable productDt = productOtherCostRepository.GetProductByUser(txtUserProduct.Text, txtUserOrigin.Text, txtUserName.Text);
            if (productDt.Rows.Count > 0)
            {
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    int n = dgvUserProduct.Rows.Add();
                    dgvUserProduct.Rows[n].Cells["user_product"].Value = productDt.Rows[i]["product"].ToString();
                    dgvUserProduct.Rows[n].Cells["user_origin"].Value = productDt.Rows[i]["origin"].ToString();
                    dgvUserProduct.Rows[n].Cells["manager"].Value = productDt.Rows[i]["manager"].ToString();
                }
            }
        }

        private void txtUserProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetUserProduct();
        }
        #endregion

        
    }
}
