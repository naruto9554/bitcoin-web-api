using Microsoft.AspNetCore.Builder;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();

var app = builder.Build();

app.ConfigureEndpoints();

app.ConfigureMiddleware();

app.Run();

public partial class Program { } // Reference for tests