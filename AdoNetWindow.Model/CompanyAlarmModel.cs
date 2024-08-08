using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class CompanyAlarmModel
    {
        public int company_id { get; set; }
        public string division { get; set; }
        public string category { get; set; }
        public string alarm_date { get; set; }
        public string alarm_remark { get; set; }
        public bool alarm_complete { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }
}
