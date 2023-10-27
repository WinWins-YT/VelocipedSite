namespace VelocipedSite.Requests.V1.Products;

public record GetProductsInCatalogRequest(long CatalogId, string ShopId);