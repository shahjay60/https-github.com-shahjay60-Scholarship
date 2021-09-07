using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Controllers
{
    public class ScholarshipController : Controller
    {
        ScholarshipEntities entity = new ScholarshipEntities();
        // GET: Scholarship
        public ActionResult Index()
        {
            var ScholarshipData = entity.tblScholarships.Where(x => x.IsActive == true).ToList();

            return View(ScholarshipData);
        }
        public ActionResult Details(int id)
        {
            var ScholarshipData = entity.tblScholarships.Where(x => x.Id == id).FirstOrDefault();

            return View(ScholarshipData);
        }

        public ActionResult LoginForScholarship(StudentLoginDomain model)
        {
            var data = entity.tbl_login.ToList().Where(x => x.UserName == model.UserName && x.Password == model.Password);
            string Message = "Invalid email or password";

            if (data != null)
                return Json("Success", JsonRequestBehavior.AllowGet);
            else
                return Json(Message, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Apply(int id)
        {
            var ScholarshipData = entity.tblScholarships.Where(x => x.Id == id).FirstOrDefault();

            return View(ScholarshipData);
        }

    }
}