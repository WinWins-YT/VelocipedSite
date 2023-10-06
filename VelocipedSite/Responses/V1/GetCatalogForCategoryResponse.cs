namespace VelocipedSite.Responses.V1;

public record GetCatalogForCategoryResponse(ItemResponse[] Items);

public record ItemResponse(string ShopId, long Id, long CategoryId, string Name, string Description, 
    string PathToImg, double Price);