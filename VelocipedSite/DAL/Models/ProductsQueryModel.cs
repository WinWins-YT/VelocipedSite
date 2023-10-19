namespace VelocipedSite.DAL.Models;

public record ProductsQueryModel
{
    public string ShopId { get; init; }
    public long CategoryId { get; init; }
}