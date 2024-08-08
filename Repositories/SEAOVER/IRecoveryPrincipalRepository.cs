using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IRecoveryPrincipalRepository
    {
        int CallStoredProcedure(string user_id, string sttdate, string enddate, string company);
        DataTable GetRecoerPrincipal(string sYearMonth,string company);
        DataTable GetPreSalesPrice(int syear, string company);
        Int32 GetPreAvgBalacePrice(string company);
        
    }
}
