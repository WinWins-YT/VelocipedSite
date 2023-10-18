namespace VelocipedSite.Models;

public record Product(long Id, long CategoryId, string ShopId, string Name, string Description, 
    string PathToImg, decimal Price);