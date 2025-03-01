using ZstdNet;
namespace MunitS.UseCases.Processors.Service.Compression;

public class CompressionService : ICompressionService
{
    public byte[] CompressChunk(byte[] data)
    {
        using var compressor = new Compressor();
        return compressor.Wrap(data);
    }
}
