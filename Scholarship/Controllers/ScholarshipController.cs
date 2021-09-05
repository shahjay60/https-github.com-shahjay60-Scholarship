using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Controllers
{
    public class ScholarshipController : Controller
    {
        // GET: Scholarship
        public ActionResult Index(int id)
        {
            return View();
        }
    }
}