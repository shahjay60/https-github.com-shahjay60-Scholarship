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
        Utilities mUtilities = new Utilities();
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
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return Json("Success", JsonRequestBehavior.AllowGet);
            }
            return Json(message, JsonRequestBehavior.AllowGet);

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

        public ActionResult ResetPassword(string EmailId)
        {
            try
            {
                var data = entity.tblStudentDetails.Where(x => x.EmailId.Trim().ToLower() == EmailId.Trim().ToLower()).FirstOrDefault();
                if (data != null)
                {
                    string body = string.Empty;
                    var lnkHref = "<a href='" + Url.Action("StudentLogin", "Login") + "'>Reset Password</a>";
                    body += "<bYour password is. </b>" + data.Password;
                    body += "<b>Please find the Login URL. </b>" + lnkHref;
                    mUtilities.SendMail(EmailId, "Reset Password!!", body);
                    return Json("Success");
                }
                else
                {
                    return Json("Error");
                }
            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.MethodName = "ResetPassword";
                mobj.ControllerName = "Login";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                entity.tblExceptions.Add(mobj);
                entity.SaveChanges();
                return RedirectToAction("Fail", "Return");
            }
        }


    }
}