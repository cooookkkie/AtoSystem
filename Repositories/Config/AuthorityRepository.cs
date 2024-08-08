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
    public class AuthorityRepository : ClassRoot, IAuthorityRepository
    {
        public StringBuilder DeleteAuthority(string user_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_Authority                        ");
            qry.Append($"\n WHERE user_id = '{user_id}'                    ");

            string sql = qry.ToString();

            return qry;
        }

        public DataTable GetAuthorityInfo(string department, string user_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                         ");
            qry.Append($"\n   department                                                                 ");
            qry.Append($"\n , user_id                                                                    ");
            qry.Append($"\n , user_name                                                                  ");
            qry.Append($"\n , group_name                                                                 ");
            qry.Append($"\n , form_name                                                                  ");
            qry.Append($"\n , IF(IFNULL(is_visible, 0) = 1, 'TRUE', 'FALSE') AS is_visible               ");
            qry.Append($"\n , IF(IFNULL(is_add, 0) = 1, 'TRUE', 'FALSE') AS is_add                       ");
            qry.Append($"\n , IF(IFNULL(is_update, 0) = 1, 'TRUE', 'FALSE') AS is_update                 ");
            qry.Append($"\n , IF(IFNULL(is_delete, 0) = 1, 'TRUE', 'FALSE') AS is_delete                 ");
            qry.Append($"\n , IF(IFNULL(is_excel, 0) = 1, 'TRUE', 'FALSE') AS is_excel                   ");
            qry.Append($"\n , IF(IFNULL(is_print, 0) = 1, 'TRUE', 'FALSE') AS is_print                   ");
            qry.Append($"\n , IF(IFNULL(is_admin, 0) = 1, 'TRUE', 'FALSE') AS is_admin                   ");
            qry.Append($"\n FROM t_Authority                                                               ");
            qry.Append($"\n WHERE 1=1                                                                      ");
            if(!string.IsNullOrEmpty(department))
                qry.Append($"\n   AND department = '{department}'                                            ");
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"\n   AND user_name = '{user_name}'                                                  ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetUserAuthority(string user_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                         ");
            qry.Append($"\n   u.department                                                                 ");
            qry.Append($"\n , u.user_id                                                                    ");
            qry.Append($"\n , u.user_name                                                                  ");
            qry.Append($"\n , a.group_name                                                                 ");
            qry.Append($"\n , a.form_name                                                                  ");
            qry.Append($"\n , IF(IFNULL(a.is_visible, 0) = 1, 'TRUE', 'FALSE') AS is_visible               ");
            qry.Append($"\n , IF(IFNULL(a.is_add, 0) = 1, 'TRUE', 'FALSE') AS is_add                       ");
            qry.Append($"\n , IF(IFNULL(a.is_update, 0) = 1, 'TRUE', 'FALSE') AS is_update                 ");
            qry.Append($"\n , IF(IFNULL(a.is_delete, 0) = 1, 'TRUE', 'FALSE') AS is_delete                 ");
            qry.Append($"\n , IF(IFNULL(a.is_excel, 0) = 1, 'TRUE', 'FALSE') AS is_excel                   ");
            qry.Append($"\n , IF(IFNULL(a.is_print, 0) = 1, 'TRUE', 'FALSE') AS is_print                   ");
            qry.Append($"\n , IF(IFNULL(a.is_admin, 0) = 1, 'TRUE', 'FALSE') AS is_admin                   ");
            qry.Append($"\n , 1 AS id                                                                      ");
            qry.Append($"\n FROM users AS u                                                                ");
            qry.Append($"\n INNER JOIN t_Authority AS a                                                    ");
            qry.Append($"\n   ON u.user_id = a.user_id                                                     ");
            qry.Append($"\n WHERE u.user_id = '{user_id}'                                                       ");
            qry.Append($"\n UNION ALL                                                                      ");
            qry.Append($"\n SELECT                                                                         ");
            qry.Append($"\n   u.department                                                                 ");
            qry.Append($"\n , u.user_id                                                                    ");
            qry.Append($"\n , u.user_name                                                                  ");
            qry.Append($"\n , a.group_name                                                                 ");
            qry.Append($"\n , a.form_name                                                                  ");
            qry.Append($"\n , IF(IFNULL(a.is_visible, 0) = 1, 'TRUE', 'FALSE') AS is_visible               ");
            qry.Append($"\n , IF(IFNULL(a.is_add, 0) = 1, 'TRUE', 'FALSE') AS is_add                       ");
            qry.Append($"\n , IF(IFNULL(a.is_update, 0) = 1, 'TRUE', 'FALSE') AS is_update                 ");
            qry.Append($"\n , IF(IFNULL(a.is_delete, 0) = 1, 'TRUE', 'FALSE') AS is_delete                 ");
            qry.Append($"\n , IF(IFNULL(a.is_excel, 0) = 1, 'TRUE', 'FALSE') AS is_excel                   ");
            qry.Append($"\n , IF(IFNULL(a.is_print, 0) = 1, 'TRUE', 'FALSE') AS is_print                   ");
            qry.Append($"\n , IF(IFNULL(a.is_admin, 0) = 1, 'TRUE', 'FALSE') AS is_admin                   ");
            qry.Append($"\n , 2 AS id                                                                      ");
            qry.Append($"\n FROM users AS u                                                                ");
            qry.Append($"\n INNER JOIN t_Authority AS a                                                    ");
            qry.Append($"\n   ON u.department = a.user_id                                                  ");
            qry.Append($"\n WHERE u.user_id = '{user_id}'                                                       ");
            qry.Append($"\n   AND IFNULL(a.is_individual, 0) = FALSE                                                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public StringBuilder InsertAuthority(AuthorityModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_Authority (   ");
            qry.Append($"\n   department                ");
            qry.Append($"\n , user_id                   ");
            qry.Append($"\n , user_name                 ");
            qry.Append($"\n , group_name                ");
            qry.Append($"\n , form_name                 ");
            qry.Append($"\n , is_visible                ");
            qry.Append($"\n , is_add                    ");
            qry.Append($"\n , is_update                 ");
            qry.Append($"\n , is_delete                 ");
            qry.Append($"\n , is_excel                  ");
            qry.Append($"\n , is_print                  ");
            qry.Append($"\n , is_admin                  ");
            qry.Append($"\n , edit_user                 ");
            qry.Append($"\n , updatetime                ");
            qry.Append($"\n , is_individual             ");
            qry.Append($"\n ) VALUES (                  ");
            qry.Append($"\n   '{model.department }'                ");
            qry.Append($"\n , '{model.user_id    }'               ");
            qry.Append($"\n , '{model.user_name  }'               ");
            qry.Append($"\n , '{model.group_name }'               ");
            qry.Append($"\n , '{model.form_name  }'               ");
            qry.Append($"\n ,  {model.is_visible }                ");
            qry.Append($"\n ,  {model.is_add     }                ");
            qry.Append($"\n ,  {model.is_update  }                ");
            qry.Append($"\n ,  {model.is_delete  }                ");
            qry.Append($"\n ,  {model.is_excel   }                ");
            qry.Append($"\n ,  {model.is_print   }                ");
            qry.Append($"\n ,  {model.is_admin   }                ");
            qry.Append($"\n , '{model.edit_user  }'               ");
            qry.Append($"\n , '{model.updatetime }'               ");
            qry.Append($"\n ,  {model.is_individual}              ");
            qry.Append($"\n )                                     ");

            string sql = qry.ToString();

            return qry;
        }

        public StringBuilder DeleteUsersAuthority(string user_id, bool is_individual)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_Authority WHERE is_individual = {is_individual}   ");
            if(!string.IsNullOrEmpty(user_id))
                qry.Append($"\n AND user_id = '{user_id}'   ");


            string sql = qry.ToString();

            return qry;
        }

        public DataTable GetAuthority(string user_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                  ");
            qry.Append($"\n   department                                                                 ");
            qry.Append($"\n , user_id                                                                    ");
            qry.Append($"\n , user_name                                                                  ");
            qry.Append($"\n , group_name                                                                 ");
            qry.Append($"\n , form_name                                                                  ");
            qry.Append($"\n , IF(IFNULL(is_visible, 0) = 1, 'TRUE', 'FALSE') AS is_visible               ");
            qry.Append($"\n , IF(IFNULL(is_add, 0) = 1, 'TRUE', 'FALSE') AS is_add                       ");
            qry.Append($"\n , IF(IFNULL(is_update, 0) = 1, 'TRUE', 'FALSE') AS is_update                 ");
            qry.Append($"\n , IF(IFNULL(is_delete, 0) = 1, 'TRUE', 'FALSE') AS is_delete                 ");
            qry.Append($"\n , IF(IFNULL(is_excel, 0) = 1, 'TRUE', 'FALSE') AS is_excel                   ");
            qry.Append($"\n , IF(IFNULL(is_print, 0) = 1, 'TRUE', 'FALSE') AS is_print                   ");
            qry.Append($"\n , IF(IFNULL(is_admin, 0) = 1, 'TRUE', 'FALSE') AS is_admin                   ");
            qry.Append($"\n , edit_user                                                                  ");
            qry.Append($"\n , updatetime                                                                  ");
            qry.Append($"\n FROM  t_Authority                                                                                                    ");
            qry.Append($"\n WHERE 1=1                                                                                                            ");
            qry.Append($"\n   AND user_id = '{user_id}'                                                                                          ");

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
