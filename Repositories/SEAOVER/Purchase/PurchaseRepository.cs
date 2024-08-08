using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SEAOVER.Purchase
{
    public class PurchaseRepository : ClassRoot, IPurchaseRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        private static string userId;
        public void SetSeaoverId(string user_id)
        {
            userId = user_id;
        }
        public int CallStoredProcedure(string user_id, string sttdate, string enddate)
        {
            userId = user_id;
            int result = 0;

            dbInstance3.Connection.Close();
            SqlCommand cmd = new SqlCommand("SP_매입현황", (SqlConnection)dbInstance3.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 300;

            cmd.Parameters.AddWithValue("@draw_fymd", sttdate);
            cmd.Parameters.AddWithValue("@draw_tymd", enddate);
            cmd.Parameters.AddWithValue("@proc_fish_hnm", "%%");
            cmd.Parameters.AddWithValue("@proc_size_hnm", "%%");
            cmd.Parameters.AddWithValue("@proc_won_hnm", "%%");
            cmd.Parameters.AddWithValue("@work_id", userId);

            result = cmd.ExecuteNonQuery();
            return result;
        }
        public DataTable GetPurchaseIncomProduct(string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                             ");
            qry.Append($"\n   DISTINCT                          ");
            qry.Append($"\n   품명                               ");
            qry.Append($"\n , 원산지                              ");
            qry.Append($"\n , 규격                               ");
            qry.Append($"\n , REPLACE(단위, 'kg', '') AS 단위      ");
            qry.Append($"\n , 품명 + '_' + 원산지 + '_' + 규격 + '_' + REPLACE(단위, 'kg', '') AS 품목코드      ");
            qry.Append($"\n FROM 매입현황                          ");
            qry.Append($"\n WHERE 사용자 = '200009'               ");
            qry.Append($"\n   AND 구분 = '매입'                    ");
            qry.Append($"\n   AND 매입구분 = '수입'              ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("품명", product)}                     ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("원산지", origin)}                     ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("규격", sizes)}                     ");
            qry.Append($"\n		 GROUP BY LEFT(CONVERT(varchar, 매입일자,112),6), 품명, 원산지, 규격, 단위                                                                                                  ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetPurchaseProductList(string sttdate, string enddate, string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n		 SELECT                                                                                                                                                          ");
            qry.Append($"\n		   LEFT(CONVERT(varchar, 매입일자,112),6) AS 매입일자                                                                                                                   ");
            qry.Append($"\n		 , 품명, 원산지, 규격                                                                                                                                                  ");
            qry.Append($"\n		 , REPLACE(CASE WHEN 단위 = '-' OR 단위 = '.' OR 단위 = 'box' OR 단위 = 'b/x' OR 단위 = '박스' OR 단위 = '팩' OR 단위 = '묶음' OR CHARINDEX('부정관', 단위) > 0 THEN 'NULL'           ");
            qry.Append($"\n		 	  WHEN  CHARINDEX('(', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('(', 단위))                                                                                    ");
            qry.Append($"\n		 	  WHEN  CHARINDEX('~', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('~', 단위))                                                                                    ");
            qry.Append($"\n		 	  ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(단위, '내외', ''), '전후', ''), '벌크', ''), '이상', ''), '부정', '')                                                   ");
            qry.Append($"\n		   END, 'kg', '') AS 단위                                                                                                                                         ");
            qry.Append($"\n		 , ISNULL(AVG(CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END), 0) AS 단가                                                                                                    ");
            qry.Append($"\n		 , AVG(CASE WHEN 매입구분 = '국내매입' THEN CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END  ELSE null END) AS 단가2                                                       ");
            qry.Append($"\n		 , AVG(CASE WHEN 매입구분 <> '국내매입' THEN CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END  ELSE null END) AS 단가3                                                            ");
            qry.Append($"\n		 FROM 매입현황                                                                                                                                                      ");
            qry.Append($"\n		 WHERE 사용자 = '200009'                                                                                                                                           ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND 매입일자 >= '{sttdate}'                ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND 매입일자 <= '{enddate}'                ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("품명", product)}                     ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("원산지", origin)}                     ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   {commonRepository.whereSql("규격", sizes)}                     ");
            qry.Append($"\n		 GROUP BY LEFT(CONVERT(varchar, 매입일자,112),6), 품명, 원산지, 규격, 단위                                                                                                  ");
            
            
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPurchaseProduct(string sttdate, string enddate, string product, string origin, string sizes, string unit, string purchase_type, string product_code = "", string sizes_code = "", string select_unit = "")
        {
            

            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                                                      ");
            qry.Append($"\n   p.sdate AS 매입일자                                                                                                                                                                                   ");
            qry.Append($"\n , ISNULL(t.단가, 0) AS 단가, ISNULL(t.단가2, 0) AS 단가2, ISNULL(t.단가3, 0) AS 단가3                                                                                                       ");
            qry.Append($"\n , 0 AS 오퍼가                                                                                                                                           ");
            qry.Append($"\n FROM (                                                                                                                                                                                      ");
            qry.Append($"\n 	{GetYearMonth(sttdate, enddate)}                                                                                                                                                        ");
            qry.Append($"\n ) AS p                                                                                                                                                                                      ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                                                                           ");
            qry.Append($"\n	 SELECT                                                                                                                                                                                     ");
            qry.Append($"\n	   매입일자                                                                                                                                                                                 ");
            qry.Append($"\n	 , AVG(단가 / 단위 * {unit}) AS 단가                                                                                                                                                        ");
            qry.Append($"\n	 , ISNULL(AVG(단가2 / 단위 * {unit}), 0) AS 단가2                                                                                                                                           ");
            qry.Append($"\n	 , ISNULL(AVG(단가3 / 단위 * {unit}), 0) AS 단가3                                                                                                                                           ");
            qry.Append($"\n	 FROM (                                                                                                                                                                                     ");
            qry.Append($"\n		 SELECT                                                                                                                                                                                 ");
            qry.Append($"\n		   매입일자                                                                                                                                                                             ");
            qry.Append($"\n		 , CASE WHEN ISNUMERIC(REPLACE(단위, '.', '')) = 1 THEN 단위 ELSE '1' END AS 단위                                                                                                       ");
            qry.Append($"\n		 , AVG(단가) AS 단가, AVG(단가2) AS 단가2, AVG(단가3) AS 단가3                                                                                                                          ");
            qry.Append($"\n		 FROM (                                                                                                                                                                                 ");
            qry.Append($"\n			 SELECT                                                                                                                                                                             ");
            qry.Append($"\n			   LEFT(CONVERT(varchar, 매입일자,112),6) AS 매입일자                                                                                                                               ");
            qry.Append($"\n			 , REPLACE(REPLACE(CASE WHEN 단위 = '-' OR 단위 = '.' OR 단위 = 'box' OR 단위 = 'b/x' OR 단위 = '박스' OR 단위 = '팩' OR 단위 = '묶음' OR CHARINDEX('부정관', 단위) > 0 THEN 'null'       ");
            qry.Append($"\n			 	  WHEN  CHARINDEX('(', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('(', 단위))                                                                                                  ");
            qry.Append($"\n			 	  WHEN  CHARINDEX('~', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('~', 단위))                                                                                                  ");
            qry.Append($"\n			 	  ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(단위, '내외', ''), '전후', ''), '벌크', ''), '이상', ''), '부정', '')                                                            ");
            qry.Append($"\n			   END, 'kg', ''), '..', '.') AS 단위                                                                                                                                                           ");
            qry.Append($"\n			 , CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END AS 단가                                                                                                                                                                            ");
            qry.Append($"\n			 , CASE WHEN 매입구분 = '국내매입' THEN CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END  ELSE null END AS 단가2                                                                                                     ");
            qry.Append($"\n			 , CASE WHEN 매입구분 <> '국내매입' THEN CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END  ELSE null END AS 단가3                                                                                                    ");
            qry.Append($"\n			 FROM 매입현황                                                                                                                                                                      ");
            qry.Append($"\n			 WHERE 사용자 = '200009'                                                                                                                                                            ");
            if (!string.IsNullOrEmpty(purchase_type))
                qry.Append($"\n   AND 매입구분 = '{purchase_type}'           ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND 매입일자 >= '{sttdate}'                ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND 매입일자 <= '{enddate}'                ");
            
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND 원산지 = '{origin}'                    ");
            /*if (!string.IsNullOrEmpty(sizes))
            {
                if (product.Contains("쭈꾸미 200G*12") || product.Contains("쭈꾸미 180G*12"))
                {
                    if (sizes.Contains("6미"))
                        qry.Append($"\n   AND (규격 LIKE '%6미%' OR 규격 LIKE '%80/100%')                      ");
                    else if (sizes.Contains("8미"))
                        qry.Append($"\n   AND (규격 LIKE '%8미%' OR 규격 LIKE '%50/80%')                      ");
                    else if (sizes.Contains("10미"))
                        qry.Append($"\n   AND (규격 LIKE '%10미%' OR 규격 LIKE '%30/50%')                      ");
                    else if (sizes.Contains("15미"))
                        qry.Append($"\n   AND (규격 LIKE '%15미%' OR 규격 LIKE '%20/30%')                      ");
                    else
                        qry.Append($"\n   AND 규격 = '{sizes}'                       ");
                }
                if (product.Contains("해물모듬"))
                {
                    if (sizes.Contains("600g*10") || sizes.Contains("600G*10"))
                        qry.Append($"\n   AND 규격 LIKE '%600g*10%'                      ");
                    else if (sizes.Contains("700g*10") || sizes.Contains("700G*10"))
                        qry.Append($"\n   AND 규격 LIKE '%700g*10%'                      ");
                    else
                        qry.Append($"\n   AND 규격 = '{sizes}'                       ");
                }
                else
                {
                    qry.Append($"\n   AND 규격 = '{sizes}'                       ");
                }
                
            }*/

            if (string.IsNullOrEmpty(product_code) && string.IsNullOrEmpty(sizes_code))
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND 품명 = '{product}'                     ");
                if (!string.IsNullOrEmpty(sizes))
                {
                    if (product.Contains("쭈꾸미 200G*12") || product.Contains("쭈꾸미 180G*12"))
                    {
                        if (sizes.Contains("6미"))
                            qry.Append($"\n   AND (규격 LIKE '%6미%' OR 규격 LIKE '%80/100%')                      ");
                        else if (sizes.Contains("8미"))
                            qry.Append($"\n   AND (규격 LIKE '%8미%' OR 규격 LIKE '%50/80%')                      ");
                        else if (sizes.Contains("10미"))
                            qry.Append($"\n   AND (규격 LIKE '%10미%' OR 규격 LIKE '%30/50%')                      ");
                        else if (sizes.Contains("15미"))
                            qry.Append($"\n   AND (규격 LIKE '%15미%' OR 규격 LIKE '%20/30%')                      ");
                        else
                            qry.Append($"\n   AND 규격 = '{sizes}'                       ");
                    }
                    else if (product.Contains("해물모듬"))
                    {
                        if (sizes.Contains("600g*10") || sizes.Contains("600G*10"))
                            qry.Append($"\n   AND 규격 LIKE '%600g*10%'                      ");
                        else if (sizes.Contains("700g*10") || sizes.Contains("700G*10"))
                            qry.Append($"\n   AND 규격 LIKE '%700g*10%'                      ");
                        else
                            qry.Append($"\n   AND 규격 = '{sizes}'                       ");
                    }
                    else
                    {
                        //qry.Append($"\n   AND 규격 = '{sizes}'                       ");
                        qry.Append($"\n   AND 규격 LIKE '%{sizes}%'                       ");
                    }        
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(product_code))
                    qry.Append($"\n   AND 품명코드 = '{product_code}'                     ");
                if (!string.IsNullOrEmpty(sizes_code))
                    qry.Append($"\n   AND 규격코드 = '{sizes_code}'                     ");
            }


            qry.Append($"\n		 ) AS t                                                                                                                                                            ");
            qry.Append($"\n		 WHERE ISNUMERIC(REPLACE(단위, '.', '')) = 1                                                                                                                         ");
            qry.Append($"\n		   AND REPLACE(단위, '.', '') != '0'                                                                                                                            ");
            if (!string.IsNullOrEmpty(select_unit))
                qry.Append($"\n   AND t.단위 = '{select_unit}'           ");
            qry.Append($"\n		 GROUP BY 매입일자, 단위                                                                                                                                                ");
            qry.Append($"\n	 ) AS t                                                                                                                                                                ");
            qry.Append($"\n	 GROUP BY 매입일자                                                                                                                                                         ");
            qry.Append($"\n ) AS t                                                                                                                                                                 ");
            qry.Append($"\n   ON p.sdate = t.매입일자                                                                                                                                                  ");
            qry.Append($"\n ORDER BY 매입일자 DESC, 단가 ASC                                                                                                                                                      ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPurchaseProductForDashboard(string sttdate, string enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge, string purchase_type)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                                                      ");
            qry.Append($"\n   p.sdate AS 매입일자                                                                                                                                                                       ");
            qry.Append($"\n , ISNULL(t.단가, 0) AS 단가, ISNULL(t.단가2, 0) AS 단가2, ISNULL(t.단가3, 0) AS 단가3                                                                                                       ");
            qry.Append($"\n , 0 AS 환율                                                                                                                                                ");
            qry.Append($"\n , 0 AS 오퍼가                                                                                                                                              ");
            qry.Append($"\n , 10 AS division                                                                                                                                           ");
            qry.Append($"\n FROM (                                                                                                                                                                                      ");
            qry.Append($"\n 	{GetYearMonth(sttdate, enddate)}                                                                                                                                                        ");
            qry.Append($"\n ) AS p                                                                                                                                                                                      ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                                                                           ");
            qry.Append($"\n	 SELECT                                                                                                                                                                                     ");
            qry.Append($"\n	   매입일자                                                                                                                                                                                 ");

            if (string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n	 , AVG(단가) AS 단가                                                                                                                                                        ");
                qry.Append($"\n	 , ISNULL(AVG(단가2), 0) AS 단가2                                                                                                                                           ");
                qry.Append($"\n	 , ISNULL(AVG(단가3), 0) AS 단가3                                                                                                                                           ");
            }
            else if (unit != "0")
            {
                qry.Append($"\n	 , AVG(단가 / 단위 * {unit}) AS 단가                                                                                                                                                        ");
                qry.Append($"\n	 , ISNULL(AVG(단가2 / 단위 * {unit}), 0) AS 단가2                                                                                                                                           ");
                qry.Append($"\n	 , ISNULL(AVG(단가3 / 단위 * {unit}), 0) AS 단가3                                                                                                                                           ");
            }
            else 
            {
                qry.Append($"\n	 , AVG(단가 / 단위 ) AS 단가                                                                                                                                                        ");
                qry.Append($"\n	 , ISNULL(AVG(단가2 / 단위 ), 0) AS 단가2                                                                                                                                           ");
                qry.Append($"\n	 , ISNULL(AVG(단가3 / 단위 ), 0) AS 단가3                                                                                                                                           ");
            }
            qry.Append($"\n	 FROM (                                                                                                                                                                                     ");
            qry.Append($"\n		 SELECT                                                                                                                                                                                 ");
            qry.Append($"\n		   매입일자                                                                                                                                                                             ");
            qry.Append($"\n		 , CASE WHEN ISNUMERIC(REPLACE(단위, '.', '')) = 1 THEN 단위 ELSE '1' END AS 단위                                                                                                       ");
            qry.Append($"\n		 , AVG(단가) AS 단가, AVG(단가2) AS 단가2, AVG(단가3) AS 단가3                                                                                                                          ");
            qry.Append($"\n		 FROM (                                                                                                                                                                                 ");
            qry.Append($"\n			 SELECT                                                                                                                                                                             ");
            qry.Append($"\n			   LEFT(CONVERT(varchar, 매입일자,112),6) AS 매입일자                                                                                                                               ");
            qry.Append($"\n			 , REPLACE(CASE WHEN 단위 = '-' OR 단위 = '.' OR 단위 = 'box' OR 단위 = 'b/x' OR 단위 = '박스' OR 단위 = '팩' OR 단위 = '묶음' OR CHARINDEX('부정관', 단위) > 0 THEN 'null'       ");
            qry.Append($"\n			 	  WHEN  CHARINDEX('(', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('(', 단위))                                                                                                  ");
            qry.Append($"\n			 	  WHEN  CHARINDEX('~', 단위) > 0 THEN SUBSTRING(단위, 0, CHARINDEX('~', 단위))                                                                                                  ");
            qry.Append($"\n			 	  ELSE REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(단위, '내외', ''), '전후', ''), '벌크', ''), '이상', ''), '부정', '')                                                            ");
            qry.Append($"\n			   END, 'kg', '') AS 단위                                                                                                                                                           ");
            qry.Append($"\n			 , CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END AS 단가                                                                                                                                                                            ");
            qry.Append($"\n			 , CASE WHEN 매입구분 = '국내매입' THEN CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END  ELSE null END AS 단가2                                                                                                     ");
            qry.Append($"\n			 , CASE WHEN 매입구분 <> '국내매입' THEN CASE WHEN 단가 = 0 THEN NULL ELSE 단가 END  ELSE null END AS 단가3                                                                                                    ");
            qry.Append($"\n			 FROM 매입현황                                                                                                                                                                      ");
            qry.Append($"\n			 WHERE 사용자 = '200009'                                                                                                                                                            ");
            qry.Append($"\n			   AND 본선명 NOT LIKE '%BL%'                                                                                                                                                            ");
            qry.Append($"\n			   AND 본선명 NOT LIKE '%B/L%'                                                                                                                                                            ");
            qry.Append($"\n			   AND 매입처 NOT LIKE '%아토무역%'                                                                                                                                                            ");
            qry.Append($"\n			   AND 매입처 NOT LIKE '%에이티오%'                                                                                                                                                            ");
            qry.Append($"\n			   AND 매입처 NOT LIKE '%아토코리아%'                                                                                                                                                            ");
            qry.Append($"\n			   AND 매입처 NOT LIKE '%에스제이씨푸드/신영국%'                                                                                                                                                            ");


            if (!string.IsNullOrEmpty(purchase_type))
                qry.Append($"\n   AND 매입구분 = '{purchase_type}'           ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND 매입일자 >= '{sttdate}'                ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND 매입일자 <= '{enddate}'                ");
            if (!isMerge)
            {
                if (!string.IsNullOrEmpty(product))
                    qry.Append($"\n   AND 품명 = '{product}'                ");
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   AND 원산지 = '{origin}'                ");
                if (!string.IsNullOrEmpty(sizes))
                    qry.Append($"\n   AND 규격 = '{sizes}'                ");
                if (!string.IsNullOrEmpty(unit))
                    qry.Append($"\n   AND REPLACE(단위, 'KG', '') = '{unit}'                ");
            }
            else if (sub_product != null && !string.IsNullOrEmpty(sub_product))
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
                        if (product.Length > 1)
                        {
                            qry.Append($"품명 = '{sub[0]}' ");
                            qry.Append($"AND 원산지 = '{sub[1]}' ");
                            qry.Append($"AND 규격 = '{sub[2]}' ");
                            qry.Append($"AND REPLACE(단위, 'KG', '') = '{sub[6]}' ");
                            qry.Append($") ");
                        }
                    }
                    qry.Append($"\n)");
                }
            }
                
            qry.Append($"\n		 ) AS t                                                                                                                                                            ");
            qry.Append($"\n		 WHERE ISNUMERIC(REPLACE(단위, '.', '')) = 1                                                                                                                         ");
            
            qry.Append($"\n		 GROUP BY 매입일자, 단위                                                                                                                                                ");
            qry.Append($"\n	 ) AS t                                                                                                                                                                ");
            qry.Append($"\n	 GROUP BY 매입일자                                                                                                                                                         ");
            qry.Append($"\n ) AS t                                                                                                                                                                 ");
            qry.Append($"\n   ON p.sdate = t.매입일자                                                                                                                                                  ");
            qry.Append($"\n ORDER BY 매입일자 DESC, 단가 ASC                                                                                                                                                      ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetPurchase(string sttdate, string enddate, string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                 ");
            qry.Append($"\n   매입일자                                                                                                 ");
            qry.Append($"\n , 매입구분                                                                                                 ");
            qry.Append($"\n , 매입처                                                                                                  ");
            qry.Append($"\n , 보관처                                                                                                  ");
            qry.Append($"\n , 적요                                                                                                   ");
            qry.Append($"\n , 단위                                                                                                   ");
            qry.Append($"\n , 수량 * CONVERT(float, 단위) / {unit} AS 수량                                                                  ");
            qry.Append($"\n , 단가 * CONVERT(float, 단위) / {unit} AS 단가                                                                  ");
            qry.Append($"\n , 총매입금액 * CONVERT(float, 단위) / {unit} AS 총매입금액                                                            ");
            qry.Append($"\n FROM(                                                                                                  ");
            qry.Append($"\n 	SELECT                                                                                             ");
            qry.Append($"\n 	  매입일자                                                                                             ");
            qry.Append($"\n 	, 매입구분                                                                                             ");
            qry.Append($"\n 	, 매입처                                                                                              ");
            qry.Append($"\n 	, 보관처                                                                                              ");
            qry.Append($"\n 	, 적요                                                                                               ");
            qry.Append($"\n 	, CASE WHEN 단위 = '.' OR 단위 = '-' OR 단위 = '\' THEN '1' ELSE REPLACE(단위, 'kg', '') END AS 단위        ");
            qry.Append($"\n 	, 수량                                                                                               ");
            qry.Append($"\n 	, 단가                                                                                               ");
            qry.Append($"\n 	, 총매입금액                                                                                           ");
            qry.Append($"\n 	FROM 매입현황                                                                                          ");
            qry.Append($"\n 	WHERE 사용자 = '200009'                                                                               ");
            qry.Append($"\n 	  AND 매입일자 >= '{sttdate}'                                                                         ");
            qry.Append($"\n 	  AND 매입일자 <= '{enddate}'                                                                         ");
            qry.Append($"\n 	  AND 품명 = '{product}'                                                                            ");
            qry.Append($"\n 	  AND 원산지 = '{origin}'                                                                                 ");
            qry.Append($"\n 	  AND 규격 = '{sizes}'                                                                               ");
            qry.Append($"\n ) AS t                                                                                                 ");
            qry.Append($"\n WHERE 1=1                                                                                              ");
            qry.Append($"\n   AND ISNUMERIC(단위) = 1                                                                                ");
            qry.Append($"\n ORDER BY 매입일자 DESC                                                                                                                                                      ");

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

        public DataTable GetExchangeRate(string sttdate, string enddate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                 ");
            qry.Append($"\n   DATE_FORMAT(base_date, '%Y%m') AS base_date          ");
            qry.Append($"\n , AVG(exchange_rate) AS exchange_rate                  ");
            qry.Append($"\n FROM t_finance                                         ");
            qry.Append($"\n WHERE 1=1                                              ");
            if(!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND base_date >= '{sttdate}'     ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND base_date <= '{enddate}'     ");
            qry.Append($"\n GROUP BY DATE_FORMAT(base_date, '%Y%m')                ");

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
