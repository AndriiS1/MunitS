namespace MunitS.Domain.Directory;

public abstract class DirectoryBase
{
    public abstract string Value { get; }

    public override string ToString()
    {
        return Value;
    }
}
