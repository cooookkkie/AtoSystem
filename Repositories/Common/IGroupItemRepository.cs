using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Common
{
    public interface IGroupItemRepository
    {
        StringBuilder InsertSql(GroupItemModel model);
        StringBuilder DeleteSql(GroupItemModel model);
        StringBuilder DeleteItemSql(GroupItemModel model);
        DataTable GetGroup(string id, string sub_id, string division, string manager);
        DataTable GetItem(string id, string sub_id, string division, string group_name);
    }
}
