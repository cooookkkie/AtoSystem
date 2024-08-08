using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Group
{
    public interface IGroupRepository
    {
        DataTable GetGroupList(string division, string group_name, string form_name);
        DataTable GetGroup(int id, string category = "", string product = "", string origin = "", string sizes = "", string unit = "", string manager1 = "", string manager2 = "", string manager3 = "");
        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        StringBuilder InsertGroup(GroupModel model);
        StringBuilder DeleteGroup(GroupModel model);
        StringBuilder DeleteGroup2(GroupModel model);
        StringBuilder DeleteGroup(int id);
        StringBuilder UpdateGroup(int id, string group, string name);
    }
}
