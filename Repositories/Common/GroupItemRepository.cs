using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Common
{
    public class GroupItemRepository : ClassRoot, IGroupItemRepository
    {
        public StringBuilder DeleteSql(GroupItemModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE FROM t_group_item               ");
            qry.Append($"\n WHERE 1 = 1                       ");
            qry.Append($"\n   AND id = {model.id}             ");

            string sql = qry.ToString();

            return qry;
        }
        public StringBuilder DeleteItemSql(GroupItemModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n DELETE t_group_item               ");
            qry.Append($"\n WHERE 1 = 1                       ");
            qry.Append($"\n   AND id = {model.id}             ");
            qry.Append($"\n   AND sub_id = {model.sub_id}             ");

            string sql = qry.ToString();

            return qry;
        }
        public StringBuilder InsertSql(GroupItemModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\n INSERT INTO t_group_item (        ");
            qry.Append($"\n   id                              ");
            qry.Append($"\n , sub_id                          ");
            qry.Append($"\n , division                        ");
            qry.Append($"\n , group_name                      ");
            qry.Append($"\n , item_code                       ");
            qry.Append($"\n , manager                         ");
            qry.Append($"\n , updatetime                      ");
            qry.Append($"\n , edit_user                       ");
            qry.Append($"\n ) VALUES (                        ");
            qry.Append($"\n    {model.id}                     ");
            qry.Append($"\n ,  {model.sub_id}                 ");
            qry.Append($"\n , '{model.division}'              ");
            qry.Append($"\n , '{model.group_name}'            ");
            qry.Append($"\n , '{model.item_code}'             ");
            qry.Append($"\n , '{model.manager}'             ");
            qry.Append($"\n , '{model.updatetime}'            ");
            qry.Append($"\n , '{model.edit_user}'             ");
            qry.Append($"\n )                                 ");

            string sql = qry.ToString();

            return qry;
        }
        public DataTable GetGroup(string id, string sub_id, string division, string manager)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                             ");
            qry.Append($"\n  id                               ");
            qry.Append($"\n, division                         ");
            qry.Append($"\n, group_name                       ");
            qry.Append($"\n, manager                          ");
            qry.Append($"\nFROM t_group_item                  ");
            qry.Append($"\nWHERE 1 = 1                        ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n  AND id = {id}                    ");
            if (!string.IsNullOrEmpty(sub_id))
                qry.Append($"\n  AND sub_id = {sub_id}            ");
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n  AND division = '{division}'      ");
            if (!string.IsNullOrEmpty(manager))
                qry.Append($"\n  AND manager = '{manager}'    ");
            qry.Append($"\nGROUP BY id, division, group_name  ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
        public DataTable GetItem(string id, string sub_id, string division, string group_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"\nSELECT                             ");
            qry.Append($"\n  *                                ");
            qry.Append($"\nFROM t_group_item                  ");
            qry.Append($"\nWHERE 1 = 1                        ");
            if (!string.IsNullOrEmpty(id))
                qry.Append($"\n  AND id = {id}                    ");
            if (!string.IsNullOrEmpty(sub_id))
                qry.Append($"\n  AND sub_id = {sub_id}            ");
            if (!string.IsNullOrEmpty(division))
                qry.Append($"\n  AND division = '{division}'      ");
            if (!string.IsNullOrEmpty(group_name))
                qry.Append($"\n  AND group_name = '{group_name}'    ");

            string sql = qry.ToString();
            dbInstance.Connection.Close();
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable("SEAOVER");
            dt.Load(dr);
            dr.Close();

            return dt;
        }
    }
}
