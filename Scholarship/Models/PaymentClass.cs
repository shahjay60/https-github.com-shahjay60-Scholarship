using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scholarship.Models
{
    public class PaymentClass
    {
        public int StdId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string ScholarshipName { get; set; }
        public string ScholarshipDetail { get; set; }
        public int ScholarshipId { get; set; }
        public int ScholarshipAmount { get; set; }
    }
}