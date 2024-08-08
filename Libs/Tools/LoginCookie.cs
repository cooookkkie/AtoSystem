using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Libs.Tools
{
    public class LoginCookie
    {
        private string _path;

        public LoginCookie(string path, string prefix, string postifx)
        {
            _path = path;
            _SetCookiePath(prefix, postifx);
        }
        public LoginCookie(string prefix, string postfix)
        /*: this(Path.Combine(MyApplication.Root, "Cookies"),  prefix, postfix)*/
        : this(Path.Combine(@"C:\", "Cookies"),  prefix, postfix)
        {

        }

        //인자가 있는 생성자에 현재경로에 Log폴더를 포함한 경로로 다시 생성
        public LoginCookie()
            /*: this(Path.Combine(MyApplication.Root, "Cookies"),null, null)*/
            : this(Path.Combine(@"C:\", "Cookies"), null, null)
        {
        }
        public bool CheckDeleteBackup()
        {
            bool isDelete = false;
            // 폴더 경로 안에 있는 모든 폴더 가져오기
            string[] subFolders = Directory.GetDirectories(@"C:\Cookies\TEMP\DAILYBUSINESS\");

            // 가져온 폴더 출력
            foreach (string subFolder in subFolders)
            {
                string folderName = Path.GetFileName(subFolder);
                if (folderName.Length == 8)
                {
                    folderName = folderName.Substring(0, 4) + "-" + folderName.Substring(4, 2) + "-" + folderName.Substring(6, 2);
                    DateTime folder_date;
                    if (DateTime.TryParse(folderName, out folder_date) && folder_date < DateTime.Now.AddMonths(-1))
                    {
                        isDelete = true;
                        break;
                    }
                }
            }
            return isDelete;
        }
        public void DeleteBackup()
        {
            // 폴더 경로 안에 있는 모든 폴더 가져오기
            string[] subFolders = Directory.GetDirectories(@"C:\Cookies\TEMP\DAILYBUSINESS\");

            // 가져온 폴더 출력
            foreach (string subFolder in subFolders)
            {
                string folderName = Path.GetFileName(subFolder);
                if (folderName.Length == 8)
                {
                    folderName = folderName.Substring(0, 4) + "-" + folderName.Substring(4, 2) + "-" + folderName.Substring(6, 2);
                    DateTime folder_date;
                    if (DateTime.TryParse(folderName, out folder_date) && folder_date < DateTime.Now.AddMonths(-1))
                        Directory.Delete(subFolder, true);
                }
            }
        }

        private void _SetCookiePath(string prefix, string postfix)
        {
            string path = String.Empty;
            string name = String.Empty;

            _path = Path.Combine(_path, path);
            //경로가 존재하지 않으면 생성(System.IO 를 using 했기때문에 생략)
            if (!Directory.Exists(_path))
                 Directory.CreateDirectory(_path);
            //접두, 접미
            if (!String.IsNullOrEmpty(prefix))
                name = prefix + name;
            if (!String.IsNullOrEmpty(postfix))
                name = name + postfix;
            //확장자
            name += ".txt";

            //_path에 File 이름을 포함
            _path = Path.Combine(_path, name);
        }

        public void Write(bool auth, string id, string pass)
        {
            try
            {
                //경로의 파일을 열어서 data 기록, 없으면 생성(true)
                using (StreamWriter writer = new StreamWriter(_path, false))
                {
                    if (auth)
                    {
                        writer.Write(auth + "/" + id + "/" + pass);
                    }
                    else
                    {
                        writer.Write("");
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public List<string> GetFolderList(string rootPath)
        {
            // 가져온 텍스트 파일 목록을 출력합니다.
            List<string> folderList = new List<string>();
            string[] directories = Directory.GetDirectories(rootPath);
            foreach (string directory in directories)
            {
                Console.WriteLine(directory);
                string[] sub_directories = Directory.GetDirectories(directory);
                foreach (string sub_directory in sub_directories)
                {
                    folderList.Add(sub_directory);
                }
            }
            return folderList;
        }


        public List<string> GetTempList(string rootPath)
        {
            // 시작 폴더부터 모든 텍스트 파일 목록을 가져옵니다.
            string[] allTextFiles = GetAllTextFiles(rootPath);

            // 가져온 텍스트 파일 목록을 출력합니다.
            List<string> fileList = new List<string>();
            foreach (string file in allTextFiles)
            {
                fileList.Add(file);
            }
            return fileList;
        }

        static string[] GetAllTextFiles(string folderPath)
        {
            string[] textFiles = new string[0];
            // 현재 폴더에서 모든 텍스트 파일 목록을 가져옵니다.
            if (Directory.Exists(folderPath))
            { 
                textFiles = Directory.GetFiles(folderPath, "*.txt");

                // 모든 하위 폴더에서 텍스트 파일 목록을 재귀적으로 가져옵니다.
                string[] subfolderPaths = Directory.GetDirectories(folderPath);
                foreach (string subfolderPath in subfolderPaths)
                {
                    string[] subfolderTextFiles = GetAllTextFiles(subfolderPath);
                    textFiles = textFiles.Concat(subfolderTextFiles).ToArray();
                }
            }
            return textFiles;
        }

        public void SaveDailyTempWrite(Dictionary<int, string> shtDataDic, Dictionary<int, string> shtNameDic)
        {


            string resultData = "";
            foreach (int key in shtDataDic.Keys)
            {
                string shtName = "Sheet" + key;
                if (shtNameDic.ContainsKey(key))
                    shtName = shtNameDic[key];
                resultData += "\n$$$Sheet" + key + "$\n" + shtName + "$\n"  + shtDataDic[key];
            }

            using (StreamWriter writer = new StreamWriter(_path, false))
            {
                //TEMP FILE
                writer.Write(resultData.Trim());
            }
            //경로의 파일을 열어서 data 기록, 없으면 생성(true)
            string main_directory = Path.GetDirectoryName(_path);
            main_directory = Path.GetDirectoryName(main_directory) + @"\DAILYBUSINESS.txt";
            using (StreamWriter writer = new StreamWriter(main_directory, false))
            {
                //TEMP FILE
                writer.Write(resultData.Trim());
            }

        }

        public void SaveSalesManagerSetting(Dictionary<string, string> styleDic)
        {
            using (StreamWriter writer = new StreamWriter(_path, false))
            {
                //덮어쓰기
                string writeTxt = "";
                foreach (string key in styleDic.Keys)
                {
                    writeTxt += "\n" + key + "^" + styleDic[key];
                }
                writer.Write(writeTxt.Trim());
            }
        }
        public void DailyTempWrite(DataGridView dgv)
        {
            //경로의 파일을 열어서 data 기록, 없으면 생성(true)
            if (dgv.Rows.Count > 0)
            {
                using (StreamWriter writer = new StreamWriter(_path, false))
                {
                    //컬럼 정보
                    string columnInfo = "";
                    for (int i = 0; i < dgv.Columns.Count; i++)
                        columnInfo += "|" + dgv.Columns[i].Name;
                    //데이터정보
                    string dataInfo = "";
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        //cell_style
                        string total_cell_style = "";
                        string rowInfo = "";
                        for (int j = 0; j < dgv.Columns.Count; j++)
                        {
                            if (dgv.Rows[i].Cells[j].Value == null)
                                dgv.Rows[i].Cells[j].Value = "";

                            DataGridViewCell cell = dgv.Rows[i].Cells[j];
                            float font_size = 9;
                            string font_name = "나눔고딕";
                            bool font_bold = false;
                            bool font_italic = false;

                            Font fff = cell.Style.Font;
                            if (fff == null)
                                fff = dgv.Font;
                            if (fff != null)
                            {
                                font_size = fff.Size;
                                font_name = fff.Name;
                                font_bold = fff.Bold;
                                font_italic = fff.Italic;
                            }
                            Color fore_col = cell.Style.ForeColor;
                            if (fore_col.Name == "0")
                                fore_col = Color.Black;
                            //string fore_col_rgb = fore_col.R.ToString("X2") + "," + fore_col.G.ToString("X2") + "," + fore_col.B.ToString("X2");
                            int fore_col_rgb = fore_col.ToArgb();
                            Color back_col = cell.Style.BackColor;
                            if (back_col.Name == "0")
                                back_col = Color.White;
                            //string back_col_rgb = back_col.R.ToString("X2") + "," + back_col.G.ToString("X2") + "," + back_col.B.ToString("X2");
                            int back_col_rgb = back_col.ToArgb();
                            string cell_style = font_size.ToString() + "_" + font_name + "_" + font_bold.ToString() + "_" + font_italic.ToString() + "_#" + fore_col_rgb + "_#" + back_col_rgb;

                            //rowInfo += "|" + dgv.Rows[i].Cells[j].Value.ToString();
                            rowInfo += "|" + dgv.Rows[i].Cells[j].Value.ToString() + "^" + cell_style;
                        }

                        rowInfo = rowInfo.Substring(1, rowInfo.Length - 1);

                        dataInfo += "\n" + rowInfo;
                    }
                    columnInfo = columnInfo.Substring(1, columnInfo.Length - 1);
                    dataInfo = dataInfo.Trim();
                    //TEMP FILE
                    writer.Write(columnInfo + "\n\n" + dataInfo);
                }

                //경로의 파일을 열어서 data 기록, 없으면 생성(true)
                string main_directory = Path.GetDirectoryName(_path);
                main_directory = Path.GetDirectoryName(main_directory) + @"\DAILYBUSINESS.txt";
                using (StreamWriter writer = new StreamWriter(main_directory, false))
                {
                    //컬럼 정보
                    string columnInfo = "";
                    for (int i = 0; i < dgv.Columns.Count; i++)
                        columnInfo += "|" + dgv.Columns[i].Name;
                    //데이터정보
                    string dataInfo = "";
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        string rowInfo = "";
                        for (int j = 0; j < dgv.Columns.Count; j++)
                        {
                            if (dgv.Rows[i].Cells[j].Value == null)
                                dgv.Rows[i].Cells[j].Value = "";
                            DataGridViewCell cell = dgv.Rows[i].Cells[j];
                            float font_size = 9;
                            string font_name = "나눔고딕";
                            bool font_bold = false;
                            bool font_italic = false;

                            Font fff = cell.Style.Font;
                            if (fff == null)
                                fff = dgv.Font;
                            if (fff != null)
                            {
                                font_size = fff.Size;
                                font_name = fff.Name;
                                font_bold = fff.Bold;
                                font_italic = fff.Italic;
                            }

                            Color fore_col = cell.Style.ForeColor;
                            if (fore_col.Name == "0")
                                fore_col = Color.Black;
                            //string fore_col_rgb = fore_col.R.ToString("X2") + "," + fore_col.G.ToString("X2") + "," + fore_col.B.ToString("X2");
                            int fore_col_rgb = fore_col.ToArgb();
                            Color back_col = cell.Style.BackColor;
                            if (back_col.Name == "0")
                                back_col = Color.White;
                            //string back_col_rgb = back_col.R.ToString("X2") + "," + back_col.G.ToString("X2") + "," + back_col.B.ToString("X2");
                            int back_col_rgb = back_col.ToArgb();
                            string cell_style = font_size.ToString() + "_" + font_name + "_" + font_bold.ToString() + "_" + font_italic.ToString() + "_#" + fore_col_rgb + "_#" + back_col_rgb;

                            //rowInfo += "|" + dgv.Rows[i].Cells[j].Value.ToString();
                            rowInfo += "|" + dgv.Rows[i].Cells[j].Value.ToString() + "^" + cell_style;
                        }

                        rowInfo = rowInfo.Substring(1, rowInfo.Length - 1);

                        dataInfo += "\n" + rowInfo;
                    }
                    columnInfo = columnInfo.Substring(1, columnInfo.Length - 1);
                    dataInfo = dataInfo.Trim();
                    //TEMP FILE
                    writer.Write(columnInfo + "\n\n" + dataInfo);
                }
            }
        }


        public string GetDailyTempWriteTxt(DataGridView dgv)
        {
            //경로의 파일을 열어서 data 기록, 없으면 생성(true)
            string resultTxt = "";
            if (dgv.Rows.Count > 0)
            {
                //컬럼 정보
                string columnInfo = "";
                for (int i = 0; i < dgv.Columns.Count; i++)
                    columnInfo += "|" + dgv.Columns[i].Name;
                //데이터정보
                string dataInfo = "";
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    //cell_style
                    string total_cell_style = "";
                    string rowInfo = "";
                    for (int j = 0; j < dgv.Columns.Count; j++)
                    {
                        if (dgv.Rows[i].Cells[j].Value == null)
                            dgv.Rows[i].Cells[j].Value = "";

                        DataGridViewCell cell = dgv.Rows[i].Cells[j];
                        float font_size = 9;
                        string font_name = "나눔고딕";
                        bool font_bold = false;
                        bool font_italic = false;

                        Font fff = cell.Style.Font;
                        if (fff == null)
                            fff = dgv.Font;
                        if (fff != null)
                        {
                            font_size = fff.Size;
                            font_name = fff.Name;
                            font_bold = fff.Bold;
                            font_italic = fff.Italic;
                        }
                        Color fore_col = cell.Style.ForeColor;
                        if (fore_col.Name == "0")
                            fore_col = Color.Black;
                        //string fore_col_rgb = fore_col.R.ToString("X2") + "," + fore_col.G.ToString("X2") + "," + fore_col.B.ToString("X2");
                        int fore_col_rgb = fore_col.ToArgb();
                        Color back_col = cell.Style.BackColor;
                        if (back_col.Name == "0")
                            back_col = Color.White;
                        //string back_col_rgb = back_col.R.ToString("X2") + "," + back_col.G.ToString("X2") + "," + back_col.B.ToString("X2");
                        int back_col_rgb = back_col.ToArgb();
                        string cell_style = font_size.ToString() + "_" + font_name + "_" + font_bold.ToString() + "_" + font_italic.ToString() + "_#" + fore_col_rgb + "_#" + back_col_rgb;

                        //rowInfo += "|" + dgv.Rows[i].Cells[j].Value.ToString();
                        rowInfo += "|" + dgv.Rows[i].Cells[j].Value.ToString() + "^" + cell_style;
                    }

                    rowInfo = rowInfo.Substring(1, rowInfo.Length - 1);

                    dataInfo += "\n" + rowInfo;
                }
                columnInfo = columnInfo.Substring(1, columnInfo.Length - 1);
                dataInfo = dataInfo.Trim();
                //TEMP FILE
                resultTxt = columnInfo + "\n\n" + dataInfo;
            }
            return resultTxt;
        }

        public string[] AuthAccount()
        {
            string cookieValue = "";
            string[] account = new string[1];

            try
            {
                //경로의 파일을 열어서 data 기록, 없으면 생성(true)
                using (var cookie = new StreamReader(_path))
                {
                     cookieValue = cookie.ReadToEnd();
                    if (!string.IsNullOrEmpty(cookieValue))
                    {
                        account = cookieValue.Split('/');
                    }
                    else
                    {
                        account = null;
                    }
                }
            }
            catch (Exception ex)
            {
                account = null;
            }

            return account;
        }

        public string GetTempFile(string folder, string file)
        {
            string rootPath = @"C:\Cookies\TEMP\DAILYBUSINESS";
            if(folder != null && !string.IsNullOrEmpty(folder))
                rootPath += @"\" + folder;
            if (file != null && !string.IsNullOrEmpty(file))
                rootPath += @"\" + file;

            FileInfo fi = new FileInfo(rootPath);
            if (!fi.Exists)
                return null;

            string cookieValue = "";
            try
            {
                //경로의 파일을 열어서 data 기록, 없으면 생성(true)
                using (var cookie = new StreamReader(rootPath))
                {
                    cookieValue = cookie.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                
            }

            return cookieValue;
        }

        public string GetTempFileString(string folder, string file)
        {
            string rootPath = _path;
            if (folder != null && !string.IsNullOrEmpty(folder))
                rootPath += @"\" + folder;
            if (file != null && !string.IsNullOrEmpty(file))
                rootPath += @"\" + file;

            FileInfo fi = new FileInfo(rootPath);
            if (!fi.Exists)
                return null;

            string cookieValue = "";
            try
            {
                //경로의 파일을 열어서 data 기록, 없으면 생성(true)
                using (var cookie = new StreamReader(rootPath))
                {
                    cookieValue = cookie.ReadToEnd();
                }
            }
            catch (Exception ex)
            {

            }

            return cookieValue;
        }

    }
}
