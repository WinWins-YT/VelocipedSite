namespace VelocipedSite.Responses.V1;

public record GetShopsResponse(IEnumerable<ShopResponse> Shops);

public record ShopResponse(string Name, string PathToImg, string Id);