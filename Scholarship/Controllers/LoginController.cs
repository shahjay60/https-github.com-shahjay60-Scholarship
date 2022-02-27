using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
            //var data = entity.tblUsers.ToList().Where(x => x.UserName == model.UserName && x.Password == model.Password).FirstOrDefault();
            //string Message = "Invalid email or password";

            int? userId = entity.ValidateUser(model.UserName, model.Password).FirstOrDefault();
            string message = string.Empty;

            //if (data !=null)
            //    return Json("Success", JsonRequestBehavior.AllowGet);
            //else
            //    return Json(Message, JsonRequestBehavior.AllowGet);

            switch (userId.Value)
            {
                case -1:
                    message = "Username and/or password is incorrect.";
                    break;
                case -2:
                    message = "Account has not been activated.";
                    break;
                default:
                    FormsAuthentication.SetAuthCookie(model.UserName,false);
                    return Json("Success", JsonRequestBehavior.AllowGet);
            }
              return Json(message, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult StudentLogin(StudentLoginDomain model)
        {
            var data = entity.tblStudentDetails.ToList().Where(x => x.UserName == model.UserName && x.Password == model.Password).FirstOrDefault();
            //string Message = "Invalid email or password";

            int? userId = entity.ValidateStudent(model.UserName, model.Password).FirstOrDefault();
            string message = string.Empty;

            //if (data !=null)
            //    return Json("Success", JsonRequestBehavior.AllowGet);
            //else
            //    return Json(Message, JsonRequestBehavior.AllowGet);

            switch (userId.Value)
            {
                case -1:
                    message = "Username and/or password is incorrect.";
                    break;
                case -2:
                    message = "Account has not been activated.";
                    break;
                default:
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return Json(data.Id, JsonRequestBehavior.AllowGet);

            }
            return Json(message, JsonRequestBehavior.AllowGet);

            //if (data != null)
            //    return Json(data.Id, JsonRequestBehavior.AllowGet);
            //else
            //    return Json(Message, JsonRequestBehavior.AllowGet);
        }
        [Authorize]

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("AdminLogin");
        }
        public ActionResult ChangePwd()
        {
            return View();
        }

    }
}