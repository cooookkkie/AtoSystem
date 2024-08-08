using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using AdoNetWindow.SaleManagement.DuplicateCompany;
using System.Data.OleDb;
using Repositories.Config;
using AdoNetWindow.Model;
using System.Fabric.Management.ServiceModel;
using CFXCOMLib;
using Libs.Tools;
using System.Net;
using System.Threading;
using Microsoft.VisualBasic.Logging;
using static System.Net.WebRequestMethods;
using MySqlX.XDevAPI.Relational;


namespace AdoNetWindow.Common.FaxManager
{
    public partial class FaxManager : Form
    {
        private string stFolderPath1;
        private System.IO.DirectoryInfo di;
        Libs.Tools.Common common = new Libs.Tools.Common();
        Libs.MessageBox messageBox = new Libs.MessageBox();
        List<Libs.Tools.FaxSender> faxList = new List<Libs.Tools.FaxSender> ();
        IAuthorityRepository authorityRepository = new AuthorityRepository();

        static Excel.Application excelApp = null;
        static Excel.Workbook workBook = null;
        static Excel.Worksheet workSheet = null;

        UsersModel um;
        CFXCOMLib.Connection m_Connection;

        public FaxManager(UsersModel um)
        {
            InitializeComponent();
            this.um = um;
            m_Connection = common.LoginFaxServer("admin", "admin");
        }

        private void FaxManager_Load(object sender, EventArgs e)
        {
            cbYear.Text = DateTime.Now.Year.ToString();
            cbMonth.Text = DateTime.Now.Month.ToString();
            cbDay.Text = DateTime.Now.Day.ToString();    
        }

