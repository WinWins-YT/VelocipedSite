using Microsoft.AspNetCore.Mvc;
using VelocipedSite.DAL.Exceptions;
using VelocipedSite.DAL.Models;
using VelocipedSite.DAL.Repositories.Interfaces;
using VelocipedSite.Models;
using VelocipedSite.Requests.V1;
using VelocipedSite.Requests.V1.Profile;
using VelocipedSite.Responses.V1;
using VelocipedSite.Responses.V1.Profile;
using VelocipedSite.Utilities;

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
                Token = Guid.Parse((ReadOnlySpan<char>)request.Token)
            });

            var isValid = DateTime.UtcNow.Subtract(token.ValidUntil).TotalSeconds > 0;

            _logger.LogInformation("Checked token {Token}, result is {IsValid}", request.Token, isValid);
            return new CheckTokenResponse
            {
                IsValid = isValid,
                ValidUntil = token.ValidUntil
            };
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError("Checked token {Token}: {Message}", request.Token, ex.Message);
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
                Token = Guid.Parse((ReadOnlySpan<char>)request.Token)
            });

            var isValid = token.ValidUntil.Subtract(DateTime.UtcNow).TotalSeconds > 0;
            
            _logger.LogInformation("Getting user by token {Token}, IsValid: {IsValid}", request.Token, isValid);
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
                User = new User(user.Id, user.Email, user.FirstName, user.LastName, user.Address, user.Phone)
            };
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError("Getting user by token {Token}: {Message}", request.Token, ex.Message);
            return new GetUserByTokenResponse
            {
                IsValid = false,
                User = null
            };
        }
    }

    [HttpPost]
    public async Task InvalidateToken(InvalidateTokenRequest request)
    {
        _logger.LogInformation("Invalidating token {Token}", request.Token);
        var ids = await _profileRepository.RemoveToken(new TokenQuery
        {
            Token = Guid.Parse((ReadOnlySpan<char>)request.Token)
        });
        _logger.LogInformation("Invalidation succeeded, deleted token with ID {Id}", ids.First());
    }

    [HttpPost]
    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest request)
    {
        try
        {
            _logger.LogInformation("Authenticating user with e-mail: {Email}", request.Email);
            var user = await _profileRepository.GetUserByEmail(new EmailQuery
            {
                Email = request.Email
            });

            var crypt = new PasswordCrypt(user.Password);

            if (!crypt.Verify(request.Password))
            {
                _logger.LogInformation("Authentication of user with e-mail {Email} is failed: Password is invalid", request.Email);
                return new AuthenticateResponse(false, "");
            }

            var token = await _profileRepository.CreateTokenForUser(new CreateTokenQuery
            {
                UserId = user.Id
            });

            _logger.LogInformation("Authentication of user with e-mail {Email} is succeeded, token: {Token}", request.Email, token.Token);
            return new AuthenticateResponse(true, token.Token.ToString());
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError("Authentication of user with e-mail {Email} failed: {Message}", request.Email, ex.Message);
            return new AuthenticateResponse(false, "");
        }
    }
}