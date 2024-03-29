namespace EmailAzureFunction2
{
    internal class SmptConfiguration
    {
        public string SmptServer { get; set; }
        public int SmptPort { get; set; }
        public string SmptUsername { get; set; }
        public string SmptPassword { get; set; }
        public bool EnableSsl { get; set; }
    }
}


