using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class ContractRecommendationModel
    {
        public string product { get; set; }
        public string origin { get; set; }
        public string division { get; set; }
        public int month { get; set; }
        public int recommend_level { get; set; }
        public string remark { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }
}
