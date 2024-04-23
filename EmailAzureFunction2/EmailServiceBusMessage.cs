namespace EmailAzureFunction2
{
    public class EmailServiceBusMessage
    {
       public string EmailSender { get; set; }
        public string EmailReceiver { get; set; }
        public string EmailBody { get; set; }
        public string EmailSubject { get; set; }
        public string NotificationType { get; set; }
    }
}


//{
//    "EmailSender": "test sender"
//    "EmailReceiver": "test receiver"
//    "EmailBody": "test body"
//    "EmailSubject": "test subject"
//    "NotificationType": "EmailConfirmation"
//}