using Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SEAOVER
{
    public class StockRepository : ClassRoot, IStockRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        private static string userId;
        public int CallStoredProcedureSTOCK(string user_id, string enddate, bool isDash = false)
        {
            userId = user_id;
            int result = 0;

            SqlCommand cmd = new SqlCommand("SP_품명별재고현황", (SqlConnection)dbInstance3.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            if(isDash)
                cmd.Parameters.AddWithValue("@saup_cd", "20");
            else
                cmd.Parameters.AddWithValue("@saup_cd", "%%");
            cmd.Parameters.AddWithValue("@proc_ymd", enddate);
            cmd.Parameters.AddWithValue("@fish_hnm", "%%");
            cmd.Parameters.AddWithValue("@size_hnm", "%%");
            cmd.Parameters.AddWithValue("@won_hnm", "%%");
            cmd.Parameters.AddWithValue("@cargo_hnm", "%%");
            if (isDash)
                cmd.Parameters.AddWithValue("@work_id", "dashboard");
            else
                cmd.Parameters.AddWithValue("@work_id", userId);

            result = cmd.ExecuteNonQuery();
            return result;
        }

        public DataTable GetExistStock()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                       ");
            qry.Append($"\n t.*                                                                          ");
            qry.Append($"\n FROM(                                                                        ");
            qry.Append($"\n	 SELECT                                                                      ");
            qry.Append($"\n	 품명                                                                          ");
            qry.Append($"\n	 , 품명코드                                                                     ");
            qry.Append($"\n	 , 원산지                                                                      ");
            qry.Append($"\n	 , 원산지코드                                                                    ");
            qry.Append($"\n	 , 규격                                                                        ");
            qry.Append($"\n	 , 규격코드                                                                     ");
            qry.Append($"\n	 , REPLACE(단위, 'kg', '') AS 단위                                              ");
            qry.Append($"\n	 , SUM(CASE WHEN ISNULL(통관, '미통관') = '통관' THEN 재고수 ELSE 0 END) AS 재고수      ");
            qry.Append($"\n	 , SUM(ISNULL(예약수, 0)) AS 예약수                                               ");
            qry.Append($"\n	 FROM 품명별재고현황                                                               ");
            qry.Append($"\n	 WHERE 사용자 = '{userId}'                                                       ");
            qry.Append($"\n	 GROUP BY 품명, 품명코드, 원산지, 원산지코드, 규격, 규격코드, REPLACE(단위, 'kg', '')           ");
            qry.Append($"\n ) AS t                                                                       ");
            qry.Append($"\n WHERE 재고수 - 예약수 > 0                                                         ");

            string sql = qry.ToString();

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetStockAndWarehouse(bool isInStock = true)
        {
            StringBuilder qry = new StringBuilder();
            //재고있느 품목만 최신화
            if (isInStock)
            {
                qry.Append($"\n SELECT                                                                                                                                   ");
                qry.Append($"\n   t.*                                                                                                                                    ");
                qry.Append($"\n FROM (                                                                                                                                   ");
                qry.Append($"\n SELECT                                                                                                                                   ");
                qry.Append($"\n   t1.품명                                                                                                                                  ");
                qry.Append($"\n , t1.원산지                                                                                                                                 ");
                qry.Append($"\n , t1.규격                                                                                                                                  ");
                qry.Append($"\n , t1.품명코드                                                                                                                                  ");
                qry.Append($"\n , t1.원산지코드                                                                                                                                 ");
                qry.Append($"\n , t1.규격코드                                                                                                                                  ");
                qry.Append($"\n , REPLACE(t1.단위, 'kg', '') AS 단위                                                                                                         ");
                qry.Append($"\n , SUM(ISNULL(t1.재고수, 0)) AS 재고수                                                                                                              ");
                qry.Append($"\n , SUM(ISNULL(t1.예약수, 0)) AS 예약수                                                                                                         ");
                qry.Append($"\n , SUM(ISNULL(t1.재고수, 0) - ISNULL(t1.예약수, 0)) AS 실재고                                                                    ");
                qry.Append($"\n , t2.매출단가                                                                                                                                ");
                qry.Append($"\n , t1.창고                                                                                                                                  ");
                qry.Append($"\n , t1.적요                                                                                                                                  ");
                qry.Append($"\n , CASE WHEN t2.부가세유무 = 'Y' THEN '과세' ELSE '' END AS 부가세유무                                        ");
                qry.Append($"\n FROM 품명별재고현황 AS t1                                                                                                                      ");
                qry.Append($"\n LEFT OUTER JOIN (                                                                                                                        ");
                qry.Append($"\n	SELECT                                                                                                                                   ");
                qry.Append($"\n	  품명, 원산지, 규격, SEAOVER단위, 매출단가, 부가세유무                                                                                                         ");
                qry.Append($"\n 	FROM 업체별시세관리 AS a                                                                                                                   ");
                qry.Append($"\n 	WHERE 사용자 = '{userId}'                                                                                                                 ");
                qry.Append($"\n	   AND 매출단가 > 10                                                                                                                         ");
                qry.Append($"\n	   AND 매입일자 = (                                                                                                                          ");
                qry.Append($"\n	   	SELECT MAX(매입일자)                                                                                                                     ");
                qry.Append($"\n	   	FROM 업체별시세관리 AS b                                                                                                                   ");
                qry.Append($"\n	   	WHERE 사용자 = '{userId}'                                                                                                                 ");
                qry.Append($"\n	   	  AND a.품명  = b.품명                                                                                                                   ");
                qry.Append($"\n	   	  AND a.원산지  = b.원산지                                                                                                                ");
                qry.Append($"\n	   	  AND a.규격  = b.규격                                                                                                                   ");
                qry.Append($"\n	   	  AND a.SEAOVER단위  = b.SEAOVER단위 )                                                                                                   ");
                qry.Append($"\n ) AS t2                                                                                                                                  ");
                qry.Append($"\n   ON t1.품명  = t2.품명                                                                                                                      ");
                qry.Append($"\n  AND t1.원산지  = t2.원산지                                                                                                                   ");
                qry.Append($"\n  AND t1.규격  = t2.규격                                                                                                                      ");
                qry.Append($"\n  AND REPLACE(t1.단위, 'kg', '')  = t2.SEAOVER단위                                                                                            ");
                qry.Append($"\n WHERE t1.사용자 = '{userId}'                                                        ");
                qry.Append($"\n GROUP BY  t1.품명 , t1.원산지 , t1.규격 , REPLACE(t1.단위, 'kg', ''), t2.매출단가 , t1.창고 , t1.적요, t1.품명코드, t1.원산지코드, t1.규격코드, t2.부가세유무  ");
                qry.Append($"\n ) AS t  ");
                qry.Append($"\n WHERE 실재고 > 0 ");
                qry.Append($"\n  ORDER BY CASE WHEN 적요 LIKE '%먼저%' THEN 1 ELSE 2 END, 실재고 DESC ");
            }
            else
            {
                qry.Append($"\n  SELECT                                                                                                                                ");
                qry.Append($"\n  DISTINCT                                                                                                                              ");
                qry.Append($"\n    t.품명                                                                                                                               ");
                qry.Append($"\n  , t.원산지                                                                                                                              ");
                qry.Append($"\n  , t.규격                                                                                                                               ");
                qry.Append($"\n  , t.품명코드                                                                                                                             ");
                qry.Append($"\n  , t.원산지코드                                                                                                                           ");
                qry.Append($"\n  , t.규격코드                                                                                                                             ");
                qry.Append($"\n  , t.단위 AS 단위                                                                                                                         ");
                qry.Append($"\n  , ISNULL(t.재고수, 0) AS  재고수                                                                                                          ");
                qry.Append($"\n  , ISNULL(t.예약수, 0) AS  예약수                                                                                                          ");
                qry.Append($"\n  , ISNULL(t.실재고, 0) AS  실재고                                                                                                          ");
                qry.Append($"\n  , t.매출단가                                                                                                                             ");
                qry.Append($"\n  , t.창고                                                                                                                               ");
                qry.Append($"\n  , t.적요                                                                                                                               ");
                qry.Append($"\n  , t.부가세유무                                                                                                                           ");
                qry.Append($"\n  , CASE WHEN 적요 LIKE '%먼저%' THEN 1 ELSE 2 END AS 우선순위                                                                                ");
                qry.Append($"\n  FROM (                                                                                                                                ");
                qry.Append($"\n  SELECT                                                                                                                                ");
                qry.Append($"\n    t1.품명                                                                                                                              ");
                qry.Append($"\n  , t1.원산지                                                                                                                             ");
                qry.Append($"\n  , t1.규격                                                                                                                              ");
                qry.Append($"\n  , t1.품목코드 AS 품명코드                                                                                                                   ");
                qry.Append($"\n  , t1.원산지코드                                                                                                                          ");
                qry.Append($"\n  , t1.규격코드                                                                                                                            ");
                qry.Append($"\n  , t1.SEAOVER단위 AS 단위                                                                                                                 ");
                qry.Append($"\n  , t2.재고수                                                                                                                             ");
                qry.Append($"\n  , t2.예약수                                                                                                                             ");
                qry.Append($"\n  , t2.실재고                                                                                                                             ");
                qry.Append($"\n  , t1.매출단가                                                                                                                            ");
                qry.Append($"\n  , t2.창고                                                                                                                              ");
                qry.Append($"\n  , t2.적요                                                                                                                              ");
                qry.Append($"\n  , CASE WHEN t1.부가세유무 = 'Y' THEN '과세' ELSE '' END AS 부가세유무                                                                          ");
                qry.Append($"\n  FROM (                                                                                                                                ");
                qry.Append($"\n  	SELECT                                                                                                                             ");
                qry.Append($"\n 	  품명, 품목코드, 원산지, 원산지코드, 규격, 규격코드, SEAOVER단위, 매출단가, 부가세유무                                                                    ");
                qry.Append($"\n  	FROM 업체별시세관리 AS a                                                                                                                ");
                qry.Append($"\n  	WHERE 사용자 = '{userId}'                                                                                                              ");
                qry.Append($"\n 	   AND 매출단가 > 10                                                                                                                  ");
                qry.Append($"\n 	   AND 매입일자 = (                                                                                                                   ");
                qry.Append($"\n 	   	SELECT MAX(매입일자)                                                                                                              ");
                qry.Append($"\n 	   	FROM 업체별시세관리 AS b                                                                                                            ");
                qry.Append($"\n 	   	WHERE 사용자 = '{userId}'                                                                                                          ");
                qry.Append($"\n 	   	  AND a.품명  = b.품명                                                                                                            ");
                qry.Append($"\n 	   	  AND a.원산지  = b.원산지                                                                                                         ");
                qry.Append($"\n 	   	  AND a.규격  = b.규격                                                                                                            ");
                qry.Append($"\n 	   	  AND a.SEAOVER단위  = b.SEAOVER단위 )                                                                                            ");
                qry.Append($"\n  ) AS t1                                                                                                                               ");
                qry.Append($"\n  LEFT OUTER JOIN (                                                                                                                     ");
                qry.Append($"\n 	SELECT                                                                                                                             ");
                qry.Append($"\n 	 품명                                                                                                                               ");
                qry.Append($"\n 	, 원산지                                                                                                                             ");
                qry.Append($"\n 	, 규격                                                                                                                              ");
                qry.Append($"\n 	, 단위                                                                                                                              ");
                qry.Append($"\n     , SUM(ISNULL(재고수, 0)) AS 재고수                                                                                                     ");
                qry.Append($"\n     , SUM(ISNULL(예약수, 0)) AS 예약수                                                                                                     ");
                qry.Append($"\n     , SUM(ISNULL(재고수, 0) - ISNULL(예약수, 0)) AS 실재고                                                                                    ");
                qry.Append($"\n     , 창고                                                                                                                              ");
                qry.Append($"\n     , 적요                                                                                                                              ");
                qry.Append($"\n 	FROM 품명별재고현황                                                                                                                     ");
                qry.Append($"\n 	WHERE 사용자 = '{userId}'                                                                                                              ");
                qry.Append($"\n 	GROUP BY 품명, 원산지, 규격, 단위, 창고, 적요                                                                                                ");
                qry.Append($"\n ) AS t2                                                                                                                                ");
                qry.Append($"\n    ON t1.품명  = t2.품명                                                                                                                  ");
                qry.Append($"\n   AND t1.원산지  = t2.원산지                                                                                                               ");
                qry.Append($"\n   AND t1.규격  = t2.규격                                                                                                                  ");
                qry.Append($"\n   AND t1.SEAOVER단위 = REPLACE(t2.단위, 'kg', '')                                                                                         ");
                qry.Append($"\n  ) AS t                                                                                                                                ");
                qry.Append($"\n   ORDER BY 품명, 원산지, 규격, 단위, CASE WHEN 적요 LIKE '%먼저%' THEN 1 ELSE 2 END, 실재고 DESC                                                   ");
            }
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPendingStock()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                             ");
            qry.Append($"\n   atoNo                            ");
            qry.Append($"\n , MAX(입고일자) AS 입고일자        ");
            qry.Append($"\n , 매입처                           ");
            qry.Append($"\n , 통관                             ");
            qry.Append($"\n FROM 품명별재고현황                ");
            qry.Append($"\n WHERE 사용자 = '{userId}'          ");
            //qry.Append($"\n   AND 통관 = '통관'                ");
            qry.Append($"\n   AND ISNULL(atoNo, '') <> ''      ");
            qry.Append($"\n GROUP BY atoNo, 매입처, 통관               ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductSales(DateTime sttdate, DateTime enddate, string product = "", string origin = "", string sizes = "", string unit = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                                                     ");
            qry.Append($"\n  t1.품명                                                                                                                    ");
            qry.Append($"\n, t1.원산지                                                                                                                   ");
            qry.Append($"\n, t1.규격                                                                                                                    ");
            qry.Append($"\n, t1.규격2                                                                                                                   ");
            qry.Append($"\n, t1.규격3                                                                                                                   ");
            qry.Append($"\n, t1.규격4                                                                                                                   ");
            qry.Append($"\n, t1.단위                                                                                                                    ");
            qry.Append($"\n, t1.재고수                                                                                                                   ");
            qry.Append($"\n, t1.재고금액                                                                                                                  ");
            qry.Append($"\n, ISNULL(AVG(t2.매출수), 0) AS 매출수0                                                                                         ");
            qry.Append($"\n, ISNULL(AVG(t2.매출금액), 0) AS 매출금액0                                                                                         ");
            qry.Append($"\n, ISNULL(SUM(t2.매출수), 0) AS 매출수1                                                                                         ");
            qry.Append($"\n, ISNULL(SUM(t2.매출금액), 0) AS 매출금액1                                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 1 THEN t2.매출수 END), 0) AS 매출수2                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 1 THEN t2.매출금액 END) , 0)AS 매출금액2                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 2 THEN t2.매출수 END), 0) AS 매출수3                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 2 THEN t2.매출금액 END), 0) AS 매출금액3                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 3 THEN t2.매출수 END), 0) AS 매출수4                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 3 THEN t2.매출금액 END), 0) AS 매출금액4                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 4 THEN t2.매출수 END), 0) AS 매출수5                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 4 THEN t2.매출금액 END), 0) AS 매출금액5                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 5 THEN t2.매출수 END), 0) AS 매출수6                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 5 THEN t2.매출금액 END), 0) AS 매출금액6                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 6 THEN t2.매출수 END), 0) AS 매출수7                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 6 THEN t2.매출금액 END), 0) AS 매출금액7                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 7 THEN t2.매출수 END), 0) AS 매출수8                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 7 THEN t2.매출금액 END), 0) AS 매출금액8                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 8 THEN t2.매출수 END), 0) AS 매출수9                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 8 THEN t2.매출금액 END), 0) AS 매출금액9                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 9 THEN t2.매출수 END), 0) AS 매출수10                                                                         ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 9 THEN t2.매출금액 END), 0) AS 매출금액10                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 10 THEN t2.매출수 END), 0) AS 매출수11                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 10 THEN t2.매출금액 END), 0) AS 매출금액11                                                                     ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 11 THEN t2.매출수 END), 0) AS 매출수12                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 11 THEN t2.매출금액 END), 0) AS 매출금액12                                                                     ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 12 THEN t2.매출수 END), 0) AS 매출수13                                                                       ");
            qry.Append($"\n, ISNULL(AVG(CASE WHEN t2.월 = 12 THEN t2.매출금액 END), 0) AS 매출금액13                                                                     ");
            qry.Append($"\n, t1.최소단위                                                                                                                  ");
            qry.Append($"\nFROM(                                                                                                                      ");
            qry.Append($"\n	SELECT                                                                                                                    ");
            qry.Append($"\n	  t1.품명                                                                                                                   ");
            qry.Append($"\n	, t1.원산지                                                                                                                  ");
            qry.Append($"\n	, t1.규격                                                                                                                   ");
            qry.Append($"\n	, ISNULL(t1.규격2, t2.규격2) AS 규격2                                                                                           ");
            qry.Append($"\n	, CASE WHEN ISNULL(t1.규격2, t2.규격2) = '.' THEN NULL                                                                        ");
            qry.Append($"\n 	 	   WHEN ISNULL(t1.규격2, t2.규격2) = '-' THEN NULL                                                                    ");
            qry.Append($"\n 	 	   WHEN ISNUMERIC(ISNULL(t1.규격2, t2.규격2)) = 1 THEN CONVERT(float, ISNULL(t1.규격2, t2.규격2))                        ");
            qry.Append($"\n 	 	   ELSE NULL                                                                                                      ");
            qry.Append($"\n 	  END AS 규격3                                                                                                          ");
            qry.Append($"\n 	, CASE WHEN ISNULL(t1.규격2, t2.규격2) = '.' THEN ISNULL(t1.규격2, t2.규격2)                                                 ");
            qry.Append($"\n 	 	   WHEN ISNULL(t1.규격2, t2.규격2) = '-' THEN ISNULL(t1.규격2, t2.규격2)                                                 ");
            qry.Append($"\n 	 	   WHEN ISNUMERIC(ISNULL(t1.규격2, t2.규격2)) = 1 THEN NULL                                                           ");
            qry.Append($"\n 	 	   ELSE ISNULL(t1.규격2, t2.규격2)                                                                                    ");
            qry.Append($"\n 	  END AS 규격4                                                                                                          ");
            qry.Append($"\n	, t1.단위                                                                                                                   ");
            qry.Append($"\n	, t1.재고수                                                                                                                  ");
            qry.Append($"\n	, t1.재고금액                                                                                                                 ");
            qry.Append($"\n	, t2.단위 AS 최소단위                                                                                                           ");
            qry.Append($"\n	FROM(                                                                                                                     ");
            qry.Append($"\n		SELECT                                                                                                                ");
            qry.Append($"\n		  품명                                                                                                                  ");
            qry.Append($"\n		, 원산지                                                                                                                 ");
            qry.Append($"\n		, 규격                                                                                                                  ");
            qry.Append($"\n		, CASE WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                                    ");
            qry.Append($"\n 			  CASE WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))               ");
            qry.Append($"\n 				   WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격)) END           ");
            qry.Append($"\n 		       WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                     ");
            qry.Append($"\n 		       WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                      ");
            qry.Append($"\n 		       ELSE 규격                                                                                                    ");
            qry.Append($"\n 		  END AS 규격2                                                                                                      ");
            qry.Append($"\n		, REPLACE(단위, 'kg', '') AS 단위                                                                                         ");
            qry.Append($"\n		, SUM(ISNULL(재고수,0) - ISNULL(예약수,0)) AS 재고수                                                                          ");
            qry.Append($"\n		, SUM((ISNULL(재고수,0) - ISNULL(예약수,0)) * (CASE WHEN 재고수 > 0 THEN 재고금액 / 재고수 ELSE 0 END)) AS 재고금액                     ");
            qry.Append($"\n		FROM 품명별재고현황                                                                                                         ");
            qry.Append($"\n		WHERE 사용자 = '{userId}'                                                                                                  ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("품명", product));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("원산지", origin));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("규격", sizes));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("REPLACE(단위, 'kg', '')", unit));
            qry.Append($"\n		GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                                                                          ");
            qry.Append($"\n		UNION ALL                                                                                                             ");
            qry.Append($"\n		SELECT                                                                                                                ");
            qry.Append($"\n		  품명                                                                                                                  ");
            qry.Append($"\n		, 원산지                                                                                                                 ");
            qry.Append($"\n		, 규격                                                                                                                  ");
            qry.Append($"\n		, CASE WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                                    ");
            qry.Append($"\n 			  CASE WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))               ");
            qry.Append($"\n 				   WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격)) END           ");
            qry.Append($"\n 		       WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                     ");
            qry.Append($"\n 		       WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                      ");
            qry.Append($"\n 		       ELSE 규격                                                                                                    ");
            qry.Append($"\n 		  END AS 규격2                                                                                                      ");
            qry.Append($"\n		, REPLACE(단위, 'kg', '') AS 단위                                                                                         ");
            qry.Append($"\n		, 0 AS 재고수                                                                                                            ");
            qry.Append($"\n		, 0 AS 재고금액                                                                                                           ");
            qry.Append($"\n		FROM 품명별재고현황                                                                                                         ");
            qry.Append($"\n		WHERE 사용자 = '6'                                                                                                       ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("품명", product));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("원산지", origin));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("규격", sizes));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("REPLACE(단위, 'kg', '')", unit));
            qry.Append($"\n		GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                                                                          ");
            qry.Append($"\n	) AS t1                                                                                                                   ");
            qry.Append($"\n	 INNER JOIN (                                                                                                             ");
            qry.Append($"\n		 SELECT                                                                                                               ");
            qry.Append($"\n		   t.품명                                                                                                               ");
            qry.Append($"\n	 	 , t.원산지                                                                                                              ");
            qry.Append($"\n	 	 , t.규격                                                                                                               ");
            qry.Append($"\n	 	 , CASE WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                                   ");
            qry.Append($"\n 			  CASE WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))               ");
            qry.Append($"\n 				   WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격)) END           ");
            qry.Append($"\n 		       WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                     ");
            qry.Append($"\n 		       WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                      ");
            qry.Append($"\n 		       ELSE 규격                                                                                                    ");
            qry.Append($"\n 		  END AS 규격2                                                                                                      ");
            qry.Append($"\n	 	 , MIN(t.단위) AS 단위                                                                                                    ");
            qry.Append($"\n		 FROM(                                                                                                                ");
            qry.Append($"\n			 SELECT                                                                                                           ");
            qry.Append($"\n		 	  품명                                                                                                              ");
            qry.Append($"\n		 	, 원산지                                                                                                             ");
            qry.Append($"\n		 	, 규격                                                                                                              ");
            qry.Append($"\n		 	, REPLACE(단위, 'kg', '') AS 단위                                                                                     ");
            qry.Append($"\n		 	FROM 품명별재고현황                                                                                                     ");
            qry.Append($"\n		 	WHERE 사용자 = '{userId}'                                                                                              ");
            qry.Append($"\n		 	  AND 단위 <> '.' AND 단위 <> '-' AND ISNUMERIC(REPLACE(단위, 'kg', '')) = 1                                            ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("품명", product));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("원산지", origin));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("규격", sizes));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("REPLACE(단위, 'kg', '')", unit));
            qry.Append($"\n		 	UNION ALL                                                                                                         ");
            qry.Append($"\n		 	SELECT                                                                                                            ");
            qry.Append($"\n		 	  품명                                                                                                              ");
            qry.Append($"\n		 	, 원산지                                                                                                             ");
            qry.Append($"\n		 	, 규격                                                                                                              ");
            qry.Append($"\n		 	, REPLACE(단위, 'kg', '') AS 단위                                                                                     ");
            qry.Append($"\n		 	FROM 품명별매출현황                                                                                                     ");
            qry.Append($"\n		 	WHERE 사용자 = '6'                                                                                                   ");
            qry.Append($"\n		 	  AND 단위 <> '.' AND 단위 <> '-' AND ISNUMERIC(REPLACE(단위, 'kg', '')) = 1                                            ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(commonRepository.whereSql("품명", product));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(commonRepository.whereSql("원산지", origin));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(commonRepository.whereSql("규격", sizes));
            if (!string.IsNullOrEmpty(unit))
                qry.Append(commonRepository.whereSql("REPLACE(단위, 'kg', '')", unit));
            qry.Append($"\n	 	) AS t                                                                                                                ");
            qry.Append($"\n		GROUP BY  품명, 원산지, 규격                                                                                                ");
            qry.Append($"\n	) AS t2                                                                                                                   ");
            qry.Append($"\n	  ON t1.품명 = t2.품명                                                                                                        ");
            qry.Append($"\n	  AND t1.원산지 = t2.원산지                                                                                                     ");
            qry.Append($"\n	  AND t1.규격 = t2.규격                                                                                                       ");
            qry.Append($"\n) AS t1                                                                                                                    ");
            qry.Append($"\nLEFT OUTER JOIN (                                                                                                          ");

            DateTime tempDt = sttdate;
            while (tempDt <= enddate)
            {
                if(tempDt > sttdate)
                    qry.Append($"\n	UNION ALL                                                                                                                    ");
                qry.Append($"\n	SELECT                                                                                                                    ");
                qry.Append($"\n	  MONTH(CONVERT(DATE, 사용자 + '01')) AS 월                                                                                   ");
                qry.Append($"\n	, 품명                                                                                                                      ");
                qry.Append($"\n	, 원산지                                                                                                                     ");
                qry.Append($"\n	, 규격                                                                                                                      ");
                qry.Append($"\n	, REPLACE(단위, 'kg', '') AS 단위                                                                                             ");
                qry.Append($"\n	, SUM(매출수) AS 매출수                                                                                                         ");
                qry.Append($"\n	, SUM(매출금액) AS 매출금액                                                                                                      ");
                qry.Append($"\n	FROM 품명별매출현황                                                                                                             ");
                qry.Append($"\n	WHERE 사용자 = '{tempDt.ToString("yyyyMM")}'                                                                                                      ");
                if (!string.IsNullOrEmpty(product))
                    qry.Append(commonRepository.whereSql("품명", product));
                if (!string.IsNullOrEmpty(origin))
                    qry.Append(commonRepository.whereSql("원산지", origin));
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append(commonRepository.whereSql("규격", sizes));
                if (!string.IsNullOrEmpty(unit))
                    qry.Append(commonRepository.whereSql("REPLACE(단위, 'kg', '')", unit));
                qry.Append($"\n	GROUP BY 사용자, 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                                                                         ");
                tempDt = tempDt.AddMonths(1);
            }

            qry.Append($"\n) AS t2                                                                                                                    ");
            qry.Append($"\n  ON t1.품명 = t2.품명                                                                                                         ");
            qry.Append($"\n  AND t1.원산지 = t2.원산지                                                                                                      ");
            qry.Append($"\n  AND t1.규격 = t2.규격                                                                                                        ");
            qry.Append($"\n  AND t1.단위 = t2.단위                                                                                                        ");
            qry.Append($"\nGROUP BY t1.품명, t1.원산지, t1.규격, t1.규격2, t1.규격3, t1.규격4, t1.단위, t1.재고수, t1.재고금액, t1.최소단위                                  ");
            qry.Append($"\nORDER BY t1.품명, t1.원산지, t1.규격3, t1.규격2                                                                                    ");
            qry.Append($"\n  , CASE WHEN CHARINDEX('대', t1.규격4) > 0  THEN 1                                                                           ");
            qry.Append($"\n         WHEN CHARINDEX('중', t1.규격4) > 0 THEN 2                                                                            ");
            qry.Append($"\n         WHEN CHARINDEX('소', t1.규격4) > 0 THEN 3                                                                            ");
            qry.Append($"\n         WHEN CHARINDEX('L', t1.규격4) > 0 THEN 4                                                                            ");
            qry.Append($"\n         WHEN CHARINDEX('M', t1.규격4) > 0 THEN 5                                                                            ");
            qry.Append($"\n         WHEN CHARINDEX('S', t1.규격4) > 0 THEN 6    END                                                                     ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        

        public DataTable GetDashboardDetail(string division, string ato_no = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                    ");
            qry.Append($"\n   AtoNo                                   ");
            qry.Append($"\n , SUM(ISNULL(재고금액, 0)) AS 재고금액    ");
            qry.Append($"\n FROM 품명별재고현황                       ");
            qry.Append($"\n WHERE 사용자 = 'dashboard'                ");
            if(division == "대행재고")
                qry.Append($"\n   AND 매입처 LIKE '%(대)%'           ");
            else if (division == "유산스")
                qry.Append($"\n   AND 매입처 LIKE '%(유)%'           ");
            else if (division == "미결제재고")
                qry.Append($"\n   AND 매입처 LIKE '%(일)%'           ");
            if (!string.IsNullOrEmpty(ato_no))
                qry.Append($"\n   {commonRepository.whereSql("AtoNo", ato_no)}   ");
            qry.Append($"\n GROUP BY AtoNo                            ");
            qry.Append($"\n ORDER BY AtoNo                            ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetStockDashboard()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                          ");
            qry.Append($"\n    '총 재고' AS 구분                            ");
            qry.Append($"\n  , SUM(재고금액) AS 재고금액                    ");
            qry.Append($"\n  , 1 AS id                                      ");
            qry.Append($"\n  FROM (                                         ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    null AS AtoNo                               ");
            qry.Append($"\n	  , 0 AS 재고금액                               ");
            qry.Append($"\n	  UNION ALL                                     ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    AtoNo                                       ");
            qry.Append($"\n	  , ISNULL(재고금액, 0) AS 재고금액             ");
            qry.Append($"\n	  FROM 품명별재고현황                           ");
            qry.Append($"\n	  WHERE 사용자 = 'dashboard'                    ");
            qry.Append($"\n  ) AS t                                         ");
            qry.Append($"\n  UNION ALL                                      ");

            qry.Append($"\n SELECT                                          ");
            qry.Append($"\n    '은행 담보재고' AS 구분                      ");
            qry.Append($"\n  , SUM(재고금액) AS 재고금액                    ");
            qry.Append($"\n  , 2 AS id                                      ");
            qry.Append($"\n  FROM (                                         ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    null AS AtoNo                               ");
            qry.Append($"\n	  , 0 AS 재고금액                               ");
            qry.Append($"\n	  UNION ALL                                     ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    AtoNo                                       ");
            qry.Append($"\n	  , ISNULL(재고금액, 0) AS 재고금액             ");
            qry.Append($"\n	  FROM 품명별재고현황                           ");
            qry.Append($"\n	  WHERE 사용자 = 'dashboard'                    ");
            qry.Append($"\n	    AND 차입일자 IS NOT NULL AND 차입일자 <> '' ");
            qry.Append($"\n  ) AS t                                         ");
            qry.Append($"\n  UNION ALL                                      ");
            qry.Append($"\n  SELECT                                      ");
            qry.Append($"\n 	  '은행 담보재고(아토분)' AS 구분                 ");
            qry.Append($"\n	  , SUM(t.차입잔액)                             ");
            qry.Append($"\n	  , 3 AS id                                  ");
            qry.Append($"\n	  FROM(                                      ");
            qry.Append($"\n  	  SELECT                                 ");
            qry.Append($"\n	    DISTINCT                                 ");
            qry.Append($"\n	    차입ID                                     ");
            qry.Append($"\n	  , 차입잔액                                    ");
            qry.Append($"\n	  FROM 품명별재고현황                             ");
            qry.Append($"\n	  WHERE 사용자 = 'dashboard'                   ");
            qry.Append($"\n	    AND 차입일자 IS NOT NULL AND 차입일자 <> ''    ");
            qry.Append($"\n	  ) AS t                                     ");
            /*qry.Append($"\n SELECT                                          ");
            qry.Append($"\n    '은행 담보재고(아토분)' AS 구분              ");
            qry.Append($"\n  , SUM(재고금액) * 0.3 AS 재고금액              ");
            qry.Append($"\n  , 3 AS id                                      ");
            qry.Append($"\n  FROM (                                         ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    null AS AtoNo                               ");
            qry.Append($"\n	  , 0 AS 재고금액                               ");
            qry.Append($"\n	  UNION ALL                                     ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    AtoNo                                       ");
            qry.Append($"\n	  , ISNULL(재고금액, 0) AS 재고금액             ");
            qry.Append($"\n	  FROM 품명별재고현황                           ");
            qry.Append($"\n	  WHERE 사용자 = 'dashboard'                    ");
            qry.Append($"\n	    AND 차입일자 IS NOT NULL AND 차입일자 <> '' ");
            qry.Append($"\n  ) AS t                                         ");*/
            qry.Append($"\n  UNION ALL                                      ");

            qry.Append($"\n SELECT                                          ");
            qry.Append($"\n    '대행재고' AS 구분                           ");
            qry.Append($"\n  , SUM(재고금액) AS 재고금액                    ");
            qry.Append($"\n  , 4 AS id                                      ");
            qry.Append($"\n  FROM (                                         ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    null AS AtoNo                               ");
            qry.Append($"\n	  , 0 AS 재고금액                               ");
            qry.Append($"\n	  UNION ALL                                     ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    AtoNo                                       ");
            qry.Append($"\n	  , ISNULL(재고금액, 0) AS 재고금액             ");
            qry.Append($"\n	  FROM 품명별재고현황                           ");
            qry.Append($"\n	  WHERE 사용자 = 'dashboard'                    ");
            qry.Append($"\n	    AND 매입처 LIKE '%(대)%'                    ");
            qry.Append($"\n  ) AS t                                         ");
            qry.Append($"\n  UNION ALL                                      ");

            qry.Append($"\n  SELECT                                         ");
            qry.Append($"\n    '유산스' AS 구분                             ");
            qry.Append($"\n  , SUM(재고금액) AS 재고금액                    ");
            qry.Append($"\n  , 5 AS id                                      ");
            qry.Append($"\n  FROM (                                         ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    null AS AtoNo                               ");
            qry.Append($"\n	  , 0 AS 재고금액                               ");
            qry.Append($"\n	  UNION ALL                                     ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    AtoNo                                       ");
            qry.Append($"\n	  , ISNULL(재고금액, 0) AS 재고금액             ");
            qry.Append($"\n	  FROM 품명별재고현황                           ");
            qry.Append($"\n	  WHERE 사용자 = 'dashboard'                    ");
            qry.Append($"\n	    AND 매입처 LIKE '%(유)%'                    ");
            qry.Append($"\n  ) AS t                                         ");
            qry.Append($"\n  UNION ALL                                      ");

            qry.Append($"\n  SELECT                                         ");
            qry.Append($"\n    '미결제재고' AS 구분                         ");
            qry.Append($"\n  , SUM(재고금액) AS 재고금액                    ");
            qry.Append($"\n  , 6 AS id                                      ");
            qry.Append($"\n  FROM (                                         ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    null AS AtoNo                               ");
            qry.Append($"\n	  , 0 AS 재고금액                               ");
            qry.Append($"\n	  UNION ALL                                     ");
            qry.Append($"\n	  SELECT                                        ");
            qry.Append($"\n	    AtoNo                                       ");
            qry.Append($"\n	  , ISNULL(재고금액, 0) AS 재고금액             ");
            qry.Append($"\n	  FROM 품명별재고현황                           ");
            qry.Append($"\n	  WHERE 사용자 = 'dashboard'                    ");
            qry.Append($"\n	    AND 매입처 LIKE '%(일)%'                    ");
            qry.Append($"\n  ) AS t                                         ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetGuarnteeStock()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                      ");
            qry.Append($"\n   매입처                                                                                                    ");
            qry.Append($"\n , CAST(SUM(재고금액) AS char) AS 재고금액                                                                  ");
            qry.Append($"\n , CAST(SUM(재고수) AS char) AS 재고수                                                                                     ");
            qry.Append($"\n , CONVERT(CHAR(10),입고일자,23) AS 입고일자                                                                 ");
            qry.Append($"\n , AtoNo                                                                                                     ");
            qry.Append($"\n , STUFF((                                                                                                   ");
            qry.Append($"\n 	SELECT ',' + 품명 + '|' + 원산지 + '|' + 규격 + '|' + 단위 + '|' + CONVERT(NVARCHAR(10), 재고수)        ");
            qry.Append($"\n 	FROM 품명별재고현황 AS a                                                                                ");
            qry.Append($"\n 	WHERE 사용자 = '{userId}'                                                                                 ");
            qry.Append($"\n      AND a.매입처 =  b.매입처                                                                                 ");
            qry.Append($"\n      AND a.입고일자 =  b.입고일자                                                                             ");
            qry.Append($"\n      AND a.차입일자 IS NULL                                                                                      ");
            //qry.Append($"\n      AND a.매입처 LIKE '%완)%'                                                                                   ");
            qry.Append($"\n      AND a.입고일자 >= '{DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd")}'                                     ");

            qry.Append($"\n   AND 매입처 NOT LIKE '%-DL%'                                                                            ");
            qry.Append($"\n   AND 매입처 NOT LIKE '%-LT%'                                                                            ");

            qry.Append($"\n   AND 창고 NOT LIKE '%장림동양%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%해물가스%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%해물까스%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%생선가스%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%생선까스%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%피바지락%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%백합%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%자숙콩%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%파인샤베트%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%새우젓%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%두리안%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%피바지락%'                                                                            ");

            qry.Append($"\n   AND 품명 NOT LIKE '%고노와다%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%망고스틴%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%번데기%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%연어알%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%냉동딸기%'                                                                            ");

            qry.Append($"\n   AND AtoNo NOT LIKE '%DW%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%HS%'                                                                            ");

            qry.Append($"\n   AND AtoNo NOT LIKE '%AD%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%SJ%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%JD%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%OD%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%EX%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%YD%'                                                                            ");

            qry.Append($"\n    GROUP BY 매입처, 입고일자, 품명, 원산지, 규격, 단위, CONVERT(NVARCHAR(10), 재고수)                       ");
            qry.Append($"\n    FOR xml path('')), 1, 1, '') AS 품명                                                                     ");
            qry.Append($"\n FROM 품명별재고현황 AS b                                                                                    ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                   ");
            qry.Append($"\n   AND 차입일자 IS NULL                                                                                      ");
            //qry.Append($"\n   AND 매입처 LIKE '%완)%'                                                                                   ");
            qry.Append($"\n   AND 입고일자 >= '{DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd")}'                                     ");

            qry.Append($"\n   AND 매입처 NOT LIKE '%-DL%'                                                                            ");
            qry.Append($"\n   AND 매입처 NOT LIKE '%-LT%'                                                                            ");

            qry.Append($"\n   AND 창고 NOT LIKE '%장림동양%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%해물가스%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%해물까스%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%생선가스%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%생선까스%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%피바지락%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%백합%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%자숙콩%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%파인샤베트%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%새우젓%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%두리안%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%피바지락%'                                                                            ");

            qry.Append($"\n   AND 품명 NOT LIKE '%고노와다%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%망고스틴%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%번데기%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%연어알%'                                                                            ");
            qry.Append($"\n   AND 품명 NOT LIKE '%냉동딸기%'                                                                            ");

            qry.Append($"\n   AND AtoNo NOT LIKE '%DW%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%HS%'                                                                            ");

            qry.Append($"\n   AND AtoNo NOT LIKE '%AD%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%SJ%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%JD%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%OD%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%EX%'                                                                            ");
            qry.Append($"\n   AND AtoNo NOT LIKE '%YD%'                                                                            ");

            qry.Append($"\n GROUP BY 매입처, 입고일자, AtoNo                                                                                   ");
            qry.Append($"\n ORDER BY SUM(재고금액) DESC ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
    }
}
