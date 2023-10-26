namespace VelocipedSite.Models;

public record User(long Id, string Email, string Password, string FirstName, string LastName, string Address, string Phone);