using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ContractRecommendation
{
    public interface IContractRecommendationRepository
    {
        StringBuilder DeleteRecommend(string product, string origin);
        StringBuilder DeleteRecommendAsOne(ContractRecommendationModel cm);
        StringBuilder InsertRecommend(ContractRecommendationModel cm);
        int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        DataTable GetRecommend(string product, string origin, string period = "", string contract = "");
        DataTable GetRecommendGroupConcat(string product, string origin, string period = "", string contract = "");
        DataTable GetRecommendAsOne(string product, string origin);
        
    }
}
