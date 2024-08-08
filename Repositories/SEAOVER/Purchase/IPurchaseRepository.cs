using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SEAOVER.Purchase
{
    public interface IPurchaseRepository
    {
        void SetSeaoverId(string user_id);
        int CallStoredProcedure(string user_id, string sttdate, string enddate);
        DataTable GetPurchaseProductList(string sttdate, string enddate, string product, string origin, string sizes, string unit);
        DataTable GetPurchaseProduct(string sttdate, string enddate, string product, string origin, string sizes, string unit, string purchase_type, string product_code = "", string sizes_code = "", string select_unit = "");
        DataTable GetExchangeRate(string sttdate, string enddate);
        DataTable GetPurchase(string sttdate, string enddate, string product, string origin, string sizes, string unit);
        DataTable GetPurchaseProductForDashboard(string sttdate, string enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge, string purchase_type);
        DataTable GetPurchaseIncomProduct(string product, string origin, string sizes, string unit);
    }
}
