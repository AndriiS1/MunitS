using Cassandra.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MunitS.Domain.Bucket.BucketById;
using MunitS.Domain.Bucket.BucketByName;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.FolderPrefixes.FolderPrefixByParentPrefix;
using MunitS.Domain.Metadata.MedataByObjectId;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByParentPrefix;
using MunitS.Infrastructure.Data;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Infrastructure.Data.Repositories.Division;
using MunitS.Infrastructure.Data.Repositories.FolderPrefix.FolderPrefixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.Metadata;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByParentPrefixRepository;
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

        MappingConfiguration.Global.Define<BucketsByIdMapping>();
        MappingConfiguration.Global.Define<BucketsByNameMapping>();
        MappingConfiguration.Global.Define<DivisionsByBucketIdMapping>();
        MappingConfiguration.Global.Define<MetadataByObjectIdMapping>();
        MappingConfiguration.Global.Define<ObjectsByFileKeyMapping>();
        MappingConfiguration.Global.Define<ObjectsByParentPrefixMapping>();
        MappingConfiguration.Global.Define<FolderPrefixByParentPrefixMapping>();
    }

    private static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBucketByIdRepository, BucketByIdByIdRepository>();
        builder.Services.AddSingleton<IBucketByNameRepository, BucketByIdByNameRepository>();
        builder.Services.AddSingleton<IMetadataByObjectIdRepository, MetadataByObjectIdRepository>();
        builder.Services.AddSingleton<IObjectByFileKeyRepository, ObjectByFileKeyRepository>();
        builder.Services.AddSingleton<IObjectByParentPrefixRepository, ObjectByParentPrefixRepository>();
        builder.Services.AddSingleton<IDivisionRepository, DivisionRepository>();
        builder.Services.AddSingleton<IFolderPrefixByParentPrefixRepository, FolderPrefixByIdByParentPrefixRepository>();
    }
}
