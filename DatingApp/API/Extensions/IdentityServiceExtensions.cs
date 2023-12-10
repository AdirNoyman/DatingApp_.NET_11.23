
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            // Configure the authentication middleware to validate the token that was passed by the user, by validating the key that was passed (check if it is the same key that is stored in the server)
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

    }
}