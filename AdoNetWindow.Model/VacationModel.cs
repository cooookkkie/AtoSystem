using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class VacationModel
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string vacation_type { get; set; }
        public string sttdate { get; set; }
        public string enddate { get; set; }
        public double used_days { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }


    public class VacationAgreementModel
    {
        public int year { get; set; }
        public int month { get; set; }
        public bool is_agreement { get; set; }
        public string user_id { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }

}
