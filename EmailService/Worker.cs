using System.Net;
using System.Net.Mail;
using EmailService.RabbitMQ;
using Microsoft.VisualBasic;

namespace EmailService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var rabbitMQConsumer = scope.ServiceProvider.GetRequiredService<IRabbitMQConsumer>();
                rabbitMQConsumer.ConsumeMessages("email_queue", SendEmail);
            }
        }
    }
    private async Task SendEmail(string message)
    {
        // Your message handling logic
        _logger.LogInformation("[{Timestamp}] {Message}", DateTimeOffset.Now, message);
        var smtpClient = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            Credentials = new NetworkCredential("mrrudnickiunity@gmail.com", "eumr lwdg aryb crtm"),
            EnableSsl = true
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress("mrrudnickiunity@gmail.com"),
            Subject = "test",
            Body = message,
            IsBodyHtml = true
        };

        mailMessage.To.Add("mrrudnickiunity@gmail.com");
        await smtpClient.SendMailAsync(mailMessage);
    }
}
