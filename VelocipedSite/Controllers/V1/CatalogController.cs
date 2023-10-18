using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class CatalogController : ControllerBase
{
    private readonly ICatalogRepository _catalogRepository;
    private readonly ILogger<CatalogController> _logger;

    public CatalogController(ICatalogRepository catalogRepository, ILogger<CatalogController> logger)
    {
        _catalogRepository = catalogRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<GetCatalogCategoriesResponse> GetCatalogCategories(
        [FromQuery] GetCatalogCategoriesRequest request)
    {
        var categories = await _catalogRepository.Query(new CatalogQueryModel
        {
            ShopId = request.ShopId
        });

        _logger.LogInformation("Get all catalog categories succeeded");
        return new GetCatalogCategoriesResponse(categories
            .Select(x => new CatalogCategory(x.Id, x.Name, x.PathToImg)));
    }

    [HttpGet]
    public async Task<GetCatalogCategoryByIdResponse> GetCatalogCategoryById(
        [FromQuery] GetCatalogCategoryByIdRequest request)
    {
        var category = await _catalogRepository.QueryById(new CatalogQueryModel
        {
            Id = request.CatalogId
        });

        _logger.LogInformation("Get catalog category by ID {Id} succeeded", request.CatalogId);
        return new GetCatalogCategoryByIdResponse(new CatalogCategory(category.Id, category.Name, category.PathToImg));
    }
}