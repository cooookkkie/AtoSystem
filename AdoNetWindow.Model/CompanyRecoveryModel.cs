using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class CompanyRecoveryModel
    {
        public string company_code { get; set; }
        public double last_year_profit { get; set; }
        public double net_operating_capital_rate { get; set; }
        public double operating_capital_rate { get; set; }
        public double ato_capital_rate { get; set; }
        public double equity_capital_rate { get; set; }
        public int target_recovery_month { get; set; }
        public string remark { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
    }
}
