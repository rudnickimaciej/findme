using System.Net.Mail;
using System.Net;

namespace Email
{
    public class EmailServiceBusMessage
    {
        public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string NotificationType { get; set; }
    }
    public interface IEmailClient
    {
        //Task Send(EmailServiceBusMessage message);
        Task Send(EmailServiceBusMessage message, string EmailHTMLBody = null);

    }

    public class EmailClient : IEmailClient
    {
        //private readonly IOptions<> _smptConfiguration;



        public async Task Send(EmailServiceBusMessage message, string EmailHTMLBody = null)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential("mrrudnickiunity@gmail.com", "eumr lwdg aryb crtm"),
                    EnableSsl = true
                };
                var mailMessage = new MailMessage
                {
                    //From = new MailAddress(message.EmailSender), //PROD
                    From = new MailAddress("mrrudnickiunity@gmail.com"),  //TEST
                    Subject = message.EmailSubject,
                    Body = EmailHTMLBody,
                    IsBodyHtml = true
                };

                //mailMessage.To.Add(message.EmailReceiver); //PROD
                mailMessage.To.Add("mrrudnickiunity@gmail.com"); //TEST

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
