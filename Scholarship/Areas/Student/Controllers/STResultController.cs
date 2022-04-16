using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Areas.Student.Controllers
{
    public class STResultController : Controller
    {
        // GET: Student/STResult
        ScholarshipEntities db = new ScholarshipEntities();

        public ActionResult Index()
        {
            int studentId = (int)Session["Id"];

            ViewBag.Scrore = db.tblStudentResults.Where(x => x.StudentId == studentId).Select(x => x.result).FirstOrDefault();

            return View();
        }
    }
}