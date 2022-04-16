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
        public ActionResult Index()
        {
            try
            {
                string score = Session["correctAns"].ToString();
                int std = (int)Session["Std"];
                int stdId = (int)Session["stdId"];
                int totalTime = (int)Session["totalTime"];

                tblStudentResult mObj = new tblStudentResult();
                mObj.Standard = stdId;
                mObj.StudentId = std;
                mObj.Time = Convert.ToString(totalTime);
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
    }
}