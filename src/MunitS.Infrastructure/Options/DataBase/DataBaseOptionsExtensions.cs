using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace MunitS.Infrastructure.Options.DataBase;

public static class DataBaseOptionsExtensions
{
    public static void ConfigureDatabaseOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<DataBaseOptions>()
            .BindConfiguration(DataBaseOptions.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}
