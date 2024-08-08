using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IBookmarkRepository
    {
        StringBuilder SupperDeleteSql(int form_id);
        StringBuilder UpdateOrderSql(int form_id, int order_count);
        DataTable GetFormList(int form_type, string group_name, string form_name, string create_user, bool is_favorite = false, string user_id = "");
        List<BookmarkModel> GetBookmark(string user_id, int form_type, string formname, string mananger);
        int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        StringBuilder InsertSql(BookmarkModel model);
        StringBuilder DeleteSql(BookmarkModel model);
        DataTable GetFormData(int id);
    }
}
