using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class CommonRepository : ClassRoot, ICommonRepository
    {
        public double GetTrq()
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT trq_price FROM t_trq    ");
            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToDouble(command.ExecuteScalar());
        }

        public int UpdateTrq(double trq)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" UPDATE t_trq SET trq_price = {trq}, updatetime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'    ");
            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public StringBuilder UpdateData(string db_name, string update, string where)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE {db_name}                                 ");
            qry.Append($"\n SET {update}                                 ");
            qry.Append($"\n WHERE 1=1 AND {where}                              ");
            
            string sql = qry.ToString();

            return qry;
        }

        public StringBuilder UpdateData(string db_name, string[] col, string[] val, string where)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n UPDATE {db_name}                                 ");
            qry.Append($"\n SET                                              ");
            if (col.Length > 0 && val.Length > 0)
            {
                string updateCol = "";
                for (int i = 0; i < col.Length; i++)
                {
                    if (string.IsNullOrEmpty(updateCol))
                    {
                        if(AddSlashes(val[i]).Equals("TRUE") || AddSlashes(val[i]).Equals("FALSE") || AddSlashes(val[i]).Equals("true") || AddSlashes(val[i]).Equals("false"))
                            updateCol = col[i] + " = " + AddSlashes(val[i]) ;
                        else
                            updateCol = col[i] + " = '" + AddSlashes(val[i]) + "'";
                    }
                    else
                    {
                        if (AddSlashes(val[i]).Equals("TRUE") || AddSlashes(val[i]).Equals("FALSE") || AddSlashes(val[i]).Equals("true") || AddSlashes(val[i]).Equals("false"))
                            updateCol += "\n, " + col[i] + " = " + AddSlashes(val[i]);
                        else
                            updateCol += "\n, " + col[i] + " = '" + AddSlashes(val[i]) + "'";
                    }
                }
                qry.Append($"\n {updateCol}                                              ");
            }
            
            qry.Append($"\n WHERE 1=1 AND {where}                              ");

            string sql = qry.ToString();

            return qry;
        }

        public DataTable SelectData(string col, string db, string[] whrCol, string[] whrVal)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n SELECT                                  ");
            qry.Append($"\n   {col}                                 ");
            qry.Append($"\n FROM {db}                               ");
            qry.Append($"\n WHERE 1=1                               ");
            if (whrCol.Length == whrVal.Length && whrCol.Length > 0)
            {
                for (int i = 0; i < whrCol.Length; i++)
                    qry.Append($"\n AND {whrCol[i]} = {whrVal[i]}                               ");
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
        public StringBuilder DeleteSql(string dbName, string id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM {dbName} WHERE id = {id}");
            return qry;
        }

        public DataTable SelectAsOne(string dbName, string colName, string whr, string val)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT {colName} FROM {dbName}");
            qry.Append($"\n WHERE 1=1");
            if (!string.IsNullOrEmpty(whr) && !string.IsNullOrEmpty(val))
                qry.Append($"\n   AND {whr} = {val}");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable SelectAsOneLike(string dbName, string colName, string whr, string val)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT {colName} FROM {dbName}");
            qry.Append($"\n WHERE 1=1");
            if (!string.IsNullOrEmpty(whr) && !string.IsNullOrEmpty(val))
                qry.Append($"\n   AND {whr} LIKE '%{val}%'");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }

        public int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null)
        {
            string sql;
            int errCnt = 0;
            int i;
            if (sqlList.Count > 0)
            {

                MySqlConnection conn = (MySqlConnection)dbInstance.Connection;
                MySqlCommand command = conn.CreateCommand();
                command.CommandTimeout = 10000;
                transaction = conn.BeginTransaction();
                try
                {
                    int susccesCnt = 0;
                    
                    for (i = 0; i < sqlList.Count; i++)
                    {
                        sql = sqlList[i].ToString();
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                        susccesCnt++;
                        errCnt = i;
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
        public int GetNextId(string dbName, string colName, string whrCol = "", string whrVal = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT IF( MAX({colName}) IS NULL, 1, MAX({colName}) +1) AS id FROM {dbName} WHERE 1=1 ");
            if(!string.IsNullOrEmpty(whrCol) && !string.IsNullOrEmpty(whrVal))
                qry.Append($" AND {whrCol} = {whrVal} ");

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public int GetNextIdMulti(string dbName, string colName, string[] whrCol = null, string[] whrVal = null)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" SELECT IF( MAX({colName}) IS NULL, 1, MAX({colName}) +1) AS id FROM {dbName} WHERE 1=1 ");
            if (whrCol != null && whrVal != null)
            {
                for (int i = 0; i < whrCol.Length; i++)
                {
                    qry.Append($" AND {whrCol[i]} = {whrVal[i]} ");
                }
            }
                

            string sql = qry.ToString();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);

            return Convert.ToInt32(command.ExecuteScalar());
        }
        public string whereSql(string whrColumn, string whrValue, bool isStart = false)
        {
            string whrStr = "";
            if (whrValue == null || string.IsNullOrEmpty(whrValue))
                return whrStr;
            //통으로 검색
            whrValue = whrValue.Trim();
            whrColumn = whrColumn.Trim();
            if (!string.IsNullOrEmpty(whrColumn) && !string.IsNullOrEmpty(whrColumn))
            {
                if (whrValue.Substring(whrValue.Length - 1, 1).Equals(";"))
                    whrStr = $" AND {whrColumn} LIKE '%{whrValue.Substring(0, whrValue.Length - 1)}%'";
                //띄어쓰기 나눠서 검색
                else
                {
                    string[] tempStr = null;
                    string tempWhr = "";
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
                                        if (isStart)
                                            tempWhr = $"\n	   {whrColumn} LIKE '{tempStr[i]}%' ";
                                        else
                                            tempWhr = $"\n	   {whrColumn} LIKE '%{tempStr[i]}%' ";
                                    }
                                    else
                                    {
                                        if (isStart)
                                            tempWhr += $"\n	   OR {whrColumn} LIKE '{tempStr[i]}%' ";
                                        else
                                            tempWhr += $"\n	   OR {whrColumn} LIKE '%{tempStr[i]}%' ";
                                    }
                                }
                            }
                            whrStr = $"\n	 AND ( {tempWhr} )";
                        }
                        else
                        {
                            if (isStart)
                                whrStr = $"\n	  AND {whrColumn} LIKE '{whrValue}%'";
                            else
                                whrStr = $"\n	  AND {whrColumn} LIKE '%{whrValue}%'";
                        }
                    }
                }
            }
            return whrStr;
        }
        public string whereSql(string whrColumn, string whrValue, string separator, bool isExactly = false)
        {
            string whrStr = "";
            //통으로 검색
            whrValue = whrValue.Trim();
            whrColumn = whrColumn.Trim();
            if (!string.IsNullOrEmpty(whrColumn) && !string.IsNullOrEmpty(whrColumn))
            {
                if (whrValue.Substring(whrValue.Length - 1, 1).Equals(";"))
                {
                    if(!isExactly)
                        whrStr = $" AND {whrColumn} = '{whrValue.Substring(0, whrColumn.Length - 2)}'";
                    else
                        whrStr = $" AND {whrColumn} LIKE '%{whrValue.Substring(0, whrColumn.Length - 2)}%'";
                }
                //띄어쓰기 나눠서 검색
                else
                {
                    string[] tempStr = null;
                    string tempWhr = "";
                    if (!string.IsNullOrEmpty(whrValue.Trim()))
                    {
                        tempStr = whrValue.Split(new string[] { separator }, StringSplitOptions.None);
                        if (tempStr.Length > 1)
                        {
                            for (int i = 0; i < tempStr.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tempStr[i].Trim()))
                                {
                                    if (string.IsNullOrEmpty(tempWhr))
                                    {
                                        if (isExactly)
                                            tempWhr = $"\n	   {whrColumn} = '{tempStr[i].Trim()}' ";
                                        else
                                            tempWhr = $"\n	   {whrColumn} LIKE '%{tempStr[i].Trim()}%' ";
                                    }
                                    else
                                    {
                                        if (isExactly)
                                            tempWhr += $"\n	   OR {whrColumn} = '{tempStr[i].Trim()}' ";
                                        else
                                            tempWhr += $"\n	   OR {whrColumn} LIKE '%{tempStr[i].Trim()}%' ";
                                    }
                                }
                            }
                            whrStr = $"\n	 AND ( {tempWhr} )";
                        }
                        else
                        {
                            if (isExactly)
                                whrStr = $"\n	  AND {whrColumn} = '{whrValue}'";
                            else
                                whrStr = $"\n	  AND {whrColumn} LIKE '%{whrValue}%'";
                        }
                    }
                }
            }
            return whrStr;
        }
    }
}
