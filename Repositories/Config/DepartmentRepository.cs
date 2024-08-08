using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Config
{
    public class DepartmentRepository : ClassRoot, IDepartmentRepository
    {
        public DataTable GetDepartment(string name, string auth)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                    ");
            qry.Append($"\n  *                                                                                                                     ");
            qry.Append($"\n FROM  t_department                                                                                                     ");
            qry.Append($"\n WHERE 1=1                                                                                                              ");
            if (!string.IsNullOrEmpty(name))
            {
                qry.Append($"\n   AND name LIKE '%{name}%'                                                                                        ");
            }
            if (!string.IsNullOrEmpty(auth))
            {
                qry.Append($"\n   AND auth_level = {auth}                                                                                               ");
            }
            qry.Append($"\n GROUP BY name, auth_level                                                                                             ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetDepartment(string name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                    ");
            qry.Append($"\n  *                                                                                                                     ");
            qry.Append($"\n FROM  t_department                                                                                                     ");
            qry.Append($"\n WHERE 1=1                                                                                                              ");
            if (!string.IsNullOrEmpty(name))
            {
                qry.Append($"\n   AND name = '{name}'                                                                                        ");
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

        public StringBuilder InsertDepartment(DepartmentModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nINSERT INTO t_department (      ");
            qry.Append($"\n id                             ");
            qry.Append($"\n, name                          ");
            qry.Append($"\n, auth_level                    ");
            qry.Append($"\n) VALUES (                      ");
            qry.Append($"\n   {model.id}                   ");
            qry.Append($"\n, '{model.name}'                ");
            qry.Append($"\n,  {model.auth_level}           ");
            qry.Append($"\n)                               ");

            string sql = qry.ToString();

            return qry;
        }

        public StringBuilder UpdateDepartment(DepartmentModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_department SET                 ");
            qry.Append($"\n  name  = '{model.name}'                 ");
            qry.Append($"\n , auth_level = {model.auth_level}       ");
            qry.Append($"\n WHERE id = {model.id}                   ");

            string sql = qry.ToString();

            return qry;
        }

        public StringBuilder DeleteDepartment(DepartmentModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_department                ");
            qry.Append($"\n WHERE id = {model.id}                   ");

            string sql = qry.ToString();

            return qry;
        }
    }
}
