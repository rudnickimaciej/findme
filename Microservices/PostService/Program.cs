using PostService.Database;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using PetService.API.Features.MissingPetPost;
using Shared.Bus;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//Bus
builder.Services.AddSingleton<IEventBus>(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    return new RabbitMQBus(scopeFactory);
});

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
builder.Services.AddValidatorsFromAssembly(assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region seed
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var logger = services.GetRequiredService<ILogger<Program>>();

app.MapGet("api/missingpets", () =>
{
    return "MissingPets";
});
//try
//{
//    var context = services.GetRequiredService<DataContext>();

//    await context.Database.MigrateAsync();
//    Seed.SeedData(context);
//}
//catch (Exception ex)
//{
//    logger.LogError(ex, "An error occured during migration");
//}
//AddMissingPetPost.MapEndpoint(app);
//GetMissingPetPost.MapEndpoint(app);

#endregion
app.Run();

