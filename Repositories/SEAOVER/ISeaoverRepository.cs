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
    public interface ISeaoverRepository
    {
        DataTable SelectData(string db_name, string col, string user_id, string[] whrCol, string[] whrVal, string ord);
        DataTable GetAllDataOrderBy(string sttdate, string enddate, string product, string origin);
        DataTable GetAllData(string product, string origin, string sizes, string unit, string seaover_unit, string manager1, string manager2, string division);
        DataTable GetAllData(string sttdate, string enddate, string productList);
        DataTable SelectData(string db_name, string col, string user_id, string whr, string ord);
        DataTable GetProductInfo(string sttdate, string enddate, string division, string product, string origin, string sizes, string seaover_unit, string warehouse, string purchase_company, bool is_exactly = true);
        DataTable GetProductByCode(string product, string origin, string sizes, string unit);
        DataTable GetAllData(string sttdate, string enddate, string product, string origin, string sizes, string seaover_unit, bool is_exactly = true);
        DataTable GetProductCode(string product, string origin, string sizes);
        DataTable GetMarketPrice(string product, string origin, string sizes, string unit, string division, string manager1, string manager2);
        DataTable GetStockAndSalesDetail(string product, string origin, string sizes, string unit, string sales_term, bool isIntegrateUnit = false);
        DataTable GetStockAndSales(string product, string origin, string sizes, string unit, string sales_term);
        int UpdateProduct(string id, string sub_id, string product, string origin, string sizes, string box_weight, string cost_unit);
        DataTable GetCurrentMinPriceProduct(string categoryCode, string category, string product, string origin, string sizes, string unit, string division, string manager, int avgCnt);
        DataTable GetOneColumn(string colName, string whr, string val);
        List<SeaoverProductModel> GetProductForCompany(string product = null, string origin = null);
        List<SeaoverProductModel> GetProductAsOneGroupby(string product = null, string origin = null, string sizes = null, string unit = null);
        List<SeaoverProductModel> GetProductAsOne(string product = null, string origin = null, string sizes = null, string unit = null);
        int CallStoredProcedure(string user_id, string sttdate, string enddate);
        DataTable GetCurrentProduct(string category_code, string category, string product, string origin, string sizes, string unit, string division, string manager, string warehouse
                                , bool chkDiv, bool chkPrice, bool overAveragePrice, string isVat, bool isStock, int increaseSalePrice);
        DataTable GetProduct(string category_code, string category, string product, string origin, string sizes, string unit, string division, string manager, bool chkDiv, bool chkPrice);
        List<SeaoverProductModel> GetProductSimple(string product, string origin, string sizes);
        DataTable GetProductTapble(string category = null, string product = null, string origin = null, string sizes = null, string unit = null, string manager1 = null, string manager2 = null, string division = null);
        DataTable GetProductTable();
        DataTable GetProductTapble2();
        DataTable GetProductTapble3();
        DataTable GetProductTapble4();
        DataTable GetProductTable2(string product = null, bool isExactly = false, string origin = null, string sizes = null, string unit = null);
        DataTable GetProductTable3(string product = null, string origin = null, int maxCount = 100, bool isExactly = false);
        DataTable GetPriceForCostAccount(string sttdate, string enddate, string product, string origin, string sizes, string unit);
        DataTable GetPriceForCostAccount2(string sttdate, string enddate, string product, string origin, string sizes, string unit);
        DataTable GetCostCalculateForPurchasePrice(string sttdate, string enddate, string product, string origin, string sizes, string unit);
        List<FormDataTitle> GetFormDataTitle(int formType, string id, string formname, string manager);
        List<FormDataModel> GetFormData(string form_type, string id);
        List<FormDataModel> GetFormDataTemp(string form_type, string form_id, string temp_id);
        StringBuilder AllDataUpdateExlude(FormDataModel model);
        StringBuilder InsertFormData(FormDataModel model);
        StringBuilder InsertFormData2(FormDataModel model);
        StringBuilder DeleteFormData(string id);
        StringBuilder UpdateFormDataTemp(int temp_id, int id);
        StringBuilder DeleteFormDataTemp(string id);
        StringBuilder InsertFormDataTemp(FormDataModel model);
        DataTable GetFormDataTemp(int id);
        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        int UpdateExculde(FormDataModel model);
        int getNextId();
        DataTable GetProductPriceInfo(string product = null, string origin = null, string sizes = null, string unit = null);
        DataTable GetStockAndSalesDetail2(string product, string origin, string sizes, string unit, string sales_term, string sub_product, bool isMerge);

        int UpdateFormDataRock(string id, bool is_rock, string password);
    }
}
