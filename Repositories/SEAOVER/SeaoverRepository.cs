using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class SeaoverRepository : ClassRoot, ISeaoverRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        /*private string userId = "200009";*/
        private static string userId;

        public int UpdateFormDataRock(string id, bool is_rock, string password)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_form_data SET            ");
            qry.Append($"   is_rock = {is_rock}             ");
            qry.Append($" , password = '{password}'         ");
            qry.Append($" WHERE id = {id}                ");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }


        public int CallStoredProcedure(string user_id, string sttdate, string enddate)
        {
            userId = user_id;
            int result = 0;

            SqlCommand cmd = new SqlCommand("SP_업체별시세관리", (SqlConnection)dbInstance3.Connection);
            cmd.CommandType = CommandType.StoredProcedure;

            /*            cmd.Parameters.AddWithValue("@seek_fymd", sttdate);
                        cmd.Parameters.AddWithValue("@seek_tymd", enddate);
                        cmd.Parameters.AddWithValue("@item_hnm", "%dfasdfasdf%");
                        cmd.Parameters.AddWithValue("@fish_hnm", "%asdfasdf%");
                        cmd.Parameters.AddWithValue("@size_hnm", "%asdfasdfasd%");
                        cmd.Parameters.AddWithValue("@won_hnm", "%asdfasdfa%");
                        cmd.Parameters.AddWithValue("@id1_hnm", "%asdfasdfas%");
                        cmd.Parameters.AddWithValue("@id2_hnm", "%asdfasdfa%");
                        cmd.Parameters.AddWithValue("@id3_hnm", "%fffdfd%");
                        cmd.Parameters.AddWithValue("@work_id", user_id);

                        result = cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("SP_업체별시세관리", (SqlConnection)dbInstance3.Connection);
                        cmd.CommandType = CommandType.StoredProcedure;*/

            cmd.Parameters.AddWithValue("@seek_fymd", sttdate);
            cmd.Parameters.AddWithValue("@seek_tymd", enddate);
            cmd.Parameters.AddWithValue("@item_hnm", "%%");
            cmd.Parameters.AddWithValue("@fish_hnm", "%%");
            cmd.Parameters.AddWithValue("@size_hnm", "%%");
            cmd.Parameters.AddWithValue("@won_hnm", "%%");
            cmd.Parameters.AddWithValue("@id1_hnm", "%%");
            cmd.Parameters.AddWithValue("@id2_hnm", "%%");
            cmd.Parameters.AddWithValue("@id3_hnm", "%%");
            cmd.Parameters.AddWithValue("@work_id", user_id);

            result = cmd.ExecuteNonQuery();
            return result;
        }

        public DataTable SelectData(string db_name, string col, string user_id, string[] whrCol, string[] whrVal, string ord)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                     ");
            qry.Append($"\n   {col}                                                                                    ");
            qry.Append($"\n FROM {db_name}                                                                             ");
            qry.Append($"\n WHERE 1=1                                                                                  ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n   AND 사용자 = '{user_id}'                                                                 ");
            if (whrCol.Length > 0 && whrVal.Length > 0)
            {
                for (int i = 0; i < whrCol.Length; i++)
                    qry.Append($"\n   {commonRepository.whereSql(whrCol[i], whrVal[i])}                                ");
            }
            if (!string.IsNullOrEmpty(ord))
                qry.Append($"\n   ORDER BY {ord}                                                                       ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable SelectData(string db_name, string col, string user_id, string whr, string ord)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                     ");
            qry.Append($"\n   {col}                                                                                    ");
            qry.Append($"\n FROM {db_name}                                                                             ");
            qry.Append($"\n WHERE 1=1                                                                                  ");
            if (!string.IsNullOrEmpty(user_id))
            qry.Append($"\n   AND 사용자 = '{user_id}'                                                                 ");
            qry.Append($"\n   AND {whr}                                                                                ");
            if (!string.IsNullOrEmpty(ord))
                qry.Append($"\n   ORDER BY {ord}                                                                       ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductByCode(string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                            ");
            qry.Append($"\n   *                                                                                               ");
            qry.Append($"\n FROM 업체별시세관리                                                                               ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                         ");
           
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND 품목코드 = '{product}'                                                                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND 원산지코드 = '{origin}'                                                                     ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND 규격코드 = '{sizes}'                                                                        ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND SEAOVER단위 = '{unit}'                                                          ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductInfo(string sttdate, string enddate, string division, string product, string origin, string sizes, string seaover_unit, string warehouse, string purchase_company, bool is_exactly = true)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                            ");
            qry.Append($"\n   구분                                                                                               ");
            qry.Append($"\n , 대분류                                                                                               ");
            qry.Append($"\n , 품명                                                                                               ");
            qry.Append($"\n , 품목코드                                                                                               ");
            qry.Append($"\n , 원산지                                                                                               ");
            qry.Append($"\n , 원산지코드                                                                                               ");
            qry.Append($"\n , 규격                                                                                               ");
            qry.Append($"\n , 규격코드                                                                                               ");
            qry.Append($"\n , 단위                                                                                             ");
            qry.Append($"\n , 단위수량                                                                                             ");
            qry.Append($"\n , 가격단위                                                                                             ");
            qry.Append($"\n , 묶음수                                                                                             ");
            qry.Append($"\n , SEAOVER단위                                                                                             ");
            qry.Append($"\n , 매출단가 * ISNULL(단위수량,1) * ISNULL(묶음수,1) AS 매출단가                                                                                              ");
            qry.Append($"\n , CASE WHEN  매입처 LIKE '%재고%' AND ISNULL(매입단가, 1) = 1 THEN 매입단가 ELSE 매입단가 * ISNULL(단위수량,1) * ISNULL(묶음수,1) END AS 매입단가                                                                                            ");

            qry.Append($"\n , 매입처                                                                                              ");
            qry.Append($"\n , 보관처                                                                                              ");
            qry.Append($"\n , 매입일자                                                                                              ");
            qry.Append($"\n , 비고1                                                                                              ");
            qry.Append($"\n , 비고2                                                                                              ");
            qry.Append($"\n , 담당자1                                                                                              ");
            qry.Append($"\n , 담당자2                                                                                             ");
            //qry.Append($"\n , 비고3                                                                                              ");
            qry.Append($"\n FROM 업체별시세관리                                                                               ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                         ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND 매입일자 >= '{sttdate}'                                                                      ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND 매입일자 <= '{enddate}'                                                                      ");
            //qry.Append($"\n   AND 매입단가 > 10                                                                               ");
            if (!string.IsNullOrEmpty(division))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 구분 = '{division}'                                                                      ");
                else
                    qry.Append($"\n   {commonRepository .whereSql("구분", division)}                                                                      ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 품명 = '{product}'                                                                      ");
                else
                    qry.Append($"\n   {commonRepository .whereSql("품명", product)}                                                                      ");
            }

            if (!string.IsNullOrEmpty(origin))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 원산지 = '{origin}'                                                                     ");
                else
                    qry.Append($"\n   {commonRepository .whereSql("원산지", origin)}                                                                      ");
            }

            if (!string.IsNullOrEmpty(sizes))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 규격 = '{sizes}'                                                                        ");
                else
                    qry.Append($"\n   {commonRepository .whereSql("규격", sizes)}                                                                      ");
            }

            if (!string.IsNullOrEmpty(seaover_unit))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 단위 = '{seaover_unit}'                                                          ");
                else
                    qry.Append($"\n   {commonRepository .whereSql("단위", seaover_unit)}                                                                      ");
            }

            if (!string.IsNullOrEmpty(warehouse))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 보관처 = '{warehouse}'                                                          ");
                else
                    qry.Append($"\n   {commonRepository .whereSql("보관처", warehouse)}                                                                      ");
            }

            if (!string.IsNullOrEmpty(purchase_company))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 매입처 = '{purchase_company}'                                                          ");
                else
                    qry.Append($"\n   {commonRepository .whereSql("매입처", purchase_company)}                                                                      ");
            }

            qry.Append($"\n  ORDER BY CASE WHEN 매입단가 IS NULL THEN 2 ELSE 1 END, 매입단가 ASC, CASE WHEN 매입처 LIKE '%재고%' THEN 1 ELSE 2 END, 매입일자 DESC                                                         ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetAllData(string sttdate, string enddate, string product, string origin, string sizes, string seaover_unit, bool is_exactly = true)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                            ");
            qry.Append($"\n   *                                                                                               ");
            qry.Append($"\n FROM 업체별시세관리                                                                               ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                         ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND 매입일자 >= '{sttdate}'                                                                      ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND 매입일자 <= '{enddate}'                                                                      ");
            //qry.Append($"\n   AND 매입단가 > 10                                                                               ");
            if (!string.IsNullOrEmpty(product))
            { 
                if(is_exactly)
                    qry.Append($"\n   AND 품명 = '{product}'                                                                      ");
                else
                    qry.Append($"\n   AND 품명 LIKE '%{product}%'                                                                      ");
            }

            if (!string.IsNullOrEmpty(origin))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 원산지 = '{origin}'                                                                     ");
                else
                    qry.Append($"\n   AND 원산지 LIKE '%{origin}%'                                                                     ");
            }

            if (!string.IsNullOrEmpty(sizes))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 규격 = '{sizes}'                                                                        ");
                else
                    qry.Append($"\n   AND 규격 LIKE '%{sizes}%'                                                                        ");
            }

            if (!string.IsNullOrEmpty(seaover_unit))
            {
                if (is_exactly)
                    qry.Append($"\n   AND 단위 = '{seaover_unit}'                                                          ");
                else
                    qry.Append($"\n   AND 단위 LIKE '{seaover_unit}%'                                                          ");
            }
                
            qry.Append($"\n  ORDER BY CASE WHEN 매입단가 IS NULL THEN 2 ELSE 1 END, 매입단가 ASC, CASE WHEN 매입처 LIKE '%재고%' THEN 1 ELSE 2 END, 매입일자 DESC                                                         ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetAllDataOrderBy(string sttdate, string enddate, string product, string origin)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                   ");
            qry.Append($"\n   t.품명                                                                                                                 ");
            qry.Append($"\n , t.원산지                                                                                                               ");
            qry.Append($"\n , t.규격                                                                                                                 ");
            qry.Append($"\n , ISNULL(t.규격2, '') AS 규격2                                                                                           ");
            qry.Append($"\n , ISNULL(t.규격3, 0) AS 규격3                                                                                            ");
            qry.Append($"\n , ISNULL(t.규격4, '') AS 규격4                                                                                           ");
            qry.Append($"\n , t.단위                                                                                                                 ");
            qry.Append($"\n , t.가격단위                                                                                                             ");
            qry.Append($"\n , t.단위수량                                                                                                             ");
            qry.Append($"\n , t.SEAOVER단위                                                                                                          ");
            qry.Append($"\n,  t.품명 + '^' + t.원산지 + '^' + t.규격 + '^' + t.SEAOVER단위  AS 품목코드                                              ");
            if (!string.IsNullOrEmpty(product) && !string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n, CASE WHEN t.품명 = '{product}' AND t.원산지 = '{origin}' THEN 0           ");
                qry.Append($"\n       WHEN t.품명 LIKE '%{product}%' AND t.원산지 = '{origin}' THEN 1      ");
                qry.Append($"\n       WHEN t.품명 LIKE '%{product}%' THEN 2                         ");
                qry.Append($"\n       ELSE 3 END sortIdx                                        ");
            }
            qry.Append($"\n , CAST(ISNULL(t2.통관, 0)  AS float) AS 통관                         ");
            qry.Append($"\n , CAST(ISNULL(t2.미통관, 0) AS float)  AS 미통관                       ");
            qry.Append($"\n , CAST(ISNULL(t2.예약수, 0) AS float)  AS 예약수                       ");
            qry.Append($"\n , CAST(ISNULL(t2.매출수, 0) AS float)  AS 매출수                       ");

            qry.Append($"\n FROM (                                                                                                                   ");
            qry.Append($"\n 	SELECT                                                                                                               ");
            qry.Append($"\n 	 t.*                                                                                                                 ");
            qry.Append($"\n 	, CASE                                                                                                               ");
            qry.Append($"\n 	     WHEN t.규격2 = '.' THEN NULL                                                                                    ");
            qry.Append($"\n 		  WHEN t.규격2 = '-' THEN NULL                                                                                   ");
            qry.Append($"\n 		  WHEN ISNUMERIC(t.규격2) = 1 THEN CONVERT(float, t.규격2)                                                       ");
            qry.Append($"\n 		  ELSE NULL                                                                                                      ");
            qry.Append($"\n 	 END AS 규격3                                                                                                        ");
            qry.Append($"\n 	, CASE                                                                                                               ");
            qry.Append($"\n 		  WHEN t.규격2 = '.' THEN t.규격2                                                                                ");
            qry.Append($"\n 		  WHEN t.규격2 = '-' THEN t.규격2                                                                                ");
            qry.Append($"\n 		  WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                          ");
            qry.Append($"\n 		  ELSE t.규격2                                                                                                   ");
            qry.Append($"\n 	 END AS 규격4                                                                                                        ");
            qry.Append($"\n 	FROM(                                                                                                                ");
            //qry.Append($"\n 		 SELECT TOP 1000                                                                                                 ");
            qry.Append($"\n 		 SELECT                                                                                                          ");
            qry.Append($"\n 		   ISNULL(원산지, '') AS 원산지                                                                                  ");
            qry.Append($"\n 		 , ISNULL(품명, '') AS 품명                                                                                      ");
            qry.Append($"\n 		 , ISNULL(규격, '') AS 규격                                                                                      ");
            qry.Append($"\n 		 , CASE                                                                                                          ");
            qry.Append($"\n 		  WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                               ");
            qry.Append($"\n 		  	CASE                                                                                                         ");
            qry.Append($"\n 			  	WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))         ");
            qry.Append($"\n 			  	WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격))        ");
            qry.Append($"\n 		  	END                                                                                                          ");
            qry.Append($"\n 		  WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                  ");
            qry.Append($"\n 		  WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                    ");
            qry.Append($"\n 		  WHEN CHARINDEX('UP', 규격) > 0 THEN  SUBSTRING(규격, 0, CHARINDEX('UP', 규격))                                 ");
            qry.Append($"\n 		  ELSE 규격 END AS 규격2                                                                                         ");
            qry.Append($"\n 		 , ISNULL(단위, '') AS 단위                                                                        ");
            qry.Append($"\n 		 , ISNULL(가격단위, '') AS 가격단위                                                                        ");
            qry.Append($"\n 		 , CAST(ISNULL(단위수량, '1') AS CHAR) AS 단위수량                                                                        ");
            qry.Append($"\n 		 , ISNULL(SEAOVER단위, '') AS SEAOVER단위                                                                        ");
            qry.Append($"\n 		 FROM 업체별시세관리                                                                                             ");
            qry.Append($"\n 		 WHERE 사용자 = '{userId}'                                                                                       ");
            qry.Append($"\n 	) AS t                                                                                                          ");
            qry.Append($"\n 	GROUP BY t.원산지, t.품명, t.규격, t.규격2, t.SEAOVER단위, t.단위, t.가격단위, t.단위수량                                     ");
            qry.Append($"\n ) AS t                                                                                                              ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                      ");
            qry.Append($"\n	SELECT                                                                                                                                 ");
            qry.Append($"\n	  품명                                                                                                                                  ");
            qry.Append($"\n	, 원산지                                                                                                                                 ");
            qry.Append($"\n	, 규격                                                                                                                                  ");
            qry.Append($"\n	, 단위                                                                                                                                  ");
            qry.Append($"\n	, SUM(통관)  AS 통관                                                                                                                      ");
            qry.Append($"\n	, SUM(미통관) AS 미통관                                                                                                                     ");
            qry.Append($"\n	, SUM(예약수) AS 예약수                                                                                                                     ");
            qry.Append($"\n	, SUM(매출수) AS 매출수                                                                                                                     ");
            qry.Append($"\n	FROM(                                                                                                                                  ");
            qry.Append($"\n		SELECT                                                                                                                             ");
            qry.Append($"\n		  품명                                                                                                                              ");
            qry.Append($"\n		, 원산지                                                                                                                             ");
            qry.Append($"\n		, 규격                                                                                                                              ");
            qry.Append($"\n		, CASE WHEN 단위 = '.' OR 단위 = '-' THEN null ELSE REPLACE(단위, 'kg', '') END AS 단위                                                  ");
            qry.Append($"\n		, CASE WHEN 통관 = '통관' THEN ISNULL(재고수, 0) ELSE 0 END AS 통관                                                                       ");
            qry.Append($"\n		, CASE WHEN 통관 <> '통관' THEN ISNULL(재고수, 0) ELSE 0 END AS 미통관                                                                     ");
            qry.Append($"\n		, ISNULL(예약수 , 0) AS 예약수                                                                                                          ");
            qry.Append($"\n		, 0 AS 매출수                                                                                                                        ");
            qry.Append($"\n		FROM 품명별재고현황                                                                                                                     ");
            qry.Append($"\n		WHERE 사용자 = '{userId}'                                                                                                              ");
            qry.Append($"\n		    AND ((적요 NOT LIKE '%제외%' AND 적요 NOT LIKE '%back%' AND 적요 NOT LIKE '%백쉽%' AND 적요 NOT LIKE '%빽쉽%' ) OR 적요 IS NULL)           ");
            qry.Append($"\n		UNION ALL                                                                                                                          ");
            qry.Append($"\n		SELECT                                                                                                                             ");
            qry.Append($"\n		  품명                                                                                                                              ");
            qry.Append($"\n		, 원산지                                                                                                                             ");
            qry.Append($"\n		, 규격                                                                                                                              ");
            qry.Append($"\n		, CASE WHEN 단위 = '.' OR 단위 = '-' THEN null ELSE REPLACE(단위, 'kg', '') END AS 단위                                                  ");
            qry.Append($"\n		, 0 AS 통관                                                                                                                         ");
            qry.Append($"\n		, 0 AS 미통관                                                                                                                        ");
            qry.Append($"\n		, 0 AS 예약수                                                                                                                        ");
            qry.Append($"\n		, ISNULL(매출수, 0) AS 매출수                                                                                                           ");
            qry.Append($"\n		FROM 매출현황                                                                                                                         ");
            qry.Append($"\n		WHERE 사용자 = '200009'                                                                                                              ");
            qry.Append($"\n 	      AND 매출자 <> '199990'                                                                 ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%아토무역%'       )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%아토코리아%'      )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%에이티오%'       )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%에스제이씨푸드/신영국%')                                                       ");
            qry.Append($"\n 	  AND 매출일자 >= '{sttdate}' AND 매출일자 <= '{enddate}'                                  ");
            qry.Append($"\n	) AS t                                                                                                                                 ");
            qry.Append($"\n	GROUP BY 품명, 원산지, 규격, 단위                                                                                                             ");
            qry.Append($"\n ) AS t2                                                                                                                                ");
            qry.Append($"\n   ON t.품명 = t2.품명                                                                                                                     ");
            qry.Append($"\n   AND t.원산지 = t2.원산지                                                                                                                  ");
            qry.Append($"\n   AND t.규격 = t2.규격                                                                                                                    ");
            qry.Append($"\n   AND t.SEAOVER단위  = t2.단위                                                                                                            ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetAllData(string product, string origin, string sizes, string unit, string seaover_unit, string manager1, string manager2, string division)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                            ");
            qry.Append($"\n *                                                                                            ");
            qry.Append($"\n FROM (                                                                                            ");

            qry.Append($"\n SELECT                                                                                            ");
            qry.Append($"\n  DISTINCT 품명, 원산지, 규격, 단위                                                                        ");
            qry.Append($"\n , ISNULL(가격단위, '') AS 가격단위                                                                ");
            qry.Append($"\n , CAST(ISNULL(단위수량, 1) AS CHAR) AS 단위수량                                                                ");
            qry.Append($"\n , ISNULL(SEAOVER단위, '') AS SEAOVER단위                                                          ");
            qry.Append($"\n , 적요, 단가수정일                                                                                ");
            /*qry.Append($"\n , ISNULL(매입단가, 0) AS 매입단가                                                                ");
            qry.Append($"\n , ISNULL(매출단가, 0) AS 매출단가                                                                ");*/
            qry.Append($"\n , ISNULL(담당자1, '') AS 담당자1                                                                ");
            qry.Append($"\n , ISNULL(담당자2, '') AS 담당자2                                                                ");
            qry.Append($"\n , ISNULL(구분, '') AS 구분                                                                ");
            //qry.Append($"\n , 비고1, 비고2, 부가세유무                                                                ");
            qry.Append($"\n , 품명 + '^' + 원산지 + '^' + 규격 + '^' + SEAOVER단위 AS 품목코드                              ");
            qry.Append($"\n , ROW_NUMBER() OVER(PARTITION BY 품명, 원산지, 규격, 단위, ISNULL(가격단위, ''),CAST(ISNULL(단위수량, 1) AS CHAR), ISNULL(SEAOVER단위, '') ORDER BY 단가수정일 DESC )  AS rowidx             ");
            qry.Append($"\n FROM 업체별시세관리                                                                               ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                         ");
            
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND 품명 LIKE '%{product}%'                                                                      ");

            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND 원산지 LIKE '%{origin}%'                                                                     ");

            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND 규격 LIKE '%{sizes}%'                                                                        ");

            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n   AND 단위 LIKE '{unit}%'                                                          ");

            if (!string.IsNullOrEmpty(seaover_unit))
                qry.Append($"\n   AND SEAOVER단위 LIKE '{seaover_unit}%'                                                          ");

            if (!string.IsNullOrEmpty(manager1))
                qry.Append($"\n   AND 담당자1 LIKE '{manager1}%'                                                          ");

            if (!string.IsNullOrEmpty(manager2))
                qry.Append($"\n   AND 담당자2 LIKE '{manager2}%'                                                          ");

            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n   {commonRepository.whereSql("구분", division)}                                                          ");
            qry.Append($"\n ) AS t                                                                                            ");
            qry.Append($"\n WHERE t.rowidx = 1                                                                                            ");


            //qry.Append($"\n  ORDER BY CASE WHEN 매입단가 IS NULL THEN 2 ELSE 1 END, 매입단가 ASC, CASE WHEN 매입처 LIKE '%재고%' THEN 1 ELSE 2 END, 매입일자 DESC                                                         ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetAllData(string sttdate, string enddate, string productList)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                            ");
            qry.Append($"\n   대분류                                                                                          ");
            qry.Append($"\n , 매입일자                                                                                        ");
            qry.Append($"\n , 매입처                                                                                          ");
            qry.Append($"\n , 품명                                                                                            ");
            qry.Append($"\n , 품목코드                                                                                        ");
            qry.Append($"\n , 원산지                                                                                          ");
            qry.Append($"\n , 원산지코드                                                                                      ");
            qry.Append($"\n , 규격                                                                                            ");
            qry.Append($"\n , 규격코드                                                                                        ");
            qry.Append($"\n , ISNULL(단위       , '1') AS 단위                                                                ");
            qry.Append($"\n , ISNULL(가격단위   , '') AS 가격단위                                                             ");
            qry.Append($"\n , ISNULL(단위수량   , 1) AS 단위수량                                                              ");
            qry.Append($"\n , ISNULL(SEAOVER단위, '1') AS SEAOVER단위                                                         ");
            qry.Append($"\n , 보관처                                                                                          ");
            qry.Append($"\n , 적요                                                                                            ");
            qry.Append($"\n , 단가수정일                                                                                     ");
            qry.Append($"\n , ISNULL(매입단가, 0) AS 매입단가                                                                 ");
            qry.Append($"\n , ISNULL(매출단가, 0) AS 매출단가                                                                 ");
            qry.Append($"\n , 담당자1                                                                                     ");
            qry.Append($"\n , 담당자2                                                                                    ");
            qry.Append($"\n , 비고1                                                                                    ");
            qry.Append($"\n , 비고2                                                                                    ");
            qry.Append($"\n , 부가세유무                                                                                    ");



            qry.Append($"\n FROM 업체별시세관리                                                                               ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                         ");
            if (!string.IsNullOrEmpty(sttdate))
                qry.Append($"\n   AND 매입일자 >= '{sttdate}'                                                                      ");
            if (!string.IsNullOrEmpty(enddate))
                qry.Append($"\n   AND 매입일자 <= '{enddate}'                                                                      ");
            //qry.Append($"\n   AND 매입단가 > 10                                                                               ");
            if (!string.IsNullOrEmpty(productList))
            {
                string[] products = productList.Split('\n');
                if (products.Length > 0)
                {
                    string whr = "";
                    qry.Append($"\n   AND (                                                                                        ");
                    foreach (string product in products)
                    {
                        if (!string.IsNullOrEmpty(whr))
                            whr += " OR ";
                        string[] p = product.Split('^');
                        whr += ($"\n   (품명 = '{p[0]}'                                                                   ");
                        whr += ($"\n   AND 원산지 = '{p[1]}'                                                                ");
                        whr += ($"\n   AND 규격 = '{p[2]}'                                                                 ");
                        whr += ($"\n   AND 단위 = '{p[3]}'                                                                  ");
                        whr += ($"\n   AND ISNULL(가격단위, '') = '{p[4]}'                                                            ");
                        whr += ($"\n   AND ISNULL(단위수량, 1) = '{p[5]}'                                                            ");
                        whr += ($"\n   AND SEAOVER단위 = '{p[6]}')                                                          ");
                    }
                    qry.Append($"\n  {whr}                                                                                         ");
                    qry.Append($"\n   )                                                                                        ");
                }
            }
            qry.Append($"\n  ORDER BY CASE WHEN 매입단가 IS NULL THEN 2 ELSE 1 END, 매입단가 ASC, CASE WHEN 매입처 LIKE '%재고%' THEN 1 ELSE 2 END, 매입일자 DESC                                                         ");


            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductCode(string product, string origin, string sizes)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                            ");
            qry.Append($"\n   품목코드                                                                                        ");
            qry.Append($"\n , 품명                                                                                            ");
            qry.Append($"\n , 원산지코드                                                                                      ");
            qry.Append($"\n , 원산지                                                                                          ");
            qry.Append($"\n , 규격코드                                                                                        ");
            qry.Append($"\n , 규격                                                                                            ");
            qry.Append($"\n FROM 업체별시세관리                                                                               ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                         ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   AND 품명 = '{product}'                                                                      ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   AND 원산지 = '{origin}'                                                                     ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n   AND 규격 = '{sizes}'                                                                        ");
            
            qry.Append($"\n GROUP BY 품목코드, 품명, 원산지코드, 원산지, 규격코드, 규격                                       ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetMarketPrice(string product, string origin, string sizes, string unit, string division, string manager1, string manager2)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                              ");
            qry.Append($"\n   t1.품명                                                                                                                             ");
            qry.Append($"\n , t1.원산지                                                                                                                            ");
            qry.Append($"\n , t1.규격                                                                                                                             ");
            qry.Append($"\n , t1.단위                                                                                                                             ");
            qry.Append($"\n , ISNULL(t1.가격단위, '') AS 가격단위                                                                                                      ");
            qry.Append($"\n , ISNULL(t1.단위수량, 0) AS 단위수량                                                                                                       ");
            qry.Append($"\n , ISNULL(t1.SEAOVER단위, 0) AS SEAOVER단위                                                                                              ");
            qry.Append($"\n , ISNULL(t1.계산단위, 0) AS 계산단위                                                                                                       ");
            qry.Append($"\n , ISNULL(t1.담당자1, '') AS 담당자1                                                                                                      ");
            qry.Append($"\n , ISNULL(t1.담당자2, '') AS 담당자2                                                                                                      ");
            qry.Append($"\n , CASE WHEN t1.구분 = '999' THEN '' ELSE t1.구분 END AS 구분                                                                               ");
            qry.Append($"\n , ISNULL(t2.일반시세, 0) AS 일반시세                                                                                                       ");
            qry.Append($"\n FROM (                                                                                                                              ");
            qry.Append($"\n 	SELECT                                                                                                                          ");
            qry.Append($"\n 	  품명                                                                                                                            ");
            qry.Append($"\n 	, 원산지                                                                                                                           ");
            qry.Append($"\n 	, 규격                                                                                                                            ");
            qry.Append($"\n 	, 단위                                                                                                                            ");
            qry.Append($"\n 	, 가격단위                                                                                                                          ");
            qry.Append($"\n 	, 단위수량                                                                                                                          ");
            qry.Append($"\n 	, SEAOVER단위                                                                                                                     ");
            qry.Append($"\n 	, CASE                                                                                                                          ");
            qry.Append($"\n 		WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(float, SEAOVER단위)/CONVERT(float, 단위수량)                                        ");
            qry.Append($"\n 		ELSE SEAOVER단위 END AS 계산단위                                                                                                 ");
            qry.Append($"\n 	, 담당자1                                                                                                                          ");
            qry.Append($"\n 	, 담당자2                                                                                                                          ");
            qry.Append($"\n 	, MIN(ISNULL(구분, '999')) AS 구분                                                                                                    ");
            qry.Append($"\n 	FROM 업체별시세관리                                                                                                                   ");
            qry.Append($"\n 	WHERE 사용자 = '{userId}'                                                                                                            ");
            if(!string.IsNullOrEmpty(product))
                qry.Append($"\n 	{commonRepository .whereSql("품명", product)}                                                                                                            ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n 	{commonRepository .whereSql("원산지", origin)}                                                                                                            ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n 	{commonRepository .whereSql("규격", sizes)}                                                                                                            ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n 	{commonRepository .whereSql("단위", unit)}                                                                                                            ");
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n 	{commonRepository .whereSql("구분", division)}                                                                                                            ");
            if (!string.IsNullOrEmpty(manager1))
                qry.Append($"\n 	{commonRepository .whereSql("담당자1", manager1)}                                                                                                            ");
            if (!string.IsNullOrEmpty(manager2))
                qry.Append($"\n 	{commonRepository .whereSql("담당자2", manager2)}                                                                                                            ");
            qry.Append($"\n 	GROUP BY 품명, 원산지, 규격, 단위, 가격단위, 단위수량, SEAOVER단위, 담당자1, 담당자2                                                                  ");
            qry.Append($"\n ) AS t1                                                                                                                             ");
            qry.Append($"\n LEFT OUTER JOIN(                                                                                                                    ");
            qry.Append($"\n 	SELECT                                                                                                                          ");
            qry.Append($"\n 	  t.품명                                                                                                                          ");
            qry.Append($"\n 	, t.원산지                                                                                                                         ");
            qry.Append($"\n 	, t.규격                                                                                                                          ");
            qry.Append($"\n 	, t.계산단위                                                                                                                        ");
            qry.Append($"\n 	, AVG(t.매입단가) AS 일반시세                                                                                                          ");
            qry.Append($"\n 	FROM(                                                                                                                           ");
            qry.Append($"\n 	   SELECT                                                                                                                       ");
            qry.Append($"\n 		  t.*                                                                                                                       ");
            qry.Append($"\n 		, ROW_NUMBER() OVER(PARTITION BY t.품명, t.원산지, t.규격, t.계산단위 ORDER BY t.매입일자 DESC, t.매입단가 ASC) AS Row#                      ");
            qry.Append($"\n 		FROM (                                                                                                                      ");
            qry.Append($"\n 			SELECT                                                                                                                  ");
            qry.Append($"\n 			  품명 AS 품명                                                                                                              ");
            qry.Append($"\n 			, 원산지 AS 원산지                                                                                                           ");
            qry.Append($"\n 			, 규격 AS 규격                                                                                                              ");
            qry.Append($"\n 			, 매입단가 AS 매입단가                                                                                                         ");
            qry.Append($"\n 			, 매입처 AS 매입처                                                                                                           ");
            qry.Append($"\n 			, CASE                                                                                                                  ");
            qry.Append($"\n 				WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(float, SEAOVER단위)/CONVERT(float, 단위수량)                                ");
            qry.Append($"\n 				ELSE SEAOVER단위 END AS 계산단위                                                                                         ");
            qry.Append($"\n 			, 매입일자 AS 매입일자                                                                                                         ");
            qry.Append($"\n 			FROM 업체별시세관리                                                                                                           ");
            qry.Append($"\n 			WHERE 사용자 = '{userId}'                                                                                                    ");
            qry.Append($"\n 			   AND 매입단가 > 10                                                                                                        ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n 	{commonRepository .whereSql("품명", product)}                                                                                                            ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n 	{commonRepository .whereSql("원산지", origin)}                                                                                                            ");
            if (!string.IsNullOrEmpty(sizes))
                qry.Append($"\n 	{commonRepository .whereSql("규격", sizes)}                                                                                                            ");
            if (!string.IsNullOrEmpty(unit))
                qry.Append($"\n 	{commonRepository .whereSql("단위", unit)}                                                                                                            ");
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n 	{commonRepository .whereSql("구분", division)}                                                                                                            ");
            if (!string.IsNullOrEmpty(manager1))
                qry.Append($"\n 	{commonRepository .whereSql("담당자1", manager1)}                                                                                                            ");
            if (!string.IsNullOrEmpty(manager2))
                qry.Append($"\n 	{commonRepository .whereSql("담당자2", manager2)}                                                                                                            ");
            qry.Append($"\n 	     ) AS t                                                                                                                     ");
            qry.Append($"\n 	 ) AS t                                                                                                                         ");
            qry.Append($"\n 	WHERE Row# <= 3                                                                                                                 ");
            qry.Append($"\n 	GROUP BY t.품명, t.원산지, t.규격, t.계산단위                                                                                             ");
            qry.Append($"\n ) AS t2                                                                                                                             ");
            qry.Append($"\n   ON t1.품명 = t2.품명                                                                                                                  ");
            qry.Append($"\n   AND t1.원산지 = t2.원산지                                                                                                              ");
            qry.Append($"\n   AND t1.규격 = t2.규격                                                                                                                 ");
            qry.Append($"\n   AND ISNULL(t1.계산단위, 0) = ISNULL(t2.계산단위, 0)                                                                                      ");
            qry.Append($"\n WHERE t2.일반시세 > 0                                                                                                                   ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetStockAndSales(string product, string origin, string sizes, string unit, string sales_term)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                 ");
            qry.Append($"\n   품명                                                                                                 ");
            qry.Append($"\n , 원산지                                                                                               ");
            qry.Append($"\n , 규격                                                                                                 ");
            qry.Append($"\n , SUM(재고수 * CONVERT(float, 단위) / {unit}) AS 재고수                                                ");
            qry.Append($"\n , SUM(매출수 * CONVERT(float, 단위) / {unit}) AS 매출수                                                ");
            qry.Append($"\n FROM (                                                                                                 ");
            qry.Append($"\n 	SELECT                                                                                             ");
            qry.Append($"\n 	  품명                                                                                             ");
            qry.Append($"\n 	, 원산지                                                                                           ");
            qry.Append($"\n 	, 규격                                                                                         ");
            qry.Append($"\n 	, CASE WHEN 단위 = '.' OR 단위 = '-' THEN null ELSE REPLACE(단위, 'kg', '') END AS 단위        ");
            qry.Append($"\n 	, ISNULL(재고수, 0) - ISNULL(예약수, 0) AS 재고수                                              ");
            qry.Append($"\n 	, 0 AS 매출수                                                                                  ");
            qry.Append($"\n 	FROM 품명별재고현황                                                                            ");
            qry.Append($"\n 	WHERE 사용자 = '{userId}'                                                                      ");
            qry.Append($"\n	    AND 품명 = '{product}'                                                                         ");
            qry.Append($"\n	    AND 원산지 = '{origin}'                                                                        ");
            qry.Append($"\n	    AND 규격 = '{sizes}'                                                                           ");
            qry.Append($"\n 	UNION ALL                                                                                      ");
            qry.Append($"\n 	SELECT                                                                                         ");
            qry.Append($"\n 	  품명                                                                                         ");
            qry.Append($"\n 	, 원산지                                                                                       ");
            qry.Append($"\n 	, 규격                                                                                         ");
            qry.Append($"\n 	, CASE WHEN 단위 = '.' OR 단위 = '-' THEN null ELSE REPLACE(단위, 'kg', '') END AS 단위        ");
            qry.Append($"\n 	, 0 AS 재고수                                                                                  ");
            qry.Append($"\n 	, ISNULL(매출수, 0) AS 매출수                                                                  ");
            qry.Append($"\n 	FROM 품명별매출현황                                                                            ");
            qry.Append($"\n 	WHERE 사용자 = '{sales_term}'                                                                  ");
            qry.Append($"\n	    AND 품명 = '{product}'                                                                         ");
            qry.Append($"\n	    AND 원산지 = '{origin}'                                                                        ");
            qry.Append($"\n	    AND 규격 = '{sizes}'                                                                           ");
            qry.Append($"\n  ) AS t                                                                                            ");
            qry.Append($"\n  WHERE ISNUMERIC(t.단위) = 1                                                                       ");
            qry.Append($"\n  GROUP BY 품명, 원산지, 규격                                                                       ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetStockAndSalesDetail(string product, string origin, string sizes, string unit, string sales_term, bool isIntegrateUnit = false)
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n   SELECT                                                                                      ");
            qry.Append($"\n    품명                                                                                         ");
            qry.Append($"\n  , 원산지                                                                                        ");
            qry.Append($"\n  , 규격                                                                                         ");
            qry.Append($"\n  , SUM(통관) / {unit.ToString()} AS 통관                                                                        ");
            qry.Append($"\n  , SUM(미통관) / {unit.ToString()} AS 미통관                                                                     ");
            qry.Append($"\n  , SUM(예약수) / {unit.ToString()} AS 예약수                                                                     ");
            qry.Append($"\n  , SUM(매출수) / {unit.ToString()} AS 매출수                                                                     ");
            qry.Append($"\n  FROM (                                                                                       ");
            qry.Append($"\n 	SELECT                                                                                    ");
            qry.Append($"\n 	  품명                                                                                      ");
            qry.Append($"\n 	, 원산지                                                                                     ");
            qry.Append($"\n 	, 규격                                                                                      ");
            qry.Append($"\n 	, 통관 * CONVERT(float, 단위) AS 통관                                                          ");
            qry.Append($"\n 	, 미통관 * CONVERT(float, 단위) AS 미통관                                                                                                       ");
            qry.Append($"\n 	, 예약수 * CONVERT(float, 단위) AS 예약수                                                                                                       ");
            qry.Append($"\n 	, 매출수 * CONVERT(float, 단위) AS 매출수                                                                                                       ");
            qry.Append($"\n 	FROM(                                                                                                                                           ");
            qry.Append($"\n 	  	SELECT                                                                                                                                      ");
            qry.Append($"\n 	 	  품명                                                                                                                                      ");
            qry.Append($"\n 	 	, 원산지                                                                                                                                    ");
            qry.Append($"\n 	 	, 규격                                                                                                                                      ");
            qry.Append($"\n 	 	, CASE WHEN 단위 = '.' OR 단위 = '-' THEN null ELSE REPLACE(단위, 'kg', '') END AS 단위                                                     ");
            qry.Append($"\n 	 	, CASE WHEN 통관 = '통관' THEN ISNULL(재고수, 0) ELSE 0 END AS 통관                                                                         ");
            qry.Append($"\n 	 	, CASE WHEN 통관 <> '통관' THEN ISNULL(재고수, 0) ELSE 0 END AS 미통관                                                                      ");
            qry.Append($"\n 	 	, ISNULL(예약수 , 0) AS 예약수                                                                                                        ");
            qry.Append($"\n 	 	, 0 AS 매출수                                                                                                                         ");
            qry.Append($"\n 	 	FROM 품명별재고현황                                                                                                                   ");
            qry.Append($"\n 	    WHERE 사용자 = '{userId}'                                                                                                             ");
            qry.Append($"\n	            AND ((적요 NOT LIKE '%제외%' AND 적요 NOT LIKE '%back%' AND 적요 NOT LIKE '%백쉽%' AND 적요 NOT LIKE '%빽쉽%' ) OR 적요 IS NULL)  ");
            qry.Append($"\n	            AND 품명 = '{product}'                                                                                                            ");
            qry.Append($"\n	            AND 원산지 = '{origin}'                                                                                                           ");
            qry.Append($"\n	            AND 규격 = '{sizes}'                                                                                                              ");
            qry.Append($"\n 	 	UNION ALL                                                                                                                             ");
            qry.Append($"\n 	 	SELECT                                                                                                                                ");
            qry.Append($"\n 	 	  품명                                                                                                                                ");
            qry.Append($"\n 	 	, 원산지                                                                                                                              ");
            qry.Append($"\n 	 	, 규격                                                                                                                                ");
            qry.Append($"\n 	 	, CASE WHEN 단위 = '.' OR 단위 = '-' THEN null ELSE REPLACE(단위, 'kg', '') END AS 단위                                               ");
            qry.Append($"\n 	 	, 0 AS 통관                                                                                                                           ");
            qry.Append($"\n 	 	, 0 AS 미통관                                                                                                                         ");
            qry.Append($"\n 	 	, 0 AS 예약수                                                                                                                         ");
            qry.Append($"\n 	 	, ISNULL(매출수, 0) AS 매출수                                                                                                         ");
            qry.Append($"\n 	 	FROM 매출현황                                                                                                                         ");
            qry.Append($"\n 	    WHERE 사용자 = '200009'                                                                                                               ");
            qry.Append($"\n 	      AND 매출자 <> '199990'                                                                                                              ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%아토무역%'       )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%아토코리아%'      )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%에이티오%'       )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%에스제이씨푸드/신영국%')                                                       ");
            DateTime sDate = DateTime.Now.AddMonths(-Convert.ToInt16(sales_term));
            if(sales_term == "45")
                sDate = DateTime.Now.AddDays(-Convert.ToInt16(sales_term));
            //2023-10-20 범준과장님 요청으로 당일매출은 제외
            qry.Append($"\n 	      AND 매출일자 >= '{sDate.ToString("yyyy-MM-dd")}' AND 매출일자 <= '{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}'                                  ");
            qry.Append($"\n	            AND 품명 = '{product}'                                                                         ");
            qry.Append($"\n	            AND 원산지 = '{origin}'                                                                        ");
            qry.Append($"\n	            AND 규격 = '{sizes}'                                                                           ");
            qry.Append($"\n 	) AS t                                                                                    ");
            qry.Append($"\n 	WHERE ISNUMERIC(t.단위) = 1                                                                 ");
            if (!isIntegrateUnit)
                qry.Append($"\n    AND t.단위 = '{unit}'                                                                           ");
            qry.Append($"\n   ) AS t                                                                                      ");
            qry.Append($"\n   GROUP BY 품명, 원산지, 규격                                                                       ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        //영업 대시보드에서 대표품목 검색을 위해 새로만듬
        public DataTable GetStockAndSalesDetail2(string product, string origin, string sizes, string unit, string sales_term, string sub_product, bool isMerge)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n   SELECT                                                                                      ");
            qry.Append($"\n    품명                                                                                         ");
            qry.Append($"\n  , 원산지                                                                                        ");
            qry.Append($"\n  , 규격                                                                                         ");
            qry.Append($"\n  , 단위                                                                                      ");
            if (string.IsNullOrEmpty(unit))
            {
                qry.Append($"\n  , SUM(통관) AS 통관                                                                        ");
                qry.Append($"\n  , SUM(미통관) AS 미통관                                                                     ");
                qry.Append($"\n  , SUM(예약수) AS 예약수                                                                     ");
                qry.Append($"\n  , SUM(매출수) AS 매출수                                                                     ");
            }
            else
            {
                qry.Append($"\n  , SUM(통관 * CAST(단위 AS float) / {unit}) AS 통관                                                                        ");
                qry.Append($"\n  , SUM(미통관 * CAST(단위 AS float) / {unit}) AS 미통관                                                                     ");
                qry.Append($"\n  , SUM(예약수 * CAST(단위 AS float) / {unit}) AS 예약수                                                                     ");
                qry.Append($"\n  , SUM(매출수 * CAST(단위 AS float) / {unit}) AS 매출수                                                                     ");
            }
            qry.Append($"\n  FROM (                                                                                       ");
            qry.Append($"\n 	SELECT                                                                                    ");
            qry.Append($"\n 	  품명                                                                                      ");
            qry.Append($"\n 	, 원산지                                                                                     ");
            qry.Append($"\n 	, 규격                                                                                      ");
            qry.Append($"\n 	, 단위                                                                                      ");
            qry.Append($"\n 	, 통관  AS 통관                                                          ");
            qry.Append($"\n 	, 미통관  AS 미통관                                                        ");
            qry.Append($"\n 	, 예약수  AS 예약수                                                        ");
            qry.Append($"\n 	, 매출수  AS 매출수                                                        ");
            qry.Append($"\n 	FROM(                                                                                     ");
            qry.Append($"\n 	  	SELECT                                                                                ");
            qry.Append($"\n 	 	  품명                                                                                  ");
            qry.Append($"\n 	 	, 원산지                                                                                 ");
            qry.Append($"\n 	 	, 규격                                                                                  ");
            qry.Append($"\n 	 	, CASE WHEN 단위 = '.' OR 단위 = '-' THEN null ELSE REPLACE(단위, 'kg', '') END AS 단위      ");
            qry.Append($"\n 	 	, CASE WHEN 매입구분 = '국내매입' OR 통관 = '통관' OR 통관 = '' THEN ISNULL(재고수, 0) ELSE 0 END AS 통관                           ");
            qry.Append($"\n 	 	, CASE WHEN 매입구분 <> '국내매입' AND 통관 <> '통관' THEN ISNULL(재고수, 0) ELSE 0 END AS 미통관                         ");
            qry.Append($"\n 	 	, ISNULL(예약수 , 0) AS 예약수                                                             ");
            qry.Append($"\n 	 	, 0 AS 매출수                                                                            ");
            qry.Append($"\n 	 	FROM 품명별재고현황                                                                         ");
            qry.Append($"\n 	    WHERE 사용자 = '{userId}'                                                                      ");
            qry.Append($"\n	            AND ((적요 NOT LIKE '%제외%' AND 적요 NOT LIKE '%back%' AND 적요 NOT LIKE '%백쉽%' AND 적요 NOT LIKE '%빽쉽%' ) OR 적요 IS NULL)  ");
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
            qry.Append($"\n 	 	UNION ALL                                                                             ");
            qry.Append($"\n 	 	SELECT                                                                                ");
            qry.Append($"\n 	 	  품명                                                                                  ");
            qry.Append($"\n 	 	, 원산지                                                                                 ");
            qry.Append($"\n 	 	, 규격                                                                                  ");
            qry.Append($"\n 	 	, CASE WHEN 단위 = '.' OR 단위 = '-' THEN null ELSE REPLACE(단위, 'kg', '') END AS 단위      ");
            qry.Append($"\n 	 	, 0 AS 통관                                                                             ");
            qry.Append($"\n 	 	, 0 AS 미통관                                                                            ");
            qry.Append($"\n 	 	, 0 AS 예약수                                                                            ");
            qry.Append($"\n 	 	, ISNULL(매출수, 0) AS 매출수                                                              ");
            qry.Append($"\n 	 	FROM 매출현황                                                                             ");
            qry.Append($"\n 	    WHERE 사용자 = '200009'                                                                  ");
            qry.Append($"\n 	      AND 매출자 <> '199990'                                                                 ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%아토무역%'       )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%아토코리아%'      )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%에이티오%'       )                                                   ");
            qry.Append($"\n 	      AND NOT (ISNULL(매출수, 0) >= 100 AND 매출처 LIKE '%에스제이씨푸드/신영국%')                                                       ");


            DateTime sDate = DateTime.Now.AddMonths(-Convert.ToInt16(sales_term.Replace("개월", "").Replace("일", "")));
            if(sales_term.Contains("45"))
                sDate = DateTime.Now.AddDays(-Convert.ToInt16(sales_term));
            //2023-10-20 범준과장님 요청 당일매출은 빼기
            qry.Append($"\n 	      AND 매출일자 >= '{sDate.ToString("yyyy-MM-dd")}' AND 매출일자 <= '{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}'                                  ");
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
            qry.Append($"\n 	) AS t                                                                                    ");
            qry.Append($"\n 	WHERE ISNUMERIC(t.단위) = 1                                                                 ");
            qry.Append($"\n   ) AS t                                                                                      ");
            qry.Append($"\n   GROUP BY 품명, 원산지, 규격, 단위                                                                       ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }


        public int UpdateProduct(string id, string sub_id, string product, string origin, string sizes, string box_weight, string cost_unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_customs SET            ");
            qry.Append($"   product = '{product}'        ");
            qry.Append($" , origin = '{origin}'          ");
            qry.Append($" , sizes = '{sizes}'            ");
            qry.Append($" , box_weight = '{box_weight}'  ");
            qry.Append($" , cost_unit = '{cost_unit}'    ");
            qry.Append($" WHERE id = {id}                ");
            qry.Append($"   AND sub_id = {sub_id}        ");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }
        public DataTable GetProductPriceInfo(string product = null, string origin = null, string sizes = null, string unit = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                   ");
            qry.Append($"\n   매입처 AS company                                                                                                      ");
            qry.Append($"\n , 매입일자 AS updatetime                                                                                                 ");
            qry.Append($"\n , 단위, 단위수량, SEAOVER단위                                                                                            ");
            qry.Append($"\n , (매입단가 * 단위수량)  AS purchase_price                                                                               ");
            qry.Append($"\n , (매출단가 * 단위수량)  AS sales_price                                                                                  ");
            qry.Append($"\n FROM 업체별시세관리                                                                                                      ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                                ");
            qry.Append($"\n   AND 매입단가 > 100                                                                                                   ");
            qry.Append($"\n   AND 품명 = '{product}'                                                                                               ");
            qry.Append($"\n   AND 원산지 = '{origin}'                                                                                              ");
            qry.Append($"\n   AND 규격 = '{sizes}'                                                                                                 ");
            qry.Append($"\n ORDER BY 매입일자 DESC                                                                                                 ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetOneColumn(string colName, string whr, string val)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                    ");
            qry.Append($"\n   {colName}                                                                                               ");
            qry.Append($"\n FROM 업체별시세관리                                                                                       ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                 ");
            if (!string.IsNullOrEmpty(whr) && !string.IsNullOrEmpty(val))
            { 
                qry.Append($"\n   AND {whr} LIKE '%{val}%'                                                                              ");
            }
            qry.Append($"\n GROUP BY  {colName} ");
            qry.Append($"\n ORDER BY  {colName} ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public List<SeaoverProductModel> GetProductForCompany(string product = null, string origin = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                      ");
            qry.Append($"\n   품명                      ");
            qry.Append($"\n , 원산지                    ");
            qry.Append($"\n FROM 업체별시세관리         ");
            qry.Append($"\n WHERE 사용자 = '{userId}' AND 원산지 IS NOT NULL    ");
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n	   AND 품명 LIKE '%{product}%'                                                                      ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n	   AND 원산지 LIKE '%{origin}%'                                                                     ");
            }
            qry.Append($"\n GROUP BY 품명, 원산지       ");
            qry.Append($"\n ORDER BY 품명, 원산지       ");

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetProductForCompany(dr);
        }
        private List<SeaoverProductModel> SetProductForCompany(SqlDataReader rd)
        {
            List<SeaoverProductModel> list = new List<SeaoverProductModel>();
            while (rd.Read())
            {
                SeaoverProductModel model = new SeaoverProductModel();
                model.product = rd["품명"].ToString();
                model.origin = rd["원산지"].ToString();
                list.Add(model);
            }
            rd.Close();
            return list;
        }

        public DataTable GetProductTapble1()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                ");
            qry.Append($"\n   대분류                                                                                                ");
            qry.Append($"\n , 품목                                                                                               ");
            qry.Append($"\n , 원산지                                                                                              ");
            qry.Append($"\n , 규격                                                                                               ");
            qry.Append($"\n , 단위                                                                                                 ");
            qry.Append($"\n , 가격단위                                                                                               ");
            qry.Append($"\n , 단위수량                                                                                               ");
            qry.Append($"\n , SEAOVER단위                                                                                          ");
            qry.Append($"\n , ISNULL(매출단가, 0) AS 매출단가                                                                            ");
            qry.Append($"\n , 구분                                                                                                 ");
            qry.Append($"\n , MAX(CONVERT(VARCHAR(10), 단가수정일, 120))    AS 단가수정일                                                                             ");
            qry.Append($"\n , 담당자1                                                                                                ");
            qry.Append($"\n FROM 업체별시세관리                                                                                         ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                 ");
            qry.Append($"\n   AND 대분류 <> ''                                                                                      ");
            qry.Append($"\n   AND 대분류 IS NOT NULL                                                                                ");
            qry.Append($"\n GROUP BY  대분류, 원산지코드, 품목코드, 규격코드, 단위	, 가격단위, 단위수량, SEAOVER단위, 매출단가, 구분, 단가수정일, 담당자1                    ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductTapble2()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                   ");
            qry.Append($"\n   대분류                                                                                                 ");
            qry.Append($"\n , 원산지코드                                                                                             ");
            qry.Append($"\n , 품목코드                                                                                               ");
            qry.Append($"\n , 규격코드                                                                                               ");
            qry.Append($"\n , 단위                                                                                                   ");
            qry.Append($"\n , 가격단위                                                                                               ");
            qry.Append($"\n , 단위수량                                                                                               ");
            qry.Append($"\n , SEAOVER단위                                                                                            ");
            qry.Append($"\n , ISNULL(매출단가, 0) AS 매출단가                                                                        ");
            qry.Append($"\n , 구분                                                                                                   ");
            qry.Append($"\n , MAX(CONVERT(VARCHAR(10), 단가수정일, 120))    AS 단가수정일                                            ");
            qry.Append($"\n , 담당자1                                                                                                ");
            qry.Append($"\n , CASE WHEN ISNULL(부가세유무, 'N') = 'N' THEN '' ELSE '과세' END 부가세유무                             ");
            qry.Append($"\n FROM 업체별시세관리                                                                                      ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                ");
            /*qry.Append($"\n   AND 대분류 <> ''                                                                                      ");
            qry.Append($"\n   AND 대분류 IS NOT NULL                                                                                ");*/
            qry.Append($"\n   AND 매입일자 IS NOT NULL                                                                                ");
            //qry.Append($"\n   AND 단가수정일 IS NOT NULL        ");
            qry.Append($"\n GROUP BY  대분류, 원산지코드, 품목코드, 규격코드, 단위, 가격단위, 단위수량, SEAOVER단위, 매출단가, 구분, 담당자1, 부가세유무                    ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductTapble4()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                   ");
            qry.Append($"\n   대분류                                                                                                 ");
            qry.Append($"\n , 원산지코드                                                                                             ");
            qry.Append($"\n , 품목코드                                                                                               ");
            qry.Append($"\n , 규격코드                                                                                               ");
            qry.Append($"\n , 단위                                                                                                   ");
            qry.Append($"\n , 가격단위                                                                                               ");
            qry.Append($"\n , 단위수량                                                                                               ");
            qry.Append($"\n , SEAOVER단위                                                                                            ");
            qry.Append($"\n , ISNULL(매출단가, 0) AS 매출단가                                                                        ");
            //qry.Append($"\n , 구분                                                                                                   ");
            qry.Append($"\n , MAX(CONVERT(VARCHAR(10), 단가수정일, 120))    AS 단가수정일                                            ");
            //qry.Append($"\n , 담당자1                                                                                                ");
            //qry.Append($"\n , CASE WHEN ISNULL(부가세유무, 'N') = 'N' THEN '' ELSE '과세' END 부가세유무                             ");
            qry.Append($"\n FROM 업체별시세관리                                                                                      ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                ");
            /*qry.Append($"\n   AND 대분류 <> ''                                                                                      ");
            qry.Append($"\n   AND 대분류 IS NOT NULL                                                                                ");*/
            qry.Append($"\n   AND 매입일자 IS NOT NULL                                                                                ");
            qry.Append($"\n   AND 단가수정일 IS NOT NULL        ");
            qry.Append($"\n GROUP BY  대분류, 원산지코드, 품목코드, 규격코드, 단위, 가격단위, 단위수량, SEAOVER단위, 매출단가                    ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductTapble3()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                ");
            qry.Append($"\n   품명                                                                                                ");
            qry.Append($"\n , 원산지                                                                                              ");
            qry.Append($"\n , 규격                                                                                                ");
            qry.Append($"\n , 단위                                                                                                ");
            qry.Append($"\n , SEAOVER단위                                                                                         ");
            qry.Append($"\n , 매출단가                                                                                            ");
            qry.Append($"\n , 매입처                                                                                              ");
            qry.Append($"\n , 보관처                                                                                              ");
            qry.Append($"\n , 매입단가                                                                                            ");
            qry.Append($"\n FROM 업체별시세관리                                                                                   ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                             ");
            qry.Append($"\n   AND 매출단가 > 6                                                                                    ");
            qry.Append($"\n   AND (단가수정일 > '{DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd")}'                             ");
            qry.Append($"\n   OR (단가수정일 = NULL AND 매입일자 > '{DateTime.Now.AddMonths(-2).ToString("yyyy-MM-dd")}'))        ");
            qry.Append($"\n  ORDER BY 품명, 원산지, 규격, 단위, 매출단가                                                          ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductTapble(string category =null, string product= null, string origin = null, string sizes = null, string unit = null, string manager1 =null, string manager2= null, string division = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                ");
            qry.Append($"\n   ISNULL(대분류, '') AS 대분류                                                                        ");
            qry.Append($"\n , ISNULL(대분류1, '') AS 대분류1                                                                      ");
            qry.Append($"\n , ISNULL(원산지, '') AS 원산지                                                                        ");
            qry.Append($"\n , ISNULL(품명, '') AS 품명                                                                            ");
            qry.Append($"\n , ISNULL(규격, '') AS 규격                                                                            ");
            qry.Append($"\n , ISNULL(단위, '') AS 단위                                                                            ");
            qry.Append($"\n , ISNULL(가격단위, '') AS 가격단위                                                                    ");
            qry.Append($"\n , ISNULL(단위수량, 0) AS 단위수량                                                                    ");
            qry.Append($"\n , ISNULL(SEAOVER단위, '') AS SEAOVER단위                                                              ");
            qry.Append($"\n , ISNULL(구분, '') AS 구분                                                                            ");
            qry.Append($"\n , ISNULL(담당자1, '') AS 담당자1                                                                      ");
            qry.Append($"\n , ISNULL(담당자2, '') AS 담당자2                                                                      ");
            qry.Append($"\n FROM (                                                                                                ");
            qry.Append($"\n   SELECT                                                                                              ");
            qry.Append($"\n      CASE                                                                                             ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Aa' THEN '라운드새우류'                        ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ab' THEN '새우살류'                            ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ba' THEN '쭈꾸미류'                            ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bb' THEN '낙지류'                              ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bc' THEN '갑오징어류'                          ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bd' THEN '오징어류'                            ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Be' THEN '문어류'                              ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'C' THEN '갑각류'                               ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Da' THEN '어패류'                              ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Db' THEN '살류'                                ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Dc' THEN '해물류'                              ");
            qry.Append($"\n  	  	WHEN CONVERT(char, SUBSTRING(대분류, 1, 3)) = 'Eab' THEN '선어특수/가공품'                    ");
            qry.Append($"\n  	  	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ea' THEN '초밥류'                              ");
            qry.Append($"\n  	  	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Eb' THEN '기타수산가공품'                      ");
            qry.Append($"\n  	  	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ec' THEN '기타식품가공품'                      ");
            qry.Append($"\n  	  	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ed' THEN '축산가공품'                          ");
            qry.Append($"\n  	  	WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ee' THEN '야채과일류'                          ");
            qry.Append($"\n      	WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'F' THEN '선어류'                               ");
            qry.Append($"\n      	ELSE ISNULL(대분류, '')                                                                       ");
            qry.Append($"\n      END AS 대분류                                                                                    ");
            qry.Append($"\n      , ISNULL(대분류, '') AS 대분류1                                                                  ");
            qry.Append($"\n      , ISNULL(원산지, '')  AS 원산지                                                                  ");
            qry.Append($"\n      , ISNULL(품명, '')  AS 품명                                                                      ");
            qry.Append($"\n      , ISNULL(규격, '')  AS 규격                                                                      ");
            qry.Append($"\n      , ISNULL(단위, '')  AS 단위                                                                      ");
            qry.Append($"\n      , ISNULL(가격단위, '')  AS 가격단위                                                              ");
            qry.Append($"\n      , ISNULL(단위수량, 0)  AS 단위수량                                                              ");
            qry.Append($"\n      , ISNULL(SEAOVER단위, '') AS SEAOVER단위                                                         ");
            qry.Append($"\n      , ISNULL(구분, '')  AS 구분                                                                      ");
            qry.Append($"\n      , ISNULL(담당자1, '')  AS 담당자1                                                                ");
            qry.Append($"\n      , ISNULL(담당자2, '')  AS 담당자2                                                                ");
            qry.Append($"\n      FROM 업체별시세관리                                                                              ");
            qry.Append($"\n      WHERE 사용자 = '{userId}'                                                                        ");
            qry.Append($"\n ) AS t                                                                                                ");
            qry.Append($"\n WHERE 1=1                                                                                             ");

            string[] tempStr = null;
            string tempWhr = "";

            if (!(category == null || string.IsNullOrEmpty(category)))
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
                            tempWhr = $"\n	   t.대분류   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.대분류   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.대분류   LIKE '%{category}%'                                                                         ");
                }

            }

            if (!(product == null || string.IsNullOrEmpty(product)))
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
                            tempWhr = $"\n	   t.품명   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.품명   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.품명   LIKE '%{product}%'                                                                         ");
                }
            }

            if (!(origin == null || string.IsNullOrEmpty(origin)))
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
                            tempWhr = $"\n	   t.원산지   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.원산지   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.원산지   LIKE '%{origin}%'                                                                         ");
                }
            }

            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
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
                            tempWhr = $"\n	   t.규격   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.규격   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.규격   LIKE '%{sizes}%'                                                                         ");
                }
            }

            if (!(manager1 == null || string.IsNullOrEmpty(manager1)))
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
                            tempWhr = $"\n	   t.담당자1   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.담당자1   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.담당자1   LIKE '%{manager1}%'                                                                         ");
                }
            }
            if (!(manager2 == null || string.IsNullOrEmpty(manager2)))
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
                            tempWhr = $"\n	   t.담당자2   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.담당자2   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.담당자2   LIKE '%{manager2}%'                                                                         ");
                }
            }
            if (!(division == null || string.IsNullOrEmpty(division)))
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
                            tempWhr = $"\n	   t.구분   LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.구분   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.구분  LIKE '%{division}%'                                                                         ");
                }
            }

            qry.Append($"\n GROUP BY  대분류1, 대분류, 원산지, 품명, 규격, 단위	, 가격단위, 단위수량, SEAOVER단위, 구분, 담당자1, 담당자2                 ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductTable()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                ");
            qry.Append($"\n   대분류                                                                                                ");
            qry.Append($"\n , 원산지코드                                                                                              ");
            qry.Append($"\n , 품목코드                                                                                               ");
            qry.Append($"\n , 규격코드                                                                                               ");
            qry.Append($"\n , 단위                                                                                                 ");
            qry.Append($"\n , 가격단위                                                                                               ");
            qry.Append($"\n , 단위수량                                                                                               ");
            qry.Append($"\n , SEAOVER단위                                                                                          ");
            qry.Append($"\n , ISNULL(매출단가, 0) AS 매출단가                                                                            ");
            qry.Append($"\n , 구분                                                                                                 ");
            qry.Append($"\n , MAX(CONVERT(VARCHAR(10), 단가수정일, 120))    AS 단가수정일                                                                             ");
            qry.Append($"\n , 담당자1                                                                                                ");
            qry.Append($"\n FROM 업체별시세관리                                                                                         ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                 ");
            qry.Append($"\n   AND 대분류 <> ''                                                                                      ");
            qry.Append($"\n   AND 대분류 IS NOT NULL                                                                                ");
            qry.Append($"\n GROUP BY  대분류, 원산지코드, 품목코드, 규격코드, 단위	, 가격단위, 단위수량, SEAOVER단위, 매출단가, 구분, 단가수정일, 담당자1                    ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProductTable2(string product = null, bool isExactly = false, string origin = null, string sizes = null, string unit = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                   ");
            qry.Append($"\n   t.*                                                                                                                    ");
            qry.Append($"\n FROM (                                                                                                                   ");
            qry.Append($"\n 	SELECT                                                                                                               ");
            qry.Append($"\n 	 t.*                                                                                                                 ");
            qry.Append($"\n 	, CASE                                                                                                               ");
            qry.Append($"\n 	     WHEN t.규격2 = '.' THEN NULL                                                                                    ");
            qry.Append($"\n 		  WHEN t.규격2 = '-' THEN NULL                                                                                   ");
            qry.Append($"\n 		  WHEN ISNUMERIC(t.규격2) = 1 THEN CONVERT(float, t.규격2)                                                       ");
            qry.Append($"\n 		  ELSE NULL                                                                                                      ");
            qry.Append($"\n 	 END AS 규격3                                                                                                        ");
            qry.Append($"\n 	, CASE                                                                                                               ");
            qry.Append($"\n 		  WHEN t.규격2 = '.' THEN t.규격2                                                                                ");
            qry.Append($"\n 		  WHEN t.규격2 = '-' THEN t.규격2                                                                                ");
            qry.Append($"\n 		  WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                          ");
            qry.Append($"\n 		  ELSE t.규격2                                                                                                   ");
            qry.Append($"\n 	 END AS 규격4                                                                                                        ");
            qry.Append($"\n 	FROM(                                                                                                                ");
            //qry.Append($"\n 		 SELECT TOP 1000                                                                                                 ");
            qry.Append($"\n 		 SELECT                                                                                                          ");
            qry.Append($"\n 		   ISNULL(원산지, '') AS 원산지                                                                                  ");
            qry.Append($"\n 		 , ISNULL(품명, '') AS 품명                                                                                      ");
            qry.Append($"\n 		 , ISNULL(규격, '') AS 규격                                                                                      ");
            qry.Append($"\n 		 , CASE                                                                                                          ");
            qry.Append($"\n 		  WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                               ");
            qry.Append($"\n 		  	CASE                                                                                                         ");
            qry.Append($"\n 			  	WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))         ");
            qry.Append($"\n 			  	WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격))        ");
            qry.Append($"\n 		  	END                                                                                                          ");
            qry.Append($"\n 		  WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                  ");
            qry.Append($"\n 		  WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                    ");
            qry.Append($"\n 		  WHEN CHARINDEX('UP', 규격) > 0 THEN  SUBSTRING(규격, 0, CHARINDEX('UP', 규격))                                 ");
            qry.Append($"\n 		  ELSE 규격 END AS 규격2                                                                                         ");
            qry.Append($"\n 		 , ISNULL(SEAOVER단위, '') AS SEAOVER단위                                                                        ");
            qry.Append($"\n 		 , CASE                                                                                                          ");
            qry.Append($"\n 		 	WHEN 가격단위 = '팩' THEN 단위                                                                               ");
            qry.Append($"\n 		 	ELSE SEAOVER단위 END AS 단위                                                                                 ");
            qry.Append($"\n 		 , CASE WHEN 가격단위 LIKE '%팩%' AND 단위수량 > 0 THEN 단위수량 ELSE 0 END AS 트레이                            ");
            qry.Append($"\n 		 FROM 업체별시세관리                                                                                             ");
            qry.Append($"\n 		 WHERE 사용자 = '{userId}'                                                                                       ");
            string[] tempStr = null;
            string tempWhr = "";
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                if (!isExactly)
                {
                    tempStr = null;
                    tempWhr = "";
                    tempStr = product.Split(' ');
                    if (tempStr.Length > 1)
                    {
                        for (int i = 0; i < tempStr.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                            {
                                if (string.IsNullOrEmpty(tempWhr))
                                    tempWhr = $"\n	   품명  LIKE '%{tempStr[i]}%' ";
                                else
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
                else
                    qry.Append($"\n	  AND 품명 LIKE '%{product}%'                                                                         ");
            }

            if (!(origin == null || string.IsNullOrEmpty(origin)))
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

            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
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

            if (!(unit == null || string.IsNullOrEmpty(unit)))
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
                            tempWhr = $"\n	   (SEAOVER단위 LIKE '%{tempStr[i]}%' OR 단위 LIKE '%{tempStr[i]}%')";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR (SEAOVER단위 LIKE '%{tempStr[i]}%' OR 단위 LIKE '%{tempStr[i]}%')";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND (SEAOVER단위 LIKE '%{unit}%' OR 단위 LIKE '%{unit}%')                                                                        ");
                }
            }
            qry.Append($"\n 	) AS t                                                                                                          ");
            qry.Append($"\n 	GROUP BY t.원산지, t.품명, t.규격, t.규격2, t.SEAOVER단위, t.단위, t.트레이                                     ");
            qry.Append($"\n ) AS t                                                                                                              ");
            qry.Append($"\n ORDER BY t.품명, t.원산지, t.규격3                                                                                  ");
            qry.Append($"\n , CASE WHEN CHARINDEX('대', t.규격4) > 0  THEN 1                                                                    ");
            qry.Append($"\n        WHEN CHARINDEX('중', t.규격4) > 0 THEN 2                                                                     ");
            qry.Append($"\n        WHEN CHARINDEX('소', t.규격4) > 0 THEN 3                                                                     ");
            qry.Append($"\n        WHEN CHARINDEX('L', t.규격4) > 0 THEN 4                                                                      ");
            qry.Append($"\n        WHEN CHARINDEX('M', t.규격4) > 0 THEN 5                                                                      ");
            qry.Append($"\n        WHEN CHARINDEX('S', t.규격4) > 0 THEN 6                                                                      ");
            qry.Append($"\n        END, t.규격, t.SEAOVER단위                                                                                          ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetProductTable3(string product = null, string origin = null, int maxCount = 100, bool isExactly = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                    ");
            qry.Append($"\n    품명                                    ");
            qry.Append($"\n  , 원산지                                  ");
            qry.Append($"\n  FROM 업체별시세관리                       ");
            qry.Append($"\n  WHERE 사용자 = '{userId}'                   ");

            if (isExactly)
            {
                if(!string.IsNullOrEmpty(product.Trim()))
                    qry.Append($"\n    AND 품명 = '{product}'                  ");
                if (!string.IsNullOrEmpty(origin.Trim()))
                    qry.Append($"\n    AND 원산지 = '{origin}'                  ");
            }
            else
            {
                if (!string.IsNullOrEmpty(product.Trim()))
                    qry.Append($"\n    {commonRepository .whereSql("품명", product)}                   ");
                if (!string.IsNullOrEmpty(origin.Trim()))
                    qry.Append($"\n    {commonRepository .whereSql("원산지", origin)}                   ");

            }
            qry.Append($"\n  GROUP BY 품명, 원산지                     ");
            qry.Append($"\n  ORDER BY 품명, 원산지                     ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public List<SeaoverPriceModel> GetAllProduct(string category, string product, string origin, string sizes, string division)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                       ");
            qry.Append($"\n   t.*                                                                                                                        ");
            qry.Append($"\n FROM(                                                                                                                        ");
            qry.Append($"\n 	SELECT                                                                                                                   ");
            qry.Append($"\n 	  t.*                                                                                                                    ");
            qry.Append($"\n 	, CASE                                                                                                                   ");
            qry.Append($"\n 	      WHEN t.규격2 = '.' THEN NULL                                                                                         ");
            qry.Append($"\n 		  WHEN t.규격2 = '-' THEN NULL                                                                                         ");
            qry.Append($"\n 		  WHEN ISNUMERIC(t.규격2) = 1 THEN CONVERT(float, t.규격2)                                                               ");
            qry.Append($"\n 		  ELSE NULL                                                                                                          ");
            qry.Append($"\n 	  END AS 규격3                                                                                                             ");
            qry.Append($"\n 	, CASE                                                                                                                   ");
            qry.Append($"\n 		  WHEN t.규격2 = '.' THEN t.규격2                                                                                        ");
            qry.Append($"\n 		  WHEN t.규격2 = '-' THEN t.규격2                                                                                        ");
            qry.Append($"\n 		  WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                ");
            qry.Append($"\n 		  ELSE t.규격2                                                                                                         ");
            qry.Append($"\n 	  END AS 규격4                                                                                                             ");
            qry.Append($"\n 	FROM(                                                                                                                    ");
            qry.Append($"\n 		SELECT                                                                                                               ");
            qry.Append($"\n 		  대분류                                                                                                                ");
            qry.Append($"\n 		, 원산지코드                                                                                                             ");
            qry.Append($"\n 		, 원산지                                                                                                                ");
            qry.Append($"\n 		, 품목코드                                                                                                               ");
            qry.Append($"\n 		, 품명                                                                                                                 ");
            qry.Append($"\n 		, 규격코드                                                                                                               ");
            qry.Append($"\n 		, 규격                                                                                                                 ");
            qry.Append($"\n 		, CASE                                                                                                               ");
            qry.Append($"\n 			  WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                                   ");
            qry.Append($"\n 			  	CASE                                                                                                         ");
            qry.Append($"\n 				  	WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))      ");
            qry.Append($"\n 				  	WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격))     ");
            qry.Append($"\n 			  	END                                                                                                       ");
            qry.Append($"\n 			  WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                               ");
            qry.Append($"\n 			  WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                 ");
            qry.Append($"\n 			  ELSE 규격                                                                                                     ");
            qry.Append($"\n 		  END AS 규격2                                                                                                      ");
            qry.Append($"\n 		, 단위                                                                                                              ");
            qry.Append($"\n 		, 가격단위                                                                                                            ");
            qry.Append($"\n 		, 단위수량                                                                                                            ");
            qry.Append($"\n 		, SEAOVER단위                                                                                                       ");
            qry.Append($"\n 		, ISNULL(매출단가, 0) AS 매출단가                                                                                        ");
            qry.Append($"\n 		, 구분                                                                                                              ");
            qry.Append($"\n 		, 담당자1                                                                                                              ");
            qry.Append($"\n 		, CONVERT(VARCHAR(10), 단가수정일, 120)    AS 단가수정일                                                                     ");
            qry.Append($"\n 		, 비고1    AS 비고1                                                                                                            ");
            qry.Append($"\n 		FROM 업체별시세관리                                                                                                     ");
            qry.Append($"\n 		WHERE 사용자 = '{userId}'                                                                                              ");
            qry.Append($"\n 		  AND 대분류 <> ''                                                                                                   ");
            qry.Append($"\n 		  AND 대분류 IS NOT NULL                                                                                             ");
            if (!string.IsNullOrEmpty(category))
            {
                qry.Append($"\n	   AND 대분류 LIKE '%{category}%'                                                                   ");
            }
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n	   AND 품명 LIKE '%{product}%'                                                                      ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n	   AND 원산지 LIKE '%{origin}%'                                                                     ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n	   AND 규격 LIKE '%{sizes}%'                                                                        ");
            }
            if (!string.IsNullOrEmpty(division))
            {
                qry.Append($"\n	   AND 구분 = '{division}'                                                                          ");
            }
            qry.Append($"\n 		GROUP BY 대분류, 원산지코드, 원산지, 품목코드, 품명, 규격코드, 규격, 단위, 가격단위, 단위수량, SEAOVER단위, 매출단가, 구분, 담당자1, 단가수정일, 비고1                ");
            qry.Append($"\n 	) AS t                                                                                                       ");
            qry.Append($"\n ) AS t                                                                                                           ");
            qry.Append($"\n ORDER BY SUBSTRING(t.대분류, 0, 3), CAST(SUBSTRING(t.대분류, 3, 3) AS float), t.품명, t.원산지, t.규격3                     ");
            qry.Append($"\n , CASE WHEN CHARINDEX('대', t.규격4) > 0  THEN 1                                                                    ");
            qry.Append($"\n        WHEN CHARINDEX('중', t.규격4) > 0 THEN 2                                                                     ");
            qry.Append($"\n        WHEN CHARINDEX('소', t.규격4) > 0 THEN 3                                                                     ");
            qry.Append($"\n        WHEN CHARINDEX('L', t.규격4) > 0 THEN 4                                                                     ");
            qry.Append($"\n        WHEN CHARINDEX('M', t.규격4) > 0 THEN 5                                                                     ");
            qry.Append($"\n        WHEN CHARINDEX('S', t.규격4) > 0 THEN 6                                                                     ");
            qry.Append($"\n        END                                                                                                       ");

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetAllProductModel(dr);
        }
        private List<SeaoverPriceModel> SetAllProductModel(SqlDataReader rd)
        {
            List<SeaoverPriceModel> list = new List<SeaoverPriceModel>();
            while (rd.Read())
            {
                SeaoverPriceModel model = new SeaoverPriceModel();
                model.category = rd["대분류"].ToString();
                model.purchase_date = rd["매입일자"].ToString();
                model.purchase_co = rd["매입처"].ToString();
                model.origin_code = rd["원산지코드"].ToString();
                model.origin = rd["원산지"].ToString();
                model.warehouse = rd["보관처"].ToString();
                model.product_code = rd["품목코드"].ToString();
                model.product = rd["품명"].ToString();
                model.sizes_code = rd["규격코드"].ToString();
                model.sizes = rd["규격"].ToString();
                model.unit = rd["단위"].ToString();
                model.price_unit = rd["가격단위"].ToString();
                model.unit_count = rd["단위수량"].ToString();
                model.seaover_unit = rd["SEAOVER단위"].ToString();
                model.note = rd["적요"].ToString();
                model.edit_date = rd["단가수정일"].ToString();
                model.sales_price = Convert.ToDouble(rd["매출단가"].ToString());
                model.purchase_price = Convert.ToDouble(rd["매입단가"].ToString());
                model.manager1 = rd["담당자1"].ToString();
                model.manager2 = rd["담당자2"].ToString();
                model.division = rd["구분"].ToString();
                model.remark1 = rd["비고1"].ToString();
                model.remark2 = rd["비고2"].ToString();
                list.Add(model);
            }
            rd.Close();
            return list;
        }

        public DataTable GetCurrentProduct(string categoryCode, string category, string product, string origin, string sizes, string unit, string division, string manager, string warehouse
                                    , bool chkDIv, bool chkPrice, bool overAveragePrice, string isVat, bool isStock, int increaseSalePrice)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                                                                                                      ");
            qry.Append($"\n   대분류                                                                                                                                                                                                       ");
            qry.Append($"\n   , 대분류1                                                                                                                                                                                                    ");
            qry.Append($"\n   , 대분류2                                                                                                                                                                                                    ");
            qry.Append($"\n   , 대분류3                                                                                                                                                                                                    ");
            qry.Append($"\n   , 원산지코드                                                                                                                                                                                                   ");
            qry.Append($"\n   , 원산지                                                                                                                                                                                                     ");
            qry.Append($"\n   , 품목코드                                                                                                                                                                                                    ");
            qry.Append($"\n   , 품명                                                                                                                                                                                                      ");
            qry.Append($"\n   , 규격코드                                                                                                                                                                                                    ");
            qry.Append($"\n   , 규격                                                                                                                                                                                                      ");
            qry.Append($"\n   , 규격2                                                                                                                                                                                                     ");
            qry.Append($"\n   , 규격3                                                                                                                                                                                                     ");
            qry.Append($"\n   , 규격4                                                                                                                                                                                                     ");
            qry.Append($"\n   , 단위                                                                                                                                                                                                      ");
            qry.Append($"\n   , 가격단위                                                                                                                                                                                                    ");
            qry.Append($"\n   , 단위수량                                                                                                                                                                                                    ");
            qry.Append($"\n   , SEAOVER단위                                                                                                                                                                                               ");
            qry.Append($"\n   , '0  ' AS 트레이                                                                                                                                                                                            ");
            qry.Append($"\n   , 매출단가                                                                                                                                                                                                    ");
            qry.Append($"\n   , 구분                                                                                                                                                                                                      ");
            qry.Append($"\n   , CASE WHEN 단가수정일 = '1900-01-01' THEN '' ELSE 단가수정일 END AS 단가수정일                                                                                                                                         ");
            qry.Append($"\n   , 담당자1                                                                                                                                                                                                    ");
            qry.Append($"\n   , 비고1                                                                                                                                                                                                     ");
            qry.Append($"\n   , CASE WHEN 부가세유무 = 'Y' THEN '과세' ELSE '면세' END AS 부가세                                                                                                                                                     ");
            qry.Append($"\n   , 재고수                                                                                                                                                                                                     ");
            qry.Append($"\n   , 예약수                                                                                                                                                                                                     ");
            qry.Append($"\n   , '0' AS 선적수                                                                                                                                                                                                     ");
            qry.Append($"\n   , 창고                                                                                                                                                                                                      ");
            qry.Append($"\n   , 일반시세                                                                                                                                                                                                    ");
            qry.Append($"\n  FROM(                                                                                                                                                                                                       ");
            qry.Append($"\n  	SELECT                                                                                                                                                                                                   ");
            qry.Append($"\n  	  t.*                                                                                                                                                                                                    ");
            qry.Append($"\n   	, CASE                                                                                                                                                                                                   ");
            qry.Append($"\n   	      WHEN t.규격2 = '.' THEN NULL                                                                                                                                                                        ");
            qry.Append($"\n   		  WHEN t.규격2 = '-' THEN NULL                                                                                                                                                                        ");
            qry.Append($"\n   		  WHEN ISNUMERIC(t.규격2) = 1 THEN CONVERT(float, t.규격2)                                                                                                                                              ");
            qry.Append($"\n   		  WHEN CHARINDEX('S', 규격) > 0 THEN 1                                                                                                                                                                ");
            qry.Append($"\n   		  WHEN CHARINDEX('M', 규격) > 0 THEN 2                                                                                                                                                                ");
            qry.Append($"\n   		  WHEN CHARINDEX('L', 규격) > 0 THEN 3                                                                                                                                                                ");
            qry.Append($"\n   		  WHEN CHARINDEX('G*', 규격) > 0 THEN PATINDEX('%[^a-zA-Z0-9 ]%', 규격)                                                                                                                                 ");
            qry.Append($"\n   		  ELSE NULL                                                                                                                                                                                          ");
            qry.Append($"\n   	  END AS 규격3                                                                                                                                                                                            ");
            qry.Append($"\n   	, CASE                                                                                                                                                                                                   ");
            qry.Append($"\n   		  WHEN t.규격2 = '.' THEN t.규격2                                                                                                                                                                       ");
            qry.Append($"\n   		  WHEN t.규격2 = '-' THEN t.규격2                                                                                                                                                                       ");
            qry.Append($"\n   		  WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                                                                                                                                                               ");
            qry.Append($"\n   		  WHEN CHARINDEX('S', 규격) > 0 THEN                                                                                                                                                                  ");
            qry.Append($"\n   		  	CASE WHEN CHARINDEX('4', 규격) > 0 THEN '11'                                                                                                                                                      ");
            qry.Append($"\n   		  		WHEN CHARINDEX('3', 규격) > 0 THEN '12'                                                                                                                                                       ");
            qry.Append($"\n   		  		WHEN CHARINDEX('2', 규격) > 0 THEN '13'                                                                                                                                                       ");
            qry.Append($"\n   		  		WHEN CHARINDEX('1', 규격) > 0 THEN '14'                                                                                                                                                       ");
            qry.Append($"\n   		  		ELSE '10' END                                                                                                                                                                                ");
            qry.Append($"\n   		  WHEN CHARINDEX('M', 규격) > 0 THEN                                                                                                                                                                  ");
            qry.Append($"\n   		  	CASE WHEN CHARINDEX('1', 규격) > 0 THEN '21'                                                                                                                                                      ");
            qry.Append($"\n   		  		WHEN CHARINDEX('2', 규격) > 0 THEN '22'                                                                                                                                                       ");
            qry.Append($"\n   		  		WHEN CHARINDEX('3', 규격) > 0 THEN '23'                                                                                                                                                       ");
            qry.Append($"\n   		  		WHEN CHARINDEX('4', 규격) > 0 THEN '24'                                                                                                                                                       ");
            qry.Append($"\n   		  		ELSE '20' END                                                                                                                                                                                ");
            qry.Append($"\n   		  WHEN CHARINDEX('L', 규격) > 0 THEN                                                                                                                                                                  ");
            qry.Append($"\n   		  	CASE WHEN CHARINDEX('1', 규격) > 0 THEN '31'                                                                                                                                                      ");
            qry.Append($"\n   		  		WHEN CHARINDEX('2', 규격) > 0 THEN '32'                                                                                                                                                       ");
            qry.Append($"\n   		  		WHEN CHARINDEX('3', 규격) > 0 THEN '33'                                                                                                                                                       ");
            qry.Append($"\n   		  		WHEN CHARINDEX('4', 규격) > 0 THEN '34'                                                                                                                                                       ");
            qry.Append($"\n   		  		ELSE '30' END                                                                                                                                                                                ");
            qry.Append($"\n   		  ELSE t.규격2                                                                                                                                                                                        ");
            qry.Append($"\n   	  END AS 규격4                                                                                                                                                                                            ");
            qry.Append($"\n  	FROM(                                                                                                                                                                                                    ");
            qry.Append($"\n  		SELECT                                                                                                                                                                                               ");
            qry.Append($"\n  	  		  CASE                                                                                                                                                                                           ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Aa' THEN '라운드새우류'                                                                                                                               ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ab' THEN '새우살류'                                                                                                                                 ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ba' THEN '쭈꾸미류'                                                                                                                                 ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bb' THEN '낙지류'                                                                                                                                  ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bc' THEN '갑오징어류'                                                                                                                                ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bd' THEN '오징어류'                                                                                                                                 ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Be' THEN '문어류'                                                                                                                                  ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'C' THEN '갑각류'                                                                                                                                   ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Da' THEN '어패류'                                                                                                                                  ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Db' THEN '살류'                                                                                                                                   ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Dc' THEN '해물류'                                                                                                                                  ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 3)) = 'Eab' THEN '선어특수/가공품'                    ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ea' THEN '초밥류'                              ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Eb' THEN '기타수산가공품'                      ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ec' THEN '기타식품가공품'                      ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ed' THEN '축산가공품'                          ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ee' THEN '야채과일류'                          ");
            qry.Append($"\n  	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'F' THEN '선어류'                                                                                                                                   ");
            qry.Append($"\n  	  		END AS 대분류                                                                                                                                                                                      ");
            qry.Append($"\n  	  		,  대분류 AS 대분류1                                                                                                                                                                                  ");
            qry.Append($"\n 			,  CASE WHEN ISNUMERIC(SUBSTRING(t.대분류, 3, 1)) <> 1 THEN SUBSTRING(대분류, 1, 3)             ");
            qry.Append($"\n 			        WHEN ISNUMERIC(SUBSTRING(t.대분류, 2, 1)) <> 1 THEN SUBSTRING(대분류, 1, 2)             ");
            qry.Append($"\n 				    ELSE SUBSTRING(대분류, 1, 1) END AS 대분류2                                             ");
            qry.Append($"\n 			,  CASE WHEN ISNUMERIC(SUBSTRING(t.대분류, 3, 1)) <> 1 THEN SUBSTRING(대분류, 4, 3)             ");
            qry.Append($"\n 			        WHEN ISNUMERIC(SUBSTRING(t.대분류, 2, 1)) <> 1 THEN SUBSTRING(대분류, 3, 3)             ");
            qry.Append($"\n 				    ELSE SUBSTRING(대분류, 2, 3) END AS 대분류3                                             ");
            qry.Append($"\n  	 		, 원산지코드                                                                                                                                                                                         ");
            qry.Append($"\n  	 		, 원산지                                                                                                                                                                                           ");
            qry.Append($"\n  	 		, 품목코드                                                                                                                                                                                          ");
            qry.Append($"\n  	 		, 품명                                                                                                                                                                                            ");
            qry.Append($"\n  	 		, 규격코드                                                                                                                                                                                          ");
            qry.Append($"\n  	 		, 규격                                                                                                                                                                                            ");
            qry.Append($"\n  	 		, CASE                                                                                                                                                                                           ");
            qry.Append($"\n  	 			  WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                                                                                                               ");
            qry.Append($"\n  	 			  	CASE                                                                                                                                                                                     ");
            qry.Append($"\n  	 				  	WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                                                                             ");
            qry.Append($"\n  	 				  	WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                                                                            ");
            qry.Append($"\n  	 			  	END                                                                                                                                                                                      ");
            qry.Append($"\n  	 			  WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                                                                                                     ");
            qry.Append($"\n  	 			  WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                                                                                                     ");
            qry.Append($"\n  	 			  ELSE 규격                                                                                                                                                                                   ");
            qry.Append($"\n  	 		  END AS 규격2                                                                                                                                                                                    ");
            qry.Append($"\n  	 		, 단위                                                                                                                                                                                            ");
            qry.Append($"\n   	 		, 가격단위                                                                                                                                                                                          ");
            qry.Append($"\n   	 		, 단위수량                                                                                                                                                                                          ");
            qry.Append($"\n   	 		, SEAOVER단위                                                                                                                                                                                     ");
            qry.Append($"\n  	 		, ISNULL(매출단가, 0) AS 매출단가                                                                                                                                                                       ");
            qry.Append($"\n  	 		, ISNULL(구분, '') AS 구분                                                                                                                                                                          ");
            qry.Append($"\n  	 		, MAX(CONVERT(VARCHAR(10), ISNULL(단가수정일, '1900-01-01'), 120))    AS 단가수정일                                                                                                                      ");
            qry.Append($"\n  	 		, 부가세유무                                                                                                                                                                                         ");
            qry.Append($"\n  	 		, 재고수                                                                                                                                                                                           ");
            qry.Append($"\n      		, 예약수                                                                                                                                                                                           ");
            qry.Append($"\n      		, 담당자1                                                                                                                                                                                          ");
            qry.Append($"\n      		, 담당자2                                                                                                                                                                                          ");
            qry.Append($"\n      		, 비고1                                                                                                                                                                                           ");
            qry.Append($"\n      		, 창고                                                                                                                                                                                            ");
            qry.Append($"\n      		, 일반시세                                                                                                                                                                                          ");
            qry.Append($"\n  	 		FROM (                                                                                                                                                                                           ");
            qry.Append($"\n 	 	 		SELECT                                                                                                                                                                                       ");
            qry.Append($"\n 				  t1.*                                                                                                                                                                                       ");
            qry.Append($"\n 				, t2.재고수 - t2.예약수  AS 재고수                                                                                                                                                       ");
            qry.Append($"\n 				, t2.예약수                                                                                                                                                                                    ");
            qry.Append($"\n 				, t2.창고                                                                                                                                                                                     ");
            qry.Append($"\n 				FROM (                                                                                                                                                                                       ");
            qry.Append($"\n 				    SELECT                                                                                                                                                                                   ");
            qry.Append($"\n 				      t1.*                                                                                                                                                                                   ");
            qry.Append($"\n 				    , ISNULL(t2.일반시세, 0) AS 일반시세                                                                                                                                                            ");
            qry.Append($"\n 				    FROM (                                                                                                                                                                                   ");
            qry.Append($"\n 						SELECT                                                                                                                                                                               ");
            qry.Append($"\n 					      *                                                                                                                                                                                  ");
            qry.Append($"\n 					    FROM (                                                                                                                                                                               ");
            qry.Append($"\n 						    SELECT                                                                                                                                                                           ");
            qry.Append($"\n 						      ROW_NUMBER() OVER(PARTITION BY 대분류, 원산지, 품명, 규격, 단위, ISNULL(가격단위, ''), 단위수량, SEAOVER단위 ORDER BY 단가수정일 DESC) AS Row#                                                        ");
            qry.Append($"\n 						      , *                                                                                                                                                                            ");
            qry.Append($"\n 						    FROM 업체별시세관리                                                                                                                                                                    ");
            qry.Append($"\n 			 	 		    WHERE 사용자 = '{userId}'                                                                                                                                                            ");
            //매출단가 인상/하 필터
            if(increaseSalePrice == 2)
                qry.Append($"\n 			 	 		      AND ISNULL(매출단가, 0) > ISNULL(이전매출단가, 0)                                                                                                                       ");
            else if (increaseSalePrice == 3)
                qry.Append($"\n 			 	 		      AND ISNULL(매출단가, 0) < ISNULL(이전매출단가, 0)                                                                                                                       ");
            //매입단가 0초과
            if (chkPrice)
                qry.Append($"\n 		  AND 매입단가 > 0                                                                                              ");
            //대분류 있는 품목만
            if(chkDIv)
                qry.Append($"\n 		  AND ISNULL(대분류, '') <> ''                                                                                              ");
            //구분
            if (!string.IsNullOrEmpty(division.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("구분", division.Trim())}                                                                                                                             ");
           

            //대분류 코드
            string[] tempStr = null;
            string tempWhr = "";
            if (!string.IsNullOrEmpty(categoryCode))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = categoryCode.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{categoryCode}%'                                                                         ");
                }

            }

            //원산지
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("원산지", origin.Trim())}                                                                                                                             ");
            //품명
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("품명", product.Trim())}                                                                                                                             ");
            //규격
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("규격", sizes.Trim())}                                                                                                                             ");
            //단위
            if (!string.IsNullOrEmpty(unit.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("단위", unit.Trim())}                                                                                                                             ");
            //담당자1
            if (!string.IsNullOrEmpty(manager.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("담당자1", manager.Trim())}                                                                                                                             ");
            qry.Append($"\n 						 ) AS T                                                                                                                                                                              ");
            qry.Append($"\n 						 WHERE Row# = 1                                                                                                                                                                      ");
            qry.Append($"\n 					 ) AS t1                                                                                                                                                                                 ");
            qry.Append($"\n 					 LEFT OUTER JOIN(                                                                                                                                                                        ");
            qry.Append($"\n 						 SELECT                                                                                                                                                                              ");
            qry.Append($"\n 						  대분류                                                                                                                                                                               ");
            qry.Append($"\n 						, 품명                                                                                                                                                                                ");
            qry.Append($"\n 						, 원산지                                                                                                                                                                               ");
            qry.Append($"\n 						, 규격                                                                                                                                                                                ");
            qry.Append($"\n 						, 단위                                                                                                                                                                                ");
            qry.Append($"\n 					    , AVG(매입단가) AS 일반시세                                                                                                                                                                 ");
            qry.Append($"\n 					    FROM (                                                                                                                                                                               ");
            qry.Append($"\n 						    SELECT                                                                                                                                                                           ");
            qry.Append($"\n 						      ROW_NUMBER() OVER(PARTITION BY 대분류, 원산지, 품명, 규격, 단위, ISNULL(가격단위, ''), 단위수량, SEAOVER단위 ORDER BY 단가수정일 DESC) AS Row#                                                        ");
            qry.Append($"\n 						      , *                                                                                                                                                                            ");
            qry.Append($"\n 						    FROM 업체별시세관리                                                                                                                                                                    ");
            qry.Append($"\n 			 	 		    WHERE 사용자 = '{userId}'                                                                                                                                                            ");
            qry.Append($"\n 			 	 		      AND 단가수정일 IS NOT NULL                                                                                                                                                         ");
            qry.Append($"\n 			 	 		      AND 매입단가 > 10                                                                                                                                                                 ");
            //매입단가 0초과
            if (chkPrice)
                qry.Append($"\n 		  AND 매입단가 > 0                                                                                              ");

            //구분
            if (!string.IsNullOrEmpty(division.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("구분", division.Trim())}                                                                                                                             ");

            //대분류 코드
            if (!string.IsNullOrEmpty(categoryCode))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = categoryCode.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{categoryCode}%'                                                                         ");
                }

            }

            //원산지
            if (!string.IsNullOrEmpty(origin.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("원산지", origin.Trim())}                                                                                                                             ");
            //품명
            if (!string.IsNullOrEmpty(product.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("품명", product.Trim())}                                                                                                                             ");
            //규격
            if (!string.IsNullOrEmpty(sizes.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("규격", sizes.Trim())}                                                                                                                             ");
            //단위
            if (!string.IsNullOrEmpty(unit.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("단위", unit.Trim())}                                                                                                                             ");
            //담당자1
            if (!string.IsNullOrEmpty(manager.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("담당자1", manager.Trim())}                                                                                                                             ");
            qry.Append($"\n 						 ) AS T                                                                                                                                                                              ");
            qry.Append($"\n 						 WHERE Row# <= 3                                                                                                                                                                     ");
            qry.Append($"\n 						 GROUP BY 대분류, 품명, 원산지, 규격, 단위                                                                                                                                                     ");
            qry.Append($"\n 					 ) AS t2                                                                                                                                                                                 ");
            qry.Append($"\n 					   ON t1.대분류 = t2.대분류                                                                                                                                                                   ");
            qry.Append($"\n 					   AND t1.품명 = t2.품명					                                                                                                                                                 ");
            qry.Append($"\n 					   AND t1.원산지 = t2.원산지                                                                                                                                                                  ");
            qry.Append($"\n 					   AND t1.규격 = t2.규격                                                                                                                                                                    ");
            qry.Append($"\n 					   AND t1.단위 = t2.단위                                                                                                                                                                    ");
            qry.Append($"\n 				) AS t1                                                                                                                                                                                      ");
            if(!string.IsNullOrEmpty(warehouse.Trim()))
                qry.Append($"\n 				INNER JOIN (                                                                                                                                                                            ");
            else
                qry.Append($"\n 				LEFT OUTER JOIN (                                                                                                                                                                            ");
            qry.Append($"\n 	  	    	   SELECT 품명, 원산지, 규격, 단위, SUM(ISNULL(재고수,0)) AS 재고수, SUM(ISNULL(예약수,0)) AS 예약수, SUM(ISNULL(관리자예약수,0)) AS 관리자예약수                                                                         ");
            qry.Append($"\n 	  	    	   ,  STUFF((                                                                                                                                                                                \n");
            qry.Append(@" 		            SELECT ' \n' + 창고 + ' : 재고수(' + RTRIM(CAST(SUM(ISNULL(재고수,0))-SUM(ISNULL(예약수,0))  AS CHAR)) + '), 예약수(' + RTRIM(CAST(SUM(ISNULL(예약수,0))  AS CHAR)) + ')'       ");
            qry.Append($"\n 			            FROM 품명별재고현황 AS B                                                                                                                                                                   ");
            qry.Append($"\n 			  			WHERE 사용자 = '{userId}'                                                                                                                                                                ");
            qry.Append($"\n 			              AND B.품명 = A.품명                                                                                                                                                                   ");
            qry.Append($"\n 			              AND B.원산지 = A.원산지                                                                                                                                                                 ");
            qry.Append($"\n 			              AND B.규격 = A.규격                                                                                                                                                                   ");
            qry.Append($"\n 			              AND B.단위 = A.단위                                                                                                                                                                   ");
            qry.Append($"\n 			              AND ISNULL(재고수,0) > 0                                                                                                                                        ");
            qry.Append($"\n 			            GROUP BY 품명, 원산지, 규격, 단위, 창고                                                                                                                                                       ");
            qry.Append($"\n 			            FOR XML PATH('')                                                                                                                                                                     ");
            qry.Append($"\n 			       ),1,1,'') AS 창고                                                                                                                                                                          ");
            qry.Append($"\n 		  			FROM 품명별재고현황 AS A                                                                                                                                                                       ");
            qry.Append($"\n 		  			WHERE 사용자 = '{userId}'                                                                                                                                                                    ");
            //창고
            if (!string.IsNullOrEmpty(warehouse.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("창고", warehouse.Trim())}                                                                                                                             ");
            qry.Append($"\n 		  			GROUP BY 품명, 원산지, 규격, 단위                                                                                                                                                                ");
            qry.Append($"\n 	  			) AS t2                                                                                                                                                                                      ");
            qry.Append($"\n 	  			  ON t1.원산지 = t2.원산지                                                                                                                                                                        ");
            qry.Append($"\n 	    		  AND t1.품명 = t2.품명                                                                                                                                                                         ");
            qry.Append($"\n 	    		  AND t1.규격 = t2.규격                                                                                                                                                                         ");
            qry.Append($"\n 	  	    	  AND REPLACE(t1.SEAOVER단위, 'kg', '') = REPLACE(t2.단위, 'kg', '')                                                                                                                            ");
            qry.Append($"\n 			) AS t                                                                                                                                                                                           ");
            qry.Append($"\n  	 		WHERE 1=1                                                                                                                                                                                        ");
            qry.Append($"\n   	 		GROUP BY 대분류, 원산지코드, 원산지, 품목코드, 품명, 규격코드, 규격, 단위, 가격단위, 단위수량, SEAOVER단위, 매출단가, 구분, 부가세유무, 재고수, 예약수, 담당자1, 담당자2, 비고1, 부가세유무, 창고, 일반시세                                                    ");
            qry.Append($"\n   		) AS t                                                                                                                                                                                               ");
            qry.Append($"\n    ) AS t                                                                                                                                                                                                    ");
            qry.Append($"\n  WHERE 1=1                                                                                                                                                                                                   ");
            //대분류(한글)
            if (!string.IsNullOrEmpty(category.Trim()))
                qry.Append($"\n		   	 		      {commonRepository .whereSql("t.대분류", category.Trim())}                                                                                                                             ");

            //일반시세보다 싼
            if(overAveragePrice)
                qry.Append($"\n    AND t.매출단가 <= t.일반시세                                                                                                                                                                                                   ");

            //과세여부
            if(!string.IsNullOrEmpty(isVat.Trim()) && isVat != "전체")
                qry.Append($"\n    AND CASE WHEN 부가세유무 = 'Y' THEN '과세' ELSE '면세' END = '{isVat.Trim()}'                                                                                                                                                                                                   ");

            /*if(isStock)
                qry.Append($"\n    AND ISNULL(재고수, 0) - ISNULL(예약수, 0) > 0                                                                                                                                                                                                   ");*/

            qry.Append($"\n  GROUP BY 대분류2, 대분류3, 대분류1, 대분류, 원산지코드, 원산지, 품목코드, 품명, 규격코드, 규격, 규격2, 규격3, 규격4, 단위, 가격단위, 단위수량, SEAOVER단위, 매출단가, 구분, 단가수정일, 담당자1, 비고1, 부가세유무, 재고수, 예약수, 창고, 일반시세                                  ");
            qry.Append($"\n  ORDER BY t.대분류2, t.대분류3, t.품명, t.원산지, t.규격3                                                                                                                                                               ");
            qry.Append($"\n  , CASE WHEN CHARINDEX('대', t.규격4) > 0  THEN '1'                                                                                                                                                            ");
            qry.Append($"\n         WHEN CHARINDEX('중', t.규격4) > 0 THEN '2'                                                                                                                                                             ");
            qry.Append($"\n         WHEN CHARINDEX('소', t.규격4) > 0 THEN '3'                                                                                                                                                             ");
            qry.Append($"\n         ELSE t.규격4 END                                                                                                                                                                                      ");
            qry.Append($"\n  , 단가수정일 DESC                                                                                                                                                                                               ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetCurrentMinPriceProduct(string categoryCode, string category, string product, string origin, string sizes, string unit, string division, string manager, int avgCnt)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                         ");
            qry.Append($"\n  대분류                                                                                                                                           ");
            qry.Append($"\n  , 대분류1                                                                                                                                        ");
            qry.Append($"\n  , 대분류2                                                                                                                                        ");
            qry.Append($"\n  , 대분류3                                                                                                                                        ");
            qry.Append($"\n  , 원산지코드                                                                                                                                      ");
            qry.Append($"\n  , 원산지                                                                                                                                         ");
            qry.Append($"\n  , 품목코드                                                                                                                                        ");
            qry.Append($"\n  , 품명                                                                                                                                          ");
            qry.Append($"\n  , 규격코드                                                                                                                                        ");
            qry.Append($"\n  , 규격                                                                                                                                          ");
            qry.Append($"\n  , 규격2                                                                                                                                         ");
            qry.Append($"\n  , 규격3                                                                                                                                         ");
            qry.Append($"\n  , 규격4                                                                                                                                         ");
            qry.Append($"\n  , 단위                                                                                                                                          ");
            qry.Append($"\n  , 가격단위                                                                                                                                        ");
            qry.Append($"\n  , 단위수량                                                                                                                                        ");
            qry.Append($"\n  , SEAOVER단위                                                                                                                                   ");
            qry.Append($"\n  , '0  ' AS 트레이                                                                                                              ");
            qry.Append($"\n  , 매출단가                                                                                                                                        ");
            qry.Append($"\n  , 구분                                                                                                                                          ");
            qry.Append($"\n  , CASE WHEN 단가수정일 = '1900-01-01' THEN '' ELSE 단가수정일 END AS 단가수정일                                                                                                                              ");
            qry.Append($"\n  , 담당자1                                                                                                                                        ");
            qry.Append($"\n  , 비고1                                                                                                                                        ");
            qry.Append($"\n  , CASE WHEN 부가세유무 = 'Y' THEN '과세' ELSE '' END AS 부가세                                                                                                                                        ");
            qry.Append($"\n FROM(                                                                                                                                          ");
            qry.Append($"\n 	SELECT                                                                                                                                     ");
            qry.Append($"\n 	  t.*                                                                                                                                      ");
            qry.Append($"\n  	, CASE                                                                 ");
            qry.Append($"\n  	      WHEN t.규격2 = '.' THEN NULL                                       ");
            qry.Append($"\n  		  WHEN t.규격2 = '-' THEN NULL                                       ");
            qry.Append($"\n  		  WHEN ISNUMERIC(t.규격2) = 1 THEN CONVERT(float, t.규격2)             ");
            qry.Append($"\n  		  WHEN CHARINDEX('S', 규격) > 0 THEN 1                               ");
            qry.Append($"\n  		  WHEN CHARINDEX('M', 규격) > 0 THEN 2                               ");
            qry.Append($"\n  		  WHEN CHARINDEX('L', 규격) > 0 THEN 3                               ");
            qry.Append($"\n  		  WHEN CHARINDEX('G*', 규격) > 0 THEN PATINDEX('%[^a-zA-Z0-9 ]%', 규격)         ");
            qry.Append($"\n  		  ELSE NULL                                                        ");
            qry.Append($"\n  	  END AS 규격3                                                           ");
            qry.Append($"\n  	, CASE                                                                 ");
            qry.Append($"\n  		  WHEN t.규격2 = '.' THEN t.규격2                                      ");
            qry.Append($"\n  		  WHEN t.규격2 = '-' THEN t.규격2                                      ");
            qry.Append($"\n  		  WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                              ");
            qry.Append($"\n  		  WHEN CHARINDEX('S', 규격) > 0 THEN                                 ");
            qry.Append($"\n  		  	CASE WHEN CHARINDEX('4', 규격) > 0 THEN '11'                     ");
            qry.Append($"\n  		  		WHEN CHARINDEX('3', 규격) > 0 THEN '12'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('2', 규격) > 0 THEN '13'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('1', 규격) > 0 THEN '14'                      ");
            qry.Append($"\n  		  		ELSE '10' END                                              ");
            qry.Append($"\n  		  WHEN CHARINDEX('M', 규격) > 0 THEN                                 ");
            qry.Append($"\n  		  	CASE WHEN CHARINDEX('1', 규격) > 0 THEN '21'                     ");
            qry.Append($"\n  		  		WHEN CHARINDEX('2', 규격) > 0 THEN '22'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('3', 규격) > 0 THEN '23'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('4', 규격) > 0 THEN '24'                      ");
            qry.Append($"\n  		  		ELSE '20' END                                              ");
            qry.Append($"\n  		  WHEN CHARINDEX('L', 규격) > 0 THEN                                 ");
            qry.Append($"\n  		  	CASE WHEN CHARINDEX('1', 규격) > 0 THEN '31'                     ");
            qry.Append($"\n  		  		WHEN CHARINDEX('2', 규격) > 0 THEN '32'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('3', 규격) > 0 THEN '33'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('4', 규격) > 0 THEN '34'                      ");
            qry.Append($"\n  		  		ELSE '30' END                                              ");
            qry.Append($"\n  		  ELSE t.규격2                                                       ");
            qry.Append($"\n  	  END AS 규격4                                                           ");
            qry.Append($"\n 	FROM(                                                                                                                                      ");
            qry.Append($"\n 		SELECT                                                                                                                                 ");
            qry.Append($"\n           t1.*                                                                                                                                 ");
            qry.Append($"\n 		, t2.매입단가 AS 일반시세                                                                                                                 ");
            qry.Append($"\n 		, t2.담당자1                                                                                                                              ");
            qry.Append($"\n 		, t2.비고1                                                                                                                              ");
            qry.Append($"\n 		FROM(                                                                                                                                  ");
            qry.Append($"\n 	 		SELECT                                                                                                                             ");
            qry.Append($"\n 	  		  CASE                                                                                                                             ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Aa' THEN '라운드새우류'                                                                 ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ab' THEN '새우살류'                                                                   ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ba' THEN '쭈꾸미류'                                                                   ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bb' THEN '낙지류'                                                                    ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bc' THEN '갑오징어류'                                                                  ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bd' THEN '오징어류'                                                                   ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Be' THEN '문어류'                                                                    ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'C' THEN '갑각류'                                                                     ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Da' THEN '어패류'                                                                    ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Db' THEN '살류'                                                                     ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Dc' THEN '해물류'                                                                    ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 3)) = 'Eab' THEN '선어특수/가공품'                    ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ea' THEN '초밥류'                              ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Eb' THEN '기타수산가공품'                      ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ec' THEN '기타식품가공품'                      ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ed' THEN '축산가공품'                          ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ee' THEN '야채과일류'                          ");
            qry.Append($"\n 	  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'F' THEN '선어류'                                                                     ");
            qry.Append($"\n 	  		END AS 대분류                                                                                                                           ");
            qry.Append($"\n 	  		,  대분류 AS 대분류1                                                                                                                    ");
            qry.Append($"\n 			,  CASE WHEN ISNUMERIC(SUBSTRING(t.대분류, 3, 1)) <> 1 THEN SUBSTRING(대분류, 1, 3)             ");
            qry.Append($"\n 			        WHEN ISNUMERIC(SUBSTRING(t.대분류, 2, 1)) <> 1 THEN SUBSTRING(대분류, 1, 2)             ");
            qry.Append($"\n 				    ELSE SUBSTRING(대분류, 1, 1) END AS 대분류2                                             ");
            qry.Append($"\n 			,  CASE WHEN ISNUMERIC(SUBSTRING(t.대분류, 3, 1)) <> 1 THEN SUBSTRING(대분류, 4, 3)             ");
            qry.Append($"\n 			        WHEN ISNUMERIC(SUBSTRING(t.대분류, 2, 1)) <> 1 THEN SUBSTRING(대분류, 3, 3)             ");
            qry.Append($"\n 				    ELSE SUBSTRING(대분류, 2, 3) END AS 대분류3                                             ");
            qry.Append($"\n 	 		, 원산지코드                                                                                                                            ");
            qry.Append($"\n 	 		, 원산지                                                                                                                                ");
            qry.Append($"\n 	 		, 품목코드                                                                                                                              ");
            qry.Append($"\n 	 		, 품명                                                                                                                                  ");
            qry.Append($"\n 	 		, 규격코드                                                                                                                              ");
            qry.Append($"\n 	 		, 규격                                                                                                                                  ");
            qry.Append($"\n 	 		, CASE                                                                                                                                  ");
            qry.Append($"\n 	 			  WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                                                  ");
            qry.Append($"\n 	 			  	CASE                                                                                                                            ");
            qry.Append($"\n 	 				  	WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))                            ");
            qry.Append($"\n 	 				  	WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격))                           ");
            qry.Append($"\n 	 			  	END                                                                                                                             ");
            qry.Append($"\n 	 			  WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                                                     ");
            qry.Append($"\n 	 			  WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                                       ");
            qry.Append($"\n 	 			  ELSE 규격                                                                                                                         ");
            qry.Append($"\n 	 		  END AS 규격2                                                                                                                          ");
            qry.Append($"\n 	 		, 단위                                                                                                                                  ");
            qry.Append($"\n  	 		, CASE WHEN 가격단위 = '묶' THEN ''                                                 ");
            qry.Append($"\n  	 			ELSE ISNULL(가격단위, '') END AS 가격단위                                          ");
            qry.Append($"\n  	 		, CASE WHEN 가격단위 = '묶' THEN 1                                                  ");
            qry.Append($"\n  	 			ELSE ISNULL(단위수량, '') END AS 단위수량                                          ");
            qry.Append($"\n  	 		, CASE WHEN 가격단위 = '묶' THEN CONVERT(float, SEAOVER단위) / CONVERT(float, 단위수량)  ");
            qry.Append($"\n 	 	 		ELSE SEAOVER단위 END AS SEAOVER단위                                            ");
            qry.Append($"\n 	 		, ISNULL(매출단가, 0) AS 매출단가                                                                                                       ");
            qry.Append($"\n 	 		, ISNULL(구분, '') AS 구분                                                                                                              ");
            qry.Append($"\n 	 		, MAX(CONVERT(VARCHAR(10), ISNULL(단가수정일, '1900-01-01'), 120))    AS 단가수정일                                                     ");
            qry.Append($"\n 	 		, 부가세유무                                                                                                                            ");
            qry.Append($"\n 	 		FROM 업체별시세관리                                                                                                                     ");
            qry.Append($"\n 	 		WHERE 사용자 = '{userId}'                                                                                                               ");
            string[] tempStr = null;
            string tempWhr = "";

            if (!string.IsNullOrEmpty(categoryCode))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = categoryCode.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{categoryCode}%'                                                                         ");
                }

            }
            if (!string.IsNullOrEmpty(product))
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
            if (!string.IsNullOrEmpty(origin))
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
            if (!string.IsNullOrEmpty(sizes))
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
            if (!string.IsNullOrEmpty(unit))
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
            if (!string.IsNullOrEmpty(division))
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
            if (!string.IsNullOrEmpty(manager))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = manager.Split(' ');
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
                    qry.Append($"\n	  AND 담당자1 LIKE '%{manager}%'                                                                         ");
                }
            }
            qry.Append($"\n  	 		GROUP BY 대분류, 원산지코드, 원산지, 품목코드, 품명, 규격코드, 규격, 단위, CASE WHEN 가격단위 = '묶' THEN ''                             ");
            qry.Append($"\n  	 			ELSE ISNULL(가격단위, '') END, CASE WHEN 가격단위 = '묶' THEN 1                                                    ");
            qry.Append($"\n  	 			ELSE ISNULL(단위수량, '') END, CASE WHEN 가격단위 = '묶' THEN CONVERT(float, SEAOVER단위) / CONVERT(float, 단위수량)    ");
            qry.Append($"\n 	 	 		ELSE SEAOVER단위 END, 매출단가, 구분, 부가세유무                                                                              ");
            qry.Append($"\n  		) AS t1                                                                                                                                         ");
            qry.Append($"\n  		INNER JOIN (                                                                                                                                    ");
            qry.Append($"\n 				SELECT                                                                                                             ");
            qry.Append($"\n 			       T.대분류                                                                                                           ");
            qry.Append($"\n 			     , T.원산지코드                                                                                                        ");
            qry.Append($"\n 			     , T.품목코드                                                                                                          ");
            qry.Append($"\n 			     , T.규격코드                                                                                                          ");
            qry.Append($"\n 			     , T.단위                                                                                                            ");
            qry.Append($"\n 			     , CASE WHEN T.가격단위 = '묶' THEN ''                                                                                 ");
            qry.Append($"\n 	  	 			ELSE ISNULL(T.가격단위, '') END AS 가격단위                                                                           ");
            qry.Append($"\n 	  	 		 , CASE WHEN T.가격단위 = '묶' THEN 1                                                                                  ");
            qry.Append($"\n 	  	 			ELSE ISNULL(T.단위수량, '') END AS 단위수량                                                                           ");
            qry.Append($"\n 	  	 		 , CASE WHEN T.가격단위 = '묶' THEN CONVERT(float, T.SEAOVER단위) / CONVERT(float, T.단위수량)                               ");
            qry.Append($"\n 	 	 	 		ELSE T.SEAOVER단위 END AS SEAOVER단위                                                                              ");
            qry.Append($"\n 			     , T.구분                                                                                                            ");
            qry.Append($"\n 			     , T.단가수정일                                                                                                        ");
            qry.Append($"\n 			     , AVG(T.매입단가) AS 매입단가                                                                                            ");
            qry.Append($"\n 			     , T.담당자1                                                                                                          ");
            qry.Append($"\n 			     , MAX(T.비고1)  AS 비고1                                                                                              ");
            qry.Append($"\n			     FROM (                                                                                                                                     ");
            qry.Append($"\n 			     	SELECT                                                                                                         ");
            qry.Append($"\n 			       	  ROW_NUMBER() OVER(PARTITION BY 대분류, 단가수정일, 원산지코드, 품목코드, 규격코드, 단위, CASE WHEN 가격단위 = '묶' THEN ''            ");
            qry.Append($"\n   	 					ELSE ISNULL(가격단위, '') END, CASE WHEN 가격단위 = '묶' THEN 1                                                            ");
            qry.Append($"\n   	 					ELSE ISNULL(단위수량, '') END, CASE WHEN 가격단위 = '묶' THEN CONVERT(float, SEAOVER단위) / CONVERT(float, 단위수량)            ");
            qry.Append($"\n  	 	 				ELSE SEAOVER단위 END ORDER BY 단가수정일 DESC) AS Row#                                                                   ");
            qry.Append($"\n 			       	, *                                                                                                            ");
            qry.Append($"\n 			       	FROM 업체별시세관리                                                                                                  ");
            qry.Append($"\n			         WHERE 사용자 = '{userId}'                                                                                                              ");
            qry.Append($"\n			     	  AND 매입단가 > 6                                                                                                                      ");
            qry.Append($"\n			     ) AS T                                                                                                                                     ");
            qry.Append($"\n			     WHERE Row# <= {avgCnt}                                                                                                                     ");
            
            if (!string.IsNullOrEmpty(manager))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = manager.Split(' ');
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
                    qry.Append($"\n	  AND 담당자1 LIKE '%{manager}%'                                                                         ");
                }
            }
            qry.Append($"\n 			     GROUP BY T.대분류, T.원산지코드, T.품목코드, T.규격코드, T.단위, CASE WHEN 가격단위 = '묶' THEN ''                                    ");
            qry.Append($"\n   	 			ELSE ISNULL(가격단위, '') END, CASE WHEN 가격단위 = '묶' THEN 1                                                            ");
            qry.Append($"\n   	 			ELSE ISNULL(단위수량, '') END, CASE WHEN 가격단위 = '묶' THEN CONVERT(float, SEAOVER단위) / CONVERT(float, 단위수량)            ");
            qry.Append($"\n  	 	 		ELSE SEAOVER단위 END, T.구분, T.단가수정일, T.담당자1                                                                         ");
            qry.Append($"\n  		) AS t2                                                                                                              ");
            qry.Append($"\n  		  ON ISNULL(t1.대분류1, '') = ISNULL(t2.대분류, '')                                                                                                                  ");
            qry.Append($"\n  		  AND t1.원산지코드 = t2.원산지코드                                                                                                          ");
            qry.Append($"\n  		  AND t1.품목코드 = t2.품목코드                                                                                                              ");
            qry.Append($"\n  		  AND t1.규격코드 = t2.규격코드                                                                                                              ");
            qry.Append($"\n  		  AND t1.단위 = t2.단위                                                                                                                      ");
            qry.Append($"\n  		  AND t1.가격단위 = ISNULL(t2.가격단위, '')                                                                                                  ");
            qry.Append($"\n  		  AND t1.단위수량 = ISNULL(t2.단위수량, '')                                                                                                  ");
            qry.Append($"\n  		  AND t1.SEAOVER단위 = t2.SEAOVER단위                                                                                                        ");
            qry.Append($"\n  		  AND t1.구분 = ISNULL(t2.구분, '')                                                                                                          ");
            qry.Append($"\n  		  AND t1.단가수정일 = ISNULL(t2.단가수정일, '1900-01-01')                                                                                    ");
            qry.Append($"\n  	) AS t                                                                                                                                           ");
            qry.Append($"\n   ) AS t                                                                                                                                             ");
            qry.Append($"\n WHERE 1=1                                                                                                                                            ");
            qry.Append($"\n   AND 매출단가 <= 일반시세                                                                                                                           ");
            if (!string.IsNullOrEmpty(category))
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
                            tempWhr = $"\n	   t.대분류 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.대분류 LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.대분류 LIKE '%{category}%'                                                                         ");
                }
            }
            qry.Append($"\n GROUP BY 대분류2, 대분류3, 대분류, 대분류1, 원산지코드, 원산지, 품목코드, 품명, 규격코드, 규격, 규격2, 규격3, 규격4, 단위, 가격단위, 단위수량, SEAOVER단위, 매출단가, 구분, 단가수정일, 담당자1, 비고1, 부가세유무         ");
            qry.Append($"\n ORDER BY t.대분류2, t.대분류3, t.품명, t.원산지, t.규격3                                                                                                  ");
            qry.Append($"\n , CASE WHEN CHARINDEX('대', t.규격4) > 0  THEN '1'                                                                                                  ");
            qry.Append($"\n        WHEN CHARINDEX('중', t.규격4) > 0 THEN '2'                                                                                                   ");
            qry.Append($"\n        WHEN CHARINDEX('소', t.규격4) > 0 THEN '3'                                                                                                   ");
            qry.Append($"\n        ELSE t.규격4 END                                                                                                  ");
            qry.Append($"\n , 단가수정일 DESC                                                                                                                                     ");

            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProduct(string categoryCode, string category, string product, string origin, string sizes, string unit, string division, string manager, bool chkDiv, bool chkPrice)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                       ");
            qry.Append($"\n    t.대분류                                                                ");
            qry.Append($"\n  , t.대분류1                                                               ");
            qry.Append($"\n  , t.대분류2                                                               ");
            qry.Append($"\n  , t.대분류3                                                               ");
            qry.Append($"\n  , t.원산지코드                                                              ");
            qry.Append($"\n  , t.원산지                                                                ");
            qry.Append($"\n  , t.품목코드                                                               ");
            qry.Append($"\n  , t.품명                                                                 ");
            qry.Append($"\n  , t.규격코드                                                               ");
            qry.Append($"\n  , t.규격                                                                 ");
            qry.Append($"\n  , t.규격2                                                                ");
            qry.Append($"\n  , t.규격3                                                                ");
            qry.Append($"\n  , t.규격4                                                                ");
            qry.Append($"\n  , t.단위                                                                 ");
            qry.Append($"\n  , t.가격단위                                                               ");
            qry.Append($"\n  , t.단위수량                                                               ");
            qry.Append($"\n  , t.SEAOVER단위                                                          ");
            qry.Append($"\n  , '0  ' AS 트레이                                                                                                              ");
            qry.Append($"\n  , t.매출단가                                                               ");
            qry.Append($"\n  , t.구분                                                                 ");
            qry.Append($"\n  , CASE WHEN t.단가수정일 = '1900-01-01' THEN '' ELSE t.단가수정일 END AS 단가수정일  ");
            qry.Append($"\n  , t.담당자1                                                               ");
            qry.Append($"\n  , t.비고1                                                                ");
            qry.Append($"\n  , CASE WHEN t.부가세유무 = 'Y' THEN '과세' ELSE '' END AS 부가세                                                                ");
            qry.Append($"\n FROM(                                                                                                                        ");
            qry.Append($"\n 	SELECT                                                                                                                   ");
            qry.Append($"\n 	  t.*                                                                                                                    ");
            qry.Append($"\n  	, CASE                                                                 ");
            qry.Append($"\n  	      WHEN t.규격2 = '.' THEN NULL                                       ");
            qry.Append($"\n  		  WHEN t.규격2 = '-' THEN NULL                                       ");
            qry.Append($"\n  		  WHEN ISNUMERIC(t.규격2) = 1 THEN CONVERT(float, t.규격2)             ");
            qry.Append($"\n  		  WHEN CHARINDEX('S', 규격) > 0 THEN 1                               ");
            qry.Append($"\n  		  WHEN CHARINDEX('M', 규격) > 0 THEN 2                               ");
            qry.Append($"\n  		  WHEN CHARINDEX('L', 규격) > 0 THEN 3                               ");
            qry.Append($"\n  		  WHEN CHARINDEX('G*', 규격) > 0 THEN PATINDEX('%[^a-zA-Z0-9 ]%', 규격)           ");
            qry.Append($"\n  		  ELSE NULL                                                        ");
            qry.Append($"\n  	  END AS 규격3                                                           ");
            qry.Append($"\n  	, CASE                                                                 ");
            qry.Append($"\n  		  WHEN t.규격2 = '.' THEN t.규격2                                      ");
            qry.Append($"\n  		  WHEN t.규격2 = '-' THEN t.규격2                                      ");
            qry.Append($"\n  		  WHEN ISNUMERIC(t.규격2) = 1 THEN NULL                              ");
            qry.Append($"\n  		  WHEN CHARINDEX('S', 규격) > 0 THEN                                 ");
            qry.Append($"\n  		  	CASE WHEN CHARINDEX('4', 규격) > 0 THEN '11'                     ");
            qry.Append($"\n  		  		WHEN CHARINDEX('3', 규격) > 0 THEN '12'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('2', 규격) > 0 THEN '13'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('1', 규격) > 0 THEN '14'                      ");
            qry.Append($"\n  		  		ELSE '10' END                                              ");
            qry.Append($"\n  		  WHEN CHARINDEX('M', 규격) > 0 THEN                                 ");
            qry.Append($"\n  		  	CASE WHEN CHARINDEX('1', 규격) > 0 THEN '21'                     ");
            qry.Append($"\n  		  		WHEN CHARINDEX('2', 규격) > 0 THEN '22'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('3', 규격) > 0 THEN '23'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('4', 규격) > 0 THEN '24'                      ");
            qry.Append($"\n  		  		ELSE '20' END                                              ");
            qry.Append($"\n  		  WHEN CHARINDEX('L', 규격) > 0 THEN                                 ");
            qry.Append($"\n  		  	CASE WHEN CHARINDEX('1', 규격) > 0 THEN '31'                     ");
            qry.Append($"\n  		  		WHEN CHARINDEX('2', 규격) > 0 THEN '32'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('3', 규격) > 0 THEN '33'                      ");
            qry.Append($"\n  		  		WHEN CHARINDEX('4', 규격) > 0 THEN '34'                      ");
            qry.Append($"\n  		  		ELSE '30' END                                              ");
            qry.Append($"\n  		  ELSE t.규격2                                                       ");
            qry.Append($"\n  	  END AS 규격4                                                           ");
            qry.Append($"\n 	FROM(                                                                                                                    ");
            qry.Append($"\n 		SELECT                                                                                                               ");
            qry.Append($"\n  		  CASE                                                                                    ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Aa' THEN '라운드새우류'                ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ab' THEN '새우살류'                  ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ba' THEN '쭈꾸미류'                  ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bb' THEN '낙지류'                   ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bc' THEN '갑오징어류'                 ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Bd' THEN '오징어류'                  ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Be' THEN '문어류'                   ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'C' THEN '갑각류'                    ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Da' THEN '어패류'                   ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Db' THEN '살류'                    ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Dc' THEN '해물류'                   ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 3)) = 'Eab' THEN '선어특수/가공품'                    ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ea' THEN '초밥류'                              ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Eb' THEN '기타수산가공품'                      ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ec' THEN '기타식품가공품'                      ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ed' THEN '축산가공품'                          ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 2)) = 'Ee' THEN '야채과일류'                          ");
            qry.Append($"\n  			WHEN CONVERT(char, SUBSTRING(대분류, 1, 1)) = 'F' THEN '선어류'                    ");
            qry.Append($"\n  		END AS 대분류                                                                          ");
            qry.Append($"\n  		,  대분류 AS 대분류1                                                                   ");
            qry.Append($"\n 			,  CASE WHEN ISNUMERIC(SUBSTRING(t.대분류, 3, 1)) <> 1 THEN SUBSTRING(대분류, 1, 3)             ");
            qry.Append($"\n 			        WHEN ISNUMERIC(SUBSTRING(t.대분류, 2, 1)) <> 1 THEN SUBSTRING(대분류, 1, 2)             ");
            qry.Append($"\n 				    ELSE SUBSTRING(대분류, 1, 1) END AS 대분류2                                             ");
            qry.Append($"\n 			,  CASE WHEN ISNUMERIC(SUBSTRING(t.대분류, 3, 1)) <> 1 THEN SUBSTRING(대분류, 4, 3)             ");
            qry.Append($"\n 			        WHEN ISNUMERIC(SUBSTRING(t.대분류, 2, 1)) <> 1 THEN SUBSTRING(대분류, 3, 3)             ");
            qry.Append($"\n 				    ELSE SUBSTRING(대분류, 2, 3) END AS 대분류3                                             ");
            qry.Append($"\n 		, 원산지코드                                                                                                             ");
            qry.Append($"\n 		, 원산지                                                                                                                ");
            qry.Append($"\n 		, 품목코드                                                                                                               ");
            qry.Append($"\n 		, 품명                                                                                                                 ");
            qry.Append($"\n 		, 규격코드                                                                                                               ");
            qry.Append($"\n 		, 규격                                                                                                                 ");
            qry.Append($"\n 		, CASE                                                                                                               ");
            qry.Append($"\n 			  WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                                   ");
            qry.Append($"\n 			  	CASE                                                                                                         ");
            qry.Append($"\n 				  	WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))      ");
            qry.Append($"\n 				  	WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격))     ");
            qry.Append($"\n 			  	END                                                                                                       ");
            qry.Append($"\n 			  WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                               ");
            qry.Append($"\n 			  WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                 ");
            qry.Append($"\n 			  ELSE 규격                                                                                                     ");
            qry.Append($"\n 		  END AS 규격2                                                                                                      ");
            qry.Append($"\n 		, 단위                                                                                                              ");
            qry.Append($"\n 		, 가격단위                                                         ");
            qry.Append($"\n 		, 단위수량                                                         ");
            qry.Append($"\n 		, SEAOVER단위                                                           ");
            qry.Append($"\n 		, ISNULL(매출단가, 0) AS 매출단가                                                                                        ");
            qry.Append($"\n 		, 구분                                                                                                              ");
            qry.Append($"\n 		, ISNULL(담당자1, '') AS 담당자1                                                                                  ");
            qry.Append($"\n 		, CONVERT(VARCHAR(10), 단가수정일, 120)    AS 단가수정일                                                                                                         ");
            qry.Append($"\n 		, 비고1    AS 비고1                                                                                                           ");
            qry.Append($"\n 		, 부가세유무                                                                                                           ");
            qry.Append($"\n 		FROM 업체별시세관리                                                                                                     ");
            qry.Append($"\n 		WHERE 사용자 = '{userId}'                                                                                              ");
            
            if (chkDiv)
            {
                qry.Append($"\n 		  AND 대분류 <> ''                                                                                                   ");
                qry.Append($"\n 		  AND 대분류 IS NOT NULL                                                                                             ");
                /*qry.Append($"\n 		  AND 매입단가 > 0                                                                                              ");*/
            }
            if (chkPrice)
            {
                qry.Append($"\n 		  AND 매입단가 > 0                                                                                              ");
            }
            string[] tempStr = null;
            string tempWhr = "";

            if (!string.IsNullOrEmpty(categoryCode))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = categoryCode.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (string.IsNullOrEmpty(tempWhr))
                        {
                            tempWhr = $"\n	   대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR 대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 대분류 COLLATE Korean_Wansung_CS_AS LIKE '%{categoryCode}%'                                                                         ");
                }

            }

            if (!string.IsNullOrEmpty(product))
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
            if (!string.IsNullOrEmpty(origin))
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
            if (!string.IsNullOrEmpty(sizes))
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
            if (!string.IsNullOrEmpty(unit))
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
            if (!string.IsNullOrEmpty(division))
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
            if (!string.IsNullOrEmpty(manager))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = manager.Split(' ');
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
                    qry.Append($"\n	  AND 담당자1 LIKE '%{manager}%'                                                                         ");
                }
            }
            qry.Append($"\n  		GROUP BY 대분류, 원산지코드, 원산지, 품목코드, 품명, 규격코드, 규격, 단위, 가격단위, 단위수량, SEAOVER단위            ");
            qry.Append($"\n 				 , 매출단가, 구분, 담당자1, 단가수정일, 비고1, 부가세유무                                                                                                       ");
            qry.Append($"\n 	) AS t                                                                                                       ");
            qry.Append($"\n ) AS t                                                                                                           ");
            qry.Append($"\n WHERE 1=1                                                                                                           ");
            if (!string.IsNullOrEmpty(category))
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
                            tempWhr = $"\n	   t.대분류 LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR t.대분류 LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND t.대분류 LIKE '%{category}%'                                                                         ");
                }
            }

            qry.Append($"\n ORDER BY t.대분류2, t.대분류3, t.품명, t.원산지, t.규격3                     ");
            qry.Append($"\n , CASE WHEN CHARINDEX('대', t.규격4) > 0  THEN '1'                                                                                                  ");
            qry.Append($"\n        WHEN CHARINDEX('중', t.규격4) > 0 THEN '2'                                                                                                   ");
            qry.Append($"\n        WHEN CHARINDEX('소', t.규격4) > 0 THEN '3'                                                                                                   ");
            qry.Append($"\n        ELSE t.규격4 END                                                                                                  ");
            qry.Append($"\n , 단가수정일 DESC                                                                                                                                     ");
            string sql = qry.ToString();
            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        private List<SeaoverPriceModel> SetProductModel(SqlDataReader rd)
        {
            List<SeaoverPriceModel> list = new List<SeaoverPriceModel>();
            while (rd.Read())
            {
                SeaoverPriceModel model = new SeaoverPriceModel();

                model.category = rd["대분류"].ToString();
                model.category_code = rd["대분류1"].ToString();
                model.origin_code = rd["원산지코드"].ToString();
                model.origin = rd["원산지"].ToString();
                model.product_code = rd["품목코드"].ToString();
                model.product = rd["품명"].ToString();
                model.sizes_code = rd["규격코드"].ToString();
                model.sizes = rd["규격"].ToString();
                model.unit = rd["단위"].ToString();
                model.price_unit = rd["가격단위"].ToString();
                model.unit_count = rd["단위수량"].ToString();
                model.seaover_unit = rd["SEAOVER단위"].ToString();
                model.sales_price = Convert.ToDouble(rd["매출단가"].ToString());
                model.purchase_price = 0;
                model.division = rd["구분"].ToString();
                model.manager1 = rd["담당자1"].ToString();
                if (rd["단가수정일"] == null)
                {
                    model.edit_date = "";
                }
                else 
                {
                    model.edit_date = rd["단가수정일"].ToString();
                }
                
                model.remark1 = rd["비고1"].ToString();
                list.Add(model);
            }
            rd.Close();
            return list;            
        }

        //===========================================================================================================================
        public List<FormDataModel> GetFormData(string form_type, string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	 SELECT                                                                                                 ");
            qry.Append($"\n  id                                                                   ");
            qry.Append($"\n, sid                                                                  ");
            qry.Append($"\n, form_type                                                            ");
            qry.Append($"\n, form_name                                                            ");
            qry.Append($"\n, category                                                             ");
            qry.Append($"\n, category_code                                                        ");
            qry.Append($"\n, product_code                                                         ");
            qry.Append($"\n, product                                                              ");
            qry.Append($"\n, origin_code                                                          ");
            qry.Append($"\n, origin                                                               ");
            qry.Append($"\n, weight                                                               ");
            qry.Append($"\n, sizes_code                                                           ");
            qry.Append($"\n, sizes                                                                ");
            qry.Append($"\n, sales_price                                                          ");
            qry.Append($"\n, purchase_price                                                       ");
            qry.Append($"\n, unit                                                                 ");
            qry.Append($"\n, price_unit                                                           ");
            qry.Append($"\n, unit_count                                                           ");
            qry.Append($"\n, seaover_unit                                                         ");
            qry.Append($"\n, division                                                             ");
            qry.Append($"\n, page                                                                 ");
            qry.Append($"\n, cnt                                                                  ");
            qry.Append($"\n, row_index                                                            ");
            qry.Append($"\n, area                                                                 ");
            qry.Append($"\n, updatetime                                                           ");
            qry.Append($"\n, edit_user                                                            ");
            qry.Append($"\n, create_user                                                          ");
            qry.Append($"\n, accent                                                               ");
            qry.Append($"\n, form_remark                                                          ");
            qry.Append($"\n, IF(IFNULL(is_rock, 0) = 0, 'FALSE', 'TRUE') AS is_rock               ");
            qry.Append($"\n, password                                                             ");
            qry.Append($"\n	 FROM t_form_data                                                                                       ");
            qry.Append($"\n	 WHERE 1=1                                                                                              ");
            if (!string.IsNullOrEmpty(form_type))
            {
                qry.Append($"\n	   AND form_type = '{form_type}'                                                                    ");
            }
            if (!string.IsNullOrEmpty(id))
            {
                qry.Append($"\n	   AND id = {id}                                                                                    ");
            }
            qry.Append($"\n  ORDER BY sid                                                                                           ");

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetFormDataModel(dr);
        }
        public List<FormDataModel> GetFormDataTemp(string form_type, string form_id, string temp_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	 SELECT                                                               ");
            qry.Append($"\n  id                                                                   ");
            qry.Append($"\n, sid                                                                  ");
            qry.Append($"\n, form_type                                                            ");
            qry.Append($"\n, form_name                                                            ");
            qry.Append($"\n, category                                                             ");
            qry.Append($"\n, category_code                                                        ");
            qry.Append($"\n, product_code                                                         ");
            qry.Append($"\n, product                                                              ");
            qry.Append($"\n, origin_code                                                          ");
            qry.Append($"\n, origin                                                               ");
            qry.Append($"\n, weight                                                               ");
            qry.Append($"\n, sizes_code                                                           ");
            qry.Append($"\n, sizes                                                                ");
            qry.Append($"\n, sales_price                                                          ");
            qry.Append($"\n, purchase_price                                                       ");
            qry.Append($"\n, unit                                                                 ");
            qry.Append($"\n, price_unit                                                           ");
            qry.Append($"\n, unit_count                                                           ");
            qry.Append($"\n, seaover_unit                                                         ");
            qry.Append($"\n, division                                                             ");
            qry.Append($"\n, page                                                                 ");
            qry.Append($"\n, cnt                                                                  ");
            qry.Append($"\n, row_index                                                            ");
            qry.Append($"\n, area                                                                 ");
            qry.Append($"\n, updatetime                                                           ");
            qry.Append($"\n, edit_user                                                            ");
            qry.Append($"\n, create_user                                                          ");
            qry.Append($"\n, accent                                                               ");
            qry.Append($"\n, form_remark                                                          ");
            qry.Append($"\n, IF(IFNULL(is_rock, 0) = 0, 'FALSE', 'TRUE') AS is_rock               ");
            qry.Append($"\n, password                                                             ");
            qry.Append($"\n	 FROM t_form_data_temp                                                ");
            qry.Append($"\n	 WHERE 1=1                                                                                              ");
            if (!string.IsNullOrEmpty(form_type))
            {
                qry.Append($"\n	   AND form_type = '{form_type}'                                                                    ");
            }
            if (!string.IsNullOrEmpty(form_id))
            {
                qry.Append($"\n	   AND id = {form_id}                                                                               ");
            }
            if (!string.IsNullOrEmpty(temp_id))
            {
                qry.Append($"\n	   AND temp_id = {temp_id}                                                                               ");
            }
            qry.Append($"\n  ORDER BY sid                                                                                           ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetFormDataModel(dr);
        }
        private List<FormDataModel> SetFormDataModel(MySqlDataReader rd)
        {
            List<FormDataModel> list = new List<FormDataModel>();
            while (rd.Read())
            {
                FormDataModel model = new FormDataModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.sid = Convert.ToInt32(rd["sid"].ToString());
                model.form_type = Convert.ToInt32(rd["form_type"].ToString());
                model.form_name = rd["form_name"].ToString();
                model.category = rd["category"].ToString();
                model.category_code = rd["category_code"].ToString();
                model.product_code = rd["product_code"].ToString();
                model.product = rd["product"].ToString();
                model.origin_code = rd["origin_code"].ToString();
                model.origin = rd["origin"].ToString();
                model.weight = rd["weight"].ToString();
                model.sizes_code = rd["sizes_code"].ToString();
                model.sizes = rd["sizes"].ToString();
                model.sales_price = Convert.ToDouble(rd["sales_price"]);
                model.purchase_price = Convert.ToDouble(rd["purchase_price"]);
                model.unit = rd["unit"].ToString();
                model.price_unit = rd["price_unit"].ToString();
                model.unit_count = rd["unit_count"].ToString();
                model.seaover_unit = rd["seaover_unit"].ToString();
                model.division = rd["division"].ToString();
                model.page = Convert.ToInt16(rd["page"]);
                model.cnt = Convert.ToInt16(rd["cnt"]);
                model.row_index = Convert.ToInt16(rd["row_index"]);
                model.area = rd["area"].ToString();
                model.updatetime = rd["updatetime"].ToString();
                model.edit_user = rd["edit_user"].ToString();
                model.create_user = rd["create_user"].ToString();
                model.accent = Convert.ToBoolean(rd["accent"]);
                model.form_remark = rd["form_remark"].ToString();

                bool is_rock;
                if (!bool.TryParse(rd["is_rock"].ToString(), out is_rock))
                    is_rock = false;
                model.is_rock = is_rock;
                model.password = rd["password"].ToString();
                list.Add(model);
            }
            rd.Close();
            return list;
        }

        public List<FormDataTitle> GetFormDataTitle(int formType, string id, string formname, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	 SELECT                                                                                                 ");
            qry.Append($"\n	   id                                                                                                   ");
            qry.Append($"\n	 , form_type                                                                                            ");
            qry.Append($"\n	 , form_name                                                                                            ");
            qry.Append($"\n	 , create_user                                                                                          ");
            qry.Append($"\n	 , DATE_FORMAT(updatetime, '%Y-%m-%d') AS updatetime                                                    ");
            qry.Append($"\n	 FROM t_form_data                                                                                       ");
            qry.Append($"\n	 WHERE 1=1                                                                                              ");
            if(formType == 3)
                qry.Append($"\n	   AND form_type = {formType}                                                                           ");
            else
                qry.Append($"\n	   AND form_type != 3                                                                           ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n	   AND id = {id}                                                                ");
            if (!string.IsNullOrEmpty(formname))
                qry.Append($"\n	   AND form_name LIKE '%{formname}%'                                                                ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n	   AND create_user LIKE '%{manager}%'                                                                ");
            qry.Append($"\n  GROUP BY id, form_type, form_name, create_user, DATE_FORMAT(updatetime, '%Y-%m-%d')                     ");
            qry.Append($"\n  ORDER BY updatetime desc                                                                                ");

            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetFormDataTitleModel(dr);
        }

        private List<FormDataTitle> SetFormDataTitleModel(MySqlDataReader rd)
        {
            List<FormDataTitle> list = new List<FormDataTitle>();
            while (rd.Read())
            {
                FormDataTitle model = new FormDataTitle();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.form_type = Convert.ToInt32(rd["form_type"].ToString());
                model.form_name = rd["form_name"].ToString();
                model.updatetime = rd["updatetime"].ToString();
                model.edit_user = rd["create_user"].ToString();

                list.Add(model);
            }
            rd.Close();
            return list;
        }

        public StringBuilder AllDataUpdateExlude(FormDataModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_form_data SET                                    ");
            qry.Append($" sales_price =  {model.sales_price}                        ");
            qry.Append($" WHERE 1=1                                                 ");
            qry.Append($" AND category_code = '{model.category_code}'               ");
            qry.Append($" AND product_code = '{model.product_code}'                 ");
            qry.Append($" AND origin_code = '{model.origin_code}'                   ");
            qry.Append($" AND sizes_code = '{model.sizes_code}'                     ");
            qry.Append($" AND unit = '{model.unit}'                                 ");
            qry.Append($" AND price_unit = '{model.price_unit}'                     ");
            qry.Append($" AND unit_count = '{model.unit_count}'                     ");
            qry.Append($" AND seaover_unit = '{model.seaover_unit}'                 ");

            return qry;
        }

        public StringBuilder DeleteFormDataTemp(string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_form_data_temp          ");
            qry.Append($" WHERE id = {id}                       ");
            qry.Append($"   AND temp_id > 5                     ");
            return qry;
        }
        public StringBuilder UpdateFormDataTemp(int temp_id, int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_form_data_temp SET           ");
            qry.Append($"   temp_id = temp_id + 1               ");
            qry.Append($" WHERE id = {id}                       ");
            return qry;
        }
        public StringBuilder DeleteFormData(string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_form_data          ");
            qry.Append($" WHERE id = {id}                  ");

            string sql = qry.ToString();
            return qry;
        }
        public StringBuilder InsertFormData(FormDataModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_form_data (        ");
            qry.Append($"\n   id                           ");
            qry.Append($"\n , sid                          ");
            qry.Append($"\n , form_type                    ");
            qry.Append($"\n , form_name                    ");
            qry.Append($"\n , category                     ");
            qry.Append($"\n , category_code                 ");
            qry.Append($"\n , product_code                 ");
            qry.Append($"\n , product                      ");
            qry.Append($"\n , origin_code                  ");
            qry.Append($"\n , origin                       ");
            qry.Append($"\n , weight                       ");
            qry.Append($"\n , sizes_code                   ");
            qry.Append($"\n , sizes                        ");
            qry.Append($"\n , purchase_price               ");
            qry.Append($"\n , sales_price                  ");
            qry.Append($"\n , unit                         ");
            qry.Append($"\n , price_unit                   ");
            qry.Append($"\n , unit_count                   ");
            qry.Append($"\n , seaover_unit                 ");
            qry.Append($"\n , division                     ");
            qry.Append($"\n , page                         ");
            qry.Append($"\n , cnt                          ");
            qry.Append($"\n , row_index                    ");
            qry.Append($"\n , area                         ");
            qry.Append($"\n , updatetime                   ");
            qry.Append($"\n , edit_user                    ");
            qry.Append($"\n , create_user                  ");
            qry.Append($"\n , accent                       ");
            qry.Append($"\n , form_remark                  ");
            qry.Append($"\n , is_rock                      ");
            qry.Append($"\n , password                     ");
            qry.Append($"\n ) VALUES (                     ");
            qry.Append($"\n    {model.id}                  ");
            qry.Append($"\n ,  {model.sid}                 ");
            qry.Append($"\n ,  {model.form_type}           ");
            qry.Append($"\n , '{model.form_name}'          ");
            qry.Append($"\n , '{model.category}'           ");
            qry.Append($"\n , '{model.category_code}'      ");
            qry.Append($"\n , '{model.product_code}'       ");
            qry.Append($"\n , '{model.product}'            ");
            qry.Append($"\n , '{model.origin_code}'        ");
            qry.Append($"\n , '{model.origin}'             ");
            qry.Append($"\n , '{model.weight}'             ");
            qry.Append($"\n , '{model.sizes_code}'         ");
            qry.Append($"\n , '{model.sizes}'              ");
            qry.Append($"\n ,  {model.purchase_price}      ");
            qry.Append($"\n ,  {model.sales_price}         ");
            qry.Append($"\n , '{model.unit}'               ");
            qry.Append($"\n , '{model.price_unit}'         ");
            qry.Append($"\n , '{model.unit_count}'         ");
            qry.Append($"\n , '{model.seaover_unit}'       ");
            qry.Append($"\n , '{model.division}'           ");
            qry.Append($"\n ,  {model.page}                ");
            qry.Append($"\n ,  {model.cnt}                 ");
            qry.Append($"\n ,  {model.row_index}           ");
            qry.Append($"\n , '{model.area}'               ");
            qry.Append($"\n , '{model.updatetime}'         ");
            qry.Append($"\n , '{model.edit_user}'          ");
            qry.Append($"\n , '{model.create_user}'        ");
            qry.Append($"\n ,  {model.accent}              ");
            qry.Append($"\n , '{model.form_remark}'        ");
            qry.Append($"\n ,  {model.is_rock}             ");
            qry.Append($"\n , '{model.password}'           ");
            qry.Append($"\n )                              ");

            return qry;
        }
        public StringBuilder InsertFormData2(FormDataModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_form_data (        ");
            qry.Append($"\n   id                           ");
            qry.Append($"\n , sid                          ");
            qry.Append($"\n , form_type                    ");
            qry.Append($"\n , form_name                    ");
            qry.Append($"\n , category                     ");
            qry.Append($"\n , category_code                 ");
            qry.Append($"\n , product_code                 ");
            qry.Append($"\n , product                      ");
            qry.Append($"\n , origin_code                  ");
            qry.Append($"\n , origin                       ");
            qry.Append($"\n , sizes_code                   ");
            qry.Append($"\n , sizes                        ");
            qry.Append($"\n , sizes2                       ");
            qry.Append($"\n , purchase_price               ");
            qry.Append($"\n , tray_price                   ");
            qry.Append($"\n , sales_price                  ");
            qry.Append($"\n , unit                         ");
            qry.Append($"\n , price_unit                   ");
            qry.Append($"\n , unit_count                   ");
            qry.Append($"\n , seaover_unit                 ");
            qry.Append($"\n , cost_unit                    ");
            qry.Append($"\n , weight                       ");
            qry.Append($"\n , division                     ");
            qry.Append($"\n , page                         ");
            qry.Append($"\n , cnt                          ");
            qry.Append($"\n , row_index                    ");
            qry.Append($"\n , area                         ");
            qry.Append($"\n , updatetime                   ");
            qry.Append($"\n , edit_user                    ");
            qry.Append($"\n , create_user                  ");
            qry.Append($"\n , accent                       ");
            qry.Append($"\n , form_remark                  ");
            qry.Append($"\n , remark                       ");
            qry.Append($"\n , is_tax                       ");
            qry.Append($"\n ) VALUES (                     ");
            qry.Append($"\n    {model.id}                  ");
            qry.Append($"\n ,  {model.sid}                 ");
            qry.Append($"\n ,  {model.form_type}           ");
            qry.Append($"\n , '{model.form_name}'          ");
            qry.Append($"\n , '{model.category}'           ");
            qry.Append($"\n , '{model.category_code}'      ");
            qry.Append($"\n , '{model.product_code}'       ");
            qry.Append($"\n , '{model.product}'            ");
            qry.Append($"\n , '{model.origin_code}'        ");
            qry.Append($"\n , '{model.origin}'             ");
            qry.Append($"\n , '{model.sizes_code}'         ");
            qry.Append($"\n , '{model.sizes}'              ");
            qry.Append($"\n , '{model.sizes2}'              ");
            qry.Append($"\n ,  {model.purchase_price}      ");
            qry.Append($"\n ,  '{model.tray_price}'        ");
            qry.Append($"\n ,  {model.sales_price}         ");
            qry.Append($"\n , '{model.unit}'               ");
            qry.Append($"\n , '{model.price_unit}'         ");
            qry.Append($"\n , '{model.unit_count}'         ");
            qry.Append($"\n , '{model.seaover_unit}'       ");
            qry.Append($"\n , '{model.cost_unit}'          ");
            qry.Append($"\n , '{model.weight}'             ");
            qry.Append($"\n , '{model.division}'           ");
            qry.Append($"\n ,  {model.page}                ");
            qry.Append($"\n ,  {model.cnt}                 ");
            qry.Append($"\n ,  {model.row_index}           ");
            qry.Append($"\n , '{model.area}'               ");
            qry.Append($"\n , '{model.updatetime}'         ");
            qry.Append($"\n , '{model.edit_user}'          ");
            qry.Append($"\n , '{model.create_user}'        ");
            qry.Append($"\n ,  {model.accent}              ");
            qry.Append($"\n , '{model.form_remark}'        ");
            qry.Append($"\n , '{model.remark}'             ");
            qry.Append($"\n , '{model.is_tax}'             ");
            qry.Append($"\n )                              ");

            string sql = qry.ToString();
            return qry;
        }
        public StringBuilder InsertFormDataTemp(FormDataModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_form_data_temp (   ");
            qry.Append($"\n   temp_id                      ");
            qry.Append($"\n , id                           ");
            qry.Append($"\n , sid                          ");
            qry.Append($"\n , form_type                    ");
            qry.Append($"\n , form_name                    ");
            qry.Append($"\n , category                     ");
            qry.Append($"\n , category_code                ");
            qry.Append($"\n , product_code                 ");
            qry.Append($"\n , product                      ");
            qry.Append($"\n , origin_code                  ");
            qry.Append($"\n , origin                       ");
            qry.Append($"\n , weight                       ");
            qry.Append($"\n , sizes_code                   ");
            qry.Append($"\n , sizes                        ");
            qry.Append($"\n , purchase_price               ");
            qry.Append($"\n , sales_price                  ");
            qry.Append($"\n , unit                         ");
            qry.Append($"\n , price_unit                   ");
            qry.Append($"\n , unit_count                   ");
            qry.Append($"\n , seaover_unit                 ");
            qry.Append($"\n , division                     ");
            qry.Append($"\n , page                         ");
            qry.Append($"\n , cnt                          ");
            qry.Append($"\n , row_index                    ");
            qry.Append($"\n , area                         ");
            qry.Append($"\n , updatetime                   ");
            qry.Append($"\n , edit_user                    ");
            qry.Append($"\n , create_user                  ");
            qry.Append($"\n , accent                       ");
            qry.Append($"\n , form_remark                  ");
            qry.Append($"\n , is_rock                      ");
            qry.Append($"\n , password                     ");
            qry.Append($"\n ) VALUES (                     ");
            qry.Append($"\n    1                           ");
            qry.Append($"\n ,  {model.id}                  ");
            qry.Append($"\n ,  {model.sid}                 ");
            qry.Append($"\n ,  {model.form_type}           ");
            qry.Append($"\n , '{model.form_name}'          ");
            qry.Append($"\n , '{model.category}'           ");
            qry.Append($"\n , '{model.category_code}'      ");
            qry.Append($"\n , '{model.product_code}'       ");
            qry.Append($"\n , '{model.product}'            ");
            qry.Append($"\n , '{model.origin_code}'        ");
            qry.Append($"\n , '{model.origin}'             ");
            qry.Append($"\n , '{model.weight}'             ");
            qry.Append($"\n , '{model.sizes_code}'         ");
            qry.Append($"\n , '{model.sizes}'              ");
            qry.Append($"\n ,  {model.purchase_price}      ");
            qry.Append($"\n ,  {model.sales_price}         ");
            qry.Append($"\n , '{model.unit}'               ");
            qry.Append($"\n , '{model.price_unit}'         ");
            qry.Append($"\n , '{model.unit_count}'         ");
            qry.Append($"\n , '{model.seaover_unit}'       ");
            qry.Append($"\n , '{model.division}'           ");
            qry.Append($"\n ,  {model.page}                ");
            qry.Append($"\n ,  {model.cnt}                 ");
            qry.Append($"\n ,  {model.row_index}           ");
            qry.Append($"\n , '{model.area}'               ");
            qry.Append($"\n , '{model.updatetime}'         ");
            qry.Append($"\n , '{model.edit_user}'          ");
            qry.Append($"\n , '{model.create_user}'        ");
            qry.Append($"\n ,  {model.accent}              ");
            qry.Append($"\n , '{model.form_remark}'        ");
            qry.Append($"\n ,  {model.is_rock}             ");
            qry.Append($"\n , '{model.password}'           ");
            qry.Append($"\n )                              ");

            return qry;
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

        public int getNextId()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	 SELECT                                                                                                 ");
            qry.Append($"\n	   if(MAX(id) IS NULL, 1, MAX(id)) + 1 AS id                                                            ");
            qry.Append($"\n	 FROM t_form_data                                                                                       ");
            qry.Append($"\n	 WHERE 1=1                                                                                              ");
            
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            int result;
            try
            {
                result = Convert.ToInt32(command.ExecuteScalar()); 
            }
            catch (Exception e)
            {
                result = 1;
            }
            return result;
        }

        public DataTable GetFormDataTemp(int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	 SELECT                                                                                                 ");
            qry.Append($"\n	   temp_id                                                                                              ");
            qry.Append($"\n	 , id                                                                                                   ");
            qry.Append($"\n	 , updatetime                                                                                           ");
            qry.Append($"\n	 FROM t_form_data_temp                                                                                  ");
            qry.Append($"\n	 WHERE 1=1                                                                                              ");
            qry.Append($"\n	   AND id = {id}                                                                                        ");
            qry.Append($"\n  GROUP BY temp_id, id, updatetime                                                                       ");

            /*MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance2.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetFormDataTempModel(dr);*/

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetPriceForCostAccount(string sttdate, string enddate, string product, string origin, string sizes, string unit)
        {
            product = product.Replace("^", " ");
            origin = origin.Replace("^", " ");
            sizes = sizes.Replace("^", " ");
            unit = unit.Replace("^", " ");

            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                              ");
            qry.Append($"\n    품명                                                                                                                             ");
            qry.Append($"\n  , 원산지                                                                                                                           ");
            qry.Append($"\n  , 규격                                                                                                                             ");
            qry.Append($"\n  , 단위                                                                                                                             ");
            qry.Append($"\n  , 단위수량                                                                                                                         ");
            qry.Append($"\n  , 가격단위                                                                                                                         ");
            qry.Append($"\n  , SEAOVER단위                                                                                                                      ");
            qry.Append($"\n  , CASE WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(float, SEAOVER단위) / CONVERT(float, 단위수량)                       ");
            qry.Append($"\n         WHEN CHARINDEX('팩', 가격단위) > 0 THEN CONVERT(float, SEAOVER단위)                                                         ");
            qry.Append($"\n      ELSE SEAOVER단위 END AS 계산단위                                                                                               ");
            qry.Append($"\n  , ISNULL(매입단가, 0) AS 매입단가                                                                                                  ");
            qry.Append($"\n  , ISNULL(매출단가, 0) AS 매출단가                                                                                                  ");
            qry.Append($"\n  , 단가수정일                                                                                                                       ");
            qry.Append($"\n  FROM  업체별시세관리                                                                                                               ");
            qry.Append($"\n  WHERE 사용자 = '{userId}'                                                                                                          ");
            string[] tempStr = null;
            string tempWhr = "";
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = product.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (!string.IsNullOrEmpty(tempStr[i]))
                            {
                                if (string.IsNullOrEmpty(tempWhr))
                                {
                                    tempWhr = $"\n	   품명  LIKE '%{tempStr[i]}%' ";
                                }
                                else
                                {
                                    tempWhr += $"\n	   OR 품명   LIKE '%{tempStr[i]}%' ";
                                }
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 품명   LIKE '%{product}%'                                                                         ");
                }
            }

            if (!(origin == null || string.IsNullOrEmpty(origin)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = origin.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (!string.IsNullOrEmpty(tempStr[i]))
                            {
                                if (string.IsNullOrEmpty(tempWhr))
                                {
                                    tempWhr = $"\n	   원산지  LIKE '%{tempStr[i]}%' ";
                                }
                                else
                                {
                                    tempWhr += $"\n	   OR 원산지   LIKE '%{tempStr[i]}%' ";
                                }
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 원산지   LIKE '%{origin}%'                                                                         ");
                }
            }
            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = sizes.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (!string.IsNullOrEmpty(tempStr[i]))
                            {
                                if (string.IsNullOrEmpty(tempWhr))
                                {
                                    tempWhr = $"\n	   규격  LIKE '%{tempStr[i]}%' ";
                                }
                                else
                                {
                                    tempWhr += $"\n	   OR 규격   LIKE '%{tempStr[i]}%' ";
                                }
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 규격   LIKE '%{sizes}%'                                                                         ");
                }
            }
            qry.Append($"\n  GROUP BY 품명, 원산지, 규격, 단위, 단위수량, 가격단위, SEAOVER단위, 매입단가, 매입처, 매출단가, 단가수정일                                                   ");
            qry.Append($"\n  ORDER BY 품명, 원산지, 규격, SEAOVER단위, 매입단가, 단가수정일 DESC                                                                           ");

            string sql = qry.ToString();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCostCalculateForPurchasePrice(string sttdate, string enddate, string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                ");
            qry.Append($"\n   품명                                                                                                 ");
            qry.Append($"\n , 원산지                                                                                                ");
            qry.Append($"\n , 규격                                                                                                 ");
            qry.Append($"\n , 단위                                                                                                 ");
            qry.Append($"\n , 단위수량                                                                                               ");
            qry.Append($"\n , 가격단위                                                                                               ");
            qry.Append($"\n , SEAOVER단위                                                                                          ");
            qry.Append($"\n , CASE WHEN 가격단위 = '묶' OR 가격단위 = '묶음' THEN CONVERT(varchar ,CONVERT(float, SEAOVER단위) / CONVERT(float, 단위수량))         ");
            qry.Append($"\n        WHEN 가격단위 = '팩' THEN CONVERT(varchar ,CONVERT(float, SEAOVER단위) / CONVERT(float, 단위수량))    ");
            qry.Append($"\n     ELSE SEAOVER단위                                                                                   ");
            qry.Append($"\n     END AS 계산단위                                                                                      ");
            qry.Append($"\n , ISNULL(매입단가, 0) AS 매입단가                                                                            ");
            qry.Append($"\n , 단가수정일                                                                                              ");
            qry.Append($"\n FROM  업체별시세관리                                                                                        ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                                                                                  ");
            qry.Append($"\n   AND 매입단가 > 10                                                                                        ");
            if (!string.IsNullOrEmpty(sttdate))
            { 
                qry.Append($"\n	  AND 단가수정일 >= '{sttdate}'                                                                            ");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                qry.Append($"\n	  AND 단가수정일 <= '{enddate}'                                                                            ");
            }
            string[] tempStr = null;
            string tempWhr = "";
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = product.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   품명  LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR 품명   LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 품명   LIKE '%{product}%'                                                                         ");
                }
            }

            if (!(origin == null || string.IsNullOrEmpty(origin)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = origin.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   원산지  LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR 원산지   LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 원산지   LIKE '%{origin}%'                                                                         ");
                }
            }
            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = sizes.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   규격  LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR 규격   LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 규격   LIKE '%{sizes}%'                                                                         ");
                }
            }
            /*if (!(unit == null || string.IsNullOrEmpty(unit)))
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
                            tempWhr = $"\n	   SEAOVER단위  LIKE '%{tempStr[i]}%' ";
                        }
                        else
                        {
                            tempWhr += $"\n	   OR SEAOVER단위   LIKE '%{tempStr[i]}%' ";
                        }

                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND SEAOVER단위   LIKE '%{unit}%'                                                                         ");
                }
            }*/
            qry.Append($"\n GROUP BY 품명, 원산지, 규격, 단위, 단위수량, 가격단위, SEAOVER단위, 매입단가, 단가수정일                                      ");
            if (!string.IsNullOrEmpty(sttdate) && !string.IsNullOrEmpty(enddate))
            {
                qry.Append($"\n ORDER BY 품명, 원산지, 규격, SEAOVER단위, 매입단가                                                               ");
            }
            else
            {
                qry.Append($"\n ORDER BY 품명, 원산지, 규격, SEAOVER단위, 단가수정일 DESC                                                               ");
            }

            /*MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance2.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetFormDataTempModel(dr);*/

            string sql = qry.ToString();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetPriceForCostAccount2(string sttdate, string enddate, string product, string origin, string sizes, string unit)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                             ");
            qry.Append($"\n   품명                                             ");
            qry.Append($"\n , 원산지                                           ");
            qry.Append($"\n , 규격                                             ");
            qry.Append($"\n , SEAOVER단위                                      ");
            qry.Append($"\n , ISNULL(매입단가, 0) AS 매입단가                  ");
            qry.Append($"\n , 매입처                                           ");
            qry.Append($"\n , ISNULL(매출단가, 0) AS 매출단가                  ");
            qry.Append($"\n , 단가수정일                                       ");
            qry.Append($"\n FROM  업체별시세관리                               ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                          ");
            qry.Append($"\n	  AND 단가수정일 >= '{sttdate}'                    ");
            qry.Append($"\n	  AND 단가수정일 <= '{enddate}'                    ");
            qry.Append($"\n   AND 품명 = '{product}'                           ");
            qry.Append($"\n   AND 원산지 = '{origin}'                          ");
            qry.Append($"\n   AND 규격 = '{sizes}'                             ");
            qry.Append($"\n   AND SEAOVER단위 = '{unit}'                       ");
            qry.Append($"\n   AND 매입처 <> '재고'   AND 매입단가 > 1          ");
            qry.Append($"\n GROUP BY 품명, 원산지, 규격, SEAOVER단위, 매입단가, 매입처, 매출단가, 단가수정일    ");
            qry.Append($"\n  ORDER BY 단가수정일 DESC                                                           ");

            /*MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance2.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetFormDataTempModel(dr);*/

            string sql = qry.ToString();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        private List<FormDataTemp> SetFormDataTempModel(MySqlDataReader rd)
        {
            List<FormDataTemp> list = new List<FormDataTemp>();
            while (rd.Read())
            {
                FormDataTemp model = new FormDataTemp();
                model.temp_id = Convert.ToInt32(rd["temp_id"].ToString());
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.updatetime = rd["updatetime"].ToString();

                list.Add(model);
            }
            rd.Close();
            return list;
        }

        public int UpdateExculde(FormDataModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_form_data SET                  ");
            qry.Append($" sales_price = {model.sales_price}       ");
            qry.Append($" WHERE 1=1                               ");
            qry.Append($"   AND category_code = '{model.category_code}'              ");
            qry.Append($"   AND product_code = '{model.product_code}'                ");
            qry.Append($"   AND origin_code = '{model.origin_code}'                  ");
            qry.Append($"   AND sizes_code = '{model.sizes_code}'                    ");
            qry.Append($"   AND unit = '{model.unit}'                                ");
            qry.Append($"   AND price_unit = '{model.price_unit}'                    ");
            qry.Append($"   AND unit_count = '{model.unit_count}'                    ");
            qry.Append($"   AND seaover_unit = '{model.seaover_unit}'                ");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public List<SeaoverProductModel> GetProductSimple(string product, string origin, string sizes)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	 SELECT                                                                                               ");
            qry.Append($"\n	   품명                                                                                               ");
            qry.Append($"\n	 , 원산지                                                                                             ");
            qry.Append($"\n	 , 규격                                                                                               ");
            qry.Append($"\n	 FROM 업체별시세관리                                                                                  ");
            qry.Append($"\n  WHERE 사용자 = '{userId}'                                                                            ");
            if (!string.IsNullOrEmpty(product))
            {
                qry.Append($"\n	   AND 품명 LIKE '%{product}%'                                                                ");
            }
            if (!string.IsNullOrEmpty(origin))
            {
                qry.Append($"\n	   AND 원산지 LIKE '%{origin}%'                                                                ");
            }
            if (!string.IsNullOrEmpty(sizes))
            {
                qry.Append($"\n	   AND 규격 LIKE '%{sizes}%'                                                                ");
            }
            qry.Append($"\n  GROUP BY 품명, 원산지, 규격                                                                      ");

            string sql = qry.ToString();

            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetProductSimpleModel(dr);
        }

        public List<SeaoverProductModel> GetProductAsOne(string product = null, string origin = null, string sizes = null, string unit = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	 SELECT                                                                                           ");
            qry.Append($"\n	   품명                                                                                           ");
            qry.Append($"\n	 , 원산지                                                                                         ");
            qry.Append($"\n	 , 규격                                                                                           ");
            qry.Append($"\n	 , SEAOVER단위                                                                                    ");
            qry.Append($"\n	 FROM 업체별시세관리                                                                              ");
            qry.Append($"\n WHERE 사용자 = '{userId}'                          ");
            qry.Append($"\n	   AND 품명 = '{product}'                                                                         ");
            qry.Append($"\n	   AND 원산지 = '{origin}'                                                                        ");
            if (sizes != null)
            { 
                qry.Append($"\n	   AND 규격 = '{sizes}'                                                                           ");
            }
            if (unit != null)
            { 
                qry.Append($"\n	   AND SEAOVER단위 = '{unit}'                                                                     ");
            }
            string sql = qry.ToString();

            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return  SetProductAsOne(dr);
        }
        public List<SeaoverProductModel> GetProductAsOneGroupby(string product = null, string origin = null, string sizes = null, string unit = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                               ");
            qry.Append($"\n   p.품명                                                                                                             ");
            qry.Append($"\n , p.원산지                                                                                                           ");
            qry.Append($"\n , p.규격                                                                                                             ");
            qry.Append($"\n , p.규격2                                                                                                            ");
            qry.Append($"\n , p.SEAOVER단위                                                                                                      ");
            qry.Append($"\n , CASE                                                                                                               ");
            qry.Append($"\n   	WHEN p.가격단위 = '팩' THEN p.단위                                                                               ");
            qry.Append($"\n    	ELSE p.SEAOVER단위                                                                                               ");
            qry.Append($"\n    	END AS 단위                                                                                                      ");
            qry.Append($"\n FROM(                                                                                                                ");
            qry.Append($"\n 	SELECT                                                                                                           ");
            qry.Append($"\n 	  p.품명                                                                                                         ");
            qry.Append($"\n 	, p.원산지                                                                                                       ");
            qry.Append($"\n 	, p.규격                                                                                                         ");
            qry.Append($"\n 	, CASE                                                                                                           ");
            qry.Append($"\n 	  WHEN ISNUMERIC(p.규격2)=1  THEN p.규격2                                                                        ");
            qry.Append($"\n 	  ELSE '9999'                                                                                                    ");
            qry.Append($"\n 	  END AS 규격2                                                                                                   ");
            qry.Append($"\n 	, p.SEAOVER단위                                                                                                  ");
            qry.Append($"\n 	, p.단위                                                                                                         ");
            qry.Append($"\n 	, p.가격단위                                                                                                     ");
            qry.Append($"\n 	FROM(                                                                                                            ");
            qry.Append($"\n 		SELECT                                                                                                       ");
            qry.Append($"\n 		  품명                                                                                                       ");
            qry.Append($"\n 		, 원산지                                                                                                     ");
            qry.Append($"\n 		, 규격                                                                                                       ");
            qry.Append($"\n 		, CASE                                                                                                       ");
            qry.Append($"\n 		  WHEN CHARINDEX('미', 규격) > 0 AND CHARINDEX('/', 규격) > 0 THEN                                           ");
            qry.Append($"\n 			CASE                                                                                                     ");
            qry.Append($"\n 				WHEN CHARINDEX('미', 규격) > CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('/', 규격))     ");
            qry.Append($"\n 				WHEN CHARINDEX('미', 규격) < CHARINDEX('/', 규격) THEN  SUBSTRING(규격, 0, CHARINDEX('미', 규격))    ");
            qry.Append($"\n 			END                                                                                                      ");
            qry.Append($"\n 		  WHEN CHARINDEX('미', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('미', 규격))                              ");
            qry.Append($"\n 		  WHEN CHARINDEX('/', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('/', 규격))                                ");
            qry.Append($"\n 		  WHEN CHARINDEX('kg', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('kg', 규격))                              ");
            qry.Append($"\n 		  WHEN CHARINDEX('L', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('L', 규격))                                ");
            qry.Append($"\n 		  WHEN CHARINDEX('UP', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('UP', 규격))                              ");
            qry.Append($"\n 		  WHEN CHARINDEX('번', 규격) > 0 THEN SUBSTRING(규격, 0, CHARINDEX('번', 규격))                              ");
            qry.Append($"\n 		  ELSE '9999'                                                                                                ");
            qry.Append($"\n 		  END AS 규격2                                                                                               ");
            qry.Append($"\n 		, SEAOVER단위                                                                                                ");
            qry.Append($"\n 		, 단위                                                                                                       ");
            qry.Append($"\n 		, 가격단위                                                                                                   ");
            qry.Append($"\n 		 FROM 업체별시세관리                                                                                         ");
            qry.Append($"\n 		 WHERE 사용자 = '{userId}'                                                                                   ");
            string[] tempStr = null;
            string tempWhr = "";
            if (!(product == null || string.IsNullOrEmpty(product)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = product.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        { 
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   품명  LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR 품명   LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 품명   LIKE '%{product}%'                                                                         ");
                }
            }
            if (!(origin == null || string.IsNullOrEmpty(origin)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = origin.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   원산지  LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR 원산지   LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 원산지   LIKE '%{origin}%'                                                                         ");
                }
            }

            if (!(sizes == null || string.IsNullOrEmpty(sizes)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = sizes.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   규격  LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR 규격   LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND 규격   LIKE '%{sizes}%'                                                                         ");
                }
            }
            if (!(unit == null || string.IsNullOrEmpty(unit)))
            {
                tempStr = null;
                tempWhr = "";
                tempStr = unit.Split(' ');
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                tempWhr = $"\n	   SEAOVER단위  LIKE '%{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR SEAOVER단위   LIKE '%{tempStr[i]}%' ";
                            }
                        }
                    }
                    qry.Append($"\n	 AND ( {tempWhr} )                                                                                ");
                }
                else
                {
                    qry.Append($"\n	  AND SEAOVER단위   LIKE '%{unit}%'                                                                         ");
                }
            }
            qry.Append($"\n 		GROUP BY 품명, 원산지, 규격, SEAOVER단위, 단위, 가격단위                                               ");
            qry.Append($"\n 	) AS p                                                                                                     ");
            qry.Append($"\n ) AS p                                                                                                         ");
            qry.Append($"\n ORDER BY p.품명, p.원산지, CONVERT(float, p.규격2), p.규격, p.SEAOVER단위                                      ");
            
            string sql = qry.ToString();

            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetProductAsOne(dr);
        }

        private List<SeaoverProductModel> SetProductAsOne(SqlDataReader rd)
        {
            List<SeaoverProductModel> list = new List<SeaoverProductModel>();
            while (rd.Read())
            {
                SeaoverProductModel model = new SeaoverProductModel();
                model.product = rd["품명"].ToString();
                model.origin = rd["원산지"].ToString();
                model.sizes = rd["규격"].ToString();
                model.seaover_unit = rd["SEAOVER단위"].ToString();
                model.unit = rd["단위"].ToString();

                list.Add(model);
            }
            rd.Close();
            return list;
        }


        private List<SeaoverProductModel> SetProductSimpleModel(SqlDataReader rd)
        {
            List<SeaoverProductModel> list = new List<SeaoverProductModel>();
            while (rd.Read())
            {
                SeaoverProductModel model = new SeaoverProductModel();
                model.product = rd["품명"].ToString();
                model.origin = rd["원산지"].ToString();
                model.sizes = rd["규격"].ToString();

                list.Add(model);
            }
            rd.Close();
            return list;
        }
    }
}
