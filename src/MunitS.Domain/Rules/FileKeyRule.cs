namespace MunitS.Domain.Rules;

public static class FileKeyRule
{
    public static string GetFileName(string fileKey)
    {
        var folderParts = fileKey.Split('/');
        var fileNameWithExtension = folderParts[^1];
        var nameParts = fileNameWithExtension.Split('.');
        var extension = GetExtension(fileKey);

        var fileNameWithoutExtension = nameParts.Length > 1
            ? string.Join(".", nameParts[..^1]) // everything except extension
            : fileNameWithExtension; // no extension present

        return $"{fileNameWithoutExtension}.{extension}";
    }

    public static string GetParentPrefix(string fileKey)
    {
        var parts = fileKey.Split('/');
        var folderParts = string.Join("/", parts[new Range(0, parts.Length - 1)]);
        var parentPrefix =  "/" + folderParts;

        if (folderParts.Length > 1)
        {
            parentPrefix += "/";
        }
        
        return parentPrefix; 
    }

    public static string GetExtension(string fileKey)
    {
        var parts = fileKey.Split('.');
        return parts[^1].ToLower();
    }
}
