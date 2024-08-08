using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Config
{
    public interface IAuthorityRepository
    {
        DataTable GetUserAuthority(string user_id);
        StringBuilder InsertAuthority(AuthorityModel model);
        StringBuilder DeleteAuthority(string user_id);
        DataTable GetAuthority(string user_id);
        DataTable GetAuthorityInfo(string department, string user_name);
        StringBuilder DeleteUsersAuthority(string user_id, bool is_individual);
    }
}
