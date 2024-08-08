using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SaleProduct
{
    public interface IProductExcludedSalesRepository
    {
        DataTable GetExcludedSalesByMonth(string sttdate, string enddate, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit, string sub_product, bool isMerge);
        DataTable GetExcludedSales(string sttdate, string enddate, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit);
        DataTable GetExcludedSalesAsOne(string sttdate, string enddate, string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit);
        StringBuilder DeleteExcludedSales(int id);
        StringBuilder InsertExcludedSales(ProductExcludedSalesModel model);
    }
}
