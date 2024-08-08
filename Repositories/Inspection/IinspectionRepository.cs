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
    public interface IinspectionRepository
    {
        DataTable GetInspectinoList(string etdSttdate, string etdEnddate, string etaSttdate, string etaEnddate, string inSttdate, string inEnddate
                                        , string ato_no, string bl_no, string warehouse, string cc_status, string income_manger
                                        , string product, string origin, string sizes, string unit, string inspection_manager, string inspection_status
                                        , int id = 0, int sub_id = 0);
        List<InspectionModel> GetInspection(DateTime sttdate, DateTime enddate, string warehouse, string origin, string product, string sizes, string manager);
        int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        StringBuilder DeleteInspection(InspectionInfoModel model);
        StringBuilder InsertInspection(InspectionInfoModel model);
        List<InspectionModel> GetInspectionSchedule(DateTime sttdate, DateTime enddate, string origin, string product, string sizes, string manager);
    }
}
