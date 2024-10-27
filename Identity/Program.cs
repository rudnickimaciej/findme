using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Identity.Context;
using Identity.DI;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using JwtTokenAuthentication;
using Identity.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<TokenService>();

// var keyVaultUrl = builder.Configuration.GetSection("KeyVaultConfig:KeyVaultUrl");

// if (!Convert.ToBoolean(builder.Configuration["IsLocal"]))
// {
//     builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl.Value!.ToString()), new DefaultAzureCredential(
//         new DefaultAzureCredentialOptions { ManagedIdentityClientId = "2f512807-0c81-47b3-8909-6faf67099ab2" }//required when using user ManagedIdentity
// ));
// };

//var keyVaultUrl = builder.Configuration.GetSection("KeyVaultConfig:KeyVaultUrl");
//var tenantId = builder.Configuration.GetSection("KeyVaultConfig:TenantId");
//var clientId = builder.Configuration.GetSection("KeyVaultConfig:ClientId");
//var clientSecret = builder.Configuration.GetSection("KeyVaultConfig:ClientSecretId");
//var credential = new ClientSecretCredential(tenantId.Value!.ToString(), clientId.Value!.ToString(), clientSecret.Value!.ToString());

//builder.Configuration.AddAzureKeyVault(keyVaultUrl.Value!.ToString(), clientId.Value!.ToString(), clientSecret.Value!.ToString(), new DefaultKeyVaultSecretManager());
//var client = new SecretClient(new Uri(keyVaultUrl.Value!.ToString()), credential);
//var sqlconnection = builder.Configuration["sqlconnectionstring"];
////var sqlconnection = client.GetSecret("sqlconnectionstring").Value.Value.ToString();
//builder.Services.AddDbContext<DataContext>(options =>
//{
//    options.UseSqlServer(sqlconnection);
//});

//builder.Services.AddIdentityServices(builder.Configuration);
//builder.Services.AddDataProtection();
//builder.Services.AddJwtAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var logger = services.GetRequiredService<ILogger<Program>>();
// logger.LogInformation($"DefaultConnection: {builder.Configuration.GetConnectionString("DefaultConnection")}"); // Log "Test" message
// logger.LogInformation($"Azure Key Vault sqlconnectionstring: {sqlconnection.Substring(0, 10)}"); // Log "Test" message

// try
// {
//     var context = services.GetRequiredService<DataContext>();

//     var userManager = services.GetRequiredService<UserManager<AppUser>>();
//     await context.Database.MigrateAsync();
//     await Seed.SeedData(userManager);
// }
// catch (Exception ex)
// {
//     logger.LogError(ex, "An error occured during migration");
// }
app.Run();
app.UseDeveloperExceptionPage();
