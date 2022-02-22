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
    }
}