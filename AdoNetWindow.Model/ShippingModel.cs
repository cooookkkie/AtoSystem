using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ShippingModel
    {
        public int custom_id { get; set; }
        public double shipping_ex_rate { get; set; }
        public double payment_ex_rate { get; set; }
        public double customs_clearance_ex_rate { get; set; }
        public double amount_per_box { get; set; }
        public double cost_per_box { get; set; }
        public double lc_opening_charge { get; set; }
        public double lc_telegraph_charge { get; set; }
        public double lc_opening_conversion_fee { get; set; }
        public double usance_underwriting_fee { get; set; }
        public double import_collection_fee { get; set; }
        public double usance_acceptance_fee { get; set; }
        public double discount_charge { get; set; }
        public double banking_expenses { get; set; }
        public double agency_fee { get; set; }
        public double total_amount_seaover { get; set; }
        public double amount_matched_seaover { get; set; }
        public string remark { get; set; }
    }
}
