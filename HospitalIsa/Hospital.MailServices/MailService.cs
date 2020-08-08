using HospitalIsa.BLL.Models;
using System;
using System.Net.Mail;

namespace Hospital.MailService
{
    public class MailService
    {

        public void SendEmail(MailPOCO mailModel)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            try
            {
                using (SmtpServer)
                {
                    mail.Subject = mailModel.Subject;
                    mail.From = new MailAddress("isa.hospital2020@gmail.com");
                    mail.To.Add(new MailAddress(mailModel.Receiver));

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("isa.hospital2020@gmail.com", "Hospital123");
                    SmtpServer.EnableSsl = true;
                    
                    mail.Body = mailModel.Body;
                    
                    SmtpServer.Send(mail);
                    //MessageBox.Show("mail Send");
                    //Console.WriteLine("Mail Sent");
                }
            } catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //if  (SmtpServer is IDisposable) myObject.Dispose();
                SmtpServer.Dispose();
            }

        }
    }
}

