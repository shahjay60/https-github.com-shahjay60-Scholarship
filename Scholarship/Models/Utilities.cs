using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Scholarship.Models
{
    public class Utilities
    {
        private string Host = ConfigurationManager.AppSettings["host"];
        private string Port = ConfigurationManager.AppSettings["port"];
        private string Email = ConfigurationManager.AppSettings["emailId"];
        private string Password = ConfigurationManager.AppSettings["password"];

        public void SendMail(string to, string subject, string Message)
        {
            MailMessage msgs = new MailMessage();
            msgs.To.Add(to);
            MailAddress address = new MailAddress(Email);
            msgs.From = address;
            msgs.Subject = subject;
            msgs.Body = Message;
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
        }
    }
}