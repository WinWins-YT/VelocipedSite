using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1.Catalog;

public record GetCatalogCategoriesResponse(IEnumerable<CatalogCategory> Categories);