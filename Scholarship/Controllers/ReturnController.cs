using Aspose.Pdf.Facades;
using HiQPdf;
using Scholarship.Models;
using Spire.Pdf;
using Spire.Pdf.HtmlConverter;
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
            string DocPath = Server.MapPath("~/EmailTemplate/paymentreceipt.html"); //Orginal Pdf
            string fileName = Server.MapPath("~/EmailTemplate/Foobar.pdf");
            var content = System.IO.File.ReadAllText(DocPath);
            content = content.Replace("#rcptno", rcptNo);
            content = content.Replace("#rcptdate", DateTime.Now.ToShortDateString());
            content = content.Replace("#name", model.Name);
            ////Write new HTML string to file
            System.IO.File.WriteAllText(fileName, content);

            string pdffileName = Server.MapPath("~/EmailTemplate/PaymentReceipt - Copy.pdf");

            Aspose.Pdf.Heading heading2 = new Aspose.Pdf.Heading(1);
            Aspose.Pdf.Document doc = new Aspose.Pdf.Document(pdffileName);
            PdfContentEditor editor = new PdfContentEditor();
            editor.BindPdf(doc);
            int PageCount = doc.Pages.Count;
            for (int i = 1; i <= PageCount; i++)
            {
                // CHANGE TEXT
                editor.ReplaceText("GVB0001", i, rcptNo);
                editor.ReplaceText("23, Feb 2022", i, DateTime.Now.ToString("dd/MMM/yy"));
                editor.ReplaceText("Bill To :", i, "Bill To :"+ model.Name);
            }
            // DOWNLOAD
            doc.Save(pdffileName);

            //PdfDocument doc = new PdfDocument();

            //PdfPageSettings setting = new PdfPageSettings();
            //setting.Size = Spire.Pdf.PdfPageSize.A4;

            //string htmlCode = content;
            //PdfHtmlLayoutFormat htmlLayoutFormat = new PdfHtmlLayoutFormat();
            //htmlLayoutFormat.IsWaiting = true;


            //Thread thread = new Thread(() =>
            //{ doc.LoadFromHTML(htmlCode, false, setting, htmlLayoutFormat); });
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start();
            //thread.Join();

            ////Save pdf file.
            //string fileName = Server.MapPath("~/EmailTemplate/output-wiki.pdf");

            //doc.SaveToFile(fileName);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                string text = string.Empty;
                MailMessage mm = new MailMessage();
                mm.To.Add(model.EmailId);
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
                mm.Attachments.Add(new Attachment(DocPath));
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
                mtblStdPaymentDetail.Stdid = id;
                mtblStdPaymentDetail.ScholarshipId = Convert.ToString(scholarshipid);
                mtblStdPaymentDetail.Amount = Scholarship.Amount;
                mtblStdPaymentDetail.TransacrtionId = "101";
                mtblStdPaymentDetail.TransactionDate = DateTime.Now;
                mtblStdPaymentDetail.ReceiptNumber = paymentRecid;

                entity.tblStdPaymentDetails.Add(mtblStdPaymentDetail);
                entity.SaveChanges();

                if (System.IO.File.Exists(pdffileName))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(DocPath);
                }

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