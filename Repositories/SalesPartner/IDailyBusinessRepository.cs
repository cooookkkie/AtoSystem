using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SalesPartner
{
    public interface IDailyBusinessRepository
    {
        DataTable GetTempList(string user_id, int document_id);
        DataTable GetDocumentList(string user_id, string document_name);
        DataTable GetCompanyInquireText(string user_id, string company);
        DataTable GetDailyBusiness(string user_id, string sttdate, string enddate, string company, string product, int document_id = 0);
        StringBuilder InsertData(DailyBusinessModel model);
        StringBuilder DeleteData(string user_id, int document_id, string sttdate, string enddate);
        StringBuilder InsertTempData(DailyBusinessModel model);
        StringBuilder DeleteTempData(string userid, int document_id, int id);
        DataTable GetTempData(string user_id, int document_id, int id);
    }
}
