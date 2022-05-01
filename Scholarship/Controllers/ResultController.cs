using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Controllers
{
    public class ResultController : Controller
    {
        ScholarshipEntities db = new ScholarshipEntities();

        // GET: Result
        public ActionResult Index(int Std, int StdId,string time)
        {
            try
            {
                string score = "0";

                if (!string.IsNullOrEmpty(Session["correctAns"].ToString()))
                    score = Session["correctAns"].ToString();
                int std = Std;
                int stdId = StdId;
                string totalTime = time;

                tblStudentResult mObj = new tblStudentResult();
                mObj.Standard = stdId;
                mObj.StudentId = std;
                mObj.Time = totalTime;
                mObj.CreataionDate = DateTime.Now;
                mObj.result = score;

                db.tblStudentResults.Add(mObj);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.ControllerName = "Register";
                mobj.MethodName = "StudentRegister";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                db.tblExceptions.Add(mobj);
                db.SaveChanges();

                return RedirectToAction("Index", "Exception");
            }
            return View();
        }

        public ActionResult Result()
        {
            var data = (from sp in db.tblStdPaymentDetails.ToList()
                        join sc in db.tblScholarships.ToList() on sp.ScholarshipId equals Convert.ToString(sc.Id)
                        join s in db.tblStudentResults.ToList() on sp.Stdid equals s.StudentId
                        join st in db.tblStudentDetails.ToList() on sp.Stdid equals st.Id
                        select new StdResultDetails
                        {
                            StudentName = st.Name + " " + st.ParentName + " " + st.SurName,
                            ScholarshipName = sc.Name,
                            Score = s.result,
                        }).OrderByDescending(x => x.Score).Take(50).ToList();

            return View(data);
        }
        public ActionResult Certifcate()
        {
            return View();
        }

    }
}