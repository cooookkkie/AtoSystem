using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RecoveryPrincipal
{
    public class RecoveryPrincipalPriceRepository: ClassRoot, IRecoveryPrincipalPriceRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        private static string userId;
        public void SetSeaoverId(string user_id)
        {
            userId = user_id;
        }
        public DataTable GetRecoveryPrincipalTemp(DateTime sttdate, DateTime enddate, string company, bool isExistBalance = false, bool out_business = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                      ");
            qry.Append($"\n   c.거래처코드                                                                                                                              ");
            qry.Append($"\n , c.거래처명                                                                                                                                ");
            qry.Append($"\n , c.매출자                                                                                                                                ");
            qry.Append($"\n , CONVERT(float, ISNULL(c.매출한도금액, 0)) AS 매출한도금액                                                                                                 ");
            qry.Append($"\n , CONVERT(float, ISNULL(c.현재미수잔액, 0)) AS 현재미수잔액                                                                                                 ");
            qry.Append($"\n , CONVERT(float, ISNULL(s.당월매출금액, 0)) AS 당월매출금액                                                                                                 ");
            qry.Append($"\n , CONVERT(float, ISNULL(s.전월매출금액, 0)) AS 전월매출금액                                                                                                 ");
            qry.Append($"\n , CONVERT(float, ISNULL(s.전전월매출금액, 0)) AS 전전월매출금액                                                                                                 ");
            qry.Append($"\n , CONVERT(float, ISNULL(s.전전전월매출금액, 0)) AS 전전전월매출금액                                                                                                 ");
            qry.Append($"\n , CONVERT(float, ISNULL(s.누적매출금액, 0)) AS 누적매출금액                                                                                                 ");
            qry.Append($"\n , CONVERT(float, ISNULL(s.당월마진금액, 0)) AS 당월마진금액                                                                                                 ");
            qry.Append($"\n , CONVERT(float, ISNULL(s.누적마진금액, 0)) AS 누적마진금액                                                                                                 ");
            qry.Append($"\n , CONVERT(VARCHAR, ISNULL(s.최근매출일자, ''), 120) AS 최근매출일자                                                                                             ");
            qry.Append($"\n , CONVERT(float, ISNULL(b.당월최고미수  , 0)) AS 당월최고미수                                                                                                       ");
            qry.Append($"\n , CONVERT(float, ISNULL(b.전월최고미수  , 0)) AS 전월최고미수                                                                                                       ");
            qry.Append($"\n , CONVERT(float, ISNULL(b.전전월최고미수  , 0)) AS 전전월최고미수                                                                                                       ");
            qry.Append($"\n , CONVERT(float, ISNULL(b.평균미수  , 0)) AS 평균미수                                                                                                       ");
            qry.Append($"\n , CONVERT(float, ISNULL(p.당월입금금액, 0)) AS 당월입금금액                                                                                                 ");
            qry.Append($"\n , CONVERT(VARCHAR, ISNULL(p.최근입금일자, ''), 120) AS 최근입금일자                                                                         ");
            qry.Append($"\n , CONVERT(float, ISNULL(c.현재미수잔액, 0) - ISNULL(s.누적마진금액, 0)) AS 손익분기잔액                                                                                                 ");
            qry.Append($"\n FROM (                                                                                                                                      ");
            qry.Append($"\n 	SELECT                                                                                            ");
            qry.Append($"\n 	  c.거래처코드                                                                                        ");
            qry.Append($"\n 	, c.거래처명                                                                                         ");
            qry.Append($"\n 	, c.매출한도금액                                                                                       ");
            qry.Append($"\n 	, c.매출자                                                                                       ");
            qry.Append($"\n 	, b.미수잔액 AS 현재미수잔액                                                                              ");
            qry.Append($"\n 	FROM(                                                                                             ");
            qry.Append($"\n 		SELECT                                                                                        ");
            qry.Append($"\n 		  *                                                                                           ");
            qry.Append($"\n		FROM(                                                                                             ");
            qry.Append($"\n			SELECT                                                                                        ");
            qry.Append($"\n			  거래처코드                                                                                      ");
            qry.Append($"\n			, 거래처명                                                                                       ");
            qry.Append($"\n			, ISNULL(매출한도금액, 0) AS 매출한도금액                                                              ");
            qry.Append($"\n			, ROW_NUMBER() OVER(PARTITION BY 거래처코드 ORDER BY 사용자 DESC) AS rownum                         ");
            qry.Append($"\n			, 사용자                                                                                        ");
            qry.Append($"\n			, 매출자                                                                                        ");
            qry.Append($"\n			FROM 업체별월별매출현황                                                                              ");
            qry.Append($"\n		) AS c                                                                                            ");
            qry.Append($"\n		WHERE 1=1                                                                                         ");
            qry.Append($"\n		  AND rownum = 1 	                                                                              ");
            qry.Append($"\n 	) AS c                                                                                            ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                                                                 ");
            qry.Append($"\n 		SELECT                                                                                        ");
            qry.Append($"\n 	  	  거래처코드                                                                                      ");
            qry.Append($"\n 		, 잔액 AS 미수잔액                                                                                       ");
            qry.Append($"\n 		FROM 거래처별일일미수 AS a                                                                              ");
            qry.Append($"\n 		WHERE 날짜 = '{DateTime.Now.ToString("yyyy-MM-dd")}'                                                           ");
            /*qry.Append($"\n 		SELECT                                                                                        ");
            qry.Append($"\n 	  	  거래처코드                                                                                      ");
            qry.Append($"\n 		, 미수잔액                                                                                       ");
            qry.Append($"\n 		FROM 업체별월별매출현황 AS a                                                                              ");
            //'qry.Append($"\n 		WHERE 사용자 = (SELECT MAX(사용자) FROM 업체별월별매출현황 AS b WHERE a.거래처코드 = b.거래처코드)                                               ");
            qry.Append($"\n 		WHERE 사용자 = '{DateTime.Now.ToString("yyyyMM")}'                                                           ");*/
            qry.Append($"\n 	) AS b                                                                                            ");
            qry.Append($"\n 	  ON c.거래처코드 = b.거래처코드                                                                          ");
            qry.Append($"\n 	WHERE 1=1                                                                                         ");
            qry.Append($"\n ) AS c                                                                                                                                      ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                           ");
            qry.Append($"\n 	SELECT                                                                                                                                  ");
            qry.Append($"\n 	  매출처코드                                                                                                                               ");
            qry.Append($"\n 	, SUM(CASE WHEN 매출일자 > '{sttdate.ToString("yyyy-MM-dd")}' AND 매출일자 <= '{enddate.ToString("yyyy-MM-dd")}' THEN ISNULL(매출금액, 0) + ISNULL(부가세, 0) ELSE 0 END) AS 당월매출금액          ");
            qry.Append($"\n 	, SUM(CASE WHEN 매출일자 > '{sttdate.AddMonths(-1).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{sttdate.ToString("yyyy-MM-dd")}' THEN ISNULL(매출금액, 0) + ISNULL(부가세, 0) ELSE 0 END) AS 전월매출금액          ");
            qry.Append($"\n 	, SUM(CASE WHEN 매출일자 > '{sttdate.AddMonths(-2).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{sttdate.AddMonths(-1).ToString("yyyy-MM-dd")}' THEN ISNULL(매출금액, 0) + ISNULL(부가세, 0) ELSE 0 END) AS 전전월매출금액         ");
            qry.Append($"\n 	, SUM(CASE WHEN 매출일자 > '{sttdate.AddMonths(-3).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{sttdate.AddMonths(-2).ToString("yyyy-MM-dd")}' THEN ISNULL(매출금액, 0) + ISNULL(부가세, 0) ELSE 0 END) AS 전전전월매출금액         ");
            qry.Append($"\n 	, SUM(CASE WHEN 매출일자 > '{sttdate.ToString("yyyy-MM-dd")}' AND 매출일자 <= '{enddate.ToString("yyyy-MM-dd")}' THEN ISNULL(손익, 0) END) AS 당월마진금액         ");
            qry.Append($"\n 	, SUM(CASE WHEN 매출일자 <= '{sttdate.ToString("yyyy-MM-dd")}' THEN ISNULL(손익, 0) END) AS 누적마진금액         ");
            qry.Append($"\n 	, SUM(CASE WHEN 매출일자 <= '{sttdate.ToString("yyyy-MM-dd")}' THEN ISNULL(매출금액, 0) + ISNULL(부가세, 0) ELSE 0 END) AS 누적매출금액		                           ");
            qry.Append($"\n 	, MAX(매출일자) AS 최근매출일자                                                                                                                ");                                                                                                     
            qry.Append($"\n 	FROM 매출현황                                                                                                                            ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                                                                                                  ");
            qry.Append($"\n 	GROUP BY 매출처코드                                                                                                                      ");
            qry.Append($"\n ) AS s                                                                                                                                       ");
            qry.Append($"\n   ON c.거래처코드 = s.매출처코드                                                                                                             ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                            ");
            qry.Append($"\n 	SELECT                                                                                                                                   ");
            qry.Append($"\n 	  거래처코드                                                                                                                                   ");
            qry.Append($"\n 	, SUM(당월최고미수) AS 당월최고미수                                                                                                   ");
            qry.Append($"\n 	, SUM(전월최고미수) AS 전월최고미수                                                                                                   ");
            qry.Append($"\n 	, SUM(전전월최고미수) AS 전전월최고미수                                                                                                   ");
            qry.Append($"\n 	, SUM(평균미수) AS 평균미수                                                                                                   ");
            qry.Append($"\n 	FROM(                                                                                                                                   ");
            qry.Append($"\n 	SELECT                                                                                                                                   ");
            qry.Append($"\n 	  거래처코드                                                                                                                             ");
            qry.Append($"\n 	, 0 AS 당월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 전월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 전전월최고미수                                                                                                                  ");
            qry.Append($"\n 	, AVG(잔액) AS 평균미수                                                                                                                  ");
            qry.Append($"\n 	FROM 거래처별일일미수                                                                                                                    ");
            qry.Append($"\n 	WHERE 날짜 > '{sttdate.ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}'                                            ");
            qry.Append($"\n 	GROUP BY 거래처코드                                                                                                                      ");
            qry.Append($"\n 	UNION ALL                                                                                                                      ");
            qry.Append($"\n 	SELECT                                                                                                                                   ");
            qry.Append($"\n 	  거래처코드                                                                                                                             ");
            qry.Append($"\n 	, MAX(잔액) AS 당월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 전월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 전전월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 평균미수                                                                                                                  ");
            qry.Append($"\n 	FROM 거래처별일일미수                                                                                                                    ");
            qry.Append($"\n 	WHERE 날짜 >= '{new DateTime(enddate.Year, enddate.Month, 1).ToString("yyyy-MM-dd")}' AND 날짜 <= '{new DateTime(enddate.Year, enddate.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd")}'                                            ");
            qry.Append($"\n 	GROUP BY 거래처코드                                                                                                                      ");
            qry.Append($"\n 	UNION ALL                                                                                                                      ");
            qry.Append($"\n 	SELECT                                                                                                                                   ");
            qry.Append($"\n 	  거래처코드                                                                                                                             ");
            qry.Append($"\n 	, 0 AS 당월최고미수                                                                                                                  ");
            qry.Append($"\n 	, MAX(잔액) AS 전월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 전전월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 평균미수                                                                                                                  ");
            qry.Append($"\n 	FROM 거래처별일일미수                                                                                                                    ");
            qry.Append($"\n 	WHERE 날짜 >= '{new DateTime(enddate.AddMonths(-1).Year, enddate.AddMonths(-1).Month, 1).ToString("yyyy-MM-dd")}' AND 날짜 <= '{new DateTime(enddate.Year, enddate.Month, 1).AddDays(-1).ToString("yyyy-MM-dd")}'                                            ");
            qry.Append($"\n 	GROUP BY 거래처코드                                                                                                                      ");
            qry.Append($"\n 	UNION ALL                                                                                                                      ");
            qry.Append($"\n 	SELECT                                                                                                                                   ");
            qry.Append($"\n 	  거래처코드                                                                                                                             ");
            qry.Append($"\n 	, 0 AS 당월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 전월최고미수                                                                                                                  ");
            qry.Append($"\n 	, MAX(잔액) AS 전전월최고미수                                                                                                                  ");
            qry.Append($"\n 	, 0 AS 평균미수                                                                                                                  ");
            qry.Append($"\n 	FROM 거래처별일일미수                                                                                                                    ");
            qry.Append($"\n 	WHERE 날짜 >= '{new DateTime(enddate.AddMonths(-2).Year, enddate.AddMonths(-2).Month, 1).ToString("yyyy-MM-dd")}' AND 날짜 <= '{new DateTime(enddate.AddMonths(-1).Year, enddate.AddMonths(-1).Month, 1).AddDays(-1).ToString("yyyy-MM-dd")}'                                            ");
            qry.Append($"\n 	GROUP BY 거래처코드                                                                                                                      ");
            qry.Append($"\n 	) AS b                                                                                                                      ");
            qry.Append($"\n 	GROUP BY 거래처코드                                                                                                         ");
            qry.Append($"\n ) AS b                                                                                                                                       ");
            qry.Append($"\n   ON c.거래처코드 = b.거래처코드                                                                                                             ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                            ");
            qry.Append($"\n 	SELECT                                                                                                                                   ");
            qry.Append($"\n 	  거래처코드                                                                                                                             ");
            qry.Append($"\n 	, SUM(ISNULL(입금금액, 0)) AS 당월입금금액                                                                                               ");
            qry.Append($"\n 	, MAX(입금일자) AS 최근입금일자                                                                                                          ");
            qry.Append($"\n 	FROM 거래처별결제현황                                                                                                                    ");
            qry.Append($"\n 	WHERE 입금일자 >= '{sttdate.ToString("yyyy-MM-dd")}' AND 입금일자 <= '{enddate.ToString("yyyy-MM-dd")}'                                    ");
            qry.Append($"\n 	GROUP BY 거래처코드                                                                                                                      ");
            qry.Append($"\n ) AS p                                                                                                                                       ");
            qry.Append($"\n   ON c.거래처코드 = p.거래처코드                                                                                                             ");
            qry.Append($"\n WHERE 1=1                                                                                                                                    ");
            if (isExistBalance)
                qry.Append($"\n   AND 현재미수잔액 > 0                                                                                                                   ");
            if(!string.IsNullOrEmpty(company))
                qry.Append($"\n   AND c.거래처명 LIKE '%{company}%'                                                                                                                   ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public int CallStoredProcedure(string user_id, string sttdate, string enddate, string company = "")
        {
            userId = user_id;
            int result = 0;

            SqlCommand cmd = new SqlCommand("SP_업체별매출", (SqlConnection)dbInstance3.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@issu_saup", "%%");
            cmd.Parameters.AddWithValue("@draw_fymd", sttdate);
            cmd.Parameters.AddWithValue("@draw_tymd", enddate);
            cmd.Parameters.AddWithValue("@proc_hnm", "%" + company + "%");
            cmd.Parameters.AddWithValue("@work_id", userId);

            result = cmd.ExecuteNonQuery();
            return result;
        }

        public DataTable GetRecoveryINfo(string company_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                             ");
            qry.Append($"\n   t.거래처코드                                                         ");
            qry.Append($"\n , t.거래처명                                                           ");
            qry.Append($"\n , ISNULL(t1.매출금액, 0) AS 평균매출금액                                    ");
            qry.Append($"\n , ISNULL(t1.손익금액, 0) AS 평균손익금액                                    ");
            qry.Append($"\n , ISNULL(t1.미수잔액, 0) AS 평균미수잔액                                    ");
            qry.Append($"\n , ISNULL(t1.전손익금액, 0) AS 평균전누적손익금액                                  ");
            qry.Append($"\n , ISNULL(t2.매출금액, 0) AS 매출금액                                      ");
            qry.Append($"\n , ISNULL(t2.손익금액, 0) AS 손익금액                                      ");
            qry.Append($"\n , ISNULL(t2.미수잔액, 0) AS 미수잔액                                      ");
            qry.Append($"\n , ISNULL(t2.전손익금액, 0) AS 전누적손익금액                                     ");
            qry.Append($"\n , ISNULL(t2.사용자, 0) AS 전월                                         ");
            qry.Append($"\n , ISNULL(t3.매출금액, 0) AS 전매출금액                                     ");
            qry.Append($"\n , ISNULL(t3.손익금액, 0) AS 전손익금액                                     ");
            qry.Append($"\n , ISNULL(t3.미수잔액, 0) AS 전미수잔액                                     ");
            qry.Append($"\n , ISNULL(t3.사용자, 0) AS 전전월                                        ");
            qry.Append($"\n , ISNULL(t3.전손익금액, 0) AS 전전누적손익금액                                    ");

            qry.Append($"\n , ISNULL(t4.매출금액, 0) AS 전전매출금액                                     ");
            qry.Append($"\n , ISNULL(t4.손익금액, 0) AS 전전손익금액                                     ");
            qry.Append($"\n , ISNULL(t4.미수잔액, 0) AS 전전미수잔액                                     ");
            qry.Append($"\n , ISNULL(t4.사용자, 0) AS 전전전월                                        ");
            qry.Append($"\n , ISNULL(t4.전손익금액, 0) AS 전전전누적손익금액                                    ");

            qry.Append($"\n FROM (                                                             ");
            qry.Append($"\n 	SELECT                                                         ");
            qry.Append($"\n 	  DISTINCT                                                     ");
            qry.Append($"\n 	  거래처코드                                                       ");
            qry.Append($"\n 	, 거래처명                                                         ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n ) AS t                                                             ");
            qry.Append($"\n LEFT OUTER JOIN (                                                  ");
            qry.Append($"\n	SELECT                                                             ");
            qry.Append($"\n	  t.거래처코드                                                         ");
            qry.Append($"\n    , t.거래처명                                                        ");
            qry.Append($"\n    , t.매출금액                                                        ");
            qry.Append($"\n    , t.손익금액                                                        ");
            qry.Append($"\n    , t.미수잔액                                                        ");
            qry.Append($"\n    , t2.손익금액 AS 전손익금액                                             ");
            qry.Append($"\n	FROM(                                                              ");
            qry.Append($"\n	 	SELECT                                                         ");
            qry.Append($"\n	 	  거래처코드                                                       ");
            qry.Append($"\n	 	, 거래처명                                                         ");
            qry.Append($"\n	 	, SUM(ISNULL(매출금액, 0) + ISNULL(부가세, 0)) AS 매출금액               ");
            qry.Append($"\n	 	, SUM(ISNULL(손익금액, 0)) AS 손익금액                                ");
            qry.Append($"\n	 	, SUM(ISNULL(미수잔액, 0)) AS 미수잔액                                ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n	 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n	       AND 사용자 < 202308                                            ");
            qry.Append($"\n	   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n	 	GROUP BY 거래처코드, 거래처명                                          ");
            qry.Append($"\n 	) AS t                                                         ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                              ");
            qry.Append($"\n		SELECT                                                         ");
            qry.Append($"\n	 	  거래처코드                                                       ");
            qry.Append($"\n	 	, SUM(ISNULL(손익금액, 0)) AS 손익금액                                ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n	 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n	 	   AND 거래처코드 ='{company_code}'                                       ");
            qry.Append($"\n	       AND 사용자 < (                                                 ");
            qry.Append($"\n	       	SELECT                                                     ");
            qry.Append($"\n		 	  MAX(사용자)                                                 ");
            qry.Append($"\n		 	FROM 업체별월별매출현황 AS b                                       ");
            qry.Append($"\n		 	WHERE 거래처코드 ='{company_code}'                                    ");
            qry.Append($"\n		 	  AND ISNULL(매출금액, 0) > 0                                  ");
            qry.Append($"\n		 	  AND 사용자 < {DateTime.Now.ToString("yyyyMM")}                                         ");
            qry.Append($"\n	       )                                                           ");
            qry.Append($"\n	   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n	   	 GROUP BY 거래처코드                                               ");
            qry.Append($"\n   	) AS t2                                                        ");
            qry.Append($"\n   	  ON t.거래처코드 = t2.거래처코드                                       ");
            qry.Append($"\n ) AS t1                                                            ");
            qry.Append($"\n   ON t.거래처코드 = t1.거래처코드                                           ");
            qry.Append($"\n LEFT OUTER JOIN (                                                  ");
            qry.Append($"\n    SELECT                                                          ");
            qry.Append($"\n      t.거래처코드                                                      ");
            qry.Append($"\n    , t.거래처명                                                        ");
            qry.Append($"\n    , t.매출금액                                                        ");
            qry.Append($"\n    , t.손익금액                                                        ");
            qry.Append($"\n    , t.미수잔액                                                        ");
            qry.Append($"\n    , t.사용자                                                         ");
            qry.Append($"\n    , t2.손익금액 AS 전손익금액                                             ");
            qry.Append($"\n    FROM(                                                           ");
            qry.Append($"\n	   	SELECT                                                         ");
            qry.Append($"\n	 	  거래처코드                                                       ");
            qry.Append($"\n	 	, 거래처명                                                         ");
            qry.Append($"\n	 	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                    ");
            qry.Append($"\n	 	, ISNULL(손익금액, 0) AS 손익금액                                     ");
            qry.Append($"\n	 	, ISNULL(미수잔액, 0) AS 미수잔액                                     ");
            qry.Append($"\n	 	, 사용자                                                          ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n	 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n	 	   AND 거래처코드 ='{company_code}'                                       ");
            qry.Append($"\n	       AND 사용자 = (                                                 ");
            qry.Append($"\n	       	SELECT                                                     ");
            qry.Append($"\n		 	  MAX(사용자)                                                 ");
            qry.Append($"\n		 	FROM 업체별월별매출현황 AS b                                       ");
            qry.Append($"\n		 	WHERE 거래처코드 ='{company_code}'                                    ");
            qry.Append($"\n		 	  AND ISNULL(매출금액, 0) > 0                                  ");
            qry.Append($"\n		 	  AND 사용자 < {DateTime.Now.ToString("yyyyMM")}                                         ");
            qry.Append($"\n	       )                                                           ");
            qry.Append($"\n	   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n   	) AS t                                                         ");
            qry.Append($"\n   	LEFT OUTER JOIN (                                              ");
            qry.Append($"\n		SELECT                                                         ");
            qry.Append($"\n	 	  거래처코드                                                       ");
            qry.Append($"\n	 	, SUM(ISNULL(손익금액, 0)) AS 손익금액                                ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n	 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n	 	   AND 거래처코드 ='{company_code}'                                       ");
            qry.Append($"\n	       AND 사용자 < (                                                 ");
            qry.Append($"\n	       	SELECT                                                     ");
            qry.Append($"\n		 	  MAX(사용자)                                              ");
            qry.Append($"\n		 	FROM 업체별월별매출현황 AS b                                       ");
            qry.Append($"\n		 	WHERE 거래처코드 ='{company_code}'                                    ");
            qry.Append($"\n		 	  AND ISNULL(매출금액, 0) > 0                                  ");
            qry.Append($"\n		 	  AND 사용자 < {DateTime.Now.ToString("yyyyMM")}                                         ");
            qry.Append($"\n	       )                                                           ");
            qry.Append($"\n	   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n	   	 GROUP BY 거래처코드                                               ");
            qry.Append($"\n   	) AS t2                                                        ");
            qry.Append($"\n   	  ON t.거래처코드 = t2.거래처코드                                       ");
            qry.Append($"\n ) AS t2                                                            ");
            qry.Append($"\n   ON t.거래처코드 = t2.거래처코드                                           ");
            qry.Append($"\n LEFT OUTER JOIN (                                                  ");
            qry.Append($"\n    SELECT                                                          ");
            qry.Append($"\n      t.거래처코드                                                      ");
            qry.Append($"\n    , t.거래처명                                                        ");
            qry.Append($"\n    , t.매출금액                                                        ");
            qry.Append($"\n    , t.손익금액                                                        ");
            qry.Append($"\n    , t.미수잔액                                                        ");
            qry.Append($"\n    , t.사용자                                                         ");
            qry.Append($"\n    , t2.손익금액 AS 전손익금액                                             ");
            qry.Append($"\n    FROM(                                                           ");
            qry.Append($"\n	   	SELECT                                                         ");
            qry.Append($"\n	 	  거래처코드                                                       ");
            qry.Append($"\n	 	, 거래처명                                                         ");
            qry.Append($"\n	 	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                    ");
            qry.Append($"\n	 	, ISNULL(손익금액, 0) AS 손익금액                                     ");
            qry.Append($"\n	 	, ISNULL(미수잔액, 0) AS 미수잔액                                     ");
            qry.Append($"\n	 	, 사용자                                                          ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n	 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n	 	   AND 거래처코드 ='{company_code}'                                       ");
            qry.Append($"\n	       AND 사용자 = (                                                 ");
            qry.Append($"\n	       	SELECT                                                     ");
            qry.Append($"\n		 	  MAX(사용자) - 1                                             ");
            qry.Append($"\n		 	FROM 업체별월별매출현황 AS b                                       ");
            qry.Append($"\n		 	WHERE 거래처코드 ='{company_code}'                                    ");
            qry.Append($"\n		 	  AND ISNULL(매출금액, 0) > 0                                  ");
            qry.Append($"\n		 	  AND 사용자 < {DateTime.Now.ToString("yyyyMM")}                                         ");
            qry.Append($"\n	       )                                                           ");
            qry.Append($"\n	   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n   	) AS t                                                         ");
            qry.Append($"\n   		LEFT OUTER JOIN (                                          ");
            qry.Append($"\n		SELECT                                                         ");
            qry.Append($"\n	 	  거래처코드                                                       ");
            qry.Append($"\n	 	, SUM(ISNULL(손익금액, 0)) AS 손익금액                                ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n	 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n	 	   AND 거래처코드 ='{company_code}'                                       ");
            qry.Append($"\n	       AND 사용자 < (                                                 ");
            qry.Append($"\n	       	SELECT                                                     ");
            qry.Append($"\n		 	  MAX(사용자) - 1                                             ");
            qry.Append($"\n		 	FROM 업체별월별매출현황 AS b                                       ");
            qry.Append($"\n		 	WHERE 거래처코드 ='{company_code}'                                    ");
            qry.Append($"\n		 	  AND ISNULL(매출금액, 0) > 0                                  ");
            qry.Append($"\n		 	  AND 사용자 < {DateTime.Now.ToString("yyyyMM")}                                         ");
            qry.Append($"\n	       )                                                           ");
            qry.Append($"\n	   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n	   	 GROUP BY 거래처코드                                               ");
            qry.Append($"\n   	) AS t2                                                        ");
            qry.Append($"\n   	  ON t.거래처코드 = t2.거래처코드                                       ");
            qry.Append($"\n ) AS t3                                                            ");
            qry.Append($"\n   ON t.거래처코드 = t3.거래처코드                                           ");
            qry.Append($"\n LEFT OUTER JOIN (                                                  ");
            qry.Append($"\n    SELECT                                                          ");
            qry.Append($"\n      t.거래처코드                                                      ");
            qry.Append($"\n    , t.거래처명                                                        ");
            qry.Append($"\n    , t.매출금액                                                        ");
            qry.Append($"\n    , t.손익금액                                                        ");
            qry.Append($"\n    , t.미수잔액                                                        ");
            qry.Append($"\n    , t.사용자                                                         ");
            qry.Append($"\n    , t2.손익금액 AS 전손익금액                                             ");
            qry.Append($"\n    FROM(                                                           ");
            qry.Append($"\n	   	SELECT                                                         ");
            qry.Append($"\n	 	  거래처코드                                                       ");
            qry.Append($"\n	 	, 거래처명                                                         ");
            qry.Append($"\n	 	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                    ");
            qry.Append($"\n	 	, ISNULL(손익금액, 0) AS 손익금액                                     ");
            qry.Append($"\n	 	, ISNULL(미수잔액, 0) AS 미수잔액                                     ");
            qry.Append($"\n	 	, 사용자                                                          ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n	 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n	 	   AND 거래처코드 ='{company_code}'                                       ");
            qry.Append($"\n	       AND 사용자 = (                                                 ");
            qry.Append($"\n	       	SELECT                                                     ");
            qry.Append($"\n		 	  MAX(사용자) - 2                                             ");
            qry.Append($"\n		 	FROM 업체별월별매출현황 AS b                                       ");
            qry.Append($"\n		 	WHERE 거래처코드 ='{company_code}'                                    ");
            qry.Append($"\n		 	  AND ISNULL(매출금액, 0) > 0                                  ");
            qry.Append($"\n		 	  AND 사용자 < {DateTime.Now.ToString("yyyyMM")}                                         ");
            qry.Append($"\n	       )                                                           ");
            qry.Append($"\n	   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n   	) AS t                                                         ");
            qry.Append($"\n   		LEFT OUTER JOIN (                                          ");
            qry.Append($"\n		SELECT                                                         ");
            qry.Append($"\n	 	  거래처코드                                                       ");
            qry.Append($"\n	 	, SUM(ISNULL(손익금액, 0)) AS 손익금액                                ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                                                ");
            qry.Append($"\n	 	WHERE 사용자 <> 999999                                            ");
            qry.Append($"\n	 	   AND 거래처코드 ='{company_code}'                                       ");
            qry.Append($"\n	       AND 사용자 < (                                                 ");
            qry.Append($"\n	       	SELECT                                                     ");
            qry.Append($"\n		 	  MAX(사용자) - 2                                             ");
            qry.Append($"\n		 	FROM 업체별월별매출현황 AS b                                       ");
            qry.Append($"\n		 	WHERE 거래처코드 ='{company_code}'                                    ");
            qry.Append($"\n		 	  AND ISNULL(매출금액, 0) > 0                                  ");
            qry.Append($"\n		 	  AND 사용자 < {DateTime.Now.ToString("yyyyMM")}                                         ");
            qry.Append($"\n	       )                                                           ");
            qry.Append($"\n	   	  AND 거래처코드 ='{company_code}'                                        ");
            qry.Append($"\n	   	 GROUP BY 거래처코드                                               ");
            qry.Append($"\n   	) AS t2                                                        ");
            qry.Append($"\n   	  ON t.거래처코드 = t2.거래처코드                                       ");
            qry.Append($"\n ) AS t4                                                            ");
            qry.Append($"\n   ON t.거래처코드 = t4.거래처코드                                           ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCompanyRecovery(DateTime sttdate, DateTime enddate, string company, string manager, bool isExistBalance, bool isLitigation)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n WITH sales_table AS (                                                           ");
            qry.Append($"\n 	SELECT                                                                      ");
            qry.Append($"\n 	   매출처코드                                                                   ");
            qry.Append($"\n 	 , 매출일자                                                                    ");
            qry.Append($"\n  	 , ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                              ");
            qry.Append($"\n 	 , ISNULL(매출금액, 0) - ISNULL(매입금액, 0) AS 손익금액                             ");
            qry.Append($"\n 	FROM 매출현황                                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                                       ");
            
            qry.Append($"\n )                                                                               ");
            qry.Append($"\n SELECT                                                                          ");
            qry.Append($"\n   t.*                                                                           ");
            qry.Append($"\n , ISNULL(p.매출금액 , 0) AS 전월누적매출                                                ");
            qry.Append($"\n , ISNULL(p.손익금액 , 0) AS 전월누적마진                                                ");
            qry.Append($"\n , ISNULL(t0.매출금액  , 0)    AS 매출금액0                                            ");
            qry.Append($"\n , ISNULL(t0.손익금액  , 0)      AS 마진금액0                                          ");
            qry.Append($"\n , ISNULL(t1.매출금액  , 0)    AS 매출금액1                                            ");
            qry.Append($"\n , ISNULL(t1.손익금액  , 0)      AS 마진금액1                                          ");
            qry.Append($"\n , ISNULL(t2.매출금액  , 0)    AS 매출금액2                                            ");
            qry.Append($"\n , ISNULL(t2.손익금액  , 0)      AS 마진금액2                                          ");
            qry.Append($"\n , ISNULL(t3.매출금액  , 0)    AS 매출금액3                                            ");
            qry.Append($"\n , ISNULL(t3.손익금액  , 0)      AS 마진금액3                                          ");
            qry.Append($"\n FROM (                                                                          ");
            qry.Append($"\n  	SELECT                                                                      ");
            qry.Append($"\n  	  거래처코드                                                                    ");
            qry.Append($"\n  	, 거래처명                                                                     ");
            qry.Append($"\n  	, ISNULL(매출한도금액, 0) AS 매출한도금액                                         "); 
            qry.Append($"\n  	, 폐업유무                                                                     ");
            qry.Append($"\n  	, 매출자                                                                      ");
            qry.Append($"\n  	, 최종매출일자                                                                   ");
            qry.Append($"\n  	FROM 업체별월별매출현황                                                            ");
            qry.Append($"\n 	WHERE 사용자 = {DateTime.Now.ToString("yyyyMM")}                                                         ");
            qry.Append($"\n 	  AND 폐업유무 = 'N'                                                           ");
            
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            if (isExistBalance)
                qry.Append($"\n 	  AND ISNULL(미수잔액, 0) > 0                                                  ");
            /*if(isLitigation)
                qry.Append($"\n 	  AND 거래처명 NOT LIKE '%소송%' AND 거래처명 NOT LIKE '%(S)%'                                                          ");*/
            qry.Append($"\n ) AS t                                                                          ");
            qry.Append($"\n LEFT OUTER JOIN (                                                               ");
            qry.Append($"\n 	SELECT                                                                      ");
            qry.Append($"\n 	   매출처코드                                                                   ");
            qry.Append($"\n  	 , SUM(매출금액) AS 매출금액                                                      ");
            qry.Append($"\n 	 , SUM(손익금액) AS 손익금액                                                      ");
            qry.Append($"\n 	FROM sales_table                                                            ");
            qry.Append($"\n 	WHERE 1=1                                                                   ");
            qry.Append($"\n   	  AND 매출일자 >= '2016-01-01'  AND 매출일자 < '{sttdate.ToString("yyyy-MM-dd")}'                       ");
            qry.Append($"\n   	GROUP BY 매출처코드  	                                                        ");
            qry.Append($"\n ) AS p                                                                          ");
            qry.Append($"\n   ON t.거래처코드 = p.매출처코드                                                        ");
            qry.Append($"\n LEFT OUTER JOIN (                                                               ");
            qry.Append($"\n 	SELECT                                                                      ");
            qry.Append($"\n 	   매출처코드                                                                   ");
            qry.Append($"\n  	 , SUM(매출금액) AS 매출금액                                                      ");
            qry.Append($"\n 	 , SUM(손익금액) AS 손익금액                                                      ");
            qry.Append($"\n 	FROM sales_table                                                            ");
            qry.Append($"\n 	WHERE 1=1                                                                   ");
            qry.Append($"\n   	  AND 매출일자 >= '{sttdate.ToString("yyyy-MM-dd")}'  AND 매출일자 >= '{enddate.ToString("yyyy-MM-dd")}'                      ");
            qry.Append($"\n   	GROUP BY 매출처코드                                                             ");
            qry.Append($"\n ) AS t0                                                                         ");
            qry.Append($"\n   ON t.거래처코드 = t0.매출처코드                                                       ");
            qry.Append($"\n LEFT OUTER JOIN (                                                               ");
            qry.Append($"\n 	SELECT                                                                      ");
            qry.Append($"\n 	   매출처코드                                                                   ");
            qry.Append($"\n  	 , SUM(매출금액) AS 매출금액                                                      ");
            qry.Append($"\n 	 , SUM(손익금액) AS 손익금액                                                      ");
            qry.Append($"\n 	FROM sales_table                                                            ");
            qry.Append($"\n 	WHERE 1=1                                                                   ");
            qry.Append($"\n   	  AND 매출일자 >= '{sttdate.AddMonths(-1).ToString("yyyy-MM-dd")}'  AND 매출일자 >= '{enddate.AddMonths(-1).ToString("yyyy-MM-dd")}'                      ");
            qry.Append($"\n   	GROUP BY 매출처코드                                                             ");
            qry.Append($"\n ) AS t1                                                                         ");
            qry.Append($"\n   ON t.거래처코드 = t1.매출처코드                                                       ");
            qry.Append($"\n LEFT OUTER JOIN (                                                               ");
            qry.Append($"\n 	SELECT                                                                      ");
            qry.Append($"\n 	   매출처코드                                                                   ");
            qry.Append($"\n  	 , SUM(매출금액) AS 매출금액                                                      ");
            qry.Append($"\n 	 , SUM(손익금액) AS 손익금액                                                      ");
            qry.Append($"\n 	FROM sales_table                                                            ");
            qry.Append($"\n 	WHERE 1=1                                                                   ");
            qry.Append($"\n   	  AND 매출일자 >= '{sttdate.AddMonths(-2).ToString("yyyy-MM-dd")}'  AND 매출일자 >= '{enddate.AddMonths(-2).ToString("yyyy-MM-dd")}'                      ");
            qry.Append($"\n   	GROUP BY 매출처코드                                                             ");
            qry.Append($"\n ) AS t2                                                                         ");
            qry.Append($"\n   ON t.거래처코드 = t2.매출처코드                                                       ");
            qry.Append($"\n LEFT OUTER JOIN (                                                               ");
            qry.Append($"\n 	SELECT                                                                      ");
            qry.Append($"\n 	   매출처코드                                                                   ");
            qry.Append($"\n  	 , SUM(매출금액) AS 매출금액                                                      ");
            qry.Append($"\n 	 , SUM(손익금액) AS 손익금액                                                      ");
            qry.Append($"\n 	FROM sales_table                                                            ");
            qry.Append($"\n 	WHERE 1=1                                                                   ");
            qry.Append($"\n   	  AND 매출일자 >= '{sttdate.AddMonths(-3).ToString("yyyy-MM-dd")}'  AND 매출일자 >= '{enddate.AddMonths(-3).ToString("yyyy-MM-dd")}'                      ");
            qry.Append($"\n   	GROUP BY 매출처코드                                                             ");
            qry.Append($"\n ) AS t3                                                                         ");
            qry.Append($"\n   ON t.거래처코드 = t3.매출처코드                                                       ");
            qry.Append($"\n ORDER BY t.거래처명, t.매출자, t.최종매출일자                                             ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;

        }
        public DataTable GetCompanyBalance(DateTime sttdate, DateTime enddate)
        {
            DateTime sttMonthFirstDate = new DateTime(enddate.AddMonths(-2).Year, enddate.AddMonths(-2).Month, 1);

            StringBuilder qry = new StringBuilder();
            qry.Append($"\n   	WITH dates_table AS (                                                                          ");
            qry.Append($"\n   	    SELECT DATEADD(DAY, number, '{sttMonthFirstDate.ToString("yyyy-MM-dd")}') AS date_field                                    ");
            qry.Append($"\n   	    FROM master..spt_values                                                                    ");
            qry.Append($"\n   	    WHERE type = 'P'                                                                           ");
            qry.Append($"\n   	    AND DATEADD(DAY, number, '{sttMonthFirstDate.ToString("yyyy-MM-dd")}') <= '{enddate.ToString("yyyy-MM-dd")}'          ");
            qry.Append($"\n   	), trading_partners AS (                                                                       ");
            qry.Append($"\n   		SELECT                                               ");
            qry.Append($"\n          t.trading_partner_code                              ");
            qry.Append($"\n   		, ISNULL(t2.전월미수잔액, 0) AS 전월미수잔액                  ");
            qry.Append($"\n   		FROM(                                                ");
            qry.Append($"\n	 		SELECT DISTINCT 거래처코드 AS trading_partner_code       ");
            qry.Append($"\n	   		FROM 업체별월별매출현황                                     ");
            //qry.Append($"\n	   		WHERE 거래처코드 = '0002655'                             ");
            qry.Append($"\n   		) AS t                                               ");
            qry.Append($"\n   		LEFT OUTER JOIN (                                    ");
            qry.Append($"\n   			SELECT                                           ");
            qry.Append($"\n	   		  거래처코드                                             ");
            qry.Append($"\n	   		, ISNULL(미수잔액, 0) AS 전월미수잔액                        ");
            qry.Append($"\n	   		FROM 업체별월별매출현황                                     ");
            qry.Append($"\n	   		WHERE 사용자 = '{sttMonthFirstDate.AddMonths(-1).ToString("yyyyMM")}'                                ");
            qry.Append($"\n   		) AS t2                                              ");
            qry.Append($"\n   		  ON t.trading_partner_code = t2.거래처코드              ");
            qry.Append($"\n   	), sales_table AS (                                                                            ");
            qry.Append($"\n   		SELECT                                                                                     ");
            qry.Append($"\n   		  매출처코드                                                                                   ");
            qry.Append($"\n   		, 매출일자                                                                                    ");
            qry.Append($"\n   		, 매출처                                                                                     ");
            qry.Append($"\n   		, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                                               ");
            qry.Append($"\n   		FROM 매출현황                                                                                 ");
            qry.Append($"\n   		WHERE 1=1                                                                                  ");
            qry.Append($"\n   		  AND 사용자 = '200009'                                                                      ");
            qry.Append($"\n   		  AND 매출일자 >= '{sttMonthFirstDate.ToString("yyyy-MM-dd")}'                                                                ");
            qry.Append($"\n   	), payments_table AS (                                                                         ");
            qry.Append($"\n   		SELECT                                                                                     ");
            qry.Append($"\n   		  거래처코드                                                                                   ");
            qry.Append($"\n   		, 입금일자                                                                                    ");
            qry.Append($"\n   		, 거래처명                                                                                    ");
            qry.Append($"\n   		, ISNULL(입금금액, 0) AS 입금금액                                                                ");
            qry.Append($"\n   		FROM 거래처별결제현황                                                                            ");
            qry.Append($"\n   		WHERE 1=1                                                                                  ");
            qry.Append($"\n   		  AND 사용자 = '200009'                                                                      ");
            qry.Append($"\n   		  AND 입금일자 >= '{sttMonthFirstDate.ToString("yyyy-MM-dd")}'                                                                ");
            qry.Append($"\n   	)                                                                                              ");
            qry.Append($"\n SELECT                                                                                         ");
            qry.Append($"\n    거래처코드                                                                                         ");
            qry.Append($"\n  , SUM(CASE WHEN 날짜 >= '{enddate.AddMonths(-1).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 매출 ELSE 0 END) AS 매출1           ");
            qry.Append($"\n  , SUM(CASE WHEN 날짜 >= '{enddate.AddMonths(-1).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 결제 ELSE 0 END) AS 결제1           ");
            qry.Append($"\n  , AVG(CASE WHEN 날짜 >= '{enddate.AddMonths(-1).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 잔액 ELSE null END) AS 잔액1        ");

            qry.Append($"\n  , SUM(CASE WHEN 날짜 >= '{enddate.AddDays(-45).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 매출 ELSE 0 END) AS 매출45           ");
            qry.Append($"\n  , SUM(CASE WHEN 날짜 >= '{enddate.AddDays(-45).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 결제 ELSE 0 END) AS 결제45          ");
            qry.Append($"\n  , AVG(CASE WHEN 날짜 >= '{enddate.AddDays(-45).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 잔액 ELSE null END) AS 잔액45        ");

            qry.Append($"\n  , SUM(CASE WHEN 날짜 >= '{enddate.AddMonths(-2).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 매출 ELSE 0 END) AS 매출2           ");
            qry.Append($"\n  , SUM(CASE WHEN 날짜 >= '{enddate.AddMonths(-2).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 결제 ELSE 0 END) AS 결제2           ");
            qry.Append($"\n  , AVG(CASE WHEN 날짜 >= '{enddate.AddMonths(-2).ToString("yyyy-MM-dd")}' AND 날짜 <= '{enddate.ToString("yyyy-MM-dd")}' THEN 잔액 ELSE null END) AS 잔액2        ");

            qry.Append($"\n FROM (                                                                                         ");
            qry.Append($"\n 	SELECT                                                                                         ");
            qry.Append($"\n     dates_table.date_field AS 날짜,                                                                  ");
            qry.Append($"\n     trading_partners.trading_partner_code AS 거래처코드,                                               ");
            qry.Append($"\n     ISNULL(trading_partners.전월미수잔액,0) AS 전월미수잔액,                                               ");
            qry.Append($"\n 	ISNULL((                                                                                       ");
            qry.Append($"\n     	SELECT SUM(매출금액)                                                                          ");
            qry.Append($"\n     	FROM sales_table                                                                           ");
            qry.Append($"\n     	WHERE sales_table.매출일자 = dates_table.date_field                                           ");
            qry.Append($"\n     	  AND sales_table.매출처코드 = trading_partners.trading_partner_code)                          ");
            qry.Append($"\n    	, 0) AS 매출,                                                                                    ");
            qry.Append($"\n    	ISNULL((                                                                                       ");
            qry.Append($"\n     	SELECT SUM(입금금액)                                                                          ");
            qry.Append($"\n     	FROM payments_table                                                                        ");
            qry.Append($"\n     	WHERE payments_table.입금일자 = dates_table.date_field                                        ");
            qry.Append($"\n     	  AND payments_table.거래처코드 = trading_partners.trading_partner_code)                       ");
            qry.Append($"\n    	, 0) AS 결제,                                                                                    ");
            qry.Append($"\n     ISNULL((                                                                                       ");
            qry.Append($"\n     	SELECT SUM(매출금액)                                                                          ");
            qry.Append($"\n     	FROM sales_table                                                                           ");
            qry.Append($"\n     	WHERE 1=1                                                                                  ");
            qry.Append($"\n     	  AND sales_table.매출일자 <= dates_table.date_field                                          ");
            qry.Append($"\n     	  AND sales_table.매출처코드 = trading_partners.trading_partner_code)                          ");
            qry.Append($"\n    	, 0)                                                                                           ");
            qry.Append($"\n    	- ISNULL((                                                                                     ");
            qry.Append($"\n     	SELECT SUM(입금금액)                                                                          ");
            qry.Append($"\n     	FROM payments_table                                                                        ");
            qry.Append($"\n     	WHERE 1=1                                                                                  ");
            qry.Append($"\n     	  AND payments_table.입금일자 <= dates_table.date_field                                       ");
            qry.Append($"\n     	  AND payments_table.거래처코드 = trading_partners.trading_partner_code)                       ");
            qry.Append($"\n    	, 0)                                                                                     ");
            qry.Append($"\n    	+ ISNULL(trading_partners.전월미수잔액,0) AS 잔액                                              ");
            qry.Append($"\n FROM                                                                                               ");
            qry.Append($"\n     dates_table                                                                                    ");
            qry.Append($"\n CROSS JOIN                                                                                         ");
            qry.Append($"\n     trading_partners                                                                               ");
            qry.Append($"\n ) AS t                                                                               ");
            qry.Append($"\n WHERE 1=1                                                                                         ");
            qry.Append($"\n GROUP BY 거래처코드                                                                               ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCompanyList(DateTime now_date, string company, string manager, double capital1, double capital2, double capital3, double capital4, double profit, int division)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                ");
            qry.Append($"\n   t1.거래처코드                                                                         ");
            qry.Append($"\n , t1.거래처명                                                                         ");
            qry.Append($"\n , ISNULL(t1.매출금액, 0) AS 매출금액                                                  ");
            qry.Append($"\n , ISNULL(t1.미수잔액, 0) AS 미수잔액                                                  ");
            qry.Append($"\n , ISNULL(t1.월평균잔액, 0) AS 월평균잔액                                              ");
            qry.Append($"\n , ISNULL(t1.손익금액, 0) AS 한달마진                                                  ");
            qry.Append($"\n , ISNULL(t2.전월누적마진, 0) + ISNULL(t1.손익금액, 0) AS 누적마진                     ");
            qry.Append($"\n , ISNULL(t2.전월누적마진, 0) AS 전월누적마진                                          ");

            qry.Append($"\n , ISNULL(t2.전월누적미수잔액, 0) AS 전월누적미수잔액                                  ");
            qry.Append($"\n , ISNULL(t2.전월누적월월평균잔액, 0) AS 전월누적월월평균잔액                          ");

            qry.Append($"\n , ISNULL(t3.순영업자본, 0) AS 순영업자본                                              ");
            qry.Append($"\n , ISNULL(t3.영업자본, 0) AS 영업자본                                                  ");
            qry.Append($"\n , ISNULL(t3.아토영업, 0) AS 아토영업                                                  ");
            qry.Append($"\n , ISNULL(t3.자기자본, 0) AS 자기자본                                                  ");
            qry.Append($"\n , ISNULL(t4.미수잔액, 0) AS 한달전미수잔액                                            ");
            qry.Append($"\n , ISNULL(t4.월평균잔액, 0) AS 한달전월평균잔액                                        ");
            qry.Append($"\n , ISNULL(t5.미수잔액, 0) AS 두달전미수잔액                                            ");
            qry.Append($"\n , ISNULL(t5.월평균잔액, 0) AS 두달전월평균잔액                                        ");
            qry.Append($"\n , ISNULL(t6.미수잔액, 0) AS 세달전미수잔액                                            ");
            qry.Append($"\n , ISNULL(t6.월평균잔액, 0) AS 세달전월평균잔액                                        ");
            qry.Append($"\n , ISNULL(t6.월평균잔액, 0) AS 세달전월평균잔액                                        ");
            qry.Append($"\n , ISNULL(t7.매출금액, 0) AS 한달전매출금액          ");
            qry.Append($"\n , ISNULL(t7.손익금액, 0) AS 한달전손익금액          ");
            qry.Append($"\n , ISNULL(t8.매출금액, 0) AS 두달전매출금액          ");
            qry.Append($"\n , ISNULL(t8.손익금액, 0) AS 두달전손익금액          ");
            qry.Append($"\n , ISNULL(t9.매출금액, 0) AS 세달전매출금액          ");
            qry.Append($"\n , ISNULL(t9.손익금액, 0) AS 세달전손익금액          ");
            qry.Append($"\n , t1.매출자                                                                           ");
            qry.Append($"\n , t1.최종매출일자                                                                     ");
            qry.Append($"\n FROM(                                                                                 ");
            if (division == 2)
            {
                qry.Append($"\n 	SELECT                                                                            ");
                qry.Append($"\n 	  거래처코드                                                                      ");
                qry.Append($"\n 	, 거래처명                                                                        ");
                qry.Append($"\n 	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                             ");
                qry.Append($"\n 	, ISNULL(손익금액, 0) AS 손익금액                                                 ");
                qry.Append($"\n 	, ISNULL(미수잔액, 0) AS 미수잔액                                                 ");
                qry.Append($"\n 	, ISNULL(월평균잔액, 0) AS 월평균잔액                                             ");
                qry.Append($"\n 	, 매출자                                                                          ");
                qry.Append($"\n 	, 최종매출일자                                                                    ");
                qry.Append($"\n 	FROM 업체별월별매출현황                                                           ");
                qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
                qry.Append($"\n 	  AND 사용자 = {now_date.ToString("yyyyMM")}                                  ");
                if (!string.IsNullOrEmpty(company))
                    qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
                if (!string.IsNullOrEmpty(manager))
                    qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
                 qry.Append($"\n 	  AND ISNULL(미수잔액, 0) > 0                                                 ");
            }
            else
            {
                qry.Append($"\n	SELECT                                                         ");
                qry.Append($"\n	  t1.거래처코드                                                    ");
                qry.Append($"\n	, t1.거래처명                                                      ");
                qry.Append($"\n	, ISNULL(t2.매출금액, 0) + ISNULL(t2.부가세, 0) AS 매출금액              ");
                qry.Append($"\n 	, ISNULL(t2.손익금액, 0) AS 손익금액                              ");
                qry.Append($"\n 	, ISNULL(t2.미수잔액, 0) AS 미수잔액                              ");
                qry.Append($"\n 	, ISNULL(t2.월평균잔액, 0) AS 월평균잔액                            ");
                qry.Append($"\n 	, t3.매출자                                                   ");
                qry.Append($"\n 	, t3.최종매출일자                                               ");
                qry.Append($"\n	FROM(                                                          ");
                qry.Append($"\n		SELECT                                                     ");
                qry.Append($"\n	 	  거래처코드                                                   ");
                qry.Append($"\n	 	, 거래처명                                                     ");
                qry.Append($"\n	 	FROM 업체별월별매출현황                                            ");
                qry.Append($"\n	 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}                     ");
                if (!string.IsNullOrEmpty(company))
                    qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
                if (!string.IsNullOrEmpty(manager))
                    qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
                qry.Append($"\n	 	GROUP BY 거래처코드, 거래처명                                      ");
                qry.Append($"\n 	) AS t1                                                    ");
                qry.Append($"\n 	LEFT OUTER JOIN (                                          ");
                qry.Append($"\n	 	SELECT                                                     ");
                qry.Append($"\n	 	  *                                                        ");
                qry.Append($"\n	 	FROM 업체별월별매출현황                                            ");
                qry.Append($"\n	 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}                     ");
                qry.Append($"\n	 	  AND 사용자 = {now_date.ToString("yyyyMM")}                                         ");
                qry.Append($"\n 	) AS t2                                                    ");
                qry.Append($"\n	  ON t1.거래처코드 = t2.거래처코드                                      ");
                qry.Append($"\n	LEFT OUTER JOIN (                                              ");
                qry.Append($"\n	 	SELECT                                                     ");
                qry.Append($"\n	 	  거래처코드                                                   ");
                qry.Append($"\n	 	, 매출자                                                      ");
                qry.Append($"\n	 	, 최종매출일자                                                  ");
                qry.Append($"\n	 	FROM 업체별월별매출현황 AS a                                       ");
                qry.Append($"\n	 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}                     ");
                qry.Append($"\n	 	  AND 최종매출일자 = (                                          ");
                qry.Append($"\n	 	  	SELECT MAX(최종매출일자) FROM 업체별월별매출현황 AS b               ");
                qry.Append($"\n	 	  	WHERE b.사용자 >= 201601 AND b.사용자 <= {now_date.ToString("yyyyMM")}             ");
                qry.Append($"\n	 	  	  AND b.거래처코드 = a.거래처코드                               ");
                qry.Append($"\n	 	  	GROUP BY 거래처코드                                        ");
                qry.Append($"\n	 	 ) GROUP BY   거래처코드, 매출자	, 최종매출일자                      ");
                qry.Append($"\n 	) AS t3                                                    ");
                qry.Append($"\n	  ON t1.거래처코드 = t3.거래처코드                                      ");
            }
            qry.Append($"\n ) AS t1                                                                               ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                     ");
            qry.Append($"\n 	SELECT                                                                            ");
            qry.Append($"\n 	  거래처코드                                                                      ");
            qry.Append($"\n 	, SUM(ISNULL(손익금액, 0)) AS 전월누적마진                                        ");
            qry.Append($"\n 	, SUM(ISNULL(미수잔액, 0)) AS 전월누적미수잔액                                    ");
            qry.Append($"\n 	, SUM(ISNULL(월평균잔액, 0)) AS 전월누적월월평균잔액                              ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                           ");
            qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
            qry.Append($"\n 	  AND 사용자 <= {now_date.AddMonths(-1).ToString("yyyyMM")}                   ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n 	GROUP BY 거래처코드                                                               ");
            qry.Append($"\n ) AS t2                                                                               ");
            qry.Append($"\n   ON t1.거래처코드 = t2.거래처코드                                                    ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                     ");
            qry.Append($"\n 	SELECT                                                                            ");
            qry.Append($"\n 	  거래처코드                                                                      ");
            qry.Append($"\n 	, SUM(ISNULL(월평균잔액, 0) * {capital1} * {profit} / 12) AS 순영업자본                   ");
            qry.Append($"\n 	, SUM(ISNULL(월평균잔액, 0) * {capital2} * {profit} / 12) AS 영업자본                     ");
            qry.Append($"\n 	, SUM(ISNULL(월평균잔액, 0) * {capital3} * 3) AS 아토영업                              ");
            qry.Append($"\n 	, SUM(ISNULL(월평균잔액, 0) * {capital4} * {profit} / 12) AS 자기자본                     ");
            qry.Append($"\n 	FROM 업체별월별매출현황 AS a                                                      ");
            qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
            qry.Append($"\n 	  AND 사용자 >= (SELECT MIN(사용자) FROM 업체별월별매출현황 AS b                  ");
            qry.Append($"\n 	 				WHERE 월 IS NOT NULL                                              ");
            qry.Append($"\n 	 				  AND 사용자 >= 201601                                            ");
            qry.Append($"\n 	 				  AND 사용자 <= {now_date.ToString("yyyyMM")}                 ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n 	 				  AND a.거래처명 = b.거래처명)                                    ");
            qry.Append($"\n 	  AND 사용자 <= {now_date.ToString("yyyyMM")}                                 ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n 	GROUP BY 거래처코드                                                               ");
            qry.Append($"\n ) AS t3                                                                               ");
            qry.Append($"\n   ON t1.거래처코드 = t3.거래처코드                                                    ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                     ");
            qry.Append($"\n 	SELECT                                                                            ");
            qry.Append($"\n 	  거래처코드                                                                      ");
            qry.Append($"\n 	, ISNULL(미수잔액, 0) AS 미수잔액                                                 ");
            qry.Append($"\n 	, ISNULL(월평균잔액, 0) AS 월평균잔액                                             ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                           ");
            qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
            qry.Append($"\n 	  AND 사용자 = {now_date.AddMonths(-1).ToString("yyyyMM")}                    ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n  	GROUP BY 거래처코드, ISNULL(미수잔액, 0)	, ISNULL(월평균잔액, 0)               ");
            qry.Append($"\n ) AS t4                                                                               ");
            qry.Append($"\n   ON t1.거래처코드 = t4.거래처코드                                                    ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                     ");
            qry.Append($"\n 	SELECT                                                                            ");
            qry.Append($"\n 	  거래처코드                                                                      ");
            qry.Append($"\n 	, ISNULL(미수잔액, 0) AS 미수잔액                                                 ");
            qry.Append($"\n 	, ISNULL(월평균잔액, 0) AS 월평균잔액                                             ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                           ");
            qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
            qry.Append($"\n 	  AND 사용자 = {now_date.AddMonths(-2).ToString("yyyyMM")}                    ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n  	GROUP BY 거래처코드, ISNULL(미수잔액, 0)	, ISNULL(월평균잔액, 0)               ");
            qry.Append($"\n ) AS t5                                                                               ");
            qry.Append($"\n   ON t1.거래처코드 = t5.거래처코드                                                    ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                     ");
            qry.Append($"\n 	SELECT                                                                            ");
            qry.Append($"\n 	  거래처코드                                                                      ");
            qry.Append($"\n 	, ISNULL(미수잔액, 0) AS 미수잔액                                                 ");
            qry.Append($"\n 	, ISNULL(월평균잔액, 0) AS 월평균잔액                                             ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                           ");
            qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
            qry.Append($"\n 	  AND 사용자 = {now_date.AddMonths(-3).ToString("yyyyMM")}                    ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n  	GROUP BY 거래처코드, ISNULL(미수잔액, 0)	, ISNULL(월평균잔액, 0)               ");
            qry.Append($"\n ) AS t6                                                                               ");
            qry.Append($"\n   ON t1.거래처코드 = t6.거래처코드                                                    ");

            qry.Append($"\n  LEFT OUTER JOIN (                                                                     ");
            qry.Append($"\n 	SELECT                                                                             ");
            qry.Append($"\n 	  거래처코드                                                                           ");
            qry.Append($"\n 	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                                        ");
            qry.Append($"\n 	, ISNULL(손익금액, 0) AS 손익금액                                                         ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                                    ");
            qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
            qry.Append($"\n 	  AND 사용자 = {now_date.AddMonths(-1).ToString("yyyyMM")}                    ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n 	  AND NOT (ISNULL(매출금액, 0) + ISNULL(부가세, 0) = 0 AND ISNULL(손익금액, 0) = 0 )         ");
            qry.Append($"\n  	GROUP BY 거래처코드, ISNULL(매출금액, 0) + ISNULL(부가세, 0)	, ISNULL(손익금액, 0)              ");
            qry.Append($"\n ) AS t7                                                                                ");
            qry.Append($"\n   ON t1.거래처코드 = t7.거래처코드                                                              ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                      ");
            qry.Append($"\n 	SELECT                                                                             ");
            qry.Append($"\n 	  거래처코드                                                                           ");
            qry.Append($"\n 	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                                        ");
            qry.Append($"\n 	, ISNULL(손익금액, 0) AS 손익금액                                                         ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                                    ");
            qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
            qry.Append($"\n 	  AND 사용자 = {now_date.AddMonths(-2).ToString("yyyyMM")}                    ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n 	  AND NOT (ISNULL(매출금액, 0) + ISNULL(부가세, 0) = 0 AND ISNULL(손익금액, 0) = 0 )         ");
            qry.Append($"\n  	GROUP BY 거래처코드, ISNULL(매출금액, 0) + ISNULL(부가세, 0)	, ISNULL(손익금액, 0)              ");
            qry.Append($"\n ) AS t8                                                                                ");
            qry.Append($"\n   ON t1.거래처코드 = t8.거래처코드                                                              ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                      ");
            qry.Append($"\n 	SELECT                                                                             ");
            qry.Append($"\n 	  거래처코드                                                                           ");
            qry.Append($"\n 	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                                        ");
            qry.Append($"\n 	, ISNULL(손익금액, 0) AS 손익금액                                                         ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                                    ");
            qry.Append($"\n 	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}            ");
            qry.Append($"\n 	  AND 사용자 = {now_date.AddMonths(-3).ToString("yyyyMM")}                    ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
            qry.Append($"\n 	  AND NOT (ISNULL(매출금액, 0) + ISNULL(부가세, 0) = 0 AND ISNULL(손익금액, 0) = 0 )         ");
            qry.Append($"\n  	GROUP BY 거래처코드, ISNULL(매출금액, 0) + ISNULL(부가세, 0)	, ISNULL(손익금액, 0)              ");
            qry.Append($"\n ) AS t9                                                                                ");
            qry.Append($"\n   ON t1.거래처코드 = t9.거래처코드                                                              ");

            qry.Append($"\n ORDER BY t1.거래처명, t1.매출자, t1.최종매출일자                                           ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCompanyList2(DateTime now_date, string company, string manager, double capital1, double capital2, double capital3, double capital4, double profit, int division, bool out_business = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                              ");
            qry.Append($"\n   t.*                                                               ");
            qry.Append($"\n , ISNULL(p.전월누적매출 , 0) AS 전월누적매출                                           ");
            qry.Append($"\n , ISNULL(p.전월누적마진 , 0) AS 전월누적마진                                           ");
            qry.Append($"\n , ISNULL(p.전월누적미수 , 0) AS 전월누적미수                                           ");
            qry.Append($"\n , ISNULL(p.전월누적평잔 , 0) AS 전월누적평잔                                           ");
            qry.Append($"\n , ISNULL(t1.매출금액  , 0)    AS 매출금액1                                 ");
            qry.Append($"\n , ISNULL(t1.미수잔액  , 0)    AS 미수잔액1                                 ");
            qry.Append($"\n , ISNULL(t1.월평균잔액 , 0)     AS 월평균잔액1                               ");
            qry.Append($"\n , ISNULL(t1.손익금액  , 0)      AS 마진금액1                               ");
            qry.Append($"\n , ISNULL(t2.매출금액  , 0)    AS 매출금액2                                 ");
            qry.Append($"\n , ISNULL(t2.미수잔액  , 0)    AS 미수잔액2                                 ");
            qry.Append($"\n , ISNULL(t2.월평균잔액 , 0)     AS 월평균잔액2                               ");
            qry.Append($"\n , ISNULL(t2.손익금액  , 0)      AS 마진금액2                               ");
            qry.Append($"\n , ISNULL(t3.매출금액  , 0)    AS 매출금액3                                 ");
            qry.Append($"\n , ISNULL(t3.미수잔액  , 0)    AS 미수잔액3                                 ");
            qry.Append($"\n , ISNULL(t3.월평균잔액 , 0)     AS 월평균잔액3                               ");
            qry.Append($"\n , ISNULL(t3.손익금액  , 0)      AS 마진금액3                               ");
            qry.Append($"\n , ISNULL(t4.매출금액  , 0)    AS 매출금액4                                 ");
            qry.Append($"\n , ISNULL(t4.미수잔액  , 0)    AS 미수잔액4                                 ");
            qry.Append($"\n , ISNULL(t4.월평균잔액 , 0)     AS 월평균잔액4                               ");
            qry.Append($"\n , ISNULL(t4.손익금액  , 0)      AS 마진금액4                               ");
            qry.Append($"\n , ISNULL(t5.매출금액  , 0)    AS 매출금액5                                 ");
            qry.Append($"\n , ISNULL(t5.미수잔액  , 0)    AS 미수잔액5                                 ");
            qry.Append($"\n , ISNULL(t5.월평균잔액 , 0)     AS 월평균잔액5                               ");
            qry.Append($"\n , ISNULL(t5.손익금액  , 0)      AS 마진금액5                               ");
            qry.Append($"\n , ISNULL(t6.매출금액  , 0)    AS 매출금액6                                 ");
            qry.Append($"\n , ISNULL(t6.미수잔액  , 0)    AS 미수잔액6                                 ");
            qry.Append($"\n , ISNULL(t6.월평균잔액 , 0)     AS 월평균잔액6                               ");
            qry.Append($"\n , ISNULL(t6.손익금액  , 0)      AS 마진금액6                               ");
            qry.Append($"\n , ISNULL(t7.매출금액  , 0)    AS 매출금액7                                 ");
            qry.Append($"\n , ISNULL(t7.미수잔액  , 0)    AS 미수잔액7                                 ");
            qry.Append($"\n , ISNULL(t7.월평균잔액 , 0)     AS 월평균잔액7                               ");
            qry.Append($"\n , ISNULL(t7.손익금액  , 0)      AS 마진금액7                               ");
            qry.Append($"\n , ISNULL(t8.매출금액  , 0)    AS 매출금액8                                 ");
            qry.Append($"\n , ISNULL(t8.미수잔액  , 0)    AS 미수잔액8                                 ");
            qry.Append($"\n , ISNULL(t8.월평균잔액 , 0)     AS 월평균잔액8                               ");
            qry.Append($"\n , ISNULL(t8.손익금액  , 0)      AS 마진금액8                               ");
            qry.Append($"\n , ISNULL(t9.매출금액  , 0)    AS 매출금액9                                 ");
            qry.Append($"\n , ISNULL(t9.미수잔액  , 0)    AS 미수잔액9                                 ");
            qry.Append($"\n , ISNULL(t9.월평균잔액 , 0)     AS 월평균잔액9                               ");
            qry.Append($"\n , ISNULL(t9.손익금액  , 0)      AS 마진금액9                               ");
            qry.Append($"\n , ISNULL(t10.매출금액 , 0)     AS 매출금액10                               ");
            qry.Append($"\n , ISNULL(t10.미수잔액 , 0)     AS 미수잔액10                               ");
            qry.Append($"\n , ISNULL(t10.월평균잔액, 0)      AS 월평균잔액10                             ");
            qry.Append($"\n , ISNULL(t10.손익금액 , 0)       AS 마진금액10                             ");
            qry.Append($"\n , ISNULL(t11.매출금액 , 0)     AS 매출금액11                               ");
            qry.Append($"\n , ISNULL(t11.미수잔액 , 0)     AS 미수잔액11                               ");
            qry.Append($"\n , ISNULL(t11.월평균잔액, 0)      AS 월평균잔액11                             ");
            qry.Append($"\n , ISNULL(t11.손익금액 , 0)       AS 마진금액11                             ");
            qry.Append($"\n , ISNULL(t12.매출금액 , 0)     AS 매출금액12                               ");
            qry.Append($"\n , ISNULL(t12.미수잔액 , 0)     AS 미수잔액12                               ");
            qry.Append($"\n , ISNULL(t12.월평균잔액, 0)      AS 월평균잔액12                             ");
            qry.Append($"\n , ISNULL(t12.손익금액 , 0)       AS 마진금액12                             ");
            qry.Append($"\n FROM (                                                              ");

            if (division == 2)
            {
                qry.Append($"\n  	SELECT                                                          ");
                qry.Append($"\n  	  거래처코드                                                        ");
                qry.Append($"\n  	, 거래처명                                                          ");
                qry.Append($"\n  	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 당월매출                     ");
                qry.Append($"\n  	, ISNULL(손익금액, 0) AS 당월마진                                      ");
                qry.Append($"\n  	, ISNULL(미수잔액, 0) AS 당월미수                                      ");
                qry.Append($"\n  	, ISNULL(월평균잔액, 0) AS 당월평잔                                     ");
                qry.Append($"\n  	, 폐업유무                                                           ");
                qry.Append($"\n  	, 매출자                                                           ");
                qry.Append($"\n  	, 최종매출일자                                                       ");
                qry.Append($"\n  	FROM 업체별월별매출현황                                                 ");
                qry.Append($"\n 	WHERE 사용자 = {now_date.ToString("yyyyMM")}                                  ");
                if (!string.IsNullOrEmpty(company))
                    qry.Append($"\n 	  AND 거래처명 LIKE '%{company}%'                                                 ");
                if (!string.IsNullOrEmpty(manager))
                    qry.Append($"\n 	  AND 매출자 LIKE '%{manager}%'                                                   ");
                qry.Append($"\n 	  AND ISNULL(미수잔액, 0) > 0                                                 ");
                if(!out_business)
                    qry.Append($"\n 	  AND 폐업유무 = 'N'                                                 ");
            }
            else
            {
                qry.Append($"\n		SELECT                                                         ");
                qry.Append($"\n		  t1.거래처코드                                                    ");
                qry.Append($"\n		, t1.거래처명                                                      ");
                qry.Append($"\n		, ISNULL(t2.매출금액, 0) + ISNULL(t2.부가세, 0) AS 당월매출              ");
                qry.Append($"\n 	, ISNULL(t2.손익금액, 0) AS 당월마진                              ");
                qry.Append($"\n 	, ISNULL(t2.미수잔액, 0) AS 당월미수                              ");
                qry.Append($"\n 	, ISNULL(t2.월평균잔액, 0) AS 당월평잔                            ");
                qry.Append($"\n 	, t3.폐업유무                                                   ");
                qry.Append($"\n 	, t3.매출자                                                   ");
                qry.Append($"\n 	, t3.최종매출일자                                               ");
                qry.Append($"\n		FROM(                                                          ");
                qry.Append($"\n			SELECT                                                     ");
                qry.Append($"\n	 		  거래처코드                                                   ");
                qry.Append($"\n	 		, 거래처명                                                     ");
                qry.Append($"\n	 		FROM 업체별월별매출현황                                            ");
                qry.Append($"\n	 		WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}                     ");
                if (!string.IsNullOrEmpty(company))
                    qry.Append($"\n 	  	AND 거래처명 LIKE '%{company}%'                                                 ");
                if (!string.IsNullOrEmpty(manager))
                    qry.Append($"\n 	  	AND 매출자 LIKE '%{manager}%'                                                   ");
                if (!out_business)
                    qry.Append($"\n 	  AND 폐업유무 = 'N'                                                 ");
                qry.Append($"\n	 		GROUP BY 거래처코드, 거래처명                                      ");
                qry.Append($"\n 	) AS t1                                                    ");
                qry.Append($"\n 	LEFT OUTER JOIN (                                          ");
                qry.Append($"\n	 		SELECT                                                     ");
                qry.Append($"\n	 		  *                                                        ");
                qry.Append($"\n	 		FROM 업체별월별매출현황                                            ");
                qry.Append($"\n	 		WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}                     ");
                qry.Append($"\n	 		  AND 사용자 = {now_date.ToString("yyyyMM")}                                         ");
                qry.Append($"\n 	) AS t2                                                    ");
                qry.Append($"\n	  		ON t1.거래처코드 = t2.거래처코드                                      ");
                qry.Append($"\n		LEFT OUTER JOIN (                                              ");
                qry.Append($"\n	 		SELECT                                                     ");
                qry.Append($"\n	 		  거래처코드                                                   ");
                qry.Append($"\n	 		, 매출자                                                      ");
                qry.Append($"\n	 		, 최종매출일자                                                  ");
                qry.Append($"\n	 		, 폐업유무                                                  ");
                qry.Append($"\n	 		FROM 업체별월별매출현황 AS a                                       ");
                qry.Append($"\n	 		WHERE 사용자 >= 201601 AND 사용자 <= {now_date.ToString("yyyyMM")}                     ");
                qry.Append($"\n	 		  AND 최종매출일자 = (                                          ");
                qry.Append($"\n	 		  	SELECT MAX(최종매출일자) FROM 업체별월별매출현황 AS b               ");
                qry.Append($"\n	 		  	WHERE b.사용자 >= 201601 AND b.사용자 <= {now_date.ToString("yyyyMM")}             ");
                qry.Append($"\n	 		  	  AND b.거래처코드 = a.거래처코드                               ");
                qry.Append($"\n	 		  	GROUP BY 거래처코드                                        ");
                qry.Append($"\n	 		) GROUP BY   거래처코드, 매출자	, 최종매출일자, 폐업유무                      ");
                qry.Append($"\n 	) AS t3                                                    ");
                qry.Append($"\n	  ON t1.거래처코드 = t3.거래처코드                                      ");
            }
            qry.Append($"\n ) AS t                                                              ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n  	SELECT                                                          ");
            qry.Append($"\n  	  거래처코드                                                        ");
            qry.Append($"\n  	, SUM(ISNULL(매출금액, 0)) AS 전월누적매출                               ");
            qry.Append($"\n  	, SUM(ISNULL(손익금액, 0)) AS 전월누적마진                               ");
            qry.Append($"\n  	, SUM(ISNULL(미수잔액, 0)) AS 전월누적미수                               ");
            qry.Append($"\n  	, SUM(ISNULL(월평균잔액, 0)) AS 전월누적평잔                              ");
            qry.Append($"\n  	FROM 업체별월별매출현황                                                 ");
            qry.Append($"\n  	WHERE 사용자 >= 201601 AND 사용자 <= {now_date.AddMonths(-1).ToString("yyyyMM")}                          ");
            qry.Append($"\n  	GROUP BY 거래처코드                                                 ");
            qry.Append($"\n ) AS p                                                              ");
            qry.Append($"\n   ON t.거래처코드 = p.거래처코드                                             ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-1).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t1                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t1.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-2).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t2                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t2.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-3).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t3                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t3.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-4).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t4                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t4.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-5).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t5                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t5.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-6).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t6                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t6.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-7).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t7                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t7.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-8).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t8                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t8.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-9).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t9                                                             ");
            qry.Append($"\n   ON t.거래처코드 = t9.거래처코드                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-10).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t10                                                            ");
            qry.Append($"\n   ON t.거래처코드 = t10.거래처코드                                           ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-11).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t11                                                            ");
            qry.Append($"\n   ON t.거래처코드 = t11.거래처코드                                           ");
            qry.Append($"\n LEFT OUTER JOIN (                                                   ");
            qry.Append($"\n 	SELECT * FROM 업체별월별매출현황 WHERE 사용자 = {now_date.AddMonths(-12).ToString("yyyyMM")}                     ");
            qry.Append($"\n ) AS t12                                                            ");
            qry.Append($"\n   ON t.거래처코드 = t12.거래처코드                                           ");
            qry.Append($"\n ORDER BY t.거래처명, t.매출자, t.최종매출일자                                           ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetRecoveryDetail(DateTime sdate, int accounts_receivable_terms, string company_code, string company)
        {
            DateTime enddate = sdate;
            DateTime sttdate = new DateTime(sdate.AddYears(-1).Year, sdate.AddYears(-1).Month, 1);
            if (accounts_receivable_terms == 1)
                sttdate = new DateTime(sdate.AddYears(-1).AddMonths(-1).Year, sdate.AddYears(-1).AddMonths(-1).Month, 1);
            else if (accounts_receivable_terms == 2)
                sttdate = new DateTime(sdate.AddYears(-1).AddMonths(-2).Year, sdate.AddYears(-1).AddMonths(-2).Month, 1);
            else if (accounts_receivable_terms == 3)
                sttdate = new DateTime(sdate.AddYears(-1).AddMonths(-3).Year, sdate.AddYears(-1).AddMonths(-3).Month, 1);
            //시작전달말 기준 미수금을 가져와 시작달 1일부터 잔금을 계산하고 sttdate ~ enddate 가지만 평잔을 구함
            DateTime preLastdate = sttdate.AddMonths(-1);

            StringBuilder qry = new StringBuilder();
            qry.Append($"\nWITH dates_table AS (                                                                          ");
            qry.Append($"\n    SELECT DATEADD(DAY, number, '{sttdate.ToString("yyyy-MM-dd")}') AS date_field                                    ");
            qry.Append($"\n    FROM master..spt_values                                                                    ");
            qry.Append($"\n    WHERE type = 'P'                                                                           ");
            qry.Append($"\n    AND DATEADD(DAY, number, '{sttdate.ToString("yyyy-MM-dd")}') <= '{enddate.ToString("yyyy-MM-dd")}'                                     ");
            qry.Append($"\n), trading_partners AS (                                                                       ");

            qry.Append($"\n	SELECT                                                                           ");
            qry.Append($"\n	  t.trading_partner_code                                                         ");
            qry.Append($"\n	, ISNULL(a.미수잔액, 0) AS 전월미수잔액                                                  ");
            qry.Append($"\n	, ISNULL(a.전월매출금액, 0) AS 전월매출금액                                               ");
            qry.Append($"\n	, ISNULL(a.전월마진금액, 0) AS 전월마진금액                                               ");
            qry.Append($"\n	FROM (                                                                           ");
            qry.Append($"\n	    SELECT DISTINCT 거래처코드 AS trading_partner_code                               ");
            qry.Append($"\n		FROM 업체별월별매출현황                                                             ");
            qry.Append($"\n		WHERE 1=1                                                             ");
            if (!string.IsNullOrEmpty(company_code))
                qry.Append($"\n		  {commonRepository.whereSql("거래처코드", company_code, ",", true)}                                                                  ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n		  AND 거래처명 LIKE '%{company}%'                                                                  ");
            qry.Append($"\n	) AS t                                                                           ");
            qry.Append($"\n	LEFT OUTER JOIN (                                                                ");
            qry.Append($"\n	    SELECT                                                                       ");
            qry.Append($"\n	      거래처코드                                                                     ");
            qry.Append($"\n	    , SUM(미수잔액) AS 미수잔액                                                        ");
            qry.Append($"\n	    , SUM(전월매출금액) AS 전월매출금액                                                   ");
            qry.Append($"\n	    , SUM(전월마진금액) AS 전월마진금액                                                   ");
            qry.Append($"\n	    FROM (                                                                       ");
            qry.Append($"\n			SELECT                                                                   ");
            qry.Append($"\n			  거래처코드                                                                 ");
            qry.Append($"\n			, ISNULL(미수잔액, 0) AS 미수잔액                                              ");
            qry.Append($"\n			, 0 AS 전월매출금액                                                           ");
            qry.Append($"\n			, 0 AS 전월마진금액                                                           ");
            qry.Append($"\n			FROM 업체별월별매출현황                                                         ");
            qry.Append($"\n			WHERE 사용자 = {preLastdate.ToString("yyyyMM")}                                                       ");
            if (!string.IsNullOrEmpty(company_code))
                qry.Append($"\n		  {commonRepository.whereSql("거래처코드", company_code, ",", true)}                                                                  ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n		  AND 거래처명 LIKE '%{company}%'                                                                  ");
            qry.Append($"\n			UNION ALL                                                                ");
            qry.Append($"\n			SELECT                                                                   ");
            qry.Append($"\n		 	  매출처코드 AS 거래처코드                                                       ");
            qry.Append($"\n		 	, 0 AS 미수잔액                                                             ");
            qry.Append($"\n			, SUM(ISNULL(매출금액, 0) + ISNULL(부가세, 0)) AS 전월매출금액                     ");
            qry.Append($"\n			, SUM(ISNULL(손익, 0)) AS 전월마진금액                                         ");
            qry.Append($"\n			FROM 매출현황                                                               ");
            qry.Append($"\n			WHERE 사용자 = '200009'                                                    ");
            qry.Append($"\n			  AND 매출일자 < '{sttdate.ToString("yyyy-MM-dd")}'                                              ");
            if (!string.IsNullOrEmpty(company_code))
                qry.Append($"\n		  {commonRepository.whereSql("매출처코드", company_code, ",", true)}                                                                  ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n		  AND 매출처 LIKE '%{company}%'                                                                  ");
            qry.Append($"\n			GROUP BY 매출처코드                                                          ");
            qry.Append($"\n		) AS t                                                                       ");
            qry.Append($"\n		GROUP BY 거래처코드                                                              ");
            qry.Append($"\n	) AS a                                                                           ");
            qry.Append($"\n	  ON t.trading_partner_code = a.거래처코드                                           ");

            qry.Append($"\n), sales_table AS (                                                                            ");
            qry.Append($"\n	SELECT                                                                                        ");
            qry.Append($"\n	  매출처코드                                                                                      ");
            qry.Append($"\n	, 매출일자                                                                                       ");
            qry.Append($"\n	, 매출처                                                                                        ");
            qry.Append($"\n	, ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 매출금액                                                  ");
            qry.Append($"\n	, ISNULL(손익, 0) AS 손익                                                  ");
            qry.Append($"\n	FROM 매출현황                                                                                    ");
            qry.Append($"\n	WHERE 1=1                                                                                     ");
            qry.Append($"\n	  AND 사용자 = '200009'                                                                         ");
            qry.Append($"\n	  AND 매출일자 >= '{sttdate.ToString("yyyy-MM-dd")}'                                                                   ");
            qry.Append($"\n), payments_table AS (                                                                         ");
            qry.Append($"\n	SELECT                                                                                        ");
            qry.Append($"\n	  거래처코드                                                                                      ");
            qry.Append($"\n	, 입금일자                                                                                       ");
            qry.Append($"\n	, 거래처명                                                                                       ");
            qry.Append($"\n	, ISNULL(입금금액, 0) AS 입금금액                                                                   ");
            qry.Append($"\n	FROM 거래처별결제현황                                                                               ");
            qry.Append($"\n	WHERE 1=1                                                                                     ");
            qry.Append($"\n	  AND 사용자 = '200009'                                                                         ");
            qry.Append($"\n	  AND 입금일자 >= '{sttdate.ToString("yyyy-MM-dd")}'                                                                   ");
            qry.Append($"\n)                                                                                              ");
            qry.Append($"\nSELECT                                   ");
            qry.Append($"\n  날짜                                     ");
            qry.Append($"\n, SUM(전월매출금액) AS 전월매출금액               ");
            qry.Append($"\n, SUM(전월마진금액) AS 전월마진금액               ");
            qry.Append($"\n, SUM(전월마진금액) AS 전월마진금액               ");
            qry.Append($"\n, SUM(매출) AS 매출                         ");
            qry.Append($"\n, SUM(손익) AS 손익                         ");
            qry.Append($"\n, SUM(결제) AS 결제                         ");
            qry.Append($"\n, SUM(잔액) AS 잔액                         ");
            qry.Append($"\nFROM(                                    ");
            qry.Append($"\nSELECT                                                                                         ");
            qry.Append($"\n    dates_table.date_field AS 날짜,                                                              ");
            qry.Append($"\n    trading_partners.trading_partner_code AS 거래처코드,                                           ");
            qry.Append($"\n    trading_partners.전월매출금액 AS 전월매출금액,                                           ");
            qry.Append($"\n    trading_partners.전월마진금액 AS 전월마진금액,                                           ");
            qry.Append($"\n	ISNULL((                                                                                      ");
            qry.Append($"\n    	SELECT SUM(매출금액)                                                                         ");
            qry.Append($"\n    	FROM sales_table                                                                          ");
            qry.Append($"\n    	WHERE sales_table.매출일자 = dates_table.date_field                                          ");
            qry.Append($"\n    	  AND sales_table.매출처코드 = trading_partners.trading_partner_code)                         ");
            qry.Append($"\n   	, 0) AS 매출,                                                                               ");
            qry.Append($"\n	ISNULL((                                                                                      ");
            qry.Append($"\n    	SELECT SUM(손익)                                                                         ");
            qry.Append($"\n    	FROM sales_table                                                                          ");
            qry.Append($"\n    	WHERE sales_table.매출일자 = dates_table.date_field                                          ");
            qry.Append($"\n    	  AND sales_table.매출처코드 = trading_partners.trading_partner_code)                         ");
            qry.Append($"\n   	, 0) AS 손익,                                                                               ");
            qry.Append($"\n   	ISNULL((                                                                                  ");
            qry.Append($"\n    	SELECT SUM(입금금액)                                                                         ");
            qry.Append($"\n    	FROM payments_table                                                                       ");
            qry.Append($"\n    	WHERE payments_table.입금일자 = dates_table.date_field                                       ");
            qry.Append($"\n    	  AND payments_table.거래처코드 = trading_partners.trading_partner_code)                      ");
            qry.Append($"\n   	, 0) AS 결제,                                                                               ");
            qry.Append($"\n    전월미수잔액 + ISNULL((                                                                         ");
            qry.Append($"\n    	SELECT SUM(매출금액)                                                                         ");
            qry.Append($"\n    	FROM sales_table                                                                          ");
            qry.Append($"\n    	WHERE 1=1                                                                                 ");
            qry.Append($"\n    	  AND sales_table.매출일자 <= dates_table.date_field                                         ");
            qry.Append($"\n    	  AND sales_table.매출처코드 = trading_partners.trading_partner_code)                         ");
            qry.Append($"\n   	, 0)                                                                                      ");
            qry.Append($"\n   	- ISNULL((                                                                                ");
            qry.Append($"\n    	SELECT SUM(입금금액)                                                                         ");
            qry.Append($"\n    	FROM payments_table                                                                       ");
            qry.Append($"\n    	WHERE 1=1                                                                                 ");
            qry.Append($"\n    	  AND payments_table.입금일자 <= dates_table.date_field                                      ");
            qry.Append($"\n    	  AND payments_table.거래처코드 = trading_partners.trading_partner_code)                      ");
            qry.Append($"\n   	, 0) AS 잔액                                                                                ");
            qry.Append($"\nFROM                                                                                           ");
            qry.Append($"\n    dates_table                                                                                ");
            qry.Append($"\nCROSS JOIN                                                                                     ");
            qry.Append($"\n    trading_partners                                                                           ");
            qry.Append($"\n) AS t                                                                           ");
            qry.Append($"\nGROUP BY 날짜                                                           ");
            qry.Append($"\nORDER BY 날짜 DESC                                                           ");



            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCompanyList(string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                      ");
            qry.Append($"\n   거래처코드                                                                ");
            qry.Append($"\n , 거래처명                                                                  ");
            qry.Append($"\n FROM 업체별월별매출현황                                                     ");
            qry.Append($"\n WHERE 사용자 >= 201601                                                      ");
            qry.Append($"\n   AND 사용자 <= {DateTime.Now.ToString("yyyyMM")}                           ");
            if (!string.IsNullOrEmpty(company))
                qry.Append($"\n   AND 거래처명 LIKE '%{company}%'                                       ");
            qry.Append($"\n GROUP BY 거래처코드, 거래처명                                                             ");
            qry.Append($"\n ORDER BY 거래처명                                                             ");

            string sql = qry.ToString();

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetRecoveryPrincipal(int year, string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                          ");
            qry.Append($"\n *                               ");
            qry.Append($"\n FROM t_recovery_principal       ");
            qry.Append($"\n WHERE 1=1                       ");
            qry.Append($"\n   AND year =  {year}            ");
            qry.Append($"\n   AND company = '{company}'     ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public Int32 GetPreAvgBalance(int year, string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                           ");
            qry.Append($"\n AVG(balance_due)  AS balance_due ");
            qry.Append($"\n FROM t_recovery_principal        ");
            qry.Append($"\n WHERE 1=1                        ");
            qry.Append($"\n   AND year <  {year}             ");
            qry.Append($"\n   AND company = '{company}'      ");
            qry.Append($"\n GROUP BY company            ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            try
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch
            {
                return -1;
            }
        }

        public StringBuilder DeleteRecoveryPrincipal(int year, string company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_recovery_principal WHERE company = '{company}' AND year = {year}           ");

            return qry;
        }
        public StringBuilder InsertRecoveryPrincipal(RecoveryPrincipalModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_recovery_principal (   ");
            qry.Append($"   company                            ");
            qry.Append($" , year                               ");
            qry.Append($" , month                              ");
            qry.Append($" , monthly_sales_price                ");
            qry.Append($" , cumulative_sales_price             ");
            qry.Append($" , balance_due                        ");
            qry.Append($" , averge_balance_due                 ");
            qry.Append($" , grade                              ");
            qry.Append($" , updatetime                         ");
            qry.Append($" ) VALUES (                           ");
            qry.Append($"   '{model.company}'                  ");
            qry.Append($" ,  {model.year}                      ");
            qry.Append($" ,  {model.month}                     ");
            qry.Append($" ,  {model.monthly_sales_price}       ");
            qry.Append($" ,  {model.cumulative_sales_price}    ");
            qry.Append($" ,  {model.balance_due}               ");
            qry.Append($" ,  {model.averge_balance_due}        ");
            qry.Append($" , '{model.grade}'                    ");
            qry.Append($" , '{model.updatetime}'               ");
            qry.Append($" )                                    ");

            return qry;
        }

        public int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
        {
            if (sqlList.Count > 0)
            {
                dbInstance.Connection.Close();
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
