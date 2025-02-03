namespace MunitS.Domain.Metadata.Exceptions;

public class MetadataDeserializationException: Exception
{
    public MetadataDeserializationException()
    {
    }

    public MetadataDeserializationException(string message)
        : base(message)
    {
    }

    public MetadataDeserializationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
