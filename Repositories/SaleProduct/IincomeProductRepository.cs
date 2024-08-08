using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SaleProduct
{
    public interface IincomeProductRepository
    {
        DataTable GetIncomeProduct(string division, string product, string origin, string manager);
        StringBuilder InsertData(IncomeProductModel model);
        StringBuilder DeleteData(IncomeProductModel model);
    }
}
