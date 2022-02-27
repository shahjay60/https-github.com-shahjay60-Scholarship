using Aspose.Pdf;
using Aspose.Pdf.Facades;
using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        private string Psurl = ConfigurationManager.AppSettings["surl"];
        private string Pfurl = ConfigurationManager.AppSettings["furl"];
        private string PUrl = ConfigurationManager.AppSettings["Url"];
        private string Pkey = ConfigurationManager.AppSettings["key"];
        private string Psalt = ConfigurationManager.AppSettings["salt"];
        private string Pservice_provider = ConfigurationManager.AppSettings["service_provider"];
        private string Host = ConfigurationManager.AppSettings["host"];
        private string Port = ConfigurationManager.AppSettings["port"];
        private string Email = ConfigurationManager.AppSettings["emailId"];
        private string Password = ConfigurationManager.AppSettings["password"];
        public ActionResult Index(int stdid, int ScholarshipId)
        {
            var model = entity.tblStudentDetails.ToList().Where(x => x.Id == stdid).FirstOrDefault();
            var Scholarshipmodel = entity.tblScholarships.ToList().Where(x => x.Id == ScholarshipId).FirstOrDefault();
            PaymentClass mPaymentClass = new PaymentClass();

            mPaymentClass.ScholarshipAmount = 121;
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
            try
            {

                RemotePost myremotepost = new RemotePost();
                string key = Pkey;
                string salt = Psalt;
                myremotepost.Url = PUrl;
                myremotepost.Add("key", key);
                string txnid = Generatetxnid();
                myremotepost.Add("txnid", txnid);
                myremotepost.Add("amount", Convert.ToString(model.ScholarshipAmount));
                myremotepost.Add("productinfo", model.ScholarshipName);
                myremotepost.Add("firstname", model.Name);
                myremotepost.Add("phone", model.ContactNumber);
                myremotepost.Add("email", model.Email);
                myremotepost.Add("surl", Psurl);//Change the success url here depending upon the port number of your local system.
                myremotepost.Add("furl", Pfurl);//Change the failure url here depending upon the port number of your local system.
                myremotepost.Add("service_provider", Pservice_provider);
                string hashString = key + "|" + txnid + "|" + model.ScholarshipAmount + "|" + model.ScholarshipName + "|" + model.Name + "|" + model.Email + "|||||||||||" + salt;
                //string hashString = "3Q5c3q|2590640|3053.00|OnlineBooking|vimallad|ladvimal@gmail.com|||||||||||mE2RxRwx";
                string hash = Generatehash512(hashString);
                myremotepost.Add("hash", hash);

                int? paymentRecid = entity.tblStdPaymentDetails.ToList()
                                         .OrderByDescending(x => x.Id)
                                         .Select(c => c.ReceiptNumber)
                                         .FirstOrDefault();
                if (paymentRecid == null || paymentRecid == 0)
                {
                    paymentRecid = 1;
                }
                else
                {
                    paymentRecid = paymentRecid + 1;
                }

                #region Send payment receipt
                var Savepath = Server.MapPath("~/EmailTemplate/HTML-to-PDF.pdf");
                string rcptNo = "";
                if (paymentRecid <= 99)
                {
                    rcptNo = "GVB000" + paymentRecid;
                }
                else
                {
                    rcptNo = "GVB00" + paymentRecid;
                }
                string DocPath = Server.MapPath("~/EmailTemplate/PaymentReceipt-converted.pdf"); //Orginal Pdf

                // OPEN DOCUMENTS
                Aspose.Pdf.Document doc = new Aspose.Pdf.Document(DocPath);
                PdfContentEditor editor = new PdfContentEditor();
                editor.BindPdf(doc);
                int PageCount = doc.Pages.Count;
                for (int i = 1; i <= PageCount; i++)
                {
                    // CHANGE TEXT
                    editor.ReplaceText("GVB0001", i, rcptNo);
                    editor.ReplaceText("23, Feb 2022", i, DateTime.Now.ToString("dd, MMMM yyyy"));
                    editor.ReplaceText("Name", i, model.Name);
                }
                // DOWNLOAD
                doc.Save(DocPath);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    string text = string.Empty;
                    MailMessage mm = new MailMessage();
                    mm.To.Add(model.Email);
                    MailAddress address = new MailAddress(Email);
                    mm.From = address;

                    mm.Subject = "PAYMENT RECEIPT FOR EST";

                    text = "Online registration transaction has been processed successfully for Eduxam scholarship test." +
                        "<br>Receipt copy is attached <br><br>Best Regards,<br>Team Eduxam<br>" +
                        "EMAIL: info@Eduxam.in" +
                        "<br>This e-mail and any files transmitted with it are for the sole use of the intended recipient(s)" +
                        " <br>" +
                        "and may contain confidential and privileged information." +
                        " If you are not the intended recipient, please contact the sender by reply <br>" +
                        " e-mail and destroy all copies and the original message. <br>" +
                        "Any unauthorized review, use, disclosure, dissemination, forwarding,<br>" +
                        " printing or copying of this email or any action taken in reliance on <br>" +
                        " this e-mail is strictly prohibited and may be unlawful. <br>" +
                        "The recipient acknowledges that Eduxam LLP or its subsidiaries and associated <br>" +
                        " companies are unable to exercise control or ensure or guarantee the <br>" +
                        "integrity of/overthe contents of the information contained in e-mail transmissions and<br>" +
                        " further acknowledges that any views expressed in this message are those of the individual sender <br>" +
                        " and no binding nature of the message shall be implied or assumed unless the sender does <br>" +
                        " so expressly with due authority of Eduxam LLP. <br>" +
                        "Before opening any attachments please check them for viruses and defects.";

                    mm.Body = text;
                    mm.Attachments.Add(new Attachment(Savepath));
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "relay-hosting.secureserver.net";
                    smtp.EnableSsl = false;
                    smtp.Port = Convert.ToInt32(Port);
                    smtp.UseDefaultCredentials = false;

                    NetworkCredential credentials = new NetworkCredential(Email, Password);
                    smtp.Credentials = credentials;

                    smtp.Send(mm);

                    tblStdPaymentDetail mtblStdPaymentDetail = new tblStdPaymentDetail();
                    mtblStdPaymentDetail.Stdid = model.StdId;
                    mtblStdPaymentDetail.ScholarshipId = Convert.ToString(model.ScholarshipId);
                    mtblStdPaymentDetail.Amount = model.ScholarshipAmount;
                    mtblStdPaymentDetail.TransacrtionId = txnid;
                    mtblStdPaymentDetail.TransactionDate = DateTime.Now;
                    mtblStdPaymentDetail.ReceiptNumber = paymentRecid;

                    entity.tblStdPaymentDetails.Add(mtblStdPaymentDetail);
                    entity.SaveChanges();
                }
                #endregion

                myremotepost.Post();
                return View();
            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.MethodName = "Index";
                mobj.ControllerName = "Payment";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                entity.tblExceptions.Add(mobj);
                entity.SaveChanges();
                return RedirectToAction("Fail", "Return");
            }
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