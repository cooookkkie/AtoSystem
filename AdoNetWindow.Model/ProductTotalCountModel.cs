using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ProductTotalCountModel
    {
        public int main_row_index { get; set; }
        public double total_seaover_unpending { get; set; }
        public double total_seaover_pending { get; set; }
        public double total_reserved_stock { get; set; }
        public double total_shipment_qty { get; set; }
        public double total_unpending_qty_before { get; set; }
        public double total_unpending_qty_after { get; set; }
        public double total_sales_count { get; set; }
        public double total_excluded_qty { get; set; }
        public double total_pending_cost{ get; set; }
    }
}
