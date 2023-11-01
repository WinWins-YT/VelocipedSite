namespace VelocipedSite.DAL.Models;

public record UpdateUserQuery
{
    public long UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Address { get; init; }
    public string Phone { get; init; }
}