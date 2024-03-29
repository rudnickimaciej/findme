using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JwtTokenAuthentication
{


    public static class JwtExtensions
    {
        public const string SecurityKey = "epvSYyqLkHsugjG8KxtrEPazf3UmRVCb7cXnFWwNB6MJA42TDZsfbAUGKa6BWDJSxV4vcN9ynzFdHP2wMRk5ZrCTupYL3tqEheQjNPsdCaxbQtyq3LR6jAgZWreKuS4kDTBh8cnFzU7fYMmE5pHJ2vmvArfy9HD8FBE7TnsSLWC6RZqcw4eNpYhGzguQ2JUXKb3xjMPVKjeGCQvYpSE6ZtaxXFuDTRzN7LsA4qkHBc2WUwMJP3Vdmrby5g";

        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))
                };
            });
        }

    }
}