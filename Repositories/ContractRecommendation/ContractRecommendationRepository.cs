using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ContractRecommendation
{
    public class ContractRecommendationRepository: ClassRoot, IContractRecommendationRepository
    {
        ICommonRepository commonRepository = new CommonRepository();
        public StringBuilder DeleteRecommend(string product, string origin)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_product_contract_recommendation WHERE product = '{product}' AND origin = '{origin}'         ");

            return qry;
        }
        public StringBuilder DeleteRecommendAsOne(ContractRecommendationModel cm)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_product_contract_recommendation");
            qry.Append($" WHERE product = '{cm.product}'                  ");
            qry.Append($"   AND origin = '{cm.product}'                  ");
            qry.Append($"   AND division = '{cm.division}'                  ");
            qry.Append($"   AND month =  {cm.month}                  ");

            return qry;
        }

        public StringBuilder InsertRecommend(ContractRecommendationModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_product_contract_recommendation (           ");
            qry.Append($"   product                               ");
            qry.Append($" , origin                                ");
            qry.Append($" , division                              ");
            qry.Append($" , month                                 ");
            qry.Append($" , recommend_level                       ");
            qry.Append($" , remark                                ");
            qry.Append($" , edit_user                             ");
            qry.Append($" , updatetime                            ");
            qry.Append($" ) VALUES (                              ");
            qry.Append($"   '{model.product}'                     ");
            qry.Append($" , '{model.origin}'                      ");
            qry.Append($" , '{model.division}'                    ");
            qry.Append($" ,  {model.month}                        ");
            qry.Append($" ,  {model.recommend_level}              ");
            qry.Append($" , '{model.remark}'                      ");
            qry.Append($" , '{model.edit_user}'                   ");
            qry.Append($" , '{model.updatetime}'                  ");
            qry.Append($" )                                       ");

            return qry;
        }

        public DataTable GetRecommendAsOne(string product, string origin)
        {
            StringBuilder qry = new StringBuilder();                          
            qry.Append($"\n SELECT                                            ");
            qry.Append($"\n *                                                 ");
            qry.Append($"\n FROM t_product_contract_recommendation            ");
            qry.Append($"\n WHERE 1=1                       ");
            qry.Append($"\n   AND product = '{product}'     ");
            qry.Append($"\n   AND origin = '{origin}'       ");

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetRecommend(string product, string origin, string period = "", string contract = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                            ");
            qry.Append($"\n    *                                              ");
            qry.Append($"\n FROM t_product_contract_recommendation            ");
            qry.Append($"\n WHERE 1=1                                         ");
            if(!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("product", product)}                                       ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("origin", origin)}                                       ");
            if (!string.IsNullOrEmpty(period))
                qry.Append($"\n   AND (division = '조업시기' {commonRepository.whereSql("month", period)} )            ");
            if (!string.IsNullOrEmpty(contract))
                qry.Append($"\n   AND (division = '계약시기' {commonRepository.whereSql("month", contract)} )            ");

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public DataTable GetRecommendGroupConcat(string product, string origin, string period = "", string contract = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                                                         ");
            qry.Append($"\n * FROM (                                                                       ");
            qry.Append($"\n SELECT                                                                         ");
            qry.Append($"\n   product                                                                      ");
            qry.Append($"\n , origin                                                                       ");
            qry.Append($"\n , GROUP_CONCAT(IF(division = '조업시기', sMonth, null)) AS operating           ");
            qry.Append($"\n , GROUP_CONCAT(IF(division = '계약시기', sMonth, null)) AS contract            ");
            qry.Append($"\n , edit_user                                                                    ");
            qry.Append($"\n , updatetime                                                                   ");
            qry.Append($"\n , GROUP_CONCAT(IF(division = '조업시기', remark, null)) AS operating_remark    ");
            qry.Append($"\n , GROUP_CONCAT(IF(division = '계약시기', remark, null)) AS contract_remark     ");
            qry.Append($"\n FROM (                                                                         ");
            qry.Append($"\n	 SELECT                                                                        ");
            qry.Append($"\n	   product, origin, division, GROUP_CONCAT('^', CONCAT(month, '_', IFNULL(recommend_level, 1))) AS sMonth, remark       ");
            qry.Append($"\n	 , edit_user                                                                   ");
            qry.Append($"\n	 , updatetime                                                                  ");
            qry.Append($"\n	 FROM t_product_contract_recommendation                                        ");
            qry.Append($"\n     WHERE 1=1                                                                  ");
            if (!string.IsNullOrEmpty(product))
                qry.Append($"\n   {commonRepository.whereSql("product", product)}                                       ");
            if (!string.IsNullOrEmpty(origin))
                qry.Append($"\n   {commonRepository.whereSql("origin", origin)}                                         ");
            qry.Append($"\n	 GROUP BY product, origin, division, remark, edit_user, updatetime             ");
            qry.Append($"\n ) AS t2                                                                        ");
            qry.Append($"\n GROUP BY product, origin, edit_user, updatetime                               ");
            qry.Append($"\n ) AS t                                                                        ");
            qry.Append($"\n WHERE 1=1                                                                      ");
            if (!string.IsNullOrEmpty(period))
                qry.Append($"\n   {whereMonthSql("t.operating", period)}                                        ");
            if (!string.IsNullOrEmpty(contract))
                qry.Append($"\n   {whereMonthSql("t.contract", contract)}                                        ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        private string whereMonthSql(string whrColumn, string whrValue)
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
                                tempWhr = $"\n	   {whrColumn} LIKE '%^{tempStr[i]}%' ";
                            }
                            else
                            {
                                tempWhr += $"\n	   OR {whrColumn} LIKE '%^{tempStr[i]}%' ";
                            }
                        }
                    }
                    whrStr = $"\n	 AND ( {tempWhr} )";
                }
                else
                {
                    whrStr = $"\n	  AND {whrColumn} LIKE '%^{whrValue}%'";
                }
            }
            return whrStr;
        }

        public int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
        {
            if (sqlList.Count > 0)
            {

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

        
    }
}
