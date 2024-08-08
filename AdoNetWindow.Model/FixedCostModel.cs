using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class FixedCostModel
    {
        public int ID { get; set; }
        public int sub_id { get; set; }
        public string contract_year { get; set; }
        public string ato_no { get; set; }
        public string pi_date { get; set; }
        public string contract_no { get; set; }
        public string shipper { get; set; }
        public string lc_open_date { get; set; }
        public string lc_payment_date { get; set; }
        public string bl_no { get; set; }
        public string shipment_date { get; set; }
        public string etd { get; set; }
        public string eta { get; set; }
        public string warehousing_date { get; set; }
        public string pending_check { get; set; }
        public string cc_status { get; set; }
        public string origin { get; set; }
        public string product { get; set; }
        public string sizes { get; set; }
        public string box_weight { get; set; }
        public string unit_price { get; set; }
        public string quantity_on_paper { get; set; }
        public string actual_quantity_warehoused { get; set; }
        public string weight_on_paper { get; set; }
        public string actual_shippment_weight { get; set; }
        public string tariff_rate { get; set; }
        public string vat_rate { get; set; }
        public string payment_exchange { get; set; }
        public string customs_clearance_exchange_rate { get; set; }
        public string cost_price_per_box1 { get; set; }
        public string invoice_amount_usd { get; set; }
        public string invoice_amount_krw { get; set; }
        public string bank_settlement_amount { get; set; }
        public string taxable_value_box { get; set; }
        public string additional_amount_box { get; set; }
        public string vat_per_box { get; set; }
        public string amount_vat { get; set; }
        public string international_logistics_cost { get; set; }
        public string loading_screening_costs { get; set; }
        public string total_union_cost_box { get; set; }
        public string total_union_cost { get; set; }
        public string refrigeration_charge { get; set; }
        public string lc_opening_charge { get; set; }
        public string lc_telegraph_charge { get; set; }
        public string lc_opening_conversion_fee { get; set; }
        public string usance_underwriting_fee { get; set; }
        public string import_collection_fee { get; set; }
        public string usance_withdrawal_acceptance_fee { get; set; }
        public string bu_dicount_charge { get; set; }
        public string other_expenses { get; set; }
        public string agency_fee { get; set; }
        public string cost_price_per_box2 { get; set; }
        public string total_cost { get; set; }
        public string total_amount_by_size { get; set; }
        public string total_amount_matched_seaover { get; set; }
        public string note { get; set; }
        public string before_trq { get; set; }
        public string after_trq { get; set; }
        public string amount_difference { get; set; }
        public string bank_cost_per_box { get; set; }
        public string cost_per_weight { get; set; }
        public string trq_price { get; set; }
        public string unapplied_taxable_value { get; set; }
        public string unapplied_duty { get; set; }
        public string tariff_per_box { get; set; }
        public string unapplied_box_unit_price { get; set; }
        public string trq_cost_per_weight { get; set; }
        public string trq_cost { get; set; }
        public string trq_other_expenses { get; set; }
        public string trq_final_cost_per_box { get; set; }
        public string trq_total_amount_per_product { get; set; }
        public string total_amount_seaover { get; set; }
        public string manager { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }
}



