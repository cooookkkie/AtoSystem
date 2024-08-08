using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppConfiguration
{
    public class ResourceConfigurationMgr
    {
        public string GetServerPath()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = config.GetSection("connectionStrings");
            string data = null;
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
                string ftpPath = config.ConnectionStrings.ConnectionStrings["ResourceServer"].ToString();
                string user = config.ConnectionStrings.ConnectionStrings["ResourceServer_ID"].ToString();
                string pwd = config.ConnectionStrings.ConnectionStrings["ResourceServer_PASSWORD"].ToString();
                

                // WebRequest.Create로 Http,Ftp,File Request 객체를 모두 생성할 수 있다.
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpPath);
                // FTP 다운로드한다는 것을 표시
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                // 익명 로그인이 아닌 경우 로그인/암호를 제공해야
                req.Credentials = new NetworkCredential(user, pwd);

                // FTP Request 결과를 가져온다.
                using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
                {
                    // FTP 결과 스트림
                    Stream stream = resp.GetResponseStream();

                    // 결과를 문자열로 읽기 (바이너리로 읽을 수도 있다)
                    
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        data = reader.ReadToEnd();
                        
                    }
                }
            }
            return data;
        }
    }
}
