using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetWindow.Model
{
    public class LoanModel
    {
        public string bank { get; set; }
        public string division { get; set; }
        public double usance_loan_limit { get; set; }
        public double atsight_loan_limit { get; set; }
    }
    public class LoanInfo
    {
        public string bank { get; set; }
        public string division { get; set; }
        public double usance_loan_limit { get; set; }
        public double usance_loan_used { get; set; }
        public double atsight_loan_limit { get; set; }
        public double atsight_loan_used { get; set; }
    }
}
