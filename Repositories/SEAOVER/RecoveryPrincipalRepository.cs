using Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RecoveryPrincipalRepository : ClassRoot, IRecoveryPrincipalRepository
    {
        private static string userId;
        public void SetSeaoverId(string user_id)
        {
            userId = user_id;
        }
        public int CallStoredProcedure(string user_id, string sttdate, string enddate, string company)
        {
            userId = user_id;
            int result = 0;

            SqlCommand cmd = new SqlCommand("SP_업체별매출", (SqlConnection)dbInstance3.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@issu_saup", "%%");
            cmd.Parameters.AddWithValue("@draw_fymd", sttdate);
            cmd.Parameters.AddWithValue("@draw_tymd", enddate);
            cmd.Parameters.AddWithValue("@proc_hnm", company);
            cmd.Parameters.AddWithValue("@work_id", userId);

            result = cmd.ExecuteNonQuery();
            return result;
        }

        

        public DataTable GetRecoerPrincipal(string sYearMonth, string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                      ");
            qry.Append($"\n   거래처명                                                                                                                                                  ");
            qry.Append($"\n , 사용자                                                                                                                                                    ");
            qry.Append($"\n , CONVERT(DATE,사용자 + '01') AS 기준년월                                                                                                                   ");
            qry.Append($"\n , 월                                                                                                                                                        ");
            qry.Append($"\n , ISNULL(매출수, 0) AS 매출수                                                                                                                               ");
            qry.Append($"\n , ISNULL(매입금액, 0) AS 매입금액                                                                                                                           ");
            qry.Append($"\n , ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                                                                                                       ");
            qry.Append($"\n , ISNULL(손익금액, 0) AS 손익금액                                                                                                                           ");
            qry.Append($"\n , ISNULL(미수잔액, 0) AS 미수잔액                                                                                                                           ");
            qry.Append($"\n , ISNULL(월평균잔액, 0) AS 월평균잔액                                                                                                                       ");
            qry.Append($"\n , CASE WHEN (ISNULL(매출금액, 0) + ISNULL(부가세, 0)) > 0 THEN ISNULL(손익금액, 0) / (ISNULL(매출금액, 0) + ISNULL(부가세, 0)) * 100 ELSE 0 END AS 마진율   ");
            qry.Append($"\n , ISNULL(비용, 0) AS 비용                                                                                                                                   ");
            qry.Append($"\n , ISNULL(전화번호, '') AS 전화번호                                                                                                                          ");
            qry.Append($"\n , ISNULL(팩스번호, '') AS 팩스번호                                                                                                                          ");
            qry.Append($"\n , ISNULL(휴대폰, '') AS 휴대폰                                                                                                                              ");
            qry.Append($"\n , CONVERT (DATE, 사용자 + '01') AS 기준년도                                                                                                                 ");
            qry.Append($"\n FROM 업체별월별매출현황 AS a                                                                                                                                ");
            qry.Append($"\n WHERE 사용자 >= 201601 AND 사용자 >= (                                        ");
            qry.Append($"\n 		SELECT MIN(사용자) FROM 업체별월별매출현황 AS b                       ");
            qry.Append($"\n 		WHERE 월 IS NOT NULL                                                  ");
            qry.Append($"\n 		AND 사용자 >= 201601                                                  ");
            qry.Append($"\n 		AND 사용자 <= {sYearMonth.ToString()}                                 ");
            qry.Append($"\n   		AND a.거래처명 = b.거래처명                                           ");
            qry.Append($"\n	 )                                                                            ");
            qry.Append($"\n   AND 사용자 <= {sYearMonth.ToString()}                                       ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n   AND 거래처명 = '{company}'                                       ");
            qry.Append($"\n ORDER BY 사용자                                                        ");


            string sql = qry.ToString();

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();
            
            return dt;
        }
        public DataTable GetPreSalesPrice(int syear, string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                                     ");
            qry.Append($"\n    t1.거래처명                                                                                                                                                                 ");
            qry.Append($"\n  , t1.매출금액                                                                                                                                                                 ");
            qry.Append($"\n  , t1.마진금액                                                                                                                                                                 ");
            qry.Append($"\n  , t1.마진율                                                                                                                                                                  ");
            qry.Append($"\n  , t2.미수잔액 AS 전월미수잔액                                                                                                                                                      ");
            qry.Append($"\n  , t2.월평균잔액 AS 전월평균잔액                                                                                                                                                     ");
            qry.Append($"\n  FROM(                                                                                                                                                                     ");
            qry.Append($"\n 	 SELECT                                                                                                                                                                ");
            qry.Append($"\n 	   거래처명                                                                                                                                                                ");
            qry.Append($"\n 	 , SUM(ISNULL(매출금액, 0)) + SUM(ISNULL(부가세, 0)) AS 매출금액                                                                                                                 ");
            qry.Append($"\n 	 , SUM(ISNULL(손익금액, 0)) AS 마진금액                                                                                                                                       ");
            qry.Append($"\n 	 , CASE WHEN (SUM(ISNULL(매출금액, 0)) + SUM(ISNULL(부가세, 0))) > 0 THEN SUM(ISNULL(손익금액, 0)) / (SUM(ISNULL(매출금액, 0)) + SUM(ISNULL(부가세, 0))) * 100  ELSE 0 END AS 마진율     ");
            qry.Append($"\n 	 FROM 업체별월별매출현황                                                                                                                                                       ");
            qry.Append($"\n 	 WHERE 사용자 < {syear}                                                                                                                                                      ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n    AND 거래처명 = '{company}'                                       ");
            qry.Append($"\n 	 GROUP BY 거래처명                                                                                                                                                         ");
            qry.Append($"\n  ) AS t1                                                                                                                                                                   ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                                                                         ");
            qry.Append($"\n 	 SELECT                                                                                                                                                                ");
            qry.Append($"\n 	   거래처명                                                                                                                                                                ");
            qry.Append($"\n 	 , ISNULL(미수잔액, 0) AS 미수잔액                                                                                                                                            ");
            qry.Append($"\n 	 , ISNULL(월평균잔액, 0) AS 월평균잔액                                                                                                                                          ");
            qry.Append($"\n 	 FROM 업체별월별매출현황 AS a                                                                                                                                                  ");
            qry.Append($"\n 	 WHERE a.사용자 < {syear}                                                                                                                                                    ");
            qry.Append($"\n 	   AND CONVERT(datetime, CONVERT(varchar,a.사용자) + '-' + CONVERT(varchar,a.월) + '-' + '1') = (                                                                          ");
            qry.Append($"\n 		   	SELECT                                                                                                                                                         ");
            qry.Append($"\n 			MAX(CONVERT (datetime, CONVERT(varchar,사용자) + '-' + CONVERT(varchar,월) + '-' + '1'))                                                                           ");
            qry.Append($"\n 			FROM 업체별월별매출현황 AS b                                                                                                                                           ");
            qry.Append($"\n 			WHERE a.거래처명 = b.거래처명                                                                                                                                         ");
            qry.Append($"\n 			AND b.사용자 < {syear}                                                                                                                                               ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n             AND b.거래처명 = '{company}'                                       ");
            qry.Append($"\n 			GROUP BY 거래처명	                                                                                                                                               ");
            qry.Append($"\n 			)                                                                                                                                                              ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n   AND a.거래처명 = '{company}'                                       ");
            qry.Append($"\n  ) AS t2                                                                                                                                                                   ");
            qry.Append($"\n    ON t1.거래처명 = t2.거래처명                                                                                                                                                   ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public Int32 GetPreAvgBalacePrice(string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT AVG(ISNULL(매출금액, 0) + ISNULL(부가세, 0)) FROM 업체별월별매출현황 ");
            if (!string.IsNullOrEmpty(company))
            {
                qry.Append($"\n WHERE 사용자 = '{userId}' AND 거래처명 LIKE '%{company}%' ");
            }

            string sql = qry.ToString();

            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            try
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                return -1;
            }
        }
    }
}
