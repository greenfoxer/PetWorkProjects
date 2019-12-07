using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;

namespace Messenger
{
    public class messenger
    {
        List<string> emails;
        string header;
        string message;
        public messenger(List<string> emails, string header, string message)
        {
            this.emails = new List<string>(emails);
            this.header = header;
            this.message = message;
        }
        public string Send()
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("SSS-robot@goznak.ru", "SSS");
            mail.Subject = header;
            mail.Body = "<div>" + message + "</div>" +
                "<div style=\"margin-top:50px; border-top:1px solid #bbb; color:#bbb; font-size:0.8em;\">Это письмо отправлено роботом. Пожалуйста, не отвечайте на него." +
                "<br/>По вопросам формирования отчета обращайтесь в <span style=\"font-weight:bold\">Отдел промышленной безопасности, охраны труда и экологии МТГ - филиал АО \"Гознак\"</span></div>";
            mail.IsBodyHtml = true;
            foreach (var us in emails)
            {
                mail.To.Add(new MailAddress(us));
            }
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "01exch01.gz.local";
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
                return ("success");
            }
            catch (Exception e)
            {
                return (e.ToString());
            }
        }
    }
}
