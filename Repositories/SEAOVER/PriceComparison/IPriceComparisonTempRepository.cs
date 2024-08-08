using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdoNetWindow.Model;

namespace Repositories.SEAOVER.PriceComparison
{
    public interface IPriceComparisonTempRepository
    {
        DataTable GetPrieComparisonTempData(DateTime standardDate, int isOutStockLimintDays = 0
                                            , string product = "", string origin = "", string sizes = "", string unit = "", string division = "", string manager = "", string contents = "", string remark = "");

        StringBuilder DeleteSettingData(string product_code);
        StringBuilder InsertSettingData(PriceComparisonTempSettingModel model);
    }
}
