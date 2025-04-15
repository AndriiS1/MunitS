namespace MunitS.Domain.Rules;

public static class SignatureRule
{
    public static string GetSignature(string bucketId, string uploadId, int partNumber, long expiresAt)
    {
        return $"bucketId={bucketId}&uploadId={uploadId}&partNumber={partNumber}&expiresAt={expiresAt}";
    }
}
