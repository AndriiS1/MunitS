using Microsoft.AspNetCore.Builder;
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
}
