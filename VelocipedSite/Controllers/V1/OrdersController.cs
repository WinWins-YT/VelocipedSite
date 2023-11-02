using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1.Orders;
using VelocipedSite.Responses.V1.Orders;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class OrdersController : ControllerBase
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IProductsRepository _productsRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrdersRepository ordersRepository, IProductsRepository productsRepository, 
        IProfileRepository profileRepository, ILogger<OrdersController> logger)
    {
        _ordersRepository = ordersRepository;
        _productsRepository = productsRepository;
        _profileRepository = profileRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request)
    {
        try
        {
            _logger.LogInformation("Creating order for user {Token}", request.Token);
            var p = request.Products.Select(x => _productsRepository.QueryById(new ProductQueryByIdModel
            {
                CategoryId = x.CategoryId,
                ProductId = x.Id,
                ShopId = x.ShopId
            }));

            var products = await Task.WhenAll(p);

            Guid.TryParse(request.Token, out var guid);
            var user = await _profileRepository.GetUserFromToken(new TokenQuery
            {
                Token = guid
            });

            var orderId = await _ordersRepository.CreateOrder(new OrderQuery
            {
                UserId = user.Id,
                Address = request.Address,
                Phone = request.Phone,
                Products = products,
                TotalPrice = request.TotalPrice,
                SaleValue = request.SaleValue
            });

            _logger.LogInformation("Order for user {Token} created successfully, OrderId: {OrderId}", request.Token, orderId);
            return new CreateOrderResponse(orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Creating order failed: {Message}", ex);
            return new CreateOrderResponse(-1);
        }
    }

    [HttpPost]
    public async Task<GetOrdersForUserResponse> GetOrdersForUser(GetOrdersForUserRequest request)
    {
        _logger.LogInformation("Getting orders for user {Token}", request.Token);
        Guid.TryParse(request.Token, out var guid);
        var user = await _profileRepository.GetUserFromToken(new TokenQuery
        {
            Token = guid
        });

        var orders = await _ordersRepository.GetOrdersForUser(new UserIdQuery
        {
            UserId = user.Id
        });

        _logger.LogInformation("Getting orders for user {Token} is succeeded", request.Token);
        return new GetOrdersForUserResponse(orders.Select(x => new Order(x.Id, x.Status, x.Date, x.Address, 
            x.Phone, x.Products.Select(y => new Product(y.Id, y.CategoryId, y.ShopId, y.Name, y.Description, 
                y.PathToImg, y.Price, y.IsOnSale, y.SalePrice)).ToArray(), x.TotalPrice, x.SaleValue)).ToArray());
    }

    [HttpPost]
    public async Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request)
    {
        try
        {
            _logger.LogInformation("Cancelling order {OrderId} for user {Token}", request.OrderId, request.Token);
            Guid.TryParse(request.Token, out var guid);
            var user = await _profileRepository.GetUserFromToken(new TokenQuery
            {
                Token = guid
            });

            await _ordersRepository.CancelOrder(new CancelOrderQuery
            {
                OrderId = request.OrderId,
                UserId = user.Id
            });

            _logger.LogInformation("Order {OrderId} for user {Token} is succeeded", request.OrderId, request.Token);
            return new CancelOrderResponse();
        }
        catch (EntityNotFoundException exception)
        {
            _logger.LogError("Cancelling order {OrderId} for user {Token} is failed: {Message}", request.OrderId, request.Token, exception.Message);
            return new CancelOrderResponse();
        }
    }
}