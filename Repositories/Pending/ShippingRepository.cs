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
    public class ShippingRepository : ClassRoot, IShippingRepository
    {
        public ShippingModel GetShipping(string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	 SELECT                                                                                                 ");
            qry.Append($"\n   custom_id                                                          ");
            qry.Append($"\n , IFNULL(shipping_ex_rate          , 0) AS shipping_ex_rate          ");
            qry.Append($"\n , IFNULL(payment_ex_rate           , 0) AS payment_ex_rate           ");
            qry.Append($"\n , IFNULL(customs_clearance_ex_rate , 0) AS customs_clearance_ex_rate ");
            qry.Append($"\n , IFNULL(amount_per_box            , 0) AS amount_per_box            ");
            qry.Append($"\n , IFNULL(cost_per_box              , 0) AS cost_per_box              ");
            qry.Append($"\n , IFNULL(lc_opening_charge         , 0) AS lc_opening_charge         ");
            qry.Append($"\n , IFNULL(lc_telegraph_charge       , 0) AS lc_telegraph_charge       ");
            qry.Append($"\n , IFNULL(lc_opening_conversion_fee , 0) AS lc_opening_conversion_fee ");
            qry.Append($"\n , IFNULL(usance_underwriting_fee   , 0) AS usance_underwriting_fee   ");
            qry.Append($"\n , IFNULL(import_collection_fee     , 0) AS import_collection_fee     ");
            qry.Append($"\n , IFNULL(usance_acceptance_fee     , 0) AS usance_acceptance_fee     ");
            qry.Append($"\n , IFNULL(discount_charge           , 0) AS discount_charge           ");
            qry.Append($"\n , IFNULL(banking_expenses          , 0) AS banking_expenses          ");
            qry.Append($"\n , IFNULL(agency_fee                , 0) AS agency_fee                ");
            qry.Append($"\n , IFNULL(total_amount_seaover      , 0) AS total_amount_seaover      ");
            qry.Append($"\n , IFNULL(amount_matched_seaover    , 0) AS amount_matched_seaover    ");
            qry.Append($"\n , IFNULL(remark                    , '') AS remark                   ");
            qry.Append($"\n	 FROM t_shipping                                                                                       ");
            qry.Append($"\n	 WHERE 1=1                                                                                              ");
            if (!string.IsNullOrEmpty(id))
            {
                qry.Append($"\n	   AND custom_id = {id}                                                                                    ");
            }
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetShippingModel(dr);
        }
        private ShippingModel SetShippingModel(MySqlDataReader rd)
        {
            List<ShippingModel> list = new List<ShippingModel>();
            ShippingModel model = new ShippingModel();
            while (rd.Read())
            {
                model.custom_id = Convert.ToInt32(rd["custom_id"].ToString());
                model.shipping_ex_rate = Convert.ToDouble(rd["shipping_ex_rate"].ToString());
                model.payment_ex_rate = Convert.ToDouble(rd["payment_ex_rate"].ToString());
                model.customs_clearance_ex_rate = Convert.ToDouble(rd["customs_clearance_ex_rate"].ToString());
                model.amount_per_box = Convert.ToDouble(rd["amount_per_box"].ToString());
                model.cost_per_box = Convert.ToDouble(rd["cost_per_box"].ToString());
                model.lc_opening_charge = Convert.ToDouble(rd["lc_opening_charge"].ToString());
                model.lc_telegraph_charge = Convert.ToDouble(rd["lc_telegraph_charge"].ToString());
                model.lc_opening_conversion_fee = Convert.ToDouble(rd["lc_opening_conversion_fee"].ToString());
                model.usance_underwriting_fee = Convert.ToDouble(rd["usance_underwriting_fee"].ToString());
                model.import_collection_fee = Convert.ToDouble(rd["import_collection_fee"].ToString());
                model.usance_acceptance_fee = Convert.ToDouble(rd["usance_acceptance_fee"].ToString());
                model.discount_charge = Convert.ToDouble(rd["discount_charge"].ToString());
                model.banking_expenses = Convert.ToDouble(rd["banking_expenses"].ToString());
                model.agency_fee = Convert.ToDouble(rd["agency_fee"].ToString());
                model.total_amount_seaover = Convert.ToDouble(rd["total_amount_seaover"].ToString());
                model.amount_matched_seaover = Convert.ToDouble(rd["amount_matched_seaover"].ToString());
                model.remark = rd["remark"].ToString();
                list.Add(model);
            }
            rd.Close();
            return model;
        }

        public StringBuilder InsertShipping(ShippingModel model)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($"   INSERT INTO                                    ");
            qry.Append($"\n t_shipping                                     ");
            qry.Append($"\n  (                                             ");
            qry.Append($"\n   custom_id                                    ");
            qry.Append($"\n , shipping_ex_rate                             ");
            qry.Append($"\n , payment_ex_rate                              ");
            qry.Append($"\n , customs_clearance_ex_rate                    ");
            qry.Append($"\n , amount_per_box                               ");
            qry.Append($"\n , cost_per_box                                 ");
            qry.Append($"\n , lc_opening_charge                            ");
            qry.Append($"\n , lc_telegraph_charge                          ");
            qry.Append($"\n , lc_opening_conversion_fee                    ");
            qry.Append($"\n , usance_underwriting_fee                      ");
            qry.Append($"\n , import_collection_fee                        ");
            qry.Append($"\n , usance_acceptance_fee                        ");
            qry.Append($"\n , discount_charge                              ");
            qry.Append($"\n , banking_expenses                             ");
            qry.Append($"\n , agency_fee                                   ");
            qry.Append($"\n , total_amount_seaover                         ");
            qry.Append($"\n , amount_matched_seaover                       ");
            qry.Append($"\n , remark                                       ");
            qry.Append($"\n ) VALUES (                                     ");
            qry.Append($"\n    {model.custom_id                     }      ");
            qry.Append($"\n ,  {model.shipping_ex_rate}      ");
            qry.Append($"\n ,  {model.payment_ex_rate               }      ");
            qry.Append($"\n ,  {model.customs_clearance_ex_rate     }      ");
            qry.Append($"\n ,  {model.amount_per_box                }      ");
            qry.Append($"\n ,  {model.cost_per_box                  }      ");
            qry.Append($"\n ,  {model.lc_opening_charge             }      ");
            qry.Append($"\n ,  {model.lc_telegraph_charge           }      ");
            qry.Append($"\n ,  {model.lc_opening_conversion_fee     }      ");
            qry.Append($"\n ,  {model.usance_underwriting_fee       }      ");
            qry.Append($"\n ,  {model.import_collection_fee         }      ");
            qry.Append($"\n ,  {model.usance_acceptance_fee         }      ");
            qry.Append($"\n ,  {model.discount_charge               }      ");
            qry.Append($"\n ,  {model.banking_expenses              }      ");
            qry.Append($"\n ,  {model.agency_fee                    }      ");
            qry.Append($"\n ,  {model.total_amount_seaover          }      ");
            qry.Append($"\n ,  {model.amount_matched_seaover        }      ");
            qry.Append($"\n , '{model.remark                        }'     ");
            qry.Append($"\n )                                              ");

            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder UpdateShipping(ShippingModel model)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($"   UPDATE                                         ");
            qry.Append($"\n t_shipping SET                                 ");
            qry.Append($"\n   remark = '{model.remark}'                    ");
            qry.Append($"\n WHERE custom_id =  {model.custom_id}           ");

            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder DeleteShipping(int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_shipping           ");
            qry.Append($"\n  WHERE 1=1                     ");
            qry.Append($"\n    AND custom_id = {id}        ");

            string sql = qry.ToString();
            return qry;
        }
    }   
}
