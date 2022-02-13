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
    public class PaymentController : Controller
    {
        // GET: Payment
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Index(int stdid, int ScholarshipId)
        {
            var model = entity.tblStudentDetails.ToList().Where(x => x.Id == stdid).FirstOrDefault();
            var Scholarshipmodel = entity.tblScholarships.ToList().Where(x => x.Id == ScholarshipId).FirstOrDefault();
            PaymentClass mPaymentClass = new PaymentClass();

            mPaymentClass.ScholarshipAmount = Scholarshipmodel.Amount;
            mPaymentClass.ScholarshipName = Scholarshipmodel.Name;
            mPaymentClass.StdId = model.Id;
            mPaymentClass.Name = model.Name + " " + model.ParentName + " " + model.SurName;
            mPaymentClass.Email = model.EmailId;
            mPaymentClass.ContactNumber = model.ContactNumber;
            mPaymentClass.ScholarshipId = ScholarshipId;
            mPaymentClass.ScholarshipDetail = Scholarshipmodel.Details;

            return View(mPaymentClass);
        }
        [HttpPost]
        public ActionResult Index(PaymentClass model)
        {
            RemotePost myremotepost = new RemotePost();

            //for testing purpose
            //string key = "rjQUPktU";
            //string salt = "e5iIg1jwi8";

            //for production purpose
            string key = "ECmL0f";
            string salt = "UJptQ40mBaaFGIY6zj7eIeVKHIofiI2V";


            //posting all the parameters required for integration.

            myremotepost.Url = "https://secure.payu.in/_payment";


           // myremotepost.Url = "https://test.payu.in/_payment";


            myremotepost.Add("key", key);
            string txnid = Generatetxnid();
            myremotepost.Add("txnid", txnid);
            myremotepost.Add("amount", Convert.ToString(model.ScholarshipAmount));
            myremotepost.Add("productinfo", model.ScholarshipName);
            myremotepost.Add("firstname", model.Name);
            myremotepost.Add("phone", model.ContactNumber);
            myremotepost.Add("email", model.Email);
            myremotepost.Add("surl", "http://eduxam.in/Return/Success" + model.StdId);//Change the success url here depending upon the port number of your local system.
            myremotepost.Add("furl", "http://eduxam.in/Return/Fail");//Change the failure url here depending upon the port number of your local system.

            //myremotepost.Add("surl", "https://localhost:44372/Return/Success");//Change the success url here depending upon the port number of your local system.
            //myremotepost.Add("furl", "https://localhost:44372/Return/Fail");//Change the failure url here depending upon the port number of your local system.

            myremotepost.Add("service_provider", "payu_paisa");
            string hashString = key + "|" + txnid + "|" + model.ScholarshipAmount + "|" + model.ScholarshipName + "|" + model.Name + "|" + model.Email + "|||||||||||" + salt;
            //string hashString = "3Q5c3q|2590640|3053.00|OnlineBooking|vimallad|ladvimal@gmail.com|||||||||||mE2RxRwx";
            string hash = Generatehash512(hashString);
            myremotepost.Add("hash", hash);

            tblStdPaymentDetail mtblStdPaymentDetail = new tblStdPaymentDetail();
            mtblStdPaymentDetail.Stdid = model.StdId;
            mtblStdPaymentDetail.ScholarshipId =Convert.ToString(model.ScholarshipId);
            mtblStdPaymentDetail.Amount = model.ScholarshipAmount;
            mtblStdPaymentDetail.TransacrtionId = txnid;
            mtblStdPaymentDetail.TransactionDate = DateTime.Now;

            entity.tblStdPaymentDetails.Add(mtblStdPaymentDetail);
            entity.SaveChanges();

            myremotepost.Post();
            return View();
        }

        public class RemotePost
        {
            private System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();

            public string Url = "";
            public string Method = "post";
            public string FormName = "form1";

            public void Add(string name, string value)
            {
                Inputs.Add(name, value);
            }

            public void Post()
            {
                System.Web.HttpContext.Current.Response.Clear();

                System.Web.HttpContext.Current.Response.Write("<html><head>");

                System.Web.HttpContext.Current.Response.Write(string.Format("</head><body onload=\"document.{0}.submit()\">", FormName));
                System.Web.HttpContext.Current.Response.Write(string.Format("<form name=\"{0}\" method=\"{1}\" action=\"{2}\" >", FormName, Method, Url));
                for (int i = 0; i < Inputs.Keys.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(string.Format("<input name=\"{0}\" type=\"hidden\" value=\"{1}\">", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));
                }
                System.Web.HttpContext.Current.Response.Write("</form>");
                System.Web.HttpContext.Current.Response.Write("</body></html>");

                System.Web.HttpContext.Current.Response.End();
            }
        }

        //Hash generation Algorithm

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }
        public string Generatetxnid()
        {

            Random rnd = new Random();
            string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
            string txnid1 = strHash.ToString().Substring(0, 20);

            return txnid1;
        }

    }
}