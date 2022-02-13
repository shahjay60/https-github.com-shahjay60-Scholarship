using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scholarship.Models
{
    public class StudentLoginDomain
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? ScholarshipId { get; set; }

    }
}