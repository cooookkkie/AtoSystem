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
    public interface IProductGroupRepository
    {
        StringBuilder DeleteSubProduct(string main_id, string sub_id);
        DataTable GetProductGroup(string product = "", string origin = "", string sizes = "", string unit = ""
            , string item_product = "", string item_origin = "", string item_sizes = "", string item_unit = "");
        DataTable GetProductGroup2(string product = "", string origin = "", string sizes = "", string unit = "");
        DataTable GetProductGroup3(string product = "", string origin = "", string sizes = "", string unit = "");
        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        StringBuilder InsertProductGroup(ProductGroupModel model);
        StringBuilder DeleteProductGroup(ProductGroupModel model);
        DataTable GetSubProduct(string product = "", string origin = "", string sizes = "", string unit = "", string price_unit = "", string unit_count = "", string seaover_unit = "");
    }
}
