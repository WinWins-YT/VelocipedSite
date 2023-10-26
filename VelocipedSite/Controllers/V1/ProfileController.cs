using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1;
using VelocipedSite.Responses.V1;

namespace VelocipedSite.Controllers.V1;

[ApiController]
[Route("/api/v1/[controller]/[action]")]
public class ProfileController : ControllerBase
{
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IProfileRepository profileRepository, ILogger<ProfileController> logger)
    {
        _profileRepository = profileRepository;
        _logger = logger;
    }

    [HttpPost]
    public async Task<CheckTokenResponse> CheckToken(CheckTokenRequest request)
    {
        try
        {
            var token = await _profileRepository.GetToken(new TokenQuery
            {
                Token = request.Token
            });

            var isValid = DateTime.UtcNow.Subtract(token.ValidUntil).TotalSeconds > 0;

            return new CheckTokenResponse
            {
                IsValid = isValid,
                ValidUntil = token.ValidUntil
            };
        }
        catch (EntityNotFoundException)
        {
            return new CheckTokenResponse
            {
                IsValid = false,
                ValidUntil = new DateTime()
            };
        }
    }

    [HttpPost]
    public async Task<GetUserByTokenResponse> GetUserByToken(GetUserByTokenRequest request)
    {
        try
        {
            var token = await _profileRepository.GetToken(new TokenQuery
            {
                Token = request.Token
            });

            var isValid = token.ValidUntil.Subtract(DateTime.UtcNow).TotalSeconds > 0;

            if (!isValid)
                return new GetUserByTokenResponse
                {
                    User = null,
                    IsValid = false
                };

            var user = await _profileRepository.GetUserFromToken(new TokenQuery
            {
                Token = token.Token
            });

            return new GetUserByTokenResponse
            {
                IsValid = true,
                User = new User(user.Id, user.Email, user.Password, user.FirstName, user.LastName, user.Address, user.Phone)
            };
        }
        catch (EntityNotFoundException)
        {
            return new GetUserByTokenResponse
            {
                IsValid = false,
                User = null
            };
        }
    }
}