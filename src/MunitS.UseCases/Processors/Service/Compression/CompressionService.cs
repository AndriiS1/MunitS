using System.IO.Compression;
namespace MunitS.UseCases.Processors.Service.Compression;

public class CompressionService : ICompressionService
{
    public byte[] CompressChunk(byte[] data)
    {
        using var outputMemoryStream = new MemoryStream();
        
        using (var gzipStream = new GZipStream(outputMemoryStream, CompressionLevel.Optimal))
        {
            gzipStream.Write(data, 0, data.Length);
        }
        
        return outputMemoryStream.ToArray();
    }
}
