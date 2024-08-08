using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class GroupItemModel
    {
        public int id { get; set; }
        public int sub_id { get; set; }
        public string division { get; set; }
        public string group_name { get; set; }
        public string item_code { get; set; }
        public string manager { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
    }
}
