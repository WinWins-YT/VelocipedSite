namespace VelocipedSite.DAL.Models;

public record UserQuery
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Address { get; init; }
    public string Phone { get; init; }
}