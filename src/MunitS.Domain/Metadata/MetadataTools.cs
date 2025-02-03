using System.Text.Json;
using MunitS.Domain.Metadata.Exceptions;
namespace MunitS.Domain.Metadata;

public static class MetadataTools
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions() 
    { 
        WriteIndented = true
    };
    
    public static Metadata GetMetadata(MetadataPath path)
    {
        var stringMetadata = File.ReadAllText(path.Value);
        
        if(stringMetadata == null) throw new FileNotFoundException();
        
        var metadata = JsonSerializer.Deserialize<Metadata>(stringMetadata);
        
        if(metadata == null) throw new MetadataDeserializationException($"Cannot deserialize metadata from {path.Value}");
        
        return metadata;
    }
    
    public static async Task Write(this Metadata metadata)
    {
        var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(metadata, Options);
        await File.WriteAllBytesAsync(metadata.Path.Value, utf8Bytes);
    }
}
