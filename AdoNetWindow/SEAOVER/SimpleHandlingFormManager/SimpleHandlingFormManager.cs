using AdoNetWindow.Model;
using AdoNetWindow.SEAOVER.TwoLine;
using Repositories;
using Repositories.Config;
using Repositories.SEAOVER;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.SEAOVER.SimpleHandlingFormManager
{
    public partial class SimpleHandlingFormManager : Form
    {
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;
        IPriceComparisonRepository priceComparisonRepository = new PriceComparisonRepository();
        ISeaoverRepository seaoverRepository = new SeaoverRepository();
        IStockRepository stockRepository = new StockRepository();
        IBookmarkRepository bookmarkRepository = new BookmarkRepository();
        UsersModel um;
        List<SeaoverCopyModel> clipboardModel = new List<SeaoverCopyModel>();
        public SimpleHandlingFormManager(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
        }

        private void SimpleHandlingFormManager_Load(object sender, EventArgs e)
        {
            dgvProduct.Init(false);
            try
            {
                //업체별시세현황 스토어프로시져 호출
                string sttdate = DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd");
                string enddate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                string user_id = um.seaover_id;
                if (seaoverRepository.CallStoredProcedure(user_id, sttdate, enddate) == 0)
                {
                    MessageBox.Show(this,"호출 내용이 없음");
                    this.Activate();
                    return;
                }
                //품명별매출현황 스토어프로시져 호출
                stockRepository.CallStoredProcedureSTOCK(um.seaover_id, DateTime.Now.ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message);
                this.Activate();
                return;
            }

            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "품목단가표", "is_excel"))
                    btnExcel.Visible = false;
            }

            SetHeaderStyle();
            GetBookmark();
        }

        #region Method
        public void InputSeaoverProduct(List<SeaoverPriceModel> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int n = dgvProduct.Rows.Add();
                dgvProduct.Rows[n].Cells["product_code"].Value = list[i].product_code;
                dgvProduct.Rows[n].Cells["product"].Value = list[i].product;
                dgvProduct.Rows[n].Cells["real_product"].Value = list[i].product;

                dgvProduct.Rows[n].Cells["origin_code"].Value = list[i].origin_code;
                dgvProduct.Rows[n].Cells["origin"].Value = list[i].origin;
                dgvProduct.Rows[n].Cells["real_origin"].Value = list[i].origin;

                dgvProduct.Rows[n].Cells["sizes_code"].Value = list[i].sizes_code;
                dgvProduct.Rows[n].Cells["sizes"].Value = list[i].sizes;
                dgvProduct.Rows[n].Cells["real_sizes"].Value = list[i].sizes;

                dgvProduct.Rows[n].Cells["unit"].Value = list[i].unit;
                dgvProduct.Rows[n].Cells["price_unit"].Value = list[i].price_unit;
                dgvProduct.Rows[n].Cells["unit_count"].Value = list[i].unit_count;
                dgvProduct.Rows[n].Cells["seaover_unit"].Value = list[i].seaover_unit;

                dgvProduct.Rows[n].Cells["sales_price"].Value = list[i].sales_price;

            }
        }
        public void SetClipboardModel(List<SeaoverCopyModel> clipboard)
        {
            clipboardModel = clipboard;
        }
        public void InputProduct()
        {
            dgvProduct.AutoPaste(false);
            if (clipboardModel.Count > 0)
            {
                for (int i = 0; i < clipboardModel.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = dgvProduct.Rows[n];

                    row.Cells["product"].Value = clipboardModel[i].product;
                    row.Cells["real_product"].Value = clipboardModel[i].product;
                    row.Cells["product_code"].Value = clipboardModel[i].product_code;

                    row.Cells["origin"].Value = clipboardModel[i].origin;
                    row.Cells["real_origin"].Value = clipboardModel[i].origin;
                    row.Cells["origin_code"].Value = clipboardModel[i].origin_code;

                    row.Cells["sizes"].Value = clipboardModel[i].sizes;
                    row.Cells["real_sizes"].Value = clipboardModel[i].sizes;
                    row.Cells["sizes_code"].Value = clipboardModel[i].sizes_code;

                    row.Cells["unit"].Value = clipboardModel[i].unit;

                    row.Cells["pricE_unit"].Value = clipboardModel[i].price_unit;
                    row.Cells["unit_count"].Value = clipboardModel[i].unit_count;
                    row.Cells["seaover_unit"].Value = clipboardModel[i].seaover_unit;

                    row.Cells["sales_price"].Value = 0;
                    row.Cells["warehouse"].Value = "";
                    row.Cells["is_tax"].Value = clipboardModel[i].is_tax;
                }
            }
        }
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvProduct;
            dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv.Columns[0].Resizable = DataGridViewTriState.False;
            dgv.Columns[0].HeaderCell.Style.BackColor = Color.Beige;

            dgv.Columns["product_code"].Visible = false;
            dgv.Columns["origin_code"].Visible = false;
            dgv.Columns["sizes_code"].Visible = false;

            dgv.Columns["product"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["product"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["origin"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["origin"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sizes"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["sizes"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["price_unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["price_unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["unit_count"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["unit_count"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["seaover_unit"].HeaderCell.Style.BackColor = Color.FromArgb(43, 94, 170); ;
            dgv.Columns["seaover_unit"].HeaderCell.Style.ForeColor = Color.White;

            dgv.Columns["sales_price"].HeaderCell.Style.BackColor = Color.DarkGreen;
            dgv.Columns["sales_price"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["sales_price"].DefaultCellStyle.Font = new Font("나눔고딕", 10, FontStyle.Bold);
            dgv.Columns["sales_price"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);

            dgv.Columns["warehouse"].HeaderCell.Style.BackColor = Color.DarkGreen;
            dgv.Columns["warehouse"].HeaderCell.Style.ForeColor = Color.White;
            dgv.Columns["warehouse"].DefaultCellStyle.BackColor = Color.FromArgb(198, 224, 180);
        }

        private void GetSalesPrice()
        {
            DataGridView dgv = dgvProduct;
            dgv.EndEdit();
            if (dgv.Rows.Count > 0)
            {
                //단가 테이블
                DataTable dt = stockRepository.GetStockAndWarehouse(rbInStock.Checked);
                //품목순회
                int errCnt;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    errCnt = 0;
                    DataGridViewRow row = dgv.Rows[i];
                    foreach (DataGridViewCell c in dgv.Rows[i].Cells)
                    {
                        if (c.Value == null && c.ColumnIndex > 0)
                            c.Value = "";
                    }

                    //단가 표시
                    if (!string.IsNullOrEmpty(row.Cells["product"].Value.ToString()))
                    {
                        string whrStr;

                        //Where sql
                        if (row.Cells["real_product"].Value == null || string.IsNullOrEmpty(row.Cells["real_product"].Value.ToString()))
                        {
                            whrStr = $"품명코드 = '{row.Cells["product_code"].Value}'";
                            whrStr += $" AND 원산지코드 = '{row.Cells["origin_code"].Value}'";
                            whrStr += $" AND 규격코드 = '{row.Cells["sizes_code"].Value}'";
                            whrStr += $" AND 단위 = '{dgv.Rows[i].Cells["seaover_unit"].Value}'";
                        }
                        else
                        {
                            whrStr = $"품명 = '{row.Cells["real_product"].Value}'";
                            whrStr += $" AND 원산지 = '{row.Cells["real_origin"].Value}'";
                            whrStr += $" AND 규격 = '{row.Cells["real_sizes"].Value}'";
                            whrStr += $" AND 단위 = '{dgv.Rows[i].Cells["seaover_unit"].Value}'";

                        }
                    retry:
                        if (errCnt == 0)
                        {
                            DataRow[] dr = dt.Select(whrStr);
                            if (dr.Length == 0)
                            {
                                //Where sql
                                if (row.Cells["real_product"].Value == null || string.IsNullOrEmpty(row.Cells["real_product"].Value.ToString()))
                                {
                                    whrStr = $"품명코드 = '{row.Cells["product_code"].Value}'";
                                    whrStr += $" AND 원산지코드 = '{row.Cells["origin_code"].Value}'";
                                    whrStr += $" AND 규격코드 = '{row.Cells["sizes_code"].Value}'";
                                    whrStr += $" AND 단위 = '{dgv.Rows[i].Cells["unit"].Value}'";
                                }
                                else
                                {
                                    whrStr = $"품명 = '{row.Cells["real_product"].Value}'";
                                    whrStr += $" AND 원산지 = '{row.Cells["real_origin"].Value}'";
                                    whrStr += $" AND 규격 = '{row.Cells["real_sizes"].Value}'";
                                    whrStr += $" AND 단위 = '{dgv.Rows[i].Cells["unit"].Value}'";
                                }
                            }



                            if (dr.Length > 0)
                            {
                                DataTable tmpDt = dr.CopyToDataTable();

                                //해당 Row 선택
                                if (tmpDt.Rows.Count > 1)
                                {
                                    if (cbStockMax.Checked)
                                    {
                                        int max_index = -1;
                                        /*double stock = 0;
                                        for (int j = 0; j < tmpDt.Rows.Count; j++)
                                        {
                                            double tmp_stock;
                                            if (!double.TryParse(tmpDt.Rows[j]["재고수"].ToString(), out tmp_stock))
                                                tmp_stock = 0;
                                            double tmp_reserved;
                                            if (!double.TryParse(tmpDt.Rows[j]["예약수"].ToString(), out tmp_reserved))
                                                tmp_reserved = 0;
                                            //비교
                                            if (stock < tmp_stock - tmp_reserved)
                                            {
                                                max_index = j;
                                                stock = tmp_stock - tmp_reserved;
                                            }
                                        }

                                        if (max_index == -1)*/
                                            max_index = 0;

                                        double sales_price;
                                        if (!double.TryParse(tmpDt.Rows[max_index]["매출단가"].ToString(), out sales_price))
                                            sales_price = 0;

                                        row.Cells["sales_price"].Value = sales_price.ToString("#,##0");
                                        row.Cells["warehouse"].Value = tmpDt.Rows[max_index]["창고"].ToString();
                                        row.Cells["isTax"].Value = tmpDt.Rows[max_index]["부가세유무"].ToString();
                                        row.Cells[0].Value = true;


                                    }
                                    else
                                    { 
                                        dgv.CurrentRow.Selected = false;
                                        dgv.Rows[i].Selected = true;
                                        dgv.FirstDisplayedScrollingRowIndex = i;
                                        //단가리스트 생성
                                        PriceSelectManager pm = new PriceSelectManager();
                                        Point p = dgv.PointToScreen(dgv.GetCellDisplayRectangle(0, i, false).Location);
                                        p = new Point(p.X, p.Y + dgv.Rows[i].Height);

                                        pm.StartPosition = FormStartPosition.Manual;
                                        string[] price = pm.Manager2(tmpDt, p);
                                        row.Cells["sales_price"].Value = Convert.ToInt32(price[4]).ToString("#,##0");
                                        row.Cells["warehouse"].Value = price[5].ToString();
                                        row.Cells[0].Value = true;
                                    }
                                }
                                else if (tmpDt.Rows.Count == 1)
                                {
                                    double sales_price;
                                    if(!double.TryParse(tmpDt.Rows[0]["매출단가"].ToString(), out sales_price))
                                        sales_price = 0;


                                    row.Cells["sales_price"].Value = sales_price.ToString("#,##0");
                                    row.Cells["warehouse"].Value = tmpDt.Rows[0]["창고"].ToString();
                                    row.Cells["isTax"].Value = tmpDt.Rows[0]["부가세유무"].ToString();
                                    row.Cells[0].Value = true;
                                }   
                            }
                            else
                            {
                                whrStr = $"품명 = '{row.Cells["product"].Value}'";
                                whrStr += $" AND 원산지 = '{row.Cells["origin"].Value}'";
                                whrStr += $" AND 규격 = '{row.Cells["sizes"].Value}'";
                                whrStr += $" AND 단위 = '{dgv.Rows[i].Cells["unit"].Value}'";
                                errCnt++;
                                goto retry;

                                row.Cells["warehouse"].Value = "";
                                row.Cells[0].Value = true;
                            }
                        }
                    }


                    //과세표시
                }
            }
        }
        private void GetBookmark()
        {
            lvBookmark.Items.Clear();

            List<BookmarkModel> model = new List<BookmarkModel>();
            model = bookmarkRepository.GetBookmark(um.user_id, 2, txtFormname1.Text, txtManager1.Text);

            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = model[i].form_id.ToString();
                    lvi.SubItems.Add(model[i].form_name.ToString());
                    lvi.SubItems.Add(model[i].edit_user.ToString());

                    lvBookmark.Items.Add(lvi);
                }
            }
        }
        //품목서 리스트 클릭후 데이터 불러오기
        private void GetFormData(int idx, string id)
        {
            dgvProduct.Rows.Clear();
            //저장값 불러오기
            List<FormDataModel> model = seaoverRepository.GetFormData("2", id);
            DataTable productDt = seaoverRepository.GetProductByCode("", "", "", "");
            if (model.Count > 0)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    FormDataModel m = model[i];
                    //Row 추가
                    int n = dgvProduct.Rows.Add();

                    dgvProduct.Rows[n].Cells["product"].Value = m.product;
                    dgvProduct.Rows[n].Cells["origin"].Value = m.origin;
                    dgvProduct.Rows[n].Cells["sizes"].Value = m.sizes;
                    dgvProduct.Rows[n].Cells["product_code"].Value = m.product_code;
                    dgvProduct.Rows[n].Cells["origin_code"].Value = m.origin_code;
                    dgvProduct.Rows[n].Cells["sizes_code"].Value = m.sizes_code;
                    dgvProduct.Rows[n].Cells["unit"].Value = m.unit;
                    dgvProduct.Rows[n].Cells["price_unit"].Value = m.price_unit;
                    dgvProduct.Rows[n].Cells["unit_count"].Value = m.unit_count;
                    dgvProduct.Rows[n].Cells["seaover_unit"].Value = m.seaover_unit;

                    dgvProduct.Rows[n].Cells["sales_price"].Value = m.sales_price.ToString("#,##0");
                    dgvProduct.Rows[n].Cells["warehouse"].Value = "";

                    dgvProduct.Rows[n].Cells["isTax"].Value = m.is_tax;


                    if (productDt.Rows.Count > 0)
                    {
                        string whr = "품목코드 = '" + m.product_code + "' AND 원산지코드 = '" + m.origin_code + "' AND 규격코드 = '" + m.sizes_code + "'";
                        DataRow[] dr = productDt.Select(whr);
                        if (dr.Length > 0)
                        {
                            dgvProduct.Rows[n].Cells["real_product"].Value = dr[0]["품명"].ToString();
                            dgvProduct.Rows[n].Cells["real_origin"].Value = dr[0]["원산지"].ToString();
                            dgvProduct.Rows[n].Cells["real_sizes"].Value = dr[0]["규격"].ToString();
                        }
                    }
                }
            }
        }
        #endregion

        #region Key event
        private void SimpleHandlingFormManager_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetSalesPrice();
                        break;
                    case Keys.E:
                        btnExcel.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.V:
                        InputProduct();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        cbStockMax.Checked = !cbStockMax.Checked;
                        break;
                    case Keys.F5:
                        dgvProduct.Rows.Clear();
                        break;
                    case Keys.F4:
                        btnGetProduct.PerformClick();
                        break;
                }
            }
        }
        private void SimpleHandlingFormManager_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.V:
                        InputProduct();
                        break;
                }
            }

        }
        private void txtFormname1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GetBookmark();
            }
        }
        #endregion

        #region Excel download
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void ExcelDownload()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "씨오버", "품목단가표", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            DataGridView dgv = dgvProduct;
            dgv.EndEdit();
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;

                if (dgv.Rows.Count > 1)
                {
                    BasicSheetsSetting(excelApp, workBook, workSheet, dgv.Rows.Count);
                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        workSheet.Cells[4 + i, 1].Value = i + 1;
                        workSheet.Cells[4 + i, 2].Value = dgv.Rows[i].Cells["product"].Value.ToString();
                        workSheet.Cells[4 + i, 3].Value = dgv.Rows[i].Cells["origin"].Value.ToString();
                        workSheet.Cells[4 + i, 4].Value = " " + dgv.Rows[i].Cells["sizes"].Value.ToString();
                        workSheet.Cells[4 + i, 5].Value = dgv.Rows[i].Cells["unit"].Value.ToString();
                        workSheet.Cells[4 + i, 6].Value = dgv.Rows[i].Cells["price_unit"].Value.ToString();
                        workSheet.Cells[4 + i, 7].Value = dgv.Rows[i].Cells["unit_count"].Value.ToString();
                        workSheet.Cells[4 + i, 8].Value = dgv.Rows[i].Cells["seaover_unit"].Value.ToString();
                        workSheet.Cells[4 + i, 9].Value = dgv.Rows[i].Cells["sales_price"].Value.ToString();
                        workSheet.Cells[4 + i, 10].Value = dgv.Rows[i].Cells["warehouse"].Value.ToString();
                        workSheet.Cells[4 + i, 11].Value = dgv.Rows[i].Cells["isTax"].Value.ToString();
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
        private void BasicSheetsSetting(Excel.Application excel, Excel.Workbook wb, Excel.Worksheet wk, int rows)
        {
            workSheet.Name = "품목단가표";

            Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[2, 11]];
            rg1.Merge();
            rg1.Font.Size = 20;
            rg1.Font.Bold = true;
            rg1.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rg1.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
            rg1.Value = "품 목 단 가 표";
            
            //Column Width
            wk.Columns["A"].ColumnWidth = 5;       //번호
            wk.Cells[3, 1].Value = "번호";
            wk.Columns["B"].ColumnWidth = 30;      //품명
            wk.Cells[3, 2].Value = "품명";
            wk.Columns["C"].ColumnWidth = 10.5;    //원산지
            wk.Cells[3, 3].Value = "원산지";
            wk.Columns["D"].ColumnWidth = 10.5;    //규격
            wk.Cells[3, 4].Value = "규격";
            wk.Columns["E"].ColumnWidth = 10.5;    //단위
            wk.Cells[3, 5].Value = "단위";

            wk.Columns["F"].ColumnWidth = 10.5;    //단위
            wk.Cells[3, 6].Value = "가격단위";

            wk.Columns["G"].ColumnWidth = 10.5;    //단위
            wk.Cells[3, 7].Value = "단위수량";

            wk.Columns["H"].ColumnWidth = 10.5;    //단위
            wk.Cells[3, 8].Value = "S단위";


            wk.Columns["I"].ColumnWidth = 13;      //단가
            wk.Cells[3, 9].Value = "단가";
            wk.Columns["J"].ColumnWidth = 30;      //창고
            wk.Cells[3, 10].Value = "창고";
            wk.Columns["K"].ColumnWidth = 10.5;    //단위
            wk.Cells[3, 11].Value = "과세";

            //Font Style
            wk.Columns["I"].HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            wk.Columns["I"].NumberFormatLocal = "#,##0";

            //Border Line Style
            rg1 = wk.Range[wk.Cells[3, 1], wk.Cells[4 + rows, 11]];
            rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
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

        #region Button
        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelDownload();
        }

        private void btnSearching_Click(object sender, EventArgs e)

        {
            GetSalesPrice();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvProduct.Rows.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnGetProduct_Click(object sender, EventArgs e)
        {
            GetProductList gp = new GetProductList(um, this);
            gp.Show();
        }
        #endregion

        #region Datagridview event
        private void lvBookmark_MouseClick(object sender, MouseEventArgs e)
        {
            //품목서 클릭
            if (e.Button.Equals(MouseButtons.Left))
            {
                if (lvBookmark.SelectedItems.Count != 0)
                {
                    int idx = lvBookmark.SelectedItems[0].Index;
                    string id = lvBookmark.Items[idx].SubItems[0].Text;
                    GetFormData(idx, id);
                }
            }
        }
        private void dgvProduct_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                if (e.ColumnIndex == 0)
                {
                    bool isChecked;
                    try
                    {
                        isChecked = Convert.ToBoolean(dgvProduct.Rows[0].Cells[0].Value);
                    }
                    catch
                    {
                        isChecked = false;
                    }
                    for (int i = 0; i < dgvProduct.Rows.Count; i++)
                    {
                        dgvProduct.Rows[i].Cells[0].Value = !isChecked;


                    }
                }
            }
        }
        private void dgvProduct_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion

        private void btnDailyBusinessCopy_Click(object sender, EventArgs e)
        {
            string clipboardTxt = "";
            if(dgvProduct.Rows.Count > 0)
            {
                for (int i = 0; i < dgvProduct.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvProduct.Rows[i];

                    string txt = "	" + row.Cells["origin"].Value.ToString().Replace("\n", "").Replace("\t", "").Trim();
                    txt += "	" + row.Cells["product"].Value.ToString().Replace("\n", "").Replace("\t", "").Trim();
                    txt += "	" + row.Cells["sizes"].Value.ToString().Replace("\n", "").Replace("\t", "").Trim();
                    txt += "	" + row.Cells["seaover_unit"].Value.ToString().Replace("\n", "").Replace("\t", "").Trim();
                    txt += "	1";
                    txt += "	" + row.Cells["sales_price"].Value.ToString().Replace("\n", "").Replace("\t", "").Trim();
                    txt += "	" + row.Cells["sales_price"].Value.ToString().Replace("\n", "").Replace("\t", "").Trim();
                    txt += "	" + row.Cells["warehouse"].Value.ToString().Replace("\n", "").Replace("\t", "").Trim();

                    clipboardTxt += "\r\n" + txt.Trim();
                }
            }

            if (!string.IsNullOrEmpty(clipboardTxt.Trim()))
                Clipboard.SetDataObject((object)clipboardTxt.Trim());

            MessageBox.Show(this,"영업일보 '원산지' 컬럼에서 붙혀넣기 하시면 됩니다!");
            this.Activate();
        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
