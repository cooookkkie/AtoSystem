using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class AuthorityModel
    {
        public string department { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string group_name { get; set; }
        public string form_name { get; set; }
        public bool is_visible { get; set; }
        public bool is_add { get; set; }
        public bool is_update { get; set; }
        public bool is_delete { get; set; }
        public bool is_excel { get; set; }
        public bool is_print { get; set; }
        public bool is_admin { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
        public bool is_individual { get; set; }
    }
}
