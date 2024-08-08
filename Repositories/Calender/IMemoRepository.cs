using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IMemoRepository
    {
        List<MemoModel> GetMemo( int year, int month, int id = 0, bool dm = false, IDbTransaction transaction = null);
        List<MemoModel> GetTradeMemo(DateTime sttdate, DateTime enddate, int id = 0, bool dm = false, string manager = "", string contents = "", IDbTransaction transaction = null);
        List<MemoModel> GetTradeMemo2(DateTime enddate, int id = 0, bool dm = false, string manager = "", IDbTransaction transaction = null);
        List<MemoModel> GetSaleMemo(DateTime sttdate, DateTime enddate, int id = 0, bool dm = false, IDbTransaction transaction = null);

        List<MemoModel> GetMemoDay(int year, int month, int days, IDbTransaction transaction = null);
        MemoModel GetMemoAsOne(int id = 0, IDbTransaction transaction = null);
        int AddMemo (MemoModel model, IDbTransaction transaction = null);
        int UpdateMemo(MemoModel model, IDbTransaction transaction = null);
        int PaymentMemo(int id, IDbTransaction transaction = null);
        int DeleteMemo(int id, IDbTransaction transaction = null);
        int GetNextId(IDbTransaction transaction = null);
    }
}


