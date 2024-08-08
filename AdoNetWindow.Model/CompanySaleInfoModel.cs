using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class CompanySaleInfoModel
    {
        public int company_id { get; set; }
        public int sub_id { get; set; }
        public string distribution { get; set; }
        public string handling_item { get; set; }
        public bool isPotential1 { get; set; }
        public bool isPotential2 { get; set; }
        public bool isNonHandled { get; set; }
        public bool isOutBusiness { get; set; }
        public bool isNotSendFax { get; set; }
        public string updatetimne { get; set; }
        public string edit_user { get; set; }
    }
}
