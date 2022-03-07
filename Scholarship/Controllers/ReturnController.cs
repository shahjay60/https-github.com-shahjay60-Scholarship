using Aspose.Pdf.Facades;
using NReco.PdfGenerator;
using Scholarship.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Controllers
{
    public class ReturnController : Controller
    {
        // GET: Return
        ScholarshipEntities entity = new ScholarshipEntities();
        Utilities mUtilities = new Utilities();

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
        public ActionResult Success(int id = 0, int scholarshipid = 0)
        {

            int? paymentRecid = entity.tblStdPaymentDetails.ToList()
                                     .OrderByDescending(x => x.Id)
                                     .Select(c => c.ReceiptNumber)
                                     .FirstOrDefault();

            var model = entity.tblStudentDetails.ToList().Where(x => x.Id == id).FirstOrDefault();
            var Scholarship = entity.tblScholarships.ToList().Where(x => x.Id == scholarshipid).FirstOrDefault();

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
            if (paymentRecid <= 9)
            {
                rcptNo = "GVB000" + paymentRecid;
            }
            else
            {
                rcptNo = "GVB00" + paymentRecid;
            }



            //string DocPath = Server.MapPath("~/EmailTemplate/PdfToHTML.html"); //Orginal html
            //var content = System.IO.File.ReadAllText(DocPath);
            //content = content.Replace("GVB0001", rcptNo);
            //content = content.Replace("23, Feb 2022", DateTime.Now.ToShortDateString());
            //content = content.Replace("#name", model.Name);
            //content = content.Replace("#src", "http://www.eduxam.in/Assests/images/logo1.jpeg");


            //var htmlToPdf = new HtmlToPdfConverter();
            //var pdfContentType = "application/pdf";

            //var pdfBytes = htmlToPdf.GeneratePdf(content);
            //var filepath = Server.MapPath("~/EmailTemplate/PdfToHTML" + id + ".pdf"); ;

            //var tempFolder = System.IO.Path.GetTempPath();
            //System.IO.File.WriteAllBytes(filepath, pdfBytes);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                string text = string.Empty;
                text = "your Login info <b>Username</b>: " + model.UserName + " and <b>Password</b>:" + model.Password + " ";
                text += "<br>Online registration transaction has been processed successfully for Eduxam scholarship test." +
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

                mUtilities.SendMail(model.EmailId, "PAYMENT DETAILS AND LOGIN CREDNETIALS FOR EST", text);

                tblStdPaymentDetail mtblStdPaymentDetail = new tblStdPaymentDetail();
                mtblStdPaymentDetail.Stdid = id;
                mtblStdPaymentDetail.ScholarshipId = Convert.ToString(scholarshipid);
                mtblStdPaymentDetail.Amount = Scholarship.Amount;
                mtblStdPaymentDetail.TransacrtionId = "101";
                mtblStdPaymentDetail.TransactionDate = DateTime.Now;
                mtblStdPaymentDetail.ReceiptNumber = paymentRecid;

                entity.tblStdPaymentDetails.Add(mtblStdPaymentDetail);
                entity.SaveChanges();
                GC.Collect();

                //if (System.IO.File.Exists(filepath))
                //{
                //    // If file found, delete it    
                //    System.IO.File.Delete(filepath);
                //}

            }
            #endregion
            return View();
        }
        public ActionResult Fail()
        {
            return View();
        }
    }
}