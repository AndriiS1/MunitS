namespace MunitS.Domain.Division.DivisionCounter;

public class DivisionCounter
{
    public required Guid BucketId { get; init; }
    public required string Type { get; init; }
    public required Guid Id { get; init; }
    public required long ObjectsCount { get; init; }
}
