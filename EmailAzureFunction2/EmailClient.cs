using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailAzureFunction2
{
    public interface IEmailClient
    {
        Task Send(EmailServiceBusMessage message);
    }

    public class EmailClient : IEmailClient
    {
        private readonly ILogger _logger;
        //private readonly IOptions<> _smptConfiguration;

        public EmailClient(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Send(EmailServiceBusMessage message)
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
                    Body = message.EmailBody,
                    IsBodyHtml = true
                };

                //mailMessage.To.Add(message.EmailReceiver); //PROD
                mailMessage.To.Add("mrrudnickiunity@gmail.com"); //TEST

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
