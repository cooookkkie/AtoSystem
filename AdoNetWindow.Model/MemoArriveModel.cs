using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class MemoArriveModel
    {
        public int id{ get; set; }
        public int sub_id { get; set; }
        public string content { get; set; }
        public string edit_name { get; set; }
        public string edit_date { get; set; }
    }
}
