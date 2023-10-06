namespace VelocipedSite.Responses.V1;

public record GetCatalogCategoriesResponse(CatalogCategory[] Categories);

public record CatalogCategory(int Id, string Name, string PathToImg);