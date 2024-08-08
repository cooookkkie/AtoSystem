using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class HideModelasdfasd
    {
        public string category_code { get; set; }
        public string product_code { get; set; }
        public string origin_code { get; set; }
        public string sizes_code { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
        public string until_date { get; set; }
        public bool price_up { get; set; }
        public bool price_down { get; set; }
        public bool stock_up { get; set; }
        public int stock_up_qty { get; set; }
        public bool stock_down { get; set; }
        public int stock_down_qty { get; set; }
        public bool exhaust { get; set; }
    }

    public class HideModel
    {
        public int id { get; set; }
        public string division { get; set; }
        public string category { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string seaover_unit { get; set; }
        public string until_date { get; set; }
        public string hide_mode { get; set; }
        public string hide_details { get; set; }
        public string remark { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }
}
