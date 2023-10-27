namespace VelocipedSite.DAL.Entities;

public record ProductEntity_V1(long Id, string ShopId, long CategoryId, string Name,
    string Description, string PathToImg, decimal Price, 
    bool IsOnSale, DateTime SaleStart, DateTime SaleEnd, decimal SalePrice);