using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
namespace MunitS.Infrastructure.Options.Storage;

public static class StorageOptionsExtensions
{
    public static void ConfigureStorageOptions(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<StorageOptions>()
            .BindConfiguration(StorageOptions.Section)
            .Validate(ValidateOptions)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }

    private static bool ValidateOptions(StorageOptions options)
    {
        return !Directory.Exists(options.RootDirectory) ? throw new ArgumentException($"Directory {options.RootDirectory} does not exist") : true;
    }   
}
