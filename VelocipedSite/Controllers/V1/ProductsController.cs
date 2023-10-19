using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class ProductsController : ControllerBase
{
    private readonly IProductsRepository _productsRepository;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductsRepository productsRepository, ILogger<ProductsController> logger)
    {
        _productsRepository = productsRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<GetProductsInCatalogResponse> GetProductsInCategory(
        [FromQuery] GetProductsInCatalogRequest request)
    {
        var products = await _productsRepository.Query(new ProductsQueryModel
        {
            ShopId = request.ShopId,
            CategoryId = request.CatalogId
        });
        
        _logger.LogInformation("Get products in category {Id}", request.CatalogId);
        return new GetProductsInCatalogResponse(products.Select(x =>
            new Product(x.Id, x.CategoryId, x.ShopId, x.Name, x.Description, x.PathToImg, x.Price)));
    }

    [HttpGet]
    public async Task<GetProductByIdResponse> GetProductById(
        [FromQuery] GetProductByIdRequest request)
    {
        var product = await _productsRepository.QueryById(new ProductQueryByIdModel
        {
            ShopId = request.ShopId,
            CategoryId = request.CategoryId,
            ProductId = request.ProductId
        });

        _logger.LogInformation("Get product by ID {Id}", request.ProductId);
        return new GetProductByIdResponse(new Product(product.Id, product.CategoryId, product.ShopId, 
            product.Name, product.Description, product.PathToImg, product.Price));
    }
}