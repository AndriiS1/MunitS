using MunitS.Infrastructure.Options.Storage;
using MunitS.UseCases;

namespace MunitS;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddGrpc();
        builder.ConfigureOptions();

        var app = builder.Build();
        app.ConfigureUseCases();
        app.Run();
    }
}
