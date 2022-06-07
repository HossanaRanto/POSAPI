using Microsoft.EntityFrameworkCore;
using POSAPI.Data;
using POSAPI.Helper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
var connectionstring = "Server=localhost;User=root;Password=;Database=PosDb";
builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring));
});
builder.Services.AddScoped<JwtHelper>();
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<JwtHelper>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options
            .WithOrigins(new[] { "http://localhost:3000" })
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

app.UseAuthorization();

app.MapControllers();

app.Run();
