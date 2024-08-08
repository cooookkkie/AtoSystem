using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IMemoArriveRepository
    {
        List<MemoArriveModel> GetArriveModel(int id, int sub_id);
        int InsertMemo(MemoArriveModel model);
        int DeleteMemo(int id, int sub_id, string content, string edit_name);
    }
}


