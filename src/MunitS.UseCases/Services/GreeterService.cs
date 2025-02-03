using Grpc.Core;
using MunitS.Protos;
namespace MunitS.UseCases.Services;

public class GreeterService : Greeter.GreeterBase
{
    public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return new HelloReply
        {
            Message = "Hello " + request.Name
        };
    }
}
