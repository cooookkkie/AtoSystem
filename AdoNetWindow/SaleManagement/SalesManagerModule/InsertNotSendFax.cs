using AdoNetWindow.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Repositories;
using Repositories.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.SalesManagerModule
{
    public partial class InsertNotSendFax : Form
    {
        INotSendFaxRepository notSendFaxRepository = new NotSendFaxRepository();
        ICommonRepository commonRepository = new CommonRepository();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        UsersModel um;
        SalesManager sm = null;
        Libs.Tools.Common common = new Libs.Tools.Common();
        public InsertNotSendFax(UsersModel um, SalesManager sm)
        {
            InitializeComponent();
            this.um = um;
            this.sm = sm;
        }
        #region Button
        private void btnInsert_Click(object sender, EventArgs e)
        {
            dgvFax.AllowUserToAddRows = false;

            if (dgvFax.Rows.Count == 0)
            {
                messageBox.Show(this, "등록할 데이터가 없습니다.");
                dgvFax.AllowUserToAddRows = true;
                return;
            }
            else
            {
                if (messageBox.Show(this, dgvFax.RowCount.ToString("#,##0") + "개의 팩스금지 데이터를 등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    dgvFax.AllowUserToAddRows = true;
                    return;
                }

                List<StringBuilder> sqlList = new List<StringBuilder>();
                foreach (DataGridViewRow row in dgvFax.Rows)
                {
                    if (row.Cells["fax"].Value != null && !string.IsNullOrEmpty(row.Cells["fax"].Value.ToString()))
                    {
                        NotSendFaxModel model = new NotSendFaxModel();
                        model.fax = row.Cells["fax"].Value.ToString();
                        model.updatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        model.edit_user = um.user_name;

                        StringBuilder sql = notSendFaxRepository.InsertCompany(model);
                        sqlList.Add(sql);
                    }
                }

                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    dgvFax.AllowUserToAddRows = true;
                    messageBox.Show(this, "등록중 에러가 발생하였습니다.");
                }
                else
                {
                    dgvFax.AllowUserToAddRows = true;
                    sm.GetNotSendFaxData();
                    this.Dispose();
                }
            }
        }
        private void btnGetExcel_Click(object sender, EventArgs e)
        {
            string excel_path = ChooseExcelFile();
            if (excel_path != null && !string.IsNullOrEmpty(excel_path))
            {
                DataTable dataDt = ExcelToDataTable(excel_path);
                if (dataDt != null || dataDt.Rows.Count > 0)
                {
                    SelectColumn sc = new SelectColumn(dataDt);


                    string fax_column_name = sc.GetColumn();
                    if (fax_column_name == null)
                    {
                        messageBox.Show(this, "컬럼을 선택하지 않으셨습니다!");
                        return;
                    }
                    else
                    {
                        dgvFax.Rows.Clear();
                        for (int i = 0; i < dataDt.Rows.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(dataDt.Rows[i][fax_column_name].ToString().Trim())) 
                            {
                                int n = dgvFax.Rows.Add();
                                dgvFax.Rows[n].Cells["fax"].Value = dataDt.Rows[i][fax_column_name].ToString();
                            }
                        }
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion

        #region Method
        static Dictionary<string, int> duplicateColumnDic = new Dictionary<string, int>();
        public static DataTable ExcelToDataTable(string filePath)
        {
            duplicateColumnDic = new Dictionary<string, int>();
            DataTable dt = new DataTable();
            IWorkbook workbook;

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // 파일 확장자에 따라 적절한 Workbook 생성
                if (filePath.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(stream); // .xls 파일용
                }
                else if (filePath.EndsWith(".xlsx"))
                {
                    workbook = new XSSFWorkbook(stream); // .xlsx 파일용
                }
                else
                {
                    throw new Exception("Invalid file extension");
                }

                ISheet sheet = workbook.GetSheetAt(0); // 첫 번째 시트 가져오기
                IRow headerRow = sheet.GetRow(0); // 첫 번째 행을 헤더로 사용
                int cellCount = headerRow.LastCellNum;

                // 헤더를 기준으로 DataTable의 컬럼 생성

                for (int j = 0; j < cellCount; j++)
                {
                    string col_name = headerRow.GetCell(j).ToString();
                    if (string.IsNullOrEmpty(col_name))
                        col_name = "NULL";


                    if (!dt.Columns.Contains(col_name))
                        dt.Columns.Add(col_name);
                    else
                    {
                        int duplicate_cnt;
                        if (duplicateColumnDic.ContainsKey(col_name))
                        {
                            duplicate_cnt = duplicateColumnDic[col_name];
                            dt.Columns.Add(col_name + "_" + ++duplicate_cnt);
                        }
                        else
                        {
                            duplicate_cnt = 0;
                            duplicateColumnDic.Add(col_name, duplicate_cnt);
                            dt.Columns.Add(col_name + "_" + duplicate_cnt);
                            duplicate_cnt++;
                        }
                    }
                }

                // 데이터 로우를 DataTable에 추가
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null) continue; // 빈 행은 스킵
                    DataRow dataRow = dt.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = row.GetCell(j).ToString();
                    }

                    dt.Rows.Add(dataRow);
                }
            }

            return dt;
        }
        public static string ChooseExcelFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // 필터 설정 (Excel 파일만 표시)
                openFileDialog.Filter = "Excel Files|*.xls*;*.csv;";
                openFileDialog.Title = "Select an Excel File";

                // 대화 상자를 표시하고 결과 확인
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // 선택된 파일의 경로 반환
                    return openFileDialog.FileName;
                }
                else
                {
                    // 취소되면 빈 문자열 반환
                    return string.Empty;
                }
            }
        }


        #endregion

        #region Key event
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Control tb = common.FindFocusedControl(this);

            //붙혀넣기시 로우추가 설정
            if (keyData == (Keys.Control | Keys.V))
            {
                //행추가 붙여넣기
                int col = dgvFax.CurrentCell.ColumnIndex;
                int row = dgvFax.CurrentCell.RowIndex;
                string clipText = Clipboard.GetText();
                string[] lines = clipText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                dgvFax.Rows.Add(lines.Length - 1);
                dgvFax.EndEdit();
                dgvFax.CurrentCell = dgvFax.Rows[row].Cells[col];
                dgvFax.Paste();
            }
            //복사하기
            else if (keyData == (Keys.Control | Keys.C))
            {
                if (dgvFax.Focused)
                    dgvFax.Copy();
                return true;
            }
            //삭제하기
            if (dgvFax.Focused && keyData == Keys.Delete)
            {
                dgvFax.Delete();
                return true;
            }
            else
                return base.ProcessCmdKey(ref msg, keyData);

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
        private void InsertNotSendFax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            { 
                switch(e.KeyCode)
                {
                    case Keys.A:
                        btnInsert.PerformClick();
                        break;
                    case Keys.X:
                        btnExit.PerformClick();
                        break;
                }
            }
        }
    }
}
