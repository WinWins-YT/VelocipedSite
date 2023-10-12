namespace VelocipedSite.DAL.Entities;

public record ShopEntityV1
{
    public long Id { get; init; }
    public string ShopId { get; init; } = "";
    public string Name { get; init; } = "";
    public string PathToImg { get; init; } = "";
}