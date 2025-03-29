using MediatR;
using MunitS.Infrastructure.Data.Repositories.FolderPrefix.FolderPrefixByParentPrefixRepository;
using MunitS.Infrastructure.Data.Repositories.Object.ObjectByParentPrefixRepository;
using MunitS.Protos;
using MunitS.UseCases.Processors.Objects.Mappers;
namespace MunitS.UseCases.Processors.Objects.Queries.GetObjects;

public class UploadFileCommandHandler(IObjectByParentPrefixRepository objectByParentPrefixRepository,
    IFolderPrefixByParentPrefixRepository folderPrefixByParentPrefixRepository): IRequestHandler<GetObjectsQuery, GetObjectsByPrefixResponse>
{
    public async Task<GetObjectsByPrefixResponse> Handle(GetObjectsQuery query, CancellationToken cancellationToken)
    {
        var getObjectsTasks = objectByParentPrefixRepository.GetAll(Guid.Parse(query.Request.BucketId), query.Request.Prefix);
        
        var getFoldersTask = folderPrefixByParentPrefixRepository.GetAll(Guid.Parse(query.Request.BucketId), query.Request.Prefix);
        
        await Task.WhenAll(getObjectsTasks, getFoldersTask);
        
        return ObjectResponseMappers.FormatGetBucketResponse(await getObjectsTasks, await getFoldersTask);
    }
}
