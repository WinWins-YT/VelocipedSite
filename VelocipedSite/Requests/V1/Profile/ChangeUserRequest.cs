namespace VelocipedSite.Requests.V1.Profile;

public record UpdateUserRequest(string Token, string FirstName, string LastName, string Address, string Phone);