using AdoNetWindow.Model;
using Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Company
{
    public class CompanySaleInfoRepository : ClassRoot, ICompanySaleInfoRepository
    {
        public StringBuilder DeleteCompany(string company_id, string sub_id = "")
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"DELETE FROM t_company_sale_info WHERE company_id = {company_id}          ");
            if(!string.IsNullOrEmpty(sub_id))
                qry.Append($"  AND sub_id = {sub_id}          ");
            return qry;
        }

        public StringBuilder InsertCompany(CompanySaleInfoModel model)
        {
            StringBuilder qry = new StringBuilder();
            qry.Append($"INSERT INTO t_company_sale_info (          ");
            qry.Append($"  company_id                               ");
            qry.Append($", sub_id                                   ");
            qry.Append($", handling_item                            ");
            qry.Append($", isPotential1                             ");
            qry.Append($", isPotential2                             ");
            qry.Append($", isNonHandled                             ");
            qry.Append($", isOutBusiness                            ");
            qry.Append($", isNotSendFax                             ");
            qry.Append($", updatetimne                              ");
            qry.Append($", edit_user                                ");
            qry.Append($") VALUES (                                 ");
            qry.Append($"   {model.company_id   }                             ");
            qry.Append($",  {model.sub_id       }                             ");
            qry.Append($", '{model.handling_item}'                            ");
            qry.Append($", '{model.isPotential1 }'                            ");
            qry.Append($", '{model.isPotential2 }'                            ");
            qry.Append($", '{model.isNonHandled }'                            ");
            qry.Append($", '{model.isOutBusiness}'                            ");
            qry.Append($", '{model.isNotSendFax }'                            ");
            qry.Append($", '{model.updatetimne  }'                            ");
            qry.Append($", '{model.edit_user    }'                            ");

            return qry;
        }
    }
}
