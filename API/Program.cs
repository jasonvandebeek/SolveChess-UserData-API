using Microsoft.EntityFrameworkCore;
using SolveChess.DAL.Model;
using SolveChess.Logic.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SolveChess.Logic.Interfaces;
using SolveChess.API.Exceptions;
using SolveChess.Logic.DAL;
using SolveChess.DAL;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    var mysqlConnectionString = Environment.GetEnvironmentVariable("SolveChess_MySQLConnectionString") ?? throw new MissingEnvVariableException("No connection string found in .env variables!");

    options.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString));
});

builder.Services.AddScoped<IUserDataDal, UserDataDal>();

builder.Services.AddScoped<HttpClient>();

builder.Services.AddScoped<IUserService, UserService>();

string origins = Environment.GetEnvironmentVariable("SolveChess_CorsUrls") ?? throw new MissingEnvVariableException("No cors urls found in .env variables!");
string[] originArray = origins.Split(';');

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", builder => builder.WithOrigins(originArray)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSecret = Environment.GetEnvironmentVariable("SolveChess_JwtSecret") ?? throw new MissingEnvVariableException("No jwt secret string found in .env variables!");

    var tokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidIssuer = "SolveChess Authenticator",
        ValidAudience = "SolveChess API",
    };

    options.TokenValidationParameters = tokenValidationParameters;
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["AccessToken"];
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowOrigin");

app.MapControllers();

app.Run();
