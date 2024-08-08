using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class SalesPartnerModel
    {
        public string name { get; set; }
        public double target_margin_rate { get; set; }
        public double target_recovery_month { get; set; }
        public string company_grade { get; set; }
        public string manager { get; set; }
        public string updatetime { get; set; }
        public string remark { get; set; }
    }
}
