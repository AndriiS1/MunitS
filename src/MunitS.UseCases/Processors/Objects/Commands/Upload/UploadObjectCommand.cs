using MediatR;
using Microsoft.AspNetCore.Http;
namespace MunitS.UseCases.Processors.Objects.Commands.Upload;

public sealed record UploadObjectCommand(Guid BucketId, Guid UploadId, IFormFile PartData, int PartNumber) : IRequest<IResult>;
