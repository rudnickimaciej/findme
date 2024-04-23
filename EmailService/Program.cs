using EmailService;
using EmailService.RabbitMQ;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services)=>
    {
        IConfiguration configuration = hostContext.Configuration;
        string busConnectionString = configuration["BusConnectionString"];
        services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>(provider => { return new RabbitMQConsumer("busConnectionString"); });
        services.AddHostedService<Worker>();
    })
    .Build();

host.Run();
