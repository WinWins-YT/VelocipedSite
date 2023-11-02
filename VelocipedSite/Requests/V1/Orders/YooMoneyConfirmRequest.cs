namespace VelocipedSite.Requests.V1.Orders;

public record YooMoneyConfirmRequest
{
    public string Label { get; init; }
    public string Withdraw_amount { get; init; }
}