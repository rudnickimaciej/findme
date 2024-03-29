using Infra.IoC;
using Microsoft.EntityFrameworkCore;
using PetService.API.Extensions;
using PetService.API.FileAPI;
using PetService.Application.Profiles;
using PetService.Infrastructure;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using PetService.API.Features.MissingPetPost;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = builder.Configuration.GetSection("KeyVaultConfig:KeyVaultUrl");
var tenantId = builder.Configuration.GetSection("KeyVaultConfig:TenantId");
var clientId = builder.Configuration.GetSection("KeyVaultConfig:ClientId");
var clientSecret = builder.Configuration.GetSection("KeyVaultConfig:ClientSecretId");
var credential = new ClientSecretCredential(tenantId.Value!.ToString(), clientId.Value!.ToString(), clientSecret.Value!.ToString());

builder.Configuration.AddAzureKeyVault(keyVaultUrl.Value!.ToString(), clientId.Value!.ToString(), clientSecret.Value!.ToString(), new DefaultKeyVaultSecretManager());
var client = new SecretClient(new Uri(keyVaultUrl.Value!.ToString()), credential);

var sqlconnection = client.GetSecret("sqlconnectionstring").Value.Value.ToString();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(sqlconnection);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(builder.Configuration);
// builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.RegisterRabbitMQ();
builder.Services.AddAutoMapper(typeof(MissingPetProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:3000") // Add your client's origin here
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
var fileAPIEndpoint = builder.Configuration["APIs:FileAPI"];
builder.Services.AddScoped<IUploadFilesService, UploadFilesService>();


builder.Services.AddHttpClient("FileAPI", client =>
   {
       client.BaseAddress = new Uri(fileAPIEndpoint);
   });
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowLocalhost");

app.MapControllers();


#region seed data
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();

logger.LogInformation($"DefaultConnection: {builder.Configuration.GetConnectionString("DefaultConnection")}"); // Log "Test" message
logger.LogInformation($"Azure Key Vault sqlconnectionstring: {sqlconnection.Substring(0, 10)}"); // Log "Test" message

try
{
    var context = services.GetRequiredService<DataContext>();

    // var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    Seed.SeedData(context);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}

#endregion

CreateMissingPetPost.MapEndpoint(app);
app.Run();
