using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class PriceComparisonModel
    {
        public string category_code { get; set; }
        public string category { get; set; }
        public string category2 { get; set; }
        public string category3 { get; set; }
        public string product_code { get; set; }
        public string product { get; set; }
        public string origin_code { get; set; }
        public string origin { get; set; }
        public string sizes_code { get; set; }
        public string sizes { get; set; }
        public string sizes2 { get; set; }
        public string sizes3 { get; set; }
        public string sizes4 { get; set; }
        public string unit { get; set; }
        public string unit_count { get; set; }
        public string price_unit { get; set; }
        public string seaover_unit { get; set; }
        public double total_stock { get; set; }
        public double reserved_stock { get; set; }
        public string reserved_stock_detail { get; set; }
        public int sales_count { get; set; }
        public double purchase_price { get; set; }
        public double sales_price { get; set; }
        public double average_price { get; set; }
        public string average_price_details { get; set; }
        public string manager1 { get; set; }
        public string manager2 { get; set; }
        public string division { get; set; }
        public double sales_cost_price { get; set; }
    }
    public class ShortManagementModel
    {
        public string category_code { get; set; }
        public string category { get; set; }
        public string category2 { get; set; }
        public string category3 { get; set; }
        public string product_code { get; set; }
        public string product { get; set; }
        public string origin_code { get; set; }
        public string origin { get; set; }
        public string sizes_code { get; set; }
        public string sizes { get; set; }
        public string sizes2 { get; set; }
        public string sizes3 { get; set; }
        public string sizes4 { get; set; }
        public string unit { get; set; }
        public string unit_count { get; set; }
        public string price_unit { get; set; }
        public string seaover_unit { get; set; }
        public double total_stock { get; set; }
        public double reserved_stock { get; set; }
        public string reserved_stock_detail { get; set; }
        public double real_stock { get; set; }
        public int sales_count { get; set; }
        public double purchase_price { get; set; }
        public double sales_price { get; set; }
        public double average_price { get; set; }
        public double enable_sales_days { get; set; }
        public double averge_month_sales_count { get; set; }
        public double averge_day_sales_count { get; set; }
        public string manager1 { get; set; }
        public string manager2 { get; set; }
        public string division { get; set; }
        public bool price { get; set; }
        public bool exhaust { get; set; }
        public DateTime enddate { get; set; }
    }

    public class ShortManagerModel
    {
        public string category { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public double real_stock { get; set; }
        public double avg_sales_day { get; set; }
        public double avg_sales_month { get; set; }
        public string exhaust_date { get; set; }
        public string enddate { get; set; }
        public double offer_price { get; set; }
        public double cost_price { get; set; }
    }

    public class shipmentModel
    {
        public string etd { get; set; }
        public string eta { get; set; }
        public string in_date { get; set; }
        public string qty { get; set; }
    }

    public class PrieComparisonTempModel
    {
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string price_unit { get; set; }
        public string unit_count { get; set; }
        public string seaover_unit { get; set; }
        public string division { get; set; }
        public double purchase_price { get; set; }
        public double sales_price { get; set; }
        public double sales_count { get; set; }
        public double works_day { get; set; }
        public double seaover_unpending_stock { get; set; }
        public double seaover_pending_stock { get; set; }
        public double shipment_stock { get; set; }
        public double unpending_before_stock { get; set; }
        public double unpending_after_stock { get; set; }
        public double updatetime { get; set; }
    }
}

