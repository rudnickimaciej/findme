using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Reflection;
using System.Resources;
using Microsoft.Azure.Functions.Worker;
using RabbitMQ.Client.Events;

namespace EmailAzureFunction2
{
    public class Function1
    {
        private readonly IEmailClient _emailClient;
        private readonly ResourceManager _resourceManager = new ResourceManager("EmailAzureFunction2.Templates", Assembly.GetExecutingAssembly());


        public Function1(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        [FunctionName("Function1")]
        public void Run([ServiceBusTrigger("sendemailqueue", Connection = "azureservicebusconnection", AutoCompleteMessages = true)] string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            EmailServiceBusMessage message = JsonConvert.DeserializeObject<EmailServiceBusMessage>(myQueueItem);
            string emailTemplate = GetEmailTemplate(message.NotificationType);

            _emailClient.Send(message, emailTemplate);
        }
        //[Function("Function2")]
        //public void Run2([RabbitMQTrigger("email-queue", HostName = "localhost")] BasicDeliverEventArgs args, ILogger log)
        //{
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {args}");
        //    EmailServiceBusMessage message = JsonConvert.DeserializeObject<EmailServiceBusMessage>(args.Body.ToString());
        //    string emailTemplate = GetEmailTemplate(message.NotificationType);

        //    _emailClient.Send(message, emailTemplate);
        //}
        private string GetEmailTemplate(string notificationType)
        {
            string emailTemplate = LoadResource(notificationType);
            return emailTemplate;
        }

        private string LoadResource(string resourceName)
        {
            string templatePath = $"./Templates/{resourceName}.html";
            string emailTemplate = File.ReadAllText(templatePath);
            return emailTemplate;
        }


    }
}
