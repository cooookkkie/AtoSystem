using Libs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Repositories.SEAOVER.Sales
{
    public interface ISalesRepository
    {
        DataTable GetSalesDashboardByCompany(DateTime sttdate, DateTime enddate, string user_name);
        DataTable GetSalesDashboardByProduct(DateTime sttdate, DateTime enddate, List<string> product, string user_name);
        DataTable GetSalesForSalesDashborad(string username, string product, string origin, string sizes);
        DataTable GetHandledItem(string company, string seaover_code = "");
        DataTable GetNoneHandlingProductDashboard(string product, string origin, string sizes, string unit, List<DataGridViewRow> except_product);
        DataTable GetsalesLedger(string company_code);
        DataTable GetSalesAmount(DateTime sttdate, DateTime enddate);
        DataTable GetSalesQty(int sales_terms, string sub_product);
        DataTable GetSalesAmountDashboard(string user_name, DateTime sdate, int saleTears, int sttYear, int sttMonth, int endYear, int endMonth);
        DataTable GetSalesForSalesDashborad(List<string> userList);
        DataTable GetHandlingProductDashboard(string company);
        DataTable GetCompanyList();
        DataTable GetCurrentSales(string sttdate, string enddate, string sale_company, string product, string origin, string sizes, string unit, string purchase_company);
        DataTable GetSales(string sttdate, string enddate, string sale_company, string product, string origin, string sizes, string unit, string purchase_company);
        DataTable GetIntegretionSaleQty(string product, string origin, string sizes, string user_code);
        DataTable GetHandlingItemDetail(string company_code);
        DataTable GetHandlingItem(string company_code);
        DataTable GetDuplicateSeaoverList();
        DataTable GetSales(string sttdate, string enddate, string product, string origin, string sizes, string unit, bool allUnit = false);
        DataTable GetAverageSalesByMonth(string product, string origin, string sizes, string unit, bool allUnit = false, string col_name = "매출수");
        DataTable GetSalesGroupMonth(DateTime sttdate, DateTime enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge, int terms_type = 1, bool isAtoSale = false);
        DataTable GetSalesGroupMonth(DateTime sttdate, DateTime enddate, int until_days, string product, string origin, string sizes, string unit, string sub_product, bool isMerge, bool isAtoSale = false);
        DataTable GetMonthSales(int months);
        void SetSeaoverId(string user_id);
        DataTable GetSalesProduct(string sttdate, string enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge, string sales_type);
        DataTable GetSaleCompany(string company, bool isExactly, string tel, string fax, string phone, string other_phone, string manager = ""
            , string sale_date = "", bool is_not_out_business = false, bool is_not_litigation = false, string seaover_code = "");
        DataTable GetAverageSalesByMonth2(DateTime eDate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge = false, string col_name = "매출수");
        DataTable GetBusinessCompanyForSalesDashborad(int sttYear, int endYear, List<string> userList);
    }
}
