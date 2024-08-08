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
    public partial class UserControlCreditTitle : UserControl
    {
        public UserControlCreditTitle(string division)
        {
            InitializeComponent();
            lbDivision.Text = division;
        }
    }
}
