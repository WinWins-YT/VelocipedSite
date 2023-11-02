namespace VelocipedSite.DAL.Entities;

public record ProductEntityV1
{
    public long Id { get; init; } 
    public string ShopId { get; init; } 
    public long CategoryId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; } 
    public string PathToImg { get; init; } 
    public decimal Price { get; init; } 
    public bool IsOnSale { get; init; }
    public DateTime SaleStart { get; init; }
    public DateTime SaleEnd { get; init; }
    public decimal SalePrice { get; init; }
}