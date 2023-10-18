using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1;

public record GetProductsInCatalogResponse(IEnumerable<Product> Products);