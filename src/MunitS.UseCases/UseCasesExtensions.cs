using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MunitS.UseCases.Services;
namespace MunitS.UseCases;

public static class UseCasesExtensions
{
    public static void ConfigureUseCases(this WebApplication app)
    {
        app.ConfigureGrpcServices();
    }
    
    private static void ConfigureGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<StorageService>();
    }

    private static void AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<StorageService>());
    }
}
