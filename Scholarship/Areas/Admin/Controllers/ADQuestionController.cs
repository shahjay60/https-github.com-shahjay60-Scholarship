using Scholarship.Areas.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Areas.Admin.Controllers
{
    public class ADQuestionController : Controller
    {
        // GET: Admin/ADQuestion
        ScholarshipEntities entity = new ScholarshipEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetQuestiondata(int id=0)
        {
            var model = entity.tblQuestions.Where(e=>e.Standard==id).ToList();
            //return View(model);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            return View();
        }
        //kenirah
        [HttpPost]
        public ActionResult AddOuestions(List<QuestionInfo> QuestionsDetails)
        {
            var qus = entity.tblQuestions.ToList();
            foreach (var item in QuestionsDetails)
            { 
                tblQuestion model = new tblQuestion();
                model.Standard = item.Standard;
                model.Question = item.Question;
                model.OptionA = item.OptionA;
                model.OptionB = item.OptionB;
                model.OptionC = item.OptionC;
                model.OptionD = item.OptionD;
                model.CorrectAns = item.CorrectAnswer;

                entity.Entry(model).State = EntityState.Modified;
                entity.SaveChanges();
                 
                
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetbyID(int ID)
        {
            var Employee = entity.tblQuestions.Where(x => x.Id ==ID).FirstOrDefault();
            return Json(Employee, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(tblQuestion model)
        {
            var exisitingData = entity.tblQuestions.Where(x => x.Id == model.Id).FirstOrDefault();
            entity.tblQuestions.Add(exisitingData);
            entity.SaveChanges();
            return Json("suceess", JsonRequestBehavior.AllowGet);
        }
        public JsonResult Delete(int ID)
        {
            var exisitingData = entity.tblQuestions.Where(x => x.Id ==ID).FirstOrDefault();
            entity.tblQuestions.Remove(exisitingData);
            entity.SaveChanges();
            return Json("suceess" , JsonRequestBehavior.AllowGet);
        }

    }
}