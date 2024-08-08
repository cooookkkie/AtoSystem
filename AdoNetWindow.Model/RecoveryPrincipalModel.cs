using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class RecoveryPrincipalModel
    {
        public string company { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public double monthly_sales_price { get; set; }
        public double cumulative_sales_price { get; set; }
        public double balance_due { get; set; }
        public double averge_balance_due { get; set; }
        public string grade { get; set; }
        public string updatetime { get; set; }
    }
}
