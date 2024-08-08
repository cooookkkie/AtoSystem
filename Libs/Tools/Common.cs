using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Encoder = System.Drawing.Imaging.Encoder;
using System.Security.AccessControl;
using System.Security.Principal;
using Spire.Pdf;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;
using iTextSharp.xmp.impl.xpath;
using System.Configuration;
using System.Threading;
using System.Security.Policy;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace Libs.Tools
{
    public class Common
    {     

        public bool IsCheckFaxServer()
        {
            bool networkState = NetworkInterface.GetIsNetworkAvailable();
            bool pingResult = true;

            //네트워크가 연결이 되어있다면
            if (networkState)
            {
                string faxIp = "";
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConfigurationSection section = config.GetSection("connectionStrings");
                if (section != null)
                {
                    if (!section.IsReadOnly())
                    {
                        if (section.SectionInformation.IsProtected == false)
                        {
                            section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                            section.SectionInformation.ForceSave = true;
                            config.Save(ConfigurationSaveMode.Full);
                        }
                    }
                    faxIp = config.ConnectionStrings.ConnectionStrings["FaxServerIp"].ToString();

                }

                if (!string.IsNullOrEmpty(faxIp))
                {
                    Ping pingSender = new Ping();

                    //Ping 체크 (IP, TimeOut 지정)
                    PingReply reply = pingSender.Send(faxIp, 300);

                    //상태가 Success이면 true반환
                    pingResult = reply.Status == IPStatus.Success;
                }

            }

            return networkState & pingResult;
        }
        public CFXCOMLib.Connection LoginFaxServer(string id, string pw)
        {
            CFXCOMLib.Connection m_Connection;
            m_Connection = new CFXCOMLib.Connection();
            bool networkState = NetworkInterface.GetIsNetworkAvailable();
            bool pingResult = true;

            //네트워크가 연결이 되어있다면
            if (networkState)
            {
                string faxIp = "";
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                ConfigurationSection section = config.GetSection("connectionStrings");
                if (section != null)
                {
                    if (!section.IsReadOnly())
                    {
                        if (section.SectionInformation.IsProtected == false)
                        {
                            section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                            section.SectionInformation.ForceSave = true;
                            config.Save(ConfigurationSaveMode.Full);
                        }
                    }
                    faxIp = config.ConnectionStrings.ConnectionStrings["FaxServerIp"].ToString();
                }
                

                if (!string.IsNullOrEmpty(faxIp))
                {
                    Ping pingSender = new Ping();

                    //Ping 체크 (IP, TimeOut 지정)
                    PingReply reply = pingSender.Send(faxIp, 300);

                    //상태가 Success이면 true반환
                    pingResult = reply.Status == IPStatus.Success;
                }

                m_Connection.Login(faxIp, id, pw);

            }

            return m_Connection;
        }


        public bool CheckAuthority(DataTable authorityDt, string group_name, string form_name, string col_name)
        {
            bool isFlag = false;
            //권한확인
            if (authorityDt != null && authorityDt.Rows.Count > 0)
            {
                //유저별 권한가져오기
                DataRow[] dr = authorityDt.Select("id = 1");
                if (dr.Length > 0)
                    authorityDt = dr.CopyToDataTable();
                //없으면 부서별 권한가져오기
                else
                {
                    dr = authorityDt.Select("id = 2");
                    authorityDt = dr.CopyToDataTable();
                }
                authorityDt.AcceptChanges();

                //사용가능한 메뉴 설정
                foreach (DataRow ddr in authorityDt.Rows)
                {
                    if (group_name == ddr["group_name"].ToString() && form_name == ddr["form_name"].ToString())
                    {
                         if (!bool.TryParse(ddr[col_name].ToString(), out isFlag))
                            isFlag = false;
                        break;
                    }
                }
            }
            return isFlag;
        }
        public string AddSlashes(string InputTxt)
        {
            // List of characters handled:
            // \000 null  
            // \010 backspace
            // \011 horizontal tab
            // \012 new line
            // \015 carriage return
            // \032 substitute
            // \042 double quote
            // \047 single quote
            // \134 backslash
            // \140 grave accent

            string Result = InputTxt;
            try
            {
                Result = System.Text.RegularExpressions.Regex.Replace(InputTxt, @"[\000\010\011\012\015\032\042\047\134\140]", "\\$0");
            }
            catch (Exception Ex)
            {
                // handle any exception here
                Console.WriteLine(Ex.Message);
            }
            return Result;
        }
        public bool isContains(string tempString1, string tempString2)
        {
            return tempString1.IndexOf(tempString2, StringComparison.OrdinalIgnoreCase) >= 0;
            //return false;
        }

        public int GetWeekOfYear(DateTime sourceDate, CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
            {
                cultureInfo = CultureInfo.CurrentCulture;
            }

            CalendarWeekRule calendarWeekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;

            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;

            return cultureInfo.Calendar.GetWeekOfYear(sourceDate, calendarWeekRule, firstDayOfWeek);
        }

        public DateTime GetFirstDateOfWeek(int year, int week)
        {
            DateTime firstDateOfYear = new DateTime(year, 1, 1);
            DateTime firstDateOfFirstWeek = firstDateOfYear.AddDays(7 - (int)(firstDateOfYear.DayOfWeek) + 1);
            return firstDateOfFirstWeek.AddDays(7 * (week - 1));
        }
        public string whereSql(string whrColumn, string whrValue)
        {
            string[] tempStr = null;
            string tempWhr = "";
            string whrStr = "";
            if (!string.IsNullOrEmpty(whrValue.Trim()))
            {
                tempStr = whrValue.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   {whrColumn} LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR {whrColumn} LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    whrStr = $"\n	 AND ( {tempWhr} )";
                }
                else
                {
                    whrStr = $"\n	  AND {whrColumn} LIKE '%{whrValue}%'";
                }
            }
            return whrStr;
        }

        #region Capture
        public void ScreenCaptureForm(Point _point, Size _size, string save_path)
        {
            Rectangle rectangle = new Rectangle(_point, _size);

            Bitmap bitmap = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics g = Graphics.FromImage(bitmap);

            g.CopyFromScreen(rectangle.Left, rectangle.Top, 0, 0, rectangle.Size);

            bitmap.Save(save_path, ImageFormat.Png);
            bitmap.Dispose();
        }

        #endregion

        #region Version check
        public void VersionUpgrade()
        {
            string clientFile = "http://atotrading.iptime.org/AtoSystemSettup.exe";
            string localFile = @"C:\Users\아토30\Desktop\setUp.exe";
            DownloadFile(localFile, clientFile);   // localFile 존재 시 덮어쓰기 됨
        }

        private void DownloadFile(string localFile, string clientFile)
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(fileDownloader_DownloadFileCompleted);   // 다운로드 완료 후
                client.DownloadFileAsync(new Uri(clientFile), localFile);
            }
            // 에러발생 시
            catch (Exception error) { System.Windows.Forms.MessageBox.Show(error.Message); }
        }

        void fileDownloader_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string tmpSetupPath = e.UserState.ToString();
            Console.WriteLine("다운로드 완료 : " + tmpSetupPath);
            Process.Start(tmpSetupPath);    // 다운받은 파일을 실행하고 
        }

        public string GetCurrentVersion(string strDeployUrl)
        {
            string version = "";
            XmlDocument xmlDoc = new XmlDocument();

            Uri site = new Uri(strDeployUrl);
            try
            {
                HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(site);
                wReq.ReadWriteTimeout = 1000;
                wReq.UserAgent = ".NET FrameworkExample Client";

                HttpWebResponse wResp = (HttpWebResponse)wReq.GetResponse();
                Stream respStream = wResp.GetResponseStream();
                StreamReader reader = new StreamReader(respStream, Encoding.Default);
                String respHTML = reader.ReadToEnd();

                reader.Close();
                wResp.Close();
                xmlDoc.LoadXml(respHTML);
                version = xmlDoc.ChildNodes[1].FirstChild.Attributes["version"].Value;
                xmlDoc = null;
            }
            catch (Exception)
            {
                version = "";
            }
            return version;
        }
        public string GetAppVer()
        {
            try
            {
                //게시(Clickonce 등 일 경우) 응용프로그램 버전
                //return string.Format("{0} (배포버전)",
                return string.Format("{0}",
                    ApplicationDeployment.CurrentDeployment.CurrentVersion);
            }
            catch
            {
                //로컬 빌드 버전일 경우 (현재 어셈블리 버전)
                //return string.Format("{0} (빌드버전)",
                return string.Format("{0}",
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            }
        }
        #endregion

        #region Datagridview, Form capture
        public Bitmap ScreenCaptureForm(Point _point, Size _size)
        {
            Rectangle rec = new Rectangle(_point, _size);

            Bitmap bitmap = new Bitmap(rec.Width, rec.Height);
            Graphics g = Graphics.FromImage(bitmap);

            g.CopyFromScreen(rec.Left, rec.Top, 0, 0, rec.Size);

            return bitmap;
        }
        public void GetDgvSelectCellsCapture(DataGridView dgv)
        {
            dgv.EndEdit();
            if (dgv.SelectedCells.Count > 0)
            {
                int row_max = 0, row_min = 9999;
                int col_max = 0, col_min = 9999;
                for (int i =0; i < dgv.Rows.Count; i++)
                {
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        if (dgv.Rows[i].Cells[j].Selected)
                        { 
                            if(i >= row_max)
                                row_max = i;
                            if(i <= row_min)
                                row_min = i;
                            if(j >= col_max)
                                col_max = j;
                            if(j <= col_min)
                                col_min = j;
                        }
                    }
                }

                //DataGridViewCell first_cell = dgv.SelectedCells[dgv.SelectedCells.Count - 1];
                int first_display_scroll_index = dgv.FirstDisplayedScrollingRowIndex;
                DataGridViewCell first_cell = dgv.Rows[row_min].Cells[col_min];
                Point p = dgv.PointToScreen(dgv.GetCellDisplayRectangle(first_cell.ColumnIndex, -1, false).Location);

                int width = 0;
                foreach (DataGridViewCell cell in dgv.Rows[first_cell.RowIndex].Cells)
                {
                    if (cell.Selected == true)
                    {
                        width += cell.Size.Width;
                    }
                }
                int height = dgv.ColumnHeadersHeight;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    DataGridViewCell cell = dgv.Rows[i].Cells[first_cell.ColumnIndex];
                    if (cell.Selected == true)
                    {
                        height += cell.Size.Height;
                    }
                    else
                    {
                        dgv.Rows[i].Visible = false;
                    }
                }
                Size s = new Size(width, height);

                dgv.ClearSelection();
                dgv.Update();
                Bitmap bt = ScreenCaptureForm(p, s);
                Clipboard.SetImage(bt);
                //다시 원래상태
                for (int i = 0; i < dgv.Rows.Count; i++)
                    dgv.Rows[i].Visible = true;
                dgv.CurrentCell = first_cell;
                dgv.FirstDisplayedScrollingRowIndex = first_display_scroll_index;
            }
        }

        public void GetDgvSelectCellsCapture(Form form, DataGridView dgv)
        {
            dgv.EndEdit();
            if (dgv.SelectedCells.Count > 0)
            {
                int row_max = 0, row_min = 9999;
                int col_max = 0, col_min = 9999;

                foreach (DataGridViewCell cell in dgv.SelectedCells)
                {
                    if (cell.RowIndex >= row_max)
                        row_max = cell.RowIndex;
                    if (cell.RowIndex <= row_min)
                        row_min = cell.RowIndex;
                    if (cell.ColumnIndex >= col_max)
                        col_max = cell.ColumnIndex;
                    if (cell.ColumnIndex <= col_min)
                        col_min = cell.ColumnIndex;
                }
                //DataGridViewCell first_cell = dgv.SelectedCells[dgv.SelectedCells.Count - 1];
                int first_display_scroll_index = dgv.FirstDisplayedScrollingRowIndex;
                DataGridViewCell first_cell = dgv.Rows[row_min].Cells[col_min];
                Point p = dgv.PointToScreen(dgv.GetCellDisplayRectangle(first_cell.ColumnIndex, -1, false).Location);

                int width = 0;
                foreach (DataGridViewCell cell in dgv.Rows[first_cell.RowIndex].Cells)
                {
                    if (cell.Selected == true)
                    {
                        width += cell.Size.Width;
                    }
                }
                int height = dgv.ColumnHeadersHeight;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    DataGridViewCell cell = dgv.Rows[i].Cells[first_cell.ColumnIndex];
                    if (cell.Selected == true)
                    {
                        height += cell.Size.Height;
                    }
                    else
                    {
                        dgv.Rows[i].Visible = false;
                    }
                }
                Size s = new Size(width, height);

                form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                int pre_height = form.Height;
                if (form.Height < p.Y + s.Height)
                    form.Height += form.Height + (10000);
                dgv.ClearSelection();


                Thread.Sleep(500);
                dgv.Update();
                Bitmap bt = ScreenCaptureForm(p, s);
                Clipboard.SetImage(bt);
                //다시 원래상태
                foreach (DataGridViewRow row in dgv.Rows)
                    row.Visible = true;
                dgv.CurrentCell = first_cell;
                dgv.FirstDisplayedScrollingRowIndex = first_display_scroll_index;
                form.Height = pre_height;
            }
        }
        #endregion

        #region PageDocumnet Pages count
        //Count pages
        public int GetPageCount(PrintDocument pd)
        {
            int count = 0;
            PrintController original = pd.PrintController;
            pd.PrintController = new PreviewPrintController();
            pd.PrintPage += (sender, e) => count++;
            pd.Print();
            pd.PrintController = original;
            return count;
        }

        #endregion


        public DataTable ConvertListToDatatable(IEnumerable<dynamic> v)
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
        public DataTable ConvertSelectListToDatatable(DataGridView dgv)
        {
            try
            {
                if (dgv.ColumnCount == 0)
                    return null;
                DataTable dtSource = new DataTable();
                //////create columns
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (col.ValueType == null)
                        dtSource.Columns.Add(col.Name, typeof(string));
                    else
                        dtSource.Columns.Add(col.Name, col.ValueType);
                    dtSource.Columns[col.Name].Caption = col.HeaderText;
                }
                ///////insert row data
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Selected)
                    {
                        DataRow drNewRow = dtSource.NewRow();
                        foreach (DataColumn col in dtSource.Columns)
                        {
                            drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value;
                        }
                        dtSource.Rows.Add(drNewRow);
                    }
                }
                return dtSource;
            }
            catch
            {
                return null;
            }
        }
        public static DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            object[] values = new object[props.Count];

            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }
            return table;
        }
        public DataTable ConvertDgvToDataTable(DataGridView data)
        {
            
            DataTable table = new DataTable();

            if (data.Rows.Count > 0)
            { 
                //Set Columns
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    table.Columns.Add(data.Columns[i].Name, typeof(string));
                }
                //Data insert
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    object[] values = new object[data.Columns.Count];
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        values[j] = data.Rows[i].Cells[j].Value;
                    }
                    table.Rows.Add(values);
                }
            }
            return table;
        }
        public DataTable ConvertDgvToDataTable(DataGridView data, bool only_select = false)
        {

            DataTable table = new DataTable();

            if (data.Rows.Count > 0)
            {
                //Set Columns
                int col_cnt = 0;
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    if (data.Rows[0].Cells[i].Selected)
                    {
                        table.Columns.Add(data.Columns[i].Name, typeof(string));
                        col_cnt++;
                    }
                }
                //Data insert
                for (int i = 0; i < data.Rows.Count - 1; i++)
                {
                    object[] values = new object[col_cnt];
                    for (int j = 0; j < data.Columns.Count; j++)
                    {
                        if (data.Rows[0].Cells[j].Selected)
                            values[j] = data.Rows[i].Cells[j].Value;
                    }
                    table.Rows.Add(values);
                }
            }
            return table;
        }
        public DataTable ConvertDgvToDataTableColumn(DataGridView data)
        {

            DataTable table = new DataTable();

            if (data.ColumnCount > 0)
            {
                //Set Columns
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    table.Columns.Add(data.Columns[i].Name, typeof(string));
                }
            }
            return table;
        }

        #region 시작프로그램 등록/해제
        // 부팅시 시작 프로그램을 등록하는 레지스트리 경로
        private static readonly string _startupRegPath =
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        private Microsoft.Win32.RegistryKey GetRegKey(string regPath, bool writable)
        {
            return Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regPath, writable);
        }

        // 부팅시 시작 프로그램 등록
        public void AddStartupProgram(string programName, string executablePath)
        {
            using (var regKey = GetRegKey(_startupRegPath, true))
            {
                try
                {
                    // 키가 이미 등록돼 있지 않을때만 등록
                    if (regKey.GetValue(programName) == null)
                        regKey.SetValue(programName, executablePath);

                    regKey.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // 등록된 프로그램 제거
        public void RemoveStartupProgram(string programName)
        {
            using (var regKey = GetRegKey(_startupRegPath, true))
            {
                try
                {
                    // 키가 이미 존재할때만 제거
                    if (regKey.GetValue(programName) != null)
                        regKey.DeleteValue(programName, false);

                    regKey.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        #endregion


        #region 캡쳐기능

        // 사용예: ScreenCopy.Copy("test.png");
        // 
        public static void Copy(string outputFilename)
        {
            // 주화면의 크기 정보 읽기
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            // 2nd screen = Screen.AllScreens[1]

            // 픽셀 포맷 정보 얻기 (Optional)
            int bitsPerPixel = Screen.PrimaryScreen.BitsPerPixel;
            PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
            if (bitsPerPixel <= 16)
            {
                pixelFormat = PixelFormat.Format16bppRgb565;
            }
            if (bitsPerPixel == 24)
            {
                pixelFormat = PixelFormat.Format24bppRgb;
            }

            // 화면 크기만큼의 Bitmap 생성
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, pixelFormat);

            // Bitmap 이미지 변경을 위해 Graphics 객체 생성
            using (Graphics gr = Graphics.FromImage(bmp))
            {
                // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                gr.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
            }

            // Bitmap 데이타를 파일로 저장
            bmp.Save(outputFilename);

            System.Drawing.Image returnImage = null;
            if (Clipboard.ContainsImage())
            {
                returnImage = Clipboard.GetImage();
                Clipboard.SetImage(bmp);
            }

            Clipboard.SetImage(bmp);

            bmp.Dispose();
        }
        #endregion


        #region  gridview Datatable 변환
        public static DataTable GetDataGridViewAsDataTable(DataGridView _DataGridView)
        {
            try
            {
                if (_DataGridView.ColumnCount == 0)
                    return null;
                DataTable dtSource = new DataTable();
                //////create columns
                foreach (DataGridViewColumn col in _DataGridView.Columns)
                {
                    if (col.ValueType == null)
                        dtSource.Columns.Add(col.Name, typeof(string));
                    else
                        dtSource.Columns.Add(col.Name, col.ValueType);
                    dtSource.Columns[col.Name].Caption = col.HeaderText;
                }
                ///////insert row data
                foreach (DataGridViewRow row in _DataGridView.Rows)
                {
                    if (row.Index < _DataGridView.Rows.Count - 1)
                    {
                        DataRow drNewRow = dtSource.NewRow();
                        foreach (DataColumn col in dtSource.Columns)
                        {
                            drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value;
                        }
                        dtSource.Rows.Add(drNewRow);
                    }
                }
                return dtSource;
            }
            catch
            {
                return null;
            }
        }

        public static DataTable GetDataGridViewAsDataTable2(DataGridView _DataGridView)
        {
            try
            {
                if (_DataGridView.ColumnCount == 0)
                    return null;
                DataTable dtSource = new DataTable();
                //////create columns
                foreach (DataGridViewColumn col in _DataGridView.Columns)
                {
                    if (col.ValueType == null)
                        dtSource.Columns.Add(col.Name, typeof(string));
                    else
                        dtSource.Columns.Add(col.Name, col.ValueType);
                    dtSource.Columns[col.Name].Caption = col.HeaderText;
                }
                ///////insert row data
                foreach (DataGridViewRow row in _DataGridView.Rows)
                {
                    if (row.Index < _DataGridView.Rows.Count)
                    {
                        DataRow drNewRow = dtSource.NewRow();
                        foreach (DataColumn col in dtSource.Columns)
                        {
                            if (row.Cells[col.ColumnName].ValueType == typeof(bool))
                            {
                                bool isCheck;
                                if (row.Cells[col.ColumnName].Value == null || !bool.TryParse(row.Cells[col.ColumnName].Value.ToString(), out isCheck))
                                    isCheck = false;
                                drNewRow[col.ColumnName] = isCheck.ToString();
                            }
                            else
                                drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value;
                        }
                        dtSource.Rows.Add(drNewRow);
                    }
                }
                return dtSource;
            }
            catch
            {
                return null;
            }
        }
        #endregion


        #region
        public static string autoHypehen(string tel)
        {
            if (string.IsNullOrEmpty(tel))
            {
                tel = "051-256-3100";
            }
            string tmpTel = tel.Replace("-", "");
            string tel1 = string.Empty;
            string tel2 = string.Empty;
            string tel3 = string.Empty;
            string tel1_total = string.Empty;

            if (tmpTel.Length > -2 && tmpTel.Length < 8)
            {
                if (tmpTel.Substring(0, 2) != "02")
                {
                    if (tmpTel.Length == 3)
                    {
                        tel1_total = tmpTel + "-";
                    }
                    else if (tmpTel.Length > 3 && tmpTel.Length < 6)
                    {
                        tel1 = tmpTel.Substring(0, 3);
                        tel2 = tmpTel.Substring(3, tmpTel.Length - 3);
                        tel1_total = tel1 + "-" + tel2;
                    }
                    else if (tmpTel.Length > 3 && tmpTel.Length > 6)
                    {
                        tel1 = tmpTel.Substring(0, 3);
                        tel2 = tmpTel.Substring(3, 3);
                        tel3 = tmpTel.Substring(6, tmpTel.Length - 6);
                        tel1_total = tel1 + "-" + tel2 + "-" + tel3;
                    }
                    else
                    {
                        tel1_total = tel;
                    }
                }
                else
                {
                    if (tmpTel.Length == 2)
                    {
                        tel1_total = tmpTel + "-";
                    }
                    else if (tmpTel.Length > 2 && tmpTel.Length < 6)
                    {
                        tel1 = tmpTel.Substring(0, 2);
                        tel2 = tmpTel.Substring(2, tmpTel.Length - 2);
                        tel1_total = tel1 + "-" + tel2;
                    }
                    else if (tmpTel.Length > 2 && tmpTel.Length > 5)
                    {
                        tel1 = tmpTel.Substring(0, 2);
                        tel2 = tmpTel.Substring(2, 3);
                        tel2 = tmpTel.Substring(5, tmpTel.Length - 5);
                        tel1_total = tel1 + "-" + tel2 + "-" + tel3;
                    }
                }
            }
            else if (tmpTel.Length == 8 && tmpTel.Substring(0, 2) == "02")
            {
                tel1 = tmpTel.Substring(0, 2);
                tel2 = tmpTel.Substring(2, 3);
                tel3 = tmpTel.Substring(3, 3);
                tel1_total = tel1 + "-" + tel2 + "-" + tel3;
            }
            else if (tmpTel.Length == 8 && tmpTel.Substring(0, 2) != "02")
            {
                tel1 = tmpTel.Substring(0, 4);
                tel2 = tmpTel.Substring(4, 4);
                tel1_total = tel1 + "-" + tel2;
            }

            else if (tmpTel.Length == 9 && tmpTel.Substring(0, 2) == "02")
            {
                tel1 = tmpTel.Substring(0, 2);
                tel2 = tmpTel.Substring(2, 3);
                tel3 = tmpTel.Substring(5, 4);
                tel1_total = tel1 + "-" + tel2 + "-" + tel3;
            }
            else if (tmpTel.Length == 9 && tmpTel.Substring(0, 2) != "02")
            {
                tel1 = tmpTel.Substring(0, 3);
                tel2 = tmpTel.Substring(3, 4);
                tel3 = tmpTel.Substring(7, 2);
                tel1_total = tel1 + "-" + tel2 + "-" + tel3;
            }

            else if (tmpTel.Length == 10 && tmpTel.Substring(0, 2) == "02")
            {
                tel1 = tmpTel.Substring(0, 2);
                tel2 = tmpTel.Substring(2, 4);
                tel3 = tmpTel.Substring(6, 4);
                tel1_total = tel1 + "-" + tel2 + "-" + tel3;
            }
            else if (tmpTel.Length == 10 && tmpTel.Substring(0, 2) != "02")
            {
                tel1 = tmpTel.Substring(0, 3);
                tel2 = tmpTel.Substring(3, 3);
                tel3 = tmpTel.Substring(6, 4);
                tel1_total = tel1 + "-" + tel2 + "-" + tel3;
            }
            else if (tmpTel.Length == 11)
            {
                tel1 = tmpTel.Substring(0, 3);
                tel2 = tmpTel.Substring(3, 4);
                tel3 = tmpTel.Substring(7, 4);
                tel1_total = tel1 + "-" + tel2 + "-" + tel3;
            }
            else
            {
                tel1_total = tel;
            }

            return tel1_total;
        }
        #endregion

        #region Input Box
        public DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textbox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();
            form.Text = title;
            label.Text = promptText;
            textbox.Text = value;
            buttonOk.Text = "Ok";
            buttonCancel.Text = "Canlcel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;
            label.SetBounds(9, 20, 372, 13);
            textbox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);
            label.AutoSize = true;
            textbox.Anchor = textbox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textbox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;
            DialogResult dialogResult = form.ShowDialog();
            value = textbox.Text;
            return dialogResult;
        }
        #endregion


        #region 영업일 Dictionary 만들기
        Dictionary<int, DateTime> workingDic = new Dictionary<int, DateTime>();
        public string GetWorkingDate(int enable_sales)
        {
        retry:
            if (enable_sales > 365)
                return "1년 이상 판매가능";
            else if (workingDic.ContainsKey(enable_sales))
                return workingDic[enable_sales].ToString("yyyy-MM-dd");
            else
            {
                SetWorkingDate();
                goto retry;
            }

        }
        private void SetWorkingDate()
        {
            workingDic = new Dictionary<int, DateTime>();
            DateTime tempDate = DateTime.Now;
            int days = 0;
            while (days <= 365)
            {
                if (!HolidayCheck(tempDate))
                {
                    workingDic.Add(days, tempDate);
                    days++;
                }
                tempDate = tempDate.AddDays(1);
            }
        }

        #endregion


        #region 음력 날짜만들기(공휴일 포함)
        List<DateTime> holiday = new List<DateTime>();
        Dictionary<int, List<DateTime>> yearDic = new Dictionary<int, List<DateTime>>();
        public void GetShortDay3(DateTime sdate, double stock, double sales, DataRow[] pendingDt, out string sttDate, out string endDate)
        {
            string sttdate = null;
            string enddate = null;
            bool isHolidays = false;
            //stock += sales;
            retry:
            if (stock <= 0 && sales <= 0 && (pendingDt == null || pendingDt.Length == 0))
            {
                sttdate = sdate.ToString("yyyy-MM-dd");
                enddate = "예정없음";
            }
            else if (stock > 0 && sales <= 0)
            {
                sttdate = "3000-01-01";
                enddate = "";
            }
            else if (stock <= 0 && sales <= 0)
            {
                sttdate = sdate.ToString("yyyy-MM-dd");
                enddate = "";
            }
            else if (stock <= 0 && sales > 0 && pendingDt != null && pendingDt.Length > 0)
            {
                sttdate = sdate.ToString("yyyy-MM-dd");   
                enddate = pendingDt[0]["pending_date"].ToString();

                bool isRe = false;
                for (int i = 0; i < pendingDt.Length; i++)
                {
                    if (!string.IsNullOrEmpty(pendingDt[i]["pending_date"].ToString()) && Convert.ToDateTime(pendingDt[i]["pending_date"].ToString()) < sdate)
                    {
                        pendingDt[i]["pending_date"] = "";
                        stock += Convert.ToDouble(pendingDt[i]["quantity_on_paper"].ToString());
                        isRe = true;
                    }
                }
                if (isRe)
                    goto retry; 
            }
            else if (stock > 0 && sales > 0 && (stock / sales) > 365)
            {
                sttdate = "1년이상 판매가능";
                enddate = "";
            }
            else
            {
                DateTime tmpDate = sdate;
                int days = 0;
                while (sttdate == null && enddate == null)
                {
                    tmpDate = sdate.AddDays(days);
                    isHolidays = false;

                    if (!yearDic.ContainsKey(tmpDate.Year))
                    {
                        //음력계산 법정공휴일
                        //신정
                        DateTime dt = new DateTime(tmpDate.Year - 1, 12, 30);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 1, 1);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 1, 2);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        //석가탄신일
                        dt = new DateTime(tmpDate.Year, 4, 8);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 8, 14);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 8, 15);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 8, 16);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        //양력 법정공휴일
                        //신정
                        dt = new DateTime(tmpDate.Year, 1, 1);
                        holiday.Add(dt);
                        //삼일절
                        dt = new DateTime(tmpDate.Year, 3, 1);
                        holiday.Add(dt);
                        //어린이날
                        dt = new DateTime(tmpDate.Year, 5, 5);
                        holiday.Add(dt);
                        //현충일
                        dt = new DateTime(tmpDate.Year, 6, 6);
                        holiday.Add(dt);
                        //광복절
                        dt = new DateTime(tmpDate.Year, 8, 15);
                        holiday.Add(dt);
                        //개천절
                        dt = new DateTime(tmpDate.Year, 10, 3);
                        holiday.Add(dt);
                        //한글날
                        dt = new DateTime(tmpDate.Year, 10, 9);
                        holiday.Add(dt);
                        //크리스마스
                        dt = new DateTime(tmpDate.Year, 12, 25);
                        holiday.Add(dt);

                        yearDic.Add(tmpDate.Year, holiday);
                    }
                    //주말
                    if (tmpDate.DayOfWeek == DayOfWeek.Saturday || tmpDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        isHolidays = true;
                    }
                    //특수, q 공휴일
                    else if (HolidayCheck(tmpDate))
                    {
                        isHolidays = true;
                    }
                    else
                    {
                        List<DateTime> tempHoliday = yearDic[tmpDate.Year];
                        for (int j = 0; j < tempHoliday.Count; j++)
                        {
                            if (tempHoliday[j].ToString("yyyy-MM-dd") == tmpDate.ToString("yyyy-MM-dd"))
                            {
                                isHolidays = true;
                                break;
                            }
                        }
                    }

                    //입고내역
                    if (pendingDt != null)
                    { 
                        for (int j = 0; j < pendingDt.Length; j++)
                        {
                            DateTime pending_date;
                            if (DateTime.TryParse(pendingDt[j]["pending_date"].ToString(), out pending_date))
                            {
                                if (tmpDate >= pending_date)
                                {
                                    pendingDt[j]["pending_date"] = "";
                                    stock += Convert.ToDouble(pendingDt[j]["quantity_on_paper"].ToString());
                                }
                            }
                        }
                    }

                    //영업일
                    if (!isHolidays)
                    {
                        stock -= sales;
                    }

                    //쇼트기간
                    if (stock <= 0 && sttdate == null)
                    {
                        sttdate = tmpDate.ToString("yyyy-MM-dd");
                        //입고내역
                        enddate = "예정없음";
                        if (pendingDt != null)
                        {
                            for (int j = 0; j < pendingDt.Length; j++)
                            {
                                DateTime pending_date;
                                if (DateTime.TryParse(pendingDt[j]["pending_date"].ToString(), out pending_date))
                                {
                                    enddate = pendingDt[j]["pending_date"].ToString();
                                    break;
                                }
                            }
                        }

                        sttDate = sttdate;
                        endDate = enddate;
                        return;
                    }

                    days++;
                }
            }
            sttDate = sttdate;
            endDate = enddate;
            return;
        }
        public void GetShortDay2(DateTime sdate, double stock, double sales, out DateTime workDays)
        {
            if (stock == 0)
            {
                workDays = sdate;
                return;
            }

            int tmpDays = 0;
            bool isHolidays = false;
            workDays = sdate;
            //stock += sales;
            if (stock / sales > 3650 || sales < 0)
            {
                workDays = new DateTime(3000, 1, 1);
            }
            else
            { 

                while (stock > 0)
                {
                    DateTime tmpDate = sdate.AddDays(tmpDays);
                    isHolidays = false;

                    if (!yearDic.ContainsKey(tmpDate.Year))
                    {
                        //음력계산 법정공휴일
                        //신정
                        DateTime dt = new DateTime(tmpDate.Year - 1, 12, 30);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 1, 1);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 1, 2);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        //석가탄신일
                        dt = new DateTime(tmpDate.Year, 4, 8);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 8, 14);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 8, 15);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        dt = new DateTime(tmpDate.Year, 8, 16);
                        dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                        holiday.Add(dt);
                        //양력 법정공휴일
                        //신정
                        dt = new DateTime(tmpDate.Year, 1, 1);
                        holiday.Add(dt);
                        //삼일절
                        dt = new DateTime(tmpDate.Year, 3, 1);
                        holiday.Add(dt);
                        //어린이날
                        dt = new DateTime(tmpDate.Year, 5, 5);
                        holiday.Add(dt);
                        //현충일
                        dt = new DateTime(tmpDate.Year, 6, 6);
                        holiday.Add(dt);
                        //광복절
                        dt = new DateTime(tmpDate.Year, 8, 15);
                        holiday.Add(dt);
                        //개천절
                        dt = new DateTime(tmpDate.Year, 10, 3);
                        holiday.Add(dt);
                        //한글날
                        dt = new DateTime(tmpDate.Year, 10, 9);
                        holiday.Add(dt);
                        //크리스마스
                        dt = new DateTime(tmpDate.Year, 12, 25);
                        holiday.Add(dt);

                        yearDic.Add(tmpDate.Year, holiday);
                    }

                    //주말
                    if (tmpDate.DayOfWeek == DayOfWeek.Saturday || tmpDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        isHolidays = true;
                    }
                    //특수, q 공휴일
                    else if (HolidayCheck(tmpDate))
                    {
                        isHolidays = true;
                    }
                    else
                    {
                        List<DateTime> tempHoliday = yearDic[tmpDate.Year];
                        for (int j = 0; j < tempHoliday.Count; j++)
                        {
                            if (tempHoliday[j].ToString("yyyy-MM-dd") == tmpDate.ToString("yyyy-MM-dd"))
                            {
                                isHolidays = true;
                                break;
                            }
                        }
                    }

                    //재고반영
                    if (!isHolidays)
                    {
                        stock -= sales;
                        if (stock <= 0)
                        {
                            break;
                        }
                    }
                    tmpDays += 1;
                }
            }
            workDays = workDays.AddDays(tmpDays);
        }

        public void GetShortDay(DateTime sdate, int days, out DateTime workDays)
        {
            int tmpDays = 0;
            workDays = sdate;
            while (tmpDays < days)
            {
                DateTime tmpDate = sdate.AddDays(tmpDays);
                tmpDays += 1;
                if (!yearDic.ContainsKey(tmpDate.Year))
                {
                    //음력계산 법정공휴일
                    //신정
                    DateTime dt = new DateTime(tmpDate.Year - 1, 12, 30);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(tmpDate.Year, 1, 1);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(tmpDate.Year, 1, 2);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    //석가탄신일
                    dt = new DateTime(tmpDate.Year, 4, 8);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(tmpDate.Year, 8, 14);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(tmpDate.Year, 8, 15);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(tmpDate.Year, 8, 16);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    //양력 법정공휴일
                    //신정
                    dt = new DateTime(tmpDate.Year, 1, 1);
                    holiday.Add(dt);
                    //삼일절
                    dt = new DateTime(tmpDate.Year, 3, 1);
                    holiday.Add(dt);
                    //어린이날
                    dt = new DateTime(tmpDate.Year, 5, 5);
                    holiday.Add(dt);
                    //현충일
                    dt = new DateTime(tmpDate.Year, 6, 6);
                    holiday.Add(dt);
                    //광복절
                    dt = new DateTime(tmpDate.Year, 8, 15);
                    holiday.Add(dt);
                    //개천절
                    dt = new DateTime(tmpDate.Year, 10, 3);
                    holiday.Add(dt);
                    //한글날
                    dt = new DateTime(tmpDate.Year, 10, 9);
                    holiday.Add(dt);
                    //크리스마스
                    dt = new DateTime(tmpDate.Year, 12, 25);
                    holiday.Add(dt);

                    yearDic.Add(tmpDate.Year, holiday);
                }

                //주말
                if (tmpDate.DayOfWeek == DayOfWeek.Saturday || tmpDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    tmpDays += 1;
                }
                //특수, q 공휴일
                else if (HolidayCheck(tmpDate))
                {
                    tmpDays += 1;
                }
                else
                { 
                    List<DateTime> tempHoliday = yearDic[tmpDate.Year];
                    for (int j = 0; j < tempHoliday.Count; j++)
                    {
                        if (tempHoliday[j] == tmpDate)
                        {
                            tmpDays += 1;
                            break;
                        }
                    }
                }
            }
            workDays = workDays.AddDays(tmpDays);
        }

        Dictionary<string, (int[], string[])> holidayDic = new Dictionary<string, (int[], string[])>();

        public void GetPlusDay(DateTime sdate, int days, out DateTime workDays)
        {

            int tmpDays = 0;
            workDays = sdate;
            DateTime tmpDate = sdate.AddDays(tmpDays);

            for (int i = 1; i <= days; i++)
            {
                tmpDate = tmpDate.AddDays(1);
            retry:
                //주말일 경우 수정
                if (tmpDate.DayOfWeek == DayOfWeek.Saturday)
                    tmpDate = tmpDate.AddDays(1);
                else if (tmpDate.DayOfWeek == DayOfWeek.Sunday)
                    tmpDate = tmpDate.AddDays(2);

                //공효일 데이터 불러오기
                int[] no;
                string[] name;
                if (holidayDic.ContainsKey(tmpDate.ToString("yyyy-MM")))
                {
                    var dicValues = holidayDic[tmpDate.ToString("yyyy-MM")];
                    no = dicValues.Item1;
                    name = dicValues.Item2;
                }
                else
                {
                    getRedDay(tmpDate.Year, tmpDate.Month, out no, out name);
                    holidayDic.Add(tmpDate.ToString("yyyy-MM"), (no, name));
                }
                //휴일체크
                foreach (int n in no)
                {
                    if (n == tmpDate.Day)
                    {
                        tmpDate = tmpDate.AddDays(1);
                        goto retry;
                    }
                }
            }

            workDays = tmpDate;
        }

        public void GetStock(DateTime sdate, DateTime edate, double sales_count, double stock, out double real_stock)
        {
            if (sdate == edate)
            {
                real_stock = stock;
                return;
            }

            DateTime tmpDate = new DateTime(sdate.Year, sdate.Month, sdate.Day);
            while (tmpDate <= edate && stock > 0)
            {
                if(!HolidayCheck(tmpDate))
                    stock -= sales_count;

                tmpDate = tmpDate.AddDays(1);
            }
            if (stock < 0)
                stock = 0;
            real_stock = stock;
        }

        public void GetMinusDay(DateTime sdate, int days, out DateTime workDays)
        {
            int tmpDays = 0;
            workDays = sdate;
            DateTime tmpDate = sdate.AddDays(tmpDays);

            for (int i = 1; i <= days; i++)
            {
                tmpDate = tmpDate.AddDays(-1);
            retry:
                //주말일 경우 수정
                if (tmpDate.DayOfWeek == DayOfWeek.Saturday)
                    tmpDate = tmpDate.AddDays(-2);
                else if (tmpDate.DayOfWeek == DayOfWeek.Sunday)
                    tmpDate = tmpDate.AddDays(-1);


                //공효일 데이터 불러오기
                int[] no;
                string[] name;
                if (holidayDic.ContainsKey(tmpDate.ToString("yyyy-MM")))
                {
                    var dicValues = holidayDic[tmpDate.ToString("yyyy-MM")];
                    no = dicValues.Item1;
                    name = dicValues.Item2;
                }
                else
                {
                    getRedDay(tmpDate.Year, tmpDate.Month, out no, out name);
                    holidayDic.Add(tmpDate.ToString("yyyy-MM"), (no, name));
                }

                foreach (int n in no)
                {
                    if (n == tmpDate.Day)
                    {
                        tmpDate = tmpDate.AddDays(-1);
                        goto retry;
                    }
                }
            }

            workDays = tmpDate;
        }

        public void GetExhausedDateDayd(DateTime sDate, double stock, double sales, int standard, List<DataRow> pendingDt, out DateTime exhausted_date, out double totalExhaustedCnt, bool isSimple = false)
        {
            DateTime StartDate = sDate;                          //기준일자
            totalExhaustedCnt = 0;                               //쇼트수량
            int totalExhaustedDays = 0;                          //쇼트일자
            int idx = -1;
            //판매량이 0일 경우 무한대로 
            
            if (stock > standard && sales <= 0)
            {
                exhausted_date = new DateTime(3000, 1, 1);
                return;
            }

            //재고도 입고도 판매도 0이면 회전율 0
            if (stock == 0 && (pendingDt == null || pendingDt.Count == 0))
            {
                exhausted_date = sDate.AddDays(-1);
                if (exhausted_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                    exhausted_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                return;
            }

            //금일 이전에 선적내역이 있으면 재고반영
            if (pendingDt != null)
            {
                for (int i = pendingDt.Count - 1; i >= 0; i--)
                {
                    DateTime warehousing_date = Convert.ToDateTime(pendingDt[i]["warehousing_date"].ToString());
                    if (warehousing_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                    {
                        stock += Convert.ToDouble(pendingDt[i]["quantity_on_paper"].ToString());
                        pendingDt.Remove(pendingDt[i]);
                    }
                }
                if (stock < 0)
                    stock = 0;
            }


            //재고계산
            while (stock > standard || (pendingDt != null && pendingDt.Count > 0))
            {
                //++
                idx++;
                StartDate = sDate.AddDays(idx);

                //심플하게 볼때는 1년 이상은 생략
                if (isSimple && sDate.AddYears(1) <= StartDate)
                {
                    exhausted_date = new DateTime(3000, 1, 1);
                    return;
                }
                //선적내역이 있으면 재고반영
                if (pendingDt != null)
                { 
                    for (int i = pendingDt.Count - 1; i >= 0; i--)
                    {
                        DateTime warehousing_date = Convert.ToDateTime(pendingDt[i]["warehousing_date"].ToString());
                        if (warehousing_date < Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")))
                            warehousing_date = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

                        if (Convert.ToDouble(pendingDt[i]["quantity_on_paper"].ToString()) != 0 
                            && warehousing_date.ToString("yyyy-MM-dd") == StartDate.ToString("yyyy-MM-dd"))
                        {
                            stock += Convert.ToDouble(pendingDt[i]["quantity_on_paper"].ToString());
                            pendingDt.Remove(pendingDt[i]);
                        }
                    }
                }
                bool isHoliday = HolidayCheck(StartDate);  //휴일여부
                //공휴일이 아니면 반영
                if (!isHoliday)
                    stock -= sales;
                //재고가 0 이하일때(소트)
                if (stock <= standard && (pendingDt != null && pendingDt.Count > 0))
                {
                    if (!isHoliday)
                    {
                        totalExhaustedCnt -= sales;
                        totalExhaustedDays++;
                    }
                    stock = 0;
                }   
            }
            //휴일인 경우 가까운 영업일로 변경
            retry:                
            if (HolidayCheck(StartDate))
            {
                StartDate = StartDate.AddDays(1);
                goto retry;
            }

            //return
            exhausted_date = StartDate;
        }
        public void GetPreMonthStock(DateTime sDate, DateTime eDate, double stock, double sales, DataTable pendingDt, out double pre_stock)
        {
            DataTable copyDt = pendingDt.Copy();
            DateTime StartDate = sDate;                          //기준일자
            int idx = 0;
            //재고계산
            while (StartDate <= eDate)
            {
                //선적내역이 있으면 재고반영
                if (copyDt != null)
                {
                    for (int i = copyDt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (Convert.ToDouble(copyDt.Rows[i]["qty"].ToString()) > 0
                            && copyDt.Rows[i]["warehousing_date"].ToString() == StartDate.ToString("yyyy-MM-dd"))
                        {
                            stock += Convert.ToDouble(copyDt.Rows[i]["qty"].ToString());
                            copyDt.Rows.Remove(copyDt.Rows[i]);
                        }
                    }
                }

                bool isHoliday = HolidayCheck(StartDate);  //휴일여부
                //공휴일이 아니면 반영
                if (!isHoliday)
                    stock -= sales;
                //재고가 0 이하일때(소트)
                if (stock <= 0)
                {
                    stock = 0;
                }
                //++
                idx++;
                StartDate = sDate.AddDays(idx);
            }
            //return
            pre_stock = stock;
        }

        //음력 공휴일계산=========================================================

        public void GetUserWorkDay(DateTime sttdate, DateTime enddate, DataTable vacationDt, out int workDays)
        {
            workDays = 0;

            if (sttdate == enddate)
            {
                workDays = 0;
                return;
            }

            int DayInterval = 0;
            while (sttdate.AddDays(DayInterval) <= enddate)
            {
                workDays += 1;

                DateTime StartDate = sttdate.AddDays(DayInterval);
                DayInterval += 1;
                if (!yearDic.ContainsKey(StartDate.Year))
                {
                    //음력계산 법정공휴일
                    //신정
                    DateTime dt = new DateTime(StartDate.Year - 1, 12, 30);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 1, 1);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 1, 2);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    //석가탄신일
                    dt = new DateTime(StartDate.Year, 4, 8);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 8, 14);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 8, 15);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 8, 16);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    //양력 법정공휴일
                    //신정
                    dt = new DateTime(StartDate.Year, 1, 1);
                    holiday.Add(dt);
                    //삼일절
                    dt = new DateTime(StartDate.Year, 3, 1);
                    holiday.Add(dt);
                    //어린이날
                    dt = new DateTime(StartDate.Year, 5, 5);
                    holiday.Add(dt);
                    //현충일
                    dt = new DateTime(StartDate.Year, 6, 6);
                    holiday.Add(dt);
                    //광복절
                    dt = new DateTime(StartDate.Year, 8, 15);
                    holiday.Add(dt);
                    //개천절
                    dt = new DateTime(StartDate.Year, 10, 3);
                    holiday.Add(dt);
                    //한글날
                    dt = new DateTime(StartDate.Year, 10, 9);
                    holiday.Add(dt);
                    //크리스마스
                    dt = new DateTime(StartDate.Year, 12, 25);
                    holiday.Add(dt);

                    yearDic.Add(StartDate.Year, holiday);
                }
                //주말
                if (StartDate.DayOfWeek == DayOfWeek.Saturday || StartDate.DayOfWeek == DayOfWeek.Sunday)
                    workDays -= 1;
                //법정, 특수공휴일
                else if (HolidayCheck(StartDate))
                    workDays -= 1;
                else
                {
                    List<DateTime> tempHoliday = yearDic[StartDate.Year];
                    for (int j = 0; j < tempHoliday.Count; j++)
                    {
                        if (tempHoliday[j] == StartDate)
                        {
                            workDays -= 1;
                            break;
                        }
                    }
                }
                //개인 연차, 반차사용
                if (vacationDt != null && vacationDt.Rows.Count > 0)
                {
                    foreach (DataRow d in vacationDt.Rows)
                    {
                        DateTime vacationSttdt = Convert.ToDateTime(d["sttdate"].ToString());
                        double vacationDayCnt = Convert.ToDouble(d["used_days"].ToString());
                        DateTime vacationEnddt = vacationSttdt.AddDays(vacationDayCnt);

                        if (StartDate >= vacationSttdt && StartDate <= vacationEnddt)
                        {
                            workDays -= 1;
                            break;
                        }
                    }
                }
            }
        }

        public void GetWorkDay(DateTime sttdate, DateTime enddate, out int workDays)
        {
            workDays = 0;

            if (sttdate == enddate)
            {
                workDays = 0;
                return;
            }

            if (sttdate.Year <= 1900)
            {
                workDays = 0;
                return;
            }
            else if (enddate.Year >= 2100)
            {
                workDays = 0;
                return;
            }


            int DayInterval = 0;
            while (sttdate.AddDays(DayInterval) <= enddate)
            {
                workDays += 1;

                DateTime StartDate = sttdate.AddDays(DayInterval);
                DayInterval += 1;
                if (!yearDic.ContainsKey(StartDate.Year))
                {
                    //음력계산 법정공휴일
                    //신정
                    DateTime dt = new DateTime(StartDate.Year - 1, 12, 30);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 1, 1);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 1, 2);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    //석가탄신일
                    dt = new DateTime(StartDate.Year, 4, 8);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 8, 14);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 8, 15);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(StartDate.Year, 8, 16);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    //양력 법정공휴일
                    //신정
                    dt = new DateTime(StartDate.Year, 1, 1);
                    holiday.Add(dt);
                    //삼일절
                    dt = new DateTime(StartDate.Year, 3, 1);
                    holiday.Add(dt);
                    //어린이날
                    dt = new DateTime(StartDate.Year, 5, 5);
                    holiday.Add(dt);
                    //현충일
                    dt = new DateTime(StartDate.Year, 6, 6);
                    holiday.Add(dt);
                    //광복절
                    dt = new DateTime(StartDate.Year, 8, 15);
                    holiday.Add(dt);
                    //개천절
                    dt = new DateTime(StartDate.Year, 10, 3);
                    holiday.Add(dt);
                    //한글날
                    dt = new DateTime(StartDate.Year, 10, 9);
                    holiday.Add(dt);
                    //크리스마스
                    dt = new DateTime(StartDate.Year, 12, 25);
                    holiday.Add(dt);
                    
                    yearDic.Add(StartDate.Year, holiday);
                }
                //주말
                if (StartDate.DayOfWeek == DayOfWeek.Saturday || StartDate.DayOfWeek == DayOfWeek.Sunday)
                    workDays -= 1;
                //법정, 특수공휴일
                else if(HolidayCheck(StartDate))
                    workDays -= 1;
                else
                {
                    List<DateTime> tempHoliday = yearDic[StartDate.Year];
                    for (int j = 0; j < tempHoliday.Count; j++)
                    {
                        if (tempHoliday[j] == StartDate)
                        {
                            workDays -= 1;
                            break;
                        }
                    }
                }
            }
        }

        public void GetEndDate(DateTime sttdate, int workDays, out DateTime enddate)
        {

            if (sttdate.Year <= 1900)
            {
                enddate = new DateTime(1900, 1, 1);
                return;
            }
            else if (workDays == 0)
            {
                enddate = sttdate;
                return;
            }

            enddate = sttdate;
            while (workDays > 0)
            {
                enddate = enddate.AddDays(-1);
                if (!yearDic.ContainsKey(enddate.Year))
                {
                    //음력계산 법정공휴일
                    //신정
                    DateTime dt = new DateTime(enddate.Year - 1, 12, 30);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(enddate.Year, 1, 1);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(enddate.Year, 1, 2);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    //석가탄신일
                    dt = new DateTime(enddate.Year, 4, 8);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(enddate.Year, 8, 14);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(enddate.Year, 8, 15);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    dt = new DateTime(enddate.Year, 8, 16);
                    dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                    holiday.Add(dt);
                    //양력 법정공휴일
                    //신정
                    dt = new DateTime(enddate.Year, 1, 1);
                    holiday.Add(dt);
                    //삼일절
                    dt = new DateTime(enddate.Year, 3, 1);
                    holiday.Add(dt);
                    //어린이날
                    dt = new DateTime(enddate.Year, 5, 5);
                    holiday.Add(dt);
                    //현충일
                    dt = new DateTime(enddate.Year, 6, 6);
                    holiday.Add(dt);
                    //광복절
                    dt = new DateTime(enddate.Year, 8, 15);
                    holiday.Add(dt);
                    //개천절
                    dt = new DateTime(enddate.Year, 10, 3);
                    holiday.Add(dt);
                    //한글날
                    dt = new DateTime(enddate.Year, 10, 9);
                    holiday.Add(dt);
                    //크리스마스
                    dt = new DateTime(enddate.Year, 12, 25);
                    holiday.Add(dt);

                    yearDic.Add(enddate.Year, holiday);
                }
                //주말
                if (enddate.DayOfWeek == DayOfWeek.Saturday || enddate.DayOfWeek == DayOfWeek.Sunday)
                {
                    workDays += 1;
                }
                //법정, 특수공휴일
                else if (HolidayCheck(enddate))
                {
                    workDays += 1;
                }
                else
                {
                    List<DateTime> tempHoliday = yearDic[enddate.Year];
                    for (int j = 0; j < tempHoliday.Count; j++)
                    {
                        if (tempHoliday[j] == enddate)
                        {
                            workDays += 1;
                            break;
                        }
                    }
                }
                workDays -= 1;
            }

            return;
        }

        public bool checkFormActive(string formname)
        {
            FormCollection fc = Application.OpenForms;
            bool isFormActive = false;
            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "formname")
                {
                    frm.Activate();
                    isFormActive = true;
                }
            }

            return isFormActive;
        }
        public string strDatetime(string txt)
        {
            DateTime nDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (txt.All(Char.IsDigit))
            {
                if (txt.Length == 8)
                {
                    txt = txt.Substring(0, 4) + "-" + txt.Substring(4, 2) + "-" + txt.Substring(6, 2);
                }
                else if (txt.Length == 4)
                {
                    txt = DateTime.Now.Year + "-" + txt.Substring(0, 2) + "-" + txt.Substring(2, 2);

                    if(nDate > Convert.ToDateTime(txt))
                        txt = Convert.ToDateTime(DateTime.Now.AddYears(1).Year + "-" + txt.Substring(0, 2) + "-" + txt.Substring(2, 2)).ToString("yyyy-MM-dd");
                }
            }
            else
            {
                if (txt.Length <= 4 && txt.Contains('/'))
                {

                    string[] tmp = txt.Split('/');
                    if (txt.Length > 0)
                    {
                        if (tmp[0].Length < 2)
                            tmp[0] = tmp[0].PadLeft(2, '0');
                        if (tmp[1].Length < 2)
                            tmp[1] = tmp[1].PadLeft(2, '0');

                        txt = DateTime.Now.Year + "-" + tmp[0] + "-" + tmp[1];
                        if (DateTime.TryParse(txt, out DateTime tempDt) && DateTime.Now.Month == 12 && tempDt.Month >= 1 && tempDt.Month <= 5)
                            txt = (DateTime.Now.Year + 1) + "-" + tmp[0] + "-" + tmp[1];
                        /*if (nDate > Convert.ToDateTime(txt))
                            txt = Convert.ToDateTime(DateTime.Now.AddYears(1).Year + "-" + tmp[0] + "-" + tmp[1]).ToString("yyyy-MM-dd");*/
                    }
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParse(txt, out dt))
                        txt = dt.ToString("yyyy-MM-dd");
                }                
            }
            return txt;
        }

        public Control FindFocusedControl(Control container)
        {
            foreach (Control childControl in container.Controls)
            {
                if (childControl.Focused)
                { return childControl; }
            }
            foreach (Control childControl in container.Controls)
            {
                Control maybeFocusedControl = FindFocusedControl(childControl);
                if (maybeFocusedControl != null)
                {
                    return maybeFocusedControl;
                }
            }
            return null; // Couldn't find any, darn!
        }

        public bool HolidayCheck(DateTime StartDate)
        {
            bool isHoliday = false;

            
            //주말
            if (StartDate.DayOfWeek == DayOfWeek.Saturday || StartDate.DayOfWeek == DayOfWeek.Sunday)
            {
                isHoliday = true;
            }
            else
            {
                //공효일 데이터 불러오기
                int[] no;
                string[] name;
                if (holidayDic.ContainsKey(StartDate.ToString("yyyy-MM")))
                {
                    var dicValues = holidayDic[StartDate.ToString("yyyy-MM")];
                    no = dicValues.Item1;
                    name = dicValues.Item2;
                }
                else
                {
                    getRedDay(StartDate.Year, StartDate.Month, out no, out name);
                    holidayDic.Add(StartDate.ToString("yyyy-MM"), (no, name));
                }

                foreach (int day in no)
                {
                    if (day == StartDate.Day)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                }




                /*List<DateTime> holidayList = GetYearHoliday(StartDate);
                for (int i = 0; i < holidayList.Count; i++)
                {
                    if (holidayList[i].ToString("yyyy-MM-dd") == StartDate.ToString("yyyy-MM-dd"))
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                }

                //법정 공휴일
                //신정
                if (StartDate.Month == 1 && StartDate.Day == 1)
                {
                    isHoliday = true;
                }
                //삼일절
                else if (StartDate.Month == 3 && StartDate.Day == 1)
                {
                    isHoliday = true;
                }
                //어린이날
                else if (StartDate.Month == 5 && StartDate.Day == 5)
                {
                    isHoliday = true;
                }
                //현충일
                else if (StartDate.Month == 6 && StartDate.Day == 6)
                {
                    isHoliday = true;
                }
                //광복절
                else if (StartDate.Month == 8 && StartDate.Day == 15)
                {
                    isHoliday = true;
                }
                //개천절
                else if (StartDate.Month == 10 && StartDate.Day == 3)
                {
                    isHoliday = true;
                }
                //한글날
                else if (StartDate.Month == 10 && StartDate.Day == 9)
                {
                    isHoliday = true;
                }
                //크리스마스
                else if (StartDate.Month == 12 && StartDate.Day == 25)
                {
                    isHoliday = true;
                }


                //특수 공휴일
                if (StartDate.Year == 2022)
                {
                    //대통령선거
                    if (StartDate.Month == 3 && StartDate.Day == 9)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                    //지방선거
                    else if (StartDate.Month == 6 && StartDate.Day == 1)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                    //추석 대체공휴일
                    else if (StartDate.Month == 9 && StartDate.Day == 12)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                    //한글날 대체공휴일
                    else if (StartDate.Month == 10 && StartDate.Day == 10)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                }
                else if (StartDate.Year == 2023)
                {
                    //설날 대체공휴일
                    if (StartDate.Month == 1 && StartDate.Day == 24)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                }
                else if (StartDate.Year == 2024)
                {
                    //설날 대체공휴일
                    if (StartDate.Month == 2 && StartDate.Day == 12)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                    //국회의원 선거
                    else if (StartDate.Month == 4 && StartDate.Day == 10)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                    //근로자의 날
                    else if (StartDate.Month == 5 && StartDate.Day == 1)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                    //어린이날 대체공휴일
                    else if (StartDate.Month == 5 && StartDate.Day == 6)
                    {
                        isHoliday = true;
                        return isHoliday;
                    }
                }*/
            }

            return isHoliday;
        }


        public List<DateTime> GetYearHoliday(DateTime StartDate)
        {
            List<DateTime> holiday = new List<DateTime>();
            if (!yearDic.ContainsKey(StartDate.Year))
            {
                //음력계산 법정공휴일
                //신정
                DateTime dt = new DateTime(StartDate.Year - 1, 12, 30);
                dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                holiday.Add(dt);

                dt = new DateTime(StartDate.Year, 1, 1);
                dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                holiday.Add(dt);

                dt = new DateTime(StartDate.Year, 1, 2);
                dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                holiday.Add(dt);

                //석가탄신일
                dt = new DateTime(StartDate.Year, 4, 8);
                dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                holiday.Add(dt);

                dt = new DateTime(StartDate.Year, 8, 14);
                dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                holiday.Add(dt);

                dt = new DateTime(StartDate.Year, 8, 15);
                dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                holiday.Add(dt);

                dt = new DateTime(StartDate.Year, 8, 16);
                dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
                holiday.Add(dt);

                //양력 법정공휴일
                //신정
                dt = new DateTime(StartDate.Year, 1, 1);
                holiday.Add(dt);

                //삼일절
                dt = new DateTime(StartDate.Year, 3, 1);
                holiday.Add(dt);

                //어린이날
                dt = new DateTime(StartDate.Year, 5, 5);
                holiday.Add(dt);

                //현충일
                dt = new DateTime(StartDate.Year, 6, 6);
                holiday.Add(dt);

                //광복절
                dt = new DateTime(StartDate.Year, 8, 15);
                holiday.Add(dt);

                //개천절
                dt = new DateTime(StartDate.Year, 10, 3);
                holiday.Add(dt);

                //한글날
                dt = new DateTime(StartDate.Year, 10, 9);
                holiday.Add(dt);

                //크리스마스
                dt = new DateTime(StartDate.Year, 12, 25);
                holiday.Add(dt);

                yearDic.Add(StartDate.Year, holiday);

                
            }
            else
            {
                foreach (int list in yearDic.Keys)
                {
                    if (list == StartDate.Year)
                    {
                        holiday = yearDic[list];
                    }
                }
            }
            return holiday;
        }


        public void getRedDay(int year, int month, out int[] no, out string[] name1)
        {
            List<int> no2 = new List<int>();
            List<string> name2 = new List<string>();

            //특수 공휴일
            if (year == 2022)
            {
                switch (month)
                {
                    case 3:
                        no2.Add(9);
                        name2.Add("대통령 선거");
                        break;
                    case 6:
                        no2.Add(1);
                        name2.Add("지방 선거");
                        break;
                    case 9:
                        no2.Add(12);
                        name2.Add("추석 대체");
                        break;
                    case 10:
                        no2.Add(10);
                        name2.Add("한글날 대체");
                        break;

                    default:
                        no = null;
                        name1 = null;

                        break;
                }
            }
            else if (year == 2023)
            {
                switch (month)
                {
                    case 1:
                        no2.Add(24);
                        name2.Add("설 대체공휴일");
                        break;
                    case 10:
                        no2.Add(2);
                        name2.Add("추석 대체공휴일");
                        break;
                    default:
                        no = null;
                        name1 = null;

                        break;
                }
            }
            else if (year == 2024)
            {
                switch (month)
                {
                    case 2:
                        no2.Add(12);
                        name2.Add("설 대체공휴일");
                        break;
                    case 4:
                        no2.Add(10);
                        name2.Add("국회의원 선거");
                        break;
                    case 5:
                        no2.Add(1);
                        name2.Add("근로자의 날");

                        no2.Add(6);
                        name2.Add("어린이날 대체공휴일");
                        break;

                    default:
                        no = null;
                        name1 = null;

                        break;
                }
            }

            //법정 공휴일
            switch (month)
            {
                case 1:
                    no2.Add(1);
                    name2.Add("신정");
                    break;
                case 3:
                    no2.Add(1);
                    name2.Add("삼일절");
                    no2.Add(1);
                    name2.Add("삼일절");
                    break;
                case 5:
                    no2.Add(5);
                    name2.Add("어린이날");
                    break;
                case 6:
                    no2.Add(6);
                    name2.Add("현충일");
                    break;
                case 8:
                    no2.Add(15);
                    name2.Add("광복절");
                    break;
                case 9:
                    break;
                case 10:
                    no2.Add(3);
                    name2.Add("개천절");
                    no2.Add(9);
                    name2.Add("한글날");
                    break;
                case 12:
                    no2.Add(25);
                    name2.Add("크리스마스");
                    break;
                default:
                    no = null;
                    name1 = null;
                    break;
            }

            //고정 공휴일
            DateTime dt = new DateTime(year - 1, 12, 30);
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 1, 1);
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 1, 2);//설날
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//설날
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("설날");
            }

            dt = new DateTime(year, 4, 8);//석가탄신일
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);//석가탄신일
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("석가탄신일");
            }

            dt = new DateTime(year, 8, 14);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            dt = new DateTime(year, 8, 15);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            dt = new DateTime(year, 8, 16);//추석
            dt = convertKoreanMonth(dt.Year, dt.Month, dt.Day);
            if (dt.Month == month)
            {
                no2.Add(dt.Day);
                name2.Add("추석");
            }

            no = no2.ToArray();
            name1 = name2.ToArray();
        }

        public DateTime SetCurrentWorkDate(DateTime sdate, bool isDayPlus = false)
        {
            int[] no1, no2;
            string[] name1, name2;
            DateTime tempDt;
            DateTime tempDt1 = new DateTime(sdate.Year, sdate.Month, 1);
            DateTime tempDt2 = new DateTime(sdate.AddMonths(-1).Year, sdate.AddMonths(-1).Month, 1);
            /*getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
            getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);*/

            //tempDt1 휴일데이터
            if (holidayDic.ContainsKey(tempDt1.ToString("yyyy-MM")))
            {
                var dicValues = holidayDic[tempDt1.ToString("yyyy-MM")];
                no1 = dicValues.Item1;
                name1 = dicValues.Item2;
            }
            else
            {
                getRedDay(tempDt1.Year, tempDt1.Month, out no1, out name1);
                holidayDic.Add(tempDt1.ToString("yyyy-MM"), (no1, name1));
            }
            //tempDt2 휴일데이터
            if (holidayDic.ContainsKey(tempDt2.ToString("yyyy-MM")))
            {
                var dicValues = holidayDic[tempDt2.ToString("yyyy-MM")];
                no2 = dicValues.Item1;
                name2 = dicValues.Item2;
            }
            else
            {
                getRedDay(tempDt2.Year, tempDt2.Month, out no2, out name2);
                holidayDic.Add(tempDt2.ToString("yyyy-MM"), (no2, name2));
            }



        retry:
            //주말일 경우 수정
            if (sdate.DayOfWeek == DayOfWeek.Saturday)
                sdate = sdate.AddDays(-2);
            else if (sdate.DayOfWeek == DayOfWeek.Sunday)
                sdate = sdate.AddDays(-1);


            //법정공휴일일 경우
            if (sdate.Month == tempDt1.Month)
            {
                foreach (int n in no1)
                {
                    if (n == sdate.Day)
                    {
                        if(!isDayPlus)
                            sdate = sdate.AddDays(-1);
                        else
                            sdate = sdate.AddDays(1);
                        goto retry;
                    }
                }
            }
            else if (sdate.Month == tempDt2.Month)
            {
                foreach (int n in no2)
                {
                    if (n == sdate.Day)
                    {
                        if (!isDayPlus)
                            sdate = sdate.AddDays(-1);
                        else
                            sdate = sdate.AddDays(1);
                        goto retry;
                    }
                }
            }

            return sdate;
        }

        private DateTime convertKoreanMonth(int n음력년, int n음력월, int n음력일)//음력을 양력 변환
        {
            System.Globalization.KoreanLunisolarCalendar 음력 =
            new System.Globalization.KoreanLunisolarCalendar();

            if (n음력년 > 2050)
            {
                return new DateTime();
            }

            bool b달 = 음력.IsLeapMonth(n음력년, n음력월);
            int n윤월;

            if (음력.GetMonthsInYear(n음력년) > 12)
            {
                n윤월 = 음력.GetLeapMonth(n음력년);
                if (b달)
                    n음력월++;
                if (n음력월 > n윤월)
                    n음력월++;
            }
            try
            {
                음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
            }
            catch
            {
                if (n음력월 > 12)
                {
                    n음력년 += 1;
                    n음력월 = n음력월 - 12;
                }

                if (n음력년 > 2050)
                {
                    return new DateTime();
                }

                return 음력.ToDateTime(n음력년, n음력월, 음력.GetDaysInMonth(n음력년, n음력월), 0, 0, 0, 0);//음력은 마지막 날짜가 매달 다르기 때문에 예외 뜨면 그날 맨 마지막 날로 지정
            }

            return 음력.ToDateTime(n음력년, n음력월, n음력일, 0, 0, 0, 0);
        }
        //음력 공휴일계산=========================================================
        #endregion

        #region 
        public static bool isDatetime(string date)
        {
            DateTime temp;
            if (DateTime.TryParse(date, out temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        //숫자콤마
        public string GetN2(string A)
        {
            if (A.Substring(0, 1) == ".")
            {
                A = "0" + A;
            }
            if (A.Substring(A.Length - 1, 1) == ".")
            {
                A = A + "0";
            }


            double B = Convert.ToDouble(A);
            string result;
            if (B == (int)B)
                result = B.ToString("#,##0");
            else
            {
                string[] tmp = B.ToString().Split('.');
                if (tmp.Length > 0)
                {
                    result = B.ToString("N" + tmp[1].Length.ToString());
                }
                else
                {
                    result = B.ToString("N2");
                }
            }
                

            return result;
        }
        #region 환율가져오기
        public double GetExchangeRateKEBBank(string cur_unit)
        {
            double exchange_rate = 1400;
            string url = "https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey=0JyQWM2ulpDV7pUxivERvlJvntDlyJuB&searchdate=20240711&data=AP01";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    JArray jsonArray = JArray.Parse(responseBody);

                    foreach (var item in jsonArray)
                    {
                        if (item["cur_unit"].ToString() == "USD")
                        {
                            return Convert.ToDouble(item["deal_bas_r"]);
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }

            return exchange_rate;



            /*return 1400;*/
            /*try
            {
                *//*string url = "http://fx.kebhana.com/FER1101M.web";*//*
                //2023-10-16 서비스 갑자기 안됌
                string url = "https://quotation-api-cdn.dunamu.com/v1/forex/recent?codes=FRX.KRW" + cur_unit;

                //request를 openapi 서버로 전송해보자!
                WebRequest wr = WebRequest.Create(url);
                wr.Method = "GET";

                WebResponse wrs = wr.GetResponse();
                Stream s = wrs.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);

                string response = sr.ReadToEnd();

                string[] square = response.Split('[');
                if (square.Length > 0)
                {
                    string[] curly = square[1].Split('{');
                    if (curly.Length > 0)
                    {
                        for (int i = 0; i < curly.Length; i++)
                        {
                            if (curly[i].Contains("basePrice"))
                            {
                                string[] unit = curly[i].Split(',');

                                for (int j = 0; j < unit.Length; j++)
                                {
                                    if (unit[j].Contains("basePrice"))
                                    {
                                        string exRate = unit[j].Replace("basePrice", "").Replace("}", "").Replace(":", "").Replace("\"", "").Replace("\t", "").Replace("\n", "").Replace("\r", "");
                                        double d;
                                        if (double.TryParse(exRate, out d))
                                        {
                                            return d;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //안되면 두번째 환율 가져오기
            catch
            {
                return GetExchangeRateKEBBank2();
            }
            return 0;*/
        }

        public double GetExchangeRateKEBBank2()
        {
            try
            {
                string url = "https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey=fi5xbDDUDrbWRfKgYZ3v9WK6HeTNTfqa&searchdate=20231016&data=AP01";

                //request를 openapi 서버로 전송해보자!
                WebRequest wr = WebRequest.Create(url);
                wr.Method = "GET";

                WebResponse wrs = wr.GetResponse();
                Stream s = wrs.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.Default);

                string response = sr.ReadToEnd();

                string[] square = response.Replace("[", "").Replace("]", "").Split('{');
                if (square.Length > 0)
                {
                    foreach (string curly in square)
                    {
                        if (curly.Contains("USD"))
                        {
                            string[] unit = curly.Split(new string[] { "deal_bas_r\":" }, StringSplitOptions.None);
                            if (unit.Length > 1)
                            { 
                                string[] unit2 = unit[1].Replace("\"", "").Split(new string[] { ",bkpr" }, StringSplitOptions.None);
                                if (unit2.Length > 0)
                                {
                                    double d;
                                    if (double.TryParse(unit2[0], out d))
                                    {
                                        return d;
                                    }
                                }
                            }

                            for (int j = 0; j < unit.Length; j++)
                            {
                                if (unit[j].Contains("deal_bas_r"))
                                {
                                    string exRate = unit[j].Replace("basePrice", "").Replace("}", "").Replace(":", "").Replace("\"", "").Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace(":", "").Replace(@"\", "");
                                    double d;
                                    if (double.TryParse(exRate, out d))
                                    {
                                        return d;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //안되면 두번째 환율 가져오기
            catch
            { }
            return 1400;
        }


        public void convertCheck(string str)
        {
            Encoding encKr = Encoding.GetEncoding("euc-kr");
            EncodingInfo[] encods = Encoding.GetEncodings();
            Encoding destEnc = Encoding.UTF8;

            foreach (EncodingInfo ec in encods)
            {
                Encoding enc = ec.GetEncoding();
                byte[] sorceBytes = enc.GetBytes(str);
                byte[] encBytes = Encoding.Convert(encKr, destEnc, sorceBytes);

                System.Diagnostics.Debug.WriteLine(string.Format("{0}({1}) : {2}", enc.EncodingName, enc.BodyName, destEnc.GetString(encBytes)));
            }
        }

        public double GetExchangeRate(string cur_unit)
        {
            string yourkey = "fi5xbDDUDrbWRfKgYZ3v9WK6HeTNTfqa";
            string query = "https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey=" + yourkey + "&data=AP01&cur_unit=" + cur_unit;

            //request를 openapi 서버로 전송해보자!
            WebRequest wr = WebRequest.Create(query);
            wr.Method = "GET";

            WebResponse wrs = wr.GetResponse();
            Stream s = wrs.GetResponseStream();
            StreamReader sr = new StreamReader(s);

            string response = sr.ReadToEnd();

            //richTextBox1.Text = response;
            double eRate = 0;
            string d;
            List<string> exchange_data = JsonConvert.DeserializeObject<List<string>>(response);
            if (exchange_data.Count > 0)
            {
                d = exchange_data[0];
            }

            return eRate;
        }

        private class exchange
        {
            public string cur_unit { get; set; }
            public string ttb { get; set; }
            public string tts { get; set; }
            public string cur_nm { get; set; }
        }
        private class exchangeKeb
        {
            public string cur_unit { get; set; }
            public string ttb { get; set; }
            public string tts { get; set; }
            public string cur_nm { get; set; }
        }
        #endregion

        #region 이미지, PDF 파일 TIFF로 변환
        public bool CheckDirectory(string path, bool isMake = false)
        {
            bool isComplete = false;
            DirectoryInfo di = new DirectoryInfo(path);
            if (!di.Exists)
            {
                if (isMake)
                {
                    di.Create();
                    isComplete = true;
                }
            }
            else
                isComplete = true;

            return isComplete;
        }
        public bool DeleteFilesInDirectory(string path)
        {
            bool isComplete = false;
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists)
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                isComplete = true;
            }

            return isComplete;
        }

        public static DataTable DataGridView_To_Datatable(DataGridView dg)
        {
            dg.EndEdit();
            DataTable ExportDataTable = new DataTable();
            foreach (DataGridViewColumn col in dg.Columns)
            {
                ExportDataTable.Columns.Add(col.Name);
            }
            foreach (DataGridViewRow row in dg.Rows)
            {
                DataRow dRow = ExportDataTable.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                ExportDataTable.Rows.Add(dRow);
            }
            return ExportDataTable;
        }

        public void ConvertToTiff(string imagePath, string outputTiffPath, out string save_path)
        { 
            string extend_name = Path.GetExtension(imagePath);
            if (extend_name == ".pdf" || extend_name == ".PDF")
            {
                outputTiffPath += ".tif";
                ConvertPdfToTiff2(imagePath, outputTiffPath);
                
            }
            else if (extend_name == ".jpg" || extend_name == ".JPG"
                || extend_name == ".jpeg" || extend_name == ".JPEG"
                || extend_name == ".png" || extend_name == ".PNG"
                || extend_name == ".bmp" || extend_name == ".BMP")
            {
                outputTiffPath += ".tif";
                ConvertImageToTiff(imagePath, outputTiffPath);
            }
            else
            {
                outputTiffPath += Path.GetExtension(imagePath);
                File.Copy(imagePath, outputTiffPath);
            }
            save_path = outputTiffPath;
        }

        public void ConvertImageToTiff(string imagePath, string outputTiffPath)
        {
            using (var image = Image.FromFile(imagePath))
            {
                var tiffEncoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Tiff.Guid);
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionNone);
                image.Save(outputTiffPath, tiffEncoder, encoderParams);
            }
        }

        public void GrantFileAccess(string filePath)
        {
            // 파일의 보안 정보 가져오기
            var fileSecurity = File.GetAccessControl(filePath);

            // 보안 권한 수정을 위한 보안 규칙 생성
            var accessRule = new FileSystemAccessRule(
                new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), // 모든 사용자에게 권한 부여
                FileSystemRights.FullControl, // 읽기 및 쓰기 권한 부여
                AccessControlType.Allow);

            // 보안 규칙 추가
            fileSecurity.AddAccessRule(accessRule);

            // 파일의 보안 정보 업데이트
            File.SetAccessControl(filePath, fileSecurity);
        }

        public static void ConvertPdfToTiff(string pdfFilePath, string tiffFilePath)
        {
            // 변환을 위해 소스 파일 PDF 로드
            var converter = new GroupDocs.Conversion.Converter(pdfFilePath);
            // 대상 형식 TIFF에 대한 변환 옵션 준비
            var convertOptions = converter.GetPossibleConversions()["tiff"].ConvertOptions;
            // TIFF 형식으로 변환
            converter.Convert(tiffFilePath, convertOptions);
        }

        public static void ConvertPdfToTiff2(string pdfFilePath, string tiffFilePath)
        {
            PdfDocument document = new PdfDocument();
            document.LoadFromFile(pdfFilePath);
            JoinTiffImages(SaveAsImage(document), tiffFilePath, EncoderValue.CompressionLZW);
        }

        private static Image[] SaveAsImage(PdfDocument document)
        {
            Image[] images = new Image[document.Pages.Count];
            for (int i = 0; i < document.Pages.Count; i++)
            {
                images[i] = document.SaveAsImage(i);
            }
            return images;
        }

        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            throw new Exception(mimeType + " mime type not found in ImageCodecInfo");
        }

        public static void JoinTiffImages(Image[] images, string outFile, EncoderValue compressEncoder)
        {
            //use the save encoder
            Encoder enc = Encoder.SaveFlag;
            EncoderParameters ep = new EncoderParameters(2);
            ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.MultiFrame);
            ep.Param[1] = new EncoderParameter(Encoder.Compression, (long)compressEncoder);
            Image pages = images[0];
            int frame = 0;
            ImageCodecInfo info = GetEncoderInfo("image/tiff");
            foreach (Image img in images)
            {
                if (frame == 0)
                {
                    pages = img;
                    //save the first frame
                    pages.Save(outFile, info, ep);
                }

                else
                {
                    //save the intermediate frames
                    ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.FrameDimensionPage);

                    pages.SaveAdd(img, ep);
                }
                if (frame == images.Length - 1)
                {
                    //flush and close.
                    ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.Flush);
                    pages.SaveAdd(ep);
                }
                frame++;
            }
        }
        #endregion

        #region 특정위치 윈도우/컨트롤 구하기
        #region 포인트에서 윈도우 구하기 - WindowFromPoint(point)

        /// <summary>
        /// 포인트에서 윈도우 구하기
        /// </summary>
        /// <param name="point">포인트</param>
        /// <returns>윈도우 핸들</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "0")]
        public static extern IntPtr WindowFromPoint(Point point);

        #endregion

        #region 컨트롤 구하기 - GetControl(point)

        /// <summary>
        /// 컨트롤 구하기
        /// </summary>
        /// <param name="point">포인트</param>
        /// <returns>컨트롤</returns>
        /// <remarks>포인트는 화면 좌표계 포인트이다.</remarks>
        public Control GetControl(Point point)
        {
            return Control.FromChildHandle(WindowFromPoint(point));
        }

        #endregion
        #endregion

    }
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
