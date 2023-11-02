using VelocipedSite.DAL.Enums;

namespace VelocipedSite.Models;

public record Order(long Id, OrderStatus Status, DateTime Date, string Address, string Phone, Product[] Products, decimal TotalPrice, decimal SaleValue);