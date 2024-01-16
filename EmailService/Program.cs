using EmailService;
using EmailService.RabbitMQ;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>(provider => { return new RabbitMQConsumer("amqp://guest:guest@localhost:5672"); });
        services.AddHostedService<Worker>();
    })
    .Build();


host.Run();
