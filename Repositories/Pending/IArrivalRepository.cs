using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Pending
{
    public interface IArrivalRepository
    {
        int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        StringBuilder InsertSql(ArrivalModel model);
        StringBuilder DeleteSql(string id);

        StringBuilder UpdateSql(string id, string whr, string val);
    }
}
