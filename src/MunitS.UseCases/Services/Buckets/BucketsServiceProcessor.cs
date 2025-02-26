using MediatR;
namespace MunitS.UseCases.Services.Buckets;

public class BucketsServiceProcessor(IMediator mediator) : Protos.BucketsService.BucketsServiceBase
{
}

