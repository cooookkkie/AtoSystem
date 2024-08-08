using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class InspectionModel
    {
        public int id { get; set; }
        public int sub_id{ get; set; }
        public string warehousing_date { get; set; }
        public int warehousing_date_score { get; set; }
        public string warehouse { get; set; }
        public string origin { get; set; }
        public string product { get; set; }
        public string sizes { get; set; }
        public string box_weight { get; set; }
        public double quantity_on_paper { get; set; }
        public string inspection_date { get; set; }
        public string inspection_results { get; set; }
        public string inspection_manager { get; set; }
        public string edit_user { get; set; }
        public string edit_date { get; set; }
    }

    public class InspectionInfoModel
    {
        public int id { get; set; }
        public int sub_id { get; set; }
        public int inspection_cnt { get; set; }
        public string inspection_date { get; set; }
        public string status { get; set; }
        public string remark { get; set; }
        public string manager { get; set; }
        public string edit_user { get; set; }
        public string updatetime { get; set; }
    }
}
