using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices();

var app = builder.Build();
app.ConfigureMiddleware(app.Environment);
app.MapEndpoints();

app.Run();