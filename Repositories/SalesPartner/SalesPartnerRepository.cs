using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SalesPartner
{
    public class SalesPartnerRepository: ClassRoot, ISalesPartnerRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public DataTable GetCurrentCompanySales()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                 ");
            qry.Append($"\n   company_id                                                                                           ");
            qry.Append($"\n  , sub_id                                                                                              ");
            qry.Append($"\n  , is_sales                                                                                            ");
            qry.Append($"\n  , contents                                                                                            ");
            qry.Append($"\n  , log                                                                                                 ");
            qry.Append($"\n  , remark                                                                                              ");
            qry.Append($"\n  , from_ato_manager                                                                                     ");
            qry.Append($"\n  , to_ato_manager                                                                                     ");
            qry.Append($"\n  , from_category                                                                                     ");
            qry.Append($"\n  , to_category                                                                                     ");
            qry.Append($"\n  , CAST(updatetime AS CHAR) AS updatetime                                                              ");
            qry.Append($"\n  , edit_user                                                                                           ");
            qry.Append($"\n  , rownum                                                                                              ");
            qry.Append($"\n  FROM (                                                                                                ");
            qry.Append($"\n 		SELECT                                                                                         ");
            qry.Append($"\n 		t1.*                                                                                           ");
            qry.Append($"\n 		, (CASE @grp1 WHEN t1.company_id THEN @rownum := @rownum + 1 ELSE @rownum := 1 end) as rownum  ");
            qry.Append($"\n 		, (@grp1:= t1.company_id) AS grp1                                                              ");
            qry.Append($"\n         FROM t_company_sales AS t1, (SELECT @rownum:=0, @grp1:=0) AS r                                 ");
            qry.Append($"\n         ORDER BY company_id, updatetime DESC                                                           ");
            qry.Append($"\n ) AS t1                                                                                                ");
            qry.Append($"\n WHERE rownum = 1                                                                                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetUpdateList(string company_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT * FROM t_company_sales             ");
            qry.Append($"\n WHERE company_id = {company_id}           ");
            qry.Append($"\n ORDER BY updatetime DESC                  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public StringBuilder UpdateSalesInfo(CompanySalesModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_company_sales SET                      ");
            qry.Append($"\n   updatetime = '{model.updatetime}'             ");
            qry.Append($"\n , is_sales = {model.is_sales}                   ");
            //qry.Append($"\n , log = '{model.log}'                           ");
            //qry.Append($"\n , contents = '{model.contents}'                 ");
            qry.Append($"\n , remark = '{model.remark}'                     ");
            qry.Append($"\n , edit_user = '{model.edit_user}'               ");
            qry.Append($"\n WHERE company_id = {model.company_id}           ");
            qry.Append($"\n   AND sub_id = {model.sub_id}                   ");
            qry.Append($"\n   AND edit_user = '{model.edit_user}'           ");

            string sql = qry.ToString();
            return qry;
        }
        public DataTable GetSaleInfo(string sttdate, string enddate, string division, string company, bool isExactly, string edit_user, string company_id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                   ");
            qry.Append($"\n    s.company_id                                                           ");
            qry.Append($"\n  , s.sub_id                                                               ");
            qry.Append($"\n  , c.name AS company                                                      ");
            qry.Append($"\n  , IF(IFNULL(s.is_sales, 0) = 1, 'TRUE', 'FALSE') AS is_sales             ");
            qry.Append($"\n  , s.log                                                                  ");
            qry.Append($"\n  , s.contents                                                             ");
            qry.Append($"\n  , s.remark                                                               ");
            qry.Append($"\n  , s.updatetime                                                           ");
            qry.Append($"\n  , s.edit_user                                                            ");
            qry.Append($"\n  , IF(IFNULL(c.isPotential1, 0) = 1, 'TRUE', 'FALSE') AS isPotential1     ");
            qry.Append($"\n  , IF(IFNULL(c.isPotential2, 0) = 1, 'TRUE', 'FALSE') AS isPotential2     ");
            qry.Append($"\n  , s.from_ato_manager                                                       ");
            qry.Append($"\n  , s.to_ato_manager                                                         ");
            qry.Append($"\n  , s.from_category                                                          ");
            qry.Append($"\n  , s.to_category                                                            ");


            qry.Append($"\n  FROM t_company_sales AS s                                                ");
            qry.Append($"\n  LEFT OUTER JOIN t_company AS c                                           ");
            qry.Append($"\n    ON s.company_id = c.id                                                 ");
            qry.Append($"\n  WHERE 1=1                                                                ");
            if (!string.IsNullOrEmpty(company_id))
                qry.Append($"\n    AND company_id = {company_id}                                                                ");
            if (!string.IsNullOrEmpty(company))
            {
                if (isExactly)
                    qry.Append($"\n   AND c.name LIKE '%{company}%'         ");
                else
                    qry.Append($"\n   {commonRepository.whereSql("c.name", company)}         ");
            }
            if (!string.IsNullOrEmpty(sttdate))
            {
                DateTime sttdt;
                if (DateTime.TryParse(sttdate, out sttdt))
                    qry.Append($"\n   AND DATE_FORMAT(s.updatetime, '%Y-%m-%d') >= '{sttdt.ToString("yyyy-MM-dd")}'          ");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                DateTime enddt;
                if (DateTime.TryParse(enddate, out enddt))
                    qry.Append($"\n   AND DATE_FORMAT(s.updatetime, '%Y-%m-%d') <= '{enddt.ToString("yyyy-MM-dd")}'          ");
            }
            if (!string.IsNullOrEmpty(edit_user))
                qry.Append($"\n   {commonRepository.whereSql("s.edit_user", edit_user)}      ");

            
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesSumary(int sYear, string department, string edit_user)
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n SELECT                                                                                                                                                                ");
            qry.Append($"\n t.*                                                                                                                                                                ");
            qry.Append($"\n FROM (                                                                                                                                                                ");

            qry.Append($"\n     SELECT                                                                                                                                                                ");
            qry.Append($"\n	      edit_user                                                                                                                                                               ");
            qry.Append($"\n	    , DATE_FORMAT(updatetime,'%Y-%m-%d') AS updatetime                                                                                                                        ");
            qry.Append($"\n  	, 0 AS add_cnt                                                                                                                                                        ");
            qry.Append($"\n  	, count(*) AS sales_cnt                                                                                                                                               ");
            qry.Append($"\n     , SUM(IF(TIME(updatetime) < '13:00:00', 1, 0)) AS am_cnt                                                                                                             ");
            qry.Append($"\n     , SUM(IF(TIME(updatetime) >= '13:00:00', 1, 0)) AS pm_cnt                                                                                                            ");
            qry.Append($"\n  	, SUM(IF(to_category = '잠재1', 1, 0)) AS potential1_cnt                                                                                                                ");
            qry.Append($"\n  	, SUM(IF(to_category = '잠재2', 1, 0)) AS potential2_cnt                                                                                                                ");
            qry.Append($"\n     FROM(                                                                                                                                                                 ");
            qry.Append($"\n 		SELECT                                                                                                                                                            ");
            qry.Append($"\n 		  *                                                                                                                                                               ");
            qry.Append($"\n 		, CASE WHEN @grp1 = DATE_FORMAT(updatetime,'%Y-%m-%d') AND @grp2 = company_id AND @grp3 = edit_user THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum   ");
            qry.Append($"\n 		, (@grp1:= DATE_FORMAT(updatetime,'%Y-%m-%d')) AS dum1                                                                                                            ");
            qry.Append($"\n 		, (@grp2:= company_id) AS dum2                                                                                                                                    ");
            qry.Append($"\n        , (@grp2:= edit_user) AS dum3                                                                                                                                      ");
            qry.Append($"\n 		FROM t_company_sales AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='') AS r                                                                                        ");
            qry.Append($"\n 		WHERE 1=1                                                                                                                                                         ");
            qry.Append($"\n 		AND is_sales = true                                                                                                                                               ");

            if (sYear > 0)
            {
                DateTime sttdate = new DateTime(sYear, 1, 1);
                DateTime enddate = new DateTime(sYear, 12, 31);
                qry.Append($"\n 	  AND DATE_FORMAT(updatetime, '%Y-%m-%d') >= '{sttdate.ToString("yyyy-MM-dd")}'                              ");
                qry.Append($"\n 	  AND DATE_FORMAT(updatetime, '%Y-%m-%d') <= '{enddate.ToString("yyyy-MM-dd")}'                              ");
            }
            if (!string.IsNullOrEmpty(edit_user))
                qry.Append($"\n 	  {commonRepository.whereSql("edit_user", edit_user)}                   ");
            

            qry.Append($"\n 		ORDER BY updatetime DESC                                                                                                                                          ");
            qry.Append($"\n     ) AS t1                                                                                                                                                               ");
            qry.Append($"\n     WHERE rownum = 1                                                                                                                                                      ");
            qry.Append($"\n     GROUP BY edit_user, DATE_FORMAT(updatetime,'%Y-%m-%d')                                                                                                                ");
            qry.Append($"\n ) AS t                                                                                                                                 ");
            qry.Append($"\n LEFT OUTER JOIN users AS u                                                                                                             ");
            qry.Append($"\n   ON t.edit_user = u.user_name                                                                                                         ");
            qry.Append($"\n WHERE 1=1                                                                                                                              ");
            if (!string.IsNullOrEmpty(department) && department != "전체")
                qry.Append($"\n  AND u.department = '{department}'                                  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesPartner3(string group_name, string company, bool isExactly, string tel, string registration_number, string ceo, string manager
                                    , bool is_all_potential = true, bool is_potential1 = false, bool is_potential2 = false
                                    , bool is_all_out = false, bool is_non_handled = false, bool is_out_business = false, bool is_not_send_fax = false
                                    , bool isHide = false, string currentSaleDate = "", string distribution = "", string handling_item = "", string remark2 = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                                                                                                                                            ");
            qry.Append($"\nt.*                                                                                                                                               ");
            qry.Append($"\n, s.contents AS sales_cotents                                                                                                                     ");
            qry.Append($"\n, s.log AS sales_log                                                                                                                              ");
            qry.Append($"\n, s.remark AS sales_remark                                                                                                                        ");
            qry.Append($"\n, s.from_ato_manager AS pre_ato_manager                                                                                                           ");
            qry.Append($"\n, '' AS current_sale_date                                                                                                                         ");
            qry.Append($"\n, '' AS current_sale_manager                                                                                                                      ");

            qry.Append($"\n, 0 AS duplicate_common_count                                                                                                                     ");
            qry.Append($"\n, 0 AS duplicate_myData_count                                                                                                                     ");
            qry.Append($"\n, 0 AS duplicate_potential1_count                                                                                                                 ");
            qry.Append($"\n, 0 AS duplicate_potential2_count                                                                                                                 ");
            qry.Append($"\n, 0 AS duplicate_trading_count                                                                                                                    ");
            qry.Append($"\n, 0 AS duplicate_nonHandled_count                                                                                                                 ");
            qry.Append($"\n, 0 AS duplicate_notSendFax_count                                                                                                                 ");
            qry.Append($"\n, 0 AS duplicate_outBusiness_count                                                                                                                ");
            qry.Append($"\n, '' AS duplicate_result                                                                                                                          ");

            qry.Append($"\n, 00 AS industry_type_rank                                                                                                                        ");

            qry.Append($"\n, '' AS category                                                                                                                                  ");
            qry.Append($"\n, 0 AS main_id                                                                                                                                    ");
            qry.Append($"\n, 0 AS sub_id                                                                                                                                     ");


            qry.Append($"\nFROM(                                                                                                                                             ");
            qry.Append($"\n  SELECT                                                                                                                                          ");
            qry.Append($"\n     c.id                                                                                                                                         ");
            qry.Append($"\n   , c.division                                                                                                                                   ");
            qry.Append($"\n   , c.registration_number                                                                                                                        ");
            qry.Append($"\n   , c.group_name                                                                                                                                 ");
            qry.Append($"\n   , c.name                                                                                                                                       ");
            qry.Append($"\n   , c.origin                                                                                                                                     ");
            qry.Append($"\n   , c.address                                                                                                                                    ");
            qry.Append($"\n   , c.ceo                                                                                                                                        ");
            qry.Append($"\n   , c.fax                                                                                                                                        ");
            qry.Append($"\n   , c.company_manager                                                                                                                            ");
            qry.Append($"\n   , c.tel                                                                                                                                        ");
            qry.Append($"\n   , c.company_manager_position                                                                                                                   ");
            qry.Append($"\n   , c.email                                                                                                                                      ");
            qry.Append($"\n   , c.remark                                                                                                                                     ");
            qry.Append($"\n   , c.sns1                                                                                                                                       ");
            qry.Append($"\n   , c.sns2                                                                                                                                       ");
            qry.Append($"\n   , c.sns3                                                                                                                                       ");
            qry.Append($"\n   , c.web                                                                                                                                        ");
            qry.Append($"\n   , c.ato_manager                                                                                                                                ");
            qry.Append($"\n   , CAST(c.createtime AS CHAR) AS createtime                                                                                                     ");
            qry.Append($"\n   , CAST(c.updatetime AS CHAR) AS updatetime                                                                                                     ");
            qry.Append($"\n   , c.phone                                                                                                                                      ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement1, 0) = 1, 'TRUE', 'FALSE') AS isManagement1                                                                         ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement2, 0) = 1, 'TRUE', 'FALSE') AS isManagement2                                                                         ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement3, 0) = 1, 'TRUE', 'FALSE') AS isManagement3                                                                         ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement4, 0) = 1, 'TRUE', 'FALSE') AS isManagement4                                                                         ");
            qry.Append($"\n   , IF(IFNULL(c.isHide, 0) = 1, 'TRUE', 'FALSE') AS isHide                                                                                       ");
            qry.Append($"\n   , IF(IFNULL(c.isPotential1, 0) = 1, 'TRUE', 'FALSE') AS isPotential1                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isPotential2, 0) = 1, 'TRUE', 'FALSE') AS isPotential2                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isTrading, 0) = 1, 'TRUE', 'FALSE') AS isTrading                                                                                 ");
            qry.Append($"\n   , IF(IFNULL(c.isNonHandled, 0) = 0, 'FALSE', 'TRUE') AS isNonHandled                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isOutBusiness, 0) = 0, 'FALSE', 'TRUE') AS isOutBusiness                                                                         ");
            qry.Append($"\n   , IF(IFNULL(c.isNotSendFax, 0) = 0, 'FALSE', 'TRUE') AS isNotSendFax                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isDelete, 0) = 0, 'FALSE', 'TRUE') AS isDelete                                                                                   ");
            qry.Append($"\n   , IFNULL(c.seaover_company_code, '') AS seaover_company_code                                                                                   ");
            qry.Append($"\n   , c.distribution                                                                                                                               ");
            qry.Append($"\n   , c.handling_item                                                                                                                              ");
            qry.Append($"\n   , c.payment_date                                                                                                                               ");
            qry.Append($"\n   , c.remark2                                                                                                                                    ");
            qry.Append($"\n   , c.remark4                                                                                                                                    ");
            qry.Append($"\n   , '' AS is_sales                                                                                                                               ");
            qry.Append($"\n   , '' AS sales_contents                                                                                                                         ");
            qry.Append($"\n   , '' AS sales_log                                                                                                                              ");
            qry.Append($"\n   , '' AS sales_remark                                                                                                                           ");
            qry.Append($"\n   , CAST(c.updatetime AS char) AS sales_updatetime                                                                                               ");
            qry.Append($"\n   , '' AS sales_edit_user                                                                                                                        ");
            qry.Append($"\n   , sales_comment AS sales_comment                                                                                                               ");
            qry.Append($"\n   , CAST(IFNULL(alarm_month, 0) AS char) AS alarm_month                                                                                          ");
            qry.Append($"\n   , CAST(IFNULL(alarm_week, '') AS char) AS alarm_week                                                                                           ");
            qry.Append($"\n   , IFNULL(alarm_complete_date, '') AS alarm_complete_date                                                                                       ");
            qry.Append($"\n   , a.alarms AS alarm_date                                                                                                                       ");
            qry.Append($"\n   , c.other_phone                                                                                                                                ");
            qry.Append($"\n   , c.industry_type                                                                                                                              ");
            qry.Append($"\n  FROM (                                                                                                                                          ");
            qry.Append($"\n  	SELECT                                                                                                                                       ");
            qry.Append($"\n        *                                                                                                                                         ");
            qry.Append($"\n      FROM t_company                                                                                                                              ");
            qry.Append($"\n      WHERE division = '매출처'                                                                                                                     ");
            qry.Append($"\n  ) AS c                                                                                                                                          ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                                               ");
            qry.Append($"\n  	SELECT                                                                                                                                       ");
            qry.Append($"\n       company_id                                                                                                                                 ");
            qry.Append($"\n     , group_concat(alarm_date,'_',category,'_',division,'_',IF(IFNULL(alarm_complete, 0)=0,'FALSE','TRUE')) AS alarms                            ");
            qry.Append($"\n     FROM t_company_alarm                                                                                                                         ");
            qry.Append($"\n     GROUP BY company_id                                                                                                                          ");
            qry.Append($"\n  ) AS a                                                                                                                                          ");
            qry.Append($"\n    ON c.id = a.company_id                                                                                                                        ");
            qry.Append($"\n  WHERE 1=1                                                                                                                                       ");
            //잠재1, 2
            if (!is_all_potential)
            {
                qry.Append($"\n   AND IFNULL(isPotential1, 0) = {is_potential1}                                                                                               ");
                qry.Append($"\n   AND IFNULL(isPotential2, 0) = {is_potential2}                                                                                               ");
            }

            //그룹명
            if (!string.IsNullOrEmpty(group_name))
                qry.Append($"\n       {commonRepository.whereSql("group_name", group_name)}                                                                                                  ");
            //상호
            if (!string.IsNullOrEmpty(company))
            {
                if (isExactly)
                    qry.Append($"\n       AND name LIKE '%{company}%'                                                                                                  ");
                else
                    qry.Append($"\n       {commonRepository.whereSql("name", company)}                                                                                                  ");
            }
            //전화번호
            if (!string.IsNullOrEmpty(tel))
                qry.Append($"\n       AND (REPLACE(REPLACE(tel, '-', ''), ' ', '') LIKE '%{tel}%' OR REPLACE(REPLACE(fax, '-', ''), ' ', '') LIKE '%{tel}%' OR REPLACE(REPLACE(phone, '-', ''), ' ', '') LIKE '%{tel}%')                                                             ");
            //사업자번호
            if (!string.IsNullOrEmpty(registration_number))
                qry.Append($"\n       {commonRepository.whereSql("registration_number", registration_number)}                                                                                                  ");
            //대표자명
            if (!string.IsNullOrEmpty(ceo))
                qry.Append($"\n       {commonRepository.whereSql("ceo", ceo)}                                                                                                  ");
            qry.Append($"\n) AS t                                                                                                                                            ");
            qry.Append($"\nLEFT OUTER JOIN(                                                                                                                                  ");
            qry.Append($"\n	 SELECT                                                                                                                                          ");
            qry.Append($"\n	   company_id                                                                                                                                    ");
            qry.Append($"\n	  , sub_id                                                                                                                                       ");
            qry.Append($"\n	  , is_sales                                                                                                                                     ");
            qry.Append($"\n	  , contents                                                                                                                                     ");
            qry.Append($"\n	  , log                                                                                                                                          ");
            qry.Append($"\n	  , remark                                                                                                                                       ");
            qry.Append($"\n	  , from_ato_manager                                                                                                                             ");
            qry.Append($"\n	  , to_ato_manager                                                                                                                               ");
            qry.Append($"\n	  , from_category                                                                                                                                ");
            qry.Append($"\n	  , to_category                                                                                                                                  ");
            qry.Append($"\n	  , CAST(updatetime AS CHAR) AS updatetime                                                                                                       ");
            qry.Append($"\n	  , edit_user                                                                                                                                    ");
            qry.Append($"\n	  , rownum                                                                                                                                       ");
            qry.Append($"\n	  FROM (                                                                                                                                         ");
            qry.Append($"\n			SELECT                                                                                                                                   ");
            qry.Append($"\n			t1.*                                                                                                                                     ");
            qry.Append($"\n			, (CASE @grp1 WHEN t1.company_id THEN @rownum := @rownum + 1 ELSE @rownum := 1 end) as rownum                                            ");
            qry.Append($"\n			, (@grp1:= t1.company_id) AS grp1                                                                                                        ");
            qry.Append($"\n			 FROM t_company_sales AS t1, (SELECT @rownum:=0, @grp1:=0) AS r                                                                          ");
            qry.Append($"\n			 ORDER BY company_id, updatetime DESC                                                                                                    ");
            qry.Append($"\n	 ) AS t1                                                                                                                                         ");
            qry.Append($"\n	 WHERE rownum = 1                                                                                                                                ");
            qry.Append($"\n) AS s                                                                                                                                            ");
            qry.Append($"\n  ON t.id = s.company_id                                                                                                                          ");
            qry.Append($"\nORDER BY t.updatetime, t.name                                                                                                                     ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            command.CommandTimeout = 300;
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesPartner2(string group_name, string company, bool isExactly, string tel, string registration_number, string ceo, string manager
                                    , bool is_all_potential = true, bool is_potential1 = false, bool is_potential2 = false
                                    , bool is_all_out = false, bool is_non_handled = false, bool is_out_business = false, bool is_not_send_fax = false
                                    , bool isHide = false, string currentSaleDate = "", string distribution = "", string handling_item = "", string remark2 = "")
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n  SELECT                                                                                                                                            ");
            qry.Append($"\n     c.id                                                                                                                                           ");
            qry.Append($"\n   , c.division                                                                                                                                     ");
            qry.Append($"\n   , c.registration_number                                                                                                                          ");
            qry.Append($"\n   , c.group_name                                                                                                                                   ");
            qry.Append($"\n   , c.name                                                                                                                                         ");
            qry.Append($"\n   , c.origin                                                                                                                                       ");
            qry.Append($"\n   , c.address                                                                                                                                      ");
            qry.Append($"\n   , c.ceo                                                                                                                                          ");
            qry.Append($"\n   , c.fax                                                                                                                                          ");
            qry.Append($"\n   , c.company_manager                                                                                                                              ");
            qry.Append($"\n   , c.tel                                                                                                                                          ");
            qry.Append($"\n   , c.company_manager_position                                                                                                                     ");
            qry.Append($"\n   , c.email                                                                                                                                        ");
            qry.Append($"\n   , c.remark                                                                                                                                       ");
            qry.Append($"\n   , c.sns1                                                                                                                                         ");
            qry.Append($"\n   , c.sns2                                                                                                                                         ");
            qry.Append($"\n   , c.sns3                                                                                                                                         ");
            qry.Append($"\n   , c.web                                                                                                                                          ");
            qry.Append($"\n   , c.ato_manager                                                                                                                                  ");
            qry.Append($"\n   , CAST(c.createtime AS CHAR) AS createtime                                                                                                       ");
            qry.Append($"\n   , CAST(c.updatetime AS CHAR) AS updatetime                                                                                                       ");
            qry.Append($"\n   , c.phone                                                                                                                                        ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement1, 0) = 1, 'TRUE', 'FALSE') AS isManagement1                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement2, 0) = 1, 'TRUE', 'FALSE') AS isManagement2                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement3, 0) = 1, 'TRUE', 'FALSE') AS isManagement3                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement4, 0) = 1, 'TRUE', 'FALSE') AS isManagement4                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isHide, 0) = 1, 'TRUE', 'FALSE') AS isHide                                                                                         ");
            qry.Append($"\n   , IF(IFNULL(c.isPotential1, 0) = 1, 'TRUE', 'FALSE') AS isPotential1                                                                             ");
            qry.Append($"\n   , IF(IFNULL(c.isPotential2, 0) = 1, 'TRUE', 'FALSE') AS isPotential2                                                                             ");
            qry.Append($"\n   , IF(IFNULL(c.isTrading, 0) = 1, 'TRUE', 'FALSE') AS isTrading                                                                                   ");
            qry.Append($"\n   , IF(IFNULL(c.isNonHandled, 0) = 0, 'FALSE', 'TRUE') AS isNonHandled                                                                             ");
            qry.Append($"\n   , IF(IFNULL(c.isOutBusiness, 0) = 0, 'FALSE', 'TRUE') AS isOutBusiness                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isNotSendFax, 0) = 0, 'FALSE', 'TRUE') AS isNotSendFax                                                                             ");
            qry.Append($"\n   , IF(IFNULL(c.isDelete, 0) = 0, 'FALSE', 'TRUE') AS isDelete                                                                                     ");
            qry.Append($"\n   , IFNULL(c.seaover_company_code, '') AS seaover_company_code                                                                                     ");
            qry.Append($"\n   , c.distribution                                                                                                                                 ");
            qry.Append($"\n   , c.handling_item                                                                                                                                ");
            qry.Append($"\n   , c.payment_date                                                                                                                                 ");
            qry.Append($"\n   , c.remark2                                                                                                                                      ");
            qry.Append($"\n   , c.remark4                                                                                                                                      ");
            qry.Append($"\n   , c.remark5                                                                                                                                      ");
            qry.Append($"\n   , c.remark6                                                                                                                                      ");
            qry.Append($"\n   , c.address                                                                                                                                      ");
            qry.Append($"\n   , '' AS is_sales                                                                                                                                 ");
            qry.Append($"\n   , '' AS sales_contents                                                                                                                           ");
            qry.Append($"\n   , '' AS sales_log                                                                                                                                ");
            qry.Append($"\n   , '' AS sales_remark                                                                                                                             ");
            qry.Append($"\n   , CAST(c.updatetime AS char) AS sales_updatetime                                                                                                 ");
            qry.Append($"\n   , '' AS sales_edit_user                                                                                                                          ");
            qry.Append($"\n   , sales_comment AS sales_comment                                                                                                                 ");
            qry.Append($"\n   , CAST(IFNULL(alarm_month, 0) AS char) AS alarm_month                                                                                            ");
            qry.Append($"\n   , CAST(IFNULL(alarm_week, '') AS char) AS alarm_week                                                                                              ");
            qry.Append($"\n   , IFNULL(alarm_complete_date, '') AS alarm_complete_date                                                                                                           ");
            qry.Append($"\n   , a.alarms AS alarm_date                                                                                                                                      ");
            qry.Append($"\n   , c.other_phone                                                                                                                                      ");
            qry.Append($"\n   , c.industry_type                                                                                                                                ");
            qry.Append($"\n   , c.industry_type2                                                                                                                               ");
            qry.Append($"\n  FROM (                                                                                                                                            ");
            qry.Append($"\n  	SELECT                                                                                                                                         ");
            qry.Append($"\n        *                                                                                                                                           ");
            qry.Append($"\n      FROM t_company                                                                                                                                ");
            qry.Append($"\n      WHERE division = '매출처'                                                                                                                        ");
            //qry.Append($"\n       AND IFNULL(isHide, 0) = {isHide}  ");

            //거래금지 OR 숨김거래처
            /*if (!is_all_out)
            {
                qry.Append($"\n       AND IFNULL(isNonHandled, 0) = {is_non_handled}  ");
                qry.Append($"\n       AND IFNULL(isOutBusiness, 0) = {is_out_business}  ");
                qry.Append($"\n       AND IFNULL(isNotSendFax, 0) = {is_not_send_fax}  ");
            }
            else
            {
                qry.Append($"\n       AND (IFNULL(isNonHandled, 0) = TRUE  ");
                qry.Append($"\n       OR IFNULL(isOutBusiness, 0) = TRUE  ");
                qry.Append($"\n       OR IFNULL(isNotSendFax, 0) = TRUE)  ");
            }*/


            //잠재1, 2
            if (!is_all_potential)
            {
                qry.Append($"\n   AND IFNULL(isPotential1, 0) = {is_potential1}                                                                                               ");
                qry.Append($"\n   AND IFNULL(isPotential2, 0) = {is_potential2}                                                                                               ");
            }

            //그룹명
            if (!string.IsNullOrEmpty(group_name))
                qry.Append($"\n       {commonRepository.whereSql("group_name", group_name)}                                                                                                  ");
            //상호
            if (!string.IsNullOrEmpty(company))
            {
                if (isExactly)
                    qry.Append($"\n       AND name LIKE '%{company}%'                                                                                                  ");
                else
                    qry.Append($"\n       {commonRepository.whereSql("name", company)}                                                                                                  ");
            }
            //전화번호
            if (!string.IsNullOrEmpty(tel))
                qry.Append($"\n       AND (REPLACE(REPLACE(tel, '-', ''), ' ', '') LIKE '%{tel}%' OR REPLACE(REPLACE(fax, '-', ''), ' ', '') LIKE '%{tel}%' OR REPLACE(REPLACE(phone, '-', ''), ' ', '') LIKE '%{tel}%')                                                             ");
            //사업자번호
            if (!string.IsNullOrEmpty(registration_number))
                qry.Append($"\n       {commonRepository.whereSql("registration_number", registration_number)}                                                                                                  ");
            //대표자명
            if (!string.IsNullOrEmpty(ceo))
                qry.Append($"\n       {commonRepository.whereSql("ceo", ceo)}                                                                                                  ");
            //담당자
            /*if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n       AND users = '{manager.Trim()}'                                                                                                 ");*/

            qry.Append($"\n  ) AS c                                                                                                                                            ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                                                                                                 ");
            qry.Append($"\n  	SELECT                                                                                                                                         ");
            qry.Append($"\n       company_id                                                                                                                                   ");
            qry.Append($"\n     , group_concat(alarm_date,'_',category,'_',division,'_',IF(IFNULL(alarm_complete, 0)=0,'FALSE','TRUE')) AS alarms                                           ");
            qry.Append($"\n     FROM t_company_alarm                                                                                                                           ");
            qry.Append($"\n     GROUP BY company_id                                                                                                                            ");
            qry.Append($"\n  ) AS a                                                                                                                                            ");
            qry.Append($"\n    ON c.id = a.company_id                                                                                                                          ");
            qry.Append($"\n  WHERE 1=1                                                                                                                                         ");
            qry.Append($"\n  ORDER BY c.updatetime, c.name                                                                                                                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            command.CommandTimeout = 300;
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetSalesPartner(string group_name, string company, bool isExactly, string tel, string registration_number, string ceo, string manager
                                    , bool is_all_potential = true, bool is_potential1 = false, bool is_potential2 = false
                                    , bool is_all_out = false, bool is_non_handled = false, bool is_out_business = false, bool is_not_send_fax = false
                                    , bool isHide = false, string currentSaleDate = "", string distribution = "", string handling_item = "", string remark2 = "")
        {
            StringBuilder qry = new StringBuilder();

            qry.Append($"\n  SELECT                                                                                                                                            ");
            qry.Append($"\n     c.id                                                                                                                                           ");
            qry.Append($"\n   , c.division                                                                                                                                     ");
            qry.Append($"\n   , c.registration_number                                                                                                                          ");
            qry.Append($"\n   , c.group_name                                                                                                                                   ");
            qry.Append($"\n   , c.name                                                                                                                                         ");
            qry.Append($"\n   , c.origin                                                                                                                                       ");
            qry.Append($"\n   , c.address                                                                                                                                      ");
            qry.Append($"\n   , c.ceo                                                                                                                                          ");
            qry.Append($"\n   , c.fax                                                                                                                                          ");
            qry.Append($"\n   , c.company_manager                                                                                                                              ");
            qry.Append($"\n   , c.tel                                                                                                                                          ");
            qry.Append($"\n   , c.company_manager_position                                                                                                                     ");
            qry.Append($"\n   , c.email                                                                                                                                        ");
            qry.Append($"\n   , c.remark                                                                                                                                       ");
            qry.Append($"\n   , c.sns1                                                                                                                                         ");
            qry.Append($"\n   , c.sns2                                                                                                                                         ");
            qry.Append($"\n   , c.sns3                                                                                                                                         ");
            qry.Append($"\n   , c.web                                                                                                                                          ");
            qry.Append($"\n   , c.ato_manager                                                                                                                                  ");
            qry.Append($"\n   , CAST(c.createtime AS CHAR) AS createtime                                                                                                       ");
            qry.Append($"\n   , CAST(c.updatetime AS CHAR) AS updatetime                                                                                                       ");
            qry.Append($"\n   , c.phone                                                                                                                                        ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement1, 0) = 1, 'TRUE', 'FALSE') AS isManagement1                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement2, 0) = 1, 'TRUE', 'FALSE') AS isManagement2                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement3, 0) = 1, 'TRUE', 'FALSE') AS isManagement3                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isManagement4, 0) = 1, 'TRUE', 'FALSE') AS isManagement4                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isHide, 0) = 1, 'TRUE', 'FALSE') AS isHide                                                                                         ");
            qry.Append($"\n   , IF(IFNULL(c.isPotential1, 0) = 1, 'TRUE', 'FALSE') AS isPotential1                                                                             ");
            qry.Append($"\n   , IF(IFNULL(c.isPotential2, 0) = 1, 'TRUE', 'FALSE') AS isPotential2                                                                             ");
            qry.Append($"\n   , IF(IFNULL(c.isTrading, 0) = 1, 'TRUE', 'FALSE') AS isTrading                                                                                   ");
            qry.Append($"\n   , IF(IFNULL(c.isNonHandled, 0) = 0, 'FALSE', 'TRUE') AS isNonHandled                                                                             ");
            qry.Append($"\n   , IF(IFNULL(c.isOutBusiness, 0) = 0, 'FALSE', 'TRUE') AS isOutBusiness                                                                           ");
            qry.Append($"\n   , IF(IFNULL(c.isNotSendFax, 0) = 0, 'FALSE', 'TRUE') AS isNotSendFax                                                                             ");
            qry.Append($"\n   , IF(IFNULL(c.isDelete, 0) = 0, 'FALSE', 'TRUE') AS isDelete                                                                                     ");
            qry.Append($"\n   , IFNULL(c.seaover_company_code, '') AS seaover_company_code                                                                                     ");
            qry.Append($"\n   , c.distribution                                                                                                                                 ");
            qry.Append($"\n   , c.handling_item                                                                                                                                ");
            qry.Append($"\n   , c.remark2                                                                                                                                ");
            qry.Append($"\n   , c.address                                                                                                                                      ");
            qry.Append($"\n   , '' AS is_sales                                                                                                                         ");
            qry.Append($"\n   , '' AS sales_contents                                                                                                                   ");
            qry.Append($"\n   , '' AS sales_log                                                                                                                   ");
            qry.Append($"\n   , '' AS sales_remark                                                                                                                       ");
            /*qry.Append($"\n   , s.is_sales AS is_sales                                                                                                                         ");
            qry.Append($"\n   , s.contents AS sales_contents                                                                                                                   ");
            qry.Append($"\n   , s.log AS sales_log                                                                                                                   ");
            qry.Append($"\n   , s.remark AS sales_remark                                                                                                                       ");*/
            qry.Append($"\n   , CAST(c.updatetime AS char) AS sales_updatetime                                                                                                 ");
            qry.Append($"\n   , '' AS sales_edit_user                                                                                                                 ");
            qry.Append($"\n  FROM (                                                                                                                                            ");
            qry.Append($"\n  	SELECT                                                                                                                                         ");
            qry.Append($"\n        *                                                                                                                                           ");
            qry.Append($"\n      FROM t_company                                                                                                                                ");
            qry.Append($"\n      WHERE division = '매출처'                                                                                                                        ");
            qry.Append($"\n       AND IFNULL(isHide, 0) = {isHide}  ");

            //거래금지 OR 숨김거래처
            if (!is_all_out)
            {
                qry.Append($"\n       AND IFNULL(isNonHandled, 0) = {is_non_handled}  ");
                qry.Append($"\n       AND IFNULL(isOutBusiness, 0) = {is_out_business}  ");
                //qry.Append($"\n       AND IFNULL(isNotSendFax, 0) = {is_not_send_fax}  ");
            }
            else
            {
                qry.Append($"\n       AND (IFNULL(isNonHandled, 0) = TRUE  ");
                qry.Append($"\n       OR IFNULL(isOutBusiness, 0) = TRUE  ");
                //qry.Append($"\n       OR IFNULL(isNotSendFax, 0) = TRUE)  ");
            }


            //잠재1, 2
            if (!is_all_potential)
            {
                qry.Append($"\n   AND IFNULL(isPotential1, 0) = {is_potential1}                                                                                               ");
                qry.Append($"\n   AND IFNULL(isPotential2, 0) = {is_potential2}                                                                                               ");
            }

            //그룹명
            if (!string.IsNullOrEmpty(group_name))
                qry.Append($"\n       {commonRepository.whereSql("group_name", group_name)}                                                                                                  ");
            //상호
            if (!string.IsNullOrEmpty(company))
            {
                if (isExactly)
                    qry.Append($"\n       AND name LIKE '%{company}%'                                                                                                  ");
                else
                    qry.Append($"\n       {commonRepository.whereSql("name", company)}                                                                                                  ");
            }
            //전화번호
            if (!string.IsNullOrEmpty(tel))
                qry.Append($"\n       AND (REPLACE(REPLACE(tel, '-', ''), ' ', '') LIKE '%{tel}%' OR REPLACE(REPLACE(fax, '-', ''), ' ', '') LIKE '%{tel}%' OR REPLACE(REPLACE(phone, '-', ''), ' ', '') LIKE '%{tel}%')                                                             ");
            //사업자번호
            if (!string.IsNullOrEmpty(registration_number))
                qry.Append($"\n       {commonRepository.whereSql("registration_number", registration_number)}                                                                                                  ");
            //대표자명
            if (!string.IsNullOrEmpty(ceo))
                qry.Append($"\n       {commonRepository.whereSql("ceo", ceo)}                                                                                                  ");
            //담당자
            /*if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n       AND users = '{manager.Trim()}'                                                                                                 ");*/
            
            qry.Append($"\n  ) AS c                                                                                                                                            ");
            /*qry.Append($"\n  LEFT OUTER JOIN (                                                                                                                                 ");
            qry.Append($"\n  	SELECT                                                                                                                                         ");
            qry.Append($"\n       *                                                                                                                                            ");
            qry.Append($"\n      FROM t_company_sales AS a WHERE updatetime = (SELECT MAX(updatetime) FROM t_company_sales AS b WHERE b.company_id = a.company_id)             ");
            qry.Append($"\n  ) AS s                                                                                                                                            ");
            qry.Append($"\n    ON c.id = s.company_id                                                                                                                          ");*/
            qry.Append($"\n  WHERE 1=1                                                                                                                                         ");
            qry.Append($"\n  ORDER BY c.updatetime, c.name                                                                                                                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            command.CommandTimeout = 1000000;
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public StringBuilder InsertPartnerSales(CompanySalesModel cm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_company_sales (        ");
            qry.Append($"\n   company_id                         ");
            qry.Append($"\n , sub_id                             ");
            qry.Append($"\n , is_sales                           ");
            qry.Append($"\n , contents                           ");
            qry.Append($"\n , remark                             ");
            qry.Append($"\n , updatetime                         ");
            qry.Append($"\n , edit_user                          ");
            qry.Append($"\n , log                                ");
            qry.Append($"\n , pre_ato_manager                    ");
            qry.Append($"\n , from_ato_manager                   ");
            qry.Append($"\n , from_category                      ");
            qry.Append($"\n , to_ato_manager                   ");
            qry.Append($"\n , to_category                      ");

            qry.Append($"\n ) VALUES (                           ");
            qry.Append($"\n    {cm.company_id}                   ");
            qry.Append($"\n ,  {cm.sub_id}                       ");
            qry.Append($"\n ,  {cm.is_sales}                     ");
            qry.Append($"\n , '{cm.contents}'                    ");
            qry.Append($"\n , '{cm.remark}'                      ");
            qry.Append($"\n , '{cm.updatetime}'                  ");
            qry.Append($"\n , '{cm.edit_user}'                   ");
            qry.Append($"\n , '{cm.log}'                         ");
            qry.Append($"\n , '{cm.pre_ato_manager}'             ");

            qry.Append($"\n , '{cm.from_ato_manager}'             ");
            qry.Append($"\n , '{cm.from_category}'             ");
            qry.Append($"\n , '{cm.to_ato_manager}'             ");
            qry.Append($"\n , '{cm.to_category}'             ");
            qry.Append($"\n )                                    ");

            string sql = qry.ToString();

            return qry;
        }
        public StringBuilder DeletePartnerSales(int company_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_company_sales WHERE company_id = {company_id}        ");
            

            string sql = qry.ToString();

            return qry;
        }

        public DataTable GetUserSaleDashboard(string sttdate, string enddate, string user_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                                                                                                           ");
            qry.Append($"\n    updatetime                                                                                                                                                     ");
            qry.Append($"\n  , SUM(add_cnt) AS add_cnt                                                                                                                                        ");
            qry.Append($"\n  , SUM(sales_cnt) AS sales_cnt                                                                                                                                    ");
            qry.Append($"\n  , SUM(am_cnt) AS am_cnt                                                                                                                                          ");
            qry.Append($"\n  , SUM(pm_cnt) AS pm_cnt                                                                                                                                          ");
            qry.Append($"\n  , SUM(potential1_cnt) AS potential1_cnt                                                                                                                          ");
            qry.Append($"\n  , SUM(potential2_cnt) AS potential2_cnt                                                                                                                          ");
            qry.Append($"\n  FROM(                                                                                                                                                            ");
            qry.Append($"\n     SELECT                                                                                                                                                        ");
            qry.Append($"\n  	  DATE_FORMAT(updatetime,'%Y-%m-%d') AS updatetime                                                                                                            ");
            qry.Append($"\n  	, SUM(IF(to_category = '내DATA', 1, 0)) AS add_cnt                                                                                                             ");
            qry.Append($"\n  	, 0 AS sales_cnt                                                                                                                                              ");
            qry.Append($"\n      , 0 AS am_cnt                                                                                                                                                ");
            qry.Append($"\n      , 0 AS pm_cnt                                                                                                                                                ");
            qry.Append($"\n  	, 0 AS potential1_cnt                                                                                                                                         ");
            qry.Append($"\n  	, 0 AS potential2_cnt                                                                                                                                         ");
            qry.Append($"\n     FROM(                                                                                                                                                         ");
            qry.Append($"\n 		SELECT                                                                                                                                                    ");
            qry.Append($"\n 		  *                                                                                                                                                       ");
            qry.Append($"\n 		, CASE WHEN @grp1 = DATE_FORMAT(updatetime,'%Y-%m-%d') AND @grp2 = company_id THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum                 ");
            qry.Append($"\n 		, (@grp1:= DATE_FORMAT(updatetime,'%Y-%m-%d')) AS dum1                                                                                                    ");
            qry.Append($"\n 		, (@grp2:= company_id) AS dum2                                                                                                                            ");
            qry.Append($"\n 		FROM t_company_sales AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='') AS r                                                                                ");
            qry.Append($"\n 		WHERE 1=1                                                                                                                                                 ");
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"\n   {commonRepository.whereSql("edit_user", user_name)}      ");
            if (!string.IsNullOrEmpty(sttdate))
            {
                DateTime sttdt;
                if (DateTime.TryParse(sttdate, out sttdt))
                    qry.Append($"\n   AND DATE_FORMAT(updatetime, '%Y-%m-%d') >= '{sttdt.ToString("yyyy-MM-dd")}'          ");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                DateTime enddt;
                if (DateTime.TryParse(enddate, out enddt))
                    qry.Append($"\n   AND DATE_FORMAT(updatetime, '%Y-%m-%d') <= '{enddt.ToString("yyyy-MM-dd")}'          ");
            }
            qry.Append($"\n 		ORDER BY updatetime DESC                                                                                                                                  ");
            qry.Append($"\n     ) AS t1                                                                                                                                                       ");
            qry.Append($"\n     WHERE rownum = 1                                                                                                                                              ");
            qry.Append($"\n     GROUP BY DATE_FORMAT(updatetime,'%Y-%m-%d')                                                                                                                   ");
            qry.Append($"\n     UNION ALL                                                                                                                                                     ");
            qry.Append($"\n     SELECT                                                                                                                                                        ");
            qry.Append($"\n  	  DATE_FORMAT(updatetime,'%Y-%m-%d') AS updatetime                                                                                                            ");
            qry.Append($"\n  	, 0 AS add_cnt                                                                                                                                                ");
            qry.Append($"\n  	, count(*) AS sales_cnt                                                                                                                                       ");
            qry.Append($"\n      , SUM(IF(TIME(updatetime) < '13:00:00', 1, 0)) AS am_cnt                                                                                                     ");
            qry.Append($"\n      , SUM(IF(TIME(updatetime) >= '13:00:00', 1, 0)) AS pm_cnt                                                                                                    ");
            qry.Append($"\n  	, SUM(IF(to_category = '잠재1', 1, 0)) AS potential1_cnt                                                                                                        ");
            qry.Append($"\n  	, SUM(IF(to_category = '잠재2', 1, 0)) AS potential2_cnt                                                                                                        ");
            qry.Append($"\n     FROM(                                                                                                                                                         ");
            qry.Append($"\n 		SELECT                                                                                                                                                    ");
            qry.Append($"\n 		  *                                                                                                                                                       ");
            qry.Append($"\n 		, CASE WHEN @grp1 = DATE_FORMAT(updatetime,'%Y-%m-%d') AND @grp2 = company_id THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum                 ");
            qry.Append($"\n 		, (@grp1:= DATE_FORMAT(updatetime,'%Y-%m-%d')) AS dum1                                                                                                    ");
            qry.Append($"\n 		, (@grp2:= company_id) AS dum2                                                                                                                            ");
            qry.Append($"\n 		FROM t_company_sales AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='') AS r                                                                                ");
            qry.Append($"\n 		WHERE 1=1                                                                                                                                                 ");
            qry.Append($"\n 		  AND is_sales = true                                                                                                                                     ");
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"\n   {commonRepository.whereSql("edit_user", user_name)}      ");
            if (!string.IsNullOrEmpty(sttdate))
            {
                DateTime sttdt;
                if (DateTime.TryParse(sttdate, out sttdt))
                    qry.Append($"\n   AND DATE_FORMAT(updatetime, '%Y-%m-%d') >= '{sttdt.ToString("yyyy-MM-dd")}'          ");
            }
            if (!string.IsNullOrEmpty(enddate))
            {
                DateTime enddt;
                if (DateTime.TryParse(enddate, out enddt))
                    qry.Append($"\n   AND DATE_FORMAT(updatetime, '%Y-%m-%d') <= '{enddt.ToString("yyyy-MM-dd")}'          ");
            }
            qry.Append($"\n 		ORDER BY updatetime DESC                                                                                                                                  ");
            qry.Append($"\n     ) AS t1                                                                                                                                                       ");
            qry.Append($"\n     WHERE rownum = 1                                                                                                                                              ");
            qry.Append($"\n     GROUP BY DATE_FORMAT(updatetime,'%Y-%m-%d')                                                                                                                   ");
            qry.Append($"\n ) AS t1                                                                                                                                                           ");
            qry.Append($"\n GROUP BY updatetime                                                                                                                                               ");
            qry.Append($"\n ORDER BY updatetime DESC                                                                                                                                          ");

            
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

