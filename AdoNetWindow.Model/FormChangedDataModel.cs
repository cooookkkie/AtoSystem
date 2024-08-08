using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class FormChangedDataModel
    {
        public int id { get; set; }
        public string column_name { get; set; }
        public string column_code { get; set; }
        public string origin_text { get; set; }
        public string changed_text { get; set; }
    }
}
