namespace MunitS.Domain.Rules;

public static class FileKeyRule
{
    public static string GetFileName(string fileKey)
    {
        var parts = fileKey.Split('/');
        return parts[^1];
    }

    public static string GetParentPrefix(string fileKey)
    {
        var parts = fileKey.Split('/');
        return string.Join("/", parts[new Range(0, parts.Length - 1)]);
    }

    public static string GetExtension(string fileKey)
    {
        var parts = fileKey.Split('.');
        return parts[^1];
    }
}
