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
    public interface IPurchasePriceRepository
    {
        
        DataTable GetMultiPurchaseDashborad(string warehouse, string product, string origin, string sizes, string unit, string manager);
        DataTable GetPurchaseShipperList(string company, string product = "", string origin = "", string sizes = "", string unit = "");
        StringBuilder UpdatePurchasePrice(PurchasePriceModel ppm);
        DataTable GetPurchaseDashborad(string sttdate, string enddate, List<string[]> productList);

        DataTable GetPurchasePriceList(string purchase_date, string product, string origin, string sizes, string unit, string id = "");
        DataTable GetPurchasePriceList(string sttdate, string enddate, string product, string origin, string sizes, string unit, string id = "");
        DataTable GetCurrentPrice();
        DataTable GetCurrentUpdatetime();
        int GetNextId();
        DataTable GetPurchasePriceById(string product, string origin, string sizes, string unit, string updatetime, string company, string unit_price);
        PurchasePriceModel GetPurchasePriceAsOne(string id);
        List<PurchasePriceModel> GetPurchasePriceForChart(string sttdate, string enddate, string product, string origin, string sizes, string unit, string company = "");
        DataTable GetPurchasePriceForChartMonth(string sttdate, string enddate, string product, string origin, string sizes, string unit, string company = "");
        DataTable GetPurchasePriceForChartDay(string sttdate, string enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge);
        DataTable GetCostAccounting(string sdate, string edate, string product, string origin, string sizes, string unit, string company, bool exactly, string manager, int sortType = 1, bool isExactly = false, bool isCurrent = false);
        DataTable GetCostAccounting2(string sdate, string edate, string product, string origin, string sizes, string unit, string company);
        DataTable GetCostAccounting3(string sdate, string product, string origin, string sizes, string unit);
        List<PurchasePriceModel> GetPurchasePrice(string sttdate, string enddate, string product, string origin, string sizes, string unit, string company, string manager);
        StringBuilder InsertPurchasePrice(PurchasePriceModel ppm);
        DataTable GetCurrentPurchasePrice(string product, string origin, string sizes, string unit);
        DataTable GetPurchasePrice(string sttdate, string enddate, string product, string origin, string sizes, string unit, string manager, string sdate = "", string price = "");
        StringBuilder DeletePurchasePrice2(PurchasePriceModel ppm);
        StringBuilder DeletePurchasePrice(string id);
        int UpdateTrans(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        DataTable GetTradeManaer(string product, string origin, string sizes, string unit, string manager, string operating = "");
        DataTable GetRankingPurchasePriceASOne(string sttdate, string enddate, string product, string origin, string sizes, string unit, string manager, double purchase_price);
        DataTable GetTradeManaerAsOne(string product, string origin, string sizes, string unit, double exchange_rate);
        DataTable GetPurchasePrice2(string sttdate, string enddate, string product, string origin, string sizes, string unit, string company, bool isExactly, string manager, string purchase_id = "");
        DataTable GetTradeManaer(string product, string origin, string manager);
        DataTable GetPurhcasePriceAverage(string product, string origin, string sizes, string unit, string sub_product, bool isMerge);
    }
}
