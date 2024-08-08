using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ConfigModel
    {
        public string user_id { get; set; }
        public bool shipment_checked { get; set; }
        public bool payment_checked { get; set; }
        public bool memo_checked { get; set; }
        public bool unknown_checked { get; set; }
        public bool salememo_checked { get; set; }
        public bool arrive_checked { get; set; }
        public bool inspection_checked { get; set; }
    }
}
