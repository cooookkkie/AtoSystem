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
    public class AnnualRepository : ClassRoot, IAnnualRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public StringBuilder UpdateDailyTarget(string user_id, int daily_target)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE users SET daily_work_goals_amount = {daily_target}");
            qry.Append($"\n WHERE user_id = '{user_id}'");

            string sql = qry.ToString();
            return qry;
        }
        public StringBuilder DeleteAnnual(string user_id, DateTime sttdate)
        {
            DateTime sttDt = new DateTime(sttdate.Year, sttdate.Month, 1);
            DateTime endDt = new DateTime(sttdate.Year, sttdate.Month, 1).AddMonths(1).AddDays(-1);

            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_used_vacation WHERE user_id = '{user_id}'");
            qry.Append($" AND sttdate >= '{sttDt.ToString("yyyy-MM-dd")}'");
            qry.Append($" AND sttdate <= '{endDt.ToString("yyyy-MM-dd")}'");


            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder InsertAnnual(VacationModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_used_vacation (                             ");
            qry.Append($"\n   id                                               ");
            qry.Append($"\n , user_id                                          ");
            qry.Append($"\n , user_name                                        ");
            qry.Append($"\n , vacation_type                                    ");
            qry.Append($"\n , sttdate                                          ");
            qry.Append($"\n , enddate                                          ");
            qry.Append($"\n , used_days                                        ");
            qry.Append($"\n , updatetime                                       ");
            qry.Append($"\n , edit_user                                        ");
            qry.Append($"\n ) VALUES (                                         ");
            qry.Append($"\n    {model.id           }                                     ");
            qry.Append($"\n , '{model.user_id      }'                                    ");
            qry.Append($"\n , '{model.user_name    }'                                    ");
            qry.Append($"\n , '{model.vacation_type}'                                    ");
            qry.Append($"\n , '{model.sttdate      }'                                    ");
            if(string.IsNullOrEmpty(model.enddate))
                qry.Append($"\n , NULL                                    ");
            else 
                qry.Append($"\n , '{model.enddate      }'                                ");
            qry.Append($"\n ,  {model.used_days    }                                     ");
            qry.Append($"\n , '{model.updatetime   }'                                    ");
            qry.Append($"\n , '{model.edit_user   }'                                     ");
            qry.Append($"\n )                                                  ");

            string sql = qry.ToString();
            return qry;
        }
        public DataTable GetAnnual(string user_id, string user_name, DateTime sttdate, DateTime enddate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT * FROM t_used_vacation                      ");
            qry.Append($"\n WHERE  1 = 1                                ");
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"\n    {commonRepository.whereSql("user_name", user_name)}       ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n    {commonRepository.whereSql("user_id", user_id)}       ");

            qry.Append($"\n AND sttdate >= '{sttdate.ToString("yyyy-MM-dd")}'                                ");
            qry.Append($"\n AND sttdate <= '{enddate.ToString("yyyy-MM-dd")}'                                ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public StringBuilder DeleteAccruedAnnual(string user_id, int year)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_accrued_vacation WHERE user_id = '{user_id}' AND year = {year}");


            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder InsertAccruedAnnual(AcruedAnnualModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_accrued_vacation (                             ");
            qry.Append($"\n   user_id                                          ");
            qry.Append($"\n , year                                             ");
            qry.Append($"\n , vacation                                         ");
            qry.Append($"\n , edit_user                                        ");
            qry.Append($"\n , updatetime                                       ");
            qry.Append($"\n ) VALUES (                                         ");
            qry.Append($"\n   '{model.user_id   }'                                       ");
            qry.Append($"\n ,  {model.year      }                                        ");
            qry.Append($"\n ,  {model.vacation  }                                        ");
            qry.Append($"\n , '{model.edit_user }'                                       ");
            qry.Append($"\n , '{model.updatetime}'                                       ");
            qry.Append($"\n )                                                  ");

            string sql = qry.ToString();
            return qry;
        }


        public bool GetAgreement(string user_id, int year, int month)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                         ");
            qry.Append($"\n   year                                                                         ");
            qry.Append($"\n , month                                                                        ");
            qry.Append($"\n , IF(IFNULL(is_agreement, 0) = 0, 'FALSE', 'TRUE') AS is_agreement             ");
            qry.Append($"\n , updatetime                                                                   ");
            qry.Append($"\n , edit_user                                                                    ");
            qry.Append($"\n , user_id                                                                      ");
            qry.Append($"\n FROM t_agreement_vacation                    ");
            qry.Append($"\n WHERE 1=1                                    ");
            qry.Append($"\n   AND user_id = '{user_id}'                  ");
            qry.Append($"\n   AND year = {year}                  ");
            qry.Append($"\n   AND month = {month}                  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            bool isAgreement = false;
            if (dt.Rows.Count > 0)
                isAgreement = Convert.ToBoolean(dt.Rows[0]["is_agreement"].ToString());

            return isAgreement;
        }

        public StringBuilder DeleteAgreementAnnual(string user_id, int year, int month)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_agreement_vacation WHERE user_id = '{user_id}' AND year = {year} AND month = {month}");


            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder InsertAgreementAnnual(VacationAgreementModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_agreement_vacation (                 ");
            qry.Append($"\n   year                                             ");
            qry.Append($"\n , month                                            ");
            qry.Append($"\n , is_agreement                                     ");
            qry.Append($"\n , updatetime                                       ");
            qry.Append($"\n , edit_user                                        ");
            qry.Append($"\n , user_id                                     ");
            qry.Append($"\n ) VALUES (                                         ");
            qry.Append($"\n   {model.year        }                             ");
            qry.Append($"\n , {model.month       }                             ");
            qry.Append($"\n , {model.is_agreement}                             ");
            qry.Append($"\n , '{model.updatetime  }'                           ");
            qry.Append($"\n , '{model.edit_user   }'                           ");
            qry.Append($"\n , '{model.user_id}'                           ");
            qry.Append($"\n )                                                  ");

            string sql = qry.ToString();
            return qry;
        }


    }
}
