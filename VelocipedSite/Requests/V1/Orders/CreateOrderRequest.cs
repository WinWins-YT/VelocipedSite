using VelocipedSite.Models;

namespace VelocipedSite.Requests.V1.Orders;

public record CreateOrderRequest(string Token, string Address, string Phone, Product[] Products, decimal TotalPrice, decimal SaleValue);