using AppConfiguration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace AdoNetWindow
{
    public partial class ApplicationRoot : Form
    {
        protected ConfigurationMgr1  instance;

        public ApplicationRoot()
        {
            InitializeComponent();
            //디자인 모드에서는 동작하지 않도록 한다.
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            { 
                instance = ConfigurationMgr1.Instance();
            }
        }

    }
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            //디자인 모드에서는 동작하지 않도록 한다.
            if (System.ComponentModel.LicenseManager.UsageMode != System.ComponentModel.LicenseUsageMode.Designtime)
            {
                Type dgvType = dgv.GetType();
                PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                    BindingFlags.Instance | BindingFlags.NonPublic);
                pi.SetValue(dgv, setting, null);
            }
        }
    }
}