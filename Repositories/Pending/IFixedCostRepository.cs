using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IFixedCostRepository
    {
        List<FixedCostModel> GetFixedCost(string contract_year, string ato_no, string contract_no, string shipper, string manager, IDbTransaction transaction = null);

    }
}


