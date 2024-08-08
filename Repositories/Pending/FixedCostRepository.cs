using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class FixedCostRepository : ClassRoot, IFixedCostRepository
    {
        public List<FixedCostModel> GetFixedCost(string contract_year, string ato_no, string contract_no, string shipper, string manager, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                         ");
            qry.Append($"\n f.*                                                                                           ");
            qry.Append($"\n FROM (                                                                                        ");
            qry.Append($"\n SELECT                                                                                        ");
            qry.Append($"\n  id                                                                                           ");
            qry.Append($"\n, sub_id                                                                                       ");
            qry.Append($"\n, contract_year                                                                                ");
            qry.Append($"\n, ato_no                                                                                       ");
            qry.Append($"\n, pi_date                                                                                      ");
            qry.Append($"\n, contract_no                                                                                  ");
            qry.Append($"\n, shipper                                                                                      ");
            qry.Append($"\n, lc_open_date                                                                                 ");
            qry.Append($"\n, lc_payment_date                                                                              ");
            qry.Append($"\n, bl_no                                                                                        ");
            qry.Append($"\n, shipment_date                                                                                ");
            qry.Append($"\n, etd                                                                                          ");
            qry.Append($"\n, eta                                                                                          ");
            qry.Append($"\n, warehousing_date                                                                             ");
            qry.Append($"\n, pending_check                                                                                ");
            qry.Append($"\n, cc_status                                                                                    ");
            qry.Append($"\n, origin                                                                                       ");
            qry.Append($"\n, product                                                                                      ");
            qry.Append($"\n, sizes                                                                                        ");
            qry.Append($"\n, box_weight                                                                                   ");
            qry.Append($"\n, unit_price                                                                                   ");
            qry.Append($"\n, quantity_on_paper                                                                            ");
            qry.Append($"\n, actual_quantity_warehoused                                                                   ");
            qry.Append($"\n, weight_on_paper                                                                              ");
            qry.Append($"\n, actual_shippment_weight                                                                      ");
            qry.Append($"\n, tariff_rate                                                                                  ");
            qry.Append($"\n, vat_rate                                                                                     ");
            qry.Append($"\n, payment_exchange                                                                             ");
            qry.Append($"\n, customs_clearance_exchange_rate                                                              ");
            qry.Append($"\n, cost_price_per_box1                                                                          ");
            qry.Append($"\n, invoice_amount_usd                                                                           ");
            qry.Append($"\n, invoice_amount_krw                                                                           ");
            qry.Append($"\n, bank_settlement_amount                                                                       ");
            qry.Append($"\n, taxable_value_box                                                                            ");
            qry.Append($"\n, additional_amount_box                                                                        ");
            qry.Append($"\n, vat_per_box                                                                                  ");
            qry.Append($"\n, amount_vat                                                                                   ");
            qry.Append($"\n, international_logistics_cost                                                                 ");
            qry.Append($"\n, loading_screening_costs                                                                      ");
            qry.Append($"\n, total_union_cost_box                                                                         ");
            qry.Append($"\n, total_union_cost                                                                             ");
            qry.Append($"\n, refrigeration_charge                                                                         ");
            qry.Append($"\n, lc_opening_charge                                                                            ");
            qry.Append($"\n, lc_telegraph_charge                                                                          ");
            qry.Append($"\n, lc_opening_conversion_fee                                                                    ");
            qry.Append($"\n, usance_underwriting_fee                                                                      ");
            qry.Append($"\n, import_collection_fee                                                                        ");
            qry.Append($"\n, usance_withdrawal_acceptance_fee                                                             ");
            qry.Append($"\n, bu_dicount_charge                                                                            ");
            qry.Append($"\n, other_expenses                                                                               ");
            qry.Append($"\n, agency_fee                                                                                   ");
            qry.Append($"\n, cost_price_per_box2                                                                          ");
            qry.Append($"\n, total_cost                                                                                   ");
            qry.Append($"\n, total_amount_by_size                                                                         ");
            qry.Append($"\n, total_amount_matched_seaover                                                                 ");
            qry.Append($"\n, note                                                                                         ");
            qry.Append($"\n, before_trq                                                                                   ");
            qry.Append($"\n, after_trq                                                                                    ");
            qry.Append($"\n, amount_difference                                                                            ");
            qry.Append($"\n, bank_cost_per_box                                                                            ");
            qry.Append($"\n, cost_per_weight                                                                              ");
            qry.Append($"\n, trq_price                                                                                    ");
            qry.Append($"\n, unapplied_taxable_value                                                                      ");
            qry.Append($"\n, unapplied_duty                                                                               ");
            qry.Append($"\n, tariff_per_box                                                                               ");
            qry.Append($"\n, unapplied_box_unit_price                                                                     ");
            qry.Append($"\n, trq_cost_per_weight                                                                          ");
            qry.Append($"\n, trq_cost                                                                                     ");
            qry.Append($"\n, trq_other_expenses                                                                           ");
            qry.Append($"\n, trq_final_cost_per_box                                                                       ");
            qry.Append($"\n, trq_total_amount_per_product                                                                 ");
            qry.Append($"\n, total_amount_seaover                                                                         ");
            qry.Append($"\n, manager                                                                                      ");
            qry.Append($"\n, edit_user                                                                                    ");
            qry.Append($"\n, updatetime                                                                                   ");
            qry.Append($"\nFROM t_fixed_cost                                                                              ");
            qry.Append($"\nWHERE 1=1                                                                                      ");
            qry.Append($"\n  AND sub_id <> 9999                                                                           ");
            if (!string.IsNullOrEmpty(contract_year))
            {
                qry.Append($"\n      AND contract_year = '{contract_year}'                                                                     ");
            }
            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n      AND ato_no LIKE '%{ato_no}%'                                                                     ");
            }
            if (!string.IsNullOrEmpty(contract_no))
            {
                qry.Append($"\n      AND contract_no LIKE '%{contract_no}%'                                                                     ");
            }
            if (!string.IsNullOrEmpty(shipper))
            {
                qry.Append($"\n      AND shipper LIKE '%{shipper}%'                                                                     ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n      AND manager LIKE '%{manager}%'                                                                     ");
            }
            qry.Append($"\nUNION ALL                                                                                      ");
            qry.Append($"\nSELECT                                                                                         ");
            qry.Append($"\n  f.*                                                                                          ");
            qry.Append($"\nFROM(                                                                                          ");
            qry.Append($"\n  SELECT                                                                                       ");
            qry.Append($"\n    f1.id                                                                                      ");
            qry.Append($"\n  , f1.sub_id                                                                                  ");
            qry.Append($"\n  , IF(f1.contract_year = 'SUM' , f2.contract_year, f1.contract_year) AS contract_year         ");
            qry.Append($"\n  , IF(f1.ato_no = 'SUM' , f2.ato_no, f1.ato_no) AS ato_no                                     ");
            qry.Append($"\n  , f1.pi_date                                                                                 ");
            qry.Append($"\n  , IF(f1.contract_no = 'SUM' , f2.contract_no, f1.contract_no) AS contract_no                 ");
            qry.Append($"\n  , IF(f1.shipper = 'SUM' , f2.shipper, f1.shipper) AS shipper                                 ");
            qry.Append($"\n  , f1.lc_open_date                                                                            ");
            qry.Append($"\n  , f1.lc_payment_date                                                                         ");
            qry.Append($"\n  , f1.bl_no                                                                                   ");
            qry.Append($"\n  , f1.shipment_date                                                                           ");
            qry.Append($"\n  , f1.etd                                                                                     ");
            qry.Append($"\n  , f1.eta                                                                                     ");
            qry.Append($"\n  , f1.warehousing_date                                                                        ");
            qry.Append($"\n  , f1.pending_check                                                                           ");
            qry.Append($"\n  , f1.cc_status                                                                               ");
            qry.Append($"\n  , f1.origin                                                                                  ");
            qry.Append($"\n  , f1.product                                                                                 ");
            qry.Append($"\n  , f1.sizes                                                                                   ");
            qry.Append($"\n  , f1.box_weight                                                                              ");
            qry.Append($"\n  , f1.unit_price                                                                              ");
            qry.Append($"\n  , f1.quantity_on_paper                                                                       ");
            qry.Append($"\n  , f1.actual_quantity_warehoused                                                              ");
            qry.Append($"\n  , f1.weight_on_paper                                                                         ");
            qry.Append($"\n  , f1.actual_shippment_weight                                                                 ");
            qry.Append($"\n  , f1.tariff_rate                                                                             ");
            qry.Append($"\n  , f1.vat_rate                                                                                ");
            qry.Append($"\n  , f1.payment_exchange                                                                        ");
            qry.Append($"\n  , f1.customs_clearance_exchange_rate                                                         ");
            qry.Append($"\n  , f1.cost_price_per_box1                                                                     ");
            qry.Append($"\n  , f1.invoice_amount_usd                                                                      ");
            qry.Append($"\n  , f1.invoice_amount_krw                                                                      ");
            qry.Append($"\n  , f1.bank_settlement_amount                                                                  ");
            qry.Append($"\n  , f1.taxable_value_box                                                                       ");
            qry.Append($"\n  , f1.additional_amount_box                                                                   ");
            qry.Append($"\n  , f1.vat_per_box                                                                             ");
            qry.Append($"\n  , f1.amount_vat                                                                              ");
            qry.Append($"\n  , f1.international_logistics_cost                                                            ");
            qry.Append($"\n  , f1.loading_screening_costs                                                                 ");
            qry.Append($"\n  , f1.total_union_cost_box                                                                    ");
            qry.Append($"\n  , f1.total_union_cost                                                                        ");
            qry.Append($"\n  , f1.refrigeration_charge                                                                    ");
            qry.Append($"\n  , f1.lc_opening_charge                                                                       ");
            qry.Append($"\n  , f1.lc_telegraph_charge                                                                     ");
            qry.Append($"\n  , f1.lc_opening_conversion_fee                                                               ");
            qry.Append($"\n  , f1.usance_underwriting_fee                                                                 ");
            qry.Append($"\n  , f1.import_collection_fee                                                                   ");
            qry.Append($"\n  , f1.usance_withdrawal_acceptance_fee                                                        ");
            qry.Append($"\n  , f1.bu_dicount_charge                                                                       ");
            qry.Append($"\n  , f1.other_expenses                                                                          ");
            qry.Append($"\n  , f1.agency_fee                                                                              ");
            qry.Append($"\n  , f1.cost_price_per_box2                                                                     ");
            qry.Append($"\n  , f1.total_cost                                                                              ");
            qry.Append($"\n  , f1.total_amount_by_size                                                                    ");
            qry.Append($"\n  , f1.total_amount_matched_seaover                                                            ");
            qry.Append($"\n  , f1.note                                                                                    ");
            qry.Append($"\n  , f1.before_trq                                                                              ");
            qry.Append($"\n  , f1.after_trq                                                                               ");
            qry.Append($"\n  , f1.amount_difference                                                                       ");
            qry.Append($"\n  , f1.bank_cost_per_box                                                                       ");
            qry.Append($"\n  , f1.cost_per_weight                                                                         ");
            qry.Append($"\n  , f1.trq_price                                                                               ");
            qry.Append($"\n  , f1.unapplied_taxable_value                                                                 ");
            qry.Append($"\n  , f1.unapplied_duty                                                                          ");
            qry.Append($"\n  , f1.tariff_per_box                                                                          ");
            qry.Append($"\n  , f1.unapplied_box_unit_price                                                                ");
            qry.Append($"\n  , f1.trq_cost_per_weight                                                                     ");
            qry.Append($"\n  , f1.trq_cost                                                                                ");
            qry.Append($"\n  , f1.trq_other_expenses                                                                      ");
            qry.Append($"\n  , f1.trq_final_cost_per_box                                                                  ");
            qry.Append($"\n  , f1.trq_total_amount_per_product                                                            ");
            qry.Append($"\n  , f1.total_amount_seaover                                                                    ");
            qry.Append($"\n  , IF(f1.manager = 'SUM' , f2.manager, f1.manager) AS manager                                 ");
            qry.Append($"\n  , f1.edit_user                                                                               ");
            qry.Append($"\n  , f1.updatetime                                                                              ");
            qry.Append($"\n  FROM(                                                                                        ");
            qry.Append($"\n      SELECT                                                                                   ");
            qry.Append($"\n        *                                                                                      ");
            qry.Append($"\n      FROM t_fixed_cost AS f1                                                                  ");
            qry.Append($"\n      WHERE 1=1                                                                                ");
            qry.Append($"\n      AND f1.sub_id = 9999                                                                     ");
            qry.Append($"\n  ) AS f1                                                                                      ");
            qry.Append($"\n  INNER JOIN t_fixed_cost AS f2                                                                ");
            qry.Append($"\n    ON f1.id = f2.id                                                                           ");
            qry.Append($"\n    AND f2.sub_id = 1                                                                          ");
            qry.Append($"\n) AS f                                                                                         ");
            qry.Append($"\nWHERE 1=1                                                                                      ");
            if (!string.IsNullOrEmpty(contract_year))
            {
                qry.Append($"\n      AND f.contract_year = '{contract_year}'                                                                     ");
            }
            if (!string.IsNullOrEmpty(ato_no))
            {
                qry.Append($"\n      AND f.ato_no LIKE '%{ato_no}%'                                                                     ");
            }
            if (!string.IsNullOrEmpty(contract_no))
            {
                qry.Append($"\n      AND f.contract_no LIKE '%{contract_no}%'                                                                     ");
            }
            if (!string.IsNullOrEmpty(shipper))
            {
                qry.Append($"\n      AND f.shipper LIKE '%{shipper}%'                                                                     ");
            }
            if (!string.IsNullOrEmpty(manager))
            {
                qry.Append($"\n      AND f.manager LIKE '%{manager}%'                                                                     ");
            }
            qry.Append($"\n) AS f                                                                                         ");
            qry.Append($"\nORDER BY f.id, f.sub_id                                                                        ");


            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetFixedCostModel(dr);
        }
        private List<FixedCostModel> GetFixedCostModel(MySqlDataReader rd)
        {
            List<FixedCostModel> CustomsModelList = new List<FixedCostModel>();
            while (rd.Read())
            {
                FixedCostModel model = new FixedCostModel();
                model.ID = Convert.ToInt32(rd["id"].ToString());
                model.sub_id = Convert.ToInt32(rd["sub_id"].ToString());
                model.contract_year = rd["contract_year"].ToString();
                model.ato_no = rd["ato_no"].ToString();
                model.pi_date = rd["pi_date"].ToString();
                model.contract_no = rd["contract_no"].ToString();
                model.shipper = rd["shipper"].ToString();
                model.lc_open_date = rd["lc_open_date"].ToString();
                model.lc_payment_date = rd["lc_payment_date"].ToString();
                model.bl_no = rd["bl_no"].ToString();
                model.shipment_date = rd["shipment_date"].ToString();
                model.etd = rd["etd"].ToString();
                model.eta = rd["eta"].ToString();
                model.warehousing_date = rd["warehousing_date"].ToString();
                model.pending_check = rd["pending_check"].ToString();
                model.cc_status = rd["cc_status"].ToString();
                model.origin = rd["origin"].ToString();
                model.product = rd["product"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.box_weight = rd["box_weight"].ToString();
                model.unit_price = rd["unit_price"].ToString();
                model.quantity_on_paper = rd["quantity_on_paper"].ToString();
                model.actual_quantity_warehoused = rd["actual_quantity_warehoused"].ToString();
                model.weight_on_paper = (Convert.ToDouble(rd["weight_on_paper"]).ToString("#,##0")).ToString();
                model.actual_shippment_weight = (Convert.ToDouble(rd["actual_shippment_weight"]).ToString("#,##0")).ToString();
                model.tariff_rate = rd["tariff_rate"].ToString();
                model.vat_rate = rd["vat_rate"].ToString();
                model.payment_exchange = rd["payment_exchange"].ToString();
                model.customs_clearance_exchange_rate = rd["customs_clearance_exchange_rate"].ToString();
                model.cost_price_per_box1 = (Convert.ToDouble(rd["cost_price_per_box1"]).ToString("#,##0")).ToString();
                model.invoice_amount_usd = (Convert.ToDouble(rd["invoice_amount_usd"]).ToString("#,##0.00")).ToString();
                model.invoice_amount_krw = (Convert.ToDouble(rd["invoice_amount_krw"]).ToString("#,##0")).ToString();
                model.bank_settlement_amount = rd["bank_settlement_amount"].ToString();
                model.taxable_value_box = rd["taxable_value_box"].ToString();
                model.additional_amount_box = rd["additional_amount_box"].ToString();
                model.vat_per_box = rd["vat_per_box"].ToString();
                model.amount_vat = rd["amount_vat"].ToString();
                model.international_logistics_cost = rd["international_logistics_cost"].ToString();
                model.loading_screening_costs= rd["loading_screening_costs"].ToString();
                model.total_union_cost_box = rd["total_union_cost_box"].ToString();
                model.total_union_cost = rd["total_union_cost"].ToString();
                model.refrigeration_charge = rd["refrigeration_charge"].ToString();
                model.lc_opening_charge = rd["lc_opening_charge"].ToString();
                model.lc_telegraph_charge = rd["lc_telegraph_charge"].ToString();
                model.lc_opening_conversion_fee = rd["lc_opening_conversion_fee"].ToString();
                model.usance_underwriting_fee = rd["usance_underwriting_fee"].ToString();
                model.import_collection_fee = rd["import_collection_fee"].ToString();
                model.usance_withdrawal_acceptance_fee = rd["usance_withdrawal_acceptance_fee"].ToString();
                model.bu_dicount_charge = rd["bu_dicount_charge"].ToString();
                model.other_expenses = rd["other_expenses"].ToString();
                model.agency_fee = rd["agency_fee"].ToString();
                model.cost_price_per_box2 = rd["cost_price_per_box2"].ToString();
                model.total_cost = rd["total_cost"].ToString();
                model.total_amount_by_size = rd["total_amount_by_size"].ToString();
                model.total_amount_matched_seaover = rd["total_amount_matched_seaover"].ToString();
                model.note = rd["note"].ToString();
                model.before_trq = rd["before_trq"].ToString();
                model.after_trq = rd["after_trq"].ToString();
                model.amount_difference = rd["amount_difference"].ToString();
                model.bank_cost_per_box = rd["bank_cost_per_box"].ToString();
                model.cost_per_weight = rd["cost_per_weight"].ToString();
                model.trq_price = rd["trq_price"].ToString();
                model.unapplied_taxable_value = rd["unapplied_taxable_value"].ToString();
                model.unapplied_duty = rd["unapplied_duty"].ToString();
                model.tariff_per_box = rd["tariff_per_box"].ToString();
                model.unapplied_box_unit_price = rd["unapplied_box_unit_price"].ToString();
                model.trq_cost_per_weight = rd["trq_cost_per_weight"].ToString();
                model.trq_cost = rd["trq_cost"].ToString();
                model.trq_other_expenses = rd["trq_other_expenses"].ToString();
                model.trq_final_cost_per_box = rd["trq_final_cost_per_box"].ToString();
                model.trq_total_amount_per_product = rd["trq_total_amount_per_product"].ToString();
                model.total_amount_seaover = rd["total_amount_seaover"].ToString();
                model.manager = rd["manager"].ToString();
                model.edit_user = rd["edit_user"].ToString();
                model.updatetime = rd["updatetime"].ToString();

                CustomsModelList.Add(model); ;
            }
            rd.Close();
            return CustomsModelList;
        }

    }
}
