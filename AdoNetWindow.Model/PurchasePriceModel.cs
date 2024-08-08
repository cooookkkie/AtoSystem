using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class PurchasePriceModel
    {
        public int id { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string cost_unit { get; set; }
        public double custom { get; set; }
        public double tax { get; set; }
        public double incidental_expense { get; set; }
        public string company { get; set; }
        public double purchase_price { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
        public bool is_private { get; set; }
        public bool weight_calculate { get; set; }
        public double fixed_tariff { get; set; }
        public bool is_FOB { get; set; }
    }
}
