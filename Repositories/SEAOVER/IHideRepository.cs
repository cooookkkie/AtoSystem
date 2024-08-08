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
    public interface IHideRepository
    {
        List<HideModel> GetHide(string category, string product, string origin, string sizes, string unit);
        DataTable GetHideTable(string enddate = "", string category = "", string product = "", string origin = "", string sizes = "", string unit = "", string division = "");
        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        StringBuilder InsertHide(HideModel hm);
        StringBuilder UpdateHide(HideModel hm);
        StringBuilder DeleteHide(int id);
    }
}
