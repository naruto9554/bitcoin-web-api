using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

public class IntegrationFixture : IDisposable
{
    public HttpClient Client { get; private set; }

    public IntegrationFixture()
    {
        var factory = new ApplicationFactory();
        Client = factory.CreateClient();
    }

    public void Dispose()
    {
        Client?.Dispose();
        GC.SuppressFinalize(this);
    }
}

public class ApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        return base.CreateHost(builder);
    }
}