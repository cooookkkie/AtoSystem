using AdoNetWindow.Model;
using Repositories;
using Repositories.OverseaManufacturing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using DataTable = System.Data.DataTable;
using Excel = Microsoft.Office.Interop.Excel;

namespace AdoNetWindow.OverseaManufacturingBusiness
{
    public partial class AddData : Form
    {
        Libs.Tools.Common common = new Libs.Tools.Common();
        IOverseaManufacturingRepository overseaManufacturingRepository = new OverseaManufacturingRepository();
        ICommonRepository commonRepository = new CommonRepository();
        UsersModel um;
        public AddData(UsersModel uModel)
        {
            InitializeComponent();
            um = uModel;
            SetHeaderStyle();
            GetLastImdate();
        }

        #region Method
        private void GetLastImdate()
        {
            DataTable lastDt = overseaManufacturingRepository.GetLastImdate();
            if (lastDt.Rows.Count > 0)
                lbLastImdate.Text = "최근 처리일자 : " + Convert.ToDateTime(lastDt.Rows[0]["im_date"].ToString()).ToString("yyyy-MM-dd");
        }
        private void CheckDuplicate()
        {
            DateTime max_date = new DateTime(1900, 1, 1);
            DateTime min_date = new DateTime(2100, 1, 1);
            if (dgvData.Rows.Count > 0)
            {
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    DateTime tmpDt;
                    if (dgvData.Rows[i].Cells["im_date"].Value != null && DateTime.TryParse(dgvData.Rows[i].Cells["im_date"].Value.ToString(), out tmpDt))
                    {
                        if (max_date < tmpDt)
                            max_date = tmpDt;
                        if (min_date > tmpDt)
                            min_date = tmpDt;
                    }
                }

                System.Data.DataTable dataDt = overseaManufacturingRepository.GetData(0, min_date.ToString("yyyy-MM-dd"), max_date.ToString("yyyy-MM-dd"), "", "", "", "", "", "", "", "", false, "", false, "", "", "");
                if (dataDt.Rows.Count > 0)
                {
                    System.Data.DataTable inputDt = common.ConvertDgvToDataTable(dgvData);
                    inputDt.Columns.Add("data_code", typeof(string));
                    inputDt.AcceptChanges();
                    for (int i = 0; i < inputDt.Rows.Count; i++)
                    {
                        inputDt.Rows[i]["data_code"] = inputDt.Rows[i]["division"].ToString().Replace("'", "") + "^" + inputDt.Rows[i]["importer"].ToString().Replace("'", "") + "^"
                                                    + inputDt.Rows[i]["pname_kor"].ToString().Replace("'", "") + "^" + inputDt.Rows[i]["pname_eng"].ToString().Replace("'", "") + "^"
                                                    + inputDt.Rows[i]["manufacturing"].ToString().Replace("'", "") + "^" + inputDt.Rows[i]["im_date"].ToString().Replace("'", "") + "^"
                                                    + inputDt.Rows[i]["m_country"].ToString().Replace("'", "") + "^" + inputDt.Rows[i]["e_country"].ToString().Replace("'", "") + "^";
                    }
                    inputDt.AcceptChanges();
                    var var = (from p in inputDt.AsEnumerable()
                               join t in dataDt.AsEnumerable()
                               on p.Field<string>("data_code") equals t.Field<string>("data_code")
                               select new
                               {
                                   no = p.Field<string>("no")
                               }).ToList();

                    dataDt = ConvertListToDatatable(var);
                    int duplicate_cnt = 0;
                    if (dataDt != null)
                    {
                        for (int i = 0; i < dgvData.Rows.Count; i++)
                        {
                            string whr = "no = '" + dgvData.Rows[i].Cells["no"].Value.ToString() + "'";
                            DataRow[] dr = dataDt.Select(whr);
                            if (dr.Length > 0)
                            {
                                dgvData.Rows[i].HeaderCell.Style.BackColor = Color.Red;
                                duplicate_cnt++;
                            }
                        }
                    }
                    //재카운터
                    for(int i = 0; i < dgvData.Rows[i].Cells.Count; i++)
                    {
                        dgvData.Rows[i].Cells["no"].Value = (i + 1).ToString("#,##0");
                    }
                    //Msg
                    if (duplicate_cnt == 0)
                    {
                        MessageBox.Show(this, "중복된 데이터가 없습니다.");
                        this.Activate();
                    }
                    else
                    {
                        MessageBox.Show(this, duplicate_cnt + "개 중복된 데이터를 발견하였습니다.");
                        this.Activate();
                    }
                }
            }
        }
        //List => Datatable
        public static DataTable ConvertListToDatatable(IEnumerable<dynamic> v)
        {
            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();

            DataTable table = new DataTable();
            foreach (var info in infos)
            {
                Type propType = info.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                else
                    table.Columns.Add(info.Name, info.PropertyType);
            }

            DataRow row;

            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;
                }
                table.Rows.Add(row);
            }
            table.AcceptChanges();
            return table;
        }
        private void SetHeaderStyle()
        {
            DataGridView dgv = dgvData;
            //헤더 디자인
            dgv.EnableHeadersVisualStyles = false; // Windows XP 비주얼 스타일 적용시 추가함!

            Color darkBlue = Color.FromArgb(43, 94, 170);    //남색
            dgv.ColumnHeadersDefaultCellStyle.BackColor = darkBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("중고딕", 8, FontStyle.Bold);
        }
        private void InputData(object[,] data, int no)
        {
            for (int r = 2, i = 0; r <= data.GetLength(0); r++)
            {
                int n = dgvData.Rows.Add();
                DataGridViewRow row = dgvData.Rows[n];
                row.Cells["no"].Value = no;
                if (data[r, 2] != null)
                    row.Cells["division"].Value = data[r, 2].ToString();
                else
                    row.Cells["division"].Value = string.Empty;
                if (data[r, 3] != null)
                    row.Cells["importer"].Value = data[r, 3].ToString();
                else
                    row.Cells["importer"].Value = string.Empty;

                if (data[r, 4] != null)
                    row.Cells["pname_kor"].Value = data[r, 4].ToString();
                else
                    row.Cells["pname_kor"].Value = string.Empty;

                if (data[r, 5] != null)
                    row.Cells["pname_eng"].Value = data[r, 5].ToString();
                else
                    row.Cells["pname_eng"].Value = string.Empty;

                if (data[r, 6] != null)
                    row.Cells["product_type"].Value = data[r, 6].ToString();
                else
                    row.Cells["product_type"].Value = string.Empty;

                if (data[r, 7] != null)
                    row.Cells["manufacturing"].Value = data[r, 7].ToString();
                else
                    row.Cells["manufacturing"].Value = string.Empty;

                if (data[r, 8] != null)
                    row.Cells["im_date"].Value = data[r, 8].ToString();
                else
                    row.Cells["im_date"].Value = string.Empty;

                if (data[r, 9] != null)
                    row.Cells["until_date"].Value = data[r, 9].ToString();
                else
                    row.Cells["until_date"].Value = string.Empty;

                if (data[r, 10] != null)
                    row.Cells["m_country"].Value = data[r, 10].ToString();
                else
                    row.Cells["m_country"].Value = string.Empty;

                if (data[r, 11] != null)
                    row.Cells["e_country"].Value = data[r, 11].ToString();
                else
                    row.Cells["e_country"].Value = string.Empty;

                if (data[r, 12] != null)
                    row.Cells["frozen_num"].Value = data[r, 12].ToString();
                else
                    row.Cells["frozen_num"].Value = "";

                if (data[r, 13] != null)
                    row.Cells["lot_num"].Value = data[r, 13].ToString();
                else
                    row.Cells["lot_num"].Value = "";
                no++;
            }
        }
        #endregion
         
        #region Button
        private void btnDataUpload_Click(object sender, EventArgs e)
        {
            int no = 1;
            

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string folder_path = fbd.SelectedPath;
                if (folder_path != null)
                {
                    string[] fileList = Directory.GetFiles(folder_path);
                    if (fileList != null && fileList.Length > 0)
                    {
                        for(int i =0; i < fileList.Length; i++)
                        {
                            object[,] data = ReadExcelData(fileList[i]);
                            if (data != null)
                            {
                                dgvData.EndEdit();
                                if (dgvData.Rows.Count > 0)
                                    no = Convert.ToInt32(dgvData.Rows[dgvData.Rows.Count - 1].Cells["no"].Value.ToString()) + 1;
                                InputData(data, no);
                            }
                        }
                    }
                }
            }
            CheckDuplicate();                
        }
        private void btnInsert_Click(object sender, System.EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                MessageBox.Show(this, "등록할 내역이 없습니다.");
                this.Activate();
                return;
            }
            else
            {
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    if (dgvData.Rows[i].HeaderCell.Style.BackColor == Color.Red)
                    {
                        MessageBox.Show(this, "중복데이터가 있습니다. 중복제거후 등록해주세요.");
                        this.Activate();
                        return;
                    }
                }
            }

            if (MessageBox.Show(this, "등록하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = commonRepository.GetNextId("income", "id");
                List<StringBuilder> sqlList = new List<StringBuilder>();
                for (int i = 0; i < dgvData.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvData.Rows[i];

                    OverseaManufacturingModel model = new OverseaManufacturingModel();
                    model.id = id.ToString();
                    model.division = row.Cells["division"].Value.ToString();
                    model.importer = row.Cells["importer"].Value.ToString().Replace("'", "");
                    model.pname_kor = row.Cells["pname_kor"].Value.ToString().Replace("'", "");
                    model.pname_eng = row.Cells["pname_eng"].Value.ToString().Replace("'", "");
                    model.product_type = row.Cells["product_type"].Value.ToString().Replace("'", "");
                    model.manufacturing = row.Cells["manufacturing"].Value.ToString().Replace("'", "");
                    model.im_date = row.Cells["im_date"].Value.ToString().Replace("'", "");
                    model.until_date = row.Cells["until_date"].Value.ToString().Replace("'", "");
                    model.m_country = row.Cells["m_country"].Value.ToString().Replace("'", "");
                    model.e_country = row.Cells["e_country"].Value.ToString().Replace("'", "");
                    model.frozen_num = row.Cells["frozen_num"].Value.ToString().Replace("'", "");
                    model.lot_num = row.Cells["lot_num"].Value.ToString().Replace("'", "");
                    model.updatetime = DateTime.Now.ToString("yyyy-MM-dd");
                    model.last_edit_user_name = um.user_name;
                    model.group_id = "1";
                    model.remark = "";

                    StringBuilder sql = overseaManufacturingRepository.InsertSql(model);
                    sqlList.Add(sql);

                    id++;
                }

                if (commonRepository.UpdateTran(sqlList) == -1)
                {
                    MessageBox.Show(this, "등록중 에러가 발생하였습니다.");
                    this.Activate();
                }
                else
                {
                    MessageBox.Show(this, "등록완료");
                    this.Activate();
                    this.Dispose();
                }
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        private void btnRemoveDuplicate_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count > 0)
            {
                for (int i = dgvData.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvData.Rows[i].HeaderCell.Style.BackColor == Color.Red)
                        dgvData.Rows.Remove(dgvData.Rows[i]);
                }
            }
        }
        #endregion

        #region Key event
        private void AddData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt)
            {
                switch (e.KeyCode)
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
        #endregion

        #region Get Excel Data Method
        public object[,] ReadExcelData(string path)
        {
            object[,] data = null;
            // path는 Excel파일의 전체 경로입니다.
            // 예. D:\test\test.xslx
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;
            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Open(path);
                // path 대신 문자열도 가능합니다
                // 예. Open(@"D:\test\test.xslx");
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
                // 첫번째 Worksheet를 선택합니다.
                Excel.Range rng = ws.UsedRange;   // '여기'
                                                  // 현재 Worksheet에서 사용된 셀 전체를 선택합니다.
                data = rng.Value;

                // 열들에 들어있는 Data를 배열 (One-based array)로 받아옵니다.
                // 등록규격에 맞는 파일인지 확인
                if (data.GetLength(0) > 1)
                {
                    if (!(data[1, 2].ToString() == "구분"
                        && data[1, 3].ToString() == "수입업체"
                        && data[1, 4].ToString() == "제품명(한글)"
                        && data[1, 5].ToString() == "제품명(영문)"
                        && data[1, 7].ToString() == "해외제조업소"
                        && data[1, 10].ToString() == "제조국"
                        && data[1, 11].ToString() == "수출국"))
                    {
                        MessageBox.Show(this, "업로드 규격에 맞지 않은 파일입니다.");
                        this.Activate();
                        data = null;
                    }
                }
                else
                {
                    MessageBox.Show(this, "데이터가 존재하지 않습니다.");
                    this.Activate();
                    data = null;
                }
                
                wb.Close(true);
                excelApp.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
            return data;
        }

        static void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
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
                GC.Collect();
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            CheckDuplicate();
        }
    }
}
