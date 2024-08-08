using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public interface ICompanySaleInfoRepository
    {
        StringBuilder InsertCompany(CompanySaleInfoModel model);
        StringBuilder DeleteCompany(string company_id, string sub_id = "");
    }
}
