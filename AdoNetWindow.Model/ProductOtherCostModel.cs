using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ProductOtherCostModel
    {
        public string group_name { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string cost_unit { get; set; }
        public double custom { get; set; }
        public double tax { get; set; }
        public double production_days { get; set; }
        public double incidental_expense { get; set; }
        public bool weight_calculate { get; set; }
        public bool tray_calculate { get; set; }
        public string manager { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
        public bool isMonth { get; set; }
        public string show_sttdate { get; set; }
        public string show_enddate { get; set; }
        public string hide_date { get; set; }
        public double purchase_margin { get; set; }
        public double base_around_month { get; set; }
        public string remark { get; set; }
    }

    public class CostCalculateProcut
    {
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string cost_unit { get; set; }
        public string company { get; set; }
        public string purchase_price { get; set; }
        public string updatetime { get; set; }
    }
}
