using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Scholarship.Controllers
{
    public class RegisterController : Controller
    {
        private string Host = ConfigurationManager.AppSettings["host"];
        private string Port = ConfigurationManager.AppSettings["port"];
        private string Email = ConfigurationManager.AppSettings["emailId"];
        private string Password = ConfigurationManager.AppSettings["password"];
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(HomeController));

        // GET: Register
        ScholarshipEntities entity = new ScholarshipEntities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StudentRegister(tblStudentDetail mdata)
        {
            try
            {
                mdata.UserName = mdata.EmailId;
                mdata.Password = RandomString(8, false);

                entity.tblStudentDetails.Add(mdata);
                entity.SaveChanges();
                int id = mdata.Id;
                logger.Info("Data saved ");

                if (id != 0)
                {
                    if (mdata.Apply_Scholarship == false)
                    {
                        #region SendEmail & Password
                        //Log.Info("Register Mail started...");
                        try
                        {
                            logger.Info("Start : Email to student registration");
                            MailMessage msgs = new MailMessage();
                            msgs.To.Add(mdata.EmailId);
                            MailAddress address = new MailAddress(Email);
                            msgs.From = address;
                            string body = string.Empty;
                            using (StreamReader reader = new StreamReader(Server.MapPath("../EmailTemplate/RegistraionEmailTemplate.html")))
                            {

                                body = reader.ReadToEnd();

                            }
                            body = body.Replace("{Name}", mdata.Name); //replacing the required things  

                            body = body.Replace("{username}", mdata.EmailId);

                            body = body.Replace("{password}", mdata.Password);
                            //   msgs.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                            msgs.Subject = "Thanks for creating your account and welcome to EduXam.";
                            string htmlBody = body;
                            msgs.Body = htmlBody;
                            msgs.IsBodyHtml = true;
                            SmtpClient client = new SmtpClient();
                            client.DeliveryMethod = SmtpDeliveryMethod.Network;

                            client.EnableSsl = false;
                            client.Host = "relay-hosting.secureserver.net"; ;
                            client.Port = Convert.ToInt32(Port);
                            client.UseDefaultCredentials = false;

                            // client.Credentials = new System.Net.NetworkCredential("email@gmail.com", "pass@");
                            NetworkCredential credentials = new NetworkCredential(Email, Password);
                            client.Credentials = credentials;
                            //Send the msgs  
                            client.Send(msgs);
                            Console.Write("success");
                            logger.Info("End : Email to student registration");
                        }
                        catch (Exception ex)
                        {
                            logger.Info(string.Format("Error Message {0} , Stracktrace {1} :", ex.Message.ToString(), ex.StackTrace.ToString()));
                            tblException mobj = new tblException();
                            mobj.ControllerName = "Register";
                            mobj.MethodName = "StudentRegister";
                            mobj.Message = ex.Message;
                            mobj.StackTrace = ex.StackTrace;
                            mobj.CreatedDatetime = DateTime.Now;
                            entity.tblExceptions.Add(mobj);
                            entity.SaveChanges();

                            return RedirectToAction("Index", "Exception");
                        }


                        #endregion
                        var details = new { id = 0, scholarshipid = 0 };
                        return Json(details, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        var scholarshipdetails = entity.tblScholarships.ToList();
                        int stdId = id;
                        int scholarshipid = entity.tblScholarships.Where(x => x.MinStd <= mdata.STD && x.MaxStd >= mdata.STD)
                                                                  .Select(x => x.Id).FirstOrDefault();
                        var details = new { id = id, scholarshipid = scholarshipid };
                        return Json(details, JsonRequestBehavior.AllowGet);

                    }
                }
                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.ControllerName = "Register";
                mobj.MethodName = "StudentRegister";
                mobj.Message = ex.Message;
                mobj.StackTrace = ex.StackTrace;
                mobj.CreatedDatetime = DateTime.Now;
                entity.tblExceptions.Add(mobj);
                entity.SaveChanges();

                return RedirectToAction("Index", "Exception");
            }
        }
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}