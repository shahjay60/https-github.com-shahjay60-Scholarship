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
        IDictionary<int, string> numberNames = new Dictionary<int, string>();
        int totalCount = 0;
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
            if (data != null)
            {
                Session["StdName"] = data.Name + " " + data.ParentName + " " + data.SurName;
                Session["Std"] = data.STD;
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
            ViewBag.questionNo = Convert.ToInt32(TempData["QuestionNo"]);
            Session["a"] = Convert.ToInt32(TempData["QuestionNo"]);

            int std = Convert.ToInt32(ViewBag.std);
            totalCount = totalCount + 1;
            if (drp.QuestionNo == 1)
            {

                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == 1 && m.Standard == 3);

                TempData["qData"] = SingleQuestion;
                return RedirectToAction("Questions");
            }
            else
            {
                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == drp.QuestionNo && m.Standard == 3);
                int qus = (int)drp.QuestionNo;
                return View(SingleQuestion);
            }
        }

        public ActionResult Questions()
        {
            int qNo = 0;
            if (Session["StdName"] != null && Session["Std"] != null)
            {
                ViewBag.StudentName = Session["StdName"];
                ViewBag.std = Session["Std"];
            }
            if ((int)Session["a"] == 0)
            {
                qNo = 1;
                ViewBag.questionNo = qNo;
            }
            else
            {
                qNo = (int)Session["a"];
                ViewBag.questionNo = qNo;
            }

            tblQuestion a = (tblQuestion)TempData["qData"];

            Dictionary<int, string> myDictionary = (Dictionary<int, string>)Session["DateCollections"];

            if (myDictionary != null && myDictionary.Count>0)
            {
                a.selectedvalue = myDictionary.Where(x => x.Key == qNo).Select(x => x.Value).FirstOrDefault();
            }
            return View(a);
        }
        [HttpPost]
        public ActionResult Questions(tblQuestion aaa, string Previous, string Skip, string Next, FormCollection fc)
        {
            totalCount = totalCount + 1;
            int std = Convert.ToInt32(ViewBag.std);


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

                numberNames.Add((int)aaa.Id, aaa.selectedvalue);
            }


            var selectedData = (Dictionary<int, string>)Session["DateCollections"];

            if (selectedData != null)
            {
                foreach (var item in selectedData)
                {
                    numberNames.Add(item.Key, item.Value);
                }
            }

            Session["DateCollections"] = numberNames;
            var marks = Session["correctAns"];

            if (totalCount == 100)
            {
                return RedirectToAction("Create", "Result");
            }
            if (!string.IsNullOrEmpty(Skip))
            {
                int qId = (int)aaa.Id + 1;
                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == qId && m.Standard == 3);

                ViewBag.questionNo = qId;
                Session["a"] = SingleQuestion.Id;
                TempData["qData"] = SingleQuestion;
            }
            if (!string.IsNullOrEmpty(Next))
            {
                int qId = (int)aaa.Id + 1;

                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == qId && m.Standard == 3);

                ViewBag.questionNo = qId;
                Session["a"] = qId;
                TempData["qData"] = SingleQuestion;
            }
            if (!string.IsNullOrEmpty(Previous))
            {
                int qId = (int)aaa.Id - 1;
                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == qId && m.Standard == 3);

                ViewBag.questionNo = qId;
                Session["a"] = SingleQuestion.Id;
                TempData["qData"] = SingleQuestion;
            }
            return RedirectToAction("Questions");
        }
        public ActionResult NextQuestion()
        {
            ViewBag.StudentName = TempData["StdName"];
            ViewBag.std = TempData["Std"];

            int qNo = (int)TempData["a"];
            ViewBag.questionNo = qNo;
            tblQuestion a = (tblQuestion)TempData["qData"];
            return View(a);
        }
        [HttpPost]
        public ActionResult NextQuestion(tblQuestion aaa)
        {
            totalCount = totalCount + 1;

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

            if (totalCount == 100)
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
            totalCount = totalCount - 1;

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