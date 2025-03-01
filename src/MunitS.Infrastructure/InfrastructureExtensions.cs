using Cassandra.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MunitS.Domain.Models;
using MunitS.Infrastructure.Data;
using MunitS.Infrastructure.Data.Repositories.Bucket;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Infrastructure.Data.Repositories.Metadata;
using MunitS.Infrastructure.Data.Repositories.Object;
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
        
        builder.Services.AddSingleton(_ => new CassandraConnector(
            options!.ContactPoints,
            options.Port,
            options.KeySpace
        ));
        
        MappingConfiguration.Global.Define<ModelsMapping>();
    }

    private static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBucketRepository, BucketRepository>();
        builder.Services.AddSingleton<IMetadataRepository, MetadataRepository>();
        builder.Services.AddSingleton<IObjectRepository, ObjectRepository>();
        builder.Services.AddSingleton<IDivisionRepository, DivisionRepository>();
    }
}
