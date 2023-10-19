namespace VelocipedSite.DAL.Models;

public record ProductQueryByIdModel
{
    public string ShopId { get; init; }
    public long CategoryId { get; init; }
    public long ProductId { get; init; }
}