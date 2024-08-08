using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public class NotSendFaxRepository : ClassRoot, INotSendFaxRepository
    {
        public StringBuilder InsertCompany(NotSendFaxModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT INTO t_not_send_fax (          ");
            qry.Append($"\n   fax                              ");
            qry.Append($"\n , updatetime                       ");
            qry.Append($"\n , edit_user                        ");
            qry.Append($"\n ) VALUES (                         ");
            qry.Append($"\n   '{model.fax}'                    ");
            qry.Append($"\n , '{model.updatetime}'             ");
            qry.Append($"\n , '{model.edit_user}'              ");
            qry.Append($"\n )                                  ");

            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder DeleteCompany(string fax)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_not_send_fax            ");
            qry.Append($"\nWHERE fax = '{fax}'                 ");

            string sql = qry.ToString();
            return qry;
        }

        public DataTable GetNotSendFax(string fax)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                               ");
            qry.Append($"\n   *                                                                  ");
            qry.Append($"\n FROM t_not_send_fax                                                  ");
            qry.Append($"\n WHERE 1=1                                                            ");
            if (!string.IsNullOrEmpty(fax))
                qry.Append($"\n   AND fax LIKE '%{fax}%'                                         ");

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
