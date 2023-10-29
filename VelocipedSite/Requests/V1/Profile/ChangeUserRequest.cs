namespace VelocipedSite.Requests.V1.Profile;

public record ChangeUserRequest(string FirstName, string LastName, string Address, string Phone);