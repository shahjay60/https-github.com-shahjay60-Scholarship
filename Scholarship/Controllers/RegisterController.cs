using System;
using System.Collections.Generic;
using System.Configuration;
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
        private string Host = ConfigurationManager.AppSettings["Host"];
        private string Port = ConfigurationManager.AppSettings["Port"];
        private string Email = ConfigurationManager.AppSettings["Email"];
        private string Password = ConfigurationManager.AppSettings["Password"];

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
                mdata.Password = RandomString(4, false);

                entity.tblStudentDetails.Add(mdata);
                entity.SaveChanges();
                int id = mdata.Id;
                if (id != 0)
                {
                    #region SendEmail & Password
                    //Log.Info("Register Mail started...");
                    try
                    {

                        MailMessage msgs = new MailMessage();
                        msgs.To.Add(mdata.EmailId);
                        MailAddress address = new MailAddress(Email);
                        msgs.From = address;
                        //   msgs.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
                        msgs.Subject = "Thanks for creating your account and welcome to EduXam.";
                        string htmlBody = "Your";
                        msgs.Body = htmlBody;
                        msgs.IsBodyHtml = true;
                        SmtpClient client = new SmtpClient();
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;

                        client.EnableSsl = false;

                        //client.Host = "smtp.gmail.com";
                        //client.Port = 587;
                        client.Host = Host;
                        client.Port = Convert.ToInt32(Port);

                        // client.Credentials = new System.Net.NetworkCredential("email@gmail.com", "pass@");
                        NetworkCredential credentials = new NetworkCredential(Email, Password);
                        client.UseDefaultCredentials = false;
                        client.Credentials = credentials;
                        //Send the msgs  
                        client.Send(msgs);

                    }
                    catch (Exception ex)
                    {
                        //Log.Error("Register Mail Failed..." + ex.Message.ToString());
                    }

                    //Log.Info("Register Mail end...");


                    #endregion
                }
                return Json(id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                tblException mobj = new tblException();
                mobj.ControllerName = "Register";
                mobj.ControllerName = "StudentRegister";
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