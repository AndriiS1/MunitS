using System.ComponentModel.DataAnnotations;
namespace MunitS.Infrastructure.Options.DataBase;

public class DataBaseOptions
{
    public const string Section = "DataBase";
    [Required]
    public required string ContactPoints { get; init; }
    [Required]
    public required int Port { get; init; }
    [Required]
    public required string KeySpace { get; init; }
    public required Tables Tables { get; init; }
}

public class Tables
{
    [Required]
    public required string Buckets { get; init; }
    [Required]
    public required string Metadata { get; init; }
    [Required]
    public required string Objects { get; init; }
    [Required]
    public required string Versions { get; init; }
}
