using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AppConfiguration
{
    public class ConfigurationWebFtpMgr
    {
        private static ConfigurationWebFtpMgr instance;
        public static string fldPath { get; set; }
        private string ftpPath { get; set; }
        private string user { get; set; }
        private string pwd { get; set; }

        WebRequest webRequest;
        public WebRequest CreateFolder
        {
            get
            {
                string uri = ftpPath;
                if (!string.IsNullOrEmpty(fldPath))
                    uri += fldPath + "/";
                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(uri);
                requestFTPUploader.Credentials = new NetworkCredential(user, pwd);
                var request = requestFTPUploader;


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
                }

                return (WebRequest)requestFTPUploader;
            }
            private set { }
        }

        public WebRequest Connect
        {
            get
            {
                if (webRequest == null)
                {
                    string url = ftpPath;
                    if (!string.IsNullOrEmpty(fldPath))
                        url += "/" + ftpPath;
                    FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpPath);
                    requestFTPUploader.Credentials = new NetworkCredential(user, pwd);
                    requestFTPUploader.KeepAlive = true;
                    requestFTPUploader.Method = WebRequestMethods.Ftp.ListDirectory;
                    requestFTPUploader.UsePassive = false;

                    webRequest = requestFTPUploader;
                }
                return webRequest;
            }
            private set { }
        }
        public WebRequest Close
        {
            get
            {
                webRequest = null;
                return webRequest;
            }
            private set { }
        }

        private ConfigurationWebFtpMgr()
        {
            //디자인 모드에서는 동작하지 않도록 한다.
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadConfiguration();
            }
        }

        private void LoadConfiguration()
        {
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
                ftpPath = config.ConnectionStrings.ConnectionStrings["ResourceServer"].ToString();
                user = config.ConnectionStrings.ConnectionStrings["ResourceServer_ID"].ToString();
                pwd = config.ConnectionStrings.ConnectionStrings["ResourceServer_PASSWORD"].ToString();

            }
        }

        public static ConfigurationWebFtpMgr Instance(string folderPath = "")
        {
            fldPath = folderPath;
            if (instance == null)
                instance = new ConfigurationWebFtpMgr();
            return instance;
        }
    }
}
