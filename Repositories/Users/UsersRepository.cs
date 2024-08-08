using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class UsersRepository : ClassRoot, IUsersRepository
    {
        CommonRepository commonRepository = new CommonRepository();

        public DataTable GetOneData(string col, string department, string team, string grade, string user_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT distinct                                 ");
            qry.Append($"\n   {col}                                         ");
            qry.Append($"\n FROM users                                      ");
            qry.Append($"\n WHERE 1=1                                       ");
            if(!string.IsNullOrEmpty(department))
                qry.Append($"\n   {commonRepository.whereSql("department", department)}                                        ");
            if (!string.IsNullOrEmpty(team))
                qry.Append($"\n   {commonRepository.whereSql("team", team)}                                        ");
            if (!string.IsNullOrEmpty(grade))
                qry.Append($"\n   {commonRepository.whereSql("grade", grade)}                                        ");
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"\n   {commonRepository.whereSql("user_name", user_name)}                                        ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;

        }
        public int GetDailyTarget(string user_name)
        { 
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT IFNULL(daily_work_goals_amount, 0) FROM users WHERE user_name = '{user_name}' AND user_status = '승인'                 ");
            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            int result = Convert.ToInt32(command.ExecuteScalar());
            return result;
        }

        public DataTable GetTragetData(int data_type, string department, string user_name)
        {
            StringBuilder qry = new StringBuilder();
            if (data_type == 1)
            {
                qry.Append($"\n SELECT                                              ");
                qry.Append($"\n   d.name AS department                              ");
                qry.Append($"\n , '' AS user_id                                     ");
                qry.Append($"\n , '' AS user_name                                   ");
                qry.Append($"\n , IF(IFNULL(a.is_registration, false) = 0, 'FALSE', 'TRUE') AS  is_registration                                ");
                qry.Append($"\n FROM t_department AS d                              ");
                qry.Append($"\n LEFT OUTER JOIN (                                   ");
                qry.Append($"\n	SELECT                                              ");
                qry.Append($"\n      distinct                                       ");
                qry.Append($"\n      user_id                                        ");
                qry.Append($"\n   	, TRUE AS is_registration                         ");
                qry.Append($"\n    FROM t_Authority                                 ");
                qry.Append($"\n    WHERE IFNULL(is_individual, false) = false       ");
                qry.Append($"\n ) AS a                                              ");
                qry.Append($"\n   ON d.name = a.user_id                             ");
                qry.Append($"\n WHERE 1=1                                           ");
                if (!string.IsNullOrEmpty(department.Trim()))
                    qry.Append($"\n   AND d.name LIKE '%{department.Trim()}%'                                           ");
                qry.Append($"\n ORDER BY d.name                                       ");
            }
            else
            {
                qry.Append($"\n SELECT                                           ");
                qry.Append($"\n   d.department AS department                     ");
                qry.Append($"\n , d.user_id AS user_id                           ");
                qry.Append($"\n , d.user_name AS user_name                       ");
                qry.Append($"\n , IF(IFNULL(a.is_registration, false) = 0, 'FALSE', 'TRUE') AS  is_registration                                ");
                qry.Append($"\n FROM users AS d                                  ");
                qry.Append($"\n LEFT OUTER JOIN (                                ");
                qry.Append($"\n	SELECT                                           ");
                qry.Append($"\n      distinct                                    ");
                qry.Append($"\n      user_id                                     ");
                qry.Append($"\n	, TRUE AS is_registration                      ");
                qry.Append($"\n    FROM t_Authority                              ");
                qry.Append($"\n    WHERE IFNULL(is_individual, false) = true     ");
                qry.Append($"\n ) AS a                                           ");
                qry.Append($"\n   ON d.user_id = a.user_id                       ");
                qry.Append($"\n WHERE 1=1                                        ");
                qry.Append($"\n AND d.user_status = '승인'               ");
                
                if (!string.IsNullOrEmpty(department.Trim()))
                    qry.Append($"\n   AND d.department LIKE '%{department.Trim()}%'                                           ");
                if (!string.IsNullOrEmpty(user_name.Trim()))
                    qry.Append($"\n   AND d.user_name LIKE '%{user_name.Trim()}%'                                           ");
                qry.Append($"\n ORDER BY department, user_name                   ");
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


        public int UpdateTargetSalesAmount(string user_id, double sales_amount)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE users SET target_sales_amount = '{sales_amount}' WHERE user_id = '{user_id}'");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }
        public int UpdateCurrentDate(string user_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE users SET current_login_date = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE user_id = '{user_id}'");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public DataTable GetUsers(int year, string workplace, string department, string name, string sortType, string user_id = "", bool isRetire = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                              ");
            qry.Append($"\n   u.user_id                                         ");
            qry.Append($"\n , u.user_name                                       ");
            qry.Append($"\n , u.user_status                                     ");
            qry.Append($"\n , u.user_in_date                                    ");
            qry.Append($"\n , u.user_out_date                                   ");
            qry.Append($"\n , IFNULL(v.vacation, 0) AS vacation                 ");
            qry.Append($"\n FROM users AS u                                     ");
            qry.Append($"\n LEFT OUTER JOIN (                                   ");
            qry.Append($"\n 	SELECT                                          ");
            qry.Append($"\n     *                                               ");
            qry.Append($"\n     FROM t_accrued_vacation                         ");
            qry.Append($"\n     WHERE year = {year}                             ");
            qry.Append($"\n ) AS v                                              ");
            qry.Append($"\n   ON u.user_id = v.user_id                          ");
            qry.Append($"\n WHERE 1=1                                           ");

            if(!isRetire)
                qry.Append($"\n   AND user_status = '승인'                      ");
            else
                qry.Append($"\n   AND (user_status = '승인' OR user_status = '퇴사')    ");

            qry.Append($"\n   AND YEAR(user_in_date) <= {year}                                                                                        ");
            if (!string.IsNullOrEmpty(workplace) && workplace != "전체")
                qry.Append($"\n   AND u.workplace LIKE '%{workplace}%'                                                                                        ");
            if (!string.IsNullOrEmpty(department) && department != "전체")
                qry.Append($"\n   AND u.department LIKE '%{department}%'                                                                                        ");
            if (!string.IsNullOrEmpty(name))
                qry.Append($"\n   AND u.user_name LIKE '%{name}%'                                                                                        ");
            if (!string.IsNullOrEmpty(user_id))
                qry.Append($"\n   AND u.user_id ='{user_id}'                                                                                        ");

            if (sortType == "입사일")
                qry.Append($"\n ORDER BY u.user_in_date                                                                                              ");
            else if (sortType == "이름")
                qry.Append($"\n ORDER BY u.user_name                                                                                                 ");
            else if (sortType == "부서+이름")
                qry.Append($"\n ORDER BY u.department, u.user_name                                                                                                 ");



            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetUsersVacation(int year, string user_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                           ");
            qry.Append($"\n   u.*                                                                            ");
            qry.Append($"\n , IFNULL(v.used_days, 0) AS used_days                                            ");
            qry.Append($"\n FROM(                                                                            ");
            qry.Append($"\n 	SELECT                                                                       ");
            qry.Append($"\n 	  u.user_id                                                                  ");
            qry.Append($"\n 	, IFNULL(u.user_in_date, '1900-01-01') AS user_in_date                       ");
            qry.Append($"\n 	, IFNULL(v.vacation, 0) AS vacation                                          ");
            qry.Append($"\n 	FROM users AS u                                                              ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                                            ");
            qry.Append($"\n 		SELECT                                                                   ");
            qry.Append($"\n 		*                                                                        ");
            qry.Append($"\n 		FROM t_accrued_vacation                                                  ");
            qry.Append($"\n 		WHERE year = {year}                                                        ");
            qry.Append($"\n 	) AS v                                                                       ");
            qry.Append($"\n 	  ON u.user_id = v.user_id                                                   ");
            qry.Append($"\n 	WHERE 1=1                                                                    ");
            qry.Append($"\n 	  AND u.user_id = '{user_id}'                                               ");
            qry.Append($"\n ) AS u                                                                           ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                ");
            qry.Append($"\n 	SELECT                                                                       ");
            qry.Append($"\n       user_id                                                                    ");
            qry.Append($"\n 	, SUM(used_days) AS used_days                                                ");
            qry.Append($"\n     FROM t_used_vacation                                                         ");
            qry.Append($"\n     WHERE 1=1                                                                    ");
            qry.Append($"\n       AND user_id = '{user_id}'                                                 ");
            qry.Append($"\n       AND sttdate >= '{year}-01-01 00:00:00' AND sttdate <= '{year}-12-31 23:59:59'  ");
            qry.Append($"\n     GROUP BY user_id                                                             ");
            qry.Append($"\n ) AS v                                                                           ");
            qry.Append($"\n   ON u.user_id = v.user_id                                                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetUsers(string workplace, string department, string team, string name, string status, string grade)
        {
            string sttdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            string enddate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                                                           ");
            qry.Append($"\n   u.*                                                                                                                                                            ");
            qry.Append($"\n , IFNULL(s.daily_sales_cnt, 0) AS daily_sales_cnt                                                                                                                ");
            qry.Append($"\n , IFNULL(s.monthly_sales_cnt, 0) AS monthly_sales_cnt                                                                                                            ");
            qry.Append($"\n FROM(                                                                                                                                                            ");
            qry.Append($"\n 	SELECT                                                                                                                                                       ");
            qry.Append($"\n 	  u.user_id                                                                                                                                                  ");
            qry.Append($"\n 	, u.user_name                                                                                                                                                ");
            qry.Append($"\n 	, u.user_in_date                                                                                                                                             ");
            qry.Append($"\n 	, u.user_out_date                                                                                                                                            ");
            qry.Append($"\n 	, u.workplace                                                                                                                                                ");
            qry.Append($"\n 	, u.department                                                                                                                                               ");
            qry.Append($"\n 	, u.team                                                                                                                                               ");
            qry.Append($"\n 	, u.user_status                                                                                                                                              ");
            qry.Append($"\n 	, u.seaover_id                                                                                                                                               ");
            qry.Append($"\n 	, u.tel                                                                                                                                                      ");
            qry.Append($"\n 	, u.grade                                                                                                                                                    ");
            qry.Append($"\n 	, u.auth_level                                                                                                                                               ");
            qry.Append($"\n 	, u.daily_work_goals_amount                                                                                                                                  ");
            qry.Append($"\n 	, IFNULL(v.used_days, 0) AS used_days                                                                                                                        ");
            qry.Append($"\n 	FROM(                                                                                                                                                        ");
            qry.Append($"\n 		SELECT                                                                                                                                                   ");
            qry.Append($"\n 		   u.user_id                                                                                                                                             ");
            qry.Append($"\n 		 , u.user_name                                                                                                                                           ");
            qry.Append($"\n 		 , u.user_in_date                                                                                                                                        ");
            qry.Append($"\n 		 , u.user_out_date                                                                                                                                       ");
            qry.Append($"\n 		 , u.workplace                                                                                                                                           ");
            qry.Append($"\n 		 , u.department                                                                                                                                          ");
            qry.Append($"\n 		 , u.team                                                                                                                                            ");
            qry.Append($"\n 		 , u.user_status                                                                                                                                         ");
            qry.Append($"\n 		 , u.seaover_id                                                                                                                                          ");
            qry.Append($"\n 		 , u.tel                                                                                                                                                 ");
            qry.Append($"\n 		 , u.grade                                                                                                                                               ");
            qry.Append($"\n 		 , d.auth_level                                                                                                                                          ");
            qry.Append($"\n 		 , IFNULL(u.daily_work_goals_amount, 0 ) AS daily_work_goals_amount                                                                                      ");
            qry.Append($"\n 		 FROM  users AS u                                                                                                                                        ");
            qry.Append($"\n 		 LEFT OUTER JOIN t_department AS d                                                                                                                       ");
            qry.Append($"\n 		   ON u.department = d.name                                                                                                                              ");
            qry.Append($"\n 		 WHERE 1=1                                                                                                                                               ");
            if (!string.IsNullOrEmpty(workplace) && workplace != "전체")
                qry.Append($"\n   AND u.workplace LIKE '%{workplace}%'                                                                                  ");
            if (!string.IsNullOrEmpty(department) && department != "전체")
                qry.Append($"\n   {commonRepository.whereSql("u.department", department)}                                                                                       ");
            //qry.Append($"\n   AND u.department LIKE '%{department}%'                                                                                ");
            if (!string.IsNullOrEmpty(team) && team != "전체")
                qry.Append($"\n   {commonRepository.whereSql("u.team", team)}                                                                                       ");
            //qry.Append($"\n   AND u.team LIKE '%{team}%'                                                                                ");
            if (!string.IsNullOrEmpty(name))
                qry.Append($"\n   {commonRepository.whereSql("u.user_name", name)}                                                                                       ");
            //qry.Append($"\n   AND u.user_name LIKE '%{name}%'                                                                                       ");
            if (status != "전체" && !string.IsNullOrEmpty(status))
            {
                if (status == "승인")
                    qry.Append($"\n   AND (u.user_status = '승인')                                                                                      ");
                else
                    qry.Append($"\n   AND u.user_status = '{status}'                                                                                    ");
            }
            if (!string.IsNullOrEmpty(grade))
                qry.Append($"\n   {commonRepository.whereSql("u.grade", grade)}                                                                                       ");
            //qry.Append($"\n   AND u.grade = '{grade}'                                                                                               ");
            qry.Append($"\n 	) AS u                                                                                                                                                       ");
            qry.Append($"\n 	LEFT OUTER JOIN (                                                                                                                                            ");
            qry.Append($"\n 		SELECT                                                                                                                                                   ");
            qry.Append($"\n 		  user_id                                                                                                                                                ");
            qry.Append($"\n 		, user_name                                                                                                                                              ");
            qry.Append($"\n 		,  SUM(used_days) AS used_days                                                                                                                           ");
            qry.Append($"\n 		FROM atotrading.t_used_vacation                                                                                                                          ");
            qry.Append($"\n	        WHERE date_format(sttdate, '%Y-%m-%d') >= '{sttdate}'                         ");
            qry.Append($"\n	          AND date_format(sttdate, '%Y-%m-%d') <= '{enddate}'                         ");
            qry.Append($"\n 		GROUP BY user_id, user_name                                                                                                                              ");
            qry.Append($"\n 	) AS v                                                                                                                                                       ");
            qry.Append($"\n 	  ON u.user_id = v.user_id                                                                                                                                   ");
            qry.Append($"\n ) AS u                                                                                                                                                           ");
            qry.Append($"\n LEFT OUTER JOIN (                                                                                                                                                ");
            qry.Append($"\n 	SELECT                                                                                                                                                       ");
            qry.Append($"\n 	  edit_user                                                                                                                                                  ");
            qry.Append($"\n 	, SUM(daily_sales_cnt) AS daily_sales_cnt                                                                                                                    ");
            qry.Append($"\n 	, SUM(monthly_sales_cnt) AS monthly_sales_cnt                                                                                                                ");
            qry.Append($"\n 	FROM(                                                                                                                                                        ");
            qry.Append($"\n 		SELECT                                                                                                                                                   ");
            qry.Append($"\n 		  edit_user                                                                                                                                              ");
            qry.Append($"\n 		, COUNT(*) AS daily_sales_cnt                                                                                                                            ");
            qry.Append($"\n 		, 0 AS monthly_sales_cnt                                                                                                                                 ");
            qry.Append($"\n 		FROM(                                                                                                                                                    ");
            qry.Append($"\n 			SELECT                                                                                                                                               ");
            qry.Append($"\n 			  *                                                                                                                                                  ");
            qry.Append($"\n 			, CASE WHEN @grp1 = DATE_FORMAT(updatetime,'%Y-%m-%d') AND @grp2 = company_id THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum            ");
            qry.Append($"\n 			, (@grp1:= DATE_FORMAT(updatetime,'%Y-%m-%d')) AS dum1                                                                                               ");
            qry.Append($"\n 			, (@grp2:= company_id) AS dum2                                                                                                                       ");
            qry.Append($"\n 			FROM t_company_sales AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='') AS r                                                                           ");
            qry.Append($"\n 			WHERE 1=1                                                                                                                                            ");
            qry.Append($"\n 			  AND is_sales = true                                                                                                                                ");
            if (!string.IsNullOrEmpty(name))
                qry.Append($"\n 			  AND edit_user LIKE '%{name}%'                                                                                                                          ");
            qry.Append($"\n 			  AND DATE_FORMAT(updatetime, '%Y-%m-%d') = '{DateTime.Now.ToString("yyyy-MM-dd")}'                                                                                            ");
            qry.Append($"\n 			ORDER BY updatetime DESC                                                                                                                             ");
            qry.Append($"\n 		) AS t1                                                                                                                                                  ");
            qry.Append($"\n 		WHERE rownum = 1                                                                                                                                         ");
            qry.Append($"\n 		GROUP BY edit_user, DATE_FORMAT(updatetime,'%Y-%m-%d')                                                                                                   ");
            qry.Append($"\n 		UNION ALL                                                                                                                                                ");
            qry.Append($"\n 		SELECT                                                                                                                                                   ");
            qry.Append($"\n 		   edit_user                                                                                                                                             ");
            qry.Append($"\n 		 , 0 AS daily_sales_cnt                                                                                                                                  ");
            qry.Append($"\n 		 , COUNT(*) AS monthly_sales_cnt                                                                                                                         ");
            qry.Append($"\n 		 FROM(                                                                                                                                                   ");
            qry.Append($"\n 			SELECT                                                                                                                                               ");
            qry.Append($"\n 			  *                                                                                                                                                  ");
            qry.Append($"\n 			, CASE WHEN @grp1 = DATE_FORMAT(updatetime,'%Y-%m-%d') AND @grp2 = company_id THEN @rownum := @rownum + 1 ELSE @rownum := 1 end as rownum            ");
            qry.Append($"\n 			, (@grp1:= DATE_FORMAT(updatetime,'%Y-%m-%d')) AS dum1                                                                                               ");
            qry.Append($"\n 			, (@grp2:= company_id) AS dum2                                                                                                                       ");
            qry.Append($"\n 			FROM t_company_sales AS t1, (SELECT @rownum:=0, @grp1:='', @grp2:='') AS r                                                                           ");
            qry.Append($"\n 			WHERE 1=1                                                                                                                                            ");
            qry.Append($"\n 			  AND is_sales = true                                                                                                                                ");
            if (!string.IsNullOrEmpty(name))
                qry.Append($"\n 			  AND edit_user LIKE '%{name}%'                                                                                                                          ");
            qry.Append($"\n 			  AND DATE_FORMAT(updatetime, '%Y-%m-%d') >= '{sttdate}'                                                                                            ");
            qry.Append($"\n 			  AND DATE_FORMAT(updatetime, '%Y-%m-%d') <= '{enddate}'                                                                                            ");
            qry.Append($"\n 			ORDER BY updatetime DESC                                                                                                                             ");
            qry.Append($"\n 		) AS t1                                                                                                                                                  ");
            qry.Append($"\n 		WHERE rownum = 1                                                                                                                                         ");
            qry.Append($"\n 		GROUP BY edit_user                                                                                                                                       ");
            qry.Append($"\n 	) AS s                                                                                                                                                       ");
            qry.Append($"\n 	GROUP BY edit_user                                                                                                                                           ");
            qry.Append($"\n ) AS s                                                                                                                                                           ");
            qry.Append($"\n   ON u.user_name = s.edit_user                                                                                                                                   ");
            qry.Append($"\n ORDER BY u.department, u.user_name, u.grade                                                                                                                      ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetTeamMember(string auth_level)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                          ");
            qry.Append($"\n  user_name                      ");
            qry.Append($"\n FROM users                      ");
            qry.Append($"\n WHERE user_status = '승인'      ");
            if (!string.IsNullOrEmpty(auth_level))
            { 
                qry.Append($"\n AND auth_level = {auth_level} ");
            }
            qry.Append($"\n GROUP BY user_name              ");

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public UsersModel GetByUser(string username, string password, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                            ");
            qry.Append($"\n  u.user_id                        ");
            qry.Append($"\n, u.user_password                  ");
            qry.Append($"\n, u.user_status                    ");
            qry.Append($"\n, u.user_name                      ");
            qry.Append($"\n, u.workplace                      ");
            qry.Append($"\n, u.department                     ");
            qry.Append($"\n, d.auth_level                     ");
            qry.Append($"\n, u.unlimited_expiry_date          ");
            qry.Append($"\n, u.limit_type                     ");
            qry.Append($"\n, u.limit_max_count                ");
            qry.Append($"\n, u.tel                            ");
            qry.Append($"\n, u.grade                          ");
            qry.Append($"\n, u.form_remark                    ");
            qry.Append($"\n, u.seaover_id                     ");
            qry.Append($"\n, IFNULL(u.target_sales_amount, 0) AS target_sales_amount                     ");
            qry.Append($"\n, u.user_in_date                     ");
            qry.Append($"\n, u.excel_password                   ");
            qry.Append($"\n, IFNULL(u.daily_work_goals_amount, 0) AS daily_work_goals_amount                     ");
            qry.Append($"\n FROM users AS u                   ");
            qry.Append($"\n LEFT OUTER JOIN t_department AS d ");
            qry.Append($"\n   ON u.department = d.name        ");
            qry.Append($"\n WHERE u.user_id = '{username}' AND u.user_password = '{password}' ");
            qry.Append($"\n AND u.user_status = '승인' ");

            string sql = qry.ToString();

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetUsersModel(dr);
        }
        public UsersModel GetUserInfo(string user_id, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                            ");
            qry.Append($"  u.user_id                        ");
            qry.Append($", u.user_password                  ");
            qry.Append($", u.user_status                    ");
            qry.Append($", u.user_name                      ");
            qry.Append($", u.workplace                      ");
            qry.Append($", u.department                     ");
            qry.Append($", d.auth_level                     ");
            qry.Append($", u.unlimited_expiry_date          ");
            qry.Append($", u.limit_type                     ");
            qry.Append($", u.limit_max_count                ");
            qry.Append($", u.tel                            ");
            qry.Append($", u.grade                          ");
            qry.Append($", u.form_remark                    ");
            qry.Append($", u.seaover_id                     ");
            qry.Append($", IFNULL(u.target_sales_amount, 0) AS target_sales_amount                     ");
            qry.Append($", u.user_in_date                     ");
            qry.Append($", u.excel_password                   ");
            qry.Append($", IFNULL(u.daily_work_goals_amount, 0) AS daily_work_goals_amount                   ");
            qry.Append($" FROM users AS u                   ");
            qry.Append($" LEFT OUTER JOIN t_department AS d ");
            qry.Append($"   ON u.department = d.name        ");
            qry.Append($" WHERE u.user_id = '{user_id}'");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetUsersModel(dr);
        }
        public UsersModel GetUserInfo2(string username, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT * ");
            qry.Append($" FROM users ");
            qry.Append($" WHERE user_name = '{username}'");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetUsersModel(dr);
        }

        public List<UsersModel> GetUsers(IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT * ");
            qry.Append($" FROM users ");
            qry.Append($" WHERE user_status <> '삭제' ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetUsersList(dr);
        }
        public List<UsersModel> GetUsersList(string department, string user_name, string grade, string tel, IDbTransaction transaction = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                      ");
            qry.Append($"   user_id                   ");
            qry.Append($" , department                ");
            qry.Append($" , user_name                 ");
            qry.Append($" , user_status               ");
            qry.Append($" , auth_level                ");
            qry.Append($" , IFNULL(IF(grade = '', '담당자', grade), '담당자') AS grade      ");
            qry.Append($" , limit_type                ");
            qry.Append($" , limit_max_count           ");
            qry.Append($" , seaover_id                ");
            qry.Append($" , tel                       ");
            qry.Append($" FROM users                  ");
            qry.Append($" WHERE  user_status = '승인' ");
            if (!string.IsNullOrEmpty(department))
                qry.Append($"   AND department LIKE '%{department}%' ");
            if (!string.IsNullOrEmpty(user_name))
                qry.Append($"   AND user_name LIKE '%{user_name}%' ");
            if (!string.IsNullOrEmpty(grade))
                qry.Append($"   AND grade LIKE '%{grade}%' ");
            if (!string.IsNullOrEmpty(tel))
                qry.Append($"   AND tel LIKE '%{tel}%' ");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return GetUsersList(dr);
        }

        private List<UsersModel> GetUsersList(MySqlDataReader rd)
        {
            List<UsersModel> list = new List<UsersModel>();

            while (rd.Read())
            {
                UsersModel model = new UsersModel();
                model.user_id = rd["user_id"].ToString();
                model.user_name = rd["user_name"].ToString();
                model.department = rd["department"].ToString();
                model.user_status = rd["user_status"].ToString();
                if (!string.IsNullOrEmpty(rd["auth_level"].ToString()))
                {
                    model.auth_level = Convert.ToInt16(rd["auth_level"].ToString());
                }
                else
                {
                    model.auth_level = 0;
                }
                
                model.grade = rd["grade"].ToString(); ;
                model.limit_type = Convert.ToInt16(rd["limit_type"].ToString());
                model.limit_max_count = Convert.ToInt16(rd["limit_max_count"].ToString());
                model.seaover_id = rd["seaover_id"].ToString();
                model.tel = rd["tel"].ToString();

                list.Add(model);
            }
            rd.Close();
            return list;
        }

        public int InsertUser(UsersModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO users ");
            qry.Append($" (");
            qry.Append($"   user_id");
            qry.Append($",  user_password");
            qry.Append($",  user_status");
            qry.Append($",  user_name");
            qry.Append($",  workplace");
            qry.Append($",  department");
            qry.Append($",  auth_level");
            qry.Append($",  unlimited_expiry_date");
            qry.Append($",  limit_type");
            qry.Append($",  limit_max_count");
            qry.Append($",  tel");
            qry.Append($",  grade");
            qry.Append($",  seaover_id");
            qry.Append($",  form_remark");
            qry.Append($",  user_in_date");
            qry.Append($",  excel_password");
            qry.Append($"  ) VALUES ( ");
            qry.Append($"   '{model.user_id}'");
            qry.Append($",  '{model.user_password}'");
            qry.Append($",  '대기'");
            qry.Append($",  '{model.user_name}'");
            qry.Append($",  '{model.workplace}'");
            qry.Append($",  '{model.department}'");
            qry.Append($",   {model.auth_level}");
            qry.Append($",   null ");
            qry.Append($",   1");
            qry.Append($",   3");
            qry.Append($",  '{model.tel}'");
            qry.Append($",  '{model.grade}'");
            qry.Append($",  '{model.seaover_id}'");
            qry.Append($",  '{model.form_remark}'");
            qry.Append($",  '{model.user_in_date}'");
            qry.Append($",  '{model.excel_password}'");
            qry.Append($" ) ");


            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return Convert.ToInt16(command.ExecuteScalar());
        }

        private UsersModel GetUsersModel(MySqlDataReader rd)
        {
            UsersModel model = new UsersModel();
            while (rd.Read())
            {
                model.user_id = rd["user_id"].ToString();
                model.user_name = rd["user_name"].ToString();
                model.user_status = rd["user_status"].ToString();
                model.workplace = rd["workplace"].ToString();
                model.department = rd["department"].ToString();
                if (rd["auth_level"].ToString() == "")
                    model.auth_level = 0;
                else
                    model.auth_level = Convert.ToInt16(rd["auth_level"].ToString());
                model.limit_type = Convert.ToInt16(rd["limit_type"].ToString());
                model.limit_max_count = Convert.ToInt16(rd["limit_max_count"].ToString());
                model.tel = rd["tel"].ToString();
                model.grade = rd["grade"].ToString();
                model.form_remark = rd["form_remark"].ToString();
                model.seaover_id = rd["seaover_id"].ToString();
                double target_sales_amount;
                if (!double.TryParse(rd["target_sales_amount"].ToString(), out target_sales_amount))
                    target_sales_amount = 0;
                model.target_sales_amount = target_sales_amount;

                if(DateTime.TryParse(rd["user_in_date"].ToString(), out DateTime user_in_date))
                    model.user_in_date = user_in_date.ToString("yyyy-MM-dd");
                model.excel_password = rd["excel_password"].ToString();

                if (int.TryParse(rd["daily_work_goals_amount"].ToString(), out int daily_work_goals_amount))
                    model.daily_work_goals_amount = daily_work_goals_amount;
            }
            rd.Close();

            if (string.IsNullOrEmpty(model.user_id))
            {
                return null;
            }
            else
            {
                return model;
            }
        }

        public int UpdateUser(UsersModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE users SET ");
            qry.Append($"   user_name = '{model.user_name}'");
            qry.Append($",  tel = '{model.tel}'");
            qry.Append($",  user_status = '{model.user_status}'");
            qry.Append($" WHERE user_id = '{model.user_id}'");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return Convert.ToInt16(command.ExecuteScalar());
        }

        public int UpdateMyInfo(UsersModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE users SET                 ");
            qry.Append($"   user_name = '{model.user_name}'");
            qry.Append($",  workplace = '{model.workplace}'        ");
            qry.Append($",  department = '{model.department}'        ");
            qry.Append($",  grade = '{model.grade}'        ");
            qry.Append($",  tel = '{model.tel}'            ");
            qry.Append($",  user_status = '{model.user_status}'");
            qry.Append($",  seaover_id = '{model.seaover_id}'");
            qry.Append($",  auth_level = {model.auth_level}");
            qry.Append($",  user_in_date = '{model.user_in_date}'");
            qry.Append($",  excel_password = '{model.excel_password}'");
            qry.Append($" WHERE user_id = '{model.user_id}'");

            string sql = qry.ToString();

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return Convert.ToInt16(command.ExecuteScalar());
        }

        public int UpdateMyInfo2(UsersModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE users SET                            ");
            qry.Append($"   user_name = '{model.user_name}'           ");
            qry.Append($",  workplace = '{model.workplace}'           ");
            qry.Append($",  department = '{model.department}'         ");
            qry.Append($",  team = '{model.team}'                     ");
            qry.Append($",  grade = '{model.grade}'                   ");
            qry.Append($",  tel = '{model.tel}'                       ");
            qry.Append($",  user_status = '{model.user_status}'       ");
            qry.Append($",  seaover_id = '{model.seaover_id}'         ");
            qry.Append($",  auth_level = {model.auth_level}           ");
            qry.Append($",  user_in_date = '{model.user_in_date}'     ");
            qry.Append($",  user_out_date = '{model.user_out_date}'   ");
            qry.Append($",  excel_password = '{model.excel_password}' ");
            qry.Append($" WHERE user_id = '{model.user_id}'           ");

            string sql = qry.ToString();

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return Convert.ToInt16(command.ExecuteScalar());
        }

        public int DeleteUser(UsersModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM users ");
            qry.Append($" WHERE user_id = '{model.user_id}'");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return Convert.ToInt16(command.ExecuteScalar());
        }

        public int UpdateRemark(string user_id, string remark)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE users SET ");
            qry.Append($"   form_remark = '{remark}'");
            qry.Append($" WHERE user_id = '{user_id}'");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return Convert.ToInt16(command.ExecuteScalar());
        }
    }
}

