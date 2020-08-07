using HospitalIsa.BLL.Models;
using System.Net.Mail;

namespace Hospital.MailService
{
    public class MailService
    {

        public void SendEmail(RegisterPOCO user)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            try
            {
                using (SmtpServer)
                {
                    MailAddress adress = new MailAddress(user.Email);

                    mail.From = new MailAddress("hospital.isa@gmail.com");
                    mail.To.Add(adress);
                    if (user.EmailConfirmed == true)
                    {
                        mail.Subject = "Hospital - Registration request CONFIRMED";
                        mail.Body = "Vasa registracija je odobrena. Link za aktivaciju:" +

                                    "http://localhost:4200/";
                    }
                    else
                    {
                        mail.Subject = "Hospital - Registration request DENIED";
                        mail.Body = "ODBIJENO";
                    }
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("hospital.isa@gmail.com", "Hospital123");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    //MessageBox.Show("mail Send");
                    //Console.WriteLine("Mail Sent");
                }
            }
            finally
            {
                //if  (SmtpServer is IDisposable) myObject.Dispose();
                SmtpServer.Dispose();
            }

        }
    }
}

