
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shared.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterRabbitMQ(this IServiceCollection services)
        {
            //Domain Bus
            //services.AddSingleton<IEventBus, RabbitMQBus>(sp =>
            //{
            //    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            //    return new RabbitMQBus("amqp://guest:guest@localhost:5672", scopeFactory);
            //});

            //Subscriptions
            //services.AddTransient<TransferEventHandler>();

            ////Domain Events
            //services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

            ////Domain Banking Commands
            //services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

            ////Application Services
            //services.AddTransient<IAccountService, AccountService>();
            //services.AddTransient<ITransferService, TransferService>();

            ////Data
            //services.AddTransient<IAccountRepository, AccountRepository>();
            //services.AddTransient<ITransferRepository, TransferRepository>();
            //services.AddTransient<BankingDbContext>();
            //services.AddTransient<TransferDbContext>();
        }
    }
}
