using VelocipedSite.Models;

namespace VelocipedSite.Responses.V1.Orders;

public record GetOrdersForUserResponse(Order[] Orders);