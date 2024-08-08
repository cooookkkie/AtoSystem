using AdoNetWindow.SaleManagement.SalesManagerModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdoNetWindow.SaleManagement.AddSaleCompany
{
    public partial class UnitDays : UserControl
    {
        AddCompanyInfo aci = null;
        AlarmSettingManager asm = null;
        DateTime dt;
        public UnitDays(AddCompanyInfo aci, DateTime dt)
        {
            InitializeComponent();

            this.aci = aci;
            this.dt = dt;

            lbDay.Text = dt.Day.ToString();
            if (dt.DayOfWeek == DayOfWeek.Sunday)
                lbDay.ForeColor = Color.Red;
            else if (dt.DayOfWeek == DayOfWeek.Saturday)
                lbDay.ForeColor = Color.Blue;
        }

        public UnitDays(AlarmSettingManager asm, DateTime dt)
        {
            InitializeComponent();

            this.asm = asm;
            this.dt = dt;

            lbDay.Text = dt.Day.ToString();
            if (dt.DayOfWeek == DayOfWeek.Sunday)
                lbDay.ForeColor = Color.Red;
            else if (dt.DayOfWeek == DayOfWeek.Saturday)
                lbDay.ForeColor = Color.Blue;
        }

        private void UnitDays_MouseClick(object sender, MouseEventArgs e)
        {
            if (aci != null)
                aci.AddAlarmEvent(dt);
            else if (asm != null)
                asm.AddAlarmEvent(dt);
        }

        private void lbDay_MouseClick(object sender, MouseEventArgs e)
        {
            if (aci != null)
                aci.AddAlarmEvent(dt);
            else if(asm != null)
                asm.AddAlarmEvent(dt);
        }

        private void UnitDays_MouseLeave(object sender, EventArgs e)
        {
            this.BorderStyle = BorderStyle.None;
        }

        private void UnitDays_MouseUp(object sender, MouseEventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        private void lbDay_MouseUp(object sender, MouseEventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
