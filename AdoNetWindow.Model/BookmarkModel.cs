using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class BookmarkModel
    {
        public string user_id { get; set; }
        public int form_type { get; set; }
        public int form_id { get; set; }
        public string form_name { get; set; }
        public string edit_user { get; set; }
        public bool is_notification { get; set; }
    }
}
