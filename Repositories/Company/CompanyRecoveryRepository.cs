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
    public class CompanyRecoveryRepository : ClassRoot, ICompanyRecoveryRepository
    {
        public DataTable GetCompanyRecoveryTemp(DateTime sdate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                          ");
            qry.Append($"\n t.*                                                                            ");
            qry.Append($"\nFROM(                                                                           ");
            qry.Append($"\n  SELECT                                                                        ");
            qry.Append($"\n    *                                                                           ");
            qry.Append($"\n  , CASE WHEN current_sales_amount = 0 THEN 9999 ELSE (90 / (current_sales_amount / current_balance_amount)) / (current_margin_amount / current_sales_amount * 100) END AS current_div                                                                          ");
            qry.Append($"\n  , CASE WHEN current_sales_amount = 0 THEN 9999 ELSE (90 / (current_sales_amount / average_balance_amount)) / (current_margin_amount / current_sales_amount * 100) END AS average_div                                                                          ");
            qry.Append($"\n  , CASE WHEN current_sales_amount = 0 THEN 9999 ELSE (90 / (current_sales_amount / GREATEST(maximum_balance_amount, pre1_maximum_balance_amount, pre2_maximum_balance_amount))) / (current_margin_amount / current_sales_amount * 100) END AS maximum_div      ");
            qry.Append($"\n  FROM t_company_recovery_temp                                                  ");
            qry.Append($"\n  WHERE updatetime = (SELECT MAX(updatetime) FROM t_company_recovery_temp)      ");
            qry.Append($"\n) AS t                                                                          ");
            qry.Append($"\nWHERE t.current_div > 7 OR t.average_div > 7 OR t.maximum_div > 7               ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public StringBuilder DeleteCompany(string seaover_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_company_recovery               ");
            qry.Append($"\n WHERE company_code = '{seaover_code}'        ");
            return qry;
        }

        public StringBuilder InsertCompany(CompanyRecoveryModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_company_recovery (             ");
            qry.Append($"\n   company_code                               ");
            qry.Append($"\n , last_year_profit                           ");
            qry.Append($"\n , net_operating_capital_rate                 ");
            qry.Append($"\n , operating_capital_rate                     ");
            qry.Append($"\n , ato_capital_rate                           ");
            qry.Append($"\n , equity_capital_rate                        ");
            qry.Append($"\n , target_recovery_month                      ");
            qry.Append($"\n , remark                                     ");
            qry.Append($"\n , updatetime                                 ");
            qry.Append($"\n , edit_user                                  ");
            qry.Append($"\n ) VALUES (                                   ");
            qry.Append($"\n   '{model.company_code              }'       ");
            qry.Append($"\n ,  {model.last_year_profit          }       ");
            qry.Append($"\n ,  {model.net_operating_capital_rate}       ");
            qry.Append($"\n ,  {model.operating_capital_rate    }       ");
            qry.Append($"\n ,  {model.ato_capital_rate          }       ");
            qry.Append($"\n ,  {model.equity_capital_rate       }       ");
            qry.Append($"\n ,  {model.target_recovery_month     }       ");
            qry.Append($"\n , '{model.remark                    }'       ");
            qry.Append($"\n , '{model.updatetime                }'       ");
            qry.Append($"\n , '{model.edit_user                 }'       ");
            qry.Append($"\n )                                            ");
            return qry;
        }
    }
}
