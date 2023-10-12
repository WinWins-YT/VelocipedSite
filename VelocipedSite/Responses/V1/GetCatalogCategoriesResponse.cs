namespace VelocipedSite.Responses.V1;

public record GetCatalogCategoriesResponse(IEnumerable<CatalogCategory> Categories);

public record CatalogCategory(long Id, string Name, string PathToImg);