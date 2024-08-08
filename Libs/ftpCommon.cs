using AppConfiguration;
using FAXCOMEXLib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Libs
{
    public class ftpCommon
    {
        public delegate void ExceptionEventHandler(string LocationID, Exception ex);
        public event ExceptionEventHandler ExceptionEvent;

        public Exception LastException = null;
        NetDrive netdrive = new NetDrive();
        public bool isConnected { get; set; }

        private string ipAddr = string.Empty;
        private string port = string.Empty;
        private string userid = string.Empty;
        private string pwd = string.Empty;


        public bool StartTradePaperFolder(string contract_year, string ato_no, out string errMsg)
        {
            string drive_name = "I";
            string folder_path = "아토무역/무역/무역1/ㄴ.수입자료/서류";
            DirectoryInfo di = new DirectoryInfo("I:");
            //DirectoryInfo.Exists로 폴더 존재유무 확인, 없으면 네트워크 드라이브 연결
            if (!di.Exists)
            {
                if (!StartDirectory(folder_path, out errMsg, "", "", "ATO/" + folder_path, drive_name))
                    return false;
            }
            //폴더찾기
            int d;
            //3글자 아토번호
            if(!int.TryParse(ato_no.Substring(2, 1), out d))
                folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 3);
            //2글자 아토번호
            else
                folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 2);
            bool findFlag = false;
            di = new DirectoryInfo(folder_path);
            if (di.Exists)
            {
                //긴경우 다른 문자 포함으로 판단후 필요한 내용만 추출
                if (ato_no.Length > 5 && !ato_no.Contains("취소") && !ato_no.Contains("삭제"))
                {
                    /*if (!int.TryParse(ato_no.Substring(2, 1), out d))
                        ato_no = ato_no.Substring(0, 5);
                    else
                        ato_no = ato_no.Substring(0, 4);*/
                }

                string[] dirs = Directory.GetDirectories(folder_path);
                foreach (string dir in dirs)
                {
                    if (Path.GetFileName(dir).Contains(ato_no))
                    {
                        di = new DirectoryInfo(dir);
                        //DirectoryInfo.Exists로 폴더 존재유무 확인
                        if (di.Exists)
                            System.Diagnostics.Process.Start(dir);
                        findFlag = true;
                        break;
                    }
                }
                //결과출력
                if (findFlag)
                {
                    errMsg = "";
                    return true;
                }
                else
                {
                    errMsg = "폴더를 찾을 수 없습니다.";
                    return false;
                }
            }
            else
            {
                errMsg = "폴더를 찾을 수 없습니다.";
                return false;
            }
        }

        public bool StartTradeBoxDegisnFolder(string manager, string product, string origin, out string errMsg)
        {
            string drive_name = "U";
            string folder_path = "아토무역/무역/무역1/ㄴ.수입자료/박스디자인";
            DirectoryInfo di = new DirectoryInfo("U:");
            //DirectoryInfo.Exists로 폴더 존재유무 확인, 없으면 네트워크 드라이브 연결
            if (!di.Exists)
            {
                if (!StartDirectory(folder_path, out errMsg, "", "", "ATO/" + folder_path, drive_name))
                    return false;
            }
            //폴더찾기
            int d;
            product = product.Replace(@"\", "");
            product = product.Replace(@"/", "");
            product = product.Replace(@":", "");
            product = product.Replace(@"*", "");
            product = product.Replace(@"?", "");
            product = product.Replace(@"<", "");
            product = product.Replace(@">", "");
            product = product.Replace(@".", ",");

            origin = origin.Replace(@"\", "");
            origin = origin.Replace(@"/", "");
            origin = origin.Replace(@":", "");
            origin = origin.Replace(@"*", "");
            origin = origin.Replace(@"?", "");
            origin = origin.Replace(@"<", "");
            origin = origin.Replace(@">", "");
            origin = origin.Replace(@".", ",");


            folder_path = drive_name + @":\" + manager + @"\" + product + @"\" + origin;
            bool findFlag = false;
            di = new DirectoryInfo(folder_path);
            if (di.Exists)
            {
                try
                {
                    System.Diagnostics.Process.Start(folder_path);
                    errMsg = "";
                    return true;
                }
                catch
                {
                    folder_path = @"Z:\아토무역\무역\무역1\ㄴ.수입자료\박스디자인\" + manager + @"\" + product + @"\" + origin;
                    System.Diagnostics.Process.Start(folder_path);

                    errMsg = "";
                    return true;
                }
            }
            else
            {
                errMsg = "폴더를 찾을 수 없습니다.";
                return false;
            }
        }

        public bool CreateFolder(string folderPath)
        {
            bool isCreate = false;
            string drive_name = "U";
            string rootPath = @"아토무역/무역/무역1/ㄴ.수입자료/박스디자인";
            // 폴더가 존재하지 않으면 생성
            if (!Directory.Exists(drive_name  + @":\" + folderPath))
            {
                try
                {
                    Directory.CreateDirectory(drive_name + @":\" + folderPath);
                    Console.WriteLine("폴더가 생성되었습니다.");
                    isCreate = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("폴더가 생성중 에러가 발생하였습니다.");
                    isCreate = false;
                }

            }
            else
            {
                Console.WriteLine("폴더가 이미 존재합니다.");
                isCreate = true;
            }
            return isCreate;
        }



        public string StartTradePaperFolderPath(string contract_year, string ato_no, out string errMsg)
        {
            string drive_name = "I";
            string folder_path = "아토무역/무역/무역1/ㄴ.수입자료/서류";
            DirectoryInfo di = new DirectoryInfo("I:");
            //DirectoryInfo.Exists로 폴더 존재유무 확인, 없으면 네트워크 드라이브 연결
            if (!di.Exists)
            {
                if (!StartDirectory(folder_path, out errMsg, "", "", "ATO/" + folder_path, drive_name))
                    return null;
            }
            //폴더찾기
            int d;
            //3글자 아토번호
            if (!int.TryParse(ato_no.Substring(2, 1), out d))
                folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 3);
            //2글자 아토번호
            else
                folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 2);


            //folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 2);
            bool findFlag = false;
            di = new DirectoryInfo(folder_path);
            if (di.Exists)
            {
                string[] dirs = Directory.GetDirectories(folder_path);
                foreach (string dir in dirs)
                {
                    if (!dir.Contains("취소") && !dir.Contains("삭제") && dir.Contains(ato_no))
                    {
                        errMsg = "";
                        return dir;
                    }
                }
                
                errMsg = "폴더를 찾을 수 없습니다.";
                return "";
            }
            else
            {
                errMsg = "폴더를 찾을 수 없습니다.";
                return "";
            }
        }
        public string[] GetTradePaperFIles(string contract_year, string ato_no, out string errMsg)
        {
            string drive_name = "I";
            string folder_path = "아토무역/무역/무역1/ㄴ.수입자료/서류";
            DirectoryInfo di = new DirectoryInfo("I:");
            //DirectoryInfo.Exists로 폴더 존재유무 확인, 없으면 네트워크 드라이브 연결
            if (!di.Exists)
            {
                if (!StartDirectory(folder_path, out errMsg, "", "", "ATO/" + folder_path, drive_name))
                    return null;
            }
            //폴더찾기
            int d;
            //3글자 아토번호
            if (!int.TryParse(ato_no.Substring(2, 1), out d))
                folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 3);
            //2글자 아토번호
            else
                folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 2);
            //folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 2);
            bool findFlag = false;
            di = new DirectoryInfo(folder_path);
            if (di.Exists)
            {
                //긴경우 다른 문자 포함으로 판단후 필요한 내용만 추출
                if (ato_no.Length > 5 && !ato_no.Contains("취소") && !ato_no.Contains("삭제"))
                {
                    /*if (!int.TryParse(ato_no.Substring(2, 1), out d))
                        ato_no = ato_no.Substring(0, 5);
                    else
                        ato_no = ato_no.Substring(0, 4);*/
                }

                string[] dirs = Directory.GetDirectories(folder_path);
                foreach (string dir in dirs)
                {
                    if (dir.Contains(ato_no))
                    {
                        di = new DirectoryInfo(dir);
                        //DirectoryInfo.Exists로 폴더 존재유무 확인
                        if (di.Exists)
                        {
                            dirs = Directory.GetFiles(dir);
                            errMsg = "";
                            return dirs;
                        }
                    }
                }
                errMsg = "폴더를 찾을 수 없습니다.";
                return null;
            }
            else
            {
                errMsg = "폴더를 찾을 수 없습니다.";
                return null;
            }
        }
        public string GetTradePaperPath(string contract_year, string ato_no, out string errMsg)
        {
            string drive_name = "I";
            string folder_path = "아토무역/무역/무역1/ㄴ.수입자료/서류";
            DirectoryInfo di = new DirectoryInfo("I:");
            //DirectoryInfo.Exists로 폴더 존재유무 확인, 없으면 네트워크 드라이브 연결
            if (!di.Exists)
            {
                if (!StartDirectory(folder_path, out errMsg, "", "", "ATO/" + folder_path, drive_name))
                    return null;
            }
            //폴더찾기
            int d;
            //3글자 아토번호
            if (!int.TryParse(ato_no.Substring(2, 1), out d))
                folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 3);
            //2글자 아토번호
            else
                folder_path = drive_name + @":\" + contract_year + @"년\" + ato_no.Substring(0, 1) + @"\" + ato_no.Substring(0, 2);
            //folder_path = drive_name + @":\" + contract_year + "년/" + ato_no.Substring(0, 1) + "/" + ato_no.Substring(0, 2) ;
            di = new DirectoryInfo(folder_path);
            if (di.Exists)
            {
                string[] dirs = Directory.GetDirectories(folder_path);
                foreach (string dir in dirs)
                {
                    if (!dir.Contains("삭제") && !dir.Contains("취소") && dir.Contains(ato_no))
                    {
                        di = new DirectoryInfo(dir);
                        //DirectoryInfo.Exists로 폴더 존재유무 확인
                        if (di.Exists)
                        {
                            errMsg = "";
                            return dir.ToString();
                        }
                    }
                }
                errMsg = "폴더를 찾을 수 없습니다.";
                return "";
            }
            else
            {
                errMsg = "폴더를 찾을 수 없습니다.";
                return "";
            }
        }

        public void LoadConfiguration()
        {
            Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            this.ipAddr = currentConfig.ConnectionStrings.ConnectionStrings["ResourceServer_IP"].ConnectionString;
            this.port = currentConfig.ConnectionStrings.ConnectionStrings["ResourceServer_PORT"].ConnectionString;
            this.userid = currentConfig.ConnectionStrings.ConnectionStrings["ResourceServer_ID"].ConnectionString;
            this.pwd = currentConfig.ConnectionStrings.ConnectionStrings["ResourceServer_PASSWORD"].ConnectionString;
        }
        public Image GetUrlImage(string url)
        {
            //LoadConfiguration();
            try
            {
                using (WebClient client = new WebClient())
                {
                    byte[] imgArray;
                    imgArray = client.DownloadData(url);

                    using (MemoryStream memstr = new MemoryStream(imgArray))
                    {
                        Image img = Image.FromStream(memstr);
                        return img;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ReplaceName(string path)
        {
            return path.Replace("*", "x").Replace("?", "").Replace(">", "").Replace("<", "").Replace("|", "").Replace(".", "").Replace("/", "^");
        }
        public bool RenameDirectory(string contact_yaer, string ato_no, string update_folder_path, bool is_make = true, string origin_folder_path = "")
        {
            string drive_name = "I";
            string folder_path = "아토무역/무역/무역1/ㄴ.수입자료/서류";
            DirectoryInfo di = new DirectoryInfo("I:");
            //DirectoryInfo.Exists로 폴더 존재유무 확인, 없으면 네트워크 드라이브 연결
            string errMsg;
            if (!di.Exists)
            {
                if (!StartDirectory(folder_path, out errMsg, "", "", "ATO/" + folder_path, drive_name))
                    return false;
            }
            //기존 폴더찾기
            if(string.IsNullOrEmpty(origin_folder_path))
                origin_folder_path = GetTradePaperPath(contact_yaer, ato_no, out errMsg).Replace(@"\", @"/");
            if (origin_folder_path != null && !string.IsNullOrEmpty(origin_folder_path))
            {
                update_folder_path = drive_name + @":/" + update_folder_path;
                System.IO.Directory.Move(origin_folder_path, update_folder_path);
                return true;
            }
            else
            {
                if (is_make)
                { 
                    folder_path = drive_name + @":/" + update_folder_path;
                    if (MakeDirectory(folder_path))
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
        }



        public bool CheckDirectory(string folder_path, bool isMake = false, string root_path = "Solution/Document")
        {
            LoadConfiguration();
            FtpWebRequest ftpRequest = null;
            try
            {
                string path = string.Format("ftp://{0}:{1}/{2}", ipAddr, port, root_path);
                string[] folders = folder_path.Split('/');
                for (int i = 0; i < folders.Length; i++)
                {
                    path += "/" + folders[i];
                    ftpRequest = (FtpWebRequest)WebRequest.Create(path);
                    ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                    ftpRequest.Credentials = new NetworkCredential(userid, pwd);

                    var request = ftpRequest;
                    request.Method = WebRequestMethods.Ftp.ListDirectory;

                    try
                    {
                        using (var result = (FtpWebResponse)request.GetResponse())
                        {
                            result.Close();  //정상 종료
                        }
                    }
                    catch (WebException e)
                    {
                        FtpWebResponse response = (FtpWebResponse)e.Response;

                        if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                        {
                            Console.WriteLine("Does not exist");
                            if (isMake)
                                MakeDirectory(path);
                            else
                                return false;

                        }
                        else if (e.Status == WebExceptionStatus.ProtocolError)
                        {
                            Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                            Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                            return false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                ftpRequest = null;
                return false;
            }
            return true;
        }
        public bool StartDirectory(string folder_path, out string errMsg, string userid = "", string pwd = "", string root_path = "Solution/Document", string drive_name = "O")
        {
            LoadConfiguration();
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(pwd))
            {
                this.userid = userid;
                this.pwd = pwd;
            }

            errMsg = "";
            try
            {
                //string path = string.Format("ftp://{0}:{1}/Solution/Document", ipAddr, port);
                //string path = string.Format("ftp://{0}:{1}@{2}:6009/Solution/Document/{4}", userid, pwd, ipAddr, port, folder_path);
                //string path = string.Format("ftp://{0}:{1}@{2}:{3}/Solution/Document/{4}", userid, pwd, ipAddr, port, folder_path);

                // 네트워크 드라이브 연결
                string path = string.Format("https://{2}:{3}/{4}", this.userid, this.pwd, ipAddr, "6009", root_path);
                int getReturn = netdrive.setRemoteConnection(path, this.userid, this.pwd, $"{drive_name}:");

                if (getReturn == 1219)
                {
                    errMsg = "입력한 계정정보는 찾을 수 없습니다.";
                    return false;
                }
                else if (getReturn != 0)
                {
                    errMsg = "네트워크 드라이브를 연결할 수 없습니다.\r\n연결 정보를 확인하세요.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }


        public bool Cancelserver(string server)
        {
            try
            {
                netdrive.CencelRemoteServer("O:");
            }
            catch 
            {
                return false;
            }
            return true;
        }

        public bool MakeDirectory(string Directory)
        {
            string URI = Directory;
            FtpWebRequest ftp = WebRequest.Create(new Uri(URI)) as FtpWebRequest;
            ftp.Credentials = new NetworkCredential(userid, pwd);
            var request = ftp;
            request.Method = WebRequestMethods.Ftp.MakeDirectory;

            try
            {
                using (var resp = (FtpWebResponse)request.GetResponse())
                {
                    resp.Close();
                }
            }
            catch (WebException e)
            {
                return false;
            }
            return true;
        }

        public string[] GetFileList(string folder_path, string root_path = "Solution/Document")
        {
            LoadConfiguration();
            FtpWebRequest ftpRequest = null;
            try
            {
                string path = string.Format("ftp://{0}:{1}/{2}/{3}", ipAddr, port, root_path, folder_path);
                ftpRequest = (FtpWebRequest)WebRequest.Create(path);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpRequest.Credentials = new NetworkCredential(userid, pwd);

                var request = ftpRequest;
                request.Method = WebRequestMethods.Ftp.ListDirectory;

                try
                {
                    using (FtpWebResponse resFtp = (FtpWebResponse)request.GetResponse())
                    {
                        StreamReader reader;
                        reader = new StreamReader(resFtp.GetResponseStream());
                        string strData;
                        strData = reader.ReadToEnd();
                        string[] filesInDirectory = strData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                        resFtp.Close();
                        return filesInDirectory;
                    }
                }
                catch (WebException e)
                {
                    FtpWebResponse response = (FtpWebResponse)e.Response;


                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        Console.WriteLine("Does not exist");
                        return null;
                    }
                    else if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        Console.WriteLine("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                        Console.WriteLine("Status Description : {0}", ((HttpWebResponse)e.Response).StatusDescription);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }
        public bool UploadFiles(string folder_path, string file_path, string root_path = "Solution/Document")
        {
            LoadConfiguration();
            FtpWebRequest ftpRequest = null;
            try
            {
                string path = string.Format("ftp://{0}:{1}/{2}/{3}/{4}", ipAddr, port, root_path, folder_path, Path.GetFileName(file_path));
                ftpRequest = (FtpWebRequest)WebRequest.Create(path);
                ftpRequest.Credentials = new NetworkCredential(userid, pwd);
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                

                FileInfo fileInfo = new FileInfo(file_path);
                FileStream fileStream = fileInfo.OpenRead();

                int bufferLenth = 2048;
                byte[] buffer = new byte[bufferLenth];

                Stream uploadStream = ftpRequest.GetRequestStream();
                int contentLength = fileStream.Read(buffer, 0, bufferLenth);

                while (contentLength > 0)
                { 
                    uploadStream.Write(buffer, 0, contentLength);
                    contentLength = fileStream.Read(buffer, 0, bufferLenth);
                }

                uploadStream.Close();
                fileStream.Close();

                ftpRequest = null;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool DownloadFiles(string folder_path, string saver_path)
        {
            LoadConfiguration();
            FtpWebRequest ftpRequest = null;
            try
            {
                string path = string.Format("ftp://{0}:{1}/Solution/Document/{2}", ipAddr, port, folder_path);
                ftpRequest = (FtpWebRequest)WebRequest.Create(path);
                ftpRequest.Credentials = new NetworkCredential(userid, pwd);
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;


                //FTP서버에 파일다운로드요청
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();

                int fileContentsLength = 4096;
                byte[] fileContents = new byte[fileContentsLength];

                Stream stream = new FileStream(saver_path, FileMode.Create);

                //이진데이터로쓰기
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    using (BinaryReader br = new BinaryReader(responseStream))
                    {
                        fileContentsLength = br.Read(fileContents, 0, fileContentsLength);
                        while (fileContentsLength > 0)
                        {
                            fileContentsLength = br.Read(fileContents, 0, fileContentsLength);
                            bw.Write(fileContents);
                            fileContents = new byte[fileContentsLength];
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DownloadFilesWebClient(string folder_path, string saver_path)
        {
            LoadConfiguration();
            try
            {
                string path = string.Format("ftp://{0}:{1}/Solution/Document/{2}", ipAddr, port, folder_path);


                // WebClient 객체 생성
                using (WebClient cli = new WebClient())
                {
                    // FTP 사용자 설정
                    cli.Credentials = new NetworkCredential(userid, pwd);

                    // FTP 다운로드 실행
                    cli.DownloadFile(path, saver_path);

                    // FTP 업로드 실행
                    // cli.UploadFile(ftpPath, outputFile);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public bool DeleteFTPFile(string targetURI)
        {
            LoadConfiguration();
            try
            {
                string path = string.Format("ftp://{0}:{1}/Solution/Document/{2}", ipAddr, port, targetURI);
                FtpWebRequest ftpWebRequest = WebRequest.Create(path) as FtpWebRequest;

                ftpWebRequest.Credentials = new NetworkCredential(userid, pwd);
                ftpWebRequest.Method = WebRequestMethods.Ftp.DeleteFile;

                FtpWebResponse ftpWebResponse = ftpWebRequest.GetResponse() as FtpWebResponse;
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}


