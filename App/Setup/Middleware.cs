using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

public static class Middleware
{
    public static void ConfigureMiddleware(this WebApplication app)
    {
        var env = app.Environment;

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();

        app.UseOutputCache();

        app.UseSwagger();
        app.UseSwaggerUI(opt =>
        {
            var descriptions = app.DescribeApiVersions();
            foreach (var desc in descriptions)
            {
                var url = $"/swagger/{desc.GroupName}/swagger.json";
                var name = desc.GroupName.ToUpperInvariant();
                opt.SwaggerEndpoint(url, name);
            }
            opt.RoutePrefix = string.Empty;
        });

        app.MapHealthChecks("health");

        if (env.IsProduction())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");
            context.Response.Headers.Append("Referrer-Policy", "no-referrer");
            await next();
        });

        app.UseRateLimiter();
    }
}