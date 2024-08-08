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
    public interface IProductOtherCostRepository
    {
        DataTable GetProductOnlyOne(string product, string origin, string sizes, string seaover_unit);
        DataTable GetProductInfoAsOneExactly(string product, string origin, string sizes, string seaover_unit);
        DataTable GetProductByUser(string product, string origin, string manager);
        DataTable GetProductInfoAsOne(string product, string origin, string sizes, string unit, string seaover_unit);
        DataTable GetProductAsOne(string product, string origin, string sizes, string unit, string seaover_unit);
        DataTable GetProduct(string product, bool isExactly, string origin, string sizes, string unit, string manager = "", string group = "");
        StringBuilder InsertProduct(ProductOtherCostModel model);
        StringBuilder DeleteProduct(ProductOtherCostModel model);
        StringBuilder InsertProduct2(ProductOtherCostModel model);
        StringBuilder UpdateProduct(string product, string origin, string sizes, string unit, string seaover_unit, int tabType, bool isVal, string sdate, string edate);
        StringBuilder UpdateProductPurchaseMargin(string product, string origin, string sizes, string unit, double purchase_margin);
        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);

        StringBuilder UpdateOffer(string product_origin, string origin_origin, string sizes_origin, string unit_origin, string product, string origin, string sizes, string unit, bool weight_calculate);
        StringBuilder UpdatePendding(string product_origin, string origin_origin, string sizes_origin, string unit_origin, string product, string origin, string sizes, string unit, bool weight_calculate);
        DataTable GetProductAndPending(string product, string origin, string sizes, string seaover_unit);
    }
}
