using VelocipedSite.DAL.Entities;

namespace VelocipedSite.DAL.Models;

public record OrderQuery
{
    public long UserId { get; init; }
    public string Address { get; init; }
    public string Phone { get; init; }
    public ProductEntity_V1[] Products { get; init; }
}