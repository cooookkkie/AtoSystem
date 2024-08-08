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
    public class CompanyRepository : ClassRoot, ICompanyRepository
    {
        ICommonRepository commonRepository = new CommonRepository();

        public DataTable GetSeaoverInfo(string company, string seaover_company_code)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                   ");
            qry.Append($"\n   c.*                                                                                                    ");
            qry.Append($"\n , case when @grp = seaover_company_code then @rownum:=@rownum + 1 else @rownum :=1 end as rownum         ");
            qry.Append($"\n , (@grp := seaover_company_code) as dum                                                                  ");
            qry.Append($"\n FROM t_company AS c, (select @rownum:=0, @grp:='') AS r                                                  ");
            qry.Append($"\n WHERE division = '매출처'                                                                                  ");
            qry.Append($"\n   AND IFNULL(seaover_company_code, '') <> ''                                                             ");
            if(!string.IsNullOrEmpty(company))
                qry.Append($"\n   {commonRepository.whereSql("c.name", company)}                                                            ");
            if (!string.IsNullOrEmpty(seaover_company_code))
                qry.Append($"\n   {commonRepository.whereSql("c.seaover_company_code", seaover_company_code)}                                                            ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public StringBuilder UpdateInsurance(string company_id, string r_num, string c_num, string ceo, string kc_number, double insurance_amount, string insurance_sttdate, string insurance_enddate)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_company SET                                                       ");
            qry.Append($"\n   insurance_amount = {insurance_amount}                                    ");
            qry.Append($"\n , insurance_sttdate = '{insurance_sttdate}'         ");
            qry.Append($"\n , insurance_enddate = '{insurance_enddate}'         ");

            qry.Append($"\n , kc_number = '{kc_number}'         ");
            qry.Append($"\n , registration_number = '{r_num}'         ");
            qry.Append($"\n , corporation_number = '{c_num}'         ");
            qry.Append($"\n , ceo = '{ceo}'         ");

            qry.Append($"\n WHERE id = {company_id}                                                    ");

            return qry;
        }
        public DataTable GetInsuranceCompany(string company, bool isExactly, string corporation_number, string ceo, int top_limit_count, bool isOnlyInsurance)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                                                                         ");
            qry.Append($"\n id                                                                                                                             ");
            qry.Append($"\n , name AS company                                                                                                              ");
            qry.Append($"\n , ceo                                                                                                                          ");
            qry.Append($"\n , registration_number                                                                                                          ");
            qry.Append($"\n , corporation_number                                                                                                           ");
            qry.Append($"\n , kc_number                                                                                                                    ");
            qry.Append($"\n , insurance_amount                                                                                                             ");
            qry.Append($"\n , insurance_sttdate                                                                                                            ");
            qry.Append($"\n , insurance_enddate                                                                                                            ");
            qry.Append($"\n , IF(if(insurance_enddate <= STR_TO_DATE(NOW(), '%Y-%m-%d'), true, false) = true, 'TRUE', 'FALSE') AS insurance_status         ");
            qry.Append($"\n FROM t_company                                                                                                                 ");
            qry.Append($"\n WHERE division = '매출처'                                                                                                        ");
            qry.Append($"\n AND name <> '' AND name NOT LIKE '%소송%' AND name NOT LIKE '%(s)%' AND name NOT LIKE '%(S)%'                                  ");
            qry.Append($"\n AND isOutBusiness = false                                                                                                      ");
            qry.Append($"\n AND isDelete = false                                                                                                           ");
            if (!string.IsNullOrEmpty(company))
            {
                if(isExactly)
                    qry.Append($"\n AND name LIKE '%{company}%'                                                                              ");
                else
                    qry.Append($"\n {commonRepository.whereSql("name", company)}                                                                              ");
            }
            if (!string.IsNullOrEmpty(corporation_number))
            {
                qry.Append($"\n AND (registration_number LIKE '%{corporation_number}%' OR corporation_number LIKE '%{corporation_number}%')                                                                                                           ");
            }
            if (!string.IsNullOrEmpty(ceo))
                qry.Append($"\n {commonRepository.whereSql("name", ceo)}                                                                              ");
            if(isOnlyInsurance)
                qry.Append($"\n AND  insurance_enddate <= '{DateTime.Now.AddMonths(2).ToString("yyyy-MM-dd")}'                                                                                                        ");
            qry.Append($"\n ORDER BY insurance_enddate DESC, name                                       ");
            if (top_limit_count > 0)
                qry.Append($"\n LIMIT {top_limit_count}                                                                                                                             ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public StringBuilder UpdateCompanyColumns(string[] col, string[] val, string whr)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE t_company SET                                                              ");

            if (col.Length > 0 && val.Length > 0)
            {
                string update_col = "";
                for (int i = 0; i < col.Length; i++)
                { 
                    if(string.IsNullOrEmpty(update_col.Trim()))
                        update_col = "\n" + col[i] + " = " + val[i];
                    else
                        update_col += "\n, " + col[i] + " = " + val[i];
                }
                qry.Append(update_col);
            }

            qry.Append($"\n WHERE 1=1 AND {whr}                                                                  ");

            string sql = qry.ToString();

            return qry;
        }

        public StringBuilder UpdateAtoManger(int id, string ato_manager, string edit_user)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_company SET                                                              ");
            qry.Append($"   ato_manager = '{ato_manager}'                                                   ");
            qry.Append($" , updatetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'                   ");
            qry.Append($" , edit_user = '{edit_user}'                                                       ");
            qry.Append($" , isPotential1 = false                                                            ");
            qry.Append($" , isPotential2 = false                                                            ");
            qry.Append($" , isTrading = false                                                               ");
            qry.Append($" WHERE id = {id}                                                                   ");

            string sql = qry.ToString();

            return qry;
        }
        public DataTable GetCompanyAsOne(string name, string code = "", string id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                                                         ");
            qry.Append($"\n    id                                                                                                        ");
            qry.Append($"\n  , division                                                                                                  ");
            qry.Append($"\n  , registration_number AS registration_number                                                                ");
            qry.Append($"\n  , group_name                                                                                                ");
            qry.Append($"\n  , name AS  name                                                                                             ");
            qry.Append($"\n  , origin AS origin                                                                                          ");
            qry.Append($"\n  , address AS address                                                                                        ");
            qry.Append($"\n  , ceo AS ceo                                                                                                ");
            qry.Append($"\n  , tel AS tel                                                                                                ");
            qry.Append($"\n  , fax AS fax                                                                                                ");
            qry.Append($"\n  , phone AS phone                                                                                                ");
            qry.Append($"\n  , company_manager AS company_manager                                                                        ");
            qry.Append($"\n  , company_manager_position AS company_manager_position                                                      ");
            qry.Append($"\n  , email AS email                                                                                            ");
            qry.Append($"\n  , remark AS remark                                                                                          ");
            qry.Append($"\n  , sns1 AS sns1                                                                                              ");
            qry.Append($"\n  , sns2 AS sns2                                                                                              ");
            qry.Append($"\n  , sns3 AS sns3                                                                                              ");
            qry.Append($"\n  , web AS web                                                                                                ");
            qry.Append($"\n  , ato_manager AS ato_manager                                                                                            ");
            qry.Append($"\n  , seaover_company_code AS seaover_company_code                                                              ");
            qry.Append($"\n  , IF(IFNULL(isManagement1, 0) = 0, 'FALSE', 'TRUE') AS  isManagement1                                         ");
            qry.Append($"\n  , IF(IFNULL(isManagement2, 0) = 0, 'FALSE', 'TRUE') AS  isManagement2                                         ");
            qry.Append($"\n  , IF(IFNULL(isManagement3, 0) = 0, 'FALSE', 'TRUE') AS  isManagement3                                         ");
            qry.Append($"\n  , IF(IFNULL(isManagement4, 0) = 0, 'FALSE', 'TRUE') AS  isManagement4                                         ");
            qry.Append($"\n  , IF(IFNULL(isDelete, 0) = 0, 'FALSE', 'TRUE') AS  isDelete                                         ");
            qry.Append($"\n  , IF(IFNULL(isHide, 0) = 0, 'FALSE', 'TRUE') AS  isHide                                                       ");
            qry.Append($"\n  , createtime                                                                                                ");
            qry.Append($"\n  , updatetime                                                                                                ");
            qry.Append($"\n  , edit_user                                                                                                ");

            qry.Append($"\n  , IF(IFNULL(isPotential1, 0) = 0, 'FALSE', 'TRUE') AS  isPotential1                                         ");
            qry.Append($"\n  , IF(IFNULL(isPotential2, 0) = 0, 'FALSE', 'TRUE') AS  isPotential2                                         ");
            qry.Append($"\n  , IF(IFNULL(isTrading, 0) = 0, 'FALSE', 'TRUE') AS  isTrading                                         ");
            qry.Append($"\n  , IF(IFNULL(isNonHandled, 0) = 0, 'FALSE', 'TRUE') AS  isNonHandled                                         ");
            qry.Append($"\n  , IF(IFNULL(isNotSendFax, 0) = 0, 'FALSE', 'TRUE') AS  isNotSendFax                                         ");
            qry.Append($"\n  , IF(IFNULL(isOutBusiness, 0) = 0, 'FALSE', 'TRUE') AS  isOutBusiness                                         ");

            qry.Append($"\n  , distribution                                                                                                ");
            qry.Append($"\n  , handling_item                                                                                                ");
            qry.Append($"\n  , remark2                                                                                               ");
            qry.Append($"\n  , remark3                                                                                                ");
            qry.Append($"\n  , remark4                                                                                                ");
            qry.Append($"\n  , payment_date                                                                                                ");
            qry.Append($"\n  , alarm_complete_date                                                                                              ");
            qry.Append($"\n  , sales_comment                                                                                              ");

            qry.Append($"\n  FROM t_company                                                                                              ");
            qry.Append($"\n  WHERE 1=1                                                                                                   ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n    AND id = {id}                                                                   ");
            if (!string.IsNullOrEmpty(name))
                qry.Append($"\n    AND name = '{name}'                                                                                   ");
            if (!string.IsNullOrEmpty(code))
                qry.Append($"\n    {commonRepository.whereSql("seaover_company_code", code, ",", true)}                                            ");
            //qry.Append($"\n    AND seaover_company_code = '{code}'                                                                   ");


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public CompanyModel GetCompanyAsOne2(string name, string code = "", string id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT                                                                                                         ");
            qry.Append($"\n    id                                                                                                        ");
            qry.Append($"\n  , division                                                                                                  ");
            qry.Append($"\n  , registration_number AS registration_number                                                                ");
            qry.Append($"\n  , group_name                                                                                                ");
            qry.Append($"\n  , name AS  name                                                                                             ");
            qry.Append($"\n  , origin AS origin                                                                                          ");
            qry.Append($"\n  , address AS address                                                                                        ");
            qry.Append($"\n  , ceo AS ceo                                                                                                ");
            qry.Append($"\n  , tel AS tel                                                                                                ");
            qry.Append($"\n  , fax AS fax                                                                                                ");
            qry.Append($"\n  , phone AS phone                                                                                            ");
            qry.Append($"\n  , other_phone AS other_phone                                                                                ");
            qry.Append($"\n  , company_manager AS company_manager                                                                        ");
            qry.Append($"\n  , company_manager_position AS company_manager_position                                                      ");
            qry.Append($"\n  , email AS email                                                                                            ");
            qry.Append($"\n  , remark AS remark                                                                                          ");
            qry.Append($"\n  , sns1 AS sns1                                                                                              ");
            qry.Append($"\n  , sns2 AS sns2                                                                                              ");
            qry.Append($"\n  , sns3 AS sns3                                                                                              ");
            qry.Append($"\n  , web AS web                                                                                                ");
            qry.Append($"\n  , ato_manager AS ato_manager                                                                                            ");
            qry.Append($"\n  , seaover_company_code AS seaover_company_code                                                              ");
            qry.Append($"\n  , IF(IFNULL(isManagement1, 0) = 0, 'FALSE', 'TRUE') AS  isManagement1                                         ");
            qry.Append($"\n  , IF(IFNULL(isManagement2, 0) = 0, 'FALSE', 'TRUE') AS  isManagement2                                         ");
            qry.Append($"\n  , IF(IFNULL(isManagement3, 0) = 0, 'FALSE', 'TRUE') AS  isManagement3                                         ");
            qry.Append($"\n  , IF(IFNULL(isManagement4, 0) = 0, 'FALSE', 'TRUE') AS  isManagement4                                         ");
            qry.Append($"\n  , IF(IFNULL(isDelete, 0) = 0, 'FALSE', 'TRUE') AS  isDelete                                         ");
            qry.Append($"\n  , IF(IFNULL(isHide, 0) = 0, 'FALSE', 'TRUE') AS  isHide                                                       ");
            qry.Append($"\n  , createtime                                                                                                ");
            qry.Append($"\n  , updatetime                                                                                                ");
            qry.Append($"\n  , edit_user                                                                                                ");

            qry.Append($"\n  , IF(IFNULL(isPotential1, 0) = 0, 'FALSE', 'TRUE') AS  isPotential1                                         ");
            qry.Append($"\n  , IF(IFNULL(isPotential2, 0) = 0, 'FALSE', 'TRUE') AS  isPotential2                                         ");
            qry.Append($"\n  , IF(IFNULL(isTrading, 0) = 0, 'FALSE', 'TRUE') AS  isTrading                                         ");
            qry.Append($"\n  , IF(IFNULL(isNonHandled, 0) = 0, 'FALSE', 'TRUE') AS  isNonHandled                                         ");
            qry.Append($"\n  , IF(IFNULL(isNotSendFax, 0) = 0, 'FALSE', 'TRUE') AS  isNotSendFax                                         ");
            qry.Append($"\n  , IF(IFNULL(isOutBusiness, 0) = 0, 'FALSE', 'TRUE') AS  isOutBusiness                                         ");

            qry.Append($"\n  , distribution                                                                                                ");
            qry.Append($"\n  , handling_item                                                                                                ");
            qry.Append($"\n  , remark2                                                                                               ");
            qry.Append($"\n  , remark3                                                                                                ");
            qry.Append($"\n  , remark4                                                                                                ");
            qry.Append($"\n  , payment_date                                                                                                ");
            qry.Append($"\n  , industry_type                                                                                                ");
            qry.Append($"\n  FROM t_company                                                                                              ");
            qry.Append($"\n  WHERE 1=1                                                                                                   ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n    AND id = {id}                                                                   ");
            if (!string.IsNullOrEmpty(name))
                qry.Append($"\n    AND name = '{name}'                                                                                   ");
            if (!string.IsNullOrEmpty(code))
                qry.Append($"\n    AND seaover_company_code = '{code}'                                                                   ");

            string sql = qry.ToString();

            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return SetCompany(dr);
        }

        private CompanyModel SetCompany(MySqlDataReader rd)
        {
            CompanyModel model = null;
            while (rd.Read())
            {
                model = new CompanyModel();
                model.id = Convert.ToInt32(rd["id"].ToString());
                model.division = rd["division"].ToString();
                model.registration_number = rd["registration_number"].ToString();
                model.group_name = rd["group_name"].ToString();
                model.name = rd["name"].ToString();
                model.origin = rd["origin"].ToString();
                model.address = rd["address"].ToString();
                model.ceo = rd["ceo"].ToString();
                model.fax = rd["fax"].ToString();
                model.tel = rd["tel"].ToString();
                model.phone = rd["phone"].ToString();
                model.other_phone = rd["other_phone"].ToString();
                model.company_manager = rd["company_manager"].ToString();
                model.company_manager_position = rd["company_manager_position"].ToString();
                model.email = rd["email"].ToString();
                model.remark = rd["remark"].ToString();
                model.sns1 = rd["sns1"].ToString();
                model.sns2 = rd["sns2"].ToString();
                model.sns3 = rd["sns3"].ToString();
                model.web = rd["web"].ToString();
                model.ato_manager = rd["ato_manager"].ToString();
                model.seaover_company_code = rd["seaover_company_code"].ToString();

                model.isManagement1 = Convert.ToBoolean(rd["isManagement1"].ToString());
                model.isManagement2 = Convert.ToBoolean(rd["isManagement2"].ToString());
                model.isManagement3 = Convert.ToBoolean(rd["isManagement3"].ToString());
                model.isManagement4 = Convert.ToBoolean(rd["isManagement4"].ToString());
                model.isDelete = Convert.ToBoolean(rd["isDelete"].ToString());
                model.isHide = Convert.ToBoolean(rd["isHide"].ToString());
                model.createtime = rd["createtime"].ToString();
                model.updatetime = rd["updatetime"].ToString();
                model.edit_user = rd["edit_user"].ToString();
                model.ato_manager = rd["ato_manager"].ToString();

                model.isPotential1 = Convert.ToBoolean(rd["isPotential1"].ToString());
                model.isPotential2 = Convert.ToBoolean(rd["isPotential2"].ToString());
                model.isTrading = Convert.ToBoolean(rd["isTrading"].ToString());
                model.isNonHandled = Convert.ToBoolean(rd["isNonHandled"].ToString());
                model.isNotSendFax = Convert.ToBoolean(rd["isNotSendFax"].ToString());
                model.isOutBusiness = Convert.ToBoolean(rd["isOutBusiness"].ToString());
                model.distribution = rd["distribution"].ToString();
                model.handling_item = rd["handling_item"].ToString();
                model.remark2 = rd["remark2"].ToString();
                model.remark3 = rd["remark3"].ToString();
                model.remark4 = rd["remark4"].ToString();
                model.payment_date = rd["payment_date"].ToString();
                model.industry_type = rd["industry_type"].ToString();
            }
            rd.Close();
            return model;
        }

        public DataTable GetDuplicateList()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                          ");
            qry.Append($"\n   tel                                           ");
            qry.Append($"\n , is_duplicate                                  ");
            qry.Append($"\n FROM (                                          ");
            qry.Append($"\n     SELECT                                      ");
            qry.Append($"\n     REPLACE(tel, '-', '') AS tel                                         ");
            qry.Append($"\n     , CASE WHEN IFNULL(IsNonHandled, 0) = TRUE THEN '취급X'     ");
            qry.Append($"\n            WHEN IFNULL(isNotSendFax, 0) = TRUE THEN '팩스X'     ");
            qry.Append($"\n            WHEN IFNULL(isOutBusiness, 0) = TRUE THEN '폐업'     ");
            qry.Append($"\n            WHEN IFNULL(isPotential1, 0) = TRUE THEN '잠재1'     ");
            qry.Append($"\n            WHEN IFNULL(isPotential2, 0) = TRUE THEN '잠재2'     ");
            qry.Append($"\n            WHEN IFNULL(ato_manager, '') = '' THEN '공용DATA'     ");
            qry.Append($"\n            ELSE '내DATA' END AS is_duplicate    ");

            qry.Append($"\n     FROM t_company                              ");
            qry.Append($"\n     WHERE division = '매출처'                      ");
            qry.Append($"\n       AND IFNULL(tel, '') <> ''                 ");
            qry.Append($"\n       AND IFNULL(isDelete, 0) = 0               ");
            qry.Append($"\n     UNION ALL                                   ");
            qry.Append($"\n     SELECT                                      ");
            qry.Append($"\n     REPLACE(fax, '-', '') AS tel                                  ");
            qry.Append($"\n     , CASE WHEN IFNULL(IsNonHandled, 0) = TRUE THEN '취급X'     ");
            qry.Append($"\n            WHEN IFNULL(isNotSendFax, 0) = TRUE THEN '팩스X'     ");
            qry.Append($"\n            WHEN IFNULL(isOutBusiness, 0) = TRUE THEN '폐업'     ");
            qry.Append($"\n            WHEN IFNULL(isPotential1, 0) = TRUE THEN '잠재1'     ");
            qry.Append($"\n            WHEN IFNULL(isPotential2, 0) = TRUE THEN '잠재2'     ");
            qry.Append($"\n            WHEN IFNULL(ato_manager, '') = '' THEN '공용DATA'     ");
            qry.Append($"\n            ELSE '내DATA' END AS is_duplicate    ");
            qry.Append($"\n     FROM t_company                              ");
            qry.Append($"\n     WHERE division = '매출처'                      ");
            qry.Append($"\n       AND IFNULL(fax, '') <> ''                 ");
            qry.Append($"\n       AND IFNULL(isDelete, 0) = 0               ");
            qry.Append($"\n     UNION ALL                                   ");
            qry.Append($"\n     SELECT                                      ");
            qry.Append($"\n     REPLACE(phone, '-', '') AS tel                               ");
            qry.Append($"\n     , CASE WHEN IFNULL(IsNonHandled, 0) = TRUE THEN '취급X'     ");
            qry.Append($"\n            WHEN IFNULL(isNotSendFax, 0) = TRUE THEN '팩스X'     ");
            qry.Append($"\n            WHEN IFNULL(isOutBusiness, 0) = TRUE THEN '폐업'     ");
            qry.Append($"\n            WHEN IFNULL(isPotential1, 0) = TRUE THEN '잠재1'     ");
            qry.Append($"\n            WHEN IFNULL(isPotential2, 0) = TRUE THEN '잠재2'     ");
            qry.Append($"\n            WHEN IFNULL(ato_manager, '') = '' THEN '공용DATA'     ");
            qry.Append($"\n            ELSE '내DATA' END AS is_duplicate    ");
            qry.Append($"\n     FROM t_company                              ");
            qry.Append($"\n     WHERE division = '매출처'                      ");
            qry.Append($"\n       AND IFNULL(phone, '') <> ''               ");
            qry.Append($"\n       AND IFNULL(isDelete, 0) = 0               ");
            qry.Append($"\n  ) AS t                                         ");
            qry.Append($"\n  GROUP BY tel, is_duplicate                     ");
            


            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public StringBuilder DeleteCompany(int id, bool isDelete = true)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE t_company SET isDelete = {isDelete.ToString()}, updatetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE id = {id}             ");
            return qry;
        }
        public StringBuilder UpdateCompany(int id, string registration_number, string ceo, string tel, string fax, string phone, string other_phone)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE t_company SET                                         ");
            qry.Append($"  registration_number = '{registration_number}'              ");
            qry.Append($", ceo = '{ceo}'                                              ");
            qry.Append($", tel = '{tel}'                                              ");
            qry.Append($", fax = '{fax}'                                              ");
            qry.Append($", phone = '{phone}'                                          ");
            qry.Append($", phone = '{other_phone}'                                          ");
            qry.Append($"WHERE id = {id}                                              ");
            return qry;
        }
        public StringBuilder UpdateAlarmCompany(int id, int alarm_month, int alarm_week, string alarm_date)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"UPDATE t_company SET                                         ");
            qry.Append($"  alarm_month = {alarm_month}                                ");
            qry.Append($", alarm_week = {alarm_week}                                  ");
            qry.Append($", alarm_date = '{alarm_date}'                                ");
            qry.Append($"WHERE id = {id}                                              ");
            return qry;
        }
        public StringBuilder InsertCompany(CompanyModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT INTO t_company (            ");
            qry.Append($"\n   id                               ");
            qry.Append($"\n , division                         ");
            qry.Append($"\n , registration_number              ");
            qry.Append($"\n , group_name                       ");
            qry.Append($"\n , name                             ");
            qry.Append($"\n , origin                           ");
            qry.Append($"\n , address                          ");
            qry.Append($"\n , ceo                              ");
            qry.Append($"\n , tel                              ");
            qry.Append($"\n , fax                              ");
            qry.Append($"\n , phone                            ");
            qry.Append($"\n , other_phone                      ");
            qry.Append($"\n , company_manager                  ");
            qry.Append($"\n , company_manager_position         ");
            qry.Append($"\n , email                            ");
            qry.Append($"\n , remark                           ");
            qry.Append($"\n , sns1                             ");
            qry.Append($"\n , sns2                             ");
            qry.Append($"\n , sns3                             ");
            qry.Append($"\n , web                              ");
            qry.Append($"\n , ato_manager                      ");
            qry.Append($"\n , seaover_company_code             ");
            qry.Append($"\n , isManagement1                    ");
            qry.Append($"\n , isManagement2                    ");
            qry.Append($"\n , isManagement3                    ");
            qry.Append($"\n , isManagement4                    ");
            qry.Append($"\n , isDelete                         ");
            qry.Append($"\n , isHide                           ");
            qry.Append($"\n , createtime                       ");
            qry.Append($"\n , updatetime                       ");
            qry.Append($"\n , edit_user                        ");
            qry.Append($"\n , distribution                     ");
            qry.Append($"\n , handling_item                    ");
            qry.Append($"\n , isPotential1                     ");
            qry.Append($"\n , isPotential2                     ");
            qry.Append($"\n , isTrading                        ");
            qry.Append($"\n , isNonHandled                     ");
            qry.Append($"\n , isNotSendFax                     ");
            qry.Append($"\n , isOutBusiness                    ");
            qry.Append($"\n , remark2                          ");
            qry.Append($"\n , remark3                          ");
            qry.Append($"\n , remark4                          ");
            qry.Append($"\n , remark5                          ");
            qry.Append($"\n , remark6                          ");
            qry.Append($"\n , payment_date                     ");
            qry.Append($"\n , sales_comment                    ");
            qry.Append($"\n , alarm_month                      ");
            qry.Append($"\n , alarm_week                       ");
            qry.Append($"\n , alarm_complete_date              ");
            qry.Append($"\n , industry_type                    ");
            qry.Append($"\n , industry_type2                    ");
            qry.Append($"\n ) VALUES (                                ");
            qry.Append($"\n    {model.id                      }          ");
            qry.Append($"\n , '{AddSlashes(model.division).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.registration_number).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.group_name).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.name).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.origin).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.address).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.ceo).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.tel).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.fax).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.phone).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.other_phone).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.company_manager).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.company_manager_position).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.email).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.remark).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.sns1).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.sns2).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.sns3).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.web).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.ato_manager).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.seaover_company_code).Trim()}'         ");
            qry.Append($"\n ,  {model.isManagement1           }          ");
            qry.Append($"\n ,  {model.isManagement2           }          ");
            qry.Append($"\n ,  {model.isManagement3           }          ");
            qry.Append($"\n ,  {model.isManagement4           }          ");
            qry.Append($"\n ,  {model.isDelete                }          ");
            qry.Append($"\n ,  {model.isHide                  }          ");
            qry.Append($"\n , '{AddSlashes(model.createtime).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.updatetime).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.edit_user).Trim()}'         ");
            qry.Append($"\n , '{AddSlashes(model.distribution).Trim()}'                 ");
            qry.Append($"\n , '{AddSlashes(model.handling_item).Trim()}'                 ");
            qry.Append($"\n , {model.isPotential1    }                 ");
            qry.Append($"\n , {model.isPotential2    }                 ");
            qry.Append($"\n , {model.isTrading    }                    ");
            qry.Append($"\n , {model.isNonHandled    }                 ");
            qry.Append($"\n , {model.isNotSendFax    }                 ");
            qry.Append($"\n , {model.isOutBusiness   }                 ");
            qry.Append($"\n , '{AddSlashes(model.remark2).Trim()         }'                 ");
            qry.Append($"\n , '{AddSlashes(model.remark3).Trim()        }'                 ");
            qry.Append($"\n , '{AddSlashes(model.remark4).Trim()        }'                 ");
            qry.Append($"\n , '{AddSlashes(model.remark5).Trim()}'                 ");
            qry.Append($"\n , '{AddSlashes(model.remark6).Trim()}'                 ");
            qry.Append($"\n , '{AddSlashes(model.payment_date).Trim()}'                 ");
            qry.Append($"\n , '{AddSlashes(model.sales_comment).Trim()}'                 ");
            qry.Append($"\n , {model.alarm_month   }                 ");
            qry.Append($"\n , '{model.alarm_week   }'                 ");
            qry.Append($"\n , '{model.alarm_complete_date   }'                 ");
            qry.Append($"\n , '{AddSlashes(model.industry_type).Trim()}'                 ");
            qry.Append($"\n , '{AddSlashes(model.industry_type2).Trim()}'                 ");
            qry.Append($"\n )                                            ");

            string sql = qry.ToString();
            return qry;
        }

        public StringBuilder RealDeleteCompany(int id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_company WHERE id = {id}            ");

            return qry;
        }

        public StringBuilder UpdateCompany(CompanyModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE t_company SET                                                     ");
            qry.Append($"\n   division                  = '{model.division                }'         ");
            qry.Append($"\n , registration_number       = '{model.registration_number     }'         ");
            qry.Append($"\n , group_name                = '{model.group_name              }'         ");
            qry.Append($"\n , name                      = '{model.name                    }'         ");
            qry.Append($"\n , origin                    = '{model.origin                  }'         ");
            qry.Append($"\n , address                   = '{model.address                 }'         ");
            qry.Append($"\n , ceo                       = '{model.ceo                     }'         ");
            qry.Append($"\n , tel                       = '{model.tel                     }'         ");
            qry.Append($"\n , fax                       = '{model.fax                     }'         ");
            qry.Append($"\n , phone                     = '{model.phone                   }'         ");
            qry.Append($"\n , company_manager           = '{model.company_manager         }'         ");
            qry.Append($"\n , company_manager_position  = '{model.company_manager_position}'         ");
            qry.Append($"\n , email                     = '{model.email                   }'         ");
            qry.Append($"\n , remark                    = '{model.remark                  }'         ");
            qry.Append($"\n , sns1                      = '{model.sns1                    }'         ");
            qry.Append($"\n , sns2                      = '{model.sns2                    }'         ");
            qry.Append($"\n , sns3                      = '{model.sns3                    }'         ");
            qry.Append($"\n , web                       = '{model.web                     }'         ");
            qry.Append($"\n , ato_manager               = '{model.ato_manager             }'         ");
            qry.Append($"\n , seaover_company_code      = '{model.seaover_company_code    }'         ");
            qry.Append($"\n , isManagement1             =  {model.isManagement1           }          ");
            qry.Append($"\n , isManagement2             =  {model.isManagement2           }          ");
            qry.Append($"\n , isManagement3             =  {model.isManagement3           }          ");
            qry.Append($"\n , isManagement4             =  {model.isManagement4           }          ");
            qry.Append($"\n , isDelete                  =  {model.isDelete                }          ");
            qry.Append($"\n , isHide                    =  {model.isHide                  }          ");
            qry.Append($"\n , createtime                = '{model.createtime              }'         ");
            qry.Append($"\n , updatetime                = '{model.updatetime              }'         ");
            qry.Append($"\n , edit_user                 = '{model.edit_user               }'         ");
            qry.Append($"\n WHERE id = {model.id}                                                    ");

            string sql = qry.ToString();
            return qry;
        }


        public DataTable GetCompany(string division, string group_name, string company, bool isExactly, string origin
                                            , string sns1, string sns2, string sns3, string email, string ato_manager, string id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                        ");
            qry.Append($"\n   id                                                                          ");
            qry.Append($"\n , division                                                                    ");
            qry.Append($"\n , registration_number                                                         ");
            qry.Append($"\n , group_name                                                                  ");
            qry.Append($"\n , name                                                                        ");
            qry.Append($"\n , origin                                                                      ");
            qry.Append($"\n , address                                                                     ");
            qry.Append($"\n , ceo                                                                         ");
            qry.Append($"\n , tel                                                                         ");
            qry.Append($"\n , fax                                                                         ");
            qry.Append($"\n , phone                                                                       ");
            qry.Append($"\n , company_manager                                                             ");
            qry.Append($"\n , company_manager_position                                                    ");
            qry.Append($"\n , email                                                                       ");
            qry.Append($"\n , sns1                                                                        ");
            qry.Append($"\n , sns2                                                                        ");
            qry.Append($"\n , sns3                                                                        ");
            qry.Append($"\n , web                                                                         ");
            qry.Append($"\n , ato_manager                                                                 ");
            qry.Append($"\n , seaover_company_code                                                        ");
            qry.Append($"\n , IF(IFNULL(isManagement1, 0) = 0, 'FALSE', 'TRUE')  AS isManagement1         ");
            qry.Append($"\n , IF(IFNULL(isManagement2, 0) = 0, 'FALSE', 'TRUE')  AS isManagement2         ");
            qry.Append($"\n , IF(IFNULL(isManagement3, 0) = 0, 'FALSE', 'TRUE')  AS isManagement3         ");
            qry.Append($"\n , IF(IFNULL(isManagement4, 0) = 0, 'FALSE', 'TRUE')  AS isManagement4         ");
            qry.Append($"\n , IF(IFNULL(isHide, 0) = 0, 'FALSE', 'TRUE')  AS isHide                       ");
            qry.Append($"\n , IF(IFNULL(isDelete, 0) = 0, 'FALSE', 'TRUE')  AS isDelete                   ");
            qry.Append($"\n , createtime                                                                  ");
            qry.Append($"\n , updatetime                                                                  ");
            qry.Append($"\n , edit_user                                                                   ");
            qry.Append($"\n , remark                                                                      ");
            qry.Append($"\n FROM t_company                                                                ");
            qry.Append($"\n WHERE 1=1                                                                     ");
            qry.Append($"\n   AND IFNULL(isDelete, 0) = FALSE                                             ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n   AND id = {id}                                                     ");
            else
            {
                if (!string.IsNullOrEmpty(division) && division != "전체")
                    qry.Append($"\n   AND division = '{division}'                                                     ");
                if (!string.IsNullOrEmpty(group_name))
                    qry.Append($"\n   {commonRepository.whereSql("group_name", group_name)}                                                     ");
                if (!string.IsNullOrEmpty(company))
                {
                    if (isExactly)
                        qry.Append($"\n   AND name LIKE '%{company}%'                                                     ");
                    else
                        qry.Append($"\n   {commonRepository.whereSql("name", company)}                                                     ");
                }
                if (!string.IsNullOrEmpty(origin))
                    qry.Append($"\n   {commonRepository.whereSql("origin", origin)}                                                     ");
                if (!string.IsNullOrEmpty(sns1))
                    qry.Append($"\n   {commonRepository.whereSql("sns1", sns1)}                                                     ");
                if (!string.IsNullOrEmpty(sns2))
                    qry.Append($"\n   {commonRepository.whereSql("sns2", sns2)}                                                     ");
                if (!string.IsNullOrEmpty(sns3))
                    qry.Append($"\n   {commonRepository.whereSql("sns3", sns3)}                                                     ");
                if (!string.IsNullOrEmpty(email))
                    qry.Append($"\n   {commonRepository.whereSql("email", email)}                                                     ");
                if (!string.IsNullOrEmpty(ato_manager))
                    qry.Append($"\n   {commonRepository.whereSql("ato_manager", ato_manager)}                                                     ");
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

        public DataTable GetCompanySaleInfo(string group_name, string company, bool isExactly, string ato_manager, string seaover_company_code = ""
                            , bool isPotential1 = false, bool isPotential2 = false, bool isNonHandled = false, bool isOutBusiness = false, bool isNotSendFax = false, bool isHide = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n   SELECT                                                                       ");
            qry.Append($"\n     c.*                                                                        ");
            qry.Append($"\n   , CASE WHEN c.isNonHandled = true THEN '취급X'                                 ");
            qry.Append($"\n          WHEN c.isOutBusiness = true THEN '폐업'                                 ");
            qry.Append($"\n          WHEN c.isNotSendFax = true THEN '팩스X'                                 ");
            qry.Append($"\n          WHEN c.isPotential1 = true THEN '잠재1'                                 ");
            qry.Append($"\n          WHEN c.isPotential2 = true THEN '잠재2'                                 ");
            qry.Append($"\n          ELSE '등록된 거래처' END                                                   ");
            qry.Append($"\n   FROM(                                                                        ");
            qry.Append($"\n  	 SELECT                                                                    ");
            qry.Append($"\n  	   id                                                                      ");
            qry.Append($"\n  	 , division                                                                ");
            qry.Append($"\n  	 , registration_number                                                     ");
            qry.Append($"\n  	 , group_name                                                              ");
            qry.Append($"\n  	 , name AS company                                                         ");
            qry.Append($"\n  	 , origin                                                                  ");
            qry.Append($"\n  	 , address                                                                 ");
            qry.Append($"\n  	 , ceo                                                                     ");
            qry.Append($"\n  	 , tel                                                                     ");
            qry.Append($"\n  	 , fax                                                                     ");
            qry.Append($"\n  	 , phone                                                                   ");
            qry.Append($"\n  	 , other_phone                                                                   ");
            qry.Append($"\n  	 , company_manager                                                         ");
            qry.Append($"\n  	 , company_manager_position                                                ");
            qry.Append($"\n  	 , email                                                                   ");
            qry.Append($"\n  	 , sns1                                                                    ");
            qry.Append($"\n  	 , sns2                                                                    ");
            qry.Append($"\n  	 , sns3                                                                    ");
            qry.Append($"\n  	 , web                                                                     ");
            qry.Append($"\n  	 , ato_manager                                                             ");
            qry.Append($"\n  	 , seaover_company_code                                                    ");
            qry.Append($"\n  	 , IF(IFNULL(isManagement1, 0) = 0, 'FALSE', 'TRUE')  AS isManagement1     ");
            qry.Append($"\n  	 , IF(IFNULL(isManagement2, 0) = 0, 'FALSE', 'TRUE')  AS isManagement2     ");
            qry.Append($"\n  	 , IF(IFNULL(isManagement3, 0) = 0, 'FALSE', 'TRUE')  AS isManagement3     ");
            qry.Append($"\n  	 , IF(IFNULL(isManagement4, 0) = 0, 'FALSE', 'TRUE')  AS isManagement4     ");
            qry.Append($"\n  	 , IF(IFNULL(isHide, 0) = 0, 'FALSE', 'TRUE')  AS isHide                   ");
            qry.Append($"\n  	 , IF(IFNULL(isDelete, 0) = 0, 'FALSE', 'TRUE')  AS isDelete               ");
            qry.Append($"\n      , IF(IFNULL(isNonHandled, 0) = 0, 'FALSE', 'TRUE')  AS isNonHandled       ");
            qry.Append($"\n      , IF(IFNULL(isOutBusiness, 0) = 0, 'FALSE', 'TRUE')  AS isOutBusiness     ");
            qry.Append($"\n      , IF(IFNULL(isNotSendFax, 0) = 0, 'FALSE', 'TRUE')  AS isNotSendFax       ");
            qry.Append($"\n      , IF(IFNULL(isPotential1, 0) = 0, 'FALSE', 'TRUE')  AS isPotential1       ");
            qry.Append($"\n      , IF(IFNULL(isPotential2, 0) = 0, 'FALSE', 'TRUE')  AS isPotential2       ");
            qry.Append($"\n      , IF(IFNULL(isTrading, 0) = 0, 'FALSE', 'TRUE')  AS isTrading             ");
            qry.Append($"\n  	 , createtime                                                              ");
            qry.Append($"\n  	 , updatetime                                                              ");
            qry.Append($"\n  	 , edit_user                                                               ");
            qry.Append($"\n  	 , handling_item                                                           ");
            qry.Append($"\n  	 , distribution                                                            ");
            qry.Append($"\n  	 , remark2                                                                 ");
            qry.Append($"\n  	 , remark3                                                                 ");
            qry.Append($"\n  	 , payment_date                                                            ");
            qry.Append($"\n  	 FROM t_company                                                            ");
            qry.Append($"\n  	 WHERE 1=1                                                                 ");
            qry.Append($"\n         AND division = '매출처'                                                   ");
            if (!string.IsNullOrEmpty(group_name))
                qry.Append($"\n   {commonRepository.whereSql("group_name", group_name)}                                                     ");
            if (!string.IsNullOrEmpty(company))
            {
                if (isExactly)
                    qry.Append($"\n   AND company = '{company}'                                                    ");
                else
                    qry.Append($"\n   {commonRepository.whereSql("company", company)}                                                     ");
            }

            if (!string.IsNullOrEmpty(ato_manager))
                qry.Append($"\n   {commonRepository.whereSql("ato_manager", ato_manager)}                                                     ");

            if (!string.IsNullOrEmpty(seaover_company_code))
                qry.Append($"\n   AND seaover_company_code = '{seaover_company_code}'                                                    ");
            else
                qry.Append($"\n        AND IFNULL(seaover_company_code, '') = ''                                ");
            qry.Append($"\n   AND isHide = {isHide}                                                    ");
            qry.Append($"\n   ) AS c                                                                       ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }


        public DataTable GetCompanyRecovery(string group_name, string company, bool isExactly, string ato_manager, string seaover_company_code = ""
                            , bool isManagement1 = false, bool isManagement2 = false, bool isManagement3 = false, bool isManagement4 = false, bool isHide = false)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n  SELECT                                                                          ");
            qry.Append($"\n    c.*                                                                           ");
            qry.Append($"\n  , IFNULL(r.last_year_profit, 5.5) AS last_year_profit                           ");
            qry.Append($"\n  , IFNULL(r.net_operating_capital_rate, 13.64) AS net_operating_capital_rate     ");
            qry.Append($"\n  , IFNULL(r.operating_capital_rate, 10.45) AS operating_capital_rate             ");
            qry.Append($"\n  , IFNULL(r.ato_capital_rate, 0.8) AS ato_capital_rate                           ");
            qry.Append($"\n  , IFNULL(r.equity_capital_rate, 21.47) AS equity_capital_rate                   ");
            qry.Append($"\n  , IFNULL(r.target_recovery_month, 18) AS target_recovery_month                  ");
            qry.Append($"\n  , r.updatetime AS recovery_updatetime                                           ");
            qry.Append($"\n  , r.remark AS recovery_remark                                                   ");
            qry.Append($"\n  , r.edit_user AS recovery_edit_user                                             ");
            qry.Append($"\n  FROM(                                                                           ");
            qry.Append($"\n 	 SELECT                                                                      ");
            qry.Append($"\n 	   id                                                                        ");
            qry.Append($"\n 	 , division                                                                  ");
            qry.Append($"\n 	 , registration_number                                                       ");
            qry.Append($"\n 	 , group_name                                                                ");
            qry.Append($"\n 	 , name AS company                                                           ");
            qry.Append($"\n 	 , origin                                                                    ");
            qry.Append($"\n 	 , address                                                                   ");
            qry.Append($"\n 	 , ceo                                                                       ");
            qry.Append($"\n 	 , tel                                                                       ");
            qry.Append($"\n 	 , fax                                                                       ");
            qry.Append($"\n 	 , phone                                                                     ");
            qry.Append($"\n 	 , company_manager                                                           ");
            qry.Append($"\n 	 , company_manager_position                                                  ");
            qry.Append($"\n 	 , email                                                                     ");
            qry.Append($"\n 	 , sns1                                                                      ");
            qry.Append($"\n 	 , sns2                                                                      ");
            qry.Append($"\n 	 , sns3                                                                      ");
            qry.Append($"\n 	 , web                                                                       ");
            qry.Append($"\n 	 , ato_manager                                                               ");
            qry.Append($"\n 	 , seaover_company_code                                                      ");
            qry.Append($"\n 	 , remark                                                                    ");
            qry.Append($"\n 	 , IF(IFNULL(isManagement1, 0) = 0, 'FALSE', 'TRUE')  AS isManagement1       ");
            qry.Append($"\n 	 , IF(IFNULL(isManagement2, 0) = 0, 'FALSE', 'TRUE')  AS isManagement2       ");
            qry.Append($"\n 	 , IF(IFNULL(isManagement3, 0) = 0, 'FALSE', 'TRUE')  AS isManagement3       ");
            qry.Append($"\n 	 , IF(IFNULL(isManagement4, 0) = 0, 'FALSE', 'TRUE')  AS isManagement4       ");
            qry.Append($"\n 	 , IF(IFNULL(isTrading, 0) = 0, 'FALSE', 'TRUE')  AS isTrading               ");
            qry.Append($"\n 	 , IF(IFNULL(isHide, 0) = 0, 'FALSE', 'TRUE')  AS isHide                     ");
            qry.Append($"\n 	 , IF(IFNULL(isDelete, 0) = 0, 'FALSE', 'TRUE')  AS isDelete                 ");
            qry.Append($"\n 	 , createtime                                                                ");
            qry.Append($"\n 	 , updatetime                                                                ");
            qry.Append($"\n 	 , edit_user                                                                 ");
            qry.Append($"\n 	 FROM t_company                                                              ");
            qry.Append($"\n 	 WHERE 1=1                                                                   ");
            qry.Append($"\n 	   AND IFNULL(isDelete, 0) = FALSE                                           ");
            qry.Append($"\n 	   AND IFNULL(isHide, 0) = {isHide}                                                    ");
            qry.Append($"\n        AND IFNULL(seaover_company_code, '') <> ''                                ");
            qry.Append($"\n        AND division = '매출처'                                                      ");
            

            if (!string.IsNullOrEmpty(group_name))
                qry.Append($"\n   {commonRepository.whereSql("group_name", group_name)}                                                     ");
            if (!string.IsNullOrEmpty(company))
            {
                if (isExactly)
                    qry.Append($"\n   AND name = '{company}'                                                    ");
                else
                    qry.Append($"\n   {commonRepository.whereSql("name", company)}                                                     ");
            }

            if (!string.IsNullOrEmpty(ato_manager))
                qry.Append($"\n   {commonRepository.whereSql("ato_manager", ato_manager)}                                                     ");

            if (!string.IsNullOrEmpty(seaover_company_code))
            { 
                if(seaover_company_code.Split(',').Length > 0)
                    qry.Append($"\n   {commonRepository.whereSql("seaover_company_code", seaover_company_code, ",")}                                                    ");
                else
                    qry.Append($"\n   AND seaover_company_code = '{seaover_company_code}'                                                    ");
            }
                
            

            if (isManagement1)
                qry.Append($"\n   AND isManagement1 = {isManagement1}                                                    ");
            if (isManagement2)
                qry.Append($"\n   AND isManagement2 = {isManagement2}                                                    ");
            if (isManagement3)
                qry.Append($"\n   AND isManagement3 = {isManagement3}                                                    ");
            if (isManagement4)
                qry.Append($"\n   AND isManagement4 = {isManagement4}                                                    ");

            
            qry.Append($"\n  ) AS c                                                                          ");
            qry.Append($"\n  LEFT OUTER JOIN (                                                               ");
            qry.Append($"\n 	SELECT                                                                       ");
            qry.Append($"\n       company_code                                                               ");
            qry.Append($"\n  	 , CAST(last_year_profit AS CHAR) AS last_year_profit                            ");
            qry.Append($"\n      , CAST(net_operating_capital_rate AS CHAR) AS net_operating_capital_rate        ");
            qry.Append($"\n      , CAST(operating_capital_rate AS CHAR) AS operating_capital_rate                ");
            qry.Append($"\n      , CAST(ato_capital_rate AS CHAR) AS ato_capital_rate                            ");
            qry.Append($"\n      , CAST(equity_capital_rate AS CHAR) AS equity_capital_rate                      ");
            qry.Append($"\n      , CAST(target_recovery_month AS CHAR) AS target_recovery_month                  ");
            qry.Append($"\n     , remark                                                                     ");
            qry.Append($"\n     , updatetime                                                                 ");
            qry.Append($"\n     , edit_user                                                                  ");
            qry.Append($"\n     FROM t_company_recovery                                                      ");
            qry.Append($"\n  ) AS r                                                                          ");
            qry.Append($"\n    ON c.seaover_company_code = r.company_code                                                        ");

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
