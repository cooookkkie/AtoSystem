using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class CompanySalesModel
    {
        public int company_id { get; set; }
        public int sub_id { get; set; }
        public bool is_sales { get; set; }
        public string contents { get; set; }
        public string remark { get; set; }
        public string log { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
        public string pre_ato_manager { get; set; }

        public string from_ato_manager { get; set; }
        public string to_ato_manager { get; set; }
        public string from_category { get; set; }
        public string to_category { get; set; }
    }
}
