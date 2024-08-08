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
    public partial class UserControlBlank : UserControl
    {
        public UserControlBlank(DateTime sdate)
        {
            InitializeComponent();

            if (sdate.DayOfWeek == DayOfWeek.Saturday || sdate.DayOfWeek == DayOfWeek.Sunday)
            {
                this.Width = 119;
            }
            else
            { 
                this.Width = 259; 
            }
        }
        
    }
}
