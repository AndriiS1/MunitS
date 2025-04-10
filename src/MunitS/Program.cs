using MunitS.Apis.Objects;
using MunitS.Infrastructure;
using MunitS.UseCases;

namespace MunitS;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddGrpc();
        builder.ConfigureInfrastructure();
        builder.ConfigureUseCases();
        builder.ConfigureCors();

        var app = builder.Build();
        app.ConfigureServiceProcessors();
        
        app.UseCors();
        app.UseHttpsRedirection();
        app.MapObjectsEndpoints();
        
        app.Run();
    }
}
