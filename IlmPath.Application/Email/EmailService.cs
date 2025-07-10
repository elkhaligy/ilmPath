using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IlmPath.Application.Email
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("dyaaelabasiry@gmail.com", "pdfw yndz urap pldi");
                client.EnableSsl = true; 

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("dyaaelabasiry@gmail.com", "IlmPath"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
