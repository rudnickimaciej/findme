using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PetService.Application.Core;
using PetService.Application.MissingPets;
using PetService.Infrastructure;
using PetService.API.Options;
using Infra.IoC;

namespace PetService.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(List.Handler).Assembly));
            var assembly = typeof(Program).Assembly;
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembly));
            services.AddValidatorsFromAssembly(assembly);

            services.RegisterRabbitMQ();
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();
            services.Configure<APIOptions>(configuration.GetSection("EmailVerification"));
            //services.Configure<EmailOptions>(configuration.GetSection("EmailSettings"));
            //services.AddScoped<IEmailSender, EmailSender>();

            return services;
        }

    }
}
