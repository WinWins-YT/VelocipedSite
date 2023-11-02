using VelocipedSite.DAL.Enums;

namespace VelocipedSite.DAL.Entities;

public record OrderEntityV1
{
    public long Id { get; init; }
    public OrderStatus Status { get; init; }
    public long UserId { get; init; }
    public DateTime Date { get; init; }
    public string Address { get; init; }
    public string Phone { get; init; }
    public ProductEntityV1[] Products { get; init; }
    public decimal TotalPrice { get; init; }
    public decimal SaleValue { get; init; }
}