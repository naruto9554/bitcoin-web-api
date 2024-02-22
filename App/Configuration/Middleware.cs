using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
        app.UseSwaggerUI();

        if (env.IsProduction())
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
    }
}