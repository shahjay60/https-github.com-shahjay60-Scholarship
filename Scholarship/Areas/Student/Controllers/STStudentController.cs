using System;
using System.Collections.Generic;
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
            var model = entity.tblStudentDetails.ToList().Where(x=>x.Id==id).FirstOrDefault();
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var model = entity.tblStudentDetails.ToList().Where(x => x.Id == id).FirstOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(tblStudentDetail model)
        {
            //var model = entity.tblStudentDetails.ToList().Where(x => x.Id == id).FirstOrDefault();
            //return View(model);

            return View();
        }
    }
}