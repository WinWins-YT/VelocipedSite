namespace VelocipedSite.Responses.V1.Promocode;

public record GetPromocodeResponse(bool IsValid, decimal SaleValue, string ErrorMessage);