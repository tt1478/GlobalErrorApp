using GlobalErrorApp.Configurations;
using GlobalErrorApp.Data;
using GlobalErrorApp.IServices;
using GlobalErrorApp.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.Destructurers;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.SqlServer.Destructurers;
using Serilog.Formatting.Json;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.RollingFile;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
    .WithDefaultDestructurers()
    .WithDestructurers(new List<IExceptionDestructurer> { new SqlExceptionDestructurer(), new DbUpdateExceptionDestructurer() }))
  .MinimumLevel.Information()
  .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("SqlDatabaseConnectionString"), new MSSqlServerSinkOptions
  {
      TableName = "Logs",
      SchemaName = "dbo",
      AutoCreateSqlTable = true
  })
  .WriteTo.Sink(new RollingFileSink(
        @"C:\logs",
        new JsonFormatter(renderMessage: true), null, null))
  .CreateLogger();

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlDatabaseConnectionString")));
builder.Services.AddScoped<IDriverService, DriverService>();
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

app.UseGlobalErrorHandler();

app.Run();
