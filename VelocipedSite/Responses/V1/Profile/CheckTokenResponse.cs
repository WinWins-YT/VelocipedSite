namespace VelocipedSite.Responses.V1.Profile;

public record CheckTokenResponse
{
    public bool IsValid { get; init; }
    public DateTime ValidUntil { get; init; }
}