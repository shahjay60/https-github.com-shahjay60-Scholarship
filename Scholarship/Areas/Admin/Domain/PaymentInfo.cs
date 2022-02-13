using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scholarship.Areas.Admin.Domain
{
    public class PaymentInfo
    {
        public string StudentName { get; set; }
        public string ScholarshipName { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}