using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Group
{
    public class GroupRepository : ClassRoot, IGroupRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public StringBuilder DeleteGroup(GroupModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_group                                ");
            qry.Append($"\n WHERE id = {model.id}                              ");
            qry.Append($"\n   AND product  = '{model.product}'                 ");
            qry.Append($"\n   AND origin  = '{model.origin}'                   ");
            qry.Append($"\n   AND sizes  = '{model.sizes}'                     ");
            qry.Append($"\n   AND unit  = '{model.unit}'                       ");
            qry.Append($"\n   AND price_unit  = '{model.price_unit}'           ");
            qry.Append($"\n   AND unit_count  = '{model.unit_count}'           ");
            qry.Append($"\n   AND seaover_unit  = '{model.seaover_unit}'       ");
            //qry.Append($"\n   AND division = '{model.division}'                ");

            string sql = qry.ToString();

            return qry;
        }

        public StringBuilder DeleteGroup2(GroupModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_group                                ");
            qry.Append($"\n WHERE id = {model.id}                              ");
            qry.Append($"\n   AND product  = '{model.product}'                 ");
            qry.Append($"\n   AND origin  = '{model.origin}'                   ");
            qry.Append($"\n   AND sizes  = '{model.sizes}'                     ");
            qry.Append($"\n   AND unit  = '{model.unit}'                       ");
            qry.Append($"\n   AND cost_unit  = '{model.cost_unit}'                       ");

            string sql = qry.ToString();

            return qry;
        }


        public StringBuilder DeleteGroup(int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_group                                ");
            qry.Append($"\n WHERE id  = {id}                                   ");
            return qry;
        }

        public StringBuilder UpdateGroup(int id, string group, string name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_group SET                       ");
            qry.Append($"\n   group_name = '{group}'                 ");
            qry.Append($"\n , form_name  = '{name}'                  ");
            qry.Append($"\n WHERE id = {id}                          ");
            return qry;
        }

        public DataTable GetGroupList(string division, string group_name, string form_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                    ");
            qry.Append($"\n  id                                                                                                                    ");
            qry.Append($"\n , group_division                                                                                                       ");
            qry.Append($"\n , group_name                                                                                                           ");
            qry.Append($"\n , form_name                                                                                                            ");
            qry.Append($"\n , MAX(updatetime) AS updatetime                                                                                        ");
            qry.Append($"\n , MIN(createdatetime) AS createdatetime                                                                                ");
            qry.Append($"\n FROM  t_group                                                                                                          ");
            qry.Append($"\n WHERE 1=1                                                                                                              ");
            if (!string.IsNullOrEmpty(division))
            {
                //qry.Append($"\n   AND group_division = '{division}'                                                                                        ");
                qry.Append($"\n   {commonRepository.whereSql("group_division", division)}                                                                                        ");
            }
            if (!string.IsNullOrEmpty(group_name))
            {
                qry.Append($"\n   AND group_name LIKE '%{group_name}%'                                                                                        ");
            }
            if (!string.IsNullOrEmpty(form_name))
            {
                qry.Append($"\n   AND form_name LIKE '%{form_name}%'                                                                                        ");
            }
            qry.Append($"\n GROUP BY id, group_name, form_name                                                                                     ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetGroup(int id, string category = "", string product = "", string origin = "", string sizes = "", string unit = "", string manager1 = "", string manager2 = "", string manager3 = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                         ");
            qry.Append($"\n   g.*                                                                          ");
            qry.Append($"\n  , IFNULL(tax, 0) AS tax                                                       ");
            qry.Append($"\n  , IFNULL(custom, 0) AS custom                                                 ");
            qry.Append($"\n  , IFNULL(incidental_expense, 0) AS incidental_expense                         ");
            qry.Append($"\n  , IF(IFNULL(weight_calculate, 1) = 1, 'TRUE', 'FALSE') AS weight_calculate    ");
            qry.Append($"\n  , IF(IFNULL(tray_calculate, 0) = 1, 'TRUE', 'FALSE') AS tray_calculate        ");
            qry.Append($"\n  , IFNULL(production_days, 20) AS production_days                               ");
            qry.Append($"\n  , IFNULL(purchase_margin, 7)  AS purchase_margin                              ");
            qry.Append($"\n  , IFNULL(p.manager, '')  AS manager                                           ");
            qry.Append($"\n  , CONCAT(g.product, '^', g.origin, '^', g.sizes, '^', g.unit, '^', g.price_unit, '^', g.unit_count, '^', seaover_unit) AS group_code");
            qry.Append($"\n  FROM  t_group AS g                                                            ");
            qry.Append($"\n  LEFT OUTER JOIN t_product_other_cost AS p                                     ");
            qry.Append($"\n    ON g.product = p.product                                                    ");
            qry.Append($"\n    AND g.origin = p.origin                                                     ");
            qry.Append($"\n    AND g.sizes = p.sizes                                                       ");
            qry.Append($"\n    AND (g.unit = p.unit OR g.seaover_unit = p.unit)                            ");
            qry.Append($"\n  WHERE 1=1                                                                     ");
            qry.Append($"\n   AND id = {id}                                                                                ");
            qry.Append($"\n  GROUP BY sub_id                                                               ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public StringBuilder InsertGroup(GroupModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nINSERT INTO t_group (           ");
            qry.Append($"\n id                             ");
            qry.Append($"\n, sub_id                        ");
            qry.Append($"\n, group_division                ");
            qry.Append($"\n, group_name                    ");
            qry.Append($"\n, form_name                     ");
            qry.Append($"\n, form_type                     ");
            qry.Append($"\n, form_remark                   ");
            qry.Append($"\n, category                      ");
            qry.Append($"\n, product                       ");
            qry.Append($"\n, product_code                  ");
            qry.Append($"\n, origin                        ");
            qry.Append($"\n, origin_code                   ");
            qry.Append($"\n, sizes                         ");
            qry.Append($"\n, sizes_code                    ");
            qry.Append($"\n, unit                          ");
            qry.Append($"\n, cost_unit                     ");
            qry.Append($"\n, month_around                  ");
            qry.Append($"\n, enable_days                   ");
            qry.Append($"\n, price_unit                    ");
            qry.Append($"\n, unit_count                    ");
            qry.Append($"\n, seaover_unit                  ");
            qry.Append($"\n, division                      ");
            qry.Append($"\n, row                           ");
            qry.Append($"\n, edit_user                     ");
            qry.Append($"\n, updatetime                    ");
            qry.Append($"\n, createdatetime                ");

            qry.Append($"\n, offer_price                   ");
            qry.Append($"\n, offer_cost_price              ");
            qry.Append($"\n, offer_company                 ");
            qry.Append($"\n, offer_updatetime              ");

            qry.Append($"\n) VALUES (                        ");
            qry.Append($"\n   {model.id          }                   ");
            qry.Append($"\n,  {model.sub_id      }                   ");
            qry.Append($"\n, '{model.group_division}'                ");
            qry.Append($"\n, '{model.group_name  }'                  ");
            qry.Append($"\n, '{model.form_name   }'                  ");
            qry.Append($"\n,  {model.form_type   }                   ");
            qry.Append($"\n, '{model.form_remark }'                  ");
            qry.Append($"\n, '{model.category    }'                  ");
            qry.Append($"\n, '{model.product     }'                  ");
            qry.Append($"\n, '{model.product_code}'                  ");
            qry.Append($"\n, '{model.origin      }'                  ");
            qry.Append($"\n, '{model.origin_code}'                   ");
            qry.Append($"\n, '{model.sizes       }'                  ");
            qry.Append($"\n, '{model.sizes_code}'                    ");
            qry.Append($"\n, '{model.unit        }'                  ");
            qry.Append($"\n, '{model.cost_unit   }'                  ");
            qry.Append($"\n,  {model.month_around}                   ");
            qry.Append($"\n,  {model.enable_days}                    ");
            qry.Append($"\n, '{model.price_unit  }'                  ");
            qry.Append($"\n, '{model.unit_count  }'                  ");
            qry.Append($"\n, '{model.seaover_unit}'                  ");
            qry.Append($"\n, '{model.division    }'                  ");
            qry.Append($"\n,  {model.row         }                   ");
            qry.Append($"\n, '{model.edit_user   }'                  ");
            qry.Append($"\n, '{model.updatetime  }'                  ");
            qry.Append($"\n, '{model.createdatetime}'                ");

            qry.Append($"\n,  {model.offer_price}                    ");
            qry.Append($"\n,  {model.offer_cost_price}               ");
            qry.Append($"\n, '{model.offer_company}'                 ");
            qry.Append($"\n, '{model.offer_updatetime}'              ");

            qry.Append($"\n)                                         ");

            string sql = qry.ToString();

            return qry;
        }
        public int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
        {
            if (sqlList.Count > 0)
            {

                MySqlConnection conn = (MySqlConnection)dbInstance.Connection;
                MySqlCommand command = conn.CreateCommand();
                transaction = conn.BeginTransaction();

                try
                {
                    int susccesCnt = 0;
                    for (int i = 0; i < sqlList.Count; i++)
                    {
                        string sql = sqlList[i].ToString();
                        command.CommandText = sqlList[i].ToString();
                        command.ExecuteNonQuery();
                        susccesCnt++;
                    }

                    if (sqlList.Count == susccesCnt)
                    {
                        transaction.Commit();
                        return susccesCnt;
                    }
                    else
                    {
                        transaction.Rollback();
                        return -1;
                    }

                }
                catch (Exception e)
                {
                    try
                    {
                        Console.WriteLine(e.Message);
                        transaction.Rollback();
                    }
                    catch (MySqlException myex)
                    {
                        if (transaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + myex.GetType() +
                                              " was encountered while attempting to roll back the transaction.");
                        }
                    }
                    Console.WriteLine(e.Message);
                    return -1;
                }
                finally
                {
                    conn.Close();
                }
            }
            else
            {
                Console.WriteLine("sql null");
                return -1;
            }
        }
    }
}
