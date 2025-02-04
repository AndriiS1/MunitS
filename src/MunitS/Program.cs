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

        var app = builder.Build();
        app.ConfigureUseCases();
        app.Run();
    }
}
