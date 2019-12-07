using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
namespace fucking_mail
{
    class Program
    {
        static void Main(string[] args)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("Gradskijj_R_I@mail.ru");
            mail.Subject = "Hell-0";
            mail.Body = "<font color=red>7 days</font>";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            mail.To.Add(new MailAddress("Gradskijj_R_I" + "@mail.ru"));
            //mail.To.Add(new MailAddress("tr_s" + "@mail.ru"));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "serv.domain.local";
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
    }
}
