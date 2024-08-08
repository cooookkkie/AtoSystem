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
    public class FormChangedDataRepository : ClassRoot, IFormChangedDataRepository
    {
        public StringBuilder DeleteChangeData(string whr)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nDELETE FROM t_form_data_changed WHERE {whr}");
            return qry;
        }
        public int GetNextId()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT IF( MAX(id) IS NULL, 1, MAX(id) +1) AS id FROM t_form_data_changed");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        public DataTable GetChangeCurrentData()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                       ");
            qry.Append($"\n   id                                                                                         ");
            qry.Append($"\n , column_name                                                                                ");
            qry.Append($"\n , column_code                                                                                ");
            qry.Append($"\n , origin_text                                                                                ");
            qry.Append($"\n , changed_text                                                                               ");
            qry.Append($"\n FROM t_form_data_changed AS a                                                                ");
            qry.Append($"\n WHERE 1=1                                                                                    ");
            qry.Append($"\n 	AND id = ( SELECT                                                                        ");
            qry.Append($"\n 				MAX(id)                                                                      ");
            qry.Append($"\n 				FROM t_form_data_changed AS b                                                ");
            qry.Append($"\n                 WHERE a.column_code = b.column_code AND a.column_name = b.column_name )      ");
            qry.Append($"\n ORDER BY id DESC                                                                             ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            var table = new DataTable();
            table.Load(dr);

            return table;
        }

        public DataTable GetChangeData()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                                                         ");
            qry.Append($"\n   *                                                                                                          ");
            qry.Append($"\n  FROM t_form_data_changed                                                                                    ");
            qry.Append($"\n  WHERE 1=1                                                                                                   ");
            qry.Append($"\n  ORDER BY id                                                                                                  ");

            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            var table = new DataTable();
            table.Load(dr);
            
            return table;
        }
        private List<FormChangedDataModel> SetChangedData(MySqlDataReader rd)
        {
            List<FormChangedDataModel> list = new List<FormChangedDataModel>();
            while (rd.Read())
            {
                FormChangedDataModel model = new FormChangedDataModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.column_name = rd["column_name"].ToString();
                model.column_code = rd["column_code"].ToString();
                model.origin_text = rd["origin_text"].ToString();
                model.changed_text = rd["changed_text"].ToString();

                list.Add(model); ;
            }
            rd.Close();
            return list;
        }

        public int UpdateCustomTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
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

        public StringBuilder InsertSql(FormChangedDataModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT INTO t_form_data_changed (         ");
            qry.Append($"  id                                      ");
            qry.Append($", column_name                             ");
            qry.Append($", column_code                             ");
            qry.Append($", origin_text                             ");
            qry.Append($", changed_text                            ");
            qry.Append($") VALUES (                                ");
            qry.Append($"   {model.id}                             ");
            qry.Append($", '{model.column_name}'                   ");
            qry.Append($", '{model.column_code}'                   ");
            qry.Append($", '{model.origin_text}'                   ");
            qry.Append($", '{model.changed_text}'                  ");
            qry.Append($")                                         ");

            return qry;
        }

        public StringBuilder DeleteSql(FormChangedDataModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_form_data_changed                  ");
            qry.Append($"  WHERE column_name = '{model.column_name}'      ");
            qry.Append($"    AND column_code = '{model.column_code}'      ");

            return qry;
        }
    }
}
