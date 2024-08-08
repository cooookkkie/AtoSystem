using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class UsersModel
    {
        public string user_id { get; set; }
        public string user_password { get; set; }
        public string user_status { get; set; }
        public string user_name { get; set; }
        public string workplace { get; set; }
        public string department { get; set; }
        public string team { get; set; }
        public int auth_level { get; set; }
        public string unlimited_expiry_date { get; set; }
        public int limit_type { get; set; }
        public int limit_max_count { get; set; }
        public string tel { get; set; }
        public string grade { get; set; }
        public string seaover_id { get; set; }
        public string form_remark { get; set; }
        public string current_login_date { get; set; }
        public string user_in_date { get; set; }
        public string user_out_date { get; set; }
        public double target_sales_amount { get; set; }
        public string excel_password { get; set; }
        public int daily_work_goals_amount { get; set; }
    }

}
