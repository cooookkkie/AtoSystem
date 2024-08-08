using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class FormDataTemp
    {
        public int temp_id { get; set; }
        public int id { get; set; }
        public string updatetime { get; set; }
    }
    public class FormDataTitle
    {
        public int id { get; set; }
        public int form_type { get; set; }
        public string form_name { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
    }
    public class FormDataModel
    {
        public int id { get; set; }
        public int sid { get; set; }
        public int form_type { get; set; }
        public string form_name { get; set; }
        public string category { get; set; }
        public string category_code { get; set; }
        public string product_code { get; set; }
        public string product { get; set; }
        public string origin_code { get; set; }
        public string origin { get; set; }
        public string sizes_code { get; set; }
        public string sizes { get; set; }
        public string sizes2 { get; set; }
        public string weight { get; set; }
        public double purchase_price { get; set; }
        public double sales_price { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public int page { get; set; }
        public int cnt { get; set; }
        public int row_index { get; set; }
        public string area { get; set; }
        public string division { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
        public string create_user { get; set; }
        public bool accent { get; set; }
        public string form_remark { get; set; }

        public string remark { get; set; }
        public string tray_price { get; set; }
        public string cost_unit { get; set; }
        public string is_tax { get; set; }

        public bool is_rock { get; set; }
        public string password { get; set; }
    }

    public class DgvColumnModel
    {
        public bool accent { get; set; }
        public string category1 { get; set; }
        public string category2 { get; set; }
        public string category3 { get; set; }
        public string category { get; set; }
        public string category_code { get; set; }
        public string product_code { get; set; }
        public string product { get; set; }
        public string origin_code { get; set; }
        public string origin { get; set; }
        public string weight { get; set; }
        public string sizes_code { get; set; }
        public string sizes { get; set; }
        public string purchase_price { get; set; }
        public string sales_price { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public string division { get; set; }
        public int page { get; set; }
        public int cnt { get; set; }
        public int row_index { get; set; }
        public int rows { get; set; }
        public string area { get; set; }
    }

    public class OneFormDataModel
    {
        public int id { get; set; }
        public int sid { get; set; }
        public int form_type { get; set; }
        public string form_name { get; set; }
        public string category_code { get; set; }
        public string category_code1 { get; set; }
        public string category_code2 { get; set; }
        public string category_code3 { get; set; }
        public string category { get; set; }
        public string product_code { get; set; }
        public string product { get; set; }
        public string origin_code { get; set; }
        public string origin { get; set; }
        public string sizes_code { get; set; }
        public string sizes1 { get; set; }
        public string sizes2 { get; set; }

        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public string weight { get; set; }
        public string cost_unit { get; set; }
        public double sales_price { get; set; }
        public string sales_price_txt { get; set; }
        public double tray_price { get; set; }
        public string tray_price_txt { get; set; }
        public string is_tax{ get; set; }
        public int row_index { get; set; }
        public int page { get; set; }
        public string division { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
        public string create_user { get; set; }
        public string remark { get; set; }
        public string edit_date { get; set; }
        public bool accent { get; set; }
        public string manager1 { get; set; }
    }
}
