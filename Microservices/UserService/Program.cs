using Shared;
using Shared.Bus;
using UserService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using UserService.Features.User;
using UserService.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//Identity

//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<DataContext>();

//builder.Services.AddIdentityServer()
//    .AddApiAuthorization<AppUser, DataContext>();
builder.Services.AddIdentity<AppUser, IdentityRole>()
 .AddEntityFrameworkStores<DataContext>()
 .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 8;
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

//.AddEntityFrameworkStores<DataContext>()
//.AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider);
//builder.Services.AddDataProtection();

//Bus
builder.Services.AddSingleton<IEventBus>(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    return new AzureServiceBus(scopeFactory);
});

var assembly = typeof(Program).Assembly;

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));

//FluentValidation
builder.Services.AddValidatorsFromAssembly(assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


Register.MapEndpoint(app);
Confirm.MapEndpoint(app);

app.Run();

