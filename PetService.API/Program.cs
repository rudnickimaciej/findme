using API.Extensions;
using Infra.IoC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetService.API.Extensions;
using PetService.API.FileAPI;
using PetService.Application.Profiles;
using PetService.Domain;
using PetService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);



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

logger.LogInformation("DefaultConnection:"); // Log "Test" message
logger.LogInformation(builder.Configuration.GetConnectionString("DefaultConnection")); // Log "Test" message

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
app.Run();
