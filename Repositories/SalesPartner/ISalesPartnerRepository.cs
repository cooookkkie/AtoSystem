using AdoNetWindow.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SalesPartner
{
    public interface ISalesPartnerRepository
    {
        StringBuilder DeletePartnerSales(int company_id);
        DataTable GetCurrentCompanySales();
        DataTable GetUpdateList(string company_id);
        StringBuilder UpdateSalesInfo(CompanySalesModel model);
        DataTable GetSaleInfo(string sttdate, string enddate, string division, string company, bool isExactly, string edit_user, string company_id = "");
        DataTable GetSalesSumary(int sYear, string department, string edit_user);
        StringBuilder InsertPartnerSales(CompanySalesModel cm);
        DataTable GetSalesPartner(string group_name, string company, bool isExactly, string tel, string registration_number, string ceo, string manager
                                    , bool is_all_potential = true, bool is_potential1 = false, bool is_potential2 = false
                                    , bool is_all_out = false, bool is_non_handled = false, bool is_out_business = false, bool is_not_send_fax = false
                                    , bool isHide = false, string cerrentSaleDate = "", string distribution = "", string handling_item = "", string remark2 = "");
        DataTable GetSalesPartner2(string group_name, string company, bool isExactly, string tel, string registration_number, string ceo, string manager
                                    , bool is_all_potential = true, bool is_potential1 = false, bool is_potential2 = false
                                    , bool is_all_out = false, bool is_non_handled = false, bool is_out_business = false, bool is_not_send_fax = false
                                    , bool isHide = false, string currentSaleDate = "", string distribution = "", string handling_item = "", string remark2 = "");

        DataTable GetSalesPartner3(string group_name, string company, bool isExactly, string tel, string registration_number, string ceo, string manager
                                    , bool is_all_potential = true, bool is_potential1 = false, bool is_potential2 = false
                                    , bool is_all_out = false, bool is_non_handled = false, bool is_out_business = false, bool is_not_send_fax = false
                                    , bool isHide = false, string currentSaleDate = "", string distribution = "", string handling_item = "", string remark2 = "");
        DataTable GetUserSaleDashboard(string sttdate, string enddate, string user_name);
    }
}
