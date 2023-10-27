namespace VelocipedSite.Requests.V1.Profile;

public record RegisterRequest(string Email, string Password, string FirstName, string LastName, string Address, string Phone);