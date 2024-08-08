using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Calender
{
    public interface IAnnualRepository
    {
        StringBuilder InsertAnnual(VacationModel vm);
        StringBuilder DeleteAnnual(string user_id, DateTime sttdate);
        DataTable GetAnnual(string user_id, string user_name, DateTime sttdate, DateTime enddate);
        StringBuilder DeleteAccruedAnnual(string user_id, int year);
        StringBuilder InsertAccruedAnnual(AcruedAnnualModel model);
        StringBuilder UpdateDailyTarget(string user_id, int daily_target);
        StringBuilder DeleteAgreementAnnual(string user_id, int year, int month);
        StringBuilder InsertAgreementAnnual(VacationAgreementModel model);
        bool GetAgreement(string user_id, int year, int month);
    }
}
