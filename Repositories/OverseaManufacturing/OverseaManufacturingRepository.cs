using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OverseaManufacturing
{
    public class OverseaManufacturingRepository : ClassRoot, IOverseaManufacturingRepository
    {

        public DataTable GetDistinctData(string col)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n select                     ");
            qry.Append($"\n distinct {col}      ");
            qry.Append($"\n FROM income                ");
            qry.Append($"\n ORDER BY {col}      ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetData(string sttdate, string enddate, string product_kor, string product_eng, string search_type, string company, bool isExactly)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n select                                                           ");
            qry.Append($"\n   *                             ");
            qry.Append($"\n FROM income                                                      ");
            qry.Append($"\n WHERE im_date >= '{sttdate}' AND im_date <= '{enddate}'        ");

            if(!string.IsNullOrEmpty(product_kor))
                qry.Append($"\n   {whereSql("pname_kor" , product_kor, false, '^', isExactly)}        ");
            if (!string.IsNullOrEmpty(product_eng))
                qry.Append($"\n   {whereSql("pname_eng", product_eng, false, '^', isExactly)}        ");
            if (!string.IsNullOrEmpty(company))
            { 
                if(search_type == "수입처")
                    qry.Append($"\n   {whereSql("importer", company, false, '^', true)}        ");
                else
                    qry.Append($"\n   {whereSql("manufacturing", company, false, '^', true)}        ");
            }


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        

        public DataTable GetUploadData(string sttdate, string enddate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n select                                                           ");
            qry.Append($"\n   DATE_FORMAT(im_date, '%d') AS days                             ");
            qry.Append($"\n , SUM(IF(division = '가공식품', 1, 0)) AS processed_food         ");
            qry.Append($"\n , SUM(IF(division = '수산물', 1, 0)) AS sea_food                 ");
            qry.Append($"\n , SUM(IF(division = '축산물', 1, 0)) AS livestock_product        ");
            qry.Append($"\n , SUM(IF(division = '농.임산물', 1, 0)) AS agricultural_product  ");
            qry.Append($"\n FROM income                                                      ");
            qry.Append($"\n WHERE im_date >= '{sttdate}' AND im_date <= '{enddate}'        ");
            qry.Append($"\n GROUP BY DATE_FORMAT(im_date, '%d')                              ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetCompany(string col, string company)
        {
            StringBuilder qry = new StringBuilder();

            string col_name = "1=1";
            if (col == "수입처")
                col_name = "importer";
            else if (col == "제조공장")
                col_name = "manufacturing";
            else if (col == "한글품명")
                col_name = "pname_kor";
            else if (col == "영어품명")
                col_name = "pname_eng";

            qry.Append($"\n SELECT                                                             ");
            qry.Append($"\n   TRIM({col_name}) AS {col}     ");
            qry.Append($"\n FROM(                                                              ");
            qry.Append($"\n  SELECT                                                            ");
            qry.Append($"\n    distinct {col_name}                                               ");
            qry.Append($"\n  FROM income                                                       ");
            qry.Append($"\n  WHERE 1= 1                                                       ");
            if(!string.IsNullOrEmpty(company))
                qry.Append($"\n    AND {col_name} LIKE '%{company}%'                                                       ");
            qry.Append($"\n ) AS t                                                             ");
            qry.Append($"\n ORDER BY {col_name}                                                  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetProduct(string pname_kor, string pname_eng)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                            ");
            qry.Append($"\n    distinct pname_kor, pname_eng                                   ");
            qry.Append($"\n  FROM income                                                       ");
            qry.Append($"\n  WHERE 1=1                                                         ");
            if (!string.IsNullOrEmpty(pname_kor))
                qry.Append($"\n    AND pname_kor LIKE '%{pname_kor}%'                                                       ");
            if (!string.IsNullOrEmpty(pname_eng))
                qry.Append($"\n    AND pname_eng LIKE '%{pname_kor}%'                                                       ");
            qry.Append($"\n  ORDER BY pname_kor, pname_eng                                     ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetHandlingProduct(int type, List<string> companyList, string sortType, string sttdate, string enddate, string product_kor = "", string product_eng = "")
        {
            string company;
            if (type % 2 == 1)
                company = "manufacturing";
            else
                company = "importer";

            StringBuilder qry = new StringBuilder();
            qry.Append($"\n	  SELECT                                                      ");
            qry.Append($"\n	     {company} AS company                                 ");
            qry.Append($"\n	   , pname_kor                                                ");
            qry.Append($"\n	   , pname_eng                                                ");
            qry.Append($"\n	   , MAX(product_type)                                             ");
            qry.Append($"\n    , m_country                                             ");
            qry.Append($"\n    , e_country                                             ");
            qry.Append($"\n	   , SUM(1) AS qty                                            ");
            qry.Append($"\n	   , MAX(im_date) AS updatetime                            ");
            qry.Append($"\n	   FROM income                                                ");
            qry.Append($"\n	   WHERE 1=1                                                  ");
            if (companyList != null && companyList.Count > 0)
            {
                if (companyList.Count == 0)
                    qry.Append($"\n    AND {company} = '{companyList[0]}'           ");
                else
                {
                    qry.Append($"\n    AND (           ");
                    string whr = "";
                    for (int i = 0; i < companyList.Count; i++)
                    {
                        if (string.IsNullOrEmpty(whr))
                            whr += $"\n    {company} = '{companyList[i]}'           ";
                        else
                            whr += $"\n    OR {company} = '{companyList[i]}'           ";
                    }
                    qry.Append($"\n    {whr}           ");
                    qry.Append($"\n    )               ");
                }
            }

            if(!string.IsNullOrEmpty(product_kor))
                qry.Append($"\n	   AND pname_kor LIKE '%{product_kor}%'                                                  ");
            if (!string.IsNullOrEmpty(product_eng))
                qry.Append($"\n	   AND pname_eng LIKE '%{product_eng}%'                                                  ");

            if (DateTime.TryParse(sttdate, out DateTime sttDt))
                qry.Append($"\n	     AND im_date >= '{sttDt.ToString("yyyy-MM-dd")}'                                                  ");
            if (DateTime.TryParse(enddate, out DateTime endDt))
                qry.Append($"\n	     AND im_date <= '{endDt.ToString("yyyy-MM-dd")}'                                                  ");

            qry.Append($"\n	   GROUP BY {company}, pname_eng, pname_kor, m_country, e_country    ");






/*

            qry.Append($"\n  SELECT                                                       ");
            qry.Append($"\n    t1.company                                                 ");
            qry.Append($"\n  , t2.pname_kor                                               ");
            qry.Append($"\n  , t1.pname_eng                                               ");
            qry.Append($"\n  , t1.m_country                                               ");
            qry.Append($"\n  , t1.e_country                                               ");
            qry.Append($"\n  , t1.qty                                                     ");
            qry.Append($"\n  , t1.updatetime                                              ");
            qry.Append($"\n  FROM(                                                        ");
            qry.Append($"\n	  SELECT                                                      ");
            qry.Append($"\n		MAX(id) AS id                                             ");
            qry.Append($"\n	   , {company} AS company                                 ");
            qry.Append($"\n	   , pname_eng                                                ");
            qry.Append($"\n       , m_country                                             ");
            qry.Append($"\n       , e_country                                             ");
            qry.Append($"\n	   , SUM(1) AS qty                                            ");
            qry.Append($"\n	   , MAX(im_date) AS updatetime                            ");
            qry.Append($"\n	   FROM income                                                ");
            qry.Append($"\n	   WHERE 1=1                                                  ");
            if (companyList != null && companyList.Count > 0)
            {
                if (companyList.Count == 0)
                    qry.Append($"\n    AND {company} = '{companyList[0]}'           ");
                else
                {
                    qry.Append($"\n    AND (           ");
                    string whr = "";
                    for (int i = 0; i < companyList.Count; i++)
                    {
                        if (string.IsNullOrEmpty(whr))
                            whr += $"\n    {company} = '{companyList[i]}'           ";
                        else
                            whr += $"\n    OR {company} = '{companyList[i]}'           ";
                    }
                    qry.Append($"\n    {whr}           ");
                    qry.Append($"\n    )               ");
                }
            }
            qry.Append($"\n	   GROUP BY {company}, pname_eng, m_country, e_country    ");
            qry.Append($"\n   ) AS t1                                                     ");
            qry.Append($"\n   LEFT OUTER JOIN (                                           ");
            qry.Append($"\n		SELECT                                                    ");
            qry.Append($"\n		id                                                        ");
            qry.Append($"\n	   , pname_kor                                                ");
            qry.Append($"\n	   , pname_eng                                                ");
            qry.Append($"\n	   FROM income                                                ");
            qry.Append($"\n	   WHERE 1=1                                                  ");
            if (companyList != null && companyList.Count > 0)
            {
                if (companyList.Count == 0)
                    qry.Append($"\n    AND {company} = '{companyList[0]}'           ");
                else
                {
                    qry.Append($"\n    AND (           ");
                    string whr = "";
                    for (int i = 0; i < companyList.Count; i++)
                    {
                        if (string.IsNullOrEmpty(whr))
                            whr += $"\n    {company} = '{companyList[i]}'           ";
                        else
                            whr += $"\n    OR {company} = '{companyList[i]}'           ";
                    }
                    qry.Append($"\n    {whr}           ");
                    qry.Append($"\n    )               ");
                }
            }
            qry.Append($"\n   ) AS t2                                                     ");
            qry.Append($"\n     ON t1.id = t2.id                                          ");*/
            if (string.IsNullOrEmpty(sortType))
                qry.Append($"\n  ORDER BY {company}, pname_kor, pname_eng, m_country, e_country   ");
            else
            {
                string[] sort = sortType.Split('+');
                if (sort.Length > 0)
                {
                    qry.Append($"\n  ORDER BY ");
                    string sort_txt = "";
                    for (int i = 0; i < sort.Length; i++)
                    {
                        if(string.IsNullOrEmpty(sort_txt))
                            sort_txt = $" {GetSortColumnName(sort[i])}";
                        else
                            sort_txt += $", {GetSortColumnName(sort[i])}";
                    }
                    qry.Append($"  {sort_txt}");
                }
            }

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        private string GetSortColumnName(string col)
        {
            switch (col)
            {
                case "거래처":
                    return "company";
                case "품명(한글)":
                    return "pname_kor";
                case "품명(영문)":
                    return "pname_eng";
                case "제조국":
                    return "m_country";
                case "수출국":
                    return "e_country";
                case "최근일자":
                    return "updatetime";
                default:
                    return "";
            }
        }


        public DataTable GetLastImdate(string division = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                    ");
            qry.Append($"\n    MAX(im_date) AS im_date                ");
            qry.Append($"\n FROM income                               ");
            qry.Append($"\n WHERE 1=1                                 ");
            if(!string.IsNullOrEmpty(division))
                qry.Append($"\n   AND division = '{division}'                                 ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetDetail(int search_type, int id, string sttdate, string enddate, string division
                            , string product_kor, string product_kor2, string product_eng, string product_eng2
                            , string manufacturing, bool isExactly1, string income_company, bool isExactly2
                            , string manufacture_country, string export_country)
        {
            StringBuilder qry = new StringBuilder();
            if (search_type == 1)
            {
                qry.Append($"\n SELECT                                    ");
                qry.Append($"\n   i2.importer AS company                  ");
                qry.Append($"\n , i2.im_date AS im_date                   ");
                qry.Append($"\n , i1.pname_kor AS pname_kor               ");
                qry.Append($"\n , i1.pname_eng AS pname_eng               ");
                qry.Append($"\n FROM(                                     ");
                qry.Append($"\n    SELECT                                 ");
                qry.Append($"\n      division AS division                 ");
                qry.Append($"\n    , pname_kor AS pname_kor               ");
                qry.Append($"\n    , pname_eng AS pname_eng               ");
                qry.Append($"\n    , manufacturing AS manufacturing       ");
                qry.Append($"\n    FROM income                            ");
                qry.Append($"\n    WHERE id = {id}                        ");
                qry.Append($"\n ) AS i1                                   ");
                qry.Append($"\n LEFT OUTER JOIN (                         ");
                qry.Append($"\n   SELECT                                  ");
                qry.Append($"\n     division AS division                  ");
                qry.Append($"\n   , pname_kor AS pname_kor                ");
                qry.Append($"\n   , pname_eng AS pname_eng                ");
                qry.Append($"\n   , manufacturing AS manufacturing        ");
                qry.Append($"\n   , importer AS importer                  ");
                qry.Append($"\n   , im_date AS im_date                    ");
                qry.Append($"\n   FROM income                             ");
                qry.Append($"\n   WHERE 1=1                               ");
                if (!string.IsNullOrEmpty(sttdate))
                    qry.Append($"\n     AND im_date >= '{sttdate}'                                   ");
                if (!string.IsNullOrEmpty(enddate))
                    qry.Append($"\n     AND im_date <= '{enddate}'                                   ");
                if (!string.IsNullOrEmpty(division))
                    qry.Append($"\n     {whereSql("division", division)}                             ");
                if (!string.IsNullOrEmpty(product_kor))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor)}                         ");
                if (!string.IsNullOrEmpty(product_kor2))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor2)}                         ");
                if (!string.IsNullOrEmpty(product_eng))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng)}                         ");
                if (!string.IsNullOrEmpty(product_eng2))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng2)}                         ");

                if (!string.IsNullOrEmpty(manufacturing))
                {
                    if (isExactly1)
                        qry.Append($"\n     AND manufacturing LIKE '%{manufacturing}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("manufacturing", manufacturing)}                   ");
                }

                if (!string.IsNullOrEmpty(income_company))
                {
                    if (isExactly2)
                        qry.Append($"\n     AND importer LIKE '%{income_company}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("importer", income_company)}                       ");
                }
                if (!string.IsNullOrEmpty(manufacture_country))
                    qry.Append($"\n     {whereSql("m_country", manufacture_country)}                 ");
                if (!string.IsNullOrEmpty(export_country))
                    qry.Append($"\n     {whereSql("e_country", export_country)}                      ");
                qry.Append($"\n )AS i2                                    ");
                qry.Append($"\n   ON i1.division = i2.division            ");
                qry.Append($"\n   AND i1.pname_kor = i2.pname_kor         ");
                qry.Append($"\n   AND i1.pname_eng = i2.pname_eng         ");
                qry.Append($"\n   AND i1.manufacturing = i2.manufacturing ");
                qry.Append($"\n ORDER BY i2.im_date DESC, i2.importer          ");

            }
            else
            {
                qry.Append($"\n SELECT                                       ");
                qry.Append($"\n   i2.manufacturing AS company          ");
                qry.Append($"\n , i2.im_date AS im_date                      ");
                qry.Append($"\n , i1.pname_kor AS pname_kor                  ");
                qry.Append($"\n , i1.pname_eng AS pname_eng                  ");
                qry.Append($"\n FROM(                                        ");
                qry.Append($"\n    SELECT                                    ");
                qry.Append($"\n      division AS division                    ");
                qry.Append($"\n    , pname_kor AS pname_kor                  ");
                qry.Append($"\n    , pname_eng AS pname_eng                  ");
                qry.Append($"\n    , importer AS importer                    ");
                qry.Append($"\n    FROM income                               ");
                qry.Append($"\n    WHERE id = {id}                         ");
                qry.Append($"\n ) AS i1                                      ");
                qry.Append($"\n LEFT OUTER JOIN (                            ");
                qry.Append($"\n   SELECT                                     ");
                qry.Append($"\n     division AS division                     ");
                qry.Append($"\n   , pname_kor AS pname_kor                   ");
                qry.Append($"\n   , pname_eng AS pname_eng                   ");
                qry.Append($"\n   , manufacturing AS manufacturing           ");
                qry.Append($"\n   , importer AS importer                     ");
                qry.Append($"\n   , im_date AS im_date                       ");
                qry.Append($"\n   FROM income                                ");
                qry.Append($"\n   WHERE 1=1                                  ");
                if (!string.IsNullOrEmpty(sttdate))
                    qry.Append($"\n     AND im_date >= '{sttdate}'                                   ");
                if (!string.IsNullOrEmpty(enddate))
                    qry.Append($"\n     AND im_date <= '{enddate}'                                   ");
                if (!string.IsNullOrEmpty(division))
                    qry.Append($"\n     {whereSql("division", division)}                             ");
                if (!string.IsNullOrEmpty(product_kor))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor)}                         ");
                if (!string.IsNullOrEmpty(product_kor2))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor2)}                         ");
                if (!string.IsNullOrEmpty(product_eng))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng)}                         ");
                if (!string.IsNullOrEmpty(product_eng2))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng2)}                         ");

                if (!string.IsNullOrEmpty(manufacturing))
                {
                    if (isExactly1)
                        qry.Append($"\n     AND manufacturing LIKE '%{manufacturing}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("manufacturing", manufacturing)}                   ");
                }

                if (!string.IsNullOrEmpty(income_company))
                {
                    if (isExactly2)
                        qry.Append($"\n     AND importer LIKE '%{income_company}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("importer", income_company)}                       ");
                }
                if (!string.IsNullOrEmpty(manufacture_country))
                    qry.Append($"\n     {whereSql("m_country", manufacture_country)}                 ");
                if (!string.IsNullOrEmpty(export_country))
                    qry.Append($"\n     {whereSql("e_country", export_country)}                      ");
                qry.Append($"\n )AS i2                                       ");
                qry.Append($"\n   ON i1.division = i2.division               ");
                qry.Append($"\n   AND i1.pname_kor = i2.pname_kor            ");
                qry.Append($"\n   AND i1.pname_eng = i2.pname_eng            ");
                qry.Append($"\n   AND i1.importer = i2.importer              ");
                qry.Append($"\n ORDER BY i2.im_date DESC, i2.manufacturing        ");
            }
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetData(int search_type, string sttdate, string enddate, string division
                            , string product_kor, string product_kor2, string except1
                            , string product_eng, string product_eng2, string except2
                            , string manufacturing, bool isExactly1, string income_company, bool isExactly2
                            , string manufacture_country, string export_country, string product_type)
        {
            StringBuilder qry = new StringBuilder();
            if (search_type == 0)
            {
                qry.Append($"\n   SELECT                                                             ");
                qry.Append($"\n     *                                                                ");
                qry.Append($"\n   , CONCAT(division, '^', importer, '^', pname_kor, '^', pname_eng, '^', manufacturing, '^', im_date, '^', m_country, '^', e_country, '^') AS data_code                                                               ");
                qry.Append($"\n   , 1 AS cnt                                                         ");
                qry.Append($"\n   FROM income                                                        ");
                qry.Append($"\n   WHERE 1=1                                                          ");
                if (!string.IsNullOrEmpty(sttdate))
                    qry.Append($"\n     AND im_date >= '{sttdate}'                                   ");
                if (!string.IsNullOrEmpty(enddate))
                    qry.Append($"\n     AND im_date <= '{enddate}'                                   ");
                if (!string.IsNullOrEmpty(division))
                    qry.Append($"\n     {whereSql("division", division)}                             ");
                if (!string.IsNullOrEmpty(product_kor))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor)}                         ");
                if (!string.IsNullOrEmpty(product_kor2))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor2)}                         ");
                if (!string.IsNullOrEmpty(except1))
                    qry.Append($"\n     {whereSql("pname_kor", except1, true)}                         ");


                if (!string.IsNullOrEmpty(product_eng))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng)}                         ");
                if (!string.IsNullOrEmpty(product_eng2))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng2)}                         ");
                if (!string.IsNullOrEmpty(except2))
                    qry.Append($"\n     {whereSql("pname_eng", except2, true)}                         ");

                if (!string.IsNullOrEmpty(manufacturing))
                { 
                    if(isExactly1)
                        qry.Append($"\n     AND manufacturing LIKE '%{manufacturing}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("manufacturing", manufacturing)}                   ");
                }

                if (!string.IsNullOrEmpty(income_company))
                {
                    if (isExactly2)
                        qry.Append($"\n     AND importer LIKE '%{income_company}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("importer", income_company)}                       ");
                }
                if (!string.IsNullOrEmpty(manufacture_country))
                    qry.Append($"\n     {whereSql("m_country", manufacture_country)}                 ");
                if (!string.IsNullOrEmpty(export_country))
                    qry.Append($"\n     {whereSql("e_country", export_country)}                      ");
                if (!string.IsNullOrEmpty(product_type))
                    qry.Append($"\n     AND product_type LIKE '%{product_type}%'                      ");

                qry.Append($"\n ORDER BY im_date DESC, pname_kor, pname_eng                                                          ");
            }
            else if (search_type == 1)
            {
                qry.Append($"\n SELECT                                                               ");
                qry.Append($"\n   i1.id AS id                                                        ");
                qry.Append($"\n , i1.division AS division                                            ");
                qry.Append($"\n , i1.pname_kor AS pname_kor                                          ");
                qry.Append($"\n , i1.pname_eng AS pname_eng                                          ");
                qry.Append($"\n , MAX(i1.product_type) AS product_type                                    ");
                qry.Append($"\n , '' AS importer                                                     ");
                qry.Append($"\n , im_date                                                            ");
                qry.Append($"\n , '' AS until_date                                                   ");
                qry.Append($"\n , i1.manufacturing AS manufacturing                                  ");
                qry.Append($"\n , i1.m_country AS m_country                                          ");
                qry.Append($"\n , i1.e_country AS e_country                                          ");
                qry.Append($"\n , SUM(i2.cnt) AS cnt                                                      ");
                qry.Append($"\n , '선택'                                                             ");
                
                qry.Append($"\n FROM(                                                                ");
                qry.Append($"\n   SELECT                                                             ");
                qry.Append($"\n     id AS id                                                         ");
                qry.Append($"\n   , division AS division                                             ");
                qry.Append($"\n   , pname_kor AS pname_kor                                           ");
                qry.Append($"\n   , pname_eng AS pname_eng                                           ");
                qry.Append($"\n   , product_type AS product_type                                     ");
                qry.Append($"\n   , manufacturing AS manufacturing                                   ");
                qry.Append($"\n   , MAX(im_date) AS im_date                                          ");
                qry.Append($"\n   , m_country AS m_country                                           ");
                qry.Append($"\n   , e_country AS e_country                                           ");
                qry.Append($"\n   FROM income                                                        ");
                qry.Append($"\n   WHERE 1=1                                                          ");
                if (!string.IsNullOrEmpty(sttdate))
                    qry.Append($"\n     AND im_date >= '{sttdate}'                                   ");
                if (!string.IsNullOrEmpty(enddate))
                    qry.Append($"\n     AND im_date <= '{enddate}'                                   ");
                if (!string.IsNullOrEmpty(division))
                    qry.Append($"\n     {whereSql("division", division)}                             ");
                if (!string.IsNullOrEmpty(product_kor))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor)}                         ");
                if (!string.IsNullOrEmpty(product_kor2))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor2)}                         ");
                if (!string.IsNullOrEmpty(except1))
                    qry.Append($"\n     {whereSql("pname_kor", except1, true)}                         ");
                if (!string.IsNullOrEmpty(product_eng))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng)}                         ");
                if(!string.IsNullOrEmpty(product_eng2))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng2)}                         ");
                if (!string.IsNullOrEmpty(except2))
                    qry.Append($"\n     {whereSql("pname_eng", except2, true)}                         ");
                if (!string.IsNullOrEmpty(manufacturing))
                {
                    if (isExactly1)
                        qry.Append($"\n     AND manufacturing LIKE '%{manufacturing}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("manufacturing", manufacturing)}                   ");
                }

                if (!string.IsNullOrEmpty(income_company))
                {
                    if (isExactly2)
                        qry.Append($"\n     AND importer LIKE '%{income_company}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("importer", income_company)}                       ");
                }
                if (!string.IsNullOrEmpty(manufacture_country))
                    qry.Append($"\n     {whereSql("m_country", manufacture_country)}                 ");
                if (!string.IsNullOrEmpty(export_country))
                    qry.Append($"\n     {whereSql("e_country", export_country)}                      ");
                if (!string.IsNullOrEmpty(product_type))
                    qry.Append($"\n     AND product_type LIKE '%{product_type}%'                      ");
                qry.Append($"\n     GROUP BY division, pname_kor, pname_eng, product_type, manufacturing, m_country, e_country           ");
                qry.Append($"\n ) AS i1                                                              ");
                qry.Append($"\n INNER JOIN (                                                         ");
                qry.Append($"\n   SELECT                                                             ");
                qry.Append($"\n     division AS division                                             ");
                qry.Append($"\n   , pname_kor AS pname_kor                                           ");
                qry.Append($"\n   , pname_eng AS pname_eng                                           ");
                qry.Append($"\n   , product_type AS product_type                                     ");
                qry.Append($"\n   , manufacturing AS manufacturing                                   ");
                qry.Append($"\n   , SUM(1) AS cnt                                                    ");
                qry.Append($"\n   FROM income                                                        ");
                qry.Append($"\n   WHERE 1=1                                                          ");
                if (!string.IsNullOrEmpty(sttdate))
                    qry.Append($"\n     AND im_date >= '{sttdate}'                                   ");
                if (!string.IsNullOrEmpty(enddate))
                    qry.Append($"\n     AND im_date <= '{enddate}'                                   ");
                if (!string.IsNullOrEmpty(division))
                    qry.Append($"\n     {whereSql("division", division)}                             ");
                if (!string.IsNullOrEmpty(product_kor))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor)}                         ");
                if (!string.IsNullOrEmpty(product_kor2))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor2)}                         ");
                if (!string.IsNullOrEmpty(except1))
                    qry.Append($"\n     {whereSql("pname_kor", except1, true)}                         ");
                if (!string.IsNullOrEmpty(product_eng))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng)}                         ");
                if (!string.IsNullOrEmpty(product_eng2))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng2)}                         ");
                if (!string.IsNullOrEmpty(except2))
                    qry.Append($"\n     {whereSql("pname_eng", except2, true)}                         ");
                if (!string.IsNullOrEmpty(manufacturing))
                {
                    if (isExactly1)
                        qry.Append($"\n     AND manufacturing LIKE '%{manufacturing}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("manufacturing", manufacturing)}                   ");
                }

                if (!string.IsNullOrEmpty(income_company))
                {
                    if (isExactly2)
                        qry.Append($"\n     AND importer LIKE '%{income_company}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("importer", income_company)}                       ");
                }
                if (!string.IsNullOrEmpty(manufacture_country))
                    qry.Append($"\n     {whereSql("m_country", manufacture_country)}                 ");
                if (!string.IsNullOrEmpty(export_country))
                    qry.Append($"\n     {whereSql("e_country", export_country)}                      ");
                if (!string.IsNullOrEmpty(product_type))
                    qry.Append($"\n     AND product_type LIKE '%{product_type}%'                      ");
                qry.Append($"\n   GROUP BY division, pname_kor, pname_eng, product_type, manufacturing             ");
                qry.Append($"\n ) AS i2                                                              ");
                qry.Append($"\n    ON i1.division = i2.division                                      ");
                qry.Append($"\n    AND i1.pname_kor = i2.pname_kor                                   ");
                qry.Append($"\n    AND i1.pname_eng = i2.pname_eng                                   ");
                qry.Append($"\n    AND i1.manufacturing = i2.manufacturing                           ");
                qry.Append($"\n GROUP BY    i1.id , i1.division , i1.pname_kor  , pname_eng , i1.im_date  , i1.manufacturing  , i1.m_country  , i1.e_country    ");
                qry.Append($"\n ORDER BY i1.im_date DESC, i1.division, i1.pname_eng, i1.pname_kor, i1.manufacturing   ");
            }
            else
            {
                qry.Append($"\n SELECT                                                                    ");
                qry.Append($"\n   i1.id AS id                                                             ");
                qry.Append($"\n , i1.division AS division                                                 ");
                qry.Append($"\n , i1.importer AS importer                                                 ");
                qry.Append($"\n , '' AS manufacturing                                                     ");
                qry.Append($"\n , im_date                                                                 ");
                qry.Append($"\n , '' AS until_date                                                        ");
                qry.Append($"\n , i1.pname_kor AS pname_kor                                               ");
                qry.Append($"\n , i1.pname_eng AS pname_eng                                               ");
                qry.Append($"\n , MAX(i1.product_type) AS product_type                                         ");
                qry.Append($"\n , i1.m_country AS m_country                                               ");
                qry.Append($"\n , i1.e_country AS e_country                                               ");
                qry.Append($"\n , SUM(i2.cnt) AS cnt                                                           ");
                qry.Append($"\n , '선택'                                                                    ");
                qry.Append($"\n FROM(                                                                     ");
                qry.Append($"\n   SELECT                                                                  ");
                qry.Append($"\n     id AS id                                                              ");
                qry.Append($"\n   , division AS division                                                  ");
                qry.Append($"\n   , pname_kor AS pname_kor                                                ");
                qry.Append($"\n   , pname_eng AS pname_eng                                                ");
                qry.Append($"\n   , product_type AS product_type                                          ");
                qry.Append($"\n   , importer AS importer                                                  ");
                qry.Append($"\n   , MAX(im_date) AS im_date                                               ");
                qry.Append($"\n   , m_country AS m_country                                                ");
                qry.Append($"\n   , e_country AS e_country                                                ");
                qry.Append($"\n   FROM income                                                             ");
                qry.Append($"\n   WHERE 1=1                                                               ");
                if (!string.IsNullOrEmpty(sttdate))
                    qry.Append($"\n     AND im_date >= '{sttdate}'                                   ");
                if (!string.IsNullOrEmpty(enddate))
                    qry.Append($"\n     AND im_date <= '{enddate}'                                   ");
                if (!string.IsNullOrEmpty(division))
                    qry.Append($"\n     {whereSql("division", division)}                             ");
                if (!string.IsNullOrEmpty(product_kor))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor)}                         ");
                if (!string.IsNullOrEmpty(product_kor2))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor2)}                         ");
                if (!string.IsNullOrEmpty(except1))
                    qry.Append($"\n     {whereSql("pname_kor", except1, true)}                         ");
                if (!string.IsNullOrEmpty(product_eng))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng)}                         ");
                if (!string.IsNullOrEmpty(product_eng2))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng2)}                         ");
                if (!string.IsNullOrEmpty(except2))
                    qry.Append($"\n     {whereSql("pname_eng", except2, true)}                         ");
                if (!string.IsNullOrEmpty(manufacturing))
                {
                    if (isExactly1)
                        qry.Append($"\n     AND manufacturing LIKE '%{manufacturing}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("manufacturing", manufacturing)}                   ");
                }

                if (!string.IsNullOrEmpty(income_company))
                {
                    if (isExactly2)
                        qry.Append($"\n     AND importer LIKE '%{income_company}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("importer", income_company)}                       ");
                }
                if (!string.IsNullOrEmpty(manufacture_country))
                    qry.Append($"\n     {whereSql("m_country", manufacture_country)}                 ");
                if (!string.IsNullOrEmpty(export_country))
                    qry.Append($"\n     {whereSql("e_country", export_country)}                      ");
                if (!string.IsNullOrEmpty(product_type))
                    qry.Append($"\n     AND product_type LIKE '%{product_type}%'                      ");
                qry.Append($"\n     GROUP BY division, pname_kor, pname_eng, product_type, importer, m_country, e_country                     ");
                qry.Append($"\n ) AS i1                                                                   ");
                qry.Append($"\n INNER JOIN (                                                              ");
                qry.Append($"\n   SELECT                                                                  ");
                qry.Append($"\n     division AS division                                                  ");
                qry.Append($"\n   , pname_kor AS pname_kor                                                ");
                qry.Append($"\n   , pname_eng AS pname_eng                                                ");
                qry.Append($"\n   , product_type AS product_type                                          ");
                qry.Append($"\n   , importer AS importer                                                  ");
                qry.Append($"\n   , SUM(1) AS cnt                                                         ");
                qry.Append($"\n   FROM income                                                             ");
                qry.Append($"\n   WHERE 1=1                                                               ");
                if (!string.IsNullOrEmpty(sttdate))
                    qry.Append($"\n     AND im_date >= '{sttdate}'                                   ");
                if (!string.IsNullOrEmpty(enddate))
                    qry.Append($"\n     AND im_date <= '{enddate}'                                   ");
                if (!string.IsNullOrEmpty(division))
                    qry.Append($"\n     {whereSql("division", division)}                             ");
                if (!string.IsNullOrEmpty(product_kor))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor)}                         ");
                if (!string.IsNullOrEmpty(product_kor2))
                    qry.Append($"\n     {whereSql("pname_kor", product_kor2)}                         ");
                if (!string.IsNullOrEmpty(except1))
                    qry.Append($"\n     {whereSql("pname_kor", except1, true)}                         ");
                if (!string.IsNullOrEmpty(product_eng))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng)}                         ");
                if (!string.IsNullOrEmpty(product_eng2))
                    qry.Append($"\n     {whereSql("pname_eng", product_eng2)}                         ");
                if (!string.IsNullOrEmpty(except2))
                    qry.Append($"\n     {whereSql("pname_eng", except2, true)}                         ");
                if (!string.IsNullOrEmpty(manufacturing))
                {
                    if (isExactly1)
                        qry.Append($"\n     AND manufacturing LIKE '%{manufacturing}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("manufacturing", manufacturing)}                   ");
                }

                if (!string.IsNullOrEmpty(income_company))
                {
                    if (isExactly2)
                        qry.Append($"\n     AND importer LIKE '%{income_company}%'                       ");
                    else
                        qry.Append($"\n     {whereSql("importer", income_company)}                       ");
                }
                if (!string.IsNullOrEmpty(manufacture_country))
                    qry.Append($"\n     {whereSql("m_country", manufacture_country)}                 ");
                if (!string.IsNullOrEmpty(export_country))
                    qry.Append($"\n     {whereSql("e_country", export_country)}                      ");
                if (!string.IsNullOrEmpty(product_type))
                    qry.Append($"\n     AND product_type LIKE '%{product_type}%'                      ");
                qry.Append($"\n   GROUP BY division, pname_kor, pname_eng, product_type, importer                       ");
                qry.Append($"\n ) AS i2                                                                   ");
                qry.Append($"\n    ON i1.division = i2.division                                           ");
                qry.Append($"\n    AND i1.pname_kor = i2.pname_kor                                        ");
                qry.Append($"\n    AND i1.pname_eng = i2.pname_eng                                        ");
                qry.Append($"\n    AND i1.importer = i2.importer                                          ");
                qry.Append($"\n GROUP BY    i1.id , i1.division , i1.importer  , im_date , i1.pname_kor  , i1.pname_eng  , i1.m_country  , i1.e_country    ");
                qry.Append($"\n ORDER BY i1.im_date DESC, i1.division, i1.importer, i1.pname_eng, i1.pname_kor             ");

            }
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            command.CommandTimeout = 500000;
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        private string whereSql(string whrColumn, string whrValue, bool isExcept = false, char separator = ' ', bool is_exactly = false) 
        {
            string[] tempStr = null;
            string tempWhr = "";
            string whrStr = "";
            if (!string.IsNullOrEmpty(whrValue.Trim()))
            {
                tempStr = whrValue.Split(separator);
                if (tempStr.Length > 1)
                {
                    for (int i = 0; i < tempStr.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(tempStr[i]))
                        {
                            if (string.IsNullOrEmpty(tempWhr))
                            {
                                if (!isExcept)
                                {
                                    if (is_exactly)
                                        tempWhr = $"\n	   {whrColumn} = '{tempStr[i]}' ";
                                    else
                                        tempWhr = $"\n	   {whrColumn} LIKE '%{tempStr[i]}%' ";
                                }
                                else
                                {
                                    if (is_exactly)
                                        tempWhr = $"\n	   {whrColumn} <> '{tempStr[i]}' ";
                                    else
                                        tempWhr = $"\n	   {whrColumn} NOT LIKE '%{tempStr[i]}%' ";
                                }
                            }
                            else
                            {
                                if (!isExcept)
                                {
                                    if (is_exactly)
                                        tempWhr += $"\n	   OR {whrColumn} = '{tempStr[i]}' ";
                                    else
                                        tempWhr += $"\n	   OR {whrColumn} LIKE '%{tempStr[i]}%' ";

                                }
                                else
                                {
                                    if (is_exactly)
                                        tempWhr += $"\n	   AND {whrColumn} <> '{tempStr[i]}' ";
                                    else
                                        tempWhr += $"\n	   AND {whrColumn} NOT LIKE '%{tempStr[i]}%' ";
                                }
                            }
                        }
                    }
                    whrStr = $"\n	 AND ( {tempWhr} )";
                }
                else
                {
                    if (!isExcept)
                    {
                        if (is_exactly)
                            whrStr = $"\n	  AND {whrColumn} = '{whrValue}'";
                        else
                            whrStr = $"\n	  AND {whrColumn} LIKE '%{whrValue}%'";
                    }
                    else
                    {
                        if (is_exactly)
                            whrStr = $"\n	  AND {whrColumn} <> '{whrValue}'";
                        else
                            whrStr = $"\n	  AND {whrColumn} NOT LIKE '%{whrValue}%'";
                    }
                }
            }
            return whrStr;
        }

        public StringBuilder InsertSql(OverseaManufacturingModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO income (                  ");
            qry.Append($"\n   id                                  ");
            qry.Append($"\n , division                            ");
            qry.Append($"\n , importer                            ");
            qry.Append($"\n , pname_kor                           ");
            qry.Append($"\n , pname_eng                           ");
            qry.Append($"\n , product_type                        ");
            qry.Append($"\n , manufacturing                       ");
            qry.Append($"\n , im_date                             ");
            qry.Append($"\n , until_date                          ");
            qry.Append($"\n , m_country                           ");
            qry.Append($"\n , e_country                           ");
            qry.Append($"\n , frozen_num                          ");
            qry.Append($"\n , lot_num                             ");
            qry.Append($"\n , updatetime                          ");
            qry.Append($"\n , last_edit_user_name                 ");
            qry.Append($"\n , group_id                            ");
            qry.Append($"\n , remark                              ");
            qry.Append($"\n ) VALUES (                            ");
            qry.Append($"\n   {model.id                     }             ");
            qry.Append($"\n , '{model.division               }'             ");
            qry.Append($"\n , '{model.importer               }'             ");
            qry.Append($"\n , '{model.pname_kor              }'             ");
            qry.Append($"\n , '{model.pname_eng              }'             ");
            qry.Append($"\n , '{model.product_type           }'             ");
            qry.Append($"\n , '{model.manufacturing          }'             ");
            qry.Append($"\n , '{model.im_date                }'             ");
            qry.Append($"\n , '{model.until_date             }'             ");
            qry.Append($"\n , '{model.m_country              }'             ");
            qry.Append($"\n , '{model.e_country              }'             ");
            qry.Append($"\n , '{model.frozen_num             }'             ");
            qry.Append($"\n , '{model.lot_num                }'             ");
            qry.Append($"\n , '{model.updatetime             }'             ");
            qry.Append($"\n , '{model.last_edit_user_name    }'             ");
            qry.Append($"\n ,  {model.group_id               }              ");
            qry.Append($"\n , '{model.remark                 }'             ");
            qry.Append($"\n )                                               ");

            string sql = qry.ToString();
            return qry;
        }
    }
}
