using System.Text.Json;
using MunitS.Domain.Metadata.Exceptions;
namespace MunitS.Domain.Metadata;

public static class MetadataTools
{
    public static Metadata GetMetadata(MetadataPath path)
    {
        var stringMetadata = File.ReadAllText(path.Value);
        
        if(stringMetadata == null) throw new FileNotFoundException();
        
        var metadata = JsonSerializer.Deserialize<Metadata>(stringMetadata);
        
        if(metadata == null) throw new MetadataDeserializationException($"Cannot deserialize metadata from {path.Value}");
        
        return metadata;
    }
    
    private static void Write(this Metadata metadata)
    {
        var jsonMetadata = JsonSerializer.Serialize(metadata);
        File.WriteAllText(metadata.GetPath(), jsonMetadata);
    }
}
