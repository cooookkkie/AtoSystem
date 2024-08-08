using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Domestic
{
    public interface IDomesticRepository
    {
        StringBuilder DeleteDomestic(DomesticModel cm);
        StringBuilder InsertDomestic(DomesticModel cm);
        StringBuilder InsertDomesticExpense(DomesticExpenseModel cm);
        StringBuilder DeleteDomesticExpense(DomesticExpenseModel cm);

        DataTable GetDomestic(string domestic_id);
        DataTable GetDomesticExpense(string domestic_id);

    }
}
