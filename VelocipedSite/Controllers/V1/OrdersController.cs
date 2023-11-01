using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Requests.V1.Orders;
using VelocipedSite.Responses.V1.Orders;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrdersRepository ordersRepository, ILogger<OrdersController> logger)
    {
        _ordersRepository = ordersRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
    {
        
    }
}