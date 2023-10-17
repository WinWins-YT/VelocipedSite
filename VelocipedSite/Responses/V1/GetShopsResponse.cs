using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1;

public record GetShopsResponse(IEnumerable<ShopResponse> Shops);