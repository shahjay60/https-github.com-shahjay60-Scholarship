using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Controllers
{
    public class ReturnController : Controller
    {
        // GET: Return
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Fail()
        {
            return View();
        }
    }
}