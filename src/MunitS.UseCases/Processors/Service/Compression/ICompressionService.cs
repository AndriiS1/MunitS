namespace MunitS.UseCases.Processors.Service.Compression;

public interface ICompressionService
{
    byte[] CompressChunk(byte[] data);
}
