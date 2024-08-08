using AdoNetWindow.Common.PrintManager;
using AdoNetWindow.Model;
using AdoNetWindow.Product;
using Repositories;
using Repositories.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.DashboardForTrade
{
    public partial class PurchaseDashboard : Form
    {
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        string[] strHeaders = new string[1];
        List<string[]> productList = null;
        UsersModel um;
        public PurchaseDashboard(UsersModel um, List<string[]> productList)
        {
            InitializeComponent();
            this.um = um;
            txtSttdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            productList = productList.Distinct().ToList();
            for (int i = 0; i < productList.Count; i++)
            {
                int n = dgvPurchase.Rows.Add();
                DataGridViewRow row = dgvPurchase.Rows[n];
                row.Cells["product"].Value = productList[i][0].ToString();
                row.Cells["origin"].Value = productList[i][1].ToString();
                row.Cells["sizes"].Value = productList[i][2].ToString();
                row.Cells["unit"].Value = productList[i][3].ToString();
                //row.Cells["cost_unit"].Value = productList[i][4].ToString();
            }
            SetHeaderStyle();
        }

        private void PurchaseDashboard_Load(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가 및 재고 대시보드", "is_print"))
                {
                    btnPrinting.Visible = false;
                }
            }
        }

        #region Method
        public void InputProduct(List<string[]> productList)
        {
            DataGridViewRow row;
            for (int i = 0; i < productList.Count; i++)
            {
                bool isDuplicate = false;
                for (int j = 0; j < dgvPurchase.Rows.Count; j++)
                {
                    row = dgvPurchase.Rows[j];
                    if (row.Cells["product"].Value.ToString() == productList[i][0]
                        && row.Cells["origin"].Value.ToString() == productList[i][2]
                        && row.Cells["sizes"].Value.ToString() == productList[i][3]
                        && row.Cells["unit"].Value.ToString() == productList[i][4])
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                //중복이 없을 경우만 추가
                if (!isDuplicate)
                { 
                    int n = dgvPurchase.Rows.Add();
                    row = dgvPurchase.Rows[n];
                    row.Cells["product"].Value = productList[i][0].ToString();
                    row.Cells["origin"].Value = productList[i][1].ToString();
                    row.Cells["sizes"].Value = productList[i][2].ToString();
                    row.Cells["unit"].Value = productList[i][3].ToString();
                }
                //row.Cells["cost_unit"].Value = productList[i][4].ToString();
            }
            GetData();
        }
        private void SetHeaderStyle()
        {
            Color colBlue = Color.FromArgb(221, 235, 247);
            if (dgvPurchase.Columns.Count > 0)
            {
                dgvPurchase.Columns[0].HeaderCell.Style.BackColor = colBlue;
                dgvPurchase.Columns[1].HeaderCell.Style.BackColor = colBlue;
                dgvPurchase.Columns[2].HeaderCell.Style.BackColor = colBlue;
                dgvPurchase.Columns[3].HeaderCell.Style.BackColor = colBlue;
            }
        }
        public void GetData()
        {
            //초기화
            if(dgvPurchase.ColumnCount > 5)
            {
                for (int i = dgvPurchase.ColumnCount - 1; i >= 4; i--)
                {
                    dgvPurchase.Columns.Remove(dgvPurchase.Columns[i]);
                }
            }
            //유효성검사
            DateTime edate;
            if (!DateTime.TryParse(txtSttdate.Text, out edate))
            {
                MessageBox.Show(this,"기준일자를 다시 선택해주세요.");
                this.Activate();
                return;
            }
            DateTime sdate;
            string[] colHeaderTxt = new string[4];
            int comparisonType;
            switch (cbComparisonTerm.Text)
            {
                case "보름(15일) 비교":
                    sdate = edate.AddDays(-61);
                    colHeaderTxt[0] = "15일전";
                    colHeaderTxt[1] = "30일전";
                    colHeaderTxt[2] = "45일전";
                    colHeaderTxt[3] = "60일전";
                    comparisonType = 2;
                    break;
                case "월(약 30일) 비교":
                    sdate = edate.AddMonths(-5);
                    colHeaderTxt[0] = "한달전";
                    colHeaderTxt[1] = "두달전";
                    colHeaderTxt[2] = "세달전";
                    colHeaderTxt[3] = "네달전";
                    comparisonType = 3;
                    break;
                default:
                    sdate = edate.AddMonths(-2);
                    colHeaderTxt[0] = "1주전";
                    colHeaderTxt[1] = "2주전";
                    colHeaderTxt[2] = "3주전";
                    colHeaderTxt[3] = "4주전";
                    comparisonType = 1;
                    break;
            }
            //데이터 호출
            productList = new List<string[]>();
            Dictionary<string, int> rowDic = new Dictionary<string, int>();
            Dictionary<int, int> colDic = new Dictionary<int, int>();
            if (dgvPurchase.Rows.Count > 0)
            {

                for (int i = 0; i < dgvPurchase.Rows.Count; i++)
                {
                    string[] product = new string[4];
                    product[0] = dgvPurchase.Rows[i].Cells["product"].Value.ToString();
                    product[1] = dgvPurchase.Rows[i].Cells["origin"].Value.ToString();
                    product[2] = dgvPurchase.Rows[i].Cells["sizes"].Value.ToString();
                    product[3] = dgvPurchase.Rows[i].Cells["unit"].Value.ToString();

                    productList.Add(product);
                    rowDic.Add(product[0] + "^" + product[1] + "^" + product[2] + "^" + product[3], i);
                }

                DataTable purchaseDt = purchasePriceRepository.GetPurchaseDashborad(sdate.ToString("yyyy-MM-dd"), edate.ToString("yyyy-MM-dd"), productList);
                if (purchaseDt.Rows.Count > 0)
                {
                    this.dgvPurchase.CellPainting -= new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvPurchase_CellPainting);
                    this.dgvPurchase.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvPurchase_ColumnWidthChanged);
                    this.dgvPurchase.Scroll -= new System.Windows.Forms.ScrollEventHandler(this.dgvPurchase_Scroll);
                    this.dgvPurchase.Paint -= new System.Windows.Forms.PaintEventHandler(this.dgvPurchase_Paint);
                    //거래처 목록 출력
                    DataTable companyDt = new DataTable();
                    companyDt = purchaseDt.AsEnumerable()
                        .GroupBy(row => new
                        {
                            COMPANYID = row.Field<int>("company_id"),
                            COMPANY = row.Field<string>("company")
                        }).Select(g =>
                        {
                            var row = purchaseDt.NewRow();

                            row["company_id"] = g.Key.COMPANYID;
                            row["company"] = g.Key.COMPANY;

                            return row;
                        }).CopyToDataTable();

                    if (companyDt != null && companyDt.Rows.Count > 0)
                    {
                        strHeaders = new string[companyDt.Rows.Count];
                        for (int i = 0; i < companyDt.Rows.Count; i++)
                        {
                            //거래처 컬럼 추가
                            dgvPurchase.Columns.Add(companyDt.Rows[i]["company_id"].ToString() + "_current", "최근");
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_current"].Width = 150;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_current"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_current"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_current"].SortMode = DataGridViewColumnSortMode.NotSortable;

                            dgvPurchase.Columns.Add(companyDt.Rows[i]["company_id"].ToString() + "_pre_1w", colHeaderTxt[0]);
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_1w"].Width = 60;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_1w"].Visible = false;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_1w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_1w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                            dgvPurchase.Columns.Add(companyDt.Rows[i]["company_id"].ToString() + "_pre_2w", colHeaderTxt[1]);
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_2w"].Width = 60;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_2w"].Visible = false;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_2w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_2w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                            dgvPurchase.Columns.Add(companyDt.Rows[i]["company_id"].ToString() + "_pre_3w", colHeaderTxt[2]);
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_3w"].Width = 60;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_3w"].Visible = false;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_3w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_3w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                            dgvPurchase.Columns.Add(companyDt.Rows[i]["company_id"].ToString() + "_pre_4w", colHeaderTxt[3]);
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_4w"].Width = 60;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_4w"].Visible = false;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_4w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_4w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                            //배색
                            if (i % 2 == 0)
                            {
                                dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_4w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                                dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_3w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                                dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_2w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                                dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_pre_1w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                                dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_current"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                            }
                            //Dic, HeaderList 추가
                            colDic.Add(Convert.ToInt32(companyDt.Rows[i]["company_id"].ToString()), dgvPurchase.Columns[companyDt.Rows[i]["company_id"].ToString() + "_current"].Index);
                            strHeaders[i] = companyDt.Rows[i]["company"].ToString();
                        }


                        //매입단가 출력
                        for (int i = 0; i < dgvPurchase.Rows.Count; i++)
                        {
                            DataGridViewRow row = dgvPurchase.Rows[i];
                            for (int j = 4; j < dgvPurchase.Columns.Count; j = j + 5)
                            {
                                string[] company_id = dgvPurchase.Columns[j].Name.Split('_');
                                string whr = $"product = '{row.Cells["product"].Value.ToString()}'"
                                  + $" AND origin = '{row.Cells["origin"].Value.ToString()}'"
                                  + $" AND sizes = '{row.Cells["sizes"].Value.ToString()}'"
                                  + $" AND unit = '{row.Cells["unit"].Value.ToString()}'"
                                  + $" AND company_id = {company_id[0]}";
                                DataRow[] dr = purchaseDt.Select(whr);
                                if (dr.Length > 0)
                                {
                                    //주차별 비교
                                    if (comparisonType == 1)
                                    {
                                        int sdate_week = common.GetWeekOfYear(edate, null);
                                        for (int k = 0; k < dr.Length; k++)
                                        {
                                            DateTime purchase_date;
                                            if (DateTime.TryParse(dr[k]["updatetime"].ToString(), out purchase_date))
                                            {
                                                int purchase_week = common.GetWeekOfYear(purchase_date, null);
                                                int diff_week = sdate_week - purchase_week;
                                                if (diff_week < 5 && diff_week >= 0)
                                                {
                                                    double pre_price;
                                                    if (row.Cells[j + diff_week].Value != null && double.TryParse(row.Cells[j + diff_week].Value.ToString(), out pre_price))
                                                    {
                                                        if (pre_price > Convert.ToDouble(dr[k]["purchase_price"].ToString()))
                                                        {
                                                            row.Cells[j + diff_week].Value = dr[k]["purchase_price"].ToString();
                                                            row.Cells[j + diff_week].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.Cells[j + diff_week].Value = dr[k]["purchase_price"].ToString();
                                                        row.Cells[j + diff_week].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //보름 비교
                                    else if (comparisonType == 2)
                                    {
                                        for (int k = 0; k < dr.Length; k++)
                                        {
                                            DateTime purchase_date;
                                            if (DateTime.TryParse(dr[k]["updatetime"].ToString(), out purchase_date))
                                            {
                                                TimeSpan ts = edate.Subtract(purchase_date);
                                                int diff_day = (int)ts.TotalDays / 15;
                                                if (diff_day < 5 && diff_day >= 0)
                                                {
                                                    double pre_price;
                                                    if (row.Cells[j + diff_day].Value != null && double.TryParse(row.Cells[j + diff_day].Value.ToString(), out pre_price))
                                                    {
                                                        if (pre_price > Convert.ToDouble(dr[k]["purchase_price"].ToString()))
                                                        {
                                                            row.Cells[j + diff_day].Value = dr[k]["purchase_price"].ToString();
                                                            row.Cells[j + diff_day].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.Cells[j + diff_day].Value = dr[k]["purchase_price"].ToString();
                                                        row.Cells[j + diff_day].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //월 비교
                                    else if (comparisonType == 3)
                                    {
                                        for (int k = 0; k < dr.Length; k++)
                                        {
                                            DateTime purchase_date;
                                            if (DateTime.TryParse(dr[k]["updatetime"].ToString(), out purchase_date))
                                            {
                                                int diff_month = 12 * (edate.Year - purchase_date.Year) + (edate.Month - purchase_date.Month);
                                                if (diff_month < 5 && diff_month >= 0)
                                                {
                                                    double pre_price;
                                                    if (row.Cells[j + diff_month].Value != null && double.TryParse(row.Cells[j + diff_month].Value.ToString(), out pre_price))
                                                    {
                                                        if (pre_price > Convert.ToDouble(dr[k]["purchase_price"].ToString()))
                                                        {
                                                            row.Cells[j + diff_month].Value = dr[k]["purchase_price"].ToString();
                                                            row.Cells[j + diff_month].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.Cells[j + diff_month].Value = dr[k]["purchase_price"].ToString();
                                                        row.Cells[j + diff_month].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    this.dgvPurchase.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvPurchase_CellPainting);
                    this.dgvPurchase.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvPurchase_ColumnWidthChanged);
                    this.dgvPurchase.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dgvPurchase_Scroll);
                    this.dgvPurchase.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvPurchase_Paint);
                }
            }               
        }
        #endregion

        #region Datagridview 멀티헤더
        private void dgvPurchase_Paint(object sender, PaintEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            // 기준정보
            {
                Rectangle r1 = gv.GetCellDisplayRectangle(0, -1, false);
                int width1 = gv.GetCellDisplayRectangle(1, -1, false).Width;
                int width2 = gv.GetCellDisplayRectangle(2, -1, false).Width;
                int width3 = gv.GetCellDisplayRectangle(3, -1, false).Width;
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width + width1 + width2 + width3 - 2;
                //r1.Width = r1.Width + width1 - 2;
                r1.Height = (r1.Height - (r1.Height / 3)) - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(221, 235, 247);
                e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                e.Graphics.DrawString("품목정보", gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }
            if (strHeaders.Length > 1)
            {
                int col_inx = 3;
                for (int i =0; i < strHeaders.Length; i++)
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
                    r1.X += 1;
                    r1.Y += 1;
                    r1.Width = r1.Width + width1 + width2 + width3 + width4 - 2;
                    r1.Height = (r1.Height - (r1.Height / 3)) - 2;
                    e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                    Color colBlue = Color.FromArgb(221, 235, 247);
                    e.Graphics.FillRectangle(new SolidBrush(col), r1);
                    Font ft = new Font("나눔고딕", 9, FontStyle.Bold);
                    e.Graphics.DrawString(strHeaders[i], ft, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                }
            }
        }
        private void dgvPurchase_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {
                Rectangle r = e.CellBounds;
                r.Y += e.CellBounds.Height / 2;
                r.Height = e.CellBounds.Height - (e.CellBounds.Height / 3);
                e.PaintBackground(r, true);
                e.PaintContent(r);
                e.Handled = true;
            }
        }

        private void dgvPurchase_Scroll(object sender, ScrollEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight - gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }

        private void dgvPurchase_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;
            Rectangle rtHeader = gv.DisplayRectangle;
            rtHeader.Height = gv.ColumnHeadersHeight - gv.ColumnHeadersHeight / 2;
            gv.Invalidate(rtHeader);
        }
        #endregion

        #region Button
        private void btnGetProduct_Click(object sender, EventArgs e)
        {
            ProductManager ps = new ProductManager(um, this);

            // 부모 Form의 좌표, 크기를 계산
            int mainformX = this.Location.X;
            int mainformY = this.Location.Y;
            int mainfromWidth = this.Size.Width;
            int mainfromHeight = this.Size.Height;

            // 자식 Form의 크기를 계산
            int childformwidth = ps.Size.Width;
            int childformheight = ps.Size.Height;
            Point p = new Point(mainformX + (mainfromWidth / 2) - (childformwidth / 2), mainformY + (mainfromHeight / 2) - (childformheight / 2));

            ps.ShowDialog();
        }
        private void btnCalendarSttdate_Click(object sender, EventArgs e)
        {
            Common.Calendar calendar = new Common.Calendar();
            string sdate = calendar.GetDate(true);
            if (sdate != null)
                txtSttdate.Text = sdate;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvPurchase.Rows.Clear();
            if (dgvPurchase.Columns.Count > 4)
            {
                for (int i = dgvPurchase.Columns.Count - 1; i >= 4; i--)
                {
                    dgvPurchase.Columns.Remove(dgvPurchase.Columns[i]);
                }
            }
        }
        #endregion

        #region Key event
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
        private void PurchaseDashboard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        btnPrinting.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        btnGetProduct.PerformClick();
                        break;
                    case Keys.F5:
                        btnRefresh.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Datagridview event
        private void dgvPurchase_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 4)
            {
                string[] company_id = dgvPurchase.Columns[e.ColumnIndex].Name.Split('_');
                if (!dgvPurchase.Columns[company_id[0] + "_pre_1w"].Visible
                    && !dgvPurchase.Columns[company_id[0] + "_pre_2w"].Visible
                    && !dgvPurchase.Columns[company_id[0] + "_pre_3w"].Visible
                    && !dgvPurchase.Columns[company_id[0] + "_pre_4w"].Visible)
                {
                    dgvPurchase.Columns[company_id[0] + "_pre_1w"].Visible = true;
                    dgvPurchase.Columns[company_id[0] + "_pre_2w"].Visible = true;
                    dgvPurchase.Columns[company_id[0] + "_pre_3w"].Visible = true;
                    dgvPurchase.Columns[company_id[0] + "_pre_4w"].Visible = true;
                    dgvPurchase.Columns[company_id[0] + "_current"].Width = 60;
                }
                else
                {
                    dgvPurchase.Columns[company_id[0] + "_pre_1w"].Visible = false;
                    dgvPurchase.Columns[company_id[0] + "_pre_2w"].Visible = false;
                    dgvPurchase.Columns[company_id[0] + "_pre_3w"].Visible = false;
                    dgvPurchase.Columns[company_id[0] + "_pre_4w"].Visible = false;
                    dgvPurchase.Columns[company_id[0] + "_current"].Width = 150;
                }
            }
        }

        #endregion

        #region 인쇄, 미리보기

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;
        private void btnPrinting_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가 및 재고 대시보드", "is_print"))
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

        private void btnExcel_Click(object sender, EventArgs e)
        {

        }
    }
}
