using Cassandra.Mapping;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MunitS.Domain.Bucket.BucketById;
using MunitS.Domain.Bucket.BucketByName;
using MunitS.Domain.Bucket.BucketCounter;
using MunitS.Domain.Division.DivisionByBucketId;
using MunitS.Domain.Division.DivisionCounter;
using MunitS.Domain.Object.ObjectByFileKey;
using MunitS.Domain.Object.ObjectByUploadId;
using MunitS.Domain.ObjectSuffix.ObjectSuffixByParentPrefix;
using MunitS.Domain.Part.PartByUploadId;
using MunitS.Infrastructure.Data;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByIdRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketByNameRepository;
using MunitS.Infrastructure.Data.Repositories.Bucket.BucketCounter;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionById;
using MunitS.Infrastructure.Data.Repositories.Division.DivisionCounters;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByFileKeyRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByUploadIdRepository;
using MunitS.Infrastructure.Data.Repositories.ObjectSuffix.ObjectSuffixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.Part.PartByUploadId;
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
        MappingConfiguration.Global.Define<BucketCountersMapping>();
        MappingConfiguration.Global.Define<DivisionsByBucketIdMapping>();
        MappingConfiguration.Global.Define<DivisionCountersMapping>();
        MappingConfiguration.Global.Define<PartsByUploadIdMapping>();
        MappingConfiguration.Global.Define<ObjectsByUploadIdMapping>();
        MappingConfiguration.Global.Define<ObjectsByFileKeyMapping>();
        MappingConfiguration.Global.Define<ObjectSuffixesByParentPrefixMapping>();
    }

    private static void ConfigureRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IObjectSuffixByParentPrefixRepository, ObjectSuffixByParentPrefixRepository>();
        builder.Services.AddSingleton<IPartByUploadIdRepository, PartByUploadIdRepository>();
        builder.Services.AddSingleton<IBucketByIdRepository, BucketByIdByIdRepository>();
        builder.Services.AddSingleton<IBucketCounterRepository, BucketCounterRepository>();
        builder.Services.AddSingleton<IBucketByNameRepository, BucketByIdByNameRepository>();
        builder.Services.AddSingleton<IObjectByUploadIdRepository, ObjectByUploadIdRepository>();
        builder.Services.AddSingleton<IObjectByFileKeyRepository, ObjectByFileKeyRepository>();
        builder.Services.AddSingleton<IDivisionByIdRepository, DivisionByIdRepository>();
        builder.Services.AddSingleton<IDivisionCounterRepository, DivisionCounterRepository>();
    }
}
