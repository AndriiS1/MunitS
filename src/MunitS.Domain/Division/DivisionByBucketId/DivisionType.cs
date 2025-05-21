namespace MunitS.Domain.Division.DivisionByBucketId;

public record DivisionType
{
    public enum SizeType
    {
        Large,
        Medium,
        Small
    }
    public SizeType Type { get; private set; }
    public long ObjectsCountLimit { get; private set; }
    
    private const long LargeLimit = 10000;
    private const long MediumLimit = 1000;
    private const long SmallLimit = 100;
    
    private const long LargeObjectSizeInBytesLimit = 100000000;
    private const long MediumObjectSizeInBytesLimit = 1000000000;

    public static string GetSizeTypePrefix(SizeType sizeType)
    {
        return sizeType switch
        {
            SizeType.Large => "lg",
            SizeType.Medium => "md",
            _ => "sm" 
        };
    }
    
    public DivisionType(long objectSizeInBytes)
    {
        var typeData = objectSizeInBytes switch
        {
            <= LargeObjectSizeInBytesLimit => (SizeType.Large, LargeLimit),
            <=  MediumObjectSizeInBytesLimit => (SizeType.Medium, MediumLimit),
            _ => (SizeType.Small, SmallLimit) 
        };
        
        Type = typeData.Item1;
        ObjectsCountLimit = typeData.Item2;
    }
    
    public DivisionType(SizeType sizeType)
    {
        var objectLimit = sizeType switch
        {
            SizeType.Large => LargeLimit,
            SizeType.Medium => MediumLimit,
            _ => SmallLimit 
        };
        
        Type = sizeType;
        ObjectsCountLimit = objectLimit;
    }
};
