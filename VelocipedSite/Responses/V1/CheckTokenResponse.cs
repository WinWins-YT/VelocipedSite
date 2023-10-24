namespace VelocipedSite.Responses.V1;

public record CheckTokenResponse
{
    public bool IsValid { get; init; }
    public DateTime ValidUntil { get; init; }
}