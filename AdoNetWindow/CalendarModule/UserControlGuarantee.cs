using AdoNetWindow.Model;
using AdoNetWindow.Pending;
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
    public partial class UserControlGuarantee : UserControl
    {
        calendar cd;
        UsersModel um;
        DataRow dr;
        public UserControlGuarantee(calendar cal, UsersModel uModel, DataRow gDt)
        {
            InitializeComponent();
            lbId.Text = gDt["id"].ToString();
            string str = gDt["ato_no"].ToString() + @" \" + ((int)(Convert.ToDouble(gDt["total_amount"].ToString()) * 0.7)).ToString("#,##0");
            lb.Text = str;

            if (gDt["eta_mode"].ToString() == "1")
            {
                lb.Font = new Font("나눔고딕", 9, FontStyle.Bold);
            }

            cd = cal;
            um = uModel;
            dr = gDt;
        }

        private void UserControlGuarantee_MouseHover(object sender, EventArgs e)
        {
            string str = "ETD : " + dr["etd"].ToString()
                + "\nETA : " + dr["eta"].ToString();
            
            this.toolTip1.ToolTipTitle = dr["ato_no"].ToString();
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this, str);
        }

        private void lb_MouseHover(object sender, EventArgs e)
        {
            string str = "ETD : " + dr["etd"].ToString()
                + "\nETA : " + dr["eta"].ToString();


            this.toolTip1.ToolTipTitle = dr["ato_no"].ToString();
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(lb, str);
        }

        private void label1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int id = int.Parse(lbId.Text.ToString());
            if (um.auth_level > 2)
            {
                UnPendingManager view = new UnPendingManager(cd, um, "", id);
                view.StartPosition = FormStartPosition.CenterParent;
                view.ShowDialog();
            }
            else
            {
                PendingView view = new PendingView(um, id.ToString(), "");
                view.StartPosition = FormStartPosition.CenterParent;
                view.ShowDialog();
            }
        }
    }
}
