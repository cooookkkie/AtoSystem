using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public interface ICompanyFinanceRepository
    {
        DataTable GetCompanyFinance(string company_id);
        StringBuilder InsertCompanyFinanc(CompanyFinanceModel model);
        StringBuilder DeleteCompanyFinanc(string company_id, string year = "");
    }
}
