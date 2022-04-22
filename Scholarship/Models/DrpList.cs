using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Scholarship.Models
{
    public class CustomModel
    {
        public List<DrpList> Examlist { get; set; }
        public int ExamId { get; set; }
    }
    public class DrpList
    {
        public string exam_title { get; set; }
        public int examid { get; set; }
        public int standard { get; set; }
        public string SchoarshipName { get; set; }
        public string StudentName { get; set; }

        public List<tblQuestion> Examlist { get; set; }
        public int QuestionNo { get; set; }
    }
}