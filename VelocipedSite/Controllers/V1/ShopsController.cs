using Microsoft.AspNetCore.Mvc;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]")]
public class ShopsController : ControllerBase
{
    private readonly ILogger<ShopsController> _logger;

    private readonly ShopResponse[] _shops =
    {
        new("Шестерочка", "shet.jpg", "shesterochka"),
        new("Красное и Белое", "logokb-2022.jpg", "kb"),
        new("Ларек с шаурмой", "waypma.jpeg", "waypma"),
        new("Лисья дыра", "fox.png", "fox")
    };

    public ShopsController(ILogger<ShopsController> logger)
    {
        _logger = logger;
    }
    
    [HttpGet("[action]")]
    public GetShopsResponse GetShops([FromQuery] GetShopsRequest request)
    {
        return new GetShopsResponse(_shops);
    }

    [HttpGet("[action]")]
    public GetShopByIdResponse GetShopById([FromQuery] GetShopByIdRequest request)
    {
        return new GetShopByIdResponse(_shops.First(x => x.Id == request.Id));
    }
}