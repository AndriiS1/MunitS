using System.Text.Json;
using MunitS.Domain.Metadata.Exceptions;
namespace MunitS.Domain.Versioning;

public static class VersioningTools
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions() 
    { 
        WriteIndented = true
    };
    
    public static Versioning GetVersioning(VersioningPath path)
    {
        var rawVersioning = File.ReadAllText(path.Value);
        
        if(rawVersioning == null) throw new FileNotFoundException();
        
        var metadata = JsonSerializer.Deserialize<Versioning>(rawVersioning);
        
        if(metadata == null) throw new MetadataDeserializationException($"Cannot deserialize metadata from {path.Value}");
        
        return metadata;
    }
    
    public static async Task Write(this Versioning versioning)
    {
        var utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(versioning, Options);
        await File.WriteAllBytesAsync(versioning.Path.Value, utf8Bytes);
    }
}
