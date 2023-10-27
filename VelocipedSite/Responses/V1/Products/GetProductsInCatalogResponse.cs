using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1.Products;

public record GetProductsInCatalogResponse(IEnumerable<Product> Products);