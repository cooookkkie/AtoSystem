using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SEAOVER
{
    public class ProductGroupRepository : ClassRoot, IProductGroupRepository
    {
        public StringBuilder DeleteProductGroup(ProductGroupModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_product_group                                ");
            qry.Append($"\n WHERE product  = '{model.product}'                ");
            qry.Append($"\n   AND origin  = '{model.origin}'                  ");
            qry.Append($"\n   AND sizes  = '{model.sizes}'                    ");
            qry.Append($"\n   AND unit  = '{model.unit}'                    ");
            qry.Append($"\n   AND price_unit  = '{model.price_unit}'                    ");
            qry.Append($"\n   AND unit_count  = '{model.unit_count}'                    ");
            qry.Append($"\n   AND seaover_unit  = '{model.seaover_unit}'                    ");
            return qry;
        }

        public DataTable GetSubProduct(string product = "", string origin = "", string sizes = "", string unit = "", string price_unit = "", string unit_count = "", string seaover_unit = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                            ");
            qry.Append($"\n  *                                                                               ");
            qry.Append($"\nFROM t_product_group                                                              ");
            qry.Append($"\nWHERE 1=1                                                                         ");
            qry.Append($"\n  AND product = '{product}'                                                       ");
            qry.Append($"\n  AND origin = '{origin}'                                                         ");
            qry.Append($"\n  AND sizes = '{sizes}'                                                           ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n  AND unit = '{unit}'                                                             ");
            if(!string.IsNullOrEmpty(price_unit))
                qry.Append($"\n  AND price_unit = '{price_unit}'                                                       ");
            if (!string.IsNullOrEmpty(unit_count))
                qry.Append($"\n  AND unit_count = '{unit_count}'                                                       ");
            if(!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n  AND seaover_unit = '{seaover_unit}'                                                   ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductGroup(string product = "", string origin = "", string sizes = "", string unit = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                            ");
            qry.Append($"\n  *                                                                               ");
            qry.Append($"\nFROM t_product_group                                                                       ");
            qry.Append($"\nWHERE 1=1                                                                         ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(whereSql("product", product.Trim()));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(whereSql("origin", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(whereSql("sizes", sizes.Trim()));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(whereSql("unit", unit.Trim()));
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        private string whereSql(string whrColumn, string whrValue)
        {
            string[] tempStr = null;
            string tempWhr = "";
            string whrStr = "";
            if (!string.IsNullOrEmpty(whrValue.Trim()))
            {
                tempStr = whrValue.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   {whrColumn} LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR {whrColumn} LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    whrStr = $"\n	 AND ( {tempWhr} )";
                }
                else
                {
                    whrStr = $"\n	  AND {whrColumn} LIKE '%{whrValue}%'";
                }
            }
            return whrStr;
        }

        public StringBuilder InsertProductGroup(ProductGroupModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nINSERT INTO t_product_group (              ");
            qry.Append($"\n  main_id              ");
            qry.Append($"\n, sub_id               ");
            qry.Append($"\n, product              ");
            qry.Append($"\n, origin               ");
            qry.Append($"\n, sizes                ");
            qry.Append($"\n, unit                 ");
            qry.Append($"\n, price_unit           ");
            qry.Append($"\n, unit_count          ");
            qry.Append($"\n, seaover_unit         ");
            qry.Append($"\n, item_product         ");
            qry.Append($"\n, item_origin          ");
            qry.Append($"\n, item_sizes           ");
            qry.Append($"\n, item_unit            ");
            qry.Append($"\n, item_price_unit      ");
            qry.Append($"\n, item_unit_count     ");
            qry.Append($"\n, item_seaover_unit    ");
            qry.Append($"\n, edit_user            ");
            qry.Append($"\n, updatetime           ");
            qry.Append($"\n) VALUES (                       ");
            qry.Append($"\n   {model.main_id          }     ");
            qry.Append($"\n,  {model.sub_id           }     ");
            qry.Append($"\n, '{model.product          }'    ");
            qry.Append($"\n, '{model.origin           }'    ");
            qry.Append($"\n, '{model.sizes            }'    ");
            qry.Append($"\n, '{model.unit             }'    ");
            qry.Append($"\n, '{model.price_unit       }'    ");
            qry.Append($"\n, '{model.unit_count      }'    ");
            qry.Append($"\n, '{model.seaover_unit     }'    ");
            qry.Append($"\n, '{model.item_product     }'    ");
            qry.Append($"\n, '{model.item_origin      }'    ");
            qry.Append($"\n, '{model.item_sizes       }'    ");
            qry.Append($"\n, '{model.item_unit        }'    ");
            qry.Append($"\n, '{model.item_price_unit  }'    ");
            qry.Append($"\n, '{model.item_unit_count }'    ");
            qry.Append($"\n, '{model.item_seaover_unit}'    ");
            qry.Append($"\n, '{model.edit_user        }'    ");
            qry.Append($"\n, '{model.updatetime       }'    ");
            qry.Append($"\n)                                ");

            return qry;
        }
        public int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
        {

            if (sqlList.Count > 0)
            {
                dbInstance.Connection.Close();
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
