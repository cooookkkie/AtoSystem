using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SEAOVER
{
    public interface IStockLimitRepository
    {
        StringBuilder DeleteProduct(ProductLimitModel model);
        DataTable GetProductLimit(string product = "", string origin = "", string sizes = "", string unit = "");
        StringBuilder InsertProduct(ProductLimitModel model);
        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
    }
}
