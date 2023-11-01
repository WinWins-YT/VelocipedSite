namespace VelocipedSite.Requests.V1.Profile;

public record ChangeUserPasswordRequest(string Token, string OldPassword, string NewPassword);