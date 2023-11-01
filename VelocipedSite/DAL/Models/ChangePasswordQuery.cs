namespace VelocipedSite.DAL.Models;

public record ChangePasswordQuery
{
    public long UserId { get; init; }
    public string Password { get; init; }
}