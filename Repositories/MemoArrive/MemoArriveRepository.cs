using AdoNetWindow.Model;
using Libs;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class MemoArriveRepository : ClassRoot, IMemoArriveRepository
    {
        public List<MemoArriveModel> GetArriveModel(int id, int sub_id)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"SELECT                       ");
            qry.Append($"\n   *                       ");
            qry.Append($"\n FROM t_memo_arrive           ");
            qry.Append($"\n WHERE id = {id}              ");
            qry.Append($"\n  AND sub_id = {sub_id}      ");

            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            MySqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            return ArriveModelInfo(dr);
        }
        private List<MemoArriveModel> ArriveModelInfo(MySqlDataReader rd)
        {
            List<MemoArriveModel> list = new List<MemoArriveModel>();
            while (rd.Read())
            {
                MemoArriveModel model= new MemoArriveModel();

                model.id = Convert.ToInt32(rd["id"]);
                model.sub_id= Convert.ToInt32(rd["sub_id"]);
                model.content = rd["content"].ToString();
                model.edit_name = rd["edit_name"].ToString();
                model.edit_date = rd["edit_date"].ToString();
                list.Add(model);

            }
            rd.Close();
            return list;
        }

        public int InsertMemo(MemoArriveModel model)
        {
            //ID가 0인 경우 새ID
            StringBuilder qry = new StringBuilder();
            qry.Append($" INSERT INTO t_memo_arrive (      ");
            qry.Append($"\n   id                           ");
            qry.Append($"\n , sub_id                       ");
            qry.Append($"\n , content                      ");
            qry.Append($"\n , edit_name                    ");
            qry.Append($"\n , edit_date                    ");
            qry.Append($"\n ) VALUES (                     ");
            qry.Append($"\n    {model.id}                  ");
            qry.Append($"\n ,  {model.sub_id}              ");
            qry.Append($"\n , '{model.content}'            ");
            qry.Append($"\n , '{model.edit_name}'          ");
            qry.Append($"\n , '{model.edit_date}'          ");
            qry.Append($"\n )                              ");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }

        public int DeleteMemo(int id, int sub_id, string content, string edit_name)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($" DELETE FROM t_memo_arrive        ");
            qry.Append($"\n  WHERE 1=1                     ");
            qry.Append($"\n    AND id = {id}               ");
            qry.Append($"\n    AND sub_id = {sub_id}       ");
            qry.Append($"\n    AND content = '{content}'   ");
            qry.Append($"\n    AND edit_name = '{edit_name}'");
            MySqlCommand command = new MySqlCommand(qry.ToString(), (MySqlConnection)dbInstance.Connection);
            return command.ExecuteNonQuery();
        }
    }
}
