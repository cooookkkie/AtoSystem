using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ICommonRepository
    {
        string whereSql(string whrColumn, string whrValue, string separator, bool isExactly = false);
        string whereSql(string whrColumn, string whrValue, bool isStart = false);
        double GetTrq();
        int UpdateTrq(double trq);
        StringBuilder UpdateData(string db_name, string[] col, string[] val, string where);
        StringBuilder UpdateData(string db_name, string update, string where);
        DataTable SelectData(string col, string db, string[] whrCol, string[] whrVal);
        StringBuilder DeleteSql(string dbName, string id);
        DataTable SelectAsOne(string dbName, string colName, string whr, string val);
        DataTable SelectAsOneLike(string dbName, string colName, string whr, string val);
        int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        int GetNextId(string dbName, string colName, string whrCol = "", string whrVal = "");
        int GetNextIdMulti(string dbName, string colName, string[] whrCol = null, string[] whrVal = null);
    }
}
