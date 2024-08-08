using Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repositories.SEAOVER.Sales
{
    public class SalesRepository : ClassRoot, ISalesRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        private static string userId;
        public void SetSeaoverId(string user_id)
        {
            userId = user_id;
        }

        public DataTable GetSalesDashboardByProduct(DateTime sttdate, DateTime enddate, List<string> product, string user_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                   ");
            qry.Append($"\n   t.품명                                                  ");
            qry.Append($"\n , t.원산지                                                 ");
            qry.Append($"\n , SUM(t.매출금액) AS 매출금액                                  ");
            qry.Append($"\n , t.매출자                                                 ");
            qry.Append($"\n FROM(                                                    ");
            qry.Append($"\n 	SELECT                                               ");
            qry.Append($"\n 	  t1.품명                                             ");
            qry.Append($"\n 	, t1.원산지                                            ");
            qry.Append($"\n 	, ISNULL(t1.매출금액, 0) + ISNULL(t1.부가세, 0) AS 매출금액  ");
            qry.Append($"\n 	, t2.매출자                                            ");
            qry.Append($"\n 	FROM 매출현황 AS t1                                     ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                    ");
            qry.Append($"\n 		SELECT                                           ");
            qry.Append($"\n 		DISTINCT 매출자코드, 매출자                            ");
            qry.Append($"\n 		FROM 업체별월별매출현황                                 ");
            qry.Append($"\n 	) AS t2                                              ");
            qry.Append($"\n 	  ON t1.매출자 = t2.매출자코드                             ");
            qry.Append($"\n 	WHERE t1.사용자 = '200009'                             ");
            qry.Append($"\n 	  AND t1.매출일자 >= '{sttdate.ToString("yyyy-MM-dd")}'                       ");
            qry.Append($"\n 	  AND t1.매출일자 <= '{enddate.ToString("yyyy-MM-dd")}'                       ");
            /*if (product.Count > 0)
            {
                string product_whr = "";
                foreach (string products in product)
                {
                    string[] product_origin = products.Split('^');
                    if (product_origin.Length > 1)
                    {
                        if(string.IsNullOrEmpty(product_whr))
                            product_whr = $"\n      (t1.품명 = '{product_origin[0]}' AND t1.원산지 = '{product_origin[1]}')";
                        else
                            product_whr += $"\n      OR (t1.품명 = '{product_origin[0]}' AND t1.원산지 = '{product_origin[1]}'";

                    }                    
                }
                product_whr = "  AND (" + product_whr + ")";
                qry.Append($"\n 	  {product_whr}                      ");
            }
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"\n 	  {commonRepository.whereSql("t2.매출자", user_name)}                       ");*/
            qry.Append($"\n ) AS t                                                   ");
            qry.Append($"\n GROUP BY   t.품명, t.원산지, t.매출자                          ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesDashboardByCompany(DateTime sttdate, DateTime enddate, string user_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                               ");
            qry.Append($"\n   t.품명                                                                              ");
            qry.Append($"\n , t.원산지                                                                             ");
            qry.Append($"\n , SUM(t.매출금액) AS 매출금액                                                              ");
            qry.Append($"\n , t.매출처                                                                             ");
            qry.Append($"\n , t.매출처코드                                                                             ");
            qry.Append($"\n , t.매출자                                                                             ");
            qry.Append($"\n FROM(                                                                                ");
            qry.Append($"\n 	SELECT                                                                           ");
            qry.Append($"\n 	  t1.품명                                                                         ");
            qry.Append($"\n 	, t1.원산지                                                                        ");
            qry.Append($"\n 	, ISNULL(t1.매출금액, 0) + ISNULL(t1.부가세, 0) AS 매출금액                              ");
            qry.Append($"\n 	, t2.매출자                                                                               ");
            qry.Append($"\n 	, t1.매출처코드                                                                               ");
            qry.Append($"\n 	, LEFT(t1.매출처, LEN(t1.매출처) - CHARINDEX('/', REVERSE(t1.매출처)) + 0) AS 매출처      ");
            qry.Append($"\n 	FROM 매출현황 AS t1                                                                 ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                    ");
            qry.Append($"\n 		SELECT                                           ");
            qry.Append($"\n 		DISTINCT 매출자코드, 매출자                            ");
            qry.Append($"\n 		FROM 업체별월별매출현황                                 ");
            qry.Append($"\n 	) AS t2                                              ");
            qry.Append($"\n 	  ON t1.매출자 = t2.매출자코드                             ");
            qry.Append($"\n 	WHERE t1.사용자 = '200009'                                                         ");
            qry.Append($"\n 	  AND t1.매출일자 >= '{sttdate.ToString("yyyy-MM-dd")}'                       ");
            qry.Append($"\n 	  AND t1.매출일자 <= '{enddate.ToString("yyyy-MM-dd")}'                       ");
            qry.Append($"\n ) AS t                                                                               ");
            qry.Append($"\n GROUP BY   t.품명, t.원산지, t.매출자, t.매출처코드, t.매출처                                                      ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetsalesLedger(string company_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                            ");
            qry.Append($"\n   거래일자                           ");
            qry.Append($"\n , div                             ");
            qry.Append($"\n , SUM(ISNULL(거래금액, 0)) AS 거래금액                           ");
            qry.Append($"\n FROM(                             ");
            qry.Append($"\n	 SELECT                           ");
            qry.Append($"\n	   매출일자 AS 거래일자                  ");
            qry.Append($"\n	 , '매출' AS div                   ");
            qry.Append($"\n	 , ISNULL(매출금액, 0) + ISNULL(부가세, 0) AS 거래금액                  ");
            qry.Append($"\n	 FROM 매출현황                       ");
            qry.Append($"\n	 WHERE 사용자 = '200009'            ");
            qry.Append($"\n	   AND 매출처코드 = '{company_code}'         ");
            qry.Append($"\n	 UNION ALL                        ");
            qry.Append($"\n	 SELECT                           ");
            qry.Append($"\n	   입금일자 AS 거래일자                  ");
            qry.Append($"\n	 , '입금' AS div                   ");
            qry.Append($"\n	 , 입금금액 AS 거래금액                  ");
            qry.Append($"\n	 FROM 거래처별결제현황                   ");
            qry.Append($"\n	 WHERE 사용자 = '200009'            ");
            //qry.Append($"\n	   AND 입금자 IS NOT NULL         ");
            qry.Append($"\n	   AND 거래처코드 = '{company_code}'         ");
            qry.Append($"\n ) AS t                            ");
            qry.Append($"\n GROUP BY 거래일자, div               ");
            qry.Append($"\n ORDER BY 거래일자                    ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesAmount(DateTime sttdate, DateTime enddate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                     ");
            qry.Append($"\n  t1.거래처코드                                                ");
            qry.Append($"\n, t1.매출금액                                                  ");
            qry.Append($"\n, t2.매출자                                                   ");
            qry.Append($"\n, t2.최종매출일자                                               ");
            qry.Append($"\nFROM(                                                      ");
            qry.Append($"\n	SELECT                                                    ");
            qry.Append($"\n	거래처코드, SUM(ISNULL(매출금액, 0) + ISNULL(부가세, 0)) AS 매출금액    ");
            qry.Append($"\n	FROM 업체별월별매출현황                                           ");
            qry.Append($"\n	WHERE 사용자 >= {sttdate.ToString("yyyyMM")} AND 사용자 <= {enddate.ToString("yyyyMM")}                    ");
            qry.Append($"\n	GROUP BY 거래처코드                                           ");
            qry.Append($"\n) AS t1                                                    ");
            qry.Append($"\nLEFT OUTER JOIN (                                          ");
            qry.Append($"\n	SELECT                                                    ");
            qry.Append($"\n	거래처코드, 매출자, 최종매출일자                                      ");
            qry.Append($"\n	FROM 업체별월별매출현황 AS a                                      ");
            qry.Append($"\n	WHERE 사용자 >= {sttdate.ToString("yyyyMM")} AND 사용자 <= {enddate.ToString("yyyyMM")}                    ");
            qry.Append($"\n	  AND 사용자 = (                                             ");
            qry.Append($"\n	  	SELECT                                                ");
            qry.Append($"\n	  	MAX(사용자)                                              ");
            qry.Append($"\n		FROM 업체별월별매출현황 AS b                                  ");
            qry.Append($"\n		WHERE 사용자 >= {sttdate.ToString("yyyyMM")} AND 사용자 <= {enddate.ToString("yyyyMM")}                ");
            qry.Append($"\n		  AND a.거래처코드 = b.거래처코드                              ");
            qry.Append($"\n	  )                                                       ");
            qry.Append($"\n) AS t2                                                    ");
            qry.Append($"\n ON t1.거래처코드 = t2.거래처코드                                   ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesQty(int sales_terms, string sub_product)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                          ");
            qry.Append($"\n   품명                                            ");
            qry.Append($"\n , 원산지                                           ");
            qry.Append($"\n , 규격                                            ");
            qry.Append($"\n , REPLACE(단위, 'kg', '') AS 단위                   ");
            qry.Append($"\n , SUM(매출수) AS 매출수                              ");
            qry.Append($"\n FROM 매출현황                                       ");
            qry.Append($"\n WHERE 사용자 = '200009'                            ");
            qry.Append($"\n   AND 매출자 <> '199990'                           ");
            qry.Append($"\n   AND 매출처 NOT LIKE '%아토무역%'                    ");
            qry.Append($"\n   AND 매출처 NOT LIKE '%아토코리아%'                   ");
            qry.Append($"\n   AND 매출처 NOT LIKE '%에이티오%'                    ");
            qry.Append($"\n   AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                 ");
            if(sales_terms == 45)
                qry.Append($"\n   AND 매출일자 >= '{DateTime.Now.AddDays(-sales_terms).ToString("yyyy-MM-dd")}'                 ");
            else
                qry.Append($"\n   AND 매출일자 >= '{DateTime.Now.AddMonths(-sales_terms).ToString("yyyy-MM-dd")}'                 ");
            qry.Append($"\n   AND 매출일자 <= '{DateTime.Now.ToString("yyyy-MM-dd")}'                 ");

            if (sub_product != null && !string.IsNullOrEmpty(sub_product))
            {
                string[] products = sub_product.Trim().Split('\n');
                if (products.Length > 0)
                {
                    qry.Append($"\n   AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        if (i == 0)
                            qry.Append($"\n (");
                        else
                            qry.Append($"\n OR (");
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 1)
                        {
                            qry.Append($"품명 = '{sub[0]}' ");
                            qry.Append($"AND 원산지 = '{sub[1]}' ");
                            qry.Append($"AND 규격 = '{sub[2]}' ");
                            qry.Append($"AND REPLACE(단위, 'KG', '') = '{sub[3]}' ");
                            qry.Append($") ");
                        }
                    }
                    qry.Append($"\n)");
                }
            }
            qry.Append($"\n GROUP BY 품명, 원산지, 규격, REPLACE(단위, 'kg', '')    ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesAmountDashboard(string user_name, DateTime sdate, int saleTears, int sttYear, int sttMonth, int endYear, int endMonth)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                             ");
            qry.Append($"\n   SUM(CASE WHEN 매출일자 >= '{sdate.AddMonths(-1).ToString("yyyy-MM-dd")}' THEN t.매출금액 ELSE 0 END) 한달매출                                 ");
            qry.Append($"\n , SUM(CASE WHEN 매출일자 >= '{sdate.AddMonths(-saleTears).ToString("yyyy-MM-dd")}' THEN t.매출금액 ELSE 0 END) 평균매출                                 ");

            for (int i = sttYear; i <= endYear; i++)
            {
                DateTime sttdate = new DateTime(i, sttMonth, 1);
                DateTime enddate = new DateTime(i, endMonth, 1).AddMonths(1).AddDays(-1);
                qry.Append($"\n , SUM(CASE WHEN 매출일자 >= '{sttdate.ToString("yyyy-MM-dd")}' AND 매출일자 <= '{enddate.ToString("yyyy-MM-dd")}' THEN t.매출금액 ELSE 0 END) 매출{i.ToString()}     ");
            }
            qry.Append($"\n FROM(                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                         ");
            qry.Append($"\n 	  매출일자                                                                                         ");
            qry.Append($"\n 	, t1.매출처                                                                                       ");
            qry.Append($"\n 	, ISNULL(t1.매출금액, 0) + ISNULL(t1.부가세, 0) AS 매출금액                                              ");
            qry.Append($"\n 	, t2.매출자                                                                                       ");
            qry.Append($"\n 	FROM 매출현황 AS t1                                                                                ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                                                              ");
            qry.Append($"\n 	 	SELECT                                                                                     ");
            qry.Append($"\n 	 	DISTINCT 매출자코드, 매출자                                                                       ");
            qry.Append($"\n 	 	FROM 업체별월별매출현황                                                                            ");
            qry.Append($"\n 	 	WHERE 사용자 >= 202301                                                                        ");
            qry.Append($"\n 	 	  AND 매출자코드 IS NOT NULL                                                                   ");
            qry.Append($"\n 	) AS t2                                                                                        ");
            qry.Append($"\n 	   ON t1.매출자 = t2.매출자코드                                                                       ");
            qry.Append($"\n 	WHERE t1.사용자 = '200009'                                                                        ");
            qry.Append($"\n 	  AND t2.매출자 = '{user_name}'                                                                          ");
            qry.Append($"\n ) AS t                                                                                             ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetBusinessCompanyForSalesDashborad(int sttYear, int endYear, List<string> userList)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                           ");
            qry.Append($"\n    t1.매출일자                                                                       ");
            qry.Append($"\n  , t1.매출처                                                                        ");
            qry.Append($"\n  , t1.매출자                                                                        ");
            qry.Append($"\n  , SUBSTRING(CONVERT(CHAR(10), t2.마지막결제, 112), 0, 7) AS 마지막결제                            ");
            qry.Append($"\n  FROM (                                                                           ");
            qry.Append($"\n	 SELECT                                                                           ");
            qry.Append($"\n	   SUBSTRING(CONVERT(CHAR(10), t1.매출일자, 112), 0, 7) AS 매출일자                      ");
            qry.Append($"\n	 , t1.매출처                                                                        ");
            qry.Append($"\n	 , t2.매출자                                                                        ");
            qry.Append($"\n	 FROM 매출현황 AS t1                                                                 ");
            qry.Append($"\n	 LEFT OUTER JOIN (                                                                ");
            qry.Append($"\n	  	SELECT                                                                        ");
            qry.Append($"\n	  	DISTINCT 매출자코드, 매출자                                                          ");
            qry.Append($"\n	  	FROM 업체별월별매출현황                                                               ");
            qry.Append($"\n	  	WHERE 사용자 >= 202301                                                          ");
            qry.Append($"\n	  	  AND 매출자코드 IS NOT NULL                                                      ");
            qry.Append($"\n	 ) AS t2                                                                          ");
            qry.Append($"\n	    ON t1.매출자 = t2.매출자코드                                                         ");
            qry.Append($"\n	 WHERE t1.사용자 = '200009'                                                         ");
            qry.Append($"\n    AND t1.매출일자 >= '{new DateTime(sttYear, 1, 1).ToString("yyyy-MM-dd")}'     ");
            qry.Append($"\n    AND t1.매출일자 <= '{new DateTime(endYear, 12, 31).ToString("yyyy-MM-dd")}'     ");
            if (userList.Count > 0)
            {
                if (userList.Count == 1)
                    qry.Append($"\n   AND t2.매출자 = '{userList[0]}'                                                                 ");
                else
                {
                    qry.Append($"\n   AND (                                                                 ");
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (i == 0)
                            qry.Append($"\n   t2.매출자 = '{userList[i]}'                                                                 ");
                        else
                            qry.Append($"\n   OR t2.매출자 = '{userList[i]}'                                                                 ");
                    }
                    qry.Append($"\n   )                                                                 ");
                }
            }
            qry.Append($"\n	 GROUP BY SUBSTRING(CONVERT(CHAR(10), t1.매출일자, 112), 0, 7), t1.매출처 , t2.매출자      ");
            qry.Append($"\n ) AS t1                                                                           ");
            qry.Append($"\n LEFT OUTER JOIN                                                                   ");
            qry.Append($"\n (                                                                                 ");
            qry.Append($"\n	 SELECT                                                                           ");
            qry.Append($"\n	   MIN(t1.매출일자) AS 첫결제                                                           ");
            qry.Append($"\n	 , MAX(t1.매출일자) AS 마지막결제                                                         ");
            qry.Append($"\n	 , t1.매출처                                                                        ");
            qry.Append($"\n	 , t2.매출자                                                                        ");
            qry.Append($"\n	 FROM 매출현황 AS t1                                                                 ");
            qry.Append($"\n	 LEFT OUTER JOIN (                                                                ");
            qry.Append($"\n	  	SELECT                                                                        ");
            qry.Append($"\n	  	DISTINCT 매출자코드, 매출자                                                          ");
            qry.Append($"\n	  	FROM 업체별월별매출현황                                                               ");
            qry.Append($"\n	  	WHERE 사용자 >= 202301                                                          ");
            qry.Append($"\n	  	  AND 매출자코드 IS NOT NULL                                                      ");
            qry.Append($"\n	 ) AS t2                                                                          ");
            qry.Append($"\n	    ON t1.매출자 = t2.매출자코드                                                         ");
            qry.Append($"\n	 WHERE t1.사용자 = '200009'                                                         ");
            qry.Append($"\n    AND t1.매출일자 >= '{new DateTime(sttYear, 1, 1).ToString("yyyy-MM-dd")}'     ");
            qry.Append($"\n    AND t1.매출일자 <= '{new DateTime(endYear, 12, 31).ToString("yyyy-MM-dd")}'     ");
            if (userList.Count > 0)
            {
                if (userList.Count == 1)
                    qry.Append($"\n   AND t2.매출자 = '{userList[0]}'                                                                 ");
                else
                {
                    qry.Append($"\n   AND (                                                                 ");
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (i == 0)
                            qry.Append($"\n   t2.매출자 = '{userList[i]}'                                                                 ");
                        else
                            qry.Append($"\n   OR t2.매출자 = '{userList[i]}'                                                                 ");
                    }
                    qry.Append($"\n   )                                                                 ");
                }
            }
            qry.Append($"\n	 GROUP BY t1.매출처 , t2.매출자                                                        ");
            qry.Append($"\n ) AS t2                                                                           ");
            qry.Append($"\n   ON t1.매출자 = t2.매출자                                                             ");
            qry.Append($"\n   AND t1.매출처 = t2.매출처                                                            ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetSalesForSalesDashborad(string username, string product, string origin, string sizes)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                    ");
            qry.Append($"\n   SUM(ISNULL(t1.매출금액, 0) + ISNULL(t1.부가세, 0)) AS 매출금액                          ");
            qry.Append($"\n , LEFT(CONVERT(VARCHAR, t1.매출일자, 112), 6) AS 매출일자                                 ");
            qry.Append($"\n , t2.매출자                                                                               ");
            qry.Append($"\n FROM 매출현황 AS t1                                                                       ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                         ");
            qry.Append($"\n 	SELECT                                                                                ");
            qry.Append($"\n 	DISTINCT 매출자코드, 매출자                                                           ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                               ");
            qry.Append($"\n 	WHERE 사용자 >= 202301                                                                ");
            qry.Append($"\n 	  AND 매출자코드 IS NOT NULL                                                          ");
            qry.Append($"\n ) AS t2                                                                                   ");
            qry.Append($"\n   ON t1.매출자 = t2.매출자코드                                                            ");
            qry.Append($"\n WHERE t1.사용자 = '200009'                                                                ");
            qry.Append($"\n   AND t2.매출자 = '{username}'                                                            ");
            if(!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("t1.품명", product)}                                                            ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("t1.원산지", origin)}                                                            ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("t1.규격", sizes)}                                                            ");
            qry.Append($"\n GROUP BY LEFT(CONVERT(VARCHAR, t1.매출일자, 112), 6), t2.매출자                           ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesForSalesDashborad(List<string> userList)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                  ");
            qry.Append($"\n   SUM(ISNULL(t1.매출금액, 0) + ISNULL(t1.부가세, 0)) AS 매출금액                                                      ");
            qry.Append($"\n , LEFT(CONVERT(VARCHAR, t1.매출일자, 112), 6) AS 매출일자                                      ");
            qry.Append($"\n , t2.매출자                                                                                ");
            qry.Append($"\n FROM 매출현황 AS t1                                                                        ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                       ");
            qry.Append($"\n 	SELECT                                                                              ");
            qry.Append($"\n 	DISTINCT 매출자코드, 매출자                                                                ");
            qry.Append($"\n 	FROM 업체별월별매출현황                                                                     ");
            qry.Append($"\n 	WHERE 사용자 >= 202301                                                                 ");
            qry.Append($"\n 	  AND 매출자코드 IS NOT NULL                                                            ");
            qry.Append($"\n ) AS t2                                                                                 ");
            qry.Append($"\n   ON t1.매출자 = t2.매출자코드                                                                 ");
            qry.Append($"\n WHERE t1.사용자 = '200009'                                                                 ");
            if (userList.Count > 0)
            {
                if (userList.Count == 1)
                    qry.Append($"\n   AND t2.매출자 = '{userList[0]}'                                                                 ");
                else
                {
                    qry.Append($"\n   AND (                                                                 ");
                    for (int i = 0; i < userList.Count; i++)
                    {
                        if (i == 0)
                            qry.Append($"\n   t2.매출자 = '{userList[i]}'                                                                 ");
                        else
                            qry.Append($"\n   OR t2.매출자 = '{userList[i]}'                                                                 ");
                    }
                    qry.Append($"\n   )                                                                 ");
                }
            }
            qry.Append($"\n GROUP BY LEFT(CONVERT(VARCHAR, t1.매출일자, 112), 6), t2.매출자                               ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetHandlingProductDashboard(string company)
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n  SELECT                                                                                                           ");
            qry.Append($"\n    s.*                                                                                                            ");
            qry.Append($"\n  , c.거래처명 AS 매출처                                                                                                 ");
            qry.Append($"\n  FROM(                                                                                                            ");

            qry.Append($"\n   	SELECT                                    ");
            qry.Append($"\n  	  거래처명, 거래처코드                            ");
            qry.Append($"\n  	FROM 업체별월별매출현황 AS a                      ");
            qry.Append($"\n  	WHERE 1=1                                 ");
            qry.Append($"\n  	  AND 거래처코드 = '{company}'                  ");
            qry.Append($"\n  	  AND 월 IS NOT NULL                       ");
            qry.Append($"\n  	  AND 사용자 = (                             ");
            qry.Append($"\n  	  	SELECT                                ");
            qry.Append($"\n  	  	  MAX(ISNULL(사용자, 0))                 ");
            qry.Append($"\n  	  	FROM 업체별월별매출현황 AS b                  ");
            qry.Append($"\n  	  	WHERE b.거래처코드 = '{company}'            ");
            qry.Append($"\n  	  	  AND b.월 IS NOT NULL                 ");
            qry.Append($"\n  	  )                                   ");
            qry.Append($"\n  ) AS c                                                                                                           ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                ");
            qry.Append($"\n 	 SELECT                                                                                                       ");
            qry.Append($"\n 	   t1.매출일자                                                                                                    ");
            qry.Append($"\n 	 , t1.품명                                                                                                      ");
            qry.Append($"\n 	 , t1.원산지                                                                                                     ");
            qry.Append($"\n 	 , t1.규격                                                                                                      ");
            qry.Append($"\n 	 , t1.단위                                                                                                      ");
            qry.Append($"\n 	 , t1.매출처코드                                                                                                  ");
            qry.Append($"\n 	 , CASE WHEN ISNULL(t2.단위수량,1) > 1 THEN t1.매출단가/ ISNULL(t2.단위수량,1) ELSE t1.매출단가 END AS 전매출단가                                                                                          ");
            qry.Append($"\n 	 , t2.단위수량 AS 단위수량                                                                                          ");
            qry.Append($"\n 	 , t2.매출단가 AS 현매출단가                                                                                          ");
            qry.Append($"\n 	 , t3.최근매출수                                                                                                     ");
            qry.Append($"\n 	 , t3.매출일자 AS 최근매출일자                                                                                                  ");
            qry.Append($"\n 	 , ISNULL(t4.평균매출, 0) AS 평균매출                                                                                                  ");
            qry.Append($"\n 	 FROM (                                                                                                       ");
            qry.Append($"\n 		 SELECT                                                                                                   ");
            qry.Append($"\n 		 t.*                                                                                                      ");
            qry.Append($"\n 		 FROM (                                                                                                   ");
            qry.Append($"\n 			 SELECT                                                                                               ");
            qry.Append($"\n 			   ROW_NUMBER() OVER (PARTITION BY 품명, 원산지, 규격, 단위 ORDER BY 매출일자 DESC) AS rownum                    ");
            qry.Append($"\n 			 , t1.*                                                                                               ");
            qry.Append($"\n 			 FROM 매출현황 AS t1                                                                                      ");
            qry.Append($"\n 			 WHERE 사용자 = 200009                                                                                   ");
            qry.Append($"\n 			   AND 매출처코드 = '{company}'                                                                             ");
            qry.Append($"\n 		 ) AS t                                                                                                   ");
            qry.Append($"\n 		 WHERE rownum = 1                                                                                         ");
            qry.Append($"\n 	 ) AS t1                                                                                                      ");
            qry.Append($"\n 	 LEFT OUTER JOIN (                                                                                            ");
            qry.Append($"\n 		 SELECT                                                                                                   ");
            qry.Append($"\n 		 t.*                                                                                                      ");
            qry.Append($"\n 		 FROM (                                                                                                   ");
            qry.Append($"\n 			 SELECT                                                                                               ");
            qry.Append($"\n 			   ROW_NUMBER() OVER (PARTITION BY 품명, 원산지, 규격, SEAOVER단위 ORDER BY 매입일자 DESC) AS rownum             ");
            qry.Append($"\n 			 , t1.*                                                                                               ");
            qry.Append($"\n 			 FROM 업체별시세관리 AS t1                                                                                  ");
            qry.Append($"\n 			 WHERE 사용자 = 200009                                                                                   ");
            qry.Append($"\n 		 ) AS t                                                                                                   ");
            qry.Append($"\n 		 WHERE rownum = 1                                                                                         ");
            qry.Append($"\n 	 ) AS t2                                                                                                      ");
            qry.Append($"\n 	   ON t1.품명 = t2.품명                                                                                           ");
            qry.Append($"\n 	   AND t1.원산지 = t2.원산지                                                                                       ");
            qry.Append($"\n 	   AND t1.규격 = t2.규격                                                                                          ");
            qry.Append($"\n 	   AND REPLACE(t1.단위,'kg','') = t2.SEAOVER단위                                                                  ");
            qry.Append($"\n 	 LEFT OUTER JOIN (                                                                                            ");
            qry.Append($"\n		 SELECT                                                                                                  ");
            qry.Append($"\n		   매출일자                                                                                                  ");
            qry.Append($"\n		 , 품명                                                                                                    ");
            qry.Append($"\n		 , 원산지                                                                                                   ");
            qry.Append($"\n		 , 규격                                                                                                    ");
            qry.Append($"\n		 , 단위                                                                                                    ");
            qry.Append($"\n		 , 매출수 AS 최근매출수                                                                                         ");
            qry.Append($"\n		 FROM(                                                                                                   ");
            qry.Append($"\n	 	  	 SELECT                                                                                              ");
            qry.Append($"\n	 	  	   매출일자                                                                                              ");
            qry.Append($"\n	 	 	 , 품명                                                                                                ");
            qry.Append($"\n	 	 	 , 원산지                                                                                               ");
            qry.Append($"\n	 	 	 , 규격                                                                                                ");
            qry.Append($"\n	 	 	 , 단위                                                                                                ");
            qry.Append($"\n	 	 	 , 매출수                                                                                               ");
            qry.Append($"\n	 	 	 , ROW_NUMBER() OVER (PARTITION BY 품명, 원산지, 규격, 단위 ORDER BY 매출일자 DESC) AS rownum                   ");
            qry.Append($"\n	 	 	 FROM 매출현황 AS t1                                                                                     ");
            qry.Append($"\n	 	 	 WHERE 사용자 = 200009                                                                                  ");
            qry.Append($"\n	 		   AND 매출처코드 = '{company}'                                                                            ");
            qry.Append($"\n	 		   AND 매출수 > 0                                                                                       ");
            qry.Append($"\n         ) AS t                                                                                               ");
            qry.Append($"\n         WHERE rownum = 1                                                                                     ");
            qry.Append($"\n 	 ) AS t3                                                                                                 ");
            qry.Append($"\n 	    ON t1.품명 = t3.품명                                                                                     ");
            qry.Append($"\n 	   AND t1.원산지 = t3.원산지                                                                                  ");
            qry.Append($"\n 	   AND t1.규격 = t3.규격                                                                                     ");
            qry.Append($"\n 	   AND t1.단위 = t3.단위                                                                                     ");
            qry.Append($"\n 	 LEFT OUTER JOIN (                                                                                       ");
            qry.Append($"\n		 SELECT                                                                                                  ");
            qry.Append($"\n		   품명                                                                                                    ");
            qry.Append($"\n		 , 원산지                                                                                                   ");
            qry.Append($"\n		 , 규격                                                                                                    ");
            qry.Append($"\n		 , 단위                                                                                                    ");
            qry.Append($"\n		 , (한달 + 두달 + 세달) / 3 AS 평균매출                                                                           ");
            qry.Append($"\n		 FROM(                                                                                                   ");
            qry.Append($"\n	 	  	 SELECT                                                                                              ");
            qry.Append($"\n	 	 	   품명                                                                                                ");
            qry.Append($"\n	 	 	 , 원산지                                                                                               ");
            qry.Append($"\n	 	 	 , 규격                                                                                                ");
            qry.Append($"\n	 	 	 , 단위                                                                                                ");
            qry.Append($"\n	 	 	 , SUM(CASE WHEN 매출일자 < '{DateTime.Now.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")}' THEN 매출수 ELSE 0 END) AS 한달           ");
            qry.Append($"\n		 	 , SUM(CASE WHEN 매출일자 < '{DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd")}' AND 매출일자 >= '{DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd")}' THEN 매출수 ELSE 0 END) AS 두달           ");
            qry.Append($"\n		 	 , SUM(CASE WHEN 매출일자 < '{DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd")}' AND 매출일자 >= '{DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd")}' THEN 매출수 ELSE 0 END) AS 세달           ");
            qry.Append($"\n	 	 	 FROM 매출현황 AS t1                                                                                     ");
            qry.Append($"\n	 	 	 WHERE 사용자 = 200009                                                                                  ");
            qry.Append($"\n	 		   AND 매출처코드 = '{company}'                                                                            ");
            qry.Append($"\n	 		   AND 매출일자 >= '{DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd")}'                                                                 ");
            qry.Append($"\n	 		 GROUP BY  품명, 원산지, 규격, 단위                                                                          ");
            qry.Append($"\n         ) AS t                                                                                               ");
            qry.Append($"\n 	 ) AS t4                                                                                                 ");
            qry.Append($"\n 	    ON t1.품명 = t4.품명                                                                                     ");
            qry.Append($"\n 	   AND t1.원산지 = t4.원산지                                                                                  ");
            qry.Append($"\n 	   AND t1.규격 = t4.규격                                                                                     ");
            qry.Append($"\n 	   AND t1.단위 = t4.단위 	                                                                                 ");
            qry.Append($"\n  ) AS s                                                                                                           ");
            qry.Append($"\n    ON s.매출처코드 = c.거래처코드                                                                                          ");
            qry.Append($"\n    ORDER BY s.매출일자 DESC, s.매출처코드, s.품명, s.원산지, s.규격, s.단위                                                       ");

            
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetNoneHandlingProductDashboard(string product, string origin, string sizes, string unit, List<DataGridViewRow> except_product)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                             ");
            qry.Append($"\n   t.품명                                                                                             ");
            qry.Append($"\n , t.원산지                                                                                            ");
            qry.Append($"\n , t.규격                                                                                             ");
            qry.Append($"\n , CASE WHEN CHARINDEX('/', t.규격) > 0 THEN                 ");
            qry.Append($"\n 	CASE WHEN ISNUMERIC(SUBSTRING(t.규격, 1, CHARINDEX('/', t.규격) - 1)) = 1 THEN SUBSTRING(t.규격, 1, CHARINDEX('/', t.규격) - 1) ELSE 0 END         ");
            qry.Append($"\n	 	WHEN CHARINDEX('미', t.규격) > 0 THEN                    ");
            qry.Append($"\n 	CASE WHEN ISNUMERIC(SUBSTRING(t.규격, 1, CHARINDEX('미', t.규격) - 1)) = 1 THEN SUBSTRING(t.규격, 1, CHARINDEX('미', t.규격) - 1) ELSE 0 END         ");
            qry.Append($"\n   ELSE 0 END AS 규격1                                       ");
            qry.Append($"\n , t.seaover단위                                                                                      ");
            qry.Append($"\n , t.단위수량                                                                                           ");
            qry.Append($"\n , t.매출단가                                                                                           ");
            qry.Append($"\n , t.매입일자                                                                                           ");
            qry.Append($"\n , CASE WHEN 구분 LIKE '%10%' OR 구분 LIKE '%20%' OR 구분 LIKE '%30%' OR 구분 LIKE '%40%' OR 구분 LIKE '%50%' THEN 1 ELSE 2 END AS 구분                                         ");
            qry.Append($"\n FROM (                                                                                             ");
            qry.Append($"\n	 SELECT                                                                                            ");
            qry.Append($"\n	   ROW_NUMBER() OVER (PARTITION BY 품명, 원산지, 규격, SEAOVER단위 ORDER BY 매입일자 DESC) AS rownum          ");
            qry.Append($"\n	 , t1.*                                                                                            ");
            qry.Append($"\n	 FROM 업체별시세관리 AS t1                                                                               ");
            qry.Append($"\n	 WHERE 사용자 = '{userId}'                                                                                ");

            if(!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n   {whereSql("품명", product.Trim())}                                                                      ");
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n   {whereSql("원산지", origin.Trim())}                                                                      ");
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n   {whereSql("규격", sizes.Trim())}                                                                      ");
            if (!string.IsNullOrEmpty(unit.Trim()))
                qry.Append($"\n   {whereSql("seaover단위", unit.Trim())}                                                                      ");

            if (except_product.Count > 0)
            {
                for (int i = 0; i < except_product.Count; i++)
                {
                    qry.Append($"\n	   AND NOT (                                                                                ");
                    qry.Append($"\n	        품명 = '{except_product[i].Cells["product_name"].Value.ToString()}'                                                                  ");
                    qry.Append($"\n	        AND 원산지 = '{except_product[i].Cells["origin_name"].Value.ToString()}'                                                                  ");
                    qry.Append($"\n	        AND 규격 = '{except_product[i].Cells["sizes_name"].Value.ToString()}'                                                                  ");
                    qry.Append($"\n	        AND 단위 = '{except_product[i].Cells["unit_name"].Value.ToString().Replace("kg", "").Replace("KG", "").Replace("Kg", "")}'                                                                  ");
                    qry.Append($"\n	     )                                                                                ");

                }
            }

            qry.Append($"\n ) AS t                                                                                             ");
            qry.Append($"\n WHERE rownum = 1                                                                                   ");
            qry.Append($"\n ORDER BY t.구분, t.품명, t.원산지, t.규격1, t.규격, t.단위                                                                                   ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }


        public DataTable GetCompanyList()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                   ");
            qry.Append($"\n   t1.매출처코드                              ");
            qry.Append($"\n , t1.매출처                                 ");
            qry.Append($"\n , t2.매출자                                 ");
            qry.Append($"\n FROM(                                    ");
            qry.Append($"\n  SELECT                                  ");
            qry.Append($"\n  	s.매출처코드                             ");
            qry.Append($"\n  , c.거래처명 + '_' + c.대표자명 AS 매출처         ");
            qry.Append($"\n  , s.매출자                                 ");
            qry.Append($"\n  FROM (                                  ");
            qry.Append($"\n	  SELECT                                 ");
            qry.Append($"\n	    DISTINCT 매출처코드, 매출자                 ");
            qry.Append($"\n	  FROM 매출현황 AS a                         ");
            qry.Append($"\n	  WHERE 사용자 = 200009                     ");
            qry.Append($"\n	   AND 매출일자 = (                          ");
            qry.Append($"\n	   	SELECT MAX(매출일자)                     ");
            qry.Append($"\n	   	FROM 매출현황 AS b                       ");
            qry.Append($"\n	   	WHERE 사용자 = 200009                   ");
            qry.Append($"\n	   	  AND a.매출처코드 = b.매출처코드             ");
            qry.Append($"\n	   )                                     ");
            qry.Append($"\n  ) AS s                                  ");
            qry.Append($"\n  LEFT OUTER JOIN (                       ");
            qry.Append($"\n	  SELECT                                 ");
            qry.Append($"\n	    DISTINCT 거래처코드, 거래처명, 대표자명         ");
            qry.Append($"\n	  FROM 업체별월별매출현황                        ");
            qry.Append($"\n  ) AS c                                  ");
            qry.Append($"\n    ON s.매출처코드 = c.거래처코드                 ");
            qry.Append($"\n ) AS t1                                  ");
            qry.Append($"\nLEFT OUTER JOIN                           ");
            qry.Append($"\n(                                         ");
            qry.Append($"\nSELECT                                    ");
            qry.Append($"\nDISTINCT 매출자코드, 매출자                      ");
            qry.Append($"\nFROM 업체별월별매출현황                           ");
            qry.Append($"\n) AS t2                                   ");
            qry.Append($"\n  ON t1.매출자 = t2.매출자코드                   ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetDuplicateSeaoverList()
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n     {DuplicateSql("*").Trim()}          ");
            

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSales(string sttdate, string enddate, string sale_company, string product, string origin, string sizes, string unit, string purchase_company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n select                   ");
            qry.Append($"\n   s.매출관리번호              ");
            qry.Append($"\n , s.이전매출관리번호            ");
            qry.Append($"\n , s.매출일자                 ");
            qry.Append($"\n , s.구분                   ");
            qry.Append($"\n , s.매출처                  ");
            qry.Append($"\n , s.품명                   ");
            qry.Append($"\n , s.품명코드                 ");
            qry.Append($"\n , s.규격                   ");
            qry.Append($"\n , s.규격코드                 ");
            qry.Append($"\n , s.단위                   ");
            qry.Append($"\n , s.원산지                  ");
            qry.Append($"\n , s.본선명                  ");
            qry.Append($"\n , s.입고일자                 ");
            qry.Append($"\n , s.보관처                  ");
            qry.Append($"\n , s.적요                   ");
            qry.Append($"\n , s.매출수                  ");
            qry.Append($"\n , s.매출단가                 ");
            qry.Append($"\n , s.매출금액                 ");
            qry.Append($"\n , s.부가세                  ");
            qry.Append($"\n , s.매입관리번호              ");
            qry.Append($"\n , s.매입단가                 ");
            qry.Append($"\n , s.매입금액                 ");
            qry.Append($"\n , s.매입처                  ");
            qry.Append($"\n , s.매입일자                 ");
            qry.Append($"\n , s.매입구분                 ");
            qry.Append($"\n , m.매출자                  ");
            qry.Append($"\n FROM 매출현황 AS s           ");
            qry.Append($"\n LEFT OUTER JOIN (        ");
            qry.Append($"\n 	SELECT               ");
            qry.Append($"\n 	DISTINCT 매출자코드, 매출자 ");
            qry.Append($"\n 	FROM 업체별월별매출현황      ");
            qry.Append($"\n ) AS m                   ");
            qry.Append($"\n   ON s.매출자 = m.매출자코드    ");
            qry.Append($"\n where s.사용자 = 200009     ");
            if(!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND s.매출일자 >= '{sttdate}'     ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND s.매출일자 <= '{enddate}'     ");

            if (!string.IsNullOrEmpty(sale_company))
                qry.Append($"\n   AND s.매출처 LIKE '%{sale_company}%'     ");

            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND s.품명 LIKE '%{product}%'     ");

            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND s.원산지 LIKE '%{origin}%'     ");

            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND s.규격 LIKE '%{sizes}%'     ");

            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND s.단위 LIKE '%{unit}%'     ");

            if (!string.IsNullOrEmpty(purchase_company))
                qry.Append($"\n   AND s.매입처 LIKE '%{purchase_company}%'     ");

            qry.Append($"\n ORDER BY s.매출일자 DESC, s.매출처      ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCurrentSales(string sttdate, string enddate, string sale_company, string product, string origin, string sizes, string unit, string purchase_company)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n select                                   ");
            qry.Append($"\n   t1.*                                   ");
            qry.Append($"\n FROM (                                   ");
            qry.Append($"\n	 select                                  ");
            qry.Append($"\n	   s.매출관리번호                             ");
            qry.Append($"\n	 , s.이전매출관리번호                           ");
            qry.Append($"\n	 , s.매출일자                                ");
            qry.Append($"\n	 , s.구분                                  ");
            qry.Append($"\n	 , s.매출처                                 ");
            qry.Append($"\n	 , s.품명                                  ");
            qry.Append($"\n	 , s.품명코드                                ");
            qry.Append($"\n	 , s.규격                                  ");
            qry.Append($"\n	 , s.규격코드                                ");
            qry.Append($"\n	 , s.단위                                  ");
            qry.Append($"\n	 , s.원산지                                 ");
            qry.Append($"\n	 , s.본선명                                 ");
            qry.Append($"\n	 , s.입고일자                                ");
            qry.Append($"\n	 , s.보관처                                 ");
            qry.Append($"\n	 , s.적요                                  ");
            qry.Append($"\n	 , s.매출수                                 ");
            qry.Append($"\n	 , s.매출단가                                ");
            qry.Append($"\n	 , s.매출금액                                ");
            qry.Append($"\n	 , s.부가세                                 ");
            qry.Append($"\n	 , s.매입관리번호                             ");
            qry.Append($"\n	 , s.매입단가                                ");
            qry.Append($"\n	 , s.매입금액                                ");
            qry.Append($"\n	 , s.매입처                                 ");
            qry.Append($"\n	 , s.매입일자                                ");
            qry.Append($"\n	 , s.매입구분                                ");
            qry.Append($"\n	 , m.매출자                                 ");
            qry.Append($"\n	 FROM 매출현황 AS s                          ");
            qry.Append($"\n	 LEFT OUTER JOIN (                       ");
            qry.Append($"\n	 	SELECT                               ");
            qry.Append($"\n	 	DISTINCT 매출자코드, 매출자                 ");
            qry.Append($"\n	 	FROM 업체별월별매출현황                      ");
            qry.Append($"\n	 ) AS m                                  ");
            qry.Append($"\n	   ON s.매출자 = m.매출자코드                   ");
            qry.Append($"\n	 where s.사용자 = 200009                    ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND s.매출일자 >= '{sttdate}'     ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND s.매출일자 <= '{enddate}'     ");

            if (!string.IsNullOrEmpty(sale_company))
                qry.Append($"\n   AND s.매출처 LIKE '%{sale_company}%'     ");

            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND s.품명 LIKE '%{product}%'     ");

            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND s.원산지 LIKE '%{origin}%'     ");

            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND s.규격 LIKE '%{sizes}%'     ");

            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND s.단위 LIKE '%{unit}%'     ");

            if (!string.IsNullOrEmpty(purchase_company))
                qry.Append($"\n   AND s.매입처 LIKE '%{purchase_company}%'     ");
            qry.Append($"\n ) AS t1                                  ");
            qry.Append($"\n WHERE t1.매출일자 = (                        ");
            qry.Append($"\n 	SELECT MAX(매출일자)                     ");
            qry.Append($"\n 	FROM 매출현황 AS t2                      ");
            qry.Append($"\n 	WHERE t2.사용자 = 200009                ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND t2.매출일자 >= '{sttdate}'     ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND t2.매출일자 <= '{enddate}'     ");

            if (!string.IsNullOrEmpty(sale_company))
                qry.Append($"\n   AND t2.매출처 LIKE '%{sale_company}%'     ");

            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND t2.품명 LIKE '%{product}%'     ");

            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND t2.원산지 LIKE '%{origin}%'     ");

            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND t2.규격 LIKE '%{sizes}%'     ");

            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND t2.단위 LIKE '%{unit}%'     ");

            if (!string.IsNullOrEmpty(purchase_company))
                qry.Append($"\n   AND t2.매입처 LIKE '%{purchase_company}%'     ");
            qry.Append($"\n 	  AND t1.매출처 = t2.매출처               ");
            qry.Append($"\n 	  AND t1.품명 = t2.품명                  ");
            qry.Append($"\n 	  AND t1.원산지 = t2.원산지               ");
            qry.Append($"\n 	  AND t1.규격 = t2.규격                  ");
            qry.Append($"\n 	  AND t1.단위 = t2.단위	                 ");
            qry.Append($"\n )                                        ");

            qry.Append($"\n ORDER BY t1.매출일자, t1.품명, t1.원산지, t1.규격, t1.단위                                        ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetIntegretionSaleQty(string product, string origin, string sizes, string user_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n 	SELECT                                                               ");
            qry.Append($"\n 	  SUM(매출수) AS 매출수                                              ");
            qry.Append($"\n 	FROM 품명별매출현황                                                  ");
            qry.Append($"\n 	WHERE 품명 = '{product}'                                             ");
            qry.Append($"\n 	  AND 원산지 = '{origin}'                                            ");
            qry.Append($"\n 	  AND 규격 = '{sizes}'                                               ");
            qry.Append($"\n 	  AND 사용자 = {user_code}                                           ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetHandlingItemDetail(string company_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n 	SELECT                                                               ");
            qry.Append($"\n 	  품명                                                               ");
            qry.Append($"\n 	, 원산지                                                             ");
            qry.Append($"\n 	, 규격                                                               ");
            qry.Append($"\n 	, 단위                                                               ");
            qry.Append($"\n 	, 매출수                                                             ");
            qry.Append($"\n 	, 매출일자                                                           ");
            qry.Append($"\n 	FROM 매출현황                                                        ");
            qry.Append($"\n 	WHERE 매출처코드 = {company_code}                                    ");
            qry.Append($"\n 	ORDER BY 매출일자                                                    ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetHandlingItem(string company_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                             ");
            qry.Append($"\n   t1.*                                                                                                                                             ");
            qry.Append($"\n , t2.매출일자                                                                                                                                          ");
            qry.Append($"\n FROM(                                                                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                                                                         ");
            qry.Append($"\n 	  품명                                                                                                                                           ");
            qry.Append($"\n 	, 원산지                                                                                                                                          ");
            qry.Append($"\n 	, 규격                                                                                                                                           ");
            qry.Append($"\n 	, 단위                                                                                                                                           ");
            qry.Append($"\n 	, SUM(매출수) AS 매출수                                                                                                                             ");
            qry.Append($"\n 	, AVG(매출수) AS 평균매출수                                                                                                                             ");
            qry.Append($"\n 	FROM 매출현황                                                                                                                                      ");
            qry.Append($"\n 	WHERE 매출처코드 = {company_code}                                                                                                                         ");
            qry.Append($"\n 	GROUP BY 품명, 원산지, 규격, 단위                                                                                                                      ");
            qry.Append($"\n ) AS t1                                                                                                                                            ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                                  ");
            qry.Append($"\n 	SELECT                                                                                                                                         ");
            qry.Append($"\n       품명                                                                                                                                           ");
            qry.Append($"\n 	, 원산지                                                                                                                                          ");
            qry.Append($"\n 	, 규격                                                                                                                                           ");
            qry.Append($"\n 	, 단위                                                                                                                                           ");
            qry.Append($"\n 	, 매출일자                                                                                                                                         ");
            qry.Append($"\n 	FROM  매출현황 AS a                                                                                                                                ");
            qry.Append($"\n 	WHERE 매출처코드 = {company_code}                                                                                                                         ");
            qry.Append($"\n 	  AND 매출일자 = (SELECT MAX(매출일자) FROM 매출현황 AS b WHERE 매출처코드 = {company_code} AND a.품명 = b.품명 AND a.원산지 = b.원산지 AND a.규격 = b.규격 AND a.단위 = b.단위)     ");
            qry.Append($"\n 	  GROUP BY 품명	, 원산지, 규격, 단위, 매출일자                                                                                                 ");
            qry.Append($"\n ) AS t2                                                                                                                                            ");
            qry.Append($"\n   ON t1.품명 = t2.품명                                                                                                                                 ");
            qry.Append($"\n   AND t1.원산지 = t2.원산지                                                                                                                             ");
            qry.Append($"\n   AND t1.규격 = t2.규격                                                                                                                                ");
            qry.Append($"\n   AND t1.단위 = t2.단위                                                                                                                                ");
            //qry.Append($"\n WHERE 매출수 > 0                                                                                                                                ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        private string DuplicateSql(string col_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT {col_name} FROM (                                                                                                                                                                                            ");
            qry.Append($"\n SELECT                                                                                                                                                                                              ");
            qry.Append($"\n   거래처코드, 거래처명, 최종매출일자                                                                                                                                                                                            ");
            qry.Append($"\n , CASE WHEN CHARINDEX('(', 전화번호1) > 0 THEN SUBSTRING(전화번호1, 0, CHARINDEX('(', 전화번호1)) ELSE 전화번호1 END AS 전화번호1                                                                                     ");
            qry.Append($"\n , CASE WHEN CHARINDEX('(', 전화번호2) > 0 THEN SUBSTRING(전화번호2, 0, CHARINDEX('(', 전화번호2)) ELSE 전화번호2 END AS 전화번호2                                                                                     ");
            qry.Append($"\n , CASE WHEN CHARINDEX('(', 팩스번호1) > 0 THEN SUBSTRING(팩스번호1, 0, CHARINDEX('(', 팩스번호1)) ELSE 팩스번호1 END AS 팩스번호1                                                                                     ");
            qry.Append($"\n , CASE WHEN CHARINDEX('(', 팩스번호2) > 0 THEN SUBSTRING(팩스번호2, 0, CHARINDEX('(', 팩스번호2)) ELSE 팩스번호2 END AS 팩스번호2                                                                                     ");
            qry.Append($"\n , CASE WHEN CHARINDEX('(', 휴대폰1) > 0 THEN SUBSTRING(휴대폰1, 0, CHARINDEX('(', 휴대폰1)) ELSE 휴대폰1 END AS 휴대폰1                                                                                          ");
            qry.Append($"\n , CASE WHEN CHARINDEX('(', 휴대폰2) > 0 THEN SUBSTRING(휴대폰2, 0, CHARINDEX('(', 휴대폰2)) ELSE 휴대폰2 END AS 휴대폰2                                                                                          ");
            qry.Append($"\n , 사업자번호                                                                                          ");
            qry.Append($"\n FROM (                                                                                                                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                                                                                                                          ");
            qry.Append($"\n 	  거래처코드, 거래처명, 최종매출일자                                                                                                                                                                                        ");
            qry.Append($"\n 	, 전화번호1                                                                                                                                                                                         ");
            qry.Append($"\n 	, CASE WHEN LEN(전화번호1) > LEN(전화번호2) THEN SUBSTRING(전화번호1, 0 , LEN(전화번호1) - LEN(전화번호2) + 1) +  전화번호2 ELSE 전화번호2 END AS 전화번호2                                                                 ");
            qry.Append($"\n 	, 팩스번호1                                                                                                                                                                                         ");
            qry.Append($"\n 	, CASE WHEN LEN(팩스번호1) > LEN(팩스번호2) THEN SUBSTRING(팩스번호1, 0 , LEN(팩스번호1) - LEN(팩스번호2) + 1) +  팩스번호2 ELSE 팩스번호2 END AS 팩스번호2                                                                 ");
            qry.Append($"\n 	, 휴대폰1                                                                                                                                                                                          ");
            qry.Append($"\n 	, CASE WHEN LEN(휴대폰1) > LEN(휴대폰2) THEN SUBSTRING(휴대폰1, 0 , LEN(휴대폰1) - LEN(휴대폰2) + 1) +  휴대폰2 ELSE 휴대폰2 END AS 휴대폰2                                                                          ");
            qry.Append($"\n 	, 사업자번호                                                                                                                                                                                          ");
            qry.Append($"\n 	FROM(                                                                                                                                                                                           ");
            qry.Append($"\n 		SELECT                                                                                                                                                                                      ");
            qry.Append($"\n 		  거래처코드, 거래처명, 최종매출일자                                                                                                                                                                                    ");
            qry.Append($"\n 		, CASE WHEN CHARINDEX('/', 전화번호) > 0 THEN SUBSTRING(전화번호, 0, CHARINDEX('/', 전화번호))                                                                                                         ");
            qry.Append($"\n 		  		WHEN CHARINDEX(',', 전화번호) > 0 THEN SUBSTRING(전화번호, 0, CHARINDEX(',', 전화번호))                                                                                                        ");
            qry.Append($"\n 		  		WHEN CHARINDEX('~', 전화번호) > 0 THEN SUBSTRING(전화번호, 0, CHARINDEX('~', 전화번호))                                                                                                        ");
            qry.Append($"\n 			ELSE 전화번호 END AS 전화번호1                                                                                                                                                                 ");
            qry.Append($"\n 		, CASE WHEN CHARINDEX('/', 전화번호) > 0 THEN SUBSTRING(전화번호, CHARINDEX('/', 전화번호) + 1, LEN(전화번호))                                                                                            ");
            qry.Append($"\n 				WHEN CHARINDEX(',', 전화번호) > 0 THEN SUBSTRING(전화번호, CHARINDEX(',', 전화번호) + 1, LEN(전화번호))                                                                                           ");
            qry.Append($"\n 				WHEN CHARINDEX('~', 전화번호) > 0 THEN SUBSTRING(전화번호, CHARINDEX('~', 전화번호) + 1, LEN(전화번호))                                                                                           ");
            qry.Append($"\n 			ELSE null END AS 전화번호2                                                                                                                                                                  ");
            qry.Append($"\n 		, CASE WHEN CHARINDEX('/', 팩스번호) > 0 THEN SUBSTRING(팩스번호, 0, CHARINDEX('/', 팩스번호))                                                                                                         ");
            qry.Append($"\n 		  		WHEN CHARINDEX(',', 팩스번호) > 0 THEN SUBSTRING(팩스번호, 0, CHARINDEX(',', 팩스번호))                                                                                                        ");
            qry.Append($"\n 		  		WHEN CHARINDEX('~', 팩스번호) > 0 THEN SUBSTRING(팩스번호, 0, CHARINDEX('~', 팩스번호))                                                                                                        ");
            qry.Append($"\n 			ELSE 팩스번호 END AS 팩스번호1                                                                                                                                                                 ");
            qry.Append($"\n 		, CASE WHEN CHARINDEX('/', 팩스번호) > 0 THEN SUBSTRING(팩스번호, CHARINDEX('/', 팩스번호) + 1, LEN(팩스번호))                                                                                            ");
            qry.Append($"\n 				WHEN CHARINDEX(',', 팩스번호) > 0 THEN SUBSTRING(팩스번호, CHARINDEX(',', 팩스번호) + 1, LEN(팩스번호))                                                                                           ");
            qry.Append($"\n 				WHEN CHARINDEX('~', 팩스번호) > 0 THEN SUBSTRING(팩스번호, CHARINDEX('~', 팩스번호) + 1, LEN(팩스번호))                                                                                           ");
            qry.Append($"\n 			ELSE null END AS 팩스번호2                                                                                                                                                                  ");
            qry.Append($"\n 		, CASE WHEN CHARINDEX('/', 휴대폰) > 0 THEN SUBSTRING(휴대폰, 0, CHARINDEX('/', 휴대폰))                                                                                                            ");
            qry.Append($"\n 		  		WHEN CHARINDEX(',', 휴대폰) > 0 THEN SUBSTRING(휴대폰, 0, CHARINDEX(',', 휴대폰))                                                                                                           ");
            qry.Append($"\n 		  		WHEN CHARINDEX('~', 휴대폰) > 0 THEN SUBSTRING(휴대폰, 0, CHARINDEX('~', 휴대폰))                                                                                                           ");
            qry.Append($"\n 			ELSE 휴대폰 END AS 휴대폰1                                                                                                                                                                   ");
            qry.Append($"\n 		, CASE WHEN CHARINDEX('/', 휴대폰) > 0 THEN SUBSTRING(휴대폰, CHARINDEX('/', 휴대폰) + 1, LEN(휴대폰))                                                                                                 ");
            qry.Append($"\n 				WHEN CHARINDEX(',', 휴대폰) > 0 THEN SUBSTRING(휴대폰, CHARINDEX(',', 휴대폰) + 1, LEN(휴대폰))                                                                                                ");
            qry.Append($"\n 				WHEN CHARINDEX('~', 휴대폰) > 0 THEN SUBSTRING(휴대폰, CHARINDEX('~', 휴대폰) + 1, LEN(휴대폰))                                                                                                ");
            qry.Append($"\n 			ELSE null END AS 휴대폰2                                                                                                                                                                   ");
            qry.Append($"\n 		, 사업자번호                                                                                                ");
            qry.Append($"\n 		FROM (                                                                                                                                                                                      ");
            qry.Append($"\n 			SELECT                                                                                                                                                                                  ");
            qry.Append($"\n 			  거래처코드, 거래처명, 최종매출일자                                                                                                                                                                                ");
            qry.Append($"\n 			, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(전화번호, ')', ''), '-', ''), '. 010', ',010'), '.', ''), ' ', ''), '//', '/'), 'xx010', '010'), '계산서', '') AS 전화번호      ");
            qry.Append($"\n 			, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(팩스번호, ')', ''), '-', ''), '. 010', ',010'), '.', ''), ' ', ''), '//', '/'), 'xx010', '010'), '계산서', '') AS 팩스번호      ");
            qry.Append($"\n 			, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(휴대폰, ')', ''), '-', ''), '. 010', ',010'), '.', ''), ' ', ''), '//', '/'), 'xx010', '010'), '계산서', '') AS 휴대폰        ");
            qry.Append($"\n 			, REPLACE(REPLACE(사업자번호, '-', ''), ' ', '') AS 사업자번호        ");
            qry.Append($"\n 			FROM (SELECT * FROM (SELECT ROW_NUMBER() OVER (PARTITION BY 거래처코드 ORDER BY 최종매출일자 DESC) AS rownum, 거래처코드, 거래처명, 최종매출일자, 전화번호, 팩스번호, 휴대폰, 사업자번호 FROM 업체별월별매출현황) AS t WHERE rownum = 1) AS t   ");
            //qry.Append($"\n 			WHERE 사용자 = {DateTime.Now.ToString("yyyyMM")}                                                                                                                                                                      ");
            qry.Append($"\n 			WHERE 1=1                                                                                                                                                                      ");
            qry.Append($"\n 		) AS t                                                                                                                                                                                      ");
            qry.Append($"\n 	) AS t                                                                                                                                                                                          ");
            qry.Append($"\n ) AS t                                                                                                                                                                                              ");
            qry.Append($"\n ) AS t                                                                                                                                                                                              ");

            string sql = qry.ToString();

            return qry.ToString();
        }

        public DataTable GetSales(string sttdate, string enddate, string product, string origin, string sizes, string unit, bool all_unit = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                     ");
            qry.Append($"\n  CASE WHEN YEAR(GETDATE()) = YEAR(매출일자) THEN DATEPART(WEEK, GETDATE()) - DATEPART(WEEK, 매출일자)                                                                                            ");
            qry.Append($"\n 	 WHEN YEAR(GETDATE()) > YEAR(매출일자) THEN DATEPART(WEEK, CONVERT(DATETIME, CONVERT(varchar, YEAR(매출일자)) + '-12-31')) - DATEPART(WEEK, 매출일자) + DATEPART(WEEK, GETDATE()) END AS 주차     ");
            qry.Append($"\n  , SUM(ISNULL(매출수, 0)) AS 매출수                                                         ");
            qry.Append($"\n  , SUM(ISNULL(매출금액, 0) + ISNULL(부가세, 0)) AS 매출금액                                     ");
            qry.Append($"\n  , SUM(ISNULL(매출금액, 0)) - SUM(ISNULL(매입금액, 0)) AS 마진금액                               ");
            qry.Append($"\n  , (SUM(ISNULL(매출금액, 0)) - SUM(ISNULL(매입금액, 0))) / SUM(ISNULL(매출금액, 0)) * 100 AS 마진율 ");
            qry.Append($"\n FROM 매출현황                                                                           ");
            qry.Append($"\n WHERE 사용자 = '200009'                                                                 ");
            qry.Append($"\n   AND 매출일자 <= '{sttdate}'                                                           ");
            qry.Append($"\n   AND 매출일자 >= '{enddate}'                                                           ");
            qry.Append($"\n   AND 품명 = '{product}'                                                                ");
            qry.Append($"\n   AND 원산지 = '{origin}'                                                               ");
            qry.Append($"\n   AND 규격 = '{sizes}'                                                                  ");
            if(!all_unit)
                qry.Append($"\n   AND REPLACE(단위, 'kg', '') = '{unit}'                                                ");
            qry.Append($"\n  GROUP BY CASE WHEN YEAR(GETDATE()) = YEAR(매출일자) THEN DATEPART(WEEK, GETDATE()) - DATEPART(WEEK, 매출일자)                                                                                ");
            qry.Append($"\n  	 WHEN YEAR(GETDATE()) > YEAR(매출일자) THEN DATEPART(WEEK, CONVERT(DATETIME, CONVERT(varchar, YEAR(매출일자)) + '-12-31')) - DATEPART(WEEK, 매출일자) + DATEPART(WEEK, GETDATE()) END        ");
            qry.Append($"\n  ORDER BY CASE WHEN YEAR(GETDATE()) = YEAR(매출일자) THEN DATEPART(WEEK, GETDATE()) - DATEPART(WEEK, 매출일자)                                                                                ");
            qry.Append($"\n  	 WHEN YEAR(GETDATE()) > YEAR(매출일자) THEN DATEPART(WEEK, CONVERT(DATETIME, CONVERT(varchar, YEAR(매출일자)) + '-12-31')) - DATEPART(WEEK, 매출일자) + DATEPART(WEEK, GETDATE()) END        ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetAverageSalesByMonth(string product, string origin, string sizes, string unit, bool all_unit = false, string col_name = "매출수")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                  ");
            qry.Append($"\n   SUM(CASE WHEN 개월 = 1 THEN t.{col_name} ELSE 0 END) {col_name}1                                     ");
            qry.Append($"\n , SUM(CASE WHEN 개월 = 45 THEN t.{col_name} ELSE 0 END) {col_name}45                                   ");
            qry.Append($"\n , SUM(CASE WHEN 개월 = 2 THEN t.{col_name} ELSE 0 END) {col_name}2                                     ");
            qry.Append($"\n , SUM(CASE WHEN 개월 = 3 THEN t.{col_name} ELSE 0 END) {col_name}3                                     ");
            qry.Append($"\n , SUM(CASE WHEN 개월 = 6 THEN t.{col_name} ELSE 0 END) {col_name}6                                     ");
            qry.Append($"\n , SUM(CASE WHEN 개월 = 12 THEN t.{col_name} ELSE 0 END) {col_name}12                                   ");
            qry.Append($"\n , SUM(CASE WHEN 개월 = 18 THEN t.{col_name} ELSE 0 END) {col_name}18                                   ");
            qry.Append($"\n FROM(                                                                                   ");
            qry.Append($"\n 	SELECT                                                                              ");
            qry.Append($"\n 	  1 AS 개월                                                                           ");
            qry.Append($"\n 	, SUM({col_name}) AS {col_name}                                                                  ");
            qry.Append($"\n 	FROM 품명별매출현황                                                                       ");
            qry.Append($"\n 	WHERE 사용자 = 1                                                                       ");
            if(!all_unit)
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}' AND  REPLACE(단위, 'kg', '') = '{unit}'               ");
            else
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}'                ");
            qry.Append($"\n 	UNION ALL                                                                           ");
            qry.Append($"\n 	SELECT                                                                              ");
            qry.Append($"\n 	  45 AS 개월                                                                          ");
            qry.Append($"\n 	, SUM({col_name}) AS {col_name}                                                                  ");
            qry.Append($"\n 	FROM 품명별매출현황                                                                       ");
            qry.Append($"\n 	WHERE 사용자 = 45                                                                      ");
            if (!all_unit)
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}' AND  REPLACE(단위, 'kg', '') = '{unit}'               ");
            else
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}'                ");
            qry.Append($"\n 	UNION ALL                                                                           ");
            qry.Append($"\n 	SELECT                                                                              ");
            qry.Append($"\n 	  2 AS 개월                                                                           ");
            qry.Append($"\n 	, SUM({col_name}) AS {col_name}                                                                  ");
            qry.Append($"\n 	FROM 품명별매출현황                                                                       ");
            qry.Append($"\n 	WHERE 사용자 = 2                                                                       ");
            if (!all_unit)
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}' AND  REPLACE(단위, 'kg', '') = '{unit}'               ");
            else
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}'                ");
            qry.Append($"\n 	UNION ALL                                                                           ");
            qry.Append($"\n 	SELECT                                                                              ");
            qry.Append($"\n 	  3 AS 개월                                                                           ");
            qry.Append($"\n 	, SUM({col_name}) AS {col_name}                                                                  ");
            qry.Append($"\n 	FROM 품명별매출현황                                                                       ");
            qry.Append($"\n 	WHERE 사용자 = 3                                                                       ");
            if (!all_unit)
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}' AND  REPLACE(단위, 'kg', '') = '{unit}'               ");
            else
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}'                ");
            qry.Append($"\n 	UNION ALL                                                                           ");
            qry.Append($"\n 	SELECT                                                                              ");
            qry.Append($"\n 	  6 AS 개월                                                                           ");
            qry.Append($"\n 	, SUM({col_name}) AS {col_name}                                                                  ");
            qry.Append($"\n 	FROM 품명별매출현황                                                                       ");
            qry.Append($"\n 	WHERE 사용자 = 6                                                                       ");
            if (!all_unit)
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}' AND  REPLACE(단위, 'kg', '') = '{unit}'               ");
            else
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}'                ");
            qry.Append($"\n 	UNION ALL                                                                           ");
            qry.Append($"\n 	SELECT                                                                              ");
            qry.Append($"\n 	  12 AS 개월                                                                          ");
            qry.Append($"\n 	, SUM({col_name}) AS {col_name}                                                                  ");
            qry.Append($"\n 	FROM 품명별매출현황                                                                       ");
            qry.Append($"\n 	WHERE 사용자 = 12                                                                      ");
            if (!all_unit)
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}' AND  REPLACE(단위, 'kg', '') = '{unit}'               ");
            else
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}'                ");
            qry.Append($"\n 	UNION ALL                                                                           ");
            qry.Append($"\n 	SELECT                                                                              ");
            qry.Append($"\n 	  18 AS 개월                                                                          ");
            qry.Append($"\n 	, SUM({col_name}) AS {col_name}                                                                  ");
            qry.Append($"\n 	FROM 품명별매출현황                                                                       ");
            qry.Append($"\n 	WHERE 사용자 = 18                                                                      ");
            if (!all_unit)
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}' AND  REPLACE(단위, 'kg', '') = '{unit}'               ");
            else
                qry.Append($"\n 	  AND 품명 = '{product}' AND 원산지 = '{origin}' AND 규격 = '{sizes}'                ");
            qry.Append($"\n ) AS t                                                                                  ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetAverageSalesByMonth2(DateTime eDate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge = false, string col_name = "매출수")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                         ");
            qry.Append($"\n   SUM(t1.{col_name})  AS {col_name}1                                        ");
            qry.Append($"\n , SUM(t45.{col_name}) AS {col_name}45                                       ");
            qry.Append($"\n , SUM(t2.{col_name})  AS {col_name}2                                        ");
            qry.Append($"\n , SUM(t3.{col_name})  AS {col_name}3                                        ");
            qry.Append($"\n , SUM(t6.{col_name})  AS {col_name}6                                        ");
            qry.Append($"\n , SUM(t12.{col_name}) AS {col_name}12                                       ");
            qry.Append($"\n , SUM(t18.{col_name}) AS {col_name}18                                       ");
            qry.Append($"\n FROM(                                                          ");
            qry.Append($"\n 	SELECT                                                     ");
            qry.Append($"\n 	DISTINCT                                                   ");
            qry.Append($"\n 	  품명                                                       ");
            qry.Append($"\n 	, 원산지                                                      ");
            qry.Append($"\n 	, 규격                                                       ");
            qry.Append($"\n 	, REPLACE(단위,'KG','') AS 단위                                ");
            qry.Append($"\n 	FROM 매출현황                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                       ");
            qry.Append($"\n 	  AND 매출자 <> '199990'                                      ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토무역%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토코리아%'                              ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에이티오%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                            ");
            qry.Append($"\n 	  AND 매출일자 >= '{eDate.AddMonths(-18).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{eDate.ToString("yyyy-MM-dd")}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 = '{sizes}'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n ) AS t                                                         ");
            qry.Append($"\n LEFT OUTER JOIN                                                ");
            qry.Append($"\n (                                                              ");
            qry.Append($"\n 	SELECT                                                     ");
            qry.Append($"\n 	  품명                                                       ");
            qry.Append($"\n 	, 원산지                                                      ");
            qry.Append($"\n 	, 규격                                                       ");
            qry.Append($"\n 	, REPLACE(단위,'KG','') AS 단위                                ");
            qry.Append($"\n 	, SUM(ISNULL({col_name}, 0) * CAST(REPLACE(단위,'KG','') AS float) / {unit}) AS {col_name}                              ");
            qry.Append($"\n 	FROM 매출현황                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                       ");
            qry.Append($"\n 	  AND 매출자 <> '199990'                                      ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토무역%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토코리아%'                              ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에이티오%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                            ");
            qry.Append($"\n 	  AND 매출일자 >= '{eDate.AddMonths(-1).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{eDate.AddDays(-1).ToString("yyyy-MM-dd")}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 = '{sizes}'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	GROUP BY 품명	, 원산지	, 규격	, REPLACE(단위,'KG','')          ");
            qry.Append($"\n ) AS t1                                                        ");
            qry.Append($"\n   ON t.품명 = t1.품명                                              ");
            qry.Append($"\n   AND t.원산지 = t1.원산지                                          ");
            qry.Append($"\n   AND t.규격 = t1.규격                                             ");
            qry.Append($"\n   AND REPLACE(t.단위,'KG','') = REPLACE(t1.단위,'KG','')           ");
            qry.Append($"\n LEFT OUTER JOIN                                                ");
            qry.Append($"\n (                                                              ");
            qry.Append($"\n 	SELECT                                                     ");
            qry.Append($"\n 	  품명                                                       ");
            qry.Append($"\n 	, 원산지                                                      ");
            qry.Append($"\n 	, 규격                                                       ");
            qry.Append($"\n 	, REPLACE(단위,'KG','') AS 단위                                ");
            qry.Append($"\n 	, SUM(ISNULL({col_name}, 0) * CAST(REPLACE(단위,'KG','') AS float) / {unit}) AS {col_name}                              ");
            qry.Append($"\n 	FROM 매출현황                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                       ");
            qry.Append($"\n 	  AND 매출자 <> '199990'                                      ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토무역%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토코리아%'                              ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에이티오%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                            ");
            qry.Append($"\n 	  AND 매출일자 >= '{eDate.AddDays(-45).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{eDate.AddDays(-1).ToString("yyyy-MM-dd")}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 = '{sizes}'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	GROUP BY 품명	, 원산지	, 규격	, REPLACE(단위,'KG','')          ");
            qry.Append($"\n ) AS t45                                                       ");
            qry.Append($"\n   ON t.품명 = t45.품명                                             ");
            qry.Append($"\n   AND t.원산지 = t45.원산지                                         ");
            qry.Append($"\n   AND t.규격 = t45.규격                                            ");
            qry.Append($"\n   AND REPLACE(t.단위,'KG','') = REPLACE(t45.단위,'KG','')          ");
            qry.Append($"\n LEFT OUTER JOIN                                                ");
            qry.Append($"\n (                                                              ");
            qry.Append($"\n 	SELECT                                                     ");
            qry.Append($"\n 	  품명                                                       ");
            qry.Append($"\n 	, 원산지                                                      ");
            qry.Append($"\n 	, 규격                                                       ");
            qry.Append($"\n 	, REPLACE(단위,'KG','') AS 단위                                ");
            qry.Append($"\n 	, SUM(ISNULL({col_name}, 0) * CAST(REPLACE(단위,'KG','') AS float) / {unit}) AS {col_name}                              ");
            qry.Append($"\n 	FROM 매출현황                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                       ");
            qry.Append($"\n 	  AND 매출자 <> '199990'                                      ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토무역%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토코리아%'                              ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에이티오%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                            ");
            qry.Append($"\n 	  AND 매출일자 >= '{eDate.AddMonths(-2).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{eDate.AddDays(-1).ToString("yyyy-MM-dd")}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 = '{sizes}'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	GROUP BY 품명	, 원산지	, 규격	, REPLACE(단위,'KG','')          ");
            qry.Append($"\n ) AS t2                                                        ");
            qry.Append($"\n   ON t.품명 = t2.품명                                              ");
            qry.Append($"\n   AND t.원산지 = t2.원산지                                          ");
            qry.Append($"\n   AND t.규격 = t2.규격                                             ");
            qry.Append($"\n   AND REPLACE(t.단위,'KG','') = REPLACE(t2.단위,'KG','')           ");
            qry.Append($"\n   LEFT OUTER JOIN                                              ");
            qry.Append($"\n (                                                              ");
            qry.Append($"\n 	SELECT                                                     ");
            qry.Append($"\n 	  품명                                                       ");
            qry.Append($"\n 	, 원산지                                                      ");
            qry.Append($"\n 	, 규격                                                       ");
            qry.Append($"\n 	, REPLACE(단위,'KG','') AS 단위                                ");
            qry.Append($"\n 	, SUM(ISNULL({col_name}, 0) * CAST(REPLACE(단위,'KG','') AS float) / {unit}) AS {col_name}                              ");
            qry.Append($"\n 	FROM 매출현황                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                       ");
            qry.Append($"\n 	  AND 매출자 <> '199990'                                      ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토무역%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토코리아%'                              ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에이티오%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                            ");
            qry.Append($"\n 	  AND 매출일자 >= '{eDate.AddMonths(-3).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{eDate.AddDays(-1).ToString("yyyy-MM-dd")}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 = '{sizes}'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	GROUP BY 품명	, 원산지	, 규격	, REPLACE(단위,'KG','')          ");
            qry.Append($"\n ) AS t3                                                        ");
            qry.Append($"\n   ON t.품명 = t3.품명                                              ");
            qry.Append($"\n   AND t.원산지 = t3.원산지                                          ");
            qry.Append($"\n   AND t.규격 = t3.규격                                             ");
            qry.Append($"\n   AND REPLACE(t.단위,'KG','') = REPLACE(t3.단위,'KG','')           ");
            qry.Append($"\n LEFT OUTER JOIN                                                ");
            qry.Append($"\n (                                                              ");
            qry.Append($"\n 	SELECT                                                     ");
            qry.Append($"\n 	  품명                                                       ");
            qry.Append($"\n 	, 원산지                                                      ");
            qry.Append($"\n 	, 규격                                                       ");
            qry.Append($"\n 	, REPLACE(단위,'KG','') AS 단위                                ");
            qry.Append($"\n 	, SUM(ISNULL({col_name}, 0) * CAST(REPLACE(단위,'KG','') AS float) / {unit}) AS {col_name}                              ");
            qry.Append($"\n 	FROM 매출현황                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                       ");
            qry.Append($"\n 	  AND 매출자 <> '199990'                                      ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토무역%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토코리아%'                              ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에이티오%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                            ");
            qry.Append($"\n 	  AND 매출일자 >= '{eDate.AddMonths(-6).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{eDate.AddDays(-1).ToString("yyyy-MM-dd")}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 = '{sizes}'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	GROUP BY 품명	, 원산지	, 규격	, REPLACE(단위,'KG','')          ");
            qry.Append($"\n ) AS t6                                                        ");
            qry.Append($"\n   ON t.품명 = t6.품명                                              ");
            qry.Append($"\n   AND t.원산지 = t6.원산지                                          ");
            qry.Append($"\n   AND t.규격 = t6.규격                                             ");
            qry.Append($"\n   AND REPLACE(t.단위,'KG','') = REPLACE(t6.단위,'KG','')           ");
            qry.Append($"\n   LEFT OUTER JOIN                                              ");
            qry.Append($"\n (                                                              ");
            qry.Append($"\n 	SELECT                                                     ");
            qry.Append($"\n 	  품명                                                       ");
            qry.Append($"\n 	, 원산지                                                      ");
            qry.Append($"\n 	, 규격                                                       ");
            qry.Append($"\n 	, REPLACE(단위,'KG','') AS 단위                                ");
            qry.Append($"\n 	, SUM(ISNULL({col_name}, 0) * CAST(REPLACE(단위,'KG','') AS float) / {unit}) AS {col_name}                              ");
            qry.Append($"\n 	FROM 매출현황                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                       ");
            qry.Append($"\n 	  AND 매출자 <> '199990'                                      ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토무역%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토코리아%'                              ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에이티오%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                            ");
            qry.Append($"\n 	  AND 매출일자 >= '{eDate.AddMonths(-12).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{eDate.AddDays(-1).ToString("yyyy-MM-dd")}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 = '{sizes}'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	GROUP BY 품명	, 원산지	, 규격	, REPLACE(단위,'KG','')          ");
            qry.Append($"\n ) AS t12                                                       ");
            qry.Append($"\n   ON t.품명 = t12.품명                                             ");
            qry.Append($"\n   AND t.원산지 = t12.원산지                                         ");
            qry.Append($"\n   AND t.규격 = t12.규격                                            ");
            qry.Append($"\n   AND REPLACE(t.단위,'KG','') = REPLACE(t12.단위,'KG','')          ");
            qry.Append($"\n   LEFT OUTER JOIN                                              ");
            qry.Append($"\n (                                                              ");
            qry.Append($"\n 	SELECT                                                     ");
            qry.Append($"\n 	  품명                                                       ");
            qry.Append($"\n 	, 원산지                                                      ");
            qry.Append($"\n 	, 규격                                                       ");
            qry.Append($"\n 	, REPLACE(단위,'KG','') AS 단위                                ");
            qry.Append($"\n 	, SUM(ISNULL({col_name}, 0) * CAST(REPLACE(단위,'KG','') AS float) / {unit}) AS {col_name}                              ");
            qry.Append($"\n 	FROM 매출현황                                                  ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                       ");
            qry.Append($"\n 	  AND 매출자 <> '199990'                                      ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토무역%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%아토코리아%'                              ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에이티오%'                               ");
            qry.Append($"\n 	  AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                            ");
            qry.Append($"\n 	  AND 매출일자 >= '{eDate.AddMonths(-18).ToString("yyyy-MM-dd")}' AND 매출일자 <= '{eDate.AddDays(-1).ToString("yyyy-MM-dd")}'       ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 = '{sizes}'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	GROUP BY 품명	, 원산지	, 규격	, REPLACE(단위,'KG','')          ");
            qry.Append($"\n ) AS t18                                                       ");
            qry.Append($"\n   ON t.품명 = t18.품명                                             ");
            qry.Append($"\n   AND t.원산지 = t18.원산지                                         ");
            qry.Append($"\n   AND t.규격 = t18.규격                                            ");
            qry.Append($"\n   AND REPLACE(t.단위,'KG','') = REPLACE(t18.단위,'KG','')          ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetSalesGroupMonth(DateTime sttdate, DateTime enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge, int terms_type = 1, bool isAtoSale = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                      ");
            qry.Append($"\n    p.sdate AS 기준일자           ");
            qry.Append($"\n   , SUM(t.매출수) AS 매출수                                                                                ");
            qry.Append($"\n   , SUM(t.매출금액) AS 매출금액                                                                              ");
            qry.Append($"\n   , SUM(t.마진금액) AS 마진금액                                                                              ");
            qry.Append($"\n   , CASE WHEN SUM(t.매출금액) > 0 THEN SUM(t.마진금액) / SUM(t.매출금액) * 100 ELSE 0 END AS 마진율                ");
            qry.Append($"\n  FROM (                      ");
            qry.Append($"\n    {GetYearMonth(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"))}                                                                                                                                                          ");
            qry.Append($"\n  ) AS p                                                                                                                                                            ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                                                                 ");
            qry.Append($"\n 	 SELECT                                                                                                                                                        ");
            qry.Append($"\n 	   t.매출일자                                                                                                                                                      ");
            qry.Append($"\n 	 , SUM(CONVERT(float,t.매출수) * CONVERT(float, t.단위) / {unit}) AS 매출수                                                                                             ");
            qry.Append($"\n 	 , SUM(t.매출금액) AS 매출금액                                                                                                                                        ");
            qry.Append($"\n 	 , SUM(t.마진금액) AS 마진금액                                                                                                                                        ");
            qry.Append($"\n 	 FROM (                                                                                                                                                        ");
            qry.Append($"\n 		 SELECT                                                                                                                                                    ");
            if (terms_type == 1)
                qry.Append($"\n 		   LEFT(CONVERT(varchar, 매출일자,112),6) AS 매출일자                                                                                                             ");
            else 
            {
                qry.Append($"\n 	       CASE WHEN 매출일자 > CONVERT(datetime, CONVERT(varchar, YEAR(DATEADD(MONTH, -1, 매출일자))) + '-' + CONVERT(varchar, MONTH(DATEADD(MONTH, -1, 매출일자))) + '-{terms_type}') AND 매출일자 <= CONVERT(datetime, CONVERT(varchar, YEAR(매출일자)) + '-' + CONVERT(varchar, MONTH(매출일자)) + '-{terms_type}') THEN LEFT(CONVERT(varchar, 매출일자,112),6)      ");
                qry.Append($"\n  		 		WHEN 매출일자 > CONVERT(datetime, CONVERT(varchar, YEAR(매출일자)) + '-' + CONVERT(varchar, MONTH(매출일자)) + '-{terms_type}') THEN LEFT(CONVERT(varchar, DATEADD(MONTH, 1, 매출일자),112),6) END AS 매출일자			                                                                                                                 ");
            }

            //qry.Append($"\n 		 , REPLACE(CASE WHEN 단위 = '-' OR 단위 = '.' OR 단위 = 'box' OR 단위 = 'b/x' OR 단위 = '박스' OR 단위 = '팩' OR 단위 = '묶음' OR CHARINDEX('부정관', 단위) > 0 THEN '{unit}'     ");
            qry.Append($"\n 		 , REPLACE(CASE WHEN 단위 = '-' OR 단위 = '.' OR 단위 = 'box' OR 단위 = 'b/x' OR 단위 = '박스' OR 단위 = '팩' OR 단위 = '묶음' OR CHARINDEX('부정관', 단위) > 0 THEN null     ");
            qry.Append($"\n 		 	  WHEN  CHARINDEX('(', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('(', 단위))                                                                              ");
            qry.Append($"\n 		 	  WHEN  CHARINDEX('~', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('~', 단위))                                                                              ");
            qry.Append($"\n 		 	  ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(단위, '내외', ''), '전후', ''), '벌크', ''), '이상', ''), '부정', '')                                              ");
            qry.Append($"\n 		   END, 'kg', '') AS 단위                                                                                                                                    ");
            qry.Append($"\n 		 , ISNULL(매출수, 0) AS 매출수                                                                                                                                  ");
            qry.Append($"\n 		 , ISNULL(매출금액, 0) AS 매출금액                                                                                                                                ");
            qry.Append($"\n 		 , ISNULL(매출금액, 0) - ISNULL(매입금액, 0) AS 마진금액                                                                                                              ");
            qry.Append($"\n 		 FROM 매출현황                                                                                                                                                 ");
            qry.Append($"\n 		 WHERE 사용자 = '200009'                                                                                                                                      ");
            //관리자 매출 제외
            qry.Append($"\n 		   AND 매출자 <> '199990'                                                                                                                                     ");
            qry.Append($"\n 		   AND 매출처 NOT LIKE '%아토무역%'                                                                                                                                     ");
            qry.Append($"\n 		   AND 매출처 NOT LIKE '%에이티오%'                                                                                                                                     ");
            qry.Append($"\n 		   AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                                                                                                                                     ");
            qry.Append($"\n 		   AND 매출처 NOT LIKE '%아토코리아%'                                                                                                                                     ");

            //BL이 본선명에 있는 내역만
            if(isAtoSale)
                qry.Append($"\n 		   AND 본선명 LIKE '%B/L%'                                                                ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 LIKE '%{sizes}%'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            { 
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if(sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	 ) AS t                                                                                                                                                        ");
            qry.Append($"\n 	  GROUP BY t.매출일자                                                                                                                                       ");
            qry.Append($"\n  ) AS t                                                                                                                                                            ");
            qry.Append($"\n    ON p.sdate = t.매출일자                                                                                                                                             ");
            qry.Append($"\n  GROUP BY p.sdate                                                                                                                                       ");
            qry.Append($"\n  ORDER BY p.sdate ASC                                                                                                                                              ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesGroupMonth(DateTime sttdate, DateTime enddate, int until_days, string product, string origin, string sizes, string unit, string sub_product, bool isMerge, bool isAtoSale = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                      ");
            qry.Append($"\n    p.sdate AS 기준일자           ");
            qry.Append($"\n   , SUM(t.매출수) AS 매출수                                                                                ");
            qry.Append($"\n   , SUM(t.매출금액) AS 매출금액                                                                              ");
            qry.Append($"\n   , SUM(t.마진금액) AS 마진금액                                                                              ");
            qry.Append($"\n   , CASE WHEN SUM(t.매출금액) > 0 THEN SUM(t.마진금액) / SUM( t.매출금액) * 100 ELSE 0 END AS 마진율                ");
            qry.Append($"\n  FROM (                      ");
            qry.Append($"\n    {GetYearMonth(sttdate.ToString("yyyy-MM-dd"), enddate.ToString("yyyy-MM-dd"))}                                                                                                                                                          ");
            qry.Append($"\n  ) AS p                                                                                                                                                            ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                                                                 ");
            qry.Append($"\n 	 SELECT                                                                                                                                                        ");
            qry.Append($"\n 	   t.매출일자                                                                                                                                                      ");
            qry.Append($"\n 	 , SUM(CONVERT(float,t.매출수) * CONVERT(float, t.단위) / {unit}) AS 매출수                                                                                             ");
            qry.Append($"\n 	 , SUM(t.매출금액) AS 매출금액                                                                                                                                        ");
            qry.Append($"\n 	 , SUM(t.마진금액) AS 마진금액                                                                                                                                        ");
            qry.Append($"\n 	 FROM (                                                                                                                                                        ");
            qry.Append($"\n 		 SELECT                                                                                                                                                    ");
            qry.Append($"\n 		 CASE WHEN 매출일자 >= CONVERT(datetime, CONVERT(varchar, YEAR(매출일자)) + '-' + CONVERT(varchar, MONTH(매출일자)) + '-1')                                                                                                                                                                                    ");
            qry.Append($"\n 		 		AND 매출일자 <= CASE WHEN DAY(DATEADD(DAY, -1, DATEADD(MONTH, 1, CONVERT(datetime, CONVERT(varchar, YEAR(매출일자)) + '-' + CONVERT(varchar, MONTH(매출일자)) + '-01')))) < {until_days}                                                                                                                         ");
            qry.Append($"\n 		 							THEN CONVERT(datetime, CONVERT(varchar, YEAR(매출일자)) + '-' + CONVERT(varchar, MONTH(매출일자)) + '-' + CONVERT(varchar, DAY(DATEADD(DAY, -1, DATEADD(MONTH, 1, CONVERT(datetime, CONVERT(varchar, YEAR(매출일자)) + '-' + CONVERT(varchar, MONTH(매출일자)) + '-01'))))))           ");
            qry.Append($"\n 		 							ELSE CONVERT(datetime, CONVERT(varchar, YEAR(매출일자)) + '-' + CONVERT(varchar, MONTH(매출일자)) + '-{until_days}') END                                                                                                                                                                  ");
            qry.Append($"\n 		 		THEN LEFT(CONVERT(varchar, 매출일자,112),6)                                                                                                                                                                                                                                                     ");
            qry.Append($"\n  		 	ELSE  'NULL' END AS 매출일자                                                                                                                                                                                                                                                                        ");
            qry.Append($"\n 		 , REPLACE(CASE WHEN 단위 = '-' OR 단위 = '.' OR 단위 = 'box' OR 단위 = 'b/x' OR 단위 = '박스' OR 단위 = '팩' OR 단위 = '묶음' OR CHARINDEX('부정관', 단위) > 0 THEN null     ");
            qry.Append($"\n 		 	  WHEN  CHARINDEX('(', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('(', 단위))                                                                              ");
            qry.Append($"\n 		 	  WHEN  CHARINDEX('~', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('~', 단위))                                                                              ");
            qry.Append($"\n 		 	  ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(단위, '내외', ''), '전후', ''), '벌크', ''), '이상', ''), '부정', '')                                              ");
            qry.Append($"\n 		   END, 'kg', '') AS 단위                                                                                                                                    ");
            qry.Append($"\n 		 , ISNULL(매출수, 0) AS 매출수                                                                                                                                  ");
            qry.Append($"\n 		 , ISNULL(매출금액, 0) AS 매출금액                                                                                                                                ");
            qry.Append($"\n 		 , ISNULL(매출금액, 0) - ISNULL(매입금액, 0) AS 마진금액                                                                                                              ");
            qry.Append($"\n 		 FROM 매출현황                                                                                                                                                 ");
            qry.Append($"\n 		 WHERE 사용자 = '200009'                                                                                                                                      ");
            qry.Append($"\n 		   AND 매출자 <> '199990'                                                                                                                                     ");
            qry.Append($"\n 		   AND 매출처 NOT LIKE '%아토무역%'                                                                                                                                     ");
            qry.Append($"\n 		   AND 매출처 NOT LIKE '%에이티오%'                                                                                                                                     ");
            qry.Append($"\n 		   AND 매출처 NOT LIKE '%아토코리아%'                                                                                                                                     ");
            qry.Append($"\n 		   AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                                                                                                                                     ");

            if(isAtoSale)
                qry.Append($"\n 		   AND 본선명 LIKE 'B/L%'                        ");

            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n 		   AND 품명 = '{product}'                                                                                                                                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n 		   AND 원산지 = '{origin}'                                                                                                                                        ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n 		   AND 규격 LIKE '%{sizes}%'                                                                                                                                   ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n 		   AND REPLACE(단위, 'KG', '') = '{unit}'                                                                                                                                   ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }
            qry.Append($"\n 	 ) AS t                                                                                                                                                        ");
            qry.Append($"\n 	 WHERE t.매출일자 <> 'NULL'                                                                                                                                                      ");
            qry.Append($"\n 	 GROUP BY t.매출일자                                                                                                                                       ");
            qry.Append($"\n  ) AS t                                                                                                                                                            ");
            qry.Append($"\n    ON p.sdate = t.매출일자                                                                                                                                             ");
            qry.Append($"\n  GROUP BY p.sdate                                                                                                                                       ");
            qry.Append($"\n  ORDER BY p.sdate ASC                                                                                                                                              ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        private string GetYearMonth(string sttdate, string enddate)
        {
            string sql = "";

            DateTime sdate = Convert.ToDateTime(sttdate);
            DateTime edate = Convert.ToDateTime(enddate);
            DateTime tmpdate = sdate;


            while (tmpdate <= edate)
            {
                if (string.IsNullOrEmpty(sql))
                    sql += "\n" + "SELECT '" + tmpdate.ToString("yyyyMM") + "' AS sdate";
                else
                    sql += "\nUNION ALL\n" + "SELECT '" + tmpdate.ToString("yyyyMM") + "' AS sdate";
                tmpdate = tmpdate.AddMonths(1);
            }

            return sql;
        }
        public DataTable GetMonthSales(int months)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                ");
            qry.Append($"\n   품명                                                                                                  ");
            qry.Append($"\n , 원산지                                                                                                 ");
            qry.Append($"\n , 규격                                                                                                  ");

            for (int i = 1; i <= months; i++)
            {
                qry.Append($"\n , SUM(CASE WHEN 매출일자 <= '{DateTime.Now.AddDays(-1).AddMonths(-(i - 1)).ToString("yyyy-MM-dd")}' AND 매출일자 >= '{DateTime.Now.AddMonths(-i).ToString("yyyy-MM-dd")}' THEN 매출수 ELSE 0 END) AS 매출수{i}        ");
            }
            qry.Append($"\n FROM 매출현황                                                                                             ");
            qry.Append($"\n WHERE 사용자 = '200009'                                                                                  ");
            qry.Append($"\n   AND 매출일자 <= '{DateTime.Now.ToString("yyyy-MM-dd")}' AND 매출일자 >= '{DateTime.Now.AddDays(-1).AddMonths(-months).ToString("yyyy-MM-dd")}'                                                  ");
            qry.Append($"\n GROUP BY 품명, 원산지, 규격                                                                                 ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSaleCompany(string company, bool isExactly, string tel, string fax, string phone, string other_phone, string manager = ""
            , string sale_date = "", bool is_not_out_business = false, bool is_not_litigation = false, string seaover_code = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                                           ");
            qry.Append($"\n*                                                                                                                ");
            qry.Append($"\nFROM (                                                                                                           ");
            qry.Append($"\n  SELECT                                                                                                         ");
            qry.Append($"\n    'SEA' AS 구분                                                                                                ");
            qry.Append($"\n  , '' AS 그룹                                                                                                   ");
            qry.Append($"\n  , a.거래처명                                                                                                   ");
            qry.Append($"\n  , 사업자번호                                                                                                   ");
            qry.Append($"\n  , 대표자명                                                                                                     ");
            qry.Append($"\n  , a.전화번호                                                                                                   ");
            qry.Append($"\n  , a.팩스번호                                                                                                   ");
            qry.Append($"\n  , a.휴대폰                                                                                                     ");
            qry.Append($"\n  , a.기타연락처                                                                                                 ");
            qry.Append($"\n  , CASE WHEN a.폐업유무 = 'Y' THEN 'TRUE' ELSE 'FALSE' END AS 폐업유무                                          ");
            qry.Append($"\n  , a.업태                                                                                                       ");
            qry.Append($"\n  , a.종목                                                                                                       ");
            qry.Append($"\n  , a.참고사항                                                                                                   ");
            qry.Append($"\n  , CONVERT(CHAR(19),a.최종매출일자,20) AS 최종매출일자                                                          ");
            qry.Append($"\n  , a.매출자                                                                                                     ");
            qry.Append($"\n  , a.거래처코드                                                                                                 ");
            qry.Append($"\n  , a.사업장주소                                                                                                 ");
            qry.Append($"\n  , a.사용자                                                                                                     ");
            qry.Append($"\n  , ROW_NUMBER() OVER ( PARTITION BY a.거래처코드 ORDER BY a.사용자 DESC, a.최종매출일자 DESC) AS idx            ");
            qry.Append($"\n  FROM 업체별월별매출현황 AS a                                                                                   ");
            qry.Append($"\n  WHERE 거래처명 IS NOT NULL                                                                                     ");
            qry.Append($"\n    AND 사용자 <= {DateTime.Now.ToString("yyyyMM")}                                                              ");
            qry.Append($"\n    AND 사용자 >= 200000                                                                                         ");
            if (!string.IsNullOrEmpty(seaover_code))
                qry.Append($"\n    	  AND a.거래처코드 = '{seaover_code}'                                                                   ");
            { }
            if (!string.IsNullOrEmpty(company))
            {
                if (!isExactly)
                    qry.Append($"\n   {whereSql("a.거래처명", company)}                                                                     ");
                else
                    qry.Append($"\n   AND a.거래처명 = '%{company}%'                                                                        ");
            }
            if (!string.IsNullOrEmpty(tel))
            {
                qry.Append($"\n   AND (REPLACE(a.전화번호, '-', '') LIKE '%{tel}%'                                                          ");
                qry.Append($"\n     OR REPLACE(a.팩스번호, '-', '') LIKE '%{tel}%'                                                          ");
                qry.Append($"\n     OR REPLACE(a.기타연락처, '-', '') LIKE '%{tel}%'                                                        ");
                qry.Append($"\n     OR REPLACE(a.휴대폰, '-', '') LIKE '%{tel}%')                                                           ");
            }
            if (!string.IsNullOrEmpty(fax))
            {
                qry.Append($"\n   AND (REPLACE(a.전화번호, '-', '') LIKE '%{fax}%'                                                          ");
                qry.Append($"\n     OR REPLACE(a.팩스번호, '-', '') LIKE '%{fax}%'                                                          ");
                qry.Append($"\n     OR REPLACE(a.기타연락처, '-', '') LIKE '%{fax}%'                                                        ");
                qry.Append($"\n     OR REPLACE(a.휴대폰, '-', '') LIKE '%{fax}%')                                                           ");
            }
            if (!string.IsNullOrEmpty(phone))
            {
                qry.Append($"\n   AND (REPLACE(a.전화번호, '-', '') LIKE '%{phone}%'                                                        ");
                qry.Append($"\n     OR REPLACE(a.팩스번호, '-', '') LIKE '%{phone}%'                                                        ");
                qry.Append($"\n     OR REPLACE(a.기타연락처, '-', '') LIKE '%{phone}%'                                                      ");
                qry.Append($"\n     OR REPLACE(a.휴대폰, '-', '') LIKE '%{phone}%')                                                         ");
            }
            if (!string.IsNullOrEmpty(other_phone))
            {
                qry.Append($"\n   AND (REPLACE(a.전화번호, '-', '') LIKE '%{other_phone}%'                                                  ");
                qry.Append($"\n     OR REPLACE(a.팩스번호, '-', '') LIKE '%{other_phone}%'                                                  ");
                qry.Append($"\n     OR REPLACE(a.기타연락처, '-', '') LIKE '%{other_phone}%'                                                ");
                qry.Append($"\n     OR REPLACE(a.휴대폰, '-', '') LIKE '%{other_phone}%')                                                   ");
            }

            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   AND a.매출자 = '{manager}'                                                                                ");

            if (sale_date != "전체" && !string.IsNullOrEmpty(sale_date))
            {
                DateTime limitDate;
                if (sale_date == "15일 이상 지난")
                    limitDate = DateTime.Now.AddDays(-15);
                else if (sale_date == "한달 이상 지난")
                    limitDate = DateTime.Now.AddMonths(-1);
                else if (sale_date == "45일 이상 지난")
                    limitDate = DateTime.Now.AddDays(-45);
                else
                    limitDate = DateTime.Now.AddMonths(-2);


                qry.Append($"\n   AND (ISNULL(a.최종매출일자, '') = '' OR a.최종매출일자 <= '{limitDate.ToString("yyyy-MM-dd")}')          ");
            }
            /*if (is_not_out_business)
                qry.Append($"\n   AND CASE WHEN a.폐업유무 = 'Y' THEN 'true' ELSE 'false' END = 'false'                                                                                                                      ");*/
            /*if (is_not_litigation)
                qry.Append($"\n   AND a.거래처명 NOT LIKE '%(S)%' AND a.거래처명 NOT LIKE '%소송%'                                                                                                                      ");*/

            qry.Append($"\n   AND a.사업자번호 <> '0000000000'                                                                      ");
            qry.Append($"\n) AS a                                                                                                   ");
            qry.Append($"\nWHERE idx = 1                                                                                            ");
            qry.Append($"\nORDER BY  a.최종매출일자, a.거래처명                                                                       ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }


        public DataTable GetHandledItem(string company, string seaover_code = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT DISTINCT 매출처코드, 품명 FROM 매출현황 WHERE 사용자 = '200009'   ");

            /*qry.Append($"\n SELECT                                                                                                                                                                ");
            qry.Append($"\n   매출처코드                                                                                                                                                             ");
            qry.Append($"\n , STUFF((SELECT CAST(',' AS VARCHAR(MAX)) + 품명 FROM ( SELECT DISTINCT 매출처코드, 품명 FROM 매출현황 WHERE 사용자 = '200009') AS m FOR XML PATH('')), 1, 1, '') AS 매출품목        ");
            qry.Append($"\n FROM 매출현황 WHERE 사용자 = '200009'                                                                                                                                      ");*/
            if (!string.IsNullOrEmpty(seaover_code))
                qry.Append($"\n    	  AND 거래처코드 = '{seaover_code}'                                                                                                    ");
            if (!string.IsNullOrEmpty(company))
                    qry.Append($"\n   AND 거래처명 = '%{company}%'                                                                         ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetSalesProduct(string sttdate, string enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge, string sales_type)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                        ");
            qry.Append($"\n   LEFT(CONVERT(varchar, 매출일자,112),6) AS 매출일자                                                                                 ");
            /*qry.Append($"\n , 품명                                                                                                                          ");
            qry.Append($"\n , 원산지                                                                                                                         ");
            qry.Append($"\n , 규격                                                                                                                          ");*/
            qry.Append($"\n , SUM(ISNULL(매출수, 0)) AS 매출수                                                                                           ");
            qry.Append($"\n , SUM(ISNULL(매출금액, 0)) AS 매출금액                                                                                       ");
            qry.Append($"\n , SUM(ISNULL(매입금액, 0)) AS 매입금액                                                                                               ");
            qry.Append($"\n , SUM(ISNULL(매출금액, 0)) - SUM(ISNULL(매입금액, 0)) AS 마진금액                                                                     ");
            qry.Append($"\n , SUM(매출수 * CONVERT(float, REPLACE(REPLACE(단위, 'kg', ''), ',', '.'))) 단품매출수                                                                       ");
            qry.Append($"\n , CASE WHEN SUM(ISNULL(매출금액, 0)) > 0 THEN  (1 - (SUM(ISNULL(매입금액, 0))/SUM(ISNULL(매출금액, 0)))) * 100 ELSE 0 END AS 마진율      ");
            qry.Append($"\n FROM 매출현황                                                                                                                     ");
            qry.Append($"\n WHERE 사용자 = '200009'                                                                                                          ");
            qry.Append($"\n   AND ISNUMERIC(REPLACE(REPLACE(단위, 'kg', ''), ',', '.')) = 1                                                                                    ");
            qry.Append($"\n   AND 단위 <> '.' AND 단위 <> '-'                                                                                   ");
            qry.Append($"\n   AND 매출단가 > 0                                                                                   ");
            qry.Append($"\n   AND 매출처 NOT LIKE '%아토무역%'                                                                                   ");
            qry.Append($"\n   AND 매출처 NOT LIKE '%에이티오%'                                                                                   ");
            qry.Append($"\n   AND 매출처 NOT LIKE '%아토코리아%'                                                                                   ");
            qry.Append($"\n   AND 매출처 NOT LIKE '%에스제이씨푸드/신영국%'                                                                                   ");

            if (!string.IsNullOrEmpty(sales_type))
                qry.Append($"\n   AND 매출구분 = '{sales_type}'           ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND 매출일자 >= '{sttdate}'                ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND 매출일자 <= '{enddate}'                ");
            

            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND 품명 = '{product}'                     ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   AND 원산지 = '{origin}'                    ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n   AND 규격 = '{sizes}'                       ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n   AND REPLACE(단위, 'kg', '') = '{unit}'     ");
            }
            else
            {
                string[] products = sub_product.Trim().Split('\n');
                if (product.Length > 0)
                {
                    qry.Append($"\n AND (");
                    for (int i = 0; i < products.Length; i++)
                    {
                        string[] sub = products[i].Trim().Split('^');
                        if (sub.Length > 3)
                        {
                            if (i == 0)
                                qry.Append($"\n (");
                            else
                                qry.Append($"\n OR (");


                            qry.Append($" 품명 = '{sub[0]}'");
                            qry.Append($" AND 원산지 = '{sub[1]}'");
                            qry.Append($" AND 규격 = '{sub[2]}'");
                            qry.Append($" AND REPLACE(단위, 'KG', '') = '{sub[6]}'");
                            qry.Append($" )");
                        }
                    }
                    qry.Append($"\n ) ");
                }
            }

            qry.Append($"\n GROUP BY LEFT(CONVERT(varchar, 매출일자,112),6)                                                                     ");
            qry.Append($"\n ORDER BY 매출일자 DESC                                                                                                                  ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
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


    }
}
