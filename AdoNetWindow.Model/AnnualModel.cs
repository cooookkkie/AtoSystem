using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class AnnualModel
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string division { get; set; }
        public string workplace { get; set; }
        public string department { get; set; }
        public string grade { get; set; }
        public string user_name { get; set; }
        public string createtime { get; set; }
        public string sttdate { get; set; }
        public string enddate { get; set; }
        public float used_days { get; set; }
        public string agent { get; set; }
        public string remark { get; set; }
        public bool approval_manager { get; set; }
        public bool approval_deputy_general_manager { get; set; }
        public bool approval_general_manager { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
        public string status { get; set; }
    }
    public class AcruedAnnualModel
    {
        public string user_id { get; set; }
        public int year { get; set; }
        public double vacation { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
        
    }
}
