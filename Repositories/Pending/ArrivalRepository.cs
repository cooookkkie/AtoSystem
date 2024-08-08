using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Pending
{
    public class ArrivalRepository : ClassRoot, IArrivalRepository
    {
        public StringBuilder DeleteSql(string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_arrival        ");
            qry.Append($"\n WHERE id = {id}              ");

            return qry;
        }

        public StringBuilder InsertSql(ArrivalModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_arrival (      ");
            qry.Append($"\n   id                         ");
            qry.Append($"\n , forwarder                  ");
            qry.Append($"\n , bl_status                  ");
            qry.Append($"\n , remark                     ");
            qry.Append($"\n , receipt_document           ");
            qry.Append($"\n , quarantine_type            ");
            qry.Append($"\n , result_estimated_date      ");
            qry.Append($"\n , edit_date                  ");
            qry.Append($"\n , edit_user                  ");
            qry.Append($"\n ) VALUES(                    ");
            qry.Append($"\n    {model.id       }         ");
            qry.Append($"\n , '{model.forwarder}'        ");
            qry.Append($"\n , '{model.bl_status}'        ");
            qry.Append($"\n , '{model.remark   }'        ");
            qry.Append($"\n , '{model.receipt_document}' ");
            qry.Append($"\n , '{model.quarantine_type}'  ");
            qry.Append($"\n , '{model.result_estimated_date}'        ");
            qry.Append($"\n , '{model.edit_date}'        ");
            qry.Append($"\n , '{model.edit_user}'        ");
            qry.Append($"\n )                            ");

            return qry;
        }


        public StringBuilder UpdateSql(string id, string whr, string val)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_arrival SET              ");
            qry.Append($"\n   {whr} = '{val}'                 ");
            qry.Append($"\n WHERE id = {id}                  ");

            return qry;
        }

        public int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
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
