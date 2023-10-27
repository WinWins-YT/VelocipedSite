namespace VelocipedSite.DAL.Entities;

public record TokenEntity_V1
{
    public long Id { get; init; }
    public Guid Token { get; init; }
    public long UserId { get; init; }
    public DateTime ValidUntil { get; init; }
}