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
        IDictionary<int, string> SelectedAnswerWithQuestinId = new Dictionary<int, string>();
        List<int> SkipQuest = new List<int>();
        List<int> AttemptedQue = new List<int>();
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
                Session["stdId"] = data.Id;
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

            int std = Convert.ToInt32(Session["Std"]);
            totalCount = totalCount + 1;
            if (drp.QuestionNo == 1)
            {

                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == 1 && m.Standard == std);

                TempData["qData"] = SingleQuestion;
                return RedirectToAction("Questions");
            }
            else
            {
                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == drp.QuestionNo && m.Standard == std);
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
            int std = Convert.ToInt32(Session["Std"]);

            var totalQues = db.tblQuestions.Where(m => m.Standard == std).ToList();

            ViewBag.questionNo = totalQues;
            ViewBag.TotalquestionNo = totalQues.Count;
            Session["questionNo"] = totalQues;
            ViewBag.skipQues = qNo;
            tblQuestion a = db.tblQuestions.SingleOrDefault(m => m.Id == qNo && m.Standard == std);

            //tblQuestion a = (tblQuestion)TempData["qData"];
            ViewBag.SkipQuesList = (List<int>)Session["SkipQuest"];

            Dictionary<int, string> myDictionary = (Dictionary<int, string>)Session["DateCollections"];

            if (myDictionary != null && myDictionary.Count > 0)
            {
                a.selectedvalue = myDictionary.Where(x => x.Key == qNo).Select(x => x.Value).FirstOrDefault();
            }
            return View(a);
        }
        [HttpPost]
        public ActionResult Questions(tblQuestion aaa, string Previous, string Finished, string Next, FormCollection fc)
        {
            int std = Convert.ToInt32(Session["Std"]);
            int stdId = Convert.ToInt32(Session["stdId"]);

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
                AttemptedQue.Add((int)aaa.Id);
                SelectedAnswerWithQuestinId.Add((int)aaa.Id, aaa.selectedvalue);
            }

            var selectedData = (Dictionary<int, string>)Session["DateCollections"];

            if (selectedData != null)
            {
                foreach (var item in selectedData)
                {
                    var data = SelectedAnswerWithQuestinId.Where(x => x.Key == item.Key).Count();

                    if (data > 0)
                    {
                        SelectedAnswerWithQuestinId.Remove(item.Key);
                    }
                    SelectedAnswerWithQuestinId.Add(item.Key, item.Value);
                }
            }

            Session["DateCollections"] = SelectedAnswerWithQuestinId;
            var marks = Session["correctAns"];

            var totalQues = db.tblQuestions.Where(m => m.Standard == std).ToList();

            if (!string.IsNullOrEmpty(Next))
            {
                int qId = (int)aaa.Id + 1;


                if (!string.IsNullOrEmpty(Convert.ToString(aaa.selectedvalue)))
                {
                    tblQuestion SingleQuestion = db.tblQuestions
                                                   .SingleOrDefault(m => m.Id == qId && m.Standard == std);

                    ViewBag.questionNo = qId;
                    Session["a"] = qId;
                    TempData["qData"] = SingleQuestion;

                    var DataTobeRemove = (List<int>)Session["SkipQuest"];
                    if (DataTobeRemove != null)
                    {
                        DataTobeRemove.RemoveAll(item => item == (int)aaa.Id);
                    }
                    Session["SkipQuest"] = DataTobeRemove;
                }
                else
                {
                    tblQuestion SingleQuestion = db.tblQuestions
                                                   .SingleOrDefault(m => m.Id == qId && m.Standard == std);
                    if (SingleQuestion != null)
                    {
                        ViewBag.questionNo = qId;
                        Session["a"] = SingleQuestion.Id;
                        TempData["qData"] = SingleQuestion;
                        SkipQuest.Add((int)aaa.Id);

                        var SkipData = (List<int>)Session["SkipQuest"];
                        if (SkipData != null)
                        {
                            foreach (var item in SkipData)
                            {
                                var data = SkipQuest.Where(x => x == item).Count();

                                if (data > 0)
                                {
                                    SkipQuest.Remove(item);
                                }
                                SkipQuest.Add(item);
                            }
                        }

                        Session["SkipQuest"] = SkipQuest;
                    }
                }
            }
            if (!string.IsNullOrEmpty(Previous))
            {
                int qId = (int)aaa.Id - 1;
                tblQuestion SingleQuestion = db.tblQuestions
                                               .SingleOrDefault(m => m.Id == qId && m.Standard == std);

                ViewBag.questionNo = qId;
                Session["a"] = SingleQuestion.Id;
                TempData["qData"] = SingleQuestion;
            }

            if (!string.IsNullOrEmpty(Finished))
            {
                string dt = Request.Form["Country"].ToString();
                return RedirectToAction("Index", "Result",new {std= std, StdId = stdId, time= dt.Trim() });
            }
            return RedirectToAction("Questions");
        }
        [HttpGet]
        public ActionResult NextQuestion(int id)
        {
            ViewBag.StudentName = TempData["StdName"];
            ViewBag.std = TempData["Std"];

            int qNo = id;
            ViewBag.questionNo = qNo;
            Session["a"] = id;

            var ListOfSkipQues = (List<int>)Session["SkipQuest"];
            if (ListOfSkipQues != null)
            {
                if (ListOfSkipQues.Count > 0)
                {
                    ListOfSkipQues.RemoveAll(x => x == id);
                    Session["SkipQuest"] = null;
                    Session["SkipQuest"] = ListOfSkipQues;
                }
            }
            else
            {
                int lastAttemQues = AttemptedQue.LastOrDefault();
                for (int i = lastAttemQues; i <= id; i++)
                {
                    SkipQuest.Add(i);
                }
                Session["SkipQuest"] = SkipQuest;

            }

            return RedirectToAction("Questions");

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
            int std = Convert.ToInt32(Session["Std"]);

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