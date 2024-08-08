using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.OverseaManufacturing
{
    public interface IOverseaManufacturingRepository
    {
        DataTable GetDistinctData(string col);
        DataTable GetData(string sttdate, string enddate, string product_kor, string product_eng, string search_type, string company, bool isExactly);
        DataTable GetProduct(string pname_kor, string pname_eng);
        DataTable GetCompany(string col, string company);
        DataTable GetUploadData(string sttdate, string enddate);
        DataTable GetHandlingProduct(int type, List<string> companyDic, string sortType, string sttdate, string enddate, string product_kor = "", string product_eng = "");
        DataTable GetLastImdate(string division = "");
        DataTable GetDetail(int search_type, int id, string sttdate, string enddate, string division
                            , string product_kor, string product_kor2, string product_eng, string product_eng2
                            , string manufacturing, bool isExactly1, string income_company, bool isExactly2
                            , string manufacture_country, string export_country);
        DataTable GetData(int search_type, string sttdate, string enddate, string division
                            , string product_kor, string product_kor2, string except1
                            , string product_eng, string product_eng2, string except2
                            , string manufacturing, bool isExactly1, string income_company, bool isExactly2
                            , string manufacture_country, string export_country, string product_type);

        StringBuilder InsertSql(OverseaManufacturingModel model);
    }
}
