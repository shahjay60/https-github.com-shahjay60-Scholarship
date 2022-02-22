using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scholarship.Areas.Student
{
    public class StudentLoginModel
    {
        public int StdId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}