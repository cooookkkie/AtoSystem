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
    public class LoanRepository : ClassRoot, ILoanRepository
    {
        public List<LoanModel> GetLoanList(string bank = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   *                       ");
            qry.Append($"\n FROM t_loan        ");
            qry.Append($"\n WHERE 1=1                 ");
            if (bank != null && !string.IsNullOrEmpty(bank))
            {
                qry.Append($"\n  AND bank = '{bank}'      ");
            }
            
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetLoanList(dr);
        }

        public List<LoanInfo> GetCurrentLoan()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"   SELECT                                                                                                   ");
            qry.Append($"\n    *                                                                                                     ");
            qry.Append($"\n    FROM (                                                                                                ");
            qry.Append($"\n    SELECT                                                                                                ");
            qry.Append($"\n    IFNULL(t.bank, l.bank) AS bank                                                                        ");
            qry.Append($"\n  , IFNULL(t.division, l.division) AS division                                                            ");
            qry.Append($"\n  , IFNULL(l.usance_loan_limit, 0) AS usance_loan_limit                                                   ");
            qry.Append($"\n  , IFNULL(t.usance_loan_used, 0) AS usance_loan_used                                                     ");
            qry.Append($"\n  , IFNULL(l.atsight_loan_limit, 0) AS atsight_loan_limit                                                 ");
            qry.Append($"\n  , IFNULL(t.atsight_loan_used, 0) AS atsight_loan_used                                                   ");
            qry.Append($"\n  FROM t_loan AS l                                                                                        ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                       ");
            qry.Append($"\n    SELECT                                                                                                ");
            qry.Append($"\n      t.bank                                                                                              ");
            qry.Append($"\n    , t.division                                                                                          ");
            qry.Append($"\n    , SUM(t.usance_loan_used) AS usance_loan_used                                                         ");
            qry.Append($"\n    , SUM(t.atsight_loan_used) AS atsight_loan_used                                                       ");
            qry.Append($"\n    FROM(                                                                                                 ");
            qry.Append($"\n  	  SELECT                                                                                             ");
            qry.Append($"\n  		  t.bank                                                                                         ");
            qry.Append($"\n         , t.division                                                                                     ");
            qry.Append($"\n  		, IF(t.loan_type = '유산스', t.used_price, 0) AS usance_loan_used                                ");
            qry.Append($"\n  		, IF(t.loan_type = '일람불', t.used_price, 0) AS atsight_loan_used                               ");
            qry.Append($"\n  		FROM(                                                                                            ");
            qry.Append($"\n  			SELECT                                                                                       ");
            qry.Append($"\n  			  IFNULL(unit_price, 0) * IFNULL(quantity_on_paper, 0) * IFNULL(box_weight, 0) AS used_price ");
            qry.Append($"\n  			, IFNULL(TRIM(REPLACE(payment_bank, '에이티오', '')), '') AS bank                            ");
            qry.Append($"\n  			, IF(usance_type = 'O' OR agency_type = 'O', '유산스', '일람불') AS loan_type                ");
            qry.Append($"\n  			, IFNULL(division, '') AS division                                                           ");
            qry.Append($"\n  			 FROM t_customs                                                                              ");
            qry.Append($"\n  			 WHERE payment_date_status <> '결제완료'                                                     ");
            qry.Append($"\n  			   AND (cc_status IS NULL OR cc_status = ''OR cc_status = '미통'OR cc_status = '미통관')     ");
            qry.Append($"\n  			   AND lc_payment_date IS NOT NULL AND lc_payment_date <> ''                                 ");
            qry.Append($"\n  		) AS t                                                                                           ");
            qry.Append($"\n  		GROUP BY t.bank, t.loan_type, t.division                                                         ");
            qry.Append($"\n      ) AS t                                                                                              ");
            qry.Append($"\n      GROUP BY t.bank, t.division                                                                         ");
            qry.Append($"\n  ) AS t                                                                                                  ");
            qry.Append($"\n    ON l.bank = t.bank                                                                                    ");
            qry.Append($"\n    AND l.division = t.division                                                                           ");
            qry.Append($"\n  ) AS t                                                                                                  ");
            qry.Append($"\n  ORDER BY CASE WHEN t.division = '아토' THEN 1                                                           ");
            qry.Append($"\n                WHEN t.division = '에이티오' THEN 2                                                       ");
            qry.Append($"\n                ELSE                                                                                      ");
            qry.Append($"\n                   3                                                                                      ");
            qry.Append($"\n                END, t.division, t.bank                                                                   ");
            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetCurrentLoanList(dr);
        }

        private List<LoanInfo> SetCurrentLoanList(MySqlDataReader rd)
        {
            List<LoanInfo> list = new List<LoanInfo>();
            while (rd.Read())
            {
                LoanInfo model = new LoanInfo();

                model.bank = rd["bank"].ToString();
                model.division = rd["division"].ToString();
                model.usance_loan_limit = Convert.ToDouble(rd["usance_loan_limit"]);
                model.usance_loan_used = Convert.ToDouble(rd["usance_loan_used"]);
                model.atsight_loan_limit = Convert.ToDouble(rd["atsight_loan_limit"]);
                model.atsight_loan_used = Convert.ToDouble(rd["atsight_loan_used"]);
                list.Add(model);

            }
            rd.Close();
            return list;
        }

        private List<LoanModel> SetLoanList(MySqlDataReader rd)
        {
            List<LoanModel> list = new List<LoanModel>();
            while (rd.Read())
            {
                LoanModel model = new LoanModel();

                model.bank = rd["bank"].ToString();
                model.division = rd["division"].ToString();
                model.usance_loan_limit = Convert.ToDouble(rd["usance_loan_limit"]);
                model.atsight_loan_limit = Convert.ToDouble(rd["atsight_loan_limit"]);
                list.Add(model);

            }
            rd.Close();
            return list;
        }

        public StringBuilder DeleteLoan(string bank, string division)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_loan ");
            qry.Append($" WHERE bank = '{bank}'");
            qry.Append($"   AND division = '{division}'");

            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder InsertLoan(string bank, string division, string usance_loan_limit, string atsight_loan_limit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_loan (");
            qry.Append($"   bank                     ");
            qry.Append($" , division                 ");
            qry.Append($" , usance_loan_limit        ");
            qry.Append($" , atsight_loan_limit       ");
            qry.Append($" ) VALUES (                 ");
            qry.Append($"   '{bank}'                 ");
            qry.Append($" , '{division}'             ");
            qry.Append($" , {usance_loan_limit}      ");
            qry.Append($" , {atsight_loan_limit}     ");
            qry.Append($" )                          ");

            string sql = qry.ToString();
            return qry;
        }

        public int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
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

    }
}
