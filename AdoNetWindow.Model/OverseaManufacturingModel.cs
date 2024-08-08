using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class OverseaManufacturingModel
    {
        public string id { get; set; }
        public string division { get; set; }
        public string importer { get; set; }
        public string pname_kor { get; set; }
        public string pname_eng { get; set; }
        public string product_type { get; set; }
        public string manufacturing { get; set; }
        public string im_date { get; set; }
        public string until_date { get; set; }
        public string m_country { get; set; }
        public string e_country { get; set; }
        public string frozen_num { get; set; }
        public string lot_num { get; set; }
        public string updatetime { get; set; }
        public string last_edit_user_name { get; set; }
        public string group_id { get; set; }
        public string remark { get; set; }
    }
}
