using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MunitS.Infrastructure.Data;
using MunitS.Infrastructure.Data.Repositories.Metadata;
using MunitS.Infrastructure.Options.DataBase;
using MunitS.Infrastructure.Options.Storage;
namespace MunitS.Infrastructure;

public static class InfrastructureExtensions
{
    public static void ConfigureInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddOptions();
    }
    
    private static void AddOptions(this WebApplicationBuilder builder)
    {
        builder.ConfigureStorageOptions();
        builder.ConfigureDatabaseOptions();
        builder.ConfigureDataBase();
        builder.ConfigureRepositories();
    }
    
    private static void ConfigureDataBase(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        
        var optionsSection = configuration.GetSection(DataBaseOptions.Section);
        var options = optionsSection.Get<DataBaseOptions>();
        
        builder.Services.AddSingleton(_ => new CassandraHelper(
            options!.ContactPoints,
            options.Port,
            options.KeySpace
        ));
    }

    private static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMetadataRepository, MetadataRepository>();
    }
}
