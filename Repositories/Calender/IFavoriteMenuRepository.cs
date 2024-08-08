using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Calender
{
    public interface IFavoriteMenuRepository
    {
        List<FavoriteMenuModel> GetFavoriteMenu(string user_id);

        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);

        StringBuilder InsertSql(FavoriteMenuModel model);

        StringBuilder DeleteSql(string user_id, string category, string form_name);
    }
}
