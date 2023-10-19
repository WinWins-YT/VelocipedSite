namespace VelocipedSite.Requests.V1;

public record GetProductByIdRequest(long ProductId, long CategoryId, string ShopId);