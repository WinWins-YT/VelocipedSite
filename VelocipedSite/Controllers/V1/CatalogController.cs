using Microsoft.AspNetCore.Mvc;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;

    private readonly CatalogItemResponse[] _catalog =
    {
        new("shesterochka", new ItemResponse(0, "Бананы", "Желтые бананы", 
            "banana.jpg", 69.99)),
        new("shesterochka", new ItemResponse(1, "Яблоки", "Какие-то яблоки", 
            "apples.jpg", 99.99)),
        new("shesterochka", new ItemResponse(2, "Хлеб", "Обычный черный хлеб", 
            "bread.jpg", 39.99)),
        new("shesterochka", new ItemResponse(3, "Молоко", "Белое молоко", 
            "milk.jpeg", 79.99))
    };

    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }

    [HttpGet("[action]")]
    public GetCatalogResponse GetCatalog([FromQuery] GetCatalogRequest request)
    {
        return new GetCatalogResponse(_catalog.Where(x => x.ShopId == request.ShopId)
            .Select(x => x.Item).ToArray());
    }
}