using System.ComponentModel.DataAnnotations;
namespace MunitS.Infrastructure.Options.Storage;

public record StorageOptions
{
    public const string Section = "Storage";
    
    [Required]
    public required string RootDirectory { get; init; }

    [Required]
    public required string SignatureSecret { get; init; }

    [Required]
    public required string BaseUrl { get; init; }
}
