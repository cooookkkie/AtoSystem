using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfiguration
{
    public class ConfigurationMgr3
    {
        //싱글 패턴
        private static ConfigurationMgr3 instance;
        public string ConnectionString { get; set; }
        IDbConnection connection;
        public IDbConnection Connection
        {
            get
            {
                if (connection == null)
                    connection = new SqlConnection(ConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection;
            }
            private set { }
        }

        private ConfigurationMgr3()
        {
            //디자인 모드에서는 동작하지 않도록 한다.
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
                LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConfigurationSection section = currentConfig.GetSection("connectionStrings");
            if (section != null)
            {
                if (!section.IsReadOnly())
                {
                    if (section.SectionInformation.IsProtected == false)
                    {
                        section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                        section.SectionInformation.ForceSave = true;
                        currentConfig.Save(ConfigurationSaveMode.Full);
                    }
                }
            }
            ConnectionString = currentConfig.ConnectionStrings.ConnectionStrings["SeaoverConnection"].ConnectionString;
        }

        public static ConfigurationMgr3 Instance()
        {
            if (instance == null)
                instance = new ConfigurationMgr3();
            return instance;
        }
    }
}
