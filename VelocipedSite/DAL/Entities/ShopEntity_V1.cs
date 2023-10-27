namespace VelocipedSite.DAL.Entities;

public record ShopEntity_V1
{
    public long Id { get; init; }
    public string ShopId { get; init; } = "";
    public string Name { get; init; } = "";
    public string PathToImg { get; init; } = "";
    public decimal MinPrice { get; init; }
    public decimal DeliveryPrice { get; init; }
}