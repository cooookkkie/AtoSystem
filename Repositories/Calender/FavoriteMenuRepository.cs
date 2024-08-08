using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Calender
{
    public class FavoriteMenuRepository : ClassRoot, IFavoriteMenuRepository
    {
        public List<FavoriteMenuModel> GetFavoriteMenu(string user_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                              ");
            qry.Append($"\n   *                                                               ");
            qry.Append($"\n  FROM t_favorite_menu                                             ");
            qry.Append($"\n  WHERE user_id = '{user_id}'                                      ");
            qry.Append($"\n  ORDER BY form_number                                             ");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetFavoriteMenu(dr);
        }
        private List<FavoriteMenuModel> SetFavoriteMenu(MySqlDataReader rd)
        {
            List<FavoriteMenuModel> list = new List<FavoriteMenuModel>();
            while (rd.Read())
            {
                FavoriteMenuModel model = new FavoriteMenuModel();
                model.user_id = rd["user_id"].ToString();
                model.form_number = Convert.ToInt16(rd["form_number"].ToString());
                model.category = rd["category"].ToString();
                model.form_name = rd["form_name"].ToString();
                model.updatetime = rd["updatetime"].ToString();

                list.Add(model); ;
            }
            rd.Close();
            return list;
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

        public StringBuilder InsertSql(FavoriteMenuModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT INTO t_favorite_menu (           ");
            qry.Append($"  user_id                               ");
            qry.Append($", form_number                           ");
            qry.Append($", category                              ");
            qry.Append($", form_name                             ");
            qry.Append($", updatetime                            ");
            qry.Append($") VALUES (                                ");
            qry.Append($"  '{model.user_id}'                       ");
            qry.Append($",  {model.form_number}                    ");
            qry.Append($", '{model.category}'                      ");
            qry.Append($", '{model.form_name}'                     ");
            qry.Append($", '{DateTime.Now.ToString("yyyy-MM-dd")}' ");
            qry.Append($")                                         ");

            return qry;
        }

        public StringBuilder DeleteSql(string user_id, string category, string form_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_favorite_menu                  ");
            qry.Append($"  WHERE user_id = '{user_id}'                ");
            qry.Append($"    AND category = '{category}'              ");
            qry.Append($"    AND form_name = '{form_name}'            ");


            return qry;
        }
    }
}
