namespace VelocipedSite.Responses.V1.Profile;

public record RegisterResponse(bool IsSuccess, string Token, string ErrorMessage);