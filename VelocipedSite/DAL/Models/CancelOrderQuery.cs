namespace VelocipedSite.DAL.Models;

public record CancelOrderQuery
{
    public long OrderId { get; init; }
    public long UserId { get; init; }
}