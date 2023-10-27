namespace VelocipedSite.DAL.Models;

public record CreateTokenQuery
{
    public long UserId { get; init; }
}