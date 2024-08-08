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
    public interface ICustomsRepository
    {
        StringBuilder UpdateShipmentSchdule(int id, string shipment_date, string etd, string eta);
        DataTable GetMissingETDPendingList(DateTime standard_date, string ato_no, string manager);
        DataTable GetCollateralAvailableProduct();
        int UpdateDataAsOne(string id, string sub_id, string col_name, string col_value);
        DataTable GetUnpendingProduct2(string product, string origin, string sizes, string unit, bool integrated_unit, bool isExactly, bool isMerge = false, string sub_product = "");
        DataTable GetDashboard(string sttdate, string enddate, string product, string origin, string sizes, string unit, string sub_product, bool isMerge);
        DataTable GetPendingList2(string stt_contract_year, string end_contract_year
                                           , string ato_no, string contract_no, string shipper, string bl_no
                                           , string product, bool isExactly, string origin, string sizes, string box_weight, string manager, string cc_status);
        DataTable GetNotSeaoverProduct();
        DataTable CheckHOCO(string sttdate, string enddate);
        DataTable GetGuarantee(string sttdate, string enddate, string ato_no, string bl_no, string lc_no);
        DataTable GetArrivalSchedule(string sttYear, string endYear, string division, string bl_no, string shipping_company, string bl_status, string ato_no, string broker, string warehouse, string agency_type, int type);
        DataTable SelectProduct(string product = null, string origin = null, string sizes = null, string unit = null);
        DataTable GetProductTable(string product = null, string origin = null, string sizes = null, string unit = null);
        List<AllCustomsModel> GetUnPending(string id, IDbTransaction transaction = null);
        List<AllCustomsModel> GetAllTypePending(string type, string id, bool isNew = false, IDbTransaction transaction = null);
        List<CustomsModel> GetAll(DateTime dt, IDbTransaction transaction = null);
        List<UncommfirmModel> GetUncomfirm(string cYear, string atoNo, string shipper, string origin, string product, string sizes, string manager, string ccStatus, string payStatus, IDbTransaction transaction = null);
        List<CustomsModel> GetCustomMonth(DateTime sttdate, DateTime enddate, string ato_no, string bl_no, string lc_no, string product, string sizes, string origin, string manager, IDbTransaction transaction = null);
        List<CustomsModel> GetCustomMonthTotal(DateTime sttdate, DateTime enddate, IDbTransaction transaction = null);
        List<CustomsModel> GetShipmentMonth(DateTime sttdate, DateTime enddate, string ato_no, string bl_no, string cl_no, string product, string origin, string sizes, string manager, IDbTransaction transaction = null);
        List<CustomsModel> GetCustomUnknown(DateTime sttdate, DateTime enddate, string ato_no, string bl_no, string lc_no, string product, string origin, string sizes, string manager, IDbTransaction transaction = null);
        List<CustomsSimpleModel> GetCustom(int id, IDbTransaction transaction = null);
        CustomsTitleModel GetCustomTitle(int id, IDbTransaction transaction = null);
        int UpdateCustom(int id, DateTime payment_date, string payment_date_status, IDbTransaction transaction = null);
        int UpdateHOCO(int id, string val);
        StringBuilder UpdateSql2(string id, string division, string bl_no, string eta, string broker, string warehouse, string warehousing_date, string agency, string sanitary_certificate);
        StringBuilder UpdateSql(UncommfirmModel model);
        int UpdateCustomTran(List<StringBuilder> sqlList, MySqlTransaction transaction = null);
        List<CustomsInfoModel> GetPending(string type, string sYear, string eYear, string atoNo, bool isStart, string contractNo, string shipper, string lcNo, string blNo, string import_number
                                        , string origin, string product, string sizes, string box_weight, string manager, string usance
                                        , string division, string agency, string payment, string cc_status, IDbTransaction transaction = null, int sortType = 1
                                        , int isDelete = 1);
        List<CustomsInfoModel> GetPendingInfo(string id, IDbTransaction transaction = null);
        DataTable GetFindData(string ato_no, string lc_no, string bl_no, IDbTransaction transaction = null);
        StringBuilder InsertSql2(AllCustomsModel model);
        StringBuilder DeleteSql(string id);
        StringBuilder UpdatePaymentComplete(string id, string payment_date);
        int GetNextId();
        int UpdateData(string id, string col_name, string col_value);
        StringBuilder UpdateDataString(string id, string col_name, string col_value);
        int UpdatePayment(string id, string status);
        int UpdatePaymentDate(string id, string paymentDate);
        int DelayPaymentDate(string id, string paymentDate);
        int DelayDate(string id, string col, string val);
        List<ArriveModel> GetArrive(DateTime sttdate, DateTime enddate, string product = null, string sizes = null, string origin = null, string manager = null, int id = 0, int sub_id = 0, IDbTransaction transaction = null);
        List<ArriveModel> GetArriveDetails(DateTime sttdate, DateTime enddate, string product = null, string sizes = null, string origin = null, string manager = null, int id = 0, int sub_id = 0, IDbTransaction transaction = null);
        StringBuilder UpdatePending(AllCustomsModel model);
        DataTable GetUnpendingProduct(string product, string origin, string sizes, string unit, bool integrated_unit, bool isExactly);
        DataTable GetUnpending(string product, string origin, string sizes, string unit, int shipDays = 0);
        int GetNextAtoNo(string ato_no);
        int GetCustomsId(string ato_no);
        List<CustomsModel> GetCustomUnknown2(DateTime enddate, string ato_no, string product, string origin, string sizes, string manager, IDbTransaction transaction = null);
        DataTable GetPrepayment();
        DataTable GetOfferPriceList(string sttdate, string enddate, string product, string origin, string sizes, string unit, string purchase_price = "", string updatetime = "");
        DataTable GetPendingPriceList(string sdate, string product, string origin, string sizes, string unit);
        DataTable GetPendingProductByAtono(string ato_no, string product, string origin, string sizes, string unit);
        DataTable GetPayInfo(string division);
        DataTable GetProductForNotSalesCost(string product, string origin, string sizes, string unit, DataTable productDt, string[] sub_product = null);
        DataTable GetProblemPending();
        StringBuilder UpdateStatus(int id, string status, string warehousing_date);
        DataTable GetUnpendingProduct3(string product, string origin, string sizes, string unit, string sub_product, bool isMerge, bool isExactly, bool isSimulation = false);
        DataTable GetDuplicateAtono(string contract_year, string ato_no, bool isDelete = false, string id = "");
        DataTable GetShippingStockList();
    }
}


