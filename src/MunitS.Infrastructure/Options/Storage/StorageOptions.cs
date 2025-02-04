using System.ComponentModel.DataAnnotations;
namespace MunitS.Infrastructure.Options.Storage;

public record StorageOptions
{
    public const string Section = "Storage";
    [Required]
    public required string RootDirectory { get; init; }
}
