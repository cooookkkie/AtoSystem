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
    public class SeaoverCompanyRepository : ClassRoot, ISeaoverCompanyRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public int GetBusinessMonths(string company_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                ");
            qry.Append($"\n count(*)                                                                              ");
            qry.Append($"\n FROM(                                                                                 ");
            qry.Append($"\n  SELECT DISTINCT                                                                      ");
            qry.Append($"\n  CONVERT(VARCHAR, YEAR(매출일자), 4) +  CONVERT(VARCHAR, MONTH (매출일자), 2) AS 년월         ");
            qry.Append($"\n  FROM 매출현황                                                                           ");
            qry.Append($"\n  WHERE 사용자 = '200009'                                                                ");
            qry.Append($"\n    AND 매출수 > 0                                                                       ");
            if(!string.IsNullOrEmpty(company_code))
            qry.Append($"\n    {commonRepository.whereSql("매출처코드",  company_code, ",", true)}                                                            ");
            qry.Append($"\n ) AS t                                                                                ");
            string sql = qry.ToString();

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            int result = (int)command.ExecuteScalar();

            return result;
        }

        public DataTable GetSeaoverCompanyInfo(string company_name, string company_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                    ");
            qry.Append($"\n a.*                                       ");
            qry.Append($"\n FROM 업체별월별매출현황 AS a              ");
            qry.Append($"\n WHERE 사용자 = (                          ");
            qry.Append($"\n 	SELECT                                ");
            qry.Append($"\n 	MAX(사용자)                           ");
            qry.Append($"\n 	FROM 업체별월별매출현황 AS b          ");
            qry.Append($"\n 	WHERE a.거래처코드 = b.거래처코드     ");
            qry.Append($"\n   )                                       ");
            if(!string.IsNullOrEmpty(company_name))
                qry.Append($"\n   AND 거래처명 LIKE '%{company_name}%'                  ");
            if (!string.IsNullOrEmpty(company_code))
                qry.Append($"\n   AND 거래처코드 = '{company_code}'                     ");



            string sql = qry.ToString();

            dbInstance3.Connection.Close();
            SqlCommand command = new SqlCommand(qry.ToString(), (SqlConnection)dbInstance3.Connection);
            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetSeaoverCompanyInfo(string company_name, string company_code, string registration_number, string ceo, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                    ");
            qry.Append($"\n a.*                                       ");
            qry.Append($"\n FROM 업체별월별매출현황 AS a              ");
            qry.Append($"\n WHERE 사용자 = (                          ");
            qry.Append($"\n 	SELECT                                ");
            qry.Append($"\n 	MAX(사용자)                           ");
            qry.Append($"\n 	FROM 업체별월별매출현황 AS b          ");
            qry.Append($"\n 	WHERE a.거래처코드 = b.거래처코드     ");
            qry.Append($"\n   )                                       ");
            qry.Append($"\n   AND a.폐업유무 <> 'Y'                                       ");
            if (!string.IsNullOrEmpty(company_name))
                qry.Append($"\n   {commonRepository.whereSql("거래처명", company_name)}                  ");
            if (!string.IsNullOrEmpty(company_code))
                qry.Append($"\n   {commonRepository.whereSql("거래처코드", company_code)}                  ");
            if (!string.IsNullOrEmpty(registration_number))
                qry.Append($"\n   {commonRepository.whereSql("사업자번호", registration_number)}                  ");
            if (!string.IsNullOrEmpty(ceo))
                qry.Append($"\n   {commonRepository.whereSql("대표자명", ceo)}                  ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n   {commonRepository.whereSql("매출자", manager)}                  ");
            qry.Append($"\n ORDER BY 거래처명                                       ");

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
