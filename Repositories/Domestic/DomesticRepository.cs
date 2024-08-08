using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Domestic
{
    public class DomesticRepository : ClassRoot, IDomesticRepository
    {
        public StringBuilder DeleteDomestic(DomesticModel cm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_domestic WHERE id = {cm.id}");

            return qry;
        }


        public StringBuilder InsertDomestic(DomesticModel cm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_domestic ( ");
            qry.Append($"   id                     ");
            qry.Append($" , sub_id                 ");
            qry.Append($" , status                 ");
            qry.Append($" , etd                    ");
            qry.Append($" , warehousing_date       ");
            qry.Append($" , product                ");
            qry.Append($" , origin                 ");
            qry.Append($" , sizes                  ");
            qry.Append($" , unit                   ");
            qry.Append($" , price_unit             ");
            qry.Append($" , unit_count             ");
            qry.Append($" , seaover_unit           ");
            qry.Append($" , material_price         ");
            qry.Append($" , qty                    ");
            qry.Append($" , cost_price             ");
            qry.Append($" , weight                 ");
            qry.Append($" , work_weight            ");
            qry.Append($" , yiled                  ");
            qry.Append($" , remark                 ");
            qry.Append($" , work_fee               ");
            qry.Append($" , work_fee_per_box       ");
            qry.Append($" , transportation_fee     ");
            qry.Append($" , is_vat_transportation  ");
            qry.Append($" , bag_fee                ");
            qry.Append($" , sticker_fee            ");
            qry.Append($" , tape_fee               ");
            qry.Append($" , box_fee                ");
            qry.Append($" , is_vat_box             ");
            qry.Append($" , is_calculate           ");
            qry.Append($" , updatetime             ");
            qry.Append($" , edit_user              ");
            qry.Append($" ) VALUES (               ");
            qry.Append($"   {cm.id                   }  ");
            qry.Append($",  {cm.sub_id               }  ");
            qry.Append($", '{cm.status               }' ");
            qry.Append($", '{cm.etd     }'              ");
            qry.Append($", '{cm.warehousing_date     }' ");
            qry.Append($", '{cm.product              }' ");
            qry.Append($", '{cm.origin               }' ");
            qry.Append($", '{cm.sizes                }' ");
            qry.Append($", '{cm.unit                 }' ");
            qry.Append($", '{cm.price_unit           }' ");
            qry.Append($", '{cm.unit_count           }' ");
            qry.Append($", '{cm.seaover_unit         }' ");
            qry.Append($",  {cm.material_price       }  ");
            qry.Append($",  {cm.qty                  }  ");
            qry.Append($",  {cm.cost_price           }  ");
            qry.Append($", '{cm.weight               }' ");
            qry.Append($", '{cm.work_weight          }' ");
            qry.Append($",  {cm.yiled                }  ");
            qry.Append($", '{cm.remark               }' ");
            if (cm.work_fee > 0)
                qry.Append($",  {cm.work_fee             }  ");
            else
                qry.Append($",  NULL                        ");

            if (cm.work_fee_per_box > 0)
                qry.Append($",  {cm.work_fee_per_box     }  ");
            else
                qry.Append($",  NULL                        ");

            if (cm.transportation_fee > 0)
                qry.Append($",  {cm.transportation_fee   }  ");
            else
                qry.Append($",  NULL                        ");

            qry.Append($",  {cm.is_vat_transportation}  ");

            if (cm.bag_fee > 0)
                qry.Append($",  {cm.bag_fee              }  ");
            else
                qry.Append($",  NULL                        ");

            if (cm.sticker_fee > 0)
                qry.Append($",  {cm.sticker_fee          }  ");
            else
                qry.Append($",  NULL                        ");

            if (cm.tape_fee > 0)
                qry.Append($",  {cm.tape_fee             }  ");
            else
                qry.Append($",  NULL                        ");

            if (cm.box_fee > 0)
                qry.Append($",  {cm.box_fee              }  ");
            else
                qry.Append($",  NULL                        ");

            qry.Append($",  {cm.is_vat_box           }  ");
            qry.Append($",  {cm.is_calculate         }  ");
            qry.Append($", '{cm.updatetime           }' ");
            qry.Append($", '{cm.edit_user            }' ");
            qry.Append($")                         ");


            return qry;
        }

        public StringBuilder InsertDomesticExpense(DomesticExpenseModel cm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_domestic_expense (  ");
            qry.Append($"   domestic_id                     ");
            qry.Append($" , division                        ");
            qry.Append($" , expense_price                   ");
            qry.Append($" , expense_price_per_box           ");
            qry.Append($" , is_vat                          ");
            qry.Append($" , edit_user                       ");
            qry.Append($" , updatetime                      ");
            qry.Append($" ) VALUES (                        ");
            qry.Append($"   {cm.domestic_id}                ");
            qry.Append($", '{cm.division}'                  ");
            qry.Append($",  {cm.expense_price}              ");
            qry.Append($",  {cm.expense_price_per_box}      ");
            qry.Append($",  {cm.is_vat}                     ");
            qry.Append($", '{cm.edit_user}'                 ");
            qry.Append($", '{cm.updatetime}'                ");
            qry.Append($")                                  ");


            return qry;
        }

        public StringBuilder DeleteDomesticExpense(DomesticExpenseModel cm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_domestic_expense WHERE domestic_id = {cm.domestic_id}");

            return qry;
        }

        public DataTable GetDomestic(string domestic_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                   ");
            qry.Append($"\n id                                                                                       ");
            qry.Append($"\n , sub_id                                                                                 ");
            qry.Append($"\n , warehousing_date                                                                       ");
            qry.Append($"\n , product                                                                                ");
            qry.Append($"\n , origin                                                                                 ");
            qry.Append($"\n , sizes                                                                                  ");
            qry.Append($"\n , unit                                                                                   ");
            qry.Append($"\n , price_unit                                                                             ");
            qry.Append($"\n , unit_count                                                                             ");
            qry.Append($"\n , seaover_unit                                                                           ");
            qry.Append($"\n , material_price                                                                         ");
            qry.Append($"\n , qty                                                                                    ");
            qry.Append($"\n , cost_price                                                                             ");
            qry.Append($"\n , weight                                                                                 ");
            qry.Append($"\n , work_weight                                                                            ");
            qry.Append($"\n , yiled                                                                                  ");
            qry.Append($"\n , remark                                                                                 ");
            qry.Append($"\n , work_fee                                                                               ");
            qry.Append($"\n , work_fee_per_box                                                                       ");
            qry.Append($"\n , transportation_fee                                                                     ");
            qry.Append($"\n , IF(IFNULL(is_vat_transportation, 0) = 1, 'TRUE', 'FALSE') AS is_vat_transportation     ");
            qry.Append($"\n , bag_fee                                                                                ");
            qry.Append($"\n , sticker_fee                                                                            ");
            qry.Append($"\n , tape_fee                                                                               ");
            qry.Append($"\n , box_fee                                                                                ");
            qry.Append($"\n , IF(IFNULL(is_vat_box, 0) = 1, 'TRUE', 'FALSE') AS is_vat_box                           ");
            qry.Append($"\n , is_calculate                                                                           ");
            qry.Append($"\n , updatetime                                                                             ");
            qry.Append($"\n , edit_user                                                                              ");
            qry.Append($"\n FROM t_domestic                                                                          ");
            qry.Append($"\n WHERE 1=1                                                                                ");
            qry.Append($"\n   AND id = {domestic_id}                 ");

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetDomesticExpense(string domestic_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                            ");
            qry.Append($"\n domestic_id                                                  ");
            qry.Append($"\n , division                                                  ");
            qry.Append($"\n , expense_price                                                  ");
            qry.Append($"\n , expense_price_per_box ");
            qry.Append($"\n , IF(IFNULL(is_vat, 0) =1, 'TRUE', 'FALSE') AS is_vat                                  ");
            qry.Append($"\n , edit_user                                                  ");
            qry.Append($"\n , updatetime                                                  ");
            qry.Append($"\n FROM t_domestic_expense                           ");
            qry.Append($"\n WHERE 1=1                                         ");
            qry.Append($"\n   AND domestic_id = {domestic_id}                 ");

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
    }
}
