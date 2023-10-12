using Microsoft.AspNetCore.Mvc;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;

    private readonly ItemResponse[] _catalog =
    {
        new("shesterochka", 0, 0, "Бананы", "Желтые бананы", 
            "banana.jpg", 69.99),
        new("shesterochka", 1, 0, "Яблоки", "Какие-то яблоки", 
            "apples.jpg", 99.99),
        new("shesterochka", 2, 1, "Хлеб", "Обычный черный хлеб", 
            "bread.jpg", 39.99),
        new("shesterochka", 3, 2, "Молоко", "Белое молоко", 
            "milk.jpeg", 79.99)
    };

    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }

    [HttpGet("[action]")]
    public GetCatalogCategoriesResponse GetCatalogCategories([FromQuery] GetCatalogCategoriesRequest request)
    {
        return request.ShopId switch
        {
            "shesterochka" => new GetCatalogCategoriesResponse(new[]
            {
                new CatalogCategory(0, "Фрукты", "banana.jpg"),
                new CatalogCategory(1, "Хлебобулочные изделия", "bread.jpg"),
                new CatalogCategory(2, "Молочные продукты", "milk.jpeg")
            }),
            _ => new GetCatalogCategoriesResponse(Array.Empty<CatalogCategory>())
        };
    }
}