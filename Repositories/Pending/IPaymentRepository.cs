using AdoNetWindow.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Pending
{
    public interface IPaymentRepository
    {
        StringBuilder InsertSql(PaymentModel model);
        StringBuilder DeleteSql(PaymentModel model);
        DataTable GetPayment(string id, string contract_id, string division, string pay_date);
        DataTable GetPaymentAsOne(string id);
        int UpdateStatus(string id, string status, string payment_date);

    }
}
