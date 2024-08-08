using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class HandlingProductModel
    {
        public int cid { get; set; }
        public string product { get; set; }
        public string origin { get; set; }
        public double custom { get; set; }
        public double tax { get; set; }
        public double incidental_expense { get; set; }
        public string updatetime { get; set; }
        public string edit_user { get; set; }
        public string company { get; set; }

    }
}
