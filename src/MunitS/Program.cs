using Microsoft.AspNetCore.Http.Features;
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
        
        builder.Services.Configure<FormOptions>(options =>
        {
            options.MultipartBodyLengthLimit = 5 * 1024 * 1024;
        });

        var app = builder.Build();
        app.ConfigureServiceProcessors();
        
        app.UseCors();
        app.UseHttpsRedirection();
        app.MapObjectsEndpoints();
        
        app.Run();
    }
}
