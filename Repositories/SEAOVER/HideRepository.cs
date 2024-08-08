using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SEAOVER
{
    public class HideRepository : ClassRoot, IHideRepository
    {
        public DataTable GetHideTable(string enddate = "", string category = "", string product = "", string origin = "", string sizes = "", string unit = "", string division = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                            ");
            qry.Append($"\n  *                                                                               ");
            qry.Append($"\nFROM t_hide                                                                       ");
            qry.Append($"\nWHERE 1=1                                                                         ");
            if (!string.IsNullOrEmpty(division))
            {
                qry.Append($"\n  AND division = '{division}'                                                     ");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                qry.Append($"\n   AND until_date >= '{enddate}'                                                  ");
            }

            if (!string.IsNullOrEmpty(category))
            {
                qry.Append($"\n   AND category = '{category}'                                                  ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n   AND product = '{product}'                                                  ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n   AND origin = '{origin}'                                                  ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n   AND sizes = '{sizes}'                                                  ");
            }
            if (!string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n   AND seaover_unit = '{unit}'                                                  ");
            }

            qry.Append($"\n ORDER BY until_date DESC                                                         ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public List<HideModel> GetHide(string category, string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                    ");
            qry.Append($"\n    *                                                                                                                   ");
            qry.Append($"\n FROM  t_hide                                                                                                           ");
            qry.Append($"\n WHERE 1=1                                                                                                              ");
            if (!string.IsNullOrEmpty(category))
            {
                qry.Append($"\n   AND category = '{category}'                                                                                        ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n   AND product = '{product}'                                                                                        ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n   AND origin = '{origin}'                                                                                        ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n   AND sizes = '{sizes}'                                                                                        ");
            }
            if (!string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n   AND seaover_unit = '{unit}'                                                                                        ");
            }
            qry.Append($"\n ORDER BY until_date DESC                                                                                                ");


            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetHIde(dr);
        }

        private List<HideModel> SetHIde(MySqlDataReader rd)
        {
            List<HideModel> list = new List<HideModel>();
            while (rd.Read())
            {
                HideModel model = new HideModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.category = rd["category"].ToString();
                model.product = rd["product"].ToString();
                model.origin = rd["origin"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.seaover_unit = rd["seaover_unit"].ToString();
                model.edit_user = rd["edit_user"].ToString();
                model.updatetime = rd["updatetime"].ToString();
                model.until_date = rd["until_date"].ToString();
                model.hide_mode = rd["hide_mode"].ToString();
                model.hide_details = rd["hide_details"].ToString();
                model.remark = rd["remark"].ToString();

                list.Add(model);
            }
            rd.Close();
            return list;
        }

        public StringBuilder DeleteHide(int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_hide                                ");
            qry.Append($"\n WHERE id  = {id}                                  ");
            return qry;
        }

        public StringBuilder InsertHide(HideModel hm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nINSERT INTO t_hide (              ");
            qry.Append($"\n id                               ");
            qry.Append($"\n, division                        ");
            qry.Append($"\n, category                        ");
            qry.Append($"\n, product                         ");
            qry.Append($"\n, origin                          ");
            qry.Append($"\n, sizes                           ");
            qry.Append($"\n, seaover_unit                    ");
            qry.Append($"\n, edit_user                       ");
            qry.Append($"\n, updatetime                      ");
            qry.Append($"\n, until_date                      ");
            qry.Append($"\n, hide_mode                       ");
            qry.Append($"\n, hide_details                    ");
            qry.Append($"\n, remark                          ");
            qry.Append($"\n) VALUES (                        ");
            qry.Append($"\n    {hm.id}                       ");
            qry.Append($"\n , '{hm.division}'                ");
            qry.Append($"\n , '{hm.category}'                ");
            qry.Append($"\n , '{hm.product}'                 ");
            qry.Append($"\n , '{hm.origin}'                  ");
            qry.Append($"\n , '{hm.sizes}'                   ");
            qry.Append($"\n , '{hm.seaover_unit}'            ");
            qry.Append($"\n , '{hm.edit_user}'               ");
            qry.Append($"\n , '{hm.updatetime}'              ");
            qry.Append($"\n , '{hm.until_date}'              ");
            qry.Append($"\n , '{hm.hide_mode}'               ");
            qry.Append($"\n , '{hm.hide_details}'            ");
            qry.Append($"\n , '{hm.remark}'                  ");
            qry.Append($"\n)                                 ");

            return qry;
        }

        public StringBuilder UpdateHide(HideModel hm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_hide SET                    ");
            qry.Append($"\n  category = '{hm.category}'          ");
            qry.Append($"\n, until_date = '{hm.until_date}'      ");
            qry.Append($"\n, edit_user = '{hm.edit_user}'        ");
            qry.Append($"\n, updatetime = '{hm.updatetime}'      ");
            qry.Append($"\nWHERE id = {hm.id}                    ");
            
            string sql = qry.ToString();
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
