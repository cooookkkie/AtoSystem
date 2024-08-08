using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class MemoModel
    {
        public int id { get; set; }
        public string division { get; set; }
        public int syear { get; set; }
        public int smonth { get; set; }
        public int sday { get; set; }
        public string currency { get; set; }
        public string pay_bank { get; set; }
        public double? pay_amount { get; set; }
        public string pay_status { get; set; }
        public string contents { get; set; }
        public string manager { get; set; }
        public string updatetime { get; set; }
        public string backColor { get; set; }
        public string fontColor { get; set; }
        public string font { get; set; }
        public string font_size { get; set; }
        public string font_bold { get; set; }
        public string font_italic { get; set; }
        public int user_auth { get; set; }

        public static implicit operator List<object>(MemoModel v)
        {
            throw new NotImplementedException();
        }
    }
}
