using JwtTokenAuthentication;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile("ocelot.K8S.json", false, false);

builder.Configuration.AddJsonFile("/app/ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddJwtAuthentication();

var app = builder.Build();

await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
