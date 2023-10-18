using Microsoft.AspNetCore.Mvc;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class ProductsController : ControllerBase
{
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<GetProductsInCatalogResponse> GetProductsInCategory(
        [FromQuery] GetProductsInCatalogRequest request)
    {
        return new GetProductsInCatalogResponse(new[]
        {
            new Product(0, 0, "shesterochka", "Бананы", "Желтые бананы", "banana.jpg", new decimal(69.99)),
            new Product(1, 0, "shesterochka", "Яблоки", "Какие-то яблоки", "apples.jpg", new decimal(99.99)),
            //new Product(2, 1, "shesterochka", "Хлеб", "Обычный черный хлеб","bread.jpg", new decimal(39.99)),
            //new Product(3, 2, "shesterochka", "Молоко", "Белое молоко", "milk.jpeg", new decimal(79.99))
        });
    }
}