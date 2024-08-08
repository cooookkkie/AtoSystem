using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class SeaoverProductModel
    {
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string seaover_unit { get; set; }
        public string unit { get; set; }
    }
        public class SeaoverPriceModel
    {
        public bool accent { get; set; }
        public string category_code { get; set; }
        public string category { get; set; }
        public string purchase_date { get; set; }
        public string purchase_co { get; set; }
        public string product_code { get; set; }
        public string product { get; set; }
        public string origin_code { get; set; }
        public string origin { get; set; }
        public string warehouse { get; set; }
        public string sizes_code { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public string note { get; set; }
        public string edit_date { get; set; }
        public double sales_price { get; set; }
        public double purchase_price { get; set; }
        public string manager1 { get; set; }
        public string manager2 { get; set; }
        public string division { get; set; }
        public string remark1 { get; set; }
        public string remark2 { get; set; }


        public string weight { get; set; }
        public string sizes1 { get; set; }
        public string sizes2 { get; set; }
        public string cost_unit { get; set; }
        public string tray_price { get; set; }
        public string remark { get; set; }
        public string is_tax { get; set; }
        

    }
    public class SeaoverCopyModel
    {
        public bool accent { get; set; }
        public string category1 { get; set; }
        public string category2 { get; set; }
        public string category3 { get; set; }
        public string category_code { get; set; }
        public string category { get; set; }
        public string product_code { get; set; }
        public string product { get; set; }
        public string origin_code { get; set; }
        public string origin { get; set; }
        public string weight { get; set; }
        public string sizes_code { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public string purchase_price { get; set; }
        public string sales_price { get; set; }
        public string division { get; set; }
        public string page { get; set; }
        public string area { get; set; }
        public string row { get; set; }
        public string cnt { get; set; }
        public string row_index { get; set; }

        public string edit_date { get; set; }
        public string manager1 { get; set; }
        public string is_tax { get; set; }
    }

}

