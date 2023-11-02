namespace VelocipedSite.DAL.Entities;

public record UserEntityV1
{
    public long Id { get; init; }
    public bool Activated { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Address { get; init; }
    public string Phone { get; init; }
}