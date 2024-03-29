using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EmailAzureFunction
{
    public  class Function1
    {
        private readonly IEmailClient _emailClient;

        public Function1(IEmailClient emailClient)
        {
            _emailClient = emailClient;
        }

        [FunctionName("Function1")]
        public  void Run([ServiceBusTrigger("SendEmailQueue", Connection = "Endpoint=sb://findpet.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=Tu63jsI98VKfPqiaGvYSswyhNqsBykCTP+ASbLpEoiY=")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            EmailServiceBusMessage message = JsonConvert.DeserializeObject<EmailServiceBusMessage>(myQueueItem);
            _emailClient.Send(message);
        }
    }
}
