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
    public class CompanyGroupRepository : ClassRoot, ICompanyGroupRepository
    {
        public StringBuilder DeleteCompanyGroup(string company_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_company_group               ");
            qry.Append($"\n WHERE company_id = '{company_id}'         ");
            return qry;
        }
        public StringBuilder DeleteGroup(int main_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_company_group               ");
            qry.Append($"\n WHERE main_id = {main_id}                 ");
            return qry;
        }

        public StringBuilder InsertCompanyGroup(CompanyGroupModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_company_group (                ");
            qry.Append($"\n   main_id                                    ");
            qry.Append($"\n , sub_id                                     ");
            qry.Append($"\n , company_id                                 ");
            qry.Append($"\n , company                                    ");
            qry.Append($"\n , edit_user                                  ");
            qry.Append($"\n , updatetime                                 ");
            qry.Append($"\n ) VALUES (                                   ");
            qry.Append($"\n    {model.main_id   }                        ");
            qry.Append($"\n ,  {model.sub_id    }                        ");
            qry.Append($"\n , '{model.company_id}'                       ");
            qry.Append($"\n , '{model.company   }'                       ");
            qry.Append($"\n , '{model.edit_user }'                       ");
            qry.Append($"\n , '{model.updatetime}'                       ");
            qry.Append($"\n )                                            ");
            return qry;
        }

        public DataTable GetCompanyGroup()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                      ");
            qry.Append($"\n   g1.*                                      ");
            qry.Append($"\n , g2.cnt                                    ");
            qry.Append($"\n FROM t_company_group  AS g1                 ");
            qry.Append($"\n LEFT OUTER JOIN (                           ");
            qry.Append($"\n	SELECT                                      ");
            qry.Append($"\n      main_id                                ");
            qry.Append($"\n	, COUNT(main_id) AS cnt                     ");
            qry.Append($"\n    FROM t_company_group                     ");
            qry.Append($"\n    GROUP BY main_id                         ");
            qry.Append($"\n )  AS g2                                    ");
            qry.Append($"\n   ON g1.main_id = g2.main_id                ");
            qry.Append($"\n WHERE 1=1                                   ");
            qry.Append($"\n   AND g2.cnt > 1                            ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCompanyGroup2()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                    ");
            qry.Append($"\n  *                                                                        ");
            qry.Append($"\n FROM(                                                                     ");
            qry.Append($"\n 	SELECT                                                                ");
            qry.Append($"\n       distinct                                                            ");
            qry.Append($"\n       CAST(main_id AS CHAR) AS main_id                                    ");
            qry.Append($"\n     , CAST(sub_id AS CHAR) AS sub_id                                      ");
            qry.Append($"\n     , company_id                                                          ");
            qry.Append($"\n     , company                                                             ");
            qry.Append($"\n     , edit_user                                                           ");
            qry.Append($"\n     , updatetime                                                          ");
            qry.Append($"\n     , company_id AS group_code                                            ");
            qry.Append($"\n     , company AS company_code                                             ");
            qry.Append($"\n     FROM t_company_group                                                  ");
            qry.Append($"\n 	UNION ALL                                                             ");
            qry.Append($"\n 	SELECT                                                                ");
            qry.Append($"\n 	  main_id                                                             ");
            qry.Append($"\n 	, 9999 AS sub_id                                                      ");
            qry.Append($"\n     , MAX(CASE WHEN sub_id = 0 THEN company_id END) AS company_id         ");
            qry.Append($"\n     , MAX(CASE WHEN sub_id = 0 THEN company END) AS company               ");
            qry.Append($"\n     , MAX(CASE WHEN sub_id = 0 THEN edit_user END) AS edit_user           ");
            qry.Append($"\n     , MAX(CASE WHEN sub_id = 0 THEN updatetime END) AS updatetime         ");
            qry.Append($"\n 	, GROUP_CONCAT(company_id ORDER BY sub_id SEPARATOR ',') AS group_code");
            qry.Append($"\n 	, GROUP_CONCAT(company ORDER BY sub_id SEPARATOR ',') AS company_code ");
            qry.Append($"\n 	FROM t_company_group                                                  ");
            qry.Append($"\n     GROUP BY main_id                                                      ");
            qry.Append($"\n ) AS t                                                                    ");
            qry.Append($"\n ORDER BY t.main_id, t.sub_id                                              ");

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
