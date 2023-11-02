namespace VelocipedSite.Requests.V1.Promocode;

public record GetPromocodeRequest(string Token, string Promocode, decimal Sum);