using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CleanArchitecture.Application.FunctionalTests;

public class WebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Action<IWebHostBuilder>? _builderOptions;

    private WebApplicationFactory()
    {
    }

    private WebApplicationFactory(Action<IWebHostBuilder> builderConfiguration)
    {
        _builderOptions = builderConfiguration;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (_builderOptions is not null)
        {
            _builderOptions(builder);
        }
    }

    public static WebApplicationFactory Create() => new();

    public static WebApplicationFactory Create(Action<IWebHostBuilder> options) => new(options);
}
