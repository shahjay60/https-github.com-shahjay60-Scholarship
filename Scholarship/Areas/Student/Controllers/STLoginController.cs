using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Areas.Student.Controllers
{
    public class STLoginController : Controller
    {
        // GET: Student/STLOGin
        ScholarshipEntities entity = new ScholarshipEntities();
        Utilities mUtilities = new Utilities();

        public ActionResult ChangePassword(int id)
        {
            try
            {
                StudentLoginModel mStudentLoginModel = new StudentLoginModel();
                mStudentLoginModel.StdId = id;
                return View(mStudentLoginModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult ChangePWd(StudentLoginModel mstd)
        {
            try
            {
                var data = entity.tblStudentDetails.ToList().Where(x => x.Password == mstd.OldPassword).FirstOrDefault();
                if (data != null)
                {
                    tblStudentDetail mStudentLoginModel = new tblStudentDetail();
                    mStudentLoginModel = entity.tblStudentDetails.ToList().Where(x => x.Id == mstd.StdId).FirstOrDefault();

                    mStudentLoginModel.Password = mstd.NewPassword;
                    entity.Entry(mStudentLoginModel).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();

                    string body = string.Empty;
                    var lnkHref = "<a href='" + Url.Action("StudentLogin", "Login") + "'>Login</a>";
                    body = "<b>Please find the Login URL. </b>" + lnkHref;
                    mUtilities.SendMail(mStudentLoginModel.EmailId, "Your password change successfully..!!", body);
                    TempData["Message"] = "Password Change Successfully..!!";
                }
                else
                {
                    TempData["Message"] = "Old password is not match";
                }
                return View(mstd.StdId);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error!!";
                tblException mobj = new tblException();
                mobj.MethodName = "ChangePWd";
                mobj.ControllerName = "STDLogin";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                entity.tblExceptions.Add(mobj);
                entity.SaveChanges();
                return RedirectToAction("Fail", "../Return");
            }
        }

    }
}