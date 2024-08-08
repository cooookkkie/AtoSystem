using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class AllCustomsModel
    {
        public int id { get; set; }
        public int sub_id { get; set; }
        public int contract_year { get; set; }
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
        public string cost_unit { get; set; }
        public double unit_price { get; set; }
        public double quantity_on_paper { get; set; }
        public double qty { get; set; }
        public string manager { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
        public double tariff_rate { get; set; }
        public double vat_rate { get; set; }
        public double loading_cost_per_box { get; set; }
        public double refrigeration_charge { get; set; }
        public string total_amount_seaover { get; set; }
        public string remark { get; set; }
        public double trq_amount { get; set; }
        public double shipping_trq_amount { get; set; }
        public string payment_date { get; set; }
        public string payment_date_status { get; set; }
        public string payment_bank { get; set; }
        public string usance_type { get; set; }
        public string division { get; set; }
        public string agency_type{ get; set; }
        public string warehouse { get; set; }
        public double clearance_rate { get; set; }
        public string broker { get; set; }
        public string sanitary_certificate { get; set; }
        public double box_price_adjust { get; set; }
        public double shipping_box_price_adjust { get; set; }

        public double tax { get; set; }
        public double custom { get; set; }
        public double incidental_expense { get; set; }
        public bool weight_calculate { get; set; }

        public string loading_cost { get; set; }
        public string trq_tariff { get; set; }

        public bool is_calendar { get; set; }
        public bool is_shipment_qty { get; set; }
        public string import_number { get; set; }
        public bool is_quarantine { get; set; }
    }

    public class CustomsModel
    {
        public int id { get; set; }
        public int sub_d { get; set; }
        public int contract_year { get; set; }
        public string ato_no { get; set; }
        public string pi_date { get; set; }
        public string contract_no { get; set; }
        public string shipper { get; set; }
        public string lc_open_date { get; set; }
        public string lc_payment_date{ get; set; }
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
        public double unit_price { get; set; }
        public double quantity_on_paper { get; set; }
        public double qty { get; set; }
        public string usance_type { get; set; }
        public string division { get; set; }
        public string manager { get; set; }
        public string edit_user{ get; set; }
        public string updatetime { get; set; }
        public double tariff_rate { get; set; }
        public double vat_rate { get; set; }
        public double loading_cost_per_box { get; set; }
        public double refrigeration_charge { get; set; }
        public string total_amount_seaover { get; set; }
        public string remark { get; set; }
        public double trq_amount { get; set; }
        public string payment_date { get; set; }
        public string payment_date_status { get; set; }
        public string payment_bank { get; set; }
        public double total_amount { get; set; }
        public string currency{ get; set; }
        public string accuracy { get; set; }
        public string payment_type  { get; set; }
    }


    public class CustomsTitleModel
    {
        public int id{ get; set; }
        public int contract_year { get; set; }
        public string ato_no { get; set; }
        public string contract_no { get; set; }
        public string shipper { get; set; }
        public double total_amount { get; set; }
        public double total_weight { get; set; }
        public string cc_status { get; set; }
        public string usance_type { get; set; }
        public string agency_type { get; set; }
        public string division { get; set; }
        public string manager { get; set; }
        public string updatetime { get; set; }

    }

    public class CustomsSimpleModel
    {
        public string pi_date { get; set; }
        public string lc_open_date { get; set; }
        public string lc_payment_date { get; set; }
        public string bl_no { get; set; }
        public string shipment_date { get; set; }
        public string etd { get; set; }
        public string eta { get; set; }
        public string warehousing_date { get; set; }
        public string pending_check { get; set; }
        public string warehouse { get; set; }
        public string origin { get; set; }
        public string product { get; set; }
        public string sizes { get; set; }
        public string box_weight { get; set; }
        public double unit_price { get; set; }
        public double quantity_on_paper { get; set; }
        public double qty { get; set; }
        public double custom_weight { get; set; }
        public double tariff_rate { get; set; }
        public double vat_rate { get; set; }
        public string payment_date { get; set; }
        public string payment_date_status { get; set; }
        public string payment_bank { get; set; }
        public string remark { get; set; }
    }


    public class UncommfirmModel
    {
        public int id { get; set; }
        public int sub_d { get; set; }
        public int contract_year { get; set; }
        public string ato_no { get; set; }
        public string shipper { get; set; }
        public string shipment_date { get; set; }
        public string etd { get; set; }
        public string eta { get; set; }
        public string cc_status { get; set; }
        public string warehouse { get; set; }
        public string broker { get; set; }
        public string origin { get; set; }
        public string product { get; set; }
        public string sizes { get; set; }
        public bool weight_calculate { get; set; }
        public string box_weight { get; set; }
        public string cost_unit { get; set; }
        public double unit_price { get; set; }
        public double quantity_on_paper { get; set; }
        public double qty { get; set; }
        public double custom_weight { get; set; }
        public double clearance_rate { get; set; }
        public double loading_cost_per_box { get; set; }
        public double refrigeration_charge { get; set; }
        public double trq_amount { get; set; }
        public double shipping_trq_amount { get; set; }
        public string usance_type { get; set; }
        public string division { get; set; }
        public double tariff_rate { get; set; }
        public double vat_rate { get; set; }
        public string remark { get; set; }
        public string payment_date { get; set; }
        public string payment_date_status { get; set; }
        public string payment_bank { get; set; }
        public string manager { get; set; }
        public double box_price_adjust { get; set; }
        public double shipping_box_price_adjust { get; set; }
    }

    public class CustomsInfoModel
    {
        public int ID { get; set; }
        public int sub_id { get; set; }
        public int contract_year { get; set; }
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
        public string cc_status { get; set; }
        public string pending_check { get; set; }
        public string warehouse { get; set; }
        public string broker { get; set; }
        public string origin { get; set; }
        public string product { get; set; }
        public string sizes { get; set; }
        public string box_weight { get; set; }
        public string cost_unit { get; set; }
        public double unit_price { get; set; }
        public double quantity_on_paper { get; set; }
        public double qty { get; set; }
        public double tariff_rate { get; set; }
        public double vat_rate { get; set; }
        public double loading_cost_per_box { get; set; }
        public double refrigeration_charge { get; set; }
        public string total_amount_seaover { get; set; }
        public string remark { get; set; }
        public string import_number { get; set; }
        public double trq_amount { get; set; }
        public string payment_date { get; set; }
        public string payment_date_status { get; set; }
        public string payment_bank { get; set; }
        public string usance_type { get; set; }
        public string division { get; set; }
        public string agency_type { get; set; }
        public string sanitary_certificate { get; set; }
        public string manager { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
        public bool weight_calculate { get; set; }
    }

    public class ArriveModel
    {
        public int id { get; set; }
        public int sub_id { get; set; }
        public string eta{ get; set; }
        public string etd { get; set; }
        public string warehousing_date { get; set; }
        public int score { get; set; }
        public string pending_date { get; set; }
        public string eta_status { get; set; }
        public string origin { get; set; }
        public string product { get; set; }
        public string sizes { get; set; }
        public string box_weight { get; set; }
        public double quantity_on_paper { get; set; }
        public string manager { get; set; }
        public string cc_status { get; set; }

    }

    public class UnpendingProductModel
    {
        public string origin { get; set; }
        public string product { get; set; }
        public string sizes { get; set; }
        public string box_weight { get; set; }
        public double quantity_on_paper { get; set; }
        
    }
}


