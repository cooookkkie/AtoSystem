using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SEAOVER
{
    public interface ISeaoverCompanyRepository
    {
        int GetBusinessMonths(string company_code);
        DataTable GetSeaoverCompanyInfo(string company_name, string company_code);
        DataTable GetSeaoverCompanyInfo(string company_name, string company_code, string registration_number, string ceo, string manager);
    }
}
