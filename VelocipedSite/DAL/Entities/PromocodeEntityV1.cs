namespace VelocipedSite.DAL.Entities;

public record PromocodeEntityV1
{
    public long Id { get; init; }
    public string Code { get; init; }
    public bool IsForNew { get; init; }
    public decimal SaleValue { get; init; }
    public decimal SaleFrom { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}