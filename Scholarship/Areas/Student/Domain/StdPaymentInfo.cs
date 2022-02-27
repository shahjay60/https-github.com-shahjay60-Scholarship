using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scholarship.Areas.Student.Domain
{
    public class StdPaymentInfo
    {
        public string StudentName { get; set; }
        public string ScholarshipName { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}