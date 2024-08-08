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

namespace Repositories
{
    public class PriceComparisonRepository : ClassRoot, IPriceComparisonRepository
    {
        private static string userId;
        public void SetSeaoverId(string user_id)
        {
            userId = user_id;
        }
        public int CallStoredProcedure(string user_id, string sttdate, string enddate)
        {
            userId = user_id;
            int result = 0;

            SqlCommand cmd = new SqlCommand("SP_품명별매출수", (SqlConnection)dbInstance3.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            /*cmd.Parameters.AddWithValue("@draw_fymd", enddate);
            cmd.Parameters.AddWithValue("@draw_tymd", enddate);
            cmd.Parameters.AddWithValue("@proc_fish_hnm", "%asdfasdfasd%");
            cmd.Parameters.AddWithValue("@proc_size_hnm", "%asdfasdfasdf%");
            cmd.Parameters.AddWithValue("@proc_won_hnm", "%asdfaasdfasdfsa%");
            cmd.Parameters.AddWithValue("@work_id", userId);

            result = cmd.ExecuteNonQuery();*/

            /*cmd = new SqlCommand("SP_품명별매출수", (SqlConnection)dbInstance3.Connection);
            cmd.CommandType = CommandType.StoredProcedure;*/

            cmd.Parameters.AddWithValue("@draw_fymd", sttdate);
            cmd.Parameters.AddWithValue("@draw_tymd", enddate);
            cmd.Parameters.AddWithValue("@proc_fish_hnm", "%%");
            cmd.Parameters.AddWithValue("@proc_size_hnm", "%%");
            cmd.Parameters.AddWithValue("@proc_won_hnm", "%%");
            cmd.Parameters.AddWithValue("@work_id", userId);

            result = cmd.ExecuteNonQuery();
            return result;
        }
        public int CallStoredProcedureSTOCK(string user_id, string enddate)
        {
            userId = user_id;
            int result = 0;

            SqlCommand cmd = new SqlCommand("SP_품명별재고현황", (SqlConnection)dbInstance3.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@saup_cd", "%%");
            cmd.Parameters.AddWithValue("@proc_ymd", enddate);
            cmd.Parameters.AddWithValue("@fish_hnm", "%%");
            cmd.Parameters.AddWithValue("@size_hnm", "%%");
            cmd.Parameters.AddWithValue("@won_hnm", "%%");
            cmd.Parameters.AddWithValue("@cargo_hnm", "%%");
            cmd.Parameters.AddWithValue("@work_id", userId);

            result = cmd.ExecuteNonQuery();
            return result;
        }

        public DataTable GetNormalPriceList(DateTime sttDate, DateTime endDate, int company_count, string product, string origin, string sizes)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                           ");
            qry.Append($"\n   t.품명                                                                                                                           ");
            qry.Append($"\n , t.원산지                                                                                                                          ");
            qry.Append($"\n , t.규격                                                                                                                           ");
            qry.Append($"\n , t.계산단위 AS 단위                                                                                                                         ");
            qry.Append($"\n , t.매입단가 AS 일반시세                                                                                                                ");
            qry.Append($"\n , t.매입처                                                                                                                ");
            qry.Append($"\n , t.매입일자                                                                                                                ");
            qry.Append($"\n FROM(                                                                                                                            ");
            qry.Append($"\n    SELECT                                                                                                                        ");
            qry.Append($"\n 	  t.*                                                                                                                        ");
            qry.Append($"\n 	, ROW_NUMBER() OVER(PARTITION BY t.품명, t.원산지, t.규격, t.계산단위 ORDER BY t.매입단가 ASC) AS Row#                                    ");
            qry.Append($"\n 	FROM (                                                                                                                       ");
            qry.Append($"\n 		SELECT                                                                                                                   ");
            qry.Append($"\n 		  품명 AS 품명                                                                                                               ");
            qry.Append($"\n 		, 원산지 AS 원산지                                                                                                            ");
            qry.Append($"\n 		, 규격 AS 규격                                                                                                               ");
            qry.Append($"\n 		, 매입단가 AS 매입단가                                                                                                          ");
            qry.Append($"\n 		, 매입처 AS 매입처                                                                                                            ");
            qry.Append($"\n 		, CASE                                                                                                                   ");
            qry.Append($"\n 			WHEN CHARINDEX(가격단위, '묶') > 0 OR CHARINDEX(가격단위, '팩') > 0 THEN CASE WHEN CONVERT(float, 단위수량) > 0 THEN CONVERT(float, SEAOVER단위)/CONVERT(float, 단위수량) ELSE CONVERT(float, SEAOVER단위) END      ");
            qry.Append($"\n 			ELSE SEAOVER단위 END AS 계산단위                                                                                          ");
            qry.Append($"\n 		, 매입일자 AS 매입일자                                                                                                          ");
            qry.Append($"\n 		FROM 업체별시세관리                                                                                                            ");
            qry.Append($"\n 		WHERE 사용자 = '200009'                                                                                                     ");
            qry.Append($"\n 		   AND 매입단가 > 5                                                                                                          ");
            qry.Append($"\n 		   AND 매입일자 <= '{endDate.ToString("yyyy-MM-dd")}' AND 매입일자 >= '{sttDate.ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 		   {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 		   {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 		   {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n 	) AS t                                                                                                                       ");
            qry.Append($"\n ) AS t                                                                                                                           ");
            qry.Append($"\n WHERE Row# <= {company_count}                                                                                                                 ");
            qry.Append($"\n ORDER BY t.품명, t.원산지, t.규격, t.계산단위, Row# ASC                                                                                                                 ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetSumaryProductAllTerms(DateTime eDate, string product, string origin, string sizes)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                             ");
            qry.Append($"\n    t.품명                                                             ");
            qry.Append($"\n  , t.원산지                                                            ");
            qry.Append($"\n  , t.규격                                                             ");
            qry.Append($"\n  , t.단위                                                             ");
            qry.Append($"\n  , ISNULL(t1.매출수, 0) AS 매출수1                                       ");
            qry.Append($"\n  , ISNULL(t45.매출수, 0) AS 매출수45                                     ");
            qry.Append($"\n  , ISNULL(t2.매출수, 0) AS 매출수2                                       ");
            qry.Append($"\n  , ISNULL(t3.매출수, 0) AS 매출수3                                       ");
            qry.Append($"\n  , ISNULL(t6.매출수, 0) AS 매출수6                                       ");
            qry.Append($"\n  , ISNULL(t12.매출수, 0) AS 매출수12                                     ");
            qry.Append($"\n  , ISNULL(t18.매출수, 0) AS 매출수18                                     ");
            qry.Append($"\n  FROM(                                                              ");
            qry.Append($"\n     SELECT                                                      ");
            qry.Append($"\n     DISTINCT                                                    ");
            qry.Append($"\n   	  품명                                                        ");
            qry.Append($"\n   	, 원산지                                                       ");
            qry.Append($"\n   	, 규격                                                        ");
            qry.Append($"\n   	, REPLACE(단위, 'kg', '')  AS 단위                              ");
            qry.Append($"\n   	FROM 매출현황                                                   ");
            qry.Append($"\n   	WHERE 사용자 = '200009'                                        ");
            qry.Append($"\n   	  AND 매출자 <> '199990'                                       ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토무역%'                                ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토코리아%'                               ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에스제이씨푸드%'                             ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에이티오%'                                ");
            qry.Append($"\n   	  AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.AddMonths(-18).ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 	  {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 	  {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 	  {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n  ) AS t                                                             ");
            qry.Append($"\n  LEFT OUTER JOIN(                                                   ");
            qry.Append($"\n     SELECT                                                      ");
            qry.Append($"\n   	  품명                                                        ");
            qry.Append($"\n   	, 원산지                                                       ");
            qry.Append($"\n   	, 규격                                                        ");
            qry.Append($"\n   	, REPLACE(단위, 'kg', '')  AS 단위                              ");
            qry.Append($"\n   	, SUM(ISNULL(매출수, 0)) AS 매출수                                   ");
            qry.Append($"\n   	FROM 매출현황                                                   ");
            qry.Append($"\n   	WHERE 사용자 = '200009'                                        ");
            qry.Append($"\n   	  AND 매출자 <> '199990'                                       ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토무역%'                                ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토코리아%'                               ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에스제이씨푸드%'                             ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에이티오%'                                ");
            qry.Append($"\n   	  AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.AddMonths(-1).ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 	  {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 	  {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 	  {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n   	GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                    ");
            qry.Append($"\n  ) AS t1                                                            ");
            qry.Append($"\n    ON t.품명 = t1.품명                                                  ");
            qry.Append($"\n   AND t.원산지 = t1.원산지                                               ");
            qry.Append($"\n   AND t.규격 = t1.규격                                                  ");
            qry.Append($"\n   AND REPLACE(t.단위, 'kg', '') = REPLACE(t1.단위, 'kg', '')            ");
            qry.Append($"\n  LEFT OUTER JOIN(                                                   ");
            qry.Append($"\n     SELECT                                                      ");
            qry.Append($"\n   	  품명                                                        ");
            qry.Append($"\n   	, 원산지                                                       ");
            qry.Append($"\n   	, 규격                                                        ");
            qry.Append($"\n   	, REPLACE(단위, 'kg', '')  AS 단위                              ");
            qry.Append($"\n   	, SUM(ISNULL(매출수, 0)) AS 매출수                                   ");
            qry.Append($"\n   	FROM 매출현황                                                   ");
            qry.Append($"\n   	WHERE 사용자 = '200009'                                        ");
            qry.Append($"\n   	  AND 매출자 <> '199990'                                       ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토무역%'                                ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토코리아%'                               ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에스제이씨푸드%'                             ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에이티오%'                                ");
            qry.Append($"\n   	  AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.AddDays(-45).ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 	  {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 	  {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 	  {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n   	GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                    ");
            qry.Append($"\n  ) AS t45                                                           ");
            qry.Append($"\n    ON t.품명 = t45.품명                                                 ");
            qry.Append($"\n   AND t.원산지 = t45.원산지                                              ");
            qry.Append($"\n   AND t.규격 = t45.규격                                                 ");
            qry.Append($"\n   AND REPLACE(t.단위, 'kg', '') = REPLACE(t45.단위, 'kg', '')           ");
            qry.Append($"\n  LEFT OUTER JOIN(                                                   ");
            qry.Append($"\n     SELECT                                                      ");
            qry.Append($"\n   	  품명                                                        ");
            qry.Append($"\n   	, 원산지                                                       ");
            qry.Append($"\n   	, 규격                                                        ");
            qry.Append($"\n   	, REPLACE(단위, 'kg', '')  AS 단위                              ");
            qry.Append($"\n   	, SUM(ISNULL(매출수, 0)) AS 매출수                                   ");
            qry.Append($"\n   	FROM 매출현황                                                   ");
            qry.Append($"\n   	WHERE 사용자 = '200009'                                        ");
            qry.Append($"\n   	  AND 매출자 <> '199990'                                       ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토무역%'                                ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토코리아%'                               ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에스제이씨푸드%'                             ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에이티오%'                                ");
            qry.Append($"\n   	  AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.AddMonths(-2).ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 	  {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 	  {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 	  {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n   	GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                    ");
            qry.Append($"\n  ) AS t2                                                            ");
            qry.Append($"\n    ON t.품명 = t2.품명                                                  ");
            qry.Append($"\n   AND t.원산지 = t2.원산지                                               ");
            qry.Append($"\n   AND t.규격 = t2.규격                                                  ");
            qry.Append($"\n   AND REPLACE(t.단위, 'kg', '') = REPLACE(t2.단위, 'kg', '') 	        ");
            qry.Append($"\n  LEFT OUTER JOIN(                                                   ");
            qry.Append($"\n     SELECT                                                      ");
            qry.Append($"\n   	  품명                                                        ");
            qry.Append($"\n   	, 원산지                                                       ");
            qry.Append($"\n   	, 규격                                                        ");
            qry.Append($"\n   	, REPLACE(단위, 'kg', '')  AS 단위                              ");
            qry.Append($"\n   	, SUM(ISNULL(매출수, 0)) AS 매출수                                   ");
            qry.Append($"\n   	FROM 매출현황                                                   ");
            qry.Append($"\n   	WHERE 사용자 = '200009'                                        ");
            qry.Append($"\n   	  AND 매출자 <> '199990'                                       ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토무역%'                                ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토코리아%'                               ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에스제이씨푸드%'                             ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에이티오%'                                ");
            qry.Append($"\n   	  AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.AddMonths(-3).ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 	  {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 	  {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 	  {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n   	GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                    ");
            qry.Append($"\n  ) AS t3                                                            ");
            qry.Append($"\n    ON t.품명 = t3.품명                                                  ");
            qry.Append($"\n   AND t.원산지 = t3.원산지                                               ");
            qry.Append($"\n   AND t.규격 = t3.규격                                                  ");
            qry.Append($"\n   AND REPLACE(t.단위, 'kg', '') = REPLACE(t3.단위, 'kg', '') 	        ");
            qry.Append($"\n  LEFT OUTER JOIN(                                                   ");
            qry.Append($"\n     SELECT                                                      ");
            qry.Append($"\n   	  품명                                                        ");
            qry.Append($"\n   	, 원산지                                                       ");
            qry.Append($"\n   	, 규격                                                        ");
            qry.Append($"\n   	, REPLACE(단위, 'kg', '')  AS 단위                              ");
            qry.Append($"\n   	, SUM(ISNULL(매출수, 0)) AS 매출수                                   ");
            qry.Append($"\n   	FROM 매출현황                                                   ");
            qry.Append($"\n   	WHERE 사용자 = '200009'                                        ");
            qry.Append($"\n   	  AND 매출자 <> '199990'                                       ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토무역%'                                ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토코리아%'                               ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에스제이씨푸드%'                             ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에이티오%'                                ");
            qry.Append($"\n   	  AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.AddMonths(-6).ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 	  {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 	  {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 	  {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n   	GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                    ");
            qry.Append($"\n  ) AS t6                                                            ");
            qry.Append($"\n    ON t.품명 = t6.품명                                                  ");
            qry.Append($"\n   AND t.원산지 = t6.원산지                                               ");
            qry.Append($"\n   AND t.규격 = t6.규격                                                  ");
            qry.Append($"\n   AND REPLACE(t.단위, 'kg', '') = REPLACE(t6.단위, 'kg', '') 	        ");
            qry.Append($"\n  LEFT OUTER JOIN(                                                   ");
            qry.Append($"\n     SELECT                                                      ");
            qry.Append($"\n   	  품명                                                        ");
            qry.Append($"\n   	, 원산지                                                       ");
            qry.Append($"\n   	, 규격                                                        ");
            qry.Append($"\n   	, REPLACE(단위, 'kg', '')  AS 단위                              ");
            qry.Append($"\n   	, SUM(ISNULL(매출수, 0)) AS 매출수                                   ");
            qry.Append($"\n   	FROM 매출현황                                                   ");
            qry.Append($"\n   	WHERE 사용자 = '200009'                                        ");
            qry.Append($"\n   	  AND 매출자 <> '199990'                                       ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토무역%'                                ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토코리아%'                               ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에스제이씨푸드%'                             ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에이티오%'                                ");
            qry.Append($"\n   	  AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.AddMonths(-12).ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 	  {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 	  {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 	  {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n   	GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                    ");
            qry.Append($"\n  ) AS t12                                                           ");
            qry.Append($"\n    ON t.품명 = t12.품명                                                 ");
            qry.Append($"\n   AND t.원산지 = t12.원산지                                              ");
            qry.Append($"\n   AND t.규격 = t12.규격                                                 ");
            qry.Append($"\n   AND REPLACE(t.단위, 'kg', '') = REPLACE(t12.단위, 'kg', '') 	        ");
            qry.Append($"\n  LEFT OUTER JOIN(                                                   ");
            qry.Append($"\n     SELECT                                                      ");
            qry.Append($"\n   	  품명                                                        ");
            qry.Append($"\n   	, 원산지                                                       ");
            qry.Append($"\n   	, 규격                                                        ");
            qry.Append($"\n   	, REPLACE(단위, 'kg', '')  AS 단위                              ");
            qry.Append($"\n   	, SUM(ISNULL(매출수, 0)) AS 매출수                                   ");
            qry.Append($"\n   	FROM 매출현황                                                   ");
            qry.Append($"\n   	WHERE 사용자 = '200009'                                        ");
            qry.Append($"\n   	  AND 매출자 <> '199990'                                       ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토무역%'                                ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%아토코리아%'                               ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에스제이씨푸드%'                             ");
            qry.Append($"\n   	  AND 매출처 NOT LIKE '%에이티오%'                                ");
            qry.Append($"\n   	  AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.AddMonths(-18).ToString("yyyy-MM-dd")}'        ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n 	  {whereSql("품명", product)}                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n 	  {whereSql("원산지", origin)}                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n 	  {whereSql("규격", sizes)}                                      ");
            qry.Append($"\n   	GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                    ");
            qry.Append($"\n  ) AS t18                                                           ");
            qry.Append($"\n    ON t.품명 = t18.품명                                                 ");
            qry.Append($"\n   AND t.원산지 = t18.원산지                                              ");
            qry.Append($"\n   AND t.규격 = t18.규격                                                 ");
            qry.Append($"\n   AND REPLACE(t.단위, 'kg', '') = REPLACE(t18.단위, 'kg', '') 	        ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetNotSalesCostProduct(string product, string origin, string sizes, string unit, string[] sub_product = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                    ");
            qry.Append($"\n    t1.품명                                                                                                  ");
            qry.Append($"\n  , t1.원산지                                                                                                 ");
            qry.Append($"\n  , t1.규격                                                                                                  ");
            qry.Append($"\n  , t1.단위                                                                                                  ");
            qry.Append($"\n  , CASE WHEN ISNULL(t1.매출원가, 0) = 0 THEN CASE WHEN CHARINDEX('JD', t1.AtoNo) > 0 OR CHARINDEX('jd', t1.AtoNo) > 0 THEN 0 ELSE ISNULL(t2.매입금액, 0) END ELSE ISNULL(t1.매출원가, 0) END 매출원가 ");
            qry.Append($"\n  , t1.수량                                                                                                  ");
            qry.Append($"\n  , t1.AtoNo                                                                                                ");
            qry.Append($"\n  , t1.입고일자                                                                                                ");
            qry.Append($"\n  , t1.매입처                                                                                                 ");
            qry.Append($"\n  , t2.매입금액                                                                                                ");
            qry.Append($"\n  , 0.00 AS 환율                                                                                             ");
            qry.Append($"\n  , 0.00 AS 오퍼가                                                                                            ");
            qry.Append($"\n  , 0 AS trq                                                                                                ");
            qry.Append($"\n  , 0 AS custom_id                                                                                          ");
            qry.Append($"\n  , 0 AS sub_id                                                                                             ");
            qry.Append($"\n  , '0000-00-00' AS etd                                                                                     ");
            qry.Append($"\n  , 'FALSE' AS isPendingCalculate                                                                       ");
            qry.Append($"\n  FROM (                                                                                                    ");
            qry.Append($"\n	  SELECT                                                                                                   ");
            qry.Append($"\n	    품명                                                                                                    ");
            qry.Append($"\n	  , 원산지                                                                                                   ");
            qry.Append($"\n	  , 규격                                                                                                    ");
            qry.Append($"\n	  , REPLACE(단위, 'kg', '') AS 단위                                                                           ");
            qry.Append($"\n	  , CASE WHEN 매출원가 = 0 OR 매출원가 IS NULL THEN ISNULL(매출원가, 0)                                             ");
            qry.Append($"\n	  		WHEN 매출단가 = 0 OR 매출단가 IS NULL THEN ISNULL(매출원가, 0)                                              ");
            qry.Append($"\n	   		WHEN (매출단가 - 매출원가) / 매출원가 <= 5  THEN ISNULL(매출원가, 0)                                           ");
            qry.Append($"\n	   		ELSE 0 END AS 매출원가                                                                                ");
            qry.Append($"\n	  , 재고수 AS 수량                                                                                             ");
            qry.Append($"\n	  , AtoNo                                                                                                  ");
            qry.Append($"\n	  , 입고일자                                                                                                  ");
            qry.Append($"\n	  , 매입처                                                                                                   ");
            qry.Append($"\n	  FROM 품명별재고현황                                                                                           ");
            qry.Append($"\n	  WHERE 사용자 = '{userId}'                                                                                       ");
            /*qry.Append($"\n    AND CASE WHEN 매출원가 = 0 OR 매출원가 IS NULL THEN ISNULL(매출원가, 0)        ");
            qry.Append($"\n  		WHEN 매출단가 = 0 OR 매출단가 IS NULL THEN ISNULL(매출원가, 0)            ");
            qry.Append($"\n   		WHEN (매출단가 - 매출원가) / 매출원가 <= 5  THEN ISNULL(매출원가, 0)      ");
            qry.Append($"\n   		ELSE 0 END = 0                                                            ");*/
            if (sub_product == null || sub_product.Length == 0)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append(whereSql("품명", product.Trim()));
                if (!string.IsNullOrEmpty(origin))
                    qry.Append(whereSql("원산지", origin.Trim()));
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append(whereSql("규격", sizes.Trim()));
            }
            else
            {

                qry.Append($"\n    AND (         ");
                for (int i = 0; i < sub_product.Length; i++)
                {
                    if (i == 0)
                        qry.Append($"\n    (         ");
                    else
                        qry.Append($"\n    OR (         ");

                    string[] products = sub_product[i].Trim().Split('^');
                    if (products.Length > 5)
                    {
                        qry.Append($"\n    품명 = '{products[0]}'         ");
                        qry.Append($"\n    AND 원산지 = '{products[1]}'         ");
                        qry.Append($"\n    AND 규격 = '{products[2]}'         ");
                        qry.Append($"\n    AND REPLACE(단위, 'kg', '') = '{products[6]}'         ");
                        qry.Append($"\n    )         ");
                    }
                }
                qry.Append($"\n    )         ");
            }
            qry.Append($"\n  ) AS t1                                                                                                   ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                         ");
            qry.Append($"\n      SELECT                                                                                                ");
            qry.Append($"\n	    t.*                                                                                                    ");
            qry.Append($"\n	  FROM (                                                                                                   ");
            qry.Append($"\n		  SELECT                                                                                               ");
            qry.Append($"\n		    품명                                                   ");
            qry.Append($"\n		  , 원산지                                                  ");
            qry.Append($"\n		  , 규격                                                   ");
            qry.Append($"\n		  , REPLACE(단위,'kg','') AS 단위                            ");
            qry.Append($"\n		  , 매입처                                                  ");
            qry.Append($"\n		  , 매입금액 / 매출수 AS 매입금액                                 ");
            qry.Append($"\n		  , 매출일자                                                 ");
            qry.Append($"\n		  , ROW_NUMBER() OVER ( PARTITION BY 품명, 원산지, 규격, 단위, 매입처 ORDER BY 매입처, 매출일자 DESC ) AS #row        ");
            qry.Append($"\n		  FROM 매출현황                                                                                           ");
            qry.Append($"\n		  WHERE 사용자 = '200009'                                                                                ");
            qry.Append($"\n		    AND (매입금액 / 매출수) > 0                                                                               ");
            qry.Append($"\n		    AND 입고일자 >= '{new DateTime(DateTime.Now.AddYears(-1).Year, 1,1).ToString("yyyy-MM-dd")}'         ");
            if (sub_product == null || sub_product.Length == 0)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append(whereSql("품명", product.Trim()));
                if (!string.IsNullOrEmpty(origin))
                    qry.Append(whereSql("원산지", origin.Trim()));
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append(whereSql("규격", sizes.Trim()));
            }
            else
            {

                qry.Append($"\n    AND (         ");
                for (int i = 0; i < sub_product.Length; i++)
                {
                    if (i == 0)
                        qry.Append($"\n    (         ");
                    else
                        qry.Append($"\n    OR (         ");

                    string[] products = sub_product[i].Trim().Split('^');
                    if (products.Length > 5)
                    {
                        qry.Append($"\n    품명 = '{products[0]}'         ");
                        qry.Append($"\n    AND 원산지 = '{products[1]}'         ");
                        qry.Append($"\n    AND 규격 = '{products[2]}'         ");
                        qry.Append($"\n    AND REPLACE(단위, 'kg', '') = '{products[6]}'         ");
                        qry.Append($"\n    )         ");
                    }
                }
                qry.Append($"\n    )         ");
            }
            qry.Append($"\n	  ) AS t                                                                                                   ");
            qry.Append($"\n	  WHERE t.#row = 1                                                                                         ");
            qry.Append($"\n  ) AS t2                                                                                                   ");
            qry.Append($"\n    ON t1.품명 = t2.품명                                                                                       ");
            qry.Append($"\n    AND t1.원산지 = t2.원산지                                                                                   ");
            qry.Append($"\n    AND t1.규격 = t2.규격                                                                                      ");
            qry.Append($"\n    AND REPLACE(t1.단위,'kg','') = REPLACE(t2.단위,'kg','')                                                    ");
            qry.Append($"\n    AND t1.매입처 = t2.매입처                                                                                   ");
            qry.Append($"\n    AND t1.입고일자 <= t2.매출일자                                                                                ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetNotSalesCostProduct2(string product, string origin, string sizes, string unit, string[] sub_product = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                           ");
            qry.Append($"\n    품명                                                                           ");
            qry.Append($"\n  , 원산지                                                                         ");
            qry.Append($"\n  , 규격                                                                           ");
            qry.Append($"\n  , REPLACE(단위, 'kg', '') AS 단위                                                ");
            qry.Append($"\n  , CASE WHEN 매출원가 = 0 OR 매출원가 IS NULL THEN ISNULL(매출원가, 0)            ");
            qry.Append($"\n  		WHEN 매출단가 = 0 OR 매출단가 IS NULL THEN ISNULL(매출원가, 0)            ");
            qry.Append($"\n   		WHEN (매출단가 - 매출원가) / 매출원가 <= 5  THEN ISNULL(매출원가, 0)      ");
            qry.Append($"\n   		ELSE 0 END AS 매출원가                                                    ");
            qry.Append($"\n  , 재고수 AS 수량                                                                 ");
            qry.Append($"\n  , AtoNo                                                                          ");
            qry.Append($"\n  , 입고일자                                                                       ");
            qry.Append($"\n  , 0 AS 환율                                                                      ");
            qry.Append($"\n  , '0000-00-00' AS etd                                                                      ");
            qry.Append($"\n  FROM 품명별재고현황                                                              ");
            qry.Append($"\n  WHERE 사용자 = '{userId}'                                                        ");
            qry.Append($"\n    AND CASE WHEN 매출원가 = 0 OR 매출원가 IS NULL THEN ISNULL(매출원가, 0)        ");
            qry.Append($"\n  		WHEN 매출단가 = 0 OR 매출단가 IS NULL THEN ISNULL(매출원가, 0)            ");
            qry.Append($"\n   		WHEN (매출단가 - 매출원가) / 매출원가 <= 5  THEN ISNULL(매출원가, 0)      ");
            qry.Append($"\n   		ELSE 0 END > 0                                                            ");
            if (sub_product == null || sub_product.Length == 0)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append(whereSql("품명", product.Trim()));
                if (!string.IsNullOrEmpty(origin))
                    qry.Append(whereSql("원산지", origin.Trim()));
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append(whereSql("규격", sizes.Trim()));
            }
            else
            {
                    qry.Append($"\n    AND (         ");
                for (int i = 0; i < sub_product.Length; i++)
                {
                    if (i == 0)
                        qry.Append($"\n    (         ");
                    else
                        qry.Append($"\n    OR (         ");

                    string[] products = sub_product[i].Trim().Split('^');
                    if (products.Length > 5)
                    {
                        qry.Append($"\n    품명 = '{products[0]}'         ");
                        qry.Append($"\n    AND 원산지 = '{products[1]}'         ");
                        qry.Append($"\n    AND 규격 = '{products[2]}'         ");
                        qry.Append($"\n    AND REPLACE(단위, 'kg', '') = '{products[6]}'         ");
                        qry.Append($"\n    )         ");
                    }
                }
                qry.Append($"\n    )         ");
            }
            qry.Append($"\n  GROUP BY 품명, 원산지, 규격, 단위, AtoNo, 입고일자, 재고수                        ");
            qry.Append($"\n  	, CASE WHEN 매출원가 = 0 OR 매출원가 IS NULL THEN ISNULL(매출원가, 0)          ");
            qry.Append($"\n  		WHEN 매출단가 = 0 OR 매출단가 IS NULL THEN ISNULL(매출원가, 0)             ");
            qry.Append($"\n   		WHEN (매출단가 - 매출원가) / 매출원가 <= 5  THEN ISNULL(매출원가, 0)       ");
            qry.Append($"\n   		ELSE 0 END                                                                 ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }


        public DataTable GetCostAccountingProductInfo(string product, string origin, string sizes, int sales_terms = 6)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                            ");
            qry.Append($"\n   품명                                                                                                                              ");
            qry.Append($"\n , 원산지                                                                                                                             ");
            qry.Append($"\n , 규격                                                                                                                              ");
            qry.Append($"\n , 단위                                                                                                                              ");
            qry.Append($"\n , SUM(재고수) AS 재고수                                                                                                                ");
            qry.Append($"\n , SUM(매출수) AS 매출수                                                                                                                ");
            qry.Append($"\n FROM(                                                                                                                             ");
            qry.Append($"\n 	SELECT                                                                                                                        ");
            qry.Append($"\n 	  품명                                                                                                                          ");
            qry.Append($"\n 	, 원산지                                                                                                                         ");
            qry.Append($"\n 	, 규격                                                                                                                          ");
            qry.Append($"\n 	, 단위                                                                                                                           ");
            qry.Append($"\n 	, SUM(재고수) AS 재고수                                                                                                       ");
            qry.Append($"\n 	, SUM(매출수) AS 매출수                                                                                                  ");
            qry.Append($"\n 	FROM(                                                                                                                         ");
            qry.Append($"\n 		SELECT                                                                                                                    ");
            qry.Append($"\n 		  품명                                                                                                                      ");
            qry.Append($"\n 		, 원산지                                                                                                                     ");
            qry.Append($"\n 		, 규격                                                                                                                       ");
            qry.Append($"\n         , REPLACE(단위, 'kg', '') AS 단위                                                                                            ");
            qry.Append($"\n 		, SUM(ISNULL(재고수, 0) - ISNULL(예약수, 0)) AS 재고수                                                                       ");
            qry.Append($"\n 		, 0 AS 매출수                                                                                                                ");
            qry.Append($"\n 		FROM 품명별재고현황 WHERE 사용자 = '{userId}'                                                                                ");
            qry.Append($"\n 		  AND ISNUMERIC(REPLACE(단위, 'kg', '')) = 1                                                                                 ");
            qry.Append($"\n 		  AND (매입처 NOT LIKE '%-DL%' OR 매입처 NOT LIKE '%-LT%')                                                                   ");
            qry.Append($"\n 		  AND ((적요 NOT LIKE '%제외%' AND 적요 NOT LIKE '%back%' AND 적요 NOT LIKE '%백쉽%' AND 적요 NOT LIKE '%빽쉽%') OR 적요 IS NULL)       ");

            

            if (!string.IsNullOrEmpty(product))
                qry.Append(whereSql("품명", product.Trim()));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(whereSql("원산지", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(whereSql("규격", sizes.Trim()));
            qry.Append($"\n 		GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                                                                      ");
            qry.Append($"\n 		UNION ALL                                                                                                                 ");
            qry.Append($"\n 		SELECT                                                                                            ");
            qry.Append($"\n 		  품명                                                                                            ");
            qry.Append($"\n 		, 원산지                                                                                          ");
            qry.Append($"\n 		, 규격                                                                                            ");
            qry.Append($"\n         , REPLACE(단위, 'kg', '') AS 단위                                                                 ");
            qry.Append($"\n 		, 0 AS 재고수                                                                                     ");
            qry.Append($"\n 		, SUM(ISNULL(매출수, 0)) AS 매출수                                                                ");
            qry.Append($"\n 		FROM 매출현황                                                                                     ");
            qry.Append($"\n 		WHERE 사용자 = '200009'                                                                           ");
            qry.Append($"\n 		  AND 매출자 <> '199990'                                                                          ");
            qry.Append($"\n 		  AND 매출처 NOT LIKE '%아토무역%'                                                                ");
            qry.Append($"\n 		  AND 매출처 NOT LIKE '%아토코리아%'                                                              ");
            qry.Append($"\n 		  AND 매출처 NOT LIKE '%에이티오%'                                                                ");
            qry.Append($"\n 		  AND 매출처 NOT LIKE '%에스제이씨푸드%'                                                          ");
            qry.Append($"\n 		  AND ISNUMERIC(REPLACE(단위, 'kg', '')) = 1                                                      ");
            DateTime sttdate = DateTime.Now.AddMonths(-sales_terms);
            if(sales_terms == 45)
                sttdate = DateTime.Now.AddDays(-sales_terms);
            qry.Append($"\n 		  AND 매출일자 <= '{DateTime.Now.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{sttdate.ToString("yyyy-MM-dd")}'                                            ");
            if (!string.IsNullOrEmpty(product))
                qry.Append(whereSql("품명", product.Trim()));
            if (!string.IsNullOrEmpty(origin))
                qry.Append(whereSql("원산지", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes))
                qry.Append(whereSql("규격", sizes.Trim()));
            qry.Append($"\n 		GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                                                                      ");
            qry.Append($"\n 	) AS t                                                                                                                        ");
            qry.Append($"\n 	WHERE ISNUMERIC(단위) = 1                                                                                                     ");
            qry.Append($"\n 	GROUP BY 품명, 원산지, 규격, 단위                                                                                             ");
            qry.Append($"\n ) AS t                                                                                                                            ");
            qry.Append($"\n GROUP BY 품명, 원산지, 규격, 단위                                                                                                 ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }


        public DataTable GetSoldOutProduct()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                             ");
            qry.Append($"\n   품명코드                                                      ");
            qry.Append($"\n , 품명                                                          ");
            qry.Append($"\n , 규격코드                                                      ");
            qry.Append($"\n , 규격                                                          ");
            qry.Append($"\n , 원산지코드                                                    ");
            qry.Append($"\n , 원산지                                                        ");
            qry.Append($"\n , REPLACE(REPLACE(단위, 'KG', ''), ' ', '') AS 단위             ");
            qry.Append($"\n , SUM(ISNULL(재고수,0)) AS 재고수                                         ");
            qry.Append($"\n , SUM(ISNULL(예약수,0)) AS 예약수                                         ");
            qry.Append($"\n , STUFF((                                                                                                      ");
            qry.Append($"\n  	SELECT ',' + ISNULL(예약상세, '')                                                                              ");
            qry.Append($"\n  	FROM 품명별재고현황 AS a                                                                                       ");
            qry.Append($"\n  	WHERE 사용자 = '{userId}'                                                                                       ");
            qry.Append($"\n       AND ISNULL(품명, '') =  ISNULL(a.품명, '')                                                                   ");
            qry.Append($"\n 	  AND ISNULL(원산지, '') =  ISNULL(a.원산지, '')                                                                ");
            qry.Append($"\n 	  AND ISNULL(규격, '') =  ISNULL(a.규격, '')                                                                   ");
            qry.Append($"\n 	  AND ISNULL(단위, '') =  ISNULL(a.단위, '')                                                                   ");
            qry.Append($"\n     GROUP BY 예약상세                                                                                              ");
            qry.Append($"\n     FOR xml path('')), 1, 1, '') AS 예약상세                                                                       ");
            qry.Append($"\n FROM 품명별재고현황                                             ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                         ");
            qry.Append($"\n GROUP BY 품명코드, 품명, 규격코드, 규격, 원산지코드, 원산지, REPLACE(REPLACE(단위, 'KG', ''), ' ', '')                                            ");
            string sql = qry.ToString();

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public List<PriceComparisonModel> GetPriceComparison(int avgCnt, string category, string product, string origin, string sizes, string unit, string manager1, string manager2
                                                            , string division, bool stock, bool sales, DateTime editSttdate, DateTime editEnddate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                            ");
            qry.Append($"\n    t.*                                                                                                                         ");
            qry.Append($"\n  FROM (                                                                                                                        ");
            qry.Append($"\n  SELECT                                                                                                                        ");
            qry.Append($"\n    t1.*                                                                                                                        ");
            qry.Append($"\n  , ISNULL(t2.매입단가, 0) AS 일반시세                                                                                                     ");
            qry.Append($"\n  , CASE                                                                                                                        ");
            qry.Append($"\n       WHEN t1.규격2 = '.' THEN NULL                                                                                            ");
            qry.Append($"\n  	  WHEN t1.규격2 = '-' THEN NULL                                                                                            ");
            qry.Append($"\n  	  WHEN ISNUMERIC(t1.규격2) = 1 THEN CONVERT(float, t1.규격2)                                                               ");
            qry.Append($"\n  	  ELSE NULL                                                                                                                ");
            qry.Append($"\n    END AS 규격3                                                                                                                ");
            qry.Append($"\n  , CASE                                                                                                                        ");
            qry.Append($"\n  	  WHEN t1.규격2 = '.' THEN t1.규격2                                                                                        ");
            qry.Append($"\n  	  WHEN t1.규격2 = '-' THEN t1.규격2                                                                                        ");
            qry.Append($"\n  	  WHEN ISNUMERIC(t1.규격2) = 1 THEN NULL                                                                                   ");
            qry.Append($"\n  	  ELSE t1.규격2                                                                                                            ");
            qry.Append($"\n    END AS 규격4                                                                                                                ");
            //qry.Append($"\n , t2.담당자1                                                                                                                   ");
            //qry.Append($"\n , t2.담당자2                                                                                                                   ");
            qry.Append($"\n FROM(                                                                                                                          ");
            qry.Append($"\n     SELECT                                                                                                                         ");
            qry.Append($"\n     CASE                                                                                                                           ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Aa' THEN '라운드새우류'                                                     ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ab' THEN '새우살류'                                                         ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ba' THEN '쭈꾸미류'                                                         ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bb' THEN '낙지류'                                                           ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bc' THEN '갑오징어류'                                                       ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bd' THEN '오징어류'                                                         ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Be' THEN '문어류'                                                           ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'C' THEN '갑각류'                                                            ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Da' THEN '어패류'                                                           ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Db' THEN '살류'                                                             ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Dc' THEN '해물류'                                                           ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ea' THEN '초밥/일식류'                                                      ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Eb' THEN '기타가공품(튀김, 가금류)'                                         ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ec' THEN '기타가공품(야채, 과일)'                                           ");
            qry.Append($"\n     	WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'F' THEN '선어류'                                                            ");
            qry.Append($"\n     END AS 대분류                                                                                                                  ");
            qry.Append($"\n     ,  SUBSTRING(대분류, 0, 3) AS 대분류2                                                                                          ");
            qry.Append($"\n     ,  SUBSTRING(대분류, 3, 3) AS 대분류3                                                                                          ");
            qry.Append($"\n     , t2.대분류 AS 대분류코드                                                                                                      ");
            qry.Append($"\n     , t1.품명                                                                                                                      ");
            qry.Append($"\n     , t1.원산지                                                                                                                    ");
            qry.Append($"\n     , t2.단위                                                                                                                      ");
            qry.Append($"\n     , t1.규격                                                                                                                      ");
            qry.Append($"\n     , CASE                                                                                                                         ");
            qry.Append($"\n       WHEN CHARINDEX('미', t1.규격) > 0 AND CHARINDEX('/', t1.규격) > 0 THEN                                                       ");
            qry.Append($"\n     	CASE                                                                                                                       ");
            qry.Append($"\n      	  WHEN CHARINDEX('미', t1.규격) > CHARINDEX('/', t1.규격) THEN  SUBSTRING(t1.규격, 0, CHARINDEX('/', t1.규격))             ");
            qry.Append($"\n      	  WHEN CHARINDEX('미', t1.규격) < CHARINDEX('/', t1.규격) THEN  SUBSTRING(t1.규격, 0, CHARINDEX('미', t1.규격))            ");
            qry.Append($"\n     	END                                                                                                                        ");
            qry.Append($"\n     	  WHEN CHARINDEX('미', t1.규격) > 0 THEN SUBSTRING(t1.규격, 0, CHARINDEX('미', t1.규격))                                   ");
            qry.Append($"\n     	  WHEN CHARINDEX('/', t1.규격) > 0 THEN SUBSTRING(t1.규격, 0, CHARINDEX('/', t1.규격))                                     ");
            qry.Append($"\n     	ELSE t1.규격                                                                                                               ");
            qry.Append($"\n     	END AS 규격2                                                                                                               ");
            qry.Append($"\n     , t2.단위수량                                                                                                                  ");
            qry.Append($"\n     , t2.가격단위                                                                                                                  ");
            qry.Append($"\n     , t2.SEAOVER단위                                                                                                               ");
            qry.Append($"\n     , ISNULL(t1.재고합계, 0) AS 재고합계                                                                                           ");
            qry.Append($"\n     , ISNULL(t1.예약수, 0) AS 예약수                                                                                               ");
            qry.Append($"\n     , t1.예약상세                                                                                                                  ");
            qry.Append($"\n     , ISNULL(t1.매출수, 0) AS 매출수                                                                                               ");
            qry.Append($"\n     , ISNULL(t2.매입단가, 0) AS 매입단가                                                                                           ");
            qry.Append($"\n     , ISNULL(t2.매출단가, 0) AS 매출단가                                                                                           ");
            qry.Append($"\n     , t2.구분                                                                                                                      ");
            qry.Append($"\n     FROM (                                                                                                                         ");
            qry.Append($"\n      SELECT                                                                                                                        ");
            qry.Append($"\n        품명                                                                                                                        ");
            qry.Append($"\n      , 원산지                                                                                                                      ");
            qry.Append($"\n      , 규격                                                                                                                        ");
            qry.Append($"\n      , SUM(매출수) AS 매출수                                                                                                       ");
            qry.Append($"\n      , SUM(매출금액) AS 매출금액                                                                                                   ");
            qry.Append($"\n      , SUM(재고합계) AS 재고합계                                                                                                   ");
            qry.Append($"\n      , SUM(예약수) AS 예약수                                                                                                       ");
            qry.Append($"\n      , STUFF((                                                                                                                     ");
            qry.Append($"\n 	     	SELECT '/' + 예약상세                                                                                                  ");
            qry.Append($"\n 	     	FROM 품명별매출현황                                                                                                    ");
            qry.Append($"\n 	     	WHERE 사용자 = '{userId}'                                                                                                    ");
            qry.Append($"\n 	     	 AND 품명 = a.품명                                                                                                    ");
            qry.Append($"\n 		     AND 원산지 = a.원산지                                                                                                 ");
            qry.Append($"\n 		     AND 규격 = a.규격	                                                                                                   ");
            qry.Append($"\n 		    GROUP BY 예약상세                                                                                                      ");
            qry.Append($"\n 		    FOR xml path('')), 1, 1, '') AS 예약상세                                                                               ");
            qry.Append($"\n      FROM 품명별매출현황 a                                                                                                         ");
            qry.Append($"\n     WHERE 사용자 = '{userId}'                                                                                                                      ");

            string[] tempStr = null;
            string tempWhr = "";
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = product.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   품명 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 품명 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 품명 LIKE '%{product}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = origin.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   원산지 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 원산지 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 원산지 LIKE '%{origin}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = sizes.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   규격 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 규격 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 규격 LIKE '%{sizes}%'                                                                         ");
                }
            }
            qry.Append($"\n      GROUP BY 품명, 원산지, 규격                              ");
            qry.Append($"\n ) AS t1                                                                                                      ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                           ");
            qry.Append($"\n 	 SELECT                                                                                                                 ");
            qry.Append($"\n 	   대분류                                                                                                               ");
            qry.Append($"\n 	 , 품명                                                                                                                 ");
            qry.Append($"\n 	 , 원산지                                                                                                               ");
            qry.Append($"\n 	 , 규격                                                                                                                 ");
            qry.Append($"\n 	 , 단위                                                                                                                 ");
            qry.Append($"\n 	 , 단위수량                                                                                                             ");
            qry.Append($"\n 	 , 가격단위                                                                                                             ");
            qry.Append($"\n 	 , MIN(매입단가) AS 매입단가                                                                                            ");
            qry.Append($"\n 	 , 매출단가                                                                                                             ");
            qry.Append($"\n 	 , SEAOVER단위                                                                                                          ");

            qry.Append($"\n 	 , 구분                                                                                                                 ");
            qry.Append($"\n 	 FROM 업체별시세관리                                                                                                    ");
            qry.Append($"\n      WHERE 사용자 = '{userId}'                                                                                                              ");
            qry.Append($"\n        AND 매입단가 > 5                                                                                                     ");
            /*qry.Append($"\n        AND 매입일자 >= '{editSttdate.ToString("yyyy-MM-dd")}'                                                             ");
            qry.Append($"\n        AND 매입일자 <= '{editEnddate.ToString("yyyy-MM-dd")}'                                                             ");*/

            tempStr = null;
            tempWhr = "";
            if (!string.IsNullOrEmpty(category.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = category.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   대분류 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 대분류 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 대분류 LIKE '%{category}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = product.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   품명 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 품명 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 품명 LIKE '%{product}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = origin.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   원산지 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 원산지 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 원산지 LIKE '%{origin}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = sizes.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   규격 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 규격 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 규격 LIKE '%{sizes}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(unit.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = unit.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   단위 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 단위 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 단위 LIKE '%{unit}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(manager1.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = manager1.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   담당자1 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 담당자1 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 담당자1 LIKE '%{manager1}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(manager2.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = manager2.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   담당자2 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 담당자2 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 담당자2 LIKE '%{manager2}%'                                                                         ");
                }
            }
            if (!string.IsNullOrEmpty(division.Trim()))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = division.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   구분 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 구분 LIKE '%{tempStr[i]}%' ";
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 구분 LIKE '%{division}%'                                                                         ");
                }
            }

            qry.Append($"\n 	 GROUP BY 대분류 , 품명 , 원산지 , 규격 , 단위 , 단위수량 , 가격단위, 매출단가 , SEAOVER단위, 구분                         ");
            qry.Append($"\n  ) AS t2                                                                                                                       ");
            qry.Append($"\n   ON t1.품명 = t2.품명                                                                                                         ");
            qry.Append($"\n   AND t1.원산지 = t2.원산지                                                                                                    ");
            qry.Append($"\n   AND t1.규격 = t2.규격                                                                                                        ");
            qry.Append($"\n ) AS t1                                                                                                                        ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                                                     ");
            qry.Append($"\n 	  t.품명                                                                                                                   ");
            qry.Append($"\n 	, t.원산지                                                                                                                 ");
            qry.Append($"\n 	, t.규격                                                                                                                   ");
            qry.Append($"\n 	, t.단위                                                                                                                   ");
            qry.Append($"\n 	, t.가격단위                                                                                                               ");
            qry.Append($"\n 	, t.단위수량                                                                                                               ");
            qry.Append($"\n 	, t.SEAOVER단위                                                                                                            ");
            qry.Append($"\n 	, AVG(t.매입단가) AS 매입단가                                                                                              ");
            qry.Append($"\n 	FROM (                                                                                                                     ");
            qry.Append($"\n 		SELECT                                                                                                                 ");
            qry.Append($"\n 		  t.*                                                                                                                  ");
            qry.Append($"\n 		, ROW_NUMBER() OVER(PARTITION BY t.품명, t.원산지, t.규격, t.단위, t.가격단위, t.단위수량, t.SEAOVER단위 ORDER BY t.매입일자 DESC) AS Row#                          ");
            qry.Append($"\n 		FROM (                                                                                                                 ");
            qry.Append($"\n 			SELECT                                                                                                             ");
            qry.Append($"\n 			  품명 AS 품명                                                                                                     ");
            qry.Append($"\n 			, 원산지 AS 원산지                                                                                                 ");
            qry.Append($"\n 			, 규격 AS 규격                                                                                                     ");
            qry.Append($"\n 			, 매입단가 AS 매입단가                                                                                             ");
            qry.Append($"\n 			, 단위 AS 단위                                                                                                     ");
            qry.Append($"\n 			, ISNULL(가격단위, '') AS 가격단위                                                                                 ");
            qry.Append($"\n 			, 단위수량 AS 단위수량                                                                                             ");
            qry.Append($"\n 			, SEAOVER단위 AS SEAOVER단위                                                                                       ");
            qry.Append($"\n 			, 매입일자 AS 매입일자                                                                                         ");
            qry.Append($"\n 			FROM 업체별시세관리                                                                                                ");
            qry.Append($"\n 			WHERE 사용자 = '{userId}'                                                                                                ");
            qry.Append($"\n 			  AND 매입단가 > 5                                                                                                 ");
            qry.Append($"\n               AND 매입일자 >= '{editSttdate.ToString("yyyy-MM-dd")}'                                                         ");
            qry.Append($"\n               AND 매입일자 <= '{editEnddate.ToString("yyyy-MM-dd")}'                                                         ");
            qry.Append($"\n             GROUP BY 품명, 원산지, 규격, 매입단가, 단위, 가격단위, 단위수량, SEAOVER단위, 매입일자                           ");
            qry.Append($"\n 		) AS t                                                                                                                 ");
            qry.Append($"\n 		WHERE 1=1	                                                                                                           ");
            qry.Append($"\n 	) AS t                                                                                                                     ");
            qry.Append($"\n 	WHERE t.Row# <= {avgCnt}                                                                                                   ");
            qry.Append($"\n 	GROUP BY t.품명, t.원산지, t.규격, t.단위, t.가격단위, t.단위수량, t.SEAOVER단위                                           ");
            qry.Append($"\n ) AS t2                                                                                                                        ");
            qry.Append($"\n    ON t1.품명 = t2.품명                                                                                                        ");
            qry.Append($"\n   AND t1.원산지 = t2.원산지                                                                                                    ");
            qry.Append($"\n   AND t1.규격 = t2.규격                                                                                                        ");
            qry.Append($"\n   AND t1.단위 = t2.단위                                                                                                        ");
            qry.Append($"\n   AND ISNULL(t1.가격단위, '') = ISNULL(t2.가격단위, '')                                                                        ");
            qry.Append($"\n   AND t1.단위수량 = t2.단위수량                                                                                                ");
            qry.Append($"\n   AND t1.SEAOVER단위 = t2.SEAOVER단위                                                                                          ");
            qry.Append($"\n ) AS t                                                                                                                         ");



            qry.Append($"\n ORDER BY t.대분류, t.품명, t.원산지, t.규격3                                                                                   ");
            qry.Append($"\n , CASE WHEN CHARINDEX('대', t.규격4) > 0  THEN 1                                                                               ");
            qry.Append($"\n        WHEN CHARINDEX('중', t.규격4) > 0 THEN 2                                                                                ");
            qry.Append($"\n        WHEN CHARINDEX('소', t.규격4) > 0 THEN 3                                                                                ");
            qry.Append($"\n        WHEN CHARINDEX('L', t.규격4) > 0 THEN 4                                                                                 ");
            qry.Append($"\n        WHEN CHARINDEX('M', t.규격4) > 0 THEN 5                                                                                 ");
            qry.Append($"\n        WHEN CHARINDEX('S', t.규격4) > 0 THEN 6                                                                                 ");
            qry.Append($"\n        END                                                                                                                     ");

            string sql = qry.ToString();

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetPriceComparisonModel(dr);
        }

        public string GetAveragePurchasePriceDetail(string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit, DateTime editSttdate, DateTime editEnddate, int company_cnt = 3)
        {

            string cUnit = seaover_unit;
            if (price_unit == "묶" || price_unit == "묶음")
            {
                cUnit = (Convert.ToDouble(seaover_unit) / Convert.ToDouble(unit_count)).ToString("#,##0.00");
            }


            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                       ");
            qry.Append($"\n  STUFF((SELECT '|' + CONVERT(CHAR(10), 매입일자, 23) + ' ' + 매입처 + ' ' + REPLACE(CONVERT(VARCHAR, 매입단가, 1), '.00', '')");
            qry.Append($"\n 		 FROM (                                                                                                ");
            qry.Append($"\n 		   	 SELECT                                                                                            ");
            qry.Append($"\n 		   	   t.*                                                                                             ");
            qry.Append($"\n 		   	 FROM(                                                                                             ");
            qry.Append($"\n 			   SELECT                                                                                          ");
            qry.Append($"\n 				  t.*                                                                                          ");
            qry.Append($"\n 				, ROW_NUMBER() OVER(PARTITION BY t.품명, t.원산지, t.규격, t.계산단위 ORDER BY t.매입단가 ASC) AS Row#      ");
            qry.Append($"\n 				FROM (                                                                                         ");
            qry.Append($"\n 					SELECT                                                                                     ");
            qry.Append($"\n 					  품명 AS 품명                                                                                 ");
            qry.Append($"\n 					, 원산지 AS 원산지                                                                              ");
            qry.Append($"\n 					, 규격 AS 규격                                                                                 ");
            qry.Append($"\n 					, 매입단가 AS 매입단가                                                                            ");
            qry.Append($"\n 					, 매입처 AS 매입처                                                                              ");
            qry.Append($"\n 					, CASE                                                                                     ");
            qry.Append($"\n 						WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(float, SEAOVER단위)/CONVERT(float, 단위수량)   ");
            qry.Append($"\n 						ELSE SEAOVER단위 END AS 계산단위                                                            ");
            qry.Append($"\n 					, 매입일자 AS 매입일자                                                                            ");
            qry.Append($"\n 					FROM 업체별시세관리                                                                              ");
            qry.Append($"\n 				    WHERE 사용자 = '{userId}'                                                                                   ");
            qry.Append($"\n 				    AND 매입단가 > 5                                                                                        ");
            qry.Append($"\n 		            AND 매입일자 >= '{editSttdate.ToString("yyyy-MM-dd")}'                                                                            ");
            qry.Append($"\n 		            AND 매입일자 <= '{editEnddate.ToString("yyyy-MM-dd")}'                                                                            ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append($"\n 		            AND 품명 = '{product}'                                                                            ");
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append($"\n 		            AND 원산지 = '{origin}'                                                                            ");
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append($"\n 		            AND 규격 = '{sizes}'                                                                            ");
            }    
            if (!string.IsNullOrEmpty(unit.Trim()))
            {
                qry.Append($"\n 		            AND CONVERT(float, CASE WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(float, SEAOVER단위)/CONVERT(float, 단위수량) ELSE SEAOVER단위 END)  = {cUnit}                                                                            ");
            }
            qry.Append($"\n 			     ) AS t                                                                                        ");
            qry.Append($"\n 			 ) AS t                                                                                            ");
            qry.Append($"\n 			 WHERE Row# <= {company_cnt}                                                                                   ");
            qry.Append($"\n 	      ) AS a                                                                                               ");
            qry.Append($"\n 	   WHERE 품명 = t.품명                                                                                         ");
            qry.Append($"\n 	     AND 원산지 = t.원산지                                                                                      ");
            qry.Append($"\n 	     AND 규격 = t.규격	                                                                                       ");
            qry.Append($"\n 	     AND ISNULL(계산단위,'') = ISNULL(t.계산단위,'')                                                              ");
            qry.Append($"\n 	    GROUP BY 매입처, 매입단가, 매입일자                                                                          ");
            qry.Append($"\n 	    FOR xml path('')), 1, 1, '') AS 일반시세상세                                                                ");
            qry.Append($"\n FROM(                                                                                                          ");
            qry.Append($"\n    SELECT                                                                                                      ");
            qry.Append($"\n 	  t.*                                                                                                      ");
            qry.Append($"\n 	, ROW_NUMBER() OVER(PARTITION BY t.품명, t.원산지, t.규격, t.계산단위 ORDER BY t.매입단가 ASC) AS Row#                  ");
            qry.Append($"\n 	FROM (                                                                                                     ");
            qry.Append($"\n 		SELECT                                                                                                 ");
            qry.Append($"\n 		  품명 AS 품명                                                                                             ");
            qry.Append($"\n 		, 원산지 AS 원산지                                                                                          ");
            qry.Append($"\n 		, 규격 AS 규격                                                                                             ");
            qry.Append($"\n 		, 매입단가 AS 매입단가                                                                                        ");
            qry.Append($"\n 		, 매입처 AS 매입처                                                                                          ");
            qry.Append($"\n 		, CASE                                                                                                 ");
            qry.Append($"\n 			WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(float, SEAOVER단위)/CONVERT(float, 단위수량)               ");
            qry.Append($"\n 			ELSE SEAOVER단위 END AS 계산단위                                                                        ");
            qry.Append($"\n 		, 매입일자 AS 매입일자                                                                                        ");
            qry.Append($"\n 		FROM 업체별시세관리                                                                                          ");
            qry.Append($"\n 		WHERE 사용자 = '{userId}'                                                                                   ");
            qry.Append($"\n 		  AND 매입단가 > 5                                                                                        ");
            qry.Append($"\n 		  AND 매입일자 >= '{editSttdate.ToString("yyyy-MM-dd")}'                                                                            ");
            qry.Append($"\n 		  AND 매입일자 <= '{editEnddate.ToString("yyyy-MM-dd")}'                                                                            ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append($"\n 		            AND 품명 = '{product}'                                                                            ");
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append($"\n 		            AND 원산지 = '{origin}'                                                                            ");
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append($"\n 		            AND 규격 = '{sizes}'                                                                            ");
            }
            if (!string.IsNullOrEmpty(unit.Trim()))
            {
                qry.Append($"\n 		            AND CONVERT(float, CASE WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(float, SEAOVER단위)/CONVERT(float, 단위수량) ELSE SEAOVER단위 END)  = {cUnit}                                                                            ");
            }
            qry.Append($"\n      ) AS t                                                                                                    ");
            qry.Append($"\n  ) AS t                                                                                                        ");
            qry.Append($"\n WHERE Row# <= {company_cnt}                                                                                              ");
            qry.Append($"\n GROUP BY t.품명, t.원산지, t.규격, t.계산단위                                                                            ");
            dbInstance.Connection.Close();

            string sql = qry.ToString();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            var table = new DataTable();
            table.Load(dr);

            if (table.Rows.Count > 0)
            {
                return table.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }

            
        }
        public DataTable GetPriceComparisonDataTable(DateTime sDate, int avgCnt, string category, string product, string origin, string sizes, string unit, string manager1, string manager2
                                            , string division, DateTime editSttdate, DateTime editEnddate
                                            , bool isAdvanced = false
                                            , bool isPurchasePrice = false, int sttPrice = 0, int endPrice = 0
                                            , bool isSalesPrice = false, int salesType = 0, int sales_term = 6, int sortType = 1, DataTable productDt = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                 ");
            qry.Append($"\n   t1.대분류                                                                                                               ");
            qry.Append($"\n , t1.대분류1                                                                                                              ");
            qry.Append($"\n , t1.대분류2                                                                                                              ");
            qry.Append($"\n , t1.대분류3                                                                                                              ");
            qry.Append($"\n , t1.품명코드                                                                                                            ");
            qry.Append($"\n , t1.원산지코드                                                                                                          ");
            qry.Append($"\n , t1.규격코드                                                                                                            ");
            qry.Append($"\n , t1.품명                                                                                                                ");
            qry.Append($"\n , t1.규격                                                                                                                ");
            qry.Append($"\n , t1.규격2                                                                                                               ");
            qry.Append($"\n , ISNULL(t1.규격3, 0) AS 규격3                                                                                           ");
            qry.Append($"\n , t1.규격4                                                                                                               ");
            qry.Append($"\n , t1.원산지                                                                                                               ");
            qry.Append($"\n , CONVERT(char,ISNULL(t1.단위, 0)) AS 단위                                                                                                                ");
            qry.Append($"\n , t1.가격단위                                                                                                              ");
            qry.Append($"\n , ISNULL(t1.단위수량, 0) AS 단위수량                                                                                                              ");
            qry.Append($"\n , CASE WHEN ISNULL(t1.묶음수, 1) = 0 THEN 1 ELSE ISNULL(t1.묶음수, 1) END AS 묶음수                                                               ");
            qry.Append($"\n , CONVERT(float, ISNULL(t1.SEAOVER단위, 0)) AS SEAOVER단위                                                                                                         ");
            qry.Append($"\n , CONVERT(float, ISNULL(t1.매출단가, 0)) AS 매출단가                                                                                          ");
            qry.Append($"\n , CONVERT(CHAR(19),t1.단가수정일,20) AS 단가수정일                                                                                                             ");
            qry.Append($"\n , CONVERT(float, ISNULL(t1.최저단가, 0)) AS 최저단가                                                                                          ");
            qry.Append($"\n , t1.구분                                                                                                                ");
            qry.Append($"\n , t1.계산단위                                                                                                              ");
            qry.Append($"\n , CONVERT(float, ISNULL(t1.일반시세     , 0)) AS 일반시세                                                                                     ");
            qry.Append($"\n , ISNULL(t1.일반시세상세   , '') AS 일반시세상세                                                                                  ");
            qry.Append($"\n , CONVERT(float, ISNULL(t2.통관재고      , 0)) AS  통관재고                                                                                       ");
            qry.Append($"\n , CONVERT(float, ISNULL(t2.미통관재고      , 0)) AS  미통관재고                                                                                       ");
            qry.Append($"\n , CONVERT(float, ISNULL(t2.예약수      , 0)) AS  예약수                                                                                       ");
            qry.Append($"\n , t2.예약상세  AS 예약상세                                                                  ");
            qry.Append($"\n , CONVERT(float, ISNULL(t2.매출수      , 0)) AS  매출수                                                                                       ");
            qry.Append($"\n , CONVERT(float, ISNULL(t2.매출금액     , 0)) AS 매출금액                                                                                     ");
            qry.Append($"\n , CONVERT(float, ISNULL(t2.매출원가     , 0)) AS 매출원가                                                                                     ");
            qry.Append($"\n  , t1.품명 + '^' + t1.원산지 + '^' + t1.규격 + '^' + RTRIM(CONVERT(CHAR, t1.단위)) + '^' + RTRIM(ISNULL(t1.가격단위, '')) + '^' + RTRIM(CONVERT(CHAR, t1.단위수량)) + '^' + RTRIM(ISNULL(t1.SEAOVER단위, '')) AS 그룹코드         ");
            qry.Append($"\n  , '000000' AS main_id         ");
            qry.Append($"\n  , '000000' AS sub_id         ");
            qry.Append($"\n FROM (                                                                                                                 ");
            qry.Append($"\n 	SELECT                                                                                                             ");
            qry.Append($"\n 	  t1.대분류                                                                                                           ");
            qry.Append($"\n 	, t1.대분류1                                                                                                          ");
            qry.Append($"\n 	, t1.대분류2                                                                                                          ");
            qry.Append($"\n 	, t1.대분류3                                                                                                          ");
            qry.Append($"\n 	, t1.품명코드                                                                                                            ");
            qry.Append($"\n 	, t1.원산지코드                                                                                                         ");
            qry.Append($"\n 	, t1.규격코드                                                                                                       ");
            qry.Append($"\n 	, t1.품명                                                                                                            ");
            qry.Append($"\n 	, t1.규격                                                                                                            ");
            qry.Append($"\n 	, t1.규격2                                                                                                           ");
            qry.Append($"\n 	, t1.규격3                                                                                                           ");
            qry.Append($"\n 	, t1.규격4                                                                                                           ");
            qry.Append($"\n 	, t1.원산지                                                                                                           ");
            qry.Append($"\n 	, t1.단위                                                                                                            ");
            qry.Append($"\n 	, t1.가격단위                                                                                                          ");
            qry.Append($"\n 	, t1.단위수량                                                                                                          ");
            qry.Append($"\n 	, t1.묶음수                                                                                                            ");
            qry.Append($"\n 	, t1.SEAOVER단위                                                                                                     ");
            qry.Append($"\n 	, t1.매출단가                                                                                                          ");
            qry.Append($"\n 	, t1.단가수정일                                                                                                          ");
            qry.Append($"\n 	, t2.최저단가                                                                                                          ");
            qry.Append($"\n 	, t1.구분                                                                                                            ");
            qry.Append($"\n 	, t1.계산단위                                                                                                          ");
            qry.Append($"\n 	, t3.일반시세                                                                                                          ");
            qry.Append($"\n 	, t3.일반시세상세                                                                                                       ");
            qry.Append($"\n 	FROM (                                                                                                             ");
            qry.Append($"\n 		SELECT                                                                                                         ");
            qry.Append($"\n 		  t.*                                                                                                          ");
            qry.Append($"\n 		, CASE                                                                                                         ");
            qry.Append($"\n 			WHEN t.규격2 = '.' THEN NULL                                                                                 ");
            qry.Append($"\n 			WHEN t.규격2 = '-' THEN NULL                                                                                 ");
            qry.Append($"\n 		  	WHEN ISNUMERIC(t.규격2) = 1 THEN CONVERT(float, t.규격2)                                                       ");
            qry.Append($"\n 		  	ELSE NULL                                                                                                  ");
            qry.Append($"\n 		  END AS 규격3                                                                                                   ");
            qry.Append($"\n 		, CASE                                                                                                         ");
            qry.Append($"\n 			WHEN t.규격2 = '.' THEN t.규격2                                                                                ");
            qry.Append($"\n 			WHEN t.규격2 = '-' THEN t.규격2                                                                                ");
            qry.Append($"\n 			WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                        ");
            qry.Append($"\n 		  	ELSE t.규격2                                                                                                 ");
            qry.Append($"\n 		  END AS 규격4                                                                                                   ");
            qry.Append($"\n 		FROM(                                                                                                          ");
            qry.Append($"\n 			SELECT                                                                                                     ");
            qry.Append($"\n 			  t.*                                                                                                      ");
            qry.Append($"\n 			, CASE                                                                                                     ");
            qry.Append($"\n 			    WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                           ");
            qry.Append($"\n 				  CASE WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))        ");
            qry.Append($"\n 					   WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격))        ");
            qry.Append($"\n 			      END                                                                                                  ");
            qry.Append($"\n 			    WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                 ");
            qry.Append($"\n 			    WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                 ");
            qry.Append($"\n 			    ELSE 규격                                                                                                ");
            qry.Append($"\n 			  END AS 규격2                                                                                               ");
            qry.Append($"\n 			, CASE                                                                                                     ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Aa' THEN '라운드새우류'                                       ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Ab' THEN '새우살류'                                         ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Ba' THEN '쭈꾸미류'                                         ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Bb' THEN '낙지류'                                          ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Bc' THEN '갑오징어류'                                        ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Bd' THEN '오징어류'                                         ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Be' THEN '문어류'                                          ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 1)) = 'C' THEN '갑각류'                                           ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Da' THEN '어패류'                                          ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Db' THEN '살류'                                           ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Dc' THEN '해물류'                                          ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Ea' THEN '초밥/일식류'                                       ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Eb' THEN '기타가공품(튀김, 가금류)'                              ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 2)) = 'Ec' THEN '기타가공품(야채, 과일)'                                ");
            qry.Append($"\n 				WHEN CONVERT(char, SUBSTRING(t.대분류, 1, 1)) = 'F' THEN '선어류'                                           ");
            qry.Append($"\n 			 END AS 대분류1                                                                                               ");
            qry.Append($"\n 			,  CASE WHEN ISNUMERIC(SUBSTRING(t.대분류, 2, 1)) = 1 THEN SUBSTRING(대분류, 1, 1) ELSE SUBSTRING(대분류, 1, 2) END AS 대분류2  ");
            qry.Append($"\n 			,  CASE WHEN ISNUMERIC(SUBSTRING(t.대분류, 2, 1)) = 1 THEN SUBSTRING(대분류, 2, 3) ELSE SUBSTRING(대분류, 3, 3) END AS 대분류3   ");
            /*qry.Append($"\n 			,  SUBSTRING(대분류, 0, 3) AS 대분류2                                                                           ");
            qry.Append($"\n 			,  SUBSTRING(대분류, 3, 3) AS 대분류3                                                                           ");*/
            qry.Append($"\n 			FROM(                                                                                                      ");
            qry.Append($"\n 				SELECT                                                                                                                                          ");
            qry.Append($"\n  				 대분류                                                                                                                                            ");
            qry.Append($"\n	 				, 품명코드                                                                                                                                        ");
            qry.Append($"\n	 				, 원산지코드                                                                                                                                      ");
            qry.Append($"\n	 				, 규격코드                                                                                                                                        ");
            qry.Append($"\n 				, 품명                                                                                                                                            ");
            qry.Append($"\n 				, 규격                                                                                                                                            ");
            qry.Append($"\n 				, 원산지                                                                                                                                          ");
            qry.Append($"\n 				, CONVERT(float, 단위) AS 단위                                                                                                                    ");
            qry.Append($"\n				    , 가격단위                                                                                                                                        ");
            qry.Append($"\n 				, 단위수량                                                                                                                                        ");
            qry.Append($"\n 				, 묶음수                                                                                                                                          ");
            qry.Append($"\n 				, SEAOVER단위                                                                                                                                     ");
            qry.Append($"\n 				, 매출단가                                                                                                                                        ");
            qry.Append($"\n 				, 단가수정일                                                                                                                                        ");
            qry.Append($"\n 				, 구분                                                                                                                                            ");
            qry.Append($"\n 				, 계산단위                                                                                                                                        ");
            qry.Append($"\n  				FROM(                                                                                                                                             ");
            qry.Append($"\n						SELECT                                                                                                                                        ");
            qry.Append($"\n	 					  대분류                                                                                                                                      ");
            qry.Append($"\n	 					, 품목코드 AS  품명코드                                                                                                                       ");
            qry.Append($"\n	 					, 원산지코드                                                                                                                                  ");
            qry.Append($"\n	 					, 규격코드                                                                                                                                    ");
            qry.Append($"\n	 					, 품명                                                                                                                                                         ");
            qry.Append($"\n	 					, 규격                                                                                                                                                         ");
            qry.Append($"\n	 					, 원산지                                                                                                                                                       ");
            qry.Append($"\n	 					, 단위                                                                                                                                                         ");
            qry.Append($"\n						, CASE WHEN 가격단위='.' THEN '' WHEN 가격단위='-' THEN '' ELSE ISNULL(가격단위,'') END AS 가격단위                                                            ");
            qry.Append($"\n	 					, 단위수량                                                                                                                                                     ");
            qry.Append($"\n	 					, CASE WHEN ISNULL(묶음수, 0) = 0 THEN 1 ELSE 묶음수 END AS 묶음수                                                                                             ");
            qry.Append($"\n	 					, SEAOVER단위                                                                                                                                                  ");
            qry.Append($"\n	 					, ROUND(ISNULL(매출단가, 0), 0 , 1) AS 매출단가                                                                                                                ");
            qry.Append($"\n	 					, 매입일자 AS 단가수정일                                                                                                                                       ");
            qry.Append($"\n	 					, ISNULL(구분, '') AS 구분                                                                                                                                     ");
            //qry.Append($"\n	 					, CASE WHEN CHARINDEX(가격단위, '묶') > 0 OR CHARINDEX(가격단위, '팩') > 0  THEN CONVERT(VARCHAR, CONVERT(float, SEAOVER단위)/ CONVERT(float, 단위수량))      ");
            qry.Append($"\n	 					, CASE WHEN CHARINDEX(가격단위, '묶') > 0 OR CHARINDEX(가격단위, '팩') > 0  THEN CONVERT(float, 단위)      ");
            qry.Append($"\n	 						   ELSE SEAOVER단위                                                                                                                           ");
            qry.Append($"\n	 					  END AS 계산단위                                                                                                                                   ");
            qry.Append($"\n	 					, ROW_NUMBER() OVER(PARTITION BY 대분류, 품명, 원산지, 규격, 단위, 단위수량, CASE WHEN ISNULL(묶음수, 0) = 0 THEN 1 ELSE 묶음수 END, SEAOVER단위, 구분 ORDER BY 매입일자 DESC) AS Row#                          ");
            qry.Append($"\n	 					FROM 업체별시세관리                                                                                                                                   ");
            qry.Append($"\n	 					WHERE 사용자 = '{userId}'                                                                                                                            ");
            //강제제외 (삼점게 80/100(F) <- 중복)
            qry.Append($"\n	 					  AND NOT (품목코드 = '115' AND 규격코드 = '007')                                                                                                                            ");
            //qry.Append($"\n	 					  AND 매출단가 > 0                                                                                                                            ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append(whereSql("품명", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append(whereSql("원산지", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append(whereSql("규격", sizes.Trim()));
            }
            if (!string.IsNullOrEmpty(unit.Trim()))
            {
                qry.Append(whereSql("단위", unit.Trim()));
            }
            if (!string.IsNullOrEmpty(manager1.Trim()))
            {
                qry.Append(whereSql("담당자1", manager1.Trim()));
            }
            if (!string.IsNullOrEmpty(manager2.Trim()))
            {
                qry.Append(whereSql("담당자2", manager2.Trim()));
            }
            if (!string.IsNullOrEmpty(division.Trim()))
            {
                qry.Append(whereSql("구분", division.Trim()));
            }
            //Group
            if (productDt != null && productDt.Rows.Count > 0)
                qry.Append($"\n	 					  AND ({whereSqlOr(productDt)})");

            qry.Append($"\n					) AS t                                                                                                 ");
            qry.Append($"\n 				WHERE Row# = 1    ");
            //qry.Append($"\n 				GROUP BY 대분류, 품명, 규격, 원산지, CONVERT(float, 단위), 가격단위, 단위수량, 묶음수, SEAOVER단위, 매출단가, 구분,계산단위, 품명코드, 원산지코드, 규격코드    ");
            qry.Append($"\n 		    ) AS t                                                                                                     ");
            qry.Append($"\n 		) AS t                                                                                                         ");
            qry.Append($"\n 		WHERE 1=1                                                                                                      ");
            if (!string.IsNullOrEmpty(category.Trim()))
            {
                qry.Append(whereSql("t.대분류", category.Trim()));
            }
            qry.Append($"\n 	) AS t1                                                                                                            ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                                                                                  ");
            qry.Append($"\n 		SELECT                                                                                                         ");
            qry.Append($"\n 		   대분류                                                                                                         ");
            qry.Append($"\n 		 , 품명                                                                                                          ");
            qry.Append($"\n 		 , 원산지                                                                                                         ");
            qry.Append($"\n 		 , 규격                                                                                                          ");
            qry.Append($"\n	 	     , CASE WHEN CHARINDEX(가격단위, '묶') > 0 OR CHARINDEX(가격단위, '팩') > 0  THEN CONVERT(float, 단위)    ");
            qry.Append($"\n			        ELSE SEAOVER단위                                                                                                                         ");
            qry.Append($"\n		       END AS 계산단위                                                                                                                                 ");
            qry.Append($"\n 		 , MIN(ISNULL(매입단가, 0)) AS 최저단가                                                                               ");
            qry.Append($"\n 		 , ISNULL(구분, '') AS 구분                                                                                        ");
            qry.Append($"\n 		 FROM 업체별시세관리                                                                                                 ");
            qry.Append($"\n 		  WHERE 사용자 = '{userId}'                                                                                         ");
            qry.Append($"\n 			AND 매입단가 > 5                                                                                               ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append(whereSql("품명", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append(whereSql("원산지", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append(whereSql("규격", sizes.Trim()));
            }
            /*if (!string.IsNullOrEmpty(unit.Trim()))
            {
                qry.Append(whereSql("단위", unit.Trim()));
            }*/
            if (!string.IsNullOrEmpty(manager1.Trim()))
            {
                qry.Append(whereSql("담당자1", manager1.Trim()));
            }
            if (!string.IsNullOrEmpty(manager2.Trim()))
            {
                qry.Append(whereSql("담당자2", manager2.Trim()));
            }
            if (!string.IsNullOrEmpty(division.Trim()))
            {
                qry.Append(whereSql("구분", division.Trim()));
            }
            //Group
            if (productDt != null && productDt.Rows.Count > 0)
                qry.Append($"\n	 					  AND ({whereSqlOr(productDt)})");
            qry.Append($"\n 		 GROUP BY 대분류 , 품명 , 원산지 , 규격 , CASE WHEN CHARINDEX(가격단위, '묶') > 0 OR CHARINDEX(가격단위, '팩') > 0  THEN CONVERT(float, 단위) ELSE SEAOVER단위 END, ISNULL(구분, '') ");
            qry.Append($"\n 	) AS t2			                                                                                                   ");
            qry.Append($"\n 		ON  ISNULL(t1.대분류, '') = ISNULL(t2.대분류, '')                                                                   ");
            qry.Append($"\n 		AND ISNULL(t1.품명, '') = ISNULL(t2.품명, '')                                                                      ");
            qry.Append($"\n 		AND ISNULL(t1.원산지, '') = ISNULL(t2.원산지, '')                                                                   ");
            qry.Append($"\n 		AND ISNULL(t1.규격, '') = ISNULL(t2.규격, '')                                                                      ");
            qry.Append($"\n 		AND ISNULL(t1.계산단위, '') = ISNULL(t2.계산단위, '')                                                                      ");
            qry.Append($"\n 		AND ISNULL(t1.구분, '') = ISNULL(t2.구분, '')                                                                      ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                                                                                  ");
            qry.Append($"\n 		SELECT                                                                                                         ");
            qry.Append($"\n 		  t.품명                                                                                                         ");
            qry.Append($"\n 		, t.원산지                                                                                                        ");
            qry.Append($"\n 		, t.규격                                                                                                         ");
            qry.Append($"\n 		, t.계산단위                                                                                                       ");
            qry.Append($"\n 		, AVG(t.매입단가) AS 일반시세                                                                                         ");
            qry.Append($"\n 		, '' AS 일반시세상세                                                                ");
            qry.Append($"\n 		FROM(                                                                                                          ");
            qry.Append($"\n 		   SELECT                                                                                                      ");
            qry.Append($"\n 			  t.*                                                                                                      ");
            qry.Append($"\n 			, ROW_NUMBER() OVER(PARTITION BY t.품명, t.원산지, t.규격, t.계산단위 ORDER BY t.매입단가 ASC) AS Row#                  ");
            qry.Append($"\n 			FROM (                                                                                                     ");
            qry.Append($"\n 				SELECT                                                                                                 ");
            qry.Append($"\n 				  품명 AS 품명                                                                                             ");
            qry.Append($"\n 				, 원산지 AS 원산지                                                                                          ");
            qry.Append($"\n 				, 규격 AS 규격                                                                                             ");
            qry.Append($"\n 				, 매입단가 AS 매입단가                                                                                        ");
            qry.Append($"\n 				, 매입처 AS 매입처                                                                                          ");
            qry.Append($"\n 				, CASE                                                                                                 ");
            qry.Append($"\n 					WHEN CHARINDEX(가격단위, '묶') > 0 OR CHARINDEX(가격단위, '팩') > 0  THEN CONVERT(float, 단위)              ");
            qry.Append($"\n 					ELSE SEAOVER단위 END AS 계산단위                                                                        ");
            qry.Append($"\n 				, 매입일자 AS 매입일자                                                                                        ");
            qry.Append($"\n 				FROM 업체별시세관리                                                                                          ");
            qry.Append($"\n 				WHERE 사용자 = '{userId}'                                                                                   ");
            qry.Append($"\n 				   AND 매입단가 > 5                                                                                        ");
            qry.Append($"\n 		           AND 매입일자 >= '{editSttdate.ToString("yyyy-MM-dd")}'                                                                            ");
            qry.Append($"\n 		           AND 매입일자 <= '{editEnddate.ToString("yyyy-MM-dd")}'                                                                            ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append(whereSql("품명", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append(whereSql("원산지", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append(whereSql("규격", sizes.Trim()));
            }
            /*if (!string.IsNullOrEmpty(unit.Trim()))
            {
                qry.Append(whereSql("단위", unit.Trim()));
            }*/
            if (!string.IsNullOrEmpty(manager1.Trim()))
            {
                qry.Append(whereSql("담당자1", manager1.Trim()));
            }
            if (!string.IsNullOrEmpty(manager2.Trim()))
            {
                qry.Append(whereSql("담당자2", manager2.Trim()));
            }
            if (!string.IsNullOrEmpty(division.Trim()))
            {
                qry.Append(whereSql("구분", division.Trim()));
            }
            //Group
            if (productDt != null && productDt.Rows.Count > 0)
                qry.Append($"\n	 					  AND ({whereSqlOr(productDt)})");
            qry.Append($"\n 		     ) AS t                                                                                                    ");
            qry.Append($"\n 		 ) AS t                                                                                                        ");
            qry.Append($"\n 		WHERE Row# <= {avgCnt}                                                                                                ");
            qry.Append($"\n 		GROUP BY t.품명, t.원산지, t.규격, t.계산단위                                                                            ");
            qry.Append($"\n 	) AS t3                                                                                                            ");
            qry.Append($"\n 		ON  ISNULL(t1.품명, '') = ISNULL(t3.품명, '')                                                                      ");
            qry.Append($"\n 		AND ISNULL(t1.원산지, '') = ISNULL(t3.원산지, '')                                                                   ");
            qry.Append($"\n 		AND ISNULL(t1.규격, '') = ISNULL(t3.규격, '')                                                                      ");
            qry.Append($"\n 		AND ISNULL(t1.계산단위, '') = ISNULL(t3.계산단위, '')                                                                 ");
            qry.Append($"\n ) AS t1	                                                                                                               ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                      ");

            qry.Append($"\n 	SELECT                                                                           ");
            qry.Append($"\n 	  t.품명                                                                           ");
            qry.Append($"\n 	, t.원산지                                                                          ");
            qry.Append($"\n 	, t.규격                                                                           ");
            qry.Append($"\n 	, RTRIM(LTRIM(REPLACE(t.단위, 'kg', ''))) AS 단위                                    ");
            qry.Append($"\n 	, SUM(t.매출원가) AS 매출원가                                                           ");
            qry.Append($"\n 	, SUM(t.통관재고) AS 통관재고                                                             ");
            qry.Append($"\n 	, SUM(t.미통관재고) AS 미통관재고                                                             ");
            qry.Append($"\n 	, SUM(t.예약수) AS 예약수                                                             ");
            qry.Append($"\n 	, SUM(t.매출수) AS 매출수                                                             ");
            qry.Append($"\n 	, SUM(t.매출금액) AS 매출금액                                                           ");
            qry.Append($"\n 	, '' AS 예약상세                                         ");
            qry.Append($"\n 	FROM (                                                                           ");
            qry.Append($"\n 		SELECT                                                        ");
            qry.Append($"\n 		품명                                                            ");
            qry.Append($"\n 		, 원산지                                                         ");
            qry.Append($"\n 		, 규격                                                          ");
            qry.Append($"\n 		, REPLACE(단위, 'kg', '')  AS 단위                                ");
            qry.Append($"\n 		, 0 AS 매출원가                                                   ");
            qry.Append($"\n 		, 0 AS 통관재고                                                   ");
            qry.Append($"\n 		, 0 AS 미통관재고                                                 ");
            qry.Append($"\n 		, 0 AS 예약수                                                    ");
            qry.Append($"\n 		, SUM(ISNULL(매출수, 0)) AS 매출수                                 ");
            qry.Append($"\n 		, SUM(ISNULL(매출금액, 0)) AS 매출금액                               ");
            qry.Append($"\n 		FROM 매출현황                                                     ");
            qry.Append($"\n 		WHERE 사용자 = '200009'                                          ");
            qry.Append($"\n 		  AND 매출자 <> '199990'                                           ");
            qry.Append($"\n 		  AND 매출처 NOT LIKE '%아토무역%'                                  ");
            qry.Append($"\n 		  AND 매출처 NOT LIKE '%아토코리아%'                                 ");
            qry.Append($"\n 		  AND 매출처 NOT LIKE '%에스제이씨푸드%'                               ");
            qry.Append($"\n 		  AND 매출처 NOT LIKE '%에이티오%'                                  ");

            //2023-09-11 //// 이제 아토번호로 동원 및 한성같은 대행건들 제외하지 않고 씨오버에서 매출자를 관리자로 변경해서 관리하기로함

            DateTime eDate;
            switch (sales_term)
            {
                case 1:
                    eDate = sDate.AddMonths(-1);
                    break;
                case 45:
                    eDate = sDate.AddDays(-45);
                    break;
                case 2:
                    eDate = sDate.AddMonths(-2);
                    break;
                case 3:
                    eDate = sDate.AddMonths(-3);
                    break;
                case 6:
                    eDate = sDate.AddMonths(-6);
                    break;
                case 12:
                    eDate = sDate.AddMonths(-12);
                    break;
                case 18:
                    eDate = sDate.AddMonths(-18);
                    break;
                default:
                    eDate = sDate.AddMonths(-6);
                    break;
            }

            //2023-10-20 조회날이 당일이면 미리 매출입력을 많이 해놓은 경우가 많고 아닌경우도 있고 암튼 잘안맞기 때문에 전날로 바꿈
            if(sDate.Year == DateTime.Now.Year && sDate.Month == DateTime.Now.Month && sDate.Day == DateTime.Now.Day)
                qry.Append($"\n 		  AND 매출일자 <= '{sDate.AddDays(-1).ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.ToString("yyyy-MM-dd")}'          ");
            else
                qry.Append($"\n 		  AND 매출일자 <= '{sDate.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{eDate.ToString("yyyy-MM-dd")}'          ");

            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append(whereSql("품명", product.Trim()));
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append(whereSql("원산지", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append(whereSql("규격", sizes.Trim()));
            //2023-06-15 매출현황과 품명별매출현황 차이가 관리자 매출을 뺄수없고 전체 회전율 계산이 어려움
            /*qry.Append($"\n 		SELECT                                                                       ");
            qry.Append($"\n 		  품명                                                                         ");
            qry.Append($"\n 		, 원산지                                                                        ");
            qry.Append($"\n 		, 규격                                                                         ");
            qry.Append($"\n 		, REPLACE(단위, 'kg', '')  AS 단위                                                                       ");
            qry.Append($"\n 		, 0 AS 매출원가                                                                  ");
            qry.Append($"\n 		, 0 AS 통관재고                                                                   ");
            qry.Append($"\n 		, 0 AS 미통관재고                                                                   ");
            qry.Append($"\n 		, 0 AS 예약수                                                                   ");
            qry.Append($"\n 		, SUM(ISNULL(매출수, 0)) AS 매출수                                                ");
            qry.Append($"\n 		, SUM(ISNULL(매출금액, 0)) AS 매출금액                                              ");
            qry.Append($"\n 		FROM 품명별매출현황                                                                ");
            qry.Append($"\n 		WHERE 사용자 = '{sales_term}'                                                              ");
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append(whereSql("품명", product.Trim()));
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append(whereSql("원산지", origin.Trim()));
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append(whereSql("규격", sizes.Trim()));
            *//*if (!string.IsNullOrEmpty(unit.Trim()))
                qry.Append(whereSql("단위", unit.Trim()));*/
            //Group
            if (productDt != null && productDt.Rows.Count > 0)
                qry.Append($"\n	 					  AND ({whereSqlOr(productDt, true)})");
            qry.Append($"\n 		GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')                                                         ");
            qry.Append($"\n 		UNION ALL                                                                                                    ");
            qry.Append($"\n 		SELECT                                                                                                       ");
            qry.Append($"\n 		  품명                                                                                                       ");
            qry.Append($"\n 		, 원산지                                                                                                     ");
            qry.Append($"\n 		, 규격                                                                                                       ");
            qry.Append($"\n 		, replace(단위, 'kg', '') AS 단위                                                                            ");
            qry.Append($"\n 		, SUM(매출원가 * 재고수) / SUM(CASE WHEN 매출원가 > 0 THEN 재고수 ELSE NULL END) AS  매출원가             ");
            qry.Append($"\n 		, SUM(CASE WHEN 매입구분 = '국내매입' OR 통관 = '통관' OR 통관 = '' THEN ISNULL(재고수, 0) ELSE 0 END ) AS 통관재고                                ");
            qry.Append($"\n 		, SUM(CASE WHEN 매입구분 <> '국내매입' AND 통관 <> '통관' THEN ISNULL(재고수, 0) ELSE 0 END ) AS 미통관재고                             ");
            qry.Append($"\n 		, SUM(예약수) AS 예약수                                                                                      ");
            qry.Append($"\n 		, 0 AS 매출수                                                                                                ");
            qry.Append($"\n 		, 0 AS 매출금액                                                                                              ");
            qry.Append($"\n 		FROM 품명별재고현황 AS a                                                                                     ");
            qry.Append($"\n 		WHERE 사용자 = '{userId}'                                                                                    ");
            qry.Append($"\n 		  AND (매입처 NOT LIKE '%-DL%' OR 매입처 NOT LIKE '%-LT%')                                                   ");
            qry.Append($"\n 		  AND ((적요 NOT LIKE '%제외%' AND 적요 NOT LIKE '%back%' AND 적요 NOT LIKE '%백쉽%' AND 적요 NOT LIKE '%빽쉽%' ) OR 적요 IS NULL)  ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append(whereSql("품명", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append(whereSql("원산지", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append(whereSql("규격", sizes.Trim()));
            }
            /*if (!string.IsNullOrEmpty(unit.Trim()))
            {
                qry.Append(whereSql("단위", unit.Trim()));
            }*/
            //Group
            if (productDt != null && productDt.Rows.Count > 0)
                qry.Append($"\n	 					  AND ({whereSqlOr(productDt, true)})");
            qry.Append($"\n 		GROUP BY 품명, 원산지, 규격, replace(단위, 'kg', '')                                                    ");
            qry.Append($"\n 	) AS t                                                                           ");
            qry.Append($"\n 	GROUP BY t.품명, t.원산지, t.규격, t.단위                                                ");
            qry.Append($"\n ) AS t2                                                                                                                ");
            qry.Append($"\n 	ON ISNULL(t1.품명, '') =  ISNULL(t2.품명, '')                                                                          ");
            qry.Append($"\n 	AND ISNULL(t1.원산지, '') =  ISNULL(t2.원산지, '')                                                                      ");
            qry.Append($"\n 	AND ISNULL(t1.규격, '') =  ISNULL(t2.규격, '')                                                                         ");
            qry.Append($"\n 	AND ISNULL(t1.SEAOVER단위 , '') =  ISNULL(t2.단위, '')                                                                 ");
            qry.Append($"\nWHERE 1=1                                                                                                                  ");
            //매입단가 검색
            if (isPurchasePrice)
            {
                qry.Append($"\n	  AND t1.최저단가 >= {sttPrice}                                                                                ");
                qry.Append($"\n	  AND t1.최저단가 <= {endPrice}                                                                                ");
            }
            //단가타입(F10)
            if (isSalesPrice)
            {
                if (salesType == 1)
                {
                    qry.Append($"\n	  AND t1.매출단가 <= t1.최저단가                                                                                ");
                }
                else if (salesType == 2)
                {
                    qry.Append($"\n	  AND t1.매출단가 < t1.일반시세                                                                                ");
                }
                else if (salesType == 3)
                {
                    qry.Append($"\n	  AND t1.매출단가 >= t1.일반시세                                                                                ");
                }
            }
            qry.Append($"\n GROUP BY t1.단가수정일, t1.대분류,t1.대분류1,t1.대분류2,t1.대분류3,t1.품명코드,t1.원산지코드,t1.규격코드,t1.품명,t1.규격,t1.규격2,t1.규격3,t1.규격4,t1.원산지,t1.단위,t1.가격단위, CASE WHEN ISNULL(t1.묶음수, 1) = 0 THEN 1 ELSE ISNULL(t1.묶음수, 1) END, t1.단위수량,t1.SEAOVER단위,ISNULL(t1.매출단가,0),ISNULL(t1.최저단가,0),t1.구분,t1.계산단위,ISNULL(t1.일반시세,0),ISNULL(t1.일반시세상세,''),ISNULL(t2.통관재고,0),ISNULL(t2.미통관재고,0),ISNULL(t2.예약수,0),t2.예약상세,ISNULL(t2.매출수,0),ISNULL(t2.매출금액,0),ISNULL(t2.매출원가,0)    ");
            //정렬기준
            if (sortType == 1)
            {
                qry.Append($"\n ORDER BY t1.대분류1, t1.품명, t1.원산지, t1.규격3, t1.규격2                                                                       ");
                qry.Append($"\n , CASE WHEN CHARINDEX('대', t1.규격4) > 0  THEN 1                                                                         ");
                qry.Append($"\n        WHEN CHARINDEX('중', t1.규격4) > 0 THEN 2                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('소', t1.규격4) > 0 THEN 3                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('L', t1.규격4) > 0 THEN 4                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('M', t1.규격4) > 0 THEN 5                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('S', t1.규격4) > 0 THEN 6                                                                          ");
                qry.Append($"\n   END                                                                                                                  ");
            }
            else if (sortType == 2)
            {
                qry.Append($"\n ORDER BY t1.품명, t1.원산지, t1.규격3, t1.규격2                                                                       ");
                qry.Append($"\n , CASE WHEN CHARINDEX('대', t1.규격4) > 0  THEN 1                                                                         ");
                qry.Append($"\n        WHEN CHARINDEX('중', t1.규격4) > 0 THEN 2                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('소', t1.규격4) > 0 THEN 3                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('L', t1.규격4) > 0 THEN 4                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('M', t1.규격4) > 0 THEN 5                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('S', t1.규격4) > 0 THEN 6                                                                          ");
                qry.Append($"\n   END                                                                                                                  ");
            }
            else if (sortType == 3)
            {
                qry.Append($"\n ORDER BY t1.원산지, t1.품명, t1.규격3, t1.규격2                                                                       ");
                qry.Append($"\n , CASE WHEN CHARINDEX('대', t1.규격4) > 0  THEN 1                                                                         ");
                qry.Append($"\n        WHEN CHARINDEX('중', t1.규격4) > 0 THEN 2                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('소', t1.규격4) > 0 THEN 3                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('L', t1.규격4) > 0 THEN 4                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('M', t1.규격4) > 0 THEN 5                                                                          ");
                qry.Append($"\n        WHEN CHARINDEX('S', t1.규격4) > 0 THEN 6                                                                          ");
                qry.Append($"\n   END                                                                                                                  ");
            }
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            var table = new DataTable();
            table.Load(dr);

            return table;
        }


        public DataTable GetPriceComparisonDataTable2(string product, string origin, string sizes, string unit, string division, int avgCnt, DateTime editSttdate, DateTime editEnddate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                          ");
            qry.Append($"\n  t1.품명                                                                                         ");
            qry.Append($"\n, t1.원산지                                                                                        ");
            qry.Append($"\n, t1.규격                                                                                         ");
            qry.Append($"\n, t1.단위                                                                                         ");
            qry.Append($"\n, t1.미통관                                                                                        ");
            qry.Append($"\n, t1.통관                                                                                         ");
            qry.Append($"\n, t1.예약수                                                                                        ");
            qry.Append($"\n, t1.매출수                                                                                        ");
            qry.Append($"\n, ISNULL(t2.매입단가 / t1.단위, 0) AS 매입단가                                                                      ");
            qry.Append($"\n, ISNULL(t2.매출단가 / t1.단위, 0) AS 매출단가                                                                      ");
            qry.Append($"\n, ISNULL(t3.매입단가 / t1.단위, 0) AS 일반시세                                                                      ");
            qry.Append($"\nFROM(                                                                                           ");
            qry.Append($"\n	SELECT                                                                                         ");
            qry.Append($"\n	  품명                                                                                           ");
            qry.Append($"\n	, 원산지                                                                                          ");
            qry.Append($"\n	, 규격                                                                                           ");
            qry.Append($"\n	, MIN(단위) AS 단위                                                                                ");
            qry.Append($"\n	, SUM(미통관 * 단위) / MIN(단위) AS 미통관                                                              ");
            qry.Append($"\n	, SUM(통관 * 단위) / MIN(단위) AS 통관                                                                ");
            qry.Append($"\n	, SUM(예약수 * 단위) / MIN(단위) AS 예약수                                                              ");
            qry.Append($"\n	, SUM(매출수 * 단위) / MIN(단위) AS 매출수                                                              ");
            qry.Append($"\n	FROM(                                                                                          ");
            qry.Append($"\n		SELECT                                                                                     ");
            qry.Append($"\n		  품명                                                                                       ");
            qry.Append($"\n		, 원산지                                                                                      ");
            qry.Append($"\n		, 규격                                                                                       ");
            qry.Append($"\n		, CASE WHEN 단위 = '.' OR 단위 = '-' OR 단위 = '0'THEN  1 ELSE CONVERT(float,단위) END AS 단위         ");
            qry.Append($"\n		, SUM(미통관) AS 미통관                                                                         ");
            qry.Append($"\n		, SUM(통관) AS 통관                                                                            ");
            qry.Append($"\n		, SUM(예약수) AS 예약수                                                                         ");
            qry.Append($"\n		, SUM(매출수) AS 매출수                                                                         ");
            qry.Append($"\n		FROM (                                                                                     ");

            if (!string.IsNullOrEmpty(division))
            {
                qry.Append($"\n			SELECT                                                                                 ");
                qry.Append($"\n			  품명                                                                                   ");
                qry.Append($"\n			, 원산지                                                                                  ");
                qry.Append($"\n			, 규격                                                                                   ");
                qry.Append($"\n			, SEAOVER단위  AS 단위                                                                     ");
                qry.Append($"\n			, 0 AS 미통관                                                                             ");
                qry.Append($"\n			, 0 AS 통관                                                                              ");
                qry.Append($"\n			, 0 AS 예약수                                                                             ");
                qry.Append($"\n			, 0 AS 매출수                                                                             ");
                qry.Append($"\n			FROM 업체별시세관리                                                                          ");
                qry.Append($"\n			WHERE 사용자 = '{userId}'                                                                   ");
                qry.Append($"\n			  AND 매출단가 > 0 AND 매입단가 > 6                                                         ");
                qry.Append($"\n	  {whereSql("구분", division)}                                                                 ");
            }
            else
            { 
                qry.Append($"\n			SELECT                                                                                 ");
                qry.Append($"\n			  품명                                                                                   ");
                qry.Append($"\n			, 원산지                                                                                  ");
                qry.Append($"\n			, 규격                                                                                   ");
                qry.Append($"\n			, SEAOVER단위  AS 단위                                                                     ");
                qry.Append($"\n			, 0 AS 미통관                                                                             ");
                qry.Append($"\n			, 0 AS 통관                                                                              ");
                qry.Append($"\n			, 0 AS 예약수                                                                             ");
                qry.Append($"\n			, 0 AS 매출수                                                                             ");
                qry.Append($"\n			FROM 업체별시세관리                                                                          ");
                qry.Append($"\n			WHERE 사용자 = '{userId}'                                                                   ");
                qry.Append($"\n			  AND 매출단가 > 0 AND 매입단가 > 6                                                         ");
                qry.Append($"\n			UNION ALL                                                                              ");
                qry.Append($"\n			SELECT                                                                                 ");
                qry.Append($"\n			  품명                                                                                   ");
                qry.Append($"\n			, 원산지                                                                                  ");
                qry.Append($"\n			, 규격                                                                                   ");
                qry.Append($"\n			, REPLACE(단위, 'kg', '') AS 단위                                                          ");
                qry.Append($"\n			, CASE WHEN NOT (매입구분 = '국내매입' OR 통관 = '통관') THEN ISNULL(재고수, 0) ELSE 0 END 미통관      ");
                qry.Append($"\n			, CASE WHEN (매입구분 = '국내매입' OR 통관 = '통관') THEN ISNULL(재고수, 0) ELSE 0 END 통관           ");
                qry.Append($"\n			, ISNULL(예약수, 0) AS 예약수                                                               ");
                qry.Append($"\n			, 0 AS 매출수                                                                             ");
                qry.Append($"\n			FROM 품명별재고현황                                                                          ");
                qry.Append($"\n			WHERE 사용자 = '{userId}'                                                                   ");
                qry.Append($"\n	            AND ((적요 NOT LIKE '%제외%' AND 적요 NOT LIKE '%back%' AND 적요 NOT LIKE '%백쉽%' AND 적요 NOT LIKE '%빽쉽%' ) OR 적요 IS NULL)  ");
                qry.Append($"\n			UNION ALL                                                                              ");
                qry.Append($"\n			SELECT                                                                                 ");
                qry.Append($"\n			  품명                                                                                   ");
                qry.Append($"\n			, 원산지                                                                                  ");
                qry.Append($"\n			, 규격                                                                                   ");
                qry.Append($"\n			, REPLACE(단위, 'kg', '') AS 단위                                                          ");
                qry.Append($"\n			, 0 AS 미통관                                                                             ");
                qry.Append($"\n			, 0 AS 통관                                                                              ");
                qry.Append($"\n			, 0 AS 예약수                                                                             ");
                qry.Append($"\n			, ISNULL(매출수, 0) AS 매출수                                                               ");
                qry.Append($"\n			FROM 품명별매출현황                                                                          ");
                qry.Append($"\n			WHERE 사용자 = 6                                                                          ");
            }
            qry.Append($"\n		) AS t                                                                                     ");
            qry.Append($"\n		WHERE ISNUMERIC(단위) > 0                                                                    ");
            qry.Append($"\n		GROUP BY 품명, 원산지, 규격, 단위                                                                  ");
            qry.Append($"\n	) AS t                                                                                         ");
            qry.Append($"\n	WHERE 1=1                                                                                         ");
            if(!string.IsNullOrEmpty(product))
                qry.Append($"\n	  {whereSql("품명", product)}                                                                 ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n	  {whereSql("원산지", origin)}                                                                 ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n	  {whereSql("규격", sizes)}                                                                 ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n	  {whereSql("단위", unit)}                                                                 ");
            qry.Append($"\n	GROUP BY 품명, 원산지, 규격                                                                          ");
            qry.Append($"\n) AS t1                                                                                         ");
            qry.Append($"\nLEFT OUTER JOIN (                                                                               ");
            qry.Append($"\n	SELECT                                                                                         ");
            qry.Append($"\n	 *                                                                                             ");
            qry.Append($"\n	FROM(                                                                                          ");
            qry.Append($"\n		SELECT                                                                                     ");
            qry.Append($"\n		  ROW_NUMBER() OVER(PARTITION BY 품명, 원산지, 규격 ORDER BY 매입단가, 매출단가 DESC) AS #Row           ");
            qry.Append($"\n		, 품명                                                                                       ");
            qry.Append($"\n		, 원산지                                                                                      ");
            qry.Append($"\n		, 규격                                                                                       ");
            qry.Append($"\n		, 매출단가 * 단위수량 AS 매출단가                                                                       ");
            qry.Append($"\n		, 매입단가 * 단위수량 AS 매입단가                                                                       ");
            qry.Append($"\n		FROM 업체별시세관리 AS a                                                                         ");
            qry.Append($"\n		WHERE 사용자 = '{userId}'                                                                       ");
            qry.Append($"\n		  AND 매입일자 = (                                                                             ");
            qry.Append($"\n		  	SELECT MAX(매입일자) FROM 업체별시세관리 AS b                                                    ");
            qry.Append($"\n		  	WHERE 사용자 = '{userId}'                                                                   ");
            qry.Append($"\n		  	  AND  a.품명 = b.품명                                                                     ");
            qry.Append($"\n		  	  AND  a.원산지 = b.원산지                                                                  ");
            qry.Append($"\n		  	  AND  a.규격 = b.규격                                                                     ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n	  {whereSql("품명", product)}                                                                 ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n	  {whereSql("원산지", origin)}                                                                 ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n	  {whereSql("규격", sizes)}                                                                 ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n	  {whereSql("단위", unit)}                                                                 ");
            qry.Append($"\n		  	GROUP BY 품명, 원산지, 규격                                                                  ");
            qry.Append($"\n		  	  )                                                                                    ");
            qry.Append($"\n		  AND 매입단가 > 6                                                                             ");
            qry.Append($"\n	) AS t                                                                                         ");
            qry.Append($"\n	WHERE #Row = 1                                                                                 ");
            qry.Append($"\n) AS t2                                                                                         ");
            qry.Append($"\n  ON t1.품명 = t2.품명                                                                              ");
            qry.Append($"\n  AND t1.원산지 = t2.원산지                                                                          ");
            qry.Append($"\n  AND t1.규격 = t2.규격                                                                             ");
            qry.Append($"\nLEFT OUTER JOIN (                                                                               ");
            qry.Append($"\n	SELECT                                                                                         ");
            qry.Append($"\n	  품명                                                                                           ");
            qry.Append($"\n	, 원산지                                                                                          ");
            qry.Append($"\n	, 규격                                                                                           ");
            qry.Append($"\n	, AVG(매입단가) AS 매입단가                                                                           ");
            qry.Append($"\n	FROM(                                                                                          ");
            qry.Append($"\n		SELECT                                                                                     ");
            qry.Append($"\n		  ROW_NUMBER() OVER(PARTITION BY 품명, 원산지, 규격 ORDER BY 매입단가, 매출단가 DESC) AS #Row           ");
            qry.Append($"\n		, 품명                                                                                       ");
            qry.Append($"\n		, 원산지                                                                                      ");
            qry.Append($"\n		, 규격                                                                                       ");
            qry.Append($"\n		, 매출단가 * 단위수량 AS 매출단가                                                                       ");
            qry.Append($"\n		, 매입단가 * 단위수량 AS 매입단가                                                                       ");
            qry.Append($"\n		FROM 업체별시세관리 AS a                                                                         ");
            qry.Append($"\n		WHERE 사용자 = '{userId}'                                                                       ");
            qry.Append($"\n		  AND 매입일자 = (                                                                             ");
            qry.Append($"\n		  	SELECT MAX(매입일자) FROM 업체별시세관리 AS b                                                    ");
            qry.Append($"\n		  	WHERE 사용자 = '{userId}'                                                                   ");
            qry.Append($"\n		  	  AND  a.품명 = b.품명                                                                     ");
            qry.Append($"\n		  	  AND  a.원산지 = b.원산지                                                                  ");
            qry.Append($"\n		  	  AND  a.규격 = b.규격                                                                     ");
            qry.Append($"\n		  	GROUP BY 품명, 원산지, 규격                                                                  ");
            qry.Append($"\n		  	  )                                                                                    ");
            qry.Append($"\n 	  AND 매입일자 >= '{editSttdate.ToString("yyyy-MM-dd")}'                                                                            ");
            qry.Append($"\n 	  AND 매입일자 <= '{editEnddate.ToString("yyyy-MM-dd")}'                                                                            ");
            qry.Append($"\n		  AND 매입단가 > 6                                                                             ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n	  {whereSql("품명", product)}                                                                 ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n	  {whereSql("원산지", origin)}                                                                 ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n	  {whereSql("규격", sizes)}                                                                 ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n	  {whereSql("단위", unit)}                                                                 ");
            qry.Append($"\n	) AS t                                                                                         ");
            qry.Append($"\n	WHERE #Row <= {avgCnt}                                                                               ");
            qry.Append($"\n	GROUP BY 품명, 원산지, 규격                                                                          ");
            qry.Append($"\n) AS t3                                                                                         ");
            qry.Append($"\n  ON t1.품명 = t3.품명                                                                              ");
            qry.Append($"\n  AND t1.원산지 = t3.원산지                                                                          ");
            qry.Append($"\n  AND t1.규격 = t3.규격                                                                             ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            var table = new DataTable();
            table.Load(dr);

            return table;
        }

        private string whereSqlOr(DataTable dt, bool isSimple = false)
        {
            string total_whr = "";
            string whr = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (isSimple)
                {
                    whr = "(품명 = '" + dt.Rows[i]["product"].ToString() + "'"
                            + " AND 원산지 = '" + dt.Rows[i]["origin"].ToString() + "'"
                            + " AND 규격 = '" + dt.Rows[i]["sizes"].ToString() + "')";
                }
                else
                { 
                    whr = "(품명 = '" + dt.Rows[i]["product"].ToString() + "'"
                        + " AND 원산지 = '" + dt.Rows[i]["origin"].ToString() + "'"
                        + " AND 규격 = '" + dt.Rows[i]["sizes"].ToString() + "'"
                        + " AND 단위 = '" + dt.Rows[i]["unit"].ToString() + "'"
                        + " AND ISNULL(가격단위, '') = '" + dt.Rows[i]["price_unit"].ToString() + "'"
                        + " AND ISNULL(단위수량, '') = '" + dt.Rows[i]["unit_count"].ToString() + "'"
                        + " AND SEAOVER단위 = '" + dt.Rows[i]["seaover_unit"].ToString() + "'"
                        + " AND ISNULL(구분, '') = '" + dt.Rows[i]["division"].ToString() + "')";
                    }
                if (string.IsNullOrEmpty(total_whr))
                    total_whr = whr;
                else
                    total_whr += "\n OR " + whr;
            }
            return total_whr;
        }


        public DataTable GetReservedDetail(string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                        ");
            qry.Append($"\n   t.품명                                                                                        ");
            qry.Append($"\n , t.원산지                                                                                       ");
            qry.Append($"\n , t.규격                                                                                        ");
            qry.Append($"\n , RTRIM(LTRIM(REPLACE(t.단위, 'kg', ''))) AS 단위                                                 ");
            qry.Append($"\n , STUFF((                                                                                     ");
            qry.Append($"\n  	SELECT ' ' + 예약상세                                                                         ");
            qry.Append($"\n  	FROM 품명별재고현황                                                                             ");
            qry.Append($"\n  	WHERE 사용자 = '{userId}'                                                                      ");
            qry.Append($"\n       AND ISNULL(품명, '') =  ISNULL(t.품명, '')                                                  ");
            qry.Append($"\n 	  AND ISNULL(원산지, '') =  ISNULL(t.원산지, '')                                               ");
            qry.Append($"\n 	  AND ISNULL(규격, '') =  ISNULL(t.규격, '')                                                  ");
            qry.Append($"\n 	  AND RTRIM(LTRIM(REPLACE(단위, 'kg', ''))) =  RTRIM(LTRIM(REPLACE(t.단위, 'kg', '')))        ");
            qry.Append($"\n     GROUP BY 예약상세, 입고일자, 적요, 매출원가                                                           ");
            qry.Append($"\n     FOR xml path('')), 1, 1, '') AS 예약상세                                                      ");
            qry.Append($"\n FROM 품명별재고현황 AS t                                                                            ");
            qry.Append($"\n WHERE t.사용자 = '{userId}'                                                                        ");
            qry.Append($"\n   AND t.품명 = '{product}'                                                                        ");
            qry.Append($"\n   AND t.원산지 = '{origin}'                                                                        ");
            qry.Append($"\n   AND t.규격 = '{sizes}'                                                                        ");
            if(!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND RTRIM(LTRIM(REPLACE(t.단위, 'kg', ''))) = '{unit}'                                                                        ");
            qry.Append($"\n GROUP BY t.품명, t.원산지, t.규격, t.단위                                                             ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            var table = new DataTable();
            table.Load(dr);

            return table;
        }

        public DataTable GetProductCostComparison(string product, string origin, string sizes, string unit, string division, DateTime editSttdate, DateTime editEnddate, int avgCnt, int salesMonth)
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n SELECT                                                                                                                                                 ");
            qry.Append($"\n t.*                                                                                                                                                    ");
            qry.Append($"\n , CASE                                                                                                                                                 ");
            qry.Append($"\n 	WHEN t.규격2 = '.' THEN NULL                                                                                                                         ");
            qry.Append($"\n 	WHEN t.규격2 = '-' THEN NULL                                                                                                                         ");
            qry.Append($"\n   	WHEN ISNUMERIC(t.규격2) = 1 THEN CONVERT(float, t.규격2)                                                                                               ");
            qry.Append($"\n   	ELSE NULL                                                                                                                                          ");
            qry.Append($"\n   END AS 규격3                                                                                                                                           ");
            qry.Append($"\n , CASE                                                                                                                                                 ");
            qry.Append($"\n 	WHEN t.규격2 = '.' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN t.규격2 = '-' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                                                ");
            qry.Append($"\n   	ELSE t.규격2                                                                                                                                         ");
            qry.Append($"\n   END AS 규격4                                                                                                                                           ");
            qry.Append($"\n FROM(                                                                                                                                                  ");
            qry.Append($"\n 	SELECT                                                                                                                                             ");
            qry.Append($"\n       t.품명                                                                                                                                           ");
            qry.Append($"\n	    , t.규격                                                                                                                                          ");
            qry.Append($"\n	    , t.원산지                                                                                                                                         ");
            qry.Append($"\n	    , CONVERT(float,t.단위) AS 단위                                                                                                                     ");
            qry.Append($"\n	    , t.매출단가                                                                                                                                        ");
            qry.Append($"\n	    , t.계산단위                                                                                                                                        ");
            qry.Append($"\n	    , t.일반시세                                                                                                                                        ");
            qry.Append($"\n	    , t.매출원가                                                                                                                                        ");
            qry.Append($"\n	    , SUM(t.재고수) AS   재고수                                                                                                                           ");
            qry.Append($"\n	    , SUM(t.예약수) AS  예약수                                                                                                                            ");
            qry.Append($"\n	    , SUM(t.재고금액) AS  재고금액                                                                                                                          ");
            qry.Append($"\n	    , SUM(t.평균원가) AS 평균원가                                                                                                                           ");
            qry.Append($"\n	    , SUM(t.매출수) AS 매출수                                                                                                                             ");
            qry.Append($"\n	    , SUM(t.매출금액) AS 매출금액                                                                                                                           ");
            qry.Append($"\n	    , t.최저단가                                                                                                                                        ");
            qry.Append($"\n 	, CASE WHEN CHARINDEX('미', t.규격) > 0 AND CHARINDEX('/', t.규격) > 0 THEN                                                                            ");
            qry.Append($"\n 		  CASE WHEN CHARINDEX('미', t.규격) > CHARINDEX('/', t.규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', t.규격))                                          ");
            qry.Append($"\n 			   WHEN CHARINDEX('미', t.규격) < CHARINDEX('/', t.규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', t.규격))                                          ");
            qry.Append($"\n 	  	  END                                                                                                                                          ");
            qry.Append($"\n 	   WHEN CHARINDEX('미', t.규격) > 0 THEN SUBSTRING(t.규격, 0, CHARINDEX('미', t.규격))                                                                    ");
            qry.Append($"\n 	   WHEN CHARINDEX('/', t.규격) > 0 THEN SUBSTRING(t.규격, 0, CHARINDEX('/', t.규격))                                                                    ");
            qry.Append($"\n 	       ELSE 규격                                                                                                                                     ");
            qry.Append($"\n 	  END AS 규격2                                                                                                                                       ");
            qry.Append($"\n 	FROM(                                                                                                                                              ");
            qry.Append($"\n 		SELECT                                                                                                                                         ");
            qry.Append($"\n 		  t1.품명                                                                                                                                        ");
            qry.Append($"\n 		, t1.규격                                                                                                                                        ");
            qry.Append($"\n 		, t1.원산지                                                                                                                                       ");
            qry.Append($"\n 		, ISNULL(t1.단위  , 0) AS 단위                                                                                                                     ");
            qry.Append($"\n 		, ISNULL(t1.SEAOVER단위  , 0) AS SEAOVER단위                                                                                                                     ");
            qry.Append($"\n 		, ISNULL(t1.매출단가, 0) AS 매출단가                                                                                                                  ");
            qry.Append($"\n 		, ISNULL(t1.계산단위, 0) AS 계산단위                                                                                                                  ");
            qry.Append($"\n 		, ISNULL(t1.일반시세, 0) AS 일반시세                                                                                                                  ");
            qry.Append($"\n 		, ISNULL(t2.매출원가, 0) AS 매출원가                                                                                                                  ");
            qry.Append($"\n 		, ISNULL(t2.재고수 , 0) AS 재고수                                                                                                                   ");
            qry.Append($"\n 		, ISNULL(t2.예약수 , 0) AS 예약수                                                                                                                   ");
            qry.Append($"\n 		, ISNULL(t2.재고금액, 0) AS 재고금액                                                                                                                ");
            qry.Append($"\n 		, CASE WHEN CHARINDEX(t1.가격단위, '묶') > 0 THEN ISNULL(t2.평균원가, 0)/t1.단위수량                                                                ");
            qry.Append($"\n 			   WHEN CHARINDEX(t1.가격단위, '팩') > 0 THEN ISNULL(t2.평균원가, 0)/t1.단위수량                                                                ");
            qry.Append($"\n 			   ELSE ISNULL(t2.평균원가, 0) END AS 평균원가 			                                                                                        ");
            qry.Append($"\n 		, ISNULL(t2.매출수 , 0) AS 매출수                                                                                                                   ");
            qry.Append($"\n 		, ISNULL(t2.매출금액, 0) AS 매출금액                                                                                                                  ");
            qry.Append($"\n 		, ISNULL(t3.최저단가, 0) AS 최저단가                                                                                                                  ");
            qry.Append($"\n 		FROM(                                                                                                                                          ");
            qry.Append($"\n 			SELECT                                                                                                                                     ");
            qry.Append($"\n 		 	  t1.품명                                                                                                                                    ");
            qry.Append($"\n 		 	, t1.규격                                                                                                                                    ");
            qry.Append($"\n 		 	, t1.원산지                                                                                                                                   ");
            qry.Append($"\n 		 	, t1.단위                                                                                                                                    ");
            qry.Append($"\n 		 	, t1.SEAOVER단위                                                                                                                               ");
            qry.Append($"\n 		 	, t1.가격단위                                                                                                                                  ");
            qry.Append($"\n 		 	, t1.단위수량                                                                                                                                  ");
            qry.Append($"\n 		 	, t1.매출단가                                                                                                                                  ");
            qry.Append($"\n 		 	, t1.계산단위                                                                                                                                  ");
            qry.Append($"\n 		 	, t2.일반시세                                                                                                                                  ");
            qry.Append($"\n 		 	FROM (                                                                                                                                     ");
            qry.Append($"\n 		 		SELECT                                                                                                                                 ");
            qry.Append($"\n 		 		  품명                                                                                                                                   ");
            qry.Append($"\n 		 		, 규격                                                                                                                                   ");
            qry.Append($"\n 		 		, 원산지                                                                                                                                  ");
            qry.Append($"\n 		 		, 단위                                                                                                                                   ");
            qry.Append($"\n 		 		, 가격단위                                                                                                                                  ");
            qry.Append($"\n 		 		, 단위수량                                                                                                                                  ");
            qry.Append($"\n 		 		, SEAOVER단위                                                                                                                                   ");
            qry.Append($"\n 		 		, 매출단가                                                                                                                                 ");
            qry.Append($"\n 		 		, 계산단위                                                                                                                                 ");
            qry.Append($"\n 		 		FROM(                                                                                                                                  ");
            qry.Append($"\n 		 			SELECT                                                                                                                             ");
            qry.Append($"\n 		 			  대분류                                                                                                                              ");
            qry.Append($"\n 		 			, 품명                                                                                                                               ");
            qry.Append($"\n 		 			, 규격                                                                                                                               ");
            qry.Append($"\n 		 			, 원산지                                                                                                                              ");
            qry.Append($"\n 		 			, 단위                                                                                                                               ");
            qry.Append($"\n 		 			, 가격단위                                                                                                                                  ");
            qry.Append($"\n 		 			, 단위수량                                                                                                                                  ");
            qry.Append($"\n 		 			, SEAOVER단위                                                                                                                               ");
            qry.Append($"\n 		 			, ROUND(ISNULL(매출단가, 0), 0 , 1) AS 매출단가                                                                                           ");
            qry.Append($"\n 		 			, CASE WHEN RTRIM(LTRIM(가격단위)) = '묶' OR RTRIM(LTRIM(가격단위)) = '묶음' THEN CONVERT(VARCHAR, CONVERT(float, SEAOVER단위)/ CONVERT(float, 단위수량))        ");
            qry.Append($"\n 		 				   ELSE SEAOVER단위                                                                                                              ");
            qry.Append($"\n 		 			  END AS 계산단위                                                                                                                      ");
            qry.Append($"\n 		 			FROM 업체별시세관리                                                                                                                      ");
            qry.Append($"\n 		 			WHERE 사용자 = '{userId}'                                                                                                               ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append(whereSql("품명", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append(whereSql("원산지", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append(whereSql("규격", sizes.Trim()));
            }
            if (!string.IsNullOrEmpty(unit.Trim()))
            {
                qry.Append(whereSql("단위", unit.Trim()));
            }
            if (!string.IsNullOrEmpty(division.Trim()))
            {
                qry.Append(whereSql("구분", division.Trim()));
            }
            qry.Append($"\n 		 			) AS t                                                                                                                             ");
            qry.Append($"\n 		 		GROUP BY 대분류, 품명, 규격, 원산지, 단위, SEAOVER단위, 매출단가, 계산단위, 가격단위, 단위수량                                          ");
            qry.Append($"\n 		 	) AS t1                                                                                                                                    ");
            qry.Append($"\n 		 	LEFT OUTER JOIN (                                                                                                                          ");
            qry.Append($"\n 		 	 	SELECT                                                                                                                                 ");
            qry.Append($"\n 		 	 	  t.품명                                                                                                                                 ");
            qry.Append($"\n 		 	 	, t.원산지                                                                                                                                ");
            qry.Append($"\n 		 	 	, t.규격                                                                                                                                 ");
            qry.Append($"\n 		 	 	, t.계산단위                                                                                                                               ");
            qry.Append($"\n 		 	 	, AVG(t.매입단가) AS 일반시세                                                                                                                 ");
            qry.Append($"\n 		 	 	FROM(                                                                                                                                  ");
            qry.Append($"\n 		 	 	   SELECT                                                                                                                              ");
            qry.Append($"\n 		 	 		  t.*                                                                                                                              ");
            qry.Append($"\n 		 	 		, ROW_NUMBER() OVER(PARTITION BY t.품명, t.원산지, t.규격, t.계산단위 ORDER BY t.매입단가 ASC) AS Row#                                          ");
            qry.Append($"\n 		 	 		FROM (                                                                                                                             ");
            qry.Append($"\n 		 	 			SELECT                                                                                                                         ");
            qry.Append($"\n 		 	 			  품명 AS 품명                                                                                                                     ");
            qry.Append($"\n 		 	 			, 원산지 AS 원산지                                                                                                                  ");
            qry.Append($"\n 		 	 			, 규격 AS 규격                                                                                                                     ");
            qry.Append($"\n 		 	 			, 매입단가 AS 매입단가                                                                                                                ");
            qry.Append($"\n 		 	 			, 매입처 AS 매입처                                                                                                                  ");
            qry.Append($"\n 		 	 			, CASE                                                                                                                         ");
            qry.Append($"\n 		 	 				WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(float, SEAOVER단위)/CONVERT(float, 단위수량)                                       ");
            qry.Append($"\n 		 	 				ELSE SEAOVER단위 END AS 계산단위                                                                                                ");
            qry.Append($"\n 		 	 			, 매입일자 AS 매입일자                                                                                                                ");
            qry.Append($"\n 		 	 			FROM 업체별시세관리                                                                                                                  ");
            qry.Append($"\n 		 	 			WHERE 사용자 = '{userId}'                                                                                                           ");
            qry.Append($"\n 		 	 			   AND 매입단가 > 5                                                                                                                ");
            qry.Append($"\n 		 	 	           AND 매입일자 >= '{editSttdate.ToString("yyyy-MM-dd")}'                                                                                                    ");
            qry.Append($"\n 		 	 	           AND 매입일자 <= '{editEnddate.ToString("yyyy-MM-dd")}'                                                                                                    ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append(whereSql("품명", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append(whereSql("원산지", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append(whereSql("규격", sizes.Trim()));
            }
            if (!string.IsNullOrEmpty(unit.Trim()))
            {
                qry.Append(whereSql("단위", unit.Trim()));
            }
            if (!string.IsNullOrEmpty(division.Trim()))
            {
                qry.Append(whereSql("구분", division.Trim()));
            }
            qry.Append($"\n 		 	 	     ) AS t                                                                                                                            ");
            qry.Append($"\n 		 	 	 ) AS t                                                                                                                                ");
            qry.Append($"\n 		 	 	WHERE Row# <= {avgCnt}                                                                                                                        ");
            qry.Append($"\n 		 	 	GROUP BY t.품명, t.원산지, t.규격, t.계산단위                                                                                                    ");
            qry.Append($"\n 		 	 ) AS t2                                                                                                                                   ");
            qry.Append($"\n 		 	   ON t1.품명 = t2.품명                                                                                                                        ");
            qry.Append($"\n 		 	   AND t1.원산지 = t2.원산지                                                                                                                    ");
            qry.Append($"\n 		 	   AND t1.규격 = t2.규격                                                                                                                       ");
            qry.Append($"\n 		 	   AND t1.계산단위 = t2.계산단위                                                                                                                  ");
            qry.Append($"\n 		) AS t1                                                                                                                                        ");
            qry.Append($"\n 		LEFT OUTER JOIN (                                                                                                                              ");
            qry.Append($"\n 			SELECT                                                                                                                                     ");
            qry.Append($"\n 			  품명                                                                                                                                       ");
            qry.Append($"\n 			, 원산지                                                                                                                                      ");
            qry.Append($"\n 			, 규격                                                                                                                                       ");
            qry.Append($"\n 			, 단위                                                                                                                                       ");
            qry.Append($"\n 			, SUM(ISNULL(매출원가, 0)) AS 매출원가                                                                                                            ");
            qry.Append($"\n 			, SUM(ISNULL(재고수 , 0)) AS  재고수                                                                                                            ");
            qry.Append($"\n 			, SUM(ISNULL(예약수 , 0)) AS  예약수                                                                                                            ");
            qry.Append($"\n 			, SUM(ISNULL(재고금액, 0)) AS 재고금액                                                                                                            ");
            qry.Append($"\n 			, SUM(ISNULL(평균원가, 0)) AS 평균원가                                                                                                            ");
            qry.Append($"\n 			, SUM(ISNULL(매출수 , 0)) AS  매출수                                                                                                            ");
            qry.Append($"\n 			, SUM(ISNULL(매출금액, 0)) AS 매출금액                                                                                                            ");
            qry.Append($"\n 			FROM(                                                                                                                                      ");
            qry.Append($"\n 				SELECT                                                                                                                                 ");
            qry.Append($"\n 				  품명                                                                                                                                   ");
            qry.Append($"\n 				, 원산지                                                                                                                                  ");
            qry.Append($"\n 				, 규격                                                                                                                                   ");
            qry.Append($"\n 				, REPLACE(REPLACE(단위, 'kg', ''), '전후','') AS 단위                                                                                                          ");
            qry.Append($"\n 				, AVG(CASE WHEN 매출원가 = 0 THEN NULL ELSE 매출원가 END) AS 매출원가                                                                             ");
            qry.Append($"\n 				, SUM(ISNULL(재고수, 0)) AS 재고수                                                                                                          ");
            qry.Append($"\n 				, SUM(ISNULL(예약수, 0)) AS 예약수                                                                                                          ");
            qry.Append($"\n 				, SUM(ISNULL(재고금액, 0)) AS 재고금액                                                                                                        ");
            qry.Append($"\n 				, CASE WHEN SUM(ISNULL(재고수2, 0)) > 0 THEN SUM(ISNULL(재고금액2, 0)) / SUM(ISNULL(재고수2, 0)) ELSE 0 END AS 평균원가                            ");
            qry.Append($"\n 				, 0 AS 매출수                                                                                                                             ");
            qry.Append($"\n 				, 0 AS 매출금액                                                                                                                            ");
            qry.Append($"\n 				FROM ( 	                                                                                                                               ");
            qry.Append($"\n 				 	SELECT                                                                                                                             ");
            qry.Append($"\n 					  품명                                                                                                                               ");
            qry.Append($"\n 					, 원산지                                                                                                                              ");
            qry.Append($"\n 					, 규격                                                                                                                               ");
            qry.Append($"\n 					, 단위                                                                                                                               ");
            qry.Append($"\n 					, 매출원가                                                                                                                             ");
            qry.Append($"\n 					, 재고수                                                                                                                              ");
            qry.Append($"\n 					, 예약수                                                                                                                              ");
            qry.Append($"\n 					, 재고금액                                                                                                                             ");
            qry.Append($"\n 					, CASE WHEN 재고금액 > 0 THEN 재고수 ELSE 0 END AS 재고수2                                                                                  ");
            qry.Append($"\n 					, CASE WHEN 재고금액 > 0 THEN 재고금액 ELSE 0 END AS 재고금액2                                                                                ");
            qry.Append($"\n 					FROM 품명별재고현황                                                                                                                      ");
            qry.Append($"\n 					WHERE 사용자 = '{userId}'                                                                                                               ");
            qry.Append($"\n 					  AND (매입처 NOT LIKE '%-DL%' OR 매입처 NOT LIKE '%-LT%')                                                                              ");
            qry.Append($"\n 					  AND ((적요 NOT LIKE '%제외%' AND 적요 NOT LIKE '%back%' AND 적요 NOT LIKE '%백쉽%' AND 적요 NOT LIKE '%빽쉽%' ) OR 적요 IS NULL)  ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append(whereSql("품명", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append(whereSql("원산지", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append(whereSql("규격", sizes.Trim()));
            }

            qry.Append($"\n 				) AS a                                                                                                                                 ");
            qry.Append($"\n 				GROUP BY 품명, 원산지, 규격, REPLACE(REPLACE(단위, 'kg', ''), '전후','')                                                                                                              ");
            qry.Append($"\n 				UNION ALL                                                                                                                              ");
            qry.Append($"\n 				SELECT                                                                                                                                 ");
            qry.Append($"\n 				  품명                                                                                                                                   ");
            qry.Append($"\n 				, 원산지                                                                                                                                  ");
            qry.Append($"\n 				, 규격                                                                                                                                   ");
            qry.Append($"\n 				, REPLACE(REPLACE(단위, 'kg', ''), '전후','') AS 단위                                                                                       ");
            qry.Append($"\n 				, 0 AS 매출원가                                                                                                                            ");
            qry.Append($"\n 				, 0 AS 재고수                                                                                                                             ");
            qry.Append($"\n 				, 0 AS 예약수                                                                                                                             ");
            qry.Append($"\n 				, 0 AS 재고금액                                                                                                                            ");
            qry.Append($"\n 				, 0 AS 평균원가                                                                                                                            ");
            qry.Append($"\n 				, SUM(ISNULL(매출수, 0)) AS 매출수                                                                                                          ");
            qry.Append($"\n 				, SUM(ISNULL(매출금액, 0)) AS 매출금액                                                                                                        ");
            qry.Append($"\n 				FROM 품명별매출현황                                                                                                                          ");
            qry.Append($"\n 				WHERE 사용자 = '{salesMonth}'                                                                                                                        ");
            if (!string.IsNullOrEmpty(product.Trim()))
            {
                qry.Append(whereSql("품명", product.Trim()));
            }
            if (!string.IsNullOrEmpty(origin.Trim()))
            {
                qry.Append(whereSql("원산지", origin.Trim()));
            }
            if (!string.IsNullOrEmpty(sizes.Trim()))
            {
                qry.Append(whereSql("규격", sizes.Trim()));
            }
            qry.Append($"\n 				GROUP BY 품명, 원산지, 규격, REPLACE(REPLACE(단위, 'kg', ''), '전후','')                                                                         ");
            qry.Append($"\n 			) AS t                                                                                                                                     ");
            qry.Append($"\n 			GROUP BY 품명, 원산지, 규격, 단위                                                                                                                  ");
            qry.Append($"\n 		) AS t2                                                                                                                                        ");
            qry.Append($"\n 		 ON t1.품명 = t2.품명                                                                                                                              ");
            qry.Append($"\n 		AND t1.원산지 = t2.원산지                                                                                                                           ");
            qry.Append($"\n 		AND t1.규격 = t2.규격                                                                                                                              ");
            qry.Append($"\n 		AND t1.SEAOVER단위 = t2.단위                                                                                                                           ");
            qry.Append($"\n	 	LEFT OUTER JOIN (                                                                                                                                                              ");
            qry.Append($"\n	 		SELECT                                                                                                                                                                     ");
            qry.Append($"\n	 		   대분류                                                                                                                                                                     ");
            qry.Append($"\n	 		 , 품명                                                                                                                                                                      ");
            qry.Append($"\n	 		 , 원산지                                                                                                                                                                     ");
            qry.Append($"\n	 		 , 규격                                                                                                                                                                      ");
            qry.Append($"\n		 	 , CASE WHEN RTRIM(LTRIM(가격단위)) = '묶' OR RTRIM(LTRIM(가격단위)) = '묶음' THEN CONVERT(VARCHAR, CONVERT(float, SEAOVER단위)/ CONVERT(float, 단위수량))                                ");
            qry.Append($"\n					ELSE SEAOVER단위                                                                                                                                                     ");
            qry.Append($"\n			   END AS 계산단위                                                                                                                                                             ");
            qry.Append($"\n	 		 , MIN(ISNULL(매입단가, 0)) AS 최저단가                                                                                                                                           ");
            qry.Append($"\n	 		 , ISNULL(구분, '') AS 구분                                                                                                                                                    ");
            qry.Append($"\n	 		 FROM 업체별시세관리                                                                                                                                                             ");
            qry.Append($"\n	 		  WHERE 사용자 = '{userId}'                                                                                                                                                     ");
            qry.Append($"\n	 			AND 매입단가 > 5                                                                                                                                                           ");
            qry.Append($"\n	 		 GROUP BY 대분류 , 품명 , 원산지 , 규격 , CASE WHEN RTRIM(LTRIM(가격단위)) = '묶' OR RTRIM(LTRIM(가격단위)) = '묶음' THEN CONVERT(VARCHAR, CONVERT(float, SEAOVER단위)/ CONVERT(float, 단위수량)) ELSE SEAOVER단위 END, ISNULL(구분, '')  ");
            qry.Append($"\n	 	) AS t3			                                                                                                                                                               ");
            qry.Append($"\n	 		ON ISNULL(t1.품명, '') = ISNULL(t3.품명, '')                                                                                                                                   ");
            qry.Append($"\n	 		AND ISNULL(t1.원산지, '') = ISNULL(t3.원산지, '')                                                                                                                               ");
            qry.Append($"\n	 		AND ISNULL(t1.규격, '') = ISNULL(t3.규격, '')                                                                                                                                  ");
            qry.Append($"\n	 		AND ISNULL(t1.계산단위, '') = ISNULL(t3.계산단위, '')                                                                                                                             ");
            qry.Append($"\n 	) AS t                                                                                                                                             ");
            qry.Append($"\n 	GROUP BY t.품명, t.규격, t.원산지, CONVERT(float,t.단위), t.매출단가, t.계산단위, t.일반시세, t.매출원가, t.최저단가                                    ");
            qry.Append($"\n ) AS t                                                                                                                                                 ");
            qry.Append($"\n  ORDER BY t.품명, t.원산지, t.규격3, t.규격2                                                                                                                   ");
            qry.Append($"\n  , CASE WHEN CHARINDEX('대', CASE                                                                                                                       ");
            qry.Append($"\n 	WHEN t.규격2 = '.' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN t.규격2 = '-' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                                                ");
            qry.Append($"\n   	ELSE t.규격2                                                                                                                                         ");
            qry.Append($"\n   END) > 0  THEN 1                                                                                                                                     ");
            qry.Append($"\n         WHEN CHARINDEX('중', CASE                                                                                                                       ");
            qry.Append($"\n 	WHEN t.규격2 = '.' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN t.규격2 = '-' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                                                ");
            qry.Append($"\n   	ELSE t.규격2                                                                                                                                         ");
            qry.Append($"\n   END) > 0 THEN 2                                                                                                                                      ");
            qry.Append($"\n         WHEN CHARINDEX('소', CASE                                                                                                                       ");
            qry.Append($"\n 	WHEN t.규격2 = '.' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN t.규격2 = '-' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                                                ");
            qry.Append($"\n   	ELSE t.규격2                                                                                                                                         ");
            qry.Append($"\n   END) > 0 THEN 3                                                                                                                                      ");
            qry.Append($"\n         WHEN CHARINDEX('L', CASE                                                                                                                       ");
            qry.Append($"\n 	WHEN t.규격2 = '.' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN t.규격2 = '-' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                                                ");
            qry.Append($"\n   	ELSE t.규격2                                                                                                                                         ");
            qry.Append($"\n   END) > 0 THEN 4                                                                                                                                      ");
            qry.Append($"\n         WHEN CHARINDEX('M', CASE                                                                                                                       ");
            qry.Append($"\n 	WHEN t.규격2 = '.' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN t.규격2 = '-' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                                                ");
            qry.Append($"\n   	ELSE t.규격2                                                                                                                                         ");
            qry.Append($"\n   END) > 0 THEN 5                                                                                                                                      ");
            qry.Append($"\n         WHEN CHARINDEX('S', CASE                                                                                                                       ");
            qry.Append($"\n 	WHEN t.규격2 = '.' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN t.규격2 = '-' THEN t.규격2                                                                                                                        ");
            qry.Append($"\n 	WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                                                ");
            qry.Append($"\n   	ELSE t.규격2                                                                                                                                         ");
            qry.Append($"\n   END) > 0 THEN 6                                                                                                                                      ");
            qry.Append($"\n    END                                                                                                                                                 ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            var table = new DataTable();
            table.Load(dr);

            return table;
        }





        private string whereSql(string whrColumn, string whrValue)
        {
            string[] tempStr = null;
            string tempWhr = "";
            string whrStr = "";
            if (!string.IsNullOrEmpty(whrValue.Trim()))
            {
                tempStr = whrValue.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        { 
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   {whrColumn} LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR {whrColumn} LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    whrStr = $"\n	 AND ( {tempWhr} )";
                }
                else
                {
                    whrStr = $"\n	  AND {whrColumn} LIKE '%{whrValue}%'";
                }
            }
            return whrStr;
        }

        private string whereSqlExactly(string whrColumn, string whrValue)
        {
            string[] tempStr = null;
            string tempWhr = "";
            string whrStr = "";
            if (!string.IsNullOrEmpty(whrValue.Trim()))
            {
                tempStr = whrValue.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   {whrColumn} = '{tempStr[i]}' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR {whrColumn} = '{tempStr[i]}' ";
                            }
                        }
                    }
                    whrStr = $"\n	 AND ( {tempWhr} )";
                }
                else
                {
                    whrStr = $"\n	  AND {whrColumn} = '{whrValue}'";
                }
            }
            return whrStr;
        }

        public PriceComparisonModel GetPriceComparisonAsOne(string txtCategory, string txtProduct, string txtOrigin, string txtSizes, string txtUnit, double sales_price)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                                                                                                                            ");
            qry.Append($"\n    *                                                                                                                         ");
            qry.Append($"\n FROM  업체별시세관리                                                                                                           ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                                                      ");
            if (string.IsNullOrEmpty(txtCategory))
            {
                qry.Append($"\n   AND 대분류 IS NULL                                                                                                     ");
            }
            else
            {
                qry.Append($"\n   AND 대분류 = '{txtCategory}'                                                                                                 ");
            }
            
            qry.Append($"\n   AND 품명 = '{txtProduct}'                                                                                                    ");
            qry.Append($"\n   AND 원산지 = '{txtOrigin}'                                                                                                   ");
            qry.Append($"\n   AND 규격 = '{txtSizes}'                                                                                                      ");
            qry.Append($"\n   AND 단위 = '{txtUnit}'                                                                                                       ");
            qry.Append($"\n   AND 매출단가 = {sales_price}                                                                                                 ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetPriceComparisonModelAsOne(dr);
        }

        private List<PriceComparisonModel> SetPriceComparisonModel(SqlDataReader rd)
        {
            List<PriceComparisonModel> list = new List<PriceComparisonModel>();
            while (rd.Read())
            {
                PriceComparisonModel model = new PriceComparisonModel();
                model.category = rd["대분류1"].ToString();
                model.category2 = rd["대분류2"].ToString();
                model.category3 = rd["대분류3"].ToString();
                model.category_code = rd["대분류"].ToString();
                //model.origin_code = rd["원산지코드"].ToString();
                model.origin = rd["원산지"].ToString();
                //model.product_code = rd["품목코드"].ToString();
                model.product = rd["품명"].ToString();
                //model.sizes_code = rd["규격코드"].ToString();
                model.sizes = rd["규격"].ToString();
                model.sizes2 = rd["규격2"].ToString();
                model.sizes3 = rd["규격3"].ToString();
                model.sizes4 = rd["규격4"].ToString();
                model.unit = rd["단위"].ToString();
                model.price_unit = rd["가격단위"].ToString();
                model.unit_count = rd["단위수량"].ToString();
                model.seaover_unit = rd["SEAOVER단위"].ToString();
                model.total_stock = Convert.ToInt32(rd["재고수"]);
                model.reserved_stock = Convert.ToInt32(rd["예약수"]);
                model.reserved_stock_detail = rd["예약상세"].ToString();
                model.sales_count = Convert.ToInt32(rd["매출수"]);
                model.purchase_price = Convert.ToDouble(rd["최저단가"]);
                model.average_price = Convert.ToDouble(rd["일반시세"]);
                model.average_price_details = rd["일반시세상세"].ToString();
                model.sales_price = Convert.ToDouble(rd["매출단가"]);
                model.sales_cost_price = Convert.ToDouble(rd["매출원가"]);
                model.division= rd["구분"].ToString();
                /*model.manager1 = rd["담당자1"].ToString();
                model.manager2 = rd["담당자2"].ToString();*/
                list.Add(model);
            }
            rd.Close();
            return list;
        }

        private PriceComparisonModel SetPriceComparisonModelAsOne(SqlDataReader rd)
        {
            List<PriceComparisonModel> list = new List<PriceComparisonModel>();
            PriceComparisonModel model = new PriceComparisonModel();
            while (rd.Read())
            {
                model.category_code = rd["대분류"].ToString();
                //model.origin_code = rd["원산지코드"].ToString();
                model.origin = rd["원산지"].ToString();
                //model.product_code = rd["품목코드"].ToString();
                model.product = rd["품명"].ToString();
                //model.sizes_code = rd["규격코드"].ToString();
                model.sizes = rd["규격"].ToString();
                model.unit = rd["단위"].ToString();
                model.price_unit = rd["가격단위"].ToString();
                model.unit_count = rd["단위수량"].ToString();
                model.seaover_unit = rd["SEAOVER단위"].ToString();
                model.manager1 = rd["담당자1"].ToString();
                model.manager2 = rd["담당자2"].ToString();
            }
            rd.Close();
            return model;
        }

        public int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
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

        public DataTable GetTempPriceComparison(DateTime sdate, string product, string origin, string sizes, string unit, string manager, string division
                , bool isNoStock, string operating = "", string contract = "", int sortType = 1, int tabType = 1, bool isUserSetting = false, bool isContract = false, int sales_terms = 45, bool isAllData = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                                                           ");
            qry.Append($"\n    a.main_id, a.sub_id, a.product, a.origin, a.sizes, a.unit, a.price_unit, a.unit_count, a.seaover_unit            ");
            qry.Append($"\n  , a.shipment_stock, a.stock, a.sales_count, a.month_sales_count, a.works_day, a.month_around                       ");
            qry.Append($"\n  , CAST(a.exhaustion_date AS CHAR) AS exhaustion_date                                                               ");
            qry.Append($"\n  , CAST(a.contract_date AS CHAR) AS contract_date                                                                   ");
            qry.Append($"\n  , CAST(a.until_days1 AS CHAR) AS until_days1                                                                       ");
            qry.Append($"\n  , CAST(a.min_etd AS CHAR) AS min_etd                                                                               ");
            qry.Append($"\n  , CAST(a.until_days2 AS CHAR) AS until_days2                                                                       ");
            qry.Append($"\n  , CAST(a.isMonth AS CHAR) AS isMonth                                                                               ");
            qry.Append($"\n  , CAST(a.show_sttdate AS CHAR) AS show_sttdate                                                                     ");
            qry.Append($"\n  , CAST(a.show_enddate AS CHAR) AS show_enddate                                                                     ");
            qry.Append($"\n  , CAST(a.hide_date AS CHAR) AS hide_date                                                                           ");
            qry.Append($"\n  , CAST(a.trade_manager AS CHAR) AS trade_manager                                                                   ");
            qry.Append($"\n  , CAST(a.contract AS CHAR) AS contract                                                                             ");
            qry.Append($"\n  , CAST(a.operating AS CHAR) AS operating                                                                           ");
            qry.Append($"\n  , CAST(a.show_product AS CHAR) AS show_product                                                                     ");
            qry.Append($"\n  , CAST(a.contract_product AS CHAR) AS contract_product                                                             ");
            qry.Append($"\n  , CAST(a.month_product AS CHAR) AS month_product                                                                   ");
            qry.Append($"\n  , CAST(a.offer_updatetime AS CHAR) AS offer_updatetime                                                             ");
            qry.Append($"\n  , CONCAT(a.product , '^' , a.origin , '^' , a.sizes , '^' , a.seaover_unit) AS product_code                        ");
            qry.Append($"\n  FROM (                                                                                                                                                           ");
            qry.Append($"\n	  SELECT                                                                                                                                                          ");
            qry.Append($"\n	  a.main_id, a.sub_id, a.product, a.origin, a.sizes, a.unit, a.price_unit, a.unit_count, a.seaover_unit                                                                                ");
            qry.Append($"\n	  , a.shipment_stock, a.stock, a.sales_count, a.sales_count / a.works_day * 21 AS month_sales_count, a.works_day, a.month_around, a.exhaustion_date, a.contract_date, 0 AS until_days1, a.min_etd, 0 AS until_days2                       ");
            qry.Append($"\n	  , a.isMonth, a.show_sttdate, a.show_enddate, a.hide_date, a.trade_manager                                                                                       ");
            qry.Append($"\n	  , b.contract, b.operating                                                                                                                                       ");
            qry.Append($"\n      , CASE WHEN show_sttdate <= '{DateTime.Now.ToString("yyyy-MM-dd")}' AND show_enddate >= '{DateTime.Now.ToString("yyyy-MM-dd")}' THEN 1 ELSE 0 END AS show_product   ");
            qry.Append($"\n      , CASE WHEN b.contract LIKE '%^{DateTime.Now.Month - 1}%' OR b.contract LIKE '%^{DateTime.Now.Month}%' THEN 1 ELSE 0 END AS contract_product                                                                  ");
            qry.Append($"\n      , CASE WHEN a.isMonth = 'TRUE' THEN 1 ELSE 0 END AS month_product                                                                                            ");
            qry.Append($"\n      , '' AS offer_updatetime                                                                                                                                     ");
            qry.Append($"\n	  FROM(                                                                                                                                                           ");
            qry.Append($"\n		  SELECT                                                                                                                                                      ");
            qry.Append($"\n			a.*                                                                                                                                                       ");
            qry.Append($"\n		  FROM(                                                                                                                                                       ");
            qry.Append($"\n			 SELECT                                                                                                                                                   ");
            qry.Append($"\n			   a.*                                                                                                                                                    ");
            qry.Append($"\n			 , IF(IFNULL(b.isMonth, 0) = 1, 'TRUE', 'FALSE') AS isMonth                                                                                               ");
            qry.Append($"\n			 , date_format(IF(IFNULL(b.show_sttdate, '') = '0000-00-00', '', IFNULL(b.show_sttdate, '')),'%Y-%m-%d') AS show_sttdate                                                 ");
            qry.Append($"\n			 , date_format(IF(IFNULL(b.show_enddate, '') = '0000-00-00', '', IFNULL(b.show_enddate, '')),'%Y-%m-%d') AS show_enddate                                                ");
            qry.Append($"\n			 , IFNULL(b.hide_date, '') AS hide_date                                                                                                                   ");
            qry.Append($"\n			 FROM (                                                                                                                                                   ");
            qry.Append($"\n				 SELECT                                                                                 ");
            qry.Append($"\n				   p.main_id                                                                            ");
            qry.Append($"\n				 , p.sub_id                                                                             ");
            qry.Append($"\n				 , p.product                                                                            ");
            qry.Append($"\n				 , p.origin                                                                             ");
            qry.Append($"\n				 , p.sizes                                                                              ");
            qry.Append($"\n				 , p.unit                                                                               ");
            qry.Append($"\n				 , p.price_unit                                                                         ");
            qry.Append($"\n				 , p.unit_count                                                                         ");
            qry.Append($"\n				 , p.seaover_unit                                                                       ");
            qry.Append($"\n				 , p.shipment_stock + p.unpending_before_stock AS shipment_stock                        ");
            qry.Append($"\n				 , p.seaover_unpending_stock + p.seaover_pending_stock - reserved_stock AS stock        ");
            qry.Append($"\n				 , p.sales_count                                                                        ");
            qry.Append($"\n				 , p.works_day                                                                          ");
            qry.Append($"\n				 , p.month_around                                                                       ");
            qry.Append($"\n				 , p.exhaustion_date                                                                    ");
            qry.Append($"\n				 , p.contract_date                                                                      ");
            qry.Append($"\n				 , p.min_etd                                                                            ");
            qry.Append($"\n				 , o.manager AS trade_manager                                                           ");
            qry.Append($"\n				 FROM t_pricecomparison2 AS p                                                            ");
            qry.Append($"\n                 LEFT OUTER JOIN (                                                                   ");
            qry.Append($"\n					SELECT                                                                              ");
            qry.Append($"\n                      product                                                                        ");
            qry.Append($"\n                    , origin                                                                         ");
            qry.Append($"\n                    , sizes                                                                          ");
            qry.Append($"\n					, unit                                                                              ");
            qry.Append($"\n                    , manager                                                                        ");
            qry.Append($"\n                    FROM t_product_other_cost                                                        ");
            qry.Append($"\n                    WHERE IFNULL(manager, '') <> ''                                                  ");
            qry.Append($"\n                    GROUP BY product, origin, sizes, unit, manager                                   ");
            qry.Append($"\n                 )  AS o                                                                             ");
            qry.Append($"\n                   ON p.product = o.product                                                          ");
            qry.Append($"\n                   AND p.origin = o.origin                                                           ");
            qry.Append($"\n                   AND p.sizes = o.sizes                                                             ");
            qry.Append($"\n                   AND (p.unit = o.unit OR p.seaover_unit = o.unit)                                                                ");
            qry.Append($"\n				 WHERE DATE_FORMAT(updatetime,'%Y-%m-%d') = '{DateTime.Now.ToString("yyyy-MM-dd")}'                                   ");
            qry.Append($"\n				   AND sales_terms = {sales_terms}                                   ");
            //전체정렬 특수 검색기준
            if (isNoStock && (sortType == 0 || sortType == 1 || sortType == 2 || sortType == 3))
            {
                if(!isAllData)
                    qry.Append($"\n		   AND (date_add(contract_date, INTERVAL 0 DAY)  <= '{sdate.AddMonths(1).ToString("yyyy-MM-dd")}'                                                                        ");
                qry.Append($"\n		   OR (main_id > 0 AND sub_id <9999))                                                                               ");
            }
            else if (isNoStock && (sortType == 4 || sortType == 5 || sortType == 6))
            {
                if (!isAllData)
                    qry.Append($"\n		   AND (date_add(min_etd, INTERVAL 0 DAY) <= '{sdate.AddMonths(1).ToString("yyyy-MM-dd")}'                                                                               ");
                qry.Append($"\n		   OR (main_id > 0 AND sub_id <9999))                                                                               ");
            }

            //검색항목
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {whereSql("p.product", product)}                     ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {whereSql("p.origin", origin)}                     ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {whereSql("p.sizes", sizes)}                     ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   {whereSql("p.unit", unit)}                     ");
            
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n   {whereSql("p.division", division)}                     ");


            qry.Append($"\n				 GROUP BY product , origin , sizes , unit , price_unit , unit_count , seaover_unit                                                                    ");
            qry.Append($"\n				 , shipment_stock + unpending_before_stock , seaover_unpending_stock + seaover_pending_stock - reserved_stock                                         ");
            qry.Append($"\n				 , sales_count , works_day , month_around , exhaustion_date , contract_date , min_etd, trade_manager                                                  ");
            qry.Append($"\n			 ) AS a                                                                                                                                                   ");
            qry.Append($"\n			 LEFT OUTER JOIN (                                                                                                                                        ");
            qry.Append($"\n				 SELECT                                                                                                                                               ");
            qry.Append($"\n				   product                                                                                                                                            ");
            qry.Append($"\n				 , origin                                                                                                                                             ");
            qry.Append($"\n				 , sizes                                                                                                                                              ");
            qry.Append($"\n				 , unit                                                                                                                                               ");
            qry.Append($"\n				 , IFNULL(isMonth, 0) AS isMonth                                                                                                                      ");
            qry.Append($"\n				 , IFNULL(show_sttdate, '') AS show_sttdate                                                                                                           ");
            qry.Append($"\n				 , IFNULL(show_enddate, '') AS show_enddate                                                                                                           ");
            qry.Append($"\n				 , IFNULL(hide_date, '') AS hide_date                                                                                                                 ");
            qry.Append($"\n				 FROM t_product_other_cost                                                                                                                            ");
            qry.Append($"\n				 GROUP BY product , origin , sizes , unit , IFNULL(isMonth, 0) , IFNULL(show_sttdate, '') , IFNULL(show_enddate, '') , IFNULL(hide_date, '')          ");
            qry.Append($"\n			 )AS b                                                                                                                                                    ");
            qry.Append($"\n			   on a.product = b.product                                                                                                                               ");
            qry.Append($"\n			   AND a.origin = b.origin                                                                                                                                ");
            qry.Append($"\n			   AND a.sizes = b.sizes                                                                                                                                  ");
            qry.Append($"\n			   AND a.seaover_unit = b.unit                                                                                                                            ");
            qry.Append($"\n			 WHERE 1=1                                                                                                                            ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {whereSql("trade_manager", manager)}                     ");
            qry.Append($"\n		  ) AS a                                                                                                                                                      ");
            /*qry.Append($"\n		  LEFT OUTER JOIN (                                                                                                                                           ");
            qry.Append($"\n			SELECT                                                                                                                                                    ");
            qry.Append($"\n			   1 AS id                                                                                                                                                ");
            qry.Append($"\n			 , product                                                                                                                                                ");
            qry.Append($"\n			  , origin                                                                                                                                                ");
            qry.Append($"\n			  , sizes                                                                                                                                                 ");
            qry.Append($"\n			  , unit                                                                                                                                                  ");
            qry.Append($"\n			  , price_unit                                                                                                                                            ");
            qry.Append($"\n			  , unit_count                                                                                                                                            ");
            qry.Append($"\n			  , seaover_unit                                                                                                                                          ");
            qry.Append($"\n			  FROM t_product_group                                                                                                                                    ");
            qry.Append($"\n			  GROUP BY product, origin, sizes, unit, price_unit, unit_count, seaover_unit                                                                             ");
            qry.Append($"\n		  ) AS g                                                                                                                                                      ");
            qry.Append($"\n			ON a.product = g.product                                                                                                                                  ");
            qry.Append($"\n			AND a.origin = g.origin                                                                                                                                   ");
            qry.Append($"\n			AND a.sizes = g.sizes                                                                                                                                     ");
            qry.Append($"\n			AND a.unit = g.unit                                                                                                                                       ");
            qry.Append($"\n		  WHERE IF((a.price_unit <> g.price_unit OR a.unit_count <> g.unit_count OR a.seaover_unit <> g.seaover_unit) AND g.id IS NOT NULL, 1, 0) = 0                 ");*/
            qry.Append($"\n	  ) AS a                                                                                                                                                          ");
            qry.Append($"\n	  LEFT OUTER JOIN (                                                                                                                                               ");
            qry.Append($"\n		SELECT                                                                                                                                                        ");
            qry.Append($"\n		   product                                                                                                                                                    ");
            qry.Append($"\n		 , origin                                                                                                                                                     ");
            qry.Append($"\n		 , GROUP_CONCAT( IF(division = '조업시기', CONCAT('^' , month) , null) ORDER BY month) AS operating                                                                          ");
            qry.Append($"\n		 , GROUP_CONCAT( IF(division = '계약시기', CONCAT('^' , month) , null) ORDER BY month) AS contract                                                                           ");
            qry.Append($"\n		 FROM t_product_contract_recommendation                                                                                                                       ");
            qry.Append($"\n		 GROUP BY product, origin                                                                                                                                     ");
            qry.Append($"\n	  ) AS b                                                                                                                                                          ");
            qry.Append($"\n		ON a.product = b.product                                                                                                                                      ");
            qry.Append($"\n		AND a.origin = b.origin                                                                                                                                       ");
            qry.Append($"\n  ) AS a                                                                                                                                                           ");
            qry.Append($"\n  WHERE 1=1                                                                                                                                                        ");
            
            
            //숨김품목은 제외
            if (tabType != 7)
            {
                qry.Append($"\n    AND (IFNULL(a.hide_date, '') = '' OR a.hide_date < '{DateTime.Now.ToString("yyyy-MM-dd")}')                                                                    ");
                //관심
                if (tabType == 2)
                {
                    if (isUserSetting && isContract)
                        qry.Append($"\n	   AND (month_product = 0 OR a.show_product = 1 OR a.contract_product = 1 OR (a.main_id > 0 AND a.sub_id < 9999))        ");
                    else if (!isUserSetting && isContract)
                        qry.Append($"\n	   AND (month_product = 0 OR a.contract_product = 1 OR (a.main_id > 0 AND a.sub_id < 9999))                              ");
                    else if (isUserSetting && !isContract)
                        qry.Append($"\n	   AND (month_product = 0 OR a.show_product = 1 OR (a.main_id > 0 AND a.sub_id < 9999))                                  ");
                    else
                        qry.Append($"\n	   AND month_product = 0                                                          ");
                }
                //월별
                else if (tabType == 3)
                {
                    if (isUserSetting && isContract)
                        qry.Append($"\n	   AND (month_product = 1 OR a.show_product = 1 OR a.contract_product = 1 OR (a.main_id > 0 AND a.sub_id < 9999))        ");
                    else if (!isUserSetting && isContract)
                        qry.Append($"\n	   AND (month_product = 1 OR a.contract_product = 1 OR (a.main_id > 0 AND a.sub_id < 9999))                              ");
                    else if (isUserSetting && !isContract)
                        qry.Append($"\n	   AND (month_product = 1 OR a.show_product = 1 OR (a.main_id > 0 AND a.sub_id < 9999))                                  ");
                    else
                        qry.Append($"\n	   AND month_product = 1                                                          ");
                }
                //조업시즌
                else if (tabType == 4)
                {
                    if (!string.IsNullOrEmpty(operating))
                        qry.Append($"\n  {whereSql("a.operating", operating.Replace(" ", " ^"))}                     ");
                    else
                        qry.Append($"\n  AND 1=2                     ");
                }
                //계약시즌
                else if (tabType == 5)
                {
                    if (!string.IsNullOrEmpty(contract))
                        qry.Append($"\n  {whereSql("a.contract", contract.Replace(" ", " ^"))}                     ");
                    else
                        qry.Append($"\n  AND 1=2                     ");
                }
                //사용자정의
                else if (tabType == 6)
                    qry.Append($"\n	   AND a.division = 1                                                                                                                                         ");
            }
            //숨김 품목탭에선 숨김품목만
            else if (tabType == 7)
                qry.Append($"\n  AND  a.hide_date >= '{DateTime.Now.ToString("yyyy-MM-dd")}'                          ");

            //qry.Append($"\n  AND  a.contract_date <> ''                          ");

            if(isContract)
                qry.Append($"\n  AND  ((a.contract_date <> '' AND a.main_id = 0) OR a.main_id > 0 )                          ");
            else
                qry.Append($"\n  AND  ((a.main_id = 0) OR a.main_id > 0 )                          ");

            //정렬
            if (sortType == 0)
            {
                qry.Append($"\n ORDER BY show_product DESC, contract_product DESC, IF(date_add(a.contract_date, INTERVAL 0 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.contract_date, INTERVAL 0 DAY), '3000-01-01') ASC                   ");
                qry.Append($"\n  , IF(date_add(a.contract_date, INTERVAL 0 DAY) < '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.contract_date, INTERVAL 0 DAY), '1900-01-01') DESC                                     ");
                qry.Append($"\n  , a.product, a.origin, a.sizes                                                                                                                                   ");
            }
            else if (sortType == 1)
            {
                qry.Append($"\n ORDER BY IF(date_add(a.contract_date, INTERVAL 0 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.contract_date, INTERVAL 0 DAY), '3000-01-01') ASC  ");
                qry.Append($"\n  , IF(date_add(a.contract_date, INTERVAL 0 DAY) < '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.contract_date, INTERVAL 0 DAY), '1900-01-01') DESC          ");
                qry.Append($"\n  , a.product, a.origin, a.sizes                                                                                                       ");
            }
            else if (sortType == 2)
            {
                qry.Append($"\n ORDER BY IF(date_add(a.contract_date, INTERVAL 0 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.contract_date, INTERVAL 0 DAY), '3000-01-01') ASC  ");
                qry.Append($"\n  , a.sales_count DESC, IF(date_add(a.contract_date, INTERVAL 0 DAY) < '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.contract_date, INTERVAL 0 DAY), '1900-01-01') DESC          ");
                qry.Append($"\n  , a.product, a.origin, a.sizes                                                                                                       ");
            }
            else if (sortType == 3)
            {
                qry.Append($"\n ORDER BY IF(date_add(a.contract_date, INTERVAL 0 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.contract_date, INTERVAL 0 DAY), '3000-01-01') ASC  ");
                qry.Append($"\n  ,  a.shipment_stock + a.stock DESC, IF(date_add(a.contract_date, INTERVAL 0 DAY) < '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.contract_date, INTERVAL 0 DAY), '1900-01-01') DESC          ");
                qry.Append($"\n  , a.product, a.origin, a.sizes                                                                                                       ");
            }
            else if (sortType == 4)
            {
                qry.Append($"\n ORDER BY IF(date_add(a.min_etd, INTERVAL 0 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.min_etd, INTERVAL 0 DAY), '3000-01-01') ASC  ");
                qry.Append($"\n  , IF(date_add(a.min_etd, INTERVAL 0 DAY) < '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.min_etd, INTERVAL 0 DAY), '1900-01-01') DESC          ");
                qry.Append($"\n  , a.product, a.origin, a.sizes                                                                                                       ");
            }
            else if (sortType == 5)
            {
                qry.Append($"\n ORDER BY IF(date_add(a.min_etd, INTERVAL 0 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.min_etd, INTERVAL 0 DAY), '3000-01-01') ASC  ");
                qry.Append($"\n  , a.sales_count DESC, IF(date_add(a.min_etd, INTERVAL 0 DAY) < '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.min_etd, INTERVAL 0 DAY), '1900-01-01') DESC          ");
                qry.Append($"\n  , a.product, a.origin, a.sizes                                                                                                       ");

            }
            else if (sortType == 6)
            {
                qry.Append($"\n ORDER BY IF(date_add(a.min_etd, INTERVAL 0 DAY) >= '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.min_etd, INTERVAL 0 DAY), '3000-01-01') ASC  ");
                qry.Append($"\n  ,  a.shipment_stock + a.stock DESC, IF(date_add(a.min_etd, INTERVAL 0 DAY) < '{DateTime.Now.ToString("yyyy-MM-dd")}', date_add(a.min_etd, INTERVAL 0 DAY), '1900-01-01') DESC          ");
                qry.Append($"\n  , a.product, a.origin, a.sizes                                                                                                       ");
            }
            else if (sortType == 7)
                qry.Append($"\n ORDER BY a.product, a.origin, a.sizes   ");

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

