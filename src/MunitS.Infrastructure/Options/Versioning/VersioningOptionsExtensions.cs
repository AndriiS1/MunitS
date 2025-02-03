using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace MunitS.Infrastructure.Options.Versioning;

public static class VersioningOptionsExtensions
{
    public static void ConfigureOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<VersioningOptions>()
            .BindConfiguration(VersioningOptions.Section)
            .ValidateOnStart();
    }
}
