namespace VelocipedSite.Requests.V1.Products;

public record GetProductByIdRequest(long ProductId, long CategoryId, string ShopId);