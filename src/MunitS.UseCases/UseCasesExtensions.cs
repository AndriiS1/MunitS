using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MunitS.UseCases.Processors.Buckets;
using MunitS.UseCases.Processors.Objects;
using MunitS.UseCases.Processors.Objects.Services;
using MunitS.UseCases.Processors.Objects.Services.ObjectBuilder;
using MunitS.UseCases.Processors.Service.Compression;
using MunitS.UseCases.Processors.Service.PathRetriever;
namespace MunitS.UseCases;

public static class UseCasesExtensions
{
    public static void ConfigureServiceProcessors(this WebApplication app)
    {
        app.MapGrpcService<ObjectsServiceProcessor>();
        app.MapGrpcService<BucketsServiceProcessor>();
    }
    
    public static void ConfigureUseCases(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatr();
        builder.Services.AddSingleton<IPathRetriever, PathRetriever>();
        builder.Services.AddScoped<ICompressionService, CompressionService>();
        builder.Services.AddScoped<IObjectsBuilder, ObjectsBuilder>();
    }

    private static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ObjectsServiceProcessor>());
    }
}
