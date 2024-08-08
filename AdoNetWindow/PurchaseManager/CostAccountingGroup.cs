using AdoNetWindow.Common;
using AdoNetWindow.Common.PrintManager;
using AdoNetWindow.Model;
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
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.PurchaseManager
{
    public partial class CostAccountingGroup : Form
    {
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        public List<DataGridViewRow> clipboard = new List<DataGridViewRow> ();
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox ();
        double customRate;
        UsersModel um;

        //Excel
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;

        public CostAccountingGroup(UsersModel uModel, double trq)
        {
            InitializeComponent();
            um = uModel;
            txtTrq.Text = trq.ToString("#,##0");
        }

        private void CostAccountingGroup_Load(object sender, EventArgs e)
        {
            //환율가져오기
            customRate = common.GetExchangeRateKEBBank("USD");
            txtExchangeRate.Text = customRate.ToString("#,##0.00");

            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_excel"))
                    btnExcel.Visible = false;
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_print"))
                    btnPreview.Visible = false;
            }
        }

        #region Method
        
        public void SetBatchInput(List<DataGridViewRow> product)
        {
            foreach (CostAccountingGroupUnit con in pnMain.Controls)
            { 
                DataGridView dgv = con.GetDatagridview();
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    for (int j = 0; j < product.Count; j++)
                    {
                        if (dgv.Rows[i].Cells["product"].Value.ToString() == product[j].Cells["product"].Value.ToString()
                            && dgv.Rows[i].Cells["origin"].Value.ToString() == product[j].Cells["origin"].Value.ToString()
                            && dgv.Rows[i].Cells["sizes"].Value.ToString() == product[j].Cells["sizes"].Value.ToString()
                            && dgv.Rows[i].Cells["sizes2"].Value.ToString() == product[j].Cells["sizes2"].Value.ToString()
                            && dgv.Rows[i].Cells["box_weight"].Value.ToString() == product[j].Cells["box_weight"].Value.ToString()
                            && dgv.Rows[i].Cells["cost_unit"].Value.ToString() == product[j].Cells["cost_unit"].Value.ToString())
                        {
                            dgv.Rows[i].Cells["assort"].Value = product[j].Cells["assort"].Value;

                            break;
                        }
                    }
                }
            }
            
        }
        public void DeleteControl(CostAccountingGroupUnit cagu)
        {
            foreach (CostAccountingGroupUnit con in pnMain.Controls)
            {
                if (con == cagu)
                    pnMain.Controls.Remove(con);
            }
            RefreshControlsId();
        }
        #endregion

        #region Button
        private void btnBatchInput_Click(object sender, EventArgs e)
        {
            List<DataGridViewRow> product = new List<DataGridViewRow>();

            foreach (CostAccountingGroupUnit con in pnMain.Controls)
            {
                DataGridView dgv = con.GetDatagridview();

                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    product.Add(dgv.Rows[i]);
                }
            }

            if (product.Count > 0)
            {
                ProductAssortBatchInput pabi = new ProductAssortBatchInput(um, this);
                pabi.SetProduct(product);
                pabi.ShowDialog();
            }
            else
            {
                MessageBox.Show(this, "입력할 내역이 없습니다.");
                this.Activate();
        }
        }
        private void btnProduct_Click(object sender, EventArgs e)
        {
            CostAccounting ca = new CostAccounting(um, this);
            ca.Show();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!double.TryParse(txtExchangeRate.Text, out customRate))
            {
                customRate = common.GetExchangeRateKEBBank("USD");
                txtExchangeRate.Text = customRate.ToString("#,##0.00");
            }
            double trq;
            if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                trq = 0;

            int cnt = pnMain.Controls.Count + 1;
            CostAccountingGroupUnit cagu = new CostAccountingGroupUnit(um, this, customRate, trq);
            cagu.SetId(cnt);
            pnMain.Controls.Add(cagu);
        }

        private void RefreshControlsId()
        {
            int cnt = 1;
            foreach (CostAccountingGroupUnit con in pnMain.Controls)
            {
                con.SetId(cnt);
                cnt++;
            }
        }

        public void SetProduct(List<DataGridViewRow> product)
        {
            clipboard = product;
            foreach (CostAccountingGroupUnit con in pnMain.Controls)
            {
                con.setPaste(false);
            }
        }
        bool isPurchasePrice = true;
        public void SetProductPerCompany(List<DataGridViewRow> product, bool isPurchasePrice, double exchange_rate)
        {
            customRate = exchange_rate;
            txtExchangeRate.Text = customRate.ToString("#,##0.00");
            this.isPurchasePrice = isPurchasePrice;
            clipboard = product;

            if (clipboard.Count > 0)
            { 
                //거래처 중복삭제
                string[] companyList = new string[clipboard.Count];
                for (int i = 0; i < clipboard.Count; i++)
                { 
                    companyList[i] = clipboard[i].Cells["company"].Value.ToString();
                }
                companyList = companyList.Distinct().ToArray();

                //순회출력
                for (int i = 0; i < companyList.Length; i++)
                {
                    /*if (!double.TryParse(txtExchangeRate.Text, out customRate))
                    {
                        customRate = common.GetExchangeRateKEBBank("USD");
                        txtExchangeRate.Text = customRate.ToString("#,##0.00");
                    }*/
                    double trq;
                    if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                        trq = 0;

                    int cnt = pnMain.Controls.Count + 1;
                    CostAccountingGroupUnit cagu = new CostAccountingGroupUnit(um, this, customRate, trq);
                    cagu.SetId(cnt);
                    pnMain.Controls.Add(cagu);


                    List<DataGridViewRow> productPerCompany = new List<DataGridViewRow>();
                    for (int j = 0; j < clipboard.Count; j++)
                    {
                        if (clipboard[j].Cells["company"].Value.ToString() == companyList[i])
                        {
                            productPerCompany.Add(clipboard[j]);
                        }
                    }
                    cagu.SetProduct(productPerCompany, isPurchasePrice);
                }
            }            
        }
        

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            pnMain.Controls.Clear();    
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void cbSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int sortType = 1;
            switch (cbSortType.Text)
            {
                case "품명+원산지+규격":
                    sortType = 1;
                    break;
                case "거래처+품명+원산지+규격":
                    sortType = 2;
                    break;
                case "품명+원산지+규격+오퍼가":
                    sortType = 3;
                    break;
                case "오퍼가+품명+원산지+규격":
                    sortType = 4;
                    break;
                case "품명+원산지+규격+오퍼일자":
                    sortType = 5;
                    break;
                case "오퍼일자+품명+원산지+규격":
                    sortType = 6;
                    break;
            }

            foreach (CostAccountingGroupUnit con in pnMain.Controls)
            {
                con.Sorting(sortType);
            }
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            GetExeclColumn();
        }

        
        #endregion

        #region Key event
        private void CostAccountingGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        btnAdd.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                }
            }
            else if(e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.P:
                        btnPreview.PerformClick();
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F4:
                        btnProduct.PerformClick();
                        break;
                    case Keys.F6:
                        rbPurchaseprice.Checked = !rbPurchaseprice.Checked;
                        rbSalesPrice.Checked = !rbSalesPrice.Checked;
                        break;
                }
            }
        }
        private void txtExchangeRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double exchangeRate;
                if (!double.TryParse(txtExchangeRate.Text, out exchangeRate))
                {
                    MessageBox.Show(this, "환율 값을 확인해주세요.");
                    this.Activate();
                    return;
                }
                txtExchangeRate.Text = exchangeRate.ToString("#,##0.00");

                double trq;
                if (!double.TryParse(txtTrq.Text.Replace(",", ""), out trq))
                {
                    MessageBox.Show(this, "TRQ 값을 확인해주세요.");
                    this.Activate();
                    return;
                }
                txtTrq.Text = trq.ToString("#,##0");
                //반영
                foreach (CostAccountingGroupUnit con in pnMain.Controls)
                {
                    con.SetExchangeRateTrq(exchangeRate, trq);
                }
            }
        }
        #endregion

        #region Excel download
        public void GetExeclColumn()
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            //비밀번호 확인
            PasswordCheckManager pcm = new PasswordCheckManager(um.excel_password);
            if (!pcm.isPasswordCheck())
                return;

            try
            {
                int col_cnt = 21;
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기
                Microsoft.Office.Interop.Excel.Worksheet wk = workSheet;

                setAutomatic(excelApp, false);
                //excelApp.Visible = true;
                Excel.Range rg1;

                wk.Columns["A"].ColumnWidth = 30;
                wk.Columns["F"].ColumnWidth = 30;


                int row = 1;
                foreach (CostAccountingGroupUnit unit in pnMain.Controls)
                {
                    DataGridView dgv = unit.GetDatagridview();
                    //Title
                    int col = 0;
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        if (dgv.Columns[i].Visible && dgv.Columns[i].Name != "weight_calculate")
                        {
                            col++;
                            wk.Cells[row, col].value = dgv.Columns[i].HeaderText;
                        }
                    }
                    rg1 = wk.Range[wk.Cells[row, 1], wk.Cells[row, col_cnt]];
                    rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                    rg1.Font.Size = 11;
                    rg1.Font.Bold = true;
                    //Data
                    int stt_row = row + 1;
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        col = 0;
                        row++;
                        for (int j = 0; j < dgv.Columns.Count; j++)
                        {
                            if (dgv.Columns[j].Visible && dgv.Columns[j].Name != "weight_calculate")
                            {
                                col++;
                                wk.Cells[row, col].value = dgv.Rows[i].Cells[j].Value;
                            }
                        }
                    }
                    rg1 = wk.Range[wk.Cells[stt_row, 1], wk.Cells[row, col_cnt]];
                    rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    //Total
                    row++;
                    int total_weight, total_income_amount1, total_income_amount2;
                    unit.GetTotal(out total_weight, out total_income_amount1, out total_income_amount2);

                    wk.Cells[row, 1].value = "총 중량(kg)";
                    wk.Cells[row, 2].value = total_weight;
                    wk.Cells[row, 3].value = "총 손익금액";
                    rg1 = wk.Range[wk.Cells[row, 3], wk.Cells[row, 4]];
                    rg1.Merge();
                    wk.Cells[row, 5].value = total_income_amount1;
                    wk.Cells[row, 6].value = "총 손익금액(TRQ)";
                    wk.Cells[row, 7].value = total_income_amount2;

                    rg1 = wk.Range[wk.Cells[row, 1], wk.Cells[row, col_cnt]];
                    rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);
                    rg1.Font.Size = 11;
                    rg1.Font.Bold = true;
                    row++;
                    row++;
                }


                //속도개선 ON
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this,ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
                this.Activate();
                setAutomatic(excelApp, true);
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
            }
            finally
            {
                ReleaseObject(workSheet);
                ReleaseObject(workBook);
                ReleaseObject(excelApp);
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

        #endregion

        private void rbPurchaseprice_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPurchaseprice.Checked)
            {
                foreach (CostAccountingGroupUnit unit in pnMain.Controls)
                    unit.SetMargin(rbPurchaseprice.Checked, cbPerBox.Checked);
            }
            else if (rbSalesPrice.Checked)
            {
                foreach (CostAccountingGroupUnit unit in pnMain.Controls)
                    unit.SetMargin(rbPurchaseprice.Checked, cbPerBox.Checked);
            }
        }

        private void cbPerBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (CostAccountingGroupUnit unit in pnMain.Controls)
                unit.SetPerbox(rbPurchaseprice.Checked, cbPerBox.Checked);
        }


        #region Print
        int count = 0;
        int pageNo = 1;
        List<Bitmap> bitmaps = new List<Bitmap>();
        private void btnPreview_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "원가계산", "is_print"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            printDocument = DashboardPrint();
            PrintManager pm = new PrintManager(this, printDocument, bitmaps.Count);
            pm.ShowDialog();
        }

        public PrintDocument DashboardPrint()
        {
            //프린터 설정
            count = 0;
            pageNo = 1;
            currentPageIndex = 0;

            //
            this.TopMost = true;
            this.Activate();
            this.Update();

            PrintDocument printDocument1 = new PrintDocument();
            bitmaps = new List<Bitmap>();
            List<Control> controls = new List<Control>(pnMain.Controls.Count);
            foreach (Control con in pnMain.Controls)
                controls.Add(con);


            //최대높이
            int maxHeight = this.pnMain.Height;
            int currentHeight = 0;
            int sttIdx = 0, endIdx = 0;

            for (int i = 0; i < pnMain.Controls.Count; i++)
            {
                //출력범위 이상 넘어갈때
                if (i == pnMain.Controls.Count - 1)
                {
                    Bitmap bitmap = new Bitmap(this.Width, this.Height);
                    if (currentHeight + pnMain.Controls[i].Height > maxHeight)
                    {
                        //마지막 idx제외하고 출력
                        endIdx = i;
                        for (int j = 0; j < pnMain.Controls.Count; j++)
                        {
                            if (j >= sttIdx && j < endIdx)
                                pnMain.Controls[j].Visible = true;
                            else
                                pnMain.Controls[j].Visible = false;
                        }

                        this.Update();
                        Thread.Sleep(100);
                        bitmap = CaptureForm(this);
                        bitmaps.Add(bitmap);

                        //마지막 idx만 다시 출력
                        for (int j = 0; j < pnMain.Controls.Count; j++)
                        {
                            if (j == i)
                                pnMain.Controls[j].Visible = true;
                            else
                                pnMain.Controls[j].Visible = false;
                        }

                        this.Update();
                        Thread.Sleep(100);
                        bitmap = CaptureForm(this);
                        bitmaps.Add(bitmap);

                    }
                    else
                    {
                        //마지막 idx제외하고 출력
                        endIdx = i;
                        for (int j = 0; j < pnMain.Controls.Count; j++)
                        {
                            if (j >= sttIdx && j <= endIdx)
                                pnMain.Controls[j].Visible = true;
                            else
                                pnMain.Controls[j].Visible = false;
                        }

                        this.Update();
                        Thread.Sleep(100);
                        bitmap = CaptureForm(this);
                        bitmaps.Add(bitmap);
                    }
                }
                else if (currentHeight + pnMain.Controls[i].Height > maxHeight)
                {
                    Bitmap bitmap = new Bitmap(this.Width, this.Height);
                    //마지막 idx제외하고 출력
                    endIdx = i;
                    for (int j = 0; j < pnMain.Controls.Count; j++)
                    {
                        if (j >= sttIdx && j <= endIdx)
                            pnMain.Controls[j].Visible = true;
                        else
                            pnMain.Controls[j].Visible = false;
                    }

                    this.Update();
                    Thread.Sleep(100);
                    bitmap = CaptureForm(this);
                    bitmaps.Add(bitmap);

                    sttIdx = i;
                    currentHeight = pnMain.Controls[i].Height;
                }
                else
                    currentHeight += pnMain.Controls[i].Height;

            }
            //
            this.TopMost = false;

            for (int j = 0; j < pnMain.Controls.Count; j++)
                pnMain.Controls[j].Visible = true;

            printDocument1 = new PrintDocument();
            printDocument1.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169); // A4 용지 크기 (가로 방향)
            printDocument1.DefaultPageSettings.Landscape = true; // 가로 방향 설정
            printDocument1.PrintPage += PrintDocument_PrintPage;
            printDocument1.PrinterSettings.PrintRange = PrintRange.SomePages;
            printDocument1.PrinterSettings.FromPage = 1;
            printDocument1.PrinterSettings.ToPage = bitmaps.Count;

            return printDocument1;

            /*PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.ShowDialog();*/
        }

        static Bitmap CaptureForm(Form form)
        {
            Rectangle bounds = form.Bounds;
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return bitmap;
        }

        private PrintDocument printDocument = new PrintDocument();
        private int currentPageIndex = 0;
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (currentPageIndex < bitmaps.Count)
            {
                Bitmap bitmap = bitmaps[currentPageIndex];
                Rectangle destRect = e.MarginBounds;

                // 비트맵 이미지를 출력 용지에 맞게 확대하여 출력
                destRect.Height = 827;
                destRect.Width = 1169;

                // 이미지를 가운데에 출력
                destRect.X += (e.MarginBounds.Width - destRect.Width) / 2;
                destRect.Y += (e.MarginBounds.Height - destRect.Height) / 2;

                e.Graphics.DrawImage(bitmap, destRect);

                currentPageIndex++;
                e.HasMorePages = currentPageIndex < bitmaps.Count;
            }
        }
        //초기화
        public void InitVariable()
        {
            count = 0;
            pageNo = 1;
            DashboardPrint();
        }

        #endregion
    }
}
