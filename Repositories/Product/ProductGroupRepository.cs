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
        ICommonRepository commonRepository = new CommonRepository();
        public StringBuilder DeleteSubProduct(string main_id, string sub_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_product_group               ");
            qry.Append($"\n WHERE main_id  = {main_id}                ");
            qry.Append($"\n   AND sub_id  = {sub_id}                  ");
            return qry;
        }

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
            qry.Append($"\nSELECT                                       ");
            qry.Append($"\nt1.*                                         ");
            qry.Append($"\nFROM t_product_group AS t1                   ");
            qry.Append($"\nINNER JOIN(                                  ");
            qry.Append($"\n	SELECT                                      ");
            qry.Append($"\n	  *                                         ");
            qry.Append($"\n	FROM t_product_group                        ");
            qry.Append($"\n	WHERE 1=1                                   ");

            qry.Append($"\n  AND item_product = '{product}'                                                       ");
            qry.Append($"\n  AND item_origin = '{origin}'                                                         ");
            qry.Append($"\n  AND item_sizes = '{sizes}'                                                           ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n  AND item_unit = '{unit}'                                                             ");
            if (!string.IsNullOrEmpty(price_unit))
                qry.Append($"\n  AND item_price_unit = '{price_unit}'                                                       ");
            if (!string.IsNullOrEmpty(unit_count))
                qry.Append($"\n  AND item_unit_count = '{unit_count}'                                                       ");
            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n  AND item_seaover_unit = '{seaover_unit}'                                                   ");
            qry.Append($"\n) AS t2                                      ");
            qry.Append($"\n  ON t1.product = t2.product                 ");
            qry.Append($"\n  AND t1.origin = t2.origin                  ");
            qry.Append($"\n  AND t1.sizes = t2.sizes                    ");
            qry.Append($"\n  AND t1.unit = t2.unit                      ");
            qry.Append($"\n  AND t1.price_unit = t2.price_unit          ");
            qry.Append($"\n  AND t1.unit_count = t2.unit_count          ");
            qry.Append($"\n  AND t1.seaover_unit = t2.seaover_unit      ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductGroup(string product = "", string origin = "", string sizes = "", string unit = ""
            , string item_product = "", string item_origin = "", string item_sizes = "", string item_unit = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                            ");
            qry.Append($"\n  *                                                                               ");
            qry.Append($"\nFROM t_product_group                                                                       ");
            qry.Append($"\nWHERE 1=1                                                                         ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("product", product.Trim()));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("origin", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("sizes", sizes.Trim()));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("unit", unit.Trim()));

            if (!string.IsNullOrEmpty(item_product))
                qry.Append(commonRepository.whereSql("item_product", item_product.Trim()));
            if (!string.IsNullOrEmpty(item_origin))
                qry.Append(commonRepository.whereSql("item_origin", item_origin.Trim()));
            if (!string.IsNullOrEmpty(item_sizes))
                qry.Append(commonRepository.whereSql("item_sizes", item_sizes.Trim()));
            if (!string.IsNullOrEmpty(item_unit))
                qry.Append(commonRepository.whereSql("item_unit", item_unit.Trim()));

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductGroup2(string product = "", string origin = "", string sizes = "", string unit = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  	SELECT                                                                                                                                   ");
            qry.Append($"\n      CAST(main_id AS CHAR) AS main_id                                                                                                        ");
            qry.Append($"\n    , CAST(CASE WHEN product = item_product                                                                                                   ");
            qry.Append($"\n			AND origin = item_origin                                                                                                             ");
            qry.Append($"\n            AND sizes = item_sizes                                                                                                            ");
            qry.Append($"\n            AND unit = item_unit                                                                                                              ");
            qry.Append($"\n            AND price_unit = item_price_unit                                                                                                  ");
            qry.Append($"\n            AND unit_count = item_unit_count                                                                                                  ");
            qry.Append($"\n            AND seaover_unit = item_seaover_unit                                                                                              ");
            qry.Append($"\n		THEN 0 ELSE sub_id END AS CHAR) AS sub_id                                                                                                ");
            qry.Append($"\n    , item_product AS product                                                                                                                 ");
            qry.Append($"\n    , item_origin  AS origin                                                                                                                  ");
            qry.Append($"\n    , item_sizes  AS sizes                                                                                                                    ");
            qry.Append($"\n    , item_unit  AS unit                                                                                                                      ");
            qry.Append($"\n    , IFNULL(item_price_unit,'')  AS price_unit                                                                                                          ");
            qry.Append($"\n    , IFNULL(item_unit_count,'')  AS unit_count                                                                                                          ");
            qry.Append($"\n    , item_seaover_unit  AS seaover_unit                                                                                                           ");
            qry.Append($"\n    , concat(item_product, '^', item_origin, '^', item_sizes, '^', item_unit, '^', item_price_unit, '^', item_unit_count, '^', item_seaover_unit) AS group_code                  ");
            qry.Append($"\n 	FROM t_product_group                                                                                                                     ");
            qry.Append($"\n 	WHERE 1=1                                                                                                                     ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("product", product.Trim()));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("origin", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("sizes", sizes.Trim()));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("unit", unit.Trim()));

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductGroup3(string product = "", string origin = "", string sizes = "", string unit = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                                                                                            ");
            qry.Append($"\n t.*, g.merge_code                                                                                                                                                                ");
            qry.Append($"\nFROM(                                                                                                                                                             ");
            qry.Append($"\n	SELECT                                                                                                                                                           ");
            qry.Append($"\n	distinct                                                                                                                                                         ");
            qry.Append($"\n	  CAST(main_id AS CHAR) AS main_id                                                                                                                               ");
            qry.Append($"\n	, '9999' AS sub_id                                                                                                                                                  ");
            qry.Append($"\n	, product                                                                                                                                                        ");
            qry.Append($"\n	, origin                                                                                                                                                         ");
            qry.Append($"\n	, sizes                                                                                                                                                          ");
            qry.Append($"\n	, unit                                                                                                                                                           ");
            qry.Append($"\n	, price_unit                                                                                                                                                     ");
            qry.Append($"\n	, unit_count                                                                                                                                                     ");
            qry.Append($"\n	, seaover_unit                                                                                                                                                   ");
            qry.Append($"\n	, concat(product, '^', origin, '^', sizes, '^', unit, '^', price_unit, '^', unit_count, '^', seaover_unit) AS group_code                                         ");
            qry.Append($"\n	FROM t_product_group                                                                                                                                             ");
            qry.Append($"\n	WHERE 1=1                                                                                                                                             ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("product", product.Trim()));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("origin", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("sizes", sizes.Trim()));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("unit", unit.Trim()));
            qry.Append($"\n	UNION ALL                                                                                                                                                        ");
            qry.Append($"\n	SELECT                                                                                                                                                           ");
            qry.Append($"\n	  CAST(main_id AS CHAR) AS main_id                                                                                                                               ");
            qry.Append($"\n	, CAST(sub_id AS CHAR) AS sub_id                                                                                                                                 ");
            qry.Append($"\n	, item_product AS product                                                                                                                                        ");
            qry.Append($"\n	, item_origin  AS origin                                                                                                                                         ");
            qry.Append($"\n	, item_sizes  AS sizes                                                                                                                                           ");
            qry.Append($"\n	, item_unit  AS unit                                                                                                                                             ");
            qry.Append($"\n	, IFNULL(item_price_unit,'')  AS price_unit                                                                                                                      ");
            qry.Append($"\n	, IFNULL(item_unit_count,'')  AS unit_count                                                                                                                      ");
            qry.Append($"\n	, item_seaover_unit  AS seaover_unit                                                                                                                             ");
            qry.Append($"\n	, concat(item_product, '^', item_origin, '^', item_sizes, '^', item_unit, '^', item_price_unit, '^', item_unit_count, '^', item_seaover_unit) AS group_code      ");
            qry.Append($"\n	FROM t_product_group                                                                                                                                             ");
            qry.Append($"\n	WHERE 1=1                                                                                                                                                        ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("product", product.Trim()));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("origin", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("sizes", sizes.Trim()));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("unit", unit.Trim()));
            qry.Append($"\n) AS t                                                                                                                                                            ");

            qry.Append($"\nLEFT OUTER JOIN (                                                                                                                                                            ");
            qry.Append($"\n    SELECT                                                                                                                                                                                         ");
            qry.Append($"\n      CAST(main_id AS CHAR) AS main_id                                                                                                                                                             ");
            qry.Append($"\n    , GROUP_CONCAT( concat(item_product, '^', item_origin, '^', item_sizes, '^', item_unit, '^', item_price_unit, '^', item_unit_count, '^', item_seaover_unit) SEPARATOR '$') AS merge_code                                    ");
            qry.Append($"\n    FROM t_product_group                                                                                                                                                                           ");
            qry.Append($"\n    WHERE 1=1                                            ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("product", product.Trim()));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("origin", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("sizes", sizes.Trim()));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("unit", unit.Trim()));
            qry.Append($"\n    GROUP BY main_id                              ");
            qry.Append($"\n) AS g                                                                                                                                                           ");
            qry.Append($"\n  ON t.main_id = g.main_id                                                                                                                                       ");


            qry.Append($"\nORDER BY t.main_id, t.sub_id                                                                                                                                          ");

           
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
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
