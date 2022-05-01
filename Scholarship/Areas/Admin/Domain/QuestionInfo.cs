using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scholarship.Areas.Admin.Domain
{
    public class QuestionInfo
    {
        public int? Standard { get; set; }
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
    }
}