using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ProductLimitModel
    {
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string weight { get; set; }
        public double limit_qty { get; set; }
        public double limit_amount { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
    }
}
