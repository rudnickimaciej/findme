using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(EmailAzureFunction.Startup))]

namespace EmailAzureFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<SmptConfiguration>()
                .Configure<IConfiguration>((smtpConfigurationSettings, configuration) =>
                {
                    configuration.GetSection(nameof(SmptConfiguration)).Bind(smtpConfigurationSettings);
                });

            builder.Services.AddSingleton<IEmailClient, EmailClient>();
        }
    }
}
