using Scholarship.Areas.Admin.Domain;
using Scholarship.Areas.Student.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Areas.Student.Controllers
{
    public class STStudentController : Controller
    {
        // GET: Student/STStudent
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Index(int id)
        {
            var model = entity.tblStudentDetails.ToList().Where(x => x.Id == id).FirstOrDefault();
            ViewBag.scholarshipid = entity.tblScholarships.Where(x => x.MinStd <= model.STD && x.MaxStd >= model.STD)
                                                                  .Select(x => x.Id).FirstOrDefault();

            ViewBag.ispaymentDone = entity.tblStdPaymentDetails.Where(x => x.Stdid == id).Select(x => x.Id)
                                    .FirstOrDefault();
            ViewBag.stdid = id;

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.stdid = id;

            if (TempData["Message"]!=null)
            {
                ViewBag.STDMessage = TempData["Message"];
            }
            var model = entity.tblStudentDetails.ToList().Where(x => x.Id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(tblStudentDetail model)
        {

            try
            {
                entity.Entry(model).State = EntityState.Modified;
                entity.SaveChanges();
                TempData["Message"] = "Success";
                return RedirectToAction("Edit", model.Id);
            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.MethodName = "Edit";
                mobj.ControllerName = "STStudent";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                entity.tblExceptions.Add(mobj);
                entity.SaveChanges();
                return RedirectToAction("Fail", "../Return");
            }
        }

        [HttpGet]
        public ActionResult PaymentDetail(int id)
        {
            var Scholarships = entity.tblScholarships.ToList();
            StdPaymentInfo mPaymentInfo = new StdPaymentInfo();

            mPaymentInfo = (from sp in entity.tblStdPaymentDetails.ToList()
                            join sc in Scholarships.ToList() on sp.ScholarshipId equals Convert.ToString(sc.Id)
                            join s in entity.tblStudentDetails.ToList() on sp.Stdid equals s.Id
                            where s.Id == id
                            select new StdPaymentInfo
                            {
                                StudentName = s.Name + " " + s.ParentName + " " + s.SurName,
                                ScholarshipName = sc.Name,
                                Amount = sp.Amount,
                                TransactionDate = sp.TransactionDate,
                                TransactionId = sp.TransacrtionId
                            }).FirstOrDefault();

            return View(mPaymentInfo);
        }
    }
}