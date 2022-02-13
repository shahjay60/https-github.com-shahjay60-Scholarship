using Scholarship.Areas.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Areas.Admin.Controllers
{
    public class AdPaymentDetailsController : Controller
    {
        // GET: Admin/AdPaymentDetails
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Index()
        {

            var Scholarships = entity.tblScholarships.ToList();
            List<PaymentInfo> mPaymentInfo = new List<PaymentInfo>();

            mPaymentInfo = (from sp in entity.tblStdPaymentDetails.ToList()
                        join sc in Scholarships.ToList() on sp.ScholarshipId equals Convert.ToString(sc.Id)
                        join s in entity.tblStudentDetails.ToList() on sp.Stdid equals s.Id
                        select new PaymentInfo
                        {
                            StudentName = s.Name +" "+s.ParentName + " " + s.SurName,
                            ScholarshipName = sc.Name,
                            Amount = sp.Amount,
                            TransactionDate = sp.TransactionDate,
                            TransactionId = sp.TransacrtionId
                        }).ToList();

            return View(mPaymentInfo);
        }
    }
}