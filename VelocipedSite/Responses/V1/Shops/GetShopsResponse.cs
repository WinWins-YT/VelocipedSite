using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1.Shops;

public record GetShopsResponse(IEnumerable<Shop> Shops);