using AdoNetWindow.Model;
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
    public partial class UserControlCredit : UserControl
    {
        public UserControlCredit(LoanInfo model)
        {
            InitializeComponent();
            lbBank.Text = model.bank;
            txtAtsightLimit.Text = model.atsight_loan_limit.ToString("#,##0.00");
            txtAtsightUsed.Text = model.atsight_loan_used.ToString("#,##0.00");
            txtAtsightBalance.Text = (model.atsight_loan_limit - model.atsight_loan_used).ToString("#,##0.00");
            txtUsanceLimit.Text = model.usance_loan_limit.ToString("#,##0.00");
            txtUsanceUsed.Text = model.usance_loan_used.ToString("#,##0.00");
            txtUsanceBalance.Text = (model.usance_loan_limit - model.usance_loan_used).ToString("#,##0.00");
        }
        public UserControlCredit(double usance_total, double usance_used, double atsight_total, double atsight_used)
        {
            InitializeComponent();
            lbBank.Text = "총합";
            txtAtsightLimit.Text = atsight_total.ToString("#,##0.00");
            txtAtsightUsed.Text = atsight_used.ToString("#,##0.00");
            txtAtsightBalance.Text = (atsight_total - atsight_used).ToString("#,##0.00");
            txtUsanceLimit.Text = usance_total.ToString("#,##0.00");
            txtUsanceUsed.Text = usance_used.ToString("#,##0.00");
            txtUsanceBalance.Text = (usance_total - usance_used).ToString("#,##0.00");
        }
    }
}
