namespace VelocipedSite.Responses.V1.Profile;

public record AuthenticateResponse(bool IsSuccess, string Token);