using GenericHttpClient;
using NugetPackage.MiddlewareLogs;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
       .WriteTo.Console()
       .CreateLogger();

// Add services to the container.

//*************************
builder.Services.AddLogMiddleware(builder.Configuration);

builder.Services.AddControllers();

//*************************
builder.Services.AddGenericHttpClient(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

//**************************
app.UseLogMiddleware();

Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Program>();
    });

app.Run();
public partial class Program { }