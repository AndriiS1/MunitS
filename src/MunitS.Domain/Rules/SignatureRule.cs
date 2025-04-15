namespace MunitS.Domain.Rules;

public static class SignatureRule
{
    public static string GetSignature(string bucketId, string objectId, string uploadId, int partNumber, long expiresAt)
    {
        return $"bucketId={bucketId}&objectId={objectId}&uploadId={uploadId}&partNumber={partNumber}&expiresAt={expiresAt}";
    }
}
