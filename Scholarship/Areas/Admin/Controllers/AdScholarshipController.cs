using Scholarship.Areas.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Areas.Admin.Controllers
{
    [Authorize]
    public class AdScholarshipController : Controller
    {
        // GET: Admin/AdScholarship
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Index()
        {
            var model = entity.tblScholarships.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(tblScholarship model)
        {
            try
            {
                System.Text.RegularExpressions.Regex rx = new System.Text.RegularExpressions.Regex( " <.*?> ");


                model.Details= rx.Replace(model.Details, "");
                entity.tblScholarships.Add(model);
                entity.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.MethodName = "Add";
                mobj.ControllerName = "AdScholarship";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                entity.tblExceptions.Add(mobj);
                entity.SaveChanges();
                return Json(false, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult Edit(int id=0)
        {
            var data = entity.tblScholarships.Where(x => x.Id == id).FirstOrDefault();
            ScholarshipDomain mData = new ScholarshipDomain();
            mData.Id = data.Id;
            mData.Amount = data.Amount;
            mData.Name = data.Name;
            mData.IsActive = data.IsActive;
            mData.Details = data.Details;
            mData.MinStd = data.MinStd;
            mData.MaxStd = data.MaxStd;
            var localDateTime = data.ExamDate.Value.ToString("yyyy-MM-dd HH:mm:ss").Replace(' ', 'T');
            mData.DisplayDatetime = localDateTime;
            return View(mData);
        }
        [HttpPost]
        public ActionResult Edit(tblScholarship model)
        {
            try
            {
                var exisitingData = entity.tblScholarships.Where(x => x.Id == model.Id).FirstOrDefault();
                entity.tblScholarships.Remove(exisitingData);
                entity.SaveChanges();

                var data = entity.tblScholarships.First<tblScholarship>();
                data.Amount = model.Amount;
                data.Name = model.Name;
                data.IsActive = model.IsActive;
                data.Details = model.Details;
                data.ExamDate = model.ExamDate;
                data.MinStd = model.MinStd;
                data.MaxStd = model.MaxStd;
                entity.tblScholarships.Add(data);
                entity.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.MethodName = "Edit";
                mobj.ControllerName = "AdScholarship";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                entity.tblExceptions.Add(mobj);
                entity.SaveChanges();
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int id)
        {
            tblScholarship data = new tblScholarship();

            try
            {
                data = entity.tblScholarships.Find(id);
                entity.tblScholarships.Remove(data);
                entity.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.MethodName = "Edit";
                mobj.ControllerName = "AdScholarship";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                entity.tblExceptions.Add(mobj);
                entity.SaveChanges();
                return RedirectToAction("Index");

            }
        }
    }
}