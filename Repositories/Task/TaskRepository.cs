using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Task
{
    public class TaskRepository : ClassRoot, ITaskRepository
    {
        public DataTable GetTask(string division, string group_name, string form_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                    ");
            qry.Append($"\n  id                                                                                                                    ");
            qry.Append($"\n , group_name                                                                                                           ");
            qry.Append($"\n , form_name                                                                                                            ");
            qry.Append($"\n , MAX(updatetime) AS updatetime                                                                                        ");
            qry.Append($"\n , MIN(createdatetime) AS createdatetime                                                                                ");
            qry.Append($"\n FROM  t_group                                                                                                          ");
            qry.Append($"\n WHERE 1=1                                                                                                              ");
            if (!string.IsNullOrEmpty(division))
            {
                qry.Append($"\n   AND group_division = '{division}'                                                                                        ");
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

        public DataTable GetTaskDetail(int id)
        {
            throw new NotImplementedException();
        }

        public StringBuilder DeleteTask(int id)
        {
            throw new NotImplementedException();
        }

        public StringBuilder DeleteTaskDetail(taskModel model)
        {
            throw new NotImplementedException();
        }
        public StringBuilder InsertGroup(taskModel model)
        {
            throw new NotImplementedException();
        }
    }
}
