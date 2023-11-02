namespace VelocipedSite.Requests.V1.Orders;

public record CancelOrderRequest(long OrderId, string Token);