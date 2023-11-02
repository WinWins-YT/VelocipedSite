using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Requests.V1.Promocode;
using VelocipedSite.Responses.V1.Promocode;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class PromocodeController : ControllerBase
{
    private readonly IPromocodesRepository _promocodesRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IOrdersRepository _ordersRepository;
    private readonly ILogger<PromocodeController> _logger;

    public PromocodeController(IPromocodesRepository promocodesRepository, IProfileRepository profileRepository, 
        IOrdersRepository ordersRepository, ILogger<PromocodeController> logger)
    {
        _promocodesRepository = promocodesRepository;
        _profileRepository = profileRepository;
        _ordersRepository = ordersRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<GetPromocodeResponse> GetPromocode(GetPromocodeRequest request)
    {
        try
        {
            _logger.LogInformation("Getting promocode {Code} from sum {Sum}", request.Promocode, request.Sum);
            var promo = await _promocodesRepository.GetPromocode(new GetPromocodeQuery
            {
                Code = request.Promocode
            });

            if (promo.StartDate > DateTime.UtcNow || promo.EndDate < DateTime.UtcNow)
            {
                _logger.LogInformation("Promocode {Code} is invalid: Date is invalid", request.Promocode);
                return new GetPromocodeResponse(false, 0, "Промокод еще не активен или уже закончился");
            }

            if (promo.SaleFrom > request.Sum)
            {
                _logger.LogInformation("Promocode {Code} is invalid: Sum is too small {Sum}/{SaleFrom}",
                    request.Promocode, request.Sum, promo.SaleFrom);
                return new GetPromocodeResponse(false, 0, "Промокод действует на сумму от " + promo.SaleFrom + " руб.");
            }

            if (promo.IsForNew)
            {
                Guid.TryParse(request.Token, out var guid);
                var user = await _profileRepository.GetUserFromToken(new TokenQuery
                {
                    Token = guid
                });

                var orders = await _ordersRepository.GetOrdersForUser(new UserIdQuery
                {
                    UserId = user.Id
                });

                if (orders.Length != 0)
                {
                    _logger.LogInformation("Promocode {Code} is invalid: User must have no orders", request.Promocode);
                    return new GetPromocodeResponse(false, 0, "Промокод только для новых пользователей");
                }
            }

            _logger.LogInformation("Promocode {Code} is valid, Sale value: {SaleValue}", request.Promocode, promo.SaleValue);
            return new GetPromocodeResponse(true, promo.SaleValue, "");
        }
        catch (EntityNotFoundException exception)
        {
            _logger.LogError("Getting promocode {Code} is failed: {Message}", request.Promocode, exception.Message);
            return new GetPromocodeResponse(false, 0, "Такого промокода не существует");
        }
        catch (Exception exception)
        {
            _logger.LogError("Getting promocode {Code} is failed: {Message}", request.Promocode, exception.ToString());
            return new GetPromocodeResponse(false, 0, "Че-то сломалось");
        }
    }
}