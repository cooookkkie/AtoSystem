using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class PriceComparisonTempSettingModel
    {
        public string product_code { get; set; }
        public bool is_hide { get; set; }
        public string confirmation_date { get; set; }
        public string contents { get; set; }
        public string remark { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
    }
}
