using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public sealed class ApiFixture : IDisposable
{
    public HttpClient Client { get; private set; }

    public ApiFixture()
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
}