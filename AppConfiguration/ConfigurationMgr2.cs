using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfiguration
{
    public class ConfigurationMgr2
    {
        //싱글 패턴
        private static ConfigurationMgr2 instance;
        public string ConnectionString { get; set; }
        IDbConnection connection;
        public IDbConnection Connection
        {
            get
            {
                if (connection == null)
                    connection = new MySqlConnection(ConnectionString);
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection;
            }
            private set { }
        }

        private ConfigurationMgr2()
        {
            //디자인 모드에서는 동작하지 않도록 한다.
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                LoadConfiguration();
            }
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
            ConnectionString = currentConfig.ConnectionStrings.ConnectionStrings["WinConnection2"].ConnectionString;
        }

        public static ConfigurationMgr2 Instance()
        {
            if (instance == null)
                instance = new ConfigurationMgr2();
            return instance;
        }

    }
}
