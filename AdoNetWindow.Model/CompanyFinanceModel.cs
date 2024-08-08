using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class CompanyFinanceModel
    {
        public int company_id { get; set; }
        public int year { get; set; }
        public double capital_amount { get; set; }
        public double debt_amount { get; set; }
        public double sales_amount { get; set; }
        public double net_income_amount { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }
}
