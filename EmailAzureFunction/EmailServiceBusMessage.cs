namespace EmailAzureFunction
{
    public class EmailServiceBusMessage
    {
       public string  EmailSender { get; }
        public string EmailReceiver { get; }
        public string EmailBody { get; }
        public string EmailSubject { get; }
    }
}
