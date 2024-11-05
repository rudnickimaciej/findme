using JwtTokenAuthentication;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Configuration;
using Ocelot.Configuration.Repository;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile("ocelot.K8S.json", false, false);

// Add this to load ocelot.json or any other specific configuration file
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddJwtAuthentication();

var app = builder.Build();
app.MapWhen(context => context.Request.Path.StartsWithSegments("/routes"), appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var configRepo = app.Services.GetRequiredService<IInternalConfigurationRepository>();
        var config =  configRepo.Get();

        if (config.IsError)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Failed to retrieve routes");
            return;
        }

    var routes = config.Data.Routes.Select(route => new
        {
            UpstreamPathTemplate = route.UpstreamTemplatePattern?.OriginalValue, // UpstreamPathTemplate
            UpstreamHttpMethod = route.UpstreamHttpMethod,                       // UpstreamHttpMethod
            DownstreamScheme = route.DownstreamRoute[0].DownstreamScheme,        // DownstreamScheme
            DownstreamPathTemplate = route.DownstreamRoute[0].DownstreamHostAndPorts.Value, // DownstreamPathTemplate
            // DownstreamHostAndPorts = route.DownstreamRoute[0]..Select(hostPort => new
            // {
            //     Host = hostPort,                                            // Downstream Host
            //     Port = hostPort.Port                                             // Downstream Port
            // })
        });


        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(routes);
    });
});
await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();


app.Run();

