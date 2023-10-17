using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]")]
public class ShopsController : ControllerBase
{
    private readonly IShopsRepository _shopsRepository;
    private readonly ILogger<ShopsController> _logger;

    public ShopsController(IShopsRepository shopsRepository, ILogger<ShopsController> logger)
    {
        _shopsRepository = shopsRepository;
        _logger = logger;
    }
    
    [HttpGet("[action]")]
    public async Task<GetShopsResponse> GetShops([FromQuery] GetShopsRequest request)
    {
        var shops = await _shopsRepository.QueryAll();
        _logger.LogInformation("Get all shops succeeded");
        return new GetShopsResponse(shops
            .Select(x => new ShopResponse(x.Name, x.PathToImg, x.ShopId)));
    }

    [HttpGet("[action]")]
    public async Task<GetShopByIdResponse> GetShopById([FromQuery] GetShopByIdRequest request)
    {
        var shop = await _shopsRepository.QueryById(new ShopsQueryModel
        {
            ShopId = request.Id
        });
        _logger.LogInformation("Get shop by ID succeeded");
        return new GetShopByIdResponse(new ShopResponse(shop.Name, shop.PathToImg, shop.ShopId));
    }
}