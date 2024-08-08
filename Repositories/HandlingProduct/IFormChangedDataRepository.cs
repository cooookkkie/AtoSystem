using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFormChangedDataRepository
    {
        StringBuilder DeleteChangeData(string whr);
        int GetNextId();
        DataTable GetChangeCurrentData();
        DataTable GetChangeData();
        int UpdateCustomTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        StringBuilder InsertSql(FormChangedDataModel model);
        StringBuilder DeleteSql(FormChangedDataModel model);
    }
}
