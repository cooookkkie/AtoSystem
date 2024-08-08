using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ProductGroupModel
    {
        public int main_id { get; set; }
        public int sub_id { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public string item_product { get; set; }
        public string item_origin { get; set; }
        public string item_sizes { get; set; }
        public string item_unit { get; set; }
        public string item_price_unit { get; set; }
        public string item_unit_count { get; set; }
        public string item_seaover_unit { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }

    }
}
