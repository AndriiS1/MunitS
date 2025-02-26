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

        var app = builder.Build();
        app.ConfigureServiceProcessors();
        app.Run();
    }
}
