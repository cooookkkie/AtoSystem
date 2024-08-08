using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class DailyBusinessModel
    {
        public string user_id { get; set; }
        public int document_id { get; set; }
        public int sub_id { get; set; }
        public int id { get; set; }
        public string document_name { get; set; }
        public string input_date { get; set; }
        public string company { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public string sizes { get; set; }
        public string unit { get; set; }
        public string qty { get; set; }
        public double sale_price { get; set; }
        public string warehouse { get; set; }
        public string purchase_company { get; set; }
        public double purchase_price { get; set; }
        public string freight { get; set; }
        public string remark { get; set; }
        public string inquire { get; set; }
        public string updatetime { get; set; }
        public string createtime { get; set; }
        public string cell_style { get; set; }
    }
}
