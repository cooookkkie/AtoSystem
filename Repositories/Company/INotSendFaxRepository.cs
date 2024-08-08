using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public interface INotSendFaxRepository
    {
        StringBuilder InsertCompany(NotSendFaxModel model);
        StringBuilder DeleteCompany(string fax);
        DataTable GetNotSendFax(string fax);
    }
}
