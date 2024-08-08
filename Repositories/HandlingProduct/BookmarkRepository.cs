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
    public class BookmarkRepository : ClassRoot, IBookmarkRepository
    {
        public List<BookmarkModel> GetBookmark(string user_id, int form_type, string formname, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"  SELECT                                                                                                         ");
            qry.Append($"\n    b.form_id                                                                                                 ");
            qry.Append($"\n  , b.form_type                                                                                               ");
            qry.Append($"\n  , b.user_id                                                                                                 ");
            qry.Append($"\n  , f.form_name                                                                                               ");
            qry.Append($"\n  , f.create_user                                                                                               ");
            qry.Append($"\n  , IF(IFNULL(b.is_notification, 0) = 0, 'FALSE', 'TRUE') AS is_notification                                                  ");
            qry.Append($"\n  FROM t_bookmark AS b                                                                                        ");
            qry.Append($"\n  INNER JOIN (                                                                                                ");
            qry.Append($"\n      SELECT                                                                                                  ");
            qry.Append($"\n        id                                                                                                    ");
            qry.Append($"\n      , form_name                                                                                             ");
            qry.Append($"\n      , create_user                                                                                           ");
            qry.Append($"\n      FROM t_form_data                                                                                        ");
            qry.Append($"\n      GROUP BY id, form_name, edit_user                                                                       ");
            qry.Append($"\n  ) AS f                                                                                                      ");
            qry.Append($"\n    ON b.form_id = f.id                                                                                       ");
            qry.Append($"\n  WHERE 1=1                                                                                                   ");
            qry.Append($"\n  AND (b.user_id = '{user_id}' OR b.is_notification = TRUE)                                                       ");
            /*if(form_type == 3)
                qry.Append($"\n  AND b.form_type = {form_type}                                                                                 ");
            else
                qry.Append($"\n  AND b.form_type != 3                                                                                 ");*/
            if (!string.IsNullOrEmpty(formname))
                qry.Append($"\n  AND f.form_name LIKE '%{formname}%'                                                                       ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n  AND f.create_user LIKE '%{manager}%'                                                                      ");
            qry.Append($"\n  ORDER BY b.is_notification DESC, IFNULL(order_count, 99999) ASC                                                                     ");
            string sql = qry.ToString();

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetBookmark(dr);
        }

        private List<BookmarkModel> SetBookmark(MySqlDataReader rd)
        {
            List<BookmarkModel> list = new List<BookmarkModel>();
            while (rd.Read())
            {
                BookmarkModel model = new BookmarkModel();
                model.user_id = rd["user_id"].ToString();
                model.form_type = int.Parse(rd["form_type"].ToString());
                model.form_id = int.Parse(rd["form_id"].ToString());
                model.form_name = rd["form_name"].ToString();
                model.edit_user = rd["create_user"].ToString();
                model.is_notification = Convert.ToBoolean(rd["is_notification"].ToString());
                list.Add(model); ;
            }
            rd.Close();
            return list;
        }

        public StringBuilder UpdateOrderSql(int form_id, int order_count)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE t_bookmark SET                ");
            qry.Append($"  order_count = {order_count}        ");
            qry.Append($"WHERE is_notification = true         ");
            qry.Append($"  AND form_id = {form_id}            ");

            return qry;
        }

        public StringBuilder InsertSql(BookmarkModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT INTO t_bookmark (        ");
            qry.Append($"    user_id                         ");
            qry.Append($" ,  form_type                       ");
            qry.Append($" ,  form_id                         ");
            qry.Append($" ,  is_notification                         ");
            qry.Append($" ) VALUES (                         ");
            qry.Append($"   '{model.user_id}'                ");
            qry.Append($" ,  {model.form_type}               ");
            qry.Append($" ,  {model.form_id}                 ");
            qry.Append($" ,  {model.is_notification}                 ");
            qry.Append($" )                                  ");

            return qry;
        }
        public StringBuilder DeleteSql(BookmarkModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_bookmark              ");
            qry.Append($" WHERE user_id = '{model.user_id}'  ");
            if(model.form_type == 2 || model.form_type == 1)
                qry.Append($"   AND (form_type = 1 OR form_type = 2) ");
            qry.Append($"   AND form_id = {model.form_id}    ");

            return qry;
        }
        public StringBuilder SupperDeleteSql(int form_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_bookmark              ");
            qry.Append($" WHERE form_id = {form_id}    ");

            return qry;
        }

        public int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
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

        public DataTable GetFormList(int form_type, string group_name, string form_name, string create_user, bool is_favorite = false, string user_id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                               ");
            qry.Append($"\n   IF(b.form_type IS NULL, 'FALSE', 'TRUE') AS is_favorite                                              ");
            qry.Append($"\n , f.id                                                                                               ");
            qry.Append($"\n , f.group_name                                                                                       ");
            qry.Append($"\n , f.form_name                                                                                        ");
            qry.Append($"\n , f.form_type                                                                                        ");
            qry.Append($"\n , f.edit_user                                                                                        ");
            qry.Append($"\n , f.updatetime                                                                                       ");
            qry.Append($"\n , f.create_user                                                                                      ");
            qry.Append($"\n , IF(IFNULL(b.is_notification, 0) = 0, 'FALSE', 'TRUE') AS is_notification                           ");
            qry.Append($"\n FROM (                                                                                               ");
            qry.Append($"\n 	SELECT                                                                                           ");
            qry.Append($"\n       id                                                                                             ");
            qry.Append($"\n     , group_name                                                                                     ");
            qry.Append($"\n 	, form_name                                                                                      ");
            qry.Append($"\n 	, form_type                                                                                      ");
            qry.Append($"\n     , edit_user                                                                                      ");
            qry.Append($"\n     , DATE_FORMAT(updatetime, '%Y-%m-%d')  AS updatetime                                             ");
            qry.Append($"\n     , create_user                                                                                    ");
            qry.Append($"\n     FROM t_form_data                                                                                 ");
            qry.Append($"\n     WHERE 1=1                                                                                        ");
            if (form_type == 1)
                qry.Append($"\n       AND form_type != 3                                                                                       ");
            else if (form_type == 2)
                qry.Append($"\n       AND form_type = 3                                                                                       ");
            qry.Append($"\n     GROUP BY id, group_name, form_name, edit_user, DATE_FORMAT(updatetime, '%Y-%m-%d'), create_user                           ");
            qry.Append($"\n )AS f                                                                                                ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                    ");
            qry.Append($"\n 	SELECT                                                                                           ");
            qry.Append($"\n 	*                                                                                                ");
            qry.Append($"\n 	FROM t_bookmark                                                                                  ");
            qry.Append($"\n 	WHERE 1=1                                                                                        ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n 	  AND (user_id = '{user_id}' OR is_notification = TRUE)                                    ");
            qry.Append($"\n ) AS b                                                                                               ");
            qry.Append($"\n   ON f.id = form_id                                                                                  ");
            qry.Append($"\n WHERE 1=1                                                                                            ");
            if (!string.IsNullOrEmpty(group_name))
                qry.Append($"\n 	  AND f.group_name LIKE '%{group_name}%'                                                     ");
            if (!string.IsNullOrEmpty(form_name))
                qry.Append($"\n 	  AND f.form_name LIKE '%{form_name}%'                                                       ");
            if (!string.IsNullOrEmpty(create_user))
                qry.Append($"\n 	  AND f.create_user LIKE '%{create_user}%'                                                   ");
            if (is_favorite)
                qry.Append($"\n 	  AND IF(b.form_type IS NULL, 'FALSE', 'TRUE') = 'TRUE'                                      ");
            qry.Append($"\n ORDER BY is_notification DESC, is_favorite DESC, IF(b.user_id IS NULL, 'FALSE', 'TRUE') DESC, f.updatetime DESC, f.group_name, f.form_name   ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetFormData(int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                        ");
            qry.Append($"\n  id                                                                   ");
            qry.Append($"\n, sid                                                                  ");
            qry.Append($"\n, form_type                                                            ");
            qry.Append($"\n, form_name                                                            ");
            qry.Append($"\n, category                                                             ");
            qry.Append($"\n, category_code                                                        ");
            qry.Append($"\n, product_code                                                         ");
            qry.Append($"\n, product                                                              ");
            qry.Append($"\n, origin_code                                                          ");
            qry.Append($"\n, origin                                                               ");
            qry.Append($"\n, weight                                                               ");
            qry.Append($"\n, sizes_code                                                           ");
            qry.Append($"\n, sizes                                                                ");
            qry.Append($"\n, sales_price                                                          ");
            qry.Append($"\n, tray_price                                                           ");
            qry.Append($"\n, purchase_price                                                       ");
            qry.Append($"\n, unit                                                                 ");
            qry.Append($"\n, price_unit                                                           ");
            qry.Append($"\n, unit_count                                                           ");
            qry.Append($"\n, seaover_unit                                                         ");
            qry.Append($"\n, division                                                             ");
            qry.Append($"\n, page                                                                 ");
            qry.Append($"\n, cnt                                                                  ");
            qry.Append($"\n, row_index                                                            ");
            qry.Append($"\n, area                                                                 ");
            qry.Append($"\n, updatetime                                                           ");
            qry.Append($"\n, edit_user                                                            ");
            qry.Append($"\n, create_user                                                          ");
            qry.Append($"\n, accent                                                               ");
            qry.Append($"\n, form_remark                                                          ");
            qry.Append($"\n, remark                                                               ");
            qry.Append($"\n, cost_unit                                                            ");
            qry.Append($"\n, sizes2                                                               ");
            qry.Append($"\n, IF(IFNULL(is_rock, 0) = 0, 'FALSE', 'TRUE') AS is_rock               ");
            qry.Append($"\n, password                                                             ");
            qry.Append($"\n, is_tax                                                             ");
            qry.Append($"\n FROM t_form_data              ");
            qry.Append($"\n WHERE id = {id.ToString()}    ");
            qry.Append($"\n ORDER BY sid                  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;

        }
    }
}
