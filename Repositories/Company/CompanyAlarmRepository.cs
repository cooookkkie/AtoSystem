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
    public class CompanyAlarmRepository : ClassRoot, ICompanyAlarmRepository
    {
        public StringBuilder DeleteAlarm(string company_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_company_alarm WHERE company_id = {company_id}          ");
            return qry;
        }
        public StringBuilder InsertAlarm(CompanyAlarmModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT INTO t_company_alarm (        ");
            qry.Append($"   company_id                        ");
            qry.Append($" , division                          ");
            qry.Append($" , category                          ");
            qry.Append($" , alarm_date                        ");
            qry.Append($" , alarm_remark                      ");
            qry.Append($" , alarm_complete                    ");
            qry.Append($" , edit_user                         ");
            qry.Append($" , updatetime                        ");
            qry.Append($") VALUES (                           ");
            qry.Append($"    {model.company_id     }                    ");
            qry.Append($" , '{model.division       }'                   ");
            qry.Append($" , '{model.category     }'                   ");
            qry.Append($" , '{model.alarm_date     }'                   ");
            qry.Append($" , '{model.alarm_remark   }'                   ");
            qry.Append($" ,  {model.alarm_complete }                    ");
            qry.Append($" , '{model.edit_user      }'                   ");
            qry.Append($" , '{model.updatetime     }'                   ");
            qry.Append($")                                    ");
            return qry;
        }

        public DataTable GetCompanyAlarm(int company_id = -1, string user_name = "", string complete_date = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                   ");
            qry.Append($"\n   *                                                      ");
            qry.Append($"\n , CASE WHEN alarm_date <= '{complete_date.ToString()}' THEN 1 ELSE 0 END AS isComplete              ");
            qry.Append($"\n , alarm_date AS real_alarm_date             ");
            qry.Append($"\n , '0000-00-00' AS complete_date             ");
            qry.Append($"\n FROM t_company_alarm                                     ");
            qry.Append($"\n WHERE 1=1                                                ");
            qry.Append($"\n   AND alarm_complete = false                             ");
            if (company_id > -1)
                qry.Append($"\n   AND company_id = {company_id}                          ");
            if(!string.IsNullOrEmpty(user_name))
                qry.Append($"\n   AND edit_user = '{user_name}'                          ");
            qry.Append($"\n ORDER BY CASE WHEN alarm_date <= '{complete_date.ToString()}' THEN 1 ELSE 0 END                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCompanyAlarmOneLine(string user_name = "", string sdate = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                    ");
            qry.Append($"\n   company_id                                                                                              ");
            qry.Append($"\n , alarm_date                                                                       ");
            qry.Append($"\n , division                                                                         ");
            qry.Append($"\n , IF(IFNULL(alarm_complete, 0) = 0, 'FALSE', 'TRUE') AS alarm_complete             ");
            qry.Append($"\n FROM t_company_alarm                                                                                      ");
            qry.Append($"\n WHERE 1=1                                                                                                 ");
            
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"\n   AND edit_user = '{user_name}'                          ");

            if (!string.IsNullOrEmpty(sdate))
                qry.Append($"\n   AND alarm_date = '{sdate}'                          ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCompanyAlarmForCalendar(string user_name = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                   ");
            qry.Append($"\n   c.id AS company_id                                                                                                                     ");
            qry.Append($"\n , c.name AS company                                                                                                                      ");
            qry.Append($"\n , a.division                                                                                                                             ");
            qry.Append($"\n , a.category                                                                                                                             ");
            qry.Append($"\n , CASE WHEN a.category = '주알람' THEN DATE_FORMAT(DATE_ADD(NOW(), INTERVAL a.alarm_date - (WEEKDAY(NOW()) + 1) DAY), '%Y-%m-%d')        ");
            qry.Append($"\n     WHEN a.category = '월알람' THEN CONCAT(DATE_FORMAT(NOW(), '%Y-%m') , '-', a.alarm_date)                                              ");
            qry.Append($"\n     ELSE a.alarm_date END AS alarm_date                                                                                                  ");
            qry.Append($"\n , a.alarm_remark                                                                                                                         ");
            qry.Append($"\n , c.ato_manager                                                                                                                          ");
            qry.Append($"\n , c.alarm_complete_date                                                                                                                  ");
            qry.Append($"\n FROM t_company AS c                                                                                                                      ");
            qry.Append($"\n INNER JOIN t_company_alarm AS a                                                                                                          ");
            qry.Append($"\n   ON c.id = a.company_id                                                                                                                 ");
            qry.Append($"\n WHERE 1=1                                                                                                                                ");
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"\n   AND c.ato_manager = '{user_name}'                                                                                                      ");

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
