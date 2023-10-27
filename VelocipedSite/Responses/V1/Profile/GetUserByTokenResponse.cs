using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1.Profile;

public record GetUserByTokenResponse
{
    public User User { get; init; }
    public bool IsValid { get; init; }
}