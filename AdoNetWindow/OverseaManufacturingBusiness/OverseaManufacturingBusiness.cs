using AdoNetWindow.Model;
using Repositories;
using Repositories.OverseaManufacturing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;
using Repositories.Config;
using AdoNetWindow.Common;


namespace AdoNetWindow.OverseaManufacturingBusiness
{
    public partial class OverseaManufacturingBusiness : Form
    {
        static Microsoft.Office.Interop.Excel.Application excelApp = null;
        static Microsoft.Office.Interop.Excel.Workbook workBook = null;
        static Microsoft.Office.Interop.Excel.Worksheet workSheet = null;
        Libs.Tools.Common common= new Libs.Tools.Common();
        IOverseaManufacturingRepository overseaManufacturingRepository = new OverseaManufacturingRepository();
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um;
        public OverseaManufacturingBusiness(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            SetHeaderStyle();
        }
        private void OverseaManufacturingBusiness_Load(object sender, EventArgs e)
        {
            dgvCompany.AutoPaste(false);

            //txtSttdate.Text = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            txtEnddate.Text = DateTime.Now.ToString("yyyy-MM-dd");

            if (rbSearchType0.Checked)
            {
                dgvCompany.Columns["in_date"].Visible = true;
                dgvCompany.Columns["importer"].Visible = true;
                dgvCompany.Columns["manufacturing"].Visible = true;
                dgvCompany.Columns["cnt"].Visible = false;
            }
            else if (rbSearchType1.Checked)
            {
                dgvCompany.Columns["in_date"].Visible = false;
                dgvCompany.Columns["importer"].Visible = false;
                dgvCompany.Columns["manufacturing"].Visible = true;
                dgvCompany.Columns["cnt"].Visible = true;
            }
            else
            {
                dgvCompany.Columns["in_date"].Visible = false;
                dgvCompany.Columns["importer"].Visible = true;
                dgvCompany.Columns["manufacturing"].Visible = false;
                dgvCompany.Columns["cnt"].Visible = true;
            }

            DataTable lastDt = overseaManufacturingRepository.GetLastImdate("수산물");
            if (lastDt.Rows.Count > 0)
                lbLastImdateFish.Text = "수산물 최근 처리일자 : " + Convert.ToDateTime(lastDt.Rows[0]["im_date"].ToString()).ToString("yyyy-MM-dd");
            else
                lbLastImdateFish.Text = "수산물 최근 처리일자 : ?";
            lastDt = overseaManufacturingRepository.GetLastImdate("가공식품");
            if (lastDt.Rows.Count > 0)
                lbLastImdateFood.Text = "가공식품 최근 처리일자 : " + Convert.ToDateTime(lastDt.Rows[0]["im_date"].ToString()).ToString("yyyy-MM-dd");
            else
                lbLastImdateFish.Text = "가공식품 최근 처리일자 : ?";
            //권한
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "해외제조업소 및 수입업체 수출입", "is_excel"))
                {
                    btnExcel.Visible = false;
                }
            }

            DataTable productTypeDt = overseaManufacturingRepository.GetDistinctData("product_type");
            if (productTypeDt != null && productTypeDt.Rows.Count > 0)
            {
                for (int i = 0; i < productTypeDt.Rows.Count; i++)
                    cbProductType.Items.Add(productTypeDt.Rows[i]["product_type"].ToString());
            }
            DataTable divisionDt = overseaManufacturingRepository.GetDistinctData("division");
            if (divisionDt != null && divisionDt.Rows.Count > 0)
            {
                for (int i = 0; i < divisionDt.Rows.Count; i++)
                    cbDivision.Items.Add(divisionDt.Rows[i]["division"].ToString());
            }
        }

        #region Main method
        private void SetCounting()
        {
            double select_cnt = 0, total_cnt = 0;
            if (dgvCompany.SelectedCells.Count > 0)
            {
                for (int i = 0; i < dgvCompany.Rows.Count; i++)
                {
                    double cnt;
                    if (dgvCompany.Rows[i].Cells["cnt"].Value == null || !double.TryParse(dgvCompany.Rows[i].Cells["cnt"].Value.ToString(), out cnt))
                        cnt = 0;
                    total_cnt += cnt;
                    for (int j = 0; j < dgvCompany.Columns.Count; j++)
                    {
                        if (dgvCompany.Rows[i].Cells[j].Selected)
                        {
                            select_cnt += cnt;
                            break;
                        }
                    }
                }
                //출력
                lbSelectSummary.Text = total_cnt.ToString("#,##0");
                lbSelectCellsCount.Text = select_cnt.ToString("#,##0");
            }

            txtTotalCount.Text = "0";
            txtSelectCount.Text = "0";

            if (dgvCompany.Rows.Count > 0)
            {
                txtTotalCount.Text = dgvCompany.RowCount.ToString("#,##0");
                if (dgvCompany.CurrentCell != null)
                    txtSelectCount.Text = dgvCompany.CurrentCell.RowIndex.ToString("#,##0");
            }

        }
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvCompany;
            //헤더 디자인
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!
            dgv.RowHeadersDefaultCellStyle.BackColor = Color.SeaGreen;

            //기본(남색)
            Color darkBlue = Color.FromArgb(43, 94, 170);    //남색
            dgv.ColumnHeadersDefaultCellStyle.BackColor = darkBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("중고딕", 8, FontStyle.Bold);
            
            Color lightRosybrown = Color.FromArgb(216, 190, 190);
            dgvDetail.DefaultCellStyle.BackColor = lightRosybrown;
        }
        private void GetData(bool isMassage = true)
        {
            if (txtPnameKor.Text.Trim() == String.Empty
                && txtPnameEng.Text.Trim() == String.Empty
                && txtImporter.Text.Trim() == String.Empty
                && txtManufacturing.Text.Trim() == String.Empty
                && txtMCountry.Text.Trim() == String.Empty
                && txtECountry.Text.Trim() == String.Empty
                && cbProductType.Text.Trim() == String.Empty
                && cbDivision.Text.Trim() == String.Empty)
            {
                if (isMassage)
                {
                    MessageBox.Show(this, "전체 검색시 많은 시간이 소요되어 프로그램 사용에 문제가 될 수 있습니다. 검색항목을 최소 한개이상 입력해주십시오.");
                    this.Activate();
                }
                return;
            }
            DateTime sttdate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate))
            {
                MessageBox.Show(this, "시작일의 값이 올바르지 않습니다.");
                this.Activate();
                return;
            }
            DateTime enddate;
            if (!DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "종료일의 값이 올바르지 않습니다.");
                this.Activate();
                return;
            }

            //초기화
            dgvCompany.Rows.Clear();
            //검색타입
            int search_type;
            if (rbSearchType0.Checked)
                search_type = 0;

            else if (rbSearchType1.Checked)
                search_type = 1;
            else
                search_type = 2;
            //컬럼 숨기기, 풀기
            DataTable companyDt;
            companyDt = overseaManufacturingRepository.GetData(search_type, sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                            , cbDivision.Text, txtPnameKor.Text, txtPnameKor2.Text, txtExcept1.Text
                                                            , txtPnameEng.Text, txtPnameEng2.Text, txtExcept2.Text
                                                            , txtManufacturing.Text, cbExactly2.Checked, txtImporter.Text,cbExactly1.Checked
                                                            , txtMCountry.Text, txtECountry.Text, cbProductType.Text);
            //Get Data
            if (companyDt.Rows.Count > 0)
            {   
                //데이터 출력
                for (int i = 0; i < companyDt.Rows.Count; i++)
                {
                    int n = dgvCompany.Rows.Add();
                    DataGridViewRow row = dgvCompany.Rows[n];

                    row.Cells["id"].Value = companyDt.Rows[i]["id"].ToString();
                    row.Cells["division"].Value = companyDt.Rows[i]["division"].ToString();
                    row.Cells["pname_kor"].Value = companyDt.Rows[i]["pname_kor"].ToString();
                    row.Cells["pname_eng"].Value = companyDt.Rows[i]["pname_eng"].ToString();
                    row.Cells["product_type"].Value = companyDt.Rows[i]["product_type"].ToString();
                    row.Cells["importer"].Value = companyDt.Rows[i]["importer"].ToString();
                    row.Cells["manufacturing"].Value = companyDt.Rows[i]["manufacturing"].ToString();
                    row.Cells["m_country"].Value = companyDt.Rows[i]["m_country"].ToString();
                    row.Cells["e_country"].Value = companyDt.Rows[i]["e_country"].ToString();
                    //row.Cells["cnt"].Value = companyDt.Rows[i]["cnt"].ToString();
                    DateTime im_date;
                    if(DateTime.TryParse(companyDt.Rows[i]["im_date"].ToString(), out im_date))
                        row.Cells["in_date"].Value = im_date.ToString("yyyy-MM-dd");
                    else
                        row.Cells["in_date"].Value = "";
                    row.Cells["until_date"].Value = companyDt.Rows[i]["until_date"].ToString();
                    row.Cells["cnt"].Value = companyDt.Rows[i]["cnt"].ToString();
                }
            }
            SetCounting();
        }

        private void GetDetail(DataGridViewRow row)
        {
            DateTime sttdate;
            if (!DateTime.TryParse(txtSttdate.Text, out sttdate))
            {
                MessageBox.Show(this, "시작일의 값이 올바르지 않습니다.");
                this.Activate();
                return;
            }
            DateTime enddate;
            if (!DateTime.TryParse(txtEnddate.Text, out enddate))
            {
                MessageBox.Show(this, "종료일의 값이 올바르지 않습니다.");
                this.Activate();
                return;
            }
            //초기화
            dgvDetail.Rows.Clear();
            //검색타입
            int search_type;
            if (rbSearchType1.Checked)
                search_type = 1;
            else
                search_type = 2;
            System.Data.DataTable detailDt = overseaManufacturingRepository.GetDetail(search_type, Convert.ToInt32(row.Cells["id"].Value.ToString()), sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd")
                                                            , cbDivision.Text, txtPnameKor.Text, txtPnameKor2.Text, txtPnameEng.Text, txtPnameEng2.Text
                                                            , txtManufacturing.Text, cbExactly2.Checked, txtImporter.Text, cbExactly1.Checked
                                                            , txtMCountry.Text, txtECountry.Text);
            for (int i = 0; i < detailDt.Rows.Count; i++)
            {
                int n = dgvDetail.Rows.Add();
                DataGridViewRow newRow = dgvDetail.Rows[n];
                newRow.Cells["company"].Value = detailDt.Rows[i]["company"].ToString();
                DateTime im_date;
                if(DateTime.TryParse(detailDt.Rows[i]["im_date"].ToString(), out im_date))
                    newRow.Cells["im_date"].Value = im_date.ToString("yyyy-MM-dd");
            }
        }
        #endregion

        #region Button
        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExcelDownload();
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
        IAuthorityRepository authorityRepository = new AuthorityRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        private void btnDataUpload_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "수입관리", "해외제조업소 및 수입업체 수출입", "is_add"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }

            AddData ad = new AddData(um);
            ad.Show();
        }

        private void btnSearching_Click(object sender, EventArgs e)
        {
            GetData();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void rbSearchType0_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSearchType0.Checked)
            {
                dgvCompany.Columns["importer"].Visible = true;
                dgvCompany.Columns["manufacturing"].Visible = true;
                dgvCompany.Columns["cnt"].Visible = false;
            }
            else if (rbSearchType1.Checked)
            {
                dgvCompany.Columns["until_date"].Visible = false;
                dgvCompany.Columns["importer"].Visible = false;
                dgvCompany.Columns["manufacturing"].Visible = true;
                dgvCompany.Columns["cnt"].Visible = true;
            }
            else
            {
                dgvCompany.Columns["until_date"].Visible = false;
                dgvCompany.Columns["importer"].Visible = true;
                dgvCompany.Columns["manufacturing"].Visible = false;
                dgvCompany.Columns["cnt"].Visible = true;
            }
        }
        private void rbSearchType0_Click(object sender, EventArgs e)
        {
            if (rbSearchType0.Checked)
            {
                rbSearchType0.Checked = true;
                GetData(false);
            }
            else if (rbSearchType1.Checked)
            {
                rbSearchType1.Checked = true;
                GetData(false);
            }
            else if (rbSearchType2.Checked)
            {
                rbSearchType2.Checked = true;
                GetData(false);
            }
        }

        #endregion

        #region Key event
        private void txtEnddate_KeyDown(object sender, KeyEventArgs e)
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
        private void OverseaManufacturingBusiness_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Q:
                        GetData();
                        break;
                    case Keys.A:
                        btnDataUpload.PerformClick();
                        break;
                    case Keys.X:
                        this.Dispose();
                        break;
                    case Keys.M:
                        txtPnameKor.Focus();
                        break;
                    case Keys.N:
                        cbDivision.Text = String.Empty;
                        txtPnameKor.Text = String.Empty;
                        txtPnameKor2.Text = String.Empty;
                        txtExcept1.Text = String.Empty;
                        txtPnameEng.Text = String.Empty;
                        txtPnameEng2.Text = String.Empty;
                        txtExcept2.Text = String.Empty;
                        txtImporter.Text = String.Empty;
                        txtManufacturing.Text = String.Empty;
                        txtMCountry.Text = String.Empty;
                        txtECountry.Text = String.Empty;
                        txtPnameKor.Focus();
                        break;
                }
            }
            else if (e.Modifiers == Keys.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        if (dgvCompany.SelectedRows.Count > 1)
                        {
                            dgvCompany.ClearSelection();
                            e.Handled = false;
                        }
                        //MessageBox.Show(this,"복사기능은 차단되어 있습니다.");
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        {
                            rbSearchType0.Checked = true;
                            GetData(false);
                        }
                        break;
                    case Keys.F2:
                        {
                            rbSearchType1.Checked = true;
                            GetData(false);
                        }
                        break;
                    case Keys.F3:
                        {
                            rbSearchType2.Checked = true;
                            GetData(false);
                        }
                        break;
                }
            }
        }

        


        private void txtECountry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                GetData();
        }
        #endregion

        #region Datagridview event
        private void dgvCompany_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                GetDetail(dgvCompany.Rows[e.RowIndex]);
            }
        }
        private void dgvCompany_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (dgvCompany.SelectedRows.Count <= 1)
                { 
                    dgvCompany.ClearSelection();
                    dgvCompany.Rows[e.RowIndex].Selected = true;
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


            DataGridView dgv = dgvCompany;
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


                    //Data
                    Excel.Range rng = workSheet.get_Range("A2", "k" + (dgv.Rows.Count + 1));
                    object[,] only_data = (object[,])rng.get_Value();

                    if (dgv.Rows.Count > 0)
                    {
                        int row = dgv.Rows.Count + 1;
                        object[,] data = new object[row, 12];

                        data = only_data;

                        //row data
                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            only_data[i + 1, 1] = dgv.Rows[i].Cells["division"].Value;
                            only_data[i + 1, 2] = dgv.Rows[i].Cells["importer"].Value;
                            only_data[i + 1, 3] = dgv.Rows[i].Cells["pname_kor"].Value;
                            only_data[i + 1, 4] = dgv.Rows[i].Cells["pname_eng"].Value;
                            only_data[i + 1, 5] = dgv.Rows[i].Cells["product_type"].Value;
                            only_data[i + 1, 6] = dgv.Rows[i].Cells["manufacturing"].Value;
                            only_data[i + 1, 7] = dgv.Rows[i].Cells["in_date"].Value;
                            only_data[i + 1, 8] = dgv.Rows[i].Cells["until_date"].Value;
                            only_data[i + 1, 9] = dgv.Rows[i].Cells["m_country"].Value;
                            only_data[i + 1, 10] = dgv.Rows[i].Cells["e_country"].Value;
                            only_data[i + 1, 11] = dgv.Rows[i].Cells["cnt"].Value;
                        }

                        rng.Value = data;

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
            workSheet.Name = "해외제조업소 및 수입업체 수출입";

            //Column Width
            wk.Cells[1, 1].Value = "구분";
            wk.Cells[1, 2].Value = "수입업체";
            wk.Cells[1, 3].Value = "품명(한글)";
            wk.Cells[1, 4].Value = "품명(영어)";
            wk.Cells[1, 5].Value = "품목(유형)";
            wk.Cells[1, 6].Value = "해외제조업소";
            wk.Cells[1, 7].Value = "처리일자";
            wk.Cells[1, 8].Value = "소비일자";
            wk.Cells[1, 9].Value = "제조국";
            wk.Cells[1, 10].Value = "수출국";
            wk.Cells[1, 11].Value = "수량";

            //Border Line Style
            Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1 + rows, 11]];
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

        #region 우클릭 메뉴
        private void dgvCompany_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridView.HitTestInfo hitTestInfo; //Hit 위치 
                if (e.Button == MouseButtons.Right)
                {
                    hitTestInfo = dgvCompany.HitTest(e.X, e.Y);

                    int col = hitTestInfo.ColumnIndex;
                    int row = hitTestInfo.RowIndex;

                    if (col < 0) col = 0;
                    if (row < 0) row = 0;

                    ContextMenuStrip m = new ContextMenuStrip();
                    if (rbSearchType0.Checked)
                    {
                        m.Items.Add("해외제조업소 취급품목");
                        m.Items.Add("수입업체 취급품목");
                    }
                    else if (rbSearchType1.Checked)
                    {
                        m.Items.Add("해외제조업소 취급품목");
                    }
                    else if (rbSearchType2.Checked)
                    {
                        m.Items.Add("수입업체 취급품목");
                    }

                    //Event Method
                    m.ItemClicked += new ToolStripItemClickedEventHandler(m_ItemClicked);
                    m.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.cms_PreviewKeyDown);
                    //Create 
                    m.BackColor = Color.White;
                    m.Show(dgvCompany, e.Location);
                }
            }
            catch
            {
            }
        }
        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (dgvCompany.SelectedRows.Count > 0)
            {
                try
                {
                    DataGridViewRow dr = dgvCompany.SelectedRows[0];
                    if (dr.Index < 0)
                        return;
                    int rowindex = Convert.ToInt32(dr.Cells[0].Value);
                    switch (e.ClickedItem.Text)
                    {
                        case "해외제조업소 취급품목":
                            {
                                Dictionary<string, string> companyDic = new Dictionary<string, string>();
                                foreach (DataGridViewRow row in dgvCompany.SelectedRows)
                                {
                                    if (!companyDic.ContainsKey(row.Cells["manufacturing"].Value.ToString()))
                                        companyDic.Add(row.Cells["manufacturing"].Value.ToString(), row.Cells["manufacturing"].Value.ToString());
                                }
                                HandlingProduct hp = new HandlingProduct(um, 1, companyDic, txtSttdate.Text, txtEnddate.Text);
                                hp.Show();
                            }
                            break;
                        case "수입업체 취급품목":
                            {
                                Dictionary<string, string> companyDic = new Dictionary<string, string>();
                                foreach (DataGridViewRow row in dgvCompany.SelectedRows)
                                {
                                    if (!companyDic.ContainsKey(row.Cells["importer"].Value.ToString()))
                                        companyDic.Add(row.Cells["importer"].Value.ToString(), row.Cells["importer"].Value.ToString());
                                }
                                HandlingProduct hp = new HandlingProduct(um, 2, companyDic, txtSttdate.Text, txtEnddate.Text);
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

        private void btnUploadCalendar_Click(object sender, EventArgs e)
        {
            UploadDataCalendar udc = new UploadDataCalendar(um);
            udc.Owner = this;
            udc.Show();
        }

        private void dgvCompany_SelectionChanged(object sender, EventArgs e)
        {
            SetCounting();
        }
    }
}
