using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public interface ICompanyRecoveryRepository
    {
        DataTable GetCompanyRecoveryTemp(DateTime sdate);
        StringBuilder InsertCompany(CompanyRecoveryModel model);
        StringBuilder DeleteCompany(string seaover_code);
    }
}
