using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class CompanyGroupModel
    {
        public int main_id { get; set; }
        public int sub_id { get; set; }
        public string company_id { get; set; }
        public string company { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }
}
