using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices();

var app = builder.Build();

app.ConfigureEndpoints();

app.ConfigureMiddleware();

app.Run();

public partial class Program { } // Reference for tests