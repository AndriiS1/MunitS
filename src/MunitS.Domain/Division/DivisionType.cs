namespace MunitS.Domain.Division;


public record DivisionType
{
    public enum SizeType
    {
        Large,
        Medium,
        Small
    }
    public SizeType Type { get; private set; }
    public int ObjectsAmountLimit { get; private set; }
    
    private const int LargeLimit = 10000;
    private const int MediumLimit = 1000;
    private const int SmallLimit = 100;
    
    private const int LargeObjectSizeInBytesLimit = 10000000;
    private const int MediumObjectSizeInBytesLimit = 100000000;

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
        ObjectsAmountLimit = typeData.Item2;
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
        ObjectsAmountLimit = objectLimit;
    }
};
