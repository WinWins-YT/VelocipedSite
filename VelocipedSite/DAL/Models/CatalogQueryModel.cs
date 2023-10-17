namespace VelocipedSite.DAL.Models;

public record CatalogQueryModel
{
    public long Id { get; init; }
    public string ShopId { get; init; } = "";
}