using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface ILoanRepository
    {
        List<LoanModel> GetLoanList(string bank = null);
        List<LoanInfo> GetCurrentLoan();
        StringBuilder DeleteLoan(string bank, string division);
        StringBuilder InsertLoan(string bank, string division, string usance_loan_limit, string atsight_loan_limit);
        int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
    }
}
