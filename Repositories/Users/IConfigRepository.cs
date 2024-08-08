using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IConfigRepository
    {
        ConfigModel GetConfigChecked(string user_id);
        int InsertConfig(string user_id, string whr, string val);
        int updateConfig(string user_id, string whr, string val);
        List<CountryModel> GetCountryConfig(string name = null);
        CountryModel GetCountryConfigAsOne(string name = null);
        int InsertCountry(string name, int days);
        int updateCountry(string name, int days);
        int DeleteCountry(string name);
        DataTable GetCountry(string country);
        DataTable GetCountryDelivery(string origin);
    }
}
