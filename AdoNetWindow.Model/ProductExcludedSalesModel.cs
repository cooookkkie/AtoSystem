using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ProductExcludedSalesModel
    {
        public int id { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public string sale_date { get; set; }
        public string sale_company { get; set; }
        public double sale_qty { get; set; }
        public string remark { get; set; }
    }
}
