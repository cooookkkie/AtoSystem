using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RecoveryPrincipal
{
    public interface IRecoveryPrincipalPriceRepository
    {
        DataTable GetCompanyBalance(DateTime sttdate, DateTime enddate);
        DataTable GetCompanyRecovery(DateTime sttdate, DateTime enddate, string company, string manager, bool isExistBalance, bool isLitigation);
        DataTable GetRecoveryPrincipalTemp(DateTime sttdate, DateTime enddate, string company, bool isExistBalance = false, bool out_business = false);
        DataTable GetRecoveryINfo(string company_code);
        void SetSeaoverId(string user_id);
        int CallStoredProcedure(string user_id, string sttdate, string enddate, string company = "");
        DataTable GetRecoveryPrincipal(int year, string company);
        StringBuilder DeleteRecoveryPrincipal(int year, string company);
        StringBuilder InsertRecoveryPrincipal(RecoveryPrincipalModel cm);
        int UpdateTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        Int32 GetPreAvgBalance(int year, string company);
        DataTable GetCompanyList(DateTime now_date, string company, string manager, double capital1, double capital2, double capital3, double capital4, double profit, int division);
        DataTable GetCompanyList2(DateTime now_date, string company, string manager, double capital1, double capital2, double capital3, double capital4, double profit, int division, bool out_business = false);
        DataTable GetRecoveryDetail(DateTime sdate, int accounts_receivable_terms, string company_code, string company);
        DataTable GetCompanyList(string company);
    }
}
