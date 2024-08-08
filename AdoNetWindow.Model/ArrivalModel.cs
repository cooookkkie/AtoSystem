using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ArrivalModel
    {
        public string id { get; set; }
        public string forwarder { get; set; }
        public string bl_status { get; set; }
        public string remark { get; set; }
        public string receipt_document { get; set; }
        public string quarantine_type { get; set; }
        public string result_estimated_date { get; set; }
        public string edit_user { get; set; }
        public string edit_date { get; set; }

    }
}
