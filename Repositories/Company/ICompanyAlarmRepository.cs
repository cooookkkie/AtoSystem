using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public interface ICompanyAlarmRepository
    {
        StringBuilder DeleteAlarm(string company_id);
        StringBuilder InsertAlarm(CompanyAlarmModel model);
        DataTable GetCompanyAlarm(int company_id = -1, string user_name = "", string complete_date = "");
        DataTable GetCompanyAlarmOneLine(string user_name = "", string sdate = "");
        DataTable GetCompanyAlarmForCalendar(string user_name = "");
    }
}
