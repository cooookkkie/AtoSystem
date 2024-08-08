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
    public interface ICompanyRepository
    {
        DataTable GetSeaoverInfo(string company, string seaover_company_code);
        StringBuilder UpdateInsurance(string company_id, string r_num, string c_num, string ceo, string kc_number, double insurance_amount, string insurance_sttdate, string insurance_enddate);
        DataTable GetInsuranceCompany(string company, bool isExactly, string corporation_number, string ceo, int top_limit_count, bool isOnlyInsurance);
        StringBuilder UpdateCompanyColumns(string[] col, string[] val, string whr);
        StringBuilder UpdateAlarmCompany(int id, int alarm_month, int alarm_week, string alarm_date);
        StringBuilder UpdateCompany(int id, string registration_number, string ceo, string tel, string fax, string phone, string other_phone);
        StringBuilder UpdateAtoManger(int id, string ato_manager, string edit_user);
        DataTable GetCompanyAsOne(string name, string code = "", string id = "");
        CompanyModel GetCompanyAsOne2(string name, string code = "", string id = "");
        DataTable GetDuplicateList();
        StringBuilder UpdateCompany(CompanyModel model);
        StringBuilder InsertCompany(CompanyModel model);
        StringBuilder DeleteCompany(int id, bool isDelete = true);
        StringBuilder RealDeleteCompany(int id);
        DataTable GetCompany(string division, string group_name, string company, bool isExactly, string origin
                            , string sns1, string sns2, string sns3, string email, string ato_manager, string id = "");
        DataTable GetCompanyRecovery(string group_name, string company, bool isExactly, string ato_manager, string seaover_company_code = ""
                            , bool isManagement1 = false, bool isManagement2 = false, bool isManagement3 = false, bool isManagement4 = false, bool isHide = false);

        DataTable GetCompanySaleInfo(string group_name, string company, bool isExactly, string ato_manager, string seaover_company_code = ""
                                    , bool isPotential1 = false, bool isPotential2 = false, bool isNonHandled = false, bool isOutBusiness = false, bool isNotSendFax = false, bool isHide = false);


    }
}
