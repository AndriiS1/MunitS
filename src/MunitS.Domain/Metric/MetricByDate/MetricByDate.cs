using Cassandra;
namespace MunitS.Domain.Metric.MetricByDate;

public class MetricByDate
{
    private static readonly Dictionary<Operation, OperationType> OperationTypes = new()
    {
        {
            Metric.MetricByDate.Operation.InitiateMultipartUpload, OperationType.A
        },
        {
            Metric.MetricByDate.Operation.UploadPart, OperationType.A
        },
        {
            Metric.MetricByDate.Operation.CompleteMultipartUpload, OperationType.A
        },
        {
            Metric.MetricByDate.Operation.ListObjects, OperationType.B
        }
    };

    public required Guid BucketId { get; init; }
    public required string Type { get; init; }
    public required string Operation { get; init; }
    public required Guid Id { get; init; }
    public required LocalDate Date { get; init; }
    public required DateTimeOffset Time { get; init; }

    public static MetricByDate Create(Guid bucketId, Operation operation)
    {
        var id = Guid.NewGuid();
        return new MetricByDate
        {
            Id = id,
            BucketId = bucketId,
            Operation = operation.ToString(),
            Type = OperationTypes[operation].ToString(),
            Date = LocalDate.Parse(DateTime.UtcNow.ToString("yyyy-MM-dd")),
            Time = DateTime.UtcNow
        };
    }
}
