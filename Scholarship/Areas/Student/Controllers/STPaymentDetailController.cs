using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Areas.Student.Controllers
{
    public class STPaymentDetailController : Controller
    {
        // GET: Student/STPaymentDetail
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Index()
        {
            return View();
        }
    }
}