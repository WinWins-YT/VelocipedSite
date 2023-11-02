using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Entities;
using VelocipedSite.DAL.Enums;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1.Orders;
using VelocipedSite.Responses.V1.Orders;
using VelocipedSite.Utilities;

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

            await Mail.SendOrderCreatedEmail(user.Email,
                new Order(orderId, OrderStatus.Created, DateTime.Now, request.Address, request.Address,
                    products.Select(x => new Product(x.Id, x.CategoryId, x.ShopId, x.Name, x.Description, x.PathToImg,
                        x.Price, x.IsOnSale, x.SalePrice)).ToArray(), request.TotalPrice, request.SaleValue));
            
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

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> YooMoneyConfirm([FromForm] YooMoneyConfirmRequest request)
    {
        var appRoot = @"D:\test";
        try
        {
            _logger.LogInformation("Yoo money confirming order {OrderId} for {Amount}", request.Label, request.Withdraw_amount);
            await System.IO.File.AppendAllTextAsync(appRoot + @"\yoomoney.txt", $"Got POST for confirm: Label: {request.Label}, Amount: {request.Withdraw_amount}\n");
            
            var orderId = Convert.ToInt64(request.Label);
            var sum = Convert.ToDecimal(request.Withdraw_amount);

            var order = await _ordersRepository.GetOrderById(new OrderIdQuery
            {
                OrderId = orderId
            });

            await System.IO.File.AppendAllTextAsync(appRoot + @"\yoomoney.txt", $"Order sum: {order.TotalPrice}\n");
            if (sum == order.TotalPrice)
            {
                await _ordersRepository.ConfirmOrderPayed(new OrderIdQuery
                {
                    OrderId = orderId
                });
                await System.IO.File.AppendAllTextAsync(appRoot + @"\yoomoney.txt", "Order confirmed\n");
                _logger.LogInformation("Order {OrderId} for {Amount} is confirmed", orderId, request.Withdraw_amount);
                return Ok();
            }
            
            await System.IO.File.AppendAllTextAsync(appRoot + @"\yoomoney.txt", "Sum mismatch\n");
            _logger.LogError("Confirming order {OrderId} is failed: Sum mismatch", request.Withdraw_amount);
            return BadRequest();
        }
        catch (EntityNotFoundException exception)
        {
            await System.IO.File.AppendAllTextAsync(appRoot + @"\yoomoney.txt", $"Not found error: {exception}\n");
            _logger.LogError("Confirming order {OrderId} is failed: {Message}", request.Withdraw_amount, exception.Message);
            return NotFound(exception.Message);
        }
        catch (Exception exception)
        {
            await System.IO.File.AppendAllTextAsync(appRoot + @"\yoomoney.txt", $"Error: {exception}\n");
            _logger.LogError("Confirming order {OrderId} is failed: Internal Error, {Message}", request.Withdraw_amount, exception);
            return StatusCode(500, exception.Message);
        }
    }
}