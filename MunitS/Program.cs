using MunitS.Options.Storage;
using MunitS.Services;

namespace MunitS;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddGrpc();
        builder.ConfigureOptions();

        var app = builder.Build();
        app.ConfigureGrpcServices();
        app.Run();
    }

    private static void ConfigureGrpcServices(this WebApplication app)
    {
        app.MapGrpcService<GreeterService>();
        app.MapGrpcService<StorageService>();
    }
}
