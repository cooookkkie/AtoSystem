using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class PaymentModel
    {
        public int id { get; set; }
        public string division { get; set; }
        public int contract_id { get; set; }
        public string payment_date_status { get; set; }
        public string payment_date { get; set; }
        public string payment_currency { get; set; }
        public double payment_amount { get; set; }
        public string remark { get; set; }
        public string updatetime{ get; set; }
        public string edit_user { get; set; }
    }
}
