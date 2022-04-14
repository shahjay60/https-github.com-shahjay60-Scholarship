using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        ScholarshipEntities db = new ScholarshipEntities();
        [HttpGet]
        public ActionResult ExamLogin()
        {
            StudentLoginDomain model = new StudentLoginDomain();
            return View(model);
        }
        [HttpPost]
        public ActionResult ExamLogin(StudentLoginDomain model)
        {
            var std = db.tblStudentDetails.ToList();

            var data = db.tblStudentDetails.ToList()
                             .Where(x => x.UserName == model.UserName && x.Password == model.Password)
                             .FirstOrDefault();
            if (data!=null)
            {
                TempData["StdName"] = data.Name + " " + data.ParentName + " " + data.SurName;
                TempData["Std"] = data.STD;
                return RedirectToAction("Index", data);
            }
            else
            {
                ViewBag.NotValidUser = "Not Valid UserName and Password";
                return View();
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ExamDetails()
        {
            DrpList drp = new DrpList();
            drp.QuestionNo = 1;

            ViewBag.drpData = drp;
            ViewBag.StudentName = TempData["StdName"];
            ViewBag.std = TempData["Std"];

            ViewBag.questionNo = Convert.ToInt32(TempData["QuestionNo"]);
            TempData["a"] = Convert.ToInt32(TempData["QuestionNo"]);

            int std = Convert.ToInt32(ViewBag.std);

            if (drp.QuestionNo == 1)
            {

                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == 1 && m.Standard == std);

                TempData["qData"] = SingleQuestion;
                return RedirectToAction("NextQuestion");
            }
            else
            {
                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == drp.QuestionNo && m.Standard == std);
                int qus = (int)drp.QuestionNo;
                return View(SingleQuestion);
            }
        }
        public ActionResult NextQuestion()
        {
            int qNo = (int)TempData["a"];
            ViewBag.questionNo = qNo;
            tblQuestion a = (tblQuestion)TempData["qData"];
            return View(a);
        }
        [HttpPost]
        public ActionResult NextQuestion(tblQuestion aaa)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(aaa.selectedvalue)))
            {
                if (aaa.CorrectAns == Convert.ToString(aaa.selectedvalue))
                {
                    Session["correctAns"] = Convert.ToInt32(Session["correctAns"]) + 1;
                }
                else if (aaa.CorrectAns != Convert.ToString(aaa.selectedvalue))
                {
                    Session["correctAns"] = Convert.ToInt32(Session["correctAns"]) - 0.25;
                }
            }

            if (aaa.Id == 100)
            {
                return RedirectToAction("Create", "Result");
            }
            int qId = (int)aaa.Id + 1;
            int std = Convert.ToInt32(ViewBag.std);

            tblQuestion SingleQuestion = db.tblQuestions
                                           .SingleOrDefault(m => m.Id == qId && m.Standard == std);

            ViewBag.questionNo = qId;
            TempData["a"] = SingleQuestion.Id;
            TempData["qData"] = SingleQuestion;
            return RedirectToAction("NextQuestion");
        }

        public ActionResult PrevQuestion()
        {
            int qNo = (int)TempData["a"];
            ViewBag.questionNo = qNo;
            tblQuestion a = (tblQuestion)TempData["qData"];
            return View(a);
        }
        [HttpPost]
        public ActionResult PrevQuestion(tblQuestion aaa)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(aaa.selectedvalue)))
            {
                if (aaa.CorrectAns == Convert.ToString(aaa.selectedvalue))
                {
                    Session["correctAns"] = Convert.ToInt32(Session["correctAns"]) + 1;
                }
                else if (aaa.CorrectAns != Convert.ToString(aaa.selectedvalue))
                {
                    Session["correctAns"] = Convert.ToInt32(Session["correctAns"]) - 0.25;
                }
            }

            int qId = (int)aaa.Id - 1;
            int std = Convert.ToInt32(ViewBag.std);

            tblQuestion SingleQuestion = db.tblQuestions
                                           .SingleOrDefault(m => m.Id == qId && m.Standard == std);

            ViewBag.questionNo = qId;
            TempData["a"] = SingleQuestion.Id;
            TempData["qData"] = SingleQuestion;
            return RedirectToAction("PrevQuestion");

        }
    }
}