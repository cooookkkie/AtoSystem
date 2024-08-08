using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SEAOVER
{
    public interface IStockRepository
    {
        DataTable GetExistStock();
        int CallStoredProcedureSTOCK(string user_id, string enddate, bool isDash = false);
        DataTable GetDashboardDetail(string division, string ato_no = "");
        DataTable GetGuarnteeStock();
        DataTable GetStockDashboard();
        DataTable GetProductSales(DateTime sttdate , DateTime enddate, string product = "", string origin = "", string sizes = "", string unit = "");
        DataTable GetPendingStock();
        DataTable GetStockAndWarehouse(bool isInStock = true);
    }
}
