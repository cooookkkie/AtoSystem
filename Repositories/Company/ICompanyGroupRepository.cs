using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public interface ICompanyGroupRepository
    {
        StringBuilder InsertCompanyGroup(CompanyGroupModel model);
        StringBuilder DeleteCompanyGroup(string company_id);
        StringBuilder DeleteGroup(int main_id);
        DataTable GetCompanyGroup();
        DataTable GetCompanyGroup2();
    }
}
