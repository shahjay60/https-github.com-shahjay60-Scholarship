using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Areas.Admin.Controllers
{
    public class AdStudentDetailsController : Controller
    {
        // GET: Admin/AdStudentDetails
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Index()
        {
            var model = entity.tblStudentDetails.ToList();
            return View(model);
        }
        public ActionResult Detail(int id)
        {
            var model = entity.tblStudentDetails.ToList().Where(x => x.Id == id).FirstOrDefault() ;

            return View(model);
        }
    }
}