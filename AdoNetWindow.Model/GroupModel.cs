using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class GroupModel
    {
        public int id { get; set; }
        public int sub_id { get; set; }
        public string group_division { get; set; }
        public string group_name { get; set; }
        public string form_name { get; set; }
        public int form_type { get; set; }
        public string form_remark { get; set; }
        public string category { get; set; }
        public string product { get; set; }
        public string product_code { get; set; }
        public string origin { get; set; }
        public string origin_code { get; set; }
        public string sizes { get; set; }
        public string sizes_code { get; set; }
        public string unit { get; set; }
        public string cost_unit { get; set; }
        public double month_around { get; set; }
        public double enable_days { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public int row { get; set; }
        public string division { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
        public string createdatetime { get; set; }

        public double offer_price { get; set; }
        public double offer_cost_price { get; set; }
        public string offer_company { get; set; }
        public string offer_updatetime { get; set; }
    }
}
