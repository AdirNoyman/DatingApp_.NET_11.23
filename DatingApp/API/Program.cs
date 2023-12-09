using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>

   {
       options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
   }

);

builder.Services.AddCors();
builder.Services.AddScoped<ITokenService, TokenService>();
// Configure the authentication middleware to validate the token that was passed by the user, by validating the key that was passed (check if it is the same key that is stored in the server)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

// Allow request coming form our client
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

// Check if the users has a valid token
app.UseAuthentication();
// Check what the user is authorized to do
app.UseAuthorization();

app.MapControllers();

app.Run();
