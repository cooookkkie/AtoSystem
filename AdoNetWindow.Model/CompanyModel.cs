using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class CompanyModel
    {
        public int id { get; set; }
        public string division { get; set; }
        public string registration_number { get; set; }
        public string group_name { get; set; }
        public string name { get; set; }
        public string origin { get; set; }
        public string address { get; set; }
        public string ceo { get; set; }
        public string fax { get; set; }
        public string tel { get; set; }
        public string phone { get; set; }
        public string other_phone { get; set; }
        public string company_manager { get; set; }
        public string company_manager_position { get; set; }
        public string email { get; set; }
        public string remark { get; set; }
        public string sns1 { get; set; }
        public string sns2 { get; set; }
        public string sns3 { get; set; }
        public string web { get; set; }
        public string ato_manager { get; set; }
        public string seaover_company_code { get; set; }
        public bool isManagement1 { get; set; }
        public bool isManagement2 { get; set; }
        public bool isManagement3 { get; set; }
        public bool isManagement4 { get; set; }
        public bool isHide { get; set; }
        public bool isDelete { get; set; }
        public string updatetime { get; set; }
        public string createtime { get; set; }
        public string edit_user{ get; set; }

        public bool isPotential1 { get; set; }
        public bool isPotential2 { get; set; }
        public bool isTrading { get; set; }
        public bool isNonHandled { get; set; }
        public bool isNotSendFax { get; set; }
        public bool isOutBusiness { get; set; }
        public string distribution { get; set; }
        public string handling_item { get; set; }
        public string remark2 { get; set; }
        public string remark3 { get; set; }
        public string remark4 { get; set; }
        public string remark5 { get; set; }
        public string remark6 { get; set; }
        public string payment_date { get; set; }
        public string sales_comment { get; set; }

        public int alarm_month { get; set; }
        public string alarm_week { get; set; }
        public string alarm_complete_date { get; set; }
        public string table_index { get; set; }
        public string industry_type { get; set; }
        public string industry_type2 { get; set; }
        public string category { get; set; }

    }
}
