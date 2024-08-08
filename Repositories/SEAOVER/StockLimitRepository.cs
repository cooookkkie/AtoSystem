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
    public class StockLimitRepository : ClassRoot, IStockLimitRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public StringBuilder DeleteProduct(ProductLimitModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_product_limit_amount                ");
            qry.Append($"\n WHERE product  = '{model.product}'                ");
            qry.Append($"\n   AND origin  = '{model.origin}'                  ");
            qry.Append($"\n   AND sizes  = '{model.sizes}'                    ");
            qry.Append($"\n   AND unit  = '{model.unit}'                      ");
            qry.Append($"\n   AND weight  = '{model.weight}'                  ");
            return qry;
        }

        public DataTable GetProductLimit(string product = "", string origin = "", string sizes = "", string unit = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                            ");
            qry.Append($"\n  *                                                                               ");
            qry.Append($"\nFROM t_product_limit_amount                                                                       ");
            qry.Append($"\nWHERE 1=1                                                                         ");
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append(commonRepository .whereSql("product", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append(commonRepository.whereSql("origin", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append(commonRepository.whereSql("sizes", sizes.Trim()));
            }
            if (!string.IsNullOrEmpty(unit))
            {
                qry.Append(commonRepository.whereSql("unit", unit.Trim()));
            }
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public StringBuilder InsertProduct(ProductLimitModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nINSERT INTO t_product_limit_amount (              ");
            qry.Append($"\n  product              ");
            qry.Append($"\n, origin               ");
            qry.Append($"\n, sizes                ");
            qry.Append($"\n, unit                 ");
            qry.Append($"\n, weight               ");
            qry.Append($"\n, limit_qty            ");
            qry.Append($"\n, limit_amount         ");
            qry.Append($"\n, updatetime           ");
            qry.Append($"\n, edit_user            ");
            qry.Append($"\n) VALUES (                       ");
            qry.Append($"\n  '{model.product     }'         ");
            qry.Append($"\n, '{model.origin      }'         ");
            qry.Append($"\n, '{model.sizes       }'         ");
            qry.Append($"\n, '{model.unit        }'         ");
            qry.Append($"\n, '{model.weight      }'         ");
            qry.Append($"\n, {model.limit_qty   }         ");
            qry.Append($"\n, {model.limit_amount}         ");
            qry.Append($"\n, '{model.updatetime  }'         ");
            qry.Append($"\n, '{model.edit_user   }'         ");
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
