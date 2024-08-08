using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class taskModel
    {
        public int id { get; set; }
        public string subject { get; set; }
        public string department { get; set; }
        public string remark { get; set; }
        public int task_id { get; set; }
        public string task_contents { get; set; }
        public string task_manager { get; set; }
        public string task_start_date { get; set; }
        public string task_end_date { get; set; }
        public string task_result { get; set; }
        public string create_user { get; set; }
        public string create_datetime { get; set; }
    }
}
