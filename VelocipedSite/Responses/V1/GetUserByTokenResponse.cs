using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1;

public record GetUserByTokenResponse
{
    public User User { get; init; }
    public bool IsValid { get; init; }
}