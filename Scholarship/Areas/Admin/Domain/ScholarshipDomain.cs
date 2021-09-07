using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scholarship.Areas.Admin.Domain
{
    public class ScholarshipDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int Amount { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> ExamDate { get; set; }
        public string DisplayDatetime { get; set; }
    }
}