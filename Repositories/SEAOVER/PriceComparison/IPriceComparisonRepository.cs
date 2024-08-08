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
    public interface IPriceComparisonRepository
    {
        DataTable GetNormalPriceList(DateTime sttDate, DateTime endDate, int company_count, string product, string origin, string sizes);
        DataTable GetSumaryProductAllTerms(DateTime eDate, string product, string origin, string sizes);
        void SetSeaoverId(string user_id);
        DataTable GetSoldOutProduct();
        int CallStoredProcedure(string user_id, string sttdate, string enddate);
        int CallStoredProcedureSTOCK(string user_id, string enddate);
        string GetAveragePurchasePriceDetail(string product, string origin, string sizes, string unit, string price_unit, string unit_count, string seaover_unit
                                            , DateTime editSttdate, DateTime editEnddate, int company_cnt = 3);
        //Getdata
        DataTable GetPriceComparisonDataTable(DateTime sDate, int avgCnt, string category, string product, string origin, string sizes, string unit, string manager1, string manager2
                                            , string division, DateTime editSttdate, DateTime editEnddate
                                            , bool isAdvanced = false
                                            , bool isPurchasePrice = false, int sttPrice = 0, int endPrice = 0
                                            , bool isSalesPrice = false, int salesType = 0, int sales_term = 6, int sortType = 1
                                            , DataTable productDt = null, bool isAllStock = true);
        DataTable GetReservedDetail(string product, string origin, string sizes, string unit);
        DataTable GetProductCostComparison(string product, string origin, string sizes, string unit, string division, DateTime editSttdate, DateTime editEnddate, int avgCnt, int salesMonth);

        List<PriceComparisonModel> GetPriceComparison(int avgCnt, string txtCategory, string txtProduct, string txtOrigin, string txtSizes
                                                    , string txtUnit, string txtManager1, string txtManager2t, string division
                                                    , bool stock, bool saels, DateTime editSttdate, DateTime editEnddate);

        PriceComparisonModel GetPriceComparisonAsOne(string txtCategory, string txtProduct, string txtOrigin, string txtSizes
                                                    , string txtUnit, double sales_price);

        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        DataTable GetCostAccountingProductInfo(string product, string origin, string sizes, int sales_terms = 6);
        DataTable GetNotSalesCostProduct(string product, string origin, string sizes, string unit, string[] sub_product = null);
        DataTable GetNotSalesCostProduct2(string product, string origin, string sizes, string unit, string[] sub_product = null);

        //Temp PriceComparison
        DataTable GetTempPriceComparison(DateTime sdate, string product, string origin, string sizes, string unit, string manager, string division, bool isNoStock
            , string operating = "", string contract = "", int sortType = 1, int tabType = 1, bool isUserSetting = false, bool isContract = false, int sales_terms = 45, bool isAllData = false);




        //업체별시세현황2
        DataTable GetPriceComparisonDataTable2(string product, string origin, string sizes, string unit, string division, int avgCnt, DateTime editSttdate, DateTime editEnddate);

    }
}
