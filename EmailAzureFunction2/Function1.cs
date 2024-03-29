using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EmailAzureFunction2
{
    public class Function1
    {
        private readonly IEmailClient _emailClient;

        public Function1(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        [FunctionName("Function1")]
        public void Run([ServiceBusTrigger("sendemailqueue", Connection = "servicebusconnection", AutoCompleteMessages = true)]string myQueueItem, ILogger log )
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            EmailServiceBusMessage message = JsonConvert.DeserializeObject<EmailServiceBusMessage>(myQueueItem);
            _emailClient.Send(message);
        }
    }
}
