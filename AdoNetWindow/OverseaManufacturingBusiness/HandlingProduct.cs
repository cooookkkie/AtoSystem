using AdoNetWindow.Common;
using AdoNetWindow.Model;
using Repositories.Config;
using Repositories.OverseaManufacturing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.OverseaManufacturingBusiness
{
    public partial class HandlingProduct : Form
    {
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        IOverseaManufacturingRepository overseaManufacturingRepository = new OverseaManufacturingRepository();
        UsersModel um;
        Dictionary<string, string> companyDic;
        int viewType;
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();

        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        public HandlingProduct(UsersModel um, int viewType, string sttdate, string enddate, string product_kor = "", string product_eng = "")
        {
            InitializeComponent();
            this.um = um;
            this.viewType = viewType;

            this.txtSttdate.Text = sttdate;
            this.txtEnddate.Text = enddate;

            GetData(product_kor, product_eng);
        }

        public HandlingProduct(UsersModel um, int viewType, Dictionary<string, string> companyDic, string sttdate, string enddate)
        {
            InitializeComponent();
            this.um = um;
            this.companyDic = companyDic;
            this.viewType = viewType;

            this.txtSttdate.Text = sttdate;
            this.txtEnddate.Text = enddate;

            GetData();
        }
        private void HandlingProduct_Load(object sender, EventArgs e)
        {
            if (dgvProduct.ColumnCount > 0)
            {
                foreach (DataGridViewColumn col in dgvProduct.Columns)
                    col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "해외제조업소 및 수입업체 수출입", "is_excel"))
                    btnExcel.Visible = false;
                if (!common.CheckAuthority(authorityDt, "수입관리", "해외제조업소 및 수입업체 수출입", "is_print"))
                    btnPreview.Visible = false;
            }

        }

        #region Method
        private void GetData(string product_kor = "", string product_eng = "")
        {
            dgvProduct.Rows.Clear();

            List<string> companyList=  new List<string>();
            if (companyDic != null)
            {
                foreach (string company in companyDic.Keys)
                    companyList.Add(company);
            }

            DataTable productDt = overseaManufacturingRepository.GetHandlingProduct(viewType, companyList, cbSortType.Text, txtSttdate.Text, txtEnddate.Text, product_kor, product_eng);
            if (productDt.Rows.Count > 0)
            {
                for (int i = 0; i < productDt.Rows.Count; i++)
                {
                    int n = dgvProduct.Rows.Add();
                    DataGridViewRow row = dgvProduct.Rows[n];

                    row.Cells["company"].Value = productDt.Rows[i]["company"].ToString();
                    row.Cells["pname_kor"].Value = productDt.Rows[i]["pname_kor"].ToString();
                    row.Cells["pname_eng"].Value = productDt.Rows[i]["pname_eng"].ToString();
                    row.Cells["m_country"].Value = productDt.Rows[i]["m_country"].ToString();
                    row.Cells["e_country"].Value = productDt.Rows[i]["e_country"].ToString();
                    int qty;
                    if (!int.TryParse(productDt.Rows[i]["qty"].ToString(), out qty))
                        qty = 0;
                    row.Cells["qty"].Value = qty.ToString("#,##0");
                    DateTime updatetime;
                    if(DateTime.TryParse(productDt.Rows[i]["updatetime"].ToString(), out updatetime))
                        row.Cells["current_date"].Value = updatetime.ToString("yyyy-MM-dd");
                }
            }
        }
        #endregion

        #region Button, Combobox
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnSttDateCalendar_Click(object sender, EventArgs e)
        {
            Common.Calendar cd = new Common.Calendar();
            string sDate = cd.GetDate(true);
            if (sDate != null)
            {
                DateTime tmpDate = Convert.ToDateTime(sDate);
                TextBox tb = (TextBox)sender;
                tb.Text = tmpDate.ToString("yyyy-MM-dd");
            }
        }
        private void cbSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelDownload();
        }
        #endregion

        #region Key event
        private void HandlingProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        btnSearch.PerformClick();
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
                    case Keys.P:
                        btnPreview.PerformClick();
                        break;
                }
            }
        }
        #endregion

        #region Excel download
        private void ExcelDownload()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "해외제조업소 및 수입업체 수출입", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            //비밀번호 확인
            PasswordCheckManager pcm = new PasswordCheckManager(um.excel_password);
            if (!pcm.isPasswordCheck())
                return;

            DataGridView dgv = dgvProduct;
            dgv.EndEdit();
            //유효성검사
            if (um.auth_level < 50)
            {
                messageBox.Show(this, "접근권한이 없습니다.");
                this.Activate();
                return;
            }
            else if (dgv.Rows.Count == 0 || dgv.ColumnCount <= 1)
            {
                messageBox.Show(this, "출력할 내역이 없습니다.");
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
                    BasicSheetsSetting(excelApp, workBook, workSheet, dgv.Rows.Count);
                    for (int i = 0; i < dgv.RowCount; i++)
                    {
                        workSheet.Cells[i + 2, 1].Value = dgv.Rows[i].Cells["company"].Value.ToString();
                        workSheet.Cells[i + 2, 2].Value = dgv.Rows[i].Cells["pname_kor"].Value.ToString();
                        workSheet.Cells[i + 2, 3].Value = dgv.Rows[i].Cells["pname_eng"].Value.ToString();
                        workSheet.Cells[i + 2, 4].Value = dgv.Rows[i].Cells["m_country"].Value.ToString();
                        workSheet.Cells[i + 2, 5].Value = dgv.Rows[i].Cells["e_country"].Value.ToString();
                        workSheet.Cells[i + 2, 6].Value = dgv.Rows[i].Cells["qty"].Value.ToString();
                        workSheet.Cells[i + 2, 7].Value = dgv.Rows[i].Cells["current_date"].Value.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                messageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
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
            workSheet.Name = "취급정보";

            //Column Width
            wk.Cells[1, 1].Value = "거래처";
            wk.Cells[1, 2].Value = "품명(한글)";
            wk.Cells[1, 3].Value = "품명(영문)";
            wk.Cells[1, 4].Value = "제조국";
            wk.Cells[1, 5].Value = "수출국";
            wk.Cells[1, 6].Value = "수량";
            wk.Cells[1, 7].Value = "최근일자";
            //Border Line Style
            Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, 7]];
            rg1.ColumnWidth = 30;
            rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
            rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

            //Border Line Style
            rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1 + rows, 7]];
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

        #region Print 
        private void btnPreview_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "해외제조업소 및 수입업체 수출입", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            if (dgvProduct.Rows.Count == 0)
            {
                messageBox.Show(this, "출력할 내역이 없습니다.");
                this.Activate();
                return;
            }

            cnt = 0;
            pageNo = 1;
            this.printDocument1 = new PrintDocument();
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.DefaultPageSettings.Landscape = true;

            int pages = common.GetPageCount(printDocument1);

            Common.PrintManager.PrintManager pm = new Common.PrintManager.PrintManager(this, printDocument1, pages);
            pm.ShowDialog();
        }

        public void InitVariable()
        {
            cnt = 0;
            pageNo = 1;
        }

        //양식만들기
        int cnt = 0;
        int pageNo = 1;
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            DataGridView dgv = this.dgvProduct;

            int dialogWidth = printDocument1.DefaultPageSettings.PaperSize.Height - 40;    //페이지 전체넓이 printPreivew.Width  (가로모드라 반대로)
            int dialogHeight = printDocument1.DefaultPageSettings.PaperSize.Width;    //페이지 전체넓이 printPreivew.Height  (가로모드라 반대로)

            StringFormat sf = new StringFormat();  //컬럼안에 있는 값들 가운데로 정렬하기 위해서.
            sf.Alignment = StringAlignment.Center;
            int width, width1;             //width는 시작점 위치, width1은 datagrid 1개의 컬럼 가로길이
            int startWidth = 10;                   //시작 x좌표
            int startHeight = 140;                 //시작 y좌표
            int avgHeight = dgv.Rows[0].Height - 5;    //컬럼 하나의 높이
            int i, j;                              //반복문용 변수
            int temp = 0;                          //row 개수 세어줄것, cnt의 역활

            //Title, Footer
            e.Graphics.DrawString("취급품목", new Font("Arial", 20, FontStyle.Bold), Brushes.Black, dialogWidth / 2 - 50, 40);
            e.Graphics.DrawString("인쇄일자 :  " + DateTime.Now.ToString("yyyy-MM-dd"), new Font("Arial", 11), Brushes.Black, dialogWidth - 200, 80);
            e.Graphics.DrawString("페이지번호 : " + pageNo, new Font("Arial", 11), Brushes.Black, dialogWidth - 200, 100);

            //column 전체 넓이
            int column_width = 0;
            int column_count = 0;
            for (i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv.Columns[i].Visible == true)
                {
                    column_width += dgv.Columns[i].Width;
                    column_count += 1;
                }
            }
            int adjust_font_size = (int)(column_count / 10);
            //columnCount는 일정
            double width_rate;            //컬럼 넓이비율
            double pre_width_rate;        //이전 컬럼 넓이비율
            int pre_idx = 0;
            int tmp = 0;
            for (i = 0; i < dgv.ColumnCount; i++)
            {
                if (dgv.Columns[i].Visible == true)
                {
                    if (tmp == 0)
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = 0;
                        width1 = (int)(((double)dialogWidth / 100) * width_rate);
                    }
                    else if (tmp > 0 && i <= dgv.ColumnCount - 2)
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;

                        //width = dgv.Columns[i - 1].Width + tempWidth;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                        width1 = (int)(((double)dialogWidth / 100) * width_rate);
                    }
                    else
                    {
                        width_rate = ((double)dgv.Columns[i].Width / column_width) * 100;
                        pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;
                        //width = dgv.Columns[i - 1].Width + tempWidth;
                        //width1 = dgv.Columns[i].Width + tempWidth;
                        width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                        width1 = (int)(((double)dialogWidth / 100) * width_rate);
                    }

                    RectangleF drawRect = new RectangleF((float)(startWidth + width), (float)startHeight, (float)width1, avgHeight + 5);
                    e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width), (float)startHeight - 4, (float)width1, avgHeight + 5);
                    // e.Graphics.FillRectangle(Brushes.LightGray, (float)(startWidth + width), (float)startHeight, (float)width, avgHeight);
                    e.Graphics.DrawString(dgv.Columns[i].HeaderText, new Font("Arial", 8 - adjust_font_size, FontStyle.Bold), Brushes.Black, drawRect, sf);

                    startWidth += width;
                    tmp += 1;
                    pre_idx = i;
                }
            }

            startHeight += avgHeight + 1;
            for (i = cnt; i < dgv.RowCount; i++)
            {
                tmp = 0;
                pre_idx = 0;
                startWidth = 10;  //다시 초기화
                for (j = 0; j < dgv.ColumnCount; j++)
                {
                    if (dgv.Columns[j].Visible == true && dgv.Rows[i].Cells[j].Visible == true)
                    {
                        if (tmp == 0)
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            //width = 0;
                            //width1 = dgv.Columns[j].Width + tempWidth;
                            width = 0;
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);

                        }
                        else if (tmp > 0 && j <= dgv.ColumnCount - 2)
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;

                            //width = dgv.Columns[j - 1].Width + tempWidth;
                            //width1 = dgv.Columns[j].Width + tempWidth;
                            width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);
                        }
                        else
                        {
                            width_rate = ((double)dgv.Columns[j].Width / column_width) * 100;
                            pre_width_rate = ((double)dgv.Columns[pre_idx].Width / column_width) * 100;
                            //width = dgv.Columns[i - 1].Width + tempWidth;
                            //width1 = dgv.Columns[i].Width + tempWidth;
                            width = (int)(((double)dialogWidth / 100) * pre_width_rate);
                            width1 = (int)(((double)dialogWidth / 100) * width_rate);
                        }
                        RectangleF drawRect = new RectangleF((float)(startWidth + width), (float)startHeight + 2, (float)width1, avgHeight);
                        e.Graphics.DrawRectangle(Pens.Black, (float)(startWidth + width), (float)startHeight, (float)width1, avgHeight);
                        e.Graphics.DrawString(dgv.Rows[i].Cells[j].FormattedValue.ToString(), new Font("Arial", 8 - adjust_font_size, FontStyle.Bold), Brushes.Black, drawRect, sf);

                        startWidth += width;
                        tmp += 1;
                        pre_idx = j;
                    }
                }

                startHeight += avgHeight;
                temp++;
                cnt++;

                //한페이지당 35줄
                if (temp % 35 == 0)
                {
                    e.HasMorePages = true;
                    pageNo++;
                    return;
                }
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
                    
                    m.Items.Add("같은 품목을 취급하는 해외제조업소(영문)");
                    m.Items.Add("같은 품목을 취급하는 해외제조업소(한글)");
                    m.Items.Add("같은 품목을 취급하는 수입업체(영문)");
                    m.Items.Add("같은 품목을 취급하는 수입업체(한글)");

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvProduct, e.Location);
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
                    switch (e.ClickedItem.Text)
                    {
                        case "같은 품목을 취급하는 해외제조업소(영문)":
                            {
                                HandlingProduct hp = new HandlingProduct(um, 3, txtSttdate.Text, txtEnddate.Text, "", dr.Cells["pname_eng"].Value.ToString());
                                hp.Show();
                            }
                            break;
                        case "같은 품목을 취급하는 해외제조업소(한글)":
                            {
                                HandlingProduct hp = new HandlingProduct(um, 3, txtSttdate.Text, txtEnddate.Text, dr.Cells["pname_kor"].Value.ToString(), "");
                                hp.Show();
                            }
                            break;
                        case "같은 품목을 취급하는 수입업체(영문)":
                            {
                                HandlingProduct hp = new HandlingProduct(um, 4, txtSttdate.Text, txtEnddate.Text, dr.Cells["pname_kor"].Value.ToString(), "");
                                hp.Show();
                            }
                            break;
                        case "같은 품목을 취급하는 수입업체(한글)":
                            {
                                HandlingProduct hp = new HandlingProduct(um, 4, txtSttdate.Text, txtEnddate.Text, "", dr.Cells["pname_eng"].Value.ToString());
                                hp.Show();
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
            switch (e.KeyCode)
            {
                //우클릭 단축키
                case Keys.S:

                    break;
                case Keys.D:

                    break;
            }
        }



        #endregion

        private void dgvProduct_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgvProduct.SelectedRows.Count <= 1)
                {
                    dgvProduct.ClearSelection();
                    dgvProduct.Rows[e.RowIndex].Selected = true;
                }
            }
        }

        private void dgvProduct_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0)
            {
                int total_cnt = 0;
                foreach (DataGridViewCell cell in dgvProduct.SelectedCells)
                {
                    int cnt;
                    if (cell.Value == null || !int.TryParse(cell.Value.ToString(), out cnt))
                        cnt = 0;
                    total_cnt += cnt;
                }
                lbCount.Text = total_cnt.ToString("#,##0");    
            }
        }
    }
}