        #region Method
        public void ExcelImport(string fileName, DataGridView dgv)
        {
            // 엑셀 문서 내용 추출
            string connectionString = string.Empty;
            string sheetName = string.Empty;

            // 파일 확장자 검사
            if (System.IO.File.Exists(fileName))
            {
                if (Path.GetExtension(fileName).ToLower() == ".xls")
                {   // Microsoft.Jet.OLEDB.4.0 은 32 bit 에서만 동작되므로 빌드 할때 반드시 32bit로 할것(64bit로 하면 에러남)
                    connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0};Extended Properties=Excel 8.0;", fileName);
                }
                else if (Path.GetExtension(fileName).ToLower() == ".xlsx")
                {
                    connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=Excel 12.0;", fileName);
                }
            }

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    con.Close();
                }
            }
            //엑셀 시트 이름 설정
            DataSet data = new DataSet();
            DataTable dt = new DataTable();

            string strQuery = "SELECT * FROM [" + sheetName + "]";
            OleDbConnection oleConn = new OleDbConnection(connectionString);
            oleConn.Open();

            OleDbCommand oleCmd = new OleDbCommand(strQuery, oleConn);
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(oleCmd);

            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            data.Tables.Add(dataTable);

            //엑셀 컬럼 정보
            dgv.Rows.Clear();
            ExcelColumnManager ecm = new ExcelColumnManager(this, dataTable);
            ecm.Owner = this;
            DataTable columnDt = ecm.GetColumnSetting();
            if (columnDt != null)
            {
                Dictionary<string, string> columnDic = new Dictionary<string, string>();
                DataRow[] columnDr = columnDt.Select("input_column <> ''");
                if (columnDr.Length > 0)
                {
                    for (int i = 0; i < columnDr.Length; i++)
                    {
                        if (!columnDic.ContainsKey(columnDr[i]["excel_column"].ToString()))
                            columnDic.Add(columnDr[i]["excel_column"].ToString(), columnDr[i]["input_column_name"].ToString());
                        else
                            columnDic[columnDr[i]["excel_column"].ToString()] = columnDr[i]["input_column_name"].ToString();
                    }

                    //datagridview에 datatable(엑셀 데이터) 담기
                    //dgv.DataSource = dataTable;
                    dgv.Rows.Clear();
                    if (columnDic.Keys.Count > 0)
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            int n = dgv.Rows.Add();
                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                if (columnDic.ContainsKey(dataTable.Columns[j].ColumnName))
                                    dgv.Rows[n].Cells[columnDic[dataTable.Columns[j].ColumnName]].Value = dataTable.Rows[i][j].ToString();
                            }
                        }
                    }
                }
            }
            dataTable.Dispose();
            dataAdapter.Dispose();
            oleCmd.Dispose();

            oleConn.Close();
            oleConn.Dispose();


        }
        #endregion

        #region (Radio)Button event
        private void btnPause_Click(object sender, EventArgs e)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                Task t = tasks[i];
                if (t.IsCompleted)
                    tasks.Remove(t);
            }

            if (tasks.Count > 0)
            {
                if (messageBox.Show(this, "송신중인 팩스를 중단하시겠습니까? *이미 보내진 팩스는 되돌리 수 없습니다.", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dgvFaxNumber.Rows)
                    {
                        if (row.Cells["status"].Value == null || (!row.Cells["status"].Value.ToString().Equals("SUCCESS") && !row.Cells["status"].Value.ToString().Equals("FAILED")))
                        {
                            string result = m_Connection.StopSendFax(Convert.ToInt32(row.Cells["job_id"].Value));
                            if (result == "SUCCESS")
                                row.Cells["status"].Value = "PAUSE";
                            else
                                row.Cells["status"].Value = "PAUSE FAILED";

                        }
                    }

                    tasks.Clear();
                    progressBar1.Value = 0;
                    messageBox.Show(this, "중단완료");
                }
            }
            else
            {
                messageBox.Show(this, "작업중인 내역이 없습니다!");
            }
        }
        private void btnGetData_Click(object sender, EventArgs e)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                Task t = tasks[i];
                if (t.IsCompleted)
                    tasks.Remove(t);
            }

            if (tasks.Count > 0)
            {
                messageBox.Show(this, "팩스 송신중이라 작업할 수 없습니다. 모든작업이 끝나고 작업해주시기 바랍니다.");
                return;
            }

            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Filter = "Excel 파일 (*.xls*) | *.xls*";
                dlgOpen.Title = "Files";
                dlgOpen.Multiselect = false; // 파일 다중 선택        
                //dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < dlgOpen.FileNames.Length; i++)
                    {
                        ExcelImport(dlgOpen.FileNames[i], dgvFaxNumber);
                        break;
                    }
                }
            }
        }
        private void btnDeleteComplete_Click(object sender, EventArgs e)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                Task t = tasks[i];
                if (t.IsCompleted)
                    tasks.Remove(t);
            }

            if (tasks.Count > 0)
            {
                messageBox.Show(this, "팩스 송신중이라 작업할 수 없습니다. 모든작업이 끝나고 작업해주시기 바랍니다.");
                return;
            }

            if (dgvFaxNumber.Rows.Count > 0)
            {
                for (int i = dgvFaxNumber.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvFaxNumber.Rows[i].Cells["status"].Value != null && dgvFaxNumber.Rows[i].Cells["status"].Value.ToString() == "SUCCESS")
                        dgvFaxNumber.Rows.Remove(dgvFaxNumber.Rows[i]);
                }
            }
        }
        private async void btnSend_Click(object sender, EventArgs e)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                Task t = tasks[i];
                if (t.IsCompleted)
                    tasks.Remove(t);
            }

            if (tasks.Count > 0)
            {
                messageBox.Show(this, "팩스 송신중이라 작업할 수 없습니다. 모든작업이 끝나고 작업해주시기 바랍니다.");
                return;
            }

            SendFax();
        }

        List<Task> tasks = new List<Task>();
        private async void SendFax()
        {
            tasks = new List<Task>();

            dgvFaxNumber.AllowUserToAddRows = false;
            dgvFaxNumber.AllowUserToDeleteRows = false;
            // ProgressBar의 최대값 설정
            progressBar1.Value = 0;


            //데이터 확인
            if (dgvFaxNumber.Rows.Count == 0)
            {
                messageBox.Show(this, "보낼 내역을 추가해주세요!");
                dgvFaxNumber.AllowUserToAddRows = true;
                dgvFaxNumber.AllowUserToDeleteRows = true;
                return;
            }

            if (rbBatchAttachmentFile.Checked && dgvAttachments.Rows.Count == 0)
            {
                messageBox.Show(this, "일괄발송 첨푸파일을 추가해주세요!");
                dgvFaxNumber.AllowUserToAddRows = true;
                dgvFaxNumber.AllowUserToDeleteRows = true;
                return;
            }

            //저장위치
            Libs.Tools.CookieTools cookie = new Libs.Tools.CookieTools(null, @"temp\fax_temp");
            cookie.FileDelete();


            // ProgressBar의 최대값 설정
            progressBar1.Maximum = dgvFaxNumber.Rows.Count;

            //Msg
            if (rbBatchAttachmentFile.Checked)
            {
                if (messageBox.Show(this, "모든 수신처에 일괄발송 첨부파일을 팩스 송신하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //첨부파일 Tiff 변환
                    foreach (DataGridViewRow row in dgvAttachments.Rows)
                    {
                        if (row.Cells["path"].Value != null)
                        {
                            FileInfo fi = new FileInfo(row.Cells["path"].Value.ToString());
                            if (fi.Exists)
                            {
                                //첨부파일 Tiff 변환
                                string file_name = System.IO.Path.GetFileNameWithoutExtension(row.Cells["path"].Value.ToString()) + ".tif";
                                string save_path = cookie.GetSavePath(file_name);
                                m_Connection.ConvertToTiff(row.Cells["path"].Value.ToString(), save_path);
                                row.Cells["real_path"].Value = save_path;
                            }
                            else
                            {
                                messageBox.Show(this, "존재하지 않는 첨부파일입니다!");
                                dgvAttachments.ClearSelection();
                                row.Selected = true;

                                dgvFaxNumber.AllowUserToAddRows = true;
                                dgvFaxNumber.AllowUserToDeleteRows = true;
                                return;
                            }
                        }
                    }


                    //팩스 송신
                    int[] taskIdList = new int[dgvFaxNumber.RowCount];
                    for (int i = 0; i < dgvFaxNumber.RowCount; i++)
                    {
                        int taskId = SendFax(dgvFaxNumber.Rows[i], dgvAttachments.Rows[0].Cells["real_path"].Value.ToString());
                        if (taskId > 0)
                        {
                            taskIdList[i] = taskId;
                            dgvFaxNumber.Rows[i].Cells["job_id"].Value = taskId;
                        }
                        // 비동기로 팩스 상태를 확인하고, 성공 또는 실패할 때까지 3초마다 확인
                        tasks.Add(CheckFaxStatusAsync(taskId));
                    }

                }
                else
                {
                    dgvFaxNumber.AllowUserToAddRows = true;
                    dgvFaxNumber.AllowUserToDeleteRows = true;
                    return;
                }
            }
            else
            {
                if (messageBox.Show(this, "모든 수신처에 개별 첨부파일을 팩스 송신하시겠습니까?", "YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    //팩스 송신
                    int[] taskIdList = new int[dgvFaxNumber.RowCount];
                    for (int i = 0; i < dgvFaxNumber.RowCount; i++)
                    {
                        DataGridViewRow row = dgvFaxNumber.Rows[i];
                        if (row.Cells["attachment_path"].Value != null)
                        {
                            FileInfo fi = new FileInfo(row.Cells["attachment_path"].Value.ToString());
                            if (fi.Exists)
                            {
                                //첨부파일 Tiff 변환
                                string file_name = System.IO.Path.GetFileNameWithoutExtension(row.Cells["attachment_path"].Value.ToString()) + ".tif";
                                string save_path = cookie.GetSavePath(file_name);
                                m_Connection.ConvertToTiff(row.Cells["attachment_path"].Value.ToString(), save_path);
                                row.Cells["real_attachment_path"].Value = save_path;
                            }
                            else
                            {
                                messageBox.Show(this, "존재하지 않는 첨부파일입니다!");
                                dgvAttachments.ClearSelection();
                                row.Selected = true;

                                dgvFaxNumber.AllowUserToAddRows = true;
                                dgvFaxNumber.AllowUserToDeleteRows = true;
                                return;
                            }
                        }


                        int taskId = SendFax(row, row.Cells["real_attachment_path"].Value.ToString());
                        if (taskId > 0)
                        {
                            taskIdList[i] = taskId;
                            row.Cells["job_id"].Value = taskId;
                        }
                        // 비동기로 팩스 상태를 확인하고, 성공 또는 실패할 때까지 3초마다 확인
                        tasks.Add(CheckFaxStatusAsync(taskId));
                    }
                }
                else
                {
                    dgvFaxNumber.AllowUserToAddRows = true;
                    dgvFaxNumber.AllowUserToDeleteRows = true;
                    return;
                }
            }

            dgvFaxNumber.AllowUserToAddRows = true;
            dgvFaxNumber.AllowUserToDeleteRows = true;

            // 모든 작업이 완료될 때까지 대기
            await Task.WhenAll(tasks);
            // Marquee 멈춤
            progressBar1.MarqueeAnimationSpeed = 0; // 애니메이션 중지
            // 모든 작업이 완료된 후에 수행할 작업 추가
            MessageBox.Show("모든 작업이 완료되었습니다.");

        }
        private async Task CheckFaxStatusAsync(int taskId)
        {
            while (true)
            {
                // 팩스 상태 확인
                string status = m_Connection.GetSendFaxStatus(taskId);
                // 상태를 DataGridView에 업데이트
                UpdateStatusInUiThread(taskId, status);
                // 성공 또는 실패 상태인지 확인
                if (status.Contains("SUCCESS") || status.Contains("FAILED"))
                    break;
                // 3초 대기
                await Task.Delay(3000);
            }
        }
        private void UpdateStatusInUiThread(int taskId, string st)
        {

            string[] status = st.Split('\n');
            if (status.Length > 0)
            {
                string fax_status = status[7].Replace("STATUS:", "").Trim();
                string send_time = status[5].Replace("TIME:", "").Trim();
                string dial = status[8].Replace("DIAL:", "").Trim();


                if (dgvFaxNumber.InvokeRequired)
                {
                    dgvFaxNumber.BeginInvoke(new Action(() =>
                    {
                        // taskId와 일치하는 행을 찾아 상태를 업데이트
                        foreach (DataGridViewRow row in dgvFaxNumber.Rows)
                        {
                            if (row.Cells["job_id"].Value != null && Convert.ToInt32(row.Cells["job_id"].Value) == taskId)
                            {
                                row.Cells["status"].Value = fax_status;
                                row.Cells["reDial"].Value = dial;
                                if (fax_status == "SUCCESS" || fax_status == "FAILED")
                                {
                                    row.Cells["send_time"].Value = send_time;
                                    progressBar1.Value = progressBar1.Value + 1;
                                }
                                break;
                            }
                        }
                    }));
                }
                else
                {
                    // taskId와 일치하는 행을 찾아 상태를 업데이트
                    foreach (DataGridViewRow row in dgvFaxNumber.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["job_id"].Value) == taskId)
                        {
                            row.Cells["status"].Value = fax_status;
                            row.Cells["reDial"].Value = dial;
                            if (fax_status == "SUCCESS" || fax_status == "FAILED")
                            {
                                row.Cells["send_time"].Value = send_time;
                                progressBar1.Value = progressBar1.Value + 1;
                            }
                            break;
                        }
                    }
                }
            }
        }



        private int SendFax(DataGridViewRow row, string attachment_path)
        {
            Console.WriteLine("Login successful!");

            // 팩스 보내기
            string receiver = row.Cells["fax_number"].Value.ToString(); // 받는 사람 팩스 번호
            string sender = "0512564100"; // 발신자 - 기본값 사용
            string filePath = attachment_path;
            int taskGroupID = 0; // null 값
            int priority = 1; // 보통 우선순위
            int dialTotal = 3; // 다이얼 횟수
            int retryTime = 2; // 재시도 시간
            int pageHeader = 0; // 페이지 헤더 추가
            string timePlan = "NULL"; // 예약 전송 계획
            string timeLimitBegin = "NULL"; // 예약 전송 시작 시간
            string timeLimitEnd = "NULL"; // 예약 전송 종료 시간

            if (rbReservation.Checked)
            {
                if (int.TryParse(cbYear.Text, out int year)
                    && int.TryParse(cbMonth.Text, out int month)
                    && int.TryParse(cbDay.Text, out int day)
                    && int.TryParse(cbHour.Text, out int hour)
                    && int.TryParse(cbMinute.Text, out int minute))
                { 
                    DateTime reservedTime = new DateTime(year, month, day, hour, minute, 0);
                    timePlan = reservedTime.ToString("yyyy-MM-dd HH:mm:ss");
                    timeLimitBegin = reservedTime.ToString("yyyy-MM-dd HH:mm:ss");
                    timeLimitEnd = reservedTime.AddMonths(1).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }


            int taskID = m_Connection.StartSendFax(receiver, sender, filePath, taskGroupID, priority, dialTotal, retryTime, pageHeader, timePlan, timeLimitBegin, timeLimitEnd);
            if (taskID != 0)
            {
                Console.WriteLine("Fax submitted successfully. Task ID: " + taskID);
            }
            else
            {
                Console.WriteLine("Failed to submit fax.");
            }
            return taskID;
        }


        private void btnAttachmentSelect_Click(object sender, EventArgs e)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                Task t = tasks[i];
                if (t.IsCompleted)
                    tasks.Remove(t);
            }

            if (tasks.Count > 0)
            {
                messageBox.Show(this, "팩스 송신중이라 작업할 수 없습니다. 모든작업이 끝나고 작업해주시기 바랍니다.");
                return;
            }

            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Filter = "모든 파일 (*.*) | *.*";
                dlgOpen.Filter += "|이미지 (*.jpg, *.gif, *.png) | *.jpg; *.gif; *.png;";
                dlgOpen.Title = "Files";
                dlgOpen.Multiselect = true; // 파일 다중 선택        
                //dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    for (int i = 0; i < dlgOpen.FileNames.Length; i++)
                    {
                        int n = dgvAttachments.Rows.Add();
                        dgvAttachments.Rows[n].Cells["path"].Value = dlgOpen.FileNames[i];
                    }
                }
            }
        }

        private void btnResultDownload_Click(object sender, EventArgs e)
        {
            //권한확인
            DataTable authorityDt = authorityRepository.GetUserAuthority(um.user_id);
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                if (!common.CheckAuthority(authorityDt, "설정", "FAX", "is_excel"))
                {
                    messageBox.Show(this, "권한이 없습니다!");
                    return;
                }
            }
            if (dgvFaxNumber.Columns.Count > 0)
            {
                dgvFaxNumber.AllowUserToAddRows = false;
                List<string> col_nme = new List<string>();
                for (int i = 0; i < dgvFaxNumber.Columns.Count; i++)
                {
                    if (dgvFaxNumber.Columns[i].Visible && dgvFaxNumber.Columns[i].GetType() == typeof(DataGridViewTextBoxColumn))
                        col_nme.Add(dgvFaxNumber.Columns[i].Name);
                }
                GetExeclColumn(col_nme);
                dgvFaxNumber.AllowUserToAddRows = true;
            }
        }

        private void rbReservation_Click(object sender, EventArgs e)
        {
            if (rbImmediately.Checked)
            {
                cbYear.Enabled = false;
                cbMonth.Enabled = false;
                cbDay.Enabled = false;
                cbHour.Enabled = false;
                cbMinute.Enabled = false;
            }
            else if (rbReservation.Checked)
            {
                cbYear.Enabled = true;
                cbMonth.Enabled = true;
                cbDay.Enabled = true;
                cbHour.Enabled = true;
                cbMinute.Enabled = true;
            }
        }
        #endregion

        #region Drag & Drop
        private void dgvAttachments_DragDrop(object sender, DragEventArgs e)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                Task t = tasks[i];
                if (t.IsCompleted)
                    tasks.Remove(t);
            }

            if (tasks.Count > 0)
            {
                messageBox.Show(this, "팩스 송신중이라 작업할 수 없습니다. 모든작업이 끝나고 작업해주시기 바랍니다.");
                return;
            }

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                int n = dgvAttachments.Rows.Add();
                dgvAttachments.Rows[n].Cells["path"].Value = file;
            }
        }

        private void dgvAttachments_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }
        #endregion

        #region Datagridview event
        private void dgvFaxNumber_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvFaxNumber.Columns[e.ColumnIndex].Name == "file_path")
                {
                    if (dgvFaxNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                        dgvFaxNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                    string file_path = dgvFaxNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    using (OpenFileDialog dlgOpen = new OpenFileDialog())
                    {
                        dlgOpen.Filter = "모든 파일 (*.*) | *.*";
                        dlgOpen.Filter += "|이미지 (*.jpg, *.gif, *.png) | *.jpg; *.gif; *.png;";
                        dlgOpen.Title = "Files";
                        dlgOpen.Multiselect = true; // 파일 다중 선택        
                                                    //dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                        if (dlgOpen.ShowDialog() == DialogResult.OK)
                        {

                            for (int i = 0; i < dlgOpen.FileNames.Length; i++)
                            {
                                file_path += "^" + dlgOpen.FileNames[i];
                            }
                        }
                    }
                    dgvFaxNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = file_path;
                }
            }
        }

        private void dgvFaxNumber_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvFaxNumber.Columns[e.ColumnIndex].Name == "is_complete")
                {
                    if (Convert.ToBoolean(dgvFaxNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                        dgvFaxNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Green;
                    else 
                        dgvFaxNumber.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                }
            }
        }

        private void dgvFaxNumber_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (dgvFaxNumber.Columns[e.ColumnIndex].Name == "btnDialog")
                {
                    using (OpenFileDialog dlgOpen = new OpenFileDialog())
                    {
                        dlgOpen.Filter = "모든 파일 (*.*) | *.*";
                        dlgOpen.Filter += "|이미지 (*.jpg, *.gif, *.png) | *.jpg; *.gif; *.png;";
                        dlgOpen.Filter += "|PDF | *.pdf";
                        dlgOpen.Title = "Files";
                        dlgOpen.Multiselect = true; // 파일 다중 선택        
                                                    //dlgOpen.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                        if (dlgOpen.ShowDialog() == DialogResult.OK)
                        {
                            string file_path = "";
                            for (int i = 0; i < dlgOpen.FileNames.Length; i++)
                                file_path += "," + dlgOpen.FileNames[i];

                            file_path = file_path.Substring(1, file_path.Length - 1);

                            dgvFaxNumber.Rows[e.RowIndex].Cells["attachment_path"].Value = file_path;
                        }
                    }
                }
            }
        }

        private void dgvAttachments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            rbBatchAttachmentFile.Checked = true;
        }
        #endregion

        #region Excel download
        public void GetExeclColumn(List<string> col_name)
        {
            if (col_name.Count == 0)
                return;
            try
            {
                excelApp = new Excel.Application();                                                 //엑셀 어플리케이션 생성
                workBook = excelApp.Workbooks.Add();                                                //워크북 추가
                workSheet = workBook.Worksheets.get_Item(1) as Excel.Worksheet;                     //엑셀 첫번째 워크시트 가져오기
                Microsoft.Office.Interop.Excel.Worksheet wk = workSheet;

                setAutomatic(excelApp, false);

                //Data
                Excel.Range rng = workSheet.get_Range("A1", "CG" + (dgvFaxNumber.Rows.Count + 1));
                object[,] only_data = (object[,])rng.get_Value();

                if (dgvFaxNumber.Rows.Count > 0)
                {
                    int row = dgvFaxNumber.Rows.Count + 1;
                    int column = col_name.Count;
                    object[,] data = new object[row, column];

                    data = only_data;

                    //Header
                    for (int i = 0; i < col_name.Count; i++)
                    {
                        //wk.Cells[1, i + 1].value = dgvProduct.Columns[col_name[i]].HeaderText;

                        data[1, i + 1] = dgvFaxNumber.Columns[col_name[i]].HeaderText;
                    }

                    //row data
                    for (int i = 0; i < dgvFaxNumber.Rows.Count; i++)
                    {
                        for (int j = 0; j < col_name.Count; j++)
                        {
                            if(dgvFaxNumber.Rows[i].Cells[col_name[j]].Value != null)
                                only_data[i + 2, j + 1] = "'" + dgvFaxNumber.Rows[i].Cells[col_name[j]].Value.ToString();
                        }
                    }

                    rng.Value = data;

                }
                //Title
                int col_cnt = col_name.Count;
                Excel.Range rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.Font.Size = 11;
                rg1.Font.Bold = true;
                //Border Line Style
                rg1 = wk.Range[wk.Cells[1, 1], wk.Cells[1, col_cnt]];
                rg1.RowHeight = 18;
                rg1.ColumnWidth = 15;
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                rg1.Borders.Weight = Excel.XlBorderWeight.xlThin;
                rg1.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic, Type.Missing);

                rg1 = wk.Range[wk.Cells[2, 1], wk.Cells[dgvFaxNumber.Rows.Count + 1, col_cnt]];
                rg1.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                //속도개선 ON
                setAutomatic(excelApp, true);
                excelApp.Visible = true;
            }
            catch (Exception ex)
            {
                messageBox.Show(this, ex.Message.ToString() + "\n 생성 중 에러가 발생하였습니다.");
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                Task t = tasks[i];
                if (t.IsCompleted)
                    tasks.Remove(t);
            }

            if (tasks.Count > 0)
            {
                messageBox.Show(this, "팩스 송신중이라 작업할 수 없습니다. 모든작업이 끝나고 작업해주시기 바랍니다.");
                return;
            }
            this.Dispose();
        }

        private void FaxManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            messageBox.Show(this, "팩스 송신중이라 작업할 수 없습니다. 모든작업이 끝나고 작업해주시기 바랍니다.");
            m_Connection.Logout();
            e.Cancel = true;
        }

        
    }
}
