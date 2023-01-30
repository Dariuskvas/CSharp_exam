using System.Net;
using System.Net.Mail;

namespace RestaurantSystem.Services
{
    public class MailService
    {
        public void sendEmail(string mailService)
        {
            String SendMailFrom = "darius.kvasauskas@gmail.com";
            String SendMailTo = "darius.kvasauskas@gmail.com";
            String SendMailSubject = "Cekis uz pieetus";
            String SendMailBody = "Siunciama ceki is restorano. Aciu, kad valgete";

            try
            {
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                MailMessage email = new MailMessage();
                // START
                email.From = new MailAddress(SendMailFrom);
                email.To.Add(SendMailTo);
                email.CC.Add(SendMailFrom);
                email.Subject = SendMailSubject;
                email.Body = SendMailBody;

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(mailService);
                email.Attachments.Add(attachment);

                //END
                SmtpServer.Timeout = 5000;
                SmtpServer.EnableSsl = true;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(SendMailFrom, "robxtdftxixixzpk");
                SmtpServer.Send(email);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Cekis isiustas sekmingai");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
