using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(AdminLoginDomain model)
        {
            var data = entity.tbl_login.ToList().Where(x => x.UserName == model.UserName && x.Password == model.Password).FirstOrDefault();
            string Message = "Invalid email or password";

            if (data !=null)
                return Json("Success", JsonRequestBehavior.AllowGet);
            else
                return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult StudentLogin(StudentLoginDomain model)
        {
            var data = entity.tblStudentDetails.ToList().Where(x => x.UserName == model.UserName && x.Password == model.Password).FirstOrDefault();
            string Message = "Invalid email or password";

            if (data != null)
                return Json(data.Id, JsonRequestBehavior.AllowGet);
            else
                return Json(Message, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangePwd()
        {
            return View();
        }

    }
}