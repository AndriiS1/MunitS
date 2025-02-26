using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MunitS.UseCases.Services.Buckets;
using MunitS.UseCases.Services.Objects;
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
    }

    private static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ObjectsServiceProcessor>());
    }
}
