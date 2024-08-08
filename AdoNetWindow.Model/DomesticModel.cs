using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class DomesticModel
    {
        public int id { get; set; }
        public int sub_id{ get; set; }
        public string etd { get; set; }
        public string warehousing_date { get; set; }
        public string status { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }

        public double material_price { get; set; }
        public double qty { get; set; }
        public double cost_price { get; set; }
        public string weight { get; set; }
        public string work_weight { get; set; }
        public double yiled { get; set; }


        public double work_fee { get; set; }
        public double work_fee_per_box { get; set; }
        public double transportation_fee { get; set; }
        public bool is_vat_transportation { get; set; }
        public double bag_fee { get; set; }
        public double sticker_fee { get; set; }
        public double tape_fee { get; set; }
        public double box_fee { get; set; }
        public bool is_vat_box { get; set; }
        public bool is_calculate { get; set; }
        public string remark { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
    }


    public class DomesticExpenseModel
    {
        public int domestic_id { get; set; }
        public string division { get; set; }
        public double expense_price { get; set; }
        public double expense_price_per_box { get; set; }
        public bool is_vat { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }
}
