using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogRepository _catalogRepository;
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

    public CatalogController(ICatalogRepository catalogRepository, ILogger<CatalogController> logger)
    {
        _catalogRepository = catalogRepository;
        _logger = logger;
    }

    [HttpGet("[action]")]
    public async Task<GetCatalogCategoriesResponse> GetCatalogCategories([FromQuery] GetCatalogCategoriesRequest request)
    {
        var categories = await _catalogRepository.Query(new CatalogQueryModel
        {
            ShopId = request.ShopId
        });

        return new GetCatalogCategoriesResponse(categories
            .Select(x => new CatalogCategory(x.Id, x.Name, x.PathToImg)));

    }
}