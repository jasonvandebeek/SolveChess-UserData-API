using Microsoft.EntityFrameworkCore;
using SolveChess.DAL;
using SolveChess.DAL.Model;
using SolveChess.Logic.Service;
using SolveChess.Logic.ServiceInterfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(Environment.GetEnvironmentVariable("SolveChess_MySQLConnectionString") ?? throw new Exception("No connection string found in .env variables!"));
});

builder.Services.AddScoped<IUserService, UserService>(provider =>
{
    var dbContextOptions = provider.GetRequiredService<DbContextOptions<AppDbContext>>();
    return new UserService(new UserDataDAL(dbContextOptions));
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

app.MapControllers();

app.Run();
