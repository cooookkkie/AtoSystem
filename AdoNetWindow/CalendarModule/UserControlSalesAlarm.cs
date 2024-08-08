using AdoNetWindow.Model;
using AdoNetWindow.SaleManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.CalendarModule
{
    public partial class UserControlSalesAlarm : UserControl
    {
        calendar cd = null;
        UsersModel um = null;
        public UserControlSalesAlarm(DataRow dr, int days, calendar cd, UsersModel um, bool isAccent = false)
        {
            InitializeComponent();
            this.cd = cd;
            this.um = um;

            switch (dr["division"].ToString())
            {
                case "발주":
                    this.BackColor = Color.FromArgb(238, 235, 248);
                    break;
                case "신규":
                    this.BackColor = Color.FromArgb(226, 250, 234);
                    break;
                case "결제":
                    this.BackColor = Color.FromArgb(255, 255, 223);
                    break;
                case "기타":
                    this.BackColor = Color.White;
                    break;
            }
            lbEnd.Text = dr["division"].ToString() + "-";
            lbEnd.ForeColor = Color.Black;

            lbId.Text = dr["company_id"].ToString();
            lb.Text = dr["company"].ToString();

            if (!string.IsNullOrEmpty(dr["alarm_remark"].ToString()))
            {
                ToolTip toolTip1 = new ToolTip();
                toolTip1.ToolTipTitle = "";
                toolTip1.IsBalloon = true;
                toolTip1.SetToolTip(this, dr["alarm_remark"].ToString());
                toolTip1.SetToolTip(lb, dr["alarm_remark"].ToString());
            }
        }

        private void UserControlSalesAlarm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                AddCompanyInfo aci = new AddCompanyInfo(um, lbId.Text);
                aci.ShowDialog();
            }
            catch
            { }
        }

        private void lb_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                AddCompanyInfo aci = new AddCompanyInfo(um, lbId.Text);
                aci.ShowDialog();
            }
            catch
            { }
        }
    }
}
