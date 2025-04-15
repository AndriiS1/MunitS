using MediatR;
using Microsoft.AspNetCore.Http;
namespace MunitS.UseCases.Processors.Objects.Commands.UploadPart;

public sealed record UploadPartCommand(Guid BucketId, Guid UploadId, IFormFile PartData, int PartNumber) : IRequest<IResult>;
