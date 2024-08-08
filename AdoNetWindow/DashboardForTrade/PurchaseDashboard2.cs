using AdoNetWindow.Common;
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
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.DashboardForTrade
{
    public partial class PurchaseDashboard2 : Form
    {
        IAuthorityRepository authorityRepository = new AuthorityRepository(); 
        IPurchasePriceRepository purchasePriceRepository = new PurchasePriceRepository();
        string[] strHeaders = new string[1];
        List<string[]> productList = null;
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um;

        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;
        public PurchaseDashboard2(UsersModel um, List<string[]> productList)
        {
            InitializeComponent();
            this.um = um;
            this.productList = productList;
            this.txtSttdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            setHearder();
        }
        private void PurchaseDashboard2_Load(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;

            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가 및 재고 대시보드", "is_excel"))
                {
                    btnExcel.Visible = false;
                }
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
                for (int j = 2; j < dgvPurchase.Columns.Count; j++)
                {
                    string[] product = dgvPurchase.Columns[j].Name.Split('^');
                    if (product[0] == productList[i][0]
                        && product[1] == productList[i][2]
                        && product[2] == productList[i][3]
                        && product[3] == productList[i][4])
                    {
                        isDuplicate = true;
                        break;
                    }
                }
                //중복이 없을 경우만 추가
                if (!isDuplicate)
                {
                    string col_name = productList[i][0] + "^" + productList[i][1] + "^" + productList[i][2] + "^" + productList[i][3];
                    dgvPurchase.Columns.Add(col_name + "^current", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^current"].Width = 60;
                    dgvPurchase.Columns[col_name + "^current"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^current"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^current"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgvPurchase.Columns.Add(col_name + "^pre_1w", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^pre_1w"].Width = 60;
                    dgvPurchase.Columns[col_name + "^pre_1w"].Visible = false;
                    dgvPurchase.Columns[col_name + "^pre_1w"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^pre_1w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dgvPurchase.Columns[col_name + "^pre_1w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^pre_1w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgvPurchase.Columns.Add(col_name + "^pre_2w", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^pre_2w"].Width = 60;
                    dgvPurchase.Columns[col_name + "^pre_2w"].Visible = false;
                    dgvPurchase.Columns[col_name + "^pre_2w"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^pre_2w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dgvPurchase.Columns[col_name + "^pre_2w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^pre_2w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgvPurchase.Columns.Add(col_name + "^pre_3w", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^pre_3w"].Width = 60;
                    dgvPurchase.Columns[col_name + "^pre_3w"].Visible = false;
                    dgvPurchase.Columns[col_name + "^pre_3w"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^pre_3w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dgvPurchase.Columns[col_name + "^pre_3w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^pre_3w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgvPurchase.Columns.Add(col_name + "^pre_4w", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^pre_4w"].Width = 60;
                    dgvPurchase.Columns[col_name + "^pre_4w"].Visible = false;
                    dgvPurchase.Columns[col_name + "^pre_4w"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^pre_4w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dgvPurchase.Columns[col_name + "^pre_4w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^pre_4w"].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                //row.Cells["cost_unit"].Value = productList[i][4].ToString();
            }
            GetData();
        }
        private void setHearder()
        {
            if (productList != null)
            {
                for (int i = 0; i < productList.Count; i++)
                {
                    string col_name = productList[i][0] + "^" + productList[i][1] + "^" + productList[i][2] + "^" + productList[i][3];
                    dgvPurchase.Columns.Add(col_name + "^current", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^current"].Width = 60;
                    dgvPurchase.Columns[col_name + "^current"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^current"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^current"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgvPurchase.Columns.Add(col_name + "^pre_1w", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^pre_1w"].Width = 60;
                    dgvPurchase.Columns[col_name + "^pre_1w"].Visible = false;
                    dgvPurchase.Columns[col_name + "^pre_1w"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^pre_1w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dgvPurchase.Columns[col_name + "^pre_1w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^pre_1w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgvPurchase.Columns.Add(col_name + "^pre_2w", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^pre_2w"].Width = 60;
                    dgvPurchase.Columns[col_name + "^pre_2w"].Visible = false;
                    dgvPurchase.Columns[col_name + "^pre_2w"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^pre_2w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dgvPurchase.Columns[col_name + "^pre_2w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^pre_2w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgvPurchase.Columns.Add(col_name + "^pre_3w", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^pre_3w"].Width = 60;
                    dgvPurchase.Columns[col_name + "^pre_3w"].Visible = false;
                    dgvPurchase.Columns[col_name + "^pre_3w"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^pre_3w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dgvPurchase.Columns[col_name + "^pre_3w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^pre_3w"].SortMode = DataGridViewColumnSortMode.NotSortable;

                    dgvPurchase.Columns.Add(col_name + "^pre_4w", productList[i][3]);
                    dgvPurchase.Columns[col_name + "^pre_4w"].Width = 60;
                    dgvPurchase.Columns[col_name + "^pre_4w"].Visible = false;
                    dgvPurchase.Columns[col_name + "^pre_4w"].DefaultCellStyle.Font = new Font("나눔고딕", 9, FontStyle.Bold);
                    dgvPurchase.Columns[col_name + "^pre_4w"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                    dgvPurchase.Columns[col_name + "^pre_4w"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
                    dgvPurchase.Columns[col_name + "^pre_4w"].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }
        public void GetData()
        {
            //초기화
            dgvPurchase.Rows.Clear();
            if (dgvPurchase.ColumnCount < 1)
                return;
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
            Dictionary<int, int> rowDic = new Dictionary<int, int>();
            Dictionary<string, int> colDic = new Dictionary<string, int>();
            {
                for (int i = 2; i < dgvPurchase.Columns.Count; i++)
                {
                    string[] product = dgvPurchase.Columns[i].Name.Split('^');
                    //colDic.Add(product[0] + "^" + product[1] + "^" + product[2] + "^" + product[3], i);
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
                            //거래처 행 추가
                            int n = dgvPurchase.Rows.Add();
                            DataGridViewRow row = dgvPurchase.Rows[n];
                            row.Cells["company_id"].Value = companyDt.Rows[i]["company_id"].ToString();
                            row.Cells["company"].Value = companyDt.Rows[i]["company"].ToString();
                            //Dic, HeaderList 추가
                            rowDic.Add(Convert.ToInt32(companyDt.Rows[i]["company_id"].ToString()), n);
                        }


                        //매입단가 출력
                        for (int i = 2; i < dgvPurchase.Columns.Count; i = i + 5)
                        {
                            string[] product = dgvPurchase.Columns[i].Name.Split('^');
                            dgvPurchase.Columns[i + 1].HeaderText = colHeaderTxt[0];
                            dgvPurchase.Columns[i + 2].HeaderText = colHeaderTxt[1];
                            dgvPurchase.Columns[i + 3].HeaderText = colHeaderTxt[2];
                            dgvPurchase.Columns[i + 4].HeaderText = colHeaderTxt[3];
                            for (int j = 0; j < dgvPurchase.Rows.Count; j++)
                            {
                                DataGridViewRow row = dgvPurchase.Rows[j];

                                int company_id = Convert.ToInt32(row.Cells["company_id"].Value.ToString());
                                string whr = $"product = '{product[0]}'"
                                  + $" AND origin = '{product[1]}'"
                                  + $" AND sizes = '{product[2]}'"
                                  + $" AND unit = '{product[3]}'"
                                  + $" AND company_id = {company_id}";
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
                                                    if (row.Cells[i + diff_week].Value != null && double.TryParse(row.Cells[i + diff_week].Value.ToString(), out pre_price))
                                                    {
                                                        if (pre_price > Convert.ToDouble(dr[k]["purchase_price"].ToString()))
                                                        {
                                                            row.Cells[i + diff_week].Value = dr[k]["purchase_price"].ToString();
                                                            row.Cells[i + diff_week].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.Cells[i + diff_week].Value = dr[k]["purchase_price"].ToString();
                                                        row.Cells[i + diff_week].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
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
                                                    if (row.Cells[i + diff_day].Value != null && double.TryParse(row.Cells[i + diff_day].Value.ToString(), out pre_price))
                                                    {
                                                        if (pre_price > Convert.ToDouble(dr[k]["purchase_price"].ToString()))
                                                        {
                                                            row.Cells[i + diff_day].Value = dr[k]["purchase_price"].ToString();
                                                            row.Cells[i + diff_day].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.Cells[i + diff_day].Value = dr[k]["purchase_price"].ToString();
                                                        row.Cells[i + diff_day].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
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
                                                    if (row.Cells[i + diff_month].Value != null && double.TryParse(row.Cells[i + diff_month].Value.ToString(), out pre_price))
                                                    {
                                                        if (pre_price > Convert.ToDouble(dr[k]["purchase_price"].ToString()))
                                                        {
                                                            row.Cells[i + diff_month].Value = dr[k]["purchase_price"].ToString();
                                                            row.Cells[i + diff_month].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        row.Cells[i + diff_month].Value = dr[k]["purchase_price"].ToString();
                                                        row.Cells[i + diff_month].ToolTipText = purchase_date.ToString("yyyy-MM-dd");
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
                Rectangle r1 = gv.GetCellDisplayRectangle(1, -1, false);
                r1.X += 1;
                r1.Y += 1;
                r1.Width = r1.Width - 2;
                //r1.Width = r1.Width + width1 - 2;
                r1.Height = r1.Height - 2;
                e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                Color colBlue = Color.FromArgb(221, 235, 247);
                e.Graphics.FillRectangle(new SolidBrush(colBlue), r1);
                e.Graphics.DrawString("거래처", gv.ColumnHeadersDefaultCellStyle.Font, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
            }

            {
                Color col = Color.FromArgb(226, 239, 218);
                Font ft = new Font("나눔고딕", 9, FontStyle.Bold);
                StringFormat sf1 = new StringFormat();
                sf1.Alignment = StringAlignment.Center;
                sf1.LineAlignment = StringAlignment.Far;

                if (dgvPurchase.Columns.Count > 1)
                {
                    string tmpProduct = productList[0][0];
                    int sttProductIdx = 2;
                    string tmpOrigin = productList[0][1];
                    int sttOriginIdx = 2;
                    string tmpSizes = productList[0][2];
                    int sttSizesIdx = 2;
                    //규격
                    for (int i = 2; i < dgvPurchase.Columns.Count; i = i + 5)
                    {
                        string[] product = dgvPurchase.Columns[i].Name.Split('^');
                        //규격
                        if (product[2] != tmpSizes)
                        {
                            Rectangle r1 = gv.GetCellDisplayRectangle(sttSizesIdx, -1, false);
                            r1.X += 1;
                            r1.Y += 1;
                            r1.Width = r1.Width - 2;
                            for (int j = sttSizesIdx + 1; j < i; j++)
                                r1.Width += gv.GetCellDisplayRectangle(j, -1, false).Width;

                            r1.Height = r1.Height - (r1.Height / 4) - 2;
                            e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                            e.Graphics.FillRectangle(new SolidBrush(col), r1);
                            e.Graphics.DrawString(tmpSizes, ft, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, sf1);

                            sttSizesIdx = i;
                            tmpSizes = product[2];
                        }
                    }
                    //마지막까지 출력이 안된 Rectangle 확인 후 출력
                    if (sttSizesIdx < dgvPurchase.Columns.Count - 4)
                    {
                        Rectangle r1 = gv.GetCellDisplayRectangle(sttSizesIdx, -1, false);
                        r1.X += 1;
                        r1.Y += 1;
                        r1.Width = r1.Width - 2;
                        for (int j = sttSizesIdx + 1; j < dgvPurchase.Columns.Count; j++)
                            r1.Width += gv.GetCellDisplayRectangle(j, -1, false).Width;

                        r1.Height = r1.Height - (r1.Height / 4) - 2;
                        e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                        e.Graphics.FillRectangle(new SolidBrush(col), r1);
                        e.Graphics.DrawString(tmpSizes, ft, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, sf1);
                    }
                    //원산지
                    for (int i = 2; i < dgvPurchase.Columns.Count; i = i + 5)
                    {
                        string[] product = dgvPurchase.Columns[i].Name.Split('^');
                        //원산지
                        if (product[1] != tmpOrigin)
                        {
                            Rectangle r1 = gv.GetCellDisplayRectangle(sttOriginIdx, -1, false);
                            r1.X += 1;
                            r1.Y += 1;
                            r1.Width = r1.Width - 2;
                            for (int j = sttOriginIdx + 1; j < i; j++)
                                r1.Width += gv.GetCellDisplayRectangle(j, -1, false).Width;

                            r1.Height = r1.Height - (r1.Height / 4) * 2 - 2;
                            e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                            e.Graphics.FillRectangle(new SolidBrush(col), r1);
                            e.Graphics.DrawString(tmpOrigin, ft, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, sf1);

                            sttOriginIdx = i;
                            tmpOrigin = product[1];
                        }
                    }
                    //마지막까지 출력이 안된 Rectangle 확인 후 출력
                    if (sttOriginIdx < dgvPurchase.Columns.Count - 4)
                    {
                        Rectangle r1 = gv.GetCellDisplayRectangle(sttOriginIdx, -1, false);
                        r1.X += 1;
                        r1.Y += 1;
                        r1.Width = r1.Width - 2;
                        for (int j = sttOriginIdx + 1; j < dgvPurchase.Columns.Count; j++)
                            r1.Width += gv.GetCellDisplayRectangle(j, -1, false).Width;

                        r1.Height = r1.Height - (r1.Height / 4) * 2 - 2;
                        e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                        e.Graphics.FillRectangle(new SolidBrush(col), r1);
                        e.Graphics.DrawString(tmpOrigin, ft, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, sf1);
                    }
                    //품명
                    for (int i = 2; i < dgvPurchase.Columns.Count; i = i + 5)
                    {
                        string[] product = dgvPurchase.Columns[i].Name.Split('^');
                        //품명
                        if (product[0] != tmpProduct)
                        {
                            Rectangle r1 = gv.GetCellDisplayRectangle(sttProductIdx, -1, false);
                            r1.X += 1;
                            r1.Y += 1;
                            r1.Width = r1.Width - 2;
                            for (int j = sttProductIdx + 1; j < i; j++)
                                r1.Width += gv.GetCellDisplayRectangle(j, -1, false).Width;

                            r1.Height = (r1.Height / 4) - 2;
                            e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                            e.Graphics.FillRectangle(new SolidBrush(col), r1);
                            e.Graphics.DrawString(tmpProduct, ft, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);

                            sttProductIdx = i;
                            tmpProduct = product[0];
                        }
                    }
                    //마지막까지 출력이 안된 Rectangle 확인 후 출력
                    if (sttProductIdx < dgvPurchase.Columns.Count - 4)
                    {
                        Rectangle r1 = gv.GetCellDisplayRectangle(sttProductIdx, -1, false);
                        r1.X += 1;
                        r1.Y += 1;
                        r1.Width = r1.Width - 2;
                        for (int j = sttProductIdx + 1; j < dgvPurchase.Columns.Count; j++)
                            r1.Width += gv.GetCellDisplayRectangle(j, -1, false).Width;

                        r1.Height = (r1.Height / 4) - 2;
                        e.Graphics.DrawRectangle(new Pen(gv.BackgroundColor), r1);
                        e.Graphics.FillRectangle(new SolidBrush(col), r1);
                        e.Graphics.DrawString(tmpProduct, ft, new SolidBrush(gv.ColumnHeadersDefaultCellStyle.ForeColor), r1, format);
                    }
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
        private void PurchaseDashboard2_KeyDown(object sender, KeyEventArgs e)
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

        #region Button
        private void btnExcel_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가 및 재고 대시보드", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            //비밀번호 확인
            PasswordCheckManager pcm = new PasswordCheckManager(um.excel_password);
            if (!pcm.isPasswordCheck())
                return;

            ExcelDownload();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvPurchase.Rows.Clear();
            if (dgvPurchase.Columns.Count > 2)
            {
                for (int i = dgvPurchase.Columns.Count - 1; i >= 2; i--)
                {
                    dgvPurchase.Columns.Remove(dgvPurchase.Columns[i]);
                }
            }
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        #region Datagridview event
        private void dgvPurchase_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex > 1)
            {
                string[] product = dgvPurchase.Columns[e.ColumnIndex].Name.Split('^');
                string col_name = product[0] + "^" + product[1] + "^" + product[2] + "^" + product[3] + "^";
                if (!dgvPurchase.Columns[col_name + "pre_1w"].Visible
                    && !dgvPurchase.Columns[col_name + "pre_2w"].Visible
                    && !dgvPurchase.Columns[col_name + "pre_3w"].Visible
                    && !dgvPurchase.Columns[col_name + "pre_4w"].Visible)
                {
                    dgvPurchase.Columns[col_name + "pre_1w"].Visible = true;
                    dgvPurchase.Columns[col_name + "pre_2w"].Visible = true;
                    dgvPurchase.Columns[col_name + "pre_3w"].Visible = true;
                    dgvPurchase.Columns[col_name + "pre_4w"].Visible = true;
                    dgvPurchase.Columns[col_name + "pre_1w"].Width = 60;
                    dgvPurchase.Columns[col_name + "pre_2w"].Width = 60;
                    dgvPurchase.Columns[col_name + "pre_3w"].Width = 60;
                    dgvPurchase.Columns[col_name + "pre_4w"].Width = 60;
                    dgvPurchase.Columns[col_name + "current"].Width = 60;
                }
                else
                {
                    dgvPurchase.Columns[col_name + "pre_1w"].Visible = false;
                    dgvPurchase.Columns[col_name + "pre_2w"].Visible = false;
                    dgvPurchase.Columns[col_name + "pre_3w"].Visible = false;
                    dgvPurchase.Columns[col_name + "pre_4w"].Visible = false;
                    dgvPurchase.Columns[col_name + "pre_1w"].Width = 60;
                    dgvPurchase.Columns[col_name + "pre_2w"].Width = 60;
                    dgvPurchase.Columns[col_name + "pre_3w"].Width = 60;
                    dgvPurchase.Columns[col_name + "pre_4w"].Width = 60;
                    dgvPurchase.Columns[col_name + "current"].Width = 60;
                }
            }
        }
        #endregion

        #region Excel download
        private void ExcelDownload()
        {
            DataGridView dgv = dgvPurchase;
            dgv.EndEdit();
            //유효성검사
            if (um.auth_level < 50)
            {
                MessageBox.Show(this,"접근권한이 없습니다.");
                this.Activate();
                return;
            }
            else if (dgv.Rows.Count == 0 || dgv.ColumnCount <= 1)
            {
                MessageBox.Show(this,"출력할 내역이 없습니다.");
                this.Activate();
                return;
            }
            //엑셀 통합파일 만들기
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;

                if (dgv.Rows.Count > 0)
                {
                    BasicSheetsSetting(excelApp, workBook, workSheet, dgv.Rows.Count, dgv.ColumnCount - 1);
                    //컬럼 출력
                    string[] proudct = dgv.Columns[2].Name.Split('^');
                    string pTxt = proudct[0]; int pIdx = 2;
                    string oTxt = proudct[1]; int oIdx = 2;
                    string sTxt = proudct[2]; int sIdx = 2;

                    for (int i = 2; i < dgv.ColumnCount; i = i + 5)
                    { 
                        proudct = dgv.Columns[i].Name.Split('^');
                        //품명
                        if (pTxt != proudct[0])
                        {
                            workSheet.Cells[1, pIdx].Value = pTxt;
                            workSheet.Cells[1, pIdx].resize(1, i - pIdx).merge();
                            pTxt = proudct[0];
                            pIdx = i;
                        }
                        //원산지
                        if (oTxt != proudct[1])
                        {
                            workSheet.Cells[2, oIdx].Value = oTxt;
                            workSheet.Cells[2, oIdx].resize(1, i - oIdx).merge();
                            oTxt = proudct[1];
                            oIdx = i;
                        }
                        //사이즈
                        if (sTxt != proudct[2])
                        {
                            workSheet.Cells[3, sIdx].Value = sTxt;
                            workSheet.Cells[3, sIdx].resize(1, i - sIdx).merge();
                            sTxt = proudct[2];
                            sIdx = i;
                        }
                        

                        workSheet.Cells[4, i].Value = proudct[3];
                        workSheet.Cells[4, i + 1].Value = dgv.Columns[i + 1].HeaderText;
                        workSheet.Cells[4, i + 2].Value = dgv.Columns[i + 2].HeaderText;
                        workSheet.Cells[4, i + 3].Value = dgv.Columns[i + 3].HeaderText;
                        workSheet.Cells[4, i + 4].Value = dgv.Columns[i + 4].HeaderText;
                    }
                    //품명
                    workSheet.Cells[1, pIdx].Value = pTxt;
                    workSheet.Cells[1, pIdx].resize(1, dgv.ColumnCount - pIdx).merge();
                    //원산지
                    workSheet.Cells[2, oIdx].Value = oTxt;
                    workSheet.Cells[2, oIdx].resize(1, dgv.ColumnCount - oIdx).merge();
                    //사이즈
                    workSheet.Cells[3, sIdx].Value = sTxt;
                    workSheet.Cells[3, sIdx].resize(1, dgv.ColumnCount - sIdx).merge();


                    //데이터 출력
                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        workSheet.Cells[5 + i, 1].Value = dgv.Rows[i].Cells["company"].Value;
                        for (int j = 2; j < dgv.ColumnCount; j++)
                        {
                            workSheet.Cells[5 + i, j].Value = dgv.Rows[i].Cells[j].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
                this.Activate();
            }
            finally
            {
                setAutomatic(excelApp, true);
                //excelApp.ac
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
        }
        //Excel Sheet Basic
        private void BasicSheetsSetting(Excel.Application excel, Excel.Workbook wb, Excel.Worksheet wk, int rows, int columns)
        {
            workSheet.Name = "계약정보";

            //Column Width
            wk.Cells[1, 1].Value = "거래처";
            //Border Line Style
            Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[4, 1]];
            rg1.ColumnWidth = 35;
            rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
            rg1.Merge();

            //Border Line Style
            rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[4 + rows, columns]];
            rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

            for (int i = 2; i < columns; i = i + 5)
            {
                rg1 = wk.Range[wk.Cells[1, i], wk.Cells[4 + rows, i + 4]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
                rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
            }
        }

        /// <summary>
        /// 엑셀 객체 해재 메소드
        /// </summary>
        /// <param name="obj"></param>
        static void ReleaseObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);   //엑셀객체 해제
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();  //가비지 수집
            }

        }
        //Excel속도개선
        private void setAutomatic(Excel.Application excel, bool auto)
        {
            if (auto)
            {
                excel.DisplayAlerts = true;
                excel.Visible = true;
                excel.ScreenUpdating = true;
                excel.DisplayStatusBar = true;
                excel.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
                excel.EnableEvents = true;
            }
            else
            {
                excel.DisplayAlerts = false;
                excel.Visible = false;
                excel.ScreenUpdating = false;
                excel.DisplayStatusBar = false;
                excel.Calculation = Excel.XlCalculation.xlCalculationManual;
                excel.EnableEvents = false;
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

        

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }
        #endregion
    }
}
